(() => {
    $.namespace('CH.Evaluacion360');

    //#region CONST FILTROS
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const btnModalPeriodo = $('#btnModalPeriodo');
    //#endregion

    //#region FUNCIONES MENU
    const menuPersonal = $("#menuPersonal");
    const menuConductas = $("#menuConductas");
    const menuCuestionarios = $("#menuCuestionarios");
    const menuPeriodos = $("#menuPeriodos");
    const menuCriterios = $("#menuCriterios");
    const menuRelaciones = $('#menuRelaciones');
    const menuEvaluaciones = $('#menuEvaluaciones')
    const menuReporte360 = $('#menuReporte360')
    const menuAvances = $('#menuAvances')
    //#endregion

    //#region CONST PERIODOS
    const tblPeriodos = $('#tblPeriodos');
    let dtPeriodos;
    const modalPeriodo = $('#modalPeriodo');
    const inputNombrePeriodo = $('#inputNombrePeriodo');
    const inputFechaCierre = $('#inputFechaCierre');
    const cboEstado = $('#cboEstado');
    const btnGuardarPeriodo = $('#btnGuardarPeriodo');
    const _ESTADO = {
        ABIERTO: 'A',
        CERRADO: 'C'
    };
    //#endregion

    Evaluacion360 = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLE
            fncGetNivelAcceso()
            initTablaPeriodos();
            fncGetPeriodos();
            //#endregion

            //#region FUNCIONES MENU
            menuPersonal.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Personal';
            })

            menuConductas.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Conductas';
            })

            menuCuestionarios.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Cuestionarios';
            })

            menuPeriodos.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Periodos';
            })

            menuCriterios.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Criterios';
            })

            menuRelaciones.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Relaciones';
            })

            menuEvaluaciones.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Evaluaciones';
            })

            menuReporte360.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Reporte360';
            })

            menuAvances.click(function () {
                document.location.href = '/Administrativo/Evaluacion360/Avances';
            })
            //#endregion

            //#region FUNCIONES FILTROS
            btnFiltroBuscar.click(function () {
                fncGetPeriodos();
            })

            btnModalPeriodo.click(() => {
                fncLimpiarModal();
                $("#modalPeriodo .modal-title").text("NUEVO REGISTRO");
                btnGuardarPeriodo.data().id = 0;
                btnGuardarPeriodo.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                modalPeriodo.modal('show');
            })
            //#endregion

            //#region FUNCIONES PERIODOS
            $(".select2").select2();

            inputFechaCierre.datepicker({
                format: 'DD/MM/YYYY',
            });

            btnGuardarPeriodo.on('click', function () {
                fncCEPeriodo();
            });

            modalPeriodo.on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });
            //#endregion
        }

        //#region FUNCIONES PERIODOS
        function initTablaPeriodos() {
            dtPeriodos = tblPeriodos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombrePeriodo', title: 'Nombre del periodo' },
                    {
                        data: 'estado', title: 'Estado',
                        render: function (data, type, row) {
                            if (data == _ESTADO.ABIERTO) {
                                return 'Abierto';
                            } else if (data == _ESTADO.CERRADO) {
                                return 'Cerrado'
                            }
                        }
                    },
                    {
                        data: 'fechaCierre', title: 'Fecha cierre',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'retroalimentacion', title: 'Retroalimentacion', visible: false },
                    {
                        title: 'Opciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning btn-editarPeriodo' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger btn-eliminarPeriodo' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return btnEditar + btnEliminar;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblPeriodos.on('click', '.btn-editarPeriodo', function () {
                        let rowData = dtPeriodos.row($(this).closest('tr')).data();
                        fncGetDatosActualizarPeriodo(rowData.id);
                    });

                    tblPeriodos.on('click', '.btn-eliminarPeriodo', function () {
                        let rowData = dtPeriodos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarPeriodo(rowData.id));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function fncGetDatosActualizarPeriodo(id) {
            if (id > 0) {
                let obj = new Object();
                obj.id = id;
                axios.post('GetDatosActualizarPeriodo', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        inputNombrePeriodo.val(response.data.objPeriodo.nombrePeriodo);
                        inputFechaCierre.val(moment(response.data.objPeriodo.fechaCierre).format("DD/MM/YYYY"));
                        cboEstado.val(response.data.objPeriodo.estado);
                        cboEstado.trigger("change");
                        btnGuardarPeriodo.data().id = id;
                        $("#modalPeriodo .modal-title").text("ACTUALIZAR REGISTRO");
                        btnGuardarPeriodo.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                        modalPeriodo.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información del periodo.")
            }
        }

        function fncGetPeriodos() {
            axios.post('GetPeriodos').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtPeriodos.clear();
                    dtPeriodos.rows.add(items);
                    dtPeriodos.draw();

                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCEPeriodo() {
            let obj = fncCEPeriodoOBJ();
            if (obj != "") {
                axios.post("CEPeriodo", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetPeriodos();
                        Alert2Exito(message);
                        modalPeriodo.modal('hide');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEPeriodoOBJ() {
            let mensajeError = "";
            if (inputNombrePeriodo.val() == "") { inputNombrePeriodo.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputNombrePeriodo") }
            if (inputFechaCierre.val() == "") { inputFechaCierre.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("inputFechaCierre") }
            if (cboEstado.val() == "N/A") { $("#select2-cboEstado-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.'; } else { fncDefaultCtrls("select2-cboEstado-container") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return "";
            } else {
                let obj = new Object();
                obj.id = btnGuardarPeriodo.data().id;
                obj.nombrePeriodo = inputNombrePeriodo.val();
                obj.fechaCierre = inputFechaCierre.val();
                obj.estado = cboEstado.val();
                return obj;
            }
        }

        function fncEliminarPeriodo(id) {
            if (id > 0) {
                let objPeriodo = new Object();
                objPeriodo.idPeriodo = id;
                axios.post("EliminarPeriodo", objPeriodo).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetPeriodos();
                        modalPeriodo.modal('hide');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al eliminar el registro.");
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarModal() {
            $("input[type='text']").val("");
            cboEstado[0].selectedIndex = 0;
            cboEstado.trigger("change");
        }

        function fncDefaultCtrls(obj) {
            $(`#${obj}`).css("border", "1px solid #CCC");
        }

        function fncGetNivelAcceso() {
            axios.post('GetNivelAcceso').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (response.data.nivelAcceso == 1) {
                        menuPersonal.css("display", "block")
                        menuConductas.css("display", "block")
                        menuCuestionarios.css("display", "block")
                        menuPeriodos.css("display", "block")
                        menuCriterios.css("display", "block")
                        menuRelaciones.css("display", "block")
                        menuEvaluaciones.css("display", "block")
                        menuReporte360.css("display", "block")
                        menuAvances.css("display", "block")
                    } else if (response.data.nivelAcceso == 0) {
                        menuEvaluaciones.trigger("click")
                    }
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }
    $(document).ready(() => CH.Evaluacion360 = new Evaluacion360())
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();