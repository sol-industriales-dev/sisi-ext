(function () {
    $.namespace('Administrativo.Contabilidad.Reportes.PagoSemanal');
    PagoSemanal = function () {
        const dpFechaVencimiento = $("#dpFechaVencimiento");
        const btnBuscar = $("#btnBuscar");
        const cboFactoraje = $("#cboFactoraje");
        const cboBancos = $("#cboBancos");
        const btnPrint = $("#btnPrint");
        const cboMoneda = $("#cboMoneda");
        const cboTipoFactura = $("#cboTipoFactura");
        const cboTipoBusqueda = $("#cboTipoBusqueda");
        const tblResumenSemanal = $("#tblResumenSemanal");
        const divBtnPrint = $("#divBtnPrint");
        const divBtnExcel = $("#divBtnExcel");
        const btnExcel = $("#btnExcel");
        const ireport = $("#report");
        const fsBusqueda = $("#fsBusqueda");
        const fsTotal = $("#fsTotal");
        const btnMdlCuadro = $("#btnMdlCuadro");
        const mdlCuadro = $("#mdlCuadro");
        const mdlInteresesNafin = $('#mdlInteresesNafin');
        const btnGuardarIntNafin = $('#btnGuardarIntNafin');
        const tblIntNafin = $('#tblIntNafin');
        const txtDolar = $('#txtDolar');
        const btnExcelPagos = $('#btnExcelPagos');
        function init() {
            initForm();
            btnPrint.click(verReporte);
            btnExcelPagos.click(getExcel);
            btnExcel.click(opnModalIntNafin);
            btnBuscar.click(setBuscarResumen);
            btnMdlCuadro.click(verMdlCuadro);
            cboTipoBusqueda.change(chageTipo);
            btnGuardarIntNafin.click(setGuardarInteresesNafin);
        }
        const BuscarResumen = () => $.post('/Administrativo/Reportes/BuscarResumen', getObjBusq());
        const getCadenasTotales = () => $.post('/Administrativo/Reportes/getCadenasTotales');
        const guardarInteresesNafin = () => $.post('/Administrativo/Reportes/guardarInteresesNafin', {  lst: getLstInteresesNafin()});
        const getDolarDelDia = () => $.post('/Administrativo/Poliza/getDolarDelDia', { fecha: dpFechaVencimiento.val() });
        function initForm(){
            dpFechaVencimiento.datepicker({
                onSelect: function (dateText, inst) {
                    setDolar();
                }
            }).datepicker("setDate", getViernes(new Date()));
            initTablaResumenSemanal();
            initDataTblIntNafin();
            cboTipoFactura.val("N");
            cboTipoFactura.val("S");
            fsTotal.height(fsBusqueda.height());
            setDolar();
        }
        function initTablaResumenSemanal() {
            tblResumenSemanal.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "fechaVencimiento": function (column, row) {
                        return "";
                    }
                }
            });
        }
        function chageTipo() {
            dpFechaVencimiento.prop("disabled", cboTipoBusqueda.val() == "1");
        }
        function getObjBusq() {
            return {
                factoraje: cboFactoraje.val(),
                banco: cboBancos.val(),
                fechaVencimiento: dpFechaVencimiento.val(),
                moneda: cboMoneda.val(),
                tipoBusq: cboTipoBusqueda.val(),
                tipoFactura: cboTipoFactura.val()
            };
        }
        function setBuscarResumen() {
            BuscarResumen().done(response => {
                tblResumenSemanal.bootgrid("clear");
                if (response.success)
                    tblResumenSemanal.bootgrid("append", response.lstResultado);
                divBtnPrint.prop("hidden", true);
                divBtnExcel.prop("hidden", true);
                if (response.lstResultado.length > 0) {
                    divBtnPrint.prop("hidden", false);
                    divBtnExcel.prop("hidden", false);
                }
            })
            .catch(response => {
                tblResumenSemanal.bootgrid("clear");
                AlertaGeneral("Alerta", response.message);
            });
        }
        function verMdlCuadro() {
            mdlCuadro.modal("show");
        }
        function verReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = "46";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        function setDolar(){
            getDolarDelDia().done(response => {
                if (response.success) {
                    txtDolar.val(response.dll);
                }
            })
        }
        function opnModalIntNafin(){
            getCadenasTotales().done(response => {
                if (response.success) {
                    if(response.esVencido){
                        dtIntNafin.clear().rows.add(response.lstFactoraje).draw();
                        mdlInteresesNafin.modal("show");                        
                    }
                    else{
                        getExcel();
                    }
                }
            });
        }
        function setGuardarInteresesNafin(){
            if (esInteresValido()) {
                guardarInteresesNafin().done( response => {
                    if (response.success) {
                        getExcel();
                    }
                });   
            }
        }
        function getLstInteresesNafin() {
            let lst = [],
                fecha = dpFechaVencimiento.val(),
                tc = unmaskNumero(txtDolar.val());
            dtIntNafin.rows().iterator('row', function(ctx, i) {
                let node = $(this.row(i).node()),
                data = dtIntNafin.row(node).data(),
                esDLL = data.moneda === "DLL";
                if(data.factoraje === 'V'){
                    let save = {
                        fecha: fecha,
                        banco: data.banco,
                        totalCadenas: data.totalCP,
                        totalBanco: unmaskNumero(node.find('.banco').val()),
                        interes: +(node.find('.interes').val()),
                        tipoCambio: esDLL ? tc : 1,
                        divisa: esDLL ? 2 : 1,
                    };
                    lst.push(save);
                }
            });
            return lst;    
        }
        function esInteresValido() {
            let esValido = true;
             if (unmaskNumero(txtDolar.val()) > 0) {
                dtIntNafin.rows().iterator('row', function(ctx, i) {
                    let node = $(this.row(i).node()),
                    data = dtIntNafin.row(node).data(),
                    totalBanco = unmaskNumero(node.find('.banco').val());
                    if (data.factoraje === "V") {
                        if (data.moneda === "DLL") {
                            if(totalBanco <= 0) {
                                esValido = false;
                            }
                        }   
                    }
                });
            }   
            return esValido;
        }
        function initDataTblIntNafin() {
            dtIntNafin = tblIntNafin.DataTable({
                info: false,
                paging: false,
                destroy: true,
                ordering: false,
                searching: false,
                language: dtDicEsp,
                columns: [
                    { data: 'banco'},
                    {
                        data: 'factoraje', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(rowData.esVencido ? 'Vencido' : 'Normal');
                        }
                    },
                    { data: 'moneda'},
                    {
                        data: 'totalCP', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right`);
                            $(td).find(`input`).val(maskNumero(data));
                            $(td).find(`input`).prop('disabled', true);
                        }
                    },
                    {
                        data: 'totalB', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right banco`);
                            $(td).find(`input`).val(maskNumero(data));
                            $(td).find(`input`).prop('disabled', !rowData.esVencido);
                        }
                    },
                    {
                        data: 'intereses', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control text-right interes`);
                            $(td).find(`input`).val(data);
                            $(td).find(`input`).prop('disabled', true);
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblIntNafin.on('change', '.banco', function (event) {
                        let banco = unmaskNumero(this.value),
                            row = $(this).parents('tr'),
                            data = dtIntNafin.row(row).data(),
                            cadena = data.totalCP,
                            diff = banco - cadena;
                            if(banco > cadena){
                                interes = Math.trunc((diff / cadena) * 10000) / 10000;
                                row.find('.interes').val(interes);
                            }
                            this.value = maskNumero(banco);
                    });
                }
            });
        }
        function getExcel() {
            var url = '/Administrativo/Reportes/getExcelResumenSemanal';
            download(url)
        }
        function download(url) {
            $.when($.blockUI({ message: "Preparando archivo a descargar" })).then(function () {
                iframe = document.getElementById('iframeDownload');
                iframe.src = url;
                var timer = setInterval(function () {
                    var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                    if (iframeDoc.readyState == 'complete' || iframeDoc.readyState == 'interactive') {
                        setTimeout(function () {
                            $.unblockUI();
                        }, 1200);
                        clearInterval(timer);
                        return;
                    }
                }, 1000);
            });
        }
        function getViernes(d) {
            d = new Date(d);
            var day = d.getDay(),
                diff = d.getDate() - day + (day == 0 ? -6 : 5); // adjust when day is sunday
            return new Date(d.setDate(diff));
        }
        init();
    };
    $(document).ready(function () {
        Administrativo.Contabilidad.Reportes.PagoSemanal = new PagoSemanal();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();