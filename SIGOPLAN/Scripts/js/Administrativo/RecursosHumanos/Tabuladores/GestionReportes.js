(() => {
    $.namespace('CH.GestionReportes');

    //#region CONST FILTROS
    const cboFiltroGestionEstatus = $('#cboFiltroGestionEstatus')
    const btnFiltroBuscar = $('#btnFiltroBuscar')
    const cboFiltroGestionAño = $('#cboFiltroGestionAño');
    const btnFiltroAutorizar = $('#btnFiltroAutorizar');
    //#endregion

    //#region CONST REPORTE
    let dtGestionReportes;
    const tblGestionReporte = $('#tblGestionReporte');
    let dtPlantillaDetalle;
    const tablaPlantillaDetalle = $('#tablaPlantillaDetalle');
    const mdlReporte = $('#mdlReporte');

    const tblTabuladores = $('#tblTabuladores');
    let dtTabuladores;
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

    GestionReportes = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT

            initTblFirmas();

            initTblGestionReporte();
            initTblTabuladores();
            // fncGetTabuladoresReporte();
            initTblPlantillaDetalle();
            initTblDetallePlantillaTabuladores();
            fncGetAccesosMenu();
            $("#menuGestionReportes").addClass("opcionSeleccionada")
            //#endregion

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                if (cboFiltroGestionEstatus.val() != "") {
                    fncGetGestionReportes()
                } else {
                    Alert2Warning("Es necesario seleccionar un filtro.")
                    return "";
                }
            })

            btnFiltroAutorizar.click(function () {
                fncAutorizarMasivo();
            });
            //#endregion

            //#region FILL COMBOS
            cboFiltroGestionEstatus.fillCombo('FillCboGestionEstatus', null, false, null);
            $(".select2").select2()
            fncBusquedaDefault()
            //#endregion
        }

        //#region GESTION REPORTES
        function fncBusquedaDefault() {
            cboFiltroGestionEstatus.val(0)
            cboFiltroGestionEstatus.trigger("change")
            btnFiltroBuscar.trigger("click")
        }

        function initTblGestionReporte() {
            dtGestionReportes = tblGestionReporte.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                select: true,
                columns: [
                    { data: 'id', title: '' },
                    { data: 'año', title: 'AÑO' },
                    // { data: 'lineaNegociosCC', title: 'Linea Negocios' },
                    { data: 'descLN', title: 'LINEA DE NEGOCIOS' },
                    {
                        render: function (data, type, row, meta) {
                            let btnAutorizar = `<button class='btn btn-xs btn-success autorizar' title='Autorizar reporte.'><i class="fas fa-thumbs-up"></i></button>`
                            let btnRechazar = `<button class='btn btn-xs btn-danger rechazar' title='Rechazar reporte.'><i class="fas fa-thumbs-down"></i></button>`
                            let btnLstFirmas = `<button class='btn btn-xs btn-primary lstFirmas' title='Ver firmas.'><i class="fas fa-signature"></i></button>`
                            let btnComentario = `<button class='btn btn-xs btn-primary verComentario' title='Ver comentario rechazo.'><i class="far fa-comments"></i></button>`
                            let btnDetalle = `<button class='btn btn-xs btn-default detalle' title='Ver detalle.'><i class="fas fa-list"></i></button>`;
                            let btnDetalleReporte = `<button class='btn btn-xs btn-default detalleReporte' title='Ver detalle reporte.'><i class="fas fa-chart-bar"></i></button>`;
                            let btnReport = `<button title="Imprimir plantilla" class="btn btn-primary btn-xs imprimirDoc"><i class="fas fa-print"></i></button>`;

                            let botones = "";
                            if (row.estatus == 0) {
                                if (row.esFirmar) {
                                    botones = `${btnLstFirmas} ${btnAutorizar} ${btnRechazar} ${btnReport}`
                                } else {
                                    botones = `${btnLstFirmas} ${btnReport}`
                                }
                            } else {
                                if (row.estatus == 2) {
                                    botones = `${btnComentario} ${btnReport}`
                                } else {
                                    botones = `${btnReport}`
                                }
                            }
                            botones += ` ${btnDetalleReporte}`;
                            return botones;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblGestionReporte.on('click', '.lstFirmas', function () {
                        let rowData = dtGestionReportes.row($(this).closest('tr')).data();
                        mdlFirmas.modal("show");
                        fncGetLstAutorizantesReporte(rowData.id);
                    });

                    tblGestionReporte.on('click', '.verComentario', function () {
                        let rowData = dtGestionReportes.row($(this).closest('tr')).data();
                        mdlComentario.modal("show");
                        txtComentario.val(rowData.comentarioRechazo);
                    });

                    tblGestionReporte.on("click", ".detallePlantilla", function () {
                        let rowData = dtGestionReportes.row($(this).closest('tr')).data();
                        fncGetDetallePlantillaTabuladores(rowData.FK_LineaNegocio);
                    })

                    tblGestionReporte.on('click', '.autorizar', function () {
                        let rowData = dtGestionReportes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Autorizar plantilla', '¿Desea autorizar la plantilla seleccionada?', 'Confirmar', 'Cancelar', () => fncAutorizarRechazarReporte(rowData.id, 1, rowData.FK_LineaNegocio));
                    });

                    tblGestionReporte.on('click', '.rechazar', function () {
                        let rowData = dtGestionReportes.row($(this).closest('tr')).data();
                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar reporte",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar el reporte seleccionado?<br>Indicar el motivo:</h3>",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncGuardarComentarioRechazoReporte(rowData.id, $('.swal2-textarea').val());
                            }
                        });
                    });

                    tblGestionReporte.on('click', '.detalleReporte', function () {
                        let rowData = dtGestionReportes.row($(this).closest('tr')).data();

                        fncGetTabuladoresReporte(rowData.FK_LineaNegocio, rowData.año);
                    });

                    tblGestionReporte.on('click', '.imprimirDoc', function () {
                        let rowData = dtGestionReportes.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea imprimir la plantilla seleccionada?', 'Confirmar', 'Cancelar', () => fncGetParamsDTO_PDF(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { className: 'dt-body-center', 'targets': '_all' },
                    {
                        targets: 0,
                        checkboxes: {
                            selectRow: true
                        }
                    }
                ],
                select: {
                    style: 'multi',
                    selector: 'td:first-child'
                },
            });
        }

        function fncGetParamsDTO_PDF(idReporte) {
            axios.post('GetParametrosReporte', { idReporte }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $.blockUI({ message: "Cargando información..." });
                    var path = `/Reportes/Vista.aspx?idReporte=289`;
                    $("#report").attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDetallePlantillaTabuladores(FK_LineaNegocio) {
            if (codeCC != "") {
                let objParamDTO = {}
                objParamDTO.FK_LineaNegocio = FK_LineaNegocio
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

        function fncGetGestionReportes() {
            let obj = {}
            obj.estatus = cboFiltroGestionEstatus.val();
            obj.año = cboFiltroGestionAño.val();

            axios.post('GetGestionReportes', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtGestionReportes.clear();
                    dtGestionReportes.rows.add(items);
                    dtGestionReportes.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAutorizarRechazarReporte(id, estatus, cc) {
            if (id > 0 && estatus > 0) {
                let obj = {}
                obj.id = id
                obj.estatus = estatus
                axios.post('AutorizarRechazarReporte', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        // if (response.data.esAuthCompleta) {
                        //     var path = `/Reportes/Vista.aspx?idReporte=104&plantillaID=0&plantillaCC=${cc}&inMemory=1&esTabulador=1`;
                        //     let messageGuardar = message;
                        //     $("#report").attr("src", path);
                        //     document.getElementById('report').onload = function () {//
                        //         axios.post("NotificarPlantilla", { ccPlantilla: cc, esAuthCompleta: true }).then(response => {
                        //             let { success, items, message } = response.data;
                        //             if (success) {
                        //                 Alert2Exito(messageGuardar)
                        //             }
                        //         }).catch(error => Alert2Error(error.message));
                        //     };
                        // } else {
                        //     var path = `/Reportes/Vista.aspx?idReporte=104&plantillaID=0&plantillaCC=${cc}&inMemory=1&esTabulador=1`;
                        //     let messageGuardar = message;
                        //     $("#report").attr("src", path);
                        //     document.getElementById('report').onload = function () {//
                        //         axios.post("NotificarPlantilla", { ccPlantilla: cc, esAuthCompleta: false }).then(response => {
                        //             let { success, items, message } = response.data;
                        //             if (success) {
                        //                 Alert2Exito(messageGuardar);
                        //             }
                        //         }).catch(error => Alert2Error(error.message));
                        //     };
                        // }

                        if (estatus == 1) {
                            Alert2Exito("Reporte autorizado con exito")

                        } else {
                            Alert2Exito("Reporte rechazado con exito")

                        }
                        fncGetGestionReportes()
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

        function fncGetLstAutorizantesReporte(idReporte) {
            axios.post("GetLstAutorizantesReporte", { idReporte }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtFirmas.clear();
                    dtFirmas.rows.add(items);
                    dtFirmas.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarComentarioRechazoReporte(idReporte, comentario) {
            axios.post("GuardarComentarioRechazoReporte", { idReporte, comentario }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncAutorizarRechazarReporte(idReporte, 2);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblTabuladores() {
            dtTabuladores = tblTabuladores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'idPuesto', title: 'Id' },
                    { data: 'puestoDesc', title: 'Puesto' },
                    { data: 'lineaNegocioDesc', title: 'LN' },
                    { data: 'categoriaDesc', title: 'Cat' },
                    { data: 'descAreaDepartamento', title: 'Area/Departamento' },
                    { data: 'descSindicato', title: 'Sindicato' },
                    { data: 'tipoNominaDesc', title: 'Tipo nómina' },
                    { data: 'sueldoBaseStringActual', title: 'Sueldo base' },
                    { data: 'complementoStringActual', title: 'Complemento' },
                    { data: 'totalNominalStringActual', title: 'Total nominal' },
                    { data: 'sueldoMensualStringActual', title: 'Total mensual' },

                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ]
            });
        }

        function fncGetTabuladoresReporte(FK_LineaNegocio, year) {
            let objParamDTO = {}
            objParamDTO.FK_LineaNegocio = FK_LineaNegocio;
            objParamDTO.añoReporte = year ? year : cboFiltroGestionAño.val();

            axios.post('GetTabuladoresReporteByCC', objParamDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtTabuladores.clear();
                    dtTabuladores.rows.add(response.data.lstTabPuestos);
                    dtTabuladores.draw();
                    mdlReporte.modal("show");
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAutorizarMasivo() {
            let lstIdReportes = [];
            let rowsChecked = dtGestionReportes.column(0).checkboxes.selected();

            $.each(rowsChecked, function (index, id) {
                lstIdReportes.push(id);
            });

            console.log(lstIdReportes);

            if (lstIdReportes.length > 0) {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea autorizar los reportes seleccionados?', 'Confirmar', 'Cancelar', () => fncAutorizarReportesMasivo(lstIdReportes));

            } else {
                Alert2Warning("Debe seleccionar al menos un reporte (Linea de Negocios)");

            }
        }

        function fncAutorizarReportesMasivo(lstIdReportes) {
            axios.post("AutorizarReportesMasivo", { lstIdReportes }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Reportes autorizados con exito");

                    if (cboFiltroGestionEstatus.val() != "") {
                        fncGetGestionReportes()
                    } else {
                        Alert2Warning("Es necesario seleccionar un filtro.")
                        return "";
                    }
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
        CH.GestionReportes = new GestionReportes();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();;