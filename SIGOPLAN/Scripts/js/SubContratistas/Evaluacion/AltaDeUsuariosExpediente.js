(() => {
    $.namespace('AltaDeUsuariosExpediente.EvaluacionSubcontratista');

    var objUsuario = {
        id: '',
        _user: '',
        nombre_completo: '',
        estatus: '',
        _pass: '',
        correo: '',
        tipo: '',
        idPadre: '',
        cc: '',
        nombrePadre: '',
        idContrato: '',
    };
    const btnBuscar = $('#btnBuscar');
    const tblUsuarios = $('#tblUsuarios');
    let dtUsuarios
    const mdlAgregarEditar = $('#mdlAgregarEditar');
    const lblTextoTitulo = $('#lblTextoTitulo');
    const btnAgregar = $('#btnAgregar');
    const btnGuardar = $('#btnGuardar');
    const dboPrestadorDeServicio = $('#dboPrestadorDeServicio');
    const inpNombreCompleto = $('#inpNombreCompleto');
    const inpUsuario = $('#inpUsuario');
    const inpPassword = $('#inpPassword');
    const inpCorreo = $('#inpCorreo');


    const cboProyecto2 = $('#cboProyecto2');
    const cboContrato2 = $('#cboContrato2');
    const cboSubcontratista = $('#cboSubcontratista');
    const cboContrato3 = $('#cboContrato3');
    const cbocc3 = $('#cbocc3');

    EvaluacionSubcontratista = function () {
        (function init() {
            fncListeners();
            objUsuario.cc = cboProyecto2.val() == null ? "" : cboProyecto2.val();
            objUsuario.idContrato = cboContrato2.val() == null ? 0 : cboContrato2.val();
            objUsuario.idPadre = cboSubcontratista.val() == null ? 0 : cboSubcontratista.val();
            console.log(cboProyecto2.val())
            console.log(cboContrato2.val())
            console.log(cboSubcontratista.val())
            console.log(objUsuario)
            AccionesUsuariosExpediente(4);
        })();

        function fncListeners() {
            initTblUsuarios();
            dboPrestadorDeServicio.fillCombo('fillComboPrestadoresDeServicio', null, false, null);

            cboProyecto2.fillCombo('cboProyecto', null, false, null);
            cboContrato2.fillCombo('cboContratosBuscar', null, false, null);
            cboSubcontratista.fillCombo('cboSubcontratistas', null, false, null);

            dboPrestadorDeServicio.change(function () {
                console.log(dboPrestadorDeServicio.val())
                let obt = dboPrestadorDeServicio.val() == "" ? 0 : dboPrestadorDeServicio.val();
                let url2 = 'cboProyecto3?idSubcontratista=' + obt;
                let url3 = 'cboContratosBuscar3?idSubcontratista=' + obt;
                console.log(url2)
                cbocc3.fillCombo(url2, null, false);
                cboContrato3.fillCombo(url3, null, false, null);
            })

            $(`.select2`).select2();
            $(`.select2`).select2({ width: '100%' });
            btnBuscar.click(function () {

                objUsuario.cc = cboProyecto2.val() == null ? "" : cboProyecto2.val();
                objUsuario.idContrato = cboContrato2.val() == null ? 0 : cboContrato2.val();
                objUsuario.idPadre = cboSubcontratista.val() == null ? 0 : cboSubcontratista.val();
                console.log(objUsuario)
                AccionesUsuariosExpediente(4)
            });
            btnAgregar.click(function () {
                dboPrestadorDeServicio.val('');
                dboPrestadorDeServicio.trigger('change');
                inpNombreCompleto.val('');
                inpUsuario.val('');
                inpPassword.val('');
                inpCorreo.val('');
                btnGuardar.attr('data-idPrestador', 0);
                btnGuardar.attr('data-accion', 1);
                mdlAgregarEditar.modal('show');
                lblTextoTitulo.text('NUEVO USUARIO');
                inpUsuario.prop('disabled', false);
                dboPrestadorDeServicio.prop('disabled', false);
            });
            btnGuardar.click(function () {

                objUsuario.id = btnGuardar.attr('data-idPrestador');
                objUsuario._user = inpUsuario.val();
                objUsuario.nombre_completo = inpNombreCompleto.val();
                objUsuario.estatus = true;
                objUsuario._pass = inpPassword.val();
                objUsuario.correo = inpCorreo.val();
                objUsuario.tipo = 14;
                objUsuario.idPadre = dboPrestadorDeServicio.val();
                objUsuario.cc = cbocc3.val();
                objUsuario.idContrato = cboContrato3.val();
                console.log(dboPrestadorDeServicio.val())
                console.log(inpUsuario.val())
                console.log(inpNombreCompleto.val())
                console.log(inpPassword.val())
                console.log(inpCorreo.val())
                if (inpNombreCompleto.val() == '' || inpUsuario.val() == '' || dboPrestadorDeServicio.val() == '' || inpPassword.val() == '' || inpCorreo.val() == '') {
                    Alert2Warning('favor de llenar correctamente todos los campos.');
                } else {
                    if (btnGuardar.attr('data-accion') == "1") {
                        AccionesUsuariosExpediente(1);
                    } else {
                        AccionesUsuariosExpediente(2);
                    }
                }

            });
        }
        function initTblUsuarios() {
            dtUsuarios = tblUsuarios.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombrePadre', title: 'PRESTADOR DE SERVICIO' },
                    { data: 'nombre_completo', title: 'NOMBRE' },
                    { data: '_user', title: 'USUARIO' },
                    { data: 'correo', title: 'CORREO' },
                    {
                        render: (data, type, row, meta) => {
                            let html = ``;
                            html += `<button class='btn btn-warning btn-xs EditarUsusarios'  data-id="${row.id}"><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            html += `<button class='btn btn-danger btn-xs EliminarUsusarios'  data-id="${row.id}"><i class="fas fa-trash"></i></button>`;
                            return html;
                        }
                    },
                    //render: function (data, type, row) { }
                ],
                initComplete: function (settings, json) {
                    tblUsuarios.on('click', '.EditarUsusarios', function () {
                        let rowData = dtUsuarios.row($(this).closest('tr')).data();
                        btnGuardar.attr('data-idPrestador', rowData.id);
                        btnGuardar.attr('data-accion', 2);
                        mdlAgregarEditar.modal('show');
                        lblTextoTitulo.text('ACTUALIZAR USUARIO');
                        dboPrestadorDeServicio.val(rowData.idPadre);
                        dboPrestadorDeServicio.trigger('change');
                        inpNombreCompleto.val(rowData.nombre_completo);
                        inpUsuario.val(rowData._user);
                        inpPassword.val(rowData._pass);
                        inpCorreo.val(rowData.correo);
                        cbocc3.val(rowData.cc);
                        cboContrato3.val(rowData.idContrato);
                        inpUsuario.prop('disabled', true);
                        dboPrestadorDeServicio.prop('disabled', true);

                    });
                    tblUsuarios.on('click', '.EliminarUsusarios', function () {
                        let rowData = dtUsuarios.row($(this).closest('tr')).data();
                        objUsuario.id = rowData.id;
                        objUsuario.estatus = false;

                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => AccionesUsuariosExpediente(3));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }
        function AccionesUsuariosExpediente(Accion) {
            axios.post('AccionesUsuariosExpediente', { objUsuario: objUsuario, Accion: Accion }).then(response => {
                let { success, items, message } = response.data;
                if (Accion == 4) {
                    if (success) {
                        AddRows(tblUsuarios, items);
                    } else {
                        Alert2Error(message);
                    }
                } else if (Accion == 1 || Accion == 2) {
                    if (success) {
                        Alert2Exito(message);
                        mdlAgregarEditar.modal('hide');
                        AccionesUsuariosExpediente(4);
                    } else {
                        Alert2Error(message);
                    }
                } else {
                    if (success) {
                        Alert2Exito(message);
                        AccionesUsuariosExpediente(4);
                    } else {
                        Alert2Error(message);
                    }
                }
                mdlAgregarEditar.modal("hide");
            }).catch(error => Alert2Error(error.message));
        }
        function AddRows(tbl, lst) {
            dtUsuarios = tbl.DataTable();
            dtUsuarios.clear().draw();
            dtUsuarios.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => {
        AltaDeUsuariosExpediente.EvaluacionSubcontratista = new EvaluacionSubcontratista();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();