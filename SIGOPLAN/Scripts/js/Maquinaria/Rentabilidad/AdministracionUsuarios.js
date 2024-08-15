(
    () => {
        $.namespace('Maquinaria.Rentabilidad.CatalogoDivision');
        CatalogoDivision = function () {
            //Variables.
            let d = new Date();
            let dtTablaResponsableUsuarios;
            let reponsableID;

            //#region Selectores HTML
            const botonUsuarioResponsable = $('#botonUsuarioResponsable');
            const tablaUsuarioReponsables = $('#tablaUsuarioReponsables');
            const modalNuevoReponsable = $('#modalNuevoReponsable');
            const lblDescripcionModal = $('#lblDescripcionModal');
            const comboEstatus = $('#comboEstatus');
            const comboAC = $('#comboAC');
            const botonGuardarCambios = $('#botonGuardarCambios');
            const comboUsuarios = $('#comboUsuarios');
            const comboFiltroEstatus = $('#comboFiltroEstatus');
            //Eliminar Modal
            const botonEliminar = $('#botonEliminar');
            const lblEliminar = $('#lblEliminar');
            const modalEliminar = $('#modalEliminar');
            const comboEstatusFiltro = $(("#comboEstatusFiltro"));
            //#endregion
            //Inicializaciones
            (function int() {

                comboAC.fillCombo('/Rentabilidad/cboAreaCuenta', null, true);
                comboAC.select2();
                initTablaUsuarioResponsables();
                cargarUsuariosReponsables();
                addListeners();
                setDefault();
                cargarComboUsuarios();

            })();
            function cargarComboUsuarios() {
                $.get('/Rentabilidad/getListaUsuarios')
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            comboUsuarios.select2({
                                data: response.items
                            });
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }
            function initTablaUsuarioResponsables() {
                dtTablaResponsableUsuarios = tablaUsuarioReponsables.DataTable({
                    language: dtDicEsp,
                    destroy: true,
                    paging: false,
                    searching: false,
                    columns: [
                        { data: 'usuarioNombre', title: 'Descripcion' },
                        {
                            data: 'areaCuenta', title: 'Áreas Cuenta', render: (data, type, row) => {
                                let html = "";
                                row.areaCuenta.forEach(dato => {
                                    html += `<button class="btn btn-success displayAreaCuenta"><i class="fab fa-creative-commons-nd"></i> ${dato.areaCuenta}</button>`;
                                }
                                );
                                return html;
                            }
                        },
                        { data: 'id', render: (data, type, row) => `<button class="btn btn-success editar" data-id="${data}"><i class="fas fa-tools"></i> editar</button>` },
                        { data: 'id', render: (data, type, row) => row.estatus ? `<button class="btn btn-danger eliminar" data-id="${data}"><i class="fas fa-trash"></i> eliminar</button>` : '' }
                    ],
                    columnDefs: [
                        { className: "dt-center", "targets": "_all" }
                    ],
                    drawCallback: function (settings) {

                        tablaUsuarioReponsables.find('.editar').click(function () {
                            let ID = $(this).attr('data-id');
                            reponsableID = ID;
                            $.get('/Rentabilidad/getReponsableByID', { id: ID })
                                .then(response => {
                                    if (response.success) {
                                        // Operación exitosa.
                                        limpiarModal();
                                        comboEstatus.val(response.estatus);
                                        comboUsuarios.val(response.reponsableUsuario).prop('disabled', true);
                                        comboUsuarios.trigger('change');
                                        let listaAC = response.areaCuenta;
                                        comboAC.val(listaAC);
                                        comboAC.trigger('change');
                                        modalNuevoReponsable.modal('show');
                                    } else {
                                        // Operación no completada.
                                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                                    }
                                }, error => {
                                    // Error al lanzar la petición.
                                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                }
                                );
                        });

                        tablaUsuarioReponsables.find('.eliminar').click(function () {
                            let ID = $(this).attr('data-id');
                            reponsableID = ID;
                            lblEliminar.text('¿Estas seguro que deseas dar de baja el usuario?')
                            modalEliminar.modal('show');
                        });

                    }
                });
            }

            function bajaUsuario() {

                $.get('/Rentabilidad/bajaUsuario', { id: reponsableID })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            setDefault();
                            modalEliminar.modal('hide');
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }

            async function cargarUsuariosReponsables() {
                try {

                    $.get('/Rentabilidad/GetInfoAdministracionUsuarios', { estatus: comboFiltroEstatus.val() })
                        .then(response => {
                            if (response.success) {
                                if (dtTablaResponsableUsuarios != null) {
                                    dtTablaResponsableUsuarios.clear().draw();
                                    dtTablaResponsableUsuarios.rows.add(response.listaResposanbles).draw();
                                }
                            }
                            else
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        },
                            error => {
                                AlertaGeneral(`Operación fallida`, `Ocurrió un error al enviar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                            });
                } catch (e) { AlertaGeneral(`Operación fallida`, e.message) }
            }

            function limpiarModal() {

                comboUsuarios.val('');
                comboEstatus.val('True');
                comboAC.val(null).trigger('change');
                comboUsuarios.val(null).trigger('change');
                comboUsuarios.prop('disabled', false);
            }

            function addListeners() {
                botonGuardarCambios.click(GuardarReponsables);
                botonUsuarioResponsable.click(() => {
                    lblDescripcionModal.text('Alta de usuarios');

                    limpiarModal();
                    modalNuevoReponsable.modal('show');
                });
                comboFiltroEstatus.change(cargarUsuariosReponsables);
                botonEliminar.click(bajaUsuario);
            }

            function GuardarReponsables() {

                let objReponsables = setInfoReponsables();

                $.post('/Rentabilidad/SaveOrUpdateAdministacionUsuarios', { nuevoUsuario: objReponsables })
                    .then(response => {
                        if (response.success) {
                            // Operación exitosa.
                            modalNuevoReponsable.modal('hide');
                            limpiarModal();
                            AlertaGeneral(`Operación Exitosa`, `Los registros se han actualizado correctamente`);
                            setDefault();
                            cargarUsuariosReponsables();
                        } else {
                            // Operación no completada.
                            AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        }
                    }, error => {
                        // Error al lanzar la petición.
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            }

            function setInfoReponsables() {

                let usuarioResponsableID = comboUsuarios.select2('data')[0];//comboUsuarios.data().id;
                let estatus = comboEstatus.val();
                if (comboUsuarios.select2('data') == 0)
                    return AlertaGeneral('Alerta', 'No se Capturo el nombre de usuario, Favor de Capturar para poder continuar con el guardado.')
                if (comboAC.val().length == 0)
                    return AlertaGeneral('Alerta', 'No se asignó ningun area cuenta para este usuario, favor de agregar por lo menos una division para continuar.');

                let listaDetalle = comboAC.val().map(x => {
                    let objInfo = {
                        id: 0,
                        areaCuentaID: x,
                        usuarioResponsableID: reponsableID,
                        estatus: true,
                    }
                    return objInfo;
                });

                return {
                    id: reponsableID,
                    usuarioResponsableID: usuarioResponsableID.id,
                    estatus: estatus,
                    fechaCreacion: `${d.getDate()}/${d.getMonth()}/${d.getFullYear()}`,
                    usuarioCreadorID: 0,
                    areaCuentaResponsable: listaDetalle
                }
            }

            function setDefault() {
                reponsableID = 0;
            }
        }
        $(() => Maquinaria.Rentabilidad.CatalogoDivision = new CatalogoDivision())
            .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
            .ajaxStop($.unblockUI);
    })();