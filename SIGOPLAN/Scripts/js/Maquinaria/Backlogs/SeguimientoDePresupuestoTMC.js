(() => {
    $.namespace("Maquinaria.BackLogs.BackLogsRegistroObra");

    BackLogs = function () {
        const cboProyecto = $("#cboProyecto");

        //MENU
        const btnProgramaInspeccion = $('#btnProgramaInspeccion');
        const btnPresupuestoRehabilitacion = $('#btnPresupuestoRehabilitacion');
        const btnSeguimientoPresupuestos = $('#btnSeguimientoPresupuestos');
        const btnInformeRehabilitacion = $('#btnInformeRehabilitacion');
        const btnFrenteBackLogs = $('#btnFrenteBackLogs');
        const btnIndicadoresRehabilitacionTMC = $('#btnIndicadoresRehabilitacionTMC');
        const btnInicioTMC = $('#btnInicioTMC');
        const cboModelo = $('#cboModelo');
        const cboGrupo = $('#cboGrupo');
        const cboTipo = $('#cboTipo');
        const cboMotivo = $('#cboMotivo');
        const cboFiltroEstatusPpto = $('#cboFiltroEstatusPpto');
        const btnGuardar = $('#btnGuardar');
        const btnExportar = $('#btnExportar');
        const btnBuscarSeguimiento = $('#btnBuscarSeguimiento');
        const tblSeguimientos = $('#tblSeguimientos');
        const btnBuscar = $('#btnBuscar');
        const tblIndicadores = $('#tblIndicadores');
        let dtIndicadores;
        //END --MENU

        //#region CONST BACKLOGS
        const mdlListadoBL = $('#mdlListadoBL');
        const tblBL_CatBackLogs = $('#tblBL_CatBackLogs');
        const btnMdlCerrarBL = $('#btnMdlCerrarBL');
        const mdlListadoInsumosBL = $('#mdlListadoInsumosBL');
        const tblBL_Partes = $('#tblBL_Partes');
        let dtBL;
        let dtInsumosBL;
        const mdlDescripcionBL = $('#mdlDescripcionBL');
        const txtDescripcionBL = $('#txtDescripcionBL');
        //#endregion

        //#region CONST INSUMOS
        const mdlActualizarInsumo = $('#mdlActualizarInsumo');
        const txtInsumo = $('#txtInsumo');
        const txtCantidad = $('#txtCantidad');
        const txtParte = $('#txtParte');
        const txtArticulo = $('#txtArticulo');
        const txtUnidad = $('#txtUnidad');
        const txtCostoPromedio = $('#txtCostoPromedio');
        const btnCostoPromedio = $('#btnCostoPromedio');
        const btnCatInsumos = $('#btnCatInsumos');
        const btnActualizarInsumo = $('#btnActualizarInsumo');
        const mdlCatalogoInsumos = $('#mdlCatalogoInsumos');
        const txtFiltroInsumo = $('#txtFiltroInsumo');
        const txtFiltroDescripcion = $('#txtFiltroDescripcion');
        const btnFiltroBuscar = $('#btnFiltroBuscar');
        const tblCatInsumos = $('#tblCatInsumos');
        //#endregion

        (function init() {
            fncListeners();
            fncFillCombos();
            initTablaPptos();
            habilitardeshabilitar(true);
            initTablaIndicadores();
            initTablaBackLogs();
            initTablaInsumos();
            initTablaCatInsumos();
            bon();
            obtenerUrlPArams();
        })();

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
            // console.log('hola soy obtener url params')
            // console.log(variables)
            if (variables != undefined) {
                cboProyecto.val(variables.areaCuenta);
                cboProyecto.trigger('change');
            }
        }

        function getUrlParams(url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        }

        function fncFillCombos() {
            cboProyecto.fillCombo("/BackLogs/FillAreasCuentasTMC", {}, false);
            cboProyecto.select2({
                width: "resolve"
            });

            cboTipo.fillCombo("/BackLogs/FillTipoMaquinariaTMC", {}, false);
            cboModelo.fillCombo("/BackLogs/FillCboModelo", {}, false);
            cboGrupo.fillCombo("/BackLogs/FillCboGrupo", {}, false);
            cboMotivo.select2();
            cboProyecto.select2();
            cboTipo.select2();
            cboModelo.select2();
            cboGrupo.select2();
            cboFiltroEstatusPpto.select2();
        }

        function habilitardeshabilitar(habilitado) {
            cboModelo.attr("disabled", habilitado);
            cboGrupo.attr("disabled", habilitado);
            cboTipo.attr("disabled", habilitado);
            cboMotivo.attr("disabled", habilitado);
            btnGuardar.attr("disabled", habilitado);
            btnExportar.attr("disabled", habilitado);
            btnBuscarSeguimiento.attr("disabled", habilitado);
            btnBuscar.attr("disabled", habilitado);
        }

        function fncListeners() {

            cboProyecto.change(function (e) {
                fncGetSeguimiento();
                habilitardeshabilitar(false);
            });
            btnBuscar.click(function () {
                fncGetSeguimiento();
            });

            btnInicioTMC.click(function (e) {
                document.location.href = '/BackLogs/IndexTMC?areaCuenta=' + cboProyecto.val();
            });
            btnProgramaInspeccion.click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspTMC?areaCuenta=' + cboProyecto.val();
            })
            btnPresupuestoRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/PresupuestoRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });
            btnSeguimientoPresupuestos.click(function (e) {
                document.location.href = '/BackLogs/SeguimientoDePresupuestoTMC?areaCuenta=' + cboProyecto.val();
            });
            btnInformeRehabilitacion.click(function (e) {
                document.location.href = '/BackLogs/InformeTMC?areaCuenta=' + cboProyecto.val();
            });
            btnFrenteBackLogs.click(function (e) {
                document.location.href = '/BackLogs/FrenteTMC?areaCuenta=' + cboProyecto.val();
            });
            btnIndicadoresRehabilitacionTMC.click(function (e) {
                document.location.href = '/BackLogs/IndicadoresRehabilitacionTMC?areaCuenta=' + cboProyecto.val();
            });

            btnActualizarInsumo.click(function (e) {
                fncActualizarInsumo();
            });
            btnCatInsumos.click(function (e) {
                mdlCatalogoInsumos.modal("show");
            });
            btnFiltroBuscar.click(function (e) {
                fncGetCatInsumos();
            });
            btnCostoPromedio.click(function (e) {
                fncGetCostoPromedio();
            });
        }

        function bon() {
            $(document).ready(function () {
                $("[id*='btnCostoPromedio']").attr('disabled', 'disabled');
                $("[id*='txtInsumo']" && "[id*='txtCantidad']").keyup(function () {
                    if ($("#txtInsumo").val() != '' && $("#txtCantidad").val() != '') {
                        $("[id*='btnCostoPromedio']").removeAttr('disabled');
                    }
                    else {
                        $("[id*='btnCostoPromedio']").attr('disabled', 'disabled');

                    }
                });
            });
        }

        function initTablaPptos() {
            dtSeguimiento = tblSeguimientos.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: true,
                paging: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'idInspTMC', title: 'id', visible: false },
                    {
                        title: 'Detalle<br>ppto',
                        render: function (type, data, row) {
                            return `<button class="btn btn-primary btn-xs verLstBackLogs"><i class="fas fa-wrench"></i></button>`;
                        }
                    },
                    { data: "folioPpto", title: "Folio ppto" },
                    {
                        data: "fechaPpto", title: "Fecha ppto",
                        render: function (data, type, row) {
                            return moment(row.fechaPpto).format('DD/MM/YYYY');
                        }
                    },
                    { data: "cc", title: "C.C." },
                    { data: "descripcion", title: "Descripción", visible: false },
                    { data: "modelo", title: "Modelo", visible: false },
                    { data: "horas", title: "Horas" },
                    { data: "tipoMotivo", title: "Motivo" },
                    {
                        title: "Ppto",
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.Ppto);
                        }
                    },
                    {
                        data: "fechaRequerido", title: "Fecha<br>requerido",
                        render: function (data, type, row) {
                            return moment(row.fechaRequerido).format('DD/MM/YYYY');
                        }
                    },
                    { data: 'nombreUserVobo1', visible: false, title: 'Nombre Vobo 1' },
                    { data: 'firmaVobo1', visible: false, title: 'Firma Vobo 1' },
                    {
                        data: "vobo1", title: "VoBo<br#1", render: (data, type, row) => {
                            if (data == "AUTORIZADO") {
                                let autorizado = ``;
                                autorizado += `<button class="btn btn-success btn-xs" title="RAMON IGNACIO GARCIA MAYTORENA"><i class="fa fa-check" aria-hidden="true"></i></button>`;
                                return autorizado;
                            }
                            else {
                                if (data == "EN ESPERA") {
                                    let pendiente = ``;
                                    pendiente += `<button title="RAMON IGNACIO GARCIA MAYTORENA" class="btn btn-warning btn-xs autorizarVobo1" data-idUser='${row.idUserVobo1}'><i class="fas fa-exclamation-circle"></i></button>`;
                                    return pendiente;
                                }
                                else {
                                    if (data == "RECHAZADO") {
                                        let rechazado = ``;
                                        rechazado += `<button title="RAMON IGNACIO GARCIA MAYTORENA" class="btn btn-danger btn-xs"><i class="fa fa-times" aria-hidden="true"></i></button>`;
                                        return rechazado;
                                    }
                                }
                            }
                        }
                    },
                    {
                        data: "fechaVobo1", title: "Fecha VoBo #1", visible: false,
                        render: (data, type, row) => {
                            if (row.esVobo1 != 2) {
                                return moment(row.fechaVobo1).format('DD/MM/YYYY');
                            } else {
                                return '';
                            }
                        }
                    },
                    { data: 'nombreUserVobo2', visible: false, title: 'Nombre Vobo 2' },
                    { data: 'firmaVobo2', visible: false, title: 'Firma Vobo 2' },
                    {
                        data: "vobo2", title: "VoBo #2", render: (data, type, row) => {
                            if (data == "N/A") {
                                let autorizado = ``;
                                autorizado += ``;
                                return autorizado;
                            } else {
                                if (data == "AUTORIZADO") {
                                    let autorizado = ``;
                                    autorizado += `<button title="RICARDO PEREZ ALABAZARES" class="btn btn-success btn-xs"><i class="fa fa-check" aria-hidden="true"></i></button>`;
                                    return autorizado;
                                }
                                else {
                                    if (data == "EN ESPERA") {
                                        let pendiente = ``;
                                        pendiente += `<button title="RICARDO PEREZ ALABAZARES" class="btn btn-warning btn-xs autorizarVobo2" data-idUser='${row.idUserVobo2}'><i class="fas fa-exclamation-circle"></i></button>`;
                                        return pendiente;
                                    }
                                    else {
                                        if (data == "RECHAZADO") {
                                            let rechazado = ``;
                                            rechazado += `<button title="RICARDO PEREZ ALABAZARES" class="btn btn-danger btn-xs"><i class="fa fa-times" aria-hidden="true"></i></button>`;
                                            return rechazado;
                                        }
                                        else {
                                            let rechazado = ``;
                                            return rechazado;
                                        }
                                    }
                                }
                            }
                        }
                    },
                    {
                        data: "fechaVobo2", title: "Fecha VoBo #2", visible: false,
                        render: (data, type, row) => {
                            if (row.esVobo2 != 2) {
                                return moment(row.fechaVobo2).format('DD/MM/YYYY');
                            } else {
                                return '';
                            }
                        }
                    },
                    { data: 'nombreUserAutorizado', visible: false, title: 'Nombre Autorizante' },
                    { data: 'firmaAutorizado', visible: false, title: 'Firma Autorizante' },
                    {
                        data: "autorizado", title: "Autorizo", render: (data, type, row) => {
                            if (data == "N/A") {
                                let autorizado = ``;
                                autorizado += ``;
                                return autorizado;
                            } else {
                                if (data == "AUTORIZADO") {
                                    let autorizado = ``;
                                    autorizado += `<button title="OSCAR MANUEL ROMAN RUIZ" class="btn btn-success btn-xs"><i class="fa fa-check" aria-hidden="true"></i></button>`;
                                    return autorizado;
                                }
                                else {
                                    if (data == "EN ESPERA") {
                                        let pendiente = ``;
                                        pendiente += `<button title="OSCAR MANUEL ROMAN RUIZ" class="btn btn-warning btn-xs autorizarVobo3" data-idUser='${row.idUserAutorizado}'><i class="fas fa-exclamation-circle"></i></button>`;
                                        return pendiente;
                                    }
                                    else {
                                        if (data == "RECHAZADO") {
                                            let rechazado = ``;
                                            rechazado += `<button title="OSCAR MANUEL ROMAN RUIZ" class="btn btn-danger btn-xs"><i class="fa fa-times" aria-hidden="true"></i></button>`;
                                            return rechazado;
                                        } else {
                                            let rechazado = ``;
                                            return rechazado;
                                        }
                                    }
                                }
                            }
                        }
                    },
                    {
                        data: "fechaVobo2", title: "Fecha autorizo", visible: false,
                        render: (data, type, row) => {
                            if (row.esAutorizado != 2) {
                                return moment(row.fechaAutorizado).format('DD/MM/YYYY');
                            } else {
                                return '';
                            }
                        }
                    },
                    { data: "dias", title: "Días<br>trascurridos" },
                    { data: "estado", title: "Estatus<br>ppto" }
                ],
                drawCallback: function (settings) {
                    $('.asignarFrente').fillCombo('/BackLogs/FillCboFrentes', null, false);
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                initComplete: function (settings, json) {
                    tblSeguimientos.on('click', '.autorizarVobo1', function () {
                        let rowData = dtSeguimiento.row($(this).closest('tr')).data();
                        let title = 'En caso de rechazar, ingresar el motivo.';
                        let txt = '';
                        let strConfirmar = 'Autorizar';
                        let strCancelar = 'Rechazar';
                        let cbAceptar = '';
                        let strIcono = 'info';
                        Alert2AccionConfirmarInput(rowData.id, $('.autorizarVobo1').attr('data-idUser'), title, txt, strConfirmar, strCancelar, cbAceptar, strIcono);
                    });

                    tblSeguimientos.on('click', '.autorizarVobo2', function () {
                        let rowData = dtSeguimiento.row($(this).closest('tr')).data();
                        let title = 'En caso de rechazar, ingresar el motivo.';
                        let txt = '';
                        let strConfirmar = 'Autorizar';
                        let strCancelar = 'Rechazar';
                        let cbAceptar = '';
                        let strIcono = 'info';
                        Alert2AccionConfirmarInput(rowData.id, $('.autorizarVobo2').attr('data-idUser'), title, txt, strConfirmar, strCancelar, cbAceptar, strIcono);
                    });

                    tblSeguimientos.on('click', '.autorizarVobo3', function () {
                        let rowData = dtSeguimiento.row($(this).closest('tr')).data();
                        let title = 'En caso de rechazar, ingresar el motivo.';
                        let txt = '';
                        let strConfirmar = 'Autorizar';
                        let strCancelar = 'Rechazar';
                        let cbAceptar = '';
                        let strIcono = 'info';
                        Alert2AccionConfirmarInput(rowData.id, $('.autorizarVobo3').attr('data-idUser'), title, txt, strConfirmar, strCancelar, cbAceptar, strIcono);
                    });

                    tblSeguimientos.on("click", ".verLstBackLogs", function () {
                        let rowData = dtSeguimiento.row($(this).closest("tr")).data();
                        btnBuscar.attr("data-idSegPpto", rowData.id);
                        fncGetBL(rowData.id);
                    });
                }
            });
        }

        function Alert2AccionConfirmarInput(id, idAutorizado, strTitle, strText, strConfirmar, strCancelar, cbAceptar, strIcono) {
            mdlbtnAceptar = $("#mdlbtnAceptar");

            if (strIcono == null || strIcono == undefined || strIcono == '') {
                strIcono = 'warning';
            }

            Swal.fire({
                position: "center",
                icon: strIcono,
                title: strTitle,
                input: 'text',
                width: '35%',
                showCancelButton: true,
                html: "<h3>" + strText + "</h3>",
                confirmButtonText: strConfirmar,
                confirmButtonColor: "#5cb85c",
                cancelButtonText: strCancelar,
                cancelButtonColor: "#d9534f",
                showCloseButton: true
            }).then((result) => {
                if (result.isConfirmed) {
                    ModificarEstatus(id, idAutorizado, 1, $('.swal2-input').val());
                }
                else {
                    if ($('.swal2-input').val() == "") {
                        Alert2Warning('Es necesario indicar el motivo de rechazo del ppto.');
                    } else {
                        ModificarEstatus(id, idAutorizado, 0, $('.swal2-input').val());
                    }
                }
            });
        }

        function ModificarEstatus(id, Autorizante, Estatus, Descripcion) {
            axios.post('ModificarEstatus', {
                id: id, Autorizante: Autorizante, Estatus: Estatus, Descripcion: Descripcion
            }).catch(o_O => AlertaGeneral(o_O.message)).then(response => {
                let { success, items } = response.data;
                // console.log(items)
                if (success) {
                    fncGetSeguimiento();
                } else {
                    Alert2Warning(items)
                }
            });
        }

        function fncGetSeguimiento() {
            axios.post('GetSeguimientoPpto', {
                AreaCuenta: cboProyecto.val(),
                motivo: cboMotivo.val(),
                estatusPpto: cboFiltroEstatusPpto.val()
            }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtSeguimiento.clear();
                    dtSeguimiento.rows.add(response.data.lstSeguimiento);
                    dtSeguimiento.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaIndicadores() {
            dtIndicadores = tblIndicadores.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        title: 'Autorizado',
                        render: function (type, data, row) {
                            return `<i class="fas fa-check"></i>`;
                        }
                    },
                    {
                        title: 'En espera',
                        render: function (type, data, row) {
                            return `<i class="fas fa-exclamation"></i>`;
                        }
                    },
                    {
                        title: 'Rechazado',
                        render: function (type, data, row) {
                            return `<i class="fas fa-times"></i>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        //#region BACKLOGS
        function initTablaBackLogs() {
            dtBL = tblBL_CatBackLogs.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'folioBL', title: 'Folio BL' },
                    { data: 'noEconomico', title: 'Núm. económico' },
                    { data: 'horas', title: 'Horas' },
                    {
                        data: 'descripcion', title: 'Descripción',
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary btn-xs verDescripcionBL" title="Descripción del BL.">Descripción BL</button>`;
                        }
                    },
                    {
                        title: 'Ppto. estimado',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.presupuestoEstimado);
                        }
                    },
                    {
                        render: function (type, data, row) {
                            let btnVerInsumos = `<button class="btn btn-xs btn-primary verInsumos" title="Listado de insumos."><i class="fas fa-tools"></i></button>&nbsp;`;
                            let btnEliminarBL = `<button class="btn btn-xs btn-danger eliminarBL" title="Eliminar BackLog."><i class="fas fa-trash"></i></button>`;
                            return btnVerInsumos + btnEliminarBL;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblBL_CatBackLogs.on("click", ".verDescripcionBL", function () {
                        const rowData = dtBL.row($(this).closest("tr")).data();
                        txtDescripcionBL.val(rowData.descripcion);
                        mdlDescripcionBL.modal("show");
                    });

                    tblBL_CatBackLogs.on('click', '.verInsumos', function () {
                        let rowData = dtBL.row($(this).closest('tr')).data();
                        btnActualizarInsumo.attr("data-BL", rowData.id);
                        fncGetInsumosBL(rowData.id);
                        mdlListadoInsumosBL.modal("show");
                    });

                    tblBL_CatBackLogs.on('click', '.eliminarBL', function () {
                        let rowData = dtBL.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarBL(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetBL(idSegPpto) {
            let obj = new Object();
            obj = {
                idSegPpto: idSegPpto
            };
            axios.post("GetBLPptos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtBL.clear();
                    dtBL.rows.add(response.data.lstBL);
                    dtBL.draw();
                    mdlListadoBL.modal("show");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarBL(idBL) { //TODO
            let objEliminarBL = new Object();
            objEliminarBL = {
                id: idBL
            };
            axios.post("EliminarBackLog", objEliminarBL).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                    fncGetSeguimiento();
                    fncGetBL(btnBuscar.attr("data-idSegPpto"));
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaInsumos() {
            dtInsumosBL = tblBL_Partes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'parte', title: 'Parte' },
                    { data: 'articulo', title: 'Artículo' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        title: 'Costo<br>promedio',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.costoPromedio);
                        }
                    },
                    {
                        render: function (type, data, row) {
                            let btnActualizar = `<button class="btn btn-xs btn-warning actualizarInsumo" title="Actualizar insumo."><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-xs btn-danger eliminarInsumo" title="Eliminar insumo."><i class="fas fa-trash"></i></button>`;
                            return btnActualizar + btnEliminar;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblBL_Partes.on('click', '.actualizarInsumo', function () {
                        let rowData = dtInsumosBL.row($(this).closest('tr')).data();
                        txtInsumo.val(rowData.insumo);
                        txtCantidad.val(rowData.cantidad);
                        txtParte.val(rowData.parte);
                        txtArticulo.val(rowData.articulo);
                        txtUnidad.val(rowData.unidad);
                        txtCostoPromedio.val(rowData.costoPromedio);
                        btnActualizarInsumo.attr("data-id", rowData.id);
                        mdlActualizarInsumo.modal("show");
                    });

                    tblBL_Partes.on('click', '.eliminarInsumo', function () {
                        let rowData = dtInsumosBL.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarInsumo(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncActualizarInsumo() {
            let obj = new Object();
            obj = {
                insumo: txtInsumo.val().trim(),
                cantidad: txtCantidad.val().trim(),
                parte: txtParte.val().trim(),
                articulo: txtArticulo.val().trim(),
                unidad: txtUnidad.val().trim(),
                costoPromedio: txtCostoPromedio.val().trim(),
                id: btnActualizarInsumo.attr("data-id"),
                idBackLog: btnActualizarInsumo.attr("data-BL")
            };
            axios.post("CrearEditarParte", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al actualizar el insumo.");
                    fncGetInsumosBL(btnActualizarInsumo.attr("data-BL"));
                    mdlActualizarInsumo.modal("hide");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarInsumo(idInsumo) { //TODO
            let objEliminarInsumo = new Object();
            objEliminarInsumo = {
                id: idInsumo
            };
            axios.post("EliminarParte", objEliminarInsumo).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                    fncGetSeguimiento();
                    fncGetInsumosBL(btnActualizarInsumo.attr("data-BL"));
                    fncGetBL(btnBuscar.attr("data-idSegPpto"));
                } else {
                    Alert2Warning(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetInsumosBL(idBL) {
            let obj = new Object();
            obj = {
                idBackLog: idBL
            };
            axios.post("GetPartes", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtInsumosBL.clear();
                    dtInsumosBL.rows.add(response.data.lstPartes);
                    dtInsumosBL.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaCatInsumos() {
            dtInsumos = tblCatInsumos.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: true,
                paging: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-primary seleccionarInsumo"><i class="fas fa-arrow-circle-right"></i></button>`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCatInsumos.on('click', '.seleccionarInsumo', function () {
                        let rowData = dtInsumos.row($(this).closest('tr')).data();
                        txtInsumo.val(rowData.insumo);
                        txtArticulo.val(rowData.descripcion);
                        txtUnidad.val(rowData.unidad)
                        dtInsumos.clear();
                        dtInsumos.draw();
                        mdlCatalogoInsumos.modal("hide");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetCatInsumos() {
            let objFiltro = new Object();
            objFiltro = {
                insumo: txtFiltroInsumo.val(),
                descripcion: txtFiltroDescripcion.val()
            };
            axios.post("GetInsumos", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtInsumos.clear();
                    dtInsumos.rows.add(response.data.lstInsumos);
                    dtInsumos.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetCostoPromedio() {
            let obj = new Object();
            obj = {
                almacen: 400,
                insumo: txtInsumo.val()
            };
            axios.post("GetCostoPromedio", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    txtCostoPromedio.val(response.data.costoPromedio);
                    txtCostoPromedio.val(txtCostoPromedio.val() * txtCantidad.val());
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();