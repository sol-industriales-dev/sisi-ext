(() => {
    $.namespace('CH.GestionPlantillasPersonal');

    //#region CONST FILTROS
    const cboFiltroGestionEstatus = $('#cboFiltroGestionEstatus')
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    //#endregion

    //#region CONST TABULADORES
    let dtPlantillasPersonal;
    const tblPlantillasPersonal = $('#tblPlantillasPersonal');
    let dtPlantillaDetalle;
    const tablaPlantillaDetalle = $('#tablaPlantillaDetalle');
    //#endregion

    //#region CONST FIRMAS
    let dtFirmas
    const tblFirmas = $('#tblFirmas')
    const mdlFirmas = $('#mdlFirmas')
    const mdlComentario = $('#mdlComentario')
    const txtComentario = $('#txtComentario')
    let estatusEnum = ["-", "AUTORIZADO", "-"]
    //#endregion

    //#region CONST DETALLE PLANTILLA
    let dtDetallePlantillaTabuladores;
    const mdlDetallePlantilla = $("#mdlDetallePlantilla");
    const tblDetallePlantilla = $("#tblDetallePlantilla");
    //#endregion

    const modalPlantillaDetalle = $('#modalPlantillaDetalle');
    const mdlCargando = $('#mdlCargando');

    GestionPlantillasPersonal = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblPlantillasPersonal();
            initTblPlantillaDetalle();
            initTblFirmas();
            initTblDetallePlantillaTabuladores();
            fncGetAccesosMenu();
            $("#menuGestionPlantillasPersonal").addClass("opcionSeleccionada");
            //#endregion

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                if (cboFiltroGestionEstatus.val() != "") {
                    fncGetGestionPlantillasPersonal();
                } else {
                    Alert2Warning("Es necesario seleccionar un filtro.");
                    return "";
                }
            })
            //#endregion

            //#region FILL COMBOS
            cboFiltroGestionEstatus.fillCombo('FillCboGestionEstatus', null, false, null);
            $(".select2").select2();
            fncBusquedaDefault();
            //#endregion
        }

        //#region GESTION TABULADORES
        function fncBusquedaDefault() {
            cboFiltroGestionEstatus.val(0)
            cboFiltroGestionEstatus.trigger("change")
            btnFiltroBuscar.trigger("click")
        }

        function initTblPlantillasPersonal() {
            dtPlantillasPersonal = tblPlantillasPersonal.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'cc', title: 'CC' },
                    { data: 'personalNecesario', title: 'Personal Solicitado' },
                    { data: 'personalExistente', title: 'Personal Existente' },
                    { data: 'porContratar', title: 'Por Contratar' },
                    {
                        render: function (data, type, row, meta) {
                            let btnAutorizar = `<button class='btn btn-xs btn-success autorizar' title='Autorizar plantilla.'><i class="fas fa-thumbs-up"></i></button>`
                            let btnRechazar = `<button class='btn btn-xs btn-danger rechazar' title='Rechazar plantilla.'><i class="fas fa-thumbs-down"></i></button>`
                            let btnLstFirmas = `<button class='btn btn-xs btn-primary lstFirmas' title='Ver firmas.'><i class="fas fa-signature"></i></button>`
                            let btnComentario = `<button class='btn btn-xs btn-primary verComentario' title='Ver comentario rechazo.'><i class="far fa-comments"></i></button>`
                            let btnDetalle = `<button class='btn btn-xs btn-default detalle' title='Ver detalle.'><i class="fas fa-list"></i></button>`;
                            let btnDetallePlantilla = `<button class='btn btn-xs btn-default detallePlantilla' title='Ver detalle plantilla.'><i class="fas fa-chart-bar"></i></button>`;
                            let btnReport = `<button title="Imprimir plantilla" class="btn btn-primary btn-xs imprimirDoc"><i class="fas fa-print"></i></button>`;

                            let botones = "";
                            if (row.plantillaAutorizada == 0) {
                                if (row.esFirmar) {
                                    botones = `${btnLstFirmas} ${btnAutorizar} ${btnRechazar} ${btnDetalle} ${btnReport}`
                                } else {
                                    botones = `${btnLstFirmas} ${btnDetalle} ${btnReport}`
                                }
                            } else {
                                if (row.plantillaAutorizada == 2) {
                                    botones = `${btnComentario} ${btnDetalle} ${btnReport}`
                                } else {
                                    botones = `${btnDetalle} ${btnReport}`
                                }
                            }
                            botones += ` ${btnDetallePlantilla}`;
                            return botones;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblPlantillasPersonal.on('click', '.lstFirmas', function () {
                        let rowData = dtPlantillasPersonal.row($(this).closest('tr')).data();
                        mdlFirmas.modal("show");
                        fncGetLstAutorizantesPlantilla(rowData.id);
                    });

                    tblPlantillasPersonal.on('click', '.verComentario', function () {
                        let rowData = dtPlantillasPersonal.row($(this).closest('tr')).data();
                        mdlComentario.modal("show");
                        txtComentario.val(rowData.comentarioRechazo);
                    });

                    tblPlantillasPersonal.on("click", ".detallePlantilla", function () {
                        let rowData = dtPlantillasPersonal.row($(this).closest('tr')).data();
                        fncGetDetallePlantillaTabuladores(rowData.codeCC);
                    })

                    tblPlantillasPersonal.on('click', '.autorizar', function () {
                        let rowData = dtPlantillasPersonal.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Autorizar plantilla', '¿Desea autorizar la plantilla seleccionada?', 'Confirmar', 'Cancelar', () => fncAutorizarRechazarPlantillaPersonal(rowData.id, 1, rowData.codeCC));
                    });

                    tblPlantillasPersonal.on('click', '.rechazar', function () {
                        let rowData = dtPlantillasPersonal.row($(this).closest('tr')).data();
                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar plantilla",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar la plantilla seleccionada?<br>Indicar el motivo:</h3>",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncGuardarComentarioRechazoPlantilla(rowData.id, $('.swal2-textarea').val());
                            }
                        });
                    });

                    tblPlantillasPersonal.on('click', '.detalle', function () {
                        let rowData = dtPlantillasPersonal.row($(this).closest('tr')).data();

                        axios.post('GetPlantillaDetalle', { plantilla_id: rowData.id }).then(response => {
                            let { success, data, message } = response.data;

                            if (success) {
                                dtPlantillaDetalle.clear();
                                dtPlantillaDetalle.rows.add(response.data.data);
                                dtPlantillaDetalle.draw();
                                modalPlantillaDetalle.modal('show');
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });

                    tblPlantillasPersonal.on('click', '.imprimirDoc', function () {
                        let rowData = dtPlantillasPersonal.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea imprimir la plantilla seleccionada?', 'Confirmar', 'Cancelar', () => {
                            $.blockUI({ message: "Cargando información..." });
                            var path = `/Reportes/Vista.aspx?idReporte=104&plantillaID=${0}&pendiente=${true}&plantillaCC=${rowData.codeCC}&esTabulador=1`;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {
                                $.unblockUI();
                                openCRModal();
                            };
                        });
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    { width: '15%', targets: [1] },
                    { width: '15%', targets: [2] },
                    { width: '15%', targets: [3] },
                    { width: '10%', targets: [4] }
                ],
            });
        }

        function fncGetDetallePlantillaTabuladores(codeCC) {
            if (codeCC != "") {
                let objParamDTO = {}
                objParamDTO.cc = codeCC
                axios.post('GetDetallePlantillaTabuladores', objParamDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtDetallePlantillaTabuladores.clear();
                        dtDetallePlantillaTabuladores.rows.add(response.data.lstDetalleDTO);
                        dtDetallePlantillaTabuladores.draw();
                        mdlDetallePlantilla.modal("show");
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
            else {
                Alert2Error("Ocurrió un error al obtener el detalle de la plantilla.");
            }
        }

        function initTblDetallePlantillaTabuladores() {
            dtDetallePlantillaTabuladores = tblDetallePlantilla.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: true,
                columns: [
                    { data: 'puestoDesc', title: 'Puesto' },
                    { data: 'categoriaDesc', title: 'Categoría' },
                    { data: 'departamentoDesc', title: 'Depto.' },
                    { data: 'nominaDesc', title: 'Nómina' },
                    { data: 'personalNecesario', title: 'Personal necesario' },
                    { data: 'sueldoBaseDesc', title: 'Sueldo base' },
                    { data: 'complementoDesc', title: 'Complemento' },
                    { data: 'totalNominalDesc', title: 'Total nómina' },
                    { data: 'sueldoMensualDesc', title: 'Sueldo mensual' },
                    { data: 'esquemaPagoDesc', title: 'Esquema de pago' },
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblPlantillaDetalle() {
            dtPlantillaDetalle = tablaPlantillaDetalle.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                columns: [
                    { data: 'puestoDesc', title: 'Puesto' },
                    { data: 'personalNecesario', title: 'Personal Solicitado' },
                    { data: 'personalExistente', title: 'Personal Existente' },
                    { data: 'porContratar', title: 'Por Contratar' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '5%', targets: [0] }
                ]
            });
        }

        function fncGetGestionPlantillasPersonal() {
            let obj = {}
            obj.plantillaAutorizada = cboFiltroGestionEstatus.val()
            axios.post('GetGestionPlantillasPersonal', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtPlantillasPersonal.clear();
                    dtPlantillasPersonal.rows.add(response.data.lstPlantillasPersonalDTO);
                    dtPlantillasPersonal.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAutorizarRechazarPlantillaPersonal(id, plantillaAutorizada, cc) {
            if (id > 0 && plantillaAutorizada > 0) {
                let obj = {};
                obj.id = id;
                obj.plantillaAutorizada = plantillaAutorizada;
                mdlCargando.modal("show");
                axios.post('AutorizarRechazarPlantillaPersonal', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (response.data.esAuthCompleta) {
                            var path = `/Reportes/Vista.aspx?idReporte=104&plantillaID=0&plantillaCC=${cc}&inMemory=1&esTabulador=1`;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {//
                                axios.post("NotificarPlantilla", { ccPlantilla: cc, esAuthCompleta: true }).then(response => {
                                    let { success, items, message } = response.data;
                                    if (success) {
                                        mdlCargando.modal("hide");
                                    }
                                }).catch(error => Alert2Error(error.message));
                            };
                        } else {
                            var path = `/Reportes/Vista.aspx?idReporte=104&plantillaID=0&plantillaCC=${cc}&inMemory=1&esTabulador=1`;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {
                                axios.post("NotificarPlantilla", { ccPlantilla: cc, esAuthCompleta: false }).then(response => {
                                    let { success, items, message } = response.data;
                                    if (success) {
                                        mdlCargando.modal("hide");
                                    }
                                }).catch(error => Alert2Error(error.message));
                            };
                        }

                        Alert2Exito(message);
                        fncGetGestionPlantillasPersonal();
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
                    { data: 'nombreAutorizante', title: 'USUARIO' },
                    {
                        data: 'autorizado', title: 'ESTATUS',
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

        function fncGetLstAutorizantesPlantilla(idPlantilla) {
            axios.post("GetLstAutorizantesPlantilla", { idPlantilla }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtFirmas.clear();
                    dtFirmas.rows.add(items);
                    dtFirmas.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarComentarioRechazoPlantilla(idPlantilla, comentario) {
            axios.post("GuardarComentarioRechazoPlantilla", { idPlantilla, comentario }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncAutorizarRechazarPlantillaPersonal(idPlantilla, 2);
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
        CH.GestionPlantillasPersonal = new GestionPlantillasPersonal();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();