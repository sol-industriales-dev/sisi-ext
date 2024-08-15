(() => {
    $.namespace('CH.Historico');

    //#region CONST FILTROS
    const cboFiltro_Estatus = $("#cboFiltro_Estatus")
    const cboFiltro_CC = $("#cboFiltro_CC")
    const cboFiltro_Semaforo = $("#cboFiltro_Semaforo")
    const btnFiltroBuscar = $("#btnFiltroBuscar")
    //#endregion

    //#region CONST HISTORICO
    const lblCantDemandasSemaforoRojo = $("#lblCantDemandasSemaforoRojo")
    const lblCantDemandasSemaforoAmbar = $("#lblCantDemandasSemaforoAmbar")
    const lblCantDemandasSemaforoVerde = $("#lblCantDemandasSemaforoVerde")
    const lblCantDemandasActivas = $("#lblCantDemandasActivas")
    const lblCantDemandasCerradas = $("#lblCantDemandasCerradas")
    const tblHistorico = $('#tblHistorico')
    let dtHistorico
    //#endregion

    //#region CONST CAPTURA
    const mdlCECaptura = $('#mdlCECaptura')
    const txtCE_ClaveEmpleado = $("#txtCE_ClaveEmpleado")
    const txtCE_NombreEmpleado = $("#txtCE_NombreEmpleado")
    const txtCE_Puesto = $("#txtCE_Puesto")
    const chkCE_EmpleadoCP = $("#chkCE_EmpleadoCP")
    const txtCE_CC = $("#txtCE_CC")
    const txtCE_FechaIngreso = $("#txtCE_FechaIngreso")
    const txtCE_FechaBaja = $("#txtCE_FechaBaja")
    const txtCE_MotivoSalida = $("#txtCE_MotivoSalida")
    const txtCE_SueldoDiario = $("#txtCE_SueldoDiario")
    const txtCE_Antiguedad = $("#txtCE_Antiguedad")
    const txtCE_FechaRecibioDemanda = $("#txtCE_FechaRecibioDemanda")
    const txtCE_FechaDemanda = $("#txtCE_FechaDemanda")
    const txtCE_NumExpediente = $("#txtCE_NumExpediente")
    const cboCE_Juzgado = $("#cboCE_Juzgado")
    const cboCE_TipoDemanda = $("#cboCE_TipoDemanda")
    const cboCE_EstadoDemanda = $("#cboCE_EstadoDemanda")
    const txtCE_AQuienDemanda = $("#txtCE_AQuienDemanda")
    const txtCE_SalarioDiario = $("#txtCE_SalarioDiario")
    const txtCE_OfertaInicial = $("#txtCE_OfertaInicial")
    const txtCE_Hechos = $("#txtCE_Hechos")
    const txtCE_Peticiones = $("#txtCE_Peticiones")
    const txtCE_EstadoActual = $("#txtCE_EstadoActual")
    const txtCE_FechaAudencia = $("#txtCE_FechaAudencia")
    const cboCE_Semaforo = $("#cboCE_Semaforo")
    const txtCE_AbogadoDemandante = $("#txtCE_AbogadoDemandante")
    const txtCE_CuantiaTotal = $("#txtCE_CuantiaTotal")
    const txtCE_NegociadoCerrado = $("#txtCE_NegociadoCerrado")
    const txtCE_Finiquito = $("#txtCE_Finiquito")
    const txtCE_Diferencia = $("#txtCE_Diferencia")
    const btnCE_PorcentajeExpediente = $('#btnCE_PorcentajeExpediente')
    const btnCE_PorcentajeFiniquito = $("#btnCE_PorcentajeFiniquito")
    const divSemaforo = $('#divSemaforo')
    const txtCE_ResolucionLaudo = $('#txtCE_ResolucionLaudo')
    const txtCE_ComentarioFechaAudiencia = $('#txtCE_ComentarioFechaAudiencia')
    //#endregion

    //#region CONST SEGUIMIENTOS
    const mdlSeguimientos = $("#mdlSeguimientos")
    const tblSeguimientos = $("#tblSeguimientos")
    //#endregion

    //#region CONST CREAR ARCHIVOS ADJUNTOS
    const mdlCEArchivoAdjuntos = $("#mdlCEArchivoAdjuntos")
    const tblArchivosAdjuntos = $("#tblArchivosAdjuntos")
    let dtArchivosAdjuntos;
    //#endregion

    Historico = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblHistorico()
            fncFillCombos()
            fncGetHistorico()
            initTblSeguimientos()
            initTblArchivosAdjuntos()
            //#endregion

            //#region FUNCIONES FILTROS
            btnFiltroBuscar.click(function () {
                fncGetHistorico()
            })
            //#endregion

            //#region FUNCIONES INFORMACIÓN DEMANDA
            chkCE_EmpleadoCP.click(function () {
                return false
            })

            cboCE_Semaforo.change(function () {
                switch ($(this).val()) {
                    case "1":
                        divSemaforo.html(`<i class="fas fa-circle" style="font-size: inherit; color: green;"></i>`)
                        break;
                    case "2":
                        divSemaforo.html(`<i class="fas fa-circle" style="font-size: inherit; color: yellow;"></i>`)
                        break;
                    case "3":
                        divSemaforo.html(`<i class="fas fa-circle" style="font-size: inherit; color: red;"></i>`)
                        break;
                    default:
                        divSemaforo.html(`<i class="fas fa-circle" style="font-size: inherit; color: #eeeeee;"></i>`)
                        break;
                }
            })
            //#endregion
        }

        //#region FUNCIONES HISTORICO
        function initTblHistorico() {
            dtHistorico = tblHistorico.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    {
                        data: 'estatusDemanda', title: 'Estatus',
                        render: function (data, type, row) {
                            if (data == "Abierto") {
                                return `<i class="fas fa-unlock" style="font-size: xx-large; color: green;" title="Demanda abierta."></i>`
                            } else if (data == "Cerrado") {
                                return `<i class="fas fa-lock" style="font-size: xx-large; color: red;" title="Demanda cerrada."></i>`
                            }
                        }
                    },
                    {
                        data: 'claveEmpleado', title: 'Clave empleado',
                        render: function (data, type, row) {
                            if (row.claveEmpleado > 0) {
                                return row.claveEmpleado
                            } else {
                                return "-"
                            }
                        }
                    },
                    { data: 'nombreDemandante', title: 'Actor' },
                    { data: 'cc', title: 'CC' },
                    { data: 'demandado', title: 'Demandado' },
                    { data: 'estadoActual', title: 'Estado actual' },
                    {
                        data: 'fechaAudiencia', title: 'Fecha de audiencia',
                        render: function (data, type, row) {
                            if (row.fechaAudiencia != null) {
                                return moment(row.fechaAudiencia).format('DD/MM/YYYY')
                            } else {
                                return "-";
                            }
                        }
                    },
                    {
                        title: 'Cuantia total',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.cuantiaTotal)
                        }
                    },
                    {
                        title: 'Semaforo',
                        render: function (data, type, row) {
                            let semaforo = "";
                            switch (row.strSemaforo) {
                                case "Rojo":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: red;"></i>`
                                    break;
                                case "Ambar":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: yellow;"></i>`
                                    break;
                                case "Verde":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: green;"></i>`
                                    break;
                            }
                            return semaforo;
                        }
                    },
                    { data: 'abogadoDemandante', title: 'Abogado demandante', visible: false },
                    {
                        title: 'Negociación cerrada',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.negociadoCerrado)
                        }
                    },
                    {
                        title: 'Diferencia',
                        render: function (data, type, row) {
                            return `<button class="btn btn-${row.colorDiferencia} form-control" style="color: #fff;">${maskNumero2DCompras(row.diferencia)}</button>`
                        }
                    },
                    {
                        title: 'Archivos',
                        render: function (data, type, row, meta) {
                            let btnInfoDemanda = `<button class='btn btn-xs btn-primary btnInfoDemanda' title='Información de la demanda.'><i class="fas fa-stream"></i></button>`
                            let btnSeguimientos = `<button class='btn btn-xs btn-primary btnSeguimientos' title='Listado de seguimientos registrados.'><i class="far fa-folder-open"></i></button>`
                            let btnArchivosAdjuntos = `<button class='btn btn-xs btn-primary btnArchivosAdjuntos' title='Listado de archivos adjuntos cargados.'><i class="fas fa-archive"></i></button>`
                            return `${btnInfoDemanda} ${btnSeguimientos} ${btnArchivosAdjuntos}`
                        },
                    },
                ],
                initComplete: function (settings, json) {
                    tblHistorico.on('click', '.btnInfoDemanda', function () {
                        let rowData = dtHistorico.row($(this).closest('tr')).data()
                        fncDisabledEnableCECaptura(true)
                        fncGetDatosActualizarCaptura(rowData.id)
                    })

                    tblHistorico.on("click", ".btnSeguimientos", function () {
                        let rowData = dtHistorico.row($(this).closest("tr")).data()
                        fncGetLstSeguimientos(rowData.id)
                    })

                    tblHistorico.on("click", ".btnArchivosAdjuntos", function () {
                        let rowData = dtHistorico.row($(this).closest("tr")).data()
                        fncGetArchivosAdjuntos(rowData.id)
                        mdlCEArchivoAdjuntos.modal("show")
                    })
                },
                columnDefs: [
                    { className: 'dt-center', targets: [0, 1, 2, 3, 4, 5, 6, 8, 11] },
                    { className: 'dt-body-center', targets: [0, 1, 2, 3, 4, 5, 6, 8, 11] },
                    { className: 'dt-body-right', targets: [7, 9, 10] },
                    { width: '5%', targets: [0, 1, 6, 7, 8, 9, 10] },
                    { width: '8%', targets: [11] },
                ],
            });
        }

        function fncGetHistorico() {
            let obj = new Object()
            obj.esDemandaCerrada = cboFiltro_Estatus.val()
            obj.cc = cboFiltro_CC.val()
            obj.semaforo = cboFiltro_Semaforo.val()
            axios.post('GetHistorico', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtHistorico.clear();
                    dtHistorico.rows.add(response.data.lstHistoricoDTO);
                    dtHistorico.draw();

                    lblCantDemandasSemaforoRojo.html(response.data.objCantDemandasSemaforo.cantDemandasSemaforoRojo)
                    lblCantDemandasSemaforoAmbar.html(response.data.objCantDemandasSemaforo.cantDemandasSemaforoAmbar)
                    lblCantDemandasSemaforoVerde.html(response.data.objCantDemandasSemaforo.cantDemandasSemaforoVerde)

                    lblCantDemandasActivas.html(`Activas: ${response.data.objCantDemandasActivasCerradas.cantDemandasActivas}`)
                    lblCantDemandasCerradas.html(`Cerradas: ${response.data.objCantDemandasActivasCerradas.cantDemandasCerradas}`)
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDatosActualizarCaptura(idCaptura) {
            if (idCaptura > 0) {
                let obj = new Object()
                obj.id = idCaptura
                axios.post('GetDatosActualizarCaptura', obj).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        fncLimpiarMdlCECaptura()
                        txtCE_ClaveEmpleado.val(response.data.objCaptura.claveEmpleado)
                        txtCE_NombreEmpleado.val(response.data.objCaptura.nombreDemandante)
                        txtCE_Puesto.val(response.data.objCaptura.puesto)
                        chkCE_EmpleadoCP.prop("checked", response.data.objCaptura.esEmpleadoCP)
                        txtCE_CC.val(response.data.objCaptura.cc)
                        txtCE_FechaIngreso.val(moment(response.data.objCaptura.fechaIngreso).format('YYYY-MM-DD'))
                        txtCE_FechaBaja.val(moment(response.data.objCaptura.fechaBaja).format('YYYY-MM-DD'))
                        txtCE_MotivoSalida.val(response.data.objCaptura.motivoSalida)
                        txtCE_SueldoDiario.val(maskNumero2DCompras(response.data.objCaptura.sueldoDiario))
                        txtCE_OfertaInicial.val(maskNumero2DCompras(response.data.objCaptura.ofertaInicial))
                        txtCE_Antiguedad.val(response.data.objCaptura.antiguedad)
                        txtCE_FechaRecibioDemanda.val(moment(response.data.objCaptura.fechaRecibioDemanda).format('YYYY-MM-DD'))
                        txtCE_FechaDemanda.val(moment(response.data.objCaptura.fechaDemanda).format('YYYY-MM-DD'))
                        txtCE_NumExpediente.val(response.data.objCaptura.numExpediente)
                        cboCE_Juzgado.val(response.data.objCaptura.FK_Juzgado)
                        cboCE_Juzgado.trigger("change")
                        cboCE_TipoDemanda.val(response.data.objCaptura.FK_TipoDemanda)
                        cboCE_TipoDemanda.trigger("change")
                        cboCE_EstadoDemanda.val(response.data.objCaptura.FK_Estado)
                        cboCE_EstadoDemanda.trigger("change")
                        txtCE_AQuienDemanda.val(response.data.objCaptura.demandado)
                        txtCE_SalarioDiario.val(maskNumero2DCompras(response.data.objCaptura.salarioDiario))
                        txtCE_Hechos.val(response.data.objCaptura.hechos)
                        txtCE_Peticiones.val(response.data.objCaptura.peticiones)
                        txtCE_EstadoActual.val(response.data.objCaptura.estadoActual)
                        txtCE_FechaAudencia.val(moment(response.data.objCaptura.fechaAudiencia).format('YYYY-MM-DD'))
                        txtCE_AbogadoDemandante.val(response.data.objCaptura.abogadoDemandante)
                        txtCE_CuantiaTotal.val(maskNumero2DCompras(response.data.objCaptura.cuantiaTotal))
                        txtCE_NegociadoCerrado.val(maskNumero2DCompras(response.data.objCaptura.negociadoCerrado))
                        txtCE_Finiquito.val(maskNumero2DCompras(response.data.objCaptura.finiquitoAl100))
                        txtCE_Diferencia.val(maskNumero2DCompras(response.data.objCaptura.diferencia))
                        txtCE_ResolucionLaudo.val(response.data.objCaptura.resolucionLaudo)
                        cboCE_Semaforo.val(response.data.objCaptura.semaforo)
                        cboCE_Semaforo.trigger("change")

                        let PorcentajeExpediente = response.data.objCaptura.porcentajeExpediente
                        let PorcentajeExpedienteInput =
                            `<div div class="progress">
                                <div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="min-width: 4em; width: ${PorcentajeExpediente}%; background-color: green;">
                                    ${maskNumero2D(PorcentajeExpediente)}%
                                </div>
                             </div>`
                        btnCE_PorcentajeExpediente.html(PorcentajeExpedienteInput);

                        let PorcentajeFiniquito = response.data.objCaptura.porcentajeFiniquito
                        let PorcentajeFiniquitoInput =
                            `<div div class="progress">
                                <div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" 
                                    style="min-width: 4em; width: ${PorcentajeFiniquito}%; background-color: ${response.data.objCaptura.colorPorcentajeFiniquito};">
                                    ${maskNumero2D(PorcentajeFiniquito)}%
                                </div>
                             </div>`
                        btnCE_PorcentajeFiniquito.html(PorcentajeFiniquitoInput);

                        mdlCECaptura.modal("show")
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            } else {
                Alert2Error("Ocurrió un error al obtener la información de la captura.")
            }
        }
        //#endregion

        //#region FUNCIONES SEGUIMIENTOS
        function initTblSeguimientos() {
            dtSeguimientos = tblSeguimientos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'numConsecutivo', title: '#' },
                    {
                        title: 'Fecha registro documento',
                        render: function (data, type, row) {
                            return moment(row.fechaCreacion).format('LLL')
                        }
                    },
                    {
                        data: 'cuantia', title: 'Cuantia',
                        render: function (data, type, row) {
                            return maskNumero2DCompras(row.cuantia)
                        }
                    },
                    { data: 'abogadoDemandante', title: 'Abogado demandante' },
                    {
                        title: 'Fecha audiencia',
                        render: function (data, type, row) {
                            if (row.fechaAudiencia != null) {
                                return moment(row.fechaAudiencia).format('DD/MM/YYYY')
                            } else {
                                return `-`
                            }
                        }
                    },
                    {
                        title: 'Semaforo',
                        render: function (data, type, row) {
                            let semaforo = "";
                            switch (row.strSemaforo) {
                                case "Rojo":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: red;"></i>`
                                    break;
                                case "Ambar":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: yellow;"></i>`
                                    break;
                                case "Verde":
                                    semaforo = `<i class="fas fa-circle" style="font-size: xx-large; color: green;"></i>`
                                    break;
                                default:
                                    semaforo = "-"
                            }
                            return semaforo;
                        }
                    },
                    {
                        data: 'estadoActual', title: 'Estado actual',
                        render: function (data, type, row) {
                            if (row.estadoActual != null) {
                                return row.estadoActual
                            } else {
                                return "-"
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncGetLstSeguimientos(FK_Captura) {
            if (FK_Captura > 0) {
                let obj = {}
                obj.FK_Captura = FK_Captura
                axios.post('GetLstSeguimientos', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtSeguimientos.clear();
                        dtSeguimientos.rows.add(response.data.lstSeguimientosDTO);
                        dtSeguimientos.draw();
                        mdlSeguimientos.modal("show")
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener el seguimiento.")
            }
        }
        //#endregion

        //#region FUNCIONES ARCHIVOS ADJUNTOS
        function initTblArchivosAdjuntos() {
            dtArchivosAdjuntos = tblArchivosAdjuntos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'archivo', title: 'Archivo' },
                    { data: 'strTipoArchivo', title: 'Tipo archivo' },
                    {
                        render: function (data, type, row, meta) {
                            let btnVisualizar = `<button class='btn btn-xs btn-primary visualizarArchivo' title='Visualizar archivo.'><i class="fas fa-book-reader"></i></button>`
                            let btnDescargar = `<button class='btn btn-xs btn-primary descargarArchivo' title='Descargar archivo.'><i class="fas fa-file-download"></i></button>`
                            return `${btnVisualizar} ${btnDescargar}`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblArchivosAdjuntos.on('click', '.eliminarRegistro', function () {
                        let rowData = dtArchivosAdjuntos.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarArchivoAdjunto(rowData.id));
                    });

                    tblArchivosAdjuntos.on('click', '.visualizarArchivo', function () {
                        let rowData = dtArchivosAdjuntos.row($(this).closest('tr')).data();
                        fncVisualizarArchivoAdjunto(rowData.id)
                    });

                    tblArchivosAdjuntos.on('click', '.descargarArchivo', function () {
                        let rowData = dtArchivosAdjuntos.row($(this).closest('tr')).data();
                        fncDescargarArchivoAdjunto(rowData.rutaArchivo)
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    //{ width: '5%', targets: [0] }
                ],
            });
        }

        function fncDescargarArchivoAdjunto(rutaArchivo) {
            if (rutaArchivo != "") {
                document.location.href = `/Administrativo/DemandaCH/DescargarArchivoAdjunto?rutaArchivo=${rutaArchivo}`
            }
        }

        function fncVisualizarArchivoAdjunto(idArchivo) {
            if (idArchivo > 0) {
                let obj = new Object();
                obj.idArchivo = idArchivo
                axios.post("VisualizarArchivoAdjunto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        $('#myModal').data().ruta = null;
                        $('#myModal').modal('show');
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncGetArchivosAdjuntos(FK_Captura) {
            if (FK_Captura > 0) {
                let obj = {};
                obj.FK_Captura = FK_Captura
                axios.post('GetArchivosAdjuntos', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtArchivosAdjuntos.clear();
                        dtArchivosAdjuntos.rows.add(response.data.lstArchivos);
                        dtArchivosAdjuntos.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener el listado de archivos.")
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarMdlCECaptura() {
            $("input[type='text']").val("");
            chkCE_EmpleadoCP.prop("checked", false)
            cboCE_Juzgado[0].selectedIndex = 0
            cboCE_Juzgado.trigger("change")
            cboCE_TipoDemanda[0].selectedIndex = 0
            cboCE_TipoDemanda.trigger("change")
            cboCE_EstadoDemanda[0].selectedIndex = 0
            cboCE_EstadoDemanda.trigger("change")
            cboCE_Semaforo[0].selectedIndex = 0
            cboCE_Semaforo.trigger("change")
        }

        function fncFillCombos() {
            cboFiltro_Estatus.fillCombo('/Administrativo/DemandaCH/FillCboEstatus', null, false, null)
            cboFiltro_CC.fillCombo('/Administrativo/DemandaCH/FillCboCC', null, false, null)
            cboFiltro_Semaforo.fillCombo('/Administrativo/DemandaCH/FillCboSemaforo', null, false, null)
            cboCE_EstadoDemanda.fillCombo('/Administrativo/DemandaCH/FillCboEstados', null, false, null)
            cboCE_Juzgado.fillCombo('/Administrativo/DemandaCH/FillCboJuzgados', null, false, null)
            cboCE_Semaforo.fillCombo('/Administrativo/DemandaCH/FillCboSemaforo', null, false, null)
            cboCE_TipoDemanda.fillCombo('/Administrativo/DemandaCH/FillCboTipoDemandas', null, false, null)
            $(".select2").select2()
        }

        function fncDisabledEnableCECaptura(disabled) {
            txtCE_ClaveEmpleado.attr("disabled", disabled)
            txtCE_NombreEmpleado.attr("disabled", disabled)
            txtCE_Puesto.attr("disabled", disabled)
            chkCE_EmpleadoCP.attr("disabled", disabled)
            txtCE_FechaIngreso.attr("disabled", disabled)
            txtCE_FechaBaja.attr("disabled", disabled)
            txtCE_MotivoSalida.attr("disabled", disabled)
            txtCE_SueldoDiario.attr("disabled", disabled)
            txtCE_Antiguedad.attr("disabled", disabled)
            txtCE_FechaRecibioDemanda.attr("disabled", disabled)
            txtCE_FechaDemanda.attr("disabled", disabled)
            txtCE_NumExpediente.attr("disabled", disabled)
            cboCE_Juzgado.attr("disabled", disabled)
            cboCE_TipoDemanda.attr("disabled", disabled)
            cboCE_EstadoDemanda.attr("disabled", disabled)
            txtCE_AQuienDemanda.attr("disabled", disabled)
            txtCE_SalarioDiario.attr("disabled", disabled)
            txtCE_OfertaInicial.attr("disabled", disabled)
            txtCE_EstadoActual.attr("disabled", disabled)
            txtCE_FechaAudencia.attr("disabled", disabled)
            cboCE_Semaforo.attr("disabled", disabled)
            txtCE_AbogadoDemandante.attr("disabled", disabled)
            txtCE_CuantiaTotal.attr("disabled", disabled)
            txtCE_NegociadoCerrado.attr("disabled", disabled)
            txtCE_Finiquito.attr("disabled", disabled)
            txtCE_Diferencia.attr("disabled", disabled)
            txtCE_CC.attr("disabled", disabled)
            txtCE_ResolucionLaudo.attr("disabled", disabled)
            txtCE_Hechos.attr("disabled", disabled)
            txtCE_Peticiones.attr("disabled", disabled)
            txtCE_ComentarioFechaAudiencia.attr("disabled", disabled)
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Historico = new Historico();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();