$(function () {
    $.namespace('recursoshumanos.formatocambio.CatFormatoCambio');
    CatFormatoCambio = function () {
        _Eliminar = 0;
        mensajes = {
            PROCESANDO: 'Procesando...',
            ENVIOCORREO: 'Procesando envío de correo favor de esperar. '
        };
        const ireport = $("#report");
        const capUser = $("#capUser");
        const cboTipo = $("#cboTipo");
        const verModal = $("#verModal");
        const tblCambio = $('#tblCambio');
        const txtNumero = $("#txtNumero");
        const verFormato = $("#verFormato");
        const btnEliminar = $("#btnEliminar");
        const txtCCFiltro = $("#txtCCFiltro");
        const NumEmpleado = $("#NumEmpleado");
        const modalRechazo = $("#modalRechazo");
        const modalReportes = $("#modalReportes");
        const btnRechazoSave = $("#btnRechazoSave");
        const btnAplicarFiltros = $("#btnAplicarFiltros");
        const divFormatoPendientes = $("#divFormatoPendientes");
        const modalAutorizantes = $('#modalAutorizantes');
        const tblAutorizantes = $('#tblAutorizantes');
        let dtAutorizantes;

        var objAprobando;
        function init() {
            cboTipo.change(fnChangeTipo);
            btnRechazoSave.click(rechazar);
            btnEliminar.click(fnEliminarFormato);
            btnAplicarFiltros.click(FiltrarTablaPendientes);
            txtCCFiltro.fillCombo('/Administrativo/FormatoCambio/getCboCC', null, false, null);
            initDataTblCambio();
            loadByNotification();
            initTblAutorizantes();
        }
        const enviarCorreos = new URL(window.location.origin + '/Administrativo/FormatoCambio/enviarCorreos');
        const enviarCorreofin = new URL(window.location.origin + '/Administrativo/FormatoCambio/enviarCorreofin');
        const eliminarFormato = new URL(window.location.origin + '/Administrativo/FormatoCambio/eliminarFormato');
        const validReporteExclusion = new URL(window.location.origin + '/Administrativo/FormatoCambio/validReporteExclusion');
        const TablaFormatosPendientes = new URL(window.location.origin + '/Administrativo/FormatoCambio/TablaFormatosPendientes');
        const getPersimoUsuarioGestionFormato = new URL(window.location.origin + '/Administrativo/FormatoCambio/getPersimoUsuarioGestionFormato');
        const getFormatoIDbyNotification = new URL(window.location.origin + '/Administrativo/FormatoCambio/getFormatoIDbyNotification');
        objPanelAuth = {
            idPanelReporte: 11,
            urlLstAuth: `/Administrativo/FormatoCambio/getLstAuth`,
            urlAuth: `/Administrativo/FormatoCambio/Aprobar`,
            urlRech: `/Administrativo/FormatoCambio/Rechazar`,
            callbackAuth: async response => sendCorreoAprobado(response),
            callbackRech: response => sendCorreoRechazo(response)
        }
        async function sendCorreoAprobado(respuesta) {
            try {
                if (respuesta.success) {
                    $.blockUI({ message: mensajes.ENVIOCORREO });
                    let urlCorreo = ``;
                    let objCorreo = {};
                    if (respuesta.AprobadoTotal) {
                        urlCorreo = enviarCorreofin;
                        objCorreo = { CorreoEnviar: respuesta.CorreoEnviar, folio: respuesta.Folio, formatoID: respuesta.items };
                    } else {
                        urlCorreo = enviarCorreos;
                        objCorreo = { usuariorecibe: respuesta.usuarioEnvia, formatoID: respuesta.idFormatoCambios, tipo: "autoriza", orden: 0 };
                    }
                    let response = await ejectFetchJson(urlCorreo, objCorreo);
                    if (response.success) {
                        FiltrarTablaPendientes();
                    }
                }
            } catch (o_O) {
                console.log(o_O.message);
            }
        }
        async function sendCorreoRechazo(respuesta) {
            try {
                if (respuesta.success) {
                    let objCorreo = { usuariorecibe: respuesta.usuarioEnvia, formatoID: respuesta.idFormatoCambios, tipo: "rechazar", orden: 0 };
                    let response = await ejectFetchJson(enviarCorreos, objCorreo);
                    if (response.success) {
                        FiltrarTablaPendientes();
                    }
                }
            } catch (o_O) {
                AlertaGeneral("Alerta", "No se rechazó la solicitud, ha ocurrido un error interno.");
                console.log(o_O.message);
            }
        }
        async function initDataTblCambio() {
            let esTodoFormatoCambio = false;
            let esEliminar = false;
            response = await ejectFetchJson(getPersimoUsuarioGestionFormato);
            if (response.success) {
                esTodoFormatoCambio = response.esTodoFormatoCambio;
                esEliminar = response.esEliminar;
            }
            dtCambio = tblCambio.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { data: 'id', width: "2%" },
                    { data: 'Clave_Empleado', width: "2%" },
                    { data: 'Nombre', width: "20%", render: (data, type, row) => `${data} ${row.Ape_Paterno} ${row.Ape_Materno}` },
                    { data: 'CC' },
                    { data: 'Puesto', width: "23%" },
                    { data: 'descCategoria', title: 'Categoria', render: function (data, type, row) { return data ?? "S/N" } },
                    {
                        data: 'Aprobado', width: "2%", createdCell: (td, data, rowData, row, col) => {
                            let txt;
                            if (rowData.Aprobado) {
                                txt = `Aprobado`
                            }
                            else {
                                if (rowData.Rechazado) {
                                    txt = `Rechazado`
                                }
                                else {
                                    txt = `Pendiente`
                                }
                            }
                            $(td).html(txt);
                        }
                    },
                    {
                        data: 'id', width: "2%", createdCell: (td, data, rowData, row, col) => {
                            if (rowData.esAuditor) {
                                $(td).html(`<button>`);
                                $(td).find(`button`).addClass(`btn btn-info verDetAuditor`);
                                $(td).find(`button`).html(`Autorizantes`);
                                $(td).find(`button`).val(data);
                            } else {
                                $(td).html(`<button>`);
                                $(td).find(`button`).addClass(`btn btn-info verDet`);
                                $(td).find(`button`).html(`Autorizantes`);
                                $(td).find(`button`).val(data);
                            }


                        }
                    },
                    {
                        data: 'id', width: "2%", createdCell: (td, data, rowData, row, col) => {
                            if (rowData.Rechazado) {
                                $(td).html("");
                            } else if (rowData.editable) {
                                $(td).html(`<button>`);
                                $(td).find(`button`).addClass(`btn btn-info edit`);
                                $(td).find(`button`).html(`Editar`);
                                $(td).find(`button`).val(data);
                            }
                            else {
                                $(td).html("");
                            }
                        }
                    },
                    {
                        data: 'id', width: "2%", visible: esEliminar, createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`<button>`);
                            $(td).find(`button`).addClass(`btn btn-info btnRemove`);
                            $(td).find(`button`).html(`Eliminar`);
                            $(td).find(`button`).val(data);
                        }
                    }
                ]
                , initComplete: function (settings, json) {
                    tblCambio.on('click', '.verDet', async function (event) {
                        let formatoID = this.value;
                        setPanelAutorizantes({ id: formatoID });
                    });
                    tblCambio.on('click', '.verDetAuditor', async function (event) {
                        let formatoID = this.value;
                        // setPanelAutorizantes({ id: formatoID });
                        fncGetLstAutorizantes(formatoID);
                        modalAutorizantes.modal("show");
                    });
                    tblCambio.on('click', '.btnRemove', function (event) {
                        var formatoid = +(this.value);
                        fnEliminar(formatoid);
                    });
                    tblCambio.on('click', '.edit', async function (event) {
                        let CapturaFormato = new URL(window.location.origin + `/Administrativo/FormatoCambio/CapturaFormato?id=${this.value}`);
                        location.href = CapturaFormato;
                    });
                }
            });
            FiltrarTablaPendientes();

        }

        function fnEliminar(id) {
            _Eliminar = id;
            $("#modalEliminar").modal("show");
        }
        async function loadByNotification() {
            var autorizaID = $.urlParam('obj');
            if (autorizaID != null) {
                var response = await ejectFetchJson(getFormatoIDbyNotification, { autorizaID: autorizaID });
                if (response.success === true) {
                    setPanelAutorizantes({ id: response.formatoID });
                }
            }
        }
        async function fnEliminarFormato() {
            try {
                response = await ejectFetchJson(eliminarFormato, { formatoID: _Eliminar });
                if (response.success) {
                    $("#modalEliminar .close").click();
                    FiltrarTablaPendientes();
                    AlertaGeneral("Confirmación", "¡El formato fue eliminado correctamente!")
                }
            } catch (o_O) { }
        }
        function fnChangeTipo() {
            var _this = $(this);
            if (_this.val() == "") {
                txtNumero.val(0);
                txtNumero.prop("disabled", true);
            }
            else {
                txtNumero.val(1);
                txtNumero.prop("disabled", false);
            }
        }
        async function FiltrarTablaPendientes() {
            try {
                dtCambio.clear().draw();
                response = await ejectFetchJson(TablaFormatosPendientes, {
                    CC: txtCCFiltro.val()
                    , claveEmpleado: NumEmpleado.val() === "" ? 0 : NumEmpleado.val()
                    , id: 0
                    , estado: $("#chgEstado").val()
                    , tipo: cboTipo.val()
                    , numero: txtNumero.val() === "" ? 0 : txtNumero.val()
                });
                if (response.success) {
                    dtCambio.rows.add(response.lst).draw();
                }
            } catch (o_O) { console.log(o_O.message); }
        }
        function rechazar() {
            const comentario = $("#txtAreaNota").val();
            if (comentario === "" || comentario.trim().length < 10) {
                AlertaGeneral("Aviso", "Debe agregar un comentario mayor a 10 caracteres antes de poder rechazar la solicitud.");
                return;
            }
            modalRechazo.modal('hide');
            rechazarSolicitud(comentario);
        }
        function rechazarSolicitud(comentario) {
            $.post("/Administrativo/FormatoCambio/Rechazar", { objAutorizacion: objAprobando, comentario })
                .done(response => {
                    if (response.success) {
                        $.post('/Administrativo/FormatoCambio/enviarCorreos',
                            {
                                usuariorecibe: response.usuarioEnvia,
                                formatoID: response.idFormatoCambios,
                                tipo: "rechazar",
                                orden: 0
                            })
                            .done(response => {
                                if (response.success) {
                                    AlertaGeneral("Alerta", "Ha rechazado la solicitud correctamente.");
                                } else {
                                    AlertaGeneral("Alerta", "Se ha rechazado la solicitud, pero ocurrió un error al enviar los correos correspondientes.");
                                }
                            })
                            .fail(() => {
                                AlertaGeneral("Alerta", "Se ha rechazado la solicitud, pero ocurrió un error al enviar los correos correspondientes.");
                            });
                    } else {
                        $("#modalAprobadores .close").click();
                        AlertaGeneral("Alerta", "No se rechazó la solicitud, ha ocurrido un error interno.");
                        $.unblockUI();
                    }
                })
                .fail(() => {
                    AlertaGeneral("Alerta", "No se rechazó la solicitud, ha ocurrido un error interno.");
                })
                .always(() => {
                    $("#modalAprobadores .close").click();
                    $.unblockUI();
                    FiltrarTablaPendientes();
                });
        }
        function initTblAutorizantes() {
            dtAutorizantes = tblAutorizantes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { data: 'nombre', title: 'NOMBRE' },
                    { data: 'descripcion', title: 'DESC' },
                    {
                        render: (data, type, row, meta) => {
                            switch (row.authEstado) {
                                case 0:
                                    return "Espera";
                                case 1:
                                    return "Autorizado";
                                case 2:
                                    return "Rechazado";
                                case 3:
                                    return "AutorizanteEnTurno";
                                default:
                                    return "";
                            }
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblAutorizantes.on('click', '.classBtn', function () {
                        let rowData = dtAutorizantes.row($(this).closest('tr')).data();
                    });
                    tblAutorizantes.on('click', '.classBtn', function () {
                        let rowData = dtAutorizantes.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetLstAutorizantes(id) {
            axios.post("/Administrativo/FormatoCambio/getLstAuth", { id }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    dtAutorizantes.clear();
                    dtAutorizantes.rows.add(response.data.autorizantes);
                    dtAutorizantes.draw();
                } else {
                    dtAutorizantes.clear();
                    dtAutorizantes.draw();
                }
            }).catch(error => Alert2Error(error.message));

        }

        function loadTabla(url, obj) {
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                async: false,
                success: function (response) {

                    $("#capUser").html(response.CAPTURA);

                    if (response.comentario && response.comentario.length > 0) {
                        $('#motivoRechazo').html(`Motivo de rechazo: <strong>${response.comentario}</strong>`);
                    } else {
                        $('#motivoRechazo').html(``);
                    }

                    var tabla = "<table class=' table table-condensed table-hover table-striped text-center' id='tblDetalle'>";
                    tabla += "<thead class='bg-table-header'>"
                    tabla += "<tr>";
                    tabla += "<th class='text-center'><label>Participación</label></th>";
                    tabla += "<th class='text-center'><label>Nombre</label></th>";
                    tabla += "<th class='text-center'><label>Puesto</label></th>";
                    tabla += "<th class='text-center'><label>Estatus</label></th>";
                    tabla += "<th class='text-center'><label>Firma</label></th>";
                    tabla += "</tr>";
                    tabla += "</thead>";
                    tabla += "<tbody>";

                    var i = 0;
                    for (i; i <= response.items.length - 1; i++) {

                        tabla += "<tr>";
                        tabla += "<td class='text-center'>" + response.items[i].Responsable + "</td>";
                        tabla += "<td class='text-center'>" + response.items[i].Nombre_Aprobador + "</td>";
                        tabla += "<td class='text-center'>" + response.items[i].PuestoAprobador + "</td>";
                        tabla += "<td class='text-center'>";
                        if (response.items[i].Rechazado == true) {
                            tabla += "<label>Rechazado</label>"
                        }
                        else if (response.items[i].Autorizando == true) {
                            tabla += "<label>Autorizando</label>"
                        }
                        else {
                            if (response.items[i].Estatus == false) {
                                tabla += "<label>Pendiente</label>"
                            }
                            else {
                                tabla += "<label>Autorizado</label>"
                            }
                        }
                        if (response.items[i].Autorizando === true && response.items[i].Estatus !== true) {
                            tabla += "<td class='text-center'><button class='btn btn-success btn-xs aprobar' value='" + response.items[i].id + "'>Firmar</button> <button class='btn btn-danger btn-xs rechazar' value='" + response.items[i].id + "'>Rechazar</button></td>";
                            objAprobando = response.items[i];
                        }
                        else {
                            tabla += "<td class='text-center'>" + response.items[i].Firma + "</td>"
                        }


                        tabla += "</tr>";

                    }


                    tabla += "</tbody>";
                    tabla += "</table>";


                    $("#tableAprobaciones").html(tabla);
                    $("#modalAprobadores").modal('show');

                    $("#tblDetalle").find(".aprobar").on("click", function (e) {
                        $.ajax({
                            datatype: "json",
                            type: "POST",
                            url: "/Administrativo/FormatoCambio/Aprobar",
                            data: { objAutorizacion: objAprobando, id: $.urlParam("obj") },
                            success: function (response) {

                                $.blockUI({ message: mensajes.PROCESANDO });
                                $("#modalAprobadores").modal('hide');
                                var idFormatoCambios = response.idFormatoCambios == undefined ? response.items : response.idFormatoCambios;
                                var usuarioEnvia = response.usuarioEnvia;
                                var idReporte = "11";
                                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + idFormatoCambios + "&inMemory=1";

                                ireport.attr("src", path);
                                if (response.AprobadoTotal) {
                                    usuarioEnvia = response.CorreoEnviar;
                                    folio = response.Folio;
                                    document.getElementById('report').onload = function () {
                                        $.ajax({
                                            datatype: "json",
                                            type: "POST",
                                            url: '/Administrativo/FormatoCambio/enviarCorreofin',
                                            data: { CorreoEnviar: usuarioEnvia, folio: folio, id: response.items, formatoID: idFormatoCambios, orden: 0 },
                                            success: function (response) {
                                                $.unblockUI();

                                                ConfirmacionGeneralAccion("Confirmación", "El formato de cambio fue autorizado correctamente");
                                                //   ConfirmacionGeneralFC("Confirmación", "Fue autorizado correctamente", "bg-green");

                                            },
                                            error: function () {
                                                $.unblockUI();
                                            }
                                        });
                                    };
                                } else {
                                    $.blockUI({ message: mensajes.ENVIOCORREO });
                                    $("#modalAprobadores").modal('hide');
                                    document.getElementById('report').onload = function () {
                                        $.ajax({
                                            datatype: "json",
                                            type: "POST",
                                            url: '/Administrativo/FormatoCambio/enviarCorreos',
                                            data: { usuariorecibe: usuarioEnvia, formatoID: idFormatoCambios, tipo: "autoriza", orden: 0 },
                                            success: function (response) {
                                                ConfirmacionGeneralAccion("Confirmación", "El formato de cambio fue autorizado correctamente");
                                                //   ConfirmacionGeneralFC("Confirmación", "Fue autorizado correctamente", "bg-green");
                                                $.unblockUI();
                                            },
                                            error: function () {
                                                $.unblockUI();
                                            }
                                        });
                                    };
                                }

                            },
                            error: function () {

                            }
                        });

                        FiltrarTablaPendientes();
                    });

                    $("#tblDetalle").find(".rechazar").on("click", function () {
                        modalRechazo.modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                    });

                },
                error: function (response) {
                    $.unblockUI();
                }
            });
        }
        init();
    }
    $(document).ready(function () {
        recursoshumanos.formatocambio.CatFormatoCambio = new CatFormatoCambio();
    });
});