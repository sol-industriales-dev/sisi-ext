(() => {
    $.namespace('Administrativo.MedioAmbiente.AspectosAmbientales');

    AspectosAmbientales = function () {

        //#region CONST
        const tablaAspectosAmbientales = $('#tablaAspectosAmbientales');
        const botonAgregar = $('#botonAgregar');
        const modalAspectoAmbiental = $('#modalAspectoAmbiental');
        const inputDescripcion = $('#inputDescripcion');
        const checkboxPeligroso = $('#checkboxPeligroso');
        const selectUnidad = $('#selectUnidad');
        const selectClasificacion = $('#selectClasificacion');
        const selectFactorPeligro = $('#selectFactorPeligro');
        const botonGuardar = $('#botonGuardar');
        const lblTitleCEAspectoAmbiental = $('#lblTitleCEAspectoAmbiental');
        const titleBtnCEAspectoAmbiental = $('#titleBtnCEAspectoAmbiental');
        const chkSolidoImpregnadoHidrocarburo = $('#chkSolidoImpregnadoHidrocarburo');
        //#endregion

        let dtAspectosAmbientales;
        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        (function init() {
            initTablaAspectosAmbientales();
            agregarListeners();
            fncCargarAspectosAmbientales();

            selectUnidad.fillCombo('GetUnidadCombo', null, false, null);
            selectUnidad.select2({ width: "100%" });
            selectClasificacion.fillCombo('GetClasificacionCombo', null, false, null);
            selectClasificacion.select2({ width: "100%" });
            selectFactorPeligro.fillCombo('GetFactorPeligroCombo', null, false, 'Todos');
            convertToMultiselect('#selectFactorPeligro');
        })();

        modalAspectoAmbiental.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        checkboxPeligroso.on('change', function () {
            if (checkboxPeligroso.prop('checked')) {
                $('#divFactorPeligro').show(500);
            } else {
                $('#divFactorPeligro').hide(500);
            }
        });

        function initTablaAspectosAmbientales() {
            dtAspectosAmbientales = tablaAspectosAmbientales.DataTable({
                language: dtDicEsp,
                retrieve: true,
                paging: true,
                searching: false,
                ordering: true,
                columns: [
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        render: function (data, type, row, meta) {
                            return row.peligroso ? 'PELIGROSO' : '';
                        }, title: 'Peligroso'
                    },
                    { data: 'unidadDesc', title: 'Unidad' },
                    { data: 'clasificacionDesc', title: 'Clasificación' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn-editar btn btn-xs btn-warning" title="Editar aspecto ambiental."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn-eliminar btn btn-xs btn-danger" title="Eliminar aspecto ambiental"><i class="fas fa-trash"></i></button>`;
                            return btnEditar + btnEliminar;
                        }
                    },
                    { data: 'esSolidoImpregnadoHidrocarburo', visible: false },
                ],
                initComplete: function (settings, json) {
                    tablaAspectosAmbientales.on('click', '.btn-editar', function () {
                        let rowData = dtAspectosAmbientales.row($(this).closest('tr')).data();

                        fncLimpiarModal();
                        fncLlenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().id = rowData.id;
                        lblTitleCEAspectoAmbiental.html("ACTUALIZAR REGISTRO");
                        titleBtnCEAspectoAmbiental.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                        modalAspectoAmbiental.modal('show');

                        if (rowData.peligroso) {
                            $('#divFactorPeligro').show(500);
                        } else {
                            $('#divFactorPeligro').hide(500);
                        }
                    });

                    tablaAspectosAmbientales.on('click', '.btn-eliminar', function () {
                        let rowData = dtAspectosAmbientales.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!',
                            `¿Desea eliminar el registro seleccionado?<br><br><b>ATENCIÓN</b>:&nbsp;Se eliminará toda la información relacionada.`,
                            'Confirmar', 'Cancelar', () => fncEliminarAspectoAmbiental(rowData.id));
                    });
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" },
                    { width: '50%', targets: [3] },
                    { width: '8%', targets: [4] }
                ]
            });
        }

        function fncCargarAspectosAmbientales() {
            axios.get('GetAspectosAmbientales')
                .then(response => {
                    let { success, data } = response.data;

                    if (success) {
                        AddRows(tablaAspectosAmbientales, data);
                    } else {
                        Alert2Error(response.message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function agregarListeners() {
            botonAgregar.click(() => {
                fncLimpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().id = 0;
                lblTitleCEAspectoAmbiental.html("NUEVO REGISTRO");
                titleBtnCEAspectoAmbiental.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                modalAspectoAmbiental.modal('show');
            });
            botonGuardar.click(fncGuardarAspectoAmbiental);
        }

        function fncLimpiarModal() {
            selectUnidad[0].selectedIndex = 0;
            selectUnidad.trigger("change");
            checkboxPeligroso.prop('checked', false);
            checkboxPeligroso.trigger("change");
            $('#divFactorPeligro').hide(500);
            selectFactorPeligro.fillCombo('GetFactorPeligroCombo', null, false, 'Todos');
            convertToMultiselect('#selectFactorPeligro');
            selectClasificacion[0].selectedIndex = 0;
            selectClasificacion.trigger("change");
            $('input[type="text"]').val('');
            chkSolidoImpregnadoHidrocarburo.prop("checked", false);
            chkSolidoImpregnadoHidrocarburo.trigger("change");
            fncBorderDefault();
        }

        function fncLlenarCamposModal(data) {
            inputDescripcion.val(data.descripcion);
            checkboxPeligroso.prop('checked', data.peligroso);
            checkboxPeligroso.trigger("change");
            $('#divFactorPeligro').show(500);

            if (data.factoresPeligro.length > 0 && data.factoresPeligro != null) {
                selectFactorPeligro.val(data.factoresPeligro.map(x => x.factorPeligro));
                selectFactorPeligro.multiselect("refresh");
            }

            selectUnidad.val(data.unidad);
            selectUnidad.trigger("change");
            selectClasificacion.val(data.clasificacion);
            selectClasificacion.trigger("change");
            chkSolidoImpregnadoHidrocarburo.prop("checked", data.esSolidoImpregnadoHidrocarburo);
            chkSolidoImpregnadoHidrocarburo.trigger("change");
        }

        function fncGuardarAspectoAmbiental() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    fncNuevoAspectoAmbiental();
                    break;
                case ESTATUS.EDITAR:
                    fncEditarAspectoAmbiental();
                    break;
            }
        }

        function fncNuevoAspectoAmbiental() {
            let aspectoAmbiental = fncGetInformacionAspectoAmbiental();
            if (aspectoAmbiental != "") {
                axios.post('GuardarNuevoAspectoAmbiental', { aspectoAmbiental }).then(response => {
                    let { success, datos } = response.data;

                    if (success) {
                        modalAspectoAmbiental.modal('hide');
                        Alert2Exito(`Se ha registrado con éxito la información.`);
                        fncCargarAspectosAmbientales();
                    } else {
                        Alert2Error(response.message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncEditarAspectoAmbiental() {
            let aspectoAmbiental = fncGetInformacionAspectoAmbiental();
            if (aspectoAmbiental != "") {
                axios.post('EditarAspectoAmbiental', { aspectoAmbiental }).then(response => {
                    let { success, datos } = response.data;

                    if (success) {
                        modalAspectoAmbiental.modal('hide');
                        Alert2Exito(`Se ha registrado con éxito la información.`);
                        fncCargarAspectosAmbientales();
                    } else {
                        Alert2Error(response.message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncEliminarAspectoAmbiental(id) {
            let aspectoAmbiental = { id };

            axios.post('EliminarAspectoAmbiental', { aspectoAmbiental })
                .then(response => {
                    let { success, datos } = response.data;

                    if (success) {
                        Alert2Exito(`Se ha eliminado con éxito la información.`);
                        fncCargarAspectosAmbientales();
                    } else {
                        Alert2Error(response.message);
                    }
                }).catch(error => Alert2Error(error.message));
        }

        function fncGetInformacionAspectoAmbiental() {
            fncBorderDefault();

            let strMensajeError = "";
            if (inputDescripcion.val() == "") { inputDescripcion.css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (selectUnidad.val() == "") { $("#select2-selectUnidad-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (selectClasificacion.val() == "") { $("#select2-selectClasificacion-container").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            let valoresComboFactoresPeligro = getValoresMultiples('#selectFactorPeligro');
            let factoresPeligro = [];
            if (valoresComboFactoresPeligro.length > 0) {
                valoresComboFactoresPeligro.forEach(element => {
                    factoresPeligro.push({
                        factorPeligro: element,
                        estatus: true
                    });
                });
            }
            if (checkboxPeligroso.prop("checked") && factoresPeligro.length <= 0) {
                $("#divFactorPeligro").css('border', '2px solid red'); strMensajeError = "Es necesario llenar los campos obligatorios.";
            }

            //#region VALIDACIÓN CAMPO OBLIGATORIO FACTORES AMBIENTALES -- COMENTADO
            // if (aspectoAmbiental.peligroso && aspectoAmbiental.factoresPeligro.length == 0) {
            //     Alert2Warning(`Debe seleccionar los factores peligro.`);
            //     return
            // }
            //#endregion

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                return {
                    id: botonGuardar.data().id,
                    descripcion: inputDescripcion.val(),
                    peligroso: checkboxPeligroso.prop('checked'),
                    unidad: selectUnidad.val(),
                    clasificacion: selectClasificacion.val(),
                    estatus: true,
                    factoresPeligro: factoresPeligro,
                    esSolidoImpregnadoHidrocarburo: chkSolidoImpregnadoHidrocarburo.prop("checked")
                };
            }
        }

        function fncBorderDefault() {
            inputDescripcion.css('border', '1px solid #CCC');
            $("#select2-selectUnidad-container").css('border', '1px solid #CCC');
            $("#select2-selectClasificacion-container").css('border', '1px solid #CCC');
            $("#select2-divFactorPeligro-container").css('border', '1px solid #CCC');
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => Administrativo.MedioAmbiente.AspectosAmbientales = new AspectosAmbientales())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();