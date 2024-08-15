(() => {
$.namespace('_shared._menuClick');
    _menuClick = function (){
        const menu_click = $('#menu_click');
        let init = () => {
            $(document).mousedown(esconderMenu);
        }
        /**
         * @param {Object} menuConfig Configuración del menú
         * @param {Object} menuConfig.parametros Parametros para option.fn()
         * @param {Array} menuConfig.lstOptions Opciones del menú
         * @param {Object} menuConfig.lstOptions.option Opcion Individual
         * @param {HTML} menuConfig.lstOptions.option.text Texto
         * @param {Key} menuConfig.lstOptions.option.action Accion seleccionada
         * @param {Function} menuConfig.lstOptions.option.acfntion función a ejecutar
         */
        // menuConfig = {
        //     parametros: {},
        //     lstOptions: [
        //          {text: "text" ,action:key ,fn: parametros => { function(parametros) }
        //      ]}
        setMenuOption = () => {
            menu_click.html("");
            menuConfig.lstOptions.forEach(opt => {
                let li = $("<li>");
                li.html(opt.text);
                li.data().action = opt.action;
                li.click(setMenuAction);
                menu_click.append(li);
            });
        };
        mostrarMenu = () => {
            setMenuOption();
            menu_click.finish().toggle(100).
            css({
                top: event.pageY + "px",
                left: event.pageX + "px"
            });
        };
        function esconderMenu(e) {
            if (!$(e.target).parents(`#${menu_click.prop("id")}`).length > 0) {
                menu_click.hide(100);
            }
        }
        function setMenuAction() {
            let action = $(this).data().action;
            let optSel = menuConfig.lstOptions.find( opt => opt.action === action );
            optSel.fn(menuConfig.parametros);
            menu_click.hide(100);
        }
        init();
    }
    $(document).ready(() => {
        _shared._menuClick = new _menuClick();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();