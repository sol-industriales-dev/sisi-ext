(function () {

    $.namespace('maquinaria.captura.diaria.CapturaHorometro');

    CapturaHorometro = function () {

        var fecha = getFecha();
        var PermisoAuditor = false;


        txtCC = $("#txtCC"),
            idNotalbl = $("#idNotalbl"),
            tituloModalMaquina = $("#tituloModalMaquina"),
            txtDateCurrent = $("#txtDateCurrent"),
            modalIdRitmo = $("#modalIdRitmo"),
            modalEconomico = $("#modalEconomico"),
            modalRutina = $("#modalRutina"),
            modalnoEconomico = $("#modalnoEconomico"),
            cboModalnoEconomico = $("#cboModalnoEconomico"),
            btnAsignar = $("#btnAsignar"),
            modalCentroCostos = $("#modalCentroCostos"),
            modalDesfase = $("#modalDesfase"),
            btnReporte = $("#btnReporte"),
            btnCorteKubrix = $("#btnCorteKubrix"),
            cboTipo = $("#cboTipo"),

            //Modal de Reporete
            //-----------------
            txtModalCCFiltro = $("#txtModalCCFiltro"),
            txtModalNombreCC = $("#txtModalNombreCC"),
            txtModalFechaFiltro = $("#txtModalFechaFiltro"),
            btnModalBuscarReporte = $("#btnModalBuscarReporte"),
            iframeID = $("#iframeID"),
            cboModalTurno = $("#cboModalTurno"),
            loadingData = $("#loadingData");
        //-----------------

        modalHorasDesfase = $("#modalHorasDesfase"),
            btnAddDesfase = $("#btnAddDesfase"),
            trabajoPorSemana = $("#trabajoPorSemana"),
            trabajoPorDia = $("#trabajoPorDia"),
            titleModal = $("#titleModal"),
            btnModalCancelar = $("#btnModalCancelar"),
            cboRitmoTrabajo = $("#cboRitmoTrabajo"),
            ireport = $("#report"),
            modalReportes = $("#modalReportes"),
            btnModalGuardarRitmo = $("#btnModalGuardarRitmo"),
            txtTurno = $("#txtTurno");
        tableCaptura = $("#tableCaptura");
        gridResultado = $("#gridData");
        txtNombreCC = $("#txtNombreCC");
        btnGuardar = $("#btnGuardar");
        url = '/horometros/getTableData1';

        mensajes = {
            NOMBRE: 'Horometros',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        function ini() {
            ValidarPermisoAuditor();

            txtCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, true);

            datePicker();
            cboTipo.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });
            modalIdRitmo.text("0");
            titleModal.text("Modificacion de rutina de trabajo");
            tituloModalMaquina.text("Rutina de Trabajo");

            txtTurno.change(fillTable);
            // txtCC.change(fillTable);
            btnGuardar.click(sendInfo);
            btnModalGuardarRitmo.click(beforeSaveOrUpdate);
            //  txtDateCurrent.val(fecha);
            btnAsignar.click(abrirDesfase);

            modalCentroCostos.change(fillCboModalEconmico);
            btnAddDesfase.click(guardarDesfase);

            cboModalnoEconomico.change(getDataDesfase);
            cboModalnoEconomico.addClass('required');
            modalHorasDesfase.addClass('required');

            trabajoPorDia.change(function (e) {
                validaCantidad(modalEconomico.val());
            });
            txtDateCurrent.datepicker().datepicker("setDate", new Date());
            txtDateCurrent.datepicker("setDate", -1);
            txtModalFechaFiltro.datepicker().datepicker("setDate", new Date());
            //modalEconomico.change(getDataChange);
            btnReporte.click(verReporte);
            btnCorteKubrix.click(function (e) {
                Alert2AccionConfirmar("Enviar Notificacion", "¿Desea enviar notificación para indicar que se guardaron todos los horometros para el corte actual Kubrix?.", "Confirmar", "Cancelar",
                    () => guardarCorteKubrix())
            });
            initGrid();
            btnModalBuscarReporte.click(filtrarReporte);
            /*if (centro_costos != 0) {
                txtCC.val(centro_costos).prop('disabled', true);
                
            }*/
            fillTable();
            cboTipo.change(fillTable);
            txtDateCurrent.change(fillTable);
            idNotalbl.text('Favor de Capturar, el total de horometros por todos los turnos.');
            txtCC.change(fillTable);
        }

        function ValidarPermisoAuditor() {

            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/administrador/usuarios/getUsuariosPermisos',
                success: function (response) {
                    PermisoAuditor = response.Autorizador;
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function guardarCorteKubrix() {
            var AC = txtCC.val();
            $.ajax({
                datatype: "json",
                data: { ac: AC },
                type: "POST",
                url: '/horometros/GuardarCorteKubrixAC',
                success: function (response) {
                    if (response.success)
                        ConfirmacionGeneral("Confirmación", "Se envió la notificación", "bg-green");
                    else
                        AlertaGeneral("Alerta", "Ya existe una notificación de horómetros para el periodo actual. Si no reconoce este envío o se requiere realizarlo de nuevo, favor de comunicarse con el departamento de TI");
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }


        function filtrarReporte() {
            iframeID.addClass('hidden');
            loadingData.removeClass('hidden');
            var idReporte = "3";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&CC=" + txtCC.val() + "&pTurno=" + cboModalTurno.val() + "&pFecha=" + txtModalFechaFiltro.val();

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };

            e.preventDefault();
        }
        var text = "";
        function getDataRitmo(obj, ritmo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/horometros/getCC',
                success: function (response) {
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function validaCantidad(econo) {
            var validaEconomico = getTipoDato(econo);
            if ($(this).val() > 24 && validaEconomico != " KM") {
                $(this).val(0);
                AlertaGeneral("Alerta", "La cantidad no puede exceder de 24 horas");
            }
        }
        function getDataDesfase() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/horometros/getDataDesfase',
                data: { obj: cboModalnoEconomico.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.dataDesfase;
                    var desfase = 0;
                    if (data != null) {
                        desfase = data.horasDesfaseAcumulado != null ? data.horasDesfaseAcumulado : 0;
                    }

                    modalHorasDesfase.attr('horasDesfaseAcumulado', desfase);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function getDataChange() {
            getDataRitmo($(this).val());
        }

        function guardarDesfase() {
            if (validModalDesfase()) {
                SaveOrUpdate_Desfase(getDesfaseObject());
            }

        }
        function abrirDesfase() {
            resetValues();
            cargarValoresDesfase();
            modalDesfase.modal('show');
        }

        function cargarValoresDesfase() {

            cboModalnoEconomico.fillCombo('/Horometros/cboModalEconomico', { obj: txtCC.val() });
            modalCentroCostos.prop('disabled', true);
            modalCentroCostos.val(txtCC.val());
            fillCboModalEconmico(txtCC.val());
        }

        function fillCboModalEconmico(data) {
            cboModalnoEconomico.fillCombo('/Horometros/cboModalEconomico', { obj: data });
        }


        function DateServerSend() {
            var dateTypeVar = txtDateCurrent.datepicker('getDate');
            return $.datepicker.formatDate('mm-dd-yy', dateTypeVar);
        }


        function fillTableCombo() {
            bootG(url, txtCC.val(), $(this).val(), txtDateCurrent.val());
        }

        txtCC.keypress(function (e) {
            if (e.which == 13) {
                fillTable();
            }
        });

        $(document).keyup(function (e) {
            if (e.keyCode === 13) $("#dialogalertaGeneral .close").click();  // enter
        });
        function fillTable() {
            if (txtCC.val() != "") {
                bootG(url, txtCC.val(), txtTurno.val(), txtDateCurrent.val());

            }
        }

        function beforeSaveOrUpdate() {

            if (true) {
                saveOrUpdateRitmo(getPlainObject());
            }
        }

        function getPlainObject() {
            return {
                id: modalIdRitmo.val(),
                economico: modalEconomico.val().trim(),
                horasDiarias: trabajoPorDia.val(),
                horasSemana: trabajoPorSemana.val()

            }
        }

        function resetValues() {

            cboModalnoEconomico.val('');
            modalHorasDesfase.val('');
        }

        function valid() {
            var state = true;
            if (!modalEconomico.valid()) { state = false; }
            if (!trabajoPorDia.valid()) { state = false; }
            if (!trabajoPorSemana.valid()) { state = false; }

            return state;
        }

        function verReporte(e) {


            var CC = txtCC.val();
            var turno = txtTurno.val();
            var fecha = txtDateCurrent.val();

            var flag = true;
            if (CC == null || turno == null || fecha == "") {
                flag = false;

            }
            if (flag) {
                var idReporte = "3";

                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&CC=" + txtCC.val() + "&pTurno=" + txtTurno.val() + "&pFecha=" + txtDateCurrent.val();

                ireport.attr("src", path);

                $.blockUI({ message: mensajes.PROCESANDO });

                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();

                };
                e.preventDefault();
            }
            else {
                $.unblockUI();
                AlertaGeneral("Alerta", "Debe seleccionar un filtro", "bg-red");
            }

        }

        function getDesfaseObject() {
            var desfase = redondear(Number(modalHorasDesfase.attr('horasDesfaseAcumulado')) + Number(modalHorasDesfase.val()), 2);
            return {
                Economico: cboModalnoEconomico.val().trim(),
                horasDesfase: modalHorasDesfase.val(),
                horasDesfaseAcumulado: desfase,
                estado: true,
                fecha: txtDateCurrent.val(),
                CC: modalCentroCostos.val()
            }
        }

        function validModalDesfase() {
            var state = true;
            if (cboModalnoEconomico.val() == null) { state = false; }
            if (modalHorasDesfase.val() == "") { state = false; }
            return state;
        }

        ini();
        function sendInfo() {
            var datos = [];
            var tbl = $('table#gridData tr').get().map(function (row) {
                return $(row).find('td').get().map(function (cell) {
                    if ($(cell).children().prop('tagName') == 'INPUT') {

                        if ($(cell).children().is(':disabled')) {
                            return 0;
                        }
                        else {
                            return $(cell).children().val();
                        }
                    }
                    if ($(cell).children().prop('tagName') == 'LABEL') {

                        var res = $(cell).text()
                        if (res.indexOf("HR") >= 0) {
                            return res.replace(" HR", "");
                        }
                        else if (res.indexOf("KM") >= 0) {
                            return res.replace(" KM", "");
                        }

                        return $(cell).text();
                    }
                    if ($(cell).children().prop('tagName') == 'BUTTON') {
                        return $(cell).children().attr('data-ritmo');
                    }
                });
            });
            var array = [];

            $.each(tbl, function (index, value) {
                if (index != 0 && value[2] != 0) {
                    var json = {};
                    json.Economico = value[0];
                    json.CC = txtCC.val();
                    json.HorasTrabajo = value[2];
                    json.Horometro = value[3];
                    json.HorometroAcumulado = value[5];
                    json.Desfase = value[4];
                    json.Fecha = txtDateCurrent.val();
                    json.Turno = txtTurno.val();
                    json.Ritmo = value[6];
                    json.FechaCaptura = new Date();
                    if (value[0] != null) {
                        array.push(json);
                    }
                }
            });
            if (array.length > 0) {
                saveOrUpdate(array);
            }
            else {
                ConfirmacionGeneral("Alerta", "No se encontraron datos de captura", "bg-red");
            }

        }

        function getnoEconomicos() {
            modalEconomico.clearCombo();
            modalEconomico.append("<option> Seleccione: </option>");
            var tbl = $('table#gridData tr').get().map(function (row) {
                return $(row).find('td').get().map(function (cell) {
                    if ($(cell).children().prop('tagName') == 'LABEL') {
                        return $(cell).text();
                    }
                });
            });
            $.each(tbl, function (index, value) {
                if (index != 0) {
                    var Select = "";
                    if (value[0] != null) {
                        modalEconomico.append("<option value=\"" + value[0] + "\">" + value[0] + "</option>");
                    }
                }
            });
        }

        function saveOrUpdateRitmo(obj) {

            $.ajax({
                url: '/horometros/SaveOrUpdate_Ritmo',
                type: 'POST',
                dataType: 'json',
                data: { obj: obj, ritmo: cboRitmoTrabajo.val() },
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    bootG(url, txtCC.val(), txtTurno.val(), txtDateCurrent.val());
                    modalRutina.modal('hide');

                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function SaveOrUpdate_Desfase(obj) {
            $.ajax({
                url: '/horometros/SaveOrUpdate_Desfase',
                type: 'POST',
                dataType: 'json',
                data: { obj: obj },
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    /// poner validacion true
                    resetValues();
                    modalDesfase.modal('hide');
                    bootG(url, txtCC.val(), txtTurno.val(), txtDateCurrent.val());
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function saveOrUpdate(array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/horometros/SaveOrUpdate_Horometros',
                type: 'POST',
                dataType: 'json',
                data: { array: array },
                success: function (response) {
                    if (response.success == true) {
                        ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                        bootG(url, txtCC.val(), txtTurno.val(), txtDateCurrent.val());
                    }
                    else {
                        ConfirmacionGeneral("Alerta", response.message, "bg-red");
                        $.unblockUI();
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        var editIndex = undefined;

        function initGrid() {

            gridResultado.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                },
                formatters: {
                    "economico": function (column, row) {
                        return "<label data-val='" + row.Economico + "' class='eco'>" + row.Economico + "</label>";
                    },
                    "horometro": function (column, row) {
                        var tipo = getTipoDato(row.Economico);
                        return "<label data-val='" + row.Horometro + "' >" + (row.Horometro + tipo) + "</label>";
                    },
                    "editNumber": function (column, row) {
                        var tipo = getTipoDato(row.Economico);
                        if (row.habilidatado) {

                            return "<input type=\"number\" class=\"horasTrabajo\" data-horometro='" + row.Horometro + "' min='0' value='" + row.HorasTrabajo + "' data-promedioHoras='" + row.promedioHoras + "' data-isHR='" + tipo + "' disabled>";

                        }
                        else {
                            return "<input type=\"number\" class=\"horasTrabajo\" data-horometro='" + row.Horometro + "' min='0' data-promedioHoras='" + row.promedioHoras + "' value='0' data-isHR='" + tipo + "' data-valorMaximo='" + row.maximoHoras + "' >";
                        }
                    },
                    "resultado": function (column, row) {
                        var tipo = getTipoDato(row.Economico);
                        return "<label class=\"resultado\" data-resultado='" + row.HorometroAcumulado + "'>" + (row.HorometroAcumulado + tipo) + "</label>";
                    },
                    "horometroActual": function (column, row) {
                        var tipo = getTipoDato(row.Economico);
                        return "<label class=\"sumHorometro\">" + (row.HorometroActual + tipo) + "</label>";
                    },
                    "ritmoTrabajo": function (column, row) {
                        if (row.Ritmo == true) {
                            return "<button type='button' class='btn btn-warning ritmoTrabajo'  tabindex='-1' data-ritmo='" + row.Ritmo + "' data-economico='" + row.Economico + "' >" +
                                "<span class='glyphicon glyphicon-edit '></span> " +
                                " </button>"
                                ;
                        }
                        else {
                            return "<button type='button' class='btn btn-success ritmoTrabajo'  tabindex='-1' data-ritmo='" + row.Ritmo + "' data-economico='" + row.Economico + "'>" +
                                "<span class='glyphicon glyphicon-plus '></span> " +
                                " </button>"
                                ;
                        }
                    },
                    "Desfase": function (column, row) {
                        return "<label class=\"sumTotalDesfase\">" + row.Desfase + "</label>";
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {

                if (PermisoAuditor) {
                    btnAsignar.prop('disabled', 'disabled');
                    btnGuardar.prop('disabled', 'disabled');
                    $(".horasTrabajo").prop('disabled', 'disabled');
                    $(".ritmoTrabajo").prop('disabled', 'disabled');
                }
                /* Executes after data is loaded and rendered */
                gridResultado.find(".horasTrabajo").on("change", function (e) {
                    var horaTrabajo = $(this).val();
                    var tipo = $(this).attr("data-isHR");
                    var promedioHoras = $(this).attr('data-promedioHoras');
                    var horasRestantes = $(this).attr('data-valormaximo');
                    if (" KM" != tipo) {

                        if (horaTrabajo > Number(horasRestantes)) {
                            horaTrabajo = 0;
                            AlertaGeneral("Alerta", "Solo restan " + horasRestantes + " horas para cumplir el maximo de horas diarias");
                            $(this).val(horaTrabajo);
                            return;
                        }
                        if (Number(horaTrabajo) > (Number(promedioHoras) + 4)) {
                            AlertaGeneral("Alerta", "Las horas capturadas esta por arriba del ritmo de trabajo");
                        }
                        if (Number(horaTrabajo) < Number((promedioHoras) - 4)) {
                            AlertaGeneral("Alerta", "Las horas capturadas esta por debajo del ritmo de trabajo");
                        }
                        if (horaTrabajo == "" || horaTrabajo == null) {
                            horaTrabajo = 0;
                        }
                    }
                    $(this).attr("value", horaTrabajo);
                    $(this).val(horaTrabajo);
                    var horasDesfase = $(this).parents('tr').find(".resultado").attr('data-resultado');
                    var resultado = redondear(Number(horaTrabajo) + Number($(this).attr('data-horometro')), 2);
                    var resultado2 = redondear(Number(horaTrabajo) + Number(horasDesfase), 2);
                    $(this).parents('tr').find(".sumHorometro").text(resultado + tipo);
                    $(this).parents('tr').find(".resultado").text(resultado2 + tipo);
                    $(this).parents('tr').find(".sumHorometro").attr("data-val", resultado);
                    $(this).parents('tr').find(".resultado").attr("data-val", resultado2);

                });

                gridResultado.find(".horasDesfase").on("change", function (e) {
                    var horaDesfase = $(this).val();
                    var desfaseTotal = redondear(Number(horaDesfase) + Number($(this).attr('data-desfase')), 2);
                    $(this).attr("value", horaDesfase);
                    var horometroActual = $(this).parents('tr').find(".sumHorometro").text();
                    var resultado = redondear(Number(horaDesfase) + Number(horometroActual), 2);
                    $(this).parents('tr').find(".resultado").attr("data-val", resultado);
                    $(this).parents('tr').find(".resultado").text(resultado);
                    $(this).parents('tr').find(".sumTotalDesfase").text(desfaseTotal);

                });
                gridResultado.find(".ritmoTrabajo").on("click", function (e) {
                    var Economico = $(this).attr('data-economico');
                    var ritmo = $(this).attr('data-ritmo');
                    modalEconomico.val(Economico);
                    modalRutina.modal('show');
                    getDataRitmo(Economico, ritmo);
                    cboRitmoTrabajo.val("true");

                });
            });
        }
        function redondear(numero, digitos) {
            let base = Math.pow(10, digitos);
            let entero = Math.round(numero * base);
            return entero / base;
        }
        function getDataRitmo(obj, ritmo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/horometros/getDataRitmo',
                data: { obj: obj },
                success: function (response) {
                    $.unblockUI();
                    var data = response.dataRitmo;
                    if (data != null) {
                        modalIdRitmo.val(data.id);
                    }
                    else {
                        modalIdRitmo.val(0);
                    }

                    if (ritmo == "true") {
                        trabajoPorDia.val(data.horasDiarias);
                        trabajoPorSemana.val(data.horasSemana);
                    }
                    else {
                        trabajoPorDia.val(0);
                        trabajoPorSemana.val(0);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function getDataCentroCostos(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/horometros/getCentroCostos',
                data: { obj: obj },
                success: function (response) {
                    $.unblockUI();
                    var nomb = response.centroCostos;
                    txtNombreCC.val(nomb);
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function bootG(url, obj, turno, fecha) {
            $.blockUI({ message: mensajes.PROCESANDO });
            var tipoMaquina = cboTipo.val();
            if (cboTipo.val() == "") {
                tipoMaquina = 0;
            }
            $.ajax({
                datatype: "json",
                type: "POST",
                url: url,
                data: { obj: obj, turno: turno, fecha: fecha, Tipo: tipoMaquina },
                success: function (response) {
                    $.unblockUI();
                    txtNombreCC.val("");
                    var data = response.maquinasHorometro;
                    btnAsignar.addClass('hidden');
                    btnReporte.addClass('hidden');
                    gridResultado.bootgrid("clear");
                    var nomb = response.centroCostos;

                    txtNombreCC.val(nomb);
                    if (data != null) {
                        gridResultado.bootgrid("append", data);
                        gridResultado.bootgrid('reload');
                        gridResultado.addClass('fix-tableGrid');



                        if (data.length > 0) {
                            btnAsignar.removeClass('hidden');
                            btnReporte.removeClass('hidden');
                            btnCorteKubrix.removeClass('hidden');

                        }
                        else {
                            btnAsignar.addClass('hidden');
                            btnReporte.removeClass('btnReporte');
                            btnCorteKubrix.addClass('hidden');

                        }

                    }
                    else {
                        txtNombreCC.val("");
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        //Se obtiene el dato si es kilometraje u horometro.
        function getTipoDato(econo) {
            var res = econo.split("-")[0];
            var tipo = "";
            switch (res) {
                case "PU":
                case "TA":
                case "TRA":
                case "CAP":
                case "CV":
                case "CGA":
                case "TU":
                case "CP":
                case "PD":
                case "OR":
                case "CEX":
                case "CLL":
                case "CSE":
                case 'CPE':
                    tipo = " KM";
                    break;
                default:
                    tipo = " HR";
                    break;
            }
            switch (econo) {
                case "CGA-03":
                case "OR-42":
                case "PD-33":
                    tipo = " HR";
                    break;
                case "PD-22":
                case "CEX-05":
                case "CEX-06":
                case "OR-35":
                case "CM-03":
                case "OR-38":
                case "CLL-07":
                case "CUA-03":
                    tipo = " KM";
                    break;
                default:
                    tipo = tipo;
                    break;
            }
            return tipo;
        }
        function datePicker() {
            var now = new Date(),
                year = now.getYear() + 1900;
            mes = now.getMonth();
            day = now.getDate();
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = $("#txtDateCurrent")
                    .datepicker({

                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1,
                        defaultDate: new Date(year, 00, 01),
                        maxDate: new Date(year, mes, day),
                        //minDate: new Date(year, mes, day - 5),
                        onSelect: function () {
                            $(this).trigger('change');
                        }
                    })
                    .on("change", function () {

                        //var date = $(this).val();
                        //var array = new Array();
                        //array = date.split('/');
                        //$(this).datepicker('setDate', new Date(array[2], array[1] - 1, 1));
                        //fechaFin.datepicker('setDate', new Date(array[2], array[1], 0));
                        $(this).blur();

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
    };

    $(document).ready(function () {
        maquinaria.captura.diaria.CapturaHorometro = new CapturaHorometro();
    });
})();