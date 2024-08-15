(function () {

    $.namespace('maquinaria.inventario.MaquinariaRentada');

    MaquinariaRentada = function () {

        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        txtId = $("#txtId");
        txtFolio = $("#txtFolio");
        cboCC = $("#cboCC");
        txtPeriodoDel = $("#txtPeriodoDel");
        txtPeriodoAl = $("#txtPeriodoAl");
        txtNoEconomico = $("#txtNoEconomico");
        txtEquipo = $("#txtEquipo");
        txtNoSerie = $("#txtNoSerie");
        txtModelo = $("#txtModelo");
        txtProveedor = $("#txtProveedor");
        txtObra = $("#txtObra");
        txtNoFactura = $("#txtNoFactura");
        txtDepGarantia = $("#txtDepGarantia");
        txtTramiteDG = $("#txtTramiteDG");
        txtNotaCredito = $("#txtNotaCredito");
        txtAplicoNC = $("#txtAplicoNC");
        txtNBaHoraMensual = $("#txtNBaHoraMensual");
        txtHorometroInicial = $("#txtHorometroInicial");
        txtHorometroFinal = $("#txtHorometroFinal");
        txtHorasTrabajadas = $("#txtHorasTrabajadas");
        txtHorasExtras = $("#txtHorasExtras");
        txtTotalHRS = $("#txtTotalHRS");
        txtPrecioMes = $("#txtPrecioMes");
        txtSeguroMes = $("#txtSeguroMes");
        txtIVA = $("#txtIVA");
        txtTotalRenta = $("#txtTotalRenta");
        txtOrdenCompra = $("#txtOrdenCompra");
        txtContraRecibo = $("#txtContraRecibo");
        txtAnotaciones = $("#txtAnotaciones");
        cbMoneda = $("#cbMoneda");
        btnBuscar = $("#btnBuscar");
        btnGuardar = $(".btnGuardar");
        btnNuevo = $("#btnNuevo");
        btnNuevo2 = $("#btnNuevo2");
        btnCancelar = $("#btnCancelar");
        btnFolio = $(".btnFolio");
        tblMaquinasRentadas = $("#tblMaquinasRentadas");
        filtroNoEconomico = $("#filtroNoEconomico");
        filtroPeriodoInicio = $("#filtroPeriodoInicio");
        filtroPeriodoFin = $("#filtroPeriodoFin");
        cbDifHoras = $('#cbDifHoras');
        txtDifHorasExtra = $("#txtDifHorasExtra");
        txtDifHoraContra = $("#txtDifHoraContra");
        txtDifHoraFactura = $("#txtDifHoraFactura");
        txtDifHoraOrdenCompra = $("#txtDifHoraOrdenCompra");
        dtDifHorafecha = $("#dtDifHorafecha");
        cbCargoDaño = $("#cbCargoDaño");
        txtDañoHorasExtra = $("#txtDañoHorasExtra");
        txtDañoFactura = $("#txtDañoFactura");
        txtDañoOrdenCompra = $("#txtDañoOrdenCompra");
        cbFletes = $("#cbFletes");
        txtFletesExtras = $("#txtFletesExtras");
        txtFletesFactura = $("#txtFletesFactura");
        txtFletesOrdenCompra = $("#txtFletesOrdenCompra");
        btnActualizar = $("#btnActualizar");
        btnNuevoRegistro = $("#btnNuevoRegistro");
        btnSiguiente = $("#btnSiguiente");
        btnModalNuevoRegistro = $("#btnModalNuevoRegistro");
        btnModalActualizar = $("#btnModalActualizar");
        btnModalSiguiente = $("#btnModalSiguiente");
        modalNuevo = $("#modalNuevo");
        modalActualizar = $("#modalActualizar");
        modalSiguiente = $("#modalSiguiente");
        btnCancelNuevo = $("#btnCancelNuevo");
        btnCancelActualizar = $("#btnCancelActualizar");
        btnCancelSiguiente = $("#btnCancelSiguiente");
        cboProveedor = $("#cboProveedor");
        btnProceso = $("#btnProceso");
        ireport = $("#report");
        modalProceso = $("#modalProceso");
        btnModalRptProceso = $("#btnModalRptProceso");
        txProcesoConclusion = $("#txProcesoConclusion");
        txTC = $("#txTC");
        modalActualizarRenta = $("#modalActualizarRenta");
        btnModalActualizarRenta = $("#btnModalActualizarRenta");
        txtEsEditar = $('#txtEsEditar');
        var myChart = null;
        var PesosBar = null;
        var DllsBar = null;
        function init() {
            InitModal();
            getDataPiker();
            getCombobox();
            btnBuscar.click(fnBuscar);
            btnNuevoRegistro.unbind("click").click(validaGuardado);
            btnModalNuevoRegistro.unbind("click").click(GuardarMaquina);
            btnSiguiente.unbind("click").click(validaGuardado);
            btnSiguiente.hide();
            btnModalSiguiente.unbind("click").click(GuardarMaquina);
            btnCancelar.click(fnCancelar);
            btnNuevo.click(fnNuevo);
            btnNuevo2.click(fnNuevo);
            txtNoEconomico.change(leeEconomico);
            cboCC.change(funCargarNoEconomico);
            txtPrecioMes.change(fnTotalRenta);
            txtSeguroMes.change(fnTotalRenta);
            initSwitch();
            btnActualizar.unbind("click").click(openActualizaroModal);
            btnModalActualizar.unbind("click").click(fnActualilzar);
            btnCancelNuevo.click(closeNuevoModal);
            btnCancelActualizar.click(closeActualizaroModal);
            btnCancelSiguiente.click(closeSiguienteModal);
            cboProveedor.change(fnGetProveedorMoneda);
            txtHorometroFinal.change(fnHorasTrabajadas);
            txtHorometroInicial.change(fnHorasTrabajadas);
            btnProceso.click(abrirModalProceso);
            btnProceso.hide();
            btnModalRptProceso.click(verReporte);
            btnModalActualizarRenta.click(closeModalActualizarRenta);
        }
        function InitModal() {
            modal1 = modalNuevo.dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",
            });
            modal2 = modalActualizar.dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",
            });
            modal3 = modalSiguiente.dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",
            });
            modalProceso = modalProceso.dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",
            });
            modal4 = modalActualizarRenta.dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",
            });
        }
        function closeNuevoModal() {
            modal1.dialog("close");
        }
        function closeActualizaroModal() {
            modal2.dialog("close");
        }
        function closeSiguienteModal() {
            modal3.dialog("close");
        }
        function openNuevoModal() {
            modal1.dialog("open");
        }
        function openActualizaroModal() {
            modal2.dialog("open");
        }
        function openSiguienteModal() {
            modal3.dialog("open");
        }
        function closeModalActualizarRenta() {
            modal4.dialog("close");
        }
        function initSwitch() {
            txtTramiteDG.bootstrapSwitch();
            txtAplicoNC.bootstrapSwitch();
            cbDifHoras.bootstrapSwitch();
            cbCargoDaño.bootstrapSwitch();
            cbFletes.bootstrapSwitch();
            // cbMoneda.bootstrapSwitch();
        }
        function funCargarNoEconomico() {
            filtroNoEconomico.fillCombo('/MaquinariaRentada/GetMaquinasRentadasPorCentroCosto', { idAc: cboCC.find('option:selected').data('prefijo') });
            if ($("#cboCC option:selected").text() == "--Seleccione--") {
                filtroNoEconomico.fillCombo('/MaquinariaRentada/GetMaquinasRentadasPorCentroCosto', { idAc: 0 });
            }
        }

        function fnBuscar() {
            bootG('/MaquinariaRentada/GetMaquinaRentadaNueva');
        }
        function fnLenarTabla() {
            $("#cboCC option").prop("selected", true);
            fnBuscar();
            $("#cboCC option").prop("selected", false);
        }
        function leeEconomico() {
            if ($("#txtNoEconomico option:selected").text() == "--Seleccione--") {
                txtEquipo.prop("disabled", true);
                txtNoSerie.prop("disabled", true);
                txtModelo.prop("disabled", true);
                txtProveedor.prop("disabled", true);
                txtObra.val("");
            } else {
                //CargarMaquina();
                getDetalleMaquinaExiste($(this).find('option:selected').data('comboid'));
                CargaAC_CC_Horometro(txtNoEconomico.find('option:selected').data('comboid'));
                txtEquipo.prop("disabled", true);
                txtNoSerie.prop("disabled", true);
                txtModelo.prop("disabled", true);
                txtProveedor.prop("disabled", true);
            }
        }
        function CargaAC_CC_Horometro(idCc) {
            $.get('/MaquinariaRentada/getAreaCuentaCentroCostoHorometroPorIdCentroCosto', { id: idCc })
                .always($.blockUI({ message: mensajes.PROCESANDO }))
                .then(response => {
                    $.unblockUI();
                    if (response.success){
                        txtObra.val(response.dataAreaCuenta.AreaCuenta.Nombre);
                        txtObra.data('obraid', response.dataAreaCuenta.AreaCuenta.Id);
                        txtHorometroInicial.val(response.dataHorometro.Horometro.HorometroInicial);

                        txtEquipo.val(response.dataCentroCosto.CentroCosto.Equipo);
                        txtNoSerie.val(response.dataCentroCosto.CentroCosto.NumeroSerie);
                        txtModelo.val(response.dataCentroCosto.CentroCosto.Modelo);
                        txtProveedor.val(response.dataCentroCosto.CentroCosto.Proveedor);
                    }else{
                        AlertaGeneral('Alerta', 'Error al consultar la información:\t' + response.message);

                        txtObra.val('');
                        txtObra.data('obraid', '');
                        txtHorometroInicial.val('');

                        txtEquipo.val('');
                        txtNoSerie.val('');
                        txtModelo.val('');
                        txtProveedor.val('');
                    }
                }, error => {
                    $.unblockUI();
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText);
                });
        }

        function CargarMaquina() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MaquinariaRentada/GetMaquinaEconomico",
                type: "POST",
                datatype: "json",
                data: { economico: txtNoEconomico.val() },
                success: function (response) {
                    $.unblockUI();
                    txtEquipo.val(response.equipo);
                    txtNoSerie.val(response.serie);
                    txtModelo.val(response.modelo);
                    txtProveedor.val(response.proveedor);
                    txtProveedor.data('proveedorId', response.proveedorId);
                    console.log(txtProveedor.data('proveedorId'));
                    // cbMoneda.bootstrapSwitch('state', response.moneda);
                    cboProveedor.val(response.idProveedor);
                    txtHorometroInicial.val(response.HorometroInicial);
                    txtHorometroFinal.val(response.HorometroFinal);
                    txtHorasTrabajadas.val(response.HorometroTotal);
                    if (response.idProveedor == undefined) {
                        cboProveedor.val(0);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function bootG(url) {
            $.blockUI({ message: mensajes.PROCESANDO });
            console.log(filtroPeriodoFin.val());
            $.ajax({
                url: url,
                type: "GET",
                datatype: "json",
                data: { idAreaCuenta: cboCC.find('option:selected').data('prefijo'), idCentroCosto: filtroNoEconomico.find('option:selected').data('prefijo'), periodoIni: filtroPeriodoInicio.val(), periodoFin: filtroPeriodoFin.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.MaquinasRentadas;
                    var htmlRegistro = CrearRegistroEditable(data);
                    data = htmlRegistro;
                    tblMaquinasRentadas.bootgrid("clear");
                    for (let index = 0; index < data.length; index++) {
                        //$.datepicker.formatDate('dd/mm/yy', new Date(parseInt(variable)))
                        // var jsDate = moment(netDateTime).toDate();
                        data[index].PeriodoDel = moment(data[index].PeriodoDel).format('DD/MM/YYYY');
                        data[index].PeriodoA = moment(data[index].PeriodoA).format('DD/MM/YYYY');
                    }
                    tblMaquinasRentadas.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function cargarGraProceso(url) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                data: { ccs: getValoresMultiples("#cboCC"), NoEconomico: filtroNoEconomico.val(), PeriodoInicio: filtroPeriodoInicio.val(), PeriodoFin: filtroPeriodoFin.val() },
                success: function (response) {
                    $.unblockUI();
                    setGraficaProesoPesos(response.lstPesos);
                    setGraficaProesoDlls(response.lstDlls);
                    txProcesoConclusion.val(response.reporte.Conclusion);
                    txTC.val(response.reporte.Tc);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function fnGetProveedorMoneda() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MaquinariaRentada/getProveedorMoneda',
                type: "POST",
                datatype: "json",
                data: { idProveedor: cboProveedor.val() },
                success: function (response) {
                    $.unblockUI();
                    // cbMoneda.bootstrapSwitch('state', response);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function validaGuardado() {
            var state = true;
            var mensaje = 'Los campos en rojo son obligatorios';
            if (!validarCampo(txtPeriodoDel)) { state = false; }
            if (!validarCampo(txtPeriodoAl)) { state = false; }
            if (!validarCampo(cbMoneda)) { state = false; }
            if (!validarCampo(txtNoEconomico)) { state = false; mensaje = 'Es necesario capturar el equipo y obra' }
            if (txtObra.val() == '') { state = false; mensaje = 'Es necesario capturar el equipo y obra' }
            if (state) {
                openNuevoModal();
            }else{
                AlertaGeneral('Alerta', mensaje);
            }
        }

        function GuardarMaquina() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MaquinariaRentada/GuardadoMaquinaRentada2',
                type: "POST",
                datatype: "json",
                data: getObjMaquinaRentada_Nueva(),
                success: function (response) {
                    if (response.success){
                        $.unblockUI();
                        ConfirmacionGeneral("Confirmación", "¡Registro guardado correctamente!");
                        fnNuevo();
                        setInterval(function () {
                            window.localStorage.href = "/MaquinariaRentada/Captura";
                        }, 2000);
                    }else{
                        AlertaGeneral('Alerta', 'Error al regsitrar renta:\t' + response.message);
                    }
                },
                error: function (error) {
                    $.unblockUI('Alerta', 'Error al realizar la consulta al servidor:\t' + error.statusText);
                }
            });
            modal1.dialog("close");
            modal3.dialog("close");
        }


        function fnActualilzar() {
            var state = true;
            if (!validarCampo(txtPeriodoDel)) { state = false; }
            if (!validarCampo(txtPeriodoAl)) { state = false; }
            if (state) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/MaquinariaRentada/ActualizarMaquinaRentada',
                    type: "POST",
                    datatype: "json",
                    data: getObjMaquinaRentada_Nueva(),
                    success: function (response) {

                        $.unblockUI();
                        ConfirmacionGeneral("Confirmación", "¡Registro guardado correctamente!");
                        setInterval(function () {
                            window.localStorage.href = "/MaquinariaRentada/Captura";
                        }, 2000);
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            modal2.dialog("close");
        }
        cbDifHoras.on('switchChange.bootstrapSwitch', function () {
            fnCambioDifHoras();
        });
        cbCargoDaño.on('switchChange.bootstrapSwitch', function () {
            fnCambioCargoDaño();
        });
        cbFletes.on('switchChange.bootstrapSwitch', function () {
            fnCambioFletes();
        });
        function fnNuevo() {
            btnNuevoRegistro.show();
            btnActualizar.hide();
            clearFormulario();
            HabilitarFormulario();
            $("#step-2").removeAttr("hidden");
            $("#step-1").attr('hidden', 'hidden');
            txtNoEconomico.prop('disabled', false);
            txtEsEditar.val('NO');
        }
        function fnNoPermitir(){
            btnNuevoRegistro.hide();
            btnActualizar.hide();
        }
        function fnSiPermitir(){
            btnNuevoRegistro.show();
        }

        function fnPermitirEdicion()
        {
            btnActualizar.show();
            btnNuevoRegistro.hide();
        }

        function fnCancelar() {
            $("#step-1").removeAttr("hidden");
            $("#step-2").attr('hidden', 'hidden');
            txtEsEditar.val('NO');
            HabilitarFormulario();
        }

        function abrirModalProceso() {
            modalProceso.dialog("open");
            cargarGraProceso('/MaquinariaRentada/cargarGraProceso');
        }

        function setGraficaProesoPesos(data) {
            if (PesosBar != null) {
                PesosBar.destroy();
            }
            var pesosPrecio = [];
            var pesosEconomico = [];
            for (var i = 0; i < data.length; i++) {
                if (data[i].NoEconomico != null) {
                    pesosPrecio.push(data[i].PrecioMes);
                    pesosEconomico.push(data[i].NoEconomico);
                }
            }
            PesosBar = new Chart(document.getElementById("GraficaProcesoPesos"), {
                type: 'bar',
                data: {
                    labels: pesosEconomico,
                    datasets: [
                        {
                            backgroundColor: "#4972CB",
                            data: pesosPrecio
                        }
                    ]
                },
                options: {
                    legend: { display: false },
                    title: {
                        display: true,
                        text: 'Rentas en pesos'
                    }
                }
            });

            PesosBar.resize();
        }

        function setGraficaProesoDlls(data) {
            if (DllsBar != null) {
                DllsBar.destroy();
            }
            var dllsPrecio = [];
            var dllsEconomico = [];
            for (var i = 0; i < data.length; i++) {
                if (data[i].NoEconomico != null) {
                    dllsPrecio.push(data[i].PrecioMes);
                    dllsEconomico.push(data[i].NoEconomico);
                }
            }
            DllsBar = new Chart(document.getElementById("GraficaProcesoDlls"), {
                type: 'bar',
                data: {
                    labels: dllsEconomico,
                    datasets: [
                        {
                            backgroundColor: "#4972CB",
                            data: dllsPrecio
                        }
                    ]
                },
                options: {
                    legend: { display: false },
                    title: {
                        display: true,
                        text: 'Rentas en dólares'
                    }
                }
            });
            DllsBar.resize();
        }

        function verReporte() {
            var idReporte = $(this).val();
            if (validarModal(idReporte)) {
                $.blockUI({ message: mensajes.PROCESANDO });
                var CC = cboCC.val();
                var fechaDel = filtroPeriodoInicio.val();
                var fechaAl = filtroPeriodoFin.val();
                var ProcespConclucion = txProcesoConclusion.val();
                var TC = txTC.val();
                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&CC=" + CC + "&fechaDel=" + fechaDel + "&fechaAl=" + fechaAl + "&ProcespConclucion=" + ProcespConclucion + "&TC=" + TC;
                ireport.attr("src", path);
                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
                modalProceso.dialog("close");
            }
        }

        function validarModal(idReporte) {
            var state = true;
            switch (idReporte) {
                case "40":
                    if (!validarCampo(txTC)) { state = false; }
                    if (!validarCampo(txProcesoConclusion)) { state = false; }
                    break;
                default:
                    state = false;
            }
            return state;
        }
        function getDataPiker() {
            txtPeriodoDel.datepicker().datepicker("setDate", new Date());
            txtPeriodoAl.datepicker().datepicker("setDate", new Date());
            filtroPeriodoInicio.datepicker().datepicker("setDate", new Date());
            filtroPeriodoFin.datepicker().datepicker("setDate", new Date());
            dtDifHorafecha.datepicker().datepicker("setDate", new Date());
        }

        function getCombobox() {
            cboCC.fillCombo('/MaquinariaRentada/GetAreasCuenta', null, false, null);
            txtNoEconomico.fillCombo('/MaquinariaRentada/GetAllMaquinas', null, false, null);
            cboProveedor.fillCombo('/MaquinariaRentada/FillCboProveedor', null, false, null);
            //txtObra.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false, null);
        }

        function fnHorasTrabajadas() {
            if (txtHorometroInicial.val() == "") { txtHorometroFinal.val("0"); }
            if (txtHorometroFinal.val() == "") { txtHorometroFinal.val("0"); }
            var Inicial = parseFloat(txtHorometroInicial.val());
            var Final = parseFloat(txtHorometroFinal.val());
            txtHorasTrabajadas.val(Inicial - Final);
        }

        function CrearRegistroEditable(data) {
            for (var i in data) {
                var d = document.createElement('button');
                var mas = document.createElement('button');
                mas.innerHTML = '<button type="button" class="btn btn-success btnMas" value="' + data[i].Id + '" data-horometroFinal="' + data[i].HorometroFinal + '" onclick="abrirModalNuevaRenta(this.value, ' + data[i].HorometroFinal + ' )">+</button>';
                if(data[i].HorometroFinal == null){
                    d.innerHTML = d.innerHTML + '<button type="button" class="btn btn-success btnFolio" value="' + data[i].Id + '"  onclick="getDetalleMaquina(this.value)"> Editar</button>  ' + data[i].Folio;
                }
                
                if (data[i].DifHora) { d.innerHTML = d.innerHTML + ' <span class="label  label-warning">Diferencia de horas</span>' }
                if (data[i].CargoDaño) { d.innerHTML = d.innerHTML + ' <span class="label  label-danger">Existe cargo por daño</span>' }
                if (data[i].Fletes) { d.innerHTML = d.innerHTML + ' <span class="label  label-primary">Existe fletes</span>' }
                data[i].Folio = d.innerHTML;
                data[i].Id = mas.innerHTML;
                data[i].TotalRenta = maskNumero(data[i].TotalRenta);
                // if (data[i].HorometroFinal == '' || data[i].HorometroFinal == null) {

                // } else {
                //     data[i].Id = mas.innerHTML;
                // }

                var m = document.createElement('label');
                m.innerHTML = m.innerHTML + '<label>' + data[i].TotalRenta + '</label>';
                if (data[i].IdTipoMoneda == 1) { m.innerHTML = m.innerHTML + '<span class="badge">Mx</span>' } else { m.innerHTML = m.innerHTML + '<span class="badge">US</span>' }
                data[i].TotalRenta = m.innerHTML;
            }
            return data;
        }

        function desHabilitarFormulario() {
            btnGuardar.prop('disabled', true);
            txtObra.prop('disabled', true);
            txtNoFactura.prop('disabled', true);
            txtDepGarantia.prop('disabled', true);
            txtTramiteDG.prop('disabled', true);
            txtNotaCredito.prop('disabled', true);
            txtAplicoNC.prop('disabled', true);
            txtNBaHoraMensual.prop('disabled', true);
            txtPeriodoDel.prop('disabled', true);
            txtPeriodoAl.prop('disabled', true);
            txtHorometroInicial.prop('disabled', true);
            txtHorometroFinal.prop('disabled', true);
            txtHorasTrabajadas.prop('disabled', true);
            txtHorasExtras.prop('disabled', true);
            txtTotalHRS.prop('disabled', true);
            txtPrecioMes.prop('disabled', true);
            txtSeguroMes.prop('disabled', true);
            txtOrdenCompra.prop('disabled', true);
            txtContraRecibo.prop('disabled', true);
            txtAnotaciones.prop('disabled', true);
            cbDifHoras.prop('disabled', true);
            cbCargoDaño.prop('disabled', true);
            cbFletes.prop('disabled', true);
            cbMoneda.prop('disabled', true);
        }

        function getObjMaquinaRentada_Nueva() {
            var objMaquina = {
                Id: txtId.val(),
                Folio: txtFolio.val(),
                IdCentroCosto: txtNoEconomico.find('option:selected').data('comboid'),
                IdProveedor: txtProveedor.val(), ///// Se busca por NombreProveedor en BD
                NombreProveedor: txtProveedor.val(),
                IdAreaCuenta: txtObra.data('obraid'),
                NumFactura: txtNoFactura.val(),
                DepGarantia: txtDepGarantia.val(),
                TramiteDG: txtTramiteDG.bootstrapSwitch('state'),
                NotaCredito: txtNotaCredito.val(),
                AplicaNC: txtAplicoNC.bootstrapSwitch('state'),
                BaseHoraMensual: txtNBaHoraMensual.val(),
                PeriodoDel: txtPeriodoDel.val(),
                PeriodoA: txtPeriodoAl.val(),
                HorometroInicial: txtHorometroInicial.val(),
                HorometroFinal: txtHorometroFinal.val(),
                HorasTrabajadas: txtHorasTrabajadas.val(),
                HorasExtras: txtHorasExtras.val(),
                TotalHorasExtras: txtTotalHRS.val(),
                PrecioPorMes: txtPrecioMes.val(),
                SeguroPorMes: txtSeguroMes.val(),
                IVA: txtIVA.val(),
                TotalRenta: txtTotalRenta.val(),
                OrdenCompra: txtOrdenCompra.val(),
                ContraRecibo: txtContraRecibo.val(),
                Anotaciones: txtAnotaciones.val(),
                IdTipoMoneda: cbMoneda.find('option:selected').val(),
                DifHora: cbDifHoras.bootstrapSwitch('state'),
                DifHoraExtra: txtDifHorasExtra.val(),
                DifContraRecibo: txtDifHoraContra.val(),
                DifFactura: txtDifHoraFactura.val(),
                DifOrdenCompra: txtDifHoraOrdenCompra.val(),
                DifFechaContraRecibo: dtDifHorafecha.val(),
                CargoDaño: cbCargoDaño.bootstrapSwitch('state'),
                CargoDañoFactura: txtDañoFactura.val(),
                CargoDañoOrdenCompra: txtDañoOrdenCompra.val(),
                Fletes: cbFletes.bootstrapSwitch('state'),
                FletesFactura: txtFletesFactura.val(),
                FletesOrdenCompra: txtFletesOrdenCompra.val(),
                Activo: true,
                FechaCreacion: new Date(),
                FechaModificacion: new Date(),
                IdUsuario: 1,
            };
            return objMaquina;
        }
        
        function getObjMaquinaRentada() {
            var obraText = txtObra.val();
            var obraVal = txtObra.find('option:selected').text();
            var cont = 0;
            for (var i = 0; i < obraVal.length; i++) {
                if (obraVal.charAt(i) == '-') {
                    cont++;
                }
                if (cont == 2) {
                    cont = i;
                }
            }
            obraVal = obraVal.substring(cont);
            console.log(obraVal);

            var objMaquina = {
                id: txtId.val(),
                Folio: txtFolio.val(),
                NoEconomico: txtNoEconomico.val(),
                Equipo: txtEquipo.val(),
                NoSerie: txtNoSerie.val(),
                Modelo: txtModelo.val(),
                idProveedor: cboProveedor.val(),
                Proveedor: txtProveedor.val(),
                Obra: obraVal,
                CC: txtObra.val(),
                NoFactura: txtNoFactura.val(),
                DepGarantia: txtDepGarantia.val(),
                TramiteDG: txtTramiteDG.bootstrapSwitch('state'),
                NotaCredito: txtNotaCredito.val(),
                AplicaNC: txtAplicoNC.bootstrapSwitch('state'),
                BaseHoraMensual: txtNBaHoraMensual.val(),
                PeriodoDel: txtPeriodoDel.val(),
                PeriodoA: txtPeriodoAl.val(),
                HorometroInicial: txtHorometroInicial.val(),
                HorometroFinal: txtHorometroFinal.val(),
                HorasTrabajadas: txtHorasTrabajadas.val(),
                HorasExtras: txtHorasExtras.val(),
                TotalHorasExtras: txtTotalHRS.val(),
                PrecioMes: txtPrecioMes.val(),
                SeguroMes: txtSeguroMes.val(),
                IVA: txtIVA.val(),
                TotalRenta: txtTotalRenta.val(),
                OrdenCompra: txtOrdenCompra.val(),
                ContraRecibo: txtContraRecibo.val(),
                Anotaciones: txtAnotaciones.val(),
                Moneda: cbMoneda.find('option:selected').val(),
                DifHora: cbDifHoras.bootstrapSwitch('state'),
                DifHoraHoraExtra: '',
                DifHoraContraRecibo: '',
                DifHoraFactura: '',
                DifHoraOrdenCompra: '',
                DifHoraFecha: new Date(),
                CargoDañoHoraExtra: '',
                CargoDaño: cbCargoDaño.bootstrapSwitch('state'),
                CargoDañoContraRecibo: '',
                CargoDañoFactura: '',
                CargoDañoOrdenCompra: '',
                CargoDañoFecha: new Date(),
                Fletes: cbFletes.bootstrapSwitch('state'),
                FletesHoraExtra: '',
                FletesContraRecibo: '',
                FletesNoFactura: '',
                FletesOrdenCompra: '',
                FletesFecha: new Date()
            };
            if (cbDifHoras.bootstrapSwitch('state')) {
                objMaquina.DifHoraHoraExtra = txtDifHoraContra.val();
                objMaquina.DifHoraContraRecibo = txtDifHorasExtra.val();
                objMaquina.DifHoraFactura = txtDifHoraFactura.val();
                objMaquina.DifHoraOrdenCompra = txtDifHoraOrdenCompra.val();
                objMaquina.DifHoraFecha = dtDifHorafecha.val();
            }
            if (cbCargoDaño.bootstrapSwitch('state')) {
                objMaquina.CargoDañoHoraExtra = txtDañoHorasExtra.val();
                objMaquina.CargoDañoFactura = txtDañoFactura.val();
                objMaquina.CargoDañoOrdenCompra = txtDañoOrdenCompra.val();
            }
            if (cbFletes.bootstrapSwitch('state')) {
                objMaquina.FletesHoraExtra = txtDañoHorasExtra.val();
                objMaquina.FletesNoFactura = txtFletesFactura.val();
                objMaquina.FletesOrdenCompra = txtFletesOrdenCompra.val();
            }
            return objMaquina;
        }


        function clearFormulario() {
            txtId.val("");
            txtFolio.val("");
            txtNoEconomico.val("");
            txtEquipo.val("");
            txtNoSerie.val("");
            txtModelo.val("");
            txtProveedor.val("");
            txtObra.val("");
            txtNoFactura.val("");
            txtDepGarantia.val("");
            txtTramiteDG.prop("checked", false);
            txtNotaCredito.val("");
            txtAplicoNC.prop("checked", false);
            txtNBaHoraMensual.val("");
            txtPeriodoDel.val("");
            txtPeriodoAl.val("");
            txtHorometroInicial.val("");
            txtHorometroFinal.val("");
            txtHorasTrabajadas.val("");
            txtHorasExtras.val("");
            txtTotalHRS.val("");
            txtPrecioMes.val("");
            txtSeguroMes.val("");
            txtIVA.val('');
            txtTotalRenta.val("");
            txtOrdenCompra.val("");
            txtContraRecibo.val("");
            txtAnotaciones.val("");
            // cbMoneda.bootstrapSwitch('state', false);
            cbDifHoras.bootstrapSwitch('state', false);
            txtDifHoraContra.val('');
            txtDifHorasExtra.val('');
            txtDifHoraFactura.val('');
            txtDifHoraOrdenCompra.val('');
            dtDifHorafecha.val('');
            cbCargoDaño.bootstrapSwitch('state', false);
            txtDañoHorasExtra.val('');
            txtDañoFactura.val('');
            txtDañoOrdenCompra.val('');
            cbFletes.bootstrapSwitch('state', false);
            txtFletesExtras.val('');
            txtFletesFactura.val('');
            txtFletesOrdenCompra.val('');
            cboProveedor.val('');
        }

        function getDetalleMaquinaExiste(idCentroCosto){
            console.log(idCentroCosto);
            btnActualizar.hide();
            btnNuevoRegistro.show();
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MaquinariaRentada/getExisteMaquinaRentadaPorIdCentroCosto",
                type: "GET",
                datatype: "json",
                data: { idCentroCosto: idCentroCosto },
                success: function (response) {
                    if(response.success){
                        if(!response.Existe){
                            if(txtEsEditar.val() == 'SI')
                            {
                                fnPermitirEdicion();
                                setObjMaquinaRentada(response.MaquinaRentada, true);
                            }else{
                                fnNuevo();
                                fnNoPermitir();
                                AlertaGeneral('Alerta', 'El centro de costo cuenta con una renta abierta, termine la renta o seleccione otro centro de costo');
                            }
                        }else{
                            fnSiPermitir();
                            setObjMaquinaRentada(response.MaquinaRentada, true);
                        }
                        
                        // $.unblockUI();
                        // $("#step-2").removeAttr("hidden");
                        // $("#step-1").attr('hidden', 'hidden');
                        // txtNoEconomico.prop('disabled', false);
                        // console.log(response.MaquinaRentada);
                        // setObjMaquinaRentada(response.MaquinaRentada);
                        // if(response.RentaTerminada){
                        //     ConfirmacionGeneral("Confirmación", "¡Se terminó la renta!, Ahora puede agregar otro periodo de renta");
                        // }
                        // console.log('getDetalleMaquinaEditarTerminar ' + id);
                        // $(document).find('.btnFolio[value="'+id+'"]').hide();
                        // $(document).find('.btnMas[value="'+id+'"]').attr('onclick', 'abrirModalNuevaRenta(this.value, 1)');
            
                        // fnTotalRenta();
                        // fnCambioDifHoras();
                        // fnCambioCargoDaño();
                        // fnCambioFletes();
                    }else{
                        if(!response.Existe){
                            AlertaGeneral('Alerta', 'Error al consultar la información:\t' + response.message);
                        }else{
                        }
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.inventario.MaquinariaRentada = new MaquinariaRentada();

        $(document).on('click', '#btnModalTerminarRenta', function(){
            console.log('terminarRenta id: ' + $(this).val());
            $.post('/MaquinariaRentada/TerminarRenta', { idMaquinaRentada: $(this).val() })
            .always($.blockUI({ message: mensajes.PROCESANDO }))
            .then(response => {
                $.unblockUI();
            if (response.success){
                $('#modalActualizarRenta').dialog('close');
                    ConfirmacionGeneral("Confirmación", "¡Se terminó la renta!");
                    $(document).find('.btnMas[value="'+$(this).val()+'"]').attr('onclick', 'abrirModalNuevaRenta(this.value, 1)');
                    console.log('actualizarOpcion = ' + $(this).attr('onclick'));
                    $(document).find('.btnFolio[value="'+$(this).val()+'"]').hide();
                }else{
                    AlertaGeneral('Alerta', response.message)
                }
            }, error => {
                $.unblockUI();
                AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText);
            });
        });
    });
    })();

    

function abrirModalNuevaRenta(id, terminada) {
    $('#btnModalActualizarRenta').val(id);
    $('#btnModalTerminarRenta').val(id);
    $('#modalActualizarRenta').dialog('open');
    $('#btnModalTerminarRenta').show();
    console.log(terminada);
    console.log('abrirModalNuevaRenta');
    if (terminada != null){
        console.log('abrirModalNuevaRenta IF true');
        $('#btnModalTerminarRenta').hide();
        $('#btnModalActualizarRenta').show();
    }else{
        console.log('abrirModalNuevaRenta IF false');
        $('#btnModalActualizarRenta').hide();
        btnModalActualizarRenta.show();
    }

}

function getDetalleMaquinaEditar(id){
    btnActualizar.hide();
    btnNuevoRegistro.show();
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: "/MaquinariaRentada/GetMaquinaDetalleEditarTerminar",
        type: "GET",
        datatype: "json",
        data: { id: id },
        success: function (response) {
            if(response.success){
                $.unblockUI();
                $("#step-2").removeAttr("hidden");
                $("#step-1").attr('hidden', 'hidden');
                txtNoEconomico.prop('disabled', false);
                console.log(response.MaquinaRentada);
                setObjMaquinaRentada(response.MaquinaRentada);
                if(response.RentaTerminada){
                    ConfirmacionGeneral("Confirmación", "¡Se terminó la renta!, Ahora puede agregar otro periodo de renta");
                }
                console.log('getDetalleMaquinaEditarTerminar ' + id);
                $(document).find('.btnFolio[value="'+id+'"]').hide();
                $(document).find('.btnMas[value="'+id+'"]').attr('onclick', 'abrirModalNuevaRenta(this.value, 1)');
    
                fnTotalRenta();
                fnCambioDifHoras();
                fnCambioCargoDaño();
                fnCambioFletes();
            }else{
                AlertaGeneral('Alerta', 'Error al consultar la información:\t' + response.message);
            }
        },
        error: function () {
            $.unblockUI();
        }
    });
}

function getDetalleMaquina(id) {
    btnActualizar.show();
    btnNuevoRegistro.hide();
    $.blockUI({ message: mensajes.PROCESANDO });
    $.ajax({
        url: "/MaquinariaRentada/GetMaquinaDetalleEditar",
        type: "GET",
        datatype: "json",
        data: { id: id },
        success: function (response) {
            $.unblockUI();
            $("#step-2").removeAttr("hidden");
            $("#step-1").attr('hidden', 'hidden');
            txtNoEconomico.prop('disabled', false);
            txtEsEditar.val('SI');
            setObjMaquinaRentada(response.MaquinaRentada);

            fnTotalRenta();
            fnCambioDifHoras();
            fnCambioCargoDaño();
            fnCambioFletes();
        },
        error: function () {
            $.unblockUI();
        }
    });
}
function fnTotalRenta() {
    if (txtPrecioMes.val() == "") { txtPrecioMes.val(0); }
    if (txtSeguroMes.val() == "") { txtSeguroMes.val(0); }
    var ivaMes = parseFloat(txtPrecioMes.val() * .16).toFixed(2);
    var ivaSeguro = parseFloat(txtSeguroMes.val() * .16).toFixed(2);
    txtIVA.val(parseFloat(ivaMes) + parseFloat(ivaSeguro));
    var total = parseFloat(txtIVA.val()) + parseFloat(txtPrecioMes.val()) + parseFloat(txtSeguroMes.val());
    txtTotalRenta.val(parseFloat(total).toFixed(2));
}

function fnCambioDifHoras() {
    if (cbDifHoras.bootstrapSwitch('state')) {
        txtDifHoraContra.prop('disabled', false);
        txtDifHorasExtra.prop('disabled', false);
        txtDifHoraFactura.prop('disabled', false);
        txtDifHoraOrdenCompra.prop('disabled', false);
        dtDifHorafecha.prop('disabled', false);
    }
    else {
        txtDifHoraContra.prop('disabled', true);
        txtDifHorasExtra.prop('disabled', true);
        txtDifHoraFactura.prop('disabled', true);
        txtDifHoraOrdenCompra.prop('disabled', true);
        dtDifHorafecha.prop('disabled', true);
    }
}
function fnCambioCargoDaño() {
    if (cbCargoDaño.bootstrapSwitch('state')) {
        txtDañoHorasExtra.prop('disabled', false);
        txtDañoFactura.prop('disabled', false);
        txtDañoOrdenCompra.prop('disabled', false);
    }
    else {
        txtDañoHorasExtra.prop('disabled', true);
        txtDañoFactura.prop('disabled', true);
        txtDañoOrdenCompra.prop('disabled', true);
    }
}
function fnCambioFletes() {
    if (cbFletes.bootstrapSwitch('state')) {
        txtFletesExtras.prop('disabled', false);
        txtFletesFactura.prop('disabled', false);
        txtFletesOrdenCompra.prop('disabled', false);
    }
    else {
        txtFletesExtras.prop('disabled', true);
        txtFletesFactura.prop('disabled', true);
        txtFletesOrdenCompra.prop('disabled', true);
    }
}
function setObjMaquinaRentada(obj, NuevaCaptura) {
    txtId.val(obj.Id);
    txtFolio.val(obj.Folio);
    txtNoEconomico.find('option:selected').prop('selected', false);
    txtNoEconomico.find('option[data-comboid="'+obj.IdCentroCosto+'"]').attr('selected', true);
    txtNoEconomico.val(obj.CentroCosto);
    console.log('idproveedor ' + obj.IdProveedor);
    txtProveedor.val(obj.IdProveedor);
    console.log('valor input idproveedor '+txtProveedor.val());
    if(!NuevaCaptura){
        txtNoEconomico.change();
    }else{

    }
    if (txtEsEditar.val() == 'SI')
    {
        txtNoEconomico.prop('disabled', true);
    }
    txtNoFactura.val(obj.NumFactura);
    txtDepGarantia.val(obj.DepGarantia);
    txtTramiteDG.bootstrapSwitch('state', obj.TramiteDG);
    // txtTramiteDG.prop("checked", obj.TramiteDG);
    txtNotaCredito.val(obj.NotaCredito);
    txtAplicoNC.bootstrapSwitch('state', obj.AplicaNC);
    // txtAplicoNC.prop("checked", obj.AplicaNC);
    txtNBaHoraMensual.val(obj.BaseHoraMensual);
    txtPeriodoDel.datepicker("setDate", $.datepicker.formatDate('dd/mm/yy', new Date(obj.PeriodoDel)));
    txtPeriodoAl.datepicker("setDate", $.datepicker.formatDate('dd/mm/yy', new Date(obj.PeriodoA)));
    txtHorometroInicial.val(obj.HorometroInicial);
    // txtHorometroFinal.val(obj.HorometroFinal);
    txtHorasTrabajadas.val(obj.HorasTrabajadas);
    txtHorasExtras.val(obj.HorasExtras);
    txtTotalHRS.val(obj.TotalHorasExtras);
    txtPrecioMes.val(obj.PrecioPorMes);
    txtSeguroMes.val(obj.SeguroPorMes);
    txtIVA.val(obj.IVA);
    txtTotalRenta.val(obj.TotalRenta);
    txtOrdenCompra.val(obj.OrdenCompra);
    txtContraRecibo.val(obj.ContraRecibo);
    txtAnotaciones.val(obj.Anotaciones);
    cbMoneda.find('option:selected').prop('selected', false);
    cbMoneda.find('option[value="'+obj.IdTipoMoneda+'"]').attr('selected', true);
    cbMoneda.val(obj.IdTipoMoneda);
    cbDifHoras.bootstrapSwitch('state', obj.DifHora);
    txtDifHorasExtra.val(obj.DifHoraExtra);
    txtDifHoraContra.val(obj.DifContraRecibo);
    txtDifHoraFactura.val(obj.DifFactura);
    txtDifHoraOrdenCompra.val(obj.DifOrdenCompra);
    if(obj.DifFechaContraRecibo != null)
    {
        console.log('no es nul');dtDifHorafecha.datepicker('setDate', $.datepicker.formatDate('dd/mm/yy', new Date(obj.DifFechaContraRecibo)));
    }else{
        console.log('es nul');dtDifHorafecha.val('');
    }
    cbCargoDaño.bootstrapSwitch('state', obj.CargoDaño);
    txtDañoFactura.val(obj.CargoDañoFactura);
    txtDañoOrdenCompra.val(obj.CargoDañoOrdenCompra);
    cbFletes.bootstrapSwitch('state', obj.Fletes);
    txtFletesFactura.val(obj.FletesFactura);
    txtFletesOrdenCompra.val(obj.FletesOrdenCompra);
}
function HabilitarFormulario() {
    txtEquipo.prop("disabled", true);
    txtNoSerie.prop("disabled", true);
    txtModelo.prop("disabled", true);
    txtProveedor.prop("disabled", true);

    btnGuardar.prop('disabled', false);
    txtNoFactura.prop('disabled', false);
    txtDepGarantia.prop('disabled', false);
    txtTramiteDG.prop('disabled', false);
    txtNotaCredito.prop('disabled', false);
    txtAplicoNC.prop('disabled', false);
    txtNBaHoraMensual.prop('disabled', false);
    txtPeriodoDel.prop('disabled', false);
    txtPeriodoAl.prop('disabled', false);
    txtHorometroInicial.prop('disabled', false);
    txtHorometroFinal.prop('disabled', false);
    txtHorasTrabajadas.prop('disabled', false);
    txtHorasExtras.prop('disabled', false);
    txtTotalHRS.prop('disabled', false);
    txtPrecioMes.prop('disabled', false);
    txtSeguroMes.prop('disabled', false);
    txtOrdenCompra.prop('disabled', false);
    txtContraRecibo.prop('disabled', false);
    txtAnotaciones.prop('disabled', false);
    cbDifHoras.prop('disabled', false);
    cbCargoDaño.prop('disabled', false);
    cbFletes.prop('disabled', false);
    cbMoneda.prop('disabled', false);
}
