(function () {
    $.namespace('Administrativo.Contabilidad.Reportes.TotalSemanal');
    TotalSemanal = function () {
        mensajes = { PROCESANDO: 'Procesando...' };
        tblResumenSemanal = $("#tblResumenSemanal");
        cboTipo = $("#cboTipo");
        dpAnio = $("#dpAnio");
        btnCargar = $("#btnCargar");
        divBtnExcel = $("#divBtnExcel");
        btnExcel = $("#btnExcel");
        mdlFactura = $("#mdlFactura");
        tblFactura = $("#tblFactura");
        mdlInfo = $(".mdlInfo");
        var lstResultado;
        function init() {
            btnCargar.click(cargarTabla);
            btnExcel.click(download);
            initComboBox();
            cargarTabla();
            mdlFactura.on('shown.bs.modal', function () { $(this).find('.modal-dialog').css({ width: 'auto', height: 'auto', 'max-height': '100%' }); });
        }

        function initComboBox(){
            dpAnio.fillCombo('/Administrativo/Reportes/FillComboAnio', null, false, null); 
            dpAnio.val(new Date().getUTCFullYear());
        }

        function cargarTabla() {
            if ($('#tblResumenSemanal tr').length > 0)
                tblResumenSemanal.DataTable().destroy();
            $.ajax({
                url: '/Administrativo/Reportes/SetAcomuladoTotal',
                type: 'POST',
                dataType: 'json',
                data: { tipo: cboTipo.val(), anio: dpAnio.val()},
                success: function (response) {
                    $.unblockUI();
                    dataset = response.lstRenglon
                    divBtnExcel.prop("hidden", !(dataset.length > 0));
                    var my_columns = [];
                    var i = 0;
                    $.each( dataset[0], function( key, value ) {
                            var my_item = {};
                            my_item.data = key;
                            my_item.title = response.lstSemanas[i++];
                            my_item.sClass = key == 0 ? "" : "text-right";
                            my_columns.push(my_item);
                    });
                    var table = tblResumenSemanal.DataTable({
                        scrollY: false,
                        scrollX: true,
                        scrollCollapse: true,
                        searching: false,
                        ordering: false,
                        paging: false,
                        fixedColumns: true,
                        data: dataset,
                        "columns": my_columns,
                        "initComplete": function (settings, json) {
                            tblResumenSemanal.find('tbody').on('dblclick', 'td', function () {
                                let valor =  $(this).text();
                                if(Number(valor.replace(/[^0-9\.]+/g, "") != 0 && table.row($(this)).data()[0] != "")){
                                    var fecha = $(tblResumenSemanal.find('thead').find('tr')[0].childNodes[this._DT_CellIndex.column].innerHTML).text();
                                    mdlInfo[0].innerHTML = fecha;
                                    let response = $.post('/Administrativo/Reportes/getLstFacturaSemana', {
                                        nombre: table.row($(this)).data()[0],
                                        semana: this.cellIndex,
                                        tipo: cboTipo.val(),
                                        anio: dpAnio.val(),
                                        moneda: this._DT_CellIndex.row < 9 ? 1 : 2
                                    }, function (response) { 
                                        mdlInfo[1].innerHTML = response.semana;
                                        mdlInfo[2].innerHTML = response.banco;
                                        mdlInfo[3].innerHTML = response.factoraje;
                                        mdlInfo[4].innerHTML = response.total;
                                        mdlInfo[5].innerHTML = response.moneda;
                                        tblFactura.DataTable({
                                            destroy: true,
                                            deferRender: true,
                                            data: response.data,
                                            language: dtDicEsp,
                                            columnDefs:[
                                                {targets: 0, data: 'factura'},
                                                {targets: 1, data: 'proveedor'},
                                                {targets: 2, data: 'tipoCC'},
                                                {targets: 3, data: 'cc'},
                                                {targets: 4, data: 'saldo', sClass: 'text-right'},
                                                {targets: 5, data: 'emision'},
                                                {targets: 6, data: 'vencimiento'},
                                                {targets: 7, data: 'banco'},
                                                {targets: 8, data: 'factoraje'},
                                                {targets: 9, data: 'moneda'},
                                                {targets: 10, data: 'tc', sClass: 'text-right'},
                                            ]
                                        });
                                        mdlFactura.modal('show');
                                    }, 'json');
                                }
                            });
                        }
                    });
                    $("#tblResumenSemanal_info").addClass("hidden");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function download() {
            $.when($.blockUI({ message: "Preparando archivo a descargar" })).then(function () {
                iframe = document.getElementById('iframeDownload');
                iframe.src = '/Administrativo/Reportes/getExcelAcomulado';

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

        function ReasignaBanco() {
            $.blockUI({ message: "Sincronizando bancos" });
            $.ajax({
                url: '/Administrativo/Reportes/ReasignaBanco',
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                }
            });
        }
        init();
    };
    $(document).ready(function () {
        Administrativo.Contabilidad.Reportes.TotalSemanal = new TotalSemanal();
    });
})();