(() => {
    $.namespace("Maquinaria.BackLogs.BackLogsRegistroObra");

    BackLogs = function () {
        const cboProyecto = $("#cboProyecto");

        //#region CONST CATALOGO CONJUNTOS
        const btnConjuntosCatConjuntos = $('#btnConjuntosCatConjuntos');
        const txtNombreFrente = $('#txtNombreFrente');
        const cboUsuarios = $('#cboUsuarios');
        const cboModelo = $('#cboModelo');
        const cboGrupo = $('#cboGrupo');
        const cboTipo = $('#cboTipo');
        const cboMotivo = $('#cboMotivo');
        const cargar = $('#cargar');
        const cboFrentes = $('#cboFrentes');
        const btnBuscarSeguimiento = $('#btnBuscarSeguimiento');
        const btnCrearEditarCancelarConjuntoCatConjuntos = $('#btnCrearEditarCancelarConjuntoCatConjuntos');
        const btnNuevoConjuntoCatConjuntos = $('#btnNuevoConjuntoCatConjuntos');
        const btnCollapseConjuntosCatConjuntos = $('#btnCollapseConjuntosCatConjuntos');
        const divCrearEditarConjuntoCatConjuntos = $('#divCrearEditarConjuntoCatConjuntos');
        const tblBL_CatFrentes = $('#tblBL_CatFrentes');
        const tblFrentes = $('#tblFrentes');
        const tblAsignacionFrente = $('#tblAsignacionFrente');
        const modalCrearEditarFrente = $('#modalCrearEditarFrente');
        const btnCrearEditarFrente = $('#btnCrearEditarFrente');
        const btnCrearEditarFrenteCatFrente = $('#btnCrearEditarFrenteCatFrente');
        const btnGuardar = $('#btnGuardar');
        const btnExportar = $('#btnExportar');
        const btnBuscarDetFrente = $('#btnBuscarDetFrente');
        const cboFiltroFrente = $('#cboFiltroFrente');
        const cboFiltroEstatusSeguimientoFrente = $('#cboFiltroEstatusSeguimientoFrente');
        let objGuardado = [];
        //#endregion

        //MENU
        const btnProgramaInspeccion = $('#btnProgramaInspeccion');
        const btnPresupuestoRehabilitacion = $('#btnPresupuestoRehabilitacion');
        const btnSeguimientoPresupuestos = $('#btnSeguimientoPresupuestos');
        const btnInformeRehabilitacion = $('#btnInformeRehabilitacion');
        const btnFrenteBackLogs = $('#btnFrenteBackLogs');
        const btnIndicadoresRehabilitacionTMC = $('#btnIndicadoresRehabilitacionTMC');
        const btnInicioTMC = $('#btnInicioTMC');

        //END --MENU
        let dtFrentes;
        let dtnombre;
        let dtasignacion;

        (function init() {
            fncListeners();
            fncFillCombos();
            fncFillModelo();
            fncFillGrupo();
            initFrentes();
            fncFillUsuario();
            fncFillTipo();
            initDataTblnombre();
            initDataTblaAsignacion();
            fncFillFrentes();
            fncHabilitarDeshabilitarFormulario();
            obtenerUrlPArams();
        })();

        function obtenerUrlPArams() {
            const variables = getUrlParams(window.location.href);
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

        function fncHabilitarDeshabilitarFormulario() {
            let idProyecto = cboProyecto.val();

            if (idProyecto == "") {
                cboModelo.attr("disabled", true);
                cboGrupo.attr("disabled", true);
                cboTipo.attr("disabled", true);
                cboMotivo.attr("disabled", true);
                btnGuardar.attr("disabled", true);
                btnExportar.attr("disabled", true);
                btnCrearEditarFrente.attr("disabled", true);
                btnBuscarSeguimiento.attr("disabled", true);
                cboFrentes.attr("disabled", true);
                btnBuscarDetFrente.attr("disabled", true);
            } else {
                cboGrupo.attr("disabled", false);
                cboModelo.attr("disabled", false);
                cboTipo.attr("disabled", false);
                cboMotivo.attr("disabled", false);
                btnGuardar.attr("disabled", false);
                btnExportar.attr("disabled", false);
                btnCrearEditarFrente.attr("disabled", false);
                btnBuscarSeguimiento.attr("disabled", false);
                cboFrentes.attr("disabled", false);
                btnBuscarDetFrente.attr("disabled", false);
            }
        }

        function fncFillCombos() {
            cboProyecto.fillCombo("/BackLogs/FillAreasCuentasTMC", {}, false);
            cboProyecto.select2({
                width: "resolve"
            });
            cboMotivo.select2();
            fncFillFrentes();
        }

        function fncFillModelo() {
            cboModelo.fillCombo("/BackLogs/FillCboModeloTMC", {}, false);
            cboModelo.select2({
                width: "resolve"
            });
        }

        function fncFillGrupo() {
            cboGrupo.fillCombo("/BackLogs/FillCboGrupoTMC", {}, false);
            cboGrupo.select2({
                width: "resolve"
            });
        }

        function fncFillTipo() {
            cboTipo.fillCombo("/BackLogs/FillTipoMaquinariaTMC", {}, false);
            cboTipo.select2({
                width: "resolve"
            });
        }

        function fncFillUsuario() {
            cboUsuarios.fillCombo("/BackLogs/FillcboUsuarios", {}, false);
            cboUsuarios.select2({ width: "100%" });
            cboUsuarios.trigger("change");

        }

        function fncFillFrentes() {
            cboFrentes.attr("multiple", true);
            cboFrentes.fillCombo("/BackLogs/FillCboFrentes", {}, true, "TODOS", () => convertToMultiselectSelectAll(cboFrentes));
        }

        function fncListeners() {
            cboProyecto.change(function (e) {
                if ($(this).val() != "") {
                    fncFillGrupo();
                } else {
                    Alert2Warning("Es necesario seleccionar un proyecto.");
                }
                fncHabilitarDeshabilitarFormulario();
            });

            cboFiltroFrente.fillCombo("/BackLogs/FillCboFrentes", null, false, null);
            cboFiltroFrente.select2();

            cboFiltroEstatusSeguimientoFrente.select2();

            //#region EVENTOS CATALOGO FRENTES
            btnCrearEditarFrente.click(function (e) {
                fncLimpiarCrearEditarConjunto();
                fncFillFrentes();
                if (divCrearEditarConjuntoCatConjuntos.hasClass('in')) {
                    btnCrearEditarCancelarConjuntoCatConjuntos.trigger('click');

                }
                modalCrearEditarFrente.modal("show");
                fncGetFrentes();
            });

            btnNuevoConjuntoCatConjuntos.click(function (e) {
                fncLimpiarCrearEditarConjunto();
            });

            btnCrearEditarCancelarConjuntoCatConjuntos.click(function (e) {
                fncLimpiarCrearEditarConjunto();

            });

            btnCrearEditarFrenteCatFrente.click(function (e) {

                if (cboUsuarios.val() != "") {
                    fncCrearEditarFrente();
                } else {
                    Alert2Warning("No puede dar de alta un frente sin un usuario asignado.");
                }
            });

            cargar.click(function (e) {
                fncFillFrentes();
            });

            btnBuscarSeguimiento.click(function (e) {
                fncGetSeguimiento();
            });

            btnGuardar.click(function () {
                fncCrearDetFrente();
            });

            btnBuscarDetFrente.click(function () {
                fncGetFrente();
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
            //#endregion
        }


        //#region FUNCIONES CATALOGO FRENTE
        function initFrentes() {
            dtFrentes = tblBL_CatFrentes.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreFrente', title: 'FRENTE' },
                    { data: 'UsuarioCreacion', title: 'USUARIO ASIGNADO' },
                    { data: 'idUsuarioAsignado', title: 'idUsuarioAsignado', visible: false },
                    { data: 'id', title: 'id', visible: false },

                    {
                        render: function (data, type, row) {
                            let editarFrente = `<button class='btn-editar btn-xs btn btn-warning editarFrente' data-id="${row.id}" title="Actualizar asignación de frente."><i class='fas fa-pencil-alt'></i></button>&nbsp;`;
                            let eliminarFrente = `<button class='btn-eliminar btn-xs btn btn-danger eliminarFrente' data-id="${row.id}" title="Eliminar asignación de frente."><i class="far fa-trash-alt"></i></button>`;
                            return editarFrente + eliminarFrente;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblBL_CatFrentes.on("click", ".editarFrente", function () {
                        let rowData = dtFrentes.row($(this).closest('tr')).data();
                        btnCrearEditarFrenteCatFrente.attr("data-id", rowData.id);
                        cboUsuarios.val(rowData.idUsuarioAsignado);
                        cboUsuarios.trigger("change");
                        txtNombreFrente.val(rowData.nombreFrente);
                        if (!divCrearEditarConjuntoCatConjuntos.hasClass('in')) {
                            btnCollapseConjuntosCatConjuntos.trigger('click');
                        }
                    });

                    tblBL_CatFrentes.on("click", ".eliminarFrente", function () {
                        let rowData = dtFrentes.row($(this).closest("tr")).data();
                        let idFrente = parseFloat(rowData.id);
                        Alert2AccionConfirmar("¡Cuidado!", "¿Desea eliminar el registro seleccionado?", "Confirmar", "Cancelar", () => fncEliminarFrente(idFrente));
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
            });
        }


        function fncLimpiarCrearEditarConjunto() {
            btnCrearEditarFrenteCatFrente.attr("data-id", 0);
            txtNombreFrente.val("");
            cboUsuarios.val("");
            fncFillFrentes();

        }

        function fncCrearEditarFrente() {
            let objConjunto = fncCrearObjFrente();
            if (objConjunto["nombreFrente"] != "") {
                axios.post('CrearEditarFrente', objConjunto).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetFrentes();
                        txtNombreFrente.val("");
                        cboUsuarios.val("");
                        btnCrearEditarFrenteCatFrente.attr("data-id", 0);
                        Alert2Exito("Se registro correctamente la frente.");
                        if (cboProyecto.val() != "") {
                            fncFillGrupo();
                            fncFillModelo();
                            fncFillFrentes();
                            fncGetSeguimiento();
                            fncFillFrentes();
                        }
                        cboUsuarios[0].selectedIndex = 0;
                        cboUsuarios.trigger("change");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Es necesario indicar nombre del frente.");
            }
        }

        function fncCrearObjFrente() {
            let objFrente = new Object();
            let id = btnCrearEditarFrenteCatFrente.attr("data-id");
            let nombreFrente = txtNombreFrente.val();
            let idUsuarioAsignado = cboUsuarios.val();

            if (nombreFrente == "")
                Alert2Warning("Es necesario indicar nombre del frente.");

            objFrente = {
                id: id,
                nombreFrente: nombreFrente,
                idUsuarioAsignado: idUsuarioAsignado
            };
            return objFrente;
        }

        function fncGetFrentes() {
            axios.post('GetFrente').catch(o_O => Alert2Error(o_O.message)).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtFrentes.clear();
                    dtFrentes.rows.add(response.data.lstFrentes);
                    dtFrentes.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }



        function fncEliminarFrente(id) {
            if (id > 0) {
                let obj = new Object();
                obj = {
                    idFrente: id
                };
                axios.post('EliminarFrente', obj)
                    .catch(o_O => Alert2Error(o_O.message))
                    .then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha eliminado con éxito el frente.");
                            fncGetFrentes();
                            if (cboProyecto.val() != "") {
                                fncFillFrentes();
                            }
                        }
                        else {
                            Alert2Error("No se puede eliminar este frente ya que se encuentra asignado a un presupuesto.");
                        }
                    }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al intentar eliminar el frente.");
            }
        }
        //#endregion
        //TABLA SEGUIMIENTO 
        function initDataTblaAsignacion() {
            dtasignacion = tblAsignacionFrente.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'idInspTMC', title: 'id', visible: false },
                    { data: "folioPpto", title: "Folio ppto" },
                    {
                        data: "fechaPpto", title: "Fecha ppto<br>terminado",
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
                        data: "fechaRequerido", title: "Fecha<br>requerido",
                        render: function (data, type, row) {
                            return moment(row.fechaRequerido).format('DD/MM/YYYY');
                        }
                    },
                    { data: "estado", title: "Estatus" },
                    {
                        title: "Ppto",
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.Ppto);
                        }
                    },
                    {
                        data: "idFrente", title: "Asignar a: ", render: (data, type, row) => {
                            let cboFrentes = `<select class="form-control asignarFrente" style="width: 250px" data-idFrente="${data}"></select> `;
                            return cboFrentes;
                        }
                    },
                    {
                        title: "Ver detalles: ", visible: false, render: (data, type, row) => {
                            let checkbox = ``;
                            checkbox += `<input type="checkbox"></input>&nbsp;`;
                            return checkbox;
                        }
                    }
                ],
                drawCallback: function (settings) {
                    $('.asignarFrente').fillCombo('/BackLogs/FillCboFrentes', null, false);
                    let tr = $('#tblAsignacionFrente').find('tr');
                    for (let i = 1; i < tr.length; i++) {
                        $(tr[i]).find('select').val($(tr[i]).find('select').attr('data-idFrente'))
                    }
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        //MOSTRAR DATOS SEGUIMIENTO
        function fncFiltros() {
            let objFiltro = new Object();
            objFiltro = {
                TipoMotivo: cboMotivo.val(),
            };
            return objFiltro;
        }

        function fncGetSeguimiento() {
            axios.post('GetSeguimientoPptoFrentes', {
                AreaCuenta: cboProyecto.val(),
                ObraoRenta: $('#cboMotivo').val(),
                idFrente: cboFiltroFrente.val() > 0 ? cboFiltroFrente.val() : 0
            }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtasignacion.clear();
                    dtasignacion.rows.add(response.data.lstSeguimientoFrente);
                    dtasignacion.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //FIN --MOSTRAR DATOS SEGUIMIENTO

        //FIN --TABLA SEGUIMIENTO 

        //TABLAS FRENTE #1
        function initDataTblnombre() {
            dtnombre = tblFrentes.DataTable({
                language: dtDicEsp,
                destroy: false,
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: "id", title: "id", visible: false },
                    { data: "lstSeguimiento", title: "C.C." },
                    { data: "Frente", title: "Frente" },
                    { data: "folioPpto", title: "Folio ppto" },
                    {
                        data: "fechaAsignacion", title: "Fecha asignación",
                        render: function (data, type, row) {
                            return moment(row.fechaAsignacion).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: "avance", title: "Avance",
                        render: function (data, type, row) {
                            let porcentaje = "";
                            if (data <= 19) {
                                porcentaje = `<div class="progress">
                                                    <div class="progress-bar bg-danger" role="progressbar" style="width: 20%" aria-valuenow="100"  aria-valuemin="0" aria-valuemax="100">${data}%</div>
                                                </div>`;
                            } else {
                                porcentaje = `<div class="progress">
                                                    <div class="progress-bar bg-danger" role="progressbar" style="width: ${data}%" aria-valuenow="100"  aria-valuemin="0" aria-valuemax="100">${data}%</div>
                                                </div>`;
                            }
                            return porcentaje;
                        }
                    },
                    {
                        data: "fechaRequerido", title: "Fecha requerido",
                        render: function (data, type, row) {
                            return moment(row.fechaRequerido).format('DD/MM/YYYY');
                        }
                    },
                    {
                        data: "fechaPromesa", title: "Fecha promesa",
                        render: function (data, type, row) {
                            return moment(row.fechaPromesa).format('DD/MM/YYYY');
                        }
                    },
                    { data: "tipoMotivo", title: "Motivo" },
                    {
                        render: function (data, type, row) {
                            let eliminarFrente = `<button class='btn-eliminar btn btn-xs btn-danger eliminarDetFrente' data-id="${row.id}">` +
                                `<i class="far fa-trash-alt"></i>
                                                </button>`;
                            return eliminarFrente;
                        }
                    },
                    //render: function (data, type, row) { }
                ],
                initComplete: function (settings, json) {
                    tblFrentes.on('click', '.eliminarDetFrente', function () {
                        let rowData = dtnombre.row($(this).closest("tr")).data();
                        let idFrente = parseFloat(rowData.id);
                        Alert2AccionConfirmar("¡Cuidado!", "¿Desea eliminar el registro seleccionado?", "Confirmar", "Cancelar", () => fncEliminarDetFrente(idFrente));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetFrente() {
            let arrFrentes = new Array();
            cboFrentes.val().forEach(element => {
                arrFrentes.push(element);
            });
            axios.post('GetDetFrentes', {
                AreaCuenta: cboProyecto.val(),
                lstFrentes: arrFrentes,
                estatusSeguimientoFrente: cboFiltroEstatusSeguimientoFrente.val() != "" ? cboFiltroEstatusSeguimientoFrente.val() : 0
            }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtnombre.clear();
                    dtnombre.rows.add(response.data.lstDetFrentes);
                    dtnombre.draw();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearDetFrente() {
            let objSeg = CrearDatosDetFrentes();
            axios.post('CrearDetFrentes2', { parametros: objSeg }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se asignó correctamente el frente.");
                    fncGetSeguimiento();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));

        }

        function CrearDatosDetFrentes() {
            let MensajeError = "";
            let objSeg = new Array();
            var datosTable = $("#tblAsignacionFrente").DataTable().rows().data();
            $("#tblAsignacionFrente").find('tbody').find('tr').each(function (index, value) {
                let idFrente = $(value).find('.asignarFrente').val();
                if (idFrente != "" && idFrente != null) {
                    obj = {
                        areaCuenta: cboProyecto.val(),
                        idFrente: $(value).find('.asignarFrente').val(),
                        idSeguimientoPpto: datosTable[index].id,
                        idInspTMC: datosTable[index].idInspTMC,
                    };
                    objSeg.push(obj);

                }
            });
            return objSeg;
        }

        function fncEliminarDetFrente(id) {
            if (id > 0) {
                let obj = new Object();
                obj = {
                    idFrente: id
                };
                axios.post('EliminarDetFrente', obj)
                    .catch(o_O => Alert2Error(o_O.message))
                    .then(response => {
                        let { success, items, message } = response.data;
                        if (success) {
                            Alert2Exito("Se ha eliminado con éxito el frente.");
                            fncGetFrentes();
                            if (cboProyecto.val() != "") {
                                fncGetFrente();
                            }
                        }
                        else {
                            Alert2Error(message);
                        }
                    }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al intentar eliminar el frente.");
            }
        }
        //END -- TABLA FRENTE #1
    }

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();