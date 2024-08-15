(function () {

    $.namespace('maquinaria.overhaul.reportestaller');

    reportestaller = function () {
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        txtFechaInicio = $("#txtFechaInicio"),
        txtFechaFin = $("#txtFechaFin"),
        btnBuscarPrecision = $("#btnBuscarPrecision"),
        btnReportePrecision = $("#btnReportePrecision"),
        tblPrecision = $("#tblPrecision");

        function init() {

        }

        init();
    };
    $(document).ready(function () {
        maquinaria.overhaul.reportestaller = new reportestaller();
    });
})();