(function () {
    $.namespace('Administrativo.Contabilidad.Reportes.CadenaProductiva');
    CadenaProductiva = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        var tableProveedores,
            btnGuardar = $("#btnGuardar"),
            tblDetalleProveedores = $("#tblDetalleProveedores"),
            modalDetalleProveedores = $("#modalDetalleProveedores"),
            btnVerAdjuntos = $("#btnVerAdjuntos"),
            btnSetDatosAdjuntos = $("#btnSetDatosAdjuntos"),
            cboBancos = $("#cboBancos"),
            cboTipoFactura = $("#cboTipoFactura"),
            ireport = $("#report"),
            btnRegresar = $("#btnRegresar"),
            btnImprimir = $("#btnImprimir");
        btnExportCadena = $("#btnExportCadena"),
            txtCentroCostos = $("#txtCentroCostos"),
            ListaDetalleVencimientos = $("#ListaDetalleVencimientos"),
            ListaVencimientos = $("#ListaVencimientos"),
            tblVencimientoDet = $("#tblVencimientoDet"),
            tbFechaVencimiento = $("#tbFechaVencimiento"),
            tbFechaEmision = $("#tbFechaEmision"),
            btnVerReporte = $("#btnVerReporte"),
            tbCentroCostos = $("#tbCentroCostos"),
            btnCerrar = $("#btnCerrar"),
            tblVencimiento = $("#tblVencimiento");
        table = tblVencimiento.DataTable({
            language: dtDicEsp,
            "bFilter": true
        });
        function init() {
            tbCentroCostos.onEnter(getDataVencimiento);
            btnSetDatosAdjuntos.click(AdjuntarFacturas);
            btnImprimir.click(verReporte);
            btnRegresar.click(regresar);
            cboTipoFactura.change(getDataVencimiento);
            tbFechaEmision.datepicker().datepicker("setDate", new Date());
            tbFechaVencimiento.datepicker().datepicker("setDate", new Date());
            btnVerAdjuntos.click(verlistaAjuntos);
            btnCerrar.click(fnCerrar);
            btnGuardar.click(GuardarInfo);
            modalDetalleProveedores.on('shown.bs.modal', function () { $(this).find('.modal-dialog').css({ width: 'auto', height: 'auto', 'max-height': '100%' }); });
        }
        function fnCerrar() {
            modalDetalleProveedores.modal("hide");
        }
        var dataSetCompleto = [];
        var TempProvv = [];
        function AdjuntarFacturas() {
            $.ajax({
                url: '/Administrativo/Reportes/setVariosProveedores',
                type: 'POST',
                dataType: 'json',
                data: { array: dataSetCompleto, centro_costos: "01" },
                success: function (response) {
                    TempProvv.push(dataSetCompleto);
                    btnSetDatosAdjuntos.prop("disabled", true);
                    AlertaGeneral("Confirmación", "Registros adjuntados correctamente");
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function regresar() {
            $.ajax({
                url: '/Administrativo/Reportes/clearProveedores',
                type: 'POST',
                dataType: 'json',
                async: false,
                success: function (response) {
                    TempProvv = [];
                    window.location.href = "/Administrativo/Reportes/CadenaProductiva";
                },
                error: function (response) {
                }
            });
            ListaVencimientos.removeClass('hide');
            ListaDetalleVencimientos.addClass('hide');
        }
        function GuardarInfo() {
            var response = $.post('/Administrativo/Reportes/Guardar', {
                lst: getFacturaParcial()
            }, function (response) {
                modalDetalleProveedores.modal('hide');
                AlertaGeneral("Confirmación", "Archivo guardado correctamente");
                setInterval(function () {
                    window.location.href = "/Administrativo/Reportes/CadenaProductiva";
                }, 2000);
            }, 'json');
            response.fail(function (response) {
                AlertaGeneral("Alerta", response.message);
            });
        }
        function getFacturaParcial() {
            let lst = [];
            tableProveedores.rows().iterator('row', function (context, index) {
                let node = $(this.row(index).node()),
                    total = unmaskNumero(node.find('td:eq(5)').text()),
                    abonado = unmaskNumero(node.find('td:eq(6)').text()),
                    abonar = unmaskNumero(node.find('.txtAbono').val());
                if (abonar != total && +(abonado + abonar).toFixed(2) <= total)
                    lst.push({
                        numProv: node.find('td:eq(0)').text(),
                        factura: node.find('td:eq(2)').text(),
                        total: total,
                        abonado: abonar
                    });
            });
            return lst;
        }
        function verlistaAjuntos() {
            tableProveedores = tblDetalleProveedores.DataTable({
                ajax: { url: '/Administrativo/Reportes/getListaAdjuntos' },
                destroy: true,
                language: dtDicEsp,
                "bInfo": false,
                "bFilter": false,
                paging: true,
                "columns": [
                    { data: "numProveedor" },
                    { data: "proveedor" },
                    { data: "factura" },
                    { data: "fechaS" },
                    { data: "fechaVencimientoS" },
                    {
                        data: "total", createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(maskNumero(cellData)).addClass("text-right");
                        }
                    },
                    {
                        data: "abono",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(maskNumero(cellData)).addClass("text-right");
                        }
                    },
                    {
                        data: "diff",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`<input/>`);
                            $(td).find("input").addClass('form-control txtAbono text-right');
                            $(td).find("input").val(maskNumero(cellData));
                        }
                    },
                    {
                        data: "diff",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(`<input/>`);
                            $(td).find("input").addClass('form-control txtDiff text-right');
                            $(td).find("input").val(maskNumero(0));
                            $(td).find("input").attr("disabled", true);
                        }
                    },
                    { data: "centro_costos" },
                    { data: "nombCC" }
                ],
                "initComplete": function (settings, json) {
                    tableProveedores.on('change', '.txtAbono', function () {
                        let row = $(tableProveedores.row($(this).parents('tr')).node()),
                            abono = unmaskNumero(row.find('.txtAbono').val()),
                            total = unmaskNumero(row.find('td:eq(5)').text()),
                            abonado = unmaskNumero(row.find('td:eq(6)').text()),
                            diff = total - abonado;
                        if (abono <= 0 || abono > diff) {
                            let data = tableProveedores.row($(this).parents('tr')).data();
                            abono = data.diff;
                            diff = 0;
                        } else {
                            diff = diff - abono;
                        }
                        row.find('.txtAbono').val(maskNumero(abono));
                        row.find('.txtDiff').val(maskNumero(diff));
                    });
                }
            });
            modalDetalleProveedores.modal('show');
        }
        function getDataVencimiento() {
            table.clear().draw();
            table.destroy();
            table = tblVencimiento.DataTable({
                destroy: true,
                "ajax": {
                    "url": "/Administrativo/Reportes/getInfoVencimiento",
                    "dataSrc": "datosTable",
                    data: function (d) {
                        d.centrocostos = tbCentroCostos.val(),
                        d.tipoFactura = cboTipoFactura.val()
                    }
                },
                language: dtDicEsp,
                "bInfo": false,
                "bFilter": false,
                scrollY: '50vh',
                scrollCollapse: true,
                "bFilter": true,
                paging: false,
                "columns": [
                    { data: "numProveedor" },
                    { data: "proveedor" },
                    { data: "factura" },
                    { data: "fecha." },
                    { data: "fechaVencimiento" },
                    { data: "fechaTimbrado" },
                    {
                        data: "saldoFactura", createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData).addClass("text-right");
                        }
                    },
                    { data: "centro_costos" },
                    { data: "tipoCambio" },
                    { data: "tipoFactura" },
                    { data: null }
                ],
                columnDefs: [
                    // {
                    //     orderable: false,
                    //     className: 'select-checkbox',
                    //     targets: 9
                    // },
                    {
                        targets: [10],
                        render: function(data, type, row) {
                            if (row.bloqueado) {
                                return '';
                            } else {
                                return '<input type="checkbox" class="select-checkbox">';
                            }
                        },
                        className: 'text-center'
                    }
                 ],
                 drawCallback: function(settings) {
                    var data = tblVencimiento.DataTable().row(0).data();
                    if (data != undefined) {
                        if (data.bloqueado) {
                            btnVerAdjuntos.hide();
                            AlertaGeneral('Alerta', `PROVEEDOR ${data.descripcionBloqueo}. MODULO DE SUBCONTRATISTAS`);
                        } else {
                            btnVerAdjuntos.show();
                        }
                    }
                 },
                select: {
                    style: 'multi',
                    selector: 'td:last-child'
                },
            });
            btnVerReporte.removeClass('hide');
            $('#btnVerReporte').click(function () {
                var dataSet = table.rows('.selected').data();
                var dataSetDet = [];
                dataSetCompleto = [];
                for (var i = 0; i < table.rows('.selected').data().length; i++) {
                    var array = new Array();
                    array.push(dataSet[i].centro_costos);
                    array.push(dataSet[i].factura);
                    array.push(dataSet[i].proveedor);
                    array.push(dataSet[i].saldoFactura);
                    array.push(dataSet[i].nombCC);
                    array.push(dataSet[i].tipoMoneda == 1 ? 'MN' : 'USD');
                    array.push(dataSet[i].area_cuenta);
                    array.push(dataSet[i].orden_compra);
                    dataSetDet.push(array);
                    dataSetCompleto.push(dataSet[i]);
                }
                if (dataSetCompleto.length > 0) {
                    SendData(dataSetCompleto, tbCentroCostos.val());
                    viewReportPrev(dataSetDet, tbCentroCostos.val());
                }
                else {
                    AlertaGeneral("Alerta", "Debe seleccionar por lo menos una factura para ver el detalle.");
                }

            });

        }
        table.on('select', function (e, dt, type, indexes) {
            if (type === 'row') {
                var data = table.rows(indexes).data()[0];
                var tipoFactura = data.tipoFactura;
                if (tipoFactura != "Vencida") {
                    AlertaGeneral("Notificacion", "Esta factura no esta vencida aun.");
                }
            }
        });
        var tableDet = tblVencimientoDet.DataTable();
        function viewReportPrev(dataSet, centro_costos) {
            ListaVencimientos.addClass('hide');
            ListaDetalleVencimientos.removeClass('hide');
            tableDet.clear().draw();
            tableDet.destroy();

            tableDet = $('#tblVencimientoDet').DataTable({
                "columnDefs": [
                    { "visible": false, "targets": 4 }
                ],
                language: dtDicEsp,
                "bFilter": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                paging: false,
                info: false,
                "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(4, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td colspan="5">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });
                },
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    //Tipo moneda
                    tipoMoneda = api
                        .column(5)
                        .data()[0];
                    // Total over all pages
                    total = api
                        .column(3)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Total over this page
                    pageTotal = api
                        .column(3, { page: 'current' })
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);

                    // Update footer
                    $(api.column(3).footer()).html(`${maskNumero(total)} ${tipoMoneda}`);
                }
            });
            $('#btnExportCadena').click(function () {
                var IF = cboBancos.val();
                var json = {};
                json.IF = IF;
                json.FechaEmision = tbFechaEmision.val();
                json.FechaVencimiento = tbFechaVencimiento.val();
                getDocumento(json);
            });
        }
        function verReporte(e) {
            var idReporte = "24";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
            e.preventDefault();
        }

        function getDocumento(array) {
            $.ajax({
                url: '/Administrativo/Reportes/setDatosID',
                type: 'POST',
                dataType: 'json',
                data: { array: array },
                success: function (response) {
                    window.location.href = '/Administrativo/Reportes/getFileDownload';
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function SendData(array, centro_costos) {
            $.ajax({
                url: '/Administrativo/Reportes/setDatosFactura',
                type: 'POST',
                dataType: 'json',
                data: { array: array, centro_costos: centro_costos },
                success: function (response) { },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        init();
    };

    $(document).ready(function () {
        Administrativo.Contabilidad.Reportes.CadenaProductiva = new CadenaProductiva();
    })
        .ajaxStart(() => {
            $.blockUI({
                baseZ: 100000,
                message: 'Procesando...'
            });
        })
        .ajaxStop(() => { $.unblockUI(); });
})();


