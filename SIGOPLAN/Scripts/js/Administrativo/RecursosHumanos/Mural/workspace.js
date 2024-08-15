
$(function () {

    $.namespace('recursoshumanos.mural');
    let currendMural = 0;
    let clsMural = $('.clsMural');
    let clsMuralPropio = $('.clsMuralPropio');
    let clsMuralOtro = $('.clsMuralOtro');
    let clsMuralNombre = $('.clsMuralNombre');
    //Crear
    let btnOpenCreateMural = $('#btnOpenCreateMural');
    let modal_crear_mural = $("#modal_crear_mural");
    let btnCloseCreateMural = $('#btnCloseCreateMural');
    let txtMuralName = $('#txtMuralName');
    let txtMuralDesc = $('#txtMuralDesc');
    let btnCrearMural = $('#btnCrearMural');

    //Duplicar
    let modal_duplicar_mural = $('#modal_duplicar_mural');
    let btnCloseDuplicarMural = $('#btnCloseDuplicarMural');
    let txtMuralDuplicarName = $('#txtMuralDuplicarName');
    let txtMuralDuplicarDesc = $('#txtMuralDuplicarDesc');
    let btnDuplicarMural = $('#btnDuplicarMural');

    //Eliminar
    let modal_eliminar_mural = $('#modal_eliminar_mural');
    let btnCloseEliminarMural = $('#btnCloseEliminarMural');
    let txtMuralEliminarName = $('#txtMuralEliminarName');
    let btnEliminarMural = $('#btnEliminarMural');

    //Permisos
    let modal_permisos_mural = $('#modal_permisos_mural');
    let btnClosePermisosMural = $('#btnClosePermisosMural');
    let txtPermisoUsuario = $("#txtPermisoUsuario");
    let cboTipoPermiso = $("#cboTipoPermiso");
    let btnGuardarPermiso = $('#btnGuardarPermiso');
    let tblDataUsuariosPermiso = $("#tblDataUsuariosPermiso");

    const _getMuralList = new URL(window.location.origin + '/Administrativo/Mural/getMuralList');
    const _createNewMural = new URL(window.location.origin + '/Administrativo/Mural/createNewMural');
    const _renameMural = new URL(window.location.origin + '/Administrativo/Mural/renameMural');
    const _duplicateNewMural = new URL(window.location.origin + '/Administrativo/Mural/duplicateNewMural');
    const _deleteMural = new URL(window.location.origin + '/Administrativo/Mural/deleteMural');
    const _setUsuarioMural = new URL(window.location.origin + '/Administrativo/Mural/setUsuarioMural');
    const _updateUsuarioMural = new URL(window.location.origin + '/Administrativo/Mural/updateUsuarioMural');
    const _deleteUsuarioMural = new URL(window.location.origin + '/Administrativo/Mural/deleteUsuarioMural');
    const _getUserMuralList = new URL(window.location.origin + '/Administrativo/Mural/getUserMuralList');

    mural = function () {
        var actual;

        function init() {

            $('.clsListadoMurales').on("click", '.openmenu', function () {
                actual = $(this);
                $(this).siblings('.menuacciones').toggleClass('hidden');
        
            });
            //var obj ={};
            //obj.id=1;
            //obj.nombre='Prueba';
            //obj.image='None';
            //obj.modificado='hace 2 horas';

            

            //var html2 = createMuralCardOtros(obj);
            //clsMuralOtro.append(html2);

            btnOpenCreateMural.click(fnOpenCreateMural);
            btnCloseCreateMural.click(fnCloseCreateMural);
            btnCrearMural.click(fnCreateMural);
            fnLoadMuralList(true);
            fnLoadMuralList(false);
            $('.clsListadoMurales').on('change','.clsMuralNombre',fnRenameMural);
            $('.clsListadoMurales').on('click','.item-button-Accesos',fnOpenAccesosMural);
            $('.clsListadoMurales').on('click','.item-button-Duplicar',fnOpenDuplicarMural);
            $('.clsListadoMurales').on('click','.item-button-Eliminar',fnOpenEliminarMural);
            btnEliminarMural.click(fnEliminarMural);
            btnCloseEliminarMural.click(fnCloseEliminarMural);
            btnDuplicarMural.click(fnDuplicarMural);
            btnEliminarMural.click(fnEliminarMural);
            btnCloseDuplicarMural.click(fnCloseDuplicarMural);
            btnClosePermisosMural.click(fnCloseAccesosMural);
            txtPermisoUsuario.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            btnGuardarPermiso.click(fnGuardarPermiso);
            initTable();
        }
        function initTable() {
            tblDataUsuariosPermiso = $("#tblDataUsuariosPermiso").DataTable({
                columns: [
                    { data: 'id'},
                    { data: 'usuarioNombre', width: "300px" },
                    { data: 'tipo', width: "150px", render: function ( data, type, row ) { var d = `<select class="form-control cboTipoPermiso" data-id="${row.id}"><option value="1">Facilitador</option><option value="2">Solo lectura</option></select>`; $(d).val(row.tipo); return d;}},
                        { data: 'tipo', width: "50px" , render: function ( data, type, row ) { return `<button data-id="${row.id}" class="clsEliminarPermiso">Eliminar</button>`;} }
                ],
                                columnDefs: [ { targets: 0, "visible": false } ],
                                initComplete: function (settings, json) {
                                    tblDataUsuariosPermiso.on('change', '.cboTipoPermiso', fnActualizarUsuario);
                                    tblDataUsuariosPermiso.on('click', '.clsEliminarPermiso', fnEliminarUsuario);
                                },
                                "bPaginate": false,
                                "searching": false,
                                "bFilter": false,
                                "bInfo": true,
                                });

                        }
                        function fnSelRevisa(event, ui) {
                            $(this).data("id", ui.item.id);
                            $(this).data("nombre", ui.item.value);
                        }
                        function fnSelNull(event, ui) {
                            if (ui.item === null || $(this).val()=='') {
                                $(this).val("");
                                $(this).data("id", "");
                                $(this).data("nombre", "");
                                AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
                            }

                        }
                        function fnCleanMural(propio){
                            if(propio){
                                $(".clsMuralPropio").find(".clsMural").remove();
                            }
                            else{
                                $(".clsMuralOtro").find(".clsMural").remove();
                            }
                        }
                        function fnCloseCreateMural(){
                            modal_crear_mural.hide();
                        }
                        function fnCloseEliminarMural(){
                            modal_eliminar_mural.hide();
                        }
                        function fnCloseDuplicarMural(){
                            modal_duplicar_mural.hide();
                        }
                        function fnCloseAccesosMural(){
                            modal_permisos_mural.hide();
                        }
                        async function fnLoadMuralList(propio){
                            try {
                                response = await ejectFetchJsonNoBlock(_getMuralList, { propio : propio });
                                if (response.success) {
                                    var data = response.data;
                                    fnCleanMural(propio);
                                    $.each(data,function(i,e){
                                        if(propio){
                                            var html = createMuralCard(e);
                                            clsMuralPropio.append(html);
                                        }
                                        else{
                                            var html = createMuralCardOtros(e);
                                            clsMuralOtro.append(html);
                                        }
                                    });
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al cargar los murales`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
                        async function fnLoadUserList(){
                            try {
                                var id = currendMural;
                                tblDataUsuariosPermiso.clear().draw();
                                response = await ejectFetchJsonNoBlock(_getUserMuralList, { id: id });
                                if (response.success) {
                                    var data = response.data;
                                    tblDataUsuariosPermiso.rows.add(data).draw();
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al cargar los murales`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
        
                        async function fnActualizarUsuario()
                        {
                            try {
                                var id = $(this).data("id");
                                var tipo = $(this).val();;
                                response = await ejectFetchJsonNoBlock(_updateUsuarioMural, {id:id,tipo:tipo});
                                if (response.success) {
    
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al actualizar el usuario`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
                        async function fnEliminarUsuario()
                        {
                            try {
                                var id = $(this).data("id");
                                var user = $(this).data("userid");
                                response = await ejectFetchJsonNoBlock(_deleteUsuarioMural, {id:id});
                                if (response.success) {
                                    fnLoadUserList();
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al eliminar el usuario`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
                        function fnOpenCreateMural(){
                            txtMuralName.val('');
                            txtMuralDesc.val('');
                            modal_crear_mural.show();
                        }
                        function fnOpenAccesosMural(){
                            var _this = $(this);
                            currendMural = _this.data('id');
            
                            txtPermisoUsuario.val("");
                            cboTipoPermiso.val(1);
                            modal_permisos_mural.show();
                            fnLoadUserList();
                        }
                        function fnOpenDuplicarMural(){
                            var _this = $(this);
                            currendMural = _this.data('id');
                            txtMuralDuplicarName.val('');
                            txtMuralDuplicarDesc.val('');
                            modal_duplicar_mural.show();
                        }
                        function fnOpenEliminarMural(){
                            var _this = $(this);
                            currendMural = _this.data('id');
                            txtMuralEliminarName.val(_this.data('nombre'));
                            modal_eliminar_mural.show();
                        }
                        async function fnGuardarPermiso()
                        {
                            try {

                                var idUsuario = txtPermisoUsuario.data('id');
                                var idMural = currendMural;
                                var tipo = cboTipoPermiso.val();
                                response = await ejectFetchJsonNoBlock(_setUsuarioMural, { idUsuario : idUsuario, idMural : idMural,  tipo:tipo });
                                if (response.success) {
                                    fnLoadUserList();
                                    txtPermisoUsuario.val("");
                                    cboTipoPermiso.val(1);
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al crear mural`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
                        async function fnCreateMural()
                        {
                            try {
                                var name = txtMuralName.val();
                                var desc = txtMuralDesc.val();
                                response = await ejectFetchJsonNoBlock(_createNewMural, { nombre : name, desc : desc });
                                if (response.success) {
                                    modal_crear_mural.hide();
                                    fnLoadMuralList(true);
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al crear mural`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
                        async function fnRenameMural()
                        {
                            try {
                                var _this = $(this);
                                var id = _this.data('id');
                                var name = _this.val();
                                response = await ejectFetchJsonNoBlock(_renameMural, {id:id, nombre : name });
                                if (response.success) {
                                    fnLoadMuralList(true);
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al crear mural`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
                        async function fnDuplicarMural()
                        {
                            try {
                                var id = currendMural;
                                var name = txtMuralDuplicarName.val();
                                var desc = txtMuralDuplicarDesc.val();
                                response = await ejectFetchJsonNoBlock(_duplicateNewMural, {id:id, nombre : name, desc : desc });
                                if (response.success) {
                                    modal_duplicar_mural.hide();
                                    fnLoadMuralList(true);
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al duplicar el mural`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
                        async function fnEliminarMural()
                        {
                            try {
                                var id = currendMural;
                                response = await ejectFetchJsonNoBlock(_deleteMural, {id:id});
                                if (response.success) {
                                    modal_eliminar_mural.hide();
                                    fnLoadMuralList(true);
                                } else {
                                    AlertaGeneral(`Error`, `Ocurrió un error al eliminar el mural`);
                                }
                            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
                        }
                        function createMuralCard(obj) {
                            var modificado = moment(obj.fechaModificacion).format("DD/MM/YYYY hh:mm:ss A");
                            var html = `<li class="dashboard-grid-cell cell-thumb clsMural">
                                                <div aria-label="" class="dashboard-thumb thumb thumb-0" role="" title="" style="opacity: 1; animation: unset;">
                                                    <a aria-label="Untitled mural" href="/Administrativo/Mural/Index?id=${obj.id}" title="Untitled mural">
                                                        <div class="thumb-image-link">
                                                            <img class="thumb-image" alt="Untitled mural" src="${obj.icono}">
                                                        </div>
                                                    </a>    
                                                    <div class="thumb-actions">
                                                        <div class="ui-accessible-menu light-theme openmenu">
                                                            <button type="button" data-qa="thumb-dropdown" tabindex="0">
                                                                <div class="ui-dropdown-box-trigger"><div class="ui-dropdown-trigger"></div></div>
                                                            </button>
                                                        </div>
                                                        <div class="ui-callout menu bottom middle tip-size-mid tip-end color-neutral menuacciones hidden">
                                                            <span class="tip-safezone"></span>
                                                            <ul class="ui-menu with-icons">
                                                                <li class="icon">
                                                                    <button class='item-button-Accesos' data-qa="item-button-Archive" data-id="${obj.id}" type="button" tabindex="-1">
                                                                        <i class="icon m-icon-check"></i>
                                                                        <span class="item-label">Accesos</span>
                                                                    </button>
                                                                </li>
                                                                <li class="icon">
                                                                    <button class='item-button-Duplicar' data-qa="item-button-Duplicate" data-id="${obj.id}" type="button" tabindex="-1">
                                                                        <i class="icon m-contextual-duplicate"></i>
                                                                        <span class="item-label">Duplicar</span>
                                                                    </button>
                                                                </li>
                                                                <li class="icon">
                                                                    <button class='item-button-Eliminar' data-qa="item-button-Delete" data-nombre="${obj.nombre}" data-id="${obj.id}" type="button" tabindex="-1">
                                                                        <i class="icon m-contextual-trash-filled"></i>
                                                                        <span class="item-label">Eliminar</span>
                                                                    </button>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                    <div class="dashboard-grid-mural-info">
                                                        <center>
                                                            <div class="item data">
                                                                <h3 class="thumb-title">
                                                                    <input type="text" data-id="${obj.id}" class="form-control input_mural clsMuralNombre" value="${obj.nombre}" />
                                                                </h3>
                                                                <p>${modificado}</p>
                                                            </div>
                                                        </center>
                                                    </div>
                                                </div>
                                            </li>`;
                            return html;
                        }
                        function createMuralCardOtros(obj) {
            
                            var modificado = moment(obj.fechaModificacion).format("DD/MM/YYYY hh:mm:ss A");
                            var html = `<li class="dashboard-grid-cell cell-thumb clsMural">
                                                <div aria-label="" class="dashboard-thumb thumb thumb-0" role="" title="" style="opacity: 1; animation: unset;">
                                                    <a aria-label="Untitled mural" href="/Administrativo/Mural/Index?id=${obj.id}" title="Untitled mural">
                                                        <div class="thumb-image-link">
                                                            <img class="thumb-image" alt="Untitled mural" src="${obj.icono}">
                                                        </div>
                                                    </a>    
                                                    <div class="dashboard-grid-mural-info">
                                                        <center>
                                                            <div class="item data">
                                                                <h3 class="thumb-title">
                                                                    <input type="text" data-id="${obj.id}" class="form-control input_mural clsMuralNombre" readonly="true" value="${obj.nombre}" />
                                                                </h3>
                                                                <p>${modificado}</p>
                                                            </div>
                                                        </center>
                                                    </div>
                                                </div>
                                            </li>`;
                            return html;
                        }
                        init();
                    }
    $(document).ready(function () {
        recursoshumanos.mural = new mural();
    });
});



