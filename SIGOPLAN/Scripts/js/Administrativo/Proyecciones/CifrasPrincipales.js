(function () {

    $.namespace('Administrativo.Proyecciones.CifrasPrincipales');

    CifrasPrincipales = function () {


        mensajes = {
            NOMBRE: 'Proyecciones Financieras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        lblAreaPPL = $("#lblAreaPPL"),
        lblAreaPPLTotal = $("#lblAreaPPLTotal"),

        btn1 = $("#btn1"),
        btn2 = $("#btn2"),
        btn3 = $("#btn3"),


        tbAnioAnteriorVentaProyectada = $("#tbAnioAnteriorVentaProyectada");// .val(DataResult.VentaProyectadaAnioAnterior);
        tbProyeccionMesVentaProyectada = $("#tbProyeccionMesVentaProyectada"); //.val(DataResult.VentaProyectadaMesActual);
        tbProyectadoAnioVentaProyectada = $("#tbProyectadoAnioVentaProyectada"); //.val(DataResult.VentaProyectadaAlAnio);
        tbAnioAnteriorVentaReal = $("#tbAnioAnteriorVentaReal"); // .val(DataResult.VentaRealAnioAnterior);
        tbProyeccionesMesVentaReal = $("#tbProyeccionesMesVentaReal"); //.val(DataResult.VentaRealMesActual);
        tbProyectadoAnioMesVentaReal = $("#tbProyectadoAnioMesVentaReal"); //.val(DataResult.VentaRealProyectdaAlAnio);
        tbAnioAnteriorUtilidadPlaneada = $("#tbAnioAnteriorUtilidadPlaneada"); //.val(DataResult.UtilidadPlaneadaAnioAnterior);
        tbProyecionesMesUtilidadPlaneada = $("#tbProyecionesMesUtilidadPlaneada");//.val(DataResult.UtilidadPlaneadaMesActual);
        tbProyecionesAnioUtilidadPlaneada = $("#tbProyecionesAnioUtilidadPlaneada"); //.val(DataResult.UtilidadPlaneadaAnioActual);
        tbAnioAnteriorUtilidadReal = $("#tbAnioAnteriorUtilidadReal");//.val(DataResult.UtilidadRealAnioAnterior);
        tbProyecionesMesUtilidadReal = $("#tbProyecionesMesUtilidadReal");//.val(DataResult.UtilidadRealMesActual);
        tbProyecionesAnioUtilidadReal = $("#tbProyecionesAnioUtilidadReal");//.val(DataResult.UtilidadRealAnioActual);

        tbCumpliminetoAnioAnterior = $("#tbCumpliminetoAnioAnterior");
        tbCumpliminetoMesActual = $("#tbCumpliminetoMesActual"),
        tbCumpliminetoAnioActual = $("#tbCumpliminetoAnioActual"),
        tbPorUtilidadPlaneadaAnioAnterior = $("#tbPorUtilidadPlaneadaAnioAnterior"),
        tbPorUtilidadPlaneadaMesActual = $("#tbPorUtilidadPlaneadaMesActual"),
        tbProUtilidadPlaneadaAnioActual = $("#tbProUtilidadPlaneadaAnioActual"),
        tbPorUtilidadRealAnioAnterior = $("#tbPorUtilidadRealAnioAnterior"),
        tbPorUtilidadRealMesActual = $("#tbPorUtilidadRealMesActual"),
        tbPorUtilidadRealAnioActual = $("#tbPorUtilidadRealAnioActual"),


        btnModalResultadoAcumulado = $("#btnModalResultadoAcumulado"),
        btnModalFlujoFinalAnio = $("#btnModalFlujoFinalAnio"),
        btnModalVentas = $("#btnModalVentas"),

        ModalAreasPPL = $("#ModalAreasPPL"),
        tblDivisionesMassVentas = $("#tblDivisionesMassVentas"),
        tblCifrasPrincipales = $("#tblCifrasPrincipales"),
        cboEscenario = $("#cboEscenario"),
        tbDivisor = $("#tbDivisor"),
        tbMesesInicio = $("#tbMesesInicio"),
        btnCargarInfo = $("#btnCargarInfo"),
        cboPeriodo = $("#cboPeriodo");

        tbDescripcion1 = $("#tbDescripcion1");
        tbVenta1 = $("#tbVenta1");
        tbPorcentaje1 = $("#tbPorcentaje1");
        tbDescripcion2 = $("#tbDescripcion2");
        tbVenta2 = $("#tbVenta2");
        tbPorcentaje2 = $("#tbPorcentaje2");
        tbDescripcion3 = $("#tbDescripcion3");
        tbVenta3 = $("#tbVenta3");
        tbPorcentaje3 = $("#tbPorcentaje3"),

        btnResultadoMensual = $("#btnResultadoMensual");
        btnFlujoEfectivo = $("#btnFlujoEfectivo");
        btnVentasMensuales = $("#btnVentasMensuales");
        btnUtilidadBruta = $("#btnUtilidadBruta");
        btnlGastosAdmo = $("#btnlGastosAdmo");
        btnVentaAcumulada = $("#btnVentaAcumulada");
        btnResultadoACumulado = $("#btnResultadoACumulado");
        btnMargenResultadoAcumulado = $("#btnMargenResultadoAcumulado"),
        btnModalTblResultadoMensual = $("#btnModalTblResultadoMensual"),
        btnModalTblFlujoEfectivo = $("#btnModalTblFlujoEfectivo"),
        btnModalTblVentasMensuales = $("#btnModalTblVentasMensuales"),
        btnModalTblUtilidadBruta = $("#btnModalTblUtilidadBruta"),
        btnModalTblGastosAdmo = $("#btnModalTblGastosAdmo"),
        btnModalTblVentaAcumulada = $("#btnModalTblVentaAcumulada"),
        btnModalTblResultadoACumulado = $("#btnModalTblResultadoACumulado"),
        btnModalTblMargenResultadoAcumulado = $("#btnModalTblMargenResultadoAcumulado"),

        modalTblResultadoMensual = $("#modalTblResultadoMensual"),
        modalTblFlujoEfectivo = $("#modalTblFlujoEfectivo"),
        modalTblVentasMensuales = $("#modalTblVentasMensuales"),
        modalTblUtilidadBruta = $("#modalTblUtilidadBruta"),
        modalTblGastosAdmo = $("#modalTblGastosAdmo"),
        modalTblVentaAcumulada = $("#modalTblVentaAcumulada"),
        modalTblResultadoACumulado = $("#modalTblResultadoACumulado"),
        modalTblMargenResultadoAcumulado = $("#modalTblMargenResultadoAcumulado");

        var myChart1 = null;
        var myChart2 = null;
        var myChart3 = null;
        var myChart4 = null;
        var myChart5 = null;
        var myChart6 = null;
        var myChart7 = null;
        var myChart8 = null;

        function init() {
            Chart.defaults.global.legend.display = false;
            tbVenta1.DecimalFixNS(0);
            tbVenta2.DecimalFixNS(0);
            tbVenta3.DecimalFixNS(0);
            tblDivisionesMassVentasGrid = $("#tblDivisionesMassVentas").DataTable({});
            tabla = $('#tblResultadoMensual').DataTable({});
            tabla2 = $('#tblFlujoEfectivo').DataTable({});
            tabla3 = $('#tblVentasMensuales').DataTable({});
            tabla4 = $('#tblUtilidadBruta').DataTable({});
            tabla5 = $('#tblGastosAdmo').DataTable({});
            tabla6 = $('#tblVentaAcumulada').DataTable({});
            tabla7 = $("#tblResultadoACumulado").DataTable({});
            tabla8 = $("#tblMargenResultadoAcumulado").DataTable({});
            //  tabla9 = $('#tblCifrasPrincipales').DataTable({});
            btnCargarInfo.click(LoadDataTables);
            LoadDataTables();

            initModal();
            //btnResultadoMensual.click(modalGrafica);
            //btnFlujoEfectivo.click(modalGrafica);
            //btnVentasMensuales.click(modalGrafica);
            //btnUtilidadBruta.click(modalGrafica);
            //btnlGastosAdmo.click(modalGrafica);
            //btnVentaAcumulada.click(modalGrafica);
            //btnResultadoACumulado.click(modalGrafica);
            //btnMargenResultadoAcumulado.click(modalGrafica);

            //        btnModalTblResultadoMensual.click(fnModalTblResultadoMensual);
            //       btnModalTblFlujoEfectivo.click(fnModalTblFlujoEfectivo);
            //      btnModalTblVentasMensuales.click(fnModalTblVentasMensuales);
            //     btnModalTblUtilidadBruta.click(fnModalTblUtilidadBruta);
            //    btnModalTblGastosAdmo.click(fnModalTblGastosAdmo);
            //   btnModalTblVentaAcumulada.click(fnModalTblVentaAcumulada);
            cboEscenario.fillCombo('/proyecciones/fillCboEscenarios', { tipo: 1 },true);
            cboEscenario.change(LoadDataTables);
            tbMesesInicio.change(LoadDataTables);
            cboPeriodo.change(LoadDataTables);
            btnModalResultadoAcumulado.click(fnModalTblResultadoMensual);

            btnModalTblMargenResultadoAcumulado.click(fnModalTblMargenResultadoAcumulado);
            btnModalFlujoFinalAnio.click(fnModalTblFlujoEfectivo);
            btnModalVentas.click(fnModalTblVentasMensuales);
            btn1.click(OpenModalObrasPPL);
            btn2.click(OpenModalObrasPPL);
            btn3.click(OpenModalObrasPPL);

            $('a[href="#tabResultadoAcumulado"]').click(CargarGrafica);
            $('a[href="#tabUtilidadBrutaPro"]').click(CargarGrafica);
            $('a[href="#tabporSobreAcumMeses"]').click(CargarGrafica);
         //   $('a[href="#tabVentasMensuales"]').click(CargarGrafica);
            $('a[href="#tabVentaAcumuladaMeses"]').click(CargarGrafica);

        }

        function CargarGrafica() {
            var num = Number($(this).attr('data-grafica'));

            switch (num) {
                case 1:
                    SetGraficaModal1(num);
                    break;
                case 2:
                    SetGraficaModal2(num);
                    break;
                case 3:
                    SetGraficaModal3(num);
                    break;
                case 4:
                    SetGraficaModal4(num);
                    break;
                case 5:
                    SetGraficaModal5(num);
                    break;
                case 6:
                    SetGraficaModal6(num);
                    break;
                case 7:
                    SetGraficaModal7(num);
                    break;
                case 8:
                    SetGraficaModal8(num);
                    break;
                default:

            }

        }

        function reDrawTablas() {
            tabla.draw();
            tabla2.draw();
            tabla3.draw();
            tabla4.draw();
            tabla5.draw();
            tabla6.draw();
            tabla7.draw();
            tabla8.draw();
        }
        function fnModalTblResultadoMensual() {

            $('a[href="#tabResultadoMensual"]').trigger('click');
            SetGraficaModal1(1);

            modalTblResultadoMensual.modal("show");
            reDrawTablas();
        }

        function fnModalTblFlujoEfectivo() {
            SetGraficaModal2(2);
            modalTblFlujoEfectivo.modal("show");
            reDrawTablas();
        }
        function fnModalTblVentasMensuales() {
            $('a[href="#tabVentasMensuales"]').trigger('click');
            SetGraficaModal3(3);
            modalTblVentasMensuales.modal("show");
            reDrawTablas();


        }
        function fnModalTblUtilidadBruta() {
            SetGraficaModal4(4);
            modalTblUtilidadBruta.modal("show");
            reDrawTablas();
        }
        function fnModalTblGastosAdmo() {
            SetGraficaModal5(5);
            modalTblGastosAdmo.modal("show");
            reDrawTablas();
        }
        function fnModalTblVentaAcumulada() {
            SetGraficaModal6(6);
            modalTblVentaAcumulada.modal("show");
            reDrawTablas();
        }
        function fnModalTblResultadoACumulado() {
            SetGraficaModal7(7);
            modalTblResultadoACumulado.modal("show");
            reDrawTablas();
        }
        function fnModalTblMargenResultadoAcumulado() {
            SetGraficaModal8(8);
            modalTblMargenResultadoAcumulado.modal("show");
            reDrawTablas();
        }

        function LoadDataTables() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //  url: '/proyecciones/LoadObra',
                url: '/Proyecciones/CapturaObraSetData',
                type: 'POST',
                dataType: 'json',
                data: { parametros: getFGB() },
                //data: { escenario: 1, divisor: 1000, meses: 3, anios: 2017 },
                success: function (response) {

                    $('.lblEscenario').each(function () {
                        $(this).text('Escenario ' + $("#cboEscenario option:selected").text());
                    });

                    deleteValores();


                    var ListaDivisionesVentas = response.ListaDivisionesVentas;
                    var DataResult = response.DataResultTabla;

                    tbAnioAnteriorVentaProyectada.val(DataResult.VentaProyectadaAnioAnterior);
                    tbProyeccionMesVentaProyectada.val(DataResult.VentaProyectadaMesActual);
                    tbProyectadoAnioVentaProyectada.val(DataResult.VentaProyectadaAlAnio);
                    tbAnioAnteriorVentaReal.val(DataResult.VentaRealAnioAnterior);
                    tbProyeccionesMesVentaReal.val(DataResult.VentaRealMesActual);
                    tbProyectadoAnioMesVentaReal.val(DataResult.VentaRealProyectdaAlAnio);
                    tbAnioAnteriorUtilidadPlaneada.val(DataResult.UtilidadPlaneadaAnioAnterior);
                    tbProyecionesMesUtilidadPlaneada.val(DataResult.UtilidadPlaneadaMesActual);
                    tbProyecionesAnioUtilidadPlaneada.val(DataResult.UtilidadPlaneadaAnioActual);
                    tbAnioAnteriorUtilidadReal.val(DataResult.UtilidadRealAnioAnterior);
                    tbProyecionesMesUtilidadReal.val(DataResult.UtilidadRealMesActual);
                    tbProyecionesAnioUtilidadReal.val(DataResult.UtilidadRealAnioActual);
                    tbCumpliminetoAnioAnterior.val(DataResult.porCumpliminetoAnioAnterior);
                    tbCumpliminetoMesActual.val(DataResult.porCumplimientoMesActual);
                    tbCumpliminetoAnioActual.val(DataResult.porCumplimientoAnioActual);

                    tbPorUtilidadPlaneadaAnioAnterior.val(DataResult.porUtilidadPlaneadaAnioAnterior);
                    tbPorUtilidadPlaneadaMesActual.val(DataResult.porUtilidadPlaneadaMesActual);
                    tbProUtilidadPlaneadaAnioActual.val(DataResult.porUtilidadPlenadaAnioActual);
                    tbPorUtilidadRealAnioAnterior.val(DataResult.porUtilidadRealAnioAnterior);
                    tbPorUtilidadRealMesActual.val(DataResult.porUtilidadRealMesActual);
                    tbPorUtilidadRealAnioActual.val(DataResult.porUtilidadRealAnioActual);

                    if (ListaDivisionesVentas != undefined) {
                        setDivisionesObrasVentasTotales(ListaDivisionesVentas);
                    }
                    var VentaAcumulada = response.VentaAcumulada;
                    var UtilidadNeta = response.UtilidadNeta;
                    var VentasNetas = response.VentasNetas;
                    var SaldoFinalFlujoEfectivo = response.SaldoFinalFlujoEfectivo;
                    var TotalGtoOperacion = response.TotalGtoOperacion;
                    var UtilidadPromedioBruta = response.UtilidadPromedioBruta;
                    var ResultadoMensualAcumulado = response.ResultadoMensualAcumulado;
                    var PorcentajeVentasResultado = response.PorcentajeVentasResultado;
                    var DetallePorcentaje = response.DetallePorcentaje;
                    var UtilidadBrutaDetalle = response.UtilidadBrutaDetalle;

                    if (UtilidadNeta != undefined && VentasNetas != undefined && SaldoFinalFlujoEfectivo != undefined && TotalGtoOperacion != undefined && UtilidadPromedioBruta != undefined) {

                        dataSetTabla9 = [];
                        var CifrasPrincipalesDTO = {};
                        CifrasPrincipalesDTO.Descripcion = "UTILIDAD ACUMULADA";
                        CifrasPrincipalesDTO.Monto = ReturnFormat(UtilidadNeta.Total);
                        dataSetTabla9.push(CifrasPrincipalesDTO);

                        $("#tblT3").val(CifrasPrincipalesDTO.Descripcion);
                        $("#tblT4").val(CifrasPrincipalesDTO.Monto);
                        var CifrasPrincipalesDTO = {};

                        CifrasPrincipalesDTO.Descripcion = "FLUJO FINAL AÑO";
                        CifrasPrincipalesDTO.Monto = ReturnFormat(SaldoFinalFlujoEfectivo.Fecha12);;
                        dataSetTabla9.push(CifrasPrincipalesDTO);

                        $("#tblT5").val(CifrasPrincipalesDTO.Descripcion);
                        $("#tblT6").val(CifrasPrincipalesDTO.Monto);

                        var CifrasPrincipalesDTO = {};
                        CifrasPrincipalesDTO.Descripcion = "VENTAS ACUMULADAS";
                        CifrasPrincipalesDTO.Monto = ReturnFormat(VentasNetas.Total /*/ tbDivisor.val()*/);;
                        dataSetTabla9.push(CifrasPrincipalesDTO);

                        $("#tblT1").val(CifrasPrincipalesDTO.Descripcion);
                        $("#tblT2").val(CifrasPrincipalesDTO.Monto);

                        LoadTabla1(UtilidadNeta);
                        LoadTabla2(SaldoFinalFlujoEfectivo);
                        LoadTabla3(VentasNetas);
                        LoadTabla4(UtilidadPromedioBruta, UtilidadBrutaDetalle);
                        LoadTabla5(TotalGtoOperacion);
                        LoadTabla6(VentaAcumulada);
                        //LoadTabla7(VentasNetas);
                        LoadTabla8(PorcentajeVentasResultado, ResultadoMensualAcumulado, DetallePorcentaje);

                        //      setTable9(dataSetTabla9);
                        setFixInputs();
                        //SetGrafica();
                        btnModalTblResultadoMensual.prop("disabled", false);
                        btnModalTblFlujoEfectivo.prop("disabled", false);
                        btnModalTblVentasMensuales.prop("disabled", false);
                        btnModalTblUtilidadBruta.prop("disabled", false);
                        btnModalTblGastosAdmo.prop("disabled", false);
                        btnModalTblVentaAcumulada.prop("disabled", false);
                        btnModalTblResultadoACumulado.prop("disabled", false);
                        btnModalTblMargenResultadoAcumulado.prop("disabled", false);
                        $(".HidenTH").addClass('hide');
                        setWidth();
                    }
                    else {
                        btnModalTblResultadoMensual.prop("disabled", true);
                        btnModalTblFlujoEfectivo.prop("disabled", true);
                        btnModalTblVentasMensuales.prop("disabled", true);
                        btnModalTblUtilidadBruta.prop("disabled", true);
                        btnModalTblGastosAdmo.prop("disabled", true);
                        btnModalTblVentaAcumulada.prop("disabled", true);
                        btnModalTblResultadoACumulado.prop("disabled", true);
                        btnModalTblMargenResultadoAcumulado.prop("disabled", true);
                    }
                    tbMesesInicio.val(response.ultimoMes2);
                    cboPeriodo.val(response.ultimoAnio2);
                    $.unblockUI();

                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function ReturnFormat(cellData) {
            var monto = Math.round(Number(cellData), -1);
            var nf = new Intl.NumberFormat("es-MX");
            return nf.format(monto);
        }

        function deleteValores() {

            tabla.clear().draw();
            tabla2.clear().draw();
            tabla3.clear().draw();
            tabla4.clear().draw();
            tabla5.clear().draw();
            tabla6.clear().draw();
            tabla7.clear().draw();
            tabla8.clear().draw();
            tbDescripcion1.val('');
            tbVenta1.val('');
            tbPorcentaje1.val('');
            tbDescripcion2.val('');
            tbVenta2.val('');
            tbPorcentaje2.val('');
            tbDescripcion3.val('');
            tbVenta3.val('');
            tbPorcentaje3.val('');
            if (myChart1 != null) {
                myChart1.clear();
            }
            if (myChart2 != null) {
                myChart2.clear();
            }
            if (myChart3 != null) {
                myChart3.clear();
            }
            if (myChart4 != null) {
                myChart4.clear();
            }
            if (myChart5 != null) {
                myChart5.clear();
            }
            if (myChart6 != null) {
                myChart6.clear();
            }
            if (myChart7 != null) {
                myChart7.clear();
            }
            if (myChart8 != null) {
                myChart8.clear();
            }
        }

        function setWidth() {
            //$("#tblFlujoEfectivo").children('tbody').children('tr').children('td').css('width', '120px');
            //$("#tblVentasMensuales").children('tbody').children('tr').children('td').css('width', '120px');
            //$("#tblUtilidadBruta").children('tbody').children('tr').children('td').css('width', '120px');
            //$("#tblGastosAdmo").children('tbody').children('tr').children('td').css('width', '120px');
            //$("#tblVentaAcumulada").children('tbody').children('tr').children('td').css('width', '120px');
            //$("#tblResultadoACumulado").children('tbody').children('tr').children('td').css('width', '120px');
            //$("#tblMargenResultadoAcumulado").children('tbody').children('tr').children('td').css('width', '120px');
        }


        function GetInfoTaba() {

        }

        function setFixInputs() {
            inputChange = $(".porcentage");
            $.each(inputChange, function (i, e) {
                $(e).val(parseFloat($(e).val()).toFixed(2) + '%');
            });
            dinero = $(".dinero");
            $.each(dinero, function (i, e) {
                $(e).val(parseFloat($(e).val()).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
            });
            fnReloadCustomDecialEvent();
        }

        function setDivisionesObrasVentasTotales(dataset) {

            if (dataset.length > 0) {
                for (var i = 0; i < 3 ; i++) {
                    var id = i + 1;
                    $('#tbLugar' + id).val(dataset[i].Lugar);
                    $('#tbDescripcion' + id).val(dataset[i].Obra);
                    $('#tbVenta' + id).setVal(dataset[i].Venta);
                    $('#tbPorcentaje' + id).val((dataset[i].Procentaje * 100).toFixed(2) + '%');
                    $('#btn' + id).attr("data-Area", dataset[i].Area);

                }
            }

        }

        function OpenModalObrasPPL() {
            lblAreaPPL.text('');
            lblAreaPPLTotal.text('');
            var idArea = $(this).attr('data-Area');
            var Descripcion = $(this).parents('tr').children().children('.input-group').children('.clsDescripcion').val();
            var Total = $(this).parents('tr').children().children('.clsVentasTotales').val()
            GetDataObrasPPL(idArea);
            lblAreaPPL.text(Descripcion);
            lblAreaPPLTotal.text(Total);
            ModalAreasPPL.modal('show');
        }

        function GetDataObrasPPL(idArea) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GetTablaFlujoIngresoGeneral',
                type: 'POST',
                dataType: 'json',
                data: { id: idArea },
                success: function (response) {
                    FujoIngresoGeneral = response.FujoIngresoGeneral;
                    SetTableObrasPPL(FujoIngresoGeneral);
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function SetTableObrasPPL(dataSet) {
            var tituloMeses = [];
            var date = new Date();

            tituloMeses = GetPeriodoMeses(1);

            tblDivisionesMassVentasGrid = $('#tblDivisionesMassVentas').DataTable({
                "bFilter": false,
                destroy: true,
                scrollCollapse: true,
                data: dataSet,
                "ordering": false,
                "aoColumnDefs": [
                    { "bSortable": false, "aTargets": [0] }
                ],
                "aaSorting": [[1, 'asc']],
                columns: [
                    {
                        "title": "Descripción", data: "AreaNombre", "width": "200px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }

                        }
                    },

                    {
                        "title": "Total", data: "Monto", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData) / 1000);
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            $(td).addClass('CLSColorTotal');

                        }
                    },
                       {
                           "title": "%", data: "Total", "width": "90px",
                           createdCell: function (td, cellData, rowData, row, col) {
                               var monto = Number(cellData).toFixed(2);

                               $(td).text(monto + "%");
                               $(td).addClass('text-right');
                               $(td).addClass('CLSColorPorcentajes');

                           }
                       },
                        {
                            "title": tituloMeses[0], data: "Fecha1", "width": "90px",
                            createdCell: function (td, cellData, rowData, row, col) {
                                var monto = Math.round(Number(cellData / 1000));
                                var nf = new Intl.NumberFormat("es-MX");
                                $(td).text(nf.format(monto));
                                $(td).addClass('text-right');
                                if (Number(rowData.Area) == 0) {
                                    $(td).addClass('CLSColorTotal');
                                }
                            }
                        },
                    {
                        "title": tituloMeses[1], data: "Fecha2", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[2], data: "Fecha3", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[3], data: "Fecha4", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[4], data: "Fecha5", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[5], data: "Fecha6", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[6], data: "Fecha7", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[7], data: "Fecha8", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[8], data: "Fecha9", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[9], data: "Fecha10", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[10], data: "Fecha11", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[11], data: "Fecha12", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var monto = Math.round(Number(cellData / 1000));
                            var nf = new Intl.NumberFormat("es-MX");
                            $(td).text(nf.format(monto));
                            $(td).addClass('text-right');
                            if (Number(rowData.Area) == 0) {
                                $(td).addClass('CLSColorTotal');
                            }
                        }
                    },
                ],
                "bPaginate": false,
                "bFilter": false,

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

        function GetDataGrafica() {
            var Array = [];
            Array.push(quitDineroFormat($(".CMES1MRA").val()));
            Array.push(quitDineroFormat($(".CMES2MRA").val()));
            Array.push(quitDineroFormat($(".CMES3MRA").val()));
            Array.push(quitDineroFormat($(".CMES4MRA").val()));
            Array.push(quitDineroFormat($(".CMES5MRA").val()));
            Array.push(quitDineroFormat($(".CMES6MRA").val()));
            Array.push(quitDineroFormat($(".CMES7MRA").val()));
            Array.push(quitDineroFormat($(".CMES8MRA").val()));
            Array.push(quitDineroFormat($(".CMES9MRA").val()));
            Array.push(quitDineroFormat($(".CMES10MRA").val()));
            Array.push(quitDineroFormat($(".CMES11MRA").val()));
            Array.push(quitDineroFormat($(".CMES12MRA").val()));
            return Array;

        }
        var randomColor = function (opacity) {
            return 'rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',' + (opacity || '.3') + ')';
        };

        function SetGrafica() {
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            GetDataGrafica();

            var data = {
                labels: datah,
                datasets: [
                {
                    data: GetDataGrafica(),
                    borderColor: randomColor(0.4),
                    backgroundColor: randomColor(0.5)
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
                                beginAtZero: false
                            }
                        }]
                    }
                }
            });

            myChart.resize();


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


        function LoadTabla1(UtilidadNeta, dataSetV) {

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
                                //<div class='dt-body-right'></div>
                                json.MES1 = "<input type='text' class='setBlank CMES1PPL inputChange dinero' data-mes='" + listaIdMes[0] + "' value='" + Math.round(dataSet[i].MES1) + "' >";
                                json.MES2 = "<input type='text' class='setBlank CMES2PPL inputChange dinero' data-mes='" + listaIdMes[1] + "' value='" + Math.round(dataSet[i].MES2) + "' >";
                                json.MES3 = "<input type='text' class='setBlank CMES3PPL inputChange dinero' data-mes='" + listaIdMes[2] + "' value='" + Math.round(dataSet[i].MES3) + "' >";
                                json.MES4 = "<input type='text' class='setBlank CMES4PPL inputChange dinero' data-mes='" + listaIdMes[3] + "' value='" + Math.round(dataSet[i].MES4) + "' >";
                                json.MES5 = "<input type='text' class='setBlank CMES5PPL inputChange dinero' data-mes='" + listaIdMes[4] + "' value='" + Math.round(dataSet[i].MES5) + "' >";
                                json.MES6 = "<input type='text' class='setBlank CMES6PPL inputChange dinero' data-mes='" + listaIdMes[5] + "' value='" + Math.round(dataSet[i].MES6) + "' >";
                                json.MES7 = "<input type='text' class='setBlank CMES7PPL inputChange dinero' data-mes='" + listaIdMes[6] + "' value='" + Math.round(dataSet[i].MES7) + "' >";
                                json.MES8 = "<input type='text' class='setBlank CMES8PPL inputChange dinero' data-mes='" + listaIdMes[7] + "' value='" + Math.round(dataSet[i].MES8) + "' >";
                                json.MES9 = "<input type='text' class='setBlank CMES9PPL inputChange dinero' data-mes='" + listaIdMes[8] + "' value='" + Math.round(dataSet[i].MES9) + "' >";
                                json.MES10 = "<input type='text' class='setBlank CMES10PPL inputChange dinero' data-mes='" + listaIdMes[9] + "' value='" + Math.round(dataSet[i].MES10) + "' >";
                                json.MES11 = "<input type='text' class='setBlank CMES11PPL inputChange dinero' data-mes='" + listaIdMes[10] + "' value='" + Math.round(dataSet[i].MES11) + "' >";
                                json.MES12 = "<input type='text' class='setBlank CMES12PPL inputChange dinero' data-mes='" + listaIdMes[11] + "' value='" + Math.round(dataSet[i].MES12) + "' >";
                                json.Total = "<input type='text' class='setBlank CTotalPPL inputChange dinero CLSColorTotal'  value='" + dataSet[i].Total + "' >";

                                result.push(json);
                                break;
                            }
                        case 1:
                            {
                                json.MES1 = "<input type='text' class='setBlank CMES1SumPPL inputChange dinero' data-mes='" + listaIdMes[0] + "'  value='" + dataSet[i].MES1 + "' >";
                                json.MES2 = "<input type='text' class='setBlank CMES2SumPPL inputChange dinero' data-mes='" + listaIdMes[1] + "'' value='" + dataSet[i].MES2 + "' >";
                                json.MES3 = "<input type='text' class='setBlank CMES3SumPPL inputChange dinero' data-mes='" + listaIdMes[2] + "'' value='" + dataSet[i].MES3 + "' >";
                                json.MES4 = "<input type='text' class='setBlank CMES4SumPPL inputChange dinero' data-mes='" + listaIdMes[3] + "'  value='" + dataSet[i].MES4 + "' >";
                                json.MES5 = "<input type='text' class='setBlank CMES5SumPPL inputChange dinero' data-mes='" + listaIdMes[4] + "'  value='" + dataSet[i].MES5 + "' >";
                                json.MES6 = "<input type='text' class='setBlank CMES6SumPPL inputChange dinero' data-mes='" + listaIdMes[5] + "'' value='" + dataSet[i].MES6 + "' >";
                                json.MES7 = "<input type='text' class='setBlank CMES7SumPPL inputChange dinero' data-mes='" + listaIdMes[6] + "'  value='" + dataSet[i].MES7 + "' >";
                                json.MES8 = "<input type='text' class='setBlank CMES8SumPPL inputChange dinero' data-mes='" + listaIdMes[7] + "'  value='" + dataSet[i].MES8 + "' >";
                                json.MES9 = "<input type='text' class='setBlank CMES9SumPPL inputChange dinero' data-mes='" + listaIdMes[8] + "'   value='" + dataSet[i].MES9 + "' >";
                                json.MES10 = "<input type='text' class='setBlank CMES10SumPPL inputChange dinero' data-mes='" + listaIdMes[9] + "' value='" + dataSet[i].MES10 + "' >";
                                json.MES11 = "<input type='text' class='setBlank CMES11SumPPL inputChange dinero' data-mes='" + listaIdMes[10] + "' value='" + dataSet[i].MES11 + "' >";
                                json.MES12 = "<input type='text' class='setBlank CMES12SumPPL inputChange dinero' data-mes='" + listaIdMes[11] + "' value='" + dataSet[i].MES12 + "' >";

                                json.Total = "";

                                // result.push(json);
                                break;
                            }
                        case 2:
                            {
                                //total = sumaLinea(dataSet[i]);
                                json.MES1 = "<input type='text' class='setBlank CMES1SumPPPL inputChange porcentage CLSColorPorcentajes' value='" + 0 + "' >";
                                json.MES2 = "<input type='text' class='setBlank CMES2SumPPPL inputChange porcentage CLSColorPorcentajes' value='" + 0 + "' >";
                                json.MES3 = "<input type='text' class='setBlank CMES3SumPPPL inputChange porcentage CLSColorPorcentajes' value='" + 0 + "' >";
                                json.MES4 = "<input type='text' class='setBlank CMES4SumPPPL inputChange porcentage CLSColorPorcentajes' value='" + 0 + "' >";
                                json.MES5 = "<input type='text' class='setBlank CMES5SumPPPL inputChange porcentage CLSColorPorcentajes' value='" + 0 + "' >";
                                json.MES6 = "<input type='text' class='setBlank CMES6SumPPPL inputChange porcentage CLSColorPorcentajes'  value='" + 0 + "' >";
                                json.MES7 = "<input type='text' class='setBlank CMES7SumPPPL inputChange porcentage CLSColorPorcentajes'  value='" + 0 + "' >";
                                json.MES8 = "<input type='text' class='setBlank CMES8SumPPPL inputChange porcentage CLSColorPorcentajes'  value='" + 0 + "' >";
                                json.MES9 = "<input type='text' class='setBlank CMES9SumPPPL inputChange porcentage CLSColorPorcentajes'  value='" + 0 + "' >";
                                json.MES10 = "<input type='text' class='setBlank CMES10SumPPPL inputChange porcentage CLSColorPorcentajes' value='" + 0 + "' >";
                                json.MES11 = "<input type='text' class='setBlank CMES11SumPPPL inputChange porcentage CLSColorPorcentajes' value='" + 0 + "' >";
                                json.MES12 = "<input type='text' class='setBlank CMES12SumPPPL inputChange porcentage CLSColorPorcentajes' value='" + 0 + "' >";


                                json.Total = "<input type='text' class='setBlank inputChange CLSColorPorcentajes' id='totalResultadoMensualPor' >";
                                json.color = '1';
                                //  json.Total = "<span style='text-align: center;'>" + '<label id=""></label></span>';

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
            json.MES1 = "<input type='text' class='setBlank CMES1FE inputChange dinero' value='" + Math.round(dataSet.Fecha1) + "' >";
            json.MES2 = "<input type='text' class='setBlank CMES2FE inputChange dinero' value='" + Math.round(dataSet.Fecha2) + "' >";
            json.MES3 = "<input type='text' class='setBlank CMES3FE inputChange dinero' value='" + Math.round(dataSet.Fecha3) + "' >";
            json.MES4 = "<input type='text' class='setBlank CMES4FE inputChange dinero' value='" + Math.round(dataSet.Fecha4) + "' >";
            json.MES5 = "<input type='text' class='setBlank CMES5FE inputChange dinero' value='" + Math.round(dataSet.Fecha5) + "' >";
            json.MES6 = "<input type='text' class='setBlank CMES6FE inputChange dinero' value='" + Math.round(dataSet.Fecha6) + "' >";
            json.MES7 = "<input type='text' class='setBlank CMES7FE inputChange dinero' value='" + Math.round(dataSet.Fecha7) + "' >";
            json.MES8 = "<input type='text' class='setBlank CMES8FE inputChange dinero' value='" + Math.round(dataSet.Fecha8) + "' >";
            json.MES9 = "<input type='text' class='setBlank CMES9FE inputChange dinero' value='" + Math.round(dataSet.Fecha9) + "' >";
            json.MES10 = "<input type='text' class='setBlank CMES10FE inputChange dinero' value='" + Math.round(dataSet.Fecha10) + "' >";
            json.MES11 = "<input type='text' class='setBlank CMES11FE inputChange dinero' value='" + Math.round(dataSet.Fecha11) + "' >";
            json.MES12 = "<input type='text' class='setBlank CMES12FE inputChange dinero' value='" + Math.round(dataSet.Fecha12) + "' >";
            json.Total = "&nbsp;";
            result.push(json);

            SetTable(result);
        }

        function LoadTabla3(dataSet) {
            var result = [];
            var resSuma = sumaLinea(0);
            var json = {}
            json.MES1 = "<input type='text' class='setBlank CMES1VM inputChange dinero' value='" + Math.round(dataSet.Fecha1) + "' >";
            json.MES2 = "<input type='text' class='setBlank CMES2VM inputChange dinero' value='" + Math.round(dataSet.Fecha2) + "' >";
            json.MES3 = "<input type='text' class='setBlank CMES3VM inputChange dinero' value='" + Math.round(dataSet.Fecha3) + "' >";
            json.MES4 = "<input type='text' class='setBlank CMES4VM inputChange dinero' value='" + Math.round(dataSet.Fecha4) + "' >";
            json.MES5 = "<input type='text' class='setBlank CMES5VM inputChange dinero' value='" + Math.round(dataSet.Fecha5) + "' >";
            json.MES6 = "<input type='text' class='setBlank CMES6VM inputChange dinero' value='" + Math.round(dataSet.Fecha6) + "' >";
            json.MES7 = "<input type='text' class='setBlank CMES7VM inputChange dinero' value='" + Math.round(dataSet.Fecha7) + "' >";
            json.MES8 = "<input type='text' class='setBlank CMES8VM inputChange dinero' value='" + Math.round(dataSet.Fecha8) + "' >";
            json.MES9 = "<input type='text' class='setBlank CMES9VM inputChange dinero' value='" + Math.round(dataSet.Fecha9) + "' >";
            json.MES10 = "<input type='text' class='setBlank CMES10VM inputChange dinero' value='" + Math.round(dataSet.Fecha10) + "' >";
            json.MES11 = "<input type='text' class='setBlank CMES11VM inputChange dinero' value='" + Math.round(dataSet.Fecha11) + "' >";
            json.MES12 = "<input type='text' class='setBlank CMES12VM inputChange dinero' value='" + Math.round(dataSet.Fecha12) + "' >";
            json.Total = "<input type='text' class='setBlank CTotalVM inputChange dinero CLSColorTotal' value='" + resSuma + "' >";

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
            json2.MES11 = "<th class='OnlyBottomBorder'></th>";
            json2.MES12 = "<th class='OnlyBottomBorder text-right'>Promedio mensual</th>";
            json2.Total = "<input type='text' class='setBlank CTotalVMDiv inputChange dinero CLSColorTotal' value='" + resSuma / 12 + "' >";
            result.push(json2);

            SetTable2(result);
        }
        function LoadTabla4(dataSet, UtilidadBrutaDetalle) {
            var result = [];
            var json = {}
            json.MES1 = "<input type='text' class='setBlank CMES1UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha1 + "'  value='" + dataSet.Fecha1 + "' >";
            json.MES2 = "<input type='text' class='setBlank CMES2UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha2 + "'  value='" + dataSet.Fecha2 + "' >";
            json.MES3 = "<input type='text' class='setBlank CMES3UBP noEditable inputChange porcentage CLSColorPorcentajes'title='" + UtilidadBrutaDetalle.Fecha3 + "'   value='" + dataSet.Fecha3 + "' >";
            json.MES4 = "<input type='text' class='setBlank CMES4UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha4 + "'  value='" + dataSet.Fecha4 + "' >";
            json.MES5 = "<input type='text' class='setBlank CMES5UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha5 + "'  value='" + dataSet.Fecha5 + "' >";
            json.MES6 = "<input type='text' class='setBlank CMES6UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha6 + "'  value='" + dataSet.Fecha6 + "' >";
            json.MES7 = "<input type='text' class='setBlank CMES7UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha7 + "'  value='" + dataSet.Fecha7 + "' >";
            json.MES8 = "<input type='text' class='setBlank CMES8UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha8 + "'  value='" + dataSet.Fecha8 + "' >";
            json.MES9 = "<input type='text' class='setBlank CMES9UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha9 + "'  value='" + dataSet.Fecha9 + "' >";
            json.MES10 = "<input type='text' class='setBlank CMES10UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha10 + "'   value='" + dataSet.Fecha10 + "' >";
            json.MES11 = "<input type='text' class='setBlank CMES11UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha11 + "'   value='" + dataSet.Fecha11 + "' >";
            json.MES12 = "<input type='text' class='setBlank CMES12UBP noEditable inputChange porcentage CLSColorPorcentajes' title='" + UtilidadBrutaDetalle.Fecha12 + "'  value='" + dataSet.Fecha12 + "' >";
            json.Total = "&nbsp;";
            json.color = "1";
            result.push(json);

            SetTable3(result);
            //LoadTabla5(dataSet);
        }
        function LoadTabla5(dataSet) {
            var result = [];
            var json = {}
            var resSuma = sumaLinea(dataSet);
            json.MES1 = "<input type='text' class='setBlank CMES1GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha1) + "' >";
            json.MES2 = "<input type='text' class='setBlank CMES2GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha2) + "' >";
            json.MES3 = "<input type='text' class='setBlank CMES3GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha3) + "' >";
            json.MES4 = "<input type='text' class='setBlank CMES4GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha4) + "' >";
            json.MES5 = "<input type='text' class='setBlank CMES5GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha5) + "' >";
            json.MES6 = "<input type='text' class='setBlank CMES6GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha6) + "' >";
            json.MES7 = "<input type='text' class='setBlank CMES7GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha7) + "' >";
            json.MES8 = "<input type='text' class='setBlank CMES8GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha8) + "' >";
            json.MES9 = "<input type='text' class='setBlank CMES9GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha9) + "' >";
            json.MES10 = "<input type='text' class='setBlank CMES10GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha10) + "' >";
            json.MES11 = "<input type='text' class='setBlank CMES11GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha11) + "' >";
            json.MES12 = "<input type='text' class='setBlank CMES12GAA noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha12) + "' >";
            json.Total = "<input type='text' class='setBlank CTotalGAA noEditable inputChange dinero CLSColorTotal'  value='" + resSuma + "' >";
            result.push(json);

            SetTable4(result);
            //   LoadTabla6(dataSet);
        }
        function LoadTabla6(dataSet) {
            var result = [];
            var json = {}
            json.MES1 = "<input type='text' class=' setBlank CMES1VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha1) + "' >";
            json.MES2 = "<input type='text' class=' setBlank CMES2VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha2) + "' >";
            json.MES3 = "<input type='text' class=' setBlank CMES3VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha3) + "' >";
            json.MES4 = "<input type='text' class=' setBlank CMES4VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha4) + "' >";
            json.MES5 = "<input type='text' class=' setBlank CMES5VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha5) + "' >";
            json.MES6 = "<input type='text' class=' setBlank CMES6VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha6) + "' >";
            json.MES7 = "<input type='text' class=' setBlank CMES7VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha7) + "' >";
            json.MES8 = "<input type='text' class=' setBlank CMES8VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha8) + "' >";
            json.MES9 = "<input type='text' class=' setBlank CMES9VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha9) + "' >";
            json.MES10 = "<input type='text' class=' setBlank CMES10VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha10) + "' >";
            json.MES11 = "<input type='text' class=' setBlank CMES11VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha11) + "' >";
            json.MES12 = "<input type='text' class=' setBlank CMES12VAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha12) + "' >";
            result.push(json);

            SetTable5(result);
            //LoadTabla7(dataSet);
        }
        function LoadTabla7(dataSet) {
            var result = [];
            var json = {}
            json.MES1 = "<input type='text' class='setBlank CMES1RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha1) + "' >";
            json.MES2 = "<input type='text' class='setBlank CMES2RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha2) + "' >";
            json.MES3 = "<input type='text' class='setBlank CMES3RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha3) + "' >";
            json.MES4 = "<input type='text' class='setBlank CMES4RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha4) + "' >";
            json.MES5 = "<input type='text' class='setBlank CMES5RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha5) + "' >";
            json.MES6 = "<input type='text' class='setBlank CMES6RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha6) + "' >";
            json.MES7 = "<input type='text' class='setBlank CMES7RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha7) + "' >";
            json.MES8 = "<input type='text' class='setBlank CMES8RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha8) + "' >";
            json.MES9 = "<input type='text' class='setBlank CMES9RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha9) + "' >";
            json.MES10 = "<input type='text' class='setBlank CMES10RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha10) + "' >";
            json.MES11 = "<input type='text' class='setBlank CMES11RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha11) + "' >";
            json.MES12 = "<input type='text' class='setBlank CMES12RAM noEditable inputChange dinero' value='" + Math.round(dataSet.Fecha12) + "' >";
            result.push(json);

            //   SetTable6(result);
            //LoadTabla8(dataSet);
        }
        function LoadTabla8(dataSet, dataSetV, DetallePorcentaje) {
            var result = [];

            var json = {}
            json.MES1 = "<input type='text' class='setBlank CMES1RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha1) + "' >";
            json.MES2 = "<input type='text' class='setBlank CMES2RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha2) + "' >";
            json.MES3 = "<input type='text' class='setBlank CMES3RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha3) + "' >";
            json.MES4 = "<input type='text' class='setBlank CMES4RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha4) + "' >";
            json.MES5 = "<input type='text' class='setBlank CMES5RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha5) + "' >";
            json.MES6 = "<input type='text' class='setBlank CMES6RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha6) + "' >";
            json.MES7 = "<input type='text' class='setBlank CMES7RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha7) + "' >";
            json.MES8 = "<input type='text' class='setBlank CMES8RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha8) + "' >";
            json.MES9 = "<input type='text' class='setBlank CMES9RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha9) + "' >";
            json.MES10 = "<input type='text' class='setBlank CMES10RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha10) + "' >";
            json.MES11 = "<input type='text' class='setBlank CMES11RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha11) + "' >";
            json.MES12 = "<input type='text' class='setBlank CMES12RAM noEditable inputChange dinero' value='" + Math.round(dataSetV.Fecha12) + "' >";
            json.Total = "";
            result.push(json);

            var json = {}
            json.MES1 = "<input type='text' class='setBlank CMES1MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha1 + "'  value='" + dataSet.Fecha1 + "' >";
            json.MES2 = "<input type='text' class='setBlank CMES2MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha2 + "' value='" + dataSet.Fecha2 + "' >";
            json.MES3 = "<input type='text' class='setBlank CMES3MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha3 + "' value='" + dataSet.Fecha3 + "' >";
            json.MES4 = "<input type='text' class='setBlank CMES4MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha4 + "' value='" + dataSet.Fecha4 + "' >";
            json.MES5 = "<input type='text' class='setBlank CMES5MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha5 + "' value='" + dataSet.Fecha5 + "' >";
            json.MES6 = "<input type='text' class='setBlank CMES6MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha6 + "' value='" + dataSet.Fecha6 + "' >";
            json.MES7 = "<input type='text' class='setBlank CMES7MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha7 + "' value='" + dataSet.Fecha7 + "' >";
            json.MES8 = "<input type='text' class='setBlank CMES8MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha8 + "' value='" + dataSet.Fecha8 + "' >";
            json.MES9 = "<input type='text' class='setBlank CMES9MRA noEditable inputChange porcentage CLSColorPorcentajes'  title='" + DetallePorcentaje.Fecha9 + "'value='" + dataSet.Fecha9 + "' >";
            json.MES10 = "<input type='text' class='setBlank CMES10MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha10 + "'  value='" + dataSet.Fecha10 + "' >";
            json.MES11 = "<input type='text' class='setBlank CMES11MRA noEditable inputChange porcentage CLSColorPorcentajes' title='" + DetallePorcentaje.Fecha11 + "' value='" + dataSet.Fecha11 + "' >";
            json.MES12 = "<input type='text' class='setBlank CMES12MRA noEditable inputChange porcentage CLSColorPorcentajes'  title='" + DetallePorcentaje.Fecha12 + "' value='" + dataSet.Fecha12 + "' >";
            json.color = 1;
            result.push(json);


            SetTable7(result);



            $('.inputChange').prop('disabled', true);
            $('.noEditable').prop('disabled', true);
            // LoadTabla7(dataSet);
        }

        function sumaLinea(dataSet) {
            var suma = dataSet.Fecha1 + dataSet.Fecha2 + dataSet.Fecha3 + dataSet.Fecha4 + dataSet.Fecha5 + dataSet.Fecha6 + dataSet.Fecha7 + dataSet.Fecha8 + dataSet.Fecha9 + dataSet.Fecha10 + dataSet.Fecha11 + dataSet.Fecha12;
            //console.log(suma);

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


                scrollCollapse: true,
                data: dataSet,
                columns: [

                    {
                        "title": tituloMeses[0], data: "MES1", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }

                    },
                    {
                        "title": tituloMeses[1], data: "MES2", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[2], data: "MES3", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[3], data: "MES4", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[4], data: "MES5", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[5], data: "MES6", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[6], data: "MES7", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[7], data: "MES8", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[8], data: "MES9", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[9], data: "MES10", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[10], data: "MES11", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[11], data: "MES12", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        data: "Total", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            } else {
                                $(td).addClass('CLSColorTotal');
                            }
                        }

                    },
                ],
                "bPaginate": false,
                "bFilter": false,

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
                    { "title": tituloMeses[11], data: "MES12", "width": "90px" }
                ],

                "paging": false,
                "info": false
            });
            tabla2.draw();
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
                    { "title": tituloMeses[11], data: "MES12", "width": "95px" },
                    {
                        data: "Total", "width": "120px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('PaddingTotalMAx');
                            $(td).addClass('CLSColorTotal');
                        }
                    },
                ],

                "paging": false,
                "info": false
            });


            tabla3.on('change', '.inputChange', function () {
                //s elemento = $(this);
                OperacionesVentas();
                //  sumar(elemento, previous);
            });

            OperacionResultadosMensualPor();
            tabla3.draw();
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

                scrollCollapse: true,
                data: dataSet,
                columns: [
                    {
                        "title": tituloMeses[0], data: "MES1", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[1], data: "MES2", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[2], data: "MES3", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[3], data: "MES4", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[4], data: "MES5", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[5], data: "MES6", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[6], data: "MES7", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[7], data: "MES8", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[8], data: "MES9", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[9], data: "MES10", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[10], data: "MES11", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[11], data: "MES12", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    }
                ],

                "paging": false,
                "info": false
            });

            tabla4.draw();

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
                    {
                        data: "Total", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).addClass('CLSColorTotal');
                        }
                    },
                ],

                "paging": false,
                "info": false
            });

            tabla5.on('change', '.inputChange', function () {
                //s elemento = $(this);
                OperacionesGastosADMO();
                //  sumar(elemento, previous);
            });;

            tabla5.draw();
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
                    { "title": tituloMeses[11], data: "MES12", "width": "90px" }
                ],

                "paging": false,
                "info": false
            });
            OperacionesVentas();

            tabla6.draw();
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
                    { "title": tituloMeses[11], data: "MES12", "width": "90px" }
                ],

                "paging": false,
                "info": false
            });
            OperacionesResultadoAcumulado();

        }


        function setTable9(dataSet) {

            tabla9.clear().draw();
            tabla9.destroy();

            tabla9 = $('#tblCifrasPrincipales').DataTable({
                "bFilter": false,
                destroy: true,
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    { "title": 'Descripción', data: "Descripcion", "width": "90px" },
                    { "title": 'Monto', data: "Monto", "width": "90px" },
                ],
                "ordering": false,

                "paging": false,
                "info": false
            });
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
                scrollCollapse: true,
                data: dataSet,
                columns: [
                    {
                        "title": tituloMeses[0], data: "MES1", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[1], data: "MES2", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[2], data: "MES3", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[3], data: "MES4", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[4], data: "MES5", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[5], data: "MES6", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[6], data: "MES7", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[7], data: "MES8", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[8], data: "MES9", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[9], data: "MES10", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[10], data: "MES11", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    },
                    {
                        "title": tituloMeses[11], data: "MES12", "width": "90px",
                        createdCell: function (td, cellData, rowData, row, col) {
                            if (rowData.color == "1") {
                                $(td).addClass('CLSColorPorcentajes');
                            }
                        }
                    }
                ],

                "paging": false,
                "info": false
            });
            OperacionesMargenSobreResultado();
            tabla8.draw();
        }
        function Operaciones() {


            $('.CMES1SumPPL').val($('.CMES1PPL').val());
            $('.CMES2SumPPL').val($('.CMES2SumPPL').attr('data-mes') == "0" ? Number($('.CMES2PPL').val()) : (Number($('.CMES1SumPPL').val()) + Number($('.CMES2PPL').val())));
            $('.CMES3SumPPL').val($('.CMES3SumPPL').attr('data-mes') == "0" ? Number($('.CMES3PPL').val()) : (Number($('.CMES2SumPPL').val()) + Number($('.CMES3PPL').val())).toFixed(2));
            $('.CMES4SumPPL').val($('.CMES4SumPPL').attr('data-mes') == "0" ? Number($('.CMES4PPL').val()) : (Number($('.CMES3SumPPL').val()) + Number($('.CMES4PPL').val())));
            $('.CMES5SumPPL').val($('.CMES5SumPPL').attr('data-mes') == "0" ? Number($('.CMES5PPL').val()) : (Number($('.CMES4SumPPL').val()) + Number($('.CMES5PPL').val())));
            $('.CMES6SumPPL').val($('.CMES6SumPPL').attr('data-mes') == "0" ? Number($('.CMES6PPL').val()) : (Number($('.CMES5SumPPL').val()) + Number($('.CMES6PPL').val())));
            $('.CMES7SumPPL').val($('.CMES7SumPPL').attr('data-mes') == "0" ? Number($('.CMES7PPL').val()) : (Number($('.CMES6SumPPL').val()) + Number($('.CMES7PPL').val())));
            $('.CMES8SumPPL').val($('.CMES8SumPPL').attr('data-mes') == "0" ? Number($('.CMES8PPL').val()) : (Number($('.CMES7SumPPL').val()) + Number($('.CMES8PPL').val())));
            $('.CMES9SumPPL').val($('.CMES9SumPPL').attr('data-mes') == "0" ? Number($('.CMES9PPL').val()) : (Number($('.CMES8SumPPL').val()) + Number($('.CMES9PPL').val())));
            $('.CMES10SumPPL').val($('.CMES10SumPPL').attr('data-mes') == "0" ? Number($('.CMES10PPL').val()) : (Number($('.CMES9SumPPL').val()) + Number($('.CMES10PPL').val())));
            $('.CMES11SumPPL').val($('.CMES11SumPPL').attr('data-mes') == "0" ? Number($('.CMES11PPL').val()) : (Number($('.CMES10SumPPL').val()) + Number($('.CMES11PPL').val())));
            $('.CMES12SumPPL').val($('.CMES12SumPPL').attr('data-mes') == "0" ? Number($('.CMES12PPL').val()) : (Number($('.CMES11SumPPL').val()) + Number($('.CMES12PPL').val())));

            var sumaTotalRM = 0;

            for (var i = 1; i <= 12; i++) {
                sumaTotalRM += Number($('.CMES' + i + 'PPL').val());
            }
            $('.CTotalPPL').val(sumaTotalRM);

            OperacionesResultadoAcumulado();

            var sm = Number($(".CMES1SumPPPL").val()) +
                     Number($(".CMES2SumPPPL").val()) +
                     Number($(".CMES3SumPPPL").val()) +
                     Number($(".CMES4SumPPPL").val()) +
                     Number($(".CMES5SumPPPL").val()) +
                     Number($(".CMES6SumPPPL").val()) +
                     Number($(".CMES7SumPPPL").val()) +
                     Number($(".CMES8SumPPPL").val()) +
                     Number($(".CMES9SumPPPL").val()) +
                     Number($(".CMES10SumPPPL").val()) +
                     Number($(".CMES11SumPPPL").val()) +
                     Number($(".CMES12SumPPPL").val());
            $("#totalResultadoMensualPor").val(parseFloat(sm / 12).toFixed(2) + '%');
            // OperacionesDivisionResultadoMensual();
        }
        function OperacionesVentas() {

            var suma = Number($('.CMES1VM').val()) + Number($('.CMES2VM').val()) + Number($('.CMES3VM').val()) + Number($('.CMES4VM').val()) + Number($('.CMES5VM').val()) + Number($('.CMES6VM').val()) + Number($('.CMES7VM').val()) + Number($('.CMES8VM').val()) + Number($('.CMES9VM').val()) + Number($('.CMES10VM').val()) + Number($('.CMES11VM').val()) + Number($('.CMES12VM').val());

            $('.CTotalVM').val(suma);
            $('.CTotalVMDiv').val((suma / 12).toFixed(2));
            Operaciones();
            OperacionesVentaAcumuladaSCJ();

        }
        function OperacionesGastosADMO() {

            var suma = Number($('.CMES1GAA').val()) + Number($('.CMES2GAA').val()) + Number($('.CMES3GAA').val()) + Number($('.CMES10GAA').val()) + Number($('.CMES4GAA').val()) + Number($('.CMES5GAA').val()) + Number($('.CMES6GAA').val()) + Number($('.CMES7GAA').val()) + Number($('.CMES8GAA').val()) + Number($('.CMES9GAA').val()) + Number($('.CMES11GAA').val()) + Number($('.CMES12GAA').val());

            $('.CTotalGAA').val(suma);
        }
        function OperacionesVentaAcumulada() {

            var suma = Number($('.CMES1GAA').val()) + Number($('.CMES2GAA').val()) + Number($('.CMES3GAA').val()) + Number($('.CMES10GAA').val()) + Number($('.CMES4GAA').val()) + Number($('.CMES5GAA').val()) + Number($('.CMES6GAA').val()) + Number($('.CMES7GAA').val()) + Number($('.CMES8GAA').val()) + Number($('.CMES9GAA').val()) + Number($('.CMES11GAA').val()) + Number($('.CMES12GAA').val());

            $('.CTotalGAA').val(suma);
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

            SetTooltip('.CMES1SumPPPL', '.CMES1PPL', '.CMES1VM');
            SetTooltip('.CMES2SumPPPL', '.CMES2PPL', '.CMES2VM');
            SetTooltip('.CMES3SumPPPL', '.CMES3PPL', '.CMES3VM');
            SetTooltip('.CMES4SumPPPL', '.CMES4PPL', '.CMES4VM');
            SetTooltip('.CMES5SumPPPL', '.CMES5PPL', '.CMES5VM');
            SetTooltip('.CMES6SumPPPL', '.CMES6PPL', '.CMES6VM');
            SetTooltip('.CMES7SumPPPL', '.CMES7PPL', '.CMES7VM');
            SetTooltip('.CMES8SumPPPL', '.CMES8PPL', '.CMES8VM');
            SetTooltip('.CMES9SumPPPL', '.CMES9PPL', '.CMES9VM');
            SetTooltip('.CMES10SumPPPL', '.CMES10PPL', '.CMES10VM');
            SetTooltip('.CMES11SumPPPL', '.CMES11PPL', '.CMES11VM');
            SetTooltip('.CMES12SumPPPL', '.CMES12PPL', '.CMES12VM');

        }

        function SetTooltip(Current, Opera1, Opera2) {
            var num1 = regresarNumero(Opera1);
            var num2 = regresarNumero(Opera2);

            var concat = num1 + "/" + num2 + "=" + regresaDivisionPor(Opera1, Opera2).toFixed(2);
            $(Current).attr('title', concat);

        }
        function regresarNumero(selector) {
            return Number($(selector).val());
        }
        function regresarSuma(selector1, selector2) {
            return (regresarNumero(selector1) + regresarNumero(selector2));
        }
        function regresaDivision(selector1, selector2) {

            var res = (Math.round(regresarNumero(selector1) / regresarNumero(selector2)));

            return res;
        }
        function regresaDivisionPor(selector1, selector2) {
            var res = (regresarNumero(selector1) / regresarNumero(selector2)) * 100;
            return res;
        }

        function quitDineroFormat(currency) {
         //   var number = Number(currency.replace(',', "").replace('%', ""));

            var number = Number(currency.replace(/[\[\],|%]+/g, ''));
            return number;
        }
        function initModal() {
            dialog1 = $("#modalGrafica").dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",

            });
        }
        //function modalGrafica() {
        //    dialog1.dialog("open");
        //    SetGraficaModal($(this).val());
        //}

        function SetGraficaModal1(idModal) {
            if (myChart1 != null) {
                myChart1.clear();
            }
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            var arreglo = GetDataGraficaModal(idModal);
            var valorMaximo = Math.max.apply(null, arreglo);
            var valorMinimo = Math.min.apply(null, arreglo);
            var intervalo;
            if (valorMaximo > 100) {
                intervalo = 100;
            }
            if (valorMaximo > 1000) {
                intervalo = 50000;
            }
            var data = {
                labels: datah,
                datasets: [
                {
                    data: arreglo,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                }
                ]
            };

            var ctx = document.getElementById("LineWithLine1");



            myChart1 = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    datasetFill: true,
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                color: ['black']
                            },
                            scaleLabel: {
                                display: true,
                                labelString: ''
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: false,
                                stepSize: intervalo
                            }
                        }]
                    }
                }
            });

            myChart1.resize();
        }
        function SetGraficaModal2(idModal) {
            if (myChart2 != null) {
                myChart2.clear();
            }
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            var arreglo = GetDataGraficaModal(idModal);
            var valorMaximo = Math.max.apply(null, arreglo);
            var valorMinimo = Math.min.apply(null, arreglo);
            var intervalo;
            if (valorMaximo > 100) {
                intervalo = 100;
            }
            if (valorMaximo > 1000) {
                intervalo = 50000;
            }
            var data = {
                labels: datah,
                datasets: [
                {
                    data: arreglo,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                },
                ]
            };

            var ctx = document.getElementById("LineWithLine2");



            myChart2 = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                color: ['black']
                            },
                            scaleLabel: {
                                display: true,
                                labelString: ''
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: false,
                                stepSize: intervalo
                            }
                        }]
                    }
                }
            });

            myChart2.resize();
        }
        function SetGraficaModal3(idModal) {

            if (myChart3 != null) {
                myChart3.clear();
            }
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            var arreglo = GetDataGraficaModal(idModal);
            var valorMaximo = Math.max.apply(null, arreglo);
            var valorMinimo = Math.min.apply(null, arreglo);
            var intervalo;
            if (valorMaximo > 100) {
                intervalo = 100;
            }
            if (valorMaximo > 1000) {
                intervalo = 50000;
            }
            var data = {
                labels: datah,
                datasets: [
                {
                    data: arreglo,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                },
                ]
            };

            var ctx = document.getElementById("LineWithLine3");

            myChart3 = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                color: ['black']
                            },
                            scaleLabel: {
                                display: true,
                                labelString: ''
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: false,
                                stepSize: intervalo
                            }
                        }]
                    }
                }
            });

            myChart3.resize();
        }
        function SetGraficaModal4(idModal) {
            if (myChart4 != null) {
                myChart4.clear();
            }
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            var arreglo = GetDataGraficaModal(idModal);
            var valorMaximo = Math.max.apply(null, arreglo);
            var valorMinimo = Math.min.apply(null, arreglo);
            var intervalo;
            if (valorMaximo > 100) {
                intervalo = 100;
            }
            if (valorMaximo > 1000) {
                intervalo = 50000;
            }
            var data = {
                labels: datah,
                datasets: [
                {
                    data: arreglo,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                },
                ]
            };

            var ctx = document.getElementById("LineWithLine4");



            myChart4 = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                color: ['black']
                            },
                            scaleLabel: {
                                display: true,
                                labelString: ''
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: false,
                                stepSize: intervalo
                            }
                        }]
                    }
                }
            });

            myChart4.resize();
        }
        function SetGraficaModal5(idModal) {
            if (myChart5 != null) {
                myChart5.clear();
            }
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            var arreglo = GetDataGraficaModal(idModal);
            var valorMaximo = Math.max.apply(null, arreglo);
            var valorMinimo = Math.min.apply(null, arreglo);
            var intervalo;
            if (valorMaximo > 100) {
                intervalo = 100;
            }
            if (valorMaximo > 1000) {
                intervalo = 50000;
            }
            var data = {
                labels: datah,
                datasets: [
                {
                    data: arreglo,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                },
                ]
            };

            var ctx = document.getElementById("LineWithLine5");



            myChart5 = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                color: ['black']
                            },
                            scaleLabel: {
                                display: true,
                                labelString: ''
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: false,
                                stepSize: intervalo
                            }
                        }]
                    }
                }
            });

            myChart5.resize();
        }
        function SetGraficaModal6(idModal) {
            if (myChart6 != null) {
                myChart6.clear();
            }
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            //console.log('revisar')
            var arreglo = GetDataGraficaModal(idModal);
            var valorMaximo = Math.max.apply(null, arreglo);
            var valorMinimo = Math.min.apply(null, arreglo);
            var intervalo;
            if (valorMaximo > 100) {
                intervalo = 100;
            }
            if (valorMaximo > 1000) {
                intervalo = 50000;
            }
            var data = {
                labels: datah,
                datasets: [
                {
                    data: arreglo,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                },
                ]
            };

            var ctx = document.getElementById("LineWithLine6");



            myChart6 = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                color: ['black']
                            },
                            scaleLabel: {
                                display: true,
                                labelString: ''
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: false,
                                stepSize: intervalo
                            }
                        }]
                    }
                }
            });

            myChart6.resize();
        }
        function SetGraficaModal7(idModal) {
            if (myChart7 != null) {
                myChart7.clear();
            }
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            var arreglo = GetDataGraficaModal(idModal);
            var valorMaximo = Math.max.apply(null, arreglo);
            var valorMinimo = Math.min.apply(null, arreglo);
            var intervalo;
            if (valorMaximo > 100) {
                intervalo = 100;
            }
            if (valorMaximo > 1000) {
                intervalo = 50000;
            }
            var data = {
                labels: datah,
                datasets: [
                {
                    data: arreglo,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                },
                ]
            };

            var ctx = document.getElementById("LineWithLine7");



            myChart7 = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                color: ['black']
                            },
                            scaleLabel: {
                                display: true,
                                labelString: ''
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: false,
                                stepSize: intervalo
                            }
                        }]
                    }
                }
            });

            myChart7.resize();
        }
        function SetGraficaModal8(idModal) {
            if (myChart8 != null) {
                myChart8.clear();
            }
            var tituloMeses = [];
            var date = new Date();
            datah = GetPeriodoMeses(1);
            datah.shift();
            datah.unshift('');
            var arreglo = GetDataGraficaModal(idModal);
            var valorMaximo = Math.max.apply(null, arreglo);
            var valorMinimo = Math.min.apply(null, arreglo);
            var intervalo;
            if (valorMaximo > 100) {
                intervalo = 100;
            }
            if (valorMaximo > 1000) {
                intervalo = 50000;
            }
            var data = {
                labels: datah,
                datasets: [
                {
                    data: arreglo,
                    backgroundColor: "rgba(93, 173, 226,0.5)",
                    borderColor: "rgba(21, 67, 96 ,0.4)",
                    pointBackgroundColor: "rgba(21, 67, 96,0.5)",
                    pointBorderColor: "rgba(52, 152, 219,0.7)",
                    pointBorderWidth: 1
                },
                ]
            };

            var ctx = document.getElementById("LineWithLine8");



            myChart8 = new Chart(ctx, {
                type: 'line',
                data: data,
                options: {
                    responsive: true,
                    scales: {
                        xAxes: [{
                            display: true,
                            gridLines: {
                                color: ['black']
                            },
                            scaleLabel: {
                                display: true,
                                labelString: ''
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                beginAtZero: false,
                                stepSize: intervalo
                            }
                        }]
                    }
                }
            });

            myChart8.resize();
        }

        function GetDataGraficaModal(idModal) {
            var Array = [];
            switch (idModal) {
                case 1: Array = DatosResultadoMensual(); break;
                case 2: Array = DatosFlujoEfectivo(); break;
                case 3: Array = DatosVentasMensuales(); break;
                case 4: Array = DatosUtilidadBruta(); break;
                case 5: Array = DatosGastosAdmin(); break;
                case 6: Array = DatosVentaAcomulada(); break;
                case 7: Array = DatosResultadoAcomulada(); break;
                case 8: Array = GetDataGrafica(); break;
                default: "";
            }
            return Array;

        }

        function DatosResultadoMensual() {
            var Array = [];

            Array.push(quitDineroFormat($(".CMES1PPL").val()));
            Array.push(quitDineroFormat($(".CMES2PPL").val()));
            Array.push(quitDineroFormat($(".CMES3PPL").val()));
            Array.push(quitDineroFormat($(".CMES4PPL").val()));
            Array.push(quitDineroFormat($(".CMES5PPL").val()));
            Array.push(quitDineroFormat($(".CMES6PPL").val()));
            Array.push(quitDineroFormat($(".CMES7PPL").val()));
            Array.push(quitDineroFormat($(".CMES8PPL").val()));
            Array.push(quitDineroFormat($(".CMES9PPL").val()));
            Array.push(quitDineroFormat($(".CMES10PPL").val()));
            Array.push(quitDineroFormat($(".CMES11PPL").val()));
            Array.push(quitDineroFormat($(".CMES12PPL").val()));
            return Array;
        }
        function DatosFlujoEfectivo() {
            var Array = [];
            Array.push(quitDineroFormat($(".CMES1FE").val()));
            Array.push(quitDineroFormat($(".CMES2FE").val()));
            Array.push(quitDineroFormat($(".CMES3FE").val()));
            Array.push(quitDineroFormat($(".CMES4FE").val()));
            Array.push(quitDineroFormat($(".CMES5FE").val()));
            Array.push(quitDineroFormat($(".CMES6FE").val()));
            Array.push(quitDineroFormat($(".CMES7FE").val()));
            Array.push(quitDineroFormat($(".CMES8FE").val()));
            Array.push(quitDineroFormat($(".CMES9FE").val()));
            Array.push(quitDineroFormat($(".CMES10FE").val()));
            Array.push(quitDineroFormat($(".CMES11FE").val()));
            Array.push(quitDineroFormat($(".CMES12FE").val()));
            return Array;
        }

        function DatosVentasMensuales() {
            var Array = [];
            Array.push(quitDineroFormat($(".CMES1VM").val()));
            Array.push(quitDineroFormat($(".CMES2VM").val()));
            Array.push(quitDineroFormat($(".CMES3VM").val()));
            Array.push(quitDineroFormat($(".CMES4VM").val()));
            Array.push(quitDineroFormat($(".CMES5VM").val()));
            Array.push(quitDineroFormat($(".CMES6VM").val()));
            Array.push(quitDineroFormat($(".CMES7VM").val()));
            Array.push(quitDineroFormat($(".CMES8VM").val()));
            Array.push(quitDineroFormat($(".CMES9VM").val()));
            Array.push(quitDineroFormat($(".CMES10VM").val()));
            Array.push(quitDineroFormat($(".CMES11VM").val()));
            Array.push(quitDineroFormat($(".CMES12VM").val()));
            return Array;
        }

        function DatosUtilidadBruta() {
            var Array = [];
            Array.push(quitDineroFormat($(".CMES1UBP").val()));
            Array.push(quitDineroFormat($(".CMES2UBP").val()));
            Array.push(quitDineroFormat($(".CMES3UBP").val()));
            Array.push(quitDineroFormat($(".CMES4UBP").val()));
            Array.push(quitDineroFormat($(".CMES5UBP").val()));
            Array.push(quitDineroFormat($(".CMES6UBP").val()));
            Array.push(quitDineroFormat($(".CMES7UBP").val()));
            Array.push(quitDineroFormat($(".CMES8UBP").val()));
            Array.push(quitDineroFormat($(".CMES9UBP").val()));
            Array.push(quitDineroFormat($(".CMES10UBP").val()));
            Array.push(quitDineroFormat($(".CMES11UBP").val()));
            Array.push(quitDineroFormat($(".CMES12UBP").val()));
            return Array;
        }

        function DatosGastosAdmin() {
            var Array = [];
            Array.push(quitDineroFormat($(".CMES1GAA").val()));
            Array.push(quitDineroFormat($(".CMES2GAA").val()));
            Array.push(quitDineroFormat($(".CMES3GAA").val()));
            Array.push(quitDineroFormat($(".CMES4GAA").val()));
            Array.push(quitDineroFormat($(".CMES5GAA").val()));
            Array.push(quitDineroFormat($(".CMES6GAA").val()));
            Array.push(quitDineroFormat($(".CMES7GAA").val()));
            Array.push(quitDineroFormat($(".CMES8GAA").val()));
            Array.push(quitDineroFormat($(".CMES9GAA").val()));
            Array.push(quitDineroFormat($(".CMES10GAA").val()));
            Array.push(quitDineroFormat($(".CMES11GAA").val()));
            Array.push(quitDineroFormat($(".CMES12GAA").val()));
            return Array;
        }

        function DatosVentaAcomulada() {
            var Array = [];
            Array.push(quitDineroFormat($(".CMES1VAM").val()));
            Array.push(quitDineroFormat($(".CMES2VAM").val()));
            Array.push(quitDineroFormat($(".CMES3VAM").val()));
            Array.push(quitDineroFormat($(".CMES4VAM").val()));
            Array.push(quitDineroFormat($(".CMES5VAM").val()));
            Array.push(quitDineroFormat($(".CMES6VAM").val()));
            Array.push(quitDineroFormat($(".CMES7VAM").val()));
            Array.push(quitDineroFormat($(".CMES8VAM").val()));
            Array.push(quitDineroFormat($(".CMES9VAM").val()));
            Array.push(quitDineroFormat($(".CMES10VAM").val()));
            Array.push(quitDineroFormat($(".CMES11VAM").val()));
            Array.push(quitDineroFormat($(".CMES12VAM").val()));
            return Array;
        }

        function DatosResultadoAcomulada() {
            var Array = [];
            Array.push(quitDineroFormat($(".CMES1RAM").val()));
            Array.push(quitDineroFormat($(".CMES2RAM").val()));
            Array.push(quitDineroFormat($(".CMES3RAM").val()));
            Array.push(quitDineroFormat($(".CMES4RAM").val()));
            Array.push(quitDineroFormat($(".CMES5RAM").val()));
            Array.push(quitDineroFormat($(".CMES6RAM").val()));
            Array.push(quitDineroFormat($(".CMES7RAM").val()));
            Array.push(quitDineroFormat($(".CMES8RAM").val()));
            Array.push(quitDineroFormat($(".CMES9RAM").val()));
            Array.push(quitDineroFormat($(".CMES10RAM").val()));
            Array.push(quitDineroFormat($(".CMES11RAM").val()));
            Array.push(quitDineroFormat($(".CMES12RAM").val()));
            return Array;
        }
        init();
    };

    $(document).ready(function () {
        Administrativo.Proyecciones.CifrasPrincipales = new CifrasPrincipales();
    });
})();
