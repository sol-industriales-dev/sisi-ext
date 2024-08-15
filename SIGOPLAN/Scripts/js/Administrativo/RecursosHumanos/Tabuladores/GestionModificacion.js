(() => {
    $.namespace('CH.GestionModificacion');

    //#region CONST FILTROS
    const cboFiltroGestionEstatus = $('#cboFiltroGestionEstatus');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    //#endregion

    //#region CONST TABULADORES
    let dtGestionModificacion;
    const tblGestionModificacion = $('#tblGestionModificacion');
    //#endregion

    //#region CONST FIRMAS
    let dtFirmas;
    const tblFirmas = $('#tblFirmas');
    const mdlFirmas = $('#mdlFirmas');
    const mdlComentario = $('#mdlComentario');
    const txtComentario = $('#txtComentario');
    let estatusEnum = ["-", "AUTORIZADO", "-"];
    //#endregion

    //#region CONST TABULADORES DETALLE
    let dtTabEmpleadosActivos;
    const tblTabEmpleadosActivos = $("#tblTabEmpleadosActivos");
    const lblEmpleadosPrimerTab = $("#lblEmpleadosPrimerTab");
    const lblEmpleadosSegundoTab = $("#lblEmpleadosSegundoTab");

    let dtTabPuestos;
    const tblTabPuestos = $("#tblTabPuestos");

    const mdlModificacion = $("#mdlModificacion");
    const divTabEmpleadosActivos = $("#divTabEmpleadosActivos");
    const divTabPuestos = $("#divTabPuestos");
    //#endregion

    GestionModificacion = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblGestionModificacion();
            initTblFirmas();
            initTblTabEmpleadosActivos();
            initTblTabPuestos();
            fncGetAccesosMenu();
            $("#menuGestionModificacion").addClass("opcionSeleccionada");
            //#endregion

            //#region FILTROS
            btnFiltroBuscar.click(function () {
                fncDefaultCtrls("select2-cboFiltroGestionEstatus-container");
                if (cboFiltroGestionEstatus.val() != "") {
                    fncGetGestionModificacion();
                } else {
                    Alert2Warning("Es necesario seleccionar un filtro.");
                    $("#select2-cboFiltroGestionEstatus-container").css('border', '2px solid red');
                }
                return "";
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
            cboFiltroGestionEstatus.val(0);
            cboFiltroGestionEstatus.trigger("change");
            btnFiltroBuscar.trigger("click");
        }

        function initTblGestionModificacion() {
            dtGestionModificacion = tblGestionModificacion.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'tipoModificacionStr', title: 'Tipo modificación' },
                    { data: 'lineaNegocioStr', title: 'Linea de negocio' },
                    {
                        title: 'Fecha registro',
                        render: (data, type, row, meta) => {
                            return moment(row.fechaCreacion).format('DD/MM/YYYY');
                        }
                    },
                    {
                        title: 'Fecha aplica cambio',
                        render: (data, type, row, meta) => {
                            return moment(row.fechaAplicaCambio).format('DD/MM/YYYY');
                        }
                    },
                    {
                        render: function (data, type, row, meta) {

                            let btnAutorizar = `<button class='btn btn-xs btn-success autorizar' title='Autorizar modificación.'><i class="fas fa-thumbs-up"></i></button>`;
                            let btnRechazar = `<button class='btn btn-xs btn-danger rechazar' title='Rechazar modificación.'><i class="fas fa-thumbs-down"></i></button>`;
                            let btnLstFirmas = `<button class='btn btn-xs btn-primary lstFirmas' title='Ver firmas.'><i class="fas fa-signature"></i></button>`;
                            let btnComentario = `<button class='btn btn-xs btn-primary verComentario' title='Ver comentario rechazo.'><i class="far fa-comments"></i></button>`;
                            let btnDetalle = `<button class="btn btn-xs btn-primary tabuladorDetalle"><i class="fas fa-stream"></i></button>`;
                            let btnGenerarDetalleModificacionPDF = `<button class="btn btn-xs btn-danger generarDetalleModificacionPDF"><i class='fas fa-file-pdf'></i></button>`;

                            let botones = "";
                            if (row.modificacionAutorizada == 0) {
                                if (row.esFirmar) {
                                    botones += `${btnLstFirmas} ${btnAutorizar} ${btnRechazar}`;
                                } else {
                                    botones += `${btnLstFirmas}`;
                                }
                            } else {
                                if (row.modificacionAutorizada == 2) {
                                    botones += `${btnComentario}`;
                                } else {
                                    botones += `${btnGenerarDetalleModificacionPDF}`;
                                }
                            }
                            botones += ` ${btnDetalle}`;
                            return botones;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblGestionModificacion.on('click', '.lstFirmas', function () {
                        let rowData = dtGestionModificacion.row($(this).closest('tr')).data();
                        mdlFirmas.modal("show");
                        fncGetLstAutorizantesModificacion(rowData.id);
                    });

                    tblGestionModificacion.on('click', '.verComentario', function () {
                        let rowData = dtGestionModificacion.row($(this).closest('tr')).data();
                        mdlComentario.modal("show");
                        txtComentario.val(rowData.comentarioRechazo);
                    });

                    tblGestionModificacion.on('click', '.autorizar', function () {
                        let rowData = dtGestionModificacion.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Autorizar tabulador', '¿Desea autorizar la modificación seleccionada?', 'Confirmar', 'Cancelar', () => fncAutorizarRechazarGestionModificacion(rowData.id, 1));
                    });

                    tblGestionModificacion.on('click', '.rechazar', function () {
                        let rowData = dtGestionModificacion.row($(this).closest('tr')).data();

                        Swal.fire({
                            position: "center",
                            icon: "warning",
                            title: "Rechazar modificación",
                            input: 'textarea',
                            width: '50%',
                            showCancelButton: true,
                            html: "<h3>¿Desea rechazar la modificación seleccionada?<br>Indicar el motivo:</h3>",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#5cb85c",
                            cancelButtonText: "Cancelar",
                            cancelButtonColor: "#5c636a",
                            showCloseButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                fncGuardarComentarioRechazoModificacion(rowData.id, $('.swal2-textarea').val());
                            }
                        });
                    });

                    tblGestionModificacion.on("click", ".tabuladorDetalle", function () {
                        let rowData = dtGestionModificacion.row($(this).closest("tr")).data();
                        fncGetTabuladorDetalle(rowData.id, rowData.tipoModificacion);
                    });

                    tblGestionModificacion.on("click", ".generarDetalleModificacionPDF", function () {
                        let rowData = dtGestionModificacion.row($(this).closest("tr")).data();
                        Alert2AccionConfirmar('Generar PDF', '¿Desea generar el archivo PDF?', 'Confirmar', 'Cancelar', () => fncGenerarDetalleModificacionPDF(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: '10%', targets: [4] }
                ],
            });
        }

        function fncGenerarDetalleModificacionPDF(FK_IncrementoAnual) {
            $.blockUI({ message: "Cargando información..." });
            var path = `/Reportes/Vista.aspx?idReporte=290&FK_IncrementoAnual=${FK_IncrementoAnual}`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function fncGetGestionModificacion() {
            let obj = {};
            obj.modificacionAutorizada = cboFiltroGestionEstatus.val();
            axios.post('GetGestionModificacion', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtGestionModificacion.clear();
                    dtGestionModificacion.rows.add(response.data.lstGestionDTO);
                    dtGestionModificacion.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAutorizarRechazarGestionModificacion(id, modificacionAutorizada) {
            if (id > 0 && modificacionAutorizada > 0) {
                let obj = {};
                obj.id = id;
                obj.modificacionAutorizada = modificacionAutorizada;
                axios.post('AutorizarRechazarGestionModificacion', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetGestionModificacion();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al realizar la acción.");
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
                initComplete: function (settings, json) {
                    tblFirmas.on('click', '.classBtn', function () {
                        let rowData = dtFirmas.row($(this).closest('tr')).data();
                    });
                    tblFirmas.on('click', '.classBtn', function () {
                        let rowData = dtFirmas.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetLstAutorizantesModificacion(idModificacion) {
            axios.post("GetLstAutorizantesModificacion", { idModificacion }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtFirmas.clear();
                    dtFirmas.rows.add(items);
                    dtFirmas.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGuardarComentarioRechazoModificacion(idModificacion, comentario) {
            axios.post("GuardarComentarioRechazoModificacion", { idModificacion, comentario }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncAutorizarRechazarGestionModificacion(idModificacion, 2);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTblTabEmpleadosActivos() {
            dtTabEmpleadosActivos = tblTabEmpleadosActivos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreEmpleado' },
                    { data: 'puestoDesc' },
                    { data: 'tipoNominaDesc' },
                    { data: 'lineaNegocioDesc' },
                    { data: 'categoriaDesc' },
                    { data: 'sueldoBase_Anterior' },
                    { data: 'complemento_Anterior' },
                    { data: 'totalNominal_Anterior' },
                    { data: 'totalMensual_Anterior' },
                    { data: 'sueldoBase_Modificacion' },
                    { data: 'complemento_Modificacion' },
                    { data: 'totalNominal_Modificacion' },
                    { data: 'totalMensual_Modificacion' },
                    { data: 'tipoIncremento' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function initTblTabPuestos() {
            dtTabPuestos = tblTabPuestos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                order: [[0, "asc"]],
                columns: [
                    { data: 'puestoDesc' },
                    { data: 'tipoNominaDesc' },
                    { data: 'esquemaPagoDesc' },
                    { data: 'lineaNegocioDesc' },
                    { data: 'categoriaDesc' },
                    { data: 'sueldoBase_Modificacion' },
                    { data: 'complemento_Modificacion' },
                    { data: 'totalNominal_Modificacion' },
                    { data: 'totalMensual_Modificacion' },
                    { data: 'sueldoBase_Anterior' },
                    { data: 'complemento_Anterior' },
                    { data: 'totalNominal_Anterior' },
                    { data: 'totalMensual_Anterior' },
                    { data: 'tipoIncremento' }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetTabuladorDetalle(id, tipoModificacion) {
            let objParamDTO = {};
            objParamDTO.id = id;
            objParamDTO.tipoModificacion = tipoModificacion;
            objParamDTO.modificacionAutorizada = cboFiltroGestionEstatus.val();
            axios.post('GetModificacionDetalle', objParamDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    switch (tipoModificacion) {
                        case 1:
                            dtTabEmpleadosActivos.clear();
                            dtTabEmpleadosActivos.rows.add(response.data.lstTabEmpleadosActivos);
                            dtTabEmpleadosActivos.draw();
                            divTabEmpleadosActivos.css("display", "inline");
                            divTabPuestos.css("display", "none");

                            lblEmpleadosPrimerTab.html("Actual");
                            lblEmpleadosSegundoTab.html("Anterior");

                            if (cboFiltroGestionEstatus.val() == 0) {
                                lblEmpleadosPrimerTab.html("Actual");
                                lblEmpleadosSegundoTab.html("Modificación");
                            }

                            break;
                        case 2:
                            dtTabPuestos.clear();
                            dtTabPuestos.rows.add(response.data.lstTabPuestos);
                            dtTabPuestos.draw();
                            divTabEmpleadosActivos.css("display", "none");
                            divTabPuestos.css("display", "inline");
                            break;
                    }
                    mdlModificacion.modal("show");
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
        CH.GestionModificacion = new GestionModificacion();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();