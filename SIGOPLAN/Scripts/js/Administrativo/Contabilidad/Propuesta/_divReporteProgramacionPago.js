(() => {
    $.namespace('Administracion.Propuesta._divReporteProgramacionPago');
    _divReporteProgramacionPago = function () {
        const cboTipo = $('#cboTipo');
        const btnRptProvBuscar = $('#btnRptProvBuscar');
        const btnRptProvReporte = $('#btnRptProvReporte');
        const tblRptProv = $('#tblRptProv');
        const cboRptProvCC = $('#cboRptProvCC');
        const dpRptProvMin = $('#dpRptProvMin');
        const dpRptProvMax = $('#dpRptProvMax');
        const txtRptProvMin = $('#txtRptProvMin');
        const txtRptProvMax = $('#txtRptProvMax');
        const inputGroupBtn = $('.input-group-btn');
        const btnRptProvLimit = $('#btnRptProvLimit');
        let init = () => {
            initForm();
            btnRptProvReporte.prop("disabled", true);
            btnRptProvReporte.click(showCRModa);
            btnRptProvBuscar.click(setLstGenMovProv);
            btnRptProvLimit.click(setDataLimit);
            inputGroupBtn.click(chngSetAllSelOpt);
            // inputGroupBtn.each((i, btn) => $(btn).click());
            dpRptProvMin.change(fnResetReport);
            dpRptProvMax.change(fnResetReport);
        }
        function fnResetReport() {
            btnRptProvReporte.prop("disabled", true);
        }
        const getLstGenMovProv = new URL(window.location.origin + '/Administrativo/Propuesta/getLstGenMovProv');
        const getProgPagoRptId = new URL(window.location.origin + '/Administrativo/Propuesta/getProgPagoRptId');
        const getLimitNoProveedores = new URL(window.location.origin + '/Administrativo/Propuesta/getLimitNoProveedores');
        async function setLstGenMovProv() {
            try {
                dtRptProv.clear().draw();
                response = await ejectFetchJson(getLstGenMovProv, getForm());
                if (response.success) {
                    dtRptProv.rows.add(response.lst).draw();
                    btnRptProvReporte.prop("disabled", false);
                }
            } catch (o_O) { }
        }
        async function setIdReporte() {
            try {
                response = await ejectFetchJson(getProgPagoRptId);
                if (response.success) {
                    idCRReporte = response.idReporte;
                }
            } catch (o_O) { }
        }
        async function getLimitProv() {
            response = await ejectFetchJson(getLimitNoProveedores);
            if (response.success) {
                txtRptProvMin.val("0");
                txtRptProvMin.data().prov = response.limit[0];
                txtRptProvMax.val(response.limit[1]);
                txtRptProvMax.data().prov = response.limit[1];
            }
        }
        function setDataLimit() {
            txtRptProvMin.val(txtRptProvMin.data().prov);
            txtRptProvMax.val(txtRptProvMax.data().prov);
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
        function initDataTblRptProv() {
            dtRptProv = tblRptProv.DataTable({
                dom: 'lBfrtip',
                buttons: [ 
                    'excelHtml5', 
                ],
                destroy: true,
                language: dtDicEsp,
                ordering: false,
                paging: false,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": false,
                //"bFilter": true,
                //"bInfo": true,
                "bAutoWidth": false,
                columns: [
                    { title: `Factura`, data: 'factura' },
                    //{ title:`Fecha` ,data: 'fecha' ,createdCell: (td, data) => $(td).html($.toDate(data)) },
                    { title: `Estado`, data: 'descStatus' },
                    { title: `TM`, data: 'descTm' },
                    { title: `Area Cuenta`, data: 'areaCuenta', visible: _gpEmpresa == 2 },
                    { title: `Centro Costo`, data: 'cc' },
                    //{ title:`TMB` ,data: 'descTmb'},
                    //{ title:`TMP` ,data: 'descTmp'},
                    //{ title:`Movimiento` ,data: 'fecha_movto' ,createdCell: (td, data) => $(td).html($.toDate(data))},
                    { title: 'Saldo', class: 'text-right', data: 'monto', createdCell: (td, data) => $(td).html(maskNumero(data)) },
                    { title: 'Monto', class: 'text-right', data: 'monto_plan', createdCell: (td, data) => $(td).html(maskNumero(data)) },
                    { title: `Cuenta`, data: 'cuenta' },
                    { title: `Banco`, data: 'banco' },
                    { title: 'Programo', data: 'programo' },
                    { title: 'Autorizo', data: 'autorizo' }
                    //{ title:`OC` ,data: 'oc'},
                ]
                , drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    api.column({ page: 'current' }).data().each((group, i, dtable) => {
                        const dataBefore = dtable.data()[i - 1];
                        const data = dtable.data()[i];
                        if (i > 0) {
                            if (dataBefore.numpro !== data.numpro) {
                                let lstProv = dtable.data().filter(prov => dataBefore.numpro === prov.numpro);
                                let sumaProvMonto = lstProv.reduce((suma, current) => suma + current.monto, 0);
                                let sumaProvPlan = lstProv.reduce((suma, current) => suma + current.monto_plan, 0);
                                $(rows).eq(i).before(`<tr>
                                    <td colspan="4" class="fondoTotal">${dataBefore.numpro} - ${dataBefore.proveedor}<span style="float:right">Saldo del proveedor ${dataBefore.numpro < 9000 ? "M.N" : "DLL"}</span></td>
                                    <td class="text-right fondoTotal">${maskNumero(sumaProvMonto)}</td>
                                    <td class="text-right fondoTotal">${maskNumero(sumaProvPlan)}</td>
                                </tr>`);
                            }
                            if (i == dtable.length - 1) {
                                let lstProv = dtable.data().filter(prov => data.numpro === prov.numpro);
                                let sumaProvMonto = lstProv.reduce((suma, current) => suma + current.monto, 0);
                                let sumaProvPlan = lstProv.reduce((suma, current) => suma + current.monto_plan, 0);
                                $(rows).eq(i).after(`<tr>
                                    <td colspan="4" class="fondoTotal">${data.numpro} - ${data.proveedor}<span style="float:right">Saldo del proveedor ${data.numpro < 9000 ? "M.N" : "DLL"}</span></td>
                                    <td class="text-right fondoTotal">${maskNumero(sumaProvMonto)}</td>
                                    <td class="text-right fondoTotal">${maskNumero(sumaProvPlan)}</td>
                                </tr>`);
                            }
                        }
                    });
                }
                , initComplete: function (settings, json) {
                }
            });
        }
        function getForm() {
            return {
                tipo: cboTipo.val()
                , minMov: dpRptProvMin.datepicker("getDate")
                , maxMov: dpRptProvMax.datepicker("getDate")
                , lstCc: cboRptProvCC.val()
                , minProv: txtRptProvMin.val()
                , maxProv: txtRptProvMax.val()
            };
        }
        function initForm() {
            var ahora = new Date();
            dpRptProvMax.datepicker().datepicker("setDate", ahora);
            dpRptProvMin.datepicker().datepicker("setDate", new Date(ahora.getFullYear(), ahora.getMonth(), 1));
            setIdReporte();
            getLimitProv();

            cboRptProvCC.fillCombo('/Administrativo/Poliza/getCC', null, false, 'Todos');
            convertToMultiselect('#cboRptProvCC');

            initDataTblRptProv();
        }
        init();
    }
    $(document).ready(() => {
        Administracion.Propuesta._divReporteProgramacionPago = new _divReporteProgramacionPago();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();