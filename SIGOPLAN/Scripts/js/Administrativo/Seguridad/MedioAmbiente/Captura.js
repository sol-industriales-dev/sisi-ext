(() => {
    $.namespace('CH.Captura');

    //#region CONST

    //#region FILTROS
    const cboFiltroAgrupacion = $('#cboFiltroAgrupacion');
    const cboFiltroEstatusCaptura = $('#cboFiltroEstatusCaptura');
    const btnFiltroBuscar = $('#btnFiltroBuscar');
    const txtMesInicio = $('#txtMesInicio');
    const txtMesFinal = $('#txtMesFinal');
    const btnListadoTrayectos = $('#btnListadoTrayectos');
    const btnListadoDestinoFinal = $('#btnListadoDestinoFinal');
    //#endregion

    //#region
    const dateFormat = "dd/mm/yy";
    const showAnim = "slide";
    const fechaActual = new Date();
    const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);
    const yyyy = fechaActual.getFullYear();
    const mm = fechaActual.getMonth() + 1;
    const dd = fechaActual.getDate();
    const fechaActualInputDate = `${yyyy}/${mm}/${dd}`;
    //#endregion

    //#region CONST CRUD CAPTURAS
    const btnCECapturaModal = $('#btnCECapturaModal');
    const mdlCECaptura = $('#mdlCECaptura');
    const lblTitleCECaptura = $('#lblTitleCECaptura');
    const cboAgrupacion = $('#cboAgrupacion');
    const cboTecnicoResponsable = $('#cboTecnicoResponsable');
    const txtFechaEntrada = $('#txtFechaEntrada');
    const txtCantContenedor = $('#txtCantContenedor');
    const txtCodigoContenedor = $('#txtCodigoContenedor');
    const txtTipoContenedor = $('#txtTipoContenedor');
    const cboAspectoAmbiental = $('#cboAspectoAmbiental');
    const txtCantResiduo = $('#txtCantResiduo');
    const txtPlantaProcesoGeneracion = $('#txtPlantaProcesoGeneracion');
    const btnCECaptura = $('#btnCECaptura');
    const titleBtnCECaptura = $('#titleBtnCECaptura');
    const tablaAspectosAmbientales = $('#tablaAspectosAmbientales');
    let dtAspectosAmbientales;
    const botonQuitarPunto = $('#botonQuitarPunto');
    const botonAgregarPunto = $('#botonAgregarPunto');
    const tblS_MedioAmbienteCaptura = $('#tblS_MedioAmbienteCaptura');
    let dtCapturas;
    const txtArchivoAcopio = $('#txtArchivoAcopio');
    const btnDescargarAcopio = $('#btnDescargarAcopio');
    const btnCapturaTipoAspectoAmbiental = $('#btnCapturaTipoAspectoAmbiental');
    const btnCapturaTipoResiduoPeligroso = $('#btnCapturaTipoResiduoPeligroso');
    const btnCapturaTipoRsuRme = $('#btnCapturaTipoRsuRme');
    const divCantContenedor = $('#divCantContenedor');
    const divModalCECaptura = $('#divModalCECaptura');
    //#endregion

    //#region CONST CRUD TRAYECTOS
    const mdlCETrayecto = $('#mdlCETrayecto');
    const lblTitleCETrayecto = $('#lblTitleCETrayecto');
    const txtTratamiento = $('#txtTratamiento');
    const txtManifiesto = $('#txtManifiesto');
    const txtFechaEmbarque = $('#txtFechaEmbarque');
    const txtTipoTransporte = $('#txtTipoTransporte');
    const cboTransportista = $('#cboTransportista');
    const btnCETrayecto = $('#btnCETrayecto');
    const titleBtnCETrayecto = $('#titleBtnCETrayecto');
    const txtArchivoTrayecto = $('#txtArchivoTrayecto');
    const btnDescargarTrayecto = $('#btnDescargarTrayecto');
    const tblAspectosAmbientalesAcopioToTrayectos = $('#tblAspectosAmbientalesAcopioToTrayectos');
    const cboAgrupacionTrayecto = $('#cboAgrupacionTrayecto');
    const txtConsecutivoTrayecto = $('#txtConsecutivoTrayecto')
    let dtTrayectos;
    //#endregion

    //#region CONST CRUD DESTINO FINAL
    const mdlCEDestinoFinal = $('#mdlCEDestinoFinal');
    const lblTitleCEDestinoFinal = $('#lblTitleCEDestinoFinal');
    const txtFechaDestinoFinal = $('#txtFechaDestinoFinal');
    const cboTransportistaDestinoFinal = $('#cboTransportistaDestinoFinal');
    const btnCEDestinoFinal = $('#btnCEDestinoFinal');
    const titleBtnCEDestinoFinal = $('#titleBtnCEDestinoFinal');
    const txtArchivoDestinoFinal = $('#txtArchivoDestinoFinal');
    const btnDescargarDestinoFinal = $('#btnDescargarDestinoFinal');
    const cboAgrupacionDestinoFinal = $('#cboAgrupacionDestinoFinal');
    const tblAspectosAmbientalesTrayectoToDestinoFinal = $('#tblAspectosAmbientalesTrayectoToDestinoFinal');
    const txtConsecutivoDestinoFinal = $('#txtConsecutivoDestinoFinal')
    let dtDestinoFinal;
    //#endregion

    //#region CONST ARCHIVOS
    let dtArchivos;
    const mdlArchivos = $('#mdlArchivos');
    const tblArchivos = $('#tblArchivos');
    //#endregion

    //#endregion

    Captura = function () {
        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT DATATABLES
            initTblCapturas();
            initTblAspectosAmbientalesAcopioToTrayectos();
            initTblAspectosAmbientalesTrayectoToDestinoFinal();
            initTblArchivos();
            //#endregion

            //#region EVENTOS FILTROS
            cboFiltroAgrupacion.fillCombo("FillCboAgrupaciones", {}, false);
            cboFiltroAgrupacion.select2({ width: "100%" });

            cboFiltroEstatusCaptura.select2({ width: "100%" });

            btnFiltroBuscar.on("click", function (e) {
                fncGetCapturas();
            });

            initMonthPicker(txtMesInicio);
            txtMesInicio.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaInicioAnio);
            initMonthPicker(txtMesFinal);

            btnListadoTrayectos.on("click", function () {
                fncFillCboTransportistas();
                cboAgrupacionTrayecto.fillCombo("FillCboAgrupaciones", {}, false);
                cboAgrupacionTrayecto.select2({ width: "100%" });
                txtFechaEmbarque.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
                fncLimpiarModalCETrayecto();
                mdlCETrayecto.modal("show");
            });

            btnListadoDestinoFinal.on("click", function () {
                fncFillCboTransportistas();
                cboAgrupacionDestinoFinal.fillCombo("FillCboAgrupaciones", {}, false);
                cboAgrupacionDestinoFinal.select2({ width: "100%" });
                txtFechaDestinoFinal.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
                fncLimpiarModalCEDestinoFinal();
                mdlCEDestinoFinal.modal("show");
            });
            //#endregion

            //#region EVENTOS CAPTURAS
            fncGetCapturas();

            cboAgrupacion.fillCombo("FillCboAgrupaciones", {}, false);
            cboAgrupacion.select2({ width: "100%" });

            cboTecnicoResponsable.fillCombo("FillCboUsuarios", {}, false);
            cboTecnicoResponsable.select2({ width: "100%" });

            botonAgregarPunto.attr("disabled", true);
            botonQuitarPunto.attr("disabled", true);
            cboAgrupacion.on("change", function (e) {
                if ($(this).val() != "") {
                    botonAgregarPunto.attr("disabled", false);
                    botonQuitarPunto.attr("disabled", false);
                } else {
                    botonAgregarPunto.attr("disabled", true);
                    botonQuitarPunto.attr("disabled", true);
                }
            });

            btnCECapturaModal.on("click", function (e) {
                btnDescargarAcopio.attr("disabled", true);
                lblTitleCECaptura.html("NUEVO REGISTRO");
                titleBtnCECaptura.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                btnCECaptura.attr("data-id", 0);
                fncLimpiarModalCECapturas();
                fncBorderDefault();
                fncFillCboAspectosAmbientales();
                txtFechaEntrada.val(moment(fechaActualInputDate).format("YYYY-MM-DD"));
                btnCECaptura.attr("data-tipoCaptura", 0);
                divModalCECaptura.css("display", "none");
                btnCapturaTipoAspectoAmbiental.removeClass("btn-success").addClass("btn-primary");
                btnCapturaTipoResiduoPeligroso.removeClass("btn-success").addClass("btn-primary");
                btnCapturaTipoRsuRme.removeClass("btn-success").addClass("btn-primary");

                mdlCECaptura.modal("show");
            });

            btnCECaptura.on("click", function (e) {
                fncCrearEditarCaptura();
            });

            botonAgregarPunto.on('click', function () {
                let datos = dtAspectosAmbientales.rows().data();

                $.each(datos, function (idx, data) {
                    let row = tablaAspectosAmbientales.find('tbody tr').eq(idx);
                    data.codigoContenedor = $(row).find('.inputCodigoContenedor').val();
                    data.aspectoAmbientalID = $(row).find('.selectAspectoAmbiental').val();
                    data.cantidad = $(row).find('.inputCantidad').val();
                    data.unidadMedida = $(row).find('.inputUnidadMedida').val();
                });

                datos.push({
                    codigoContenedor: "",
                    aspectoAmbientalID: "",
                    cantidad: "",
                    unidadMedida: "",
                });

                if (btnCECaptura.attr("data-tipoCaptura") == 2) {
                    fncCantContenedor(true);
                }

                dtAspectosAmbientales.clear();
                dtAspectosAmbientales.rows.add(datos).draw();
            });

            botonQuitarPunto.on('click', function () {
                dtAspectosAmbientales.row(tablaAspectosAmbientales.find("tr.selected")).remove().draw();

                let cuerpo = tablaAspectosAmbientales.find('tbody');
                if (cuerpo.find("tr").length == 0) {
                    dtAspectosAmbientales.draw();
                } else {
                    tablaAspectosAmbientales.find('tbody tr').each(function (idx, row) {
                        let rowData = dtAspectosAmbientales.row(row).data();

                        if (rowData != undefined) {
                            rowData.codigoContenedor = $(row).find('.codigoContenedor').val();
                            rowData.aspectoAmbientalID = $(row).find('.selectAspectoAmbiental').val();
                            rowData.cantidad = $(row).find('.inputCantidad').val();
                            rowData.unidadMedida = $(row).find('.inputUnidadMedida').val();

                            dtAspectosAmbientales.row(row).data(rowData).draw();

                            let selectAspectoAmbiental = $(row).find('.selectAspectoAmbiental');
                            selectAspectoAmbiental.fillCombo('/Administrativo/MedioAmbiente/GetAspectosAmbientales', null, false, null);

                            selectAspectoAmbiental.select2();
                            selectAspectoAmbiental.find('option[value="' + rowData.aspectoAmbientalID + '"]').attr('selected', true);
                            selectAspectoAmbiental.trigger('change');
                        }
                    });
                }

                if (btnCECaptura.attr("data-tipoCaptura") == 2) {
                    fncCantContenedor(false);
                }
            });

            btnDescargarAcopio.on("click", function () {
                Alert2AccionConfirmar('Descarga', '¿Desea descargar el archivo de acopio?', 'Confirmar', 'Cancelar', () => fncDescargarArchivo(btnCECaptura.attr("data-id"), 1));
            });

            btnCapturaTipoAspectoAmbiental.on("click", function () {
                $(this).removeClass("btn-primary").addClass("btn-success");
                btnCapturaTipoResiduoPeligroso.removeClass("btn-success").addClass("btn-primary");
                btnCapturaTipoRsuRme.removeClass("btn-success").addClass("btn-primary");

                //#region MOSTRAR/OCULTAR INPUTS MODAL CREAR/EDITAR CAPTURA
                let tipoCaptura = 1;
                fncMostrarOcultarTipoCaptura(tipoCaptura);
                //#endregion

                if (dtAspectosAmbientales != undefined) {
                    tablaAspectosAmbientales.DataTable().clear().destroy();
                }
                initTablaAspectosAmbientales(tipoCaptura);
                btnCECaptura.attr("data-tipoCaptura", 1);
                divModalCECaptura.css("display", "inline");
            });

            btnCapturaTipoResiduoPeligroso.on("click", function () {
                btnCapturaTipoAspectoAmbiental.removeClass("btn-success").addClass("btn-primary");
                $(this).removeClass("btn-primary").addClass("btn-success");
                btnCapturaTipoRsuRme.removeClass("btn-success").addClass("btn-primary");

                //#region MOSTRAR/OCULTAR INPUTS MODAL CREAR/EDITAR CAPTURA
                let tipoCaptura = 2;
                fncMostrarOcultarTipoCaptura(tipoCaptura);
                //#endregion

                //#region SE INICIALIZA TABLA DE ASPECTOS AMBIENTALES, DONDE TIPO CAPTURA AA NO SE MUESTRA LA COLUMNA "CANTIDAD".
                if (dtAspectosAmbientales != undefined) {
                    tablaAspectosAmbientales.DataTable().clear().destroy();
                }
                initTablaAspectosAmbientales(tipoCaptura);
                //#endregion

                btnCECaptura.attr("data-tipoCaptura", 2);
                divModalCECaptura.css("display", "inline");
            });

            btnCapturaTipoRsuRme.on("click", function () {
                btnCapturaTipoAspectoAmbiental.removeClass("btn-success").addClass("btn-primary");
                btnCapturaTipoResiduoPeligroso.removeClass("btn-success").addClass("btn-primary");
                $(this).removeClass("btn-primary").addClass("btn-success");

                //#region MOSTRAR/OCULTAR INPUTS MODAL CREAR/EDITAR CAPTURA
                let tipoCaptura = 3;
                fncMostrarOcultarTipoCaptura(tipoCaptura);
                //#endregion

                if (dtAspectosAmbientales != undefined) {
                    tablaAspectosAmbientales.DataTable().clear().destroy();
                }
                initTablaAspectosAmbientales(tipoCaptura);

                btnCECaptura.attr("data-tipoCaptura", 3);
                divModalCECaptura.css("display", "inline");
            });
            //#endregion

            //#region EVENTOS TRAYECTOS
            btnCETrayecto.on("click", function (e) {
                fncCrearAspectoAmbientalAcopioToTrayecto();
            });

            btnDescargarTrayecto.on("click", function () {
                Alert2AccionConfirmar('Descarga', '¿Desea descargar el archivo de trayecto?', 'Confirmar', 'Cancelar', () => fncDescargarArchivo(btnCETrayecto.attr("data-id"), 2));
            });

            cboAgrupacionTrayecto.on("change", function () {
                if ($(this).val() > 0 && txtConsecutivoTrayecto.val() != "") {
                    fncGetAspectosAmbientalesToTrayectos();
                }
            });

            $('#tblAspectosAmbientalesAcopioToTrayectos tbody').on('click', 'tr', function () {
                $(this).toggleClass('selected');
            });

            txtConsecutivoTrayecto.change(function () {
                if ($(this).val() > 0 && cboAgrupacionTrayecto.val() > 0) {
                    fncGetAspectosAmbientalesToTrayectos();
                }
            })
            //#endregion

            //#region EVENTOS DESTINO FINAL
            btnCEDestinoFinal.on("click", function (e) {
                fncCrearDestinoFinal();
            });

            btnDescargarDestinoFinal.on("click", function () {
                Alert2AccionConfirmar('Descarga', '¿Desea descargar el archivo de destino final?', 'Confirmar', 'Cancelar', () => fncDescargarArchivo(btnCEDestinoFinal.attr("data-id"), 3));
            });

            cboAgrupacionDestinoFinal.on("change", function () {
                if ($(this).val() > 0 && txtConsecutivoDestinoFinal.val() != "") {
                    fncGetAspectosAmbientalesToDestinoFinal();
                }
            })

            txtConsecutivoDestinoFinal.change(function () {
                if (cboAgrupacionDestinoFinal.val() > 0 && $(this).val() != "") {
                    fncGetAspectosAmbientalesToDestinoFinal();
                }
            })

            $('#tblAspectosAmbientalesTrayectoToDestinoFinal tbody').on('click', 'tr', function () {
                // $(this).toggleClass('selected');
            });
            //#endregion
        }

        //#region CRUD CAPTURAS
        function initTblCapturas() {
            dtCapturas = tblS_MedioAmbienteCaptura.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: false,
                bFilter: false,
                info: false,
                order: [[8, "asc"]],
                columns: [
                    { data: 'folio', title: 'Folio' },
                    { data: 'nomAgrupacion', title: 'Agrupación', visible: false },
                    {
                        data: 'fechaEntrada', title: 'Entrada',
                        render: function (data, type, row) {
                            return moment(data).format("DD/MM/YYYY");
                        }
                    },
                    { data: 'responsableTecnico', title: 'Técnico responsable', visible: false },
                    { data: 'captura', title: 'Tipo captura' },
                    { data: 'strCantidadContenedor', title: 'Cant. Contenedor' },
                    { data: 'strEstatusCaptura', title: 'Estatus' },
                    {
                        title: 'Transportista', visible: false,
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-primary transportistas" title="Transportistas."><i class="fas fa-shipping-fast"></i></button>`;
                        },
                    },
                    {
                        title: 'Destino final', visible: false,
                        render: function (data, type, row, meta) {
                            if (row.estatusCaptura == 2) {
                                return `<button class="btn btn-primary destinoFinal" title="Destino final."><i class="fas fa-people-carry"></i></button>`;
                            } else {
                                return ``;
                            }
                        },
                    },
                    {
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class="btn btn-warning editarCaptura" title="Editar registro."><i class="fas fa-pencil-alt"></i></button>`;
                            let btnEliminar = `<button class="btn btn-danger eliminarTransportista" title="Eliminar registro."><i class="fas fa-trash"></i></button>`;
                            let btnArchivos = `<button class="btn btn-primary verArchivos" title="Listado de archivos"><i class='fas fa-list'></i></button>`;
                            if (row.estatusCaptura != 3) {
                                return `${btnEditar} ${btnEliminar}`;
                            } else {
                                return `${btnArchivos} ${btnEliminar}`;
                            }
                        },
                    },
                    { data: 'estatusCaptura', visible: false },
                    { data: 'id', visible: false }
                ],
                initComplete: function (settings, json) {
                    tblS_MedioAmbienteCaptura.on('click', '.editarCaptura', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        fncLimpiarModalCECapturas();
                        fncFillCboAspectosAmbientales();
                        fncGetDatosActualizarCaptura(rowData.id);
                    });

                    tblS_MedioAmbienteCaptura.on('click', '.eliminarTransportista', function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCaptura(rowData.id));
                    });

                    tblS_MedioAmbienteCaptura.on("click", ".transportistas", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        fncFillCboTransportistas();
                        fncGetDatosActualizarTrayecto(rowData.id);
                    });

                    tblS_MedioAmbienteCaptura.on("click", ".destinoFinal", function () {
                        let rowData = dtCapturas.row($(this).closest('tr')).data();
                        fncFillCboTransportistas();
                        fncGetDatosActualizarDestinoFinal(rowData.id);
                    });

                    tblS_MedioAmbienteCaptura.on("click", ".verArchivos", function () {
                        let rowData = dtCapturas.row($(this).closest("tr")).data();
                        fncGetArchivosRelCapturas(rowData.id);
                    })
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { width: "25%", targets: [0] },
                    { width: "15%", targets: [5] },
                    { width: "10%", targets: [9] },
                ],
            });
        }

        function fncGetCapturas() {
            let obj = new Object();
            obj = {
                idAgrupacion: cboFiltroAgrupacion.val(),
                estatusCaptura: cboFiltroEstatusCaptura.val(),
                mesInicio: txtMesInicio.val(),
                mesFinal: txtMesFinal.val()
            }
            axios.post("GetCapturas", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtCapturas.clear();
                    dtCapturas.rows.add(response.data.data);
                    dtCapturas.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearEditarCaptura() {
            let obj = fncObjCECaptura();
            if (obj != "") {
                axios.post("CrearEditarCaptura", obj, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCapturas();
                        mdlCECaptura.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCECaptura() {
            fncBorderDefault();
            let strMensajeError = "";
            if (cboAgrupacion.val() == "") { $("#select2-cboAgrupacion-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboTecnicoResponsable.val() == "") { $("#select2-cboTecnicoResponsable-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtFechaEntrada.val()) == "") { txtFechaEntrada.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($("#txtCantContenedor").val() == "") { $("#txtCantContenedor").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtTipoContenedor.val()) == "") { txtTipoContenedor.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtPlantaProcesoGeneracion.val()) == "") { txtPlantaProcesoGeneracion.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            const archivoAcopio = txtArchivoAcopio.get(0).files[0];
            if (archivoAcopio == undefined && parseFloat(btnCECaptura.attr("data-id")) <= 0) {
                txtArchivoAcopio.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios.";
            }

            //#region SE OBTIENE LISTADO DE ASPECTOS AMBIENTALES CAPTURADOS.
            let datos = dtAspectosAmbientales.rows().data();
            let tipoCaptura = btnCECaptura.attr("data-tipoCaptura");
            $.each(datos, function (idx, data) {
                let row = tablaAspectosAmbientales.find('tbody tr').eq(idx);
                data.aspectoAmbientalID = $(row).find('.selectAspectoAmbiental').val();
                data.cantidad = $(row).find('.inputCantidad').val();
            });

            let arrCodigoContenedor = [];
            let arrAspectosAmbientalesID = [];
            let arrCantidadAspectosAmbientales = [];
            for (let i = 0; i < datos.length; i++) {
                let aspectoAmbientalID = datos[i].aspectoAmbientalID;
                arrAspectosAmbientalesID.push(aspectoAmbientalID);

                if (tipoCaptura != 2) {
                    let cantidadAspectoAmbiental = datos[i].cantidad;
                    if (cantidadAspectoAmbiental > 0) {
                        arrCantidadAspectosAmbientales.push(cantidadAspectoAmbiental);
                    }
                }
            }

            if (arrAspectosAmbientalesID.length <= 0 && (tipoCaptura != 2 ? arrCantidadAspectosAmbientales.length <= 0 : false)) {
                if (strMensajeError == "") {
                    strMensajeError += "Es necesario indicar lo siguiente:&nbsp;";
                    arrAspectosAmbientalesID.length <= 0 ? strMensajeError += "<br>-Indicar el aspecto ambiental." : "";
                    tipoCaptura != 2 ? arrCantidadAspectosAmbientales.length <= 0 ? strMensajeError += "<br>-Indicar la cantidad." : "" : "";
                } else {
                    strMensajeError += "<br>Es necesario indicar lo siguiente:&nbsp;";
                    arrAspectosAmbientalesID.length <= 0 ? strMensajeError += "<br>-Indicar el aspecto ambiental." : "";
                    tipoCaptura != 2 ? arrCantidadAspectosAmbientales.length <= 0 ? strMensajeError += "<br>-Indicar la cantidad." : "" : "";
                }
            }
            //#endregion

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCECaptura.attr("data-id"),
                    idAgrupacion: cboAgrupacion.val(),
                    idResponsableTecnico: cboTecnicoResponsable.val(),
                    fechaEntrada: txtFechaEntrada.val(),
                    cantidadContenedor: $("#txtCantContenedor").val(),
                    tipoContenedor: txtTipoContenedor.val(),
                    lstCodigosContenedores: arrCodigoContenedor,
                    lstAspectosAmbientalesID: arrAspectosAmbientalesID,
                    lstCantidadAspectosAmbientales: arrCantidadAspectosAmbientales,
                    plantaProcesoGeneracion: txtPlantaProcesoGeneracion.val(),
                    tipoCaptura: btnCECaptura.attr("data-tipoCaptura")
                };
                let formData = new FormData();
                formData.set('_objFile', archivoAcopio);
                formData.set('_objCEDTO', JSON.stringify(obj));
                return formData;
            }
        }

        function fncGetDatosActualizarCaptura(_idCaptura) {
            if (_idCaptura > 0) {
                let obj = new Object();
                obj = {
                    _idCaptura: _idCaptura
                }
                axios.post("GetDatosActualizarCaptura", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        lblTitleCECaptura.html("ACTUALIZAR REGISTRO");
                        cboAgrupacion.val(response.data.objCaptura.idAgrupacion);
                        cboAgrupacion.trigger("change");
                        cboTecnicoResponsable.val(response.data.objCaptura.idResponsableTecnico);
                        cboTecnicoResponsable.trigger("change");
                        txtFechaEntrada.val(moment(response.data.objCaptura.fechaEntrada).format("YYYY-MM-DD"));
                        txtCodigoContenedor.val(response.data.objCaptura.codigoContenedor);
                        txtTipoContenedor.val(response.data.objCaptura.tipoContenedor);

                        initTablaAspectosAmbientales(response.data.objCaptura.tipoCaptura);
                        switch (response.data.objCaptura.tipoCaptura) {
                            case 1:
                                btnCapturaTipoAspectoAmbiental.trigger("click");
                                break;
                            case 2:
                                btnCapturaTipoResiduoPeligroso.trigger("click");
                                break;
                            case 3:
                                btnCapturaTipoRsuRme.trigger("click");
                                break;
                        }
                        $("#txtCantContenedor").val(response.data.objCaptura.cantidadContenedor);
                        // dtAspectosAmbientales.clear();
                        // dtAspectosAmbientales.rows.add(response.data.objCapturaDet);
                        // dtAspectosAmbientales.draw();
                        if (dtAspectosAmbientales != undefined) {
                            tablaAspectosAmbientales.DataTable().clear().destroy();
                            initTablaAspectosAmbientales(response.data.objCaptura.tipoCaptura);
                            dtAspectosAmbientales.clear();
                            dtAspectosAmbientales.rows.add(response.data.objCapturaDet);
                            dtAspectosAmbientales.draw();
                        }

                        btnDescargarAcopio.attr("disabled", false);

                        txtPlantaProcesoGeneracion.val(response.data.objCaptura.plantaProcesoGeneracion);
                        btnCECaptura.attr("data-id", _idCaptura);
                        btnCECaptura.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                        mdlCECaptura.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al mostrar la información de la captura.");
            }
        }

        function fncEliminarCaptura(_idCaptura) {
            let obj = new Object();
            obj = {
                _idCaptura: _idCaptura
            }
            axios.post("EliminarCaptura", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    fncGetCapturas();
                    Alert2Exito(response.data.message);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncFillCboAspectosAmbientales() {
            cboAspectoAmbiental.fillCombo("FillCboAspectosAmbientales", {}, false);
            cboAspectoAmbiental.select2({ width: "100%" });
        }

        function initTablaAspectosAmbientales(tipoCaptura) {

            let obj1 = new Object();
            let obj2 = new Object();
            let obj3 = new Object();
            let arr = new Array();
            if (tipoCaptura == 2) {
                obj1 = {
                    className: "dt-center",
                    "targets": "_all"
                }

                obj2 = {
                    "targets": 2,
                    "visible": false
                }

                obj3 = {
                    "targets": 0,
                    "visible": true
                }

                arr.push(obj1);
                arr.push(obj2);
                arr.push(obj3);
            } else {
                obj1 = {
                    className: "dt-center",
                    "targets": "_all"
                }

                obj2 = {
                    "targets": 2,
                    "visible": true
                }

                obj3 = {
                    "targets": 0,
                    "visible": false
                }

                arr.push(obj1);
                arr.push(obj2);
                arr.push(obj3);
            }

            dtAspectosAmbientales = tablaAspectosAmbientales.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                columns: [
                    {
                        data: 'codigoContenedor', title: 'Código contenedor',
                        render: function (data, type, row) {
                            return `<input class="form-control inputCodigoContenedor" style="text-align: center;" value="${row.codigoContenedor}">`
                        },
                    },
                    {
                        data: 'aspectoAmbientalID', title: 'Aspecto ambiental',
                        render: function (data, type, row) {
                            return `<select class="form-control select2 selectAspectoAmbiental"></select>`;
                        },
                    },
                    {
                        data: 'cantidad', title: 'Cantidad',
                        render: function (data, type, row) {
                            return `<input class="form-control inputCantidad" style="text-align: center;" value="${row.cantidad}">`
                        },
                    },
                    {
                        data: 'unidadMedida', title: 'Unidad medida',
                        render: function (data, type, row) {
                            return `<input class="form-control inputUnidadMedida" style="text-align: center;" value="${row.unidadMedida}" disabled="disabled">`
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tablaAspectosAmbientales.on('click', 'td', function () {
                        let row = $(this).closest('tr');
                        if (row.hasClass('selected')) {
                            row.removeClass('selected');
                        } else {
                            dtAspectosAmbientales.$('tr.selected').removeClass('selected');
                            row.addClass('selected');
                        }
                    });

                    tablaAspectosAmbientales.on('focus', 'input', function () {
                        $(this).select();
                    });

                    tablaAspectosAmbientales.on("change", ".selectAspectoAmbiental, .inputUnidadMedida", function () {
                        let optionAspectoAmbiental = $(this).find(`option[value="${$(this).val()}"]`);

                        //#region SE MUESTRA QUE TIPO DE UNIDAD DE MEDIDA ES EL ASPECTO AMBIENTAL SELECCIONADO
                        let unidadMedida = optionAspectoAmbiental.attr("data-prefijo");
                        $(this).closest("tr").find(".inputUnidadMedida").val(unidadMedida);
                        $(this).closest("tr").find(".inputUnidadMedida").attr("title", unidadMedida);
                        //#endregion

                        //#region SE OBTIENE CODIGO CONTENEDOR CONSECTUVIO EN BASE A LA AGRUPACION Y ASPECTO AMBIENTAL SELECCIONADO
                        let tipoCaptura = btnCECaptura.attr("data-tipoCaptura");
                        if (tipoCaptura == 2) {
                            let obj = new Object();
                            obj = {
                                idAgrupacion: cboAgrupacion.val(),
                                idAspectoAmbiental: $(this).val()
                            }
                            axios.post("GetUltimoConsecutivoCodContenedor", obj).then(response => {
                                let { success, items, message } = response.data;
                                if (success) {
                                    $(this).closest("tr").find(".inputCodigoContenedor").val(response.data.codigoContenedor);
                                } else {
                                    Alert2Error(message);
                                }
                            }).catch(error => Alert2Error(error.message));
                        }
                        //#endregion
                    });
                },
                createdRow: function (row, rowData) {
                    let selectAspectoAmbiental = $(row).find('.selectAspectoAmbiental');
                    selectAspectoAmbiental.fillCombo('/Administrativo/MedioAmbiente/GetAspectosAmbientales', { tipoCaptura: btnCECaptura.attr("data-tipoCaptura") }, false, null);
                    selectAspectoAmbiental.select2();
                    selectAspectoAmbiental.select2({ width: "100%" });
                    selectAspectoAmbiental.find('option[value="' + rowData.aspectoAmbientalID + '"]').attr('selected', true);
                    selectAspectoAmbiental.trigger('change');
                },
                columnDefs: arr
            });
        }

        function fncLimpiarModalCECapturas() {
            $('input[type="text"]').val("");
            cboAgrupacion[0].selectedIndex = 0;
            cboAgrupacion.trigger("change");
            cboTecnicoResponsable[0].selectedIndex = 0;
            cboTecnicoResponsable.trigger("change");
            txtArchivoAcopio.val("");
        }

        function fncMostrarOcultarTipoCaptura(tipoCaptura) {
            switch (tipoCaptura) {
                case 1:
                    divCantContenedor.html(``);
                    break;
                case 2:
                    divCantContenedor.html(`<div class="input-group marginBottom">
                                                <span class="input-group-addon">Cant. contenedor</span>
                                                <input id="txtCantContenedor" type="text" class="form-control" autocomplete="off" value="0" disabled="disabled" />
                                            </div>`);
                    break;
                case 3:
                    divCantContenedor.html(``);
                    break;
                default:
                    break;
            }
        }

        function fncCantContenedor(nuevoAspectoAmbiental) {
            let cantContenedor = $("#txtCantContenedor").val();
            let nuevoCantContenedor = 0;
            if (nuevoAspectoAmbiental) {
                nuevoCantContenedor = parseFloat(cantContenedor) + 1;
            } else {
                nuevoCantContenedor = parseFloat(cantContenedor) - 1;
            }

            if (nuevoCantContenedor > 0) {
                $("#txtCantContenedor").val(nuevoCantContenedor);
            } else {
                $("#txtCantContenedor").val(0);
            }
        }
        //#endregion

        //#region CRUD TRAYECTOS
        function initTblAspectosAmbientalesAcopioToTrayectos() {
            dtTrayectos = tblAspectosAmbientalesAcopioToTrayectos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'codigoContenedor', title: 'Código contenedor' },
                    { data: 'idAspectoAmbiental', visible: false },
                    { data: 'aspectoAmbiental', title: 'Aspecto ambiental' },
                    {
                        data: "id", visible: false,
                        render: function (data, type, row, meta) {
                            return `<input class="form-control inputAspectoAmbientalID" style="text-align: center;" value="${row.id}">`;
                        },
                    },
                ],
                initComplete: function (settings, json) {
                    tblAspectosAmbientalesAcopioToTrayectos.on('click', '.classBtn', function () {
                        let rowData = dtTrayectos.row($(this).closest('tr')).data();
                    });
                    tblAspectosAmbientalesAcopioToTrayectos.on('click', '.classBtn', function () {
                        let rowData = dtTrayectos.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetAspectosAmbientalesToTrayectos() {
            let obj = new Object();
            obj.idAgrupacion = cboAgrupacionTrayecto.val();
            obj.consecutivo = txtConsecutivoTrayecto.val();
            axios.post("GetAspectosAmbientalesToTrayectos", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtTrayectos.clear();
                    dtTrayectos.rows.add(response.data.lstAspectosAmbientales);
                    dtTrayectos.draw();
                    btnCETrayecto.data().idCaptura = response.data.lstAspectosAmbientales[0].idCaptura;
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncGetDatosActualizarTrayecto(_idCaptura) {
            if (_idCaptura > 0) {
                let obj = new Object();
                obj = {
                    _idCaptura: _idCaptura
                }
                axios.post("GetDatosActualizarTrayecto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {

                        fncLimpiarModalCETrayecto();

                        if (response.data.objCaptura != "") {
                            lblTitleCETrayecto.html("ACTUALIZAR REGISTRO");
                            txtTratamiento.val(response.data.objCaptura.tratamiento);
                            txtManifiesto.val(response.data.objCaptura.manifiesto);
                            txtFechaEmbarque.val(moment(response.data.objCaptura.fechaEmbarque).format("YYYY-MM-DD"));
                            txtTipoTransporte.val(response.data.objCaptura.tipoTransporte);
                            cboTransportista.val(response.data.objCaptura.idTransportistaTrayecto);
                            cboTransportista.trigger("change");
                            btnCETrayecto.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                            btnCETrayecto.attr("registroNuevo", 0);
                            btnDescargarTrayecto.attr("disabled", false);
                        } else {
                            lblTitleCETrayecto.html("NUEVO REGISTRO");
                            btnCETrayecto.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                            btnCETrayecto.attr("registroNuevo", 1);
                            btnDescargarTrayecto.attr("disabled", true);
                        }
                        btnCETrayecto.attr("data-id", _idCaptura);
                        mdlCETrayecto.modal("show");

                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al mostrar la información del trayecto.");
            }
        }

        function fncCrearEditarTrayecto() {
            let obj = fncObjCETrayecto();
            if (obj != "") {
                axios.post("CrearEditarTrayecto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCapturas();
                        mdlCETrayecto.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCETrayecto() {
            fncBorderDefault();
            let strMensajeError = "";
            if ($.trim(txtManifiesto.val()) == "") { txtManifiesto.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtTratamiento.val()) == "") { txtTratamiento.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtFechaEmbarque.val()) == "") { txtFechaEmbarque.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtTipoTransporte.val()) == "") { txtTipoTransporte.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboTransportista.val() == "") { $("#select2-cboTransportista-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            const archivoTrayecto = txtArchivoTrayecto.get(0).files[0];
            if (archivoTrayecto == undefined && btnCETrayecto.attr("registroNuevo") == 1) {
                txtArchivoTrayecto.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios.";
            }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCETrayecto.attr("data-id"),
                    tratamiento: txtTratamiento.val(),
                    manifiesto: txtManifiesto.val(),
                    fechaEmbarque: txtFechaEmbarque.val(),
                    tipoTransporte: txtTipoTransporte.val(),
                    idTransportistaTrayecto: cboTransportista.val()
                };
                let formData = new FormData();
                formData.set('_objFile', archivoTrayecto);
                formData.set('_objCEDTO', JSON.stringify(obj));
                return formData;
            }
        }

        function fncLimpiarModalCETrayecto() {
            $('input[type="text"]').val("");
            txtFechaEmbarque.val("");
            cboTransportista[0].selectedIndex = 0;
            cboTransportista.trigger("change")
            txtArchivoTrayecto.val("");
            if (dtTrayectos != undefined) {
                tblAspectosAmbientalesAcopioToTrayectos.DataTable().clear().destroy();
            }
            initTblAspectosAmbientalesAcopioToTrayectos();
        }

        function fncCrearAspectoAmbientalAcopioToTrayecto() {
            let obj = fncObjCAspectoAmbientalAcopioToTrayecto();
            if (obj != "") {
                axios.post("CrearAspectoAmbientalAcopioToTrayecto", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        mdlCETrayecto.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCAspectoAmbientalAcopioToTrayecto() {
            fncBorderDefault();
            let strMensajeError = "";
            if (cboAgrupacionTrayecto.val() == "") { $("#select2-cboAgrupacionTrayecto-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtTratamiento.val()) == "") { txtTratamiento.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtManifiesto.val()) == "") { txtManifiesto.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtFechaEmbarque.val()) == "") { txtFechaEmbarque.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if ($.trim(txtTipoTransporte.val()) == "") { txtTipoTransporte.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboTransportista.val() == "") { $("#select2-cboTransportista-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }

            //#region SE OBTIENE ARCHIVO
            const archivoTrayecto = txtArchivoTrayecto.get(0).files[0];
            if (archivoTrayecto == undefined && btnCETrayecto.attr("registroNuevo") == 1) {
                txtArchivoTrayecto.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios.";
            }
            //#endregion

            //#region SE OBTIENE LOS ID'S DE LOS ASPECTOS AMBIENTALES SELECCIONADOS
            let arrAspectosAmbientalesID = [];
            let rowsSeleccionados = $("#tblAspectosAmbientalesAcopioToTrayectos").DataTable().rows('.selected').data();
            for (let i = 0; i < rowsSeleccionados.length; i++) {
                arrAspectosAmbientalesID.push(rowsSeleccionados[i].id);
            }

            if (arrAspectosAmbientalesID.length <= 0) {
                strMensajeError == "" ?
                    strMensajeError = "Es necesario seleccionar al menos un aspecto ambiental." :
                    strMensajeError += "<br>Es necesario seleccionar al menos un aspecto ambiental.";
            }
            //#endregion

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCETrayecto.data().idCaptura,
                    idAgrupacion: cboAgrupacionTrayecto.val(),
                    lstAspectosAmbientalesID: arrAspectosAmbientalesID,
                    tratamiento: txtTratamiento.val(),
                    manifiesto: txtManifiesto.val(),
                    fechaEmbarque: txtFechaEmbarque.val(),
                    tipoTransporte: txtTipoTransporte.val(),
                    idTransportistaTrayecto: cboTransportista.val()
                };
                let formData = new FormData();
                formData.set('objFile', archivoTrayecto);
                formData.set('obj', JSON.stringify(obj));
                return formData;
            }
        }
        //#endregion

        //#region CRUD DESTINO FINAL
        function fncGetDatosActualizarDestinoFinal(_idCaptura) {
            if (_idCaptura > 0) {
                let obj = new Object();
                obj = {
                    _idCaptura: _idCaptura
                }
                axios.post("GetDatosActualizarDestinoFinal", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {

                        fncLimpiarModalCEDestinoFinal();

                        if (response.data.objCaptura != "") {
                            lblTitleCEDestinoFinal.html("ACTUALIZAR REGISTRO");
                            txtFechaDestinoFinal.val(moment(response.data.objCaptura.fechaDestinoFinal).format("YYYY-MM-DD"));
                            cboTransportistaDestinoFinal.val(response.data.objCaptura.idTransportistaDestinoFinal);
                            cboTransportistaDestinoFinal.trigger("change");
                            btnCEDestinoFinal.html(`<i class="fas fa-save"></i>&nbsp;Actualizar`);
                            btnCEDestinoFinal.attr("registroNuevo", 0);
                            btnDescargarDestinoFinal.attr("disabled", false);
                        } else {
                            lblTitleCEDestinoFinal.html("NUEVO REGISTRO");
                            btnCEDestinoFinal.html(`<i class="fas fa-save"></i>&nbsp;Guardar`);
                            btnCEDestinoFinal.attr("registroNuevo", 1);
                            btnDescargarDestinoFinal.attr("disabled", true);
                        }
                        btnCEDestinoFinal.attr("data-id", _idCaptura);
                        mdlCEDestinoFinal.modal("show");

                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al mostrar la información del destino final.");
            }
        }

        function fncCrearEditarDestinoFinal() {
            let obj = fncObjCEDestinoFinal();
            if (obj != "") {
                axios.post("CrearEditarDestinoFinal", obj).then(response => { // v1
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCapturas();
                        mdlCEDestinoFinal.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCrearDestinoFinal() {
            let obj = fncObjCDestinoFinal();
            if (obj != "") {
                axios.post("CrearDestinoFinal", obj).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        fncGetCapturas();
                        mdlCEDestinoFinal.modal("hide");
                        Alert2Exito(response.data.message);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncObjCEDestinoFinal() {
            fncBorderDefault();
            let strMensajeError = "";
            if (txtFechaDestinoFinal.val() == "") { txtFechaDestinoFinal.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboTransportistaDestinoFinal.val() == "") {
                $("#select2-cboTransportistaDestinoFinal-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios.";
            }
            const archivoDestinoFinal = txtArchivoDestinoFinal.get(0).files[0];
            if (archivoDestinoFinal == undefined && btnCEDestinoFinal.attr("registroNuevo") == 1) {
                txtArchivoDestinoFinal.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios.";
            }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEDestinoFinal.attr("data-id"),
                    fechaDestinoFinal: txtFechaDestinoFinal.val(),
                    idTransportistaDestinoFinal: cboTransportistaDestinoFinal.val()
                };
                let formData = new FormData();
                formData.set('_objFile', archivoDestinoFinal);
                formData.set('_objCEDTO', JSON.stringify(obj));
                return formData;
            }
        }

        function fncObjCDestinoFinal() {
            fncBorderDefault();
            let strMensajeError = "";
            if (cboAgrupacionDestinoFinal.val() == "") { cboAgrupacionDestinoFinal.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (txtFechaDestinoFinal.val() == "") { txtFechaDestinoFinal.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios."; }
            if (cboTransportistaDestinoFinal.val() == "") {
                $("#select2-cboTransportistaDestinoFinal-container").css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios.";
            }
            const archivoDestinoFinal = txtArchivoDestinoFinal.get(0).files[0];
            if (archivoDestinoFinal == undefined && btnCEDestinoFinal.attr("registroNuevo") == 1) {
                txtArchivoDestinoFinal.css("border", "2px solid red"); strMensajeError = "Es necesario llenar los campos obligatorios.";
            }

            //#region SE OBTIENE LOS ID'S DE LOS ASPECTOS AMBIENTALES SELECCIONADOS
            let datos = dtDestinoFinal.rows().data();
            $.each(datos, function (idx, data) {
                let row = tblAspectosAmbientalesTrayectoToDestinoFinal.find('tbody tr').eq(idx);
                data.id = $(row).find('.inputAspectoAmbientalID').val();
                data.cantidad = $(row).find('.inputCantidadDestinoFinal').val();
            });

            let arrAspectosAmbientalesID = [];
            let arrCantidadAspectosAmbientales = [];
            for (let i = 0; i < datos.length; i++) {
                let aspectoAmbientalID = datos[i].id;
                let cantidadAspectoAmbiental = datos[i].cantidad;

                arrAspectosAmbientalesID.push(aspectoAmbientalID);
                arrCantidadAspectosAmbientales.push(cantidadAspectoAmbiental)
            }

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                return "";
            } else {
                let obj = new Object();
                obj = {
                    id: btnCEDestinoFinal.data().idCaptura,
                    idAgrupacion: cboAgrupacionDestinoFinal.val(),
                    fechaDestinoFinal: txtFechaDestinoFinal.val(),
                    idTransportistaDestinoFinal: cboTransportistaDestinoFinal.val(),
                    lstAspectosAmbientales: arrAspectosAmbientalesID,
                    lstCantidad: arrCantidadAspectosAmbientales
                };
                let formData = new FormData();
                formData.set('objFile', archivoDestinoFinal);
                formData.set('objCDTO', JSON.stringify(obj));
                return formData;
            }
            //#endregion
        }

        function fncLimpiarModalCEDestinoFinal() {
            txtFechaDestinoFinal.val("");
            cboTransportistaDestinoFinal[0].selectedIndex = 0;
            cboTransportistaDestinoFinal.trigger("change");
            txtArchivoDestinoFinal.val("");
            if (dtDestinoFinal != undefined) {
                tblAspectosAmbientalesTrayectoToDestinoFinal.DataTable().clear().destroy();
            }
            initTblAspectosAmbientalesTrayectoToDestinoFinal();
        }

        function initTblAspectosAmbientalesTrayectoToDestinoFinal() {
            dtDestinoFinal = tblAspectosAmbientalesTrayectoToDestinoFinal.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'codigoContenedor', title: 'Código contenedor' },
                    { data: 'idAspectoAmbiental', visible: false },
                    { data: 'aspectoAmbiental', title: 'Aspecto ambiental' },
                    {
                        data: 'cantidad', title: 'Cantidad',
                        render: function (data, type, row) {
                            return `<input class="form-control inputCantidadDestinoFinal" style="text-align: center;">`;
                        }
                    },
                    {
                        data: "id", visible: true,
                        render: function (data, type, row, meta) {
                            if (data > 0) {
                                return `<input style="width: 1px;" class="form-control inputAspectoAmbientalID" style="text-align: center;" value="${row.id}" disabled="disabled">`;
                            } else {
                                data = row.idRP
                                return `<input style="width: 1px;" class="form-control inputAspectoAmbientalID" style="text-align: center;" value="${data}" disabled="disabled">`;
                            }
                        },
                    },
                    { data: 'idRP', title: 'idRP', visible: false },
                ],
                initComplete: function (settings, json) {
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' },
                    { "width": "1%", "targets": 4 },
                ],
            });
        }

        function fncGetAspectosAmbientalesToDestinoFinal() {
            let obj = new Object();
            obj.idAgrupacion = cboAgrupacionDestinoFinal.val()
            obj.consecutivo = txtConsecutivoDestinoFinal.val()
            axios.post("GetAspectosAmbientalesToDestinoFinal", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtDestinoFinal.clear();
                    dtDestinoFinal.rows.add(response.data.lstAspectosAmbientales);
                    dtDestinoFinal.draw();
                    btnCEDestinoFinal.data().idCaptura = response.data.lstAspectosAmbientales[0].idCaptura;
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region ARCHIVOS REL CAPTURAS
        function initTblArchivos() {
            dtArchivos = tblArchivos.DataTable({
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    { data: 'nombreArchivo', title: 'Archivo' },
                    { data: 'tipoArchivoDesc', title: 'Tipo archivo' },
                    {
                        render: (data, type, row, meta) => {
                            let btnVisualizar = `<button class="btn btn-primary visualizarArchivo" title="Visualizar archivo."><i class="fas fa-eye"></i></button>`;
                            let btnDescargar = `<button class="btn btn-primary descargarArchivo" title="Descargar archivo."><i class="fas fa-download"></i></button>`;
                            return `${btnVisualizar} ${btnDescargar}`;
                        }
                    },

                ],
                initComplete: function (settings, json) {
                    tblArchivos.on('click', '.visualizarArchivo', function () {
                        let rowData = dtArchivos.row($(this).closest('tr')).data();
                        fncVisualizarArchivo(rowData.id);
                    });

                    tblArchivos.on("click", ".descargarArchivo", function () {
                        let rowData = dtArchivos.row($(this).closest("tr")).data();
                        fncDescargarArchivo(rowData.idCaptura, rowData.tipoArchivo);
                    })
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetArchivosRelCapturas(idCaptura) {
            let obj = new Object();
            obj._idCaptura = idCaptura;
            axios.post('GetArchivosRelCapturas', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtArchivos.clear();
                    dtArchivos.rows.add(response.data.lstArchivos);
                    dtArchivos.draw();
                    mdlArchivos.modal("show");
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncVisualizarArchivo(idArchivo) {
            let obj = new Object();
            obj.idArchivo = idArchivo
            axios.post("VisualizarArchivo", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $('#myModal').data().ruta = null;
                    $('#myModal').modal('show');
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }
        //#endregion

        //#region FUNCIONES GENERALES
        function fncBorderDefault() {
            //#region CAPTURA
            $("#select2-cboAgrupacion-container").css("border", "1px solid #CCC");
            $("#select2-cboTecnicoResponsable-container").css("border", "1px solid #CCC");
            txtFechaEntrada.css("border", "1px solid #CCC");
            txtCantContenedor.css("border", "1px solid #CCC");
            txtCodigoContenedor.css("border", "1px solid #CCC");
            txtTipoContenedor.css("border", "1px solid #CCC");
            $("#select2-cboAspectoAmbiental-container").css("border", "1px solid #CCC");
            txtCantResiduo.css("border", "1px solid #CCC");
            txtPlantaProcesoGeneracion.css("border", "1px solid #CCC");
            txtArchivoAcopio.css("border", "1px solid #CCC");
            //#endregion

            //#region TRAYECTO
            cboAgrupacionTrayecto.css("border", "1px solid #CCC");
            txtTratamiento.css("border", "1px solid #CCC");
            txtManifiesto.css("border", "1px solid #CCC");
            txtFechaEmbarque.css("border", "1px solid #CCC");
            txtTipoTransporte.css("border", "1px solid #CCC");
            $("#select2-cboAgrupacionTrayecto-container").css("border", "1px solid #CCC");
            txtArchivoTrayecto.css("border", "1px solid #CCC");
            //#endregion

            //#region DESTINO FINAL
            txtFechaDestinoFinal.css("border", "1px solid #CCC");
            $("#select2-cboTransportistaDestinoFinal-container").css("border", "1px solid #CCC");
            txtArchivoDestinoFinal.css("border", "1px solid #CCC");
            //#endregion
        }

        function fncFillCboTransportistas() {
            cboTransportista.fillCombo("FillCboTransportistas", {}, false);
            cboTransportista.select2({ width: "100%" });

            cboTransportistaDestinoFinal.fillCombo("FillCboTransportistas", {}, false);
            cboTransportistaDestinoFinal.select2({ width: "100%" });
        }

        function fncDescargarArchivo(_idCaptura, _tipoArchivo) {
            let obj = {
                _idCaptura: _idCaptura,
                _tipoArchivo: _tipoArchivo
            }
            const filtro = JSON.stringify(obj);
            const data = new FormData();
            data.append('filtro', filtro);

            var xhr = new XMLHttpRequest();
            xhr.responseType = 'blob';
            xhr.onloadstart = () => {
                $.blockUI({
                    message: 'Procesando...',
                    baseZ: 2000
                });
            }

            xhr.onload = () => {
                if (xhr.status == 200) {
                    var blob = xhr.response;

                    if (blob.size > 0) {
                        var fileName = xhr.getResponseHeader('content-disposition');
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        link.download = fileName.split('filename=')[1];
                        link.download = link.download.replaceAll(`"`, "");
                        link.click();
                        Alert2Exito("Se ha descargado con éxito el archivo.");
                    } else {
                        Alert2Error('Ocurrió un error al descargar el archivo.');
                    }
                } else {
                    Alert2Error(`Ocurrió un error al lanzar la petición al servidor. ${xhr.status} ${xhr.statusText}`);
                }
            }

            xhr.onerror = () => {
                Alert2Error(`Ocurrió un error al lanzar la petición al servidor.`);
            }

            xhr.addEventListener('loadend', () => {
                $.unblockUI();
            });

            xhr.open('POST', 'DescargarArchivo');
            xhr.send(data);
        }

        function initMonthPicker(input) {
            $(input).datepicker({
                dateFormat: "mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                maxDate: fechaActual,
                showAnim: showAnim,
                closeText: "Aceptar",
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                        $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                    }
                },
                beforeShow: function (input, inst) {
                    inst.dpDiv.addClass('month_year_datepicker')

                    if ((datestr = $(this).val()).length > 0) {
                        year = datestr.substring(datestr.length - 4, datestr.length);
                        month = datestr.substring(0, 2);
                        $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                        $(this).datepicker('setDate', new Date(year, month - 1, 1));
                        $(".ui-datepicker-calendar").hide();
                    }
                }
            }).datepicker("setDate", fechaActual);
        }
        //#endregion
    }

    $(document).ready(() => {
        CH.Captura = new Captura();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();