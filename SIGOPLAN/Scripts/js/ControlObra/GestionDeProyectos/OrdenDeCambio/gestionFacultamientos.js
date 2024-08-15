(() => {
    $.namespace('gestionFacultamientos.ControlObra');

    const tblGestionDeFirmas = $('#tblGestionDeFirmas');
    let dttblGestionDeFirmas;
    const tblGestionDeFirmasSubcontratista = $('#tblGestionDeFirmasSubcontratista');
    let dttblGestionDeFirmasSubcontratista;
    const cboProyecto = $('#cboProyecto');
    const btnBuscar = $('#btnBuscar');
    const btnBuscarSubcontratista = $('#btnBuscarSubcontratista');

    const mdlGestionFacultamiento = $('#mdlGestionFacultamiento');
    const btnNuevo = $('#btnNuevo');
    const inpClaveEmpleado = $('#inpClaveEmpleado');
    const inpUsuario = $('#inpUsuario');
    const cboCCMultiple = $('#cboCCMultiple');
    const cboPrivilegio = $('#cboPrivilegio');
    const btnGuardar = $('#btnGuardar');

    const modalNuevoSubcontratista = $('#modalNuevoSubcontratista');
    const btnNuevoSubcontratista = $('#btnNuevoSubcontratista');
    const cboSubcontratista = $('#cboSubcontratista');
    const inputSubcontratista = $('#inputSubcontratista');
    const inputNombreCompleto = $('#inputNombreCompleto');
    const inputFirmante = $('#inputFirmante');
    const inputCorreo = $('#inputCorreo');
    const inputContrato = $('#inputContrato');
    const btnGuardarSubcontratista = $('#btnGuardarSubcontratista');
    // const _PRIVILEGIO = {
    //     ADMINISTRADOR: 1,
    //     GESTIOR_OC: 2,
    //     VISOR: 3,
    //     INTERESADOS: 4

    // }
    const _ESTATUS = {
        ACTIVO: 1,
        DESACTIVADO: 0
    }

    ControlObra = function () {
        let init = () => {
            console.log('hola')
            initDataTbltblGestionDeFirmas();
            // initDataTbltblGestionDeFirmasSubcontratistas();
            eventListeners();
            // obtenerPermisos();
        }
        init();
    }
    function eventListeners(params) {
        cboProyecto.fillCombo('getProyecto', null, false, null);
        // cboSubcontratista.fillCombo('getSubcontratista', null, false, null);
        // convertToMultiselect('#cboProyecto');
        btnNuevo.click(function () {
            mdlGestionFacultamiento.modal('show');
            btnGuardar.text('Guardar');
            btnGuardar.attr('data-id', 0);
            inpClaveEmpleado.val('');
            inpUsuario.val('');
            cboCCMultiple.val('');
            cboCCMultiple.trigger("change");
            cboPrivilegio.val('');
        });
        btnNuevoSubcontratista.click(function () {
            modalNuevoSubcontratista.modal('show');
            btnGuardarSubcontratista.text('GuardarSub');
            btnGuardarSubcontratista.attr('data-id', 0);
            inputSubcontratista.val('');
            inputNombreCompleto.val('');
            inputContrato.val('');
            inputFirmante.val('');
            inputCorreo.val('');
        });
        btnBuscar.click(function () {
            obtenerLstFacultamientos(cboProyecto.val());
        })
        btnBuscarSubcontratista.click(function () {
            obtenerLstSubcontratistas(cboSubcontratista.val());
        })
        // inpUsuario.fillCombo('obtenerUsuarios', null, false, null);
        inpClaveEmpleado.getAutocompleteValid(setDatosUsuario, verificarUsuario, { porClave: true }, 'GetUsuariosAutocomplete');
        inpUsuario.getAutocompleteValid(setDatosUsuario, verificarUsuario, { porClave: false }, 'GetUsuariosAutocomplete');
        cboCCMultiple.fillCombo('obtenerCC', null, false, null);
        cboCCMultiple.find('option[value=""]').remove();
        // cboPrivilegio.fillCombo('obtenerPrivilegios', null, false, null);
        // convertToMultiselect('#cboCCMultiple');
        btnGuardar.click(function () {
            guardarOEditar();
        })
        btnGuardarSubcontratista.click(function () {
            guardarOEditarSubcontratista();
        })
    }
    function setDatosUsuario(e, ui) {
        inpClaveEmpleado.val(ui.item.id);
        inpUsuario.val(ui.item.nombre);

    }

    function verificarUsuario(e, ui) {
        if (ui.item == null) {
            inpUsuario.val('');
        }
    }
    function obtenerPermisos() {
        axios.post('obtenerPermisos')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    console.log(response)
                    if (items != null) {
                        items.forEach(x => {

                            if (x.permiso = "ADMINISTRADOR") {
                                btnNuevo.prop("disabled", false);
                            } else if (x.permiso = "GESTOR OC") {
                                btnNuevo.prop("disabled", false);
                                btnBuscar.prop("disabled", false);
                            } else if (x.permiso = "VISOR") {
                                btnNuevo.prop("disabled", true);
                            } else if (x.permiso = "INTERESADOS") {
                                btnNuevo.prop("disabled", true);

                            }
                        });
                    }
                }
            });
    }

    //#region Metodos Facultamiento
    function AddRows(tbl, lst) {
        dttblGestionDeFirmas = tbl.DataTable();
        dttblGestionDeFirmas.clear().draw();
        dttblGestionDeFirmas.rows.add(lst).draw(false);
    }
    function initDataTbltblGestionDeFirmas() {
        dttblGestionDeFirmas = tblGestionDeFirmas.DataTable({
            destroy: true
            , language: dtDicEsp
            , columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'idEmpleado', title: 'id', visible: false },
                { data: 'nombreEmpleado', title: 'Nombre Completo' },
                // {
                //     data: 'privilegio', title: 'Privilegio',
                //     render: function (data, type, row) {
                //         if (data == _PRIVILEGIO.ADMINISTRADOR) {
                //             return 'ADMINISTRADOR';
                //         }
                //         if (data == _PRIVILEGIO.GESTIOR_OC) {
                //             return 'GESTOR OC'
                //         }
                //         if (data == _PRIVILEGIO.VISOR) {
                //             return 'VISOR';
                //         }
                //         if (data == _PRIVILEGIO.INTERESADOS) {
                //             return 'INTERESADOS';
                //         }
                //     }

                // },
                // { data: 'lstcc', title: 'cc' },
                { data: 'privilegioDesc', title: 'privilegio' },
                {
                    data: 'ccDescripcion', title: 'Centros de costos', render: (data, type, row, meta) => {
                        let html = '';

                        data.forEach(x => {
                            html += `<span class='btn btn-primary btn-sm displayCC'><i class='fab fa-creative-commons-nd'>${x}</i></span></br>`;

                        });

                        return html;
                    }
                },
                {
                    data: 'estatus', title: 'Estatus', visible: false,
                    render: function (data, type, row) {
                        if (data == _ESTATUS.ACTIVO) {
                            return 'ACTIVO';
                        }
                        if (data == _ESTATUS.DESACTIVADO) {
                            return 'DESACTIVADO'
                        }
                    }
                },
                {
                    title: 'Opciones', render: (data, type, row, meta) => {
                        let html = '';
                        html += `<button title='Editar Orden de cambio' type="" class="btn btn-success btn-sm editarGestionFirmas"><span class="glyphicon glyphicon-pencil"></span></button> `;
                        html += `<button title='Eliminar Orden de cambio' type="" class="btn btn-danger btn-sm eliminarGestionFirmas"><span class="glyphicon glyphicon-trash"></span></button> `;
                        return html;
                    }
                },
            ],
            columnDefs: [
                { width: '20%', targets: 1 },
                { width: '20%', targets: 2 },
                { width: '20%', targets: 3 },
                { width: '20%', targets: 4 },
                { width: '5%', targets: 5 },
                { width: '5%', targets: 6 },

            ]
            , initComplete: function (settings, json) {
                tblGestionDeFirmas.on("click", ".editarGestionFirmas", function () {
                    const rowData = dttblGestionDeFirmas.row($(this).closest("tr")).data();
                    mdlGestionFacultamiento.modal('show');
                    btnGuardar.text('Editar');
                    btnGuardar.attr('data-id', rowData.id);
                    llenarCamposVacios(rowData);
                });
                tblGestionDeFirmas.on("click", ".eliminarGestionFirmas", function () {
                    const rowData = dttblGestionDeFirmas.row($(this).closest("tr")).data();
                    let esActivo = $(this).attr("data-esActivo");
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
                            fncEliminar(rowData.id, esActivo);
                        }
                    });
                });




            }
        });
    }

    function fncEliminar(id) {
        axios.post('EliminarFacultamiento', { idFacultamiento: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    obtenerLstFacultamientos(cboProyecto.val());
                }
            });
    }
    function llenarCamposVacios(rowData) {
        var roots = rowData.lstcc.map(function (num) {
            return num;
        });
        console.log(roots);
        cboCCMultiple.val(roots);
        cboCCMultiple.trigger("change");

        inpUsuario.val(rowData.nombreEmpleado);
        inpUsuario.trigger("change");

        cboPrivilegio.val(rowData.privilegio);
        cboPrivilegio.trigger("change");
    }

    function obtenerLstFacultamientos(cc) {
        axios.post('obtenerLstFacultamientos', { cc: cc })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    console.log(items)
                    AddRows(tblGestionDeFirmas, items.items);
                }
            });
    }
    function getParametros() {
        var a = getValoresMultiples('#cboCCMultiple');
        console.log(a);
        let cc = '';

        a.forEach(x => {
            if (x != '') {
                cc += x + ',';
            }
        });

        let parametros = {
            idEmpleado: inpClaveEmpleado.val(),
            cc: cc,
            privilegio: cboPrivilegio.val(),
            id: btnGuardar.attr('data-id')
        };

        return parametros;
    }
    function guardarOEditar() {
        let parametros = getParametros();
        axios.post('agregarEditarFacultamientos', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    mdlGestionFacultamiento.modal('hide');
                    obtenerLstFacultamientos(cboProyecto.val());
                }
            });
    }
    //#endregion

    //#region Metodos Subcontratistas

    function AddRowsSub(tbl, lst) {
        dttblGestionDeFirmasSubcontratista = tbl.DataTable();
        dttblGestionDeFirmasSubcontratista.clear().draw();
        dttblGestionDeFirmasSubcontratista.rows.add(lst).draw(false);
    }

    function initDataTbltblGestionDeFirmasSubcontratistas() {
        dttblGestionDeFirmasSubcontratista = tblGestionDeFirmasSubcontratista.DataTable({
            destroy: true
            , language: dtDicEsp
            , columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'subContratista', title: 'SubContratista' },
                { data: 'nombreCompleto', title: 'NombreCompleto', visible: false },
                { data: 'contrato', title: 'Contrato' },
                { data: 'nombreFirmante', title: 'Firmante' },
                { data: 'correo', title: 'Correo' },

                {
                    data: 'id', title: 'Opciones', render: (data, type, row, meta) => {
                        let html = '';
                        html += `<button title='Editar Orden de cambio' type="" class="btn btn-success btn-sm editarGestionFirmasSubcontratista"><span class="glyphicon glyphicon-pencil"></span></button> `;
                        html += `<button title='Eliminar Orden de cambio' type="" class="btn btn-danger btn-sm eliminarGestionFirmasSubcontratista"><span class="glyphicon glyphicon-trash"></span></button> `;
                        return html;
                    }
                },
            ],
            columnDefs: [
                { width: '35%', targets: 1 },
                { width: '30%', targets: 2 },
                { width: '30%', targets: 3 },
                { width: '10%', targets: 4 },

            ]
            , initComplete: function (settings, json) {
                tblGestionDeFirmasSubcontratista.on("click", ".editarGestionFirmasSubcontratista", function () {
                    const rowData = dttblGestionDeFirmasSubcontratista.row($(this).closest("tr")).data();
                    modalNuevoSubcontratista.modal('show');
                    btnGuardarSubcontratista.text('Editar');
                    btnGuardarSubcontratista.attr('data-id', rowData.id);
                    llenarCamposVaciosSubcontratista(rowData);
                });
                tblGestionDeFirmasSubcontratista.on("click", ".eliminarGestionFirmasSubcontratista", function () {
                    const rowData = dttblGestionDeFirmasSubcontratista.row($(this).closest("tr")).data();
                    let esActivo = $(this).attr("data-esActivo");
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
                            fncEliminarSubcontratista(rowData.id, esActivo);
                        }
                    });
                });
            }
        });
    }

    function fncEliminarSubcontratista(id) {
        axios.post('EliminarSubcontratista', { id: id })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    obtenerLstFacultamientos(cboProyecto.val());
                }
            });
    }

    function llenarCamposVaciosSubcontratista(rowData) {

        inputSubcontratista.val(rowData.subContratista);
        inputNombreCompleto.val(rowData.inputNombreCompleto);
        inputContrato.val(rowData.contrato);
        inputCorreo.val(rowData.correo);
    }

    function obtenerLstSubcontratistas(idSub) {
        axios.post('obtenerLstSubcontratistas', { id: idSub })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    console.log(items)
                    AddRowsSub(tblGestionDeFirmasSubcontratista, items.items);
                }
            });
    }

    function getParametrosSubcontratista() {
        var a = getValoresMultiples('#cboSubcontratista');
        console.log(a);
        let cc = '';

        a.forEach(x => {
            if (x != '') {
                cc += x + ',';
            }
        });

        let parametros = {

            Subcontratista: inputSubcontratista.val(),
            nombreCompleto: inputNombreCompleto.val(),
            contrato: inputContrato.val(),
            correo: inputCorreo.val(),
            id: btnGuardarSubcontratista.attr('data-id')
        };

        return parametros;
    }

    function guardarOEditarSubcontratista() {
        let parametros = getParametrosSubcontratista();
        axios.post('agregarEditarSubcontratista', { parametros: parametros })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items } = response.data;
                if (success) {
                    modalNuevoSubcontratista.modal('hide');
                    obtenerLstSubcontratistas(cboSubcontratista.val());
                }
            });
    }

    //#endregion



    $(document).ready(() => {
        gestionFacultamientos.ControlObra = new ControlObra();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();



