
(function () {

    $.namespace('maquinaria.SOS.CargaArchivos');

    CargaArchivos = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {
            bootG('/ResguardoEquipo/GetListaAutorizacionesPendientes');   
        }
    };

    $(document).ready(function () {

        maquinaria.SOS.CargaArchivos = new CargaArchivos();
    });
})();

