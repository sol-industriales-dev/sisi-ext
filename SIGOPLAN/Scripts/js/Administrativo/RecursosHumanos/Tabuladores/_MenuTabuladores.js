(() => {
    $.namespace('CH._MenuTabuladores');

    //#region CONST
    const menuLineaNegocios = $("#menuLineaNegocios");
    const menuPuestos = $("#menuPuestos");
    const menuTabuladores = $("#menuTabuladores");
    const menuPlantillasPersonal = $("#menuPlantillasPersonal");
    const menuGestionTabuladores = $('#menuGestionTabuladores');
    const menuGestionPlantillasPersonal = $('#menuGestionPlantillasPersonal');
    const menuModificacion = $('#menuModificacion');
    const menuGestionModificacion = $('#menuGestionModificacion');
    const menuReportes = $('#menuReportes');
    const menuGestionReportes = $('#menuGestionReportes');
    //#endregion

    _MenuTabuladores = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region REDIRECCIONAMIENTOS
            menuLineaNegocios.click(function () {
                document.location.href = '/Administrativo/Tabuladores/LineaNegocio';
            });

            menuPuestos.click(function () {
                document.location.href = '/Administrativo/Reclutamientos/CatPuestos';
            });

            menuTabuladores.click(function () {
                document.location.href = '/Administrativo/Tabuladores/AsignarTabulador';
            });

            menuPlantillasPersonal.click(function () {
                document.location.href = '/Administrativo/Tabuladores/PlantillaPersonal';
            });

            menuGestionTabuladores.click(function () {
                document.location.href = '/Administrativo/Tabuladores/GestionTabuladores';
            });

            menuGestionPlantillasPersonal.click(function () {
                document.location.href = '/Administrativo/Tabuladores/GestionPlantillasPersonal';
            });

            menuModificacion.click(function () {
                document.location.href = '/Administrativo/Tabuladores/Modificacion';
            });

            menuGestionModificacion.click(function () {
                document.location.href = '/Administrativo/Tabuladores/GestionModificacion';
            });

            menuReportes.click(function () {
                document.location.href = '/Administrativo/Tabuladores/Reportes';
            });

            menuGestionReportes.click(function () {
                document.location.href = '/Administrativo/Tabuladores/GestionReportes';
            });
            //#endregion
        }
    }

    $(document).ready(() => {
        CH._MenuTabuladores = new _MenuTabuladores();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();