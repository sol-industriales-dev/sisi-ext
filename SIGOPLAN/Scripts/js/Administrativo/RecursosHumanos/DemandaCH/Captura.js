(() => {
    $.namespace('CH.Captura')

    //#region CONST FILTROS
    const btnFiltroBuscar = $("#btnFiltroBuscar")
    const btnFiltroNuevo = $("#btnFiltroNuevo")
    //#endregion

    //#region CONST TITLE VISTA
    const tipoVista = $('#tipoVista')
    //#endregion

    //#region CONST DATATABLE
    const tblCapturas = $('#tblCapturas')
    let dtCapturas
    //#endregion

    //#region CONST CREAR/EDITAR CAPTURA
    const mdlCECaptura = $('#mdlCECaptura')
    const txtCE_ClaveEmpleado = $("#txtCE_ClaveEmpleado")
    const txtCE_NombreEmpleado = $("#txtCE_NombreEmpleado")
    const cboCE_NombreEmpleado = $('#cboCE_NombreEmpleado')
    const divOcultarMostrarCboCE_NombreEmpleado = $('#divOcultarMostrarCboCE_NombreEmpleado')
    const txtCE_Puesto = $("#txtCE_Puesto")
    const chkCE_EmpleadoCP = $("#chkCE_EmpleadoCP")
    const cboCE_CC = $("#cboCE_CC")
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
    const txtCE_ComentarioFechaAudiencia = $('#txtCE_ComentarioFechaAudiencia')
    const cboCE_Semaforo = $("#cboCE_Semaforo")
    const txtCE_AbogadoDemandante = $("#txtCE_AbogadoDemandante")
    const txtCE_CuantiaTotal = $("#txtCE_CuantiaTotal")
    const txtCE_NegociadoCerrado = $("#txtCE_NegociadoCerrado")
    const txtCE_Finiquito = $("#txtCE_Finiquito")
    const txtCE_Diferencia = $("#txtCE_Diferencia")
    const divDocumentosAdjuntos = $('#divDocumentosAdjuntos')
    const btnCE_Finiquito = $("#btnCE_Finiquito")
    const btnCE_DocumentoLegal = $("#btnCE_DocumentoLegal")
    const btnCE_PorcentajeExpediente = $('#btnCE_PorcentajeExpediente')
    const btnCE_PorcentajeFiniquito = $("#btnCE_PorcentajeFiniquito")
    const btnCE_PorcentajeDocumentoLegal = $("#btnCE_PorcentajeDocumentoLegal")
    const btnCECaptura = $("#btnCECaptura")
    const btnCerrarDemanda = $('#btnCerrarDemanda')
    const div_cboCE_CC = $("#div_cboCE_CC")
    const div_txtCE_CC = $("#div_txtCE_CC")
    const txtCE_CC = $('#txtCE_CC')
    const divSemaforo = $('#divSemaforo')
    const txtCE_ResolucionLaudo = $('#txtCE_ResolucionLaudo')
    //#endregion

    //#region CONST CREAR SEGUIMIENTO
    const divSeguimiento = $("#divSeguimiento")
    const txtCE_Seguimiento_Cuantia = $("#txtCE_Seguimiento_Cuantia")
    const txtCE_Seguimiento_AbogadoDemandante = $("#txtCE_Seguimiento_AbogadoDemandante")
    const txtCE_Seguimiento_FechaAudiencia = $("#txtCE_Seguimiento_FechaAudiencia")
    const cboCE_Seguimiento_Semaforo = $("#cboCE_Seguimiento_Semaforo")
    const txtCE_Seguimiento_EstadoActual = $("#txtCE_Seguimiento_EstadoActual")
    const btNuevo_Seguimiento = $('#btNuevo_Seguimiento')
    const btnCESeguimiento = $("#btnCESeguimiento")
    //#endregion

    //#region CONST CREAR ARCHIVOS ADJUNTOS
    const mdlCEArchivoAdjuntos = $("#mdlCEArchivoAdjuntos")
    const cboCE_TipoArchivo = $('#cboCE_TipoArchivo')
    const btnExaminarArchivo = $('#btnExaminarArchivo')
    const btnCEArchivoAdjunto = $("#btnCEArchivoAdjunto")
    const tblArchivosAdjuntos = $("#tblArchivosAdjuntos")
    const txtArchivoAdjunto = $('#txtArchivoAdjunto')
    const lblFileName = $('#lblFileName')
    let dtArchivosAdjuntos;
    //#endregion

    //#region CONST ESTATUS FINIQUITOS
    const mdlEstatusFiniquito = $('#mdlEstatusFiniquito')
    const tblEstatusFiniquito = $('#tblEstatusFiniquito')
    let dtEstatusFiniquito
    //#endregion

    Captura = function () {
        (function init() {
            fncListeners()
        })()

        function fncListeners() {
            //#region INIT
            fncObtenerURLParams();
            initTblCapturas()
            initTblArchivosAdjuntos()
            fncGetCapturas()
            fncFillCombos()
            initTblEstatusFiniquito()
            //#endregion

            //#region FUNCIONES FILTROS
            btnFiltroBuscar.click(function () {
                fncGetCapturas()
            })

            btnFiltroNuevo.click(function () {
                fncLimpiarMdlCECaptura()
                setInputFormatoMoneda()
                fncMostrarPorcentajeDocumentos(false)
                fncDisabledEnableCECaptura(false)

                fncDefaultCtrls("txtCE_NombreEmpleado")
                fncDefaultCtrls("txtCE_FechaRecibioDemanda")
                fncDefaultCtrls("txtCE_FechaDemanda")
                fncDefaultCtrls("txtCE_NumExpediente")
                fncDefaultCtrls("select2-cboCE_Juzgado-container")
                fncDefaultCtrls("select2-cboCE_TipoDemanda-container")
                fncDefaultCtrls("select2-cboCE_EstadoDemanda-container")
                fncDefaultCtrls("txtCE_AQuienDemanda")
                fncDefaultCtrls("txtCE_EstadoActual")
                fncDefaultCtrls("txtCE_FechaAudencia")
                fncDefaultCtrls("txtCE_AbogadoDemandante")
                fncDefaultCtrls("select2-cboCE_Semaforo-container")

                fncOcultarMostrarCboCE_Empleados(false)
                chkCE_EmpleadoCP.trigger("click")
                div_txtCE_CC.css("display", "none")

                btnCECaptura.data().id = 0
                btnCECaptura.html("<i class='fas fa-save'></i>&nbsp;Guardar")
                mdlCECaptura.modal("show")
            })
            //#endregion

            //#region FUNCIONES CAPTURA
            btnCECaptura.click(function () {
                fncCECaptura()
            })

            txtCE_ClaveEmpleado.change(function () {
                fncGetInformacionEmpleado()
            })

            cboCE_NombreEmpleado.change(function () {
                if ($(this).val() > 0 && txtCE_ClaveEmpleado.val() <= 0) {
                    txtCE_ClaveEmpleado.val($(this).val())
                    txtCE_ClaveEmpleado.trigger("change")
                }
            })

            btnCE_DocumentoLegal.click(function () {
                fncGetArchivosAdjuntos()
                txtArchivoAdjunto.val("")
                lblFileName.text("")
                cboCE_TipoArchivo[0].selectedIndex = 0
                cboCE_TipoArchivo.trigger("change")
                btnCEArchivoAdjunto.html("<i class='fas fa-save'></i>&nbsp;Guardar")
                mdlCEArchivoAdjuntos.modal("show")
            })

            btnCerrarDemanda.click(function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea indicar como cerrada la demanda?', 'Confirmar', 'Cancelar', () => fncCerrarDemanda());
            })

            chkCE_EmpleadoCP.click(function () {
                fncOcultarMostrarCboCE_Empleados($(this).prop("checked"))
            })

            txtCE_SueldoDiario.on("change", function () {
                let valor = fncSetMaskNumero2D($(this).val())
                $(this).val(valor)
            })

            txtCE_SalarioDiario.on("change", function () {
                let valor = fncSetMaskNumero2D($(this).val())
                $(this).val(valor)
            })

            txtCE_OfertaInicial.on("change", function () {
                let valor = fncSetMaskNumero2D($(this).val())
                $(this).val(valor)
            })

            txtCE_CuantiaTotal.on("change", function () {
                let valor = fncSetMaskNumero2D($(this).val())
                $(this).val(valor)
            })

            txtCE_NegociadoCerrado.on("change", function () {
                let valor = fncSetMaskNumero2D($(this).val())
                $(this).val(valor)
            })

            txtCE_Finiquito.on("change", function () {
                let valor = fncSetMaskNumero2D($(this).val())
                $(this).val(valor)
            })

            txtCE_Diferencia.on("change", function () {
                let valor = fncSetMaskNumero2D($(this).val())
                $(this).val(valor)
            })

            txtCE_NegociadoCerrado.on("keyup", function () {
                let diferencia = fncGetDiferencia(txtCE_NegociadoCerrado.val(), txtCE_CuantiaTotal.val())
                txtCE_Diferencia.val(diferencia)
            })

            txtCE_CuantiaTotal.on("keyup", function () {
                let diferencia = fncGetDiferencia(txtCE_NegociadoCerrado.val(), txtCE_CuantiaTotal.val())
                txtCE_Diferencia.val(diferencia)
            })

            txtCE_Finiquito.on("keyup", function () {
                let diferencia = fncGetDiferencia(txtCE_NegociadoCerrado.val(), txtCE_Finiquito.val())
                txtCE_Diferencia.val(diferencia)
            })

            cboCE_CC.change(function () {
                if ($(this).val() == -1) {
                    div_txtCE_CC.css("display", "inline")
                    div_cboCE_CC.css("display", "none")
                }
            })

            $(".btn_txtCE_CC_Cancelar").click(function () {
                div_txtCE_CC.css("display", "none")
                div_cboCE_CC.css("display", "inline")
                cboCE_CC[0].selectedIndex = 0
                cboCE_CC.trigger("change")
                txtCE_CC.val("")
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

            //#region FUNCIONES SEGUIMIENTOS
            btNuevo_Seguimiento.click(function () {
                divSeguimiento.css("display", "block")
                btnCESeguimiento.css("display", "inline")
            })

            btnCESeguimiento.click(function () {
                fncCrearSeguimiento()
            })

            btnCE_Finiquito.click(function () {
                fncGetEstatusFiniquitoEmpleadoDemanda()
                mdlEstatusFiniquito.modal("show")
            })
            //#endregion

            //#region FUNCIONES ARCHIVOS ADJUNTOS
            btnCEArchivoAdjunto.click(function () {
                fncGuardarArchivoAdjunto()
            })

            btnExaminarArchivo.click(function () {
                txtArchivoAdjunto.click()
            })

            txtArchivoAdjunto.change(function () {
                lblFileName.text(` ${$(this)[0].files[0].name}`);
            })
            //#endregion
        }

        //#region FUNCIONES CAPTURA
        function initTblCapturas() {
            dtCapturas = tblCapturas.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
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
                    {
                        title: '¿Fue empleado?',
                        render: function (data, type, row, meta) {
                            if (row.esEmpleadoCP) {
                                return `<input type="checkbox" checked="checked" onclick="return false">`
                            } else {
                                return `<input type="checkbox" onclick="return false">`
                            }
                        },
                    },
                    {
                        data: 'numExpediente', title: 'Num. Expediente',
                        render: function (data, type, row, meta) {
                            if (row.numExpediente != null) {
                                return row.numExpediente
                            } else {
                                return `-`
                            }
                        },
                    },
                    {
                        title: 'Estatus demanda',
                        render: function (data, type, row) {
                            switch (row.strDemandaCerrada) {
                                case "Cerrado":
                                    return `<i class="fas fa-lock" style="font-size: xx-large; color: red;" title="Demanda cerrada."></i>`
                                case "Abierto":
                                    return `<i class="fas fa-unlock" style="font-size: xx-large; color: green;" title="Demanda abierta."></i>`
                            }
                        }
                    },
                    {
                        title: "Opciones",
                        render: function (data, type, row, meta) {
                            let btnsReturn = "";
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`
                            let btnSeguimiento = `<button class='btn btn-xs btn-primary seguimiento' title='Seguimiento.'><i class="fas fa-briefcase"></i></button>`

                            switch (tipoVista.data().tipoVista) {
                                case "SEGUIMIENTO":
                                    btnsReturn = `${btnEditar} ${btnEliminar} ${btnSeguimiento}`
                                    break;
                                case "CAPTURA":
                                    btnsReturn = `${btnEditar} ${btnEliminar}`
                                    break;
                            }
                            return btnsReturn
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblCapturas.on('click', '.editarRegistro', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data()
                        fncMostrarPorcentajeDocumentos(false)
                        fncDisabledEnableCECaptura(false)

                        fncDefaultCtrls("txtCE_NombreEmpleado")
                        fncDefaultCtrls("txtCE_FechaRecibioDemanda")
                        fncDefaultCtrls("txtCE_FechaDemanda")
                        fncDefaultCtrls("txtCE_NumExpediente")
                        fncDefaultCtrls("select2-cboCE_Juzgado-container")
                        fncDefaultCtrls("select2-cboCE_TipoDemanda-container")
                        fncDefaultCtrls("select2-cboCE_EstadoDemanda-container")
                        fncDefaultCtrls("txtCE_AQuienDemanda")
                        fncDefaultCtrls("txtCE_EstadoActual")
                        fncDefaultCtrls("txtCE_FechaAudencia")
                        fncDefaultCtrls("txtCE_AbogadoDemandante")
                        fncDefaultCtrls("select2-cboCE_Semaforo-container")

                        mdlCECaptura.data().id = rowData.id
                        fncGetDatosActualizarCaptura()
                    })

                    tblCapturas.on('click', '.eliminarRegistro', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data()
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCaptura(rowData.id))
                    })

                    tblCapturas.on('click', '.seguimiento', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data()
                        fncMostrarPorcentajeDocumentos(true)
                        fncDisabledEnableCECaptura(true)
                        mdlCECaptura.data().id = rowData.id
                        mdlCECaptura.data().claveEmpleado = rowData.claveEmpleado
                        fncGetDatosActualizarCaptura()
                    })
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    //{ className: 'dt-body-center', targets: [0] },
                    //{ className: 'dt-body-right', targets: [0] },
                    { width: '8%', targets: [0, 2, 3, 4, 5] },
                    // { width: '12%', targets: [0, 2, 3, 5] },
                ],
            })
        }

        function fncGetCapturas() {
            let obj = new Object()
            axios.post('GetCapturas', obj).then(response => {
                let { success, items, message } = response.data
                if (success) {
                    //#region FILL DATATABLE
                    dtCapturas.clear()
                    dtCapturas.rows.add(response.data.lstCapturas)
                    dtCapturas.draw()
                    //#endregion
                } else {
                    Alert2Error(message)
                }
            }).catch(error => Alert2Error(error.message))
        }

        function fncCECaptura() {
            let obj = fncCECapturaOBJ()
            if (obj != "") {
                axios.post('CECaptura', obj).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        fncGetCapturas()
                        mdlCECaptura.modal("hide")
                        Alert2Exito(message)
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            }
        }

        function fncCECapturaOBJ() {
            let mensajeError = ""
            if (chkCE_EmpleadoCP.prop("checked")) {
                if (cboCE_NombreEmpleado.val() <= 0) { $("#select2-cboCE_NombreEmpleado-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCE_NombreEmpleado-container") }
            } else {
                if (txtCE_NombreEmpleado.val() == "") { txtCE_NombreEmpleado.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_NombreEmpleado") }
            }
            if (txtCE_FechaRecibioDemanda.val() == "") { txtCE_FechaRecibioDemanda.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_FechaRecibioDemanda") }
            if (txtCE_FechaDemanda.val() == "") { txtCE_FechaDemanda.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_FechaDemanda") }
            if (cboCE_Juzgado.val() <= 0) { $("#select2-cboCE_Juzgado-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCE_Juzgado-container") }
            if (cboCE_TipoDemanda.val() <= 0) { $("#select2-cboCE_TipoDemanda-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCE_TipoDemanda-container") }
            if (cboCE_EstadoDemanda.val() <= 0) { $("#select2-cboCE_EstadoDemanda-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCE_EstadoDemanda-container") }
            if (txtCE_AQuienDemanda.val() == "") { txtCE_AQuienDemanda.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_AQuienDemanda") }
            if (txtCE_EstadoActual.val() == "") { txtCE_EstadoActual.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_EstadoActual") }
            if (txtCE_AbogadoDemandante.val() == "") { txtCE_AbogadoDemandante.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_AbogadoDemandante") }
            if (cboCE_Semaforo.val() <= 0) { $("#select2-cboCE_Semaforo-container").css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("select2-cboCE_Semaforo-container") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return ""
            } else {
                let obj = new Object()
                obj.id = btnCECaptura.data().id
                obj.claveEmpleado = txtCE_ClaveEmpleado.val()
                obj.nombreDemandante = txtCE_NombreEmpleado.val()
                obj.puesto = txtCE_Puesto.val()
                obj.esEmpleadoCP = chkCE_EmpleadoCP.prop("checked")
                obj.cc = cboCE_CC.val() <= 0 ? txtCE_CC.val() : cboCE_CC.val()
                obj.ccLibre = cboCE_CC.val() <= 0 ? true : false
                obj.fechaIngreso = txtCE_FechaIngreso.val()
                obj.fechaBaja = txtCE_FechaBaja.val()
                obj.motivoSalida = txtCE_MotivoSalida.val()
                obj.sueldoDiario = unmaskNumero(txtCE_SueldoDiario.val())
                obj.ofertaInicial = unmaskNumero(txtCE_OfertaInicial.val())
                obj.antiguedad = txtCE_Antiguedad.val()
                obj.fechaRecibioDemanda = txtCE_FechaRecibioDemanda.val()
                obj.fechaDemanda = txtCE_FechaDemanda.val()
                obj.numExpediente = txtCE_NumExpediente.val()
                obj.FK_Juzgado = cboCE_Juzgado.val()
                obj.FK_TipoDemanda = cboCE_TipoDemanda.val()
                obj.FK_Estado = cboCE_EstadoDemanda.val()
                obj.demandado = txtCE_AQuienDemanda.val()
                obj.salarioDiario = unmaskNumero(txtCE_SalarioDiario.val())
                obj.hechos = txtCE_Hechos.val()
                obj.peticiones = txtCE_Peticiones.val()
                obj.estadoActual = txtCE_EstadoActual.val()
                obj.fechaAudiencia = txtCE_FechaAudencia.val()
                obj.comentarioFechaAudiencia = txtCE_ComentarioFechaAudiencia.val()
                obj.abogadoDemandante = txtCE_AbogadoDemandante.val()
                obj.cuantiaTotal = unmaskNumero(txtCE_CuantiaTotal.val())
                obj.negociadoCerrado = unmaskNumero(txtCE_NegociadoCerrado.val())
                obj.finiquitoAl100 = unmaskNumero(txtCE_Finiquito.val())
                obj.diferencia = unmaskNumero(txtCE_Diferencia.val())
                obj.semaforo = cboCE_Semaforo.val()
                obj.resolucionLaudo = txtCE_ResolucionLaudo.val()
                return obj
            }
        }

        function fncEliminarCaptura(idCaptura) {
            if (idCaptura > 0) {
                let obj = new Object()
                obj.id = idCaptura
                axios.post('EliminarCaptura', obj).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        fncGetCapturas()
                        Alert2Exito(message)
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            } else {
                Alert2Error("Ocurrió un error al eliminar la captura")
            }
        }

        function fncGetDatosActualizarCaptura() {
            if (mdlCECaptura.data().id > 0) {
                let idCaptura = mdlCECaptura.data().id
                let obj = new Object()
                obj.id = idCaptura
                axios.post('GetDatosActualizarCaptura', obj).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        fncLimpiarMdlCECaptura()
                        btnCECaptura.html("<i class='fas fa-save'></i>&nbsp;Actualizar")
                        btnCECaptura.data().id = idCaptura
                        btnCESeguimiento.data().id = idCaptura
                        txtCE_ClaveEmpleado.val(response.data.objCaptura.claveEmpleado)
                        txtCE_NombreEmpleado.val(response.data.objCaptura.nombreDemandante)
                        txtCE_Puesto.val(response.data.objCaptura.puesto)
                        chkCE_EmpleadoCP.prop("checked", response.data.objCaptura.esEmpleadoCP)
                        fncOcultarMostrarCboCE_Empleados(chkCE_EmpleadoCP.prop("checked"))
                        if (txtCE_ClaveEmpleado.val() > 0) {
                            cboCE_NombreEmpleado.val(txtCE_ClaveEmpleado.val())
                            cboCE_NombreEmpleado.trigger("change")
                        }
                        div_cboCE_CC.css("display", "none")
                        div_txtCE_CC.css("display", "inline")
                        txtCE_CC.val(response.data.objCaptura.cc)
                        txtCE_FechaIngreso.val(moment(response.data.objCaptura.fechaIngreso).format('YYYY-MM-DD'))
                        txtCE_FechaBaja.val(moment(response.data.objCaptura.fechaBaja).format('YYYY-MM-DD'))
                        txtCE_MotivoSalida.val(response.data.objCaptura.motivoSalida)
                        txtCE_SueldoDiario.val(maskNumero2DCompras(response.data.objCaptura.sueldoDiario))
                        txtCE_OfertaInicial.val(maskNumero2DCompras(response.data.objCaptura.ofertaInicial))
                        txtCE_Hechos.val(response.data.objCaptura.hechos)
                        txtCE_Peticiones.val(response.data.objCaptura.peticiones)
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
                        txtCE_EstadoActual.val(response.data.objCaptura.estadoActual)
                        txtCE_FechaAudencia.val(moment(response.data.objCaptura.fechaAudiencia).format('YYYY-MM-DD'))
                        txtCE_ComentarioFechaAudiencia.val(response.data.objCaptura.comentarioFechaAudiencia)
                        txtCE_AbogadoDemandante.val(response.data.objCaptura.abogadoDemandante)
                        txtCE_CuantiaTotal.val(maskNumero2DCompras(response.data.objCaptura.cuantiaTotal))
                        txtCE_NegociadoCerrado.val(maskNumero2DCompras(response.data.objCaptura.negociadoCerrado))
                        txtCE_Finiquito.val(maskNumero2DCompras(response.data.objCaptura.finiquitoAl100))
                        txtCE_Diferencia.val(maskNumero2DCompras(response.data.objCaptura.diferencia))
                        txtCE_ResolucionLaudo.val(response.data.objCaptura.resolucionLaudo)
                        cboCE_Semaforo.val(response.data.objCaptura.semaforo)
                        cboCE_Semaforo.trigger("change")
                        btnCE_PorcentajeDocumentoLegal.html(`Cantidad de documentos cargados: ${response.data.objCaptura.cantDocumentosCargados}`)
                        if (response.data.objCaptura.cantDocumentosCargados > 0) {
                            btnCE_PorcentajeDocumentoLegal.css("border-width: revert")
                            btnCE_PorcentajeDocumentoLegal.css("border-color: green")
                        } else {
                            btnCE_PorcentajeDocumentoLegal.css("border-width", "")
                            btnCE_PorcentajeDocumentoLegal.css("border-color", "")
                        }

                        let PorcentajeExpediente = response.data.objCaptura.porcentajeExpediente
                        let PorcentajeExpedienteInput =
                            `<div div class="progress">
                                <div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" 
                                    style="min-width: 4em; width: ${PorcentajeExpediente}%; background-color: green;">
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

        function fncGetInformacionEmpleado() {
            if (txtCE_ClaveEmpleado != "") {
                let obj = new Object()
                obj.claveEmpleado = txtCE_ClaveEmpleado.val()
                axios.post('GetInformacionEmpleado', obj).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        if (response.data.objEmpleado.porcentajeExpediente <= 0) {
                            let PorcentajeExpedienteInput =
                                `<div div class="progress">
                                <div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="min-width: 4em;">
                                    ${0}%
                                </div>
                             </div>`
                            btnCE_PorcentajeExpediente.html(PorcentajeExpedienteInput);
                        } else {
                            btnCE_PorcentajeExpediente.html(response.data.objEmpleado.porcentajeExpediente);
                        }

                        if (response.data.objEmpleado.porcentajeFiniquito <= 0) {
                            let PorcentajeFiniquitoInput =
                                `<div div class="progress">
                                <div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="min-width: 4em; 
                                    background-color: ${response.data.objEmpleado.colorPorcentajeFiniquito}">
                                    ${0}%
                                </div>
                             </div>`
                            btnCE_PorcentajeFiniquito.html(PorcentajeFiniquitoInput);
                        } else {
                            let PorcentajeFiniquitoInput =
                                `<div div class="progress">
                                <div class="progress-bar" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="min-width: ${response.data.objEmpleado.porcentajeFiniquito}%; 
                                    background-color: ${response.data.objEmpleado.colorPorcentajeFiniquito}">
                                    ${response.data.objEmpleado.porcentajeFiniquito}%
                                </div>
                             </div>`
                            btnCE_PorcentajeFiniquito.html(PorcentajeFiniquitoInput);
                        }

                        txtCE_NombreEmpleado.val(response.data.objEmpleado.nombreDemandante)
                        cboCE_NombreEmpleado.val(txtCE_ClaveEmpleado.val())
                        cboCE_NombreEmpleado.trigger("change")
                        txtCE_Puesto.val(response.data.objEmpleado.puesto)
                        cboCE_CC.val(response.data.objEmpleado.cc)
                        cboCE_CC.trigger("change")
                        txtCE_FechaIngreso.val(moment(response.data.objEmpleado.fechaIngreso).format('YYYY-MM-DD'))
                        txtCE_FechaBaja.val(moment(response.data.objEmpleado.fechaBaja).format('YYYY-MM-DD'))
                        txtCE_MotivoSalida.val(response.data.objEmpleado.motivoBaja)
                        txtCE_SueldoDiario.val(maskNumero2DCompras(response.data.objEmpleado.sueldoDiario))
                        txtCE_Antiguedad.val(response.data.objEmpleado.antiguedad)
                        chkCE_EmpleadoCP.prop("checked", response.data.objEmpleado.esEmpleadoCP)
                        mdlCECaptura.modal("show")
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            } else {
                Alert2Error("Ocurrió un error al obtener la información del empleado.")
            }
        }

        function fncFillCombos() {
            cboCE_CC.fillCombo('/Administrativo/DemandaCH/FillCboCC', null, false, null)
            cboCE_EstadoDemanda.fillCombo('/Administrativo/DemandaCH/FillCboEstados', null, false, null)
            cboCE_Juzgado.fillCombo('/Administrativo/DemandaCH/FillCboJuzgados', null, false, null)
            cboCE_Semaforo.fillCombo('/Administrativo/DemandaCH/FillCboSemaforo', null, false, null)
            cboCE_TipoDemanda.fillCombo('/Administrativo/DemandaCH/FillCboTipoDemandas', null, false, null)
            cboCE_Seguimiento_Semaforo.fillCombo('/Administrativo/DemandaCH/FillCboSemaforo', null, false, null)
            cboCE_TipoArchivo.fillCombo('/Administrativo/DemandaCH/FillCboTipoArchivos', null, false, null)
            cboCE_NombreEmpleado.fillCombo('/Administrativo/DemandaCH/FillCboEmpleados', null, false, null)
            $(".select2").select2()
        }

        function fncMostrarPorcentajeDocumentos(mostrar) {
            if (mostrar) {
                divDocumentosAdjuntos.css("display", "block")
            } else {
                divDocumentosAdjuntos.css("display", "none")
            }
        }

        function fncCerrarDemanda() {
            if (btnCECaptura.data().id > 0) {
                let obj = new Object()
                obj.idCaptura = btnCECaptura.data().id
                axios.post('CerrarDemanda', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        mdlCECaptura.modal("hide")
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al indicar como cerrada la demanda.")
            }
        }

        function fncOcultarMostrarCboCE_Empleados(checked) {
            if (checked) {
                divOcultarMostrarCboCE_NombreEmpleado.css("display", "inline")
                txtCE_NombreEmpleado.css("display", "none")
            } else {
                divOcultarMostrarCboCE_NombreEmpleado.css("display", "none")
                txtCE_NombreEmpleado.css("display", "inline")
            }
        }
        //#endregion

        //#region FUNCIONES SEGUIMIENTOS
        function fncCrearSeguimiento() {
            let obj = fncCrearSeguimientoOBJ()
            if (obj != "") {
                axios.post('CrearSeguimiento', obj).then(response => {
                    let { success, items, message } = response.data
                    if (success) {
                        fncGetCapturas()
                        mdlCECaptura.modal("hide")
                        Alert2Exito(message)
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message))
            }
        }

        function fncCrearSeguimientoOBJ() {
            let mensajeError = ""
            if (txtCE_Seguimiento_Cuantia.val() <= 0) { txtCE_Seguimiento_Cuantia.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_Seguimiento_Cuantia") }
            if (txtCE_Seguimiento_AbogadoDemandante.val() <= 0) { txtCE_Seguimiento_AbogadoDemandante.css('border', '2px solid red'); mensajeError = 'Es necesario ingresar la información indicada.' } else { fncDefaultCtrls("txtCE_Seguimiento_AbogadoDemandante") }

            if (mensajeError != "") {
                Alert2Warning(mensajeError)
                return ""
            } else {
                let obj = new Object()
                obj.FK_Demanda = btnCESeguimiento.data().id
                obj.cuantia = txtCE_Seguimiento_Cuantia.val()
                obj.abogadoDemandante = txtCE_Seguimiento_AbogadoDemandante.val()
                obj.fechaAudiencia = txtCE_Seguimiento_FechaAudiencia.val()
                obj.semaforo = cboCE_Seguimiento_Semaforo.val()
                obj.estadoActual = txtCE_Seguimiento_EstadoActual.val()
                return obj
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
                    {
                        title: 'Fecha registro documento',
                        render: function (data, type, row) {
                            return moment(row.fechaCreacion).format('DD/MM/YYYY')
                        }
                    },
                    { data: 'archivo', title: 'Archivo' },
                    { data: 'strTipoArchivo', title: 'Tipo archivo' },
                    {
                        render: function (data, type, row, meta) {
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`
                            let btnVisualizar = `<button class='btn btn-xs btn-primary visualizarArchivo' title='Visualizar archivo.'><i class="fas fa-book-reader"></i></button>`
                            let btnDescargar = `<button class='btn btn-xs btn-primary descargarArchivo' title='Descargar archivo.'><i class="fas fa-file-download"></i></button>`
                            return `${btnEliminar} ${btnVisualizar} ${btnDescargar}`
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
                    // { width: '10%', targets: [0, 3] }
                ],
            });
        }

        function fncDescargarArchivoAdjunto(rutaArchivo) {
            if (rutaArchivo != "") {
                document.location.href = `/ Administrativo / DemandaCH / DescargarArchivoAdjunto ? rutaArchivo = ${rutaArchivo} `
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

        function fncEliminarArchivoAdjunto(idArchivo) {
            if (idArchivo > 0) {
                let obj = {};
                obj.idArchivo = idArchivo;
                axios.post('EliminarArchivoAdjunto', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetArchivosAdjuntos()
                        fncGetDatosActualizarCaptura()
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        const fncGuardarArchivoAdjunto = function () {
            var data = fncGetEvidenciaParaGuardar();
            if (btnCECaptura.data().id > 0 && cboCE_TipoArchivo.val() > 0) {
                let obj = new Object();
                obj.FK_Captura = btnCECaptura.data().id
                obj.tipoArchivo = cboCE_TipoArchivo.val()
                axios.post('GuardarArchivoAdjunto', data, { params: FK_Captura = obj }, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                    let { success, datos, message } = response.data;
                    if (success) {
                        Alert2Exito(message)
                        fncGetArchivosAdjuntos()
                        fncGetDatosActualizarCaptura()
                    } else {
                        Alert2Error(message)
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
            } else {
                if (btnCECaptura.data().id <= 0) { Alert2Error("Ocurrió un error al guardar el archivo."); }
                if (cboCE_TipoArchivo.val() <= 0) { Alert2Warning("Es necesario seleccionar el tipo de archivo."); }
            }
        }

        function fncGetEvidenciaParaGuardar() {
            let data = new FormData();
            data.append("id", $("#rowDataId").val());
            $.each(document.getElementById("txtArchivoAdjunto").files, function (i, file) {
                data.append("lstArchivos", file);
            });
            return data;
        }

        function fncGetArchivosAdjuntos() {
            if (btnCECaptura.data().id > 0) {
                let obj = {};
                obj.FK_Captura = btnCECaptura.data().id
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
            }
        }
        //#endregion

        //#region FUNCIONES ESTATUS FINIQUITO
        function initTblEstatusFiniquito() {
            dtEstatusFiniquito = tblEstatusFiniquito.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'descripcionFiniquito', title: 'Descripción' },
                    {
                        title: 'Estatus',
                        render: function (data, type, row) {
                            return `< input type = "checkbox" checked = "${row.estatusFiniquito}" onclick = "return false; " > `
                        }
                    },
                ],
                initComplete: function (settings, json) {
                    tblEstatusFiniquito.on('click', '.editarRegistro', function () {
                        let rowData = dtEstatusFiniquito.row($(this).closest('tr')).data();
                        fncGetDatosActualizarCaptura(rowData.id);
                    });
                    tblEstatusFiniquito.on('click', '.eliminarRegistro', function () {
                        let rowData = dtEstatusFiniquito.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCaptura(rowData.id));
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

        function fncGetEstatusFiniquitoEmpleadoDemanda() {
            console.log(mdlCECaptura.data().claveEmpleado);
            if (mdlCECaptura.data().claveEmpleado > 0) {
                let obj = {}
                obj.claveEmpleado = mdlCECaptura.data().claveEmpleado
                axios.post('GetEstatusFiniquitoEmpleadoDemanda', obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        dtEstatusFiniquito.clear();
                        dtEstatusFiniquito.rows.add(response.data.lstEstatusFiniquito);
                        dtEstatusFiniquito.draw();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener el estatus del finiquito del ex empleado.")
            }
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncLimpiarMdlCECaptura() {
            $("input[type='text']").val("");
            chkCE_EmpleadoCP.prop("checked", false)
            cboCE_CC[0].selectedIndex = 0
            cboCE_CC.trigger("change")
            cboCE_Juzgado[0].selectedIndex = 0
            cboCE_Juzgado.trigger("change")
            cboCE_TipoDemanda[0].selectedIndex = 0
            cboCE_TipoDemanda.trigger("change")
            cboCE_EstadoDemanda[0].selectedIndex = 0
            cboCE_EstadoDemanda.trigger("change")
            cboCE_Semaforo[0].selectedIndex = 0
            cboCE_Semaforo.trigger("change")
            cboCE_NombreEmpleado[0].selectedIndex = 0
            cboCE_NombreEmpleado.trigger("change")
            txtCE_Hechos.val("")
            txtCE_Peticiones.val("")
            txtCE_ResolucionLaudo.val("")
        }

        function fncObtenerURLParams() {
            let params = {};
            let parser = document.createElement('a');
            parser.href = window.location.href;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            switch (params.esCaptura) {
                case "0":
                    tipoVista.html("SEGUIMIENTO")
                    tipoVista.data().tipoVista = "SEGUIMIENTO"
                    break;
                case "1":
                    tipoVista.html("CAPTURA")
                    tipoVista.data().tipoVista = "CAPTURA"
                    break;
                default:
                    break;
            }
        }

        function fncDisabledEnableCECaptura(disabled) {
            txtCE_ClaveEmpleado.attr("disabled", disabled)
            txtCE_NombreEmpleado.attr("disabled", disabled)
            cboCE_NombreEmpleado.attr("disabled", disabled)
            txtCE_Puesto.attr("disabled", disabled)
            chkCE_EmpleadoCP.attr("disabled", disabled)
            cboCE_CC.attr("disabled", disabled)
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
            txtCE_ComentarioFechaAudiencia.attr("disabled", disabled)
            txtCE_Hechos.attr("disabled", disabled)
            txtCE_Peticiones.attr("disabled", disabled)

            if (disabled) {
                // MOSTRAR CONTROLES DE SEGUIMIENTO
                btnCECaptura.css("display", "none")
                btNuevo_Seguimiento.css("display", "inline")
                divSeguimiento.css("display", "none")
                btnCerrarDemanda.css("display", "inline")
            } else {
                // OCULTAR CONTROLES DE SEGUIMIENTO
                btNuevo_Seguimiento.css("display", "none")
                btnCESeguimiento.css("display", "none")
                divSeguimiento.css("display", "none")
                btnCECaptura.css("display", "inline")
                btnCerrarDemanda.css("display", "none")
            }
        }

        function fncDefaultCtrls(obj) {
            $(`#${obj} `).css("border", "1px solid #CCC");
        }

        function fncGetDiferencia(negociadoCerrado, cuantiaTotal) {
            let diferencia = unmaskNumero(cuantiaTotal) - unmaskNumero(negociadoCerrado)
            diferencia = maskNumero2DCompras(diferencia)
            return diferencia
        }

        function setInputFormatoMoneda() {
            txtCE_SueldoDiario.val(maskNumero2DCompras(0))
            txtCE_SalarioDiario.val(maskNumero2DCompras(0))
            txtCE_OfertaInicial.val(maskNumero2DCompras(0))
            txtCE_CuantiaTotal.val(maskNumero2DCompras(0))
            txtCE_NegociadoCerrado.val(maskNumero2DCompras(0))
            txtCE_Finiquito.val(maskNumero2DCompras(0))
            txtCE_Diferencia.val(maskNumero2DCompras(0))
        }

        function fncSetMaskNumero2D(valor) {
            let oldValue = unmaskNumero(valor)
            let newValue = maskNumero2DCompras(oldValue)
            return newValue
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Captura = new Captura()
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }) })
        .ajaxStop(() => { $.unblockUI() })
})()