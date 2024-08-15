(() => {
    $.namespace('Barrenacion.ReporteEjecutivo');

    controller = function () {
        const dateFormat = "dd/mm/yy";
        const fechaActual = new Date();
        const showAnim = "slide";
        // Variables.
        const comboAC = $('#comboAC');
        const inputFechaInicio = $("#inputFechaInicio");
        const inputFechaFin = $("#inputFechaFin");
        const btnBuscar = $('#btnBuscar');
        const report = $('#report');
        const comboBarrenadora = $('#comboBarrenadora');

        (function init() {
            // Lógica de inicialización.
            comboBarrenadora.fillCombo('/Barrenacion/ObtenerBarrenadorasPorCC', { areaCuenta: comboAC.val() != "" ? comboAC.val() : 0 }, false, 'Todos');
            convertToMultiselect("#comboBarrenadora");

            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker('setDate', fechaActual);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker('setDate', fechaActual);

            comboAC.fillCombo('/Barrenacion/ObtenerAC', null, false, "Todos");
            convertToMultiselect("#comboAC");
            btnBuscar.click(setRpt);
        })();

        function setRpt() {
            $.post('/Barrenacion/setInfoRptEjecutivo', { pFechaInicio: inputFechaInicio.val(), pfechaFin: inputFechaFin.val(), areaCuentas: comboAC.val(), barrenadoras: comboBarrenadora.val() })
                .then(response => {

                    if (response.success) {
                        var idReporte = "190";
                        var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pFechaInicio=" + inputFechaFin.val() + "&pfechaFin=" + inputFechaFin.val() + "&inMemory=1";

                        report.attr("src", path);

                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                        };
                    }
                    else {
                        AlertaGeneral(`Operación fallida`, `Error- ${response.message}.`);
                    }

                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }
    }

    $(() => Barrenacion.ReporteEjecutivo = new controller())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();