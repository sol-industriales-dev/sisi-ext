(() => {
    $.namespace('CatalogoDeDiviciones.EvaluacionSubcontratista');

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
    let lstRequerimientos = [];
    let lstRequerimientosEdit = [];
    const inpEvaluador = $('#inpEvaluador');
    const inpEvaluadorEdit = $('#inpEvaluadorEdit');

    const inpRequerimiento = $('#inpRequerimiento');
    const btnGuardarRequerimiento = $('#btnGuardarRequerimiento');
    const inpRequerimientoEdit = $('#inpRequerimientoEdit');
    const btnGuardarRequerimientoEdit = $('#btnGuardarRequerimientoEdit');

    const inpOrden = $('#inpOrden');
    const inpImportant = $('#inpImportant');
    const inpOrdenEdit = $('#inpOrdenEdit');
    const inpImportantEdit = $('#inpImportantEdit');
    const btnBuscarPlantilla = $('#btnBuscarPlantilla');
    const tblPlantillas = $('#tblPlantillas');
    let dtPlantillas;
    const idMdlNuevoPlantilla = $('#idMdlNuevoPlantilla');
    const btnAgregarPlantilla = $('#btnAgregarPlantilla');
    const txtPlantilla = $('#txtPlantilla');
    const contenidoElementos = $('#contenidoElementos');
    const btnNuevoPlantilla = $('#btnNuevoPlantilla');
    const cboContratos = $('#cboContratos');

    const cboPlantillasBuscar = $('#cboPlantillasBuscar');
    const cboContratosBuscar = $('#cboContratosBuscar');

    EvaluacionSubcontratista = function () {
        let init = () => {
            initDataTblCatalogo();
            // obtenerDiviciones();
            obtenerPlantillas();
            eventsEventos();
            initDatatblRequerimientos();
            initDatatblRequerimientosEdit();
            initTblPlantillas();

        }
        init();
    }
    function fnSelRevisa(event, ui) {
        $(this).data("id", ui.item.id);
        $(this).data("nombre", ui.item.value);
    }
    function fnSelNull(event, ui) {
        // console.log(ui)
        if (ui.item === null && $(this).val() != '') {
            $(this).val("");
            $(this).data("id", "");
            $(this).data("nombre", "");
            AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
        }
    }
    function fncFillFiltros() {
        cboPlantillasBuscar.fillCombo('cboPlantillasBuscar', null, null);
        cboPlantillasBuscar.select2();

        cboContratosBuscar.fillCombo('FillCboContratos', null, null);
        cboContratosBuscar.select2();
    }
    function eventsEventos() {
        fncFillFiltros();

        cboPlantillasBuscar.fillCombo('cboPlantillasBuscar', null, null);
        cboPlantillasBuscar.select2();

        btnBuscarPlantilla.click(function () {
            obtenerPlantillas();
        })
        btnAgregarPlantilla.click(function () {
            addEditPlantilla(txtPlantilla.attr('data-id'));
        });
        btnAgregar.click(function () {
            btnAgregar.attr('data-id', 0);
            addEditDiviciones();
        });
        btnModificar.click(function () {
            addEditDivicionesEdit();
        });
        btnBuscar.click(function () {
            obtenerDiviciones(txtPlantilla.attr('data-id'));
        });
        btnNuevoPlantilla.click(function () {
            $('#txtTituloPlan').text('NUEVA PLANTILLA');
            contenidoElementos.css('display', 'none');
            btnNuevo.css('display', 'none');
            txtPlantilla.attr('data-id', 0);
            txtPlantilla.val('');
            cboContratos.fillCombo('cboObtenerContratos', null, false, null);

        })
        btnNuevo.click(function () {
            inpMensaje.val('');
            inpDescripcion.val('');
            inpOrden.val('');
            inpImportant.val('');
            lstRequerimientos = [];
            inpEvaluador.fillCombo('/ControlObra/ControlObra/getUsuariosAutorizantes?term=""', null, false);
            // AddRows(tblDiviciones,[]);
            AddRows(tblRequerimientos, lstRequerimientos);
        });
        btnGuardarRequerimiento.click(function () {
            let item = {
                id: lstRequerimientos.length,
                texto: inpRequerimiento.val(),
                important: inpImportant.prop('checked'),
            }
            lstRequerimientos.push(item);
            tablaRequerimientos();
            inpRequerimiento.val('');
            inpImportant.prop('checked', false);
        });
        btnGuardarRequerimientoEdit.click(function () {
            let item = {
                id: lstRequerimientosEdit.length,
                texto: inpRequerimientoEdit.val(),
                important: inpImportantEdit.prop('checked'),
            }
            lstRequerimientosEdit.push(item);
            AddRows(tblRequerimientosEdit, lstRequerimientosEdit);
            inpRequerimientoEdit.val('');
            inpImportantEdit.prop('checked', false);
        });
    }
    function getParametrosAgregar() {
        let parametros = {
            idPlantilla: txtPlantilla.attr('data-id'),
            id: btnAgregar.attr('data-id'),
            toltips: inpMensaje.val(),
            idEvaluador: inpEvaluador.val(),
            descripcion: inpDescripcion.val(),
            orden: inpOrden.val(),
            lstRequerimientos: lstRequerimientos
        }
        return parametros;
    }
    function getParametrosEdit() {
        let parametros = {
            id: btnModificar.attr('data-id'),
            toltips: inpMensajeEdit.val(),
            idEvaluador: inpEvaluadorEdit.val(),
            descripcion: inpDescripcionEdit.val(),
            orden: inpOrdenEdit.val(),
            lstRequerimientos: lstRequerimientosEdit
        }
        return parametros;
    }
    function addEditDiviciones() {
        let parametros = getParametrosAgregar();
        axios.post('addEditDiviciones', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    Alert2Exito('Operacion realizada con exito');
                    mdlNuevaDivicion.modal('hide');
                    mdlEditarDivicion.modal('hide');
                    obtenerDiviciones(txtPlantilla.attr('data-id'));
                }
            });
    }
    function addEditDivicionesEdit() {
        let parametros = getParametrosEdit();
        axios.post('addEditDiviciones', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    Alert2Exito('Operacion realizada con exito');
                    mdlNuevaDivicion.modal('hide');
                    mdlEditarDivicion.modal('hide');
                    obtenerDiviciones(txtPlantilla.attr('data-id'));
                }
            });
    }
    function eliminarDiviciones(id) {
        axios.post('eliminarDiviciones', { id: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    if (items.estatus) {
                        obtenerDiviciones(txtPlantilla.attr('data-id'));
                        Alert2Exito(items.mensaje);
                    }

                }
            });
    }
    function obtenerDiviciones(idPlantilla) {
        axios.post('obtenerDiviciones', { idPlantilla: idPlantilla })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    AddRows(tblDiviciones, items)
                }
            });
    }
    function obtenerPlantillas() {
        // function fncAxiosPost() {
        //     axios.post(endpoint, parametros).then(response => {
        //         let { success, items, message } = response.data;
        //         if (success) {
        //             //#region FILL DATATABLE
        //             // dt.clear();
        //             // dt.rows.add(response.data.lst);
        //             // dt.draw();
        //             //#endregion
        //         } else {
        //             Alert2Error(message);
        //         }
        //     }).catch(error => Alert2Error(error.message));
        // }

        axios.post('obtenerPlantillas', { plantillas: cboPlantillasBuscar.val(), contratos: cboContratosBuscar.val() })
            .then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    AddRows(tblPlantillas, items)
                }
            });
    }
    function initTblPlantillas() {
        dtPlantillas = tblPlantillas.DataTable({
            language: dtDicEsp,
            destroy: false,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: false,
            info: false,
            columns: [
                { data: 'id', visible: false },
                { data: 'nombrePlantilla', title: 'PLANTILLA' },
                {
                    data: 'contratos', title: 'CONTRATOS',
                    render: (data, type, row, meta) => {
                        let html = ``;
                        if (data != '') {
                            var arr = data.split(',');
                            arr.forEach(element => {
                                html += `<span class='btn btn-info btn-xs displayCC'><i class='fab fa-creative-commons-nd'>&nbsp;${element}</i></span>`;
                            });
                        }
                        return html;
                    }
                },
                {
                    data: 'id',
                    createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`<button class="btn btn-warning btn-xs EditarRequerimientos" title="Actualizar plantilla."><i class="fas fa-pencil-alt"></i></button>
                                    <button class="btn btn-danger btn-xs EliminarPlantilla" title="Eliminar plantilla."><i class="fa fa-trash"></i></button>`);
                        $(td).find(`button`).attr('data-id', data);
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblPlantillas.on('click', '.EditarRequerimientos', function () {
                    let rowData = dtPlantillas.row($(this).closest('tr')).data();
                    idMdlNuevoPlantilla.modal('show');
                    cboContratos.fillCombo('/ControlObra/ControlObra/cboObtenerContratosInclu?idPlantilla=' + rowData.id, null, false);

                    var roots = rowData.lstId.map(function (num) {
                        return num;
                    });
                    // console.log(roots);
                    cboContratos.val(roots);
                    cboContratos.trigger("change");

                    contenidoElementos.css('display', 'block')
                    btnNuevo.css('display', 'block')

                    txtPlantilla.attr('data-id', rowData.id);
                    txtPlantilla.val(rowData.nombrePlantilla);
                    $('#txtTituloPlan').text('EDITAR PLANTILLA');
                    obtenerDiviciones(rowData.id)
                });
                tblPlantillas.on('click', '.EliminarPlantilla', function () {
                    let rowData = dtPlantillas.row($(this).closest('tr')).data();
                    Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncAccionConfirmar(rowData.id));
                });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                { className: 'dt-body-center', "targets": "_all" },
                { width: "10%", "targets": 3 }
            ],
        });
    }
    function fncAccionConfirmar(param) {
        axios.post('eliminarPlantilla', { id: param }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                obtenerPlantillas();
            }
        }).catch(error => Alert2Error(error.message));
    }
    function initDataTblCatalogo() {
        dtDiviciones = tblDiviciones.DataTable({
            destroy: true,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: true,
            info: false,
            language: dtDicEsp,
            columns: [
                { data: 'descripcion', title: 'DESCRIPCIÓN DE EVALUACIÓN' },
                { data: 'toltips', title: 'MENSAJE DE EVALUACIÓN' },
                {
                    data: 'id',
                    createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`<button class="btn btn-warning btn-xs editarDiviciones" title="Actualizar división."> <i class="fas fa-pencil-alt"></i></button>
                                    <button class="btn btn-danger btn-xs eliminarDiviciones" title="Eliminar división."><i class="fa fa-trash"></i></button>`);
                        $(td).find(`button`).attr('data-id', data);
                    }
                },
            ]
            , initComplete: function (settings, json) {
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
                    btnModificar.attr('data-id', rowData.id)
                    tablaRequerimientosEdit(rowData.id);
                    mdlEditarDivicion.modal("show");
                    abrirModal(rowData);
                });
            }, columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                { className: 'dt-body-center', "targets": "_all" },
                { width: '5%', targets: [2] }
                // { width: "10%", "targets": 2 }
            ],
        });
    }
    function abrirModal(data) {
        inpEvaluadorEdit.fillCombo('/ControlObra/ControlObra/inpEvaluadorinpEvaluador?term=""', null, false);
        inpEvaluadorEdit.val(data.idEvaluador);
        inpEvaluadorEdit.trigger('change');
        // console.log(data)
        inpDescripcionEdit.val(data.descripcion);
        inpMensajeEdit.val(data.toltips);
        inpOrdenEdit.val(data.orden);
        // console.log(data.important)
        inpImportantEdit.prop('checked', false);

    }
    function AddRows(tbl, lst) {
        dtGestionCambios = tbl.DataTable();
        dtGestionCambios.clear().draw();
        dtGestionCambios.rows.add(lst).draw(false);
    }
    function tablaRequerimientos() {
        AddRows(tblRequerimientos, lstRequerimientos);
    }
    function tablaRequerimientosEdit(idDiv) {
        axios.post('obtenerRequerimientos', { idDiv: idDiv })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    lstRequerimientosEdit = items;
                    AddRows(tblRequerimientosEdit, lstRequerimientosEdit);
                }
            });
    }
    function initDatatblRequerimientos() {
        dtRequerimientos = tblRequerimientos.DataTable({
            destroy: true,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: true,
            info: false,
            language: dtDicEsp,
            columns: [
                { data: 'texto', title: 'REQUERIMIENTO' },
                { data: 'important', title: 'Opcional', visible: false },
                {
                    data: 'id', title: '',
                    createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`
                            <button class="btn btn-danger btn-xs eliminarRequerimientos" title="Eliminar requerimiento."> <i class="fas fa-trash"></i></button>`);
                        $(td).find(`button`).attr('data-id', data);
                    }
                },
            ]
            , initComplete: function (settings, json) {
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
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                { width: '5%', targets: [1] }
            ],
        });
    }
    function initDatatblRequerimientosEdit() {
        dtRequerimientosEdit = tblRequerimientosEdit.DataTable({
            destroy: true,
            paging: false,
            ordering: false,
            searching: false,
            bFilter: true,
            info: false,
            language: dtDicEsp,
            columns: [
                { data: 'texto', title: 'REQUERIMIENTO' },
                {
                    data: 'important', title: 'Opcional', visible: false,
                    createdCell: function (td, data, rowData, row, col) {
                        if (data == true) {
                            $(td).html(`<input class="form-control" type="checkbox" disabled="enabled"/>`);
                            $(td).find(`input`).prop('checked', 'true');
                        } else {
                            $(td).html(`<input class="form-control" type="checkbox" disabled="enabled"/>`);
                            $(td).find(`input`).prop('checked', false);
                        }
                    }
                },
                {
                    data: 'id',
                    createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`
                            <button class="btn btn-danger btn-xs eliminarRequerimientosEdit" title="Eliminar requerimiento."><i class="fas fa-trash"></i></button>`);
                        $(td).find(`button`).attr('data-id', data);
                    }
                },
            ],
            initComplete: function (settings, json) {
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
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' },
                { width: '5%', targets: [1] }
            ],
        });
    }
    function eliminarRequerimientos(id) {
        // console.log(lstRequerimientos)
        // console.log(lstRequerimientos)
        var removed = lstRequerimientos.splice(lstRequerimientos.indexOf(id), 1);
        AddRows(tblRequerimientos, lstRequerimientos);
    }
    function eliminarRequerimientosEdit(id) {
        // console.log(lstRequerimientosEdit)
        // console.log(lstRequerimientosEdit.indexOf(id))
        var removed = lstRequerimientosEdit.splice(lstRequerimientosEdit.indexOf(id), 1);
        AddRows(tblRequerimientosEdit, lstRequerimientosEdit);
    }
    function addEditPlantilla(idPlantilla) {
        let lstContratos = '';
        var a = getValoresMultiples('#cboContratos');
        // console.log(a);

        a.forEach(x => {
            if (x != '') {
                lstContratos += x + ',';
            }
        });
        let parametros = {
            nombrePlantilla: txtPlantilla.val(),
            idPlantilla: idPlantilla,
            contratos: lstContratos
        }
        axios.post('addEditPlantilla', { objDTO: parametros }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                idMdlNuevoPlantilla.modal('hide');
                obtenerPlantillas();
                fncFillFiltros();
            } else {
                Alert2Warning(message);
            }
        }).catch(error => Alert2Error(error.message));
    }

    $(document).ready(() => {
        CatalogoDeDiviciones.EvaluacionSubcontratista = new EvaluacionSubcontratista();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();