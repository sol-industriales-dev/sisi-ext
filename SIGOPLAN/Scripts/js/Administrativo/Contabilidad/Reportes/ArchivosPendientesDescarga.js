var iframeDownload = $("#iframeDownload");
(function () {

    $.namespace('Administrativo.Contabilidad.Reportes.CadenaProductiva');

    CadenaProductiva = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        ireport = $("#report");
        mensajes = {
            NOMBRE: 'Reporte Cadena Productiva',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        btnEnvioCorreos = $("#btnEnvioCorreos"),
            cboFactoraje = $("#cboFactoraje");
        cboBancos = $("#cboBancos");
        tbFechaVencimiento = $("#tbFechaVencimiento");
        tbFechaEmision = $("#tbFechaEmision");
        tblVencimiento = $("#tblVencimiento");
        tblAplicados = $("#tblAplicados");
        hdnIdEliminar = $("#hdnIdEliminar");
        btnEliminar = $("#btnEliminar");
        modalEliminar = $("#modalEliminar");
        tbFiltroEmision = $("#tbFiltroEmision");
        btnBuscar = $("#btnBuscar");
        txtCambio = $("#txtCambio");
        btnRefresh = $("#btnRefresh");
        const mdlNafin = $('#mdlNafin');
        function init() {
            cboFactoraje.change(fnTipoFactoraje);
            tbFechaEmision.datepicker().datepicker("setDate", new Date());
            tbFechaVencimiento.datepicker().datepicker("setDate", new Date());
            tbFiltroEmision.datepicker().datepicker("setDate", new Date());
            btnEliminar.click(Eliminar);
            btnBuscar.click(LoadAplicadosPorFecha);
            btnRefresh.click(LoadAplicados);
            loadTablas();
        }
        mdlNafin.on('hidden.bs.modal', function () {
            loadTablas();
        });
        function loadTablas() {
            $.when(initGrid())
                .then(LoadInfo(), LoadAplicados());
        }
        function fnTipoFactoraje() {
            var _this = $(this);
            if (_this.val() == 'V') {
                cboBancos.val("");
                cboBancos.prop("disabled", false);
            }
            else if (_this.val() == 'N') {
                cboBancos.val(3217);
                cboBancos.prop("disabled", true);
            }
        }
        function LoadInfo() {
            $.ajax({
                url: '/Administrativo/Reportes/GetInfoDocumentos',
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        var dataRes = response.DataSend;
                        tblVencimiento.bootgrid('clear');
                        tblVencimiento.bootgrid("append", dataRes);
                        tblVencimiento.bootgrid('reload');
                    }
                },
                error: function (response) { }
            });
        }
        function LoadAplicados() {
            $.ajax({
                url: '/Administrativo/Reportes/GetInfoDocumentosAplicados',
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        var dataRes = response.DataSend;
                        tblAplicados.bootgrid('clear');
                        tblAplicados.bootgrid("append", dataRes);
                        tblAplicados.bootgrid('reload');
                    }
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function LoadAplicadosPorFecha() {
            $.ajax({
                url: '/Administrativo/Reportes/GetInfoDocumentosAplicadosPorFecha',
                type: 'POST',
                dataType: 'json',
                data: { fecha: tbFiltroEmision.val(), cambio: txtCambio.val() },
                success: function (response) {
                    if (response.success) {
                        var dataRes = response.DataSend;
                        tblAplicados.bootgrid('clear');
                        tblAplicados.bootgrid("append", dataRes);
                        tblAplicados.bootgrid('reload');
                    }
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function Eliminar() {
            $.blockUI({ message: mensajes.PROCESANDO })
            try {
                var response = $.post("/Administrativo/Reportes/Eliminar", {
                    id: hdnIdEliminar.val()
                }, function (response) {
                    if (hdnIdEliminar.data().estatus)
                        LoadInfo();
                    else
                        LoadAplicados();
                    $.unblockUI();
                    hdnIdEliminar.val(0);
                    modalEliminar.modal("hide");
                }, 'json');
            } catch (error) {
                AlertaGeneral("Alerta", error.message);
            }
        }
        function initGrid() {
            tblVencimiento.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                labels: { infos: '{{ctx.total}} Cadenas Productivas' },
                formatters: {
                    "descargar": function (column, row) {
                        if (row.estadoAutorizacion == 2) {
                            return "<button type='button' class='btn btn-primary download' data-id='" + row.id + "'>" +
                                "<span class='glyphicon glyphicon-download'></span> " +
                                " </button>";
                        } else {
                            return "";
                        }
                    },
                    "imprimir": function (column, row) {
                        return "<button type='button' class='btn btn-primary print' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-print'></span> " +
                            " </button>"
                            ;
                    },
                    "excel": function (column, row) {
                        if (row.estadoAutorizacion == 2) {
                            return "<button type='button' class='btn btn-success excel' data-id='" + row.id + "'>" +
                                "<span class='glyphicon glyphicon-th'></span> " +
                                " </button>";
                        } else {
                            return "";
                        }
                    },
                    "eliminar": function (column, row) {
                        return `<button type='button' class='btn btn-danger eliminar' data-id='${row.id}' data-estatus='${true}'><span class='glyphicon glyphicon-trash'></span></button>`;
                    },
                    "vobo": function (column, row) {
                        if (row.permisoVobo && row.estadoAutorizacion == 0) {
                            return `<button type='button' class='btn btn-primary vobo' data-id='${row.id}' data-estatus='${true}'><span class='glyphicon glyphicon-eye-open'></span></button>`;
                        } else {
                            return ``;
                        }
                    },
                    "estado": (column, row) => {
                        switch (row.estadoAutorizacion) {
                            case 0:
                                return `<span class="label label-default">Pendiente</span>`;
                            case 1:
                                return `<span class="label label-primary">VoBo aplicado</span>`;
                            case 2:
                                return `<span class="label label-success">Autorizada</span>`;
                            case 3:
                                return `<span data-comentario='${row.comentarioRechazo}' class="label label-danger">Rechazada</span>`
                            default:
                                throw "Estado de autorización indefinido.";
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblVencimiento.find(".download").on("click", function (e) {
                    var idRegistros = $(this).attr('data-id');
                    if (validateDescarga()) {
                        getDocumento(idRegistros, cboFactoraje.val(), tbFechaEmision.val(), tbFechaVencimiento.val(), cboBancos.val(), $("#cboBancos option:selected").text());
                        cboFactoraje.val("V");
                        cboFactoraje.change();
                        cboBancos.val("");
                    }
                    else {
                        AlertaGeneral("Confirmación", "¡Todos los campos son obligatorios!");
                    }

                });
                tblVencimiento.find(".print").on("click", function (e) {
                    let data = $(this).data(),
                        row = $(this).closest('tr');
                    $.ajax({
                        url: '/Administrativo/Reportes/SetDataPrint',
                        type: 'POST',
                        dataType: 'json',
                        data: {
                            id: data.id,
                            obj: {
                                idProveedor: row.find('td').eq(0).text(),
                                total: unmaskDinero(row.find('td').eq(5).text()),
                                fecha: row.find('td').eq(7).text()
                            }
                        },
                        success: function (response) {
                            $.unblockUI();
                            verReporte();
                        },
                        error: function (response) {
                            $.unblockUI();
                            AlertaGeneral("Alerta", response.message);
                        }
                    });
                });
                tblVencimiento.find(".excel").on("click", function (e) {
                    row = $(this).closest('tr');
                    getExcel($(this).attr('data-id'), {
                        idProveedor: row.find('td').eq(0).text(),
                        total: unmaskDinero(row.find('td').eq(3).text()),
                        fecha: row.find('td').eq(5).text()
                    });
                });
                tblVencimiento.find(".eliminar").on("click", function (e) {
                    hdnIdEliminar.val($(this).data().id);
                    hdnIdEliminar.data().estatus = $(this).data().estatus;
                    modalEliminar.modal("show");
                });
                tblVencimiento.find(".vobo").on("click", function (e) {
                    const cadenaID = $(this).data().id;
                    AlertaAceptarRechazarNormal("Dar VoBo",
                        "¿Está seguro que desea dar visto bueno?",
                        () => darVoBo(cadenaID),
                        null);
                });
                tblVencimiento.find("span.label-danger").on("click", function (e) {
                    let comentario = $(this).data().comentario;
                    if (comentario == null) {
                        comentario = "No se otorgó un comentario de rechazo."
                    } else {
                        comentario = comentario.split("\n").map(line => `<p>${line}</p>`).join(`</p>`);
                    }
                    AlertaGeneral(`Comentario de rechazo`, `${comentario}`);
                });
            });

            tblAplicados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                labels: { infos: '{{ctx.total}} Cadenas Productivas' },
                formatters: {
                    "descargar": function (column, row) {
                        return "<button type='button' class='btn btn-primary downloadAplicado' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-download'></span> " +
                            " </button>"
                            ;
                    },
                    "imprimir": function (column, row) {
                        return "<button type='button' class='btn btn-primary print' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-print'></span> " +
                            " </button>"
                            ;
                    },
                    "excel": function (column, row) {
                        return "<button type='button' class='btn btn-success excel' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-th'></span> " +
                            " </button>"
                            ;
                    },
                    "eliminar": function (column, row) {
                        return `<button type='button' class='btn btn-danger eliminar' data-id='${row.id}' data-estatus='${false}'><span class='glyphicon glyphicon-trash'></span></button>`;
                    },
                    "alerta": function (column, row) {
                        return row.banco == "---" ? "<button type='button' class='btn btn-warning alerta' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-alert'></span> " +
                            " </button>" : "";
                        ;
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblAplicados.find(".downloadAplicado").on("click", function (e) {
                    var idRegistros = $(this).attr('data-id');
                    getDocumentoAplicado(idRegistros);

                });
                tblAplicados.find(".print").on("click", function (e) {
                    let data = $(this).data(),
                        row = $(this).closest('tr');
                    $.ajax({
                        url: '/Administrativo/Reportes/SetDataPrint',
                        type: 'POST',
                        dataType: 'json',
                        data: {
                            id: data.id,
                            obj: {
                                idProveedor: row.find('td').eq(0).text(),
                                total: unmaskDinero(row.find('td').eq(5).text()),
                                fecha: row.find('td').eq(7).text()
                            }
                        },
                        success: function (response) {
                            $.unblockUI();
                            verReporte();
                        },
                        error: function (response) {
                            $.unblockUI();
                            AlertaGeneral("Alerta", response.message);
                        }
                    });

                });
                tblAplicados.find(".eliminar").on("click", function (e) {
                    hdnIdEliminar.val($(this).data().id);
                    hdnIdEliminar.data().estatus = $(this).data().estatus;
                    modalEliminar.modal("show");
                });
                tblAplicados.find(".excel").on("click", function (e) {
                    row = $(this).closest('tr');
                    getExcel($(this).attr('data-id'), {
                        idProveedor: row.find('td').eq(0).text(),
                        total: unmaskDinero(row.find('td').eq(5).text()),
                        fecha: row.find('td').eq(7).text()
                    });
                });
                tblAplicados.find(".alerta").on("click", function (e) {
                    var idRegistros = $(this).attr('data-id');
                    $.ajax({
                        url: '/Administrativo/Reportes/SetBancoDefault',
                        type: 'POST',
                        dataType: 'json',
                        data: { id: idRegistros },
                        success: function (response) {
                            $.unblockUI();
                            if (response.success) {
                                LoadInfo();
                                AlertaGeneral("Alerta", "El registro: " + idRegistros + " se a actualizado con éxito.");
                            }
                        },
                        error: function (response) {
                            $.unblockUI();
                            AlertaGeneral("Alerta", response.message);
                        }
                    });

                });
            });

            btnEnvioCorreos.click(enviarCorreos);
        }

        function enviarCorreos() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Administrativo/Reportes/enviarCorreosGerardo',
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        AlertaGeneral('Confirmación', 'Se Enviaron los correos');
                    } else {
                        AlertaGeneral('Alerta', 'No hay factura existente');
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function unmaskDinero(dinero) {
            return Number(dinero.replace(/[^0-9\.]+/g, ""));
        }
        function validateDescarga() {
            var state = true;
            if (cboFactoraje.val() == 'V') {
                if (!validarCampo(cboBancos)) { state = false; }
            }
            if (!validarCampo(tbFechaEmision)) { state = false; }
            if (!validarCampo(tbFechaVencimiento)) { state = false; }
            return state;
        }

        function darVoBo(cadenaID) {
            if (cadenaID == null) {
                AlertaGeneral(`Aviso`, `No se pudo definir la cadena por dar VoBo.`);
                return;
            }
            $.post('/Administrativo/Reportes/AsignarVoBoCadena', { cadenaID })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Operación exitosa.`);
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

        function getDocumento(id, Factoraje, FechaEmision, FechaVencimiento, IF, Banco) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Administrativo/Reportes/SetDataDownload',
                type: 'POST',
                dataType: 'json',
                data: { id: id, Factoraje: Factoraje, FechaEmision: FechaEmision, FechaVencimiento: FechaVencimiento, IF: IF, Banco: Banco },
                success: function (response) {
                    if (response.success) {
                        tblVencimiento.bootgrid("clear");
                        tblAplicados.bootgrid("clear");
                        LoadInfo();
                        cboFactoraje.val("V");
                        cboFactoraje.change();
                        window.location = '/Administrativo/Reportes/getFileDownload';
                    } else {
                        AlertaGeneral('Alerta', 'No hay factura existente');
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function getDocumentoAplicado(id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Administrativo/Reportes/SetDataDownloadAplicado',
                type: 'POST',
                dataType: 'json',
                data: { id: id },
                success: function (response) {
                    cboFactoraje.val("V");
                    cboFactoraje.change();
                    window.location = '/Administrativo/Reportes/getFileDownloadAplicado';
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function getExcel(id, obj) {
            download(`/Administrativo/Reportes/getExcelDownload/?id=${id}&prov=${obj.idProveedor}&monto=${obj.total}&fecha=${obj.fecha}`)
        }

        function download(url) {
            $.blockUI({ message: "Preparando archivo a descargar" });
            var link = document.createElement("button");
            link.download = url;
            link.href = url;
            $(link).unbind("click");
            location.href = url;
            $.unblockUI();
        }

        function verReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = "24";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        init();
    };

    $(document).ready(function () { Administrativo.Contabilidad.Reportes.CadenaProductiva = new CadenaProductiva(); })
        .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(function () { $.unblockUI(); });
})();