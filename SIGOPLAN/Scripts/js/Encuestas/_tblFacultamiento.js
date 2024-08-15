(() => {
$.namespace('Encuestas._tblFacultamiento');
    _tblFacultamiento = function (){
        const tblFacultamiento = $('#tblFacultamiento');
        const getLstFacultamiento = new URL(window.location.origin + '/Encuestas/Encuestas/getLstFacultamiento');
        let init = () => {
            setTblFacultamiento();
        }
        async function setTblFacultamiento() {
            try {
                response = await ejectFetchJson(getLstFacultamiento);
                if (response.success) {
                    //cargar encabezado
                    //cargar cuerpo
                }
            } catch (o_O) { AlertaGeneral('Aviso',o_O.message) }
        }
        init();
    }
    $(document).ready(() => {
        Encuestas._tblFacultamiento = new _tblFacultamiento();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();