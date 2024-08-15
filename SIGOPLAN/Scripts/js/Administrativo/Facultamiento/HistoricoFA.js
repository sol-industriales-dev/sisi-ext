(() => {
    $.namespace('administrativo.facultamiento.HistoricoFA');
    HistoricoFA = function () {
        // Variables ventana principal
        const tablaPaquetes = $("#tablaPaquetes");
        const reporte = $("#report");
        const comboCC = $("#comboCC");
        const modal = $('#modalAutorizantes');
        let dataTableCatalogo;

        // Variables modal
        const textoModalObra = $('#textoModalObra');
        const textoModalDescripcion = $('#textoModalDescripcion');

        function obtenerPaquetes(ccID) {
            $.get('/Administrativo/Facultamientos/ObtenerHistorico', {
                    ccID: ccID
                })
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
                    AlertaGeneral("Error", "Error: " + error.statusText)
                });
        }

        function obtenerAutorizantes(paqueteID, cc, descripcion) {
            $.get('/Administrativo/Facultamientos/ObtenerAutorizantes', {
                    paqueteID: paqueteID
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        cargarAutorizantes(respuesta.listaAutorizantes, cc, descripcion);
                    } else {
                        AlertaGeneral("Aviso", "Aviso: " + respuesta.error);
                    }
                })
                .fail(error => {
                    AlertaGeneral("Error", "Error: " + error.statusText)
                });
        }

        function cargarAutorizantes(listaAutorizantes, cc, descripcion) {
            modal.modal('show');
            textoModalObra.text(cc);
            textoModalDescripcion.text(descripcion);
            let contador = 1;
            for (const autorizante of listaAutorizantes) {
                const tr =
                    $(`<tr>
                    <td>${autorizante.Nombre}</td>
                    <td>${(autorizante.Autorizado) ?'Autorizado':'Pendiente'}</td>
                    <td>${(autorizante.EsAutorizante) ?'Autorizante':'VoBo '+contador++}</td>
                    </tr>`);
                $('#tablaAutorizantes tbody').append(tr);
            }
        }

        function agregarListeners() {
            modal.on("hide.bs.modal", () => {
                modal.find('tbody tr').remove();
                textoModalObra.text('');
                textoModalDescripcion.text('');
            });

            comboCC.change(function () {
                if ($(this).val() !== "") {
                    obtenerPaquetes(comboCC.val());
                } else {
                    obtenerPaquetes(0);
                }
            });

        }

        function verReporte(paqueteID) {
            $.blockUI({
                message: 'Cargando reporte...'
            });
            reporte.attr("src", `/Reportes/Vista.aspx?idReporte=99&id=${paqueteID}&inMemory=${1}&isCRModal=${true}`);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI();
            };
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
                        data: 'Estatus'
                    },
                    {
                        data: 'Version'
                    },
                    {
                        data: 'Departamento'
                    },
                    {
                        data: 'ID',
                        createdCell: function (td, cellData, rowData, row, col) {
                            const divFlex = $('<div class="flexContainer"></div>');
                            const botonReporte = $('<button class="btn btn-primary btnReporte"><i class="fa fa-book"></i> Reporte</button>');
                            const botonAutorizaciones = $('<button class="btn btn-default btnAut"><i class="fa fa-user"></i> Autorizantes</button>');
                            $(td).html(divFlex);
                            $(divFlex).append(botonReporte);
                            $(divFlex).append(botonAutorizaciones);
                        }
                    }
                ],
                drawCallback: function () {
                    $('#tablaPaquetes .btnAut').unbind().click(function () {
                        obtenerAutorizantes(dataTableCatalogo.row($(this).parents('tr')).data().ID,
                            dataTableCatalogo.row($(this).parents('tr')).data().CentroCostos,
                            dataTableCatalogo.row($(this).parents('tr')).data().Descripcion);
                    });
                    $('#tablaPaquetes .btnReporte').unbind().click(function () {
                        verReporte(dataTableCatalogo.row($(this).parents('tr')).data().ID);
                    });
                }
            });
        }

        function init() {
            obtenerPaquetes(0);
            comboCC.fillCombo('/Administrativo/Facultamientos/LlenarComboObras', null, false, null);
            agregarListeners();
        }

        init();
    }
    $(document).ready(() => administrativo.facultamiento.HistoricoFA = new HistoricoFA())
        .ajaxStart((() => $.blockUI({
            message: 'Procesando...'
        })))
        .ajaxStop(() => $.unblockUI());
})();