(() => {
    $.namespace('CapturaIncidente.Seguridad');

    Seguridad = function () {

        //#region  Selectores
        const _idEmpresa = $('#id-empresa');
        const divSeccion1 = $('#divSeccion1');
        const divSeccion2 = $('#divSeccion2');
        const divSeccion3 = $('#divSeccion3');
        const h1Titulo = $('#h1Titulo');
        const btnModalEvidencias = $('#btnModalEvidencias');
        const modalEvidencias = $('#modalEvidencias');
        const tablaEvidencias = $('#tablaEvidencias');
        const botonAgregarEvidencia = $('#botonAgregarEvidencia');
        const report = $("#report");
        const chkEsContratista = $('#chkEsContratista');
        const lblContratistas = $('#lblContratistas');
        const lblCC = $('#lblCC');
        const selectCC = $('#selectCC');
        const cboSelectCC = $('#cboSelectCC');
        const cboSelectContratista = $('#cboSelectContratista');
        //#endregion

        // Variables.
        const hoy = new Date();
        let esEdit = false;
        let evaluacionConsecuencias = 0
        let evaluacionProbabilidad = 0
        let esExterno = 0
        let informe_id = 0
        const getUrlParams = function (url) {
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
        let _flagInformePreliminar;

        let dtTablaEvidencias;

        //#region PETICIONES
        const getInforme = (id) => { return $.post('/Administrativo/IndicadoresSeguridad/GetInformePreliminarByID', { id }) };
        const getInfoEmpleado = (claveEmpleado, esContratista, idEmpresa) => {
            return $.post('/Administrativo/IndicadoresSeguridad/GetInfoEmpleado', { claveEmpleado: claveEmpleado, esContratista: esContratista, idEmpresaContratista: idEmpresa })
        };
        const getInfoEmpleadoConstratista = (empleado_id) => { return $.post('/Administrativo/IndicadoresSeguridad/GetInfoEmpleadoContratista', { empleado_id }) };
        const guardarAccidente = (incidente, grupoInvestigacion, ordenCronologico, eventoDetonador, causasInmediata, causasBasicas, causasRaiz, medidasControl) => { return $.post('/Administrativo/IndicadoresSeguridad/GuardarIncidente', { incidente, grupoInvestigacion, ordenCronologico, eventoDetonador, causasInmediata, causasBasicas, causasRaiz, medidasControl }) };
        const getIncidente = informeID => $.post('/Administrativo/IndicadoresSeguridad/ObtenerIncidentePorInformeID', { informeID });
        const guardarRegistro = (informe) => { return $.post('/Administrativo/IndicadoresSeguridad/GuardarInforme', { informe }) };
        const actualizarRegistro = (informe) => { return $.post('/Administrativo/IndicadoresSeguridad/UpdateInforme', { informe }) };
        //#endregion

        const variables = getUrlParams(window.location.href);

        // Función init autoejecutable.
        (function init() {
            _flagInformePreliminar = (variables.informe == 0 || variables.informeEditar == 1);

            cargarSelectsForm();
            fncValidarAccesoContratista();
            initNombreGrupoInvestigacion();
            initResponsableMedidasControl();
            initFechaMedidasControl();
            initTablaEvidencias();
            setDatosInicio();

            $("#chkEsContratista").bootstrapToggle();

            $('#divSubclasificacion').hide();

            $('#selectTipoAccidente').change(e => {
                const select = $(e.currentTarget);
                select.val() == 5 || (select.val() == 13 && _idEmpresa.val() == 6) ? $('#divSubclasificacion').show(500) : $('#divSubclasificacion').hide(500);
            });

            $('#claveSupervisor').on('change', function () {

                if ($(this).val() == null || $(this).val() == "") {
                    return;
                }

                let attrEsContratista = $("#btnGuardar").attr("data-esContratista");
                let idEmpresa = $('#selectCC option:selected').attr("empresa");
                let strAgrupacion = selectCC.val() != "" ? selectCC.val() : 0;
                let idAgrupacion;
                if (idEmpresa == 1000) {
                    idAgrupacion = strAgrupacion.replace("c_", "");
                } else if (idEmpresa == 2000) {
                    idAgrupacion = strAgrupacion.replace("a_", "");
                } else {
                    idAgrupacion = strAgrupacion;
                }
                let esContratista = false;
                if (attrEsContratista == "true") {
                    esContratista = true;
                } else {
                    if (chkEsContratista.prop('checked')) {
                        esContratista = true;
                    } else {
                        esContratista = false;
                    }
                }
                if (chkEsContratista.attr("esOculto") == 1 && selectCC.val() == "") {
                    Alert2Warning("Es necesario seleccionar una empresa");
                } else {
                    getInfoEmpleado(parseFloat(parseFloat($("#claveSupervisor").val())), esContratista, idAgrupacion).done(function (response) { //TODO
                        if (response.success) {
                            $('#supervisorCargo').val(response.empleadoInfo.nombreEmpleado)
                        } else {
                            $('#supervisorCargo').val('');
                        }
                    });
                }
            });

            if (variables.informe == 0 || variables.informeEditar == 1) {
                if (variables.informeEditar == 1) {
                    esEdit = true;
                    btnModalEvidencias.attr('disabled', true);

                    $.post('/Administrativo/IndicadoresSeguridad/GetInformePreliminarByID', { id: variables.informe }).then(response => {
                        if (response.success) {
                            llenarCamposInformePreliminar(response.informacion);
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );

                    bloquearCamposAutomaticos();
                } else {
                    esEdit = false;
                    btnModalEvidencias.attr('disabled', false);
                }

                $('#btnGuardar').click(guardarInformePreliminar);
                divSeccion2.css('display', 'none');
                h1Titulo.text('INFORMACIÓN PRELIMINAR.');
                $('.campoInformePreliminar').css('display', 'block');
                $('.campoIncidente').css('display', 'none');
                $('#btnModalEvidencias').click(() => { modalEvidencias.modal('show') });
            } else {
                $('#btnGuardar').click(guardarIncidente);
                divSeccion2.css('display', 'block');
                h1Titulo.text('REPORTE DE INVESTIGACIÓN DE ACCIDENTES (RIA).');
                $('.campoInformePreliminar').css('display', 'none');
                $('.campoIncidente').css('display', 'block');
                $('#btnModalEvidencias').click(() => { modalEvidencias.modal('show') });

                if (variables && variables.informe && variables.completo) {
                    deshabilitarCampos();

                    getIncidente(variables.informe)
                        .then(response => {
                            if (response.success) {
                                // Operación exitosa.
                                llenarCamposIncidente(response.informacion);
                            } else {
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.error}`);
                            }
                        }, error => {
                            // Error al lanzar la petición.
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else if (variables && variables.informe) {
                    informe_id = variables.informe
                    getInforme(variables.informe).done(response => {
                        if (response.success) {

                            const { informacion } = response;

                            $('input[name=empresa]').attr('disabled', true);
                            informacion.esExterno ? $('input[name=empresa]').first()[0].checked = true : $('input[name=empresa]').last()[0].checked = true;

                            $('#selectCC').val(informacion.idAgrupacion).change();
                            $('#selectCC').attr('disabled', true);

                            $('#selectTipoAccidente').val(informacion.tipoAccidente_id).change();
                            if (informacion.tipoAccidente_id == 5 || informacion.tipoAccidente_id == 13 && _idEmpresa.val() == 6) {
                                $('#divSubclasificacion').show();
                                if (informacion.subclasificacionID != 0) {
                                    $("#selectSubclasificacionIncidente").val(informacion.subclasificacionID).change();
                                }
                            }

                            if (informacion.departamento_id > 0) {
                                $("#selectDepartamento").val(informacion.departamento_id).change();
                            }
                            if (informacion.tipoContacto_id > 0) {
                                $("#selectTipoContacto").val(informacion.tipoContacto_id).change();
                            }
                            if (informacion.parteCuerpo_id > 0) {
                                $("#selectParteCuerpo").val(informacion.parteCuerpo_id).change();
                            }
                            if (informacion.agenteImplicado_id > 0) {
                                $("#selectAgenteImplicado").val(informacion.agenteImplicado_id).change();
                            }

                            establecerRiesgo(informacion.riesgo);

                            $('#inputHoraInicio').mask('00:00');
                            $('#inputHoraFin').mask('00:00');

                            $('#selectEmpleado').empty();
                            $('#selectEmpleado').attr('disabled', true);
                            $('#supervisorCargo').val(informacion.supervisorEmpleado).attr('disabled', true);
                            $('#claveSupervisor').val(informacion.claveSupervisor).attr('disabled', true);

                            if (informacion.esExterno) {
                                $('#selectEmpleado').off();
                                $('#selectEmpleado').append(`<option selected value="${informacion.nombreExterno}">${informacion.nombreExterno}</option>`);
                                $('#puestoPersonal').val(informacion.puestoEmpleado).attr('disabled', true);

                            } else {
                                $('#selectEmpleado').append(`<option selected value="${informacion.claveEmpleado}"></option>`);
                                $('#selectEmpleado').change();
                            }

                            $('#selectContratista').val(informacion.claveContratista).change();
                            $('#selectContratista').attr('disabled', true);

                            $('#fechaAccidente').val(moment(informacion.fechaIncidenteComplete).format("DD/MM/YYYY HH:mm"))

                            $('#selectExperienciaEmpleado').val(informacion.experienciaEmpleado_id > 0 ? informacion.experienciaEmpleado_id : '');
                            $('#selectExperienciaEmpleado').select2().change();
                            $('#selectAntiguedadEmpleado').val(informacion.antiguedadEmpleado_id > 0 ? informacion.antiguedadEmpleado_id : '');
                            $('#selectAntiguedadEmpleado').select2().change();
                            $('#selectTurnoEmpleado').val(informacion.turnoEmpleado_id > 0 ? informacion.turnoEmpleado_id : '');
                            $('#selectTurnoEmpleado').select2().change();
                            $('#horasTrabajadas').val(informacion.horasTrabajadasEmpleado > 0 ? informacion.horasTrabajadasEmpleado : '');
                            $('#diasTrabajados').val(informacion.diasTrabajadosEmpleado > 0 ? informacion.diasTrabajadosEmpleado : '');
                            $('#capacitadoP').find(`input[value="${informacion.capacitadoEmpleado ? '1' : !informacion.capacitadoEmpleado ? '0' : ''}"]`).prop('checked', true);
                            $('#accidentesA').find(`input[value="${informacion.accidentesAnterioresEmpleado ? '1' : !informacion.accidentesAnterioresEmpleado ? '0' : ''}"]`).prop('checked', true);
                        }
                    });
                }
            }

            $('#clienteIE').find('input').click(alternarPersonaExterna);
            $(".toggle-on").removeClass("btn-primary");
            $(".toggle-off").removeClass("btn-default");
            $(".toggle-on").css("width", "25%");
            $(".toggle-off").css("width", "25%");
            $("#chkEsContratista").css("width", "15%");

            chkEsContratista.change(function (e) {
                if ($(".claveEmpleado").val() != "") { $(".claveEmpleado").trigger("change"); }
                if ($("#claveSupervisor").val() != "") { $("#claveSupervisor").trigger("change"); }
                if ($(".claveEmpleadoInformo").val() != "") { $(".claveEmpleadoInformo").trigger("change"); }
            });
        })();

        $("#claveEmpleado").click(function (e) {
            $(this).select();
        });

        $("#claveSupervisor").click(function (e) {
            $(this).select();
        });

        $("#claveEmpleadoInformo").click(function (e) {
            $(this).select();
        });

        $('.claveEmpleado').on('change', function () {
            if ($(".claveEmpleado").val() == null || $(".claveEmpleado").val() == "") {
                return;
            }

            let attrEsContratista = $("#btnGuardar").attr("data-esContratista");
            let idEmpresa = $('#selectCC option:selected').attr("empresa");
            let strAgrupacion = selectCC.val() != "" ? selectCC.val() : 0;
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }
            if (attrEsContratista == "true") {
                esContratista = true;
            } else {
                if (chkEsContratista.prop('checked')) {
                    esContratista = true;
                } else {
                    esContratista = false;
                }
            }

            if (chkEsContratista.attr("esOculto") == 1 && selectCC.val() == "") {
                Alert2Warning("Es necesario seleccionar una empresa");
            } else {
                getInfoEmpleado(parseFloat($(".claveEmpleado").val()), esContratista, idAgrupacion).done(function (response) {
                    if (response.success) {
                        const fechaIngreso = moment(response.empleadoInfo.antiguedadEmpleado).format("DD/MM/YYYY");
                        $('#txtFechaIngreso').val(fechaIngreso);
                        // $('#txtFechaIngreso').val(response.empleadoInfo.antiguedadEmpleado)
                        $('#txtPuestoEmpleado').val(response.empleadoInfo.puestoEmpleado)
                        $('#nombreEmpleado').val(response.empleadoInfo.nombreEmpleado)
                    } else {
                        clearInfoEmpleado();
                        $("#nombreEmpleado").val("");
                        $("#txtPuestoEmpleado").val("");
                    }
                })
            }
        });

        $('.claveEmpleadoInformo').on('change', function () {
            let attrEsContratista = $("#btnGuardar").attr("data-esContratista");
            let idEmpresa = $('#selectCC option:selected').attr("empresa");
            let strAgrupacion = selectCC.val() != "" ? selectCC.val() : 0;
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }
            let esContratista = false;
            if (attrEsContratista == "true") {
                esContratista = true;
            } else {
                if (chkEsContratista.prop('checked')) {
                    esContratista = true;
                } else {
                    esContratista = false;
                }
            }
            if (chkEsContratista.attr("esOculto") == 1 && selectCC.val() == "") {
                Alert2Warning("Es necesario seleccionar una empresa");
            } else {
                getInfoEmpleado(parseFloat($('.claveEmpleadoInformo').val()), esContratista, idAgrupacion).done(function (response) {
                    if (response.success) {
                        $('#nombreEmpleadoInformo').val(response.empleadoInfo.nombreEmpleado)
                    } else {
                        $('#nombreEmpleadoInformo').val('')
                    }
                })
            }
        });

        $('#selectClasificacionIncidente').change(e => {
            const select = $(e.currentTarget);
            const tipoAccidenteID = select.val();

            switch (+tipoAccidenteID) {
                case 1:
                case 2:
                case 3:
                case 4:
                case 8:
                    $('#divParteCuerpo').show(500);
                    $('#divInfoExtraAccidente').show(500);
                    $('#divSubclasificacion').hide(500);
                    break;
                case 5:
                    $('#divSubclasificacion').show(500);
                    $('#divParteCuerpo').hide(500);
                    $('#divInfoExtraAccidente').show(500);
                    break;
                default:
                    $('#divSubclasificacion').hide(500);
                    $('#divInfoExtraAccidente').hide(500);
                    $('#divParteCuerpo').hide(500);
                    break;
            }
        });

        botonAgregarEvidencia.click(() => {
            const filas = dtTablaEvidencias.rows().count();

            if (filas >= 5) {
                return;
            }

            if (_flagInformePreliminar) {
                dtTablaEvidencias.row.add({
                    nombre: "",
                    fecha: "",
                    id: 0,
                    tieneEvidencia: false,
                    puedeEliminar: true
                }).draw();
            } else {
                dtTablaEvidencias.row.add({
                    nombre: "",
                    fecha: "",
                    id: 0,
                    tieneEvidencia: false,
                    tipoEvidenciaRIA: '',
                    puedeEliminar: true
                }).draw();
            }
        });

        $('#horasTrabajadas').on('change', function () {
            let valor = +$('#horasTrabajadas').val();

            if (valor % 1 != 0) {
                AlertaGeneral(`Alerta`, `Sólo se permiten números enteros en los campos de horas trabajadas y días trabajados.`);
                $('#horasTrabajadas').val('');
            }
        });

        $('#diasTrabajados').on('change', function () {
            let valor = +$('#diasTrabajados').val();

            if (valor % 1 != 0) {
                AlertaGeneral(`Alerta`, `Sólo se permiten números enteros en los campos de horas trabajadas y días trabajados.`);
                $('#diasTrabajados').val('');
            }
        });

        function deshabilitarCampos() {
            $('input').attr('disabled', true);
            $('select').attr('disabled', true);
            $('textarea').attr('disabled', true);
            $('button').attr('disabled', true);
            $('#btnGuardar').hide();
        }

        function clearInfoEmpleado() {
            $('#txtPuestoEmpleado').val('');
            $('#txtFechaIngreso').val('');
            $('#selectDepartamentoEmpleado').val('');
            // $('#claveSupervisor').val('');
            // $('#txtSupervisorEmpleado').val('');
            $('#nombreEmpleado').val('');
        }

        function llenarCamposInformePreliminar(data) {
            const fechaInforme = new Date(moment(data.fechaInforme, "DD-MM-YYYY").format());
            const fechaIngresoEmpleado = new Date(moment(data.fechaIngresoEmpleado, "DD-MM-YYYY").format());
            const fechaIncidente = new Date(moment(data.fechaIncidenteComplete))

            $("#btnGuardar").attr('valor', variables.informe);
            $("#selectCCRegistro").val(data.idAgrupacion).change();
            $("#selectCCRegistro").attr('disabled', true);

            $('#claveEmpleado').val(data.claveEmpleado);
            getInfoEmpleado(data.claveEmpleado, false, 0).done(function (response) { //TODO
                if (response.success) {
                    $('#nombreEmpleado').val(response.empleadoInfo.nombreEmpleado)
                }
            });

            $("#claveEmpleadoInformo").val(data.personaInformo).blur();

            $("#claveEmpleado").attr('disabled', true)
            $("#claveEmpleadoInformo").attr('disabled', true)

            $("#nombreEmpleado").attr('disabled', true)
            $("#nombreEmpleadoInformo").attr('disabled', true)

            $("#txtFechaInforme").datepicker("setDate", fechaInforme);
            $('#fechaAccidente').val(moment(data.fechaIncidenteComplete).format("DD/MM/YYYY HH:mm")) //$('#txtFechaIncidente').data("DateTimePicker").date(fechaIncidente);

            $("#txtFechaIngreso").attr('disabled', true);
            $("#txtFechaIngreso").datepicker("setDate", fechaIngresoEmpleado);

            $('#txtPuestoEmpleado').val(data.puestoEmpleado)
            $("#txtPuestoEmpleado").attr('disabled', true)

            if (data.departamento_id > 0) {
                $('#selectDepartamentoEmpleado').val(data.departamento_id).change();
            }

            $('#claveSupervisor').attr('disabled', true).val(data.claveSupervisor);
            $("#txtSupervisorEmpleado").attr('disabled', true).val(data.supervisorEmpleado);

            if (data.esExterno) {
                $('#selectContratista').val(data.claveContratista).change();
                $('#divContratista').show();
            } else {
                $('#selectContratista').val('').change();
            }

            $("#selectContratista").attr('disabled', true);

            $('#txtTipoLesion').val(data.tipoLesion)

            $('#txtDescripcionIncidente').val(data.descripcionIncidente)
            $('#txtAccionInmediata').val(data.accionInmediata);

            $("#selectClasificacionIncidente").val(data.tipoAccidente_id).change();
            switch (+data.tipoAccidente_id) {
                case 5:

                    $('#divSubclasificacion').show();
                    if (data.subclasificacionID > 0) {
                        $("#selectSubclasificacionIncidente").val(data.subclasificacionID).change()
                    }

                    $('#divInfoExtraAccidente').show();
                    if (data.tipoContacto_id > 0) {
                        $("#selectTipoContacto").val(data.tipoContacto_id).change()
                    }
                    if (data.agenteImplicado_id > 0) {
                        $("#selectAgenteImplicado").val(data.agenteImplicado_id).change()
                    }

                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 8:
                    $('#divInfoExtraAccidente').show();
                    $('#divParteCuerpo').show();
                    if (data.tipoContacto_id > 0) {
                        $("#selectTipoContacto").val(data.tipoContacto_id).change()
                    }
                    if (data.parteCuerpo_id > 0) {
                        $("#selectParteCuerpo").val(data.parteCuerpo_id).change()
                    }
                    if (data.agenteImplicado_id > 0) {
                        $("#selectAgenteImplicado").val(data.agenteImplicado_id).change()
                    }
                    break;
                default:
                    $('#divSubclasificacion').hide();
                    $('#divInfoExtraAccidente').hide();
                    $('#divParteCuerpo').hide();
                    break;
            }

            $("#selectProcedimientosViolados").val(data.procedimientosViolados).trigger('change.select2');

            // $('#selectRiesgo').val(data.riesgo).change()

            //$("input[name=aplicaRIA][value='" + data.aplicaRIA + "']").prop('checked', true);

            //if (data.terminado) {
            //    $("input[name=aplicaRIA]").prop('disabled', true);
            //    $('#btnGuardarRegistro').prop('disabled', true);
            //}

            $('#selectExperienciaEmpleado').val(data.experienciaEmpleado_id).trigger('change');
            $('#selectAntiguedadEmpleado').val(data.antiguedadEmpleado_id).trigger('change');
            $('#selectTurnoEmpleado').val(data.turnoEmpleado_id).trigger('change');
            $('#horasTrabajadas').val(data.horasTrabajadasEmpleado);
            $('#diasTrabajados').val(data.diasTrabajadosEmpleado);
            $('#capacitadoP').find(`input[value="${data.capacitadoEmpleado ? 1 : 0}"]`).prop('checked', true);
            $('#accidentesA').find(`input[value="${data.accidentesAnterioresEmpleado ? 1 : 0}"]`).prop('checked', true);
            $('#supervisorCargo').val(data.supervisorEmpleado);

            $('#selectTipoAccidente').val(data.tipoAccidente_id).trigger('change');
            $('#clienteIE').find(`input[value="${data.esExterno ? 1 : 0}"]`).prop('checked', true);
            $('#selectCC').val(data.idAgrupacion).change();
            $('#selectContratista').val(data.claveContratista > 0 ? data.claveContratista : '').change();
            $('#selectSubclasificacionIncidente').val(data.subclasificacionID > 0 ? data.subclasificacionID : '').change();
            $('#selectDepartamento').val(data.departamento_id).change();
            $('#lugarAccidente').val(data.lugarAccidente);
            $('#selectTipoLesion').val(data.tipoLesion_id > 0 ? data.tipoLesion_id : '').change();
            $('#selectParteCuerpo').val(data.parteCuerpo_id > 0 ? data.parteCuerpo_id : '').change();
            $('#actividadR').find(`input[value="${data.actividadRutinaria ? 1 : 0}"]`).prop('checked', true);
            $('#trabajoP').find(`input[value="${data.trabajoPlaneado ? 1 : 0}"]`).prop('checked', true);
            $('#trabajoRealizaba').val(data.trabajoRealizaba);
            $('#selectProtocoloTrabajo').val(data.protocoloTrabajo_id > 0 ? data.protocoloTrabajo_id : '').change();
            $('#descripcionAccidente').val(data.descripcionAccidente);

            establecerRiesgo(data.riesgo);
        }

        function llenarCamposIncidente(incidente) {

            // Datos generales
            $('#selectTipoAccidente').val(incidente.tipoAccidente_id).change();
            if ((incidente.tipoAccidente_id == 5 || incidente.tipoAccidente_id == 13 && _idEmpresa.val() == 6) && incidente.subclasificacionID != 0) {
                $('#divSubclasificacion').show();
                $("#selectSubclasificacionIncidente").val(incidente.subclasificacionID).change()
            }
            incidente.esExterno ? $('input[name=empresa]').first()[0].checked = true : $('input[name=empresa]').last()[0].checked = true;
            $('#selectCC').val(incidente.idAgrupacion).change();
            $('#selectContratista').val(incidente.claveContratista).change();
            $('#selectDepartamento').val(incidente.departamento_id).change();
            $('#lugarAccidente').val(incidente.lugarAccidente);
            $('#fechaAccidente').val(incidente.fechaAccidente);
            $('#selectTipoLesion').val(incidente.tipoLesion_id).change();
            $('#selectParteCuerpo').val(incidente.parteCuerpo_id).change();
            incidente.actividadRutinaria ? $('input[name=actividadRutinaria]').first()[0].checked = true : $('input[name=actividadRutinaria]').last()[0].checked = true;
            $('#selectAgenteImplicado').val(incidente.agenteImplicado_id).change();
            incidente.trabajoPlaneado ? $('input[name=trabajoPlaneado]').first()[0].checked = true : $('input[name=trabajoPlaneado]').last()[0].checked = true;
            $('#trabajoRealizaba').val(incidente.trabajoRealizaba);
            $('#selectTipoContacto').val(incidente.tipoContacto_id).change();
            $('#selectProtocoloTrabajo').val(incidente.protocoloTrabajo_id).change();

            // Persona implicada accidente
            if (incidente.esExterno) {
                $('#selectEmpleado').empty().off();
                $('#selectEmpleado').append(`<option selected value="${incidente.nombreEmpleadoExterno}">${incidente.nombreEmpleadoExterno}</option>`);
                $('#selectEmpleado').attr('disabled', true);
            } else {
                $('#selectEmpleado').empty();
                $('#selectEmpleado').append(`<option selected value="${incidente.claveEmpleado}"></option>`);
                $('#selectEmpleado').change();
            }

            $('#edadPersonal').val(incidente.edadEmpleado);
            $('#puestoPersonal').val(incidente.puestoEmpleado);
            $('#selectExperienciaEmpleado').val(incidente.experienciaEmpleado_id).change();
            $('#selectAntiguedadEmpleado').val(incidente.antiguedadEmpleado_id).change();
            $('#selectTurnoEmpleado').val(incidente.turnoEmpleado_id).change();
            $('#horasTrabajadas').val(incidente.horasTrabajadasEmpleado);
            $('#diasTrabajados').val(incidente.diasTrabajadosEmpleado);
            incidente.capacitadoEmpleado ? $('input[name=capacitado]').first()[0].checked = true : $('input[name=capacitado]').last()[0].checked = true;
            incidente.accidentesAnterioresEmpleado ? $('input[name=accidentesAnteriores]').first()[0].checked = true : $('input[name=accidentesAnteriores]').last()[0].checked = true;
            $('#claveSupervisor').val(incidente.claveSupervisor);
            $('#supervisorCargo').val(incidente.supervisorCargoEmpleado);

            // Descripción del accidente
            $('#descripcionAccidente').val(incidente.descripcionAccidente);

            // Evaluación de la pérdida si no es corregida
            establecerRiesgo(incidente.riesgo);

            // Grupo de trabajo para la investigación
            const tbodyGrupoInvestigacion = $('#tblGrupoInvestigacion tbody');
            tbodyGrupoInvestigacion.empty();
            incidente.grupoInvestigacion.forEach(x =>
                tbodyGrupoInvestigacion.append(`
                <tr class="text-center">
                    <td>${x.nombreEmpleado}</td>
                    <td>${x.puestoEmpleado}</td>
                    <td>${x.departamentoEmpleado}</td>
                    <td><input disabled type="checkbox" class="form-control esExterno" ${x.esExterno ? "checked" : ""}></td>
                </tr>
                `));

            // Orden cronológico del accidente
            $('#instruccionTrabajo').val(incidente.instruccionTrabajo);
            $('#explicacionManera').val(incidente.porqueSehizo);
            const tbodyOrdenCronologico = $('#tblOrdenCronologico tbody');
            tbodyOrdenCronologico.empty();
            incidente.ordenCronologico.forEach(x =>
                tbodyOrdenCronologico.append(`
                <tr class="text-center">
                    <td>${x}</td>
                </tr>
                `));

            // Técnica de Investigación
            $('#selectTecnicasInvestigacion').val(incidente.tecnicaInvestigacion_id).change();

            // Análisis de causas
            const tbodyEventoDetonador = $('#tblEventoDetonador tbody');
            tbodyEventoDetonador.empty();
            incidente.eventoDetonador.forEach(x =>
                tbodyEventoDetonador.append(`
                <tr class="text-center">
                    <td>${x}</td>
                </tr>
                `));

            const tbodyCausaInmediata = $('#tblCausaInmediata tbody');
            tbodyCausaInmediata.empty();
            incidente.causaInmediata.forEach(x =>
                tbodyCausaInmediata.append(`
                <tr class="text-center">
                    <td>${x}</td>
                </tr>
                `));

            const tbodyCausaBasica = $('#tblCausaBasica tbody');
            tbodyCausaBasica.empty();
            incidente.causaBasica.forEach(x =>
                tbodyCausaBasica.append(`
                <tr class="text-center">
                    <td>${x}</td>
                </tr>
                `));

            const tbodyCausaRaiz = $('#tblCausaRaiz tbody');
            tbodyCausaRaiz.empty();
            incidente.causaRaiz.forEach(x =>
                tbodyCausaRaiz.append(`
                <tr class="text-center">
                    <td>${x}</td>
                </tr>
                `));

            // Medidas de control
            $('#inputLugarJunta').val(incidente.lugarJunta);
            $('#inputFechaJunta').val(incidente.fechaJunta);
            $('#inputHoraInicio').val(incidente.horaInicio);
            $('#inputHoraFin').val(incidente.horaFin);

            const tbodyMedidasControl = $('#tblMedidasControl tbody');
            tbodyMedidasControl.empty();
            incidente.medidasControl.forEach(x =>
                tbodyMedidasControl.append(`
                <tr class="text-center">
                    <td>${x.accionPreventiva}</td>
                    <td>${x.fechaCump}</td>
                    <td>${x.responsableNombre}</td>
                    <td>${x.prioridad}</td>
                </tr>
                `));
        }

        function establecerRiesgo(riesgo) {
            switch (riesgo) {
                case 1:
                    $('input[name=riesgoPxC][value=1]')[0].checked = true;
                    break;
                case 2:
                    $('input[name=riesgoPxC][value=2]')[0].checked = true;
                    break;
                case 3:
                case 4:
                    $('input[name=riesgoPxC][value=3]')[0].checked = true;
                    break;
                case 6:
                    $('input[name=riesgoPxC][value=6]')[0].checked = true;
                    break;
                case 9:
                    $('input[name=riesgoPxC][value=9]')[0].checked = true;
                    break;
                default:
                    return;
            }
        }

        function fncValidarAccesoContratista() {
            axios.post("ValidarAccesoContratista").then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $("#divEsContratista").css("display", "none");
                    lblContratistas.css("display", "none");
                    chkEsContratista.attr("esOculto", "1");
                    $("#btnGuardar").attr("data-esContratista", true);
                    cboSelectContratista.css("display", "none");
                } else {
                    $("#btnGuardar").attr("data-esContratista", false);
                    chkEsContratista.attr("esOculto", "0");
                }
            }).catch(error => Alert2Error(error.message));
        }

        //#region CARGAR INICIAL
        function cargarSelectsForm() {
            $('#btnAddEmpleado').hide()
            $('#fechaNacimientoEmpleadoModal').datepicker({ dateFormat: 'dd/mm/yy' })
            //$('#selectCC').fillCombo('LlenarComboCC', null, false);
            $('#selectCC').fillComboSeguridad(false);
            $('#selectContratista').fillCombo('GetSubcontratistas', null, false);
            $("#selectTipoAccidente").fillCombo('GetTiposAccidentesList', null, false);
            $('#selectSubclasificacionIncidente').fillCombo('GetSubclasificacionesAccidente', null, false);
            $("#selectDepartamento").fillCombo('GetDepartamentosList', null, false);
            $("#selectTipoLesion").fillCombo('GetTiposLesionList', null, false);
            $("#selectParteCuerpo").fillCombo('GetPartesCuerposList', null, false);
            $("#selectTipoContacto").fillCombo('GetTiposContactoList', null, false);
            $("#selectProtocoloTrabajo").fillCombo('GetProtocolosTrabajoList', null, false);
            $("#selectTecnicasInvestigacion").fillCombo('GetTecnicasInvestigacionList', null, false);
            $('#selectAgenteImplicado').fillCombo('GetAgentesImplicadosList', null, false);
            $('#selectExperienciaEmpleado').fillCombo('GetExperienciaEmpleadosList', null, false);
            $('#selectAntiguedadEmpleado').fillCombo('GetAntiguedadEmpleadosList', null, false);
            $('#selectTurnoEmpleado').fillCombo('GetTurnosEmpleadoList', null, false);
            $(".selectPrioridadMedidas").fillCombo('GetPrioridadesActividad', null, false);

            $('#inputFechaJunta').datepicker({
                "dateFormat": "dd/mm/yy",
                "minDate": new Date()
            }).datepicker("option", "showAnim", "slide")
            // .datepicker("setDate", +1);
        }

        function cargaEmpleado() {

            const noEsExterno = $('input[name=empresa]').last()[0].checked;

            if (noEsExterno) {
                $('#selectEmpleado').empty();
                const url = 'GetEmpleadosCCList';
                const cc = $('#selectCC').val()

                $('#selectEmpleado').fillCombo(url, { cc }, false);
                clearInfoEmpleado()
            }

        }

        function calcularEvaluacionRiesgo() {
            let riesgo = evaluacionProbabilidad * evaluacionConsecuencias

            if (riesgo > 0) {
                riesgo = riesgo == 4 ? 3 : riesgo
                $("input[name=riesgoPxC][value='" + riesgo + "']").prop('checked', true);
            }
        }

        //#endregion

        //#region METODOS GENERALES
        function setSelectContratistas() {

            if (esExterno == "0") {
                $('#selectContratista').val("")
                $('#selectContratista').change()
            }

            $('#selectContratista').prop('disabled', esExterno == "0")
        }

        function alternarGrupoInvestigacionExterno() {
            if (this.checked) {
                $($(this).parent().parent().find('td')[0]).html(`<input type="text" class="form-control nombreInvestigacionExterno" placeholder="Nombre externo">`);
                $($(this).parent().parent().find('td')[1]).html(`<input type="text" class="form-control puestoInvestigacion">`);
                $($(this).parent().parent().find('td')[2]).html(`<input type="text" class="form-control departamentoInvestigacion">`);
            } else {
                $($(this).parent().parent().find('td')[0]).html(`<input type="text" class="form-control nombreInvestigacion nombreInvestigacionNew" placeholder="*">`);
                $($(this).parent().parent().find('td')[1]).html(`<label class="puestoInvestigacion"></label>`);
                $($(this).parent().parent().find('td')[2]).html(`<label class="departamentoInvestigacion"></label>`);
                initNombreGrupoInvestigacion();
            }
        }

        function addRowGrupoInvestigacion() {
            const tr = `
                <tr>
                    <td><input class="form-control nombreInvestigacion nombreInvestigacionNew" placeholder="*"></td>
                    <td><label class="puestoInvestigacion"></label></td>
                    <td><label class="departamentoInvestigacion"></label></td>
                    <td><input type="checkbox" class="form-control esExterno"></td>
                </tr>
            `;

            return tr;
        }

        function addRowOrdenCronologico() {
            const tr = `
                <tr>
                    <td><input type="text" class="form-control"></td>                  
                </tr>
            `;

            return tr;
        }

        function addRowMedidasControl() {
            const tr = `
                <tr>
                     <td><textarea rows="3" class="form-control"></textarea></td>
                    <td><input type="text" class="form-control fechaCump"></td>
                    <td><input class="form-control responsableMedidasControl responsableMedidasControlNew" placeholder="*"></td>
                    <td><select class="form-control select2 selectPrioridadMedidas selectPrioridadMedidasNew" style="width: 100%"></select></td>
                </tr>
            `;

            return tr;
        }

        function addRowEventoDetonador() {
            const tr = `
                <tr>
                    <td><input type="text" class="form-control"></td>                  
                </tr>
            `;

            return tr;
        }

        function addRowCausaInmediata() {
            const tr = `
                <tr>
                    <td><input type="text" class="form-control"></td>                  
                </tr>
            `;

            return tr;
        }

        function addRowCausaBasica() {
            const tr = `
                <tr>
                    <td><input type="text" class="form-control"></td>                  
                </tr>
            `;

            return tr;
        }

        function addRowCausaRaiz() {
            const tr = `
                <tr>
                    <td><input type="text" class="form-control"></td>                  
                </tr>
            `;

            return tr;
        }

        function deleteRowsTables(table, limite) {
            if ((table.length + 1) > limite) {
                table.last().remove()
            }
        }

        function validaGuardarEmpleado() {
            let countInvalidos = 0;

            // if ($('#selectContratista').val() == "") {
            //     countInvalidos++;
            // }

            $('input[type=text].nuevoEmpleado').each(function () {
                if ($(this).val().trim() == "") {
                    countValidos++;
                }
            })

            return countInvalidos > 0 ? false : true;
        }

        function guardarEmpleado() {
            if (validaGuardarEmpleado()) {

                const empleado = {
                    claveContratista: $('#selectContratista').val(),
                    nombre: $('#nombreEmpleadoModal').val(),
                    fechaNacimiento: $('#fechaNacimientoEmpleadoModal').val(),
                    puesto: $('#puestoEmpleadoModal').val()
                }

                $.blockUI({ message: "Preparando información" });
                $.post('GuardarEmpleadoSubcontratista', { empleado: empleado })
                    .done(response => {
                        if (response.success) {
                            $('#modalEmpleado').modal('hide')
                            cargaEmpleado()
                            $.unblockUI();
                        } else {
                            AlertaGeneral('Aviso', response.error)
                            $.unblockUI();
                        }
                    });

            } else {
                AlertaGeneral('Aviso', 'Es necesario ingresar todos los campos y seleccionar un contratista')
            }
        }

        function clearModal() {
            $('#nombreEmpleadoModal').val('')
            $('#fechaNacimientoEmpleadoModal').val('')
            $('#puestoEmpleadoModal').val('')
        }

        function cargarInfoEmpleado() {
            const claveEmpleado = $('#selectEmpleado').val()

            if (esExterno == "0") {
                getInfoEmpleadoCP(claveEmpleado)
            } else {
                getInfoEmpleadoContratista(claveEmpleado)
            }
        }

        function getInfoEmpleadoCP(claveEmpleado) {
            getInfoEmpleado(claveEmpleado, false, 0).done(function (response) { //TODO
                if (response.success) {

                    const today = new Date();
                    const edad = (today.getFullYear() - moment(response.empleadoInfo.edadEmpleado).toDate().getFullYear());

                    $('#selectEmpleado option').text(response.empleadoInfo.nombreEmpleado);

                    $('#edadPersonal').val(edad).attr('disabled', true)
                    $('#puestoPersonal').val(response.empleadoInfo.puestoEmpleado).attr('disabled', true);

                    if (informe_id == 0) {
                        $('#selectExperienciaEmpleado').val('').change();
                        $('#selectAntiguedadEmpleado').val('').change();
                        $('#selectTurnoEmpleado').val('').change();
                    }
                }
                else {
                    clearInfoEmpleado()
                }
            })
        }

        function getInfoEmpleadoInvestigacion(claveEmpleado, select) {
            getInfoEmpleado(claveEmpleado, false, 0).done(function (response) { //TODO
                if (response.success) {
                    select.parent().parent().find('.puestoInvestigacion').text('')
                    select.parent().parent().find('.departamentoInvestigacion').text('')

                    select.parent().parent().find('.puestoInvestigacion').append(response.empleadoInfo.puestoEmpleado)
                    select.parent().parent().find('.departamentoInvestigacion').append(response.empleadoInfo.deptoEmpleado)
                    select.parent().parent().find('.esExterno')[0].checked = false;
                }
                else {
                    select.parent().parent().find('.puestoInvestigacion').text('');
                    select.parent().parent().find('.departamentoInvestigacion').text('');
                    select.val('');
                }
            })
        }

        function getInfoEmpleadoContratista(empleado_id) {
            getInfoEmpleadoConstratista(empleado_id).done(function (response) {
                if (response.success) {
                    const today = new Date()
                    const edad = (today.getFullYear() - moment(response.empleadoInfo.edadEmpleado).toDate().getFullYear())

                    $('#edadPersonal').val(edad)
                    $('#puestoPersonal').val(response.empleadoInfo.puestoEmpleado)
                    $('#antiguedadPersonal').val('')
                    $('#turnoPersonal').val('')
                    $('#claveSupervisor').val('')
                    $('#supervisorCargo').val('')
                }
                else {
                    clearInfoEmpleado()
                }
            })
        }

        function clearInfoEmpleado() {
            $('#edadPersonal').val('')
            $('#puestoPersonal').val('')
            $('#claveSupervisor').val('')
            $('#supervisorCargo').val('')
            $('#selectExperienciaEmpleado').val('').change();
            $('#selectAntiguedadEmpleado').val('').change();
            $('#selectTurnoEmpleado').val('').change();
        }

        function initChangeGrupoInvestigacion(claveEmpleado, input) {
            getInfoEmpleadoInvestigacion(claveEmpleado, input)
        }

        function initFechaMedidasControl() {
            $('.fechaCump').datetimepicker({
                format: 'DD/MM/YYYY'
            });
        }

        function initNombreGrupoInvestigacion() {
            $('.nombreInvestigacion').getAutocompleteValid(fnGrupoInvestigacionNombre, validateInput, null, 'GetUsuariosAutocomplete');
        }

        function initResponsableMedidasControl() {
            $('.responsableMedidasControl').getAutocompleteValid(fnMedidasControlResponsable, validateInput, null, 'GetUsuariosAutocomplete');
        }

        function fnGrupoInvestigacionNombre(event, ui) {
            if (!isNaN(ui.item.id) && +ui.item.id > 0) {
                $(this).text(ui.item.value);
                $(this).attr('data-claveEmpleado', ui.item.id)
                $(this).attr('data-usuarioID', ui.item.usuarioID)
                initChangeGrupoInvestigacion(ui.item.id, $(this))
            } else {
                AlertaGeneral(`Alerta`, `La información de "${ui.item.value}" no es correcta. Comuníquese con el departamento de sistemas para resolver el problema.`);
                $(this).val('');
                $(this).text('');
                $(this).attr('data-claveEmpleado', 0);
                $(this).attr('data-usuarioID', 0);
            }
        }

        function validateInput(event, ui) {
            if (ui.item == null) {
                $(this).val('');
            }
        }

        function fnMedidasControlResponsable(event, ui) {
            if (!isNaN(ui.item.id) && +ui.item.id > 0) {
                $(this).text(ui.item.value);
                $(this).attr('data-responsable', ui.item.id);
                $(this).attr('data-usuarioID', ui.item.usuarioID);
            } else {
                AlertaGeneral(`Alerta`, `La información de "${ui.item.value}" no es correcta. Comuníquese con el departamento de sistemas.`);
                $(this).val('');
                $(this).text('');
                $(this).attr('data-responsable', 0);
                $(this).attr('data-usuarioID', 0);
            }
        }
        //#endregion

        //#region METODOS GUARDAR
        function validarGuardarIncidente() {
            let countInvalidos = 0

            if ($('input[name=actividadRutinaria]:checked').val() == undefined) {
                countInvalidos++;
            }

            if ($('input[name=trabajoPlaneado]:checked').val() == undefined) {
                countInvalidos++;
            }

            if ($('input[name=capacitado]:checked').val() == undefined) {
                countInvalidos++;
            }

            if ($('input[name=accidentesAnteriores]:checked').val() == undefined) {
                countInvalidos++;
            }

            if ($('input[name=riesgoPxC]:checked').val() == undefined) {
                countInvalidos++;
            }

            $('select.validar').each(function () {
                if (this.id == 'selectCC') {
                    if ($('input[name=empresa]:checked').val() != '1') {
                        if ($(this).val() == "") {
                            countInvalidos++;
                        }
                    }
                } else {
                    if ($(this).val().trim() == "") {
                        countInvalidos++;
                    }
                }
            })

            if (($('#selectTipoAccidente').val() == 5 || $('#selectTipoAccidente').val() == 13 && _idEmpresa.val() == 6) && ($("#selectSubclasificacionIncidente").val() == "")) {
                countInvalidos++;
            }

            if ($('input[name=empresa]:checked').val() == undefined) {
                countInvalidos++;
            }

            $('#tblGrupoInvestigacion tbody tr').find('input[type="text"]').each(function () {
                if ($(this).val() == "") {
                    countInvalidos++;
                }
            })

            $('#tblOrdenCronologico tbody tr').find('input').each(function () {
                if ($(this).val().trim() == "") {
                    countInvalidos++;
                }
            })

            $('#tblEventoDetonador tbody tr').find('input').each(function () {
                if ($(this).val().trim() == "") {
                    countInvalidos++;
                }
            })

            $('#tblCausaInmediata tbody tr').find('input').each(function () {
                if ($(this).val().trim() == "") {
                    countInvalidos++;
                }
            })

            $('#tblCausaBasica tbody tr').find('input').each(function () {
                if ($(this).val().trim() == "") {
                    countInvalidos++;
                }
            })

            $('#tblCausaRaiz tbody tr').find('input').each(function () {
                if ($(this).val().trim() == "") {
                    countInvalidos++;
                }
            })

            if ($('#inputLugarJunta').val() == "" ||
                $('#inputFechaJunta').val() == "" ||
                $('#inputHoraInicio').val() == "" ||
                $('#inputHoraFin').val() == "") {
                countInvalidos++;
            }

            return countInvalidos;
        }

        function getObjAccidente() {
            const esExterno = $('input[name=empresa]:checked').val() == "1";

            const incidente = {
                informe_id: informe_id,
                tipoAccidente_id: $('#selectTipoAccidente').val(),
                subclasificacionID: ($('#selectTipoAccidente').val() == 5 || $('#selectTipoAccidente').val() == 13 && _idEmpresa.val() == 6) ? $('#selectSubclasificacionIncidente').val() : 0,
                departamento_id: $('#selectDepartamento').val(),
                tipoLesion_id: $('#selectTipoLesion').val(),
                parteCuerpo_id: $('#selectParteCuerpo').val(),
                agenteImplicado_id: $('#selectAgenteImplicado').val(),
                experienciaEmpleado_id: $('#selectExperienciaEmpleado').val(),
                antiguedadEmpleado_id: $('#selectAntiguedadEmpleado').val(),
                turnoEmpleado_id: $('#selectTurnoEmpleado').val(),
                tipoContacto_id: $('#selectTipoContacto').val(),
                protocoloTrabajo_id: $('#selectProtocoloTrabajo').val(),
                tecnicaInvestigacion_id: $('#selectTecnicasInvestigacion').val(),
                claveContratista: +($('#selectContratista').val()),
                claveEmpleado: esExterno ? 0 : $('#selectEmpleado').val(),
                edadEmpleado: $('#edadPersonal').val(),
                horasTrabajadasEmpleado: $('#horasTrabajadas').val(),
                diasTrabajadosEmpleado: $('#diasTrabajados').val(),
                riesgo: $('input[name=riesgoPxC]:checked').val(),
                esExterno,
                trabajoPlaneado: $('input[name=trabajoPlaneado]:checked').val() == "1" ? true : false,
                actividadRutinaria: $('input[name=actividadRutinaria]:checked').val() == "1" ? true : false,
                capacitadoEmpleado: $('input[name=capacitado]:checked').val() == "1" ? true : false,
                accidentesAnterioresEmpleado: $('input[name=accidentesAnteriores]:checked').val() == "1" ? true : false,
                cc: '',
                idEmpresa: $("option:selected", $('#selectCC')).attr("empresa"),
                idAgrupacion: $('#selectCC').val(),
                lugarAccidente: $('#lugarAccidente').val(),
                fechaAccidente: '01/01/1991', //Valor por default. Se le asigna el correcto desde el back-end. // fechaAccidente: $('#fechaAccidente').val(),
                trabajoRealizaba: $('#trabajoRealizaba').val(),
                puestoEmpleado: $('#puestoPersonal').val(),
                claveSupervisor: $('#claveSupervisor').val(),
                supervisorCargoEmpleado: $('#supervisorCargo').val(),
                descripcionAccidente: $('#descripcionAccidente').val(),
                instruccionTrabajo: $('#instruccionTrabajo').val(),
                porqueSehizo: $('#explicacionManera').val(),
                nombreEmpleadoExterno: esExterno ? $('#selectEmpleado').val() : null,
                instruccionTrabajo: $('#instruccionTrabajo').val(),
                porqueSehizo: $('#explicacionManera').val(),
                lugarJunta: $('#inputLugarJunta').val(),
                fechaJunta: '01/01/1991', //Valor por default. Se le asigna el correcto desde el back-end. //fechaJunta: $('#inputFechaJunta').val(),
                horaInicio: $('#inputHoraInicio').val(),
                horaFin: $('#inputHoraFin').val(),
            };

            return incidente;
        }

        function getObjGrupoTrabajo() {
            let grupo = [];

            $('#tblGrupoInvestigacion tbody tr').each(function () {
                if ($(this).find('.esExterno')[0].checked) {
                    grupo.push({
                        claveEmpleado: 0,
                        nombreEmpleado: $(this).find('.nombreInvestigacionExterno').val(),
                        puestoEmpleado: $(this).find('.puestoInvestigacion').val(),
                        departamentoEmpleado: $(this).find('.departamentoInvestigacion').val(),
                        usuarioID: 0,
                        esExterno: true
                    });
                } else {
                    grupo.push({
                        claveEmpleado: $(this).find('.nombreInvestigacion').attr('data-claveEmpleado'),
                        nombreEmpleado: $(this).find('.nombreInvestigacion').text(),
                        puestoEmpleado: $(this).find('.puestoInvestigacion').text(),
                        departamentoEmpleado: $(this).find('.departamentoInvestigacion').text(),
                        usuarioID: $(this).find('.nombreInvestigacion').attr('data-usuarioID'),
                        esExterno: false
                    });
                }
            })

            return grupo
        }

        function getObjOrdenCronologico() {
            let orden = [];

            $('#tblOrdenCronologico tbody tr').each(function () {
                orden.push({
                    ordenCronologico: $(this).find('input').val()
                })
            });

            return orden
        }

        function getObjEventosDetonador() {
            let eventoDetonador = [];

            $('#tblEventoDetonador tbody tr').each(function () {
                eventoDetonador.push({
                    eventoDetonador: $(this).find('input').val()
                })
            })

            return eventoDetonador
        }

        function getObjCausasInmediatas() {
            let causasInmediata = [];

            $('#tblCausaInmediata tbody tr').each(function () {
                causasInmediata.push({
                    causaInmediata: $(this).find('input').val()
                })
            })

            return causasInmediata
        }

        function getObjCausasBasicas() {
            let causasBasicas = [];

            $('#tblCausaBasica tbody tr').each(function () {
                causasBasicas.push({
                    causaBasica: $(this).find('input').val()
                })
            })

            return causasBasicas
        }

        function getObjCausasRaiz() {
            let causasRaiz = [];

            $('#tblCausaRaiz tbody tr').each(function () {
                causasRaiz.push({
                    causaRaiz: $(this).find('input').val()
                })
            })

            return causasRaiz
        }

        function getObjMedidasControl() {
            let medidasControl = [];

            $('#tblMedidasControl tbody tr').each(function () {
                medidasControl.push({
                    accionPreventiva: $(this).find('textarea').val(),
                    fechaCump: $(this).find('.fechaCump').val(),
                    responsable_id: $(this).find('.responsableMedidasControl').attr('data-responsable'),
                    responsableNombre: $(this).find('.responsableMedidasControl').text(),
                    estatus: 0,
                    prioridad: $(this).find('.selectPrioridadMedidas').val(),
                    usuarioID: $(this).find('.responsableMedidasControl').attr('data-usuarioID')
                })
            })

            return medidasControl.filter(x => x.responsable_id != null);
        }

        function guardarIncidente() {
            if (validarGuardarIncidente() > 0) {
                AlertaGeneral('Aviso', 'Todos los campos necesitan ser llenados')
            } else {
                // guardarAccidente(getObjAccidente(), getObjGrupoTrabajo(), getObjOrdenCronologico(), getObjEventosDetonador(), getObjCausasInmediatas(), getObjCausasBasicas(), getObjCausasRaiz(), getObjMedidasControl()).done(function (response) {
                //     if (response.success) {
                //         $('#btnGuardar').hide();
                //         enviarCorreosMinuta(response.minutaID, response.usuarios);
                //     } else {
                //         AlertaGeneral('Aviso', response.error)
                //     }
                // });

                let incidente = getObjAccidente();

                //#region Validaciones
                if (incidente.horasTrabajadasEmpleado % 1 != 0) { //Se verifica que se capturen números enteros.
                    AlertaGeneral(`Alerta`, `Debe capturar números enteros para las horas trabajadas.`);
                    return;
                }

                if (incidente.diasTrabajadosEmpleado % 1 != 0) { //Se verifica que se capturen números enteros.
                    AlertaGeneral(`Alerta`, `Debe capturar números enteros para los días trabajados.`);
                    return;
                }
                //#endregion

                let grupoTrabajo = getObjGrupoTrabajo();
                let ordenCronologico = getObjOrdenCronologico();
                let eventosDetonador = getObjEventosDetonador();
                let causasInmediatas = getObjCausasInmediatas();
                let causasBasicas = getObjCausasBasicas();
                let causasRaiz = getObjCausasRaiz();
                let medidasControl = getObjMedidasControl();

                let data = new FormData();
                data.append('incidente', JSON.stringify(incidente));
                data.append('grupoTrabajo', JSON.stringify(grupoTrabajo));
                data.append('ordenCronologico', JSON.stringify(ordenCronologico));
                data.append('eventosDetonador', JSON.stringify(eventosDetonador));
                data.append('causasInmediatas', JSON.stringify(causasInmediatas));
                data.append('causasBasicas', JSON.stringify(causasBasicas));
                data.append('causasRaiz', JSON.stringify(causasRaiz));
                data.append('medidasControl', JSON.stringify(medidasControl));
                data.append('fechaAccidente', $('#fechaAccidente').val());
                data.append('fechaJunta', $('#inputFechaJunta').val());

                let numEvidencias = 0;
                let listaTipoEvidenciaRIA = [];

                tablaEvidencias.find('tbody tr').each((idx, row) => {
                    let tipoEvidenciaRIA = +($(row).find('.selectTipoEvidenciaRIA').val());
                    let input = $(row).find('input.inputEvidencia');
                    let file = input[0].files[0];

                    data.append('evidencias', file);
                    listaTipoEvidenciaRIA.push(tipoEvidenciaRIA);
                    numEvidencias++;
                });

                data.append('tipoEvidenciaRIA', JSON.stringify(listaTipoEvidenciaRIA));

                if (numEvidencias == 0) {
                    AlertaGeneral(`Aviso`, `No se han capturado evidencias.`);
                    return;
                } else if (tablaEvidencias.find('input.inputEvidencia').toArray().some(x => x.files.length == 0)) {
                    AlertaGeneral(`Aviso`, `Faltan evidencias por cargar.`);
                    return;
                }

                let captura = data;

                $.ajax({
                    url: '/Administrativo/IndicadoresSeguridad/GuardarIncidente',
                    data: captura,
                    async: false,
                    cache: false,
                    contentType: false,
                    processData: false,
                    method: 'POST'
                }).then(response => {
                    // if (response.success) {
                    //     AlertaGeneral('Éxito', 'Incidente registrado correctamente.')
                    //     setDatosInicio();
                    //     $('#btnGuardar').hide();
                    //     $.blockUI({ message: 'Generando reporte...' });
                    //     report.attr("src", `/Reportes/Vista.aspx?idReporte=194&informeID=${response.informeID}&inMemory=1`);
                    //     document.getElementById('report').onload = function () {
                    //         $.unblockUI();
                    //         openCRModal();
                    //     };
                    // } else {
                    //     AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    // }
                    if (response.success) {
                        $.blockUI({ message: 'Generando reporte...' });
                        report.attr("src", `/Reportes/Vista.aspx?idReporte=175&informeID=${response.informeID}&inMemory=1`);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                        $('#btnGuardar').hide();
                        AlertaGeneral('Éxito', 'Incidente registrado correctamente.');
                        enviarCorreosMinuta(response.minutaID, response.usuarios);
                    } else {
                        AlertaGeneral('Aviso', response.error)
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        }

        function guardarInformePreliminar() {
            if (esEdit) {
                let informe = getObjRegistroEdit();

                //#region Validaciones
                if (informe.horasTrabajadasEmpleado % 1 != 0) { //Se verifica que se capturen números enteros.
                    AlertaGeneral(`Alerta`, `Debe capturar números enteros para las horas trabajadas.`);
                    return;
                }

                if (informe.diasTrabajadosEmpleado % 1 != 0) { //Se verifica que se capturen números enteros.
                    AlertaGeneral(`Alerta`, `Debe capturar números enteros para los días trabajados.`);
                    return;
                }
                //#endregion

                if (informe.riesgo == 0) {
                    AlertaGeneral(`Alerta`, `Seleccione el riesgo.`);
                    return;
                }

                actualizarRegistro(informe).done(function (response) {
                    if (response.success) {
                        AlertaGeneral('Aviso', 'Incidente actualizado correctamente.')
                        setDatosInicio();
                        $('#btnGuardar').hide();
                    } else {
                        AlertaGeneral('Aviso', response.error)
                    }
                });
            } else {
                let informe = getObjRegistro();

                //#region Validaciones
                if (informe.horasTrabajadasEmpleado % 1 != 0) { //Se verifica que se capturen números enteros.
                    AlertaGeneral(`Alerta`, `Debe capturar números enteros para las horas trabajadas.`);
                    return;
                }

                if (informe.diasTrabajadosEmpleado % 1 != 0) { //Se verifica que se capturen números enteros.
                    AlertaGeneral(`Alerta`, `Debe capturar números enteros para los días trabajados.`);
                    return;
                }
                //#endregion

                if (informe.riesgo == 0) {
                    AlertaGeneral(`Alerta`, `Seleccione el riesgo.`);
                    return;
                }

                let data = new FormData();
                data.append('informe', JSON.stringify(informe));
                let numEvidencias = 0;

                tablaEvidencias.find('input.inputEvidencia').toArray().forEach(x => {
                    const file = x.files[0];
                    data.append('evidencias', file);
                    numEvidencias++;
                });

                if (numEvidencias == 0) {
                    AlertaGeneral(`Aviso`, `No se han capturado evidencias.`);
                    return;
                } else if (tablaEvidencias.find('input.inputEvidencia').toArray().some(x => x.files.length == 0)) {
                    AlertaGeneral(`Aviso`, `Faltan evidencias por cargar.`);
                    return;
                }

                let captura = data;

                $.ajax({
                    url: '/Administrativo/IndicadoresSeguridad/GuardarInforme',
                    data: captura,
                    async: false,
                    cache: false,
                    contentType: false,
                    processData: false,
                    method: 'POST'
                }).then(response => {
                    if (response.success) {
                        AlertaGeneral('Éxito', 'Incidente registrado correctamente.')
                        setDatosInicio();
                        $('#btnGuardar').hide();
                        $.blockUI({ message: 'Generando reporte...' });
                        report.attr("src", `/Reportes/Vista.aspx?idReporte=194&informeID=${response.informeID}&inMemory=1`);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        }

        function getObjRegistro() {
            let nombreExterno = null;
            let claveContratista = 0;
            let esExterno = $('#clienteIE input:checked').val() == 1;

            if (esExterno) {
                nombreExterno = $('#nombreEmpleado').val();

                if ($('#selectContratista').val() != "") {
                    claveContratista = $('#selectContratista').val();
                }
            }

            let riesgo = +($('#divRadioRiesgo').find('input:checked').val());

            if (isNaN(riesgo)) {
                riesgo = 0;
            }

            let idEmpresa = $("option:selected", $('#selectCC')).attr("empresa");
            let strAgrupacion = $('#selectCC').val();
            let idAgrupacion;
            if (idEmpresa == 1000) {
                idAgrupacion = strAgrupacion.replace("c_", "");
            } else if (idEmpresa == 2000) {
                idAgrupacion = strAgrupacion.replace("a_", "");
            } else {
                idAgrupacion = strAgrupacion;
            }

            let tipoAccidenteID = $('#selectTipoAccidente').val();
            let registroInformacion = {
                folio: 0,
                claveEmpleado: !esExterno ? $('#claveEmpleado').val() : 0,
                personaInformo: $("#claveEmpleadoInformo").val(),
                cc: '',
                idEmpresa: idEmpresa,
                idAgrupacion: idAgrupacion,
                fechaInforme: $('#txtFechaInforme').val() + ' 12:00 am',
                fechaIncidente: $('#fechaAccidente').val(), // fechaIncidente: $('#fechaAccidente').val().substring(0, 10),
                fechaIngresoEmpleado: $('#txtFechaIngreso').val() + ' 12:00 am',
                puestoEmpleado: $('#txtPuestoEmpleado').val(),
                departamentoEmpleado: $('#selectDepartamento option:selected').text(),
                departamento_id: $('#selectDepartamento').val(),
                claveSupervisor: !esExterno ? $('#claveSupervisor').val() : 0,
                supervisorEmpleado: $('#supervisorCargo').val(),
                tipoLesion: +($('#selectTipoLesion').val()) > 0 ? $('#selectTipoLesion option:selected').text() : '',
                descripcionIncidente: $('#descripcionAccidente').val().trim(),
                accionInmediata: $('#txtAccionInmediata').val().trim(),
                aplicaRIA: true, //$('input[name=aplicaRIA]:checked').val() == "1",
                tipoAccidente_id: tipoAccidenteID,
                subclasificacionID: ($('#selectTipoAccidente').val() == 5 || $('#selectTipoAccidente').val() == 13 && _idEmpresa.val() == 6) ? $('#selectSubclasificacionIncidente').val() : 0,
                riesgo, // riesgo: $('#selectRiesgo').val(),
                esExterno,
                nombreExterno,
                claveContratista,
                procedimientosViolados: $("#selectProcedimientosViolados").val().filter(value => value != "" && value > 0).map(value => ({ id: value })),
                lugarAccidente: $('#lugarAccidente').val(),
                tipoLesion_id: +($('#selectTipoLesion').val()),
                actividadRutinaria: $('input[name=actividadRutinaria]:checked').val() == '1',
                trabajoPlaneado: $('input[name=trabajoPlaneado]:checked').val() == '1',
                trabajoRealizaba: $('#trabajoRealizaba').val(),
                protocoloTrabajo_id: $('#selectProtocoloTrabajo').val(),
                experienciaEmpleado_id: $('#selectExperienciaEmpleado').val(),
                antiguedadEmpleado_id: $('#selectAntiguedadEmpleado').val(),
                turnoEmpleado_id: $('#selectTurnoEmpleado').val(),
                horasTrabajadasEmpleado: $('#horasTrabajadas').val(),
                diasTrabajadosEmpleado: $('#diasTrabajados').val(),
                capacitadoEmpleado: $('input[name=capacitado]:checked').val() == '1',
                accidentesAnterioresEmpleado: $('input[name=accidentesAnteriores]:checked').val() == '1',
                descripcionAccidente: $('#descripcionAccidente').val().trim()
            };

            if (tipoAccidenteID == 1 || tipoAccidenteID == 2 || tipoAccidenteID == 3 || tipoAccidenteID == 4 || tipoAccidenteID == 8) {
                registroInformacion.tipoContacto_id = $("#selectTipoContacto").val();
                registroInformacion.parteCuerpo_id = +($("#selectParteCuerpo").val());
                registroInformacion.agenteImplicado_id = $("#selectAgenteImplicado").val();
            } else if (tipoAccidenteID == 5 || (tipoAccidenteID == 13 && _idEmpresa.val() == 6)) {
                registroInformacion.tipoContacto_id = $("#selectTipoContacto").val();
                registroInformacion.agenteImplicado_id = $("#selectAgenteImplicado").val();
            }

            return registroInformacion;
        }

        function getObjRegistroEdit() {
            let nombreExterno = null;
            let claveContratista = 0;
            let id = $("#btnGuardar").attr('valor');
            let esExterno = $('#clienteIE input:checked').val() == 1;

            if (esExterno) {
                nombreExterno = $('#nombreEmpleado').val();

                if ($('#selectContratista').val() != "") {
                    claveContratista = $('#selectContratista').val();
                }
            }

            let riesgo = +($('#divRadioRiesgo').find('input:checked').val());

            if (isNaN(riesgo)) {
                riesgo = 0;
            }

            let tipoAccidenteID = $('#selectTipoAccidente').val();
            let registroInformacion = {
                id: id,
                departamentoEmpleado: $('#selectDepartamento option:selected').text(),
                departamento_id: $('#selectDepartamento').val(),
                fechaInforme: $('#txtFechaInforme').val(),
                fechaIncidente: $('#fechaAccidente').val(),
                fechaIngresoEmpleado: $('#txtFechaIngreso').val(),
                tipoLesion: +($('#selectTipoLesion').val()) > 0 ? $('#selectTipoLesion option:selected').text() : '',
                descripcionIncidente: $('#descripcionAccidente').val().trim(),
                descripcionAccidente: $('#descripcionAccidente').val().trim(),
                accionInmediata: $('#txtAccionInmediata').val().trim(),
                aplicaRIA: true, // $('input[name=aplicaRIA]:checked').val() == "1",
                tipoAccidente_id: tipoAccidenteID,
                subclasificacionID: tipoAccidenteID == 5 || (tipoAccidenteID == 13 && _idEmpresa.val() == 6) ? $('#selectSubclasificacionIncidente').val() : 0,
                riesgo, // riesgo: $('#selectRiesgo').val(),
                esExterno,
                nombreExterno,
                claveContratista,
                procedimientosViolados: $("#selectProcedimientosViolados").val().filter(value => value != "" && value > 0).map(value => ({ id: value })),
                horasTrabajadasEmpleado: $('#horasTrabajadas').val(),
                diasTrabajadosEmpleado: $('#diasTrabajados').val()
            };

            if (tipoAccidenteID == 1 || tipoAccidenteID == 2 || tipoAccidenteID == 3 || tipoAccidenteID == 4 || tipoAccidenteID == 8) {
                registroInformacion.tipoContacto_id = $("#selectTipoContacto").val();
                registroInformacion.parteCuerpo_id = $("#selectParteCuerpo").val();
                registroInformacion.agenteImplicado_id = $("#selectAgenteImplicado").val();
            } else if (tipoAccidenteID == 5 || (tipoAccidenteID == 13 && _idEmpresa.val() == 6)) {
                registroInformacion.tipoContacto_id = $("#selectTipoContacto").val();
                registroInformacion.agenteImplicado_id = $("#selectAgenteImplicado").val();
            }

            return registroInformacion
        }

        function enviarCorreosMinuta(minutaID, usuarios) {
            $.blockUI({ message: "Incidente guardado correctamente. Generando archivos de minuta..." });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Reportes/Vista.aspx?idReporte=4&inMemory=true&minuta=" + minutaID,
                success: function (response) {
                    $.ajax({
                        datatype: "json",
                        type: "POST",
                        url: "/Reportes/Vista.aspx?idReporte=5&inMemory=true&minuta=" + minutaID,
                        success: function (response) {
                            if (response != null) {
                                $.ajax({
                                    datatype: "json",
                                    type: "POST",
                                    url: "/SeguimientoAcuerdos/enviarCorreos",
                                    data: { minutaID, usuarios },
                                    success: function (response) {
                                        if (response.success === true) {
                                            $.unblockUI();
                                            AlertaGeneral("Confirmación", "Correos enviados correctamente");
                                            regresarInforme();
                                        }
                                        else {
                                            $.unblockUI();
                                            AlertaGeneral("Alerta", "Ocurrió un problema al enviar a los siguientes usuarios:<br/>" + response.obj);
                                        }
                                    },
                                    error: function () {
                                        $.unblockUI();
                                    }
                                });
                            }
                            else {
                                AlertaGeneral("Alerta", "Ocurrió un problema al convertir la minuta a PDF para ser enviada");
                            }
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function limpiarCampos() {
            $('input').val('');
            $('textarea').val('')
            $('td label').text('')
            $('select').val('').change()
            $('input[name="empresa"]').prop('checked', false);
            $('input[name="accidentesAnteriores"]').prop('checked', false);
            $('input[name="capacitado"]').prop('checked', false);
            $('input[name="actividadRutinaria"]').prop('checked', false);
            $('input[name="trabajoPlaneado"]').prop('checked', false);
            $('input[name="consecuencias"]').prop('checked', false);
            $('input[name="probabilidadOcurrencia"]').prop('checked', false);
            $('input[name="riesgoPxC"]').prop('checked', false);
            informe_id = 0
        }

        function regresarInforme() {
            const getUrl = window.location;
            const baseUrl = getUrl.protocol + "//" + getUrl.host;
            const urlInforme = baseUrl + `/Administrativo/IndicadoresSeguridad/CapturaInformePreliminar`;

            window.location.href = urlInforme;
        }
        //#endregion

        //#region EVENT'S CHANGE, CLICKS
        $('.select2').select2();

        $('#selectEmpleado').change(cargarInfoEmpleado)

        $('#fechaAccidente').datetimepicker({
            format: 'DD/MM/YYYY hh:mm a'
        });

        $('input[type=radio][name=empresa]').change(function () {
            esExterno = this.value
            // setSelectContratistas()
            // setBtnAddEmpleado(this.value)
        });

        $('input[type=radio][name=consecuencias]').change(function () {
            evaluacionConsecuencias = this.value
            calcularEvaluacionRiesgo()
        });

        $('input[type=radio][name=probabilidadOcurrencia]').change(function () {
            evaluacionProbabilidad = this.value
            calcularEvaluacionRiesgo()
        });

        $('#addRowGrupoInvestigacion').on('click', function () {
            $('#tblGrupoInvestigacion tbody').append(addRowGrupoInvestigacion());
            initNombreGrupoInvestigacion();

            $('#tblGrupoInvestigacion tbody input[type="checkbox"]').off().click(alternarGrupoInvestigacionExterno);
        });

        $('#tblGrupoInvestigacion tbody input[type="checkbox"]').off().click(alternarGrupoInvestigacionExterno);

        $('#addRowOrdenCronologico').on('click', function () {
            $('#tblOrdenCronologico tbody').append(addRowOrdenCronologico())
        })

        $('#addRowEventoDetonador').on('click', function () {
            $('#tblEventoDetonador tbody').append(addRowEventoDetonador())
        })

        $('#addRowCausaInmediata').on('click', function () {
            $('#tblCausaInmediata tbody').append(addRowCausaInmediata())
        })

        $('#addRowCausaBasica').on('click', function () {
            $('#tblCausaBasica tbody').append(addRowCausaBasica())
        })

        $('#addRowCausaRaiz').on('click', function () {
            $('#tblCausaRaiz tbody').append(addRowCausaRaiz())
        })

        $('#addRowMedidasControl').on('click', function () {
            $('#tblMedidasControl tbody').append(addRowMedidasControl())
            $('tbody.medidasControl tr').last().find('select.selectPrioridadMedidasNew').fillCombo('GetPrioridadesActividad', null, false);
            $('tbody.medidasControl tr').last().find('select.selectPrioridadMedidasNew').select2();

            initResponsableMedidasControl()
            initFechaMedidasControl()
        })

        $('#deleteRowGrupoInvestigacion').on('click', function () {
            deleteRowsTables($("tbody.grupoInvestigacion tr"), 6)
        })

        $('#deleteRowOrdenCronologico').on('click', function () {
            deleteRowsTables($("tbody.ordenCronologico tr"), 6)
        })

        $('#deleteRowEventoDetonador').on('click', function () {
            deleteRowsTables($("tbody.eventoDetonador tr"), 2)
        })

        $('#deleteRowCausaInmediata').on('click', function () {
            deleteRowsTables($("tbody.causaInmediata tr"), 2)
        })

        $('#deleteRowCausaBasica').on('click', function () {
            deleteRowsTables($("tbody.causaBasica tr"), 2)
        })

        $('#deleteRowCausaRaiz').on('click', function () {
            deleteRowsTables($("tbody.causaRaiz tr"), 2)
        })

        $('#deleteRowMedidasControl').on('click', function () {
            deleteRowsTables($("tbody.medidasControl tr"), 2)
        })

        $('#btnAddEmpleado').on('click', function () {
            clearModal()
            $('#modalEmpleado').modal('show')
        })

        $('#btnGuardarEmpleado').on('click', function () {
            guardarEmpleado()
        })
        //#endregion

        function setDatosInicio() {
            $('#txtFechaInforme').datepicker("setDate", hoy).datepicker({ dateFormat: "dd/mm/yy", maxDate: hoy, showAnim: "slide" });
            $('#txtFechaIngreso').datepicker({ dateFormat: "dd/mm/yy", maxDate: hoy, showAnim: "slide" });

            // $('#selectRiesgo').fillCombo('GetEvaluacionesRiesgo', null, false);
            $('#selectProcedimientosViolados').fillCombo('getTipoProcedimientosVioladosList', null, false);
        }

        function initTablaEvidencias() {
            if (_flagInformePreliminar) {
                dtTablaEvidencias = tablaEvidencias.DataTable({
                    language: dtDicEsp,
                    paging: false,
                    searching: false,
                    order: [[2, "desc"]],
                    info: false,
                    columns: [
                        { data: 'nombre', title: 'Nombre' },
                        { data: 'fecha', title: 'Fecha' },
                        {
                            data: 'tieneEvidencia', title: 'Evidencia', render: (data, type, row) =>
                                row.tieneEvidencia ?
                                    `<button title="Descargar evidencia" class="btn btn-primary descargarEvidencia"><i class="fas fa-arrow-alt-circle-down"></i></button>
                                <button title="Ver evidencia" class="btn btn-primary verEvidencia"><i class="fas fa-eye"></i></button>` :
                                    `<input type="file" accept="application/pdf, image/*" class="form-control inputEvidencia"></input>`
                        },
                        {
                            data: 'id', title: 'Eliminar', render: (data, type, row) => row.puedeEliminar ?
                                `<button class="btn btn-danger botonEliminarEvidencia"><i class="fas fa-trash"></i></button>` : ''
                        },
                    ],
                    columnDefs: [
                        { className: "dt-center", "targets": "_all" }
                    ],
                    drawCallback: function (settings) {
                        tablaEvidencias.find('button.descargarEvidencia').click(function () {
                            const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();
                            location.href = `DescargarEvidenciaInforme?evidenciaID=${evidencia.id}`;
                        });

                        tablaEvidencias.find('button.verEvidencia').click(function () {
                            const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();
                            mostrarEvidencia(evidencia.id);
                        });

                        tablaEvidencias.find('button.botonEliminarEvidencia').off().click(function () {

                            const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();

                            if (evidencia.tieneEvidencia) {
                                AlertaAceptarRechazarNormal(
                                    'Confirmar eliminación',
                                    '¿Está seguro de eliminar esta evidencia?',
                                    () => eliminarEvidencia(evidencia.id)
                                );
                            } else {
                                dtTablaEvidencias.row($(this).closest('tr')).remove().draw();
                            }
                        });
                    }
                });
            } else {
                dtTablaEvidencias = tablaEvidencias.DataTable({
                    language: dtDicEsp,
                    paging: false,
                    searching: false,
                    order: [[2, "desc"]],
                    info: false,
                    columns: [
                        { data: 'nombre', title: 'Nombre' },
                        { data: 'fecha', title: 'Fecha' },
                        {
                            data: 'tieneEvidencia', title: 'Evidencia', render: (data, type, row) =>
                                row.tieneEvidencia ?
                                    `<button title="Descargar evidencia" class="btn btn-primary descargarEvidencia"><i class="fas fa-arrow-alt-circle-down"></i></button>
                                <button title="Ver evidencia" class="btn btn-primary verEvidencia"><i class="fas fa-eye"></i></button>` :
                                    `<input type="file" accept="application/pdf, image/*" class="form-control inputEvidencia"></input>`
                        },
                        {
                            data: 'tipoEvidenciaRIA', title: 'Tipo Evidencia', render: (data, type, row, meta) =>
                                `<select class="form-control selectTipoEvidenciaRIA"></select>`
                        },
                        {
                            data: 'id', title: 'Eliminar', render: (data, type, row) => row.puedeEliminar ?
                                `<button class="btn btn-danger botonEliminarEvidencia"><i class="fas fa-trash"></i></button>` : ''
                        },
                    ],
                    columnDefs: [
                        { className: "dt-center", "targets": "_all" }
                    ],
                    createdRow: function (row, rowData) {
                        let selectTipoEvidenciaRIA = $(row).find('.selectTipoEvidenciaRIA');

                        selectTipoEvidenciaRIA.fillCombo('FillComboTipoEvidenciaRIA', null, false, null);
                        selectTipoEvidenciaRIA.find('option[value="' + rowData.tipoEvidenciaRIA + '"]').attr('selected', true);
                    },
                    drawCallback: function (settings) {
                        tablaEvidencias.find('button.descargarEvidencia').click(function () {
                            const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();
                            location.href = `DescargarEvidenciaInforme?evidenciaID=${evidencia.id}`;
                        });

                        tablaEvidencias.find('button.verEvidencia').click(function () {
                            const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();
                            mostrarEvidencia(evidencia.id);
                        });

                        tablaEvidencias.find('button.botonEliminarEvidencia').off().click(function () {

                            const evidencia = dtTablaEvidencias.row($(this).parents('tr')).data();

                            if (evidencia.tieneEvidencia) {
                                AlertaAceptarRechazarNormal(
                                    'Confirmar eliminación',
                                    '¿Está seguro de eliminar esta evidencia?',
                                    () => eliminarEvidencia(evidencia.id)
                                );
                            } else {
                                dtTablaEvidencias.row($(this).closest('tr')).remove().draw();
                            }
                        });
                    }
                });
            }
        }

        function bloquearCamposAutomaticos() {
            $('#selectExperienciaEmpleado').attr('disabled', true);
            $('#selectAntiguedadEmpleado').attr('disabled', true);
            $('#selectTurnoEmpleado').attr('disabled', true);
            $('#supervisorCargo').attr('disabled', true);
        }

        function alternarPersonaExterna() {
            let valor = +($(this).val());
            const esExterno = valor == 1;

            const inputClaveEmpleado = $('#claveEmpleado');
            const inputNombreEmpleado = $('#nombreEmpleado');

            const inputClaveSupervisor = $('#claveSupervisor');
            const inputNombreSupervisor = $('#supervisorCargo');

            inputClaveEmpleado.attr('disabled', esExterno);
            inputNombreEmpleado.attr('disabled', !esExterno);

            inputClaveSupervisor.attr('disabled', esExterno);
            inputNombreSupervisor.attr('disabled', !esExterno);

            if (esExterno) {
                $('#divContratista').show(500);
                inputClaveEmpleado.val('');
                inputClaveSupervisor.val('');
            } else {
                $('#divContratista').hide(500);

                if (inputClaveEmpleado.val().length == 0) {
                    inputNombreEmpleado.val('');
                }

                if (inputClaveSupervisor.val().length == 0) {
                    inputNombreSupervisor.val('');
                }

                $('#selectContratista').val('').change();
            }

            if (esExterno) {
                chkEsContratista.prop('checked', true);
            } else {
                chkEsContratista.prop('checked', false);
            }
            chkEsContratista.trigger('change');
        }
    }

    $(document).ready(() => CapturaIncidente.Seguridad = new Seguridad())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();