(function () {
    $.namespace('administrativo.facultamiento.AutorizacionFA');
    AutorizacionFA = function () {

        // Variables ventana principal
        fieldsetPaquetes = $("#fieldsetPaquetes");
        fieldsetReporte = $("#fieldsetReporte");
        fieldsetAutorizacion = $("#fieldsetAutorizacion");
        tablaPaquetes = $("#tablaPaquetes");
        legendHome = $("#legendHome");
        report = $("#report");

        // Variables modalRechazo
        const textAreaRechazo = $('#textAreaRechazo');

        let dataTableCatalogo;

        function init() {
            obtenerPaquetesPorAutorizar();
            legendHome.click(ocultarPaneles);
            agregarListeners();
        }

        function agregarListeners() {

            $("#modalRechazar").on("hide.bs.modal", () => {
                $(textAreaRechazo).val("");
            });
        }


        function obtenerPaquetesPorAutorizar() {
            $.get('/Administrativo/Facultamientos/ObtenerPaquetesPorAutorizar')
                .done(respuesta => {
                    if (respuesta.success) {
                        cargarPaquetes(respuesta.listaPaquetesFa);
                    } else {
                        if (respuesta.EMPTY == null) {
                            AlertaGeneral("Aviso", "Aviso: " + respuesta.error);
                        }
                        if (dataTableCatalogo != null) {
                            dataTableCatalogo.clear().draw();
                        }
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText);
                });
        }

        function cargarPaquetes(data) {
            if (dataTableCatalogo != null) {
                dataTableCatalogo.destroy();
            }
            dataTableCatalogo = tablaPaquetes.DataTable({
                language: dtDicEsp,
                destroy: true,
                order: [
                    [2, "desc"]
                ],
                data: data,
                columns: [{
                        data: 'CentroCostos'
                    },
                    {
                        data: 'Descripcion'
                    },
                    {
                        data: 'Fecha'
                    },
                    {
                        data: 'Departamento'
                    },
                    {
                        data: 'ID',
                        createdCell: function (td, cellData, rowData, row, col) {
                            const divFlex = $('<div class="flexContainer"></div>');
                            const botonAutorizar = $('<button class="btn btn-primary btnAutorizar"><i class="fa fa-key"></i> Autorizar</button>');
                            $(td).html(divFlex);
                            $(divFlex).append(botonAutorizar);
                        }
                    }
                ],
                drawCallback: function () {
                    $('#tablaPaquetes .btnAutorizar').unbind().click(function () {
                        obtenerListaAutorizantes(dataTableCatalogo.row($(this).parents('tr')).data().ID);
                    });
                }
            });
        }

        function obtenerListaAutorizantes(paqueteID) {
            $.get("/Administrativo/facultamientos/ObtenerAutorizantes", {
                    paqueteID: paqueteID
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        verReporte(paqueteID, true);
                        mostrarPaneles(paqueteID);
                        establecerPaneles(respuesta.listaAutorizantes);
                    } else {
                        AlertaGeneral("Error", "Error: " + respuesta.error)
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                });
        }

        function verReporte(paqueteID, isReporte, ordenVoBo, autCompleta, comentario) {
            // CRModal = Crystal Reports Modal.
            $.blockUI({
                message: 'Cargando reporte...'
            });
            report.attr("src", `/Reportes/Vista.aspx?idReporte=99&id=${paqueteID}&inMemory=${1}&isCRModal=${false}`);
            document.getElementById('report').onload = function () {
                if (!isReporte) {
                    if (comentario) {
                        enviarCorreoRechazo(paqueteID, comentario);
                    } else if (autCompleta) {
                        enviarCorreoAutCompleta(paqueteID);
                    } else {
                        enviarCorreoAut(paqueteID, ordenVoBo);
                    }
                } else {
                    $.unblockUI();
                }
            };
        }

        function enviarCorreoRechazo(paqueteID, comentario) {
            $.post('/Administrativo/Facultamientos/EnviarCorreoRechazo', {
                    paqueteID: paqueteID,
                    comentario: comentario
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        AlertaGeneral("Éxito", "Operación Exitosa.");
                    } else {
                        AlertaGeneral("Error", `Él rechazo fue exitoso pero
                        no se pudo enviar correo de justificación: ${respuesta.error}`);
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText);
                });
        }

        function enviarCorreoAut(paqueteID, ordenVoBo) {
            $.post('/Administrativo/Facultamientos/EnviarCorreoAutorizacion', {
                    paqueteID: paqueteID,
                    ordenVoBo: ordenVoBo
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        AlertaGeneral("Éxito", "Operación Exitosa.");
                    } else {
                        AlertaGeneral("Error", `La autorización fue exitosa pero
                        no se pudo enviar correo al siguiente autorizante: ${respuesta.error}`);
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText);
                });
        }

        function enviarCorreoAutCompleta(paqueteID) {
            $.post('/Administrativo/Facultamientos/EnviarCorreoAutorizacionCompleta', {
                    paqueteID: paqueteID
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        AlertaGeneral("Éxito", "Operación Exitosa.");
                    } else {
                        AlertaGeneral("Error", `La autorización fue exitosa pero no ocurrió un error
                        al intentar enviar un correo a todos los autorizantes: ${respuesta.error}`);
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText);
                });
        }

        function ocultarPaneles() {
            fieldsetPaquetes.prop("hidden", false);
            fieldsetAutorizacion.prop("hidden", true);
            fieldsetAutorizacion.data().paqueteID = null;
            fieldsetReporte.prop("hidden", true);
            $('#fieldsetAutorizacion > div').remove();
        }

        function mostrarPaneles(paqueteID) {
            fieldsetPaquetes.prop('hidden', true);
            fieldsetReporte.prop('hidden', false);
            fieldsetAutorizacion.prop('hidden', false);
            fieldsetAutorizacion.data().paqueteID = paqueteID;
        }

        function establecerPaneles(listaAutorizantes) {

            const divFlex = $('<div class="flex-container"></div>');

            listaAutorizantes.forEach(autorizante => {
                const divPanel = $(`<div class="panel panel-default text-center flex-item"></div>`);

                const divPanelHeading =
                    $(`<div class="panel-heading"><label>${autorizante.Nombre}</label></div>`);

                const divPanelBody = $(`<div class="panel-body"><p>${autorizante.Orden == 1 ? "Autorizante" : "VoBo " + (autorizante.Orden-1)}</p></div>`);

                const divPanelFooter = $(`<div class="panel-footer ${autorizante.Autorizado ? "panelAutorizado" : "panelPendiente" }"></div>`);

                const label = $(`<p>${autorizante.Autorizado ?"Autorizado" :"Pendiente" }</p>`);
                divPanelFooter.append(label);

                divPanel.append(divPanelHeading).append(divPanelBody).append(divPanelFooter);

                if (autorizante.UsuarioID > 0) {
                    const botonAutorizar = $("<button id='botonAutorizar' class='btn btn-success btnPanel'><i class='fa fa-check'></i> Autorizar</button>");
                    const botonRechazar = $("<button id='botonRechazar' class='btn btn-danger btnPanel'><i class='fa fa-ban'></i> Rechazar</button>");
                    divPanel.data().usuarioID = autorizante.UsuarioID;
                    divPanelBody.append(botonAutorizar).append(botonRechazar);
                }
                divFlex.append(divPanel);
            });
            fieldsetAutorizacion.append(divFlex);

            $('#botonAutorizar').on('click', x => mostrarModalAutorizar());

            $('#botonRechazar').on('click', x => mostrarModalRechazar());
        }

        function mostrarModalRechazar() {
            $('#modalRechazar').modal('show');
            $(textAreaRechazo)
                .on('change', () => $(textAreaRechazo).val(sanitizeString($(textAreaRechazo).val())));
            $("#modalAutorizar-btn-Rechazar").unbind().click(() => rechazarPaquete());
        }

        function mostrarModalAutorizar() {
            $('#modalAutorizar').modal('show');
            $("#modalAutorizar-btn-Autorizar").unbind().click(() => autorizarPaquete());
        }

        function autorizarPaquete() {
            const paqueteID = fieldsetAutorizacion.data().paqueteID;
            if (paqueteID) {
                $('#modalAutorizar').modal('hide');
                $.post('/Administrativo/Facultamientos/AutorizarPaquete', {
                        paqueteID: paqueteID
                    })
                    .done(respuesta => {
                        if (respuesta.success) {
                            if (respuesta.autCompleta) {
                                verReporte(paqueteID, false, null, true);
                            } else if (respuesta.ordenVoBo && respuesta.ordenVoBo > 0) {
                                verReporte(paqueteID, false, respuesta.ordenVoBo, false);
                            }
                            ocultarPaneles();
                            obtenerPaquetesPorAutorizar();
                        } else {
                            AlertaGeneral("Error", "Error: " + respuesta.error);
                        }
                    })
                    .fail(error => {
                        AlertaGeneral("Error", "Error: " + error.statusText);
                    });
            }
        }

        function sanitizeString(str) {
            str = str.replace(/[^a-z0-9áéíóúñü \.,]/gim, "");
            return str.trim();
        }

        function rechazarPaquete() {
            const paqueteID = fieldsetAutorizacion.data().paqueteID;
            const comentario = $(textAreaRechazo).val().trim();
            if (comentario == "" || comentario.length <= 19) {
                AlertaGeneral("Aviso", "El mensaje de rechazo debe tener un mínimo de 20 caracteres.")
                return;
            }
            if (paqueteID) {
                $('#modalRechazar').modal('hide');
                $.post('/Administrativo/Facultamientos/RechazarPaquete', {
                        paqueteID: paqueteID,
                        comentario: comentario
                    })
                    .done(respuesta => {
                        if (respuesta.success) {
                            verReporte(paqueteID, false, 0, false, comentario);
                            ocultarPaneles();
                            obtenerPaquetesPorAutorizar();
                        } else {
                            AlertaGeneral("Error", "Error: " + respuesta.error);
                        }
                    })
                    .fail(error => {
                        AlertaGeneral("Error", "Error: " + error.statusText);
                    });
            }
        }

        init();
    }
    $(document).ready(() => administrativo.facultamiento.AutorizacionFA = new AutorizacionFA())
        .ajaxStart((() => $.blockUI({
            message: 'Procesando...'
        })))
        .ajaxStop(() => $.unblockUI());
})();