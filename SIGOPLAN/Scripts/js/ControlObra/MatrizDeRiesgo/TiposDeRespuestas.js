(() => {
$.namespace('TiposDeRespuestas.ControlObra');
    
    const tblTiposDeRespuestas = $('#tblTiposDeRespuestas');
    let dttblTiposDeRespuestas;

    const btnNuevo = $('#btnNuevo');
    const mdlNuevoRiesgo = $('#mdlNuevoRiesgo');
    const titulo = $('#titulo');
    const btnSave = $('#btnSave');
    const inpRespuesta = $('#inpRespuesta');
    const btnBuscar = $('#btnBuscar');
    const checkAmenazaOportunidad = $('#checkAmenazaOportunidad');
    const inpRespuestaDesc = $('#inpRespuestaDesc');

    ControlObra = function (){
        let init = () => {
            initDatatblCatalogoDeRespuestas();
            evenlistener();
            obtenerListado(); 
        }
        init();
    }


    function evenlistener () {
        btnBuscar.click(function () {
           obtenerListado(); 
        });
        btnNuevo.click(function () {
            mdlNuevoRiesgo.modal('show');
            btnSave.attr('data-id',0);
            titulo.text('Guardar Riesgo');
            Limpiar();
        });
        btnSave.click(function () {
            guardarEditar();
        });
    }
    function Limpiar() {
        btnSave.attr('data-id',0);
        inpRespuesta.val('');
        inpRespuestaDesc.val('');
    }

    function initDatatblCatalogoDeRespuestas() {
        dttblTiposDeRespuestas = tblTiposDeRespuestas.DataTable({
            destroy: true
            ,language: dtDicEsp
            ,paging: true
            ,ordering:true
            ,searching: true
            ,bFilter: true
            ,info: true
            ,columns: [
                { data: 'descripcion', title: 'Respuesta' },
                { data: 'tipoRespuesta', title: 'Tipo' },
                { data: 'respuestaDesc', title: 'Descripcion' },
                { data: 'Opciones' ,render: (data, type, row, meta) => {
                    let html = '';

                    html = `<button class='btn btn-primary btn-sm editarRiesgo'><i class='glyphicon glyphicon-pencil'></i></button>
                            <button class='btn btn-danger btn-sm eliminarRiesgo'><i class='glyphicon glyphicon-trash'></i></button>`;

                    return html;
                }},
            ]
            ,initComplete: function (settings, json) {
                tblTiposDeRespuestas.on('click','.editarRiesgo', function () {
                    const rowData = dttblTiposDeRespuestas.row($(this).closest('tr')).data();
                    titulo.text('Editar Riesgo');
                    checkAmenazaOportunidad.prop('checked',rowData.tipoRespuesta == "Amenaza" ? true : false).change();
                    inpRespuesta.val(rowData.descripcion);
                    inpRespuestaDesc.val(rowData.respuestaDesc);
                    btnSave.attr('data-id',rowData.id);
                    mdlNuevoRiesgo.modal('show');
                });
                tblTiposDeRespuestas.on('click','.eliminarRiesgo', function () {
                    const rowData = dttblTiposDeRespuestas.row($(this).closest('tr')).data();
                    
                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>Desea eliminar el registro?</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            EliminarCategoria(rowData.id);
                        }
                    });
                });
            }
        });
    }
    function getParameters() {
        let parametros = {
            id:btnSave.attr('data-id'),
            descripcion:inpRespuesta.val(),
            tipoRespuesta:checkAmenazaOportunidad.prop('checked') ? 1:2,
            respuestaDesc:inpRespuestaDesc.val(),
        }
        console.log(checkAmenazaOportunidad.val())
        return parametros;
    }
    function AddRows(tbl, lst) {
        dttblTiposDeRespuestas = tbl.DataTable();
        dttblTiposDeRespuestas.clear().draw();
        dttblTiposDeRespuestas.rows.add(lst).draw(false);
    }
    function obtenerListado() {
        axios.post('lstMrTiposDeRespuestas').then(response => {
            let { success, items, message } = response.data;
            if (success) {
                AddRows(tblTiposDeRespuestas,items);
            }
        }).catch(error => Alert2Error(error.message));
    }
    function guardarEditar() {
        let parametros = getParameters();
        axios.post('AgregarEditarTiposDeRespuestas', {parametros:parametros}).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                obtenerListado();
                mdlNuevoRiesgo.modal('hide');
            }
        }).catch(error => Alert2Error(error.message));
    }
    function EliminarCategoria(id) {
        let parametros = {
            id:id
        }
        axios.post('EliminarTiposDeRespuestas', {parametros:parametros}).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                obtenerListado();
                mdlNuevoRiesgo.modal('hide');
            }
        }).catch(error => Alert2Error(error.message));
    }







    $(document).ready(() => {
        TiposDeRespuestas.ControlObra = new ControlObra();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();