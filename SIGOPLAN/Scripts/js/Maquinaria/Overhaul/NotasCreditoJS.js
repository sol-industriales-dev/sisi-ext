
(function () {

    $.namespace('maquinaria.Overhaul.NotasCredito');

    NotasCredito = function () {
        const selCC = $("#selCC");
        const selCCg = $("#selCCg");
        let _ID = 0;
        let cboTipo = $("#cboTipo");
        let fupAdjunto2 = $("#fupAdjunto2");
        const btnSubirN = $("#btnSubirN");
        let idNotaCreditoGlobal = 0;
        const txtValOC = $("#txtValOC");
        const txtValFactura = $("#txtValFactura");
        const btnAplicar = $("#btnAplicar");
        const modalConfirmacionDelete = $("#modalConfirmacionDelete");
        const evidenciaFile = $('#evidenciaFile');
        const btnConfirmacionDelete = $("#btnConfirmacionDelete");
        let EditID = 0;
        let idNotaCredito = "";
        const divFacturaComentarios = $("#divFacturaComentarios");
        const modalAceptacion = $("#modalAceptacion");
        const tbCantidadAbono = $("#tbCantidadAbono");
        const tbClaveCredito = $("#tbClaveCredito");
        const btnGuardarAceptacion = $("#btnGuardarAceptacion");
        const btnNuevaNotaCredito = $("#btnNuevaNotaCredito");

        let BanderaValidarGuardar = true;

        //#region Casco Reman,
        const divAlmacen = $('#divAlmacen');
        const comboAlmacen = $('#comboAlmacen');
        const comboAlmacen2 = $('#comboAlmacen2');
        const divCentroCostos = $('#divCentroCostos');
        const divInsumo = $('#divInsumo');
        const divGenerador = $('#divGenerador');
        const inputInsumo = $('#inputInsumo');
        const inputInsumoDescripcion = $("#inputInsumoDescripcion");
        const divSerieComponente = $('#divSerieComponente');
        const divCausaRemosion = $('#divCausaRemosion');
        const divHorometroEquipo = $('#divHorometroEquipo');
        const divFechaEntregaCasco = $('#divFechaEntregaCasco');
        const inputFechaCasco = $('#inputFechaCasco');
        const divHorometroComponente = $('#divHorometroComponente');
        const divMontoTotalOC = $('#divMontoTotalOC');
        const inputMontoTotalOC = $('#inputMontoTotalOC');
        const divMontotalPesos = $('#divMontotalPesos');
        const divMontoTotalDlls = $('#divMontoTotalDlls');
        //#endregion

        const mensajes = {
            NOMBRE: 'Reporte detalle de muestras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        const btnMenuPrincipal = $("#btnMenuPrincipal");
        const btnEnviarCorreo = $("#btnEnviarCorreo");
        const tbFactura = $("#tbFactura");
        const txtComentarios = $("#txtComentarios");
        const modalListaArchivos = $("#modalListaArchivos");
        const tblListaArchivo = $("#tblListaArchivo");
        const btnEditarAccion = $("#btnEditarAccion");
        const btnAddComentario = $("#btnAddComentario");
        const ulComentarios = $("#ulComentarios");
        const divVerComentario = $("#divVerComentario");
        const mdlComentarioRechazo = $('#mdlComentarioRechazo');
        const comentarioRechazo = $('#comentarioRechazo');
        const btnDescargarEvidencia = $('#btnDescargarEvidencia');
        const cboEstatus = $("#cboEstatus");
        const btnImprimirReporte = $("#btnImprimirReporte");
        const tbFechaInicio = $("#tbFechaInicio");
        const tbFechaFin = $("#tbFechaFin");
        const tbGenerador = $("#tbGenerador");
        const tbOC = $("#tbOC");
        const cboEconomico = $("#cboEconomico");
        const tbModelo = $("#tbModelo");
        const tbSerieEquipo = $("#tbSerieEquipo");
        const tbSerieComponente = $("#tbSerieComponente");
        const tbDescripcion = $("#tbDescripcion");
        const tbFecha = $("#tbFecha");
        const cboCausaRemosion = $("#cboCausaRemosion");
        const tbHorometroEquipo = $("#tbHorometroEquipo");
        const tbHorometroComponente = $("#tbHorometroComponente");
        const tbMontoPesos = $("#tbMontoPesos");
        const tbMontoDLL = $("#tbMontoDLL");
        const ireport = $("#report");
        const fupAdjunto = $("#fupAdjunto");

        const cboTipoNC = $("#cboTipoNC");
        const cboFiltroTipo = $("#cboFiltroTipo");
        const tbComentarioRechazo = $("#tbComentarioRechazo");
        const btnBuscar = $("#btnBuscar");
        function init() {
            $.fn.dataTable.moment('DD/MM/YYYY');
            cboFiltroTipo.fillCombo('/Overhaul/FillcboFiltroTipo', null, false);
            fnInicializacion();
            loadTable();
            fnEventListener();
        }

        function fnEventListener() {
            //Click Events
            btnAplicar.click(fnAplicar);
            btnNuevaNotaCredito.click(openModal);
            btnGuardarAceptacion.click(AccionGuardar);
            btnImprimirReporte.click(verReporte);
            btnConfirmacionDelete.click(UpdateDelete);
            btnAddComentario.click(Guardarcomentario);
            btnSubirN.click(subir);
            btnEditarAccion.click(EnviarEditar);
            btnEnviarCorreo.click(EnviarCorreo);
            btnBuscar.click(loadTable);
            //btnMenuPrincipal.click(loadTable);
            //Change Events
            cboFiltroTipo.change(fnTipo);
            cboEconomico.change(loadInfoMaquina);
            //cboFiltroTipo.change(loadTable);
            cboEstatus.change(EstatusSetImpresion);
            //tbFechaInicio.change(loadTable);
            //tbFechaFin.change(loadTable);
            //selCC.change(loadTable);
            cboTipoNC.change(fnTipoNotaCredito);
        }
        function fnTipo() {
            if ($(this).val() == "2") {
                $('.clsCascoReman2').removeClass('hide');
                comboAlmacen2.val('');
                comboAlmacen.trigger('change');
            }
            else {
                $('.clsCascoReman2').addClass('hide');
            }
        }
        function fnInicializacion() {

            tbFecha.datepicker().datepicker("setDate", new Date());
            inputFechaCasco.datepicker().datepicker("setDate", new Date());
            tbFechaInicio.datepicker().datepicker("setDate", new Date());
            tbFechaFin.datepicker().datepicker("setDate", new Date());

            $('.clsNotaCredito').addClass('hide');
            $('.clsCascoReman').addClass('hide');
            $('.clsCascoReman2').addClass('hide');
            //Cargado de combos 
            selCC.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
            selCCg.fillCombo('/Administrativo/Facultamiento/getComboCCEnkontrol', null, false, null);
            comboAlmacen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false);
            comboAlmacen2.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false);
            cboFiltroTipo.fillCombo('/Overhaul/FillcboFiltroTipo', null, false);
            cboTipoNC.fillCombo('/Overhaul/FillcboFiltroTipo', null, false, null);
            //Auticomplete numero de insumo
            inputInsumo.getAutocompleteValid(setInsumoDesc, validarInsumo, { porDesc: false }, '/Overhaul/getInsumo');
            // Autocomplete descripción de insumo
            inputInsumoDescripcion.getAutocompleteValid(setInsumoBusqPorDesc, validarInsumo, { porDesc: true }, '/Overhaul/getInsumo');

        }

        function setInsumoDesc(e, ui) {
            inputInsumo.val(ui.item.value);
            inputInsumoDescripcion.val(ui.item.id);
        }

        function setInsumoBusqPorDesc(e, ui) {
            inputInsumo.val(ui.item.id);
            inputInsumoDescripcion.val(ui.item.value);
        }

        function validarInsumo(e, ul) {
            if (ul.item == null) {
                inputInsumo.val('');
                inputInsumoDescripcion.val('');
            }
        }

        function fnTipoNotaCredito() {
            fnTipoNotasCreditoMostrar($(this).val())
        }

        function fnTipoNotasCreditoMostrar(tipoNota) {
            if (tipoNota == "1") {
                $('.clsNotaCredito').removeClass('hide');
                $('.clsCascoReman').addClass('hide');
                btnGuardarAceptacion.prop('disabled', false);
            }
            else if (tipoNota == "2") {
                $('.clsCascoReman').removeClass('hide');
                $('.clsNotaCredito').addClass('hide');
                btnGuardarAceptacion.prop('disabled', false);
            }
            else {
                $('.clsCascoReman').addClass('hide');
                $('.clsNotaCredito').addClass('hide');
                btnGuardarAceptacion.prop('disabled', true);
            }
        }

        function fnAplicar() {
            if (txtValOC.val() != '' && txtValFactura.val() != '') {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Overhaul/setFactura",
                    data: { id: _ID, oc: txtValOC.val(), factura: txtValFactura.val() },
                    success: function (response) {
                        LoadListaArchivos(Number(_ID))
                        CargarInformacionNota(Number(_ID));
                        btnSubirN.attr('data-id', _ID);
                        modalListaArchivos.modal('show');
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            {
                AlertaGeneral("Alerta", "¡Todos los campos son obligatorios para poder descargar los archivos!");
            }

        }

        function EnviarCorreo() {
            if (txtValOC.val() != '' && txtValFactura.val() != '') {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: "/Overhaul/SendCorreoArchivos",
                    data: {
                        obj: Number(btnEnviarCorreo.attr('data-id')), oc: txtValOC.val(), factura: txtValFactura.val()
                    },
                    success: function (response) {

                        AlertaGeneral("Confirmación", "¡Nota aplicada correctamente, el correo fue enviado!");
                        tbFactura.val("");
                        txtComentarios.val("");
                        $("#modalListaArchivos .close").click();
                        $.unblockUI();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {
                AlertaGeneral("Alerta", "¡Todos los campos son obligatorios!");
            }
        }

        function EnviarEditar() {
            SaveEditDatos();
        }

        function subir() {
            idNotaCredito = btnSubirN.attr('data-id');
            SubirArchivo(null, idNotaCredito)
        }

        function EstatusSetImpresion() {
            if (cboEstatus.val() == 1) {
                tbFechaInicio.prev('label').text('Fecha inicio nota de crédito');
                tbFechaFin.prev('label').text('Fecha fin nota de crédito');
            }
            else {
                tbFechaInicio.prev('label').text('Fecha inicio captura');
                tbFechaFin.prev('label').text('Fecha fin captura');
            }
            loadTable();
        }

        function Guardarcomentario() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Overhaul/guardarComentario",
                data: { obj: getComentarioNotaCredito(idNotaCredito), tipoComentario: btnAddComentario.attr('data-Tipo') },
                success: function (response) {

                    let comentarios = response.obj;
                    setComentarios(comentarios);

                    tbFactura.val("");
                    txtComentarios.val("");
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function getComentarioNotaCredito(idNota) {

            return {
                id: 0,
                notaCreditoID: idNota,
                comentario: txtComentarios.val(),
                usuarioNombre: '',
                usuarioID: '',
                fecha: Date.now,
                factura: tbFactura.val()
            }

        }

        function UpdateDelete() {
            if (tbComentarioRechazo.val() == '' || evidenciaFile.val() == '') {
                AlertaGeneral(`Alerta`, `Debe capturar todos los campos.`);
                return;
            }

            let state = true

            tbComentarioRechazo.addClass('required');
            if (!validarCampo(tbComentarioRechazo)) { state = false; }

            if (state) {
                SaveOrUpdateData(true);
                modalConfirmacionDelete.modal('hide');
                tbComentarioRechazo.removeClass('required');
            }
            else {
                AlertaGeneral("Alerta", "El comentario es obligatorio");
            }
        }

        function verReporte() {

            let pFechaInico = tbFechaInicio.val();
            let pFechaFin = tbFechaFin.val();
            let idReporte = "";
            let flag = true;

            if (pFechaInico == "" || pFechaFin == "") {
                flag = false;
            }
            if (flag) {

                $.blockUI({ message: mensajes.PROCESANDO });

                switch (cboEstatus.val()) {
                    case "1":
                        idReporte = "55";
                        break;
                    case "2":
                        idReporte = "32";
                        break;
                    case "3":
                        idReporte = "55";
                        break;
                    case "4":
                        idReporte = "32";
                        break;
                    default:
                }

                let path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pFechaInicio=" + tbFechaInicio.val() + "&pFechaFin=" + tbFechaFin.val() + "&tipoControl=" + (cboFiltroTipo.val() == '' ? 0 : cboFiltroTipo.val()) + "&estatus=" + cboEstatus.val() + "&cc=" + selCC.val() + "&almacen=" + (cboFiltroTipo.val() == "2" ? comboAlmacen2.val() : "");

                ireport.attr("src", path);

                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
            }
            else {
                AlertaGeneral("Alerta", "Debe seleccionar un filtro para ver el reporte", "bg-red");
            }
        }

        function AccionGuardar() {
            let bandera = btnGuardarAceptacion.attr('data-accion');

            if (bandera == "true") {
                if (validaInformacion()) {
                    SaveOrUpdateData(false);
                }
            } else {
                let idNotaCredito = btnGuardarAceptacion.attr('data-id');
                let AbonoDLL = tbCantidadAbono.val();
                let ClaveCredito = tbClaveCredito.val();
                let OC = tbOC.val();
                if (/*validateDescarga()*/true) {
                    if (AbonoDLL != "" || ClaveCredito != "") {
                        if (validateDescarga()) {
                            if (tbCantidadAbono.val() == '' || tbClaveCredito.val() == '' || fupAdjunto.val() == '') {
                                AlertaGeneral(`Alerta`, `Debe capturar todos los campos.`);
                                return;
                            }

                            saveOrUpdate(null, idNotaCredito, AbonoDLL, ClaveCredito, OC);
                        }
                    } else {
                        SaveOrUpdateData(false);
                    }
                }
            }
        }

        function validaInformacion() {
            let state = true;

            if (cboTipoNC.val() == "1") {
                tbGenerador.addClass('required');
                cboEconomico.addClass('required');
                tbSerieComponente.addClass('required');
                tbDescripcion.addClass('required');
                tbFecha.addClass('required');
                cboCausaRemosion.addClass('required');
                tbHorometroEquipo.addClass('required');
                tbHorometroComponente.addClass('required');
                tbMontoPesos.addClass('required');
                tbMontoDLL.addClass('required');
                cboTipoNC.addClass('required');
                tbOC.addClass('required');
                if (!validarCampo(tbGenerador)) { state = false; }
                if (!validarCampo(cboEconomico)) { state = false; }
                if (!validarCampo(tbSerieComponente)) { state = false; }
                if (!validarCampo(tbDescripcion)) { state = false; }
                if (!validarCampo(tbFecha)) { state = false; }
                if (!validarCampo(cboCausaRemosion)) { state = false; }
                if (!validarCampo(tbHorometroEquipo)) { state = false; }
                if (!validarCampo(tbHorometroComponente)) { state = false; }
                if (!validarCampo(tbMontoPesos)) { state = false; }
                if (!validarCampo(tbMontoDLL)) { state = false; }
                if (!validarCampo(cboTipoNC)) { state = false; }
                if (!validarCampo(tbOC)) { state = false; }
            }
            else {
                comboAlmacen.addClass('required');
                inputInsumo.addClass('required');
                inputMontoTotalOC.addClass('required');
                tbOC.addClass('required');
                if (!validarCampo(comboAlmacen)) { state = false; }
                if (!validarCampo(inputInsumo)) { state = false; }
                if (!validarCampo(inputMontoTotalOC)) { state = false; }
                if (!validarCampo(tbOC)) { state = false; }
            }
            return state;
        }

        function validateDescarga() {
            let state = true;

            tbCantidadAbono.addClass('required');
            tbClaveCredito.addClass('required');
            fupAdjunto.addClass('required');
            tbOC.addClass('required');
            if (!validarCampoVacio(tbCantidadAbono)) { state = false; }
            if (!validarCampo(tbClaveCredito)) { state = false; }
            if (!validarCampo(fupAdjunto)) { state = false; }
            if (!validarCampo(tbOC)) { state = false; }

            return state;
        }

        function saveOrUpdate(e, idNotaCredito, AbonoDLL, ClaveCredito, OC) {
            if (true) {
                let formData = new FormData();
                let file = document.getElementById("fupAdjunto").files[0];

                formData.append("idNotaCredito", JSON.stringify(idNotaCredito));
                formData.append("OC", JSON.stringify(OC));
                formData.append("AbonoDLL", JSON.stringify(AbonoDLL));
                formData.append("ClaveCredito", JSON.stringify(ClaveCredito));
                formData.append("cc", JSON.stringify(selCC.val()));
                let files = document.getElementById("fupAdjunto").files;

                $.each(files, function (i, file) {
                    formData.append('fupAdjunto[]', file);
                });

                if (file != undefined) {
                    $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: '/Overhaul/GuardarAutorizacion',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        if (response.success) {
                            modalAceptacion.modal('hide');
                            limpiarCampos();
                            tbCantidadAbono.val('').prop('disabled', true);
                            tbClaveCredito.val('').prop('disabled', true);;
                            fupAdjunto.val('').prop('disabled', true);

                            loadTable();
                            AlertaGeneral("Confirmacion", "La nota de credito ha sido actualizada correctamente");
                        }
                        else {
                            AlertaGeneral('Alerta!', response.message);
                        }
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }

        function loadInfoMaquina() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Overhaul/LoadInfoMaquinaria",
                data: { id: $(this).val() },
                success: function (response) {
                    let data = response.DatosEconomico;

                    tbModelo.val(data.Modelo);
                    tbSerieEquipo.val(data.Serie);

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function LoadListaArchivos(idNotaCredito) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Overhaul/getListaArchivos",
                data: { obj: idNotaCredito },
                success: function (response) {
                    let dataSet = response.ListaArchivos;
                    tblListaArchivo.bootgrid("clear");
                    tblListaArchivo.bootgrid("append", dataSet);
                    tblListaArchivo.bootgrid('reload');

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }
        GridListaArchivos();
        function GridListaArchivos() {
            tblListaArchivo.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {

                    "Accion": function (column, row) {
                        return "<button type='button' class='btn btn-info descargar' data-id='" + row.id + "'>" +
                            "<span class='glyphicon glyphicon-download'></span> " +
                            " </button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblListaArchivo.find(".descargar").on("click", function (e) {
                    let elemento = $(this).attr('data-id');
                    downloadURI(elemento);
                });
            });
        }

        function downloadURI(elemento) {
            let link = document.createElement("button");
            link.download = '/Overhaul/getFileDownload?id=' + elemento;
            link.href = '/Overhaul/getFileDownload?id=' + elemento;
            link.click();
            location.href = '/Overhaul/getFileDownload?id=' + elemento;
        }


        function SubirArchivo(e, idNotaCredito) {
            if (true) {
                var formData = new FormData();

                let file = document.getElementById("fupAdjunto2").files[0];
                formData.append("TipoArchivo", JSON.stringify(2));
                formData.append("idNotaCredito", JSON.stringify(idNotaCredito));

                let files = document.getElementById("fupAdjunto2").files;
                $.each(files, function (i, file) {
                    formData.append('fupAdjunto[]', file);
                });

                if (file != undefined) {
                    $.blockUI({ message: 'Cargando archivo... ¡Espere un momento!' });
                }
                $.ajax({
                    type: "POST",
                    url: '/Overhaul/SubirNuevoArchivo',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        LoadListaArchivos(idNotaCredito);
                        fupAdjunto2.val('');
                        cboTipo.val('1');
                        AlertaGeneral("Confirmacion", "El archivo se Subio exitósamente");
                    },
                    error: function (error) {
                        $.unblockUI();
                    }
                });
            } else {
                e.preventDefault()
            }
        }

        function openModal() {

            $('.clsNotaCredito').addClass('hide');
            $('.clsCascoReman').addClass('hide');
            btnGuardarAceptacion.removeClass('hide');
            btnEditarAccion.addClass('hide');
            btnGuardarAceptacion.attr('data-accion', true);
            btnGuardarAceptacion.attr('data-id', 0);
            limpiarCampos();
            cboEconomico.fillCombo('/Overhaul/cboModalEconomico');
            cboTipoNC.trigger('change');
            modalAceptacion.modal('show');
        }

        function limpiarCampos() {
            selCCg.val("");
            tbGenerador.val('').prop('disabled', false);
            tbOC.val('').prop('disabled', false);
            cboEconomico.clearCombo();
            cboEconomico.prop('disabled', false);
            tbModelo.val('');
            tbSerieEquipo.val('');
            tbSerieComponente.val('').prop('disabled', false);
            tbDescripcion.val('').prop('disabled', false);
            tbFecha.datepicker().datepicker("setDate", new Date()).prop('disabled', false);
            cboCausaRemosion.val(1).prop('disabled', false);
            tbHorometroEquipo.val('').prop('disabled', false);
            tbHorometroComponente.val('').prop('disabled', false);
            tbMontoPesos.val('').prop('disabled', false);
            tbMontoDLL.val('').prop('disabled', false);
            tbCantidadAbono.val('').prop('disabled', true);
            tbClaveCredito.val('').prop('disabled', true);
            fupAdjunto.val('').prop('disabled', true);
            cboTipoNC.val('');
            comboAlmacen.val('');
            inputMontoTotalOC.val('');
            inputInsumo.val('');
            inputInsumoDescripcion.val('');
            inputFechaCasco.datepicker().datepicker('setDate', new Date());
        }

        function loadTable() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Overhaul/loadTableNotasCredito",
                data: { objFiltro: getFiltros() },
                success: function (response) {
                    let data = response.tblNotaCredito;
                    initGrid(data);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function getFiltros() {
            return {
                Generador: "",
                cc: selCC.val(),
                OC: "",
                idEconomico: 0,
                Descripcion: "",
                FechaFin: tbFechaFin.val(),
                FechaInicio: tbFechaInicio.val(),
                TipoFiltro: cboEstatus.val(),
                FiltroTipoNC: cboFiltroTipo.val(),
                almacen: (cboFiltroTipo.val() == "2" ? comboAlmacen2.val() : "")
            }
        }

        function GetDataSendNuevo() {
            let id = btnGuardarAceptacion.attr('data-id') != undefined ? Number(btnGuardarAceptacion.attr('data-id')) : 0;

            return {
                id: id,
                Generador: tbGenerador.val(),
                OC: tbOC.val(),
                cc: selCC.val(),
                idEconomico: cboEconomico.val(),
                SerieComponente: tbSerieComponente.val(),
                Descripcion: tbDescripcion.val(),
                Fecha: tbFecha.val(),
                CausaRemosion: cboCausaRemosion.val(),
                HorometroEconomico: tbHorometroEquipo.val(),
                HorometroComponente: tbHorometroComponente.val(),
                MontoPesos: tbMontoPesos.val(),
                MontoDLL: tbMontoDLL.val(),
                AbonoDLL: "",
                ClaveCredito: "",
                RutaArchivo: "",
                Estado: 1,
                TipoNC: cboTipoNC.val(),
                noAlmacen: comboAlmacen.val(),
                numInsumo: inputInsumo.val(),
                descripcionInsumo: inputInsumoDescripcion.val(),
                fechaCasco: inputFechaCasco.val(),
                montoTotalOC: inputMontoTotalOC.val()
            }
        }

        function GetDataEditNuevo() {
            let id = EditID;

            return {
                id: id,
                cc: selCC.val(),
                Generador: tbGenerador.val(),
                OC: tbOC.val(),
                idEconomico: cboEconomico.val(),
                SerieComponente: tbSerieComponente.val(),
                Descripcion: tbDescripcion.val(),
                Fecha: tbFecha.val(),
                CausaRemosion: cboCausaRemosion.val(),
                HorometroEconomico: tbHorometroEquipo.val(),
                HorometroComponente: tbHorometroComponente.val(),
                MontoPesos: tbMontoPesos.val(),
                MontoDLL: tbMontoDLL.val(),
                AbonoDLL: tbCantidadAbono.val(),
                ClaveCredito: tbClaveCredito.val(),
                TipoNC: cboTipoNC.val()
            }
        }

        function initGrid(dataSet) {

            let tamañoY = '40vh';
            let clase = btnMenuPrincipal.attr('class');
            if (clase == "collapsed") {
                let Tamaño = ($(window).width() * 53) / 1366;
                tamañoY = Tamaño + 'vh';
            }
            else {
                let Tamaño = ($(window).width() * 37) / 1366;
                tamañoY = Tamaño + 'vh';
            }
            tblGridNotasCredito = $('#tblNotasCredito').DataTable({
                language: dtDicEsp,
                order: [[13, "asc"]],
                "bFilter": true,
                destroy: true,
                "scrollX": true,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                data: dataSet,
                columns: [
                    { "title": 'Acciones', data: "Acciones", "width": "10%" },
                    {
                        "title": 'Abonado DLLS', data: "AbonoDLL", "visible": cboEstatus.val() == "1" ? false : true
                    },
                    { "title": '# Credito', data: "ClaveCredito", "visible": cboEstatus.val() == "1" ? false : true },
                    { "title": 'CC', data: "cc" },
                    { "title": '# Almacén', data: 'Almacen', visible: true/*cboFiltroTipo.val() == '3' ? true : false*/ },
                    { "title": 'Generador', data: "Generador" },
                    { "title": 'OC', data: "OC" },
                    { "title": 'Factura', data: "Factura", "visible": cboEstatus.val() == "4" ? true : false },
                    {
                        "title": 'Equipo', data: "Equipo", createdCell: function (td, data, rowData, row, col) {
                            $(td).html(rowData.idEquipo == 0 ? '' : rowData.Equipo);
                        }
                    },
                    { "title": 'Modelo', data: "Modelo" },
                    { "title": 'Serie Equipo', data: "SerieEquipo" },
                    { "title": 'Serie Componente', data: "SerieComponente" },
                    { "title": 'Descripción', data: "Descripcion" },
                    {
                        "title": 'Fecha', data: "Fecha", createdCell: function (td, data, rowData, row, col) {
                            $(td).html(moment(data).format('DD/MM/YYYY'));
                        }
                    },
                    { "title": 'Causa Remosion', data: "CausaRemosion" },
                    { "title": 'Hora Equipo', data: "HorometroEquipo" },
                    { "title": 'Hora Componente', data: "HoraComponente" },
                    { "title": 'Total MNX', data: "MontoPesos" },
                    { "title": 'Total DLL', data: "MontoDLL" },
                    { "title": 'Total OC', data: "MontoTotalOC" },
                    { "title": 'Diferencia', data: "Diferencia" },
                    { "title": 'Tipo', data: "TipoNC" }
                ],
                "paging": false,
                "info": false,
                footerCallback: function (tfoot, data, start, end, displa) {
                    let totalMontoAbonoDLL = 0;
                    let totalMontoPesos = 0;
                    let totalMontoDLL = 0;
                    let totalMontoOC = 0;
                    let totalMontoDiferencia = 0;
                    data.forEach(function (element, index, array) {
                        totalMontoAbonoDLL += unmaskNumero(element.AbonoDLL);
                        totalMontoPesos += unmaskNumero(element.MontoPesos);
                        totalMontoDLL += unmaskNumero(element.MontoDLL);
                        totalMontoOC += unmaskNumero(element.MontoTotalOC);
                        totalMontoDiferencia += unmaskNumero(element.Diferencia);
                    });

                    $.each($(tfoot).find('th'), element => {
                        $(element).text('');
                    });

                    if (cboEstatus.val() == 1) {

                        $(tfoot).find('th').eq(14).text(maskNumero(totalMontoPesos));
                        $(tfoot).find('th').eq(15).text(maskNumero(totalMontoDLL));
                        $(tfoot).find('th').eq(16).text(maskNumero(totalMontoOC));
                        $(tfoot).find('th').eq(17).text(maskNumero(totalMontoDiferencia));
                    }
                    if (cboEstatus.val() == 4) {

                        $(tfoot).find('th').eq(1).text(maskNumero(totalMontoAbonoDLL));
                        $(tfoot).find('th').eq(17).text(maskNumero(totalMontoPesos));
                        $(tfoot).find('th').eq(18).text(maskNumero(totalMontoDLL));
                        $(tfoot).find('th').eq(19).text(maskNumero(totalMontoOC));
                        $(tfoot).find('th').eq(20).text(maskNumero(totalMontoDiferencia));
                    }
                    if (cboEstatus.val() != 4 && cboEstatus.val() != 1) {

                        $(tfoot).find('th').eq(1).text(maskNumero(totalMontoAbonoDLL));
                        $(tfoot).find('th').eq(16).text(maskNumero(totalMontoPesos));
                        $(tfoot).find('th').eq(17).text(maskNumero(totalMontoDLL));
                        $(tfoot).find('th').eq(18).text(maskNumero(totalMontoOC));
                        $(tfoot).find('th').eq(19).text(maskNumero(totalMontoDiferencia));
                    }
                }
            });
        }

        $(document).on('click', ".setAutorizacion", function () {
            btnGuardarAceptacion.attr('data-accion', false);
            let id = $(this).attr('data-id');
            CargarInformacionNota(Number(id));
            modalAceptacion.modal('show');
        });

        $(document).on('click', ".btnEditar", function () {
            let id = $(this).attr('data-id');
            EditID = Number(id);
            CargarInformacionNota2(Number(id));
            modalAceptacion.modal('show');
        });

        $(document).on('click', ".btnListaArchivos", function () {
            $(".capOC").hide();
            if (cboEstatus.val() == '2') {
                let id = $(this).attr('data-id');
                let oc = $(this).data("oc");
                let factura = $(this).data("factura");
                _ID = id;
                if (oc != 'no' && factura != 'no') {
                    LoadListaArchivos(Number(id))
                    CargarInformacionNota(Number(id));
                    btnSubirN.attr('data-id', id);
                    modalListaArchivos.modal('show');
                }
                else {
                    LoadListaArchivos(Number(id))
                    CargarInformacionNota(Number(id));
                    btnSubirN.attr('data-id', id);
                    btnEnviarCorreo.attr('data-id', id);
                    let o = (oc == 'no' ? false : true);
                    let f = (factura == 'no' ? false : true);
                    $(".capOC").show();
                    txtValOC.val((oc == 'no' ? '' : oc));
                    txtValOC.prop("disabled", o);
                    txtValFactura.val((factura == 'no' ? '' : factura));
                    txtValFactura.prop("disabled", f);
                    modalListaArchivos.modal('show');
                }
            }
            else {
                let id = $(this).attr('data-id');
                LoadListaArchivos(Number(id))
                CargarInformacionNota(Number(id));
                btnSubirN.attr('data-id', id);
                modalListaArchivos.modal('show');
            }
        });


        $(document).on('click', ".btnAddComentariosPendientes", function () {
            let id = $(this).attr('data-id');
            idNotaCredito = id;
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetComentarios',
                type: 'POST',
                dataType: 'json',
                data: { id: Number($(this).attr('data-id')), TipoComentarios: 2 },
                success: function (response) {
                    divFacturaComentarios.addClass("hide");
                    btnAddComentario.attr('data-Tipo', 2);
                    let comentarios = response.obj;
                    setComentarios(comentarios);
                    divVerComentario.modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });

        });


        $(document).on('click', ".CbtnComentario", function () {
            idNotaCredito = $(this).attr('data-id');
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetComentarios',
                type: 'POST',
                dataType: 'json',
                data: { id: Number($(this).attr('data-id')), TipoComentarios: 1 },
                success: function (response) {

                    divFacturaComentarios.removeClass("hide");
                    btnAddComentario.attr('data-Tipo', 1);
                    let comentarios = response.obj;
                    setComentarios(comentarios);
                    divVerComentario.modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });

        });

        btnDescargarEvidencia.on('click', function () {
            location.href = '/Overhaul/DescargarEvidencia?id=' + $(this).data('idComentario') + '&nombreEvidencia=' + $(this).data('nombreEvidencia');
        });

        $(document).on('click', ".btnComentarioRechazo", function () {
            idNotaCredito = $(this).attr('data-id');
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetComentariosRechazo',
                type: 'POST',
                dataType: 'json',
                data: { id: Number($(this).attr('data-id')), TipoComentarios: 0 },
                success: function (response) {
                    comentarioRechazo.text(response.obj[0].comentario);
                    if (response.obj[0].tieneEvidencia) {
                        btnDescargarEvidencia.data('nombreEvidencia', response.obj[0].nombreArchivo);
                        btnDescargarEvidencia.data('idComentario', response.obj[0].id);
                        btnDescargarEvidencia.show();
                    }
                    else {
                        btnDescargarEvidencia.data('nombreEvidencia', 0);
                        btnDescargarEvidencia.data('idComentario', 0);
                        btnDescargarEvidencia.hide();
                    }
                    mdlComentarioRechazo.modal('show');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });

        });

        function CargarInformacionNota(id) {
            cboEconomico.fillCombo('/Overhaul/cboModalEconomico');
            BanderaValidarGuardar = true;
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetDataNotaCredito',
                type: 'POST',
                dataType: 'json',
                data: { obj: id },
                success: function (response) {

                    let data = response.tblM_CapNotaCredito;
                    fnTipoNotasCreditoMostrar(data.TipoNC)
                    if (data.TipoNC == 1) {

                        selCCg.val(data.cc);
                        BanderaValidarGuardar = data.OC != null ? true : false;
                        tbGenerador.val(data.Generador).prop('disabled', true);
                        tbOC.val(data.OC).prop('disabled', true);
                        cboEconomico.val(data.idEconomico).prop('disabled', true);
                        cboEconomico.trigger('change').prop('disabled', true);
                        tbSerieComponente.val(data.SerieComponente).prop('disabled', true);
                        tbDescripcion.val(data.Descripcion).prop('disabled', true);
                        tbFecha.val(response.Fecha).prop('disabled', true);
                        cboCausaRemosion.val(data.CausaRemosion).prop('disabled', true);
                        tbHorometroEquipo.val(data.HorometroEconomico).prop('disabled', true);
                        tbHorometroComponente.val(data.HorometroComponente).prop('disabled', true);
                        tbMontoPesos.val(data.MontoPesos).prop('disabled', true);
                        tbMontoDLL.val(data.MontoDLL).prop('disabled', true);
                        cboTipoNC.val(data.TipoNC).prop('disabled', true);
                        tbCantidadAbono.prop('disabled', false);
                        tbClaveCredito.prop('disabled', false);
                        fupAdjunto.prop('disabled', false);
                    }
                    else {

                        BanderaValidarGuardar = data.OC != null ? true : false;
                        tbGenerador.val(data.Generador).prop('disabled', true);
                        tbOC.val(data.OC).prop('disabled', true);
                        cboEconomico.val(data.idEconomico).prop('disabled', true);
                        cboEconomico.trigger('change').prop('disabled', true);
                        tbSerieComponente.val(data.SerieComponente).prop('disabled', true);
                        tbDescripcion.val(data.Descripcion).prop('disabled', true);
                        tbFecha.val(response.Fecha).prop('disabled', true);
                        cboCausaRemosion.val('').prop('disabled', true);
                        tbHorometroEquipo.val(0).prop('disabled', true);
                        tbHorometroComponente.val(0).prop('disabled', true);
                        tbMontoPesos.val(data.MontoPesos).prop('disabled', true);
                        tbMontoDLL.val(data.MontoDLL).prop('disabled', true);
                        cboTipoNC.val(data.TipoNC).prop('disabled', true);
                        comboAlmacen.val(data.noAlmacen.trim()).prop('disabled', true);
                        inputInsumo.val(data.numInsumo).prop('disabled', true);
                        inputInsumoDescripcion.val(data.descripcionInsumo).prop('disabled', true);

                        tbCantidadAbono.prop('disabled', false);
                        tbClaveCredito.prop('disabled', false);
                        inputFechaCasco.val(response.fechaCasco).prop('disabled', true);
                        inputMontoTotalOC.val(data.montoTotalOC).prop('disabled', true);

                        fupAdjunto.prop('disabled', false);
                    }
                    btnGuardarAceptacion.attr('data-id', id);
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function CargarInformacionNota2(id) {
            cboEconomico.fillCombo('/Overhaul/cboModalEconomico');
            BanderaValidarGuardar = true;
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetDataNotaCredito',
                type: 'POST',
                dataType: 'json',
                data: { obj: id },
                success: function (response) {
                    let data = response.tblM_CapNotaCredito;
                    let jsonData;
                    LimpiarAutoriza();
                    if (data.CadenaModifica != "") {
                        jsonData = JSON.parse(data.CadenaModifica);

                        if (CompareData(data.Generador, jsonData.Generador, tbGenerador));
                        if (CompareData(data.OC, jsonData.OC, tbOC));
                        if (CompareData(data.SerieComponente, jsonData.SerieComponente, tbSerieComponente));
                        if (CompareData(data.Descripcion, jsonData.Descripcion, tbDescripcion));
                        if (CompareData(data.CausaRemosion, jsonData.CausaRemosion, cboCausaRemosion));
                        if (CompareData(data.HorometroEconomico, jsonData.HorometroEconomico, tbHorometroEquipo));
                        if (CompareData(data.HorometroComponente, jsonData.HorometroComponente, tbHorometroComponente));
                        if (CompareData(data.MontoPesos, jsonData.MontoPesos, tbMontoPesos));
                        if (CompareData(data.MontoDLL, jsonData.MontoDLL, tbMontoDLL));
                        if (CompareData(data.ClaveCredito, jsonData.ClaveCredito, tbClaveCredito));

                        idNotaCreditoGlobal = data.id;
                    }
                    selCCg.val(data.cc);
                    BanderaValidarGuardar = data.OC != null ? true : false;
                    tbGenerador.val(data.Generador).prop('disabled', false);
                    tbOC.val(data.OC).prop('disabled', false);

                    cboEconomico.val(data.idEconomico).prop('disabled', false);
                    cboEconomico.trigger('change').prop('disabled', true);
                    tbSerieComponente.val(data.SerieComponente).prop('disabled', false);
                    tbDescripcion.val(data.Descripcion).prop('disabled', false);
                    tbFecha.val(response.Fecha).prop('disabled', false);
                    cboCausaRemosion.val(data.CausaRemosion).prop('disabled', false);
                    tbHorometroEquipo.val(data.HorometroEconomico).prop('disabled', false);
                    tbHorometroComponente.val(data.HorometroComponente).prop('disabled', false);

                    tbMontoPesos.val(data.MontoPesos).prop('disabled', false);
                    tbMontoDLL.val(data.MontoDLL).prop('disabled', false);
                    tbCantidadAbono.val(data.AbonoDLL).prop('disabled', false);
                    tbClaveCredito.val(data.ClaveCredito).prop('disabled', false);
                    fupAdjunto.prop('disabled', true);
                    btnEditarAccion.removeClass('hide');
                    btnGuardarAceptacion.addClass('hide');
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function LimpiarAutoriza() {
            tbGenerador.removeClass('bgColorValida');
            tbGenerador.removeAttr('title');

            tbOC.removeClass('bgColorValida');
            tbOC.removeAttr('title');
            tbSerieComponente.removeClass('bgColorValida');
            tbSerieComponente.removeAttr('title');
            tbDescripcion.removeClass('bgColorValida');
            tbDescripcion.removeAttr('title');
            cboCausaRemosion.removeClass('bgColorValida');
            cboCausaRemosion.removeAttr('title');

            tbHorometroEquipo.removeClass('bgColorValida');
            tbHorometroEquipo.removeAttr('title');
            tbHorometroComponente.removeClass('bgColorValida');
            tbHorometroComponente.removeAttr('title');
            tbMontoPesos.removeClass('bgColorValida');
            tbMontoPesos.removeAttr('title');

            tbMontoDLL.removeClass('bgColorValida');
            tbMontoDLL.removeAttr('title');
            tbClaveCredito.removeClass('bgColorValida');
            tbClaveCredito.removeAttr('title');
            cboTipoNC.removeClass('bgColorValida');
            cboTipoNC.removeAttr('title');
        }

        function CompareData(objD1, objD2, elemento) {

            if (objD1 != objD2) {
                $(elemento).addClass('bgColorValida');
                $(elemento).attr('title', 'Valor Original: ' + objD1 + ', Valor Modificado: ' + objD2);
            }

        }

        $(document).on('click', ".denegado", function () {
            let id = $(this).attr('data-id');
            tbComentarioRechazo.val('');
            btnConfirmacionDelete.attr('data-id', id);
            modalConfirmacionDelete.modal('show');
        });

        function SaveOrUpdateData(tipo) {
            let Comentario = "";
            let datos = GetDataSendNuevo();
            if (datos.id == 0) {
                var year = $("#tbFecha").datepicker('getDate').getFullYear();
                if (year < 2021) {
                    AlertaGeneral("Alera", "Solo se pueden capturar notas de credito del año actual");
                    return false;
                }
            }
            if (tipo) {
                datos.Estado = 3;
                datos.id = btnConfirmacionDelete.attr('data-id');
                Comentario = tbComentarioRechazo.val();

                var formData = new FormData();
                var files = document.getElementById("evidenciaFile").files[0];

                formData.append("obj", JSON.stringify(datos));
                formData.append("Tipo", tipo);
                formData.append("ComentarioRechazo", Comentario);
                formData.append("cc", selCCg.val());
                formData.append("archivo", files);
                formData.append("notaId", datos.id);
                formData.append("estado", datos.Estado);

                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/Overhaul/NuevaNotaCredito',
                    type: 'POST',
                    dataType: 'json',
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        modalAceptacion.modal('hide');
                        AlertaGeneral("Confirmacion", "La nota de credito ha sido actualizada correctamente");
                        loadTable();
                        $.unblockUI();
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
            else {
                let Comentario = "";
                let datos = GetDataSendNuevo();
                // if (tipo) {
                //     datos.Estado = 3;
                //     datos.id = btnConfirmacionDelete.attr('data-id');
                //     Comentario = tbComentarioRechazo.val();
                // }

                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/Overhaul/NuevaNotaCredito',
                    type: 'POST',
                    dataType: 'json',
                    data: { obj: datos, Tipo: tipo, ComentarioRechazo: Comentario, cc: selCCg.val() },
                    success: function (response) {
                        modalAceptacion.modal('hide');
                        AlertaGeneral("Confirmacion", "La nota de credito ha sido actualizada correctamente");
                        loadTable();
                        $.unblockUI();
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
        }

        function SaveEditDatos() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/EditNotaCredito',
                type: 'POST',
                dataType: 'json',
                data: { obj: GetDataEditNuevo() },
                success: function (response) {
                    btnGuardarAceptacion.removeClass('hide');
                    btnEditarAccion.addClass('hide');
                    modalAceptacion.modal('hide');
                    AlertaGeneral("Confirmacion", "Se envió una notificacion para su validacion de modificacion.");
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function setComentarios(data) {
            let htmlComentario = "";
            $.each(data, function (i, e) {
                htmlComentario += "<li class='comentario' data-id='" + e.id + "'>";
                htmlComentario += "    <div class='timeline-item'>";
                htmlComentario += "        <span class='time'><i class='glyphicon glyphicon-time'></i>" + e.fecha + "</span>";
                htmlComentario += "        <h3 class='timeline-header'><a href='#'>" + e.usuarioNombre + "</a></h3>";
                htmlComentario += "        <div class='timeline-body'>";
                if (e.factura != undefined) {
                    htmlComentario += "             " + "Factura: " + e.factura + "<br/>" + e.comentario;
                }
                else {
                    htmlComentario += "             " + e.comentario;
                }
                htmlComentario += "        </div>";
                htmlComentario += "    </div>";
                htmlComentario += "</li>";
            });
            ulComentarios.html(htmlComentario);
        }
        init();
    };

    $(document).ready(function () {
        maquinaria.Overhaul.NotasCredito = new NotasCredito();
    });
})();

