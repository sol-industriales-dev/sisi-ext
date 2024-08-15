var iframeDownload = $("#iframeDownload");
(function () {

    $.namespace('Administrativo.Contabilidad.Reportes.AutorizacionCadenaPrincial');

    AutorizacionCadenaPrincial = function () {

        const tblVencimiento = $("#tblVencimiento");
        const ireport = $("#report");
        const botonAutorizar = $('#botonAutorizar');
        const botonSeleccionarTodo = $('#botonSeleccionarTodo');
        const numeroDocumentos = $('#numeroDocumentos');

        // Modal facturas
        const modalFacturas = $('#modalFacturas');
        const tablaFacturas = $('#tablaFacturas');

        (function init() {
            initGrid();
            initFacturasGrid();
            LoadInfo();
            botonAutorizar.click(autorizarDocumentos);
            botonSeleccionarTodo.click(seleccionarTodo);
        })();

        function LoadInfo() {
            $.get('/Administrativo/Reportes/GetDocumentosPorAutorizar')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        tblVencimiento.bootgrid('clear');
                        tblVencimiento.bootgrid("append", response.data);
                        tblVencimiento.bootgrid('reload');
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`));
        }

        function seleccionarTodo() {
            tblVencimiento.find("td > input.autorizar")
                .toArray()
                .forEach(x => x.checked = true);
            actualizarContadorDocumentos();
        }

        function initGrid() {
            tblVencimiento.bootgrid({
                templates: {
                    search: ""
                },
                columnSelection: false,
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                labels: { infos: '{{ctx.total}} pendientes por autorizar' },
                formatters: {
                    "imprimir": (column, row) =>
                        "<button type='button' class='btn btn-primary print' data-id='" + row.id + "'>" +
                        "<span class='glyphicon glyphicon-print'></span> " +
                        " </button>"
                    ,
                    "facturas": (column, row) =>
                        `<button type=button class='btn btn-primary facturas' data-id='${row.id}'>
                        <i class="fas fa-list-alt"></i>
                        </button>`
                    ,
                    "autorizar": (column, row) => `<input data-id=${row.id} type="checkbox" class="form-control autorizar">`,
                    "rechazar": (column, row) => `<button data-id=${row.id}  class="btn btn-danger rechazar">Rechazar <i class="fas fa-times"></i></button>`
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                actualizarContadorDocumentos();
                /* Executes after data is loaded and rendered */
                tblVencimiento.find("button.print").on("click", function (e) {
                    let data = $(this).data();
                    $.ajax({
                        url: '/Administrativo/Reportes/SetDataPrint',
                        type: 'POST',
                        dataType: 'json',
                        data: {
                            id: data.id,
                            obj: {
                                idProveedor: 0,
                                total: 0
                            }
                        },
                        success: () => {
                            $.unblockUI();
                            verReporte();
                        },
                        error: function (response) {
                            $.unblockUI();
                            AlertaGeneral("Alerta", response.message);
                        }
                    });
                });
                tblVencimiento.find(`input[type="checkbox"].autorizar`).on("click", function (e) {
                    actualizarContadorDocumentos();
                });
                tblVencimiento.find(`button.rechazar`).on("click", function (e) {
                    const data = $(this).data();
                    AlertaAceptarRechazarNormal("Rechazar documento",
                        `<p>¿Está seguro que desea rechazar el documento?<p>
                        <p>Puede dejar un comentario de rechazo (opcional).</p>
                        <textarea rows="4" cols="50" id="inputComentarioRechazo" class="form-control"></textarea>
                        `,
                        () => {
                            const comentarioRechazo = $("#inputComentarioRechazo").val();
                            rechazarDocumento(data.id, comentarioRechazo);
                        },
                        null);
                });
                tblVencimiento.find(`button.facturas`).on("click", function (e) {
                    const data = $(this).data();
                    cargarFacturasCadena(data.id);
                });
            });
        }

        function initFacturasGrid() {
            tablaFacturas.bootgrid({
                templates: {
                    search: ""
                },
                columnSelection: false,
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                labels: { infos: '{{ctx.total}} facturas' },
                formatters: {
                    "descargar": (column, row) => {
                        let button = "";
                        if (row.tienePDF) {
                            button += `<button data-numProveedor=${row.numProveedor} data-factura=${row.factura} data-centro_costos=${row.centro_costos} data-id=${row.id} 
                            class="btn btn-primary pdf"><i class="fas fa-file-pdf"></i></button>`;
                        } else {
                            button += `<span>N/D  </span>`;
                        }

                        if (row.tieneXML) {
                            button += `<button data-numProveedor=${row.numProveedor} data-factura=${row.factura} data-centro_costos=${row.centro_costos} data-id=${row.id}
                                class="btn btn-primary xml"><i class="fas fa-file-code"></i></button>`;
                        } else {
                            button += `<span>  N/D</span>`;
                        }
                        return button;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {

                /* Executes after data is loaded and rendered */
                tablaFacturas.find("button.pdf").on("click", function (e) {
                    const data = $(this).data();
                    const numProveedor = data.numproveedor;
                    const factura = data.factura;
                    const cc = data.centro_costos;
                    $.get('/Administrativo/Reportes/ObtenerRutaPDFFactura', { numProveedor, factura, cc })
                        .then(response => {
                            if (response.success) {
                                // Operación exitosa.
                                window.location.href = "/Administrativo/Propuesta/fnDownloadFile?descargar=" + response.url;
                            } else {
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                            }
                        }, error => {
                            // Error al lanzar la petición.
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                });

                tablaFacturas.find("button.xml").on("click", function (e) {
                    const data = $(this).data();
                    const numProveedor = data.numproveedor;
                    const factura = data.factura;
                    const cc = data.centro_costos;
                    $.get('/Administrativo/Reportes/ObtenerRutaXMLFactura', { numProveedor, factura, cc })
                        .then(response => {
                            if (response.success) {
                                // Operación exitosa.
                                window.location.href = "/Administrativo/Propuesta/fnDownloadFile?descargar=" + response.url;
                            } else {
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                            }
                        }, error => {
                            // Error al lanzar la petición.
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                });
            });
        }

        function cargarFacturasCadena(cadenaID) {
            if (cadenaID == null) {
                return;
            }
            $.get('/Administrativo/Reportes/ObtenerFacturasCadena', { cadenaID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        mostrarModalFacturas(response.items);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function mostrarModalFacturas(facturas) {
            modalFacturas.modal('show');
            tablaFacturas.bootgrid('clear');
            tablaFacturas.bootgrid("append", facturas);
            tablaFacturas.bootgrid('reload');
        }

        function verReporte() {
            $.blockUI({ message: "Generando reporte..." });
            var idReporte = "24";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function autorizarDocumentos() {
            const idsDocumentosPorAutorizar = obtenerDocumentosPorAutorizar();

            if (idsDocumentosPorAutorizar == null || idsDocumentosPorAutorizar.length == 0) {
                AlertaGeneral(`Aviso`, `Debe seleccionar por lo menos un documento para autorizar.`);
                return;
            }

            $.post('/Administrativo/Reportes/AutorizarDocumentos', { idsDocumentosPorAutorizar })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Documentos autorizados correctamente.`);
                        LoadInfo();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function rechazarDocumento(cadenaID, comentarioRechazo) {
            $.post('/Administrativo/Reportes/RechazarDocumento', { cadenaID, comentarioRechazo })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Se rechazó el documento correctamente.`);
                        LoadInfo();
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function obtenerDocumentosPorAutorizar() {
            return tblVencimiento.find("td > input.autorizar")
                .toArray()
                .filter(x => x.checked)
                .map(input => $(input).data().id);
        }

        function actualizarContadorDocumentos() {
            const totalDocumentosSeleccionados = tblVencimiento.find("td > input.autorizar").toArray().filter(x => x.checked);
            numeroDocumentos.html(`${totalDocumentosSeleccionados.length}`);
        }

    };

    $(document).ready(function () { Administrativo.Contabilidad.Reportes.AutorizacionCadenaPrincial = new AutorizacionCadenaPrincial(); })
        .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(function () { $.unblockUI(); });
})();