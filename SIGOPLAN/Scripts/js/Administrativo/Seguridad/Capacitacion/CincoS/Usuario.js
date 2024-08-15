(() => {
    $.namespace('Administrativo.CincoS');
    CincoS = function () {
        //#endregion ELEMENTOS
        const cboFiltroCC = $('#cboFiltroCC');
        const btnFiltroNuevo = $('#btnFiltroNuevo');
        const btnFiltroBuscar = $('#btnFiltroBuscar');

        const tblAuditores = $('#tblAuditores');

        const mdlNuevoAuditor = $('#mdlNuevoAuditor');
        const tituloNuevoAuditor = $('#tituloNuevoAuditor');
        const txtNuevoAuditor = $('#txtNuevoAuditor');
        const cboNuevoCC = $('#cboNuevoCC');
        const txtNuevoPuesto = $('#txtNuevoPuesto');
        const txtNuevoProyecto = $('#txtNuevoProyecto');
        const btnGuardarAuditor = $('#btnGuardarAuditor');

        const cboFiltroPrivilegio = $('#cboFiltroPrivilegio');
        const tblFacultamientos = $('#tblFacultamientos');
        const mdlFacultamiento = $('#mdlFacultamiento');
        const txtFacultamientoAuditor = $('#txtFacultamientoAuditor');
        const txtFacultamientoPuesto = $('#txtFacultamientoPuesto');
        const cboFacultamientoPrivilegio = $('#cboFacultamientoPrivilegio');
        const btnGuardarFacultamiento = $('#btnGuardarFacultamiento');

        const btnTabLider = $('#btnTabLider');
        const tblLideres = $('#tblLideres');
        const mdlLider5s = $('#mdlLider5s');
        const txtLider = $('#txtLider');
        const cboLiderCCs = $('#cboLiderCCs');
        const cboLiderArea = $('#cboLiderArea');
        const btnGuardarLider = $('#btnGuardarLider');

        const btnTabSubArea = $('#btnTabSubArea');
        const btnSubAreaNuevo = $('#btnSubAreaNuevo');
        const tblSubAreas = $('#tblSubAreas');
        const mdlSubArea = $('#mdlSubArea');
        const txtSubArea = $('#txtSubArea');
        const btnGuardarSubArea = $('#btnGuardarSubArea');
        //#endregion

        const CCConsultaEnum = {
            Todos: 0,
            TodosLosActivos: 1,
            TodosConCheckListCreado: 2,
            LosDelCheckList: 3,
            TodosConAuditorCreado: 4
        };

        //#region EVENTOS
        btnFiltroBuscar.on('click', function () {
            getAuditores();
        });

        btnFiltroNuevo.on('click', function () {
            tituloNuevoAuditor.text('Nuevo usuario');
            mdlNuevoAuditor.modal('show');
        });

        mdlNuevoAuditor.on('hidden.bs.modal', function () {
            limpiarModalNuevoAuditor();
        });

        txtNuevoAuditor.getAutocomplete(selectAutocompleteUsuario, null, 'GetUsuario');

        btnGuardarAuditor.on('click', function () {
            if ($("#btnGuardarAuditor").data('idAuditor')) {
                swal({
                    title: 'Alerta!',
                    text: '¿Desea continua?,Si quita CCs al usuario y el usuario esta agregado como líder de área en un checklist y el checklist corresponde a un cc que se esta quitando, el usuario sera eliminado como líder de área del checklist.',
                    icon: 'warning',
                    buttons: true,
                    dangerMode: false,
                    buttons: ['Cerrar', 'Aceptar']
                })
                    .then((aceptar) => {
                        if (aceptar) {
                            editarAuditor(crearObjetoAuditor());
                        }
                    });
            } else {
                guardarAuditor(crearObjetoAuditor());
            }
        });

        cboFiltroPrivilegio.on('change', function () {
            getFacultamientos();
        });

        mdlFacultamiento.on('hidden.bs.modal', function () {
            limpiarModalFacultamiento();
        });

        btnGuardarFacultamiento.on('click', function () {
            guardarAuditorPrivilegio(crearObjetoFacultamiento());
        });

        btnTabLider.on('click', function () {
            getTablaLideres(cboFiltroCC.val());
        });

        mdlLider5s.on('hidden.bs.modal', function () {
            limpiarModalLider();
        });

        btnGuardarLider.on('click', function () {
            guardarLider(crearObjetoLider());
        });

        btnTabSubArea.on('click', function () {
            getTablaSubAreas();
        });

        btnGuardarSubArea.on('click', function () {
            if ($(this).data('id')) {
                editarSubArea(btnGuardarSubArea.data('id'), txtSubArea.val());
            } else {
                GuardarSubArea(txtSubArea.val());
            }
        });

        btnSubAreaNuevo.on('click', function () {
            btnGuardarSubArea.data('id', '');

            mdlSubArea.modal('show');
        });

        mdlSubArea.on('hidden.bs.modal', function () {
            txtSubArea.val('');
        });
        //#endregion

        (function init() {
            initCbox();
            initTblAuditores();
            initTblFacultamientos();
            initTblLideres();
            initTblSubAreas();
        })();

        //#endregion CBOX
        function initCbox() {
            cboFiltroCC.fillCombo('GetCCs', { consulta: CCConsultaEnum.TodosConAuditorCreado, checkListId: null }, false, null, () => {
                cboFiltroCC.attr('multiple', 'multiple');
                cboFiltroCC.select2();
                cboFiltroCC.find('option:selected').remove();
            });

            cboNuevoCC.fillCombo('GetCCs', { consulta: CCConsultaEnum.TodosLosActivos, checkListId: null }, false, null, () => {
                cboNuevoCC.find('option').each(function (value, index, array) {
                    $(this).clone().appendTo(cboLiderCCs);
                });

                cboNuevoCC.attr('multiple', 'multiple');
                cboNuevoCC.select2();
                cboNuevoCC.find('option:selected').remove();

                cboLiderCCs.attr('multiple', 'multiple');
                cboLiderCCs.select2();
                cboLiderCCs.find('option:selected').remove();
            });

            cboFiltroPrivilegio.fillCombo('GetPrivilegios', null, false, null, () => {
                cboFiltroPrivilegio.find('option').each(function (value, index, array) {
                    $(this).clone().appendTo(cboFacultamientoPrivilegio);
                });

                cboFiltroPrivilegio.select2();
                cboFacultamientoPrivilegio.select2();
                cboFacultamientoPrivilegio.find("option").get(0).remove();
            });

            cboLiderArea.fillCombo('GetAreaOperativaLider', null, false, null, () => {
                cboLiderArea.select2();
                cboLiderArea.find("option").get(0).remove();
            });
        }
        //#endregion

        //#endregion TABLAS
        function initTblAuditores() {
            tblAuditores.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                columns: [
                    {
                        data: 'auditor',
                        title: 'AUDITORES',
                        className: 'dt-center'
                    },
                    {
                        title: 'CC',
                        className: 'dt-center',
                        data: 'ccs',
                        render: function (data, type, row) {
                            let labels = '';
                            for (let index = 0; index < data.length; index++) {
                                labels += `<span class="label label-info">${data[index]}</span>&nbsp;`;
                            }

                            return labels;
                        }
                    },
                    {
                        title: 'EDITAR',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            let btnTblEditar = `<button class="btn btn-warning btnEditar" title="Editar"><i class="fas fa-edit"></i></button>`;
                            let btnTblEliminar = `<button class="btn btn-danger btnEliminar" title="Eliminar"><i class="fas fa-trash"></i></button>`;

                            return btnTblEditar + ' ' + btnTblEliminar;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblAuditores.on('click', '.btnEliminar', function () {
                        const rowData = tblAuditores.DataTable().row($(this).closest('tr')).data();

                        swal({
                            title: 'Alerta!',
                            text: '¿Desea eliminar al usuario?, Si el usuario se encuentra como líder asignado a un checkList se eliminara tambien del checkList.',
                            icon: 'warning',
                            buttons: true,
                            dangerMode: false,
                            buttons: ['Cerrar', 'Aceptar']
                        })
                            .then((aceptar) => {
                                if (aceptar) {
                                    eliminarAuditor(rowData.id);
                                }
                            });
                    });

                    tblAuditores.on('click', '.btnEditar', function () {
                        const rowData = tblAuditores.DataTable().row($(this).closest('tr')).data();

                        getAuditor(rowData.id);
                    });
                },
                columnDefs: []
            });
        }

        function initTblFacultamientos() {
            tblFacultamientos.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                columns: [
                    {
                        data: 'tipoUsuario',
                        title: 'TIPO DE USUARIO',
                        className: 'dt-center'
                    },
                    {
                        data: 'nombreCompleto',
                        title: 'NOMBRE COMPLETO',
                        className: 'dt-center'
                    },
                    {
                        title: 'CC',
                        className: 'dt-center',
                        data: 'ccs',
                        render: function (data, type, row) {
                            let labels = '';
                            for (let index = 0; index < data.length; index++) {
                                labels += `<span class="label label-info">${data[index]}</span>&nbsp;`;
                            }

                            return labels;
                        }
                    },
                    {
                        title: 'EDITAR',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            let btnTblEditar = `<button class="btn btn-warning btnEditar" title="Editar"><i class="fas fa-edit"></i></button>`;
                            let btnTblEliminar = `<button class="btn btn-danger btnEliminar" title="Eliminar"><i class="fas fa-trash"></i></button>`;

                            return btnTblEditar + ' ' + btnTblEliminar;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblFacultamientos.on('click', '.btnEditar', function () {
                        const rowData = tblFacultamientos.DataTable().row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Alerta!',
                            '¿Desea cambiar el privilegio del usuario? Sí el usuario se encuentra como líder asignado a un checkList se eliminará también del checkList.',
                            'Confirmar',
                            'Cancelar',
                            () => getAuditorPrivilegio(rowData.id));

                        //#region VERSIÓN ANTERIOR
                        // swal({
                        //     title: 'Alerta!',
                        //     text: '¿Desea cambiar el privilegio del usuario?, Si el usuario se encuentra como líder asignado a un checkList se eliminara tambien del checkList.',
                        //     icon: 'warning',
                        //     buttons: true,
                        //     dangerMode: false,
                        //     buttons: ['Cerrar', 'Aceptar']
                        // })
                        //     .then((aceptar) => {
                        //         if (aceptar) {
                        //             getAuditorPrivilegio(rowData.id);
                        //         }
                        //     });
                        //#endregion
                    });

                    tblFacultamientos.on('click', '.btnEliminar', function () {
                        const rowData = tblFacultamientos.DataTable().row($(this).closest('tr')).data();

                        swal({
                            title: 'Alerta!',
                            text: '¿Desea eliminar el privilegio del usuario?, Si el usuario se encuentra como líder asignado a un checkList se eliminara tambien del checkList.',
                            icon: 'warning',
                            buttons: true,
                            dangerMode: false,
                            buttons: ['Cerrar', 'Aceptar']
                        })
                            .then((aceptar) => {
                                if (aceptar) {
                                    eliminarAuditorPrivilegio(rowData.id);
                                }
                            });
                    })
                },
                columnDefs: []
            });
        }

        function initTblLideres() {
            tblLideres.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                columns: [
                    {
                        data: 'nombre',
                        title: 'NOMBRE COMPLETO',
                        className: 'dt-center'
                    },
                    {
                        data: 'puesto',
                        title: 'PUESTO',
                        className: 'dt-center'
                    },
                    {
                        data: 'grupo',
                        title: 'GRUPO',
                        className: 'dt-center'
                    },
                    {
                        title: 'CC ASIGNADO (5S)',
                        className: 'dt-center',
                        data: 'ccs',
                        render: function (data, type, row) {
                            let labels = '';
                            for (let index = 0; index < data.length; index++) {
                                labels += `<span class="label label-info">${data[index]}</span>&nbsp;`;
                            }

                            return labels;
                        }
                    },
                    {
                        title: 'EDITAR',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            let btnTblEditar = `<button class="btn btn-warning btnEditar" title="Editar"><i class="fas fa-edit"></i></button>`;
                            let btnTblEliminar = `<button class="btn btn-danger btnEliminar" title="Eliminar"><i class="fas fa-trash"></i></button>`;

                            return btnTblEditar + ' ' + btnTblEliminar;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblLideres.on('click', '.btnEditar', function () {
                        const rowData = tblLideres.DataTable().row($(this).closest('tr')).data();

                        swal({
                            title: 'Alerta!',
                            text: '¿Desea editar el usuario?, Si el usuario se encuentra como líder asignado a un checkList se eliminara tambien del checkList.',
                            icon: 'warning',
                            buttons: true,
                            dangerMode: false,
                            buttons: ['Cerrar', 'Aceptar']
                        })
                            .then((aceptar) => {
                                if (aceptar) {
                                    getLider(rowData.id);
                                }
                            });
                    });

                    tblLideres.on('click', '.btnEliminar', function () {
                        const rowData = tblLideres.DataTable().row($(this).closest('tr')).data();

                        swal({
                            title: 'Alerta!',
                            text: '¿Desea eliminar el usuario?, Si el usuario se encuentra como líder asignado a un checkList se eliminara tambien del checkList.',
                            icon: 'warning',
                            buttons: true,
                            dangerMode: false,
                            buttons: ['Cerrar', 'Aceptar']
                        })
                            .then((aceptar) => {
                                if (aceptar) {
                                    eliminarLider(rowData.id);
                                }
                            });
                    });
                },
                columnDefs: []
            });
        }

        function initTblSubAreas() {
            tblSubAreas.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                columns: [
                    {
                        data: 'nombre',
                        title: 'SUB-AREA',
                        className: 'dt-center'
                    },
                    {
                        title: 'EDITAR',
                        className: 'dt-center',
                        render: function (data, type, row) {
                            let btnTblEditar = `<button class="btn btn-warning btnEditar" title="Editar"><i class="fas fa-edit"></i></button>`;
                            let btnTblEliminar = `<button class="btn btn-danger btnEliminar" title="Eliminar"><i class="fas fa-trash"></i></button>`;

                            return btnTblEditar + ' ' + btnTblEliminar;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblSubAreas.on('click', '.btnEditar', function () {
                        const rowData = tblSubAreas.DataTable().row($(this).closest('tr')).data();

                        getSubArea(rowData.id);
                    });

                    tblSubAreas.on('click', '.btnEliminar', function () {
                        const rowData = tblSubAreas.DataTable().row($(this).closest('tr')).data();

                        swal({
                            title: 'Alerta!',
                            text: '¿Desea eliminar el sub-area?',
                            icon: 'warning',
                            buttons: true,
                            dangerMode: false,
                            buttons: ['Cerrar', 'Aceptar']
                        })
                            .then((aceptar) => {
                                if (aceptar) {
                                    eliminarSubArea(rowData.id);
                                }
                            });
                    });
                },
                columnDefs: []
            });
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }
        //#endregion

        //#region GENERALES
        function limpiarModalNuevoAuditor() {
            txtNuevoAuditor.val('');
            txtNuevoAuditor.attr('data-index', '');
            txtNuevoAuditor.text('');
            txtNuevoAuditor.prop('disabled', false);
            txtNuevoPuesto.val('');
            txtNuevoProyecto.val('');
            cboNuevoCC.val('');
            cboNuevoCC.trigger('change');
            btnGuardarAuditor.data('idAuditor', '');
        }

        function limpiarModalFacultamiento() {
            txtFacultamientoAuditor.val('');
            txtFacultamientoPuesto.val('');
            cboFacultamientoPrivilegio.val('');
            cboFacultamientoPrivilegio.trigger('change');
            btnGuardarFacultamiento.data('id', '');
        }

        function limpiarModalLider() {
            txtLider.val('');
            cboLiderCCs.val('');
            cboLiderCCs.trigger('change');
            cboLiderArea.val('');
            cboLiderArea.trigger('change');
            btnGuardarLider.data('id', '');
        }

        function selectAutocompleteUsuario(event, ui) {
            $(this).text(ui.item.label);
            $(this).attr('data-index', ui.item.id);
            getInfoUsuario(ui.item.id);
        }

        function crearObjetoAuditor() {
            if (cboNuevoCC.val) {
                if (btnGuardarAuditor.data('idAuditor')) {
                    let auditor = {
                        id: btnGuardarAuditor.data('idAuditor'),
                        css: cboNuevoCC.val()
                    }

                    return auditor;
                } else {
                    let auditor = {
                        usuarioId: txtNuevoAuditor.attr('data-index'),
                        ccs: cboNuevoCC.val()
                    }

                    return auditor;
                }
            } else {
                swal('Alerta!', 'Tiene que agregar al menos un CC', 'warning');
            }
        }

        function crearObjetoFacultamiento() {
            //#region VERSIÓN ANTERIOR
            // let facultamiento = {
            //     id: btnGuardarFacultamiento.data('id'),
            //     privilegioId: cboFacultamientoPrivilegio.val()
            // }
            //#endregion

            let facultamiento = {
                id: btnGuardarFacultamiento.data('id'),
                lstPrivilegiosID: getValoresMultiples('#cboFacultamientoPrivilegio')
            }
            return facultamiento;
        }

        function crearObjetoLider() {
            //#region VERSION ANTERIOR
            // let lider = {
            //     id: btnGuardarLider.data('id'),
            //     ccs: cboLiderCCs.val(),
            //     grupoId: cboLiderArea.val()
            // }
            //#endregion

            let lider = {
                id: btnGuardarLider.data('id'),
                ccs: cboLiderCCs.val(),
                lstGruposID: getValoresMultiples("#cboLiderArea")
            }

            return lider;
        }
        //#endregion

        //#endregion ENDPOINT
        function getAuditores() {
            $.post('GetAuditores',
                {
                    ccs: cboFiltroCC.val()
                }).then(response => {
                    if (response.success) {
                        addRows(tblAuditores, response.items);
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getInfoUsuario(idUsuario) {
            $.get('GetInfoUsuario',
                {
                    idUsuario: idUsuario
                }).then(response => {
                    if (response.success) {
                        txtNuevoProyecto.val(response.items.proyecto);
                        txtNuevoPuesto.val(response.items.puesto);
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function guardarAuditor(usuario) {
            $.post('GuardarAuditor',
                {
                    usuario
                }).then(response => {
                    if (response.success) {
                        swal('Confirmación', 'Se guardó el usuario', 'success');
                        mdlNuevoAuditor.modal('hide');
                        getAuditores();
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function eliminarAuditor(idAuditor) {
            $.post('EliminarAuditor',
                {
                    idAuditor
                }).then(response => {
                    if (response.success) {
                        swal('Confirmación', 'Se eliminó el usuario', 'success');
                        getAuditores();
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getAuditor(idAuditor) {
            $.get('GetAuditor',
                {
                    idAuditor
                }).then(response => {
                    if (response.success) {
                        txtNuevoAuditor.val(response.items.auditor);
                        txtNuevoAuditor.text(response.items.auditor);
                        txtNuevoAuditor.attr('data-index', response.items.id);
                        txtNuevoAuditor.prop('disabled', true);
                        txtNuevoProyecto.val(response.items.proyecto);
                        txtNuevoPuesto.val(response.items.puesto);
                        cboNuevoCC.val(response.items.css);
                        cboNuevoCC.trigger('change');
                        btnGuardarAuditor.data('idAuditor', response.items.id);

                        tituloNuevoAuditor.text('Editar auditor');
                        mdlNuevoAuditor.modal('show');
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function editarAuditor(info) {
            $.post('EditarAuditor',
                {
                    info
                }).then(response => {
                    if (response.success) {
                        swal('Confirmación', 'Se editó el auditor', 'success');
                        mdlNuevoAuditor.modal('hide');
                        getAuditores();
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                })
        }

        function getFacultamientos() {
            $.post('GetFacultamientos',
                {
                    ccs: cboFiltroCC.val(),
                    privilegioId: cboFiltroPrivilegio.val()
                }).then(response => {
                    if (response.success) {
                        addRows(tblFacultamientos, response.items);
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getAuditorPrivilegio(idAuditor) {
            $.get('GetAuditorPrivilegio',
                {
                    idAuditor
                }).then(response => {
                    if (response.success) {
                        txtFacultamientoAuditor.val(response.items.auditor);
                        txtFacultamientoPuesto.val(response.items.puesto);
                        cboFacultamientoPrivilegio.val(response.items.privilegioId);
                        cboFacultamientoPrivilegio.trigger('change');
                        btnGuardarFacultamiento.data('id', response.items.id);

                        mdlFacultamiento.modal('show');
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function guardarAuditorPrivilegio(privilegio) {
            $.post('GuardarAuditorPrivilegio',
                {
                    privilegio
                }).then(response => {
                    if (response.success) {
                        // swal('Confirmación', 'Se guardó el privilegio', 'success');
                        Alert2Exito("Se ha registrado con éxito la información.");
                        mdlFacultamiento.modal('hide');
                        getFacultamientos();
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function eliminarAuditorPrivilegio(idAuditor) {
            $.post('EliminarAuditorPrivilegio',
                {
                    idAuditor
                }).then(response => {
                    if (response.success) {
                        swal('Confirmación', 'Se eliminó el privilegio', 'success');

                        getFacultamientos();
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getTablaLideres(ccs) {
            $.post('GetTablaLideres',
                {
                    ccs
                }).then(response => {
                    if (response.success) {
                        addRows(tblLideres, response.items);
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getLider(idLider) {
            $.get('GetLider',
                {
                    idLider
                }).then(response => {
                    if (response.success) {
                        //#region VERSIÓN ANTERIOR
                        // txtLider.val(response.items.nombre);
                        // cboLiderCCs.val(response.items.ccs);
                        // cboLiderCCs.trigger('change');
                        // cboLiderArea.val(response.items.grupoId);
                        // cboLiderArea.trigger('change');
                        // btnGuardarLider.data('id', response.items.id);
                        //#endregion

                        txtLider.val(response.items.nombre);
                        cboLiderCCs.val(response.items.ccs);
                        cboLiderCCs.trigger('change');
                        cboLiderArea.val(response.items.lstGruposID);
                        cboLiderArea.trigger('change');
                        btnGuardarLider.data('id', response.items.id);

                        mdlLider5s.modal('show');
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function eliminarLider(idLider) {
            $.post('EliminarLider',
                {
                    idLider
                }).then(response => {
                    if (response.success) {
                        swal('Confirmación', 'Se eliminó al usuario como líder', 'success');

                        btnTabLider.trigger('click');
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function guardarLider(info) {
            $.post('GuardarLider',
                {
                    info
                }).then(response => {
                    if (response.success) {
                        swal('Confirmación', 'Se actualizó la información del líder', 'success');

                        mdlLider5s.modal('hide');

                        btnTabLider.trigger('click');
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getTablaSubAreas() {
            $.get('GetTablaSubAreas').then(response => {
                if (response.success) {
                    addRows(tblSubAreas, response.items);
                } else {
                    swal("¡Alerta!", response.message, "warning");
                }
            }, error => {
                swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
            });
        }

        function getSubArea(id) {
            $.get('GetSubArea',
                {
                    id
                }).then(response => {
                    if (response.success) {
                        txtSubArea.val(response.items.descripcion);
                        btnGuardarSubArea.data('id', response.items.id);

                        mdlSubArea.modal('show');
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function eliminarSubArea(id) {
            $.post('EliminarSubArea',
                {
                    id
                }).then(response => {
                    if (response.success) {
                        swal('Confirmacion', 'Se elimino el subArea', 'success');

                        btnTabSubArea.trigger('click');
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function editarSubArea(id, nombre) {
            $.post('EditarSubArea',
                {
                    id,
                    nombre
                }).then(response => {
                    if (response.success) {
                        swal('Confirmacion', 'Se actualizo la informacion', 'success');

                        btnTabSubArea.trigger('click');

                        mdlSubArea.modal('hide');
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function GuardarSubArea(nombre) {
            $.post('GuardarSubArea',
                {
                    nombre
                }).then(response => {
                    if (response.success) {
                        swal('Confirmacion', 'Se guardo la informacion', 'success');

                        btnTabSubArea.trigger('click');

                        mdlSubArea.modal('hide');
                    } else {
                        swal("¡Alerta!", response.message, "warning");
                    }
                }, error => {
                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }
        //#endregion
    }

    $(document).ready(() => Administrativo.CincoS = new CincoS())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();