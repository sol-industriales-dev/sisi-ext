(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta.ProgramacionPagos');
ProgramacionPagos = function () {
    let _poliza = "";
        const chkComplementaria = $("#chkComplementaria");
        const cboCC = $("#cboCC");
        const btnBuscar = $("#btnBuscar");
        const btnGuardar = $("#btnGuardar");
        const txtProvMin = $('#txtProvMin');
        const txtProvMax = $('#txtProvMax');
        const txtPropFOC = $('#txtPropFOC');
        const txtPropHSel = $('#txtPropHSel');
        const txtPropHSelDll = $('#txtPropHSelDll');
        const txtPropFTmb = $('#txtPropFTmb');
        const txtPropFFac = $('#txtPropFFac');
        const txtPropFPag = $('#txtPropFPag');
        const txtPropFMax = $('#txtPropFMax');
        const txtPropFTmp = $('#txtPropFTmp');
        const txtPropFSol = $('#txtPropFSol');
        const txtPropFRec = $('#txtPropFRec');
        const btnProvLimit = $('#btnProvLimit');
        const txtFechaPago = $("#txtFechaPago");
        const txtFechaCorte = $("#txtFechaCorte");
        const txtPropHTotal = $('#txtPropHTotal');
        const txtPropHTotalDll = $('#txtPropHTotalDll');
        const inputGroupBtn = $('.input-group-btn');
        const tblProgramacionPagos = $("#tblProgramacionPagos");

        const txtFechaPoliza = $("#txtFechaPoliza");
        const modalPoliza = $("#modalPoliza");
        const btnGenerar = $("#btnGenerar");
        let dtProgrPagos;

        let init = () => {
            txtProvMin.change(function(){
                txtProvMax.val($(this).val());
            });
            IniciarForm();
            txtFechaPago.change(setDolarDelDia)
            btnBuscar.click(CargarTabla);
            btnProvLimit.click(setDataLimit);
            btnGuardar.click(openModalPoliza);
            inputGroupBtn.click(chngSetAllSelOpt);
            inputGroupBtn.each((i, btn) => $(btn).click());
            setPermisos(1);
            btnGenerar.click(saveSplitted);
            
        }
        function openModalPoliza(){
            if(chkComplementaria.is(":checked"))
            {
                
                let data = MontoPropPagoDTO();
                if (data.length > 0) {
                    var polizaID = 0;
                    _poliza = "tipocambio";
                    
                    var scheme = { lst: new Array(), pago: txtFechaPoliza.val(),polizaID:polizaID,tipo:chkComplementaria.is(":checked") };
                    $.sm_SplittedSaveNoMessage(SaldosMenores_GenerarPolizas_det, data, scheme, 20, CargarTabla);
                }
                else {
                    _poliza = "";
                    AlertaGeneral("Alerta", "¡Debe configurar almenos un monto para continuar!");
                }
            }
            else{
                txtFechaPoliza.datepicker().datepicker("setDate", new Date());
                modalPoliza.modal("show");
            }
            
        }
        let dlldia = 0;
        const getDolarDelDia = new URL(window.location.origin + '/Administrativo/Poliza/getDolarDelDia');
        const CargarTblProgrPagos = new URL(window.location.origin + '/Administrativo/Propuesta/getSaldosMenores');
        const getLimitNoProveedores = new URL(window.location.origin + '/Administrativo/Propuesta/getLimitNoProveedores');
        const SaldosMenores_GenerarPolizas = new URL(window.location.origin + '/Administrativo/Propuesta/SaldosMenores_GenerarPolizas');
        const SaldosMenores_GenerarPolizas_det = new URL(window.location.origin + '/Administrativo/Propuesta/SaldosMenores_GenerarPolizas_det');
        function IniciarForm() {
            getLimitProv();
            //if (_gpEmpresa == 1) {
            //    cboCC.fillCombo('/Administrativo/Poliza/getCC', {}, true, null);
            //    convertToMultiselect(cboCC);
            //}
            //else {
            //    $(".cboCC").hide();
            //    convertToMultiselect(cboCC);
            //}

            txtFechaCorte.datepicker();
            txtFechaPago.datepicker().datepicker("setDate", new Date());
            txtFechaPoliza.datepicker().datepicker("setDate", new Date());
            IniciarTblProgrPagos();
            txtPropHTotal.text(maskNumero(0));
            txtPropHSel.text(maskNumero(0));
            txtPropHTotalDll.text(maskNumero(0));
            txtPropHSelDll.text(maskNumero(0));
            setFooter({ cc: "", oc: "", saldo: 0, tipoMoneda: "", tmb: "", tmbDescripcion: "", tmp: "", tmpDescripcion: "" });
        }
        async function getLimitProv() {
            response = await ejectFetchJson(getLimitNoProveedores);
            if (response.success) {
                txtProvMin.val(response.limit[0]);
                txtProvMin.data().prov = response.limit[0];
                txtProvMax.val(response.limit[1]);
                txtProvMax.data().prov = response.limit[1];
            }
        }
        async function setDolarDelDia() {
            try {
                response = await ejectFetchJson(getDolarDelDia, { fecha: txtFechaPago.val() });
                if (response.success) {
                    dlldia = response.dll;
                }
            } catch (o_O) { dlldia = 1; }
        }
        function setPermisos(permiso) {
            if (permiso == 1) {
                btnGuardar.show();
            }
            else {
                btnGuardar.hide();
            }
        }
        function setDataLimit() {
            txtProvMin.val(txtProvMin.data().prov);
            txtProvMax.val(txtProvMin.data().prov);
            txtProvMin.change();
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
        function setSaldosTotales() {
            let sumaSaldo = 0;
            let sumaSaldoDll = 0;
            dtProgrPagos.rows().every(function (i) {
                let node = $(this.node()),
                    data = this.data();
                if (data.moneda == 'MN') {
                    //node.find('input:checkbox').prop('checked', true);
                    if (node.find('input:checkbox').prop('checked')) {
                        sumaSaldo += unmaskNumero(node.find('input').val());
                    }
                }
                else {
                    if (node.find('input:checkbox').prop('checked')) {
                        sumaSaldoDll += unmaskNumero(node.find('input').val());
                    }
                }
            });
            txtPropHSel.text(maskNumero(sumaSaldo));
            txtPropHSelDll.text(maskNumero(sumaSaldoDll));
        }
        function IniciarTblProgrPagos() {
            dtProgrPagos = tblProgramacionPagos.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": false,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                columns: [
                    { data: 'factura' },
                    {
                        data: 'vence', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(moment(data).format("DD/MM/YYYY"));
                        }
                    },

                    { data: 'tm' },
                    { data: 'cc' },
                    {
                        data: 'ac', visible: (_gpEmpresa == 2 ? true : false), createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data);
                            $(td).prop("title", rowData.acDesc);
                        }
                    },
                    { data: 'referenciaoc' },
                    { data: 'concepto' },
                    {
                        data: 'monto_plan', width: "100px", createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data)).addClass('text-right');
                        }

                    },
                    { data: 'moneda' },
                    {
                        data: 'saldo', width: "150px", createdCell: function (td, data, rowData, row, col) {
                            //if (_gpGuardar == 1) {
                            if(true){
                                //var html = `<div class='input-group'><input type="text" class="form-control monto text-right ${rowData.esPagado ? 'desactivado' : ''}" data-id="${rowData.id}" data-saldo="${rowData.monto}" data-aplicado="0" value="${maskNumero(rowData.monto_plan - data)}" ${rowData.esPagado ? "disabled" : ""}>` + (rowData.esPagado ? '' : '<span class="input-group-btn"><button type="button" class="btn btn-default btn-sm eraser" style="height: 26px;"><i class="fa fa-eraser" aria-hidden="true"></i></button></span></div>');
                                var html = `<div class="input-group"><input type="text" class="form-control monto text-right ${rowData.esPagado ? 'desactivado' : ''} data-id="${rowData.id}" data-saldo="${rowData.monto_plan}" data-aplicado="0" value="${maskNumero(rowData.monto_plan)}" readonly /><span class="input-group-addon"><input type="checkbox" /></span></div>`;
                            }
                            else {
                                var html = `<input type="text" class="form-control monto text-right ${rowData.esPagado ? 'desactivado' : ''}" data-id="${rowData.id}" data-saldo="${rowData.monto}" data-aplicado="0" value="${maskNumero(rowData.monto_plan - data)}" readonly>`;
                            }
                            $(td).html(html);
                        }
                    },
                    { data: 'tmDesc' }

                ],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    api.column({ page: 'current' }).data().each((group, i, dtable) => {
                        const dataBefore = dtable.data()[i - 1];
                        const data = dtable.data()[i];
                        if (i > 0) {
                            if (dataBefore.numpro !== data.numpro) {
                                let lstProv = dtable.data().filter(prov => dataBefore.numpro === prov.numpro);
                                let suma = lstProv.reduce((suma, current) => suma + (current.esPagado ? (current.monto_plan) : 0), 0);
                                let sumaProv = lstProv.reduce((suma, current) => suma + current.monto_plan, 0);
                                $(rows).eq(i).before(`<tr class="prov${dataBefore.numpro}">
                                    <td colspan="6" class="fondoTotal">${dataBefore.numpro} - ${dataBefore.proveedor}<span style="float:right">Saldo del proveedor</span></td>
                                    <td class="text-right fondoTotal">${maskNumero(sumaProv)}</td>
                                    <td class="fondoTotal">${dataBefore.moneda}</td>
                                    <td class="text-right fondoTotal">${maskNumero(suma)}</td>
                                    <td colspan="1"><button type="button" class="btn btn-default btn-sm btnProv" data-proveedor="${dataBefore.numpro}"><i class="fas fa-hand-holding-usd"></i> Pagar Todo</button></td>
                                </tr>`);
                            }
                            if (i == dtable.length - 1) {
                                let lstProv = dtable.data().filter(prov => data.numpro === prov.numpro);
                                let suma = lstProv.reduce((suma, current) => suma + (current.esPagado ? (current.monto_plan) : 0), 0);
                                let sumaProv = lstProv.reduce((suma, current) => suma + current.monto_plan, 0);
                                $(rows).eq(i).after(`<tr class="prov${data.numpro}">
                                    <td colspan="6" class="fondoTotal">${data.numpro} - ${data.proveedor}<span style="float:right">Saldo del proveedor</span></td>
                                    <td class="text-right fondoTotal">${maskNumero(sumaProv)}</td>
                                    <td class="fondoTotal">${data.moneda}</td>
                                    <td class="text-right fondoTotal">${maskNumero(suma)}</td>
                                    <td colspan="1"><button type="button" class="btn btn-default btn-sm btnProv" data-proveedor="${data.numpro}"><i class="fas fa-hand-holding-usd"></i> Pagar Todo</button></td>
                                </tr>`);
                            }
                        }
                    //if (_gpGuardar == 1) {
                    if(true){
                            $(".btnProv").show();
                        }
                        else {
                            $(".btnProv").hide();
                        }
                    });
                }
                , initComplete: function (settings, json) {
                     tblProgramacionPagos.find('tbody').on('click', 'tr', function () {
                         let data = dtProgrPagos.row(this).data();
                         if(true){

                             if (data !== undefined) {
                                 let inp = $(this).closest('tr').find('input')
                                     , val = unmaskNumero(inp.val());
                                 if (!inp.hasClass("desactivado")) {

                                     if (val === 0 || val > data.monto_plan) {
                                         val = data.monto_plan;
                                     }
                                     inp.val(maskNumero(val));
                                     data.saldo = val;
                                     setTotalProv(data);
                                     setSaldosTotales();
                                 }
                             }
                         }
                         setFooter(data);
                     });

                    

                    tblProgramacionPagos.on('change', 'input', function () {
                        //if (_gpGuardar == 1) {
                        if(true){
                            let row = $(this).closest('tr')
                                , data = dtProgrPagos.row(row).data();
                            if (data !== undefined) {
                                let inp = row.find('input')
                                    , val = unmaskNumero(inp.val());
                                if (val === 0 || val > data.monto_plan) {
                                    val = data.monto_plan;
                                }
                                //inp.val(maskNumero(val));
                                data.saldo = val;
                                setTotalProv(data);
                                setSaldosTotales();
                            }
                        }
                    });
                    tblProgramacionPagos.on('click', '.btnProv', function () {
                        let row = $(this).closest('tr')
                            , prev = row.prev()[0]
                            , data = dtProgrPagos.rows(prev).data()[0];

                        dtProgrPagos.rows().every(function (i) {
                            let node = $(this.node())
                                , current = this.data();
                            if (data.numpro == current.numpro) {
                                var aplicado = node.find('input').data("aplicado");

                                if (!node.find('input').hasClass("desactivado") && aplicado == "0") {
                                    //node.find('input').val(maskNumero(current.monto));
                                    node.find('input').data("aplicado", "1");
                                    node.find('input:checkbox').prop('checked', true);
                                }
                                else if (!node.find('input').hasClass("desactivado") && aplicado == "1") {
                                    //node.find('input').val(maskNumero(0));
                                    node.find('input').data("aplicado", "0");
                                    node.find('input:checkbox').prop('checked', false);
                                }
                            }
                        });
                        setTotalProv(data);
                        setSaldosTotales();
                    });
                    // tblProgramacionPagos.on('click', '.eraser', function () {
                    //     let row = $(this).closest('tr')
                    //         , data = dtProgrPagos.row(row).data();
                    //     if (data !== undefined) {
                    //         let inp = row.find('input')
                    //             , val = unmaskNumero(inp.val());

                    //         inp.val(maskNumero(0));
                    //         data.saldo = 0;
                    //         setTotalProv(data);
                    //         setSaldosTotales();
                    //     }
                    // });
                }
            });
        }

        function AbrirArchivo(url) {
            window.location.href = "/Administrativo/Propuesta/fnDownloadFile?descargar=" + url;
        }
        function setTotalProv({ proveedorID }) {
            let sumGroup = 0;
            //setDolarDelDia();
            dtProgrPagos.rows().every(function (i) {
                let node = $(this.node())
                    , current = this.data();
                if (proveedorID === current.numpro) {
                    if (node.find('input:checkbox').prop('checked')) {
                        sumGroup += unmaskNumero(node.find('input').val());
                    }
                }
            });
            tblProgramacionPagos.find(`tr.prov${proveedorID} td:eq(3)`).text(maskNumero(sumGroup));
        }
        function setFooter({ cc, oc, saldo, tipoMoneda, tmb, tmbDescripcion, tmp, tmpDescripcion }) {
            txtPropFOC.val(`${cc} - ${oc}`);
            txtPropFTmb.val(`${tmb} - ${tmbDescripcion}`);
            txtPropFFac.val(maskNumero(saldo));
            txtPropFPag.val(maskNumero(0));
            txtPropFMax.val(`${maskNumero(saldo)} - ${tipoMoneda}`);
            txtPropFTmp.val(`${tmp} - ${tmpDescripcion}`);
            txtPropFSol.val(maskNumero(saldo));
            txtPropFRec.val(maskNumero(saldo));
        }
        async function CargarTabla() {
            try {
                if(_poliza!=''){
                    if(chkComplementaria.is(":checked")){
                        AlertaGeneral("Confirmacion", "¡Se actualizo correctamente el tipo de cambio en factura para ajuste de saldo en moneda nacional!");
                    }
                    else{
                        AlertaGeneral("Confirmacion", "¡Poliza "+_poliza+" guardada correctamente!");
                    }
                }
                _poliza='';
                dtProgrPagos.clear().draw();
                txtPropHSel.text(maskNumero(0));
                txtPropHTotal.text(maskNumero(0));
                txtPropHSelDll.text(maskNumero(0));
                txtPropHTotalDll.text(maskNumero(0));
                modalPoliza.modal("hide");
                let busq =  getForm();
                response = await ejectFetchJson(CargarTblProgrPagos, busq);
                if (response.success) {
                    if (response.lst.length > 0) {
                        dtProgrPagos.rows.add(response.lst).draw();
                        txtPropHTotal.text(maskNumero(response.total));
                        txtPropHTotalDll.text(maskNumero(response.totaldll ?? 0));
                        tblProgramacionPagos.find('.btnProv').each(function () {
                            this.value = 0;
                        });
                        
                    } else {
                        AlertaGeneral("Alerta", "¡No se encontraron facturas pendientes de autorización!");
                    }
                    setSaldosTotales();
                } else {
                    AlertaGeneral("Alerta", "¡No se encontraron facturas pendientes de autorización!");
                }
            } catch (error) {
                AlertaGeneral(`Alerta`, `Error al cargar la tabla.`);
            }
        }
        function getForm(){
            return {
                fechaCorte: txtFechaCorte.val()
               ,tipoProceso: 1
               ,min: txtProvMin.val()
               ,max: txtProvMax.val()
               ,lstCc: cboCC.val()
               ,tipo: chkComplementaria.is(":checked")
            };
        }
        function saveSplitted() {
            let data = MontoPropPagoDTO();
            if (data.length > 0) {
                var total = data.reduce(function (total, currentValue) {
                    return total + currentValue.totalMN;
                }, 0);
                var moneda = data[0].moneda;
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: SaldosMenores_GenerarPolizas,
                    data: {pago: txtFechaPoliza.val(),moneda:moneda,total:(moneda=='MN'?0:total),tipo:chkComplementaria.is(":checked")},
                    traditional:true,
                    success: function(response) { 
                        var poliza = response.poliza;
                        var polizaID = response.polizaID;
                        _poliza = poliza;
                    
                        var scheme = { lst: new Array(), pago: txtFechaPoliza.val(),polizaID:polizaID,tipo:chkComplementaria.is(":checked") };
                        $.sm_SplittedSaveNoMessage(SaldosMenores_GenerarPolizas_det, data, scheme, 20, CargarTabla);
                    
                    },
                    error: function () {
                        _poliza = "";
                        AlertaGeneral("Alerta","Ocurrio un error, favor de contactar al depto de TI!");
                    }
                });
            }
            else {
                _poliza = "";
                AlertaGeneral("Alerta", "¡Debe configurar almenos un monto para continuar!");
            }
        }
        async function GuardarMontos() {
            try {
                response = await ejectFetchJson(SaldosMenores_GenerarPolizas, { lst: MontoPropPagoDTO(), pago: txtFechaPago.val() });
                if (response.success) {
                    if (response.exito) {
                        CargarTabla();
                        AlertaGeneral("Aviso", "Se han guardado los montos con éxito.");
                    }
                    else {
                        AlertaGeneral("Aviso", "Se ha producido un error. No se han guardado los montos.");
                    }
                }
            } catch (error) { }
        }
        function MontoPropPagoDTO() {
            let lst = [];
            dtProgrPagos.rows().every(function (i) {
                let node = $(this.node())
                    , data = this.data();
                if (node.find('input[type=checkbox]').is(":checked")) {
                    lst.push(data);
                }
            });
            return lst;
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta.ProgramacionPagos = new ProgramacionPagos();
    }).ajaxStart(() => {
        $.blockUI({
            baseZ: 100000,
            message: 'Procesando...'
        });
    }).ajaxStop(() => { $.unblockUI(); });
})();   