(() => {
$.namespace('CatalogoDeRespuestas.ControlObra');
    
    const btnNuevo = $('#btnNuevo');
    const mdlNuevoRiesgo = $('#mdlNuevoRiesgo');
    const titulo = $('#titulo');
    const btnSave = $('#btnSave');
    const inpRiesgo = $('#inpRiesgo');
    const btnBuscar = $('#btnBuscar');

    const tblCatalogoDeRespuestas = $('#tblCatalogoDeRespuestas');
    let dttblCatalogoDeRespuestas;



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
        inpRiesgo.val('');
    }

    function initDatatblCatalogoDeRespuestas() {
        dttblCatalogoDeRespuestas = tblCatalogoDeRespuestas.DataTable({
            destroy: true
            ,language: dtDicEsp
            ,paging: true
            ,ordering:true
            ,searching: true
            ,bFilter: true
            ,info: true
            ,columns: [
                { data: 'descripcion', title: 'descripcion' },
                { data: 'Opciones' ,render: (data, type, row, meta) => {
                    let html = '';

                    html = `<button class='btn btn-primary btn-sm editarRiesgo'><i class='glyphicon glyphicon-pencil'></i></button>
                            <button class='btn btn-danger btn-sm eliminarRiesgo'><i class='glyphicon glyphicon-trash'></i></button>`;

                    return html;
                }},
            ]
            ,initComplete: function (settings, json) {
                tblCatalogoDeRespuestas.on('click','.editarRiesgo', function () {
                    const rowData = dttblCatalogoDeRespuestas.row($(this).closest('tr')).data();
                    titulo.text('Editar Riesgo');
                    inpRiesgo.val(rowData.descripcion);
                    btnSave.attr('data-id',rowData.id);
                    mdlNuevoRiesgo.modal('show');
                });
                tblCatalogoDeRespuestas.on('click','.eliminarRiesgo', function () {
                    const rowData = dttblCatalogoDeRespuestas.row($(this).closest('tr')).data();
                
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
            descripcion:inpRiesgo.val()
        }
        return parametros;
    }
    function AddRows(tbl, lst) {
        dttblCatalogoDeRespuestas = tbl.DataTable();
        dttblCatalogoDeRespuestas.clear().draw();
        dttblCatalogoDeRespuestas.rows.add(lst).draw(false);
    }
    function obtenerListado() {
        axios.post('lstMrCategorias').then(response => {
            let { success, items, message } = response.data;
            if (success) {
                AddRows(tblCatalogoDeRespuestas,items);
            }
        }).catch(error => Alert2Error(error.message));
    }
    function guardarEditar() {
        let parametros = getParameters();
        axios.post('AgregarEditarCategoria', {parametros:parametros}).then(response => {
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
        axios.post('EliminarCategoria', {parametros:parametros}).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                obtenerListado();
                mdlNuevoRiesgo.modal('hide');
            }
        }).catch(error => Alert2Error(error.message));
    }

    


    $(document).ready(() => {
        CatalogoDeRespuestas.ControlObra = new ControlObra();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();