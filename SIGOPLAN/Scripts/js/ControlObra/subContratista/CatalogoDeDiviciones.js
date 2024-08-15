(() => {
$.namespace('CatalogoDeDiviciones.ControlObra');
    
    const btnAgregar = $('#btnAgregar');
    const btnModificar = $('#btnModificar');
    const tblDiviciones = $('#tblDiviciones');
    let dtDiviciones;
    const mdlEditarDivicion = $('#mdlEditarDivicion');
    const mdlNuevaDivicion = $('#mdlNuevaDivicion');
    const inpDescripcion = $('#inpDescripcion');
    const inpMensaje = $('#inpMensaje');
    const inpDescripcionEdit = $('#inpDescripcionEdit');
    const inpMensajeEdit = $('#inpMensajeEdit');
    const btnBuscar = $('#btnBuscar');
    const btnNuevo = $('#btnNuevo');
    const tblRequerimientos = $('#tblRequerimientos');
    const tblRequerimientosEdit = $('#tblRequerimientosEdit');
    let dtRequerimientos;
    let dtRequerimientosEdit;
    let lstRequerimientos=[];
    let lstRequerimientosEdit=[];


    const inpRequerimiento = $('#inpRequerimiento');
    const btnGuardarRequerimiento = $('#btnGuardarRequerimiento');
    const inpRequerimientoEdit = $('#inpRequerimientoEdit');
    const btnGuardarRequerimientoEdit = $('#btnGuardarRequerimientoEdit');


    ControlObra = function (){
        let init = () => {
            initDataTblCatalogo();
            obtenerDiviciones();
            eventsEventos();
            initDatatblRequerimientos();
            initDatatblRequerimientosEdit();
            
        }
        init();
    }
    function eventsEventos() {
        btnAgregar.click(function () {
            btnAgregar.attr('data-id',0);
            addEditDiviciones();
        });
        btnModificar.click(function () {
            addEditDivicionesEdit();
        });
        btnBuscar.click(function () {
            obtenerDiviciones();
        });
        btnNuevo.click(function () {
            inpMensaje.val('');
            inpDescripcion.val('');
            lstRequerimientos = [];
            AddRows(tblRequerimientos,lstRequerimientos);
        });
        btnGuardarRequerimiento.click(function () {
            let item = {
                id:lstRequerimientos.length,
                texto: inpRequerimiento.val(),
            }
            lstRequerimientos.push(item);
            tablaRequerimientos();
            inpRequerimiento.val('');
        });
        btnGuardarRequerimientoEdit.click(function () {
            let item = {
                id:lstRequerimientosEdit.length,
                texto: inpRequerimientoEdit.val(),
            }
            lstRequerimientosEdit.push(item);
            AddRows(tblRequerimientosEdit,lstRequerimientosEdit);
            inpRequerimientoEdit.val('');
        });
    }
    function getParametrosAgregar() {
        let parametros = {
            id:btnAgregar.attr('data-id'),
            toltips:inpMensaje.val(),
            descripcion:inpDescripcion.val(),
            lstRequerimientos:lstRequerimientos
        }
        return parametros;
    }
    function getParametrosEdit() {
        let parametros = {
            id:btnModificar.attr('data-id'),
            toltips:inpMensajeEdit.val(),
            descripcion:inpDescripcionEdit.val(),
            lstRequerimientos:lstRequerimientosEdit
        }
        return parametros;
    }
    function addEditDiviciones() {
        let parametros = getParametrosAgregar();
        axios.post('addEditDiviciones', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    Alert2Exito('Operacion realizada con exito');
                    mdlNuevaDivicion.modal('hide');
                    mdlEditarDivicion.modal('hide');
                    obtenerDiviciones();
                }
            });
    }
    function addEditDivicionesEdit() {
        let parametros = getParametrosEdit();
        axios.post('addEditDiviciones', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    Alert2Exito('Operacion realizada con exito');
                    mdlNuevaDivicion.modal('hide');
                    mdlEditarDivicion.modal('hide');
                    obtenerDiviciones();
                }
            });
    }
    function eliminarDiviciones(id) {
        axios.post('eliminarDiviciones', {id:id})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    if (items.estatus) {
                        obtenerDiviciones();
                        Alert2Exito(items.mensaje);
                    }
                    
                }
            });
    }
    function obtenerDiviciones() {
        axios.post('obtenerDiviciones', {})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    AddRows(tblDiviciones,items)
                }
            });
    }

    function initDataTblCatalogo() {
        dtDiviciones = tblDiviciones.DataTable({
            destroy: true
            ,paging: false
            ,ordering:false
            ,searching: false
            ,bFilter: true
            ,info: false
            ,language: dtDicEsp
            ,columns: [
                { data: 'descripcion', title: 'Descripcion Evaluacion' },
                { data: 'toltips', title: 'Mensaje Evaluacion' },
                {
                    data: 'id',title:'Acciones' ,createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`
                                    <button class="btn btn-warning editarDiviciones"> <i class="fa fa-save"></i></button>
                                    <button class="btn btn-danger eliminarDiviciones"> <i class="fa fa-save"></i></button>`);
                        $(td).find(`button`).attr('data-id',data);
                    }
                },
            ]
            ,initComplete: function (settings, json) {
                tblDiviciones.on("click", ".eliminarDiviciones", function () {
                    let strMensaje = "¿Desea eliminar el registro seleccionado?";

                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            eliminarDiviciones($(this).attr("data-id"));
                        }
                    });
                });

                tblDiviciones.on("click", ".editarDiviciones", function (e) {
                    const rowData = dtDiviciones.row($(this).closest("tr")).data();
                    btnModificar.attr('data-id',rowData.id)
                    tablaRequerimientosEdit(rowData.id);
                    mdlEditarDivicion.modal("show");
                    abrirModal(rowData);
                });
            }
        });
    }
    function abrirModal(data) {
        inpDescripcionEdit.val(data.descripcion);
        inpMensajeEdit.val(data.toltips);
    }
    function AddRows(tbl, lst) {
        dtGestionCambios = tbl.DataTable();
        dtGestionCambios.clear().draw();
        dtGestionCambios.rows.add(lst).draw(false);
    }
    function tablaRequerimientos() {
        AddRows(tblRequerimientos,lstRequerimientos);
    }
    function tablaRequerimientosEdit(idDiv) {
        axios.post('obtenerRequerimientos', {idDiv:idDiv})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    lstRequerimientosEdit = items;
                    AddRows(tblRequerimientosEdit,lstRequerimientosEdit);
                }
            });
    }
    function initDatatblRequerimientos() {
        dtRequerimientos = tblRequerimientos.DataTable({
            destroy: true
            ,paging: false
            ,ordering:false
            ,searching: false
            ,bFilter: true
            ,info: false
            ,language: dtDicEsp
            ,columns: [
                { data: 'texto', title: 'texto' },
                {
                    data: 'id' ,title:'Acciones',createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`
                        <button class="btn btn-danger eliminarRequerimientos"> <i class="fa fa-save"></i></button>`);
                         $(td).find(`button`).attr('data-id',data);
                    }
                },
            ]
            ,initComplete: function (settings, json) {
                tblRequerimientos.on("click", ".eliminarRequerimientos", function () {
                    let strMensaje = "¿Desea eliminar el registro seleccionado?";

                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            eliminarRequerimientos($(this).attr("data-id"));
                        }
                    });
                });
            }
        });
    }
    function initDatatblRequerimientosEdit() {
        dtRequerimientosEdit = tblRequerimientosEdit.DataTable({
            destroy: true
            ,paging: false
            ,ordering:false
            ,searching: false
            ,bFilter: true
            ,info: false
            ,language: dtDicEsp
            ,columns: [
                { data: 'texto', title: 'texto' },
                {
                    data: 'id' ,title:'Acciones',createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`
                        <button class="btn btn-danger eliminarRequerimientosEdit"> <i class="fa fa-save"></i></button>`);
                         $(td).find(`button`).attr('data-id',data);
                    }
                },
            ]
            ,initComplete: function (settings, json) {
                tblRequerimientosEdit.on("click", ".eliminarRequerimientosEdit", function () {
                    let strMensaje = "¿Desea eliminar el registro seleccionado?";

                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            eliminarRequerimientosEdit($(this).attr("data-id"));
                        }
                    });
                });
            }
        });
    }
    function eliminarRequerimientos(id) {
        console.log(lstRequerimientos)
        console.log(lstRequerimientos)
        var removed =   lstRequerimientos.splice(lstRequerimientos.indexOf(id), 1);
        AddRows(tblRequerimientos,lstRequerimientos);
    }
    function eliminarRequerimientosEdit(id) {
        console.log(lstRequerimientosEdit)
        console.log(lstRequerimientosEdit.indexOf(id))
        var removed =   lstRequerimientosEdit.splice(lstRequerimientosEdit.indexOf(id), 1);
        AddRows(tblRequerimientosEdit,lstRequerimientosEdit);
    }


    $(document).ready(() => {
        CatalogoDeDiviciones.ControlObra = new ControlObra();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();