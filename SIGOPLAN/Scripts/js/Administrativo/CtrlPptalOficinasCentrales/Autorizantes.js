(() => {
    $.namespace('CtrlPptalOficinasCentralesController.Autorizantes');

    //#region CONST
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const tblAutorizantes = $('#tblAutorizantes');
    let dtAutorizantes

    const mdlAutorizantes = $('#mdlAutorizantes');
    const btnNuevo = $('#btnNuevo');
    const modaltitle = $('.modal-title');

    const cboCC = $('#cboCC');
    const cboAutorizante = $('#cboAutorizante');
    const cboDescripcion = $('#cboDescripcion');
    const btnGuardar = $('#btnGuardar');

    const lblTitleMdlCEAutorizantes = $('#lblTitleMdlCEAutorizantes');
    const lblTitleBtnCEAutorizante = $('#lblTitleBtnCEAutorizante');
    //#endregion

    const objDTO = {
        id: 0,
        cc: '',
        idRow: 0,
        descripcion: '',
        idAutorizante: 0,
        idUsuarioCreacion: '',
        fechaCreacion: '',
        idUsuarioModificacion: 0,
        fechaModificacion: '',
        registroActivo: true,
    };

    const ob = {
        id: 0,
        cc: '',
        idRow: '',
        descripcion: '',
        idAutorizante: 0,
        idUsuarioCreacion: '',
        fechaCreacion: '',
        idUsuarioModificacion: 0,
        fechaModificacion: '',
        registroActivo: true,
    }

    const cboFiltroCC = $('#cboFiltroCC');

    Autorizantes = function () {
        (function init() {
            fncListeners();
            inittblAutorizantes();
        })();

        function fncListeners() {
            $(".select2").select2();
            // cboCE_PM_CC.fillCombo("FillUsuarioRelCCPptosAutorizados", { anio: $(this).val() }, false);
            cboFiltroCC.fillCombo('FillCCAutorizantes', null, false, null);
            cboCC.fillCombo('FillCCAutorizantes', null, false, null);
            cboAutorizante.fillCombo('GetAutorizantesCombo', null, false, null);
            $(".select2").select2({ width: "100%" });

            btnNuevo.click(function () {
                fncTitleMdlCEAutorizante(true);
                objDTO.id = 0;
                objDTO.cc = '';
                objDTO.idRow = 0;
                objDTO.descripcion = '';
                objDTO.idAutorizante = 0;
                objDTO.idUsuarioCreacion = '';
                objDTO.fechaCreacion = '';
                objDTO.idUsuarioModificacion = '';
                objDTO.fechaModificacion = '';
                objDTO.registroActivo = true;
                btnGuardar.attr('data-id', objDTO.id);
                cboAutorizante[0].selectedIndex = 0;
                cboAutorizante.trigger('change');
                cboCC.val(objDTO.cc);
                cboCC.trigger('change');
                cboDescripcion[0].selectedIndex = 0;
                cboDescripcion.trigger('change');
                cboCC.prop('disabled', false);
                cboDescripcion.prop('disabled', false);
                fncCamposObligatorios(false);
                mdlAutorizantes.modal('show');
            })
            cboFiltroCC.change(function () {
                ob.cc = cboFiltroCC.val();
            })
            btnFiltroBuscar.click(GetAutorizantes);
            btnGuardar.click(function () {
                AddEditAutorizantes();
            })
        }

        function GetAutorizantes() {
            axios.post('GetAutorizantes', { objDTO: ob }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    AddRows(tblAutorizantes, items);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function inittblAutorizantes() {
            dtAutorizantes = tblAutorizantes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreAutorizante', title: 'NOMBRE' },
                    { data: 'cc', title: 'CC' },
                    { data: 'descripcion', title: 'DESCRIPCIÓN' },
                    {
                        data: 'id', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`
                            <button class="btn btn-xs btn-warning editarAutorizantes" title="Editar registro."><i class="fas fa-pencil-alt"></i></button>
                            <button class="btn btn-xs btn-danger eliminarAutorizantes" title="Eliminar registro."><i class="fas fa-trash"></i></button>`);
                            $(td).find(`button`).attr('data-id', data);
                        }
                    },
                    { data: 'idCC', title: 'idCC', visible: false },
                    { data: 'idAutorizante', title: 'idAutorizante', visible: false },
                    { data: 'id', title: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblAutorizantes.on("click", ".eliminarAutorizantes", function () {
                        let strMensaje = "¿Desea eliminar el registro seleccionado?";
                        const rowData = dtAutorizantes.row($(this).closest("tr")).data();
                        objDTO.id = rowData.id;
                        objDTO.registroActivo = false;
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
                                DeleteAutorizantes($(this).attr("data-id"));
                            }
                        });
                    });

                    tblAutorizantes.on("click", ".editarAutorizantes", function (e) {
                        const rowData = dtAutorizantes.row($(this).closest("tr")).data();
                        fncTitleMdlCEAutorizante(false);

                        cboCC.val(rowData.idCC);
                        cboCC.trigger('change');

                        cboAutorizante.val(rowData.idAutorizante);
                        cboAutorizante.trigger('change');
                        
                        cboDescripcion.val(rowData.idRow);
                        cboDescripcion.trigger('change');

                        cboCC.prop('disabled', true)
                        cboDescripcion.prop('disabled', true)
                        btnGuardar.attr('data-id', rowData.id);

                        fncCamposObligatorios(false);
                        mdlAutorizantes.modal('show');
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function DeleteAutorizantes() {
            axios.post('DeleteAutorizantes', { objDTO: objDTO }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    GetAutorizantes();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function tieneVacio() {
            let va = false;
            if (objDTO.descripcion == '' || objDTO.idRow == 0 || objDTO.idAutorizante == 0 || objDTO.cc == '') {
                return true;
            }
            return va;
        }

        function AddEditAutorizantes() {
            objDTO.id = btnGuardar.attr('data-id');
            objDTO.cc = cboCC.val();
            objDTO.idRow = cboDescripcion.find('option:selected').val();
            objDTO.descripcion = cboDescripcion.find('option:selected').text();
            objDTO.idAutorizante = cboAutorizante.val();
            objDTO.idUsuarioCreacion = '';
            objDTO.fechaCreacion = '';
            objDTO.idUsuarioModificacion = '';
            objDTO.fechaModificacion = '';
            objDTO.registroActivo = true;

            if (cboCC.val() > 0 && cboDescripcion.val() > 0 && cboAutorizante.val() > 0) {
                axios.post('AddEditAutorizantes', { objDTO: objDTO }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        GetAutorizantes();
                        mdlAutorizantes.modal('hide');
                        Alert2Exito(message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                let strMensajeError = "";
                strMensajeError += cboCC.val() <= 0 ? "Es necesario indicar el CC." : "";
                strMensajeError += cboDescripcion.val() <= 0 ? "<br>Es necesario indicar el tipo de autorizante." : "";
                strMensajeError += cboAutorizante.val() <= 0 ? "<br>Es necesario indicar al autorizante." : "";
                fncCamposObligatorios(true);
                Alert2Warning(strMensajeError);
            }
        }

        function AddRows(tbl, lst) {
            dtAutorizantes = tbl.DataTable();
            dtAutorizantes.clear().draw();
            dtAutorizantes.rows.add(lst).draw(false);
        }

        function fncTitleMdlCEAutorizante(esCrear) {
            if (esCrear) {
                lblTitleMdlCEAutorizantes.html(`NUEVO REGISTRO`);
                lblTitleBtnCEAutorizante.html(`<i class='fas fa-save'></i>&nbsp;Guardar`);
                btnGuardar.attr("data-id", 0);
            } else {
                lblTitleMdlCEAutorizantes.html(`ACTUALIZAR REGISTRO`);
                lblTitleBtnCEAutorizante.html(`<i class='fas fa-save'></i>&nbsp;Actualizar`);
            }
        }

        function fncCamposObligatorios(hayError) {
            if (hayError) {
                $('#select2-cboCC-container').css('border', '2px solid red');
                $('#select2-cboDescripcion-container').css('border', '2px solid red');
                $('#select2-cboAutorizante-container').css('border', '2px solid red');
            } else {
                $('#select2-cboCC-container').css('border', '1px solid #CCC');
                $('#select2-cboDescripcion-container').css('border', '1px solid #CCC');
                $('#select2-cboAutorizante-container').css('border', '1px solid #CCC');
            }
        }
    }

    $(document).ready(() => {
        CtrlPptalOficinasCentralesController.Autorizantes = new Autorizantes();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();