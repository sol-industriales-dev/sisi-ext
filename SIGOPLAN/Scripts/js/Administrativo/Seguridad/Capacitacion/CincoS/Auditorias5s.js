(() => {
    $.namespace('CincoS.Auditorias5s');

    Auditorias5s = function () {
        //#region Selectores
        const selectCentroCosto = $('#selectCentroCosto');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFinal = $('#inputFechaFinal');
        const botonIniciarAuditoria = $('#botonIniciarAuditoria');
        const botonBuscar = $('#botonBuscar');
        const tablaAuditorias = $('#tablaAuditorias');
        const modalAuditoria = $('#modalAuditoria');
        const selectAuditoria = $('#selectAuditoria');
        const selectProyecto = $('#selectProyecto');
        const inputArea = $('#inputArea');
        const inputFechaAuditoria = $('#inputFechaAuditoria');
        const selectAuditor = $('#selectAuditor');
        const tablaInspecciones = $('#tablaInspecciones');
        const botonGuardarTerminar = $('#botonGuardarTerminar');
        const botonGuardarContinuar = $('#botonGuardarContinuar');
        const textoInspecciones = $('#textoInspecciones');
        const modalAccion = $('#modalAccion');
        const textAreaAccionRequerida = $('#textAreaAccionRequerida');
        const botonGuardarAccionRequerida = $('#botonGuardarAccionRequerida');
        const celdaPorcentaje = $('#celdaPorcentaje');
        const celdaEstatus5s = $('#celdaEstatus5s');
        const celdaEstatusAuditoria = $('#celdaEstatusAuditoria');
        //#endregion

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
        //#endregion

        let dtAuditorias;
        let dtInspecciones;

        let _botonAccionSeleccionado = null;
        let _listaLideres = [];
        let _auditoriaID = 0;
        let _auditoriaCompleta = 0;

        (function init() {
            selectCentroCosto.fillCombo('GetCCs', { consulta: 2, checkListId: null }, false, null);
            selectAuditoria.fillCombo('FillCboCheckList', null, false, null);
            inputFechaInicio.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFinal.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaActual);
            inputFechaAuditoria.datepicker({ dateFormat, showAnim });

            initTablaAuditorias();
            initTablaInspecciones();

            cargarAuditorias()

            // AddRows(tablaInspecciones, [
            //     { id: 1, inspeccion: '1', descripcion: '1' },
            //     { id: 2, inspeccion: '2', descripcion: '2' },
            //     { id: 3, inspeccion: '3', descripcion: '3' }
            // ]);

            botonIniciarAuditoria.click(() => {
                _auditoriaID = 0;
                limpiarModalAuditoria();
                selectAuditoria.attr('disabled', false);
                modalAuditoria.modal('show');
            });
            botonGuardarAccionRequerida.click(guardarAccionRequerida);
            botonBuscar.click(cargarAuditorias);
            botonGuardarContinuar.click(() => {
                _auditoriaCompleta = 0;
                guardarAuditoria();
            });
            botonGuardarTerminar.click(() => {
                _auditoriaCompleta = 1;
                guardarAuditoria();
            });
        })();

        selectAuditoria.on('change', function () {
            let checkListId = +selectAuditoria.val();

            if (checkListId > 0) {
                selectProyecto.fillCombo('FillCboProyectos', { objParamDTO: { checkListId } }, false, null);
                selectAuditor.empty();

                axios.post('GetInspeccionesRelCheckList', { objParamDTO: { checkListId } }).then(response => {
                    let { success, data, message } = response.data;

                    if (success) {
                        inputArea.val(response.data.area);
                        _listaLideres = response.data.items;
                        AddRows(tablaInspecciones, response.data.lstInspecciones);

                        textoInspecciones.text('0 inspecciones de ' + response.data.lstInspecciones.length);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                selectProyecto.empty();
                selectAuditor.empty();
                textoInspecciones.text('0 inspecciones de 0');
                dtInspecciones.clear().draw(false);
            }
        });

        selectProyecto.on('change', function () {
            let cc = selectProyecto.val();

            if (cc) {
                selectAuditor.fillCombo('FillCboAuditores', { objParamDTO: { cc } }, false, null);
            } else {
                selectAuditor.empty();
            }
        })

        function initTablaAuditorias() {
            dtAuditorias = tablaAuditorias.DataTable({
                retrieve: true,
                paging: false,
                ordering: false,
                language: dtDicEsp,
                dom: 't',
                scrollY: '45vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaAuditorias.on("click", ".imprimirReporte", function () {
                        let rowData = dtAuditorias.row($(this).closest('tr')).data();
                        var path = `/Reportes/Vista.aspx?idReporte=285&idAudi=${rowData.id}&checkListId=${rowData.checkListId}&ccs=${rowData.cc}&auditor=${rowData.auditor}&pdf=0`;
                        $("#report").attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    });

                    tablaAuditorias.on('click', '.botonEditarAuditoria', function () {
                        let rowData = dtAuditorias.row($(this).closest('tr')).data();

                        _auditoriaID = rowData.id;

                        axios.post('GetDatosActualizarAuditoria', { objParamDTO: { id: rowData.id } }).then(response => {
                            let { success, data, message } = response.data;

                            if (success) {
                                limpiarModalAuditoria();
                                cargarInformacionAuditoria(response.data.objAuditoria, response.data.items);
                                selectAuditoria.attr('disabled', true);
                                modalAuditoria.modal('show');
                            } else {
                                AlertaGeneral(`Alerta`, message);
                            }
                        }).catch(error => AlertaGeneral(`Alerta`, error.message));
                    });
                },
                columns: [
                    { data: 'id', title: 'id' },
                    { data: 'nombreAuditoria', title: 'NOMBRE DE AUDITORÍA', width: '40%' },
                    { data: 'auditor', title: 'AUDITOR', width: '20%' },
                    { data: 'cc', title: 'CC', width: '20%' },
                    { data: 'area', title: 'ÁREA', with: '10%' },
                    {
                        data: 'porcCumplimiento', title: '% CUMPLIMIENTO',
                        render: function (data, type, row) {
                            return `${row.porcCumplimiento.toFixed(2)}%`
                        },
                        width: '5%'
                    },
                    {
                        data: 'fecha', title: 'FECHA',
                        render: function (data, type, row) {
                            return moment(row.fecha).format('DD/MM/YYYY')
                        },
                        width: '5%'
                    },
                    {
                        title: '',
                        render: (data, type, row, meta) => {
                            if (row.auditoriaCompleta) {
                                return `<button title="Imprimir reporte de auditorias" class="btn btn-primary btn-xs imprimirReporte"><i class="fas fa-print"></i></button>`

                            } else {
                                return ``;
                            }
                        }
                    },
                    {
                        title: '', render: (data, type, row, meta) => {
                            return row.auditoriaCompleta ? `` : `<button class="btn btn-warning btn-xs botonEditarAuditoria"><i class="fas fa-edit"></i></button>`
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaInspecciones() {
            dtInspecciones = tablaInspecciones.DataTable({
                retrieve: true,
                paging: false,
                ordering: false,
                language: dtDicEsp,
                dom: 't',
                initComplete: function (settings, json) {
                    tablaInspecciones.on('change', '.inputDeteccion', function () {
                        let row = $(this).closest('tr');
                        let inputDeteccion = $(row).find(`.inputDeteccion`);
                        let botonDeteccion = $(row).find(`.botonDeteccion`);
                        let iconoBoton = botonDeteccion.find('i');

                        if (inputDeteccion[0].files.length > 0) {
                            botonDeteccion.addClass('btn-success');
                            botonDeteccion.removeClass('btn-default');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-upload');
                        } else {
                            botonDeteccion.addClass('btn-default');
                            botonDeteccion.removeClass('btn-success');
                            iconoBoton.addClass('fa-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });

                    tablaInspecciones.on('change', '.inputMedida', function () {
                        let row = $(this).closest('tr');
                        let inputMedida = $(row).find(`.inputMedida`);
                        let botonMedida = $(row).find(`.botonMedida`);
                        let iconoBoton = botonMedida.find('i');

                        if (inputMedida[0].files.length > 0) {
                            botonMedida.addClass('btn-success');
                            botonMedida.removeClass('btn-default');
                            iconoBoton.addClass('fa-check');
                            iconoBoton.removeClass('fa-upload');
                        } else {
                            botonMedida.addClass('btn-default');
                            botonMedida.removeClass('btn-success');
                            iconoBoton.addClass('fa-upload');
                            iconoBoton.removeClass('fa-check');
                        }
                    });

                    tablaInspecciones.on('click', '.radioBtn a', function () {
                        let rowData = dtInspecciones.row($(this).closest('tr')).data();
                        let div = $(this).closest('div');
                        let seleccion = $(this).attr('respuesta');

                        let selectLider = $(this).closest('tr').find('.selectLider');
                        let inputFecha = $(this).closest('tr').find('.inputFechaInspeccion');
                        if (seleccion == 1) {
                            inputFecha.prop('disabled', true);
                            inputFecha.val('');
                        } else {
                            inputFecha.prop('disabled', false);
                        }

                        div.find(`a[data-toggle="radioRespuesta${rowData.id}"]`).not(`[respuesta="${seleccion}"]`).removeClass('active').addClass('notActive');
                        div.find(`a[data-toggle="radioRespuesta${rowData.id}"][respuesta="${seleccion}"]`).removeClass('notActive').addClass('active');

                        calcularInspeccionesCumplimiento();
                    });

                    tablaInspecciones.on('click', '.botonAccion', function () {
                        _botonAccionSeleccionado = this;

                        textAreaAccionRequerida.val($(this).data('accion') ? $(this).data('accion') : '');
                        modalAccion.modal('show');
                        modalAccion.css('z-index', 1501);
                        $('.modal-backdrop:eq(1)').css('z-index', 1500);
                    });
                },
                createdRow: function (row, rowData) {
                    let selectLider = $(row).find('.selectLider');

                    selectLider.append('<option value="">--Seleccione--</option>')
                    _listaLideres.forEach((lider) => {
                        selectLider.append(`<option value="${lider.Value}">${lider.Text}</option>`);
                    });
                    $(row).find('.inputFechaInspeccion').datepicker({ dateFormat, showAnim });

                    if (rowData.respuesta > 0) {
                        $(row).find(".radioBtn").find(`a[data-toggle="radioRespuesta${rowData.id}"]`).not(`[respuesta="${rowData.respuesta}"]`).removeClass('active').addClass('notActive');
                        $(row).find(".radioBtn").find(`a[data-toggle="radioRespuesta${rowData.id}"][respuesta="${rowData.respuesta}"]`).removeClass('notActive').addClass('active');
                        calcularInspeccionesCumplimiento();
                    }

                    $(row).find('.botonAccion').data('accion', rowData.accion);
                    $(row).find('.selectLider').val(rowData.usuario5sId);
                    $(row).find('.inputFechaInspeccion').val(rowData.fechaStr);
                },
                columns: [
                    { data: 'inspeccion', title: 'PREGUNTA', width: '40%' },
                    {
                        width: '5%',
                        title: 'DETECCIÓN', render: function (data, type, row, meta) {
                            return `
                                <div class="text-center">
                                    ${row.idArchivoDeteccion > 0 ?
                                    `<label id="botonDeteccion_${row.id}" for="inputDeteccion_${row.id}" class="btn btn-xs btn-success botonDeteccion"><i class="fa fa-check"></i></label>` :
                                    `<label id="botonDeteccion_${row.id}" for="inputDeteccion_${row.id}" class="btn btn-xs btn-default botonDeteccion"><i class="fa fa-upload"></i></label>`
                                }
                                    <input id="inputDeteccion_${row.id}" type="file" class="inputDeteccion inputDeteccion_${row.id}" accept="application/pdf, image/*">
                                </div>
                            `;
                        }
                    },
                    {
                        width: '20%',
                        title: 'DESCRIPCIÓN', render: function (data, type, row, meta) {
                            return `<input class="form-control inputDescripcion" value="${row.descripcion}">`;
                        }
                    },
                    {
                        width: '10%',
                        title: 'RESPUESTA', render: function (data, type, row, meta) {
                            return `
                                <div class="radioBtn btn-group">
                                    <a class="btn btn-xs btn-success notActive OK" data-toggle="radioRespuesta${row.id}" respuesta="1" style="font-weight: bold;">OK</a>
                                    <a class="btn btn-xs btn-danger notActive NO_OK" data-toggle="radioRespuesta${row.id}" respuesta="2" style="font-weight: bold;">NO OK</a>
                                </div>
                            `;
                        }
                    },
                    {
                        width: '5%',
                        title: 'MEDIDA', render: function (data, type, row, meta) {
                            return `
                                <div class="text-center">
                                    ${row.idArchivoMedida > 0 ?
                                    `<label id="botonMedida_${row.id}" for="inputMedida_${row.id}" class="btn btn-xs btn-success botonMedida"><i class="fa fa-check"></i></label>` :
                                    `<label id="botonMedida_${row.id}" for="inputMedida_${row.id}" class="btn btn-xs btn-default botonMedida"><i class="fa fa-upload"></i></label>`
                                }
                                    <input id="inputMedida_${row.id}" type="file" class="inputMedida inputMedida_${row.id}" accept="application/pdf, image/*">
                                </div>
                            `;
                        }
                    },
                    {
                        width: '5%',
                        title: 'ACCIÓN', render: function (data, type, row, meta) {
                            return `${row.accion != "" && row.accion != null ?
                                `<button class="btn btn-xs btn-success botonAccion"><i class="fa fa-check"></i></button>` :
                                `<button class="btn btn-xs btn-default botonAccion"><i class="fa fa-plus"></i></button>`
                                }`;
                        }
                    },
                    {
                        width: '15%',
                        title: 'FECHA/LÍDER', render: function (data, type, row, meta) {
                            return `
                                <select class="form-control selectLider"></select>
                                <input placeholder="FECHA" class="form-control text-center inputFechaInspeccion" style="margin-top: 4px;">
                            `;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function limpiarModalAuditoria() {
            selectAuditoria.val('');
            selectAuditoria.trigger('change');
            selectProyecto.val('');
            selectProyecto.trigger('change');
            inputArea.val('');
            inputFechaAuditoria.val('');
            selectAuditor.val('');
            selectAuditor.trigger('change');
            celdaPorcentaje.text('');
            celdaEstatusAuditoria.text('');
            celdaEstatus5s.text('');
            textoInspecciones.text('0 inspecciones de 0');
            dtInspecciones.clear().draw(false);
        }

        function guardarAccionRequerida() {
            let accion = textAreaAccionRequerida.val();
            $(_botonAccionSeleccionado).data('accion', accion);

            if (accion) {
                $(_botonAccionSeleccionado).addClass('btn-success');
                $(_botonAccionSeleccionado).removeClass('btn-default');
                $(_botonAccionSeleccionado).find('i').addClass('fa-check');
                $(_botonAccionSeleccionado).find('i').removeClass('fa-plus');
            } else {
                $(_botonAccionSeleccionado).addClass('btn-default');
                $(_botonAccionSeleccionado).removeClass('btn-success');
                $(_botonAccionSeleccionado).find('i').addClass('fa-plus');
                $(_botonAccionSeleccionado).find('i').removeClass('fa-check');
            }
        }

        function cargarAuditorias() {
            axios.post('GetAuditorias', { objParamDTO: { cc: selectCentroCosto.val(), fechaInicio: inputFechaInicio.val(), fechaFinal: inputFechaFinal.val() } }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    AddRows(tablaAuditorias, response.data.lstAuditorias);
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarInformacionAuditoria(datos, listaLideres) {
            selectAuditoria.val(datos.checkListId);
            inputArea.val(datos.area);
            inputFechaAuditoria.val(datos.fechaStr);

            //#region Llenar combos de proyecto y auditor
            selectProyecto.empty();
            axios.post('FillCboProyectos', { objParamDTO: { checkListId: datos.checkListId } }).then(response => {
                let { success, data, message } = response.data;

                if (success) {
                    selectProyecto.append('<option value="">--Seleccione--</option>');

                    response.data.items.forEach((x) => {
                        selectProyecto.append(`<option value="${x.Value}">${x.Text}</option>`);
                    });

                    selectProyecto.val(datos.cc);

                    selectAuditor.empty();
                    axios.post('FillCboAuditores', { objParamDTO: { cc: datos.cc } }).then(response => {
                        let { success, data, message } = response.data;

                        if (success) {
                            selectAuditor.append('<option value="">--Seleccione--</option>');

                            response.data.items.forEach((x) => {
                                selectAuditor.append(`<option value="${x.Value}">${x.Text}</option>`);
                            });

                            selectAuditor.val(datos.usuarioAuditorId);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
                } else {
                    AlertaGeneral(`Alerta`, message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
            //#endregion

            _listaLideres = listaLideres;
            AddRows(tablaInspecciones, datos.lstAuditoriaDet);
        }

        function guardarAuditoria() {
            let objetoDatos = {
                id: _auditoriaID,
                checkListId: +selectAuditoria.val(),
                cc: selectProyecto.val(),
                fecha: inputFechaAuditoria.val(),
                usuarioAuditorId: +selectAuditor.val(),
                auditoriaCompleta: _auditoriaCompleta,
                lstAuditoriaDet: []
            };

            let data = new FormData();
            let error = "";
            let listaIndicesArchivosDeteccion = [];
            let listaIndicesArchivosMedidas = [];
            let contadorIndices = 0;

            tablaInspecciones.find('tbody tr').each(function (index, row) {
                let rowData = dtInspecciones.row(row).data();
                let archivoDeteccion = document.getElementById(`inputDeteccion_${rowData.id}`).files[0];
                let archivoMedida = document.getElementById(`inputMedida_${rowData.id}`).files[0];

                //Se aplican las validaciones cuando se va terminar el guardado.
                if (objetoDatos.auditoriaCompleta == 1) {
                    //#region Validaciones
                    if (!archivoDeteccion && rowData.idArchivoDeteccion <= 0) {
                        error = 'Favor de ingresar una detección (archivo) a cada pregunta';
                        return;
                    }

                    if (!$(row).find('.inputDescripcion').val()) {
                        error = 'Favor de ingresar una descripción a cada pregunta';
                        return;
                    }

                    if (!+$(row).find('.radioBtn a.active').attr('respuesta')) {
                        error = 'Favor de ingresar una respuesta a cada pregunta';
                        return;
                    } else {
                        if (+$(row).find('.radioBtn a.active').attr('respuesta') == 2) {
                            if (!$(row).find('.inputFechaInspeccion').val()) {
                                error = 'Favor de ingresar a una fecha a la pregunta ' + (index + 1);
                                return;
                            }
                        }
                    }

                    if (!+$(row).find('.selectLider').val()) {
                        error = 'Favor de ingresar un lider a cada pregunta';
                        return;
                    }

                    // if (!archivoMedida && rowData.idArchivoMedida <= 0) {
                    //     error = 'Favor de ingresar una medida (archivo) a cada pregunta';
                    //     return;
                    // }

                    // if (!$(row).find('.botonAccion').data('accion')) {
                    //     error = 'Favor de ingresar una acción a cada pregunta';
                    //     return;
                    // }
                    //#endregion
                }

                if (archivoDeteccion) {
                    data.append("lstDetecciones", archivoDeteccion);
                    listaIndicesArchivosDeteccion.push(contadorIndices);
                }

                if (archivoMedida) {
                    data.append("lstMedidas", archivoMedida);
                    listaIndicesArchivosMedidas.push(contadorIndices);
                }

                contadorIndices++;

                objetoDatos.lstAuditoriaDet.push({
                    auditoriaDetId: rowData.auditoriaDetId,
                    inspeccionId: rowData.id,
                    idArchivoDeteccion: rowData.idArchivoDeteccion,
                    idArchivoMedida: rowData.idArchivoMedida,
                    descripcion: $(row).find('.inputDescripcion').val(),
                    respuesta: $(row).find('.radioBtn a.active').length > 0 ? +$(row).find('.radioBtn a.active').attr('respuesta') : 0,
                    accion: $(row).find('.botonAccion').data('accion'),
                    usuario5sId: +$(row).find('.selectLider').val(),
                    fecha: $(row).find('.inputFechaInspeccion').val()
                });
            });

            if (error != "") {
                Alert2Warning(error);
                return null;
            }

            data.append("objParamDTO", JSON.stringify(objetoDatos));
            data.append('lstIndice_Detecciones', JSON.stringify(listaIndicesArchivosDeteccion));
            data.append('lstIndice_Medidas', JSON.stringify(listaIndicesArchivosMedidas));

            axios.post('CrearEditarAuditoria', data, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, datos, message } = response.data;
                if (success) {
                    if (objetoDatos.auditoriaCompleta) {
                        $("#report").attr('src', `/Reportes/Vista.aspx?idReporte=285&idAudi=${response.data.items.idAudi}&checkListId=${response.data.items.checkListId}&ccs=${response.data.items.ccs}&auditor=${response.data.items.auditor}&pdf=1`);
                        document.getElementById('report').onload = function () {
                            $.blockUI({ message: 'Procesando...' });
                            $.post('NotificarAuditoria',
                                {
                                    idAuditoria: response.data.items.idAudi
                                }).then(response2 => {
                                    $.unblockUI;
                                    if (response2.success) {
                                        swal('Confirmacion', 'Se creo la auditoria', 'success');
                                    } else {
                                        swal("¡Alerta!", response2.message + ' Se guardo la auditoria pero no se envio la notificacion', "warning");
                                    }
                                    modalAuditoria.modal('hide');
                                    botonBuscar.click();
                                }, error => {
                                    $.unblockUI;
                                    modalAuditoria.modal('hide');
                                    botonBuscar.click();
                                    swal('Alerta!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}. Se guardo la auditoria pero no se envio la notificacion`, 'error');
                                });
                        }
                    }
                } else {
                    Alert2Error(message)
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function calcularInspeccionesCumplimiento() {
            if (dtInspecciones.rows().count() > 0) {
                let contadorInspeccionCumplida = 0;
                let cantidadTotalPuntos = dtInspecciones.data().toArray().map((x) => x.cantPuntos).reduce(
                    (accumulator, currentValue) => accumulator + currentValue, 0
                );
                let cantidadCumplida = 0;

                tablaInspecciones.find('tbody tr').each(function (index, row) {
                    let rowData = dtInspecciones.row(row).data();
                    let respuesta = +$(row).find('.radioBtn a.active').attr('respuesta');

                    if (respuesta == 1 || respuesta == 2) {
                        contadorInspeccionCumplida++;

                        if (respuesta == 1) {
                            cantidadCumplida += rowData.cantPuntos;
                        }
                    }
                });

                let cumplimiento = parseFloat((cantidadCumplida * 100 / (cantidadTotalPuntos > 0 ? cantidadTotalPuntos : 1)).toFixed(2));
                celdaPorcentaje.text(cumplimiento + '%');

                //#region Estatus Auditoría
                if (cumplimiento >= 85) {
                    celdaEstatusAuditoria.text('AUDITORÍA APROBADA');
                } else {
                    celdaEstatusAuditoria.text('AUDITORÍA RECHAZADA');
                }
                //#endregion

                //#region Estatus 5's
                if (cumplimiento == 100) {
                    celdaEstatus5s.text(`IMPLEMENTACIÓN DE 5'S NIVEL DE EXCELENCIA`);
                    celdaEstatus5s.css('background-color', 'green');
                    celdaEstatus5s.css('color', 'white');
                } else if (cumplimiento >= 85 && cumplimiento <= 99.99) {
                    celdaEstatus5s.text(`5'S REQUIERE REFORZAMIENTO CONTINUO`);
                    celdaEstatus5s.css('background-color', 'orange');
                    celdaEstatus5s.css('color', 'black');
                } else if (cumplimiento >= 0 && cumplimiento <= 84.99) {
                    celdaEstatus5s.text(`5'S NIVEL CRÍTICO REQUIERE ACCIONES INMEDIATAS`);
                    celdaEstatus5s.css('background-color', 'red');
                    celdaEstatus5s.css('color', 'white');
                }
                //#endregion

                textoInspecciones.text(`${contadorInspeccionCumplida} inspecciones de ${dtInspecciones.rows().count()}`);
            }
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }

    $(document).ready(() => CincoS.Auditorias5s = new Auditorias5s()).ajaxStart(() => $.blockUI({ message: 'Procesando...' })).ajaxStop($.unblockUI);
})();