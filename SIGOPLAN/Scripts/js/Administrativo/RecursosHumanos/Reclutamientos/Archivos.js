(() => {
    $.namespace('CH.Archivos');

    //#region JQ VARS

    const tblArchivos = $('#tblArchivos');
    const btnNuevo = $('#btnNuevo');

    //#endregion

    let dtArchivos;

    //#region CREAR/EDITAR MODAL VARS

    const spanTitleCrearEditarArchivo = $('#spanTitleCrearEditarArchivo');
    const spanBtnCrearEditarArchivo = $('#spanBtnCrearEditarArchivo');
    const btnCrearEditarArchivo = $('#btnCrearEditarArchivo');
    const mdlCrearEditarArchivo = $('#mdlCrearEditarArchivo');
    const txtCrearEditarDescripcion = $('#txtCrearEditarDescripcion');

    //#endregion

    Archivos = function () {
        (function init() {

            fncListeners();
            initTblArchivos();
            fncGetArchivos();
            
        })();

        function fncListeners() {

            btnNuevo.on("click", function(){

                //limpiar mdl
                txtCrearEditarDescripcion.val('');
                btnCrearEditarArchivo.attr("data-id", 0);
                spanBtnCrearEditarArchivo.html("Guardar");
                spanTitleCrearEditarArchivo.html("Nueva solicitud");
                mdlCrearEditarArchivo.modal("show");

            });

            btnCrearEditarArchivo.on("click", function () {
                fncCrearEditarArchivo();
            });

        }
        function initTblArchivos() {
            dtArchivos = tblArchivos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    {data: 'descripcion', title: "Nombre"},
    
                    { data: 'id', title: 'id', visible: false},
                    { data: 'registroActivo', title: 'registroActivo', visible: false},
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-warning actualizarArchivo btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarArchivo btn-xs"><i class="far fa-trash-alt"></i></button>`;
                            let btns = btnActualizar + btnEliminar;
                            return btns;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblArchivos.on('click','.actualizarArchivo', function () {
                        let rowData = dtArchivos.row($(this).closest('tr')).data();
                        txtCrearEditarDescripcion.val(rowData.descripcion);
                        btnCrearEditarArchivo.attr("data-id", rowData.id);
                        spanBtnCrearEditarArchivo.html("Actualizar");
                        spanTitleCrearEditarArchivo.html("Actualizar solicitud");
                        mdlCrearEditarArchivo.modal("show");
                    });
                    tblArchivos.on('click','.eliminarArchivo', function () {
                        let rowData = dtArchivos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarArchivo(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center','targets': '_all'}
                ],
            });
        }
    
        function fncGetDataCrearEditarArchivo() {

            let strMensajeError = "";

            if (txtCrearEditarDescripcion.val() == "") { txtCrearEditarDescripcion.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let objSolicitud = new Object();
                objSolicitud = {
                    id: btnCrearEditarArchivo.attr('data-id'),
                    descripcion: txtCrearEditarDescripcion.val(),
                }
                return objSolicitud;
            }
            
        }
    
        //#endregion
    
        //#region BACKEND
    
        function fncGetArchivos() {
    
            axios.post("GetArchivos").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    console.log(items);
    
                    dtArchivos.clear();
                    dtArchivos.rows.add(items.items);
                    dtArchivos.draw();
                }
            }).catch(error => Alert2Error(error.message));
            
        }
    
        function fncCrearEditarArchivo() {
            let objArchivo = fncGetDataCrearEditarArchivo();
            if (objArchivo != "") {
                axios.post("CrearEditarArchivos", objArchivo).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetArchivos();
                        mdlCrearEditarArchivo.modal("hide");
                        let strMensaje = btnCrearEditarArchivo.attr("data-id") > 0 ? "Se ha actualiza con éxito el archivo." : "Se ha registrado con éxito el archivo.";
                        Alert2Exito(strMensaje);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));    
            }
        }

        function fncEliminarArchivo(idEliminar) {
            axios.post("EliminarArchivo", {idArchivo:idEliminar} ).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetArchivos();
                    Alert2Exito(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Archivos = new Archivos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();