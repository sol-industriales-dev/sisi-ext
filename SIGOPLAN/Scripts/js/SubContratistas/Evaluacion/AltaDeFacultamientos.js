(() => {
    $.namespace('ControlObra.AltaDeFacultamientos');

    //#region CONST
    const lblTitleModalCEFacultamiento = $('#lblTitleModalCEFacultamiento');
    const tblFacultamientos = $('#tblFacultamientos');
    let dtFacultamientos;
    let objUsuario = {
        id: '',
        tipo: '',
        cc: '',
        usuarioCreacion: '',
        fechaCrecion: '',
        usuarioModificacion: '',
        fechaModificacion: '',
        esActivo: '',
        idUsuario: '',
    }
    const mdlListadoCC = $('#mdlListadoCC');
    const cboSubContratista = $('#cboSubContratista');
    const cboTipoUsuario = $('#cboTipoUsuario');
    const cboCC = $('#cboCC');
    const cboProyecto2 = $('#cboProyecto2');
    const btnBuscar = $('#btnBuscar');
    let tipoUsuario = [
        { value: 1, text: 'ADMINISTRADOR PMO' },
        { value: 2, text: 'ADMINISTRADOR' },
        { value: 3, text: 'EVALUADOR' },
        { value: 4, text: 'CONSULTA' }
    ]
    const btnGuardarPreguntarPrimero = $('#btnGuardarPreguntarPrimero');
    const mdlAgregarEditar = $('#mdlAgregarEditar');
    const btnNuevo = $('#btnNuevo');
    let dtUsuarioRelCC;
    const tblUsuarioRelCC = $('#tblUsuarioRelCC');
    //#endregion

    AltaDeFacultamientos = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            cboProyecto2.fillCombo('FillCboProyectosFacultamientos', null, false, null);

            $("#spanCboCC").click(function (e) {
                cboCC.next(".select2-container").css("display", "block");
                cboCC.siblings("span").find(".select2-selection__rendered")[0].click();
            });

            cboCC.on('select2:close', function (e) {
                cboCC.next(".select2-container").css("display", "none");
                var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                if (seleccionados.length == 0) $("#spanCboCC").text("TODOS");
                else {
                    if (seleccionados.length == 1) $("#spanCboCC").text($(seleccionados[0]).text().slice(1));
                    else $("#spanCboCC").text(seleccionados.length.toString() + " Seleccionados");
                }
            });

            cboCC.on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) { return; }
                evt.params.originalEvent.stopPropagation();
            });

            cboSubContratista.fillCombo('cboUsuarios', null, false);

            initTblFacultamientos();
            initTblUsuarioRelCC();
            // AccionesFacultamientos(4);
            $(`.comboChange`).select2();
            $(`.comboChange`).select2({ width: '100%' });
            btnBuscar.click(function () {
                objUsuario.cc = cboProyecto2.val() == null ? "" : cboProyecto2.val();
                AccionesFacultamientos(4);
            });

            // let elemento = ``;
            // tipoUsuario.forEach(x => {
            //     elemento = `<option value="${x.value}">${x.text}</option>`;
            //     cboTipoUsuario.append(elemento);
            // });
            cboTipoUsuario.fillCombo("FillTipoUsuarioFacultamientos", null, false, null);

            cboProyecto2.on("change", function () {
                fncFocus("cboProyecto2");
            });

            cboSubContratista.on("change", function () {
                fncFocus("cboSubContratista");
            });

            cboTipoUsuario.on("change", function () {
                fncFocus("cboTipoUsuario");
            });

            btnNuevo.click(function () {
                cboSubContratista.val('');
                cboSubContratista.trigger('change');

                cboTipoUsuario.val('');
                cboTipoUsuario.trigger('change');

                // cboCC.fillCombo('FillCboProyectosFacultamientos', null, false, "Todos");
                // convertToMultiselect("#cboCC");
                // cboCC[0].selectedIndex = 0;
                // cboCC.trigger("change");
                cboCC.select2({ closeOnSelect: false });
                cboCC.fillCombo("FillCboProyectosFacultamientos", {}, false);
                cboCC.find("option").get(0).remove();
                $("#spanCboCC").trigger("click");
                $("#spanCboCC").trigger("click");

                btnNuevo.attr('data-id', 0);
                btnNuevo.attr('data-accion', 1);

                lblTitleModalCEFacultamiento.html("NUEVO FACULTAMIENTO");
            });
            btnGuardarPreguntarPrimero.click(function () {

                // let arrae = getValoresMultiples('#cboCC');
                // let itemcc = '';
                // arrae.forEach(element => {
                //     itemcc += element + ',';
                // });
                let arrCC = "";
                if (cboCC.val() == "") {
                    $("#cboCC > option").prop("selected", "selected");
                }
                cboCC.val().forEach(element => {
                    if (arrCC == "") {
                        arrCC = element;
                    } else {
                        arrCC += `,${element}`;
                    }
                });

                objUsuario.id = btnNuevo.attr('data-id');
                objUsuario.tipo = cboTipoUsuario.val();
                objUsuario.cc = arrCC;
                objUsuario.esActivo = true;
                objUsuario.idUsuario = cboSubContratista.val();
                if (cboTipoUsuario.val() == '' || cboSubContratista.val() == '') {
                    Alert2Warning('favor de llenar correctamente todos los campos.');
                } else {
                    if (btnNuevo.attr('data-accion') == "1") {
                        AccionesFacultamientos(1);
                    } else {
                        AccionesFacultamientos(2);
                    }
                }
            });
        }

        function fncFocus(obj) {
            if (obj != "") {
                setTimeout(() => $(`#${obj}`).focus(), 50);
            }
        }

        function AddRows(tbl, lst) {
            dtFacultamientos = tbl.DataTable();
            dtFacultamientos.clear().draw();
            dtFacultamientos.rows.add(lst).draw(false);
        }

        function AccionesFacultamientos(Accion) {
            axios.post('AccionesFacultamientos', { objUsuario: objUsuario, Accion: Accion }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (Accion == 4) {
                        if (success) {
                            AddRows(tblFacultamientos, items);
                        } else {
                            Alert2Error(message);
                        }
                    } else if (Accion == 1 || Accion == 2) {
                        if (success) {
                            Alert2Exito(message);
                            mdlAgregarEditar.modal('hide');
                            objUsuario.id = '';
                            objUsuario.tipo = '';
                            objUsuario.cc = '';
                            objUsuario.esActivo = '';
                            objUsuario.idUsuario = '';
                            AccionesFacultamientos(4);
                        } else {
                            Alert2Error(message);
                        }
                    } else {
                        if (success) {
                            Alert2Exito(message);
                            AccionesFacultamientos(4);
                        } else {
                            Alert2Error(message);
                        }
                    }
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblFacultamientos() {
            dtFacultamientos = tblFacultamientos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'tipoNombre', title: 'TIPO USUARIO' },
                    { data: 'nombreUsuario', title: 'NOMBRE COMPLETO' },
                    {
                        data: 'lstcc', title: 'CC',
                        render: (data, type, row, meta) => {
                            let html = '';
                            data.forEach(x => {
                                if (x != " - ") {
                                    html += `<span class=' btn-primary displayCC'>&nbsp;<i class='fab fa-creative-commons-nd'></i>${x}</span>&nbsp;`;
                                    html += '</br>';
                                }
                            });

                            if (data.length > 5) {
                                return `<button class="btn btn-primary btn-xs verListadoCC"><i class="fas fa-list"></i></button>`;
                            } else {
                                return html;
                            }
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblFacultamientos.on('click', '.editarRegistro', function () {
                        let rowData = dtFacultamientos.row($(this).closest('tr')).data();
                        btnNuevo.attr('data-id', rowData.id);
                        btnNuevo.attr('data-accion', 2);
                        cboSubContratista.val(rowData.idUsuario);
                        cboSubContratista.trigger('change');
                        cboTipoUsuario.val(rowData.tipo);
                        cboTipoUsuario.trigger('change');

                        cboCC.select2({ closeOnSelect: false });
                        cboCC.fillCombo('GetCCActualizarFacultamiento', { id: rowData.id }, false);
                        cboCC.find("option").get(0).remove();
                        $("#cboCC > option").prop("selected", "selected");
                        $("#spanCboCC").trigger("click");
                        $("#spanCboCC").trigger("click");

                        mdlAgregarEditar.modal('show');
                        lblTitleModalCEFacultamiento.html("ACTUALIZAR FACULTAMIENTO");
                    });

                    tblFacultamientos.on('click', '.eliminarRegistro', function () {
                        let rowData = dtFacultamientos.row($(this).closest('tr')).data();
                        objUsuario.id = rowData.id;
                        objUsuario.esActivo = false;
                        objUsuario.idUsuario = rowData.idUsuario;
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => AccionesFacultamientos(3));
                    });

                    tblFacultamientos.on("click", ".verListadoCC", function () {
                        let rowData = dtFacultamientos.row($(this).closest('tr')).data();
                        fncGetListadoUsuarioRelCC(rowData.id);
                        mdlListadoCC.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '15%', targets: [0] },
                    { width: '25%', targets: [1] },
                    { width: '5%', targets: [3] },
                ],
            });
        }

        function fncGetListadoUsuarioRelCC(id) {
            let obj = new Object();
            obj.id = id
            axios.post("GetListadoUsuarioRelCC", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtUsuarioRelCC.clear();
                    dtUsuarioRelCC.rows.add(response.data.lstCC);
                    dtUsuarioRelCC.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblUsuarioRelCC() {
            dtUsuarioRelCC = tblUsuarioRelCC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'descripcion', title: 'CC' }
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }
    }

    $(document).ready(() => {
        ControlObra.AltaDeFacultamientos = new AltaDeFacultamientos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();