(() => {
    $.namespace('Enkontrol._mdlUsuarioEnkontrol');
    _mdlUsuarioEnkontrol = function () {
        txtCve = $('#txtCve');
        txtNombre = $('#txtNombre');
        txtNoEnkontrol = $('#txtNoEnkontrol');
        mdlUsEnkontrol = $('#mdlUsEnkontrol');
        btnUsuarioGuardar = $('#btnUsuarioGuardar');
        const init = () => {
            setUsuario();
            btnUsuarioGuardar.click(guardarUsuario);
        },
            // getThisUsuarioEnkontrol = () => { return $.post('/Enkontrol/Requisicion/getThisUsuarioEnkontrol'); },
            saveUsuarioEnkontrol = () => {
                return $.post('/Administrador/Usuarios/saveUsuarioEnkontrol', {
                    empleado: txtCve.val(),
                    sn_empleado: txtNoEnkontrol.val()
                });
            };
        let setUsuario = () => {
            // getThisUsuarioEnkontrol().done((response) => {
            //     txtCve.val(response.empleado);
            //     txtNombre.val(response.nombre);
            //     txtNoEnkontrol.val(response.ekUsuario);
            // });
        }
        let guardarUsuario = () => {
            saveUsuarioEnkontrol().done((response) => {
                if (response.success) {
                    AlertaGeneral("Aviso", "Usuario Guardado Correctamente.");
                    mdlUsEnkontrol.modal("hide");
                }
            });
        }
        init();
    }
    $(document).ready(() => {
        Enkontrol._mdlUsuarioEnkontrol = new _mdlUsuarioEnkontrol();
    })
        // .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        // .ajaxStop(() => { $.unblockUI(); })
        ;
})();