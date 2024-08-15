(function () {

    $.namespace('maquinaria.ConfiguracionDiaria');

    ConfiguracionDiaria = function () {

        btnGuardarDiesel = $("#btnGuardarDiesel");

        mensajes = {
            NOMBRE: 'Aseguradora',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };


        function init() {
            btnGuardarDiesel.click(SaveOrUpdate_PrecioDiesel( getObjectPrecioDiesel());
        }

        function getObjectPrecioDiesel() {
            return {
                id: 0,
                precio: txtPrecioXLitro.val().trim(),
                idUsuario: 0,
                fecha: ''
            }
        }


        function SaveOrUpdate_PrecioDiesel(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/ConfiguracionDiaria/SaveOrUpdate_PrecioDiesel',
                type: 'POST',
                dataType: 'json',
                data: { obj: obj },
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    resetValues();
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

    };

    $(document).ready(function () {

        maquinaria.ConfiguracionDiaria = new ConfiguracionDiaria();
    });
})();
