(() => {
    $.namespace('Maquinaria.Captura.Diaria.ReporteEconomicosSinHorometros');

    ReporteEconomicosSinHorometros = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const selectEconomico = $('#selectEconomico');
        const inputFechaInicio = $('#inputFechaInicio');
        const botonBuscar = $('#botonBuscar');
        const botonImprimir = $('#botonImprimir');
        const tablaResultados = $('#tablaResultados');

        const report = $('#report');
        //#endregion

        let dtResultados;

        (function init() {
            selectCentroCosto.fillCombo('/CatInventario/FillComboCC', { est: true }, false);
            selectEconomico.fillCombo('/Horometros/cboModalEconomico', { obj: selectCentroCosto.val() == "" ? 0 : selectCentroCosto.val() });

            inputFechaInicio.datepicker().datepicker("setDate", new Date(new Date().getFullYear(), new Date().getMonth(), 1));

            initTablaResultados();

            botonBuscar.click(cargarEconomicosSinHorometros);
            botonImprimir.click(cargarReporte);
        })();

        function initTablaResultados() {
            dtResultados = tablaResultados.DataTable({
                retrieve: true,
                language: dtDicEsp,
                dom: 'tp',
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'ccDesc', title: 'CC' },
                    { data: 'economico', title: 'Económico' },
                    { data: 'horometroAcumuladoDesc', title: 'Horómetro Acumulado' },
                    { data: 'fechaString', title: 'Fecha' },
                    { data: 'diasTranscurridos', title: 'Días Transcurridos' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '30%', targets: [0] }
                ]
            });
        }

        function cargarEconomicosSinHorometros() {
            let areaCuenta = selectCentroCosto.val();
            let economico = selectEconomico.val();
            let fechaInicio = inputFechaInicio.val();

            if (fechaInicio == '') {
                Alert2Warning('Debe capturar una fecha de inicio.');
                return;
            }

            axios.post('/Horometros/GetEconomicosSinHorometros', { areaCuenta, economico, fechaInicio }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    dtResultados.clear().draw();
                    dtResultados.rows.add(response.data.data).draw(false);

                    botonImprimir.css('display', response.data.data.length > 0 ? 'initial' : 'none');
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarReporte() {
            report.attr("src", '/Reportes/Vista.aspx?idReporte=292');

            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
    }

    $(document).ready(() => Maquinaria.Captura.Diaria.ReporteEconomicosSinHorometros = new ReporteEconomicosSinHorometros())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();