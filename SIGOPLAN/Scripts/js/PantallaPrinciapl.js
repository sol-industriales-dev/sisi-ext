
(function () {

    $.namespace('Administrativo.Princiapl');

    Princiapl = function () {

        var dialog;
        mensajes = {
            NOMBRE: 'Proyecciones Financieras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },


        CMES1PPL = $(".CMES1PPL"),
        CMES2PPL = $(".CMES1PPL"),
        CMES3PPL = $(".CMES1PPL"),
        CMES4PPL = $(".CMES1PPL"),
        CMES5PPL = $(".CMES1PPL"),
        CMES6PPL = $(".CMES1PPL"),
        CMES7PPL = $(".CMES1PPL"),
        CMES8PPL = $(".CMES1PPL"),
        CMES9PPL = $(".CMES1PPL"),
        CMES10PPL = $(".CMES1PPL"),
        CMES11PPL = $(".CMES1PPL"),
        CMES12PPL = $(".CMES1PPL");

        CMES1SumPPL = $(".CMES1SumPPL"),
        CMES2SumPPL = $(".CMES2SumPPL"),
        CMES3SumPPL = $(".CMES3SumPPL"),
        CMES4SumPPL = $(".CMES4SumPPL"),
        CMES5SumPPL = $(".CMES5SumPPL"),
        CMES6SumPPL = $(".CMES6SumPPL"),
        CMES7SumPPL = $(".CMES7SumPPL"),
        CMES8SumPPL = $(".CMES8SumPPL"),
        CMES9SumPPL = $(".CMES9SumPPL"),
        CMES10SumPPL = $(".CME10SumPPL"),
        CMES11SumPPL = $(".CMES11SumPPL"),
        CMES12SumPPL = $(".CMES12SumPPL");

        CMES1SumPPPL = $(".CMES1SumPPPL"),
        CMES2SumPPPL = $(".CMES2SumPPPL"),
        CMES3SumPPPL = $(".CMES3SumPPPL"),
        CMES4SumPPPL = $(".CMES4SumPPPL"),
        CMES5SumPPPL = $(".CMES5SumPPPL"),
        CMES6SumPPPL = $(".CMES6SumPPPL"),
        CMES7SumPPPL = $(".CMES7SumPPPL"),
        CMES8SumPPPL = $(".CMES8SumPPPL"),
        CMES9SumPPPL = $(".CMES9SumPPPL"),
        CMES10SumPPPL = $(".CMES10SumPPPL"),
        CMES11SumPPPL = $(".CMES11SumPPPL"),
        CMES12SumPPPL = $(".CMES12SumPPPL");


        CMES1FE = $(".CMES1FE"),
        CMES2FE = $(".CMES2FE"),
        CMES3FE = $(".CMES3FE"),
        CMES4FE = $(".CMES4FE"),
        CMES5FE = $(".CMES5FE"),
        CMES6FE = $(".CMES6FE"),
        CMES7FE = $(".CMES7FE"),
        CMES8FE = $(".CMES8FE"),
        CMES9FE = $(".CMES9FE"),
        CMES10FE = $(".CMES10FE"),
        CMES11FE = $(".CMES11FE"),
        CMES12FE = $(".CMES12FE");
        CMES1UBP = $(".CMES1UBP");

        /*Parametros*/
        btnAddRegistro = $("#btnAddRegistro"),
        lblEncabezadoObra = $("#lblEncabezadoObra"),
        cboPeriodo = $("#cboPeriodo"),
        cboEscenario = $("#cboEscenario"),
        tbDivisor = $("#tbDivisor"),
        tbMesesInicio = $("#tbMesesInicio"),
        /*---------------------------------*/
        /*Container*/
        idTituloContainer = $("#idTituloContainer"),
        txtCentroCostos = $("#txtCentroCostos"),

        idVistaParcial = $("#idVistaParcial"),
        btnGastosAdmon = $("#btnGastosAdmon");
        divPrincipal = $("#divPrincipal");
        btnHome = $("#btnHome");
        tblCapturaObras = $("#tblCapturaObras"),
        btnCifrasPrincipales = $("#btnCifrasPrincipales"),
        btnEdoResultado = $("#btnEdoResultado"),
        btnFlujo = $("#btnFlujo"),
        btnBalance = $("#btnBalance"),
        btnObras = $("#btnObras"),
        btnMaquinaria = $("#btnMaquinaria"),
        btnActivoFijo = $("#btnActivoFijo"),
        btnPagosDiversos = $("#btnPagosDiversos"),
        btnCobroDiversos = $("#btnCobroDiversos"),
        btnSaldosIniciales = $("#btnSaldosIniciales"),
        ireport = $("#report"),
        btnPremisas = $("#btnPremisas");

        var myChart;

        tabla = $('#tblResultadoMensual').DataTable({});
        tabla2 = $('#tblFlujoEfectivo').DataTable({});
        tabla3 = $('#tblVentasMensuales').DataTable({});
        tabla4 = $('#tblUtilidadBruta').DataTable({});
        tabla5 = $('#tblGastosAdmo').DataTable({});
        tabla6 = $('#tblVentaAcumulada').DataTable({});
        tabla7 = $("#tblResultadoACumulado").DataTable({});
        tabla8 = $("#tblMargenResultadoAcumulado").DataTable({});

        function init() {
            var d = new Date();
            var n = d.getMonth() - 1;
            tbMesesInicio.change(LoadTabla1);
            tbMesesInicio.val(n);
            /*OBRA DE PANTALLA*/
            tbMesesInicio.change(LoadTableObras);
            btnAddRegistro.click(OpenModal);
            //  LoadTabla1();
            /*reportes*/
            idTituloContainer.text('Datos Pricipales');

            btnEdoResultado.click(VerReporte);
            btnFlujo.click(VerReporte);
            btnBalance.click(VerReporte);
            btnCifrasPrincipales.click(VerReporte);

            /**/
            btnMaquinaria.click(loadVista1);
            btnActivoFijo.click(loadVista1);
            btnPremisas.click(loadVista1);
            btnPagosDiversos.click(loadVista1);
            btnObras.click(loadVista1);
            btnGastosAdmon.click(loadVista);
            btnCobroDiversos.click(loadVista1);
            btnSaldosIniciales.click(loadVista1);
            btnHome.click(loadHome);
            var datah = ['', "ABR 2017", "MAY 2017", "JUN 2017", "JUL 2017", "AGO 2017", "SEP 2017", "OCT 2017", "NOV 2017", "DIC 2017", "ENE 2017", "FEB 2017", ""];

            var data = {
                labels: datah,
                datasets: [{
                    lineTension: 0.1, // Que tan recta esta la linea.
                    fill: false,
                    borderColor: "rgba(75,192,192,1)",
                    data: [null, '12.58', '12.77', '12.30', '12.19', '12.09', '12.02', '11.99', '11.92', '11.21', '11.23', '11.27', null],
                    steppedLine: false
                },
                {
                    lineTension: 0.1, // Que tan recta esta la linea.
                    fill: false,
                    borderColor: "rgba(75,192,192,1)",
                    data: [12.72, 12.52, 12.32, 12.12, 11.92, 11.72, 11.52, 11.32, 11.12, 10.92, 10.72, 10.52],
                    borderDash: [5, 5]
                },

                ]
            };
            var ctx = document.getElementById("LineWithLine");

            myChart = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                    scales: {
                        yAxes: [{
                            ticks: {
                                min: 10,
                                stepSize: 1,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });

            myChart.resize();
            GetCadenaCapturaObras();
        }

        function OpenModal() {
            dialog.dialog("open");
        }

        function GetCadenaCapturaObras() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //  url: '/proyecciones/LoadObra',
                url: '/Proyecciones/CapturaObraSetData',
                type: 'POST',
                dataType: 'json',
                data: { escenario: cboEscenario.val(), divisor: tbDivisor.val(), meses: tbMesesInicio.val(), anios: cboPeriodo.val() },
                //  data: { escenario: 1, meses: 3, anio: 2017 },
                success: function (response) {

                    var ListaDivisionesVentas = response.ListaDivisionesVentas;
                    setDataTable(ListaDivisionesVentas);
                    var UtilidadNeta = response.UtilidadNETA;
                    var VentasNetas = response.VentasNetas;
                    var SaldoFinalFlujoEfectivo = response.SaldoFinalFlujoEfectivo;
                    var TotalGtoOperacion = response.TotalGtoOperacion;
                    var UtilidadPromedioBruta = response.UtilidadPromedioBruta;
                    //LoadTabla1(UtilidadNeta);

                    LoadTabla1(UtilidadNeta);
                    LoadTabla2(SaldoFinalFlujoEfectivo);
                    LoadTabla3(VentasNetas);
                    LoadTabla4(UtilidadPromedioBruta);
                    LoadTabla5(TotalGtoOperacion);
                    LoadTabla6(VentasNetas);
                    LoadTabla7(VentasNetas);
                    LoadTabla8(VentasNetas);
                    


                    $.unblockUI();

                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function setDataTable(ListaDivisionesVentas) {

            for (var i = 0; i < 3; i++) {
                var id = i + 1;
                $('#lblLugar' + id).val(ListaDivisionesVentas[i].Lugar);
                $('#lblDescripcion' + id).val(ListaDivisionesVentas[i].Obra);
                $('#lblVenta' + id).val((ListaDivisionesVentas[i].Venta).toFixed(2));
                $('#lblPorcentaje2' + id).val((ListaDivisionesVentas[i].Procentaje * 100).toFixed(2));
            }

        }

        function loadHome() {
            idTituloContainer.text('Datos Pricipales');
            divPrincipal.removeClass('hide');
            $('#idVistaParcial').html('');
            $('#idVistaParcial').empty();
        }

        function loadVista() {
            var btnValue = $(this).val();
            var liga = getLiga($(this).val());
            $('#idVistaParcial').html('');
            $('#idVistaParcial').empty();
            $("#idVistaParcial").load(liga, null);
            divPrincipal.addClass('hide');

        }

        function loadVista1() {
            var btnValue = $(this).val();
            var liga = getLiga($(this).val());
            if (liga != "") {
                $('#idVistaParcial').html('');
                $('#idVistaParcial').empty();
                $('#idVistaParcial').load(liga, null);
                divPrincipal.addClass('hide');
            }

        }

        function getListaLoad(liga) {
            switch (liga) {
                case "1":
                    return '';
                    break;
                case "2":
                    return '';
                    break;
                case "3":
                    return '';
                    break;
                case "4":
                    return '';
                    break;
                case "5":
                    break;
                case "6":
                    return '';
                    break;
                case "7":
                    return '/Proyecciones/GastosAdministacionyVentas';
                    break;
                case "8":
                    return '/Proyecciones/ActivoFijo';
                    break;
                case "9":
                    return '/Proyecciones/PagosDiversos';
                    break;
                case "10":
                    return '/Proyecciones/IngresosDiversos';
                    break;
                case "11":
                    return '/Proyecciones/EstadoPosicionFinanciera';
                    break;
                case "12":
                    return '/Proyecciones/Premisas';
                    break;

                default:
                    return '';
                    break;

            }
        }

        function LoadTableObras() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/LoadObra',
                type: 'POST',
                dataType: 'json',

                success: function (response) {
                    var dataRes = response.GetData;
                    GetInfoObras(dataRes)

                    $.unblockUI();
                    txtCentroCostos.text(response.centro_costos);
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetInfoObras(dataSet) {
            var tableDet;
            var tituloMeses = [];
            var date = new Date();
            lblEncabezadoObra.text("Obras Consideradas " + (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
            tituloMeses = GetPeriodoMeses(1);
            tableDet = $('#tblCapturaObras').DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar MENU registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },

                "bFilter": false,
                destroy: true,
                "scrollX": true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: "Area" },
                    { data: "Codigo" },
                    { data: "Descripcion" },
                    { data: "Probabilidad" },
                    { data: "Margen" },
                    { data: "Monto" },
                    { "title": tituloMeses[0], data: "MES1" },
                    { "title": tituloMeses[1], data: "MES2" },
                    { "title": tituloMeses[2], data: "MES3" },
                    { "title": tituloMeses[3], data: "MES4" },
                    { "title": tituloMeses[4], data: "MES5" },
                    { "title": tituloMeses[5], data: "MES6" },
                    { "title": tituloMeses[6], data: "MES7" },
                    { "title": tituloMeses[7], data: "MES8" },
                    { "title": tituloMeses[8], data: "MES9" },
                    { "title": tituloMeses[9], data: "MES10" },
                    { "title": tituloMeses[10], data: "MES11" },
                    { "title": tituloMeses[11], data: "MES12" },
                    { data: "Total" }
                ],

                "paging": false,
                "info": false

            });


        }

        function GetPeriodoMeses(tipo) {


            var periodo = cboPeriodo.val();
            var MesInicio = tbMesesInicio.val();
            var months = ["ENE", "FEB", "MAR", "ABR", "MAY", "JUN",
                          "JUL", "AGO", "SEP", "OCT", "NOV", "DIC"];
            var tituloMeses = [];
            var ListoMonthsID = [];

            var count = 0;
            for (var i = MesInicio; i < 12; i++) {
                count++;
                //   $("#lblFecha" + count).text(months[i] + " " + periodo);
                tituloMeses.push(months[i] + " " + periodo);
                ListoMonthsID.push(i);
            }

            for (var i = 0 ; i < MesInicio; i++) {
                //  $("#lblFecha" + count).text(months[i] + " " + periodo);
                tituloMeses.push(months[i] + " " + (Number(periodo) + 1));
                ListoMonthsID.push(i);
            }

            if (tipo == 2) {
                return ListoMonthsID;
            }
            else {
                return tituloMeses;
            }


        }

        function getLiga(value) {

            switch (value) {
                case "1":
                    return '';
                    break;
                case "2":
                    return '';
                    break;
                case "3":
                    return '';
                    break;
                case "4":
                    return '';
                    break;
                case "5":
                    idTituloContainer.text('Captura de Obra');
                    return '/Proyecciones/CapturaDeObras';
                    break;
                case "6":
                    idTituloContainer.text('CxC');
                    return '/Proyecciones/CxC';
                    break;
                case "7":
                    return '/Proyecciones/GastosAdministacionyVentas';
                    break;
                case "8":
                    return '/Proyecciones/ActivoFijo';
                    break;
                case "9":
                    return '/Proyecciones/PagosDiversos';
                    break;
                case "10":
                    return '/Proyecciones/IngresosDiversos';
                    break;
                case "11":
                    idTituloContainer.text('Estado De Posición Financiera (Captura De Datos Balance Inicial)');
                    return '/Proyecciones/EstadoPosicionFinanciera';
                    break;
                case "12":
                    return '/Proyecciones/Premisas';
                    break;
                default:
                    return '';
                    break;

            }
        }

        function VerReporte() {

            var btnValue = $(this).val();
            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = btnValue;
            var Escenario = cboEscenario.val();
            var Divisor = tbDivisor.val();
            var meses = tbMesesInicio.val();
            var anio = cboPeriodo.val();

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&Escenario=" + Escenario + "&Divisor=" + Divisor + "&meses=" + meses + "&anio=" + anio;

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        var canvas = document.querySelector("#LineWithLine");

        var X, Y, W, H, r;
        function inicializarCanvas() {
            if (canvas && canvas.getContext) {
                var ctx = canvas.getContext("2d");
                if (ctx) {
                    var s = getComputedStyle(canvas);
                    var w = s.width;
                    var h = s.height;

                    W = canvas.width = w.split("px")[0];
                    H = canvas.height = h.split("px")[0];
                }
            }
        }

        function LoadTabla1(UtilidadNeta) {

            var json1 = {}

            json1.MES1 = 0;
            json1.MES2 = 0;
            json1.MES3 = 0;
            json1.MES4 = 0;
            json1.MES5 = 0;
            json1.MES6 = 0;
            json1.MES7 = 0;

            json1.MES8 = 0;
            json1.MES9 = 0;
            json1.MES10 = 0;
            json1.MES11 = 0;
            json1.MES12 = 0;
            json1.Total = 0;

            var json0 = {}
            json0.MES1 = UtilidadNeta.Fecha1;
            json0.MES2 = UtilidadNeta.Fecha2;
            json0.MES3 = UtilidadNeta.Fecha3;
            json0.MES4 = UtilidadNeta.Fecha4;
            json0.MES5 = UtilidadNeta.Fecha5;
            json0.MES6 = UtilidadNeta.Fecha6;
            json0.MES7 = UtilidadNeta.Fecha7;

            json0.MES8 = UtilidadNeta.Fecha8;
            json0.MES9 = UtilidadNeta.Fecha9;
            json0.MES10 = UtilidadNeta.Fecha10;
            json0.MES11 = UtilidadNeta.Fecha11;
            json0.MES12 = UtilidadNeta.Fecha12;
            json0.Total = UtilidadNeta.Total;


            var dataSet = [];
            var dataSet2 = [];
            dataSet.push(json0);
            dataSet.push(json1);
            dataSet.push(json1);
            dataSet2.push(json1);
            var result = [];
            if (dataSet.length == 3) {
                for (var i = 0; i < dataSet.length; i++) {
                    var json = {}
                    listaIdMes = GetPeriodoMeses(2);
                    switch (i) {
                        case 0:
                            {
                                json.MES1 = "<input type='number' class='form-control CMES1PPL inputChange' data-mes='" + listaIdMes[0] + "' value='" + dataSet[i].MES1.toFixed(2) + "' >";
                                json.MES2 = "<input type='number' class='form-control CMES2PPL inputChange' data-mes='" + listaIdMes[1] + "' value='" + dataSet[i].MES2.toFixed(2) + "' >";
                                json.MES3 = "<input type='number' class='form-control CMES3PPL inputChange' data-mes='" + listaIdMes[2] + "' value='" + dataSet[i].MES3.toFixed(2) + "' >";
                                json.MES4 = "<input type='number' class='form-control CMES4PPL inputChange' data-mes='" + listaIdMes[3] + "' value='" + dataSet[i].MES4.toFixed(2) + "' >";
                                json.MES5 = "<input type='number' class='form-control CMES5PPL inputChange' data-mes='" + listaIdMes[4] + "' value='" + dataSet[i].MES5.toFixed(2) + "' >";
                                json.MES6 = "<input type='number' class='form-control CMES6PPL inputChange' data-mes='" + listaIdMes[5] + "' value='" + dataSet[i].MES6.toFixed(2) + "' >";
                                json.MES7 = "<input type='number' class='form-control CMES7PPL inputChange' data-mes='" + listaIdMes[6] + "' value='" + dataSet[i].MES7.toFixed(2) + "' >";
                                json.MES8 = "<input type='number' class='form-control CMES8PPL inputChange' data-mes='" + listaIdMes[7] + "' value='" + dataSet[i].MES8.toFixed(2) + "' >";

                                json.MES9 = "<input type='number' class='form-control CMES9PPL inputChange' data-mes='" + listaIdMes[8] + "' value='" + dataSet[i].MES9.toFixed(2) + "' >";
                                json.MES10 = "<input type='number' class='form-control CMES10PPL inputChange' data-mes='" + listaIdMes[9] + "' value='" + dataSet[i].MES10.toFixed(2) + "' >";
                                json.MES11 = "<input type='number' class='form-control CMES11PPL inputChange' data-mes='" + listaIdMes[10] + "' value='" + dataSet[i].MES11.toFixed(2) + "' >";
                                json.MES12 = "<input type='number' class='form-control CMES12PPL inputChange' data-mes='" + listaIdMes[11] + "' value='" + dataSet[i].MES12.toFixed(2) + "' >";
                                json.Total = "<input type='number' class='form-control CTotalPPL inputChange'  value='" + dataSet[i].Total + "' >";

                                result.push(json);
                                break;
                            }
                        case 1:
                            {
                                json.MES1 = "<input type='number' class='form-control CMES1SumPPL inputChange' data-mes='" + listaIdMes[0] + "'  value='" + dataSet[i].MES1.toFixed(2) + "' >";
                                json.MES2 = "<input type='number' class='form-control CMES2SumPPL inputChange' data-mes='" + listaIdMes[1] + "'' value='" + dataSet[i].MES2.toFixed(2) + "' >";
                                json.MES3 = "<input type='number' class='form-control CMES3SumPPL inputChange' data-mes='" + listaIdMes[2] + "'' value='" + dataSet[i].MES3.toFixed(2) + "' >";
                                json.MES4 = "<input type='number' class='form-control CMES4SumPPL inputChange' data-mes='" + listaIdMes[3] + "'  value='" + dataSet[i].MES4.toFixed(2) + "' >";
                                json.MES5 = "<input type='number' class='form-control CMES5SumPPL inputChange' data-mes='" + listaIdMes[4] + "'  value='" + dataSet[i].MES5.toFixed(2) + "' >";
                                json.MES6 = "<input type='number' class='form-control CMES6SumPPL inputChange' data-mes='" + listaIdMes[5] + "'' value='" + dataSet[i].MES6.toFixed(2) + "' >";
                                json.MES7 = "<input type='number' class='form-control CMES7SumPPL inputChange' data-mes='" + listaIdMes[6] + "'  value='" + dataSet[i].MES7.toFixed(2) + "' >";
                                json.MES8 = "<input type='number' class='form-control CMES8SumPPL inputChange' data-mes='" + listaIdMes[7] + "'  value='" + dataSet[i].MES8.toFixed(2) + "' >";
                                json.MES9 = "<input type='number' class='form-control CMES9SumPPL inputChange' data-mes='" + listaIdMes[8] + "'   value='" + dataSet[i].MES9.toFixed(2) + "' >";
                                json.MES10 = "<input type='number' class='form-control CMES10SumPPL inputChange' data-mes='" + listaIdMes[9] + "' value='" + dataSet[i].MES10.toFixed(2) + "' >";
                                json.MES11 = "<input type='number' class='form-control CMES11SumPPL inputChange' data-mes='" + listaIdMes[10] + "' value='" + dataSet[i].MES11.toFixed(2) + "' >";
                                json.MES12 = "<input type='number' class='form-control CMES12SumPPL inputChange' data-mes='" + listaIdMes[11] + "' value='" + dataSet[i].MES12.toFixed(2) + "' >";

                                json.Total = "";

                                result.push(json);
                                break;
                            }
                        case 2:
                            {
                                total = sumaLinea(dataSet[i]);
                                json.MES1 = "<input type='number' class='form-control CMES1SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES2 = "<input type='number' class='form-control CMES2SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES3 = "<input type='number' class='form-control CMES3SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES4 = "<input type='number' class='form-control CMES4SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES5 = "<input type='number' class='form-control CMES5SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES6 = "<input type='number' class='form-control CMES6SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES7 = "<input type='number' class='form-control CMES7SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES8 = "<input type='number' class='form-control CMES8SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES9 = "<input type='number' class='form-control CMES9SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES10 = "<input type='number' class='form-control CMES10SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES11 = "<input type='number' class='form-control CMES11SumPPPL inputChange' value='" + 0 + "%' >";
                                json.MES12 = "<input type='number' class='form-control CMES12SumPPPL inputChange' value='" + 0 + "%' >";

                                json.Total = "'  <=% MENSUAL " + total;

                                result.push(json);
                                break;
                            }
                        default:

                    }
                }

                setInfoTable1(result);
            }
            Operaciones();
        }

        function LoadTabla2(dataSet) {
            var result = [];
            var json = {};
            json.MES1 = "<input type='number' class='form-control CMES1FE inputChange' value='" + dataSet.Fecha1.toFixed(2) + "' >";
            json.MES2 = "<input type='number' class='form-control CMES2FE inputChange' value='" + dataSet.Fecha2.toFixed(2) + "' >";
            json.MES3 = "<input type='number' class='form-control CMES3FE inputChange' value='" + dataSet.Fecha3.toFixed(2) + "' >";
            json.MES4 = "<input type='number' class='form-control CMES4FE inputChange' value='" + dataSet.Fecha4.toFixed(2) + "' >";
            json.MES5 = "<input type='number' class='form-control CMES5FE inputChange' value='" + dataSet.Fecha5.toFixed(2) + "' >";
            json.MES6 = "<input type='number' class='form-control CMES6FE inputChange' value='" + dataSet.Fecha6.toFixed(2) + "' >";
            json.MES7 = "<input type='number' class='form-control CMES7FE inputChange' value='" + dataSet.Fecha7.toFixed(2) + "' >";
            json.MES8 = "<input type='number' class='form-control CMES8FE inputChange' value='" + dataSet.Fecha8.toFixed(2) + "' >";

            json.MES9 = "<input type='number' class='form-control CMES9FE inputChange' value='" + dataSet.Fecha9.toFixed(2) + "' >";
            json.MES10 = "<input type='number' class='form-control CMES10FE inputChange' value='" + dataSet.Fecha10.toFixed(2) + "' >";
            json.MES11 = "<input type='number' class='form-control CMES11FE inputChange' value='" + dataSet.Fecha11.toFixed(2) + "' >";
            json.MES12 = "<input type='number' class='form-control CMES12FE inputChange' value='" + dataSet.Fecha12.toFixed(2) + "' >";
            json.Total = "&nbsp;";
            result.push(json);

            SetTable(result);
        }

        function LoadTabla3(dataSet) {
            var result = [];
            var resSuma = sumaLinea(0);
            var json = {}
            json.MES1 = "<input type='number' class='form-control CMES1VM inputChange' value='" + dataSet.Fecha1.toFixed(2) + "' >";
            json.MES2 = "<input type='number' class='form-control CMES2VM inputChange' value='" + dataSet.Fecha2.toFixed(2) + "' >";
            json.MES3 = "<input type='number' class='form-control CMES3VM inputChange' value='" + dataSet.Fecha3.toFixed(2) + "' >";
            json.MES4 = "<input type='number' class='form-control CMES4VM inputChange' value='" + dataSet.Fecha4.toFixed(2) + "' >";
            json.MES5 = "<input type='number' class='form-control CMES5VM inputChange' value='" + dataSet.Fecha5.toFixed(2) + "' >";
            json.MES6 = "<input type='number' class='form-control CMES6VM inputChange' value='" + dataSet.Fecha6.toFixed(2) + "' >";
            json.MES7 = "<input type='number' class='form-control CMES7VM inputChange' value='" + dataSet.Fecha7.toFixed(2) + "' >";
            json.MES8 = "<input type='number' class='form-control CMES8VM inputChange' value='" + dataSet.Fecha8.toFixed(2) + "' >";

            json.MES9 = "<input type='number' class='form-control CMES9VM inputChange' value='" + dataSet.Fecha9.toFixed(2) + "' >";
            json.MES10 = "<input type='number' class='form-control CMES10VM inputChange' value='" + dataSet.Fecha10.toFixed(2) + "' >";
            json.MES11 = "<input type='number' class='form-control CMES11VM inputChange' value='" + dataSet.Fecha11.toFixed(2) + "' >";
            json.MES12 = "<input type='number' class='form-control CMES12VM inputChange' value='" + dataSet.Fecha12.toFixed(2) + "' >";
            json.Total = "<input type='number' class='form-control CTotalVM inputChange' value='" + resSuma + "' >";;

            result.push(json);

            var json2 = {}
            json2.MES1 = "&nbsp;";
            json2.MES2 = "&nbsp;";
            json2.MES3 = "&nbsp;";
            json2.MES4 = "&nbsp;";
            json2.MES5 = "&nbsp;";
            json2.MES6 = "&nbsp;";
            json2.MES7 = "&nbsp;";
            json2.MES8 = "&nbsp;";

            json2.MES9 = "&nbsp;";
            json2.MES10 = "&nbsp;";
            json2.MES11 = "Promedio";
            json2.MES12 = "Mensual";
            json2.Total = "<input type='number' class='form-control CTotalVMDiv inputChange' value='" + resSuma / 12 + "' >";
            result.push(json2);

            SetTable2(result);
            // LoadTabla4(dataSet);
        }
        function LoadTabla4(dataSet) {
            var result = [];
            var json = {}
            json.MES1 = "<input type='number' class='form-control CMES1UBP noEditable' value='" + dataSet.Fecha1.toFixed(2) + "' >";
            json.MES2 = "<input type='number' class='form-control CMES2UBP noEditable' value='" + dataSet.Fecha2.toFixed(2) + "' >";
            json.MES3 = "<input type='number' class='form-control CMES3UBP noEditable' value='" + dataSet.Fecha3.toFixed(2) + "' >";
            json.MES4 = "<input type='number' class='form-control CMES4UBP noEditable' value='" + dataSet.Fecha4.toFixed(2) + "' >";
            json.MES5 = "<input type='number' class='form-control CMES5UBP noEditable' value='" + dataSet.Fecha5.toFixed(2) + "' >";
            json.MES6 = "<input type='number' class='form-control CMES6UBP noEditable' value='" + dataSet.Fecha6.toFixed(2) + "' >";
            json.MES7 = "<input type='number' class='form-control CMES7UBP noEditable' value='" + dataSet.Fecha7.toFixed(2) + "' >";
            json.MES8 = "<input type='number' class='form-control CMES8UBP noEditable' value='" + dataSet.Fecha8.toFixed(2) + "' >";

            json.MES9 = "<input type='number' class='form-control CMES9UBP noEditable' value='" + dataSet.Fecha9.toFixed(2) + "' >";
            json.MES10 = "<input type='number' class='form-control CMES10UBP noEditable' value='" + dataSet.Fecha10.toFixed(2) + "' >";
            json.MES11 = "<input type='number' class='form-control CMES11UBP noEditable' value='" + dataSet.Fecha11.toFixed(2) + "' >";
            json.MES12 = "<input type='number' class='form-control CMES12UBP noEditable' value='" + dataSet.Fecha12.toFixed(2) + "' >";
            json.Total = "&nbsp;";
            result.push(json);

            SetTable3(result);
            //LoadTabla5(dataSet);
        }
        function LoadTabla5(dataSet) {
            var result = [];
            var json = {}
            var resSuma = sumaLinea(dataSet);
            json.MES1 = "<input type='number' class='form-control CMES1GAA noEditable' value='" + dataSet.Fecha1.toFixed(2) + "' >";
            json.MES2 = "<input type='number' class='form-control CMES2GAA noEditable' value='" + dataSet.Fecha2.toFixed(2) + "' >";
            json.MES3 = "<input type='number' class='form-control CMES3GAA noEditable' value='" + dataSet.Fecha3.toFixed(2) + "' >";
            json.MES4 = "<input type='number' class='form-control CMES4GAA noEditable' value='" + dataSet.Fecha4.toFixed(2) + "' >";
            json.MES5 = "<input type='number' class='form-control CMES5GAA noEditable' value='" + dataSet.Fecha5.toFixed(2) + "' >";
            json.MES6 = "<input type='number' class='form-control CMES6GAA noEditable' value='" + dataSet.Fecha6.toFixed(2) + "' >";
            json.MES7 = "<input type='number' class='form-control CMES7GAA noEditable' value='" + dataSet.Fecha7.toFixed(2) + "' >";
            json.MES8 = "<input type='number' class='form-control CMES8GAA noEditable' value='" + dataSet.Fecha8.toFixed(2) + "' >";

            json.MES9 = "<input type='number' class='form-control CMES9GAA noEditable' value='" + dataSet.Fecha9.toFixed(2) + "' >";
            json.MES10 = "<input type='number' class='form-control CMES10GAA noEditable' value='" + dataSet.Fecha10.toFixed(2) + "' >";
            json.MES11 = "<input type='number' class='form-control CMES11GAA noEditable' value='" + dataSet.Fecha11.toFixed(2) + "' >";
            json.MES12 = "<input type='number' class='form-control CMES12GAA noEditable' value='" + dataSet.Fecha12.toFixed(2) + "' >";
            json.Total = "<input type='number' class='form-control CTotalGAA noEditable' value='" + resSuma + "' >";
            result.push(json);

            SetTable4(result);
            //   LoadTabla6(dataSet);
        }
        function LoadTabla6(dataSet) {
            var result = [];
            var json = {}
            json.MES1 = "<input type='number' class='form-control CMES1VAM noEditable' value='" + dataSet.Fecha1.toFixed(2) + "' >";
            json.MES2 = "<input type='number' class='form-control CMES2VAM noEditable' value='" + dataSet.Fecha2.toFixed(2) + "' >";
            json.MES3 = "<input type='number' class='form-control CMES3VAM noEditable' value='" + dataSet.Fecha3.toFixed(2) + "' >";
            json.MES4 = "<input type='number' class='form-control CMES4VAM noEditable' value='" + dataSet.Fecha4.toFixed(2) + "' >";
            json.MES5 = "<input type='number' class='form-control CMES5VAM noEditable' value='" + dataSet.Fecha5.toFixed(2) + "' >";
            json.MES6 = "<input type='number' class='form-control CMES6VAM noEditable' value='" + dataSet.Fecha6.toFixed(2) + "' >";
            json.MES7 = "<input type='number' class='form-control CMES7VAM noEditable' value='" + dataSet.Fecha7.toFixed(2) + "' >";
            json.MES8 = "<input type='number' class='form-control CMES8VAM noEditable' value='" + dataSet.Fecha8.toFixed(2) + "' >";

            json.MES9 = "<input type='number' class='form-control CMES9VAM noEditable' value='" + dataSet.Fecha9.toFixed(2) + "' >";
            json.MES10 = "<input type='number' class='form-control CMES10VAM noEditable' value='" + dataSet.Fecha10.toFixed(2) + "' >";
            json.MES11 = "<input type='number' class='form-control CMES11VAM noEditable' value='" + dataSet.Fecha11.toFixed(2) + "' >";
            json.MES12 = "<input type='number' class='form-control CMES12VAM noEditable' value='" + dataSet.Fecha12.toFixed(2) + "' >";
            json.Total = "&nbsp;";
            result.push(json);

            SetTable5(result);
            //LoadTabla7(dataSet);
        }
        function LoadTabla7(dataSet) {
            var result = [];
            var json = {}
            json.MES1 = "<input type='number' class='form-control CMES1RAM noEditable' value='" + dataSet.Fecha1.toFixed(2) + "' >";
            json.MES2 = "<input type='number' class='form-control CMES2RAM noEditable' value='" + dataSet.Fecha2.toFixed(2) + "' >";
            json.MES3 = "<input type='number' class='form-control CMES3RAM noEditable' value='" + dataSet.Fecha3.toFixed(2) + "' >";
            json.MES4 = "<input type='number' class='form-control CMES4RAM noEditable' value='" + dataSet.Fecha4.toFixed(2) + "' >";
            json.MES5 = "<input type='number' class='form-control CMES5RAM noEditable' value='" + dataSet.Fecha5.toFixed(2) + "' >";
            json.MES6 = "<input type='number' class='form-control CMES6RAM noEditable' value='" + dataSet.Fecha6.toFixed(2) + "' >";
            json.MES7 = "<input type='number' class='form-control CMES7RAM noEditable' value='" + dataSet.Fecha7.toFixed(2) + "' >";
            json.MES8 = "<input type='number' class='form-control CMES8RAM noEditable' value='" + dataSet.Fecha8.toFixed(2) + "' >";

            json.MES9 = "<input type='number' class='form-control CMES9RAM noEditable' value='" + dataSet.Fecha9.toFixed(2) + "' >";
            json.MES10 = "<input type='number' class='form-control CMES10RAM noEditable' value='" + dataSet.Fecha10.toFixed(2) + "' >";
            json.MES11 = "<input type='number' class='form-control CMES11RAM noEditable' value='" + dataSet.Fecha11.toFixed(2) + "' >";
            json.MES12 = "<input type='number' class='form-control CMES12RAM noEditable' value='" + dataSet.Fecha12.toFixed(2) + "' >";
            json.Total = "&nbsp;";
            result.push(json);

            SetTable6(result);
            //LoadTabla8(dataSet);
        }
        function LoadTabla8(dataSet) {
            var result = [];
            var json = {}
            json.MES1 = "<input type='number' class='form-control CMES1MRA noEditable' value='" + dataSet.Fecha1.toFixed(2) + "' >";
            json.MES2 = "<input type='number' class='form-control CMES2MRA noEditable' value='" + dataSet.Fecha2.toFixed(2) + "' >";
            json.MES3 = "<input type='number' class='form-control CMES3MRA noEditable' value='" + dataSet.Fecha3.toFixed(2) + "' >";
            json.MES4 = "<input type='number' class='form-control CMES4MRA noEditable' value='" + dataSet.Fecha4.toFixed(2) + "' >";
            json.MES5 = "<input type='number' class='form-control CMES5MRA noEditable' value='" + dataSet.Fecha5.toFixed(2) + "' >";
            json.MES6 = "<input type='number' class='form-control CMES6MRA noEditable' value='" + dataSet.Fecha6.toFixed(2) + "' >";
            json.MES7 = "<input type='number' class='form-control CMES7MRA noEditable' value='" + dataSet.Fecha7.toFixed(2) + "' >";
            json.MES8 = "<input type='number' class='form-control CMES8MRA noEditable' value='" + dataSet.Fecha8.toFixed(2) + "' >";

            json.MES9 = "<input type='number' class='form-control CMES9MRA noEditable' value='" + dataSet.Fecha9.toFixed(2) + "' >";
            json.MES10 = "<input type='number' class='form-control CMES10MRA noEditable' value='" + dataSet.Fecha10.toFixed(2) + "' >";
            json.MES11 = "<input type='number' class='form-control CMES11MRA noEditable' value='" + dataSet.Fecha11.toFixed(2) + "' >";
            json.MES12 = "<input type='number' class='form-control CMES12MRA noEditable' value='" + dataSet.Fecha12.toFixed(2) + "' >";
            json.Total = "&nbsp;";
            result.push(json);

            SetTable7(result);

            $('.inputChange').prop('disabled', true);
            $('.noEditable').prop('disabled', true);
            // LoadTabla7(dataSet);
        }

        function sumaLinea(dataSet) {
            var suma = dataSet.Fecha1 + dataSet.Fecha2 + dataSet.Fecha3 + dataSet.Fecha4 + dataSet.Fecha5 + dataSet.Fecha6 + dataSet.Fecha7 + dataSet.Fecha8 + dataSet.Fecha9 + dataSet.Fecha10 + dataSet.Fecha11 + dataSet.Fecha12;
            if (suma != 0) {
                suma / 12;
            }
            else {
                suma = 0;
            }
            return suma;
        }

        /**/
        function setInfoTable1(dataSet) {

            var tituloMeses = [];
            var date = new Date();

            tituloMeses = GetPeriodoMeses(1);

            tabla = $('#tblResultadoMensual').DataTable({
                "bFilter": false,
                destroy: true,

                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [

                    { "title": tituloMeses[0], data: "MES1", "width": "90px" },
                    { "title": tituloMeses[1], data: "MES2", "width": "90px" },
                    { "title": tituloMeses[2], data: "MES3", "width": "90px" },
                    { "title": tituloMeses[3], data: "MES4", "width": "90px" },
                    { "title": tituloMeses[4], data: "MES5", "width": "90px" },
                    { "title": tituloMeses[5], data: "MES6", "width": "90px" },
                    { "title": tituloMeses[6], data: "MES7", "width": "90px" },
                    { "title": tituloMeses[7], data: "MES8", "width": "90px" },
                    { "title": tituloMeses[8], data: "MES9", "width": "90px" },
                    { "title": tituloMeses[9], data: "MES10", "width": "90px" },
                    { "title": tituloMeses[10], data: "MES11", "width": "90px" },
                    { "title": tituloMeses[11], data: "MES12", "width": "90px" },
                    { data: "Total", "width": "90px" },
                ],

                "paging": false,
                "info": false

            });

            tabla.on('change', '.inputChange', function () {
                //s elemento = $(this);
                Operaciones();
                //  sumar(elemento, previous);
            });;

        }
        function SetTable(dataSet) {

            var tituloMeses = [];
            var date = new Date();

            tabla2.clear().draw();
            tabla2.destroy();

            tituloMeses = GetPeriodoMeses(1);

            tabla2 = $('#tblFlujoEfectivo').DataTable({
                "bFilter": false,
                destroy: true,

                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: "MES1", "width": "90px" },
                    { data: "MES2", "width": "90px" },
                    { data: "MES3", "width": "90px" },
                    { data: "MES4", "width": "90px" },
                    { data: "MES5", "width": "90px" },
                    { data: "MES6", "width": "90px" },
                    { data: "MES7", "width": "90px" },
                    { data: "MES8", "width": "90px" },
                    { data: "MES9", "width": "90px" },
                    { data: "MES10", "width": "90px" },
                    { data: "MES11", "width": "90px" },
                    { data: "MES12", "width": "90px" },
                    { data: "Total", "width": "90px" },
                ],

                "paging": false,
                "info": false
            });
        }
        function SetTable2(dataSet) {

            var tituloMeses = [];
            var date = new Date();

            tabla3.clear().draw();
            tabla3.destroy();

            tituloMeses = GetPeriodoMeses(1);

            tabla3 = $('#tblVentasMensuales').DataTable({
                "bFilter": false,
                destroy: true,

                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: "MES1", "width": "90px" },
                    { data: "MES2", "width": "90px" },
                    { data: "MES3", "width": "90px" },
                    { data: "MES4", "width": "90px" },
                    { data: "MES5", "width": "90px" },
                    { data: "MES6", "width": "90px" },
                    { data: "MES7", "width": "90px" },
                    { data: "MES8", "width": "90px" },
                    { data: "MES9", "width": "90px" },
                    { data: "MES10", "width": "90px" },
                    { data: "MES11", "width": "90px" },
                    { data: "MES12", "width": "90px" },
                    { data: "Total", "width": "90px" },
                ],

                "paging": false,
                "info": false
            });


            tabla3.on('change', '.inputChange', function () {
                //s elemento = $(this);
                OperacionesVentas();
                //  sumar(elemento, previous);
            });;

            OperacionResultadosMensualPor();
        }
        function SetTable3(dataSet) {

            var tituloMeses = [];
            var date = new Date();

            tabla4.clear().draw();
            tabla4.destroy();

            tituloMeses = GetPeriodoMeses(1);

            tabla4 = $('#tblUtilidadBruta').DataTable({
                "bFilter": false,
                destroy: true,

                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: "MES1", "width": "90px" },
                    { data: "MES2", "width": "90px" },
                    { data: "MES3", "width": "90px" },
                    { data: "MES4", "width": "90px" },
                    { data: "MES5", "width": "90px" },
                    { data: "MES6", "width": "90px" },
                    { data: "MES7", "width": "90px" },
                    { data: "MES8", "width": "90px" },
                    { data: "MES9", "width": "90px" },
                    { data: "MES10", "width": "90px" },
                    { data: "MES11", "width": "90px" },
                    { data: "MES12", "width": "90px" },
                    { data: "Total", "width": "90px" },
                ],

                "paging": false,
                "info": false
            });


        }
        function SetTable4(dataSet) {

            var tituloMeses = [];
            var date = new Date();

            tabla5.clear().draw();
            tabla5.destroy();

            tituloMeses = GetPeriodoMeses(1);

            tabla5 = $('#tblGastosAdmo').DataTable({
                "bFilter": false,
                destroy: true,
                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: "MES1", "width": "90px" },
                    { data: "MES2", "width": "90px" },
                    { data: "MES3", "width": "90px" },
                    { data: "MES4", "width": "90px" },
                    { data: "MES5", "width": "90px" },
                    { data: "MES6", "width": "90px" },
                    { data: "MES7", "width": "90px" },
                    { data: "MES8", "width": "90px" },
                    { data: "MES9", "width": "90px" },
                    { data: "MES10", "width": "90px" },
                    { data: "MES11", "width": "90px" },
                    { data: "MES12", "width": "90px" },
                    { data: "Total", "width": "90px" },
                ],

                "paging": false,
                "info": false
            });

            tabla5.on('change', '.inputChange', function () {
                //s elemento = $(this);
                OperacionesGastosADMO();
                //  sumar(elemento, previous);
            });;


        }
        function SetTable5(dataSet) {

            var tituloMeses = [];
            var date = new Date();

            tabla6.clear().draw();
            tabla6.destroy();

            tituloMeses = GetPeriodoMeses(1);

            tabla6 = $('#tblVentaAcumulada').DataTable({
                "bFilter": false,
                destroy: true,

                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: "MES1", "width": "90px" },
                    { data: "MES2", "width": "90px" },
                    { data: "MES3", "width": "90px" },
                    { data: "MES4", "width": "90px" },
                    { data: "MES5", "width": "90px" },
                    { data: "MES6", "width": "90px" },
                    { data: "MES7", "width": "90px" },
                    { data: "MES8", "width": "90px" },
                    { data: "MES9", "width": "90px" },
                    { data: "MES10", "width": "90px" },
                    { data: "MES11", "width": "90px" },
                    { data: "MES12", "width": "90px" },
                    { data: "Total", "width": "90px" }
                ],

                "paging": false,
                "info": false
            });
            OperacionesVentas();
        }
        function SetTable6(dataSet) {

            var tituloMeses = [];
            var date = new Date();

            tabla7.clear().draw();
            tabla7.destroy();

            tituloMeses = GetPeriodoMeses(1);

            tabla7 = $('#tblResultadoACumulado').DataTable({
                "bFilter": false,
                destroy: true,

                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: "MES1", "width": "90px" },
                    { data: "MES2", "width": "90px" },
                    { data: "MES3", "width": "90px" },
                    { data: "MES4", "width": "90px" },
                    { data: "MES5", "width": "90px" },
                    { data: "MES6", "width": "90px" },
                    { data: "MES7", "width": "90px" },
                    { data: "MES8", "width": "90px" },
                    { data: "MES9", "width": "90px" },
                    { data: "MES10", "width": "90px" },
                    { data: "MES11", "width": "90px" },
                    { data: "MES12", "width": "90px" },
                    { data: "Total", "width": "90px" }
                ],

                "paging": false,
                "info": false
            });
            OperacionesResultadoAcumulado();
        }
        function SetTable7(dataSet) {

            var tituloMeses = [];
            var date = new Date();

            tabla8.clear().draw();
            tabla8.destroy();

            tituloMeses = GetPeriodoMeses(1);

            tabla8 = $('#tblMargenResultadoAcumulado').DataTable({
                "bFilter": false,
                destroy: true,

                scrollY: '50vh',
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { data: "MES1", "width": "90px" },
                    { data: "MES2", "width": "90px" },
                    { data: "MES3", "width": "90px" },
                    { data: "MES4", "width": "90px" },
                    { data: "MES5", "width": "90px" },
                    { data: "MES6", "width": "90px" },
                    { data: "MES7", "width": "90px" },
                    { data: "MES8", "width": "90px" },
                    { data: "MES9", "width": "90px" },
                    { data: "MES10", "width": "90px" },
                    { data: "MES11", "width": "90px" },
                    { data: "MES12", "width": "90px" },
                    { data: "Total", "width": "90px" }
                ],

                "paging": false,
                "info": false
            });

            OperacionesMargenSobreResultado();
        }
        function Operaciones() {


            $('.CMES1SumPPL').val($('.CMES1PPL').val().toFixed(2));
            $('.CMES2SumPPL').val($('.CMES2SumPPL').attr('data-mes') == "0" ? Number($('.CMES2PPL').val()) : (Number($('.CMES1SumPPL').val()) + Number($('.CMES2PPL').val())).toFixed(2));
            $('.CMES3SumPPL').val($('.CMES3SumPPL').attr('data-mes') == "0" ? Number($('.CMES3PPL').val()) : (Number($('.CMES2SumPPL').val()) + Number($('.CMES3PPL').val())).toFixed(2));
            $('.CMES4SumPPL').val($('.CMES4SumPPL').attr('data-mes') == "0" ? Number($('.CMES4PPL').val()) : (Number($('.CMES3SumPPL').val()) + Number($('.CMES4PPL').val())).toFixed(2));
            $('.CMES5SumPPL').val($('.CMES5SumPPL').attr('data-mes') == "0" ? Number($('.CMES5PPL').val()) : (Number($('.CMES4SumPPL').val()) + Number($('.CMES5PPL').val())).toFixed(2));
            $('.CMES6SumPPL').val($('.CMES6SumPPL').attr('data-mes') == "0" ? Number($('.CMES6PPL').val()) : (Number($('.CMES5SumPPL').val()) + Number($('.CMES6PPL').val())).toFixed(2));
            $('.CMES7SumPPL').val($('.CMES7SumPPL').attr('data-mes') == "0" ? Number($('.CMES7PPL').val()) : (Number($('.CMES6SumPPL').val()) + Number($('.CMES7PPL').val())).toFixed(2));
            $('.CMES8SumPPL').val($('.CMES8SumPPL').attr('data-mes') == "0" ? Number($('.CMES8PPL').val()) : (Number($('.CMES7SumPPL').val()) + Number($('.CMES8PPL').val())).toFixed(2));
            $('.CMES9SumPPL').val($('.CMES9SumPPL').attr('data-mes') == "0" ? Number($('.CMES9PPL').val()) : (Number($('.CMES8SumPPL').val()) + Number($('.CMES9PPL').val())).toFixed(2));
            $('.CMES10SumPPL').val($('.CMES10SumPPL').attr('data-mes') == "0" ? Number($('.CMES10PPL').val()) : (Number($('.CMES9SumPPL').val()) + Number($('.CMES10PPL').val())).toFixed(2));
            $('.CMES11SumPPL').val($('.CMES11SumPPL').attr('data-mes') == "0" ? Number($('.CMES11PPL').val()) : (Number($('.CMES10SumPPL').val()) + Number($('.CMES11PPL').val())).toFixed(2));
            $('.CMES12SumPPL').val($('.CMES12SumPPL').attr('data-mes') == "0" ? Number($('.CMES12PPL').val()) : (Number($('.CMES11SumPPL').val()) + Number($('.CMES12PPL').val())).toFixed(2));

            var sumaTotalRM = 0;

            for (var i = 1; i <= 12; i++) {
                sumaTotalRM += Number($('.CMES' + i + 'PPL').val());
            }
            $('.CTotalPPL').val(sumaTotalRM);

            OperacionesResultadoAcumulado();
            // OperacionesDivisionResultadoMensual();

        }
        function OperacionesVentas() {

            var suma = Number($('.CMES1VM').val()) + Number($('.CMES2VM').val()) + Number($('.CMES3VM').val()) + Number($('.CMES4VM').val()) + Number($('.CMES5VM').val()) + Number($('.CMES6VM').val()) + Number($('.CMES7VM').val()) + Number($('.CMES8VM').val()) + Number($('.CMES9VM').val()) + Number($('.CMES10VM').val()) + Number($('.CMES11VM').val()) + Number($('.CMES12VM').val());

            $('.CTotalVM').val(suma.toFixed(2));
            $('.CTotalVMDiv').val((suma / 12).toFixed(2));
            Operaciones();
            OperacionesVentaAcumuladaSCJ();

        }
        function OperacionesGastosADMO() {

            var suma = Number($('.CMES1GAA').val()) + Number($('.CMES2GAA').val()) + Number($('.CMES3GAA').val()) + Number($('.CMES10GAA').val()) + Number($('.CMES4GAA').val()) + Number($('.CMES5GAA').val()) + Number($('.CMES6GAA').val()) + Number($('.CMES7GAA').val()) + Number($('.CMES8GAA').val()) + Number($('.CMES9GAA').val()) + Number($('.CMES11GAA').val()) + Number($('.CMES12GAA').val());

            $('.CTotalGAA').val(suma.toFixed(2));
        }
        function OperacionesVentaAcumulada() {

            var suma = Number($('.CMES1GAA').val()) + Number($('.CMES2GAA').val()) + Number($('.CMES3GAA').val()) + Number($('.CMES10GAA').val()) + Number($('.CMES4GAA').val()) + Number($('.CMES5GAA').val()) + Number($('.CMES6GAA').val()) + Number($('.CMES7GAA').val()) + Number($('.CMES8GAA').val()) + Number($('.CMES9GAA').val()) + Number($('.CMES11GAA').val()) + Number($('.CMES12GAA').val());

            $('.CTotalGAA').val(suma.toFixed(2));
        }
        function OperacionesResultadoAcumulado() {
            $('.CMES1RAM').val(regresarNumero(('.CMES1PPL')));

            $('.CMES2RAM').val(regresarSuma('.CMES2PPL', '.CMES1RAM'));
            $('.CMES3RAM').val(regresarSuma('.CMES3PPL', '.CMES2RAM'));
            $('.CMES4RAM').val(regresarSuma('.CMES4PPL', '.CMES3RAM'));
            $('.CMES5RAM').val(regresarSuma('.CMES5PPL', '.CMES4RAM'));
            $('.CMES6RAM').val(regresarSuma('.CMES6PPL', '.CMES5RAM'));
            $('.CMES7RAM').val(regresarSuma('.CMES7PPL', '.CMES6RAM'));
            $('.CMES8RAM').val(regresarSuma('.CMES8PPL', '.CMES7RAM'));
            $('.CMES9RAM').val(regresarSuma('.CMES9PPL', '.CMES8RAM'));
            $('.CMES10RAM').val(regresarSuma('.CMES10PPL', '.CMES9RAM'));
            $('.CMES11RAM').val(regresarSuma('.CMES11PPL', '.CMES10RAM'));
            $('.CMES12RAM').val(regresarSuma('.CMES12PPL', '.CMES11RAM'));
        }
        function OperacionesVentaAcumuladaSCJ() {
            $('.CMES1VAM').val(regresarNumero(('.CMES1VM')));

            $('.CMES2VAM').val(regresarSuma('.CMES2VM', '.CMES1VAM'));
            $('.CMES3VAM').val(regresarSuma('.CMES3VM', '.CMES2VAM'));
            $('.CMES4VAM').val(regresarSuma('.CMES4VM', '.CMES3VAM'));
            $('.CMES5VAM').val(regresarSuma('.CMES5VM', '.CMES4VAM'));
            $('.CMES6VAM').val(regresarSuma('.CMES6VM', '.CMES5VAM'));
            $('.CMES7VAM').val(regresarSuma('.CMES7VM', '.CMES6VAM'));
            $('.CMES8VAM').val(regresarSuma('.CMES8VM', '.CMES7VAM'));
            $('.CMES9VAM').val(regresarSuma('.CMES9VM', '.CMES8VAM'));
            $('.CMES10VAM').val(regresarSuma('.CMES10VM', '.CMES9VAM'));
            $('.CMES11VAM').val(regresarSuma('.CMES11VM', '.CMES10VAM'));
            $('.CMES12VAM').val(regresarSuma('.CMES12VM', '.CMES11VAM'));
        }
        function OperacionesResultadoAcumulado() {

            $('.CMES1RAM').val(regresarNumero(('.CMES1PPL')));
            $('.CMES2RAM').val(regresarSuma('.CMES2PPL', '.CMES1RAM'));
            $('.CMES3RAM').val(regresarSuma('.CMES3PPL', '.CMES2RAM'));
            $('.CMES4RAM').val(regresarSuma('.CMES4PPL', '.CMES3RAM'));
            $('.CMES5RAM').val(regresarSuma('.CMES5PPL', '.CMES4RAM'));
            $('.CMES6RAM').val(regresarSuma('.CMES6PPL', '.CMES5RAM'));
            $('.CMES7RAM').val(regresarSuma('.CMES7PPL', '.CMES6RAM'));
            $('.CMES8RAM').val(regresarSuma('.CMES8PPL', '.CMES7RAM'));
            $('.CMES9RAM').val(regresarSuma('.CMES9PPL', '.CMES8RAM'));
            $('.CMES10RAM').val(regresarSuma('.CMES10PPL', '.CMES9RAM'));
            $('.CMES11RAM').val(regresarSuma('.CMES11PPL', '.CMES10RAM'));
            $('.CMES12RAM').val(regresarSuma('.CMES12PPL', '.CMES11RAM'));


        }
        function OperacionesMargenSobreResultado() {
            $('.CMES1MRA').val(regresaDivisionPor('.CMES1RAM', '.CMES1VAM'));
            $('.CMES2MRA').val(regresaDivisionPor('.CMES2RAM', '.CMES2VAM'));
            $('.CMES3MRA').val(regresaDivisionPor('.CMES3RAM', '.CMES3VAM'));
            $('.CMES4MRA').val(regresaDivisionPor('.CMES4RAM', '.CMES4VAM'));
            $('.CMES5MRA').val(regresaDivisionPor('.CMES5RAM', '.CMES5VAM'));
            $('.CMES6MRA').val(regresaDivisionPor('.CMES6RAM', '.CMES6VAM'));
            $('.CMES7MRA').val(regresaDivisionPor('.CMES7RAM', '.CMES7VAM'));
            $('.CMES8MRA').val(regresaDivisionPor('.CMES8RAM', '.CMES8VAM'));
            $('.CMES9MRA').val(regresaDivisionPor('.CMES9RAM', '.CMES9VAM'));
            $('.CMES10MRA').val(regresaDivisionPor('.CMES10RAM', '.CMES10VAM'));
            $('.CMES11MRA').val(regresaDivisionPor('.CMES11RAM', '.CMES11VAM'));
            $('.CMES12MRA').val(regresaDivisionPor('.CMES12RAM', '.CMES12VAM'));
        }
        function OperacionResultadosMensualPor() {
            $('.CMES1SumPPPL').val(regresaDivisionPor('.CMES1PPL', '.CMES1VM'));
            $('.CMES2SumPPPL').val(regresaDivisionPor('.CMES2PPL', '.CMES2VM'));
            $('.CMES3SumPPPL').val(regresaDivisionPor('.CMES3PPL', '.CMES3VM'));
            $('.CMES4SumPPPL').val(regresaDivisionPor('.CMES4PPL', '.CMES4VM'));
            $('.CMES5SumPPPL').val(regresaDivisionPor('.CMES5PPL', '.CMES5VM'));
            $('.CMES6SumPPPL').val(regresaDivisionPor('.CMES6PPL', '.CMES6VM'));
            $('.CMES7SumPPPL').val(regresaDivisionPor('.CMES7PPL', '.CMES7VM'));
            $('.CMES8SumPPPL').val(regresaDivisionPor('.CMES8PPL', '.CMES8VM'));
            $('.CMES9SumPPPL').val(regresaDivisionPor('.CMES9PPL', '.CMES9VM'));
            $('.CMES10SumPPPL').val(regresaDivisionPor('.CMES10PPL', '.CMES10VM'));
            $('.CMES11SumPPPL').val(regresaDivisionPor('.CMES11PPL', '.CMES11VM'));
            $('.CMES12SumPPPL').val(regresaDivisionPor('.CMES12PPL', '.CMES12VM'));
        }
        function regresarNumero(selector) {
            return Number($(selector).val());
        }
        function regresarSuma(selector1, selector2) {
            return (regresarNumero(selector1) + regresarNumero(selector2)).toFixed(2);
        }
        function regresaDivision(selector1, selector2) {

            var res = (Math.round(regresarNumero(selector1) / regresarNumero(selector2)));

            return res.toFixed(2);
        }
        function regresaDivisionPor(selector1, selector2) {
            var res = (regresarNumero(selector1) / regresarNumero(selector2)) * 100;
            return res;
        }

        init();
    };

    $(document).ready(function () {

        Administrativo.Princiapl = new Princiapl();
    });
})();

