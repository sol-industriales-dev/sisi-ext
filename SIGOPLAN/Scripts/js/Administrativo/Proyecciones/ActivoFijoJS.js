(function () {
    $.namespace('administrativo.proyecciones.ActivoFijo');

    ActivoFijo = function () {
        var idGlobalRegistro = 0;
        mensajes = {
            NOMBRE: 'Proyecciones Financieras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },

        tblActivoFijo = $("#tblActivoFijo"),
        lblEncabezadoActivoFijo = $("#lblEncabezadoActivoFijo"),
        btnGuardarActivoFijoAF = $("#btnGuardarActivoFijoAF"),
        //Modal nueva area
        modalNewAreaAF = $("#modalNewAreaAF"),
        btnAddAreaAF = $("#btnAddAreaAF"),
        btnGuardarNuevaAreaAF = $("#btnGuardarNuevaAreaAF"),
        idModalDescripcionAreasAF = $("#idModalDescripcionAreasAF"),
        //Modal nuevo registro
        modalNewRegistroAF = $("#modalNewRegistroAF"),
        btnAddRegistroAF = $("#btnAddRegistroAF"),
        btnGuardarRegistroAF = $("#btnGuardarRegistroAF"),
        txtModalConceptoAF = $("#idModalConceptoAF"),
        idModalObraAreaAF = $("#idModalObraAreaAF"),
        idModalNumObraAF = $("#idModalNumObraAF"),
        btnBuscarActivoFijoAF = $("#btnBuscarActivoFijoAF"),
        //Filtros
        tbMesesInicio = $("#tbMesesInicio"),
        cboPeriodo = $("#cboPeriodo"),
        btnCargarInfo = $("#btnCargarInfo");
        var idRegistro = 0;

        function init() {
            InitModal();
            LoadTableAF();
            btnAddAreaAF.click(OpenModalArea);
            btnAddRegistroAF.click(OpenModal);
            btnGuardarNuevaAreaAF.click(SaveAreas);
            btnGuardarActivoFijoAF.click(GuardarActivoFijo);
            idModalObraAreaAF.fillCombo('/proyecciones/GetCboAreas');
            idModalNumObraAF.fillCombo('/proyecciones/GetCboObras');
            btnCargarInfo.click(LoadTableAF);
        }
        function InitModal() {
            dialog = modalNewRegistroAF.dialog({
                autoOpen: false,
                resizable: true,
                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",

            });
            dialog1 = modalNewAreaAF.dialog({
                autoOpen: false,
                resizable: true,

                draggable: false,
                modal: true,
                height: "auto",
                width: "auto",

            });
        }

        function OpenModal() {
            limpiarCampos();
            idModalObraAreaAF.fillCombo('/proyecciones/GetCboAreas');
            dialog.dialog("open");
        }
        function OpenModalArea() {
            dialog1.dialog("open");
        }

        function limpiarCampos() {
            $("#idModalDescripcionAreasAF").val('');
            $("#idModalConceptoAF").val('');
            idModalObraAreaAF.val('');
        }

        function LoadTblActivoFijo(dataSet, tblTotal) {
            if (dataSet == undefined) {
                dataSet = [];
            }
            var Array = [];
            for (var i = 0; i < dataSet.length; i++) {
                JsonTbl = {};
                JsonTbl.id = dataSet[i].id;
                JsonTbl.Concepto = dataSet[i].Concepto;
                JsonTbl.Area = dataSet[i].Area;
                JsonTbl.NumObra = dataSet[i].NumObra;
                JsonTbl.Fecha1 = dataSet[i].Fecha1;
                JsonTbl.Fecha2 = dataSet[i].Fecha2;
                JsonTbl.Fecha3 = dataSet[i].Fecha3;
                JsonTbl.Fecha4 = dataSet[i].Fecha4;
                JsonTbl.Fecha5 = dataSet[i].Fecha5;
                JsonTbl.Fecha6 = dataSet[i].Fecha6;
                JsonTbl.Fecha7 = dataSet[i].Fecha7;
                JsonTbl.Fecha8 = dataSet[i].Fecha8;
                JsonTbl.Fecha9 = dataSet[i].Fecha9;
                JsonTbl.Fecha10 = dataSet[i].Fecha10;
                JsonTbl.Fecha11 = dataSet[i].Fecha11;
                JsonTbl.Fecha12 = dataSet[i].Fecha12;
                JsonTbl.Total = dataSet[i].Total;
                Array.push(JsonTbl);
            }
            if (Array.length > 0) {
                for (var i = 0; i < 1; i++) {
                    JsonTbl = {};
                    JsonTbl.id = "";
                    JsonTbl.Concepto = "";
                    JsonTbl.Area = "";
                    JsonTbl.NumObra = tblTotal[0].NumObra;
                    JsonTbl.Fecha1 = tblTotal[0].Fecha1;
                    JsonTbl.Fecha2 = tblTotal[0].Fecha2;
                    JsonTbl.Fecha3 = tblTotal[0].Fecha3;
                    JsonTbl.Fecha4 = tblTotal[0].Fecha4;
                    JsonTbl.Fecha5 = tblTotal[0].Fecha5;
                    JsonTbl.Fecha6 = tblTotal[0].Fecha6;
                    JsonTbl.Fecha7 = tblTotal[0].Fecha7;
                    JsonTbl.Fecha8 = tblTotal[0].Fecha8;
                    JsonTbl.Fecha9 = tblTotal[0].Fecha9;
                    JsonTbl.Fecha10 = tblTotal[0].Fecha10;
                    JsonTbl.Fecha11 = tblTotal[0].Fecha11;
                    JsonTbl.Fecha12 = tblTotal[0].Fecha12;
                    JsonTbl.Total = tblTotal[0].Total;
                }

                Array.push(JsonTbl);
            }
            fillTblActivoFijo(Array);
        }
        var tableDet = $('#tblActivoFijo').DataTable({});

        function fillTblActivoFijo(dataSet) {
            var tituloMeses = [];
            var date = new Date();

            tableDet.clear().draw();
            tableDet.destroy();
            lblEncabezadoActivoFijo.text("Activo fijo de " + (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear());
            tituloMeses = GetPeriodoMeses();
            tableDet = $("#tblActivoFijo").DataTable({
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
                "order": false,
                destroy: true,
                data: dataSet,
                "columnDefs": [
                    { "visible": false, "targets": 0 }
                ],
                columns: [
                      { data: "id" },
                      { data: "Concepto" },
                      { data: "Area" },
                      { data: "NumObra" },
                      { "title": tituloMeses[0], data: "Fecha1", "width": "90px" },
                      { "title": tituloMeses[1], data: "Fecha2", "width": "90px" },
                      { "title": tituloMeses[2], data: "Fecha3", "width": "90px" },
                      { "title": tituloMeses[3], data: "Fecha4", "width": "90px" },
                      { "title": tituloMeses[4], data: "Fecha5", "width": "90px" },
                      { "title": tituloMeses[5], data: "Fecha6", "width": "90px" },
                      { "title": tituloMeses[6], data: "Fecha7", "width": "90px" },
                      { "title": tituloMeses[7], data: "Fecha8", "width": "90px" },
                      { "title": tituloMeses[8], data: "Fecha9", "width": "90px" },
                      { "title": tituloMeses[9], data: "Fecha10", "width": "90px" },
                      { "title": tituloMeses[10], data: "Fecha11", "width": "90px" },
                      { "title": tituloMeses[11], data: "Fecha12", "width": "90px" },
                      { data: "Total" }
                ],
                "paging": false,
                "info": false
            });
            $('input').addClass('dt-body-right');
            $('td').addClass('dt-body-right');
            var previous;

            tableDet.on('focusin', '.mes', function () {
                previous = $(this).val();
            }).on('change', '.mes', function () {
                elemento = $(this);
                sumar(elemento, previous);
            });;

        }


        function sumar(elemento, previous) {

            var rowValue = elemento.parents('tr').children().find('.mes');
            lblTotal = elemento.parents('tr').children().find('.lblTotal');
            var Total = 0;
            for (var i = 0; i < rowValue.length; i++) {
                Total += +($(rowValue[i]).val().replace(/[^0-9\.]/g, ''));
            }
            lblTotal.text(formatCurrency(Math.round(Total, 2)));

            for (var i = 0; i < 13; i++) {
                var arrGeneral = [];
                var lblTotalFecha = 0;
                $('#tblActivoFijo > tbody > tr').each(function (index, e) {
                    lblTotalFecha += +(("" + $(e).find('.fecha' + i).val()).replace(/[^0-9\.]/g, ''));
                });
                $(".lblTotalFecha" + i).text(formatCurrency(Math.round(lblTotalFecha, 2)));

            }

        }

        function formatCurrency(total) {
            var neg = false;
            if (total < 0) {
                neg = true;
                total = Math.abs(total);
            }
            return (neg ? "-$" : '$') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
        }

        function GetSelector(claseadd, element, index, valor) {
            return "<input type='number' id = '" + element + "_" + index + "' class='form-control " + claseadd + "' value ='" + valor + "'>";
        }

        function GuardarActivoFijo() {
            var obj = getActivoFijoObj();
            if (obj != null) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/proyecciones/GuardarActivoFijo',
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    data: JSON.stringify({ obj: obj }),
                    success: function (response) {
                        txtModalConceptoAF.val('');
                        idModalObraAreaAF.val('');
                        idModalNumObraAF.val('');
                        dialog.dialog("close");
                        ConfirmacionGeneral('Confirmacion', 'El registro se guardo Correctamente');
                        LoadTableAF();
                        $.unblockUI();
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
            else {
                AlertaGeneral("Alerta", "Se debe elegir un area.");
            }
        }

        function SaveAreas() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GuardarNuevaArea',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: idModalDescripcionAreasAF.val() }),
                success: function (response) {
                    idModalDescripcionAreasAF.val('');
                    dialog1.dialog("close");
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function LoadTableAF() {
            var mes = GetPeriodoMeses();
         //   idTituloContainer.text('Proyección de adquisición de activo fijo ' + mes[0]);
            $.blockUI({ message: mensajes.PROCESANDO });
            var objFiltro = getFGB();
            $.ajax({
                url: '/proyecciones/GetFillTableAF',
                type: 'POST',
                dataType: 'json',
                data: { objFiltro: objFiltro },
                success: function (response) {
                    var tblTotal = response.GetTotal;
                    var Estatus = response.estatus;

                    if (response.EstadoRegreso > 0) {
                        var dataRes = response.GetData;
                        idGlobalRegistro = response.id;
                        LoadTblActivoFijo(dataRes, tblTotal);
                        setFixInputs();

                        if (response.EstadoRegreso == 1) {
                            btnGuardarActivoFijoAF.removeClass('hide');
                            btnAddRegistroAF.removeClass('hide');
                        } else {
                            btnGuardarActivoFijoAF.addClass('hide');
                            btnAddRegistroAF.addClass('hide');
                        }
                    } else {
                        LoadTblActivoFijo(null);
                        btnGuardarActivoFijoAF.addClass('hide');
                        btnAddRegistroAF.addClass('hide');
                    }
                  

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function setFixInputs() {
            inputChange = $(".mes");
            $.each(inputChange, function (i, e) {
                $(e).DecimalFixNS(2);
            });
            fnReloadCustomDecialEvent();
        }

        function redondear(valor) {
            var sinComas = removeCommas(valor);
            var redondeado = Math.round(sinComas);
            var conCommas = addCommas(redondeado.toFixed(2));
            return conCommas;
        }

        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }

        $(document).on('change', '.mes', function () {
            var valor = redondear($(this).val());
            $(this).val(valor);
        });

        function GetPeriodoMeses() {
            var periodo = cboPeriodo.val();
            var MesInicio = tbMesesInicio.val();
            var months = ["ENE", "FEB", "MAR", "ABR", "MAY", "JUN",
                          "JUL", "AGO", "SEP", "OCT", "NOV", "DIC"];
            var tituloMeses = [];

            var count = 0;
            for (var i = MesInicio; i < 12; i++) {
                count++;
                tituloMeses.push(months[i] + " " + periodo);
            }

            for (var i = 0 ; i < MesInicio; i++) {
                tituloMeses.push(months[i] + " " + (Number(periodo) + 1));
            }
            return tituloMeses;

        }

        function getActivoFijoObj() {

            var Array = [];
            var tbl = $('table#tblActivoFijo tr').get().map(function (row) {
                return $(row).find('td').get().map(function (cell) {
                    return $(cell);
                });

            });

            $.each(tbl, function (index, value) {
                var JsonData = {};
                if (value.length != 0 && $(value[0]).text() != "Ningún dato disponible en esta tabla" && $(value[0]).text() != "") {
                    JsonData.ID = 0;
                    JsonData.Concepto = $(value[0]).text();
                    JsonData.Area = $(value[1]).text();
                    JsonData.NumObra = $(value[2]).text();
                    JsonData.Fecha1 = getValueHtml(value[3]);
                    JsonData.Fecha2 = getValueHtml(value[4]);
                    JsonData.Fecha3 = getValueHtml(value[5]);
                    JsonData.Fecha4 = getValueHtml(value[6]);
                    JsonData.Fecha5 = getValueHtml(value[7]);
                    JsonData.Fecha6 = getValueHtml(value[8]);
                    JsonData.Fecha7 = getValueHtml(value[9]);
                    JsonData.Fecha8 = getValueHtml(value[10]);
                    JsonData.Fecha9 = getValueHtml(value[11]);
                    JsonData.Fecha10 = getValueHtml(value[12]);
                    JsonData.Fecha11 = getValueHtml(value[13]);
                    JsonData.Fecha12 = getValueHtml(value[14]);
                    //JsonData.Total = $(value[15]).text();
                    Array.push(JsonData);
                }
            });
            if ($("#idModalObraAreaAF").val() != '') {
                var NuevoRegistro = {
                    id: 0,
                    Concepto: txtModalConceptoAF.val(),
                    Area: $("#idModalObraAreaAF").val() != '' ? $("#idModalObraAreaAF option:selected").text() : '',
                    Obra: $("#idModalNumObraAF").val() != '' ? $("#idModalNumObraAF option:selected").text() : '',
                    Fecha1: 0,
                    Fecha2: 0,
                    Fecha3: 0,
                    Fecha4: 0,
                    Fecha5: 0,
                    Fecha6: 0,
                    Fecha7: 0,
                    Fecha8: 0,
                    Fecha9: 0,
                    Fecha10: 0,
                    Fecha11: 0,
                    Fecha12: 0
                    //Total: 0
                };

                if (NuevoRegistro.Concepto != "" && NuevoRegistro.Concepto != undefined) { Array.push(NuevoRegistro); }

            }
            var jsonString = JSON.stringify(Array, null, 0);
            var objFiltro = getFGB();
            return {
                id: idGlobalRegistro,
                CadenaJson: jsonString,
                Estatus: true,
                Mes: objFiltro.mes,
                Anio: objFiltro.anio,
            }
        }

        function getValueHtml(cadena) {
            return $(cadena).children().children('input').val();
        }


        init();

    };

    $(document).ready(function () {
        administrativo.proyecciones.ActivoFijo = new ActivoFijo();
    });
})();