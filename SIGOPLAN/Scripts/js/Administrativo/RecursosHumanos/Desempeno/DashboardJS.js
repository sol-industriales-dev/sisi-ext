(function () {
    $.namespace('RecursosHumanos.Desempeno.Dashboard');
    Dashboard = function () {
        const menuEscritorio = $("#menuEscritorio");
        const menuCalendario = $('#menuCalendario');
        const menuAdministracion = $('#menuAdministracion');
        const menuReportes = $('#menuReportes');
        const menuSalirVerComo = $('#menuSalirVerComo');

        const divPrincipal = $('#divPrincipal');
        
        const btnReportes = $('#btnReportes');

        function init() {
            $('#menuPrincipal div').on('click', function () {
                $('.opcionSeleccionada').removeClass('opcionSeleccionada');
                $(this).toggleClass('opcionSeleccionada');
            });

            menuEscritorio.click(CargarEscritorio);
            menuCalendario.click(CargarCalendario);
            menuAdministracion.click(CargarEmpleados);
            menuReportes.click(CargarReportes);
            menuSalirVerComo.click(CargarSalirVerComo);

            // menuSalirVerComo.click(function (e) {
            //     Alert2AccionConfirmar(
            //         "¡Cuidado!",
            //         "¿Desea salir de ver como?",
            //         "Confirmar",
            //         "Cancelar",
            //         
            //     );
            // });

            CargarEscritorio();
        }
        function CargarEscritorio() {
            divPrincipal.load('/Administrativo/Desempeno/_Escritorio');
        }
        function CargarCalendario() {
            divPrincipal.load('/Administrativo/Desempeno/_Calendario');
        }
        function CargarEmpleados() {
            divPrincipal.load('/Administrativo/Desempeno/_Empleados');
        }
        function CargarReportes() {
            divPrincipal.load('/Administrativo/Desempeno/_Reportes');
        }
        function CargarSalirVerComo() {
            window.location.href = "/Administrativo/Desempeno/SalirVerComo";
        }
        init();
    };
    $(document).ready(function () {
        RecursosHumanos.Desempeno.Dashboard = new Dashboard();
    }).ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
      .ajaxStop(() => { $.unblockUI(); });
})();