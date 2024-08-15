(() => {
    $.namespace('ADMIN_FINANZAS.Proyectos');

    Proyectos = function () {

        //#region CONST FILTROS
        const cboFiltro_TipoFiltro = $('#cboFiltro_TipoFiltro');
        const cboFiltro_Busqueda = $('#cboFiltro_Busqueda');
        const btnFiltro_Buscar = $("#btnFiltro_Buscar");
        const btnFiltro_Nuevo = $("#btnFiltro_Nuevo");
        const cboFiltro_Proyecto_Prospeccion = $('#cboFiltro_Proyecto_Prospeccion');
        const cboFiltro_Proyecto_LaborVenta = $('#cboFiltro_Proyecto_LaborVenta');
        const cboFiltro_Proyecto_Cotizacion = $('#cboFiltro_Proyecto_Cotizacion');
        const cboFiltro_Proyecto_Negociacion = $('#cboFiltro_Proyecto_Negociacion');
        const cboFiltro_Proyecto_Cierre = $('#cboFiltro_Proyecto_Cierre');

        const TipoFiltrosEnum = { CLIENTE: 1, DIVISION: 2, RESPONSABLE: 3, PRIORIDAD: 4 }
        //#endregion

        //#region CONST CREAR/EDITAR PROYECTO
        const mdlCE_Proyecto = $("#mdlCE_Proyecto");
        const txtCE_Proyecto_NombreProyecto = $("#txtCE_Proyecto_NombreProyecto");
        const cboCE_Proyecto_Cliente = $("#cboCE_Proyecto_Cliente");
        const txtCE_Proyecto_Cliente = $('#txtCE_Proyecto_Cliente');
        const div_cboCE_Proyecto_Cliente = $('#div_cboCE_Proyecto_Cliente');
        const div_txtCE_Proyecto_Cliente = $('#div_txtCE_Proyecto_Cliente');
        const cboCE_Proyecto_Prioridad = $("#cboCE_Proyecto_Prioridad");
        const cboCE_Proyecto_Division = $("#cboCE_Proyecto_Division");
        const cboCE_Proyecto_Pais = $('#cboCE_Proyecto_Pais');
        const cboCE_Proyecto_Estado = $('#cboCE_Proyecto_Estado');
        const cboCE_Proyecto_Municipio = $('#cboCE_Proyecto_Municipio');
        const txtCE_Proyecto_Importe = $("#txtCE_Proyecto_Importe");
        const txtCE_Proyecto_FechaInicio = $("#txtCE_Proyecto_FechaInicio");
        const cboCE_Proyecto_Estatus = $("#cboCE_Proyecto_Estatus");
        const cboCE_Proyecto_Escenario = $("#cboCE_Proyecto_Escenario");
        const cboCE_Proyecto_Responsable = $("#cboCE_Proyecto_Responsable");
        const txtCE_Proyecto_Contacto = $('#txtCE_Proyecto_Contacto');
        const txtCE_Proyecto_DescripcionObra = $('#txtCE_Proyecto_DescripcionObra');
        const div_txtCE_Proyecto_Contacto = $('#div_txtCE_Proyecto_Contacto');
        const divProyectosProspeccion = $('#divProyectosProspeccion');
        const divProyectosLaborVenta = $("#divProyectosLaborVenta");
        const divProyectosCotizacion = $("#divProyectosCotizacion");
        const divProyectosNegociacion = $("#divProyectosNegociacion");
        const divProyectosCierre = $("#divProyectosCierre");
        const btnCE_Proyecto = $("#btnCE_Proyecto");
        const btnEliminarProyecto = $('#btnEliminarProyecto');
        const btnActualizarProyecto = $('#btnActualizarProyecto');
        const cboCE_Proyecto_Riesgo = $('#cboCE_Proyecto_Riesgo');

        const PrioridadEnum = { Prospeccion: 1, LaborDeVenta: 2, Cotizacion: 3, Negociacion: 4, Cierre: 5 }
        const EscenariosEnum = { A: 1, B: 2, C: 3, D: 4, E: 5, T: 6 };
        const EstatusEnum = { Ganado: 12, Perdido: 13 }
        //#endregion

        //#region CONST RESUMEN PROYECTO
        const mdlResumen_Proyecto = $('#mdlResumen_Proyecto');
        const lblResumen_Generales_Cliente = $("#lblResumen_Generales_Cliente");
        const lblResumen_Generales_TipoCliente = $('#lblResumen_Generales_TipoCliente');
        const lblResumen_Generales_Division = $("#lblResumen_Generales_Division");
        const lblResumen_Generales_ImporteAprox = $("#lblResumen_Generales_ImporteAprox");
        const lblResumen_Generales_Ubicacion = $("#lblResumen_Generales_Ubicacion");
        const lblResumen_Generales_FechaInicio = $("#lblResumen_Generales_FechaInicio");
        const lblResumen_Generales_Estatus = $("#lblResumen_Generales_Estatus");
        const lblResumen_Generales_Escenario = $("#lblResumen_Generales_Escenario");
        const lblResumen_Generales_Prioridad = $("#lblResumen_Generales_Prioridad");
        const lblResumen_Generales_Responsable = $("#lblResumen_Generales_Responsable");
        const lblResumen_Generales_Riesgo = $("#lblResumen_Generales_Riesgo");
        const lblResumen_Generales_DescripcionObra = $('#lblResumen_Generales_DescripcionObra');
        const divResumen_Contactos = $("#divResumen_Contactos");
        const lblResumen_UltimoComentario = $("#lblResumen_UltimoComentario");
        const lblResumen_ProximaAccion_Accion = $('#lblResumen_ProximaAccion_Accion');
        const lblResumen_ProximaAccion_Fecha = $("#lblResumen_ProximaAccion_Fecha");
        const lblResumen_ProximaAccion_Responsable = $("#lblResumen_ProximaAccion_Responsable");
        const lblResumen_ProximaAccion_Progreso = $("#lblResumen_ProximaAccion_Progreso");
        const mdlResumen_Proyecto_Title = $('#mdlResumen_Proyecto > div > div > div.modal-header.text-center > h4');
        //#endregion

        //#region CONST COMENTARIOS PROYECTO
        let dtResumenComentarios;
        const tblComentarios = $('#tblComentarios');
        const btnFiltro_Comentario = $("#btnFiltro_Comentario");
        const mdlCE_Comentario = $("#mdlCE_Comentario");
        const txtCE_Comentario = $("#txtCE_Comentario");
        const btnCE_Comentario = $("#btnCE_Comentario");
        const mdlCE_Comentario_Title = $("#mdlCE_Comentario > div > div > div.modal-header.text-center > h4");
        //#endregion

        //#region CONST ACCIONES PROYECTO
        let dtResumenAcciones;
        const tblAcciones = $('#tblAcciones');
        const btnFiltro_Accion = $("#btnFiltro_Accion");
        const mdlCE_Accion = $("#mdlCE_Accion");
        const txtCE_Accion_Accion = $("#txtCE_Accion_Accion");
        const txtCE_Accion_FechaAccion = $("#txtCE_Accion_FechaAccion");
        const cboCE_Accion_ResponsableAccion = $("#cboCE_Accion_ResponsableAccion");
        const txtCE_Accion_Progreso = $("#txtCE_Accion_Progreso");
        const btnCE_Accion = $('#btnCE_Accion');
        const mdlCE_Accion_Title = $("#mdlCE_Accion > div > div > div.modal-header.text-center > h4");

        //#region CONST ACCIONES DETALLE PROYECTO
        let dtResumenAccionesDet;
        const tblAccionesDet = $('#tblAccionesDet');
        const mdlListado_AccionesDet = $('#mdlListado_AccionesDet');
        const mdlCE_AccionDet = $("#mdlCE_AccionDet");
        const txtCE_AccionDet_Accion = $("#txtCE_AccionDet_Accion");
        const txtCE_AccionDet_FechaAccion = $("#txtCE_AccionDet_FechaAccion");
        const txtCE_AccionDet_Progreso = $("#txtCE_AccionDet_Progreso");
        const cboCE_AccionDet_ResponsableAccion = $("#cboCE_AccionDet_ResponsableAccion");
        const btnCE_AccionDet = $("#btnCE_AccionDet");
        const btnFiltro_AccionDet = $('#btnFiltro_AccionDet');
        const mdlCE_AccionDet_Title = $('#mdlCE_AccionDet > div > div > div.modal-header.text-center > h4');
        //#endregion
        //#endregion

        //#region CONST COTIZACIONES
        let dtCotizaciones;
        const tblCotizaciones = $('#tblCotizaciones');
        const btnFiltro_Cotizacion = $('#btnFiltro_Cotizacion');
        const mdlCE_Cotizacion = $("#mdlCE_Cotizacion");
        const cboCE_Cotizacion_Responsable = $("#cboCE_Cotizacion_Responsable");
        const txtCE_Cotizacion_ImporteFinal = $("#txtCE_Cotizacion_ImporteFinal");
        const txtCE_Cotizacion_fechaFinal = $("#txtCE_Cotizacion_fechaFinal");
        const txtCE_Cotizacion_ImporteRevN = $("#txtCE_Cotizacion_ImporteRevN");
        const txtCE_Cotizacion_fechaRevN = $("#txtCE_Cotizacion_fechaRevN");
        const txtCE_Cotizacion_ImporteOriginal = $("#txtCE_Cotizacion_ImporteOriginal");
        const txtCE_Cotizacion_fechaOriginal = $("#txtCE_Cotizacion_fechaOriginal");
        const txtCE_Cotizacion_Comentario = $('#txtCE_Cotizacion_Comentario');
        const btnCE_Cotizacion = $("#btnCE_Cotizacion");
        //#endregion

        (function init() {
            fncListeners();
        })();

        function fncListeners() {
            //#region INIT
            initTblAcciones();
            initTblAccionesDetalleDet();
            initTblCotizaciones();
            fncGetProyectos();
            fncFillCbos();
            fncFillCbosProyectoEtapas();
            fncIndicarMenuSeleccion();
            //#endregion

            //#region FILTROS
            cboFiltro_TipoFiltro.change(function () {
                if ($(this).val() > 0) {
                    cboFiltro_Busqueda.fillCombo('FillCboTipoBusqueda', { tipoFiltroEnum: $(this).val() }, false, null);
                } else {
                    $("#cboFiltro_Busqueda option").remove();
                }
            });

            btnFiltro_Nuevo.click(function () {
                btnCE_Proyecto.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_Proyecto.data().id = 0;
                fncLimpiarMdlCE_Proyecto();
                cboCE_Proyecto_Cliente.data().mostrar = true;
                cboCE_Proyecto_Cliente.fillCombo('FillCboClientes', null, false, null);
                mdlCE_Proyecto.modal("show");
            });

            btnFiltro_Buscar.click(function () {
                fncGetProyectos();
            });

            cboFiltro_Proyecto_Prospeccion.change(function () { fncGetProyectos(); });
            cboFiltro_Proyecto_LaborVenta.change(function () { fncGetProyectos(); });
            cboFiltro_Proyecto_Cotizacion.change(function () { fncGetProyectos(); });
            cboFiltro_Proyecto_Negociacion.change(function () { fncGetProyectos(); });
            cboFiltro_Proyecto_Cierre.change(function () { fncGetProyectos(); });
            //#endregion

            //#region CREAR/EDITAR PROYECTO
            btnCE_Proyecto.click(function () {
                if ($(this).data().id <= 0) {
                    fncCrearProyecto();
                } else {
                    fncActualizarProyecto();
                }
            });

            cboCE_Proyecto_Prioridad.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Proyecto_Estatus.fillCombo('FillCboPrioridadesEstatus', { FK_Prioridad: $(this).val() }, false, null);
                }
            });

            cboCE_Proyecto_Pais.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Proyecto_Estado.fillCombo('FillCboEstados', { FK_Pais: $(this).val() }, false, null);
                }
            });

            cboCE_Proyecto_Estado.change(function () {
                if ($(this).val() > 0) {
                    cboCE_Proyecto_Municipio.fillCombo('FillCboMunicipios', { FK_Estado: $(this).val() }, false, null);
                }
            });

            $(".fncChangeCliente").click(function () {
                fncChangeCliente();
            });

            btnEliminarProyecto.click(function () {
                Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el proyecto?', 'Confirmar', 'Cancelar', () => fncEliminarProyecto());
            });

            btnActualizarProyecto.click(function () {
                fncGetDatosActualizarProyecto(mdlResumen_Proyecto.data().idProyecto);
            });

            cboCE_Proyecto_Division.change(function () {
                if ($(this).val() > 0) {
                    let opcionResponsable = cboCE_Proyecto_Division.find(`option[value="${$(this).val()}"]`);
                    cboCE_Proyecto_Responsable.val(opcionResponsable.attr("data-prefijo"));
                } else {
                    cboCE_Proyecto_Responsable[0].selectedIndex = 0;
                }
                cboCE_Proyecto_Responsable.trigger("change");
            });
            //#endregion

            //#region RESUMEN PROYECTO
            let contenedor = document.getElementById("divSeguimiento");
            contenedor.addEventListener("click", function (event) {
                if (event.target.classList.contains("boton")) {
                    let btnProyecto = event.target;
                    mdlResumen_Proyecto.data().idProyecto = btnProyecto.getAttribute("data-id");
                    fncGetResumenProyecto();
                }
            });

            $(".tablinks").click(function (event) {
                let tab = $(this).data("tab");
                fncMostrarOcultarTab(event, tab);
            });

            cboCE_Proyecto_Prioridad.change(function () {
                if ($(this).val() > 0) {
                    fncSetEscenario($(this).val());
                }
            });

            cboCE_Proyecto_Estatus.change(function () {
                if ($(this).val() > 0) {
                    fncSetEscenario(cboCE_Proyecto_Prioridad.val());
                }
            });
            //#endregion

            //#region COMENTARIOS PROYECTO
            btnFiltro_Comentario.click(function () {
                fncLimpiarMdlCE_Comentario();
                mdlCE_Comentario_Title.html("Crear comentario");
                mdlCE_Comentario.modal("show");
            });

            btnCE_Comentario.click(function () {
                fncCrearComentario();
            });

            let contenedorComentarios = document.getElementById("divComentarios");
            contenedorComentarios.addEventListener("click", function (event) {
                if (event.target.classList.contains("btnEliminarComentario")) {
                    let btnEliminarComentario = event.target;
                    let idComentario = btnEliminarComentario.getAttribute("data-id");
                    if (idComentario > 0) {
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el comentario seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarComentario(idComentario));
                    }
                }
            });
            //#endregion

            //#region ACCIONES PROYECTO
            btnFiltro_Accion.click(function () {
                fncLimpiarMdlCE_Accion();
                txtCE_Accion_Progreso.val(0);
                mdlCE_Accion_Title.html("Crear acción");
                mdlCE_Accion.modal("show");
            });

            btnCE_Accion.click(function () {
                fncCrearAccion();
            });

            //#region ACCIONES DETALLE
            btnFiltro_AccionDet.click(function () {
                fncLimpiarMdlCE_AccionDet();
                mdlListado_AccionesDet.modal("hide");
                mdlCE_AccionDet_Title.html("Crear acción detalle");
                txtCE_AccionDet_Progreso.val(0);
                mdlCE_AccionDet.modal("show");
            });

            btnCE_AccionDet.click(function () {
                fncCrearAccionDet();
            });
            //#endregion
            //#endregion

            //#region COTIZACIONES
            btnFiltro_Cotizacion.click(function () {
                btnCE_Cotizacion.html("<i class='fas fa-save'></i>&nbsp;Guardar");
                btnCE_Cotizacion.data().id = 0;
                fncLimpiarMdlCE_Cotizacion();
                fncVerificarUltimaCotizacion();
                mdlCE_Cotizacion.modal("show");
            });

            btnCE_Cotizacion.click(function () {
                if (btnCE_Cotizacion.data().id <= 0) {
                    fncCrearCotizacion();
                } else {
                    fncActualizarCotizacion();
                }
            });
            //#endregion
        }

        //#region CREAR/EDITAR PROYECTOS
        function fncGetProyectos() {
            let objParamsDTO = {};
            switch (+cboFiltro_TipoFiltro.val()) {
                case TipoFiltrosEnum.CLIENTE:
                    objParamsDTO.FK_Cliente = cboFiltro_Busqueda.val();
                    break;
                case TipoFiltrosEnum.DIVISION:
                    objParamsDTO.FK_Division = cboFiltro_Busqueda.val();
                    break;
                case TipoFiltrosEnum.RESPONSABLE:
                    objParamsDTO.FK_UsuarioResponsable = cboFiltro_Busqueda.val();
                    break;
                case TipoFiltrosEnum.PRIORIDAD:
                    objParamsDTO.FK_Prioridad = cboFiltro_Busqueda.val();
                    break;
                default:
                    break;
            }
            objParamsDTO.FK_Estatus_Prospeccion = cboFiltro_Proyecto_Prospeccion.val();
            objParamsDTO.FK_Estatus_LaborVenta = cboFiltro_Proyecto_LaborVenta.val();
            objParamsDTO.FK_Estatus_Cotizacion = cboFiltro_Proyecto_Cotizacion.val();
            objParamsDTO.FK_Estatus_Negociacion = cboFiltro_Proyecto_Negociacion.val();
            objParamsDTO.FK_Estatus_Cierre = cboFiltro_Proyecto_Cierre.val();
            axios.post('GetProyectos', objParamsDTO).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    divProyectosProspeccion.html(response.data.htmlProyectosProspeccion);
                    divProyectosLaborVenta.html(response.data.htmlProyectosLaborVenta);
                    divProyectosCotizacion.html(response.data.htmlProyectosCotizacion);
                    divProyectosNegociacion.html(response.data.htmlProyectosNegociacion);
                    divProyectosCierre.html(response.data.htmlProyectosCierre);
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncChangeCliente() {
            if (cboCE_Proyecto_Cliente.data().mostrar) {
                div_cboCE_Proyecto_Cliente.css("display", "inline");
                div_txtCE_Proyecto_Cliente.css("display", "none");
                div_txtCE_Proyecto_Contacto.css("display", "none");
                cboCE_Proyecto_Cliente.data().mostrar = false;
                cboCE_Proyecto_Cliente.data().estaOculto = false;
            } else if (!cboCE_Proyecto_Cliente.data().mostrar) {
                div_cboCE_Proyecto_Cliente.css("display", "none");
                div_txtCE_Proyecto_Cliente.css("display", "inline");
                div_txtCE_Proyecto_Contacto.css("display", "inline");
                cboCE_Proyecto_Cliente.data().mostrar = true;
                cboCE_Proyecto_Cliente.data().estaOculto = true;
            }
        }

        function fncCrearProyecto() {
            let objParamsDTO = fncCEOBJProyecto();
            if (objParamsDTO != "") {
                axios.post('CrearProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetProyectos();
                        mdlCE_Proyecto.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncActualizarProyecto() {
            let objParamsDTO = fncCEOBJProyecto();
            if (objParamsDTO != "") {
                axios.post('ActualizarProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetProyectos();
                        mdlCE_Proyecto.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJProyecto() {
            fncDefaultCtrls("txtCE_Proyecto_NombreProyecto", false);
            fncDefaultCtrls("cboCE_Proyecto_Cliente", true);
            fncDefaultCtrls("txtCE_Proyecto_Cliente", false);
            fncDefaultCtrls("cboCE_Proyecto_Prioridad", true);
            fncDefaultCtrls("cboCE_Proyecto_Division", true);
            fncDefaultCtrls("cboCE_Proyecto_Estatus", true);
            fncDefaultCtrls("txtCE_Proyecto_Importe", false);
            fncDefaultCtrls("txtCE_Proyecto_FechaInicio", false);
            fncDefaultCtrls("cboCE_Proyecto_Escenario", true);
            fncDefaultCtrls("cboCE_Proyecto_Pais", true);
            fncDefaultCtrls("cboCE_Proyecto_Estado", true);
            fncDefaultCtrls("cboCE_Proyecto_Municipio", true);
            fncDefaultCtrls("cboCE_Proyecto_Responsable", true);
            fncDefaultCtrls("txtCE_Proyecto_Contacto", false);
            fncDefaultCtrls("cboCE_Proyecto_Riesgo", false);
            fncDefaultCtrls("txtCE_Proyecto_DescripcionObra", false);

            if (txtCE_Proyecto_NombreProyecto.val() == "") { fncValidacionCtrl("txtCE_Proyecto_NombreProyecto", false, "Es necesario indicar el nombre del proyecto."); return ""; }

            if (!cboCE_Proyecto_Cliente.data().estaOculto) {
                if (cboCE_Proyecto_Cliente.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Cliente", true, "Es necesario seleccionar un cliente."); return ""; }
            } else if (cboCE_Proyecto_Cliente.data().estaOculto) {
                if (txtCE_Proyecto_Cliente.val() == "") { fncValidacionCtrl("txtCE_Proyecto_Cliente", false, "Es necesario indicar el nombre del cliente nuevo."); return ""; }
                if (txtCE_Proyecto_Contacto.val() == "") { fncValidacionCtrl("txtCE_Proyecto_Contacto", false, "Es necesario indicar el nombre del contacto nuevo."); return ""; }
            }

            if (cboCE_Proyecto_Prioridad.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Prioridad", true, "Es necesario seleccionar la prioridad."); return ""; }
            if (cboCE_Proyecto_Division.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Division", true, "Es necesario seleccionar la división."); return ""; }
            if (cboCE_Proyecto_Estatus.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Estatus", true, "Es necesario seleccionar el estatus."); return ""; }
            if (txtCE_Proyecto_Importe.val() == "") { fncValidacionCtrl("txtCE_Proyecto_Importe", false, "Es necesario indicar el importe."); return ""; }
            if (txtCE_Proyecto_FechaInicio.val() == "") { fncValidacionCtrl("txtCE_Proyecto_FechaInicio", false, "Es necesario indicar la fecha de inicio."); return ""; }
            if (cboCE_Proyecto_Escenario.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Escenario", true, "Es necesario seleccionar el escenario."); return ""; }
            if (cboCE_Proyecto_Pais.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Pais", true, "Es necesario seleccionar el país."); return ""; }
            if (cboCE_Proyecto_Estado.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Estado", true, "Es necesario seleccionar el estado."); return ""; }
            if (cboCE_Proyecto_Municipio.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Municipio", true, "Es necesario seleccionar el municipio."); return ""; }
            if (cboCE_Proyecto_Responsable.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Responsable", true, "Es necesario seleccionar al responsable."); return ""; }
            if (cboCE_Proyecto_Riesgo.val() <= 0) { fncValidacionCtrl("cboCE_Proyecto_Riesgo", true, "Es necesario seleccionar un riesgo."); return ""; }
            if (txtCE_Proyecto_DescripcionObra.val() == "") { fncValidacionCtrl("txtCE_Proyecto_DescripcionObra", false, "Es necesario indicar la descripción de la obra."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_Proyecto.data().id;
            objParamsDTO.nombreProyecto = txtCE_Proyecto_NombreProyecto.val();
            if (cboCE_Proyecto_Cliente.val() > 0) {
                objParamsDTO.FK_Cliente = cboCE_Proyecto_Cliente.val();
            } else {
                objParamsDTO.nombreCliente = txtCE_Proyecto_Cliente.val();
                objParamsDTO.nombreContacto = txtCE_Proyecto_Contacto.val();
                objParamsDTO.esCrearClienteDesdeProyectos = true;
            }
            objParamsDTO.FK_Prioridad = cboCE_Proyecto_Prioridad.val();
            objParamsDTO.FK_Division = cboCE_Proyecto_Division.val();
            objParamsDTO.FK_Municipio = cboCE_Proyecto_Municipio.val();
            objParamsDTO.importeCotizadoAprox = unmaskNumero(txtCE_Proyecto_Importe.val());
            objParamsDTO.fechaInicio = txtCE_Proyecto_FechaInicio.val();
            objParamsDTO.FK_Estatus = cboCE_Proyecto_Estatus.val();
            objParamsDTO.FK_Escenario = cboCE_Proyecto_Escenario.val();
            objParamsDTO.FK_UsuarioResponsable = cboCE_Proyecto_Responsable.val();
            objParamsDTO.FK_Riesgo = cboCE_Proyecto_Riesgo.val();
            objParamsDTO.descripcionObra = txtCE_Proyecto_DescripcionObra.val();
            return objParamsDTO;
        }

        function fncEliminarProyecto() {
            if (mdlResumen_Proyecto.data().idProyecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = mdlResumen_Proyecto.data().idProyecto;
                axios.post('EliminarProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetProyectos();
                        mdlResumen_Proyecto.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al eliminar el proyecto.");
            }
        }

        function fncGetDatosActualizarProyecto(idProyecto) {
            if (idProyecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idProyecto;
                axios.post('GetDatosActualizarProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCE_Proyecto.data().id = idProyecto;
                        txtCE_Proyecto_NombreProyecto.val(items.nombreProyecto);
                        cboCE_Proyecto_Cliente.fillCombo('FillCboClientes', null, false, null);
                        cboCE_Proyecto_Cliente.val(items.FK_Cliente);
                        cboCE_Proyecto_Cliente.trigger("change");
                        cboCE_Proyecto_Prioridad.val(items.FK_Prioridad);
                        cboCE_Proyecto_Prioridad.trigger("change");
                        cboCE_Proyecto_Division.val(items.FK_Division);
                        cboCE_Proyecto_Division.trigger("change");
                        cboCE_Proyecto_Estatus.val(items.FK_Estatus);
                        cboCE_Proyecto_Estatus.trigger("change");
                        txtCE_Proyecto_Importe.val(maskNumero2DCompras(items.importeCotizadoAprox));
                        txtCE_Proyecto_FechaInicio.val(moment(items.fechaInicio).format('YYYY-MM-DD'));
                        cboCE_Proyecto_Escenario.val(items.FK_Escenario);
                        cboCE_Proyecto_Escenario.trigger("change");
                        cboCE_Proyecto_Pais.val(items.FK_Pais);
                        cboCE_Proyecto_Pais.trigger("change");
                        cboCE_Proyecto_Estado.val(items.FK_Estado);
                        cboCE_Proyecto_Estado.trigger("change");
                        cboCE_Proyecto_Municipio.val(items.FK_Municipio)
                        cboCE_Proyecto_Municipio.trigger("change");
                        cboCE_Proyecto_Responsable.val(items.FK_UsuarioResponsable);
                        cboCE_Proyecto_Responsable.trigger("change");
                        cboCE_Proyecto_Riesgo.val(items.FK_Riesgo)
                        cboCE_Proyecto_Riesgo.trigger("change");
                        txtCE_Proyecto_DescripcionObra.val(items.descripcionObra);
                        mdlResumen_Proyecto.modal("hide");
                        btnCE_Proyecto.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                        mdlCE_Proyecto.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncLimpiarMdlCE_Proyecto() {
            $("input[type='text']").val("");
            cboCE_Proyecto_Cliente[0].selectedIndex = 0;
            cboCE_Proyecto_Cliente.trigger("change");
            cboCE_Proyecto_Prioridad[0].selectedIndex = 0;
            cboCE_Proyecto_Prioridad.trigger("change");
            cboCE_Proyecto_Division[0].selectedIndex = 0;
            cboCE_Proyecto_Division.trigger("change");
            cboCE_Proyecto_Estatus[0].selectedIndex = 0;
            cboCE_Proyecto_Estatus.trigger("change");
            cboCE_Proyecto_Escenario[0].selectedIndex = 0;
            cboCE_Proyecto_Escenario.trigger("change");
            cboCE_Proyecto_Pais[0].selectedIndex = 0;
            cboCE_Proyecto_Pais.trigger("change");
            cboCE_Proyecto_Estado[0].selectedIndex = 0;
            cboCE_Proyecto_Estado.trigger("change");
            cboCE_Proyecto_Municipio[0].selectedIndex = 0;
            cboCE_Proyecto_Municipio.trigger("change");
            cboCE_Proyecto_Responsable[0].selectedIndex = 0;
            cboCE_Proyecto_Responsable.trigger("change");
            txtCE_Proyecto_DescripcionObra.val("");
        }

        function fncSetEscenario(idPrioridad) {
            switch (idPrioridad) {
                case "1":
                    cboCE_Proyecto_Escenario.val(EscenariosEnum.E);
                    cboCE_Proyecto_Escenario.change();
                    break;
                case "2":
                    cboCE_Proyecto_Escenario.val(EscenariosEnum.D);
                    cboCE_Proyecto_Escenario.change();
                    break;
                case "3":
                    cboCE_Proyecto_Escenario.val(EscenariosEnum.C);
                    cboCE_Proyecto_Escenario.change();
                    break;
                case "4":
                    cboCE_Proyecto_Escenario.val(EscenariosEnum.B);
                    cboCE_Proyecto_Escenario.change();
                    break;
                case "5":
                    {
                        if (cboCE_Proyecto_Estatus.val() > 0) {
                            if (cboCE_Proyecto_Estatus.val() == EstatusEnum.Ganado) {
                                cboCE_Proyecto_Escenario.val(EscenariosEnum.A);
                            } else if (cboCE_Proyecto_Estatus.val() == EstatusEnum.Perdido) {
                                cboCE_Proyecto_Escenario.val(EscenariosEnum.T);
                            }
                            cboCE_Proyecto_Escenario.change();
                        }
                        break;
                    }
            }
        }

        function fncFillCbosProyectoEtapas() {
            cboFiltro_Proyecto_Prospeccion.fillCombo('FillCboEtapas_ProyectoProspeccion', null, false, null);
            cboFiltro_Proyecto_LaborVenta.fillCombo('FillCboEtapas_ProyectoLaborVenta', null, false, null);
            cboFiltro_Proyecto_Cotizacion.fillCombo('FillCboEtapas_ProyectoCotizacion', null, false, null);
            cboFiltro_Proyecto_Negociacion.fillCombo('FillCboEtapas_ProyectoNegociacion', null, false, null);
            cboFiltro_Proyecto_Cierre.fillCombo('FillCboEtapas_ProyectoCierre', null, false, null);
        }
        //#endregion

        //#region RESUMEN PROYECTO
        function fncGetResumenProyecto() {
            if (mdlResumen_Proyecto.data().idProyecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = mdlResumen_Proyecto.data().idProyecto;
                axios.post('GetResumenProyecto', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (response.data.objProyectoDTO != null) {
                            lblResumen_Generales_Cliente.html(response.data.objProyectoDTO.nombreCliente);
                            lblResumen_Generales_TipoCliente.html(response.data.objProyectoDTO.tipoCliente);
                            lblResumen_Generales_Division.html(response.data.objProyectoDTO.division);
                            lblResumen_Generales_ImporteAprox.html(response.data.objProyectoDTO.strImporteCotizadoAprox);
                            lblResumen_Generales_Ubicacion.html(response.data.objProyectoDTO.ubicacion);
                            lblResumen_Generales_FechaInicio.html(moment(response.data.objProyectoDTO.fechaInicio).format('DD/MM/YYYY'));
                            lblResumen_Generales_Estatus.html(response.data.objProyectoDTO.estatus);
                            lblResumen_Generales_Escenario.html(response.data.objProyectoDTO.escenario);
                            lblResumen_Generales_Prioridad.html(response.data.objProyectoDTO.prioridad);
                            lblResumen_Generales_Responsable.html(response.data.objProyectoDTO.nombreCompletoResponsable);
                            lblResumen_Generales_Riesgo.html(response.data.objProyectoDTO.riesgo);
                            lblResumen_Generales_DescripcionObra.html(response.data.objProyectoDTO.descripcionObra);
                            divResumen_Contactos.html(response.data.objProyectoDTO.htmlContactos);
                        }

                        if (response.data.objUltimoComentarioDTO != null) {
                            lblResumen_UltimoComentario.html(response.data.objUltimoComentarioDTO.ultimoComentario);
                        }

                        if (response.data.objProximaAccionDTO != null) {
                            lblResumen_ProximaAccion_Accion.html(response.data.objProximaAccionDTO.accion);
                            lblResumen_ProximaAccion_Fecha.html(moment(response.data.objProximaAccionDTO.fechaProximaAccion).format('DD/MM/YYYY'));
                            lblResumen_ProximaAccion_Responsable.html(response.data.objProximaAccionDTO.nombreCompletoResponsableAccion);
                            lblResumen_ProximaAccion_Progreso.html(`${response.data.objProximaAccionDTO.progreso}%`);
                        }

                        mdlResumen_Proyecto_Title.html("Resumen proyecto");
                        mdlResumen_Proyecto.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Warning("Ocurrió un error al obtener el resumen del proyecto.");
            }
        }

        function fncMostrarOcultarTab(event, divID) {
            let i, tabcontent, tablinks;
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }

            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }

            document.getElementById(divID).style.display = "block";
            event.currentTarget.className += " active";

            switch (divID) {
                case "tabResumen":
                    fncGetResumenProyecto();
                    break;
                case "tabComentarios":
                    fncGetResumenComentarios();
                    break;
                case "tabAcciones":
                    fncGetResumenAcciones();
                    break;
                case "tabCotizaciones":
                    fncGetCotizaciones();
            }
        }

        //#region COMENTARIOS
        function fncGetResumenComentarios() {
            if (mdlResumen_Proyecto.data().idProyecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_Proyecto = mdlResumen_Proyecto.data().idProyecto;
                axios.post('GetResumenComentarios', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        tblComentarios.html(items);
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener el listado de comentarios.");
            }
        }

        function fncEliminarComentario(idComentario) {
            if (idComentario > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idComentario;
                axios.post('EliminarResumenComentario', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetResumenComentarios();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCrearComentario() {
            fncDefaultCtrls("txtCE_Comentario", false);
            if (mdlResumen_Proyecto.data().idProyecto > 0 && txtCE_Comentario.val() != "") {
                let objParamsDTO = {};
                objParamsDTO.FK_Proyecto = mdlResumen_Proyecto.data().idProyecto;
                objParamsDTO.ultimoComentario = txtCE_Comentario.val();
                axios.post('CrearComentario', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetResumenComentarios();
                        mdlCE_Comentario.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                if (mdlResumen_Proyecto.data().idProyecto <= 0) { Alert2Error("Ocurrió un error al registrar el comentario."); }
                if (txtCE_Comentario.val() == "") { fncValidacionCtrl("txtCE_Comentario", false, "Es necesario indicar el comentario."); }
            }
        }

        function fncLimpiarMdlCE_Comentario() {
            txtCE_Comentario.val("");
        }
        //#endregion

        //#region ACCIONES
        function initTblAcciones() {
            dtResumenAcciones = tblAcciones.DataTable({
                language: dtDicEsp,
                paging: true,
                ordering: true,
                searching: true,
                order: [[0, "desc"]],
                columns: [
                    { data: 'numAccion', title: 'No. Acción' },
                    { data: 'accion', title: 'Comentarios' },
                    {
                        data: 'accionFinalizada', title: "Acción finalizada",
                        render: (data, type, row, meta) => {
                            if (data) {
                                return `<button class="btn btn-xs btn-success">Acción finalizada</button>`;
                            } else {
                                return `<button class="btn btn-xs btn-danger">En proceso</button>`;
                            }
                        }
                    },
                    {
                        title: 'Acciones',
                        render: function (data, type, row, meta) {
                            let botones = "";
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            let btnDetalle = `<button class='btn btn-xs btn-primary listadoDetalle' title='Detalle de acciones.'><i class="fas fa-list"></i></button>`;
                            let btnFinalizarAccion = `<button class='btn btn-xs btn-success finalizarAccion' title='Finalizar acción.'><i class="fas fa-check"></i></button>`;
                            if (!row.accionFinalizada) {
                                botones = `${btnEliminar} ${btnDetalle} ${btnFinalizarAccion}`;
                            } else {
                                botones = `${btnEliminar} ${btnDetalle}`;
                            }
                            return botones;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblAcciones.on('click', '.eliminarRegistro', function () {
                        let rowData = dtResumenAcciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarAccion(rowData.id));
                    });

                    tblAcciones.on('click', '.listadoDetalle', function () {
                        let rowData = dtResumenAcciones.row($(this).closest('tr')).data();
                        mdlResumen_Proyecto.data().FK_Accion = rowData.id;
                        fncGetResumenAccionesDet();
                        mdlListado_AccionesDet.modal("show");
                    });

                    tblAcciones.on('click', '.finalizarAccion', function () {
                        let rowData = dtResumenAcciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea finalizar la acción?', 'Confirmar', 'Cancelar', () => fncFinalizarAccionPrincipal(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncFinalizarAccionPrincipal(idAccion) {
            if (idAccion > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idAccion;
                axios.post('FinalizarAccionPrincipal', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetResumenAcciones();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al finalizar la acción.");
            }
        }

        function fncGetResumenAcciones() {
            if (mdlResumen_Proyecto.data().idProyecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_Proyecto = mdlResumen_Proyecto.data().idProyecto;
                axios.post('GetResumenAcciones', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtResumenAcciones.clear();
                        dtResumenAcciones.rows.add(items);
                        dtResumenAcciones.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener el listado de proximas acciones.");
            }
        }

        function fncEliminarAccion(idAccion) {
            if (idAccion > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idAccion;
                axios.post('EliminarResumenAccion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetResumenAcciones();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCrearAccion() {
            let objParamsDTO = fncCEOBJAccion();
            if (objParamsDTO != "") {
                axios.post('CrearAccion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetResumenAcciones();
                        mdlCE_Accion.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJAccion() {
            fncDefaultCtrls("txtCE_Accion_Accion", false);
            fncDefaultCtrls("txtCE_Accion_FechaAccion", false);
            fncDefaultCtrls("cboCE_Accion_ResponsableAccion", true);
            fncDefaultCtrls("txtCE_Accion_Progreso", false);

            if (txtCE_Accion_Accion.val() == "") { fncValidacionCtrl("txtCE_Accion_Accion", false, "Es necesario indicar la proxima acción."); return ""; }
            if (txtCE_Accion_FechaAccion.val() == "") { fncValidacionCtrl("txtCE_Accion_FechaAccion", false, "Es necesario indicar la fecha."); return ""; }
            if (cboCE_Accion_ResponsableAccion.val() <= 0) { fncValidacionCtrl("cboCE_Accion_ResponsableAccion", true, "Es necesario seleccionar al responsable."); return ""; }
            if (txtCE_Accion_Progreso.val() <= -1) { fncValidacionCtrl("txtCE_Accion_Progreso", false, "Ocurrió un error al registrar la acción."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = mdlResumen_Proyecto.data().idProyecto;
            objParamsDTO.FK_Proyecto = mdlResumen_Proyecto.data().idProyecto;
            objParamsDTO.accion = txtCE_Accion_Accion.val();
            objParamsDTO.fechaProximaAccion = txtCE_Accion_FechaAccion.val();
            objParamsDTO.FK_UsuarioResponsable = cboCE_Accion_ResponsableAccion.val();
            objParamsDTO.progreso = txtCE_Accion_Progreso.val();
            return objParamsDTO;
        }

        function fncLimpiarMdlCE_Accion() {
            txtCE_Accion_Accion.val("");
            txtCE_Accion_Progreso.val("");
            cboCE_Accion_ResponsableAccion[0].selectedIndex = 0;
            cboCE_Accion_ResponsableAccion.trigger("change");
        }

        //#region ACCIONES DETALLE
        function initTblAccionesDetalleDet() {
            dtResumenAccionesDet = tblAccionesDet.DataTable({
                language: dtDicEsp,
                paging: true,
                ordering: true,
                searching: true,
                order: [[0, "desc"]],
                columns: [
                    { data: 'numAccionDet', title: 'No. Acción' },
                    { data: 'accion', title: 'Comentarios' },
                    {
                        data: 'accionFinalizada', title: "Acción finalizada",
                        render: (data, type, row, meta) => {
                            if (data) {
                                return `<button class="btn btn-xs btn-success">Acción finalizada</button>`;
                            } else {
                                return `<button class="btn btn-xs btn-danger">En proceso</button>`;
                            }
                        }
                    },
                    {
                        title: "Acciones",
                        render: function (data, type, row, meta) {
                            let botones = "";
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            let btnFinalizarAccion = `<button class='btn btn-xs btn-success finalizarAccion' title='Finalizar acción.'><i class="fas fa-check"></i></button>`;
                            if (!row.accionFinalizada) {
                                botones = `${btnEliminar} ${btnFinalizarAccion}`;
                            } else {
                                botones = btnEliminar;
                            }
                            return botones;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblAccionesDet.on('click', '.eliminarRegistro', function () {
                        let rowData = dtResumenAccionesDet.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarAccionDet(rowData.id));
                    });

                    tblAccionesDet.on('click', '.finalizarAccion', function () {
                        let rowData = dtResumenAccionesDet.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea finalizar la acción?', 'Confirmar', 'Cancelar', () => fncFinalizarAccionDetalle(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncFinalizarAccionDetalle(idAccionDet) {
            if (idAccionDet > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idAccionDet;
                objParamsDTO.FK_Accion = mdlResumen_Proyecto.data().FK_Accion;
                axios.post('FinalizarAccionDetalle', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetResumenAcciones();
                        fncGetResumenAccionesDet();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al finalizar la acción del detalle.");
            }
        }

        function fncGetResumenAccionesDet() {
            if (mdlResumen_Proyecto.data().FK_Accion > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_Accion = mdlResumen_Proyecto.data().FK_Accion;
                axios.post('GetResumenDetalleAccion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        //#region FILL DATATABLE
                        dtResumenAccionesDet.clear();
                        dtResumenAccionesDet.rows.add(items);
                        dtResumenAccionesDet.draw();
                        //#endregion
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener el detalle de las proximas acciones.");
            }
        }

        function fncEliminarAccionDet(idAccionDet) {
            if (idAccionDet > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idAccionDet;
                objParamsDTO.FK_Accion = mdlResumen_Proyecto.data().FK_Accion;
                axios.post('EliminarResumenAccionDetalle', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetResumenAcciones();
                        fncGetResumenAccionesDet();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCrearAccionDet() {
            let objParamsDTO = fncCEOBJAccionDet();
            if (objParamsDTO != "") {
                axios.post('CrearAccionDetalle', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetResumenAcciones();
                        fncGetResumenAccionesDet();
                        mdlListado_AccionesDet.modal("show");
                        mdlCE_AccionDet.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJAccionDet() {
            fncDefaultCtrls("txtCE_AccionDet_Accion", false);
            fncDefaultCtrls("txtCE_AccionDet_FechaAccion", false);
            fncDefaultCtrls("cboCE_AccionDet_ResponsableAccion", true);
            fncDefaultCtrls("txtCE_AccionDet_Progreso", false);

            if (txtCE_AccionDet_Accion.val() == "") { fncValidacionCtrl("txtCE_AccionDet_Accion", false, "Es necesario indicar la proxima acción."); return ""; }
            if (txtCE_AccionDet_FechaAccion.val() == "") { fncValidacionCtrl("txtCE_AccionDet_FechaAccion", false, "Es necesario indicar la fecha."); return ""; }
            if (cboCE_AccionDet_ResponsableAccion.val() <= 0) { fncValidacionCtrl("cboCE_AccionDet_ResponsableAccion", true, "Es necesario seleccionar al responsable."); return ""; }
            if (txtCE_AccionDet_Progreso.val() <= -1) { fncValidacionCtrl("txtCE_AccionDet_Progreso", false, "Ocurrió un error al registrar la acción detalle."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = mdlResumen_Proyecto.data().FK_AccionDet;
            objParamsDTO.FK_Accion = mdlResumen_Proyecto.data().FK_Accion;
            objParamsDTO.accion = txtCE_AccionDet_Accion.val();
            objParamsDTO.fechaProximaAccion = txtCE_AccionDet_FechaAccion.val();
            objParamsDTO.FK_UsuarioResponsable = cboCE_AccionDet_ResponsableAccion.val();
            objParamsDTO.progreso = txtCE_AccionDet_Progreso.val();
            return objParamsDTO;
        }

        function fncLimpiarMdlCE_AccionDet() {
            txtCE_AccionDet_Accion.val("");
            txtCE_AccionDet_Progreso.val("");
            cboCE_AccionDet_ResponsableAccion[0].selectedIndex = 0;
            cboCE_AccionDet_ResponsableAccion.trigger("change");
        }
        //#endregion
        //#endregion
        //#endregion

        //#region COTIZACIONES
        function initTblCotizaciones() {
            dtCotizaciones = tblCotizaciones.DataTable({
                language: dtDicEsp,
                paging: true,
                ordering: true,
                searching: true,
                order: [[3, "desc"]],
                columns: [
                    { data: 'nombreCompletoResponsable', title: 'Responsable' },
                    { data: 'strImporteFinal', title: "Importe final" },
                    {
                        data: 'fechaFinal', title: 'Fecha final',
                        render: (data, type, row, meta) => {
                            return moment(data).format('DD/MM/YYYY')
                        }
                    },
                    { data: 'importeRevN', title: 'Importe Rev N.' },
                    {
                        data: 'fechaRevN', title: 'Fecha Rev N.',
                        render: (data, type, row, meta) => {
                            return moment(data).format('DD/MM/YYYY')
                        }
                    },
                    { data: 'strImporteOriginal', title: 'Importe original' },
                    {
                        data: 'fechaOriginal', title: 'Fecha original',
                        render: (data, type, row, meta) => {
                            return moment(data).format('DD/MM/YYYY')
                        }
                    },
                    {
                        title: 'Acciones',
                        render: function (data, type, row, meta) {
                            let btnEditar = `<button class='btn btn-xs btn-warning editarRegistro' title='Editar registro.'><i class='fas fa-pencil-alt'></i></button>`;
                            let btnEliminar = `<button class='btn btn-xs btn-danger eliminarRegistro' title='Eliminar registro.'><i class='fas fa-trash'></i></button>`;
                            return `${btnEditar} ${btnEliminar}`;
                        },
                    }
                ],
                initComplete: function (settings, json) {
                    tblCotizaciones.on('click', '.editarRegistro', function () {
                        let rowData = dtCotizaciones.row($(this).closest('tr')).data();
                        fncLimpiarMdlCE_Cotizacion();
                        fncGetDatosActualizarCotizacion(rowData.id);
                    });

                    tblCotizaciones.on('click', '.eliminarRegistro', function () {
                        let rowData = dtCotizaciones.row($(this).closest('tr')).data();
                        Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarCotizacion(rowData.id));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
            });
        }

        function fncGetCotizaciones() {
            axios.post('GetCotizaciones').then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    //#region FILL DATATABLE
                    dtCotizaciones.clear();
                    dtCotizaciones.rows.add(items);
                    dtCotizaciones.draw();
                    //#endregion
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        function fncCrearCotizacion() {
            let objParamsDTO = fncCEOBJCotizacion();
            if (objParamsDTO != "") {
                axios.post('CrearCotizacion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCotizaciones();
                        mdlCE_Cotizacion.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncActualizarCotizacion() {
            let objParamsDTO = fncCEOBJCotizacion();
            if (objParamsDTO != "") {
                axios.post('ActualizarCotizacion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCotizaciones();
                        mdlCE_Cotizacion.modal("hide");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            }
        }

        function fncCEOBJCotizacion() {
            fncDefaultCtrls("cboCE_Cotizacion_Responsable", true);
            fncDefaultCtrls("txtCE_Cotizacion_ImporteFinal", false);
            fncDefaultCtrls("txtCE_Cotizacion_fechaFinal", false);
            fncDefaultCtrls("txtCE_Cotizacion_ImporteRevN", false);
            fncDefaultCtrls("txtCE_Cotizacion_fechaRevN", false);
            fncDefaultCtrls("txtCE_Cotizacion_ImporteOriginal", false);
            fncDefaultCtrls("txtCE_Cotizacion_fechaOriginal", false);
            fncDefaultCtrls("txtCE_Cotizacion_Comentario", false);

            if (cboCE_Cotizacion_Responsable.val() <= 0) { fncValidacionCtrl("cboCE_Cotizacion_Responsable", true, "Es necesario seleccionar al responsable."); return ""; }

            if (txtCE_Cotizacion_ImporteFinal.val() == "") { fncValidacionCtrl("txtCE_Cotizacion_ImporteFinal", false, "Es necesario indicar el importe final."); return ""; }
            if (txtCE_Cotizacion_ImporteFinal.val() <= -1) { fncValidacionCtrl("txtCE_Cotizacion_ImporteFinal", false, "Es necesario indicar el importe final con importe positivo."); return ""; }
            if (txtCE_Cotizacion_fechaFinal.val() == "") { fncValidacionCtrl("txtCE_Cotizacion_fechaFinal", false, "Es necesario indicar la fecha final."); return ""; }

            if (txtCE_Cotizacion_ImporteRevN.val() == "") { fncValidacionCtrl("txtCE_Cotizacion_ImporteRevN", false, "Es necesario indicar el importe Rev N."); return ""; }
            if (txtCE_Cotizacion_ImporteRevN.val() <= -1) { fncValidacionCtrl("txtCE_Cotizacion_ImporteRevN", false, "Es necesario indicar el importe Rev N con importe positivo."); return ""; }
            if (txtCE_Cotizacion_fechaRevN.val() == "") { fncValidacionCtrl("txtCE_Cotizacion_fechaRevN", false, "Es necesario indicar la fecha Rev N."); return ""; }

            if (txtCE_Cotizacion_ImporteOriginal.val() == "") { fncValidacionCtrl("txtCE_Cotizacion_ImporteOriginal", false, "Es necesario indicar el importe original."); return ""; }
            if (txtCE_Cotizacion_ImporteOriginal.val() <= -1) { fncValidacionCtrl("txtCE_Cotizacion_ImporteOriginal", false, "Es necesario indicar el importe original con importe positivo."); return ""; }
            if (txtCE_Cotizacion_fechaOriginal.val() == "") { fncValidacionCtrl("txtCE_Cotizacion_fechaOriginal", false, "Es necesario indicar la fecha original."); return ""; }

            if (txtCE_Cotizacion_Comentario.val() == "") { fncValidacionCtrl("txtCE_Cotizacion_Comentario", false, "Es necesario indicar el comentario."); return ""; }

            let objParamsDTO = {};
            objParamsDTO.id = btnCE_Cotizacion.data().id;
            objParamsDTO.FK_Proyecto = mdlResumen_Proyecto.data().idProyecto;
            objParamsDTO.FK_ResponsableCotizacion = cboCE_Cotizacion_Responsable.val();
            objParamsDTO.importeFinal = unmaskNumero6DCompras(txtCE_Cotizacion_ImporteFinal.val());
            objParamsDTO.fechaFinal = txtCE_Cotizacion_fechaFinal.val();
            objParamsDTO.importeRevN = txtCE_Cotizacion_ImporteRevN.val();
            objParamsDTO.fechaRevN = txtCE_Cotizacion_fechaRevN.val();
            objParamsDTO.importeOriginal = unmaskNumero6DCompras(txtCE_Cotizacion_ImporteOriginal.val());
            objParamsDTO.fechaOriginal = txtCE_Cotizacion_fechaOriginal.val();
            objParamsDTO.comentario = txtCE_Cotizacion_Comentario.val();
            return objParamsDTO;
        }

        function fncEliminarCotizacion(idCotizacion) {
            if (idCotizacion > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCotizacion;
                axios.post('EliminarCotizacion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        Alert2Exito(message);
                        fncGetCotizaciones();
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al actualizar la cotización.")
            }
        }

        function fncGetDatosActualizarCotizacion(idCotizacion) {
            if (idCotizacion > 0) {
                let objParamsDTO = {};
                objParamsDTO.id = idCotizacion
                axios.post('GetDatosActualizarCotizacion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        btnCE_Cotizacion.html("<i class='fas fa-save'></i>&nbsp;Actualizar");
                        btnCE_Cotizacion.data().id = idCotizacion;
                        cboCE_Cotizacion_Responsable.val(items.FK_ResponsableCotizacion);
                        cboCE_Cotizacion_Responsable.trigger("change");
                        txtCE_Cotizacion_ImporteFinal.val(items.importeFinal);
                        txtCE_Cotizacion_fechaFinal.val(moment(items.fechaFinal).format('YYYY-MM-DD'));
                        txtCE_Cotizacion_ImporteRevN.val(items.importeRevN);
                        txtCE_Cotizacion_fechaRevN.val(moment(items.fechaRevN).format('YYYY-MM-DD'));
                        txtCE_Cotizacion_ImporteOriginal.val(items.importeOriginal);
                        txtCE_Cotizacion_fechaOriginal.val(moment(items.fechaOriginal).format('YYYY-MM-DD'));
                        txtCE_Cotizacion_Comentario.val(items.comentario);
                        mdlCE_Cotizacion.modal("show");
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al obtener la información de la cotización");
            }
        }

        function fncLimpiarMdlCE_Cotizacion() {
            $("input[type='text']").val("");
            cboCE_Cotizacion_Responsable[0].selectedIndex = 0;
            cboCE_Cotizacion_Responsable.trigger("change");
            txtCE_Cotizacion_Comentario.val("");
        }

        function fncVerificarUltimaCotizacion() {
            if (mdlResumen_Proyecto.data().idProyecto > 0) {
                let objParamsDTO = {};
                objParamsDTO.FK_Proyecto = mdlResumen_Proyecto.data().idProyecto;
                axios.post('VerificarUltimaCotizacion', objParamsDTO).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        if (!items.hayCotizacionAnterior) {
                            let importeRevN = 1;
                            txtCE_Cotizacion_ImporteOriginal.val("$0.00");
                            txtCE_Cotizacion_fechaOriginal.val(moment(items.fechaOriginal).format('YYYY-MM-DD'));
                            txtCE_Cotizacion_ImporteRevN.val(importeRevN);
                            txtCE_Cotizacion_fechaRevN.val(moment(items.fechaRevN).format('YYYY-MM-DD'));
                        }
                        else {
                            txtCE_Cotizacion_ImporteOriginal.val(items.strImporteOriginal);
                            txtCE_Cotizacion_fechaOriginal.val(moment(items.fechaOriginal).format('YYYY-MM-DD'));
                            txtCE_Cotizacion_ImporteRevN.val(items.importeRevN);
                            txtCE_Cotizacion_fechaRevN.val(moment(items.fechaRevN).format('YYYY-MM-DD'));
                        }
                    } else {
                        Alert2Error(message);
                    }
                }).catch(error => Alert2Error(error.message));
            } else {
                Alert2Error("Ocurrió un error al verificar las cotizaciones relacionadas al proyecto.");
            }
        }
        //#endregion

        //#region GENERALES
        function fncFillCbos() {
            cboCE_Proyecto_Prioridad.fillCombo('FillCboPrioridades', null, false, null);
            cboCE_Proyecto_Division.fillCombo('FillCboDivisiones', null, false, null);
            cboCE_Proyecto_Pais.fillCombo('FillCboPaises', null, false, null);
            cboCE_Proyecto_Escenario.fillCombo('FillCboEscenarios', null, false, null);
            cboCE_Proyecto_Responsable.fillCombo('FillCboResponsables', null, false, null);
            cboCE_Accion_ResponsableAccion.fillCombo('FillCboResponsables', null, false, null);
            cboCE_AccionDet_ResponsableAccion.fillCombo('FillCboResponsables', null, false, null);
            cboCE_Cotizacion_Responsable.fillCombo('FillCboResponsables', null, false, null);
            cboCE_Proyecto_Riesgo.fillCombo('FillCboRiesgos', null, false, null);
            cboFiltro_TipoFiltro.fillCombo('FillCboTipoFiltros', null, false, null);
            $(".select2").select2();
        }

        function fncIndicarMenuSeleccion() {
            const variables = fncGetParamsURL(window.location.href);
            if (variables != undefined) {
                $("#btnMenu_Proyectos").removeClass("btn-success");
                $("#btnMenu_Proyectos").addClass("btn-primary");
            }
        }

        function fncGetParamsURL(url) {
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
        //#endregion
    }

    $(document).ready(() => {
        ADMIN_FINANZAS.Proyectos = new Proyectos();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();