(() => {
    $.namespace('Administrativo.Seguimientos');
    Seguimientos = function () {
        const tblPlanAccion = $('#tblPlanAccion');
        const cboFiltroCC = $('#cboFiltroCC');
        const cboFiltroArea = $('#cboFiltroArea');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFinal = $('#inputFechaFinal');
        const cboSeguimiento = $('#cboSeguimiento');
        const report = $("#report");
        const btnBuscar = $('#btnBuscar');
        const btnImprimir = $('#btnImprimir');
        const SeguimientoPlanAccion = $('#SeguimientoPlanAccion');
        const SeguimientoEstatus = $('#SeguimientoEstatus');
        const divRenglonesDatos = $('#divRenglonesDatos');
        const modalAccion = $('#modalAccion');
        const textAreaAccionRequerida = $('#textAreaAccionRequerida');
        const modalDeteccion = $('#modalDeteccion');
        const textAreaDescripcionDeteccion = $('#textAreaDescripcionDeteccion');
        const botonDescargarDeteccion = $('#botonDescargarDeteccion');
        const modalConfirmarRespuesta = $('#modalConfirmarRespuesta');
        const textAreaMotivo = $('#textAreaMotivo');
        const botonGuardarRespuesta = $('#botonGuardarRespuesta');
        const modalMotivo = $('#modalMotivo');
        const textAreaMotivoConsulta = $('#textAreaMotivoConsulta');
        const botonVisualizarDeteccion = $('#botonVisualizarDeteccion');
        const modalComentarioLider = $('#modalComentarioLider');
        const textAreaComentarioLider = $('#textAreaComentarioLider');
        const botonGuardarComentarioLider = $('#botonGuardarComentarioLider');

        let dtPlanAccion;

        (function init() {
            inputFechaInicio.datepicker({ dateFormat: 'yy/mm/dd' }).datepicker('option', 'showAnim', 'slide').datepicker('setDate', new Date());
            inputFechaFinal.datepicker({ dateFormat: 'yy/mm/dd' }).datepicker('option', 'showAnim', 'slide').datepicker('setDate', new Date());

            fncListeners();
            initTblPlanAccion();
            fncLLenarCombos();
        })();

        // $('.divMedidas').on('click', '.radioBtn a', function () {
        //     let div = $(this).closest('div');
        //     let seleccion = $(this).attr('respuesta');
        //     let toggle = $(this).attr('data-toggle');
        //     let botonMotivo = $(this).closest('.divControles').find('.botonMotivo');

        //     div.find(`a[data-toggle="${toggle}"]`).not(`[respuesta="${seleccion}"]`).removeClass('active').addClass('notActive');
        //     div.find(`a[data-toggle="${toggle}"][respuesta="${seleccion}"]`).removeClass('notActive').addClass('active');

        //     botonMotivo.attr('disabled', (seleccion == 1));
        //     botonMotivo.data('motivo', '');
        //     botonMotivo.addClass('btn-default');
        //     botonMotivo.removeClass('btn-success');
        //     botonMotivo.find('i').addClass('fa-comment-dots');
        //     botonMotivo.find('i').removeClass('fa-check');
        // });

        function fncListeners() {
            cboSeguimiento.on("change", function () {
                if ($(this).val() == "Estatus") {
                    SeguimientoEstatus.show();
                    SeguimientoPlanAccion.hide();
                } else if ($(this).val() == "PlanAccion") {
                    SeguimientoPlanAccion.show();
                    SeguimientoEstatus.hide();
                } else {
                    SeguimientoEstatus.hide();
                    SeguimientoPlanAccion.hide();
                }
            });

            btnImprimir.click(verReporte);

            btnBuscar.click(() => {
                let tipoSeguimiento = cboSeguimiento.val();

                switch (tipoSeguimiento) {
                    case 'Estatus':
                        getEstatusSeguimientos();
                        break;
                    case 'PlanAccion':
                        llenarTablaPlanAccion();
                        break;
                    case 'Ocultar':
                        Alert2Warning('Debe seleccionar un tipo de seguimiento.');
                        break;
                }
            });

            botonGuardarRespuesta.click(guardarRespuesta);
            botonDescargarDeteccion.click(() => {
                location.href = `/Administrativo/CincoS/DescargarArchivo?idArchivo=${+botonDescargarDeteccion.data('idarchivo')}`;
            });
            botonVisualizarDeteccion.click(() => {
                axios.post('VisualizarArchivo', { objParamDTO: { idArchivo: +botonVisualizarDeteccion.data('idarchivo') } }).then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            });

            botonGuardarComentarioLider.on("click", function () {
                fncGuardarComentarioLider(botonGuardarComentarioLider.data("auditoria-det-id"));
            });
        }

        function fncLLenarCombos() {
            cboFiltroCC.fillCombo('GetCCs', { consulta: 2, checkListId: null }, false, null);
            cboFiltroArea.fillCombo('GetAreas', null, false, null);
        }

        function initTblPlanAccion() {
            dtPlanAccion = tblPlanAccion.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'id', title: '#' },
                    { data: 'deteccion', title: 'DETECCIÓN (NO OK)' },
                    { data: 'descripcion', title: 'DESCRIPCIÓN' },
                    { data: 'medida', title: 'MEDIDA' },
                    {
                        data: 'fechaCompromiso', title: 'FECHA COMPROMISO',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'lider', title: ' LIDER' },
                    { data: 'tiempoTranscurrido', title: 'TIEMPO TRANSCURRIDO' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
            });
        }

        function llenarTablaPlanAccion() {
            axios.post('llenarTablaPlanAccion', { objParamDTO: { cc: cboFiltroCC.val(), areaId: cboFiltroArea.val() }, fechaInicio: inputFechaInicio.val(), fechaFinal: inputFechaFinal.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    AddRows(tblPlanAccion, items);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function getEstatusSeguimientos() {
            axios.post('GetSeguimientos', { objParamDTO: { cc: cboFiltroCC.val(), fechaInicio: inputFechaInicio.val(), fechaFinal: inputFechaFinal.val() } }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    cargarRenglonesDatos(response.data.lstAuditoriasDetDTO);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function verReporte() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=${282}&cc=${cboFiltroCC.val()}&areaId=${cboFiltroArea.val()}&fechaInicio=${inputFechaInicio.val()}&fechaFinal=${inputFechaFinal.val()}`);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        function cargarRenglonesDatos(datos) {
            divRenglonesDatos.empty();

            datos.forEach(renglon => {
                divRenglonesDatos.append(`
                    <div class="row">
                        <div class="col-sm-4 divDetecciones"></div>
                        <div class="col-sm-4 divMedidas"></div>
                        <div class="col-sm-4 divAcciones"></div>
                    </div>
                `);

                let ultimoRenglon = divRenglonesDatos.find('.row').last();

                switch (renglon.estatusAuditoriaDet) {
                    case 1:
                        ultimoRenglon.find('.divDetecciones').append(`
                            <div class="col-sm-12 cuadroAuditoriaDetalle">
                                <span style="display: block;">${renglon.descInspeccion}</span>
                                <div class="divControles" style="margin-top: 5px;">
                                    <button class="btn btn-sm btn-default botonAccion"><i class="fa fa-comment-dots"></i></button>
                                    <button class="btn btn-sm btn-default botonDeteccion" idarchivo="${renglon.idArchivoDeteccion}"><i class="fa fa-image"></i></button>
                                    <div class="text-center" style="display: inline-block;">
                                        <label for="inputCargarSeguimientoDeteccion_${renglon.id}" class="btn btn-sm btn-default botonCargarSeguimientoDeteccion"><i class="fa fa-upload"></i></label>
                                        <input id="inputCargarSeguimientoDeteccion_${renglon.id}" type="file" class="inputCargarSeguimientoDeteccion inputCargarSeguimientoDeteccion_${renglon.id}" auditoriadet-id="${renglon.id}" accept="application/pdf, image/*">
                                    </div>
                                    <button class="btn btn-sm btn-default botonComentariosLider" auditoria-det-id="${renglon.id}"><i class="fa fa-comment-alt"></i></button>
                                </div>
                            </div>
                        `);

                        ultimoRenglon.find('.botonAccion').data('accion', renglon.accion);
                        ultimoRenglon.find('.botonDeteccion').data('descDeteccion', renglon.descDeteccion);
                        ultimoRenglon.find('.botonComentariosLider').data('comentarioLider', renglon.comentarioLider);
                        break;
                    case 2:
                        if (renglon.aprobado == 0) {
                            ultimoRenglon.find('.divMedidas').append(`
                                <div class="col-sm-12 cuadroAuditoriaDetalle">
                                    <span style="display: block;">${renglon.descInspeccion}</span>
                                    <div class="divControles" style="margin-top: 5px;">
                                        <button class="btn btn-sm btn-default botonVisualizarArchivoSeguimiento" idarchivo="${renglon.idArchivoSeguimiento}"><i class="fa fa-eye"></i></button>
                                        <button class="btn btn-sm btn-default botonDescargarArchivoSeguimiento" idarchivo="${renglon.idArchivoSeguimiento}"><i class="fa fa-download"></i></button>
                                        <div class="radioBtn btn-group pull-right">
                                            <a class="btn btn-sm btn-success notActive" data-toggle="radioRespuesta_${renglon.id}" auditoriadet-id="${renglon.id}" idarchivo="${renglon.idArchivoSeguimiento}" respuesta="1" style="font-weight: bold;">
                                                <i class="fa fa-check"></i>&nbsp;SÍ
                                            </a>
                                            <a class="btn btn-sm btn-danger notActive" data-toggle="radioRespuesta_${renglon.id}" auditoriadet-id="${renglon.id}" idarchivo="${renglon.idArchivoSeguimiento}" respuesta="2" style="font-weight: bold;">
                                                <i class="fa fa-times"></i>&nbsp;NO
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            `);
                        } else if (renglon.aprobado == 2) {
                            ultimoRenglon.find('.divMedidas').append(`
                                <div class="col-sm-12 cuadroAuditoriaDetalle">
                                    <span style="display: block;">${renglon.descInspeccion}</span>
                                    <div class="divControles" style="margin-top: 5px;">
                                        <button class="btn btn-sm btn-default botonVisualizarArchivoSeguimiento" idarchivo="${renglon.idArchivoSeguimiento}"><i class="fa fa-eye"></i></button>
                                        <button class="btn btn-sm btn-default botonDescargarArchivoSeguimiento" idarchivo="${renglon.idArchivoSeguimiento}"><i class="fa fa-download"></i></button>
                                        <button class="btn btn-sm btn-default botonMotivo" captura="0"><i class="fa fa-comment-dots"></i></button>
                                        <div class="text-center" style="display: inline-block;">
                                            <label for="inputCargarSeguimientoMedida_${renglon.id}" class="btn btn-sm btn-default botonCargarSeguimientoMedida"><i class="fa fa-upload"></i></label>
                                            <input id="inputCargarSeguimientoMedida_${renglon.id}" type="file" class="inputCargarSeguimientoMedida inputCargarSeguimientoMedida_${renglon.id}" auditoriadet-id="${renglon.id}" accept="application/pdf, image/*">
                                        </div>
                                    </div>
                                </div>
                            `);

                            ultimoRenglon.find('.botonMotivo').data('motivo', renglon.comentarioRechazo);
                        }
                        break;
                    case 3:
                        ultimoRenglon.find('.divAcciones').append(`
                            <div class="col-sm-12 cuadroAuditoriaDetalle">
                                <span style="display: block;">${renglon.descInspeccion}</span>
                                <div class="divControles" style="margin-top: 5px;">
                                    <div class="col-sm-3" style="padding-left: 0px;">
                                        <button class="btn btn-sm btn-default botonVisualizarArchivoSeguimiento" idarchivo="${renglon.idArchivoSeguimiento}"><i class="fa fa-eye"></i></button>
                                        <button class="btn btn-sm btn-default botonDescargarArchivoSeguimiento" idarchivo="${renglon.idArchivoSeguimiento}"><i class="fa fa-download"></i></button>
                                    </div>
                                    <div class="col-sm-9" style="padding-right: 0px; margin-top: 5px;">
                                        <span style="color: rgb(0, 176, 80); font-weight: bold; font-size: 16px;">IMPLEMENTACIÓN EN ÁREA OK</span>
                                    </div>
                                </div>
                            </div>
                        `);
                        break;
                }
            });

            //#region Eventos
            $('.divMedidas').on('click', '.radioBtn a', function () {
                let div = $(this).closest('div');
                let seleccion = $(this).attr('respuesta');
                let toggle = $(this).attr('data-toggle');

                div.find(`a[data-toggle="${toggle}"]`).not(`[respuesta="${seleccion}"]`).removeClass('active').addClass('notActive');
                div.find(`a[data-toggle="${toggle}"][respuesta="${seleccion}"]`).removeClass('notActive').addClass('active');

                textAreaMotivo.attr('disabled', seleccion == 1);
                textAreaMotivo.val('');
                botonGuardarRespuesta.data('idarchivo', +$(this).attr('idarchivo'));
                botonGuardarRespuesta.data('respuesta', +seleccion);
                modalConfirmarRespuesta.modal('show');
            });

            $('.botonAccion').on('click', function () {
                textAreaAccionRequerida.val($(this).data('accion'));
                modalAccion.modal('show');
            });

            $('.botonDeteccion').on('click', function () {
                textAreaDescripcionDeteccion.val($(this).data('descDeteccion'));
                botonDescargarDeteccion.data('idarchivo', +$(this).attr('idarchivo'));
                botonVisualizarDeteccion.data('idarchivo', +$(this).attr('idarchivo'));
                modalDeteccion.modal('show');
            });

            $('.botonMotivo').on('click', function () {
                textAreaMotivoConsulta.val($(this).data('motivo'));
                modalMotivo.modal('show');
            });

            $('.botonComentariosLider').on('click', function () {
                textAreaComentarioLider.val($(this).data('comentarioLider'));
                botonGuardarComentarioLider.data("auditoria-det-id", $(this).attr('auditoria-det-id'));

                modalComentarioLider.modal('show');
            });

            $('.inputCargarSeguimientoDeteccion, .inputCargarSeguimientoMedida').on('change', function () {
                let archivo = $(this)[0].files[0];
                let auditoriaDet_id = +$(this).attr('auditoriadet-id');

                if (archivo) {
                    let data = new FormData();

                    data.append("objParamDTO", JSON.stringify({ id: auditoriaDet_id }));
                    data.append('objArchivoSeguimiento', archivo);

                    axios.post('RegistrarArchivoSeguimiento', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                        let { success, datos, message } = response.data;
                        if (success) {
                            Alert2Exito(message)
                            btnBuscar.click();
                        } else {
                            Alert2Error(message)
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                }
            });

            $('.botonVisualizarArchivoSeguimiento').on('click', function () {
                axios.post('VisualizarArchivo', { objParamDTO: { idArchivo: +$(this).attr('idarchivo') } }).then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            });

            $('.botonDescargarArchivoSeguimiento').on('click', function () {
                location.href = `/Administrativo/CincoS/DescargarArchivo?idArchivo=${+$(this).attr('idarchivo')}`;
            });
            //#endregion
        }

        function guardarRespuesta() {
            let data = {
                idArchivoSeguimiento: +botonGuardarRespuesta.data('idarchivo'),
                aprobado: +botonGuardarRespuesta.data('respuesta'),
                comentarioRechazo: textAreaMotivo.val()
            };

            axios.post('AutorizarRechazarArchivoSeguimiento', { objParamDTO: data }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    modalConfirmarRespuesta.modal('hide');
                    Alert2Exito(message);
                    btnBuscar.click();
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function fncGuardarComentarioLider(idAuditoriaDet) {
            axios.post("GuardarComentarioLider", { idAuditDet: idAuditoriaDet, comentario: textAreaComentarioLider.val() }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //CODE...
                    Alert2Exito("Comentario guardado correctamente");
                    $(`[auditoria-det-id=${idAuditoriaDet}]`).data("comentarioLider", textAreaComentarioLider.val());

                }
            }).catch(error => Alert2Error(error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear();
            dt.rows.add(lst).draw();
        }
    }

    $(document).ready(() => {
        Administrativo.Seguimientos = new Seguimientos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();