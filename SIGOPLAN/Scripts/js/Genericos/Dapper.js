(() => {
$.namespace('dapper.GenericoDapper');
    GenericoDapper = function (){
        let init = () => {
            obtenerEnkontrolCC();
        }
        init();


        function obtenerEnkontrolCC() {
            axios.post('obtenerConsultaEnkontrol', {})
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, items} = response.data;
                    if (success) {
                        console.log(items)
                    }
                });
        }




    }
    $(document).ready(() => {
        dapper.GenericoDapper = new GenericoDapper();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();