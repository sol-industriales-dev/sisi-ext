(function () {

    $.namespace('maquinaria.overhaul.componente');

    componente = function () {
        idComponente = 0,
            Actualizacion = 1,
            idBorrar = -1,
            locacionActual = 0,
            fecha_hoy = getFecha();
        let tipoUsuarioC = 6;
        ruta = '/CatComponentes/FillGrid_Componente';
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        tabInventario = $("#tabInventario"),
            frmModal = $("#frmComponentes"),
            cboFiltroGrupo = $("#cboFiltroGrupo"),
            cboFiltroModelo = $("#cboFiltroModelo"),
            txtFiltroPrefijo = $("#txtFiltroPrefijo"),
            cboFiltroEstatus = $("#cboFiltroEstatus"),

            txtFiltroNoComponente = $("#txtFiltroNoComponente"),
            //txtFiltroDescripcion = $("#txtFiltroDescripcion"),
            txtModalnoComponente = $("#txtModalnoComponente"),
            cboModalGrupoMaquina = $("#cboModalGrupoMaquina"),
            cboModeloMaquina = $("#cboModeloMaquina"),
            cboModalPrefijo = $("#cboModalPrefijo"),
            cboModalModalModeloPrefijo = $("#cboModalModalModeloPrefijo"),
            cboModalCentroCostos = $("#cboModalCentroCostos"),
            cboModalConjunto = $("#cboModalConjunto"),
            cboModalSubConjunto = $("#cboModalSubConjunto"),
            cboModalPosicion = $("#cboModalPosicion"),
            //txtModalDescripcion = $("#txtModalDescripcion"),
            //txtModalNoSerie = $("#txtModalNoSerie"),
            cboModaldNumParte = $("#cboModaldNumParte")
        txtModaldMarca = $("#txtModaldMarca"),
            txtOrdenCompra = $("#txtOrdenCompra"),
            txtCosto = $("#txtCosto"),
            txtModalHoraCiclo = $("#txtModalHoraCiclo"),
            txtModalHoraCicloActual = $("#txtModalHoraCicloActual"),
            txtModalHoraAcumuladas = $("#txtModalHoraAcumuladas"),
            txtModalVidas = $("#txtModalVidas"),
            cboModalEstatus = $("#cboModalEstatus"),
            cboModalModeloMaquina = $("#cboModalModeloMaquina"),

            btnBuscar = $("#btnBuscar_Componente"),
            btnNuevo = $("#btnNuevo_Componente"),
            btnGuardar = $("#btnModalGuardar_Componente"),
            btnCancelar = $("#btnModalCancelar_Componente"),

            btnModificar = $("#btnModificar_Componente"),

            modalAcciones = $("#modalComponente"),
            tituloModal = $("#title-modal"),
            gridResultado = $("#grid_Componente"),
            cboModalLocacion = $("#cboModalLocacion"),
            txtModalFecha = $("#txtModalFecha"),
            cboModalMarcaComponente = $("#cboModalMarcaComponente"),
            cboModalProveedor = $("#cboModalProveedor"),
            divFechaInstalacion = $("#divFechaInstalacion"),
            txtModalFechaInstalacion = $("#txtModalFechaInstalacion"),
            modalModificar = $("#modalComponenteModificar"),
            //Modal Modificacion de componentes en grupo//
            txtModalModificarHoraCiclo = $("#txtModalModificarHoraCiclo"),
            txtModalModificarGarantia = $("#txtModalModificarGarantia"),
            cboModalModificarEstatus = $("#cboModalModificarEstatus"),
            btnModalModificarGuardar = $("#btnModalModificarGuardar"),
            cboModeloInventario = $("#cboModeloInventario"),
            cboConjunto = $("#cboConjunto"),
            cboSubconjunto = $("#cboSubconjunto"),
            cboCC = $("#cboCC"),
            txtLocacion = $("#txtLocacion"),
            ckIntercambioAlta = $("#ckIntercambioAlta");
        $(document).on('click', "#btnModalEliminar", function () {
            if ($("#ulNuevo .active a").text() == "INVENTARIO") {
                eliminarComponente();
                reset();
            }
        });

        function init() {
            PermisosBotonesC();

            cboModeloInventario.select2();
            cboConjunto.select2();
            cboSubconjunto.select2();
            cboFiltroEstatus.select2();

            cboConjunto.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: -1 });
            cboModeloInventario.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: -1 });
            cboConjunto.change(CboSubConjuntoCargar);
            cboConjunto.change();
            cboCC.fillCombo('/Overhaul/FillCboLocacionYObra', {}, true);
            convertToMultiselectSelectAll(cboCC);
            cboCC.multiselect('selectAll', false).multiselect('updateButtonText');
            cboFiltroGrupo.fillCombo('/CatComponentes/FillCboGrupo_Componente');
            cboModalGrupoMaquina.fillCombo('/CatComponentes/FillCboGrupo_Componente');
            cboModalGrupoMaquina.change(FillCboModelo);
            cboModalConjunto.change(CboSubConjuntoModal);
            // cboModalCentroCostos.fillCombo('/Overhaul/FillCboObraMaquinaID');
            cboModalCentroCostos.fillCombo("/Overhaul/FillCboObraMaquinaIDComboDTO");
            cboModalMarcaComponente.fillCombo('/CatComponentes/FillCboMarcasComponentes');
            cboModalProveedor.fillCombo('/CatComponentes/FillCboLocacion', { tipoLocacion: 2 });
            cboModalCentroCostos.change(FillNoComponente);
            cboModalSubConjunto.change(subConjuntoChange);
            cboModalModeloMaquina.change(FillNoComponente);
            cboModalPosicion.change(FillNoComponente);
            cboModalModalModeloPrefijo.change(FillNoComponente);
            cboModalModeloMaquina.change(FillCboModeloMaquina);
            cboModalModeloMaquina.change(FillCboLocacionComponente);
            txtModalFecha.datepicker().datepicker("setDate", new Date());
            txtModalFechaInstalacion.datepicker().datepicker("setDate", new Date());
            cboModalLocacion.fillCombo('/CatComponentes/FillCboPosiciones_Locaciones', { idModelo: -1, tipoBusqueda: Actualizacion });
            txtModalnoComponente.addClass('required');
            cboModalCentroCostos.addClass('required');
            cboModaldNumParte.addClass('required');
            txtCosto.addClass('required');
            txtModalHoraCiclo.addClass('required');
            txtModalHoraCicloActual.addClass('required');
            txtModalHoraAcumuladas.addClass('required');
            txtModalVidas.addClass('required');
            cboModalGrupoMaquina.addClass('required');
            cboModalModeloMaquina.addClass('required');
            cboModalConjunto.addClass('required');
            cboModalSubConjunto.addClass('required');
            cboModalLocacion.addClass('required');
            txtModalFecha.addClass('required');
            cboModalMarcaComponente.addClass('required');
            cboModalProveedor.addClass('required');
            btnNuevo.click(openModal);
            btnModificar.click(openModalModificar);
            btnGuardar.click(guardar);
            btnCancelar.click(reset);
            btnBuscar.click(clickBuscar);
            cboModalLocacion.change(cargarFechaInstalacion);
            initGrid();
            //tabInventario.click(clickBuscar);
            btnModalModificarGuardar.click(guardarCambiosGrupo);
            $("#cbCicloVidaHoras").change(habilitarModificarVidaHoras);
            $("#cbGarantia").change(habilitarModificarGarantia);
            $("#cbEstatus").change(habilitarModificarEstatus);
            txtFiltroNoComponente.getAutocomplete(SelectNoComponenteInventario, null, '/Overhaul/getNoComponente');
        }

        function PermisosBotonesC() {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/PermisosBotonesAdminComp",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                success: function (response) {
                    $.unblockUI();
                    tipoUsuarioC = response.tipoUsuario;
                    if (response.tipoUsuario < 3) {
                        btnNuevo.prop("disabled", false);
                        btnModificar.prop("disabled", false);
                    }
                    else {
                        btnNuevo.prop("disabled", true);
                        btnModificar.prop("disabled", true);
                    }
                    if (response.tipoUsuario == 7) {
                        btnNuevo.prop("disabled", false);
                        btnModificar.prop("disabled", false);
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function habilitarModificarVidaHoras() {
            if ($("#cbCicloVidaHoras").is(':checked')) {
                txtModalModificarHoraCiclo.attr("disabled", false);
            }
            else {
                txtModalModificarHoraCiclo.attr("disabled", true);
            }
        }
        function habilitarModificarGarantia() {
            if ($("#cbGarantia").is(':checked')) {
                txtModalModificarGarantia.attr("disabled", false);
            }
            else {
                txtModalModificarGarantia.attr("disabled", true);
            }
        }
        function habilitarModificarEstatus() {
            if ($("#cbEstatus").is(':checked')) {
                cboModalModificarEstatus.attr("disabled", false);
            }
            else {
                cboModalModificarEstatus.attr("disabled", true);
            }
        }

        function SelectNoComponenteInventario(event, ui) { txtFiltroNoComponente.text(ui.item.noComponente); }

        function guardarCambiosGrupo() {
            let cicloVidaHorasLocal = "-1";
            let GarantiaLocal = "-1";
            let EstatusLocal = "-1";
            let ccLocal = cboCC.val();

            if ($("#cbCicloVidaHoras").is(':checked')) { cicloVidaHorasLocal = txtModalModificarHoraCiclo.val(); }
            if ($("#cbGarantia").is(':checked')) { GarantiaLocal = txtModalModificarGarantia.val(); }
            if ($("#cbEstatus").is(':checked')) { EstatusLocal = cboModalModificarEstatus.val(); }

            if (cboSubconjunto.val != "") { subconjuntoLocal = cboSubconjunto.val(); }

            //if ($("#cboSubconjunto :selected").text() != "--Seleccione--" && $("#cboSubconjunto :selected").text() != "") { subconjuntoLocal = cboSubconjunto.val(); }
            //if ($("#cboCC :selected").text() != "--Seleccione--") { ccLocal = cboCC.val(); }
            let obras = $("#cboCC option:selected");
            let obrasFinal = [];
            for (let i = 0; i < obras.length; i++) {
                obrasFinal.push({ Value: $(obras[i]).val(), Text: $(obras[i]).text(), Prefijo: $(obras[i]).attr("data-prefijo") });
            }
            $.ajax({
                url: '/CatComponentes/guardarModificacionesComponentes',
                type: 'POST',
                dataType: 'json',
                //async: false,
                contentType: 'application/json',
                data: JSON.stringify({
                    descripcionComponente: txtFiltroNoComponente.val(),
                    subconjunto: cboSubconjunto.val(),
                    cc: obrasFinal,
                    estatusActual: cboFiltroEstatus.val() == 0 ? false : true,
                    locacion: txtLocacion.val(),
                    modelo: cboModeloInventario.val(),

                    cicloVidaHoras: parseInt(cicloVidaHorasLocal),
                    garantia: parseInt(GarantiaLocal),
                    estatusNuevo: parseInt(EstatusLocal),
                }),
                success: function (response) {
                    AlertaGeneral("Alerta", "Se actualizaron los datos");
                    resetModalModificar();
                    clickBuscar();
                    btnModalModificarCancelar.click();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function resetModalModificar() {
            if ($("#cbCicloVidaHoras").is(':checked')) { $("#cbCicloVidaHoras").click(); }
            if ($("#cbGarantia").is(':checked')) { $("#cbGarantia").click(); }
            if ($("#cbEstatus").is(':checked')) { $("#cbEstatus").is(':checked').click(); }
            txtModalModificarHoraCiclo.val("0");
            txtModalModificarGarantia.val("0");
            cboModalModificarEstatus.val("1");
        }

        function CboSubConjuntoCargar() {
            if (cboConjunto.val() != null && cboConjunto.val() != "") {
                cboSubconjunto.fillCombo('/CatComponentes/FillCboSubConjunto_Componente', { idConjunto: cboConjunto.val(), idModelo: -1 });
                cboSubconjunto.attr('disabled', false);
            }
            else {
                cboSubconjunto.clearCombo();
                cboSubconjunto.attr('disabled', true);
            }
        }

        function eliminarComponente() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatComponentes/EliminarComponente',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idComponente: idComponente }),
                success: function (response) {
                    $.unblockUI();
                    ConfirmacionGeneral("Confirmación", "Se ha eliminado el componente", "bg-green");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function FillCboLocacionComponente() {
            var idModelo = cboModalModeloMaquina.val();
            if (idModelo == "") { idModelo = "-1"; }
            cboModalLocacion.fillCombo('/CatComponentes/FillCboPosiciones_Locaciones', { idModelo: idModelo, tipoBusqueda: Actualizacion });
        }

        function FillCboModeloMaquina() {
            if (cboModalModeloMaquina.val() != null && cboModalModeloMaquina.val() != "") {
                //Prefijo
                cboModalModalModeloPrefijo.fillCombo('/CatComponentes/cboModeloPrefijo', { idModelo: cboModalModeloMaquina.val() });
                cboModalModalModeloPrefijo.attr('disabled', false);
                //Conjunto
                cboModalConjunto.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: cboModalModeloMaquina.val() });
                cboModalConjunto.attr('disabled', false);
                //Locacion
                FillCboLocacionComponente();
                cboModalLocacion.attr('disabled', false);
            }
            else {
                //Prefijo
                cboModalModalModeloPrefijo.clearCombo();
                cboModalModalModeloPrefijo.attr('disabled', true);
                //Conjunto
                cboModalConjunto.clearCombo();
                cboModalModalModeloPrefijo.attr('disabled', true);
                //Locacion
                cboModalLocacion.clearCombo();
                cboModalLocacion.attr('disabled', true);
            }
        }

        function FillComponente() {
            getFolio(getObjetoFolio());
        }

        function subConjuntoChange() {
            var subConjunto = cboModalSubConjunto.find(':selected').attr('data-prefijo');
            if (subConjunto != "") {
                txtModalnoComponente.attr('disabled', true);
            }
            else {
                txtModalnoComponente.attr('disabled', false);
            }
            FillNoComponente();

            if (cboModalSubConjunto.val() != null && cboModalSubConjunto.val() != "" && cboModalSubConjunto.find(':selected').attr('data-hasposicion') == "true") {
                //Posicion
                cboModalPosicion.attr('disabled', false);
            }
            else {
                //Posicion
                cboModalPosicion.attr('disabled', true);
                cboModalPosicion.val("");
            }
            cboModaldNumParte.fillCombo('/CatComponentes/FillCboNumParte', { idModelo: cboModalModeloMaquina.val(), idSubconjunto: cboModalSubConjunto.val() });
            cboModalPosicion.fillCombo('/CatComponentes/FillCboPosiciones_Componente', { idSubconjunto: cboModalSubConjunto.val() });
        }

        function FillNoComponente() {
            var banderaUpdate = btnGuardar.attr("data-tipoGuardado") == "1";
            if (!banderaUpdate) {
                var modelo = "";
                var cc = "";
                var conjunto = "";
                var subConjunto = "";
                var posicion = "";

                if (cboModalCentroCostos.val() != null && cboModalCentroCostos.val() != "" && cboModalConjunto.find(':selected').attr('data-prefijo') != "null") {
                    cc = cboModalCentroCostos.find(':selected').attr('data-prefijo').trim();
                }
                if (cboModalConjunto.val() != null && cboModalConjunto.val() != "" && cboModalConjunto.find(':selected').attr('data-prefijo') != "null") {
                    conjunto = cboModalConjunto.find(':selected').attr('data-prefijo').trim();
                }
                if (cboModalSubConjunto.val() != null && cboModalSubConjunto.val() != "" && cboModalSubConjunto.find(':selected').attr('data-prefijo') != "null") {
                    subConjunto = cboModalSubConjunto.find(':selected').attr('data-prefijo').trim();
                }
                //if (cboModalPosicion.val() != 0) {
                //    posicion = getNomCortoPos(cboModalPosicion.val());
                //}
                if (cboModalModalModeloPrefijo.val() != null && cboModalModalModeloPrefijo.val() != "") {
                    modelo = cboModalModalModeloPrefijo.val().trim();
                }
                numComponente = modelo + cc + conjunto + subConjunto;
                txtModalnoComponente.val(numComponente);
            }
        }

        function FillCboPosicion() {
            if (cboModalSubConjunto.val() != null && cboModalSubConjunto.val() != "") {
                cboModalPosicion.fillCombo('/CatComponentes/cboModalPosicion', { idSubConjunto: cboModalSubConjunto.val() });
                cboModalPosicion.attr('disabled', false);
            }
            else {
                cboModalPosicion.clearCombo();
                cboModalPosicion.attr('disabled', true);
            }
        }

        function CboSubConjuntoModal() {
            FillCboSubConjunto(cboModalModeloMaquina.val());
        }

        function FillCboSubConjunto(idModelo, valor) {
            if (cboModalConjunto.val() != null && cboModalConjunto.val() != "") {
                $.ajax({
                    url: '/CatComponentes/FillCboSubConjunto_Componente',
                    type: 'POST',
                    dataType: 'json',
                    async: false,
                    contentType: 'application/json',
                    data: JSON.stringify({ idConjunto: cboModalConjunto.val(), idModelo: idModelo }),
                    success: function (response) {
                        if (response.success) {
                            cboModalSubConjunto.children().remove();
                            cboModalSubConjunto.append("<option value=''>--Seleccione--</option>");
                            $.each(response.items, function () {
                                if (this.Value != null) {
                                    cboModalSubConjunto.append("<option value='" + this.Value + "' name='" + this.Text + "' data-prefijo='" +
                                        this.Prefijo + "' data-hasposicion = '" + this.hasPosicion + "'>" + this.Text + "</option>");
                                }
                            });
                        } else if (response.message != '') { alertMsg(response.message); }
                    },
                    error: function (response) {
                        AlertaGeneral("Alerta", response.message);
                    }
                });
                cboModalSubConjunto.attr('disabled', false);
                FillNoComponente();
            }
            else {
                cboModalSubConjunto.clearCombo();
                cboModalSubConjunto.attr('disabled', true);
            }
        }

        function FillCboModelo() {
            txtModalnoComponente.val('');
            if (cboModalGrupoMaquina.val() != null && cboModalGrupoMaquina.val() != "") {
                cboModalModeloMaquina.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboModalGrupoMaquina.val() });
                cboModalModeloMaquina.attr('disabled', false);
            }
            else {
                cboModalModeloMaquina.clearCombo();
                cboModalModeloMaquina.attr('disabled', true);
            }
        }

        function clickBuscar() {
            filtrarGrid();
        }

        function openModal() {
            if (tipoUsuarioC < 3 || tipoUsuarioC == 7) {

                tituloModal.text("Alta Componente");
                reset();
                cboModalEstatus.prop('disabled', true);
                cboModalModeloMaquina.prop('disabled', true);
                cboModalModalModeloPrefijo.prop('disabled', true);
                cboModalConjunto.prop('disabled', true);
                cboModalSubConjunto.prop('disabled', true);
                //cboModalPosicion.prop('disabled', true);
                txtModalFecha.prop('disabled', false);
                txtModalHoraCicloActual.prop('disabled', false);
                txtModalHoraAcumuladas.prop('disabled', false);
                cboModalLocacion.prop('disabled', false);
                btnGuardar.attr("data-tipoGuardado", "0");
                modalAcciones.modal('show');
            }
        }

        function openModalModificar() {
            if (tipoUsuarioC < 3 || tipoUsuarioC == 7) {
                modalModificar.modal("show");
            }
        }

        function update() {
            tituloModal.text("Actualizar Componente");
            cboModalEstatus.prop('disabled', false);
            //cboModalLocacion.prop('disabled', false);            
            modalAcciones.modal('show');
            locacionActual = cboModalLocacion.val();
        }

        function guardar() {
            beforeSaveOrUpdate();
        }

        function beforeSaveOrUpdate() {
            if (valid()) {
                saveOrUpdate(getPlainObject(), getObjetoFolio());
            }
        }

        function getPlainObject() {

            return {
                id: idComponente,
                noComponente: txtModalnoComponente.val(),
                centroCostos: cboModalCentroCostos.val().trim(),
                //descripcion: txtModalDescripcion.val().toUpperCase().trim(),
                numParte: cboModaldNumParte.val() != "" ? cboModaldNumParte.find(':selected').text() : "",
                costo: txtCosto.val(),
                ordenCompra: txtOrdenCompra.val().trim(),
                cicloVidaHoras: txtModalHoraCiclo.val(),
                horaCicloActual: txtModalHoraCicloActual.val(),
                horasAcumuladas: txtModalHoraAcumuladas.val(),
                vidaInicio: txtModalVidas.val(),
                intercambio: ckIntercambioAlta.prop('checked'),
                conjuntoID: cboModalConjunto.val(),
                subConjuntoID: cboModalSubConjunto.val(),
                modeloEquipoID: cboModalModeloMaquina.val(),
                grupoID: cboModalGrupoMaquina.val(),
                posicionID: cboModalPosicion.val(),
                nombre_Corto: cboModalModalModeloPrefijo.val(),
                fecha: txtModalFecha.val(),
                estatus: cboModalEstatus.val() == estatus.ACTIVO ? true : false,
                marcaComponenteID: cboModalMarcaComponente.val(),
                proveedorID: cboModalProveedor.val(),
                horasCicloInicio: txtModalHoraCicloActual.val(),
                horasAcumuladasInicio: txtModalHoraAcumuladas.val()
            }
        }

        function getObjetoFolio() {
            return {
                id: 0,
                modeloID: cboModalModalModeloPrefijo.find(':selected').attr('data-prefijo'),
                conjuntoID: cboModalConjunto.val(),
                subConjuntoID: cboModalSubConjunto.val(),
                cc: cboModalCentroCostos.val(),
                folio: 0,
                prefijo: cboModalModalModeloPrefijo.val()
            }
        }

        function filtrarGrid() {

            let obras = $("#cboCC option:selected");
            let obrasFinal = [];
            for (let i = 0; i < obras.length; i++) {
                obrasFinal.push({ Value: $(obras[i]).val(), Text: $(obras[i]).text(), Prefijo: $(obras[i]).attr("data-prefijo") });
            }
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatComponentes/FillGrid_Componente',
                type: 'POST',
                dataType: 'json',
                //async: false,
                contentType: 'application/json',
                data: JSON.stringify({ componente: getFiltrosObject(), conjuntos: cboConjunto.val(), subconjuntos: cboSubconjunto.val(), cc: obrasFinal }),
                success: function (response) {
                    //$.unblockUI(); 
                    if (response.success) {
                        //gridResultado.bootgrid({
                        //    rowCount: -1,
                        //    templates: {
                        //        header: ""
                        //    }
                        //});
                        gridResultado.bootgrid("clear");
                        gridResultado.bootgrid("append", response.rows);
                        gridResultado.bootgrid('reload');
                        //setTimeout(function () {
                        $.unblockUI();
                        //}, 3000);
                    }
                    else {
                        $.unblockUI();
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                }
            });


            //loadGrid(getFiltrosObject(), ruta, gridResultado);
        }

        function getFiltrosObject() {
            return {
                id: 0,
                noComponente: txtFiltroNoComponente.val().trim(),
                //descripcion: txtFiltroDescripcion.val(),
                Estatus: cboFiltroEstatus.val() == estatus.ACTIVO ? true : false,
                centroCostos: cboCC.val() == "" ? "-1" : cboCC.val(),
                conjuntoID: cboConjunto.val() == "" ? "-1" : cboConjunto.val(),
                subConjuntoID: cboSubconjunto.val() == "" ? "-1" : (cboSubconjunto.val() == null ? "-1" : cboSubconjunto.val()),
                locacion: txtLocacion.val(),
                modeloEquipoID: cboModeloInventario.val() == "" ? "-1" : (cboModeloInventario.val() == null ? "-1" : cboModeloInventario.val())
            }
        }

        function valid() {
            var state = true;
            if (!txtModalnoComponente.valid()) { state = false; }
            //if (!txtModalDescripcion.valid()) { state = false; }
            // if (!txtModalNoSerie.valid()) { state = false; }
            //if (!cboModaldNumParte.valid()) { state = false; }
            if (!txtCosto.valid()) { state = false; }
            if (!txtOrdenCompra.valid()) { state = false; }
            if (!txtModalHoraCiclo.valid()) { state = false; }
            if (!txtModalHoraCicloActual.valid()) { state = false; }
            if (!txtModalHoraAcumuladas.valid()) { state = false; }
            if (!txtModalVidas.valid()) { state = false; }
            if (!cboModalGrupoMaquina.valid()) { state = false; }
            if (!cboModalModeloMaquina.valid()) { state = false; }
            if (!cboModalConjunto.valid()) { state = false; }
            if (!cboModalSubConjunto.valid()) { state = false; }
            if (!txtModalFecha.valid()) { state = false; }
            if (!cboModalMarcaComponente.valid()) { state = false; }
            if (!cboModalProveedor.valid()) { state = false; }
            if (!cboModalCentroCostos.valid()) { state = false; }
            if (!cboModalLocacion.valid()) { state = false; }
            return state;
        }

        function getFolio(obj) {
            $.ajax({
                url: '/CatComponentes/getFolio_Componente',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                success: function (response) {
                    FillNoComponente();
                    var temp = txtModalnoComponente.val();
                    txtModalnoComponente.val(temp + response.items);
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function saveOrUpdate(obj1, obj2) {
            $.blockUI({ message: mensajes.PROCESANDO });
            var fechaInstalacion = Actualizacion == 1 ? txtModalFecha.val() : txtModalFechaInstalacion.val();
            $.ajax({
                url: '/CatComponentes/SaveOrUpdate_Componente',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj1: obj1, obj2: obj2, Actualizacion: Actualizacion, locacion: cboModalLocacion.val(), fechaInstalacion: fechaInstalacion, tipoLocacion: $("#cboModalLocacion option:selected").attr("data-prefijo") }),
                success: function (response) {
                    modalAcciones.modal('hide');
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    reset();
                    if (Actualizacion == 1) { resetFiltros(); }
                    else { filtrarGrid(); }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //function getNomCortoPos(val) {
        //    switch (val) {
        //        case "1":
        //            return "C";
        //        case "2":
        //            return "D";
        //        case "3":
        //            return "I";
        //        case "4":
        //            return "TD";
        //        case "5":
        //            return "TI";
        //        case "6":
        //            return "D";
        //        case "7":
        //            return "DD";
        //        case "8":
        //            return "DI";
        //        default:
        //            return "";
        //    }
        //}

        function resetFiltros() {
            cboFiltroEstatus.val('1');
            //txtFiltroDescripcion.val('');
            txtFiltroNoComponente.val('');
            gridResultado.bootgrid('clear');
        }

        function initGrid() {
            gridResultado.bootgrid({
                headerCssClass: 'thead-light',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-noComponente='" + row.noComponente + "'data-centroCostos='" + row.centroCostos +
                            "' data-costo='" + row.costo + "' data-ordenCompra='" + row.ordenCompra + "' data-numParte='" + row.numParte + "' data-cicloVidaHoras='" + row.cicloVidaHoras + "' data-horaCicloActual='" + row.horaCicloActual +
                            "' data-vidaInicio='" + row.vidaInicio + "' data-horasAcumuladas='" + row.horasAcumuladas + "' data-conjuntoID='" + row.conjuntoID + "' data-subConjuntoID='" + row.subConjuntoID +
                            "' data-modeloID='" + row.modeloEquipoID + "' data-grupoID='" + row.grupoID + "' data-posicion='" + row.posicionID + "' data-marcaComponenteID='" + row.marcaComponenteID +
                            "' data-estatus='" + (row.estatus == true ? "ACTIVO" : "INACTIVO") + "' data-descripcionModal='" + row.descripcionModal + "' data-prefijo='" + row.nombre_Corto + "' data-fecha='" + row.fecha + "' data-proveedorid='" +
                            row.proveedorID + "' data-locacionID='" + row.locacionID + "' data-intercambio='" + row.intercambio + "' " + ((tipoUsuarioC > 2 && tipoUsuarioC < 7) ? "disabled" : "") + ">" + "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>";
                    },
                    "noComponente": function (column, row) {
                        return "<button type='button' class='btn btn-primary buscarComponente' data-index='" + row.id + "' data-noComponente='" + row.noComponente + "' data-locacion='" + row.locacion + "' data-tipoLocacion='" + row.tipoLocacion + "' style='width:100%'>" +
                            row.noComponente +
                            " </button>";
                    },
                    "intercambio": function (column, row) {
                        return '<span class="reciclado ' + (row.intercambio ? 'glyphicon glyphicon-ok' : '') + '"> </span>';
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                var estado = $(this).find(".eliminar").attr("data-estatus");
                if (estado == "ACTIVO") { $(this).find(".eliminar").prop('disabled', false); }
                else { $(this).find(".eliminar").prop('disabled', true); }
                gridResultado.find(".modificar").on("click", function (e) {
                    if (tipoUsuarioC < 3 || tipoUsuarioC == 7) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        idComponente = $(this).attr("data-index");
                        Actualizacion = 2;
                        datosActualizacion($(this));
                        update();
                        btnGuardar.attr("data-tipoGuardado", "1");
                    }
                });

                gridResultado.find(".buscarComponente").on("click", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    var locacionActual = $(this).attr("data-locacion");
                    var tipoLocacion = $(this).attr("data-tipoLocacion");
                    switch (tipoLocacion) {
                        case "0":
                            $("#txtFiltroNoComponenteOperando").val($(this).attr("data-noComponente"));
                            $("#tabOperando").click();
                            break;
                        case "1":
                            $("#txtFiltroNoComponenteAlmacen").val($(this).attr("data-noComponente"));
                            $("#tabAlmacen").click();
                            break;
                        case "3":
                            $("#txtFiltroNoComponenteInactivos").val($(this).attr("data-noComponente"));
                            $("#tabInactivos").click();
                            break;
                        default:
                            $("#txtFiltroNoComponenteCRC").val($(this).attr("data-noComponente"));
                            $("#tabCRC").click();
                            break;
                    }
                });
                //gridResultado.find(".eliminar").on("click", function (e) {
                //    var estado = $(this).attr("data-estatus");

                //    if (estado == "ACTIVO") {
                //        idComponente = $(this).attr("data-index");
                //        Actualizacion = 3;
                //        datosActualizacion($(this));
                //        cboModalEstatus.val("0");
                //        ConfirmacionEliminacion("Dar baja registro", "¿Esta seguro que desea dar de baja este registro? " + $(this).attr("data-noComponente"));
                //    }
                //    else {
                //        reset();
                //    }
                //});
            });
        }

        function datosActualizacion(row) {
            if (ckIntercambioAlta.prop('checked')) { ckIntercambioAlta.click(); }
            cboModalCentroCostos.val(row.attr("data-centroCostos")).prop('disabled', true);
            txtCosto.val(row.attr("data-costo"));
            txtOrdenCompra.val(row.attr("data-ordenCompra"));
            txtModalHoraCiclo.val(row.attr("data-cicloVidaHoras"));
            txtModalHoraCicloActual.val(row.attr("data-horaCicloActual"));
            txtModalHoraAcumuladas.val(row.attr("data-horasAcumuladas"));
            txtModalVidas.val(row.attr("data-vidaInicio"));
            if (row.attr("data-intercambio") == "true") { ckIntercambioAlta.click(); }
            cboModalGrupoMaquina.val(row.attr("data-grupoID")).prop('disabled', true);
            cboModalGrupoMaquina.change();
            cboModalModeloMaquina.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: row.attr("data-grupoID") });
            cboModalModeloMaquina.val(row.attr("data-modeloid")).prop('disabled', true);
            cboModalModeloMaquina.change();
            cboModalConjunto.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: -1 });
            cboModalConjunto.val(row.attr("data-conjuntoID")).prop('disabled', true);
            cboModalConjunto.change();
            cboModalSubConjunto.val(row.attr("data-subConjuntoID")).prop('disabled', true);
            cboModalSubConjunto.change();
            cboModalModalModeloPrefijo.fillCombo('/CatComponentes/cboModeloPrefijo', { idModelo: cboModalModeloMaquina.val() });
            cboModalModalModeloPrefijo.val(row.attr("data-prefijo")).prop('disabled', true);
            cboModalPosicion.val(row.attr("data-posicion"));
            cboModalEstatus.val(row.attr("data-estatus") == "ACTIVO" ? "1" : "0");
            FillCboLocacionComponente();
            var locacion = row.attr("data-locacionID");
            cboModalLocacion.val(locacion);
            txtModalnoComponente.val(row.attr("data-noComponente")).prop('disabled', true);
            row.attr("data-marcaComponenteID") == "null" ? cboModalMarcaComponente.val("") : cboModalMarcaComponente.val(row.attr("data-marcaComponenteID"));
            txtModalFecha.datepicker().datepicker("setDate", new Date(row.attr("data-fecha").match(/\d+/)[0] * 1));
            row.attr("data-proveedorid") == "null" ? cboModalProveedor.val("") : cboModalProveedor.val(row.attr("data-proveedorid"));
            row.attr("data-numParte") == "null" ? cboModaldNumParte.val("") : $("#cboModaldNumParte option:contains(" + row.attr("data-numParte") + ")").attr('selected', 'selected');
            txtModalFecha.prop('disabled', true);
            txtModalHoraCicloActual.prop('disabled', true);
            txtModalHoraAcumuladas.prop('disabled', true);
            cboModalLocacion.prop('disabled', true);
        }

        function reset() {
            idComponente = 0;
            Actualizacion = 1;
            txtModalnoComponente.val('').prop('disabled', true);
            cboModalCentroCostos.val('').prop('disabled', false);
            cboModalModalModeloPrefijo.val('').prop('disabled', false);

            //txtModalDescripcion.val('');
            //txtModalNoSerie.val('');
            cboModaldNumParte.val('');
            txtCosto.val('');
            txtOrdenCompra.val('');
            txtModalHoraCiclo.val('');
            txtModalHoraCicloActual.val('');
            txtModalHoraAcumuladas.val('');
            txtModalVidas.val('');
            if (ckIntercambioAlta.prop('checked')) { ckIntercambioAlta.click(); }
            txtModalFecha.datepicker().datepicker("setDate", new Date());

            cboModalConjunto.val('');
            cboModalSubConjunto.val('');
            cboModalGrupoMaquina.val('');
            cboModalModeloMaquina.val('');
            cboModalPosicion.val('');
            cboModalProveedor.val('');
            cboModalConjunto.prop('disabled', false);
            cboModalSubConjunto.prop('disabled', false);
            cboModalGrupoMaquina.prop('disabled', false);
            cboModalModeloMaquina.prop('disabled', false);
            cboModalPosicion.prop('disabled', false);
            txtModaldMarca.val('');
            cboModalEstatus.val('1');
            cboModalGrupoMaquina.addClass('required');
            cboModalModeloMaquina.addClass('required');


            cboModalModalModeloPrefijo.clearCombo();
            cboModalModeloMaquina.clearCombo();
            cboModalSubConjunto.clearCombo();
            cboModalLocacion.clearCombo();
            cboModaldNumParte.clearCombo();
            frmModal.validate().resetForm();
            cboModalMarcaComponente.val("");
            locacionActual = 0;
        }

        function cargarFechaInstalacion() {
            if (cboModalLocacion.val() != locacionActual && Actualizacion != 1) { divFechaInstalacion.show(); }
            else { divFechaInstalacion.css("display", "none"); }
        }

        init();

    };

    $(document).ready(function () {
        maquinaria.overhaul.componente = new componente();
    });
})();


