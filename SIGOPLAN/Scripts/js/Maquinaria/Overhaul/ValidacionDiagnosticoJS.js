(function () {

    $.namespace('maquinaria.overhaul.reporteFallas');

    ReporteFallas = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Reporte de falla.',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {

        }

        init();

    };

    $(document).ready(function () {

        maquinaria.overhaul.reporteFallas = new ReporteFallas();
    });
})();

