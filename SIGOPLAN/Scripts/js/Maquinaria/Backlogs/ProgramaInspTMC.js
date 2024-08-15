(function () {

    $.namespace("Maquinaria.Backlogs.ProgramaInspTMC");

    //#region CONST
    const btnProgramaRehabilitacion = $('#btnProgramaRehabilitacion');
    const btnProgramaInspeccion = $('#btnProgramaInspeccion');
    const btnPresupuestoRehabilitacion = $('#btnPresupuestoRehabilitacion');
    const btnSeguimientoPresupuestos = $('#btnSeguimientoPresupuestos');
    const btnFrenteBackLogs = $('#btnFrenteBackLogs');
    const btnIndicadoresRehabilitacionTMC = $('#btnIndicadoresRehabilitacionTMC');
    const btnInformeRehabilitacion = $('#btnInformeRehabilitacion');
    const lblFechaInicio = $('#lblFechaInicio');
    const lblAl = $('#lblAl');
    const lblFechaFinal = $('#lblFechaFinal');
    const cboFiltroPeriodo = $('#cboFiltroPeriodo');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const cboProyecto = $('#cboProyecto');
    const btnGuardar = $('#btnGuardar');
    const btnExportar = $('#btnExportar');
    const cboFiltroTipo = $('#cboFiltroTipo');
    const tblEnProgramacion = $('#tblEnProgramacion');
    const tblEnPrograma = $('#tblEnPrograma');

    const mdlActualizarInsp = $('#mdlActualizarInsp');
    const txtActualizarPartida = $('#txtActualizarPartida');
    const txtActualizarNoEconomico = $('#txtActualizarNoEconomico');
    const txtActualizarDescripcion = $('#txtActualizarDescripcion');
    const txtActualizarModelo = $('#txtActualizarModelo');
    const txtActualizarHoras = $('#txtActualizarHoras');
    const txtActualizarMotivo = $('#txtActualizarMotivo');
    const txtActualizarFechaProgramacion = $('#txtActualizarFechaProgramacion');
    const txtActualizarFechaRequerido = $('#txtActualizarFechaRequerido');
    const txtActualizarFechaPromesa = $('#txtActualizarFechaPromesa');
    const btnActualizarPrograma = $('#btnActualizarPrograma');
    const btnInicioTMC = $('#btnInicioTMC');
    const cboFiltroAnio = $('#cboFiltroAnio');

    let dtEnProgramacion;
    let dtEnPrograma;
    // let lstNoEconomicos = [];
    //#endregion

    BackLogs = function () {
        (function init() {
            fncListeners();
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
        };

        function fncListeners() {
            fncFillCboProyectosObra();
            fncFillCboPeriodos();
            fncHabilitarDeshabilitarControles();
            initTablaEnProgramacion();
            initTablaEnPrograma();

            btnInicioTMC.click(function (e) {
                document.location.href = '/BackLogs/IndexTMC?areaCuenta=' + cboProyecto.val();
            });

            btnProgramaInspeccion.click(function (e) {
                document.location.href = '/BackLogs/ProgramaInspTMC?areaCuenta=' + cboProyecto.val();
            });
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

            lblAl.hide();
            cboFiltroPeriodo.change(function (e) {
                if ($(this).val() > 0) {
                    let dataPrefijo = $('select[id="cboFiltroPeriodo"] option:selected').attr("data-prefijo");
                    let dataFechas = dataPrefijo.split("|");
                    for (let i = 0; i < dataFechas.length; i++) {
                        if (i == 0) {
                            lblFechaInicio.text(dataFechas[0]);
                        } else {
                            lblFechaFinal.text(dataFechas[1]);
                        }
                        lblFechaInicio.addClass("lblFechaInicioFinal");
                        lblFechaFinal.addClass("lblFechaInicioFinal");
                        lblFechaInicio.show();
                        lblFechaFinal.show();
                        lblAl.show();
                    }
                } else {
                    lblFechaInicio.hide();
                    lblAl.hide();
                    lblFechaFinal.hide();
                }
            });

            btnFiltroBuscar.click(function (e) { //OMAR
                fncGetProgramacionInspeccionTMC();
                fncGetProgramaInspeccionTMC();
            });

            cboProyecto.change(function (e) {
                if ($(this).val() != "") {
                    fncGetPeriodoActual();
                    fncFillTipoMaquinariaTMC();
                }
                fncHabilitarDeshabilitarControles();
            });

            btnGuardar.click(function (e) {
                fncCrearProgramacion();
            });

            btnActualizarPrograma.click(function (e) {
                fncActualizarInspPrograma();
            });
        }

        cboFiltroAnio.change(function () {
            fncFillCboPeriodos();
            fncGetPeriodoActual();
            fncHabilitarDeshabilitarControles();
        })

        function fncCrearProgramacion() {
            let objInsp = fncGetDatosEnProgramacion();
            if (objInsp != null && objInsp.length > 0) {
                axios.post("GuardarProgramacionTMC", objInsp).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito("Se ha registrado con éxito las rehabilitaciones.");
                        fncGetProgramacionInspeccionTMC();
                        fncGetProgramaInspeccionTMC();
                    } else {
                        Alert2Error("Es necesario ");
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetDatosEnProgramacion() {
            let strMensajeError = "";
            let objInsp = new Array();
            var datosDT = $('#tblEnProgramacion').DataTable().rows().data();

            $('#tblEnProgramacion').find('tbody').find('tr').each(function (index, value) {
                let esRehabilitar = $(value).find('.rehabilitar').prop("checked");
                let motivoRehabilitacion = $(value).find('.motivoRehabilitacion').val();
                let fechaRequerido = $(value).find('.fechaRequeridoRehabilitacion').val();
                let fechaPromesa = $(value).find('.fechaPromesaRehabilitacion').val();

                if (esRehabilitar && motivoRehabilitacion != "" && fechaRequerido != "" && fechaPromesa != "") {
                    obj = {
                        areaCuenta: $("#cboProyecto").val(),
                        periodo: $("#cboFiltroPeriodo").val(),
                        noEconomico: datosDT[index].noEconomico,
                        horas: datosDT[index].horas,
                        esRehabilitar: $(value).find('.rehabilitar').prop("checked"),
                        idMotivo: $(value).find('.motivoRehabilitacion').val(),
                        fechaRequerido: $(value).find('.fechaRequeridoRehabilitacion').val(),
                        fechaPromesa: $(value).find('.fechaPromesaRehabilitacion').val(),
                        partida: datosDT[index].partida
                    };
                    objInsp.push(obj);
                }

                if (esRehabilitar && motivoRehabilitacion == "") {
                    $(value).find('.motivoRehabilitacion').closest("td").css("background-color", "#ff0000");
                    strMensajeError += "En caso de rehabilitación, es necesario indicar el motivo.<br>";
                } else {
                    $(value).find('.motivoRehabilitacion').closest("td").css("background-color", "");
                }

                if (!esRehabilitar && motivoRehabilitacion != "") {
                    $(value).find('.rehabilitar').closest("td").css("background-color", "#ff0000");
                    strMensajeError += `En caso de seleccionar un motivo, es necesario marcar el campo "Rehabilitación".`;
                } else {
                    $(value).find('.rehabilitar').closest("td").css("background-color", "");
                }

                if ((esRehabilitar || motivoRehabilitacion != "") && fechaRequerido == "") {
                    $(value).find('.fechaRequeridoRehabilitacion').closest("td").css("background-color", "#ff0000");
                    strMensajeError += `Es necesario indicar "Fecha requerido".<br>`;
                } else {
                    $(value).find('.fechaRequeridoRehabilitacion').closest("td").css("background-color", "");
                }

                if ((esRehabilitar || motivoRehabilitacion != "") && fechaPromesa == "") {
                    $(value).find('.fechaPromesaRehabilitacion').closest("td").css("background-color", "#ff0000");
                    strMensajeError += `Es necesario indicar "Fecha promesa".<br>`;
                } else {
                    $(value).find('.fechaPromesaRehabilitacion').closest("td").css("background-color", "");
                }

                if (strMensajeError != "") {
                    Alert2Warning(strMensajeError);
                }
            });
            return objInsp;
        }

        function fncFillCboPeriodos() {
            cboFiltroPeriodo.fillCombo("/BackLogs/FillPeriodos", { anio: cboFiltroAnio.val() }, false);
            cboFiltroPeriodo.select2();
        }

        function fncFillTipoMaquinariaTMC() {
            cboFiltroTipo.fillCombo("/BackLogs/FillTipoMaquinariaTMC", {}, false);
            cboFiltroTipo.select2();
        }

        function fncHabilitarDeshabilitarControles() {
            if (cboProyecto.val() != "") {
                // cboFiltroPeriodo.attr("disabled", false);
                btnFiltroBuscar.attr("disabled", false);
                btnGuardar.attr("disabled", false);
                btnExportar.attr("disabled", false);
                cboFiltroTipo.attr("disabled", false);
            } else {
                cboFiltroPeriodo.attr("disabled", true);
                btnFiltroBuscar.attr("disabled", true);
                btnGuardar.attr("disabled", true);
                btnExportar.attr("disabled", true);
                cboFiltroTipo.attr("disabled", true);
            }
            cboFiltroPeriodo.select2();
            cboFiltroTipo.select2();
        }

        function fncFillCboProyectosObra() {
            let idProyecto = cboProyecto.val();
            if (idProyecto == null) {
                cboProyecto.fillCombo("/BackLogs/FillAreasCuentasTMC", {}, false);
                cboProyecto.select2({
                    width: "resolve"
                });
            }
        }

        function fncGetPeriodoActual() {
            axios.post("GetPeriodoActual").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let periodoActual = response.data.periodoActual;
                    cboFiltroPeriodo.val(periodoActual);
                    cboFiltroPeriodo.trigger("change");
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaEnProgramacion() {
            dtEnProgramacion = tblEnProgramacion.DataTable({
                language: dtDicEsp,
                destroy: true,
                ordering: false,
                paging: false,
                searching: false,
                bFilter: true,
                info: false,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'partida', title: 'PARTIDA' },
                    { data: 'noEconomico', title: 'ECONÓMICO' },
                    { data: 'descripcion', title: 'DESCRIPCIÓN', visible: false },
                    { data: 'modelo', title: 'MODELO', visible: false },
                    { data: 'horas', title: 'HORAS' },
                    {
                        data: "esRehabilitar", title: 'REHABILITAR',
                        render: function (data, type, row) {
                            return `<input type="checkbox" class="rehabilitar" data-checked="0" />`;
                        }
                    },
                    {
                        data: "idMotivo", title: 'MOTIVO',
                        render: function (data, type, row) {
                            return `<select class="form-control motivoRehabilitacion">
                                        <option value="">--Seleccione--</option>
                                        <option value="0">Obra</option>
                                        <option value="1">Venta</option>
                                    </select>`;
                        }
                    },
                    {
                        data: "fechaRequerido", title: 'FECHA REQUERIDO',
                        render: function (data, type, row) {
                            return `<input type="date" class="form-control fechaRequeridoRehabilitacion" />`;
                        }
                    },
                    {
                        data: "fechaPromesa", title: 'FECHA PROMESA',
                        render: function (data, type, row) {
                            return `<input type="date" class="form-control fechaPromesaRehabilitacion" />`;
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblEnProgramacion.on("click", ".rehabilitar", function () {
                        const rowData = dtEnProgramacion.row($(this).closest("tr")).data();
                        rowData.esRehabilitar = $(".rehabilitar").prop("checked");
                    });

                    tblEnProgramacion.on("change", ".motivoRehabilitacion", function () {
                        const rowData = dtEnProgramacion.row($(this).closest("tr")).data();
                        rowData.idMotivo = $(".motivoRehabilitacion").val();
                    });

                    tblEnProgramacion.on("change", ".fechaRequeridoRehabilitacion", function () {
                        const rowData = dtEnProgramacion.row($(this).closest("tr")).data();
                        rowData.fechaRequerido = $(".fechaRequeridoRehabilitacion").val();
                    });

                    tblEnProgramacion.on("change", ".fechaPromesaRehabilitacion", function () {
                        const rowData = dtEnProgramacion.row($(this).closest("tr")).data();
                        rowData.fechaPromesa = $(".fechaPromesaRehabilitacion").val();
                    });
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
            });
        }

        function fncGetProgramacionInspeccionTMC() {
            let objFiltro = new Object();
            objFiltro = {
                areaCuenta: $("#cboProyecto").val(),
                tipoMaquina: $("#cboFiltroTipo").val()
            };
            axios.post("GetProgramacionInspeccionTMC", objFiltro).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtEnProgramacion.clear();
                    dtEnProgramacion.rows.add(response.data.lstProgramacionInspTMC);
                    dtEnProgramacion.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function initTablaEnPrograma() {
            dtEnPrograma = tblEnPrograma.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', visible: false },
                    { data: 'partida', title: 'PARTIDA' },
                    { data: 'noEconomico', title: 'ECONÓMICO' },
                    { data: 'descripcion', title: 'DESCRIPCIÓN', visible: false },
                    { data: 'modelo', title: 'MODELO', visible: false },
                    { data: 'horas', title: 'HORAS' },
                    { data: 'motivo', title: 'MOTIVO' },
                    {
                        data: 'fechaProgramacion', title: 'FECHA DE PROGRAMACIÓN',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        data: 'fechaRequerido', title: 'FECHA REQUERIDO',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        data: 'fechaPromesa', title: 'FECHA PROMESA',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    {
                        render: function (data, type, row) {
                            let btnActualizar = `<button class="btn btn-warning editarPrograma"><i class="fas fa-pencil-alt"></i></button>&nbsp;`;
                            let btnEliminar = `<button class="btn btn-danger eliminarPrograma"><i class="fas fa-trash"></i></button>`;
                            let btns = btnActualizar + btnEliminar
                            return btns;
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblEnPrograma.on('click', '.editarPrograma', function () {
                        let rowData = dtEnPrograma.row($(this).closest('tr')).data();
                        fncDeshabilitarCtrlActualizarPrograma();
                        txtActualizarPartida.val(rowData.partida);
                        txtActualizarNoEconomico.val(rowData.noEconomico);
                        txtActualizarDescripcion.val(rowData.descripcion);
                        txtActualizarModelo.val(rowData.modelo);
                        txtActualizarHoras.val(rowData.horas);
                        txtActualizarMotivo.val(rowData.motivo);
                        txtActualizarFechaProgramacion.val(moment(rowData.fechaProgramacion).format("YYYY-MM-DD"));
                        txtActualizarFechaRequerido.val(moment(rowData.fechaRequerido).format("YYYY-MM-DD"));
                        txtActualizarFechaPromesa.val(moment(rowData.fechaPromesa).format("YYYY-MM-DD"));
                        btnActualizarPrograma.attr("data-id", rowData.id);
                        mdlActualizarInsp.modal("show");
                    });

                    tblEnPrograma.on('click', '.eliminarPrograma', function () {
                        let rowData = dtEnPrograma.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarProgramaInspTMC(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncDeshabilitarCtrlActualizarPrograma() {
            txtActualizarPartida.attr("disabled", true);
            txtActualizarNoEconomico.attr("disabled", true);
            txtActualizarDescripcion.attr("disabled", true);
            txtActualizarModelo.attr("disabled", true);
            txtActualizarHoras.attr("disabled", true);
            txtActualizarMotivo.attr("disabled", true);
            txtActualizarFechaProgramacion.attr("disabled", true);
        }

        function fncGetProgramaInspeccionTMC() {
            axios.post("GetProgramaInspeccionTMC").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    dtEnPrograma.clear();
                    dtEnPrograma.rows.add(response.data.lstProgramaInspTMC);
                    dtEnPrograma.draw();
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncEliminarProgramaInspTMC(id) {
            let obj = new Object();
            obj = {
                id: parseFloat(id)
            };
            axios.post("EliminarProgramaInspTMC", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al eliminar el registro.");
                    fncGetProgramacionInspeccionTMC();
                    fncGetProgramaInspeccionTMC();
                    mdlActualizarInsp.modal("hide");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncActualizarInspPrograma() {
            let obj = fncGetDatosActualizar();
            axios.post("ActualizarProgramaInspeccionTMC", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al actualizar el registro.");
                    btnActualizarPrograma.attr("data-id", 0);
                    fncGetProgramaInspeccionTMC();
                    mdlActualizarInsp.modal("hide");
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDatosActualizar() {
            let strMensajeError = "";
            txtActualizarFechaRequerido.val() == "" ? strMensajeError += `Es necesario indicar la "Fecha requerido".<br>` : "";
            txtActualizarFechaPromesa.val() == "" ? strMensajeError += `Es necesario indicar la "Fecha promesa".` : "";
            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
            } else {
                let obj = new Object();
                obj = {
                    id: btnActualizarPrograma.attr("data-id"),
                    fechaRequerido: txtActualizarFechaRequerido.val(),
                    fechaPromesa: txtActualizarFechaPromesa.val()
                };
                return obj;
            }
        }
    };

    $(document).ready(() => BackLogs = new BackLogs())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();