
(function () {

    $.namespace('maquinaria.Captura.Diaria.KPI');

    KPI = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        },
        mensajes = {
            NOMBRE: 'indicadores claves de desempeño de obras de construccion',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },

        dateIncio = "",
        dateFin = "",
        _Economico = 0,
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin"),
        txtDivision = $("#txtDivision"),
        txtCC = $("#txtCC"),
        txtNombreCC = $("#txtNombreCC"),
        cboGrupo = $("#cboGrupo"),
        cboModelo = $("#cboModelo"),
        txtFechaPeriodo = $("#txtFechaPeriodo"),
        tblData = $("#tblData"),
        btnCargar = $("#btnCargar"),
        btnRepMensual = $("#btnRepMensual"),
        btnRepMetricas = $("#btnRepMetricas"),
        btnRepGraficas = $("#btnRepGraficas"),
        modalInfoGeneral = $("#modalInfoGeneral"),
        modalTipoMantenimiento = $("#modalTipoMantenimiento"),
        modalMTTOyParo = $("#modalMTTOyParo"),
        tdTPTiempo = $("#tdTPTiempo"),
        tdTPCantidad = $("#tdTPCantidad"),
        tdTPPTiempo = $("#tdTPPTiempo"),
        tdTPPCantidad = $("#tdTPPCantidad"),
        tdTPTotal = $("#tdTPTotal"),
        tdTPTotal2 = $("#tdTPTotal2"),
        tdTNPTiempo = $("#tdTNPTiempo"),
        tdTNPCantidad = $("#tdTNPCantidad"),
        tdTNPPTiempo = $("#tdTNPPTiempo"),
        tdTNPPCantidad = $("#tdTNPPCantidad"),
        tdPTiempo = $("#tdPTiempo"),
        tdPCantidad = $("#tdPCantidad"),
        tdPPTiempo = $("#tdPPTiempo"),
        tdPPCantidad = $("#tdPPCantidad"),
        tdPTotal = $("#tdPTotal"),
        tdPTotal2 = $("#tdPTotal2"),
        tdCTiempo = $("#tdCTiempo"),
        tdCCantidad = $("#tdCCantidad"),
        tdCPTiempo = $("#tdCPTiempo"),
        tdCPCantidad = $("#tdCPCantidad"),
        tdPrTiempo = $("#tdPrTiempo"),
        tdPrCantidad = $("#tdPrCantidad"),
        tdPrPTiempo = $("#tdPrPTiempo"),
        tdPrPCantidad = $("#tdPrPCantidad"),
        tdPTotalF = $("#tdPTotalF"),
        tblDataMTTOyParo = $("#tblMTTOyParo"),
        tdIGEconomico = $("#tdIGEconomico"),
        tdIGHorometroInicial = $("#tdIGHorometroInicial"),
        tdIGHorometroFinal = $("#tdIGHorometroFinal"),
        tdIGHorasTrabajadas = $("#tdIGHorasTrabajadas"),
        tdIGDisponibilidad = $("#tdIGDisponibilidad"),
        tdIGMTBS = $("#tdIGMTBS"),
        tdIGMTTR = $("#tdIGMTTR"),
        tdIGHorasHombre = $("#tdIGHorasHombre"),
        tdIGHorasParo = $("#tdIGHorasParo"),
        tdIGRatioMantenimiento = $("#tdIGRatioMantenimiento"),
        cboAnio = $("#cboAnio"),
        cboMes = $("#cboMes");

        function init() {
            datePicker();
            //txtFechaPeriodo.datepicker({
            //    changeMonth: true,
            //    changeYear: true,
            //    showButtonPanel: true,
            //    dateFormat: 'MM yy',
            //    onClose: function (dateText, inst) {
            //        $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            //    }
            //});
            //txtFechaPeriodo.datepicker("setDate", new Date());
            var now = new Date(),
            year = now.getYear() + 1900;
            fechaIni.datepicker().datepicker("setDate", new Date(year, now.getMonth(), 1));
            fechaFin.datepicker().datepicker("setDate", new Date(year, now.getMonth() + 1, 0)).prop('disabled', true);
            getFechas();
            txtDivision.fillCombo('/Rentabilidad/fillComboDivision', null, false);
            txtCC.fillCombo('/Rentabilidad/cboCentroCostosUsuarios', null, false);
            cboGrupo.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: 1 });
            cboGrupo.change(setCboModelos);

            getDataCentroCostos(txtCC.val());

            //txtCC.change(fillTable);
            loadTable();
            $('#tblData tbody').on('click', 'tr', function () {
                if ($(this).hasClass('selected')) {
                    $(this).removeClass('selected');
                    $(".btnDisp").prop("disabled", true);
                    $(".btnDisp").attr("title", "");
                }
                else {
                    tblData.$('tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $(".btnDisp").prop("disabled", false);
                    $(".btnDisp").attr("title", "¡Primero debe seleccionar un registro!");
                }
            });
            btnCargar.click(reloadTable);
            btnRepMensual.click(getKPIMensual);
            btnRepMetricas.click(getKPIMetricas);
            btnRepGraficas.click(getKPIGraficas);
            txtDivision.change(function (e) {
                txtCC.fillCombo('/Rentabilidad/cboCentroCostosUsuarios', {divisionID: txtDivision.val()}, false);
            });

        }

        function getFechas() {
            var array2 = new Array();
            dateFinal = fechaFin.val();
            array2 = dateFinal.split('/');

					cboMes.val(array2[1]);
            dateIncio = array2[2] + "/" + array2[1]+ "/" + "01";
            dateFin = array2[2] + "/" + array2[1] + "/" + array2[0];
        }
        function setCboModelos() {
            cboModelo.fillCombo('/OT/cboModelo', { idGrupo: cboGrupo.val() });


        }

        function reloadTable() {
            $.blockUI({ message: 'Cargando información...' });
            cargarTable();
            //tblData.ajax.reload(null, false);
        }

        function cargarTable() {
            var listaCC = new Array();
            $("select#txtCC option").each(function () { listaCC.push($(this).val()); });
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/OT/getKPIGeneral',
                data: {
                    CC: (txtCC.val() == '' ? listaCC : [txtCC.val()]),
                    Tipo: (cboGrupo.val() == '' ? 0 : cboGrupo.val()),
                    Modelo: (cboModelo.val() == '' || cboModelo.val() == undefined ? 0 : cboModelo.val()),
                    FechaInicio: dateIncio, //fechaIni.datepicker('getDate'),
                    FechaFin: dateFin //fechaIni.datepicker('getDate')
                },
                success: function (response) {
                    $.unblockUI();
                    tblData.clear().rows.add(response.dataMain).draw();
                
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function loadTable() {
            $.blockUI({ message: 'Cargando información...' });
            tblData = $("#tblData").DataTable({

                columns: [
                        { data: 'id' },
                        { data: 'btnEquipo', width: "30px" },
                        { data: 'economico', width: "70px" },
                        //{ data: 'horasIdealMensual', width: "150px" },
                        { data: 'pDisponibilidad', width: "100px" },
                        { data: 'horasTrabajado', width: "100px" },
                        { data: 'horasParo', width: "70px" },
                        { data: 'pUtilizacion', width: "70px" },
                        { data: 'pMProgramadoTiempo', width: "70px" },
                        //{ data: 'pMProgramadoCantidad', width: "70px" },
                        { data: 'pPreventivoHoras', width: "70px" },
                        { data: 'pCorrectivoHoras', width: "70px" },
                        { data: 'pPredictivoHoras', width: "70px" },
                        { data: 'horasHombre', width: "50px" },
                        { data: 'MTBS', width: "50px" },
                        { data: 'MTTR', width: "50px" },
                        { data: 'parosPrincipal1', width: "50px" },
                        { data: 'parosPrincipal2', width: "50px" },
                        { data: 'parosPrincipal3', width: "50px" }
                ],
                'bProcessing': true,
                "bPaginate": false,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                "drawCallback": function (settings) {
                    $.unblockUI();
                },
                columnDefs:
                [
                    { orderable: true, targets: 0, "visible": false },
                    { orderable: true, targets: '_all' }
                ]
            });

        }
        function fillTable() {
            var _this = $(this);
            getDataCentroCostos(_this.val());
            tblData.ajax.reload();
        }
        function getDataCentroCostos(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/horometros/getCentroCostos',
                data: { obj: obj },
                async: false,
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
        function getKPIMensual() {
            var listaCC = $("select#txtCC option").map(function () { return $(this).val(); }).get();
            verReporte(61, "cc=" + (txtCC.val() == '' ? listaCC.join(',') : [txtCC.val()]) + "&tipo=" + (cboGrupo.val() == '' ? 0 : cboGrupo.val()) + "&modelo=" + (cboModelo.val() == '' || cboModelo.val() == undefined ? 0 : cboModelo.val()) + "&anio=" + fechaIni.val() + "&mes=" + fechaFin.val() + "&mesPalabra=" + $("#cboMes option:selected").text() + "&ccNombre=" + (txtCC.val() == '' ? $("#txtDivision option:selected").text() : $("#txtCC option:selected").text()), "H");
        }
        function getKPIMetricas() {
            var listaCC = $("select#txtCC option").map(function () { return $(this).val(); }).get();
            verReporte(59, "cc=" + (txtCC.val() == '' ? listaCC.join(',') : [txtCC.val()]) + "&tipo=" + (cboGrupo.val() == '' ? 0 : cboGrupo.val()) + "&modelo=" + (cboModelo.val() == '' || cboModelo.val() == undefined ? 0 : cboModelo.val()) + "&anio=" + fechaIni.val() + "&mes=" + fechaFin.val() + "&mesPalabra=" + $("#cboMes option:selected").text() + "&ccNombre=" + (txtCC.val() == '' ? $("#txtDivision option:selected").text() : $("#txtCC option:selected").text()), "H");
        }
        function getKPIGraficas() {
            var listaCC = $("select#txtCC option").map(function () { return $(this).val(); }).get();
            verReporte(60, "cc=" + (txtCC.val() == '' ? listaCC.join(',') : [txtCC.val()]) + "&tipo=" + (cboGrupo.val() == '' ? 0 : cboGrupo.val()) + "&modelo=" + (cboModelo.val() == '' || cboModelo.val() == undefined ? 0 : cboModelo.val()) + "&anio=" + fechaIni.val() + "&mes=" + fechaFin.val() + "&mesPalabra=" + $("#cboMes option:selected").text() + "&ccNombre=" + (txtCC.val() == '' ? $("#txtDivision option:selected").text() : $("#txtCC option:selected").text()).text(), "H");
        }

        function getDataCentroCostos(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/horometros/getCentroCostos',
                data: { obj: obj },
                async: false,
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

        init();
        function datePicker() {
            var now = new Date(),
            year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
              from = $("#fechaIni")
                .datepicker({

                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(year, 00, 01),
                    maxDate: new Date(year, 11, 31),
                    onSelect: function () {
                        $(this).trigger('change');
                    }
                })
                .on("change", function () {

                    var date = $(this).val();
                    var array = new Array();
                    dateIncio = "";
                    dateFin = "";

					
                    array = date.split('/');
                    $(this).datepicker('setDate', new Date(array[2], array[1] - 1, 1));
                    fechaFin.datepicker('setDate', new Date(array[2], array[1], 0));
                    $(this).blur();

                    getFechas();

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

        maquinaria.Captura.Diaria.KPI = new KPI();
    });
})();

function getKPIEquipo(id, noEconomico) {
    var listaCC = $("select#txtCC option").map(function () { return $(this).val(); }).get();
    verReporte(58, "cc=" + (txtCC.val() == '' ? listaCC.join(',') : [txtCC.val()]) + "&id=" + id + "&anio=" + fechaIni.val() + "&mes=" + fechaFin.val() + "&mesPalabra=" + fechaFin.val() + "&ccNombre=" + (txtCC.val() == '' ? $("#txtDivision option:selected").text() : $("#txtCC option:selected").text()), "H");
}
function verReporte(idReporte, parametros, orientacion) {
    $.blockUI({ message: mensajes.PROCESANDO });
    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&" + parametros;
    $("#report").attr("src", path);
    document.getElementById('report').onload = function () {
        $.unblockUI();
        openCRModal();
    };
  
}