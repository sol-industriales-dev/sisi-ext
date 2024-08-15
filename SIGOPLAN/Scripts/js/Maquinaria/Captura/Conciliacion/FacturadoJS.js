
(function () {

    $.namespace('maquinaria.captura.conciliacion.facturado');

    facturado = function () {
        mensajes = {
            NOMBRE: 'Autorizacion de Caratulas',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        
        const cboEstado = $("#cboEstado");
        const txtFolioConciliacion = $("#txtFolioConciliacion");
        const cboCC = $("#cboCC");
        const txtFechaInicio = $("#txtFechaInicio");
        const txtFechaFin = $("#txtFechaFin");
        const btnBuscar = $("#btnBuscar");
        const tblConciliaciones = $("#tblConciliaciones");
        let dtTblConciliaciones;

        const modalGuardarFactura = $("#modalGuardarFactura");
        const txtFolioFactura = $("#txtFolioFactura");
        const btnModalGuardar = $("#btnModalGuardar");
        const btnAgregarFactura = $("#btnAgregarFactura");
        const tblFacturas = $("#tblFacturas");
        let dtTblFacturas;

        const getConciliaciones = new URL(window.location.origin + '/Conciliacion/getConciliacionesAFacturar');
        const indicarFacturacion = new URL(window.location.origin + '/Conciliacion/indicarFacturacion');
        const getFacturas = new URL(window.location.origin + '/Conciliacion/getFacturasConciliacion');

        function init() {
            initTblConciliaciones();
            initTblFacturas();
            addListeners();
            btnBuscar.click(function (e) { cargarTblConciliaciones(e); });
            cboCC.select2();
            cboCC.fillCombo('/CatObra/cboCentroCostosUsuarios');
        }

        function addListeners()
        {
            $(document).on('click', "#btnModalEliminar", function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                IndicarFacturacionConciliacion(e);
            });

            btnModalGuardar.click(function (e) {
                ConfirmacionEliminacion("Desecho", "¿Desea indicar la facturación de la conciliación con el folio " + $(this).attr("data-folio") + "?");
            });
            btnAgregarFactura.click(agregarFacturaTabla);
        }

        function initTblConciliaciones()
        {
            dtTblConciliaciones = tblConciliaciones.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                paging: true,
                searching: true,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 7]
                        }
                    }
                ],
                columns: [
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'folio', title: 'Folio' },
                    { data: 'cc', title: 'Centro de Costos' },
                    {
                        data: 'fechaInicioRaw',
                        title: 'Fecha Inicio',
                        render: function (data, type, row) {
                            return row.fechaInicio
                        }
                    },
                    {
                        data: 'fechaFinRaw',
                        title: 'Fecha Fin',
                        render: function (data, type, row) {
                            return row.fechaFin
                        }
                    },
                    {
                        data: 'importe',
                        title: 'Importe',
                        render: function (data, type, row) {
                            return parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
                        }
                    },
                    {
                        data: 'facturar',
                        title: 'Facturado',
                        render: function (data, type, row) {
                            return "<button type='button' class='btn btn-primary facturar' style='' data-folio='" + row.folio + "' data-id='" + row.id + "'><i class='fa fa-file-invoice-dollar' style='font-size: 35px;'></i></span></button>";
                        }
                    },
                    {
                        data: 'facturas',
                        title: 'Facturas',
                        visible: false
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                fnInitComplete: function (oSettings, json) {
                    $('div#tblConciliaciones_filter input').addClass("form-control input-sm");
                },
                drawCallback: function () {
                    tblConciliaciones.find('.facturar').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnModalGuardar.attr("data-folio", $(this).attr("data-folio"));
                        btnModalGuardar.attr("data-id", $(this).attr("data-id"));
                        cargarTblFacturas(e);
                        txtFolioFactura.val("");
                        modalGuardarFactura.modal("show");
                    });
                }
            });
        }

        function cargarTblConciliaciones(e) {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            $('#tblConciliaciones tbody td').addClass("blurry");
            $.post(getConciliaciones, {
                estado: cboEstado.val() == "1",
                fechaInicio: txtFechaInicio.val(), 
                fechaFin: txtFechaFin.val(), 
                folio: txtFolioConciliacion.val(), 
                cc: $('#cboCC option:selected').attr('data-prefijo')
            })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        dtTblConciliaciones.clear();
                        dtTblConciliaciones.rows.add(response.conciliaciones);
                        dtTblConciliaciones.draw();
                        setTimeout(function () { $('#tblConciliaciones tbody td').removeClass("blurry"); }, 600);
                    } else {
                        // Operación no completada.
                        setTimeout(function () { $('#tblConciliaciones tbody td').removeClass("blurry"); }, 600);
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    // Error al lanzar la petición.
                    setTimeout(function () { $('#tblConciliaciones tbody td').removeClass("blurry"); }, 600);
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        function initTblFacturas()
        {
            dtTblFacturas = tblFacturas.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                paging: false,
                searching: false,
                dom: '<<t>>',
                columns: [
                    { data: 'factura', title: 'FACTURAS' },
                    {
                        data: 'eliminar',
                        title: 'ELIMINAR',
                        render: function (data, type, row) {
                            return "<button type='button' class='btn btn-primary eliminar'><i class='fas fa-minus-square'></i></span></button>";
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                order: [[0, 'asc'], [1, 'asc']],
                drawCallback: function () {
                    tblFacturas.find('.eliminar').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var index = $(this).parents("tr").index();
                        dtTblFacturas.row(index).remove().draw();
                    });
                }
            });
        }

        function cargarTblFacturas(e)
        {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            $.post(getFacturas, {
                conciliacionID: btnModalGuardar.attr("data-id")
            })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        dtTblFacturas.clear();
                        dtTblFacturas.rows.add(response.facturas);
                        dtTblFacturas.draw();
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
            );
        }

        function IndicarFacturacionConciliacion(e) {
            var facturas =
            dtTblFacturas.columns(0).data()[0];
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            if (facturas.length > 0) {
                $.post(indicarFacturacion, {
                    conciliacionID: btnModalGuardar.attr("data-id"),
                    factura: facturas
                })
                    .then(function (response) {
                        if (response.success) {
                            // Operación exitosa.
                            AlertaGeneral('Operación exitosa', 'Se actualizó el estado de la conciliación ' + btnModalGuardar.attr("data-folio"));
                            setTimeout(function () { $('#tblConciliaciones tbody td').removeClass("blurry"); }, 600);
                            btnBuscar.click();
                            modalGuardarFactura.modal("hide");
                        } else {
                            // Operación no completada.
                            setTimeout(function () { $('#tblConciliaciones tbody td').removeClass("blurry"); }, 600);
                            AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                        }
                    }, function (error) {
                        // Error al lanzar la petición.
                        setTimeout(function () { $('#tblConciliaciones tbody td').removeClass("blurry"); }, 600);
                        AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                    }
                );
            }
            else
            {
                AlertaGeneral('Operación fallida', 'Folio de factura invalido');
            }
        }

        function agregarFacturaTabla()
        {
            var factura = txtFolioFactura.val().trim();
            if (factura != "")
            {
                dtTblFacturas.row.add({ "factura": factura }).draw();
                txtFolioFactura.val("");
            }
        }

        init();

    };

    $(document).ready(function () {
        maquinaria.captura.conciliacion.facturado = new facturado();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();

