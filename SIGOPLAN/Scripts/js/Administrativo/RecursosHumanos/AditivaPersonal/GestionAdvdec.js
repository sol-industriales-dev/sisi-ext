$(function () {
    $.namespace('recursoshumanos.AditivaPersonal.GestionAdvdec');
    GestionAdvdec = function () {
        const mensajes = { PROCESANDO: 'Procesando...' };
        var _Eliminar = 0;
        const ireport = $("#report");
        const NumFolio = $("#NumFolio");
        const btnEliminar = $("#btnEliminar");
        const txtCCFiltro = $("#txtCCFiltro");
        const modalRechazo = $("#modalRechazo");
        const tblAdvPers = $("#tblAditivaDeduc");
        const btnRechazoSave = $("#btnRechazoSave");
        const tblModalApro = $("#tableAprobaciones");
        const modalaProbado = $("#modalAprobadores");
        const btnAplicarFiltros = $("#btnAplicarFiltros");
        function init() {
            initTabla();
            GetTblAdvPer();
            btnEliminar.click(fnEliminarFormato);
            btnAplicarFiltros.click(FiltrarTablaPendientes);
            txtCCFiltro.keypress(function (e) {
                if (e.which == 13) {
                    getInfoEconomico();
                }
                else { txtNombreCC.value = "" }
            });
            txtCCFiltro.keydown(function () {
                if (txtCCFiltro.value == undefined) {
                    txtNombreCC.value = ""
                }
            });
            btnRechazoSave.click(rechazar);
        }
        function initTabla() {
            tblAdvPers.bootgrid({
                align: 'center',
                selection: true,
                labels:
                {
                    infos: '{{ctx.total}} Aditivas-Deductivas'
                },
                templates: {
                    search: ""
                },
                formatters: {
                    "btn-detalle": function (column, row) {
                        if (row.link != '' && row.link != null) {
                            return `<button type='button' class='btn btn-primary verDet' data-id='${row.id}'><span class=' glyphicon glyphicon-list' style='margin-rigth:2px;'></span> Detalle </button><button type='button' class='btn btn-primary  verRep' data-id='${row.id}'><span class='glyphicon glyphicon-print'></span> Reporte </button><button data-id='${row.id}' onclick='fnDescargarEvidencia(${row.id})' class='btn btn-primary'>Evidencia</button>`;
                        }
                        else {
                            return `<button type='button' class='btn btn-primary verDet' data-id='${row.id}'><span class=' glyphicon glyphicon-list' style='margin-rigth:2px;'></span> Detalle </button><button type='button' class='btn btn-primary  verRep' data-id='${row.id}'><span class='glyphicon glyphicon-print'></span> Reporte </button>`;
                        }

                    },
                    "btn-editar": function (column, row) {
                        if (row.editable) {
                            return `<a class='btn btn-primary edit' data-toggle='collapse' data-id='${row.id}'>Editar</a>`;
                        } else { return ""; }

                    },
                    "btn-eliminar": function (column, row) {
                        if (row.editable) {
                            return `<button type='button' class='btn btn-primary  btnRemove' data-id='${row.id}'><span class='glyphicon glyphicon-trash'></span> Eliminar </button>`;
                        } else { return ""; }

                    },
                    "id_rechazado": function (column, row) {
                        return `<button type='button' class='btn btn-primary  btnRemove' data-id='${row.id}'><span class='glyphicon glyphicon-trash'></span> Eliminar </button>`;
                    },
                    "aprobado": function (column, row) {
                        var estado = "";
                        if (row.aprobado) {
                            estado = `<label data-id='${row.id}'>Aprobado</label>`;
                        } else if (row.rechazado) {
                            estado = `<label data-id='${row.id}'>Rechazado</label>`;
                        } else {
                            estado = `<label data-id='${row.id}'>Pendiente</label>`;
                        }
                        return estado
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblAdvPers.find(".verDet").on("click", function (e) {
                    loadTabla({ id: $(this).attr("data-id") }, '/Administrativo/AditivaPersonal/GetAutorizacion');
                });
                tblAdvPers.find(".verRep").on("click", function (e) {
                    verReporte(e);
                });
                tblAdvPers.find(".btnRemove").on("click", function (e) {
                    var formatoid = $(this).attr("data-id");
                    fnEliminar(formatoid);
                });
                tblAdvPers.find(".edit").on("click", function (e) {
                    var formatoid = $(this).attr("data-id");
                    EnvioEdit(formatoid);
                });
            });
        }

        function EnvioEdit(id) {
            if (id != undefined && id != null) {
                window.location = `/Administrativo/AditivaPersonal/AltaAditiva?id=${id}`;
            }
        }
        function getInfoEconomico() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Horometros/getCentroCostos",
                data: { obj: txtCCFiltro.val() },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        txtNombreCC.value = response.centroCostos;
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function initTablaModal() {
            tblModalApro.bootgrid({
                rowCount: -1,
                align: 'center',
                selection: true,
                labels:
                {
                    infos: '{{ctx.total}} Autorizadores'
                },
                templates: {
                    search: ""
                },
                formatters: {
                    "btn-detalle": function (column, row) {
                        return "<button type='button' class='btn btn-primary' data-id='" + row.id + "'>" +
                            "<span class=' glyphicon glyphicon-list' style='margin-rigth:2px;'></span> " + "Detalle" +
                            " </button>" +
                            "<button type='button' class='btn btn-primary print' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-print'></span> " + "Reporte" +
                            " </button>"
                            ;
                    },
                    "btn-editar": function (column, row) {
                        return "<button type='button' class='btn btn-primary print' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-pencil'></span> " + "Editar" +
                            " </button>"
                            ;
                    },
                    "btn-eliminar": function (column, row) {
                        return "<button type='button' class='btn btn-primary print' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-trash'></span> " + "Eliminar" +
                            " </button>"
                            ;
                    },
                    "autorizando": function (column, row) {
                        var estado = "";
                        if (row.rechazado) {
                            estado = `Rechazado`;
                        } else if (row.autorizando) {
                            estado = `Autorizando`;
                        } else {
                            if (!row.estatus) {
                                estado = `Pendiente`;
                            } else {
                                estado = `Autorizado`;
                            };
                        }
                        return `<label data-id='${row.id}'>${estado}</label>`
                    },
                    "firmar": function (column, row) {
                        if (row.autorizando === true && row.estatus !== true) {
                            return `<div  class='text-center'><button  class='btn btn-success btn-xs aprobar' data-formatoid='${row.id_AditivaDeductiva}' value='${row.id}'>Firmar</button> <button  class='btn btn-danger btn-xs rechazar' data-formatoid='${row.id_AditivaDeductiva}' value='${row.id}'>Rechazar</button></div>`;
                        } else {
                            return `<label'>${row.firma}</label>`;
                        }
                    },
                    "fechafirma": function (column, row) {
                        if (row.fechafirma === null) {
                            return `<label data-id='${row.id}'>N/D</label>`;
                        } else {
                            return `<label'>${row.fechafirma}</label>`;
                        }
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblModalApro.find(".aprobar").on("click", function (e) {
                    $.blockUI({ message: "PROCESANDO ESTA OPERACIÓN PUEDE TARDAR UNOS MINUTOS, GENERANDO CORREO..." });
                    var formatoID = $(this).data("formatoid");
                    var autorizaID = $(this).val();
                    loadaprobar(formatoID, autorizaID, 2);
                });
                tblModalApro.find(".rechazar").on("click", function (e) {
                    modalRechazo.data("formatoID", $(this).data("formatoid"))
                    modalRechazo.data("autorizaID", $(this).val())
                    modalRechazo.modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                });

            });
        }
        function rechazar() {
            $.blockUI({ message: "PROCESANDO ESTA OPERACIÓN PUEDE TARDAR UNOS MINUTOS, GENERANDO CORREO..." });
            const comentario = $("#txtAreaNota").val();
            if (comentario === "" || comentario.trim().length < 10) {
                AlertaGeneral("Aviso", "Debe agregar un comentario mayor a 10 caracteres antes de poder rechazar la solicitud.");
                return;
            }
            modalRechazo.modal('hide');
            var formatoID = modalRechazo.data("formatoID");
            var autorizaID = modalRechazo.data("autorizaID");
            loadrechazar(formatoID, autorizaID, 3, comentario);
        }
        function verReporte(e) {
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = "62";
            var path = `/Reportes/Vista.aspx?idReporte=${idReporte}&fId=${$(e.target).attr('data-id')}`;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
            e.preventDefault();
        }
        function fnEliminar(id) {
            _Eliminar = id;
            $("#modalEliminar").modal("show");
        }
        function fnEliminarFormato() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/AditivaPersonal/eliminarFormato',
                data: { formatoID: _Eliminar },
                success: function (response) {
                    $("#modalEliminar .close").click();
                    GetTblAdvPer();
                    $.unblockUI();
                    AlertaGeneral("Confirmación", "¡El formato fue eliminado correctamente!")
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function loadaprobar(formatoID, autorizaID, estatus) {
            $("#modalAprobadores").modal('hide');
            $.ajax({
                datatype: "json",
                type: "POST",
                url: " /Administrativo/AditivaPersonal/AutorizarFormato",
                data: { plantillaID: formatoID, autorizacion: autorizaID, estatus: estatus },
                success: function (response) {
                    if (response.success) {
                        var mensajeError = response.message;
                        var idReporte = "62";
                        var path = `/Reportes/Vista.aspx?idReporte=${idReporte}&fId=${formatoID}&inMemory=1`;
                        ireport.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.ajax({
                                datatype: "json",
                                type: "POST",
                                url: " /Administrativo/AditivaPersonal/EnviarCorreoFormato",
                                data: { plantillaID: formatoID, autorizacion: autorizaID, estatus: estatus },
                                success: function (response) {
                                    $.unblockUI();
                                    ConfirmacionGeneralAccion("Confirmación", "El formato de Aditiva/Deductiva fue autorizado correctamente" + (mensajeError != null && mensajeError != undefined && mensajeError != '' ? ': ' + mensajeError : ''));
                                },
                                error: function () {
                                    $.unblockUI();
                                }
                            });
                        };
                    } else {
                        AlertaGeneral("Alerta", "Error al autorizar la solicitud.");
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function loadrechazar(formatoID, autorizaID, estatus, comentario) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: " /Administrativo/AditivaPersonal/AutorizarFormato",
                data: { plantillaID: formatoID, autorizacion: autorizaID, estatus: estatus, comentario: comentario },
                success: function (response) {
                    if (response.success) {
                        $("#modalEliminar .close").click();
                        var idReporte = "62";
                        var path = `/Reportes/Vista.aspx?idReporte=${idReporte}&fId=${formatoID}&inMemory=1`;
                        ireport.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.ajax({
                                datatype: "json",
                                type: "POST",
                                url: " /Administrativo/AditivaPersonal/EnviarCorreoFormato",
                                data: { plantillaID: formatoID, autorizacion: autorizaID, estatus: estatus },
                                success: function (response) {
                                    $.unblockUI();
                                    ConfirmacionGeneralAccion("Confirmación", "Ha rechazado la solicitud correctamente.");
                                },
                                error: function () {
                                    $.unblockUI();
                                }
                            });
                        };
                        modalRechazo.data("id", null);
                    } else {
                        $.unblockUI();
                        $("#modalEliminar .close").click();
                        modalRechazo.data("id", null);
                        AlertaGeneral("Alerta", "Error al rechazar la solicitud.")
                    }
                },
                error: function () {
                    $.unblockUI();
                    $("#modalEliminar .close").click();
                    modalRechazo.data("id", null);
                    AlertaGeneral("Alerta", "Error al rechazar la solicitud.")
                }
            });
        }
        function FiltrarTablaPendientes() {
            var cc = 0;
            var folio = 0;
            var estado = $("#chgEstado").val();
            if (txtCCFiltro.val() != "") {
                cc = txtCCFiltro.val()
            }
            if (NumFolio.val() != "") {
                folio = NumFolio.val();
            }
            else {
                folio = 0;
            }
            $.ajax({
                url: '/Administrativo/AditivaPersonal/TablaFormatosPendientes/',
                datatype: "html",
                type: 'POST',
                data: { CC: cc, folio: folio, id: id, estado: estado },
                success: function (response) {
                    initTabla()
                    tblAdvPers.bootgrid("clear");
                    tblAdvPers.bootgrid("append", response);
                },
                error: function (response) {
                    $.unblockUI();
                    alert(response.message);
                }
            });
        }
        function loadTabla(obj, controller) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: controller,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                async: false,
                success: function (response) {
                    initTablaModal();
                    tblModalApro.bootgrid("clear");
                    tblModalApro.bootgrid("append", response.items);
                    $("#capUser").html(response.CAPTURA);
                    if (response.comentario && response.comentario.length > 0) {
                        $('#motivoRechazo').html(`Motivo de rechazo: <strong>${response.comentario}</strong>`);
                    } else {
                        $('#motivoRechazo').html(``);
                    }
                    modalaProbado.modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                }
            });

        }
        function GetTblAdvPer() {
            $.blockUI({ message: mensajes.PROCESANDO });
            id = $.urlParam('obj');
            if (id == null) {
                id = 0
            }
            if (id != 0) {
                $.ajax({
                    url: "/Administrativo/AditivaPersonal/GetTblAdvPer",
                    type: 'POST',
                    data: { id: id },
                    success: function (response) {
                        tblAdvPers.bootgrid("clear");
                        tblAdvPers.bootgrid("append", response);
                        $.unblockUI();
                    },
                    error: function (response) {
                        AlertaGeneral("Alerta", response.message);
                        $.unblockUI();
                    }
                });
            }
            else {
                FiltrarTablaPendientes();
            }
        }
        init();
    }
    $(document).ready(function () {
        recursoshumanos.AditivaPersonal.GestionAdvdec = new GestionAdvdec();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
});
function fnDescargarEvidencia(solicitudID) {
    location.href = '/Administrativo/AditivaPersonal/getFileDownload?id=' + solicitudID;
}
