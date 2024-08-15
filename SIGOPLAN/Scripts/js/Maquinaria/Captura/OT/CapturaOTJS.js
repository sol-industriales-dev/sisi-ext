(function () {

    $.namespace('maquinaria.OT.CapturaOT');

    CapturaOT = function () {

        idGlobalOT = 0;
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        var PermisoAuditor = false;
        var aplicaValidacion = false;
        var validacion = false;
        var horometroOT = 0;
        btnAgregarHH = $("#btnAgregarHH"),
            tbHoraEntradaHH = $("#tbHoraEntradaHH"),
            tbHoraFinalizacionHH = $("#tbHoraFinalizacionHH"),
            cboEconomicofiltro = $("#cboEconomicofiltro"),
            cboMotivoParoFiltro = $("#cboMotivoParoFiltro"),
            /*Tiempo de paro*/
            tbFechaOT = $("#tbFechaOT"),
            tbTiempoParoH = $("#tbTiempoParoH"),
            tbTiempoParoM = $("#tbTiempoParoM"),
            tbTiempoReparacionH = $("#tbTiempoReparacionH"),
            tbTiempoReparacionM = $("#tbTiempoReparacionM"),
            tbTiempoMuertoH = $("#tbTiempoMuertoH"),
            tbTiempoMuertoM = $("#tbTiempoMuertoM"),
            /**/
            FInicial = $("#FInicial"),
            FFinal = $("#FFinal"),
            ireport = $("#report"),
            tblEmpleadosTrabajoGrid = $('#tblEmpleadosTrabajo').DataTable({});
        BntRegresar = $("#BntRegresar"),
            btnNuevo = $("#btnNuevo"),
            btnExportar = $("#btnExportar"),
            Principal = $("#Principal"),
            Detalle = $("#Detalle"),
            tblListaOT = $("#tblListaOT"),
            cboTipoParo = $("#cboTipoParo"),
            cboTipoParo2 = $("#cboTipoParo2"),
            cboTipoParo3 = $("#cboTipoParo3"),
            tbOtroMotivo = $("#tbOtroMotivo"),
            tbTiempoParo = $("#tbTiempoParo"),
            tbTiempoReparacion = $("#tbTiempoReparacion"),
            tbTiempoParoTotal = $("#tbTiempoParoTotal"),
            tbTiempoMuerto = $("#tbTiempoMuerto"),
            tbTiempoMuertoDescripcion = $("#tbTiempoMuertoDescripcion"),
            txtComentarios = $("#txtComentarios"),

            tblEmpleadosTrabajo = $("#tblEmpleadosTrabajo"),
            tbHoraFinalizacion = $("#tbHoraFinalizacion"),
            tbHoraEntrada = $("#tbHoraEntrada"),
            cboMotivoParo = $("#cboMotivoParo"),
            tbHorasHombre = $("#tbHorasHombre"),
            tbPuestoEmpleado = $("#tbPuestoEmpleado"),
            tbNombreEmpleado = $("#tbNombreEmpleado"),
            btnAddPersonal = $("#btnAddPersonal"),
            cboEconomico = $("#cboEconomico"),
            tbObra = $("#tbObra"),
            tbFechaCreacion = $("#tbFechaCreacion"),
            tbModelo = $("#tbModelo"),
            tbHorometro = $("#tbHorometro"),
            cboTurno = $("#cboTurno"),

            radio1 = $("#radio1"),
            tbMotivosParoDesc = $("#tbMotivosParoDesc"),
            checkbox30 = $("#checkbox30");

        btnGuardarOT = $("#btnGuardarOT"),
            modalEliminarRegistro = $("#modalEliminarRegistro"),
            btnEliminarRegistro = $("#btnEliminarRegistro");
        btnAplicarFiltros = $("#btnAplicarFiltros");

        cboBusqCC = $("#cboBusqCC");
        txtBusqCC = $("#txtBusqCC");
        var HorometrosSeleccionado = 0;

        tblListaOT1 = $("#tblListaOT1");
        let idBL = 0;
        // let imprimir = 0;

        function init() {
            // let urlparams = getUrlParamsAdan(window.location.href);
            // // console.log(idBL)
            // if (urlparams != "" && urlparams != "undefined" &&  urlparams != undefined && urlparams != {} ) {
            //     idBL = urlparams.idBL;
            // }
            tbFechaOT.change(GetBuscarHorometro);
            ValidarPermisoAuditor();
            if (PermisoAuditor) {
                btnNuevo.prop('disabled', 'disabled');
            }

            SetToggle();
            SetRequiered();
            initGrid();

            tbFechaOT.datepicker().datepicker("setDate", new Date());

            tbFechaCreacion.datepicker().datepicker("setDate", new Date());

            var d = new Date();
            var n = d.getFullYear();
            FFinal.datepicker().datepicker("setDate", new Date());
            FInicial.datepicker().datepicker("setDate", new Date(n, d.getMonth(), 1));

            TimeData();
            tbMotivosParoDesc.prop('disabled', true);
            checkbox30.change();

            cboMotivoParo.fillCombo('/OT/cboMotivosParo', null, false);
            cboBusqCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
            cboEconomico.fillCombo('/OT/cboFillEconomicos', { cc: "" });


            tbNombreEmpleado.getAutocomplete(SelectEmpleado, null, '/OT/getEmpleados');
            cboEconomico.change(InfoEconomico);
            btnAgregarHH.click(AddTable);
            cboMotivoParo.change(SetComentario);
            btnAplicarFiltros.click(LoadTable);

            btnGuardarOT.click(GuardarCapturaOT);
            btnExportar.click(fnExportar);
            tbTiempoReparacion.val(0);
            tbTiempoMuerto.val(0);
            tbTiempoParo.val(0);
            tbTiempoMuerto.change(setComentario);
            tbTiempoMuertoH.val(0);

            BntRegresar.click(Regresar);
            btnNuevo.click(AccionNuevo);
            btnEliminarRegistro.click(DeleteDetalle);
            DisabledData();

            tbTiempoMuertoH.change(setTiempoTrabajo);
            tbTiempoMuertoM.change(setTiempoTrabajo);

            cboBusqCC.change(setCC);

            cboBusqCC.change(setEconomicos);
            cboBusqCC.change();
            cboMotivoParo.val('0');
            cboMotivoParoFiltro.fillCombo('/OT/cboMotivosParo', null, false);

            tbHorometro.change(ValidaHorometro);
            BloqueoInformacion();
            setTiempoTrabajo();

            //#region BACKLOGS
            const parametros = getUrlParams(window.location.href);
            if (parametros && parametros.idBL) {
                idBL = parseFloat(parametros.idBL);
                fncGetDataBL(idBL);
            }
            //#endregion
        }

        //#region BACKLOGS
        function fncGetDataBL(idBL) {
            let obj = new Object();
            obj = {
                idBL: idBL
            };
            axios.post("/BackLogs/GetDatosBL", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    // console.log(response.data.objBL);
                    cboBusqCC.val(response.data.objBL.areaCuenta);
                    btnNuevo.click();
                    cboEconomico.val(response.data.objBL.idCatMaquina);
                    cboEconomico.change();
                    tbHorometro.val(response.data.objBL.horometro);
                    btnOTAbierta.click(); //OT ABIERTA
                    cboMotivoParo.val(22); //BACKLOG
                    cboMotivoParo.change();
                }
            }).catch(error => Alert2Error(error.message));
        }

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

        function fncRedirigirseBackLogs(idBL) {
            //#region SE VERIFICA SI EL BL ES DE OBRA O TMC
            let obj = new Object();
            obj = {
                idBL: idBL
            };
            axios.post("/BackLogs/GetTipoBL", obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    let esObra = response.data.esObra
                    // console.log(esObra);
                    if (esObra) {
                        document.location.href = '/BackLogs/RegistroBackLogsObra';
                    } else {
                        document.location.href = '/BackLogs/PresupuestoRehabilitacionTMC';
                    }
                }
            }).catch(error => Alert2Error(error.message));
            //#endregion
        }

        // const getUrlParamsAdan = function (url) {
        //     console.log(url)
        //     let params = {};
        //     let parser = document.createElement('a');
        //     parser.href = url;
        //     let query = parser.search.substring(1);
        //     if (query == "") {
        //         params = { idBL: "" }
        //     } else {
        //         let vars = query.split('&');
        //         for (let i = 0; i < vars.length; i++) {
        //             let pair = vars[i].split('=');
        //             params[pair[0]] = decodeURIComponent(pair[1]);
        //         }
        //     }
        //     return params;
        // };
        //#endregion

        function fnExportar() {
            var url = '/OT/ExportData/?cc=' + cboBusqCC.val() + "&fechaInicio=" + FInicial.val() + "&fechaFin=" + FFinal.val() + "&economico=" + (cboEconomicofiltro.val() == "" ? 0 : cboEconomicofiltro.val()) + "&motivo=" + (cboMotivoParoFiltro.val() == "" ? 0 : cboMotivoParoFiltro.val());
            download(url);
        }
        function download(url) {
            $.blockUI({ message: "Preparando archivo a descargar" });
            iframe = document.getElementById('iframeDownload');
            iframe.src = url;

            var timer = setInterval(function () {

                var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
                // Check if loading is complete
                if (iframeDoc.readyState == 'complete' || iframeDoc.readyState == 'interactive') {
                    setTimeout(function () {
                        $.unblockUI();
                    }, 5000);

                    clearInterval(timer);
                    return;
                }
            }, 1000);
        }
        function setEconomicos() {
            cboEconomicofiltro.fillCombo('/OT/cboFillEconomicos', { cc: cboBusqCC.val() });
        }

        function ValidarPermisoAuditor() {

            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/administrador/usuarios/getUsuariosPermisos',
                success: function (response) {
                    PermisoAuditor = response.Autorizador;

                    if (PermisoAuditor) {
                        btnNuevo.prop('disabled', 'disabled');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function BloqueoInformacion() {

            tbFechaOT.prop('disabled', true);
            tbHorometro.prop('disabled', true);
            tbHoraEntrada.prop('disabled', true);
            tbHoraFinalizacion.prop('disabled', true);
            tbTiempoMuertoH.prop('disabled', true);
            tbTiempoMuertoM.prop('disabled', true);
            tbHoraEntradaHH.val('');

        }

        function AccionNuevo() {
            Limpiar();

            cboEconomico.fillCombo('/OT/cboFillEconomicos', { cc: cboBusqCC.val() });
        }

        function setDisabled() {

            tbTiempoParoH.prop('disabled', true);
            tbTiempoParoM.prop('disabled', true);
            tbTiempoReparacionH.prop('disabled', true);
            tbTiempoReparacionM.prop('disabled', true);
            tbTiempoMuertoH.prop('disabled', true);
            tbTiempoMuertoH.prop('disabled', true);

        }

        function GetDates(tipo) {

            var Fecha1 = tbHoraEntrada.val();

            var Fecha2 = tbHoraFinalizacion.val();

            if (Fecha2 != "") {
                var Tiempo1 = moment(Fecha1, 'DD/MM/YYYY HH:mm');
                var Tiempo2 = moment(Fecha2, 'DD/MM/YYYY HH:mm');

                var horas = Tiempo2.diff(Tiempo1, 'h');
                var minutos = Tiempo2.diff(Tiempo1, 'm');

                var TiempoTotal = horas + ":" + (minutos - (horas * 60));
                tbTiempoParoH.val(horas);
                tbTiempoParoM.val((minutos - (horas * 60)));
                // setTiempoTrabajo();
                CalculoTiempos(tipo);

            }
            else {

                tbTiempoParoH.val('0');
                tbTiempoParoM.val('0');
                tbTiempoReparacionH.val('0');
                tbTiempoReparacionM.val('0');
                tbTiempoMuertoH.val('0');
                tbTiempoMuertoM.val('0');
            }

        }

        function setTiempoTrabajo() {

            array1 = tblEmpleadosTrabajoGrid.data();

            var horasTrabajadasInicioM = array1.column(2).data().toArray().sort();

            array1.column(3).data().toArray().sort(function (a, b) {
                a = new Date(a.dateModified);
                b = new Date(b.dateModified);
                return a > b ? -1 : a < b ? 1 : 0;
            });

            var horasTrabajadasFinM = array1.column(3).data().toArray();
            var FechaIncioM = new Date(moment(horasTrabajadasInicioM[0], 'DD/MM/YYYY HH:mm'));
            var FechaFinM = new Date(moment(horasTrabajadasFinM[0], 'DD/MM/YYYY HH:mm'));



            tbHoraEntrada.data("DateTimePicker").date(new Date(FechaIncioM));
            tbHoraFinalizacion.data("DateTimePicker").date(new Date(FechaFinM));
            /*
            var InicioMecanico = moment(FechaIncioM, 'DD/MM/YYYY HH:mm');
            var FinMecanico = moment(FechaFinM, 'DD/MM/YYYY HH:mm');


            var FechaEntrada = tbHoraEntrada.val();
            var FechaSalida = tbHoraFinalizacion.val();

            var EntradaOT = moment(FechaEntrada, 'DD/MM/YYYY HH:mm');
            var SalidaOT = moment(FechaSalida, 'DD/MM/YYYY HH:mm');

            var totalHoras = 0;
            var totalMinutos = 0;
            var DiffEntradaH = 0;
            var DiffEntradaM = 0;
            var DiffSalidaH = 0;
            var DiffSalidaM = 0;

            if (EntradaOT != InicioMecanico) {
                DiffEntradaH = EntradaOT.diff(InicioMecanico, 'h');
                DiffEntradaM = EntradaOT.diff(InicioMecanico, 'm');
            }

            if (SalidaOT != FechaSalida) {
                DiffSalidaH = SalidaOT.diff(FinMecanico, 'h');
                DiffSalidaM = SalidaOT.diff(FinMecanico, 'm');
            }
            totalHoras = DiffEntradaH + DiffSalidaH;
            totalMinutos = DiffSalidaM + DiffEntradaM;*/
        }

        function DisabledData() {
            tbTiempoParo.prop('disabled', true);
            tbTiempoReparacion.prop('disabled', true);
            tbObra.prop('disabled', true);
            tbModelo.prop('disabled', true);
            tbFechaCreacion.prop('disabled', true);
        }

        function Limpiar() {
            // btnAgregarHH.prop('disabled', true);
            var HorometrosSeleccionado = 0;
            if (ckOTAbierta.checked) {
                $("#btnOTAbierta").trigger('click');
            }
            tbOtroMotivo.prop('disabled', true);

            cboEconomico.val('');
            tbFechaOT.prop('disabled', true);
            tbHorometro.prop('disabled', true);
            tbHoraEntrada.prop('disabled', true);
            tbHoraFinalizacion.prop('disabled', true);
            cboEconomico.clearCombo();
            tbModelo.val('');
            tbObra.val('');
            tbObra.removeAttr('data-CC');
            tbHoraEntrada.val('');
            tbHoraFinalizacion.val('');
            tbHorometro.val('');
            tbFechaCreacion.datepicker().datepicker("setDate", new Date());
            tbMotivosParoDesc.prop('disabled', true);
            SetComentario();
            TimeData();
            cboTipoParo.val('');
            cboTipoParo2.val('');
            cboTipoParo3.val('2');
            cboMotivoParo.val('');
            tbTiempoReparacion.val(0);
            tbTiempoMuerto.val(0);
            tbTiempoParo.val(0);
            tbTiempoMuertoM.val(0);
            setComentario();
            txtComentarios.val('');
            tbTiempoParoH.val(0);
            tbTiempoParoM.val(0);
            tbTiempoReparacionH.val(0);
            tbTiempoReparacionM.val(0);

            if (tblEmpleadosTrabajoGrid.data().length > 0) {
                tblEmpleadosTrabajoGrid.clear();
                tblEmpleadosTrabajoGrid.draw();
            }
            removeRequiered();
            if (cboBusqCC.val() != "") {
                Detalle.removeClass('hide');
                Principal.addClass('hide');

            }
            else {
                AlertaGeneral("Alerta", "Se debe seleccionar un centro de costos para continuar.");
            }
        }

        function ValidaHorometro() {

            var Status = false;
            if (aplicaValidacion) {

                var HorometroMas = Number(HorometrosSeleccionado) + 40;
                var horomertroMenos = (Number(HorometrosSeleccionado) - 40) < 0 ? 0 : Number(HorometrosSeleccionado) - 40;


                if (HorometroMas >= Number(tbHorometro.val()) && Number(tbHorometro.val()) >= horomertroMenos) {
                    Status = true;

                }
                else {
                    AlertaGeneral('Alerta', 'El horometro esta fuera del rango de la fecha seleccionada');
                    tbHorometro.val('');
                }

                return Status;
            }
        }

        function Regresar() {
            Detalle.addClass('hide');
            Principal.removeClass('hide');
        }

        function LoadTable() {
            $.blockUI({ message: 'Cargando información...' });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/LoadTableOT',
                async: false,
                data: { cc: cboBusqCC.val(), FechaInicio: FInicial.val(), FechaFin: FFinal.val(), economicos: cboEconomicofiltro.val() == "" ? 0 : cboEconomicofiltro.val(), tipoParo: cboMotivoParoFiltro.val() == "" ? 0 : cboMotivoParoFiltro.val() },
                success: function (response) {

                    var TablaOT = response.TablaOTC;
                    var TablaOT2 = response.TablaOTA;
                    tblListaOT.bootgrid("clear");
                    tblListaOT.bootgrid("append", TablaOT);
                    tblListaOT.bootgrid('reload');

                    tblListaOT1.bootgrid("clear");
                    tblListaOT1.bootgrid("append", TablaOT2);
                    tblListaOT1.bootgrid('reload');

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setComentario() {

            var h = tbTiempoMuertoH.val();
            var m = tbTiempoMuertoM.val();

            var c = Number(h) + Number(m);
        }

        function setValueHorasTotales() {

            var Fecha1 = tbHoraEntrada.val();
            var Fecha2 = tbHoraFinalizacion.val();

            if (Fecha2 != "") {
                var Dato1 = moment(Fecha1, 'DD/MM/YYYY HH:mm');
                var Dato2 = moment(Fecha2, 'DD/MM/YYYY HH:mm');

                var Tiempo = Number(Dato2.diff(Dato1, 'm'));

                tbTiempoParo.val((Tiempo / 60).toFixed(2));
            } else {
                tbTiempoParo.val((0).toFixed(2));
            }

        }

        function TimeData() {
            var dateNow = new Date();

            $('#tbHoraEntrada').datetimepicker({
                format: 'DD/MM/YYYY HH:mm'

            });
            $('#tbHoraFinalizacion').datetimepicker({
                format: 'DD/MM/YYYY HH:mm',
                useCurrent: true //Important! See issue #1075
            });

            $("#tbHoraEntradaHH").on("dp.change", function (e) {
                $("#tbHoraEntradaHH").data("DateTimePicker").maxDate(dateNow);
                // GetBuscarHorometro();
            });

            $("#tbHoraFinalizacion").on("dp.change", function (e) {
                GetDates(2);
            });

            $('#tbHoraEntradaHH').datetimepicker({
                format: 'DD/MM/YYYY HH:mm'

            });
            $('#tbHoraFinalizacionHH').datetimepicker({
                format: 'DD/MM/YYYY HH:mm',
                useCurrent: false //Important! See issue #1075
            });

            $("#tbHoraEntrada").on("dp.change", function (e) {
                $("#tbHoraEntradaHH").data("DateTimePicker").maxDate(dateNow);
                GetBuscarHorometro();
                GetDates(1);
                if (!ckOTAbierta.checked) {
                    tbHoraFinalizacion.prop('disabled', false);
                }
                else {
                    tbHoraFinalizacion.prop('disabled', false);
                }

            });

            $("#tbHoraFinalizacion").data("DateTimePicker").maxDate(dateNow);
            $("#tbHoraEntrada").data("DateTimePicker").maxDate(dateNow);
            $("#tbHoraFinalizacionHH").data("DateTimePicker").maxDate(dateNow);
            $("#tbHoraEntradaHH").data("DateTimePicker").maxDate(dateNow);

        }
        idRow = 0;

        function GetBuscarHorometro() {

            if (aplicaValidacion) {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/OT/GetHorometroActuales',
                    data: { idEconomico: cboEconomico.val(), tbHoraEntradaHH: tbFechaOT.val() },
                    async: false,
                    success: function (response) {
                        HorometrosSeleccionado = response.ListaHorometro;

                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
        }

        function AddTable() {

            var Fecha1 = tbHoraEntradaHH.val();
            var Fecha2 = tbHoraFinalizacionHH.val();

            var Tiempo1 = moment(Fecha1, 'DD/MM/YYYY HH:mm');
            var Tiempo2 = moment(Fecha2, 'DD/MM/YYYY HH:mm');

            var horas = Tiempo2.diff(Tiempo1, 'h');
            var minutos = Tiempo2.diff(Tiempo1, 'm');

            if (horas >= 0 && minutos >= 0) {
                var TiempoTotal = horas + ":" + (minutos - (horas * 60));

                //if (validaCamposPersonal(horas, minutos)) {
                if (true) {
                    idRow = GetMaxId();
                    idRow += 1;

                    var Data = [];
                    Data = tblEmpleadosTrabajoGrid.data();

                    var objData = {};

                    if (idGlobalOT != "0") {
                        idTempRow = 0;
                    }

                    objData.id = idTempRow;
                    objData.NombreE = tbNombreEmpleado.val();
                    objData.PuestoE = tbPuestoEmpleado.val();
                    objData.HoraInicio = tbHoraEntradaHH.val();
                    objData.HoraFin = tbHoraFinalizacionHH.val();

                    objData.HorasTrabajo = TiempoTotal;
                    var idTempRow = idRow;
                    if (idGlobalOT != "0") {
                        idTempRow = 0;
                    }
                    objData.Accion = getAccion(idTempRow);
                    objData.PersonalID = tbNombreEmpleado.attr('data-NumEmpleado');
                    objData.OrdenTrabajoID = idGlobalOT;
                    Data.push(objData);

                    setInfoTable1(Data);

                    tbNombreEmpleado.val('');
                    tbPuestoEmpleado.val('');

                    CalculoTiempos(3);

                    $("#tbHoraEntradaHH").val('');
                    $("#tbHoraFinalizacionHH").val('');

                    tbHoraEntradaHH.data("DateTimePicker").date(new Date(Tiempo1));
                    tbHoraFinalizacionHH.data("DateTimePicker").date(new Date(Tiempo2));
                    setTiempoTrabajo();

                }
                else {
                    AlertaGeneral('Alerta', 'Debe seleccionar al personal y horas trabajadas');
                }
            }
            else {
                AlertaGeneral('Alerta', 'Usted está seleccionado una fecha menor a la indicada de inicio.');
            }
        }

        function CalculoTiempos(tipo) {
            //Aqui sacamos el maximo y minimo de la tabla de personal.
            array1 = tblEmpleadosTrabajoGrid.data();

            var horasTrabajadasInicio = array1.column(2).data().toArray().sort(function (a, b) {

                a = new Date(moment(a, 'DD/MM/YYYY HH:mm'));
                b = new Date(moment(b, 'DD/MM/YYYY HH:mm'));
                return a > b ? -1 : a < b ? 1 : 0;
            });

            var horasTrabajadasFin = array1.column(3).data().toArray().sort(function (a, b) {

                a = new Date(moment(a, 'DD/MM/YYYY HH:mm'));
                b = new Date(moment(b, 'DD/MM/YYYY HH:mm'));
                return a > b ? -1 : a < b ? 1 : 0;
            });

            //var horasTrabajadasFin = array1.column(3).data().toArray().sort();

            //Obtenemos las fechas.
            var PrimerFechaPersonal = new Date(moment(horasTrabajadasInicio[horasTrabajadasInicio.length - 1], 'DD/MM/YYYY HH:mm'));
            var UltimaFechaPersonal = new Date(moment(horasTrabajadasFin[0], 'DD/MM/YYYY HH:mm'));
            switch (tipo) {
                case 1:
                    tbHoraFinalizacion.data("DateTimePicker").date(new Date(UltimaFechaPersonal));
                    break;
                case 2:
                    tbHoraEntrada.data("DateTimePicker").date(new Date(PrimerFechaPersonal));
                    break;
                case 3:
                    tbHoraEntrada.data("DateTimePicker").date(new Date(PrimerFechaPersonal));
                    tbHoraFinalizacion.data("DateTimePicker").date(new Date(UltimaFechaPersonal));
                    break;

                default:

            }



            ///Rango de fecha y hora de Orden de trabajo
            var HoraInicioOT = tbHoraEntrada.val();
            var HoraTerminacionOT = tbHoraFinalizacion.val();

            //Preparamos las fechas para hacer restas.
            var TiempoInicialOT = moment(HoraInicioOT, 'DD/MM/YYYY HH:mm');
            var TiempoFinalOT = moment(HoraTerminacionOT, 'DD/MM/YYYY HH:mm');

            var TiempoInicialPersonal = moment(PrimerFechaPersonal.toLocaleDateString() + " " + PrimerFechaPersonal.toLocaleTimeString(), 'DD/MM/YYYY HH:mm');
            var TiempoFinalPersonal = moment(UltimaFechaPersonal.toLocaleDateString() + " " + UltimaFechaPersonal.toLocaleTimeString(), 'DD/MM/YYYY HH:mm');

            var TiempoTotalIncio;
            var tiempoMinutosTotales;
            var minutos1;
            var minutos2;

            if (TiempoInicialOT != TiempoInicialPersonal) {

                var horas = TiempoInicialPersonal.diff(TiempoInicialOT, 'h');
                var minutos = TiempoInicialPersonal.diff(TiempoInicialOT, 'm');
                minutos1 = minutos;
                TiempoTotalIncio = horas + ":" + (minutos - (horas * 60));
            }

            var TiempoTotalFinal;

            if (TiempoFinalOT != TiempoFinalPersonal) {

                var horas = TiempoFinalOT.diff(TiempoFinalPersonal, 'h');
                var minutos = TiempoFinalOT.diff(TiempoFinalPersonal, 'm');
                TiempoTotalFinal = horas + ":" + (minutos - (horas * 60));

                minutos2 = minutos;
            }
            var totalMinutosMuertos = (minutos2 + minutos1);

            var HorasMuertas = (totalMinutosMuertos / 60);

            var horasSplit = HorasMuertas.toString().split('.')[0];
            var MinutosMuertos = (totalMinutosMuertos - (Number(horasSplit) * 60));

            if (HoraTerminacionOT != "") {
                tbTiempoMuertoH.val(Number(horasSplit));
                tbTiempoMuertoM.val(MinutosMuertos);
            }

            /*Tiempo de Reparacion*/
            var horasTiempoReparacion = TiempoFinalPersonal.diff(TiempoInicialPersonal, 'h')
            var MinutosTiempoReparacion = TiempoFinalPersonal.diff(TiempoInicialPersonal, 'm')

            TiempoTotalReparacion = horasTiempoReparacion + ":" + (MinutosTiempoReparacion - (horasTiempoReparacion * 60));

            tbTiempoReparacionH.val(horasTiempoReparacion);
            tbTiempoReparacionM.val((MinutosTiempoReparacion - (horasTiempoReparacion * 60)));




        }


        function SetHorasTrabajo() {

            GetDates();

        }

        function validaCamposPersonal(horas, minutos) {

            var estatus = true;
            if (tbNombreEmpleado.val().length <= 0) {
                estatus = false;
            }
            if (tbPuestoEmpleado.val().length <= 0) {
                estatus = false;
            }

            if (horas == 0 && minutos == 0) {
                estatus = false;
            }
            return estatus;

        }


        function removeRowInfo() {
            var row = $(this).attr('idRow');
            GetDates();
        }

        idRowDelete = 0;
        var ElementoTRTEMP;
        $(document).on('click', ".removeRow", function () {
            idRowDelete = 0;
            var row = $(this).attr('data-idRow');
            ElementoTR = $(this).parents('td');
            var indexTable = tblEmpleadosTrabajoGrid.row(ElementoTR)[0][0];
            var obj = tblEmpleadosTrabajoGrid.data()[indexTable];
            if (obj.OrdenTrabajoID != 0) {
                idRowDelete = obj.id;
                modalEliminarRegistro.modal('show');
                ElementoTRTEMP = $(this).parents('td');
            }
            else {
                var horasTrabajo = Number(obj.HorasTrabajo);
                var currentHoras = tbTiempoReparacion.val();
                tbTiempoReparacion.val(Number(currentHoras) - horasTrabajo);
                tblEmpleadosTrabajoGrid.row(ElementoTR).remove().draw();
            }


            SetHorasTrabajo();

        });

        function getAccion(id) {

            return "<div> <button class='btn btn-danger removeRow'data-idrow='" + id + "' type='button' > " +
                "<span class='glyphicon glyphicon-remove'></span></button> " +
                "</div>";
        }


        function DeleteDetalle() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/DeletePersonal',
                data: { id: idRowDelete },
                async: false,
                success: function (response) {
                    modalEliminarRegistro.modal('hide');

                    var indexTable = tblEmpleadosTrabajoGrid.row(ElementoTR)[0][0];
                    var obj = tblEmpleadosTrabajoGrid.data()[indexTable];
                    var horasTrabajo = Number(obj.HorasTrabajo);
                    var currentHoras = tbTiempoReparacion.val();
                    tbTiempoReparacion.val(Number(currentHoras) - horasTrabajo);
                    tblEmpleadosTrabajoGrid.row(ElementoTR).remove().draw();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }


        function SetRequiered() {
            cboEconomico.addClass('required');
            tbObra.addClass('required');
            tbHorometro.addClass('required');
            tbHoraEntrada.addClass('required');
        }

        function removeRequiered() {
            cboEconomico.removeClass('errorClass');
            tbObra.removeClass('errorClass');
            tbHorometro.removeClass('errorClass');
            tbHoraEntrada.removeClass('errorClass');
        }

        function initGrid() {
            tblListaOT.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "EditarOT": function (column, row) {

                        if (row.Estatus != "Cerrada") {
                            return "<button type='button' class='btn btn-warning EditarOT' data-idOT='" + row.id + "' >" +
                                "<span class='glyphicon glyphicon-edit'></span> " +
                                " </button>";
                        } else {
                            return "<button type='button' class='btn btn-warning EditarOT' data-idOT='" + row.id + "' >" +
                                "<span class='glyphicon glyphicon-edit'></span> " +
                                " </button>";
                        }
                    },
                    "verReporte": function (column, row) {

                        if (row.Estatus == "Cerrada") {
                            return "<button type='button' class='btn btn-primary verReporte' data-idOT='" + row.id + "' >" +
                                "<span class='glyphicon glyphicon-print'></span> " +
                                " </button>";
                        } else {
                            return "";
                        }

                    },

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */

                tblListaOT.find(".EditarOT").on("click", function (e) {
                    removeRequiered();
                    cboTurno.prop('disabled', false);
                    idGlobalOT = $(this).attr('data-idOT');
                    Principal.addClass('hide');
                    Detalle.removeClass('hide');
                    GetInfoOT(idGlobalOT);
                });

                tblListaOT.find(".verReporte").on("click", function (e) {

                    var idOT = $(this).attr('data-idOT');
                    verReporte(idOT);
                });

            });

            tblListaOT1.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "EditarOT": function (column, row) {

                        if (row.Estatus != "Cerrada") {
                            return "<button type='button' class='btn btn-warning EditarOT' data-idOT='" + row.id + "' >" +
                                "<span class='glyphicon glyphicon-edit'></span> " +
                                " </button>";
                        } else {
                            return "";
                        }
                    },
                    "verReporte": function (column, row) {

                        if (row.Estatus == "Cerrada") {
                            return "<button type='button' class='btn btn-primary verReporte' data-idOT='" + row.id + "' >" +
                                "<span class='glyphicon glyphicon-print'></span> " +
                                " </button>";
                        } else {
                            return "";
                        }

                    },

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */

                tblListaOT1.find(".EditarOT").on("click", function (e) {
                    removeRequiered();
                    idGlobalOT = $(this).attr('data-idOT');
                    Principal.addClass('hide');
                    Detalle.removeClass('hide');
                    GetInfoOT(idGlobalOT);
                });

                tblListaOT1.find(".verReporte").on("click", function (e) {
                    var idOT = $(this).attr('data-idOT');
                    verReporte(idOT);
                });

            });
        }

        function verReporte(idOT) {
            if (idBL == "") {
                idBL = 0;
            }
            var path = "/Reportes/Vista.aspx?idReporte=45&idOT=" + idOT + "&idBL=" + idBL;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };

        }

        function GetInfoOT(idGlobalOT) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/GetInfoOT',
                data: { id: idGlobalOT },
                async: false,
                success: function (response) {

                    var maquina = response.maquina;
                    var CCName = response.CCName;
                    var objOT = response.objOT;
                    var objDet = response.objDet;

                    setInfoTable1(objDet);

                    tbObra.val(CCName);
                    tbObra.attr('data-CC', response.CCMaquina);

                    cboEconomico.fillCombo('/OT/cboFillEconomicos', { cc: response.CCMaquina });
                    cboEconomico.val(maquina);


                    HorometrosSeleccionado = response.horometroValida;


                    if (HorometrosSeleccionado == 0) {
                        aplicaValidacion = false;
                    }

                    tbModelo.val(response.Modelo);
                    tbHorometro.val(objOT.horometro);
                    cboTurno.val(objOT.Turno);
                    cboTipoParo.val(objOT.TipoParo1);
                    cboTipoParo2.val(objOT.TipoParo2);
                    cboTipoParo3.val(objOT.TipoParo3);
                    cboMotivoParo.val(objOT.MotivoParo);
                    tbTiempoParo.val(objOT.TiempoTotalParo);
                    tbTiempoReparacion.val(objOT.TiempoReparacion);
                    tbTiempoMuerto.val(objOT.TiempoMuerto);

                    txtComentarios.val(objOT.Comentario);

                    cboMotivoParo.trigger('change');
                    tbTiempoMuerto.trigger('change');

                    tbTiempoMuertoDescripcion.val(objOT.DescripcionTiempoMuerto);
                    tbOtroMotivo.val(response.DescMotivoParo);
                    Fecha1 = Number(objOT.FechaEntrada.split('(')[1].split(')')[0]);
                    tbHoraEntrada.datetimepicker({ locale: 'es' });
                    tbHoraEntrada.data("DateTimePicker").date(new Date(Fecha1));


                    if (objOT.FechaSalida != null) {
                        Fecha2 = Number(objOT.FechaSalida.split('(')[1].split(')')[0]);
                        tbHoraFinalizacion.datetimepicker({ locale: 'es' });
                        tbHoraFinalizacion.data("DateTimePicker").date(new Date(Fecha2));
                    }
                    else {

                        tbHoraFinalizacion.val('');
                        tbHoraFinalizacion.data("DateTimePicker").destroy();
                        $('#tbHoraFinalizacion').datetimepicker({
                            format: 'DD/MM/YYYY HH:mm',
                            useCurrent: false
                        });

                        tbTiempoParo.val(0);
                    }

                    SetHorasTrabajo();

                    if (objOT.EstatusOT) {
                        $("#btnOTAbierta").trigger('click');
                        tbHoraFinalizacion.val('');
                    }

                    tbFechaOT.prop('disabled', false);
                    tbHorometro.prop('disabled', false);
                    tbHoraEntrada.prop('disabled', false);

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }



        function setInfoTable1(dataSet) {

            tblEmpleadosTrabajoGrid = $('#tblEmpleadosTrabajo').DataTable({
                destroy: true,
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { "title": "Nombre Personal", data: "NombreE" },
                    { "title": "Puesto Personal", data: "PuestoE" },
                    { "title": "Hora Inicio", data: "HoraInicio" },
                    { "title": "Hora Fin", data: "HoraFin" },
                    { "title": "", data: "Accion" },
                ],
                "bPaginate": false,
                "bFilter": false,
                "paging": false,
                "info": false

            });

        }

        function GetMaxId() {
            var idRow = $('.editRow[data-idrow]').get().map(function (value) {
                return Number($(value).attr('data-idrow'));
            });
            Array.prototype.max = function () {
                return Math.max.apply(null, this);
            };

            if (idRow.length != 0) {
                return idRow.max();
            }
            else {
                return 0;
            }
        }

        function SelectEmpleado(event, ui) {
            tbNombreEmpleado.text(ui.item.value);
            tbNombreEmpleado.attr('data-NumEmpleado', ui.item.id);
            $('#tbNumEmpleado').val(ui.item.id);
            SetInfoEmpleado(ui.item.id);
        }


        function SetInfoEmpleado(idEmplado) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetSingleUsuario',
                data: { id: idEmplado },
                async: false,
                success: function (response) {
                    if (response.success) {
                        tbPuestoEmpleado.val(response.Puesto.toLowerCase());
                        tbPuestoEmpleado.attr('data-CC', response.CCEmpleado);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function SetComentario() {
            if (cboMotivoParo.val() != "") {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/OT/dataMotivosPAro',
                    data: { id: cboMotivoParo.val() },
                    async: false,
                    success: function (response) {
                        var Data = response.data;

                        $("#tbTipoMantenimiento").val(Data.TiempoMantenimiento);
                        $("#tbTipoParo").val(Data.TipoParo);
                        $("#tbOtroMotivo").val(Data.DescripcionParo);

                        if (cboMotivoParo.val() == "27") {
                            cboTipoParo3.val(1);
                        }
                        else {
                            cboTipoParo3.val(2);
                        }
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {
                $("#tbTipoMantenimiento").val('');
                $("#tbTipoParo").val('');
                $("#tbOtroMotivo").val('');
            }
        }

        function InfoEconomico() {

            if (cboEconomico.val() != "") {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/OT/GetInfoEconomico',
                    type: "POST",
                    datatype: "json",
                    data: { idEconomico: cboEconomico.val() },
                    success: function (response) {
                        var objEconomico = response.dataEconomico;
                        tbHoraEntradaHH.val('');
                        tbHoraFinalizacionHH.val('');

                        tbObra.val(objEconomico.CCName);
                        tbObra.attr('data-CC', objEconomico.CC);
                        tbModelo.val(objEconomico.Modelo).prop('disabled', true);
                        aplicaValidacion = (objEconomico.tipoEquipo == 1 ? true : false);
                        tbHorometro.prop('disabled', false);

                        tbFechaOT.prop('disabled', false);
                        tbHoraEntrada.prop('disabled', false);
                        tbHoraFinalizacion.prop('disabled', false);
                        btnAgregarHH.prop('disabled', false);
                        GetBuscarHorometro();
                        $.unblockUI();

                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {
                tbObra.val('');
                tbModelo.val('').prop('disabled', true);
                tbObra.removeAttr('data-CC');
            }

        }

        function GuardarCapturaOT() {

            if (ValidateInfo()) {

                if (ckOTAbierta.checked) {
                    AlertaGeneral('Advertencia', "usted esta generando una orden de trabajo abierta");
                    tbHoraFinalizacion.val('');
                }

                GetDataCapturaOT();
                // TODO
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/OT/GuardarCapturaOT',
                    type: "POST",
                    datatype: "json",
                    data: {
                        objCapturaOT: GetDataCapturaOT(),
                        objListPersonal: GetDataTableTPersonal(),
                        idBL: idBL
                    },
                    success: function (response) {
                        if (response.success) {
                            if (idGlobalOT == 0) {
                                AlertaGeneral('Confirmacion', 'Se Agrego una nueva OT con Folio: <h1>' + response.folio + '</h1>');
                                idGlobalOT = 0;
                                AccionNuevo();

                                //#region BACKLOGS
                                if (idBL > 0) {
                                    Alert2AccionConfirmar("", "¿Desea redirigirse a BackLogs?", "Confimar", "Cancelar", () => fncRedirigirseBackLogs(idBL));
                                }
                                //#endregion
                            }
                            else {
                                AlertaGeneral('Confirmacion', 'Se actualizo la orden exitosamente con Folio: <h1>' + response.folio + '</h1>');
                                idGlobalOT = 0;
                                AccionNuevo();
                            }
                        }
                        else {
                            AlertaGeneral('Alerta', 'Ocurrio un error al guardar información');
                        }

                        $.unblockUI();

                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }

        }

        function validateEmpleados() {

            if (tblEmpleadosTrabajoGrid.data().length == 0) {
                return false;
            }
            else {
                return true;
            }
        }

        function SetToggle() {
            $('.button-checkbox').each(function () {
                // Settings
                var $widget = $(this),
                    $button = $widget.find('button'),
                    $checkbox = $widget.find('input:checkbox'),
                    color = $button.data('color'),
                    settings = {
                        on: {
                            icon: 'glyphicon glyphicon-check'
                        },
                        off: {
                            icon: 'glyphicon glyphicon-unchecked'
                        }
                    };

                // Event Handlers
                $button.on('click', function () {
                    $checkbox.prop('checked', !$checkbox.is(':checked'));
                    $checkbox.triggerHandler('change');
                    updateDisplay();
                });
                $checkbox.on('change', function () {
                    updateDisplay();
                });

                // Actions
                function updateDisplay() {
                    var isChecked = $checkbox.is(':checked');
                    // Set the button's state
                    $button.data('state', (isChecked) ? "on" : "off");

                    // Set the button's icon
                    $button.find('.state-icon')
                        .removeClass()
                        .addClass('state-icon ' + settings[$button.data('state')].icon);

                    // Update the button's color
                    if (isChecked) {
                        $("#lblOTAbierta").text('SI');
                        $button
                            .removeClass('btn-default')
                            .addClass('btn-' + color + ' active');
                    }
                    else {
                        $("#lblOTAbierta").text('NO');
                        $button
                            .removeClass('btn-' + color + ' active')
                            .addClass('btn-default');
                    }
                }

                // Initialization
                function init1() {
                    $('#tbNumEmpleado').change(function () {
                        axios.post('/HorasHombre/searchNumEmpleado?term=""&puesto=0&numEmpleado=' + $('#tbNumEmpleado').val())
                            .catch(o_O => AlertaGeneral(o_O.message))
                            .then(response => {
                                let { success, items } = response.data;
                                // console.log(response.data.id)
                                // console.log(response.data.label)
                                tbNombreEmpleado.attr('data-numempleado', response.data.id)
                                tbNombreEmpleado.text(response.data.label)
                                tbNombreEmpleado.val(response.data.label)
                                tbPuestoEmpleado.attr('data-cc', response.data.idPuesto)
                                tbPuestoEmpleado.val(response.data.prefijo)
                            });
                    })
                    updateDisplay();

                    // Inject the icon if applicable
                    if ($button.find('.state-icon').length == 0) {
                        $button.prepend('<i class="state-icon ' + settings[$button.data('state')].icon + '"></i> ');
                    }
                }
                init1();
            });
        }

        function GetDataCapturaOT() {
            let obj = new Object();
            obj = {

                id: idGlobalOT,
                EconomicoID: cboEconomico.val(),
                CC: tbObra.attr('data-CC'),
                horometro: tbHorometro.val(),
                Turno: cboTurno.val(),
                TipoParo1: cboTipoParo.val(),
                TipoParo2: cboTipoParo2.val(),
                TipoParo3: cboTipoParo3.val(),
                MotivoParo: cboMotivoParo.val(),
                DescripcionMotivo: tbOtroMotivo.val(),
                FechaEntrada: tbHoraEntrada.val(),
                FechaSalida: tbHoraFinalizacion.val(),
                TiempoTotalParo: tbTiempoParo.val(),
                TiempoReparacion: tbTiempoReparacion.val(),
                TiempoMuerto: tbTiempoMuerto.val(),
                DescripcionTiempoMuerto: tbTiempoMuertoDescripcion.val(),
                Comentario: txtComentarios.val(),
                TiempoHorasTotal: tbTiempoParoH.val(),
                TiempoHorasReparacion: tbTiempoReparacionH.val(),
                TiempoHorasMuerto: tbTiempoMuertoH.val(),
                TiempoMinutosTotal: tbTiempoParoM.val(),
                TiempoMinutosReparacion: tbTiempoReparacionM.val(),
                TiempoMinutosMuerto: tbTiempoMuertoM.val(),
                TipoOT: ckOTAbierta.checked ? 1 : 0,
                EstatusOT: ckOTAbierta.checked
            }
            console.log(obj);
            return obj;
        }

        function GetDataTableTPersonal() {

            var DataSet = [];
            var Data = tblEmpleadosTrabajoGrid.data();


            $.each(Data, function (index, value) {
                var JsonData = {};

                JsonData.PersonalID = value.PersonalID;
                JsonData.OrdenTrabajoID = value.OrdenTrabajoID;


                if (value.OrdenTrabajoID == 0) {
                    JsonData.id = 0;
                }
                else {
                    JsonData.id = value.id;
                }
                JsonData.HoraInicio = value.HoraInicio;
                JsonData.HoraFin = value.HoraFin;
                DataSet.push(JsonData);
            });

            return DataSet;

        }

        function datePicker() {

            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = tbFechaOT
                    .datepicker({
                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1,
                        defaultDate: new Date(year, 00, 01),
                        maxDate: new Date(year, 11, 31),
                        minDate: new Date(year, month, day),
                        onChangeMonthYear: function (y, m, i) {
                            var d = i.selectedDay;
                            $(this).datepicker('setDate', new Date(y, m - 1, d));
                        },
                        onSelect: function () {
                            var event;
                            if (typeof window.Event == "function") {
                                event = new Event('change');
                                this.dispatchEvent(event);
                            } else {
                                event = document.createEvent('HTMLEvents');
                                event.initEvent('change', false, false);
                                this.dispatchEvent(event);
                            }
                        },
                        onClose: function (evt, ui) {
                            $(this).datepicker("refresh");
                        }

                    })
                    .on("change", function () {
                        //  GetBuscarHorometro();


                    });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }

        function ValidateInfo() {
            var state = true;
            if (aplicaValidacion) {
                state = ValidaHorometro();
                if (!validarCampo(tbHorometro)) { state = false; }
            }
            if (!validarCampo(tbObra)) { state = false; }
            if (!validarCampo(cboEconomico)) { state = false; }

            if (!validarCampo(cboTipoParo3)) { state = false; }
            if (!validarCampo(tbHoraEntrada)) { state = false; }

            if (!ckOTAbierta.checked) {
                tbHoraFinalizacion.addClass('required');
                if (!validarCampo(tbHoraEntrada)) { state = false; }
            }
            else {
                tbHoraFinalizacion.removeClass('required');
            }

            if (tblEmpleadosTrabajoGrid.data()[0] == undefined) {
                state = false;
                AlertaGeneral('Alerta', 'Debe agregar el personal que realizo el trabajo');
            }
            TiempoParoH = Number(tbTiempoParoH.val());
            TiempoParoM = Number(tbTiempoParoM.val());
            TiempoReparacionH = Number(tbTiempoReparacionH.val());
            TiempoReparacionM = Number(tbTiempoReparacionM.val());
            TiempoMuertoH = Number(tbTiempoMuertoH.val());
            TiempoMuertoM = Number(tbTiempoMuertoM.val());

            if (TiempoParoH < 0) { state = false; }
            if (TiempoParoM < 0) { state = false; }

            if (TiempoReparacionH < 0) { state = false; }
            if (TiempoReparacionM < 0) { state = false; }

            if (TiempoMuertoH < 0) { state = false; }
            if (TiempoMuertoM < 0) { state = false; }


            return state;
        }



        function setCC() {

            var cc = $("#cboBusqCC option:selected").text()
            if (cc == "--Seleccione--") {
                txtBusqCC.val("TODOS");
            }
            else {
                txtBusqCC.val(cc);
            }
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.OT.CapturaOT = new CapturaOT();
    });
})();