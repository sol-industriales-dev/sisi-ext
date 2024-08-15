(() => {
    $.namespace('SISTEMA.SISTEMA.FormularioDatosEnKontrol');

    FormularioDatosEnKontrol = function () {

        // Variables.
        const inputNombreEmpleado = $('#inputNombreEmpleado');
        const inputPassword = $('#inputPassword');
        const inputClaveEmpleado = $('#inputClaveEmpleado');

        const botonVerificar = $('#botonVerificar');

        (function init() {
            // Lógica de inicialización.
            verificarUsuarioCorrecto();
            agregarListeners();
            $('#menuNuevo ul.horizontalMenu').hide();
        })();

        // Métodos.
        function agregarListeners() {
            botonVerificar.click(validarDatos);
        }

        function validarDatos() {

            const password = inputPassword.val().trim();
            const claveEmpleado = inputClaveEmpleado.val().trim();

            if (password == "" || password.length <= 4) {
                AlertaGeneral(`Aviso`, `Debe proporcionar una contraseña válida.`);
                return;
            } else if (password.split('')[0] !== '0') {
                AlertaGeneral(`Aviso`, `Toda clave debe comenzar con un cero.`);
                return;
            }
            else if (claveEmpleado == "") {
                AlertaGeneral(`Aviso`, `Debe proporcionar una clave de empleado válida.`);
                return;
            }

            $.post('ValidarDatosUsuarioEnKontrol', { password, claveEmpleado })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral(`Éxito`, `Datos validados corrrectamente. En un momento será redireccionado.`);
                        botonVerificar.hide();
                        setTimeout(() => window.location.href = response.redireccionURl, 4000);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function verificarUsuarioCorrecto() {
            $.get('NecesitaIngresarDatosEnKontrol')
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        if (response.noNecesitaVerificar) {
                            AlertaGeneral(`Aviso`, `No es necesario que este usuario verifique sus datos`);
                            setTimeout(() => window.location.href = response.redireccionURl, 2000);
                        } else {
                            inputNombreEmpleado.val(response.nombreCompleto);
                            inputClaveEmpleado.val(response.claveEmpleado);
                        }
                    } else {
                        AlertaGeneral(`Aviso`, `Ocurrió un error al verificar el usuario`);
                        setTimeout(() => window.location.href = response.redireccionURl, 2000);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }


    }

    $(() => SISTEMA.SISTEMA.FormularioDatosEnKontrol = new FormularioDatosEnKontrol())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();