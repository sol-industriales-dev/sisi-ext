
(function () {

    $.namespace('maquinaria.Catalogo.Inventario.HistorialAsignacionMaquinariaH');

    HistorialAsignacionMaquinariaH = function () {
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

        cboEconomicos = $("#cboEconomicos"),
        tlbHorometrosMayores = $("#tlbHorometrosMayores");

        function init() {
            cboEconomicos.fillCombo('/Horometros/cboModalEconomico', { obj: 0 });
            
            cboEconomicos.select2();
            cboEconomicos.change(LoadTablaCont);
        }

        function LoadTablaCont() {
            $.blockUI({ message: 'Cargando...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatInventario/GetHistorialMaqinaria",
                data: { EconomicoID: cboEconomicos.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.HistoricoMaquinaria;
                    tlbHorometrosMayores.bootgrid("clear");
                    tlbHorometrosMayores.bootgrid("append", data);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        init();

    };

    $(document).ready(function () {

        maquinaria.Catalogo.Inventario.HistorialAsignacionMaquinariaH = new HistorialAsignacionMaquinariaH();
    });
})();

