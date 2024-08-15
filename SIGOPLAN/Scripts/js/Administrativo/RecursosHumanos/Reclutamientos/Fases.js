(() => {
    $.namespace('CH.Fases');

    //#region CONST FASES

    //#region CONST
    const tblRH_REC_Fases = $('#tblRH_REC_Fases');
    const btnFiltroNuevaFase = $('#btnFiltroNuevaFase');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    //#endregion

    //#region CONST CREAR/EDITAR FASE
    const mdlCrearEditarFase = $('#mdlCrearEditarFase');
    const spanTitleCrearEditarFase = $('#spanTitleCrearEditarFase');
    const txtCrearEditarNombreFase = $('#txtCrearEditarNombreFase');
    const btnCrearEditarFase = $('#btnCrearEditarFase');
    const spanCrearEditarFase = $('#spanCrearEditarFase');
    //#endregion

    //#endregion

    //#region CONST ACTIVIDADES
    const btnFiltroAsignarEncargadoFase = $('#btnFiltroAsignarEncargadoFase');
    const btnFiltroNuevaActividad = $('#btnFiltroNuevaActividad');
    const spanTitleLstActividad = $('#spanTitleLstActividad');
    const mdlLstActividad = $('#mdlLstActividad');
    const tblRH_REC_Actividades = $('#tblRH_REC_Actividades');
    let dtActividades;
    const mdlCrearEditarActividad = $('#mdlCrearEditarActividad');
    const spanTitleCrearEditarActividad = $('#spanTitleCrearEditarActividad');
    const txtCrearEditarActividad = $('#txtCrearEditarActividad');
    const chkCrearEditarActividadArchivos = $('#chkCrearEditarActividadArchivos');
    const chkCrearEditarActividadObligatoria = $('#chkCrearEditarActividadObligatoria');
    const chkCrearEditarActividadGeneral = $('#chkCrearEditarActividadGeneral');
    const chkCrearEditarActividadCalificacion = $('#chkCrearEditarActividadCalificacion');
    const txtCrearEditarDescActividad = $('#txtCrearEditarDescActividad');
    const btnCrearEditarActividad = $('#btnCrearEditarActividad');
    const spanCrearEditarActividad = $('#spanCrearEditarActividad');
    const chkCrearEditarActividadNecesarioAprobar = $('#chkCrearEditarActividadNecesarioAprobar');
    const cboCrearEditarActividadTipoArchivo = $('#cboCrearEditarActividadTipoArchivo');
    //#endregion

    //#endregion

    //#region CONST PUESTOS REL FASE
    const mdlLstPuestosRelFase = $('#mdlLstPuestosRelFase');
    const tblRH_REC_PuestosRelFases = $('#tblRH_REC_PuestosRelFases');
    const cboCrearEditarPuestoRelFase = $('#cboCrearEditarPuestoRelFase');
    const btnCrearEditarPuestoRelFase = $('#btnCrearEditarPuestoRelFase');
    const btnFiltroBuscarPuestosRelFase = $('#btnFiltroBuscarPuestosRelFase');
    const divCrearPuestoRelFaseCancelar = $('#divCrearPuestoRelFaseCancelar');
    const btnCrearEditarPuestoRelFaseCancelar = $('#btnCrearEditarPuestoRelFaseCancelar');
    let dtPuestosRelFases;
    //#endregion

    //#region CONST ENCARGADOS FASES/ACTIVIDADES
    const mdlLstEncargadosFasesActividades = $('#mdlLstEncargadosFasesActividades');
    const acordionFasesActividades = $('#acordionFasesActividades');
    //#endregion

    Fases = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {

            //#region EVENTOS FASES
            initTblFases();
            fncGetFases();

            btnFiltroBuscar.on("click", function () {
                fncGetFases();
            });

            btnFiltroNuevaFase.on("click", function () {
                spanTitleCrearEditarFase.html("Nueva fase");
                txtCrearEditarNombreFase.val("");
                btnCrearEditarFase.attr("data-id", 0);
                spanCrearEditarFase.html("Guardar");
                mdlCrearEditarFase.modal("show");
            });

            btnCrearEditarFase.on("click", function () {
                fncCrearEditarFase();
            });
            //#endregion

            //#region EVENTOS ACTIVIDADES
            initTblActividades();

            btnFiltroNuevaActividad.on("click", function () {
                mdlCrearEditarActividad.modal("show");
                txtCrearEditarActividad.val("");
                chkCrearEditarActividadArchivos.prop("checked", true);
                chkCrearEditarActividadArchivos.trigger("change");
                chkCrearEditarActividadObligatoria.prop("checked", true);
                chkCrearEditarActividadObligatoria.trigger("change");
                txtCrearEditarDescActividad.val("");
                btnCrearEditarActividad.attr("data-id", 0);
                spanCrearEditarActividad.html("Guardar");
            });

            btnCrearEditarActividad.on("click", function () {
                fncCrearEditarActividad();
            });

            btnFiltroAsignarEncargadoFase.on("click", function () {
                Alert2AccionConfirmar('Encargado de fase', '¿Desea asignar un encargado a la fase completa?', 'Confirmar', 'Cancelar', () => fncSwAlert2AsignarEncargadoFase());
            });

            // $(document).on('click', '.verActividadesAcordion', function () {
            //     let idFase = $(this).data().id;
            //     fncGetActividades(idFase, 1);
            // });
            //#endregion

            //#region EVENTOS PUESTOS REL FASES
            initTblPuestosRelFase();

            //#region FILL CBOS
            cboCrearEditarPuestoRelFase.fillCombo("/Reclutamientos/FillFiltroCboPuestos", {}, false);
            cboCrearEditarPuestoRelFase.select2({ width: '100%' });

            cboCrearEditarActividadTipoArchivo.fillCombo("/Reclutamientos/FillComboEDArchivos", {}, false);
            cboCrearEditarActividadTipoArchivo.select2({ width: '100%', height: '100%' });
            //#endregion

            btnFiltroBuscarPuestosRelFase.on("click", function () {
                fncGetPuestosRelFase();
            });

            btnCrearEditarPuestoRelFaseCancelar.on("click", function () {
                divCrearPuestoRelFaseCancelar.trigger("click");
            });

            btnCrearEditarPuestoRelFase.on("click", function () {
                fncCrearEditarPuestoRelFase();
            });

            chkCrearEditarActividadArchivos.on("change", function () {
                if ($(this).prop("checked")) {
                    cboCrearEditarActividadTipoArchivo.attr("disabled", false);
                } else {
                    cboCrearEditarActividadTipoArchivo.attr("disabled", true);
                }
            });
            //#endregion
        }

        //#region CRUD FASES
        function initTblFases() {
            dtFases = tblRH_REC_Fases.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreFase', title: 'Fase' },
                    { data: 'cantActividades', title: 'Cant. actividades' },
                    {
                        render: function (data, type, row) {
                            // let btnActualizar = `<button class="btn btn-warning actualizarNombreFase btn-xs" title="Actualizar nombre de la fase."><i class="far fa-edit"></i></button>&nbsp;`;
                            let btnActividades = `<button class="btn btn-success lstActividades btn-xs" title="Listado de actividades asignadas a la fase."><i class="fas fa-tasks"></i></button>&nbsp;`;
                            // let btnPuestos = `<button class="btn btn-primary lstPuestosRelFase btn-xs" title="Listado de puestos relacionados a la fase."><i class="fas fa-user-friends"></i></button>&nbsp;`;
                            // let btnEncargados = `<button class="btn btn-primary encargadosFasesActividades btn-xs"><i class="fas fa-users-cog"></i></button>&nbsp;`;
                            // let btnEliminar = `<button class="btn btn-danger eliminarNombreFase btn-xs" title="Eliminar fase."><i class="far fa-trash-alt"></i></button>`;
                            return /*btnActualizar +*/ btnActividades;
                        }
                    },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Fases.on('click', '.actualizarNombreFase', function () {
                        let rowData = dtFases.row($(this).closest('tr')).data();
                        txtCrearEditarNombreFase.val(rowData.nombreFase);
                        btnCrearEditarFase.attr("data-id", rowData.id);
                        spanTitleCrearEditarFase.html("Actualizar fase");
                        spanCrearEditarFase.html("Actualizar");
                        mdlCrearEditarFase.modal("show");
                    });
                    tblRH_REC_Fases.on('click', '.eliminarNombreFase', function () {
                        let rowData = dtFases.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarFase(rowData.id));
                    });
                    tblRH_REC_Fases.on("click", ".lstPuestosRelFase", function () {
                        let rowData = dtFases.row($(this).closest("tr")).data();
                        btnCrearEditarPuestoRelFase.attr("data-id", rowData.id);
                        dtPuestosRelFases.clear();
                        dtPuestosRelFases.draw();
                        mdlLstPuestosRelFase.modal("show");
                    });
                    tblRH_REC_Fases.on('click', '.lstActividades', function () {
                        let rowData = dtFases.row($(this).closest('tr')).data();
                        spanTitleLstActividad.html("Actividades");
                        btnFiltroNuevaActividad.attr("data-id", rowData.id);
                        mdlLstActividad.modal("show");
                        fncGetActividades(rowData.id);
                    });
                    tblRH_REC_Fases.on("click", ".encargadosFasesActividades", function () {
                        fncGetFasesEncargados();
                        mdlLstEncargadosFasesActividades.modal("show");
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "20%", "targets": 1 }
                ],
            });
        }

        function fncGetFases() {
            axios.post("GetFases").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtFases.clear();
                    dtFases.rows.add(response.data.lstFases);
                    dtFases.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarFase() {
            let objFase = fncObjFase();
            axios.post("CrearEditarFase", objFase).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetFases();
                    mdlCrearEditarFase.modal("hide");
                    let strMensaje = btnCrearEditarFase.attr("data-id") > 0 ? "Éxito al actualizar la fase." : "Éxito al registrar la fase.";
                    Alert2Exito(strMensaje);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncObjFase() {
            let objFase = new Object();
            objFase = {
                id: btnCrearEditarFase.attr("data-id"),
                nombreFase: txtCrearEditarNombreFase.val()
            }
            return objFase;
        }

        function fncEliminarFase(idFase) {
            let objEliminar = new Object();
            objEliminar = {
                idFase: idFase
            };
            axios.post("EliminarFase", objEliminar).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetFases();
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncSwAlert2AsignarEncargadoFase(idActividad) {
            Swal.fire({
                position: "center",
                icon: 'question',
                title: 'Encargado de fase',
                width: '35%',
                html: `<h3>Seleccionar a encargado de fase<h3><h4 style="font-size: 13px;">
                        <span>Nota:&nbsp;</span>Al asignar un encargado a la fase, todas las actividades serán asignadas a él solamente, 
                        en caso que haya otro usuario encargado a una actividad en específico, se sobrescribirá.</h4>`,
                confirmButtonText: "Confirmar",
                confirmButtonColor: "#5cb85c",
                cancelButtonText: "Cancelar",
                cancelButtonColor: "#d9534f",
                showCancelButton: true,
                input: 'select',
                inputValidator: (value) => {
                    if (value > 0) {
                        fncAsignarEncargadoFase(value, idActividad);
                    } else {
                        Alert2Warning("Es necesario seleccionar a un usuario.");
                    }
                }
            });
            $(".swal2-select").fillCombo("/Reclutamientos/FillCboUsuarios", {}, false);
            $(".swal2-select").select2({ width: "100%" });
        }

        function fncAsignarEncargadoFase(idUsuarioEncargado, idActividad) {
            let obj = new Object();
            obj = {
                idFase: btnFiltroNuevaActividad.attr("data-id"),
                id: idActividad,
                idUsuarioEncargado: idUsuarioEncargado
            }
            axios.post("AsignarEncargadoFase", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Se ha asignado con éxito.");
                    fncGetActividades();
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CRUD ACTIVIDADES
        function initTblActividades() {
            dtActividades = tblRH_REC_Actividades.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'tituloActividad', title: 'Actividad' },
                    { data: 'descActividad', title: 'Descripción' },
                    {
                        data: 'esArchivos', title: 'Requiere<br>archivos', width: '5%',
                        render: function (data, type, row) {
                            if (data) {
                                return "SI";
                                // return `<input type="checkbox" checked="checked" onclick="return false" />`;
                            } else {
                                return "NO";
                                // return `<input type="checkbox" onclick="return false" />`;
                            }
                        }
                    },
                    {
                        data: 'esObligatoria', title: 'Actividad<br>obligatoria',
                        render: function (data, type, row) {
                            if (data) {
                                return `SI`
                                // return `<input type="checkbox" checked="checked" onclick="return false" />`;
                            } else {
                                return `NO`
                                // return `<input type="checkbox" onclick="return false" />`;
                            }
                        }
                    },
                    {
                        data: "esCalificacion", title: 'Requiere<br>calificación',
                        render: function (data, type, row) {
                            if (data) {
                                return `SI`
                                // return `<input type="checkbox" checked="checked" onclick="return false" />`;
                            } else {
                                return `NO`
                                // return `<input type="checkbox" onclick="return false" />`;
                            }
                        }
                    },
                    {
                        data: "esNecesarioAprobar", title: 'Requiere<br>aprobar',
                        render: function (data, type, row) {
                            if (data) {
                                return `SI`
                                // return `<input type="checkbox" checked="checked" onclick="return false" />`;
                            } else {
                                return `NO`
                                // return `<input type="checkbox" onclick="return false" />`;
                            }
                        }
                    },
                    { data: 'nombreEncargado', title: 'Encargado' },
                    {
                        "width": "8%",
                        render: function (data, type, row) {
                            // let btnActualizar = `<button class="btn btn-warning actualizarActividad btn-xs"><i class="far fa-edit"></i></button>&nbsp;`;
                            // let btnEliminar = `<button class="btn btn-danger eliminarActividad btn-xs"><i class="far fa-trash-alt"></i></button>&nbsp;`;
                            // let btnAsignarEncargado = `<button class="btn btn-primary asignarEncargado btn-xs"><i class="fas fa-thumbtack"></i></button>`;
                            // return /*btnActualizar +*/ btnEliminar + btnAsignarEncargado;
                            return '';
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'idFase', visible: false },
                    { data: 'esGeneral', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_Actividades.on('click', '.actualizarActividad', function () {
                        let rowData = dtActividades.row($(this).closest('tr')).data();
                        txtCrearEditarActividad.val(rowData.tituloActividad);
                        txtCrearEditarDescActividad.val(rowData.descActividad);
                        chkCrearEditarActividadArchivos.prop("checked", rowData.esArchivos);
                        chkCrearEditarActividadArchivos.trigger("change");
                        chkCrearEditarActividadObligatoria.prop("checked", rowData.esObligatoria);
                        chkCrearEditarActividadObligatoria.trigger("change");
                        chkCrearEditarActividadGeneral.prop("checked", rowData.esGeneral);
                        chkCrearEditarActividadGeneral.trigger("change");
                        chkCrearEditarActividadCalificacion.prop("checked", rowData.esCalificacion);
                        chkCrearEditarActividadCalificacion.trigger("change");
                        chkCrearEditarActividadNecesarioAprobar.prop("checked", rowData.esNecesarioAprobar);
                        chkCrearEditarActividadNecesarioAprobar.trigger("change");
                        cboCrearEditarActividadTipoArchivo.val(rowData.tipoArchivo == 0 ? null : rowData.tipoArchivo);
                        cboCrearEditarActividadTipoArchivo.trigger("change");
                        btnCrearEditarActividad.attr("data-id", rowData.id);
                        spanTitleCrearEditarActividad.html("Actualizar actividad");
                        spanCrearEditarActividad.html("Actualizar");
                        mdlCrearEditarActividad.modal("show");
                    });
                    tblRH_REC_Actividades.on('click', '.eliminarActividad', function () {
                        let rowData = dtActividades.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarActividad(rowData.id));
                    });
                    tblRH_REC_Actividades.on("click", ".asignarEncargado", function () {
                        let rowData = dtActividades.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('Encargado de fase', '¿Desea asignar un encargado a la actividad seleccionada?', 'Confirmar', 'Cancelar', () => fncSwAlert2AsignarEncargadoFase(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': "_all" }
                    // { "width": "8%", "targets": 6 }
                ],
            });
        }

        function fncCrearEditarActividad() {
            let objActividad = fncObjActividad();
            axios.post("CrearEditarActividad", objActividad).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetActividades(btnCrearEditarActividad.attr("data-id"));
                    mdlCrearEditarActividad.modal("hide");
                    let strMensaje = btnCrearEditarActividad.attr("data-id") > 0 ? "Éxito al actualizar la actividad." : "Éxito al registrar la actividad";
                    fncGetFases();
                    Alert2Exito(strMensaje);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncObjActividad() {
            let objActividad = new Object();
            objActividad = {
                id: btnCrearEditarActividad.attr("data-id"),
                idFase: btnFiltroNuevaActividad.attr("data-id"),
                tituloActividad: txtCrearEditarActividad.val(),
                descActividad: txtCrearEditarDescActividad.val(),
                esArchivos: chkCrearEditarActividadArchivos.prop("checked"),
                esObligatoria: chkCrearEditarActividadObligatoria.prop("checked"),
                esGeneral: chkCrearEditarActividadGeneral.prop("checked"),
                esCalificacion: chkCrearEditarActividadCalificacion.prop("checked"),
                esNecesarioAprobar: chkCrearEditarActividadNecesarioAprobar.prop("checked")
            };

            if (objActividad.esArchivos) {
                objActividad.tipoArchivo = cboCrearEditarActividadTipoArchivo.val();
            } else {
                objActividad.tipoArchivo = 0;
            }

            return objActividad;
        }

        function fncEliminarActividad(idActividad) {
            let objEliminar = new Object();
            objEliminar = {
                idActividad: idActividad
            };
            axios.post("EliminarActividad", objEliminar).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetActividades();
                    fncGetFases();
                    Alert2Exito("Se ha eliminado con éxito el registro");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetActividades(idFase, esAcordion) {
            let objActividad = new Object();
            objActividad = {
                idFase: esAcordion == 1 ? idFase : btnFiltroNuevaActividad.attr("data-id")
            };
            axios.post("GetActividades", objActividad).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    if (esAcordion == 1) {
                        //#region SE MUESTRA LAS ACTIVIDADES DE LA FASE SELECCIONADA
                        $(`#span${idFase}`).html(html);
                        //#endregion
                    } else {
                        //#region FILL DATATABLE
                        dtActividades.clear();
                        dtActividades.rows.add(response.data.lstActividades);
                        dtActividades.draw();
                        //#endregion
                    }
                } else {
                    //Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CRUD PUESTOS REL FASE
        function initTblPuestosRelFase() {
            dtPuestosRelFases = tblRH_REC_PuestosRelFases.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'puesto', title: 'Puesto' },
                    {
                        render: function (data, type, row) {
                            return `<button class="btn btn-danger eliminarPuestoRelFase btn-xs" title="Eliminar fase."><i class="far fa-trash-alt"></i></button>`;
                        }
                    },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblRH_REC_PuestosRelFases.on('click', '.eliminarPuestoRelFase', function () {
                        let rowData = dtPuestosRelFases.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarPuestoRelFase(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetPuestosRelFase() {
            let objFiltro = new Object();
            objFiltro = {
                idFase: btnCrearEditarPuestoRelFase.attr("data-id")
            };
            axios.post("GetPuestosRelFase", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtPuestosRelFases.clear();
                    dtPuestosRelFases.rows.add(response.data.lstPuestosRelFase);
                    dtPuestosRelFases.draw();
                    //#endregion
                } else {
                    //Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarPuestoRelFase() {
            let strFiltro = $('select[id="cboCrearEditarPuestoRelFase"] option:selected').text();
            let objCrearEditarPuestoRelFase = new Object();
            objCrearEditarPuestoRelFase = {
                idFase: btnCrearEditarPuestoRelFase.attr("data-id"),
                idPuesto: cboCrearEditarPuestoRelFase.val(),
                puesto: strFiltro == "--Seleccione--" ? "" : strFiltro
            };

            if (strFiltro != "--Seleccione--") {
                axios.post("CrearPuestoRelFase", objCrearEditarPuestoRelFase).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        cboCrearEditarPuestoRelFase[0].selectedIndex = 0;
                        cboCrearEditarPuestoRelFase.trigger("change");
                        fncGetPuestosRelFase(btnCrearEditarPuestoRelFase.attr("data-id"));
                        Alert2Exito("Se ha registrado con éxito el puesto a la fase");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Es necesario seleccionar un puesto.");
            }
        }

        function fncEliminarPuestoRelFase(idPuestoRelFase) {
            let objEliminar = new Object();
            objEliminar = {
                idPuestoRelFase: idPuestoRelFase
            };
            axios.post("EliminarPuestoRelFase", objEliminar).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetPuestosRelFase(idPuestoRelFase);
                    Alert2Exito("Se ha eliminado con éxito el registro.");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region CRUD ENCARGADOS FASES/ACTIVIDADES
        function fncGetFasesEncargados() {
            axios.post("GetFases").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region SE CONSTRUYE ACORDION CON LAS FASES/ACTIVIDADES
                    let lstFases = response.data.lstFases;
                    let cantFases = response.data.lstFases.length;
                    fncAcordionFasesActividades(lstFases, cantFases);
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncAcordionFasesActividades(lstFases, cantFases) {
            let arrFasesID = [];
            for (let i = 0; i < lstFases.length; i++) {
                arrFasesID.push(lstFases[i].id);
            }
            let obj = new Object();
            obj = {
                lstFasesID: arrFasesID
            }
            axios.post("GetActividades", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let acordion = "";
                    for (let i = 0; i < cantFases; i++) {
                        acordion += `<div class="panel-group" id="accordion">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle verActividadesAcordion" data-id="${lstFases[i].id}" data-toggle="collapse" data-parent="#accordion" href="#${lstFases[i].id}">${lstFases[i].nombreFase}</a>
                                        </h4>
                                    </div>
                                    <div id="${lstFases[i].id}" class="panel-collapse collapse">
                                        <div class="panel-body">${lstFases[i].nombreFase}
                                        <span id="span${lstFases[i].id}"></span>
                                    </div>
                                </div>
                            </div>`;
                    }
                    acordionFasesActividades.html(acordion);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Fases = new Fases();
    })

        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();