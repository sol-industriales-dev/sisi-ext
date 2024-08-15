(() => {
    $.namespace('CH.LineaNegocio');

    //#region CONST FILTROS
    const btnFiltroBuscar = $("#btnFiltroBuscar")
    const btnFiltroNuevo = $("#btnFiltroNuevo")
    //#endregion

    //#region CONST CREAR/EDITAR LINEA DE NEGOCIO
    const mdlCELineaNegocio = $("#mdlCELineaNegocio")
    const txtCE_LineaNegocio = $("#txtCE_LineaNegocio")
    const txtCE_LineaNegocioAbreviacion = $('#txtCE_LineaNegocioAbreviacion')
    const btnCELineaNegocio = $("#btnCELineaNegocio")
    const tblLineaNegocios = $('#tblLineaNegocios')
    let dtLineaNegocios
    //#endregion

    //#region CONST CREAR RELACION LINEA DE NEGOCIO CON CC
    const mdlCELineaNegocioRelCC = $("#mdlCELineaNegocioRelCC")
    const cboCE_CC = $("#cboCE_CC")
    const btnCELineaNegocioRelCC = $("#btnCELineaNegocioRelCC")
    const tblLineaNegociosRelCC = $('#tblLineaNegociosRelCC')
    let dtLineaNegociosRelCC
    //#endregion

    LineaNegocio = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblLineaNegocios()
            fncGetLineaNegocios()
            initTblLineaNegociosRelCC()
            $(".select2").select2()
            fncGetAccesosMenu();
            $("#menuLineaNegocios").addClass("opcionSeleccionada")
            //#endregion

            //#region FUNCIONES FILTROS
            btnFiltroBuscar.click(function () {
                fncGetLineaNegocios()
            })

            btnFiltroNuevo.click(function () {
                fncLimpiarCELineaNegocio()
                btnCELineaNegocio.data().idLineaNegocio = 0
                btnCELineaNegocio.html(`<i class='fas fa-save'></i>&nbsp;Guardar`)
                mdlCELineaNegocio.modal("show")
            })
            //#endregion

            //#region FUNCIONES CREAR/EDITAR LINEA DE NEGOCIO
            btnCELineaNegocio.click(function () {
                fncCELineaNegocio()
            })
            //#endregion

            //#region FUNCIONES RELACIONAR CC A LA LINEA DE NEGOCIO
            btnCELineaNegocioRelCC.click(function () {
                fncCELineaNegocioRelCC()
            })
            //#endregion
        }

        //#region FUNCIONES CREAR/EDITAR LINEA DE NEGOCIO
        function initTblLineaNegocios() {
            dtLineaNegocios = tblLineaNegocios.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'concepto', title: 'Linea de negocio' },
                    { data: 'abreviacion', title: 'Abreviacion' },
                    {
                        title: 'CC',
                        render: (data, type, row, meta) => {
                            return `<button class="btn btn-xs btn-primary lstCC" title="Listado de CC."><i class="fas fa-list"></i></button>`
                        }
                    },
                    {
                        title: "Opciones",
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `${btnEditar} ${btnEliminar}`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblLineaNegocios.on('click', '.editarRegistro', function () {
                        let rowData = dtLineaNegocios.row($(this).closest('tr')).data();
                        fncLimpiarCELineaNegocio()
                        fncGetDatosActualizarLineaNegocio(rowData.id);
                    });

                    tblLineaNegocios.on('click', '.eliminarRegistro', function () {
                        let rowData = dtLineaNegocios.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarLineaNegocio(rowData.id));
                    });

                    tblLineaNegocios.on('click', '.lstCC', function () {
                        let rowData = dtLineaNegocios.row($(this).closest('tr')).data();
                        btnCELineaNegocioRelCC.data().idLineaNegocio = rowData.id
                        fncGetLineaNegociosRelCC()
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '5%', targets: [2] },
                    { width: '7%', targets: [3] },
                ],
            });
        }

        function fncGetLineaNegocios() {
            axios.post('GetLineaNegocios').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtLineaNegocios.clear();
                    dtLineaNegocios.rows.add(response.data.lstLineaNegocios);
                    dtLineaNegocios.draw();
                    //#endregion
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCELineaNegocio() {
            let obj = fncCEOBJLineaNegocio()
            if (obj != "") {
                axios.post('CELineaNegocio', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetLineaNegocios()
                        mdlCELineaNegocio.modal("hide")
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJLineaNegocio() {
            let mensajeError = ""
            if (txtCE_LineaNegocio.val() == "") { txtCE_LineaNegocio.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_LineaNegocio") }
            if (txtCE_LineaNegocioAbreviacion.val() == "") { txtCE_LineaNegocioAbreviacion.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_LineaNegocioAbreviacion") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return ""
            } else {
                let obj = new Object()
                obj.id = btnCELineaNegocio.data().idLineaNegocio
                obj.concepto = txtCE_LineaNegocio.val()
                obj.abreviacion = txtCE_LineaNegocioAbreviacion.val()
                return obj
            }
        }

        function fncGetDatosActualizarLineaNegocio(idLineaNegocio) {
            if (idLineaNegocio > 0) {
                let obj = {}
                obj.id = idLineaNegocio
                axios.post('GetDatosActualizarLineaNegocio', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCELineaNegocio.data().idLineaNegocio = idLineaNegocio
                        txtCE_LineaNegocio.val(response.data.objLineaNegocio.concepto)
                        txtCE_LineaNegocioAbreviacion.val(response.data.objLineaNegocio.abreviacion)

                        btnCELineaNegocio.html(`<i class='fas fa-save'></i>&nbsp;Actualizar`)
                        mdlCELineaNegocio.modal("show")
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener la información de la línea de negocio.")
            }
        }

        function fncEliminarLineaNegocio(idLineaNegocio) {
            if (idLineaNegocio > 0) {
                let obj = {}
                obj.id = idLineaNegocio
                axios.post('EliminarLineaNegocio', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetLineaNegocios()
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al eliminar el registro.")
            }
        }

        function fncLimpiarCELineaNegocio() {
            $("input[type='text']").val("");
        }
        //#endregion

        //#region FUNCIONES RELACIONAR CC A LA LINEA DE NEGOCIO
        function initTblLineaNegociosRelCC() {
            dtLineaNegociosRelCC = tblLineaNegociosRelCC.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'ccDescripcion', title: 'CC' },
                    {
                        title: "Estatus",
                        render: (data, type, row, meta) => {
                            if (row.registroActivoCC) {
                                return `<input type="checkbox" checked="checked" onclick="return false">`
                            } else {
                                return `<input type="checkbox" onclick="return false">`
                            }
                        }
                    },
                    {
                        title: "Opciones",
                        render: function (data, type, row, meta) {
                            return `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblLineaNegociosRelCC.on('click', '.eliminarRegistro', function () {
                        let rowData = dtLineaNegociosRelCC.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarLineaNegocioRelCC(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '5%', targets: [1] }
                ],
            });
        }

        function fncGetLineaNegociosRelCC() {
            if (btnCELineaNegocioRelCC.data().idLineaNegocio > 0) {
                let obj = {}
                obj.FK_LineaNegocio = btnCELineaNegocioRelCC.data().idLineaNegocio
                axios.post('GetLineaNegociosRelCC', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtLineaNegociosRelCC.clear();
                        dtLineaNegociosRelCC.rows.add(response.data.lstLineaNegocioDetDTO);
                        dtLineaNegociosRelCC.draw();
                        fncFillCboCCDisponibles()
                        mdlCELineaNegocioRelCC.modal("show")
                        //#endregion
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener el listado de CC.")
            }
        }

        function fncCELineaNegocioRelCC() {
            let obj = fncCEOBJLineaNegocioRelCC()
            if (obj != "") {
                axios.post('CELineaNegocioRelCC', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetLineaNegociosRelCC()
                        fncFillCboCCDisponibles()
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJLineaNegocioRelCC() {
            let mensajeError = ""
            if (cboCE_CC.val() < 0) { $("#select2-cboCE_CC-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCE_CC-container") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return ""
            } else {
                let obj = new Object()
                obj.FK_LineaNegocio = btnCELineaNegocioRelCC.data().idLineaNegocio
                obj.lstCC = cboCE_CC.val()
                return obj
            }
        }

        function fncEliminarLineaNegocioRelCC(idLineaNegocioRelCC) {
            if (idLineaNegocioRelCC > 0) {
                let obj = {}
                obj.id = idLineaNegocioRelCC
                axios.post('EliminarLineaNegocioRelCC', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetLineaNegociosRelCC()
                        fncFillCboCCDisponibles()
                    } else {
                        Alert2Warning(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al eliminar la relación del CC seleccionado.")
            }
        }

        function fncFillCboCCDisponibles() {
            if (btnCELineaNegocioRelCC.data().idLineaNegocio > 0) {
                cboCE_CC.fillCombo('FillCboCCDisponibles', { FK_LineaNegocio: btnCELineaNegocioRelCC.data().idLineaNegocio }, false, null);
            } else {
                Alert2Warning("Ocurrió un error al obtener el listado de CC disponibles.")
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncGetAccesosMenu() {
            axios.post('GetAccesosMenu').then(response => {
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
    }

    $(document).ready(() => {
        CH.LineaNegocio = new LineaNegocio();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();