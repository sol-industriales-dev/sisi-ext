(() => {
    $.namespace('Administrativo.Reclutamientos.CatPuestos');
    CatPuestos = function () {
        //#region Selectores
        const tablaPuestos = $('#tablaPuestos');
        const btnFiltroBuscar = $('#btnFiltroBuscar')
        const botonAgregar = $('#botonAgregar');
        const btnFiltroGenerarExcel = $('#btnFiltroGenerarExcel')
        const modalPuesto = $('#modalPuesto');
        const inputPuesto = $('#inputPuesto');
        const inputNombre = $('#inputNombre');
        const textareaDescripcion = $('#textareaDescripcion');
        const selectTipoNomina = $('#selectTipoNomina');
        const checkSindicalizado = $('#checkSindicalizado');
        const botonGuardar = $('#botonGuardar');
        const cboCE_AreaDepartamento = $("#cboCE_AreaDepartamento")
        const cboCE_Sindicalizado = $('#cboCE_Sindicalizado')
        const cboCE_NivelMando = $("#cboCE_NivelMando")
        const cboCE_esEvaluacion = $('#cboCE_esEvaluacion');
        const txtCE_BAE = $('#txtCE_BAE');
        const divPERU_BASE = $('#divPERU_BASE');
        //#endregion

        //#region CONST ELEMENTOS OCULTOS
        const inputEmpresaActual = $('#inputEmpresaActual');
        _EMPRESA_ACTUAL = +inputEmpresaActual.val();
        //#endregion

        let dtPuestos;
        let permisoDescriptor = false;

        const ESTATUS = { NUEVO: 0, EDITAR: 1 };

        const report = $('#report');

        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { downloadDescriptor(parametros.id) } },
                { text: `<i class="fa fa-file"></i> Visualizar`, action: "visor", fn: parametros => { visualizarDescriptor(parametros.ruta) } }
            ]
        };

        (function init() {
            initTablaPuestos();
            agregarListeners();
            fncMostrarOcultarControles();
            cargarPuestos();
            tablaPuestos.DataTable().buttons('.buttonsToHide').nodes().css("display", "none");
            fncGetAccesosMenu();
            $("#menuPuestos").addClass("opcionSeleccionada")

            cboCE_AreaDepartamento.fillCombo('/Administrativo/Tabuladores/FillCboAreasDepartamentos', null, false, null);
            selectTipoNomina.fillCombo('/Administrativo/Tabuladores/FillCboTipoNomina', null, false, null);
            cboCE_Sindicalizado.fillCombo('/Administrativo/Tabuladores/FillCboSindicatos', null, false, null);
            cboCE_NivelMando.fillCombo('/Administrativo/Tabuladores/FillCboNivelMando', null, false, null);
            $(".select2").select2()

            btnFiltroBuscar.click(function () {
                cargarPuestos()
            });

            btnFiltroGenerarExcel.click(function () {
                Alert2AccionConfirmar('Descargar excel', '¿Desea descargar un archivo excel?', 'Confirmar', 'Cancelar', () => fncGenerarExcel());
            });

            $(document).on('change', '.inputDescriptor', function () {
                let file = $(this)[0].files[0];

                if (file) {
                    let formData = new FormData();
                    formData.append('file', file);
                    formData.append('puesto', $(this).data('id'));

                    $.blockUI({ message: 'Cargando archivo... !Espere un momento!' });
                    $.ajax({
                        type: 'POST',
                        url: '/Reclutamientos/CargarArchivoDescriptor',
                        data: formData,
                        dataType: 'json',
                        contentType: false,
                        processData: false,
                        success: function (response) {
                            $.unblockUI();
                            if (response.success) {
                                Alert2Exito(`Se ha registrado con éxito`);
                                btnFiltroBuscar.trigger('click');
                            } else {
                                AlertaGeneral('Alerta', response.message);
                            }
                        },
                        error: function (error) {
                            AlertaGeneral('Alerta');
                            $.unblockUI();
                        }
                    });
                }
            });
        })();

        modalPuesto.on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function fncGenerarExcel() {
            $(".ui-button-text").click();
        }

        function initTablaPuestos() {
            dtPuestos = tablaPuestos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: true,
                "bLengthChange": false,
                "autoWidth": false,
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        className: "buttonsToHide",
                        footer: true,
                        exportOptions: {
                            // columns: [':visible', 21]
                            // columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26]
                        }
                    }
                ],
                columns: [
                    { data: 'puesto', title: 'ID' },
                    { data: 'descripcion', title: 'Puesto' },
                    // { data: 'descripcion_puesto', title: 'Descripción' },
                    { data: 'areaDepartamentoDesc', title: 'Area Departamento', render: (data, type, row, meta) => { return data ?? "-" } },
                    { data: 'nivelMandoDesc', title: 'Nivel Mando', render: (data, type, row, meta) => { return data ?? "-" } },
                    { data: 'tipo_nominaDesc', title: 'Tipo nómina' },
                    { data: 'sindicatoDesc', title: 'Sindicato' },
                    {
                        data: 'esEvaluacion', title: 'Evaluacion',
                        render: (data, type, row, meta) => {
                            return data ? "APLICA" : "NO APLICA"
                        }
                    },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = permisoDescriptor ? `` : `<button title="Editar registro" class="btn-editar btn btn-xs btn-warning"><i class="fas fa-pencil-alt"></i></button>`
                            let btnEliminar = permisoDescriptor ? `` : `<button title="Eliminar registro" class="btn-eliminar btn btn-xs btn-danger"><i class="fas fa-trash"></i></button>`;
                            let btnCargarDescriptor = `<input type="file" style="display:none;" class="inputDescriptor" ><button title="Cargar descriptor" class="btn-cargar-descriptor btn btn-xs btn-${row.idDescriptor != 0 ? "success" : "info"}"><i class="fas fa-file-upload"></i></button>`;
                            let btnVisor = `<button title="Ver Descriptor" class="btn-visor btn btn-xs btn-success"><i class="fas fa-eye"></i></button>`;

                            if (row.idDescriptor != 0) {
                                return `${btnEditar} ${btnEliminar} ${btnCargarDescriptor} ${btnVisor}`;
                            } else {
                                return `${btnEditar} ${btnEliminar} ${btnCargarDescriptor}`;
                            }
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tablaPuestos.on('click', '.btn-editar', function () {
                        let rowData = dtPuestos.row($(this).closest('tr')).data();

                        limpiarModal();
                        llenarCamposModal(rowData);
                        botonGuardar.data().estatus = ESTATUS.EDITAR;
                        botonGuardar.data().puesto = rowData.puesto;
                        modalPuesto.modal('show');
                    });

                    tablaPuestos.on('click', '.btn-eliminar', function () {
                        let rowData = dtPuestos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => eliminarPuesto(rowData.puesto));
                        // AlertaAceptarRechazarNormal('Confirmar Eliminación', `¿Está seguro de eliminar el puesto "${rowData.descripcion}"?`,() => eliminarPuesto(rowData.puesto))
                    });

                    tablaPuestos.on('click', '.btn-cargar-descriptor', function () {
                        const rowData = dtPuestos.row($(this).closest('tr')).data();
                        $(this).closest('tr').find('.inputDescriptor').data('id', rowData.puesto);
                        $(this).closest('tr').find('.inputDescriptor').trigger('click');
                    });

                    tablaPuestos.on('click', '.btn-visor', function () {
                        const rowData = dtPuestos.row($(this).closest('tr')).data();

                        menuConfig.parametros = {
                            id: rowData.idDescriptor,
                            ruta: rowData.rutaDescriptor
                        }

                        mostrarMenu();
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { className: 'dt-body-center', 'targets': '_all' },
                    { width: '30%', targets: [1] },
                    // { width: '60%', targets: [2] },
                    { width: '6%', targets: [5] }
                ]
            });
        }

        function cargarPuestos() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Reclutamientos/GetPuestos')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        permisoDescriptor = response.permisoDescriptor;
                        AddRows(tablaPuestos, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function agregarListeners() {
            botonAgregar.click(() => {
                limpiarModal();
                botonGuardar.data().estatus = ESTATUS.NUEVO;
                botonGuardar.data().puesto = 0;
                modalPuesto.modal('show');
            });
            botonGuardar.click(guardarPuesto);
        }

        function limpiarModal() {
            $("input[type='text']").val("");
            cboCE_AreaDepartamento[0].selectedIndex = 0;
            cboCE_AreaDepartamento.trigger("change");
            selectTipoNomina[0].selectedIndex = 0;
            selectTipoNomina.trigger("change");
            cboCE_Sindicalizado[0].selectedIndex = 0;
            cboCE_Sindicalizado.trigger("change");
            cboCE_NivelMando[0].selectedIndex = 0;
            cboCE_NivelMando.trigger("change");
        }

        function llenarCamposModal(data) {
            inputPuesto.val(data.puesto);
            inputNombre.val(data.descripcion);
            textareaDescripcion.val(data.descripcion_puesto);
            cboCE_AreaDepartamento.val(data.FK_AreaDepartamento);
            cboCE_AreaDepartamento.trigger("change")
            selectTipoNomina.val(data.FK_TipoNomina);
            selectTipoNomina.trigger("change")
            cboCE_Sindicalizado.val(data.FK_Sindicato);
            cboCE_Sindicalizado.trigger("change")
            cboCE_NivelMando.val(data.FK_NivelMando);
            cboCE_NivelMando.trigger("change")
            cboCE_esEvaluacion.val(data.esEvaluacion ? "1" : "0");
            cboCE_esEvaluacion.trigger("change");
            txtCE_BAE.val(data.BAE);
        }

        function guardarPuesto() {
            let estatus = botonGuardar.data().estatus;

            switch (estatus) {
                case ESTATUS.NUEVO:
                    nuevaPuesto();
                    break;
                case ESTATUS.EDITAR:
                    editarPuesto();
                    break;
            }
        }

        function nuevaPuesto() {
            let puesto = getInformacionPuesto();
            if (puesto != null) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Reclutamientos/GuardarNuevoPuesto', { puesto }).always($.unblockUI).then(response => {
                    if (response.success) {
                        modalPuesto.modal('hide');
                        Alert2Exito(`Se ha registrado con éxito`);
                        cargarPuestos();
                    } else {
                        Alert2Error(`Alerta`, response.message);
                    }
                }, error => {
                    Alert2Error(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
        }

        function editarPuesto() {
            let puesto = getInformacionPuesto();
            if (puesto != null) {
                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Administrativo/Reclutamientos/EditarPuesto', { puesto }).always($.unblockUI).then(response => {
                    if (response.success) {
                        modalPuesto.modal('hide');
                        Alert2Exito(`Se ha actualizado con éxito.`);
                        cargarPuestos();
                    } else {
                        Alert2Error(`Alerta`, response.message);
                    }
                }, error => {
                    Alert2Error(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                });
            }
        }

        function eliminarPuesto(puesto_id) {
            let puesto = { puesto: puesto_id };
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Administrativo/Reclutamientos/EliminarPuesto', { puesto }).always($.unblockUI).then(response => {
                if (response.success) {
                    Alert2Exito(`Se ha eliminado con éxito.`);
                    cargarPuestos();
                } else {
                    Alert2Warning(response.message);
                }
            }, error => {
                Alert2Error(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            });
        }

        function getInformacionPuesto() {
            fncDefaultCtrls("inputNombre", false);
            fncDefaultCtrls("cboCE_AreaDepartamento", true);
            fncDefaultCtrls("selectTipoNomina", true);
            fncDefaultCtrls("cboCE_Sindicalizado", true);
            fncDefaultCtrls("cboCE_NivelMando", true);
            fncDefaultCtrls("txtCE_BAE", true);

            //#region VALIDACIONES GENERALES
            if (inputNombre.val() == "") { fncValidacionCtrl("inputNombre", false, "Es necesario indicar el nombre del puesto."); return null; }
            if (cboCE_AreaDepartamento.val() <= 0) { fncValidacionCtrl("cboCE_AreaDepartamento", true, "Es necesario seleccionar un área/departamento."); return null; }
            if (selectTipoNomina.val() <= 0) { fncValidacionCtrl("selectTipoNomina", true, "Es necesario seleccionar el tipo de nómina."); return null; }
            if (cboCE_Sindicalizado.val() <= 0) { fncValidacionCtrl("cboCE_Sindicalizado", true, "Es necesario seleccionar el tipo de sindicato."); return null; }
            if (cboCE_NivelMando.val() <= 0) { fncValidacionCtrl("cboCE_NivelMando", true, "Es necesario indicar el nivel de mando."); return null; }
            //#endregion

            //#region VALIDACIONES SOLO PERU
            if (_EMPRESA_ACTUAL == 6) {
                if (txtCE_BAE.val() == "") { fncValidacionCtrl("txtCE_BAE", false, "Es necesario indicar el BAE para el puesto."); return null; }
                if (txtCE_BAE.val() >= 101 || txtCE_BAE.val() <= -1) { fncValidacionCtrl("txtCE_BAE", false, "Es necesario indicar el BAE del 0% al 100%"); return null; }
            }
            //#endregion

            return {
                puesto: botonGuardar.data().puesto,
                descripcion: inputNombre.val(),
                descripcion_puesto: textareaDescripcion.val(),
                FK_AreaDepartamento: cboCE_AreaDepartamento.val(),
                FK_TipoNomina: selectTipoNomina.val(),
                FK_Sindicato: cboCE_Sindicalizado.val(),
                FK_NivelMando: cboCE_NivelMando.val(),
                esEvaluacion: cboCE_esEvaluacion.val() == 1 ? true : false,
                BAE: txtCE_BAE.val(),
                esActivo: true
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function downloadDescriptor(id) {
            window.location.href = '/Administrativo/Reclutamientos/getFileDownloadDescriptor?id=' + id;
        }

        function visualizarDescriptor(ruta) {
            $('#myModal').data().ruta = ruta;
            $('#myModal').modal('show');
        }

        //#region FUNCIONES SOLO TABULADORES
        function fncGetAccesosMenu() {
            axios.post('/Administrativo/Tabuladores/GetAccesosMenu').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    for (let i = 0; i <= 9; i++) {
                        switch (response.data.lstAccesosDTO[i]) {
                            case 0:
                                $("#menuLineaNegocios").css("display", "inline");
                                break;
                            case 1:
                                $("#menuPuestos").css("display", "inline");
                                break;
                            case 2:
                                $("#menuTabuladores").css("display", "inline");
                                break;
                            case 3:
                                $("#menuPlantillasPersonal").css("display", "inline");
                                break;
                            case 4:
                                $("#menuGestionTabuladores").css("display", "inline");
                                break;
                            case 5:
                                $("#menuGestionPlantillasPersonal").css("display", "inline");
                                break;
                            case 6:
                                $("#menuModificacion").css("display", "inline");
                                break;
                            case 7:
                                $("#menuGestionModificacion").css("display", "inline");
                                break;
                            case 8:
                                $("#menuReportes").css("display", "inline");
                                break;
                            case 9:
                                $("#menuGestionReportes").css("display", "inline");
                                break;
                        }
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        function fncMostrarOcultarControles() {
            if (_EMPRESA_ACTUAL == 6) { // PERU
                divPERU_BASE.css("display", "inline");
            } else {
                divPERU_BASE.css("display", "none");
            }
        }
    }
    $(document).ready(() => Administrativo.Reclutamientos.CatPuestos = new CatPuestos())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();