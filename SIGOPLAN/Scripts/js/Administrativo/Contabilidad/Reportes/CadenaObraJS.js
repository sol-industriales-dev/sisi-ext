(function () {
    $.namespace('Administrativo.Contabilidad.Reportes.CadenaObra');
    CadenaObra = function () {
        mensajes = { PROCESANDO: 'Procesando...' };
        tblResumenObra = $("#tblResumenObra");
        infoMaquina = $("#infoMaquina");
        dpFecha = $("#dpFecha");
        btnCargar = $("#btnCargar");
        btnExcel = $("#btnExcel");
        divBtnExcel = $("#divBtnExcel");
        divDatosMaquina = $("#divDatosMaquina");
        function init() {
            dpFecha.datepicker({
                    beforeShowDay: function(date) {
                        return [date.getDay() == 5];
                    }
                }).datepicker("setDate", getViernes(new Date()));
            initTabla();
            LoadCadenaObra();
            btnCargar.click(LoadCadenaObra);
            btnExcel.click(GuardarReserva);
        }
        function initTabla() {
            infoMaquina.bootgrid({
                rowCount: -1,
                headerCssClass: '',
                align: 'center',
                templates: {
                    header: '',
                    footer: ''
                },
                formatters: {
                    "obra": function (column, row) {
                        column.cssClass = "textoLimite";
                        return '<div><label class="cc" data-toggle="tooltip" title="' + (row.factoraje == null ? "" : row.factoraje) + '">' + row.factoraje +'</label></div><div class="text-right"><label class="labelFont" data-total="' + unmaskDinero(row.banco) + '">' + row.banco + '</label></div>';
                    }
                }
            });
            tblResumenObra.bootgrid({
                rowCount: -1, 
                headerCssClass: '',
                align: 'center',
                templates: {
                    header: '',
                    footer: ''
                },
                formatters: {
                    "vencimiento": function (column, row) {
                        column.cssClass = (row.concepto == null && row.fechaS != null ? "acomulado " : "") + (row.banco != undefined ? "actual" : "");
                        return row.fechaVencimientoS != undefined ? '<label class="labelFont" data-original="' + unmaskDinero(row.fechaVencimientoS) + '">' + row.fechaVencimientoS + '</label>' : '';
                    },
                    "tipo": function (column, row) {
                        column.cssClass = (row.concepto == null && row.fechaS != null ? "acomulado " : "") + (row.banco != undefined ? "actual" : "");
                        return row.concepto != undefined ? '<label class="labelFont" data-original="' + unmaskDinero(row.concepto) + '">' + row.concepto + '</label>' : '';
                    },
                    "concepto": function (column, row) {
                        column.cssClass = (row.concepto == null && row.fechaS != null ? "acomulado " : "") + (row.banco != undefined ? "actual" : "");
                        return '<label style="font-weight: normal;" data-toggle="tooltip" title="' + (row.proveedor == null ? "" : row.proveedor) + '">' + (row.proveedor == null ? "" : row.proveedor) + '</label>';
                    },
                    "cc": function (column, row) {
                        column.cssClass = (row.concepto == null && row.fechaS != null ? "acomulado " : "") + (row.banco != undefined ? "actual" : "");
                        return '<label style="font-weight: normal;" data-toggle="tooltip" title="' + (row.fechaS == null ? "" : row.fechaS) + '" data-cc="' + row.centro_costos + '">' + (row.fechaS == null ? "" : row.fechaS) + '</label>';
                    },
                    "reserva": function (column, row) {
                        column.cssClass ="text-right " + (row.concepto == null && row.fechaS != null ? "acomulado " : "") + (row.banco != undefined ? "actual" : "");
                        return row.concepto == 'RESERVA PAGO EN CADENAS' ? '<input type="text" class="form-control reserva text-right" value="' + row.numNafin +'" data-id="' + row.monto + '" data-fecha="' + row.fechaVencimientoS + '" ' + ( row.pagado ? '' : 'disabled')  +' ">' : row.numNafin != undefined ? '<label class="labelFont" data-original="' + unmaskDinero(row.numNafin) + '">' + row.numNafin + '</label>' : "";
                    },
                    "pago": function (column, row) {
                        column.cssClass ="text-right " + (row.concepto == null && row.fechaS != null ? "acomulado " : "") + (row.banco != undefined ? "actual" : "");
                        return row.saldoFactura != undefined ? '<label class="labelFont" data-original="' + unmaskDinero(row.saldoFactura) + '">' + row.saldoFactura + '</label>' : '';
                    },
                    "acomulado": function (column, row) {
                        column.cssClass ="text-right " + (row.concepto == null && row.fechaS != null ? "acomulado " : "") + (row.banco != undefined ? "actual" : "");
                        return row.banco != undefined ? '<label class="labelFont" data-original="' + unmaskDinero(row.banco) + '">' + row.banco + '</label>' : '';
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblResumenObra.find(".reserva").on("change", function (e) {
                    setReserva();
                });
                $('[data-toggle="tooltip"]').tooltip(); 
                setReserva();
            });
        }
        function LoadCadenaObra() {
            $.blockUI({ message: mensajes.PROCESANDO });
            infoMaquina.bootgrid('clear');
            tblResumenObra.bootgrid('clear');
            $.ajax({
                url: '/Administrativo/Reportes/LoadCadenaObra',
                type: 'POST',
                dataType: 'json',
                data: { fecha: dpFecha.val()},
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        infoMaquina.bootgrid("append", response.lstObra);
                        tblResumenObra.bootgrid("append", response.lstRes);
                    }
                    divBtnExcel.prop("hidden", !(response.lstRes != undefined && response.lstRes.length > 0));
                    divDatosMaquina.prop("hidden", !(response.lstObra != undefined && response.lstObra.length > 0));
                },
                error: function (response) {
                   AlertaGeneral("Alerta", response.message);
                   $.unblockUI();
                }
            });
        }
        function GuardarReserva() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Administrativo/Reportes/GuardarReserva',
                type: 'POST',
                dataType: 'json',
                data: { fecha: dpFecha.val(), lstReserva : lstReserva()},
                success: function (response) {
                    $.unblockUI();
                    response.success ? download() : AlertaGeneral("Alerta", response.message);
                },
                error: function (response) {
                   AlertaGeneral("Alerta", response.message);
                   $.unblockUI();
                }
            });
        }
        function download() {
            $.when($.blockUI({ message: "Preparando archivo a descargar" })).then(function () {
                iframe = document.getElementById('iframeDownload');
                iframe.src = '/Administrativo/Reportes/getExcelCadenaObra';

                var timer = setInterval(function () {

                    var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                    if (iframeDoc.readyState == 'complete' || iframeDoc.readyState == 'interactive') {
                        setTimeout(function () {
                            $.unblockUI();
                        }, 5000);

                        clearInterval(timer);
                        return;
                    }
                }, 1000);
            });
        }
        function lstReserva(){
            var Array = [];
            tblResumenObra.find("tr").each(function (idx, row) {
                if (idx > 0) {
                    var JsonData = {};
                    if($(this).find('td:eq(1)').text() == 'RESERVA PAGO EN CADENAS'){
                        JsonData.FechaVencimiento = $(this).find('td:eq(0)').text();
                        JsonData.CC = $(this).find('td:eq(3) label').data('cc');
                        JsonData.nomCC = $(this).find('td:eq(3) label').text();
                        JsonData.Reserva = unmaskDinero($(this).find('td:eq(4) input[type="text"]').val());
                        if(JsonData.Reserva > 0)
                            Array.push(JsonData);
                    }
                }
            });
            return Array;
        }
        function setReserva(){
            var totalRev = 0;
            var totalObra = 0;
            tblResumenObra.find('tr').each(function (idx, row) {
                if(idx > 0){
                    var numDin = $(this).find('td:eq(4) input').val();
                    var ccRsv = $(this).find('td:eq(3) label').text();
                    var overhaul = $(this).find('td:eq(4) label').text()
                    var saldo = $(this).find('td:eq(5) label').text()
                    if(numDin != undefined) {
                        var numDec = unmaskDinero(numDin);
                        totalRev += numDec;
                    }
                    if(overhaul != "" && overhaul == saldo){
                        totalRev += unmaskDinero(overhaul);
                    }
                    $(this).find('td:eq(4) input[type="text"]').val(maskDinero(numDec));
                    infoMaquina.find('tr').each(function (idx, row) {
                        if(idx > 0){
                            var html = $(this).find('td:eq(0)');
                            var total = +(html.find(".labelFont").attr('data-total'));
                            if(totalRev > 0 && !isNaN(total)){
                                var cc = html.find('.cc').text();
                                var suma = 0;
                                if(cc == ccRsv){
                                    suma = maskDinero(total - totalRev);
                                    totalObra += totalRev;
                                    html.find('.labelFont').text(suma);
                                }
                                if(cc.length == 0){
                                    suma = maskDinero(total - totalObra);
                                    html.find('.labelFont').text(suma);
                                }
                            }
                        }
                    });
                    if(ccRsv.length == 0){
                        var RevOriginal = +($(this).find('td:eq(4) label').data('original'));
                        if(!isNaN(RevOriginal)){
                            var suma = maskDinero(RevOriginal + totalRev);
                            $(this).find('td:eq(4) label').text(suma);
                            var PagOriginal = +($(this).find('td:eq(6) label').data('original'));
                            var pago = maskDinero(PagOriginal - totalRev);
                            $(this).find('td:eq(6) label').text(pago);
                            totalRev = 0;
                        }
                    }
                }
            });
        }
        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[\$\(\),]/g, ""));
        }
        function maskDinero(numero) {
            return "$" + parseFloat(numero).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,")
        }
        function getViernes(d) {
            d = new Date(d);
            var day = d.getDay(),
                diff = d.getDate() - day + (day == 0 ? -6:5);
            return new Date(d.setDate(diff));
          }
        init();
    };
    $(document).ready(function () {
        Administrativo.Contabilidad.Reportes.CadenaObra = new CadenaObra();
    });
})();