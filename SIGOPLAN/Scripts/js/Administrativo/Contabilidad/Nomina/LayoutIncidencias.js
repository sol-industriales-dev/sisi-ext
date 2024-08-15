(() => {
    $.namespace('Nomina.LayoutIncidencias');
    LayoutIncidencias = function () {
        //#region Selectores
        const selectPeriodo = $('#selectPeriodo');
        const selectEstatus = $('#selectEstatus');
        const botonBuscar = $('#botonBuscar');
        const tablaIncidencias = $('#tablaIncidencias');
        const botonExportarExcel = $('#botonExportarExcel');
        const botonExportarReporte = $('#botonExportarReporte');
        const botonCorreo = $('#botonCorreo');
        const cboTipoNomina = $('#cboTipoNomina');
        //#endregion

        let dtIncidencias;
        
        (function init() {
            initTablaIncidencias();
            cboTipoNomina.select2();
            selectPeriodo.fillComboGroup('/Nomina/getCbotPeriodoNomina', { tipoNomina: cboTipoNomina.val() }, false, null, null);

            botonBuscar.click(cargarIncidencias);
            botonExportarExcel.click(descargarExcelLayoutIncidencias);
            botonExportarReporte.click(descargarExcelReporteIncidencias);
            botonCorreo.click(() => {
                AlertaAceptarRechazarNormal('Confirmar Correo', `¿Está seguro de enviar el correo con el layout de incidencias?`, () => { enviarCorreo() });
            })
            cboTipoNomina.change(function (e)
            {
                selectPeriodo.fillComboGroup('/Nomina/getCbotPeriodoNomina', { tipoNomina: cboTipoNomina.val() }, false, undefined, function () { selectPeriodo.change(); });
            });
        })();

        function cargarIncidencias() {
            const dataPeriodo = selectPeriodo.find('option:selected').data('prefijo').split('-');
            const tipo_nomina = dataPeriodo[2];
            const periodo = +selectPeriodo.val();
            const anio = dataPeriodo[3];
            const estatus = selectEstatus.find('option:selected').attr('estatus');

            axios.post('/Administrativo/Nomina/CargarLayoutIncidencias', { anio, periodo, tipo_nomina, estatus })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaIncidencias, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initTablaIncidencias() {
            dtIncidencias = tablaIncidencias.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                ordering: false,
                scrollY: '45vh',
                scrollX: true,
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'clave_empleado', title: '# Empleado' },
                    { data: 'nombre_empleado', title: 'Nombre' },
                    { data: 'ccDesc', title: 'Centro de Costo' },
                    { data: 'total_Dias', title: 'Días' },
                    { data: 'horas_extras', title: 'Horas Extra' },
                    {
                        data: 'bonoTotal', title: 'Bono', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return maskNumero2DCompras(data);
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function descargarExcelLayoutIncidencias() {
            location.href = `/Administrativo/Nomina/DescargarExcelLayoutIncidencias`;
        }

        function descargarExcelReporteIncidencias() {
            const dataPeriodo = selectPeriodo.find('option:selected').data('prefijo').split('-');
            const tipo_nomina = +dataPeriodo[2];
            const periodo = +selectPeriodo.val();
            const anio = +dataPeriodo[3];
            const estatus = selectEstatus.find('option:selected').attr('estatus') == 'A';
            location.href = `/Administrativo/Nomina/DescargarExcelReporteIncidencias?anio=${anio}&tipo_nomina=${tipo_nomina}&periodo=${periodo}&autorizado=${estatus}`;
        }

        function enviarCorreo() {
            axios.post('/Administrativo/Nomina/EnviarCorreoLayoutIncidencias')
                .then(response => {
                    let { success, datos, message } = response.data;

                    Alert2Exito('Se ha enviado el correo.');
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Nomina.LayoutIncidencias = new LayoutIncidencias())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();