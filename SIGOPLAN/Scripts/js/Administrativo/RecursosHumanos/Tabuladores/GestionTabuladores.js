(() => {
    $.namespace('CH.GestionTabuladores');

    //#region CONST FILTROS
    const cboFiltroGestionEstatus = $('#cboFiltroGestionEstatus')
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    //#endregion

    //#region CONST TABULADORES
    let dtTabuladores
    const tblTabuladores = $('#tblTabuladores')
    //#endregion

    //#region CONST FIRMAS
    let dtFirmas
    const tblFirmas = $('#tblFirmas')
    const mdlFirmas = $('#mdlFirmas')
    const mdlComentario = $('#mdlComentario')
    const txtComentario = $('#txtComentario')
    let estatusEnum = ["-", "AUTORIZADO", "-"]
    //#endregion

    //#region CONST TABULADOR DETALLE
    let dtTabuladoresDet
    const tblTabuladoresDet = $("#tblTabuladoresDet")
    const mdlTabuladorDetalle = $("#mdlTabuladorDetalle")
    //#endregion

    GestionTabuladores = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblTabuladores()
            initTblFirmas()
            initTblTabuladorDetalle()
            fncGetAccesosMenu();
            $("#menuGestionTabuladores").addClass("opcionSeleccionada")
            //#endregion

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                fncDefaultCtrls("select2-cboFiltroGestionEstatus-container")
                if (cboFiltroGestionEstatus.val() != "") {
                    fncGetGestionTabuladores()
                } else {
                    Alert2Warning("Es necesario seleccionar un filtro.")
                    $("#select2-cboFiltroGestionEstatus-container").css('border', '2px solid red')
                }
            })
            //#endregion

            //#region FILL COMBOS
            cboFiltroGestionEstatus.fillCombo('FillCboGestionEstatus', null, false, null);
            $(".select2").select2()
            fncBusquedaDefault()
            //#endregion
        }

        //#region GESTION TABULADORES
        function fncBusquedaDefault() {
            cboFiltroGestionEstatus.val(0)
            cboFiltroGestionEstatus.trigger("change")
            btnFiltroBuscar.trigger("click")
        }

        function initTblTabuladores() {
            dtTabuladores = tblTabuladores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'puestoDesc', title: 'Puesto', render: (data, type, row, meta) => {
                            return "[" + row.FK_Puesto + "] " + data;
                        }
                    },
                    {
                        data: 'lstDescLineaNegocio', title: 'Línea de Negocio',
                        render: (data, type, row, meta) => {
                            let lineasNegocio = "";
                            for (const item of data) {
                                lineasNegocio += (item + ", ");
                            }
                            return lineasNegocio.substring(0, lineasNegocio.length - 2);
                        }
                    },
                    {
                        title: 'Detalle',
                        render: function (data, type, row, meta) {
                            let btnAutorizar = `<button class='btn btn-xs btn-success autorizar' title='Autorizar tabulador.'><i class="fas fa-thumbs-up"></i></button>`
                            let btnRechazar = `<button class='btn btn-xs btn-danger rechazar' title='Rechazar tabulador.'><i class="fas fa-thumbs-down"></i></button>`
                            let btnLstFirmas = `<button class='btn btn-xs btn-primary lstFirmas' title='Ver firmas.'><i class="fas fa-signature"></i></button>`
                            let btnComentario = `<button class='btn btn-xs btn-primary verComentario' title='Ver comentario rechazo.'><i class="far fa-comments"></i></button>`

                            let botones = ""
                            if (row.tabuladorAutorizado == 0 || row.tabuladorDetAutorizado) {
                                if (row.esFirmar && cboFiltroGestionEstatus.val() == 0) {
                                    botones = `${btnLstFirmas} ${btnAutorizar} ${btnRechazar}`
                                } else {
                                    botones = `${btnLstFirmas}`
                                }
                            } else {
                                if (row.tabuladorAutorizado == 2) {
                                    botones = `${btnComentario}`
                                } else {
                                    botones = ``
                                }
                            }
                            botones += ` <button class="btn btn-xs btn-primary tabuladorDetalle" title="Detalle"><i class="fas fa-stream"></i></button>`
                            return botones
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblTabuladores.on('click', '.lstFirmas', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                        mdlFirmas.modal("show");
                        fncGetLstAutorizantesTabulador(rowData.id);
                    });

                    tblTabuladores.on('click', '.verComentario', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                        mdlComentario.modal("show");
                        txtComentario.val(rowData.comentarioRechazo);
                    });

                    tblTabuladores.on('click', '.autorizar', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Autorizar tabulador', '¿Desea autorizar el tabulador seleccionado?', 'Confirmar', 'Cancelar', () => fncAutorizarRechazarTabulador(rowData.id, 1));
                    });

                    tblTabuladores.on('click', '.rechazar', function () {
                        let rowData = dtTabuladores.row($(this).closest('tr')).data();
                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar tabulador",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar el tabulador seleccionado?<br>Indicar el motivo:</h3>",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncGuardarComentarioRechazoTabulador(rowData.id, $('.swal2-textarea').val());
                            }
                        });
                    })

                    tblTabuladores.on("click", ".tabuladorDetalle", function () {
                        let rowData = dtTabuladores.row($(this).closest("tr")).data();
                        fncGetTabuladorDetalle(rowData.id)
                    })
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '20%', targets: [2] },
                ],
            });
        }

        function fncGetGestionTabuladores() {
            let obj = {}
            obj.tabuladorAutorizado = cboFiltroGestionEstatus.val()
            axios.post('GetGestionTabuladores', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtTabuladores.clear();
                    dtTabuladores.rows.add(response.data.lstTabuladoresDTO);
                    dtTabuladores.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAutorizarRechazarTabulador(id, tabuladorAutorizado) {
            if (id > 0 && tabuladorAutorizado > 0) {
                let obj = {}
                obj.id = id
                obj.tabuladorAutorizado = tabuladorAutorizado
                axios.post('AutorizarRechazarTabulador', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetGestionTabuladores()
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al realizar la acción.")
            }
        }

        function initTblFirmas() {
            dtFirmas = tblFirmas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreAutorizante', title: 'Autorizante' },
                    {
                        data: 'autorizado', title: 'Estatus',
                        render: function (data, type, row) {
                            return estatusEnum[data];
                        }
                    },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetLstAutorizantesTabulador(FK_Tabulador) {
            if (FK_Tabulador > 0 && cboFiltroGestionEstatus.val() != "") {
                let obj = {};
                obj.FK_Tabulador = FK_Tabulador;
                obj.autorizado = cboFiltroGestionEstatus.val()
                axios.post("GetLstAutorizantesTabulador", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        dtFirmas.clear();
                        dtFirmas.rows.add(response.data.lstAutorizantes);
                        dtFirmas.draw();
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener el listado de autorizantes.");
            }
        }

        function fncGuardarComentarioRechazoTabulador(idTabulador, comentario) {
            axios.post("GuardarComentarioRechazoTabulador", { idTabulador, comentario }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncAutorizarRechazarTabulador(idTabulador, 2);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblTabuladorDetalle() {
            dtTabuladoresDet = tblTabuladoresDet.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'lineaNegocioDesc', title: 'Línea de negocio' },
                    { data: 'categoriaDesc', title: 'Categoría' },
                    {
                        title: "Sueldo base",
                        render: (data, type, row, meta) => {
                            return maskNumero2DCompras(row.sueldoBase)
                        }
                    },
                    {
                        title: "Complemento",
                        render: (data, type, row, meta) => {
                            return maskNumero2DCompras(row.complemento)
                        }
                    },
                    {
                        title: "Total nominal",
                        render: (data, type, row, meta) => {
                            return maskNumero2DCompras(row.totalNominal)
                        }
                    },
                    {
                        title: "Sueldo mensual",
                        render: (data, type, row, meta) => {
                            return maskNumero2DCompras(row.sueldoMensual)
                        }
                    },
                    { data: 'esquemaPagoDesc', title: "Esquema de pago" },
                    { data: 'tabuladorDetAutorizadoDesc', title: 'Estatus' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', 'targets': '_all' },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncGetTabuladorDetalle(FK_Tabulador) {
            let objParamDTO = {}
            objParamDTO.FK_Tabulador = FK_Tabulador;
            objParamDTO.tabuladorDetAutorizado = cboFiltroGestionEstatus.val();
            axios.post('GetDetalleRelTabulador', objParamDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtTabuladoresDet.clear();
                    dtTabuladoresDet.rows.add(response.data.lstTabuladorDet);
                    dtTabuladoresDet.draw();
                    mdlTabuladorDetalle.modal("show")
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
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
        CH.GestionTabuladores = new GestionTabuladores();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();