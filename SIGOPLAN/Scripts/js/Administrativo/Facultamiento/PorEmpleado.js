(() => {
    $.namespace('administrativo.facultamiento.PorEmpleado');

    PorEmpleado = function () {

        // Variables principales
        let dataTableCatalogo;
        const tablaFacultamientos = $("#tablaFacultamientos")
        const inputEmpleado = $('#inputEmpleado');
        const comboCC = $('#comboCC');
        const botonBuscar = $('#botonBuscar');
        const botonLimpiar = $('#botonLimpiar');
        const reporte = $("#report");
        const botonReporteGeneral = $('#botonReporteGeneral');

        function init() {
            inputEmpleado
                .getAutocompleteValid(setClaveEmpledo, verificarClaveEmpleado, null, '/OT/getEmpleados');
            comboCC.fillCombo('/Administrativo/Facultamientos/LlenarComboObras', null, false, null);

            agregarListeners();
        }

        function agregarListeners() {

            botonBuscar.click(() => {
                const claveEmpleado = inputEmpleado.data().claveEmpleado;
                const obraID = (comboCC.val() != "") ? comboCC.val() : 0;
                if (claveEmpleado == null) {
                    AlertaGeneral("Aviso", "Debe ingresar el nombre de un empleado primero.");
                } else {
                    obtenerFacultamientos(claveEmpleado, obraID);
                }
            });

            botonLimpiar.click(() => {
                inputEmpleado.val('');
                inputEmpleado.data().claveEmpleado = null;
                comboCC[0].selectedIndex = 0;
                if (dataTableCatalogo != null) {
                    dataTableCatalogo.clear().draw();
                }
            });

            botonReporteGeneral.click(() => {
                const claveEmpleado = inputEmpleado.data().claveEmpleado;
                if (claveEmpleado == null) {
                    AlertaGeneral("Aviso", "Debe ingresar el nombre de un empleado primero.");
                } else {
                    verReporte(true, claveEmpleado);
                }
            });
        }

        function cargarFacultamientos(data, claveEmpleado) {
            if (dataTableCatalogo != null) {
                dataTableCatalogo.destroy();
            }
            dataTableCatalogo = tablaFacultamientos.DataTable({
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
                        data: 'Titulo'
                    },
                    {
                        data: 'Puesto'
                    },
                    {
                        data: 'TipoAutorizacion'
                    },
                    {
                        data: 'Fecha'
                    },
                    {
                        data: 'Estatus'
                    },
                    {
                        data: 'FacultamientoID',
                        createdCell: function (td, cellData, rowData, row, col) {
                            const divFlex = $('<div class="flexContainer"></div>');
                            const botonReporte = $('<button class="btn btn-primary btnReporte"><i class="fa fa-book"></i> Reporte</button>');
                            $(td).html(divFlex);
                            $(divFlex).append(botonReporte);
                        }
                    }
                ],
                drawCallback: function () {
                    $('#tablaFacultamientos .btnReporte').unbind().click(function () {
                        const rowData = dataTableCatalogo.row($(this).parents('tr')).data();
                        verReporte(false, claveEmpleado, rowData.FacultamientoID);
                    });
                }
            });
        }

        function verReporte(esGeneral, claveEmpleado, facultamientoID) {
            $.blockUI({
                message: 'Cargando reporte...'
            });
            if (esGeneral) {
                reporte.attr("src", `/Reportes/Vista.aspx?idReporte=100&id=${claveEmpleado}&inMemory=${1}`);
            } else {
                reporte.attr("src", `/Reportes/Vista.aspx?idReporte=101&id=${claveEmpleado}&inMemory=${1}&facultamientoID=${facultamientoID}`);
            }
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI();
            };
        }

        function obtenerFacultamientos(claveEmpleado, ccID) {
            $.blockUI({
                message: 'Procesando...'
            });
            $.get('/Administrativo/Facultamientos/ObtenerFacultamientosEmpleado', {
                    claveEmpleado: claveEmpleado,
                    centroCostosID: ccID
                })
                .done(respuesta => {
                    if (respuesta.success) {
                        cargarFacultamientos(respuesta.listaFacultamientos, claveEmpleado);
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
                })
                .always(() => {
                    $.unblockUI();
                });
        }

        function setClaveEmpledo(e, ul) {
            $(this).data().claveEmpleado = ul.item.id;
        }

        function verificarClaveEmpleado(e, ul) {
            if (ul.item == null) {
                $(this).val('');
                $(this).data().claveEmpleado = null;
            }
        }
        init();
    }
    $(document).ready(() => administrativo.facultamiento.PorEmpleado = new PorEmpleado());
})();