
(function () {

    $.namespace('administrativo.proyecciones.EstadoPosicionesFinanciera');

    EstadoPosicionesFinanciera = function () {
        var tableDet, tlbSaldoAdjunto;

        mensajes = {
            NOMBRE: 'Proyecciones Financieras',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        },

            btnCargarInfo = $("#btnCargarInfo");
        MensajeConfirmacion = $("#MensajeConfirmacion"),
            TituloConfirmacion = $("#TituloConfirmacion"),
            MensajeConfirmacionModal = $("#MensajeConfirmacionModal"),
            tblPosicionesFinacieras = $("#tblPosicionesFinacieras"),
            cboEscenario = $("#cboEscenario"),
            cboPeriodo = $("#cboPeriodo"),
            tbMesesInicio = $("#tbMesesInicio"),
            tblCxC = $("#tblCxC");
        btnGuardarEPF = $("#btnGuardarEPF");



        function int() {

            //tbMesesInicio.unbind("change");
            //btnCargarInfo.unbind("click");

            btnCargarInfo.click(GetTableData)
            btnGuardarEPF.click(getAllData);
            GetTableData();
            btnGuardarEPF.addClass('hide');
        }
        function LoadData() {
            alert(tbMesesInicio.val());
        }
        function getAllData() {

            var Array = [];

            var tbl = $('table#tblPosicionesFinacieras tr').get().map(function (row) {
                return $(row).find('td').get().map(function (cell) {
                    return $(cell);
                });
            });
            var tbl2 = $('table#tblSaldoAjustado tr').get().map(function (row) {
                return $(row).find('td').get().map(function (cell) {
                    return $(cell);
                });
            });

            var ArregloSaveData = [];
            var idGlobal = 0;
            for (var i = 0; i < tbl2.length; i++) {

                var obj1 = tbl[i];
                var obj2 = tbl2[i];
                if (obj1.length > 1) {

                    var tabla1 = tbl[i];
                    var tabla2 = tbl2[i];

                    ObjetoReturn = {};
                    var grupo = "";
                    var idRegistro = tabla2[6].children().attr('data-id');
                    if (idRegistro <= 5) {
                        grupo = GetGrupo(1);
                    }
                    else if (idRegistro > 5 && i <= 9) {
                        grupo = GetGrupo(2);
                    }
                    else if (idRegistro > 9 && i <= 18) {
                        grupo = GetGrupo(3);
                    }
                    else if (idRegistro > 18) {
                        grupo = GetGrupo(4);
                    }

                    ObjetoReturn.idInicial = idRegistro;
                    ObjetoReturn.Concepto = tabla1[0].text();

                    ObjetoReturn.Inicial = removeCommas(tabla1[1].children().val());
                    ObjetoReturn.Grupo = grupo;
                    ObjetoReturn.idSaldos = idRegistro;
                    ObjetoReturn.D1 = removeCommas(tabla2[0].children().val() == undefined ? "0" : tabla2[0].children().val());
                    ObjetoReturn.D2 = removeCommas(tabla2[2].children().val() == undefined ? "0" : tabla2[2].children().val());
                    ObjetoReturn.D3 = removeCommas(tabla2[4].children().val() == undefined ? "0" : tabla2[4].children().val());
                    ObjetoReturn.H1 = removeCommas(tabla2[1].children().val() == undefined ? "0" : tabla2[1].children().val());
                    ObjetoReturn.H2 = removeCommas(tabla2[3].children().val() == undefined ? "0" : tabla2[3].children().val());
                    ObjetoReturn.H3 = removeCommas(tabla2[5].children().val() == undefined ? "0" : tabla2[5].children().val());
                    ObjetoReturn.Saldo = removeCommas(tabla2[6].children().val() == undefined ? "0" : tabla2[6].children().val());
                    ArregloSaveData.push(ObjetoReturn);
                }

            }
            GuardarData(ArregloSaveData);
        }

        function GuardarData(Array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Proyecciones/GuardarInfoEPF',
                type: 'POST',
                dataType: 'json',
                data: { obj: Array, mes: tbMesesInicio.val(), anio: cboPeriodo.val(), id: idGlobalRegistro },
                //data: { obj: Array, mes: "4", anio: "2017", id: idGlobalRegistro },
                success: function (response) {
                    GetTableData();
                    AlertaGeneral("Confirmación", "El registro a sido actualizado correctamente");
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetValueSelector(Selector, TipoSelector) {
            GetHtml = $.parseHTML(Selector);

            switch (TipoSelector) {
                case 1:
                    var valor = $(GetHtml).val();
                    return valor == "" ? 0 : valor;
                case 2:
                    return $(GetHtml).text();
                default:
            }
        }
        function GetTableData() {
            var mes = GetPeriodoMeses();
            idTituloContainer.text('Estado De Posición Financiera (Captura De Datos Balance Inicial) ' + mes[0]);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/proyecciones/GetFillTableEPF',
                type: 'POST',
                dataType: 'json',
                data: { escenario: cboEscenario.val(), meses: tbMesesInicio.val(), anio: cboPeriodo.val() },
                // data: { escenario: 1, meses: 5, anio: 2017 },
                success: function (response) {
                    $.unblockUI();
                    if (response.EstadoRegreso > 0) {
                        var dataRes = response.GetData;
                        idGlobalRegistro = response.id;
                        LoadTablaPosicionesFinancieras(dataRes);
                        if (response.EstadoRegreso == 1) {
                            btnGuardarEPF.removeClass('hide')
                        } else {
                            btnGuardarEPF.addClass('hide');
                        }


                    }
                    else {

                        var jsonD = {};
                        dataSet = [];
                        jsonD.D1 = 0;
                        jsonD.D2 = 0;
                        jsonD.D3 = 0;
                        jsonD.Grupo = 0;
                        jsonD.H1 = 0;
                        jsonD.H2 = 0;
                        jsonD.H3 = 0;
                        jsonD.Inicial = 0;
                        jsonD.Saldo = 0;
                        jsonD.idInicial = 0;
                        jsonD.idSaldos = 0;
                        for (var i = 0; i < 25; i++) {
                            dataSet.push(jsonD);

                        }
                        idGlobalRegistro = 0;
                        LoadTablaPosicionesFinancieras(dataSet);
                        btnGuardarEPF.addClass('hide');
                    }

                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function LoadTablaPosicionesFinancieras(ListaConceptos) {
            var Array = [];
            var Array2 = [];

            var ListaDetalles = ["<label class='noBold'>Efectivo e Inversiones Temporales</label>",
                "<label class='noBold'>Clientes</label>",
                "<label class='noBold'>Otros Deudores</label>",
                "<label class='noBold'>INVENTARIOS</label>",
                "<label class='noBold'>OTROS ACTIVOS</label>",
                "<label>ACTIVO NO CIRCULANTE</label>",
                "<label class='lblPadding2 noBold'>Activo ya Existente</label>",
                "<label class='lblPadding2 noBold'>Activo Nuevo</label>",
                "<label>ACTIVO DIFERIDO</+label>",
                "<label class='lblPadding3'>Suma el Activo Total</label>",
                "<label class='lblPadding1 noBold'>Documentos e Intereses por Pagar CP</label>",
                "<label class='lblPadding1 noBold'>Proveedores y Contratistas</label>",
                "<label class='lblPadding1 noBold'>Impuestos y Derechos por Pagar</label>",
                "<label class='lblPadding1 noBold'>Otros Gastos Acumulados por Pagar</label>",
                "<label class='lblPadding1 noBold'>Acreedores Diversos</label>",
                "<label class='lblPadding1 noBold'>Documentos e Intereses por Pagar LP</label>",
                "<label class='lblPadding1 noBold'>Compañías del Grupo LP</label>",
                "<label class='lblPadding1 noBold'>Reservas de Pasivo</label>",
                "<label class='lblPadding3'>Suma el Pasivo Total</label>",
                "<label class='lblPadding1 noBold'>Capital Social</label>",
                "<label class='lblPadding1 noBold'>Aport. Futuros Aum. Capital</label>",
                "<label class='lblPadding1 noBold'>Resultado Acum. Ejercicios Anteriores</label>",
                "<label class='lblPadding1 noBold'>Exceso (Insuficiencia) en Actualización</label>",
                "<label class='lblPadding1 noBold'>Resultado del Ejercicio",
                "<label class='lblPadding3'>Suma el Capital Contable",
                "<label class='lblPadding3'>Suma el Pasivo y el Capital",
                "CUADRE",
            ];

            for (var i = 0; i < ListaDetalles.length; i++) {
                var id = i + 1;
                var grupo = "";

                JsonTbl1 = {};
                JsonTbl2 = {};

                JsonTbl1.id = id;
                JsonTbl2.id = id;

                if (i <= 5) {
                    grupo = GetGrupo(1);
                }
                else if (i > 5 && i <= 9) {
                    grupo = GetGrupo(2);
                }
                else if (i > 9 && i <= 18) {
                    grupo = GetGrupo(3);
                }
                else if (i > 18 && i < 26) {
                    grupo = GetGrupo(4);
                }

                JsonTbl1.Concepto = "<label>" + ListaDetalles[i] + "</label>";

                var pSaldo = 0;
                var pInicial = 0;
                if (i != 26) {
                    pSaldo = ListaConceptos[i].Saldo;
                    pInicial = ListaConceptos[i].Inicial;
                }

                if (i == 9) {
                    JsonTbl2.Saldo = GetSelector('CSaldosTotalesActivoEPF decimalSet', pSaldo, "data-id='" + id + "' disabled ");
                    JsonTbl1.Inicial = GetSelector('CSumaActivoEPF decimalSet', pInicial, " disabled ");
                } else
                    if (i == 18) {
                        JsonTbl2.Saldo = GetSelector('CSaldosTotalesPasivoEPF decimalSet', pSaldo, "data-id='" + id + "' disabled ");
                        JsonTbl1.Inicial = GetSelector('CPasivoEPF decimalSet', pInicial, " disabled ");
                    } else
                        if (i == 24) {
                            JsonTbl2.Saldo = GetSelector('CSaldosTotalesCapitalContableEPF decimalSet', pSaldo, "data-id='" + id + "' disabled ");
                            JsonTbl1.Inicial = GetSelector('CCapitalEPF decimalSet', pInicial, " disabled ");
                        } else if (i == 25) {
                            JsonTbl1.Inicial = GetSelector('CCapitalContableEPF decimalSet', pInicial, " disabled ");
                            JsonTbl2.Saldo = GetSelector('CSaldosTotalesCapitalEPF decimalSet', pSaldo, "data-id='" + id + "' disabled ");
                        }
                        else if (i == 26) {
                            JsonTbl1.Inicial = GetSelector('CCUADRETotal decimalSet', ListaConceptos[9].Inicial - ListaConceptos[25].Inicial, "data-id='" + id + "' disabled");
                            JsonTbl2.Saldo = GetSelector('CCUADRETotal decimalSet', ListaConceptos[9].Saldo - ListaConceptos[25].Saldo, "data-id='" + id + "' disabled ");
                        }
                        else {
                            JsonTbl1.Inicial = GetSelector('CValorInicialEPF decimalSet', pInicial, "data-id='" + id + "'");
                            JsonTbl2.Saldo = GetSelector('CSaldoTotalEPF decimalSet', pSaldo, "data-id='" + id + "' disabled ");
                        }



                JsonTbl1.Grupo = grupo;
                JsonTbl2.Grupo = grupo;
                if (i != 9 && i != 18 && i != 25 && i != 24 && i != 26) {
                    JsonTbl2.D1 = GetSelector('CD1EPF decimalSet', ListaConceptos[i].D1, "data-id='" + id + "'");
                    JsonTbl2.D2 = GetSelector('CD2EPF decimalSet', ListaConceptos[i].D2, "data-id='" + id + "'");
                    JsonTbl2.D3 = GetSelector('CD3EPF decimalSet', ListaConceptos[i].D3, "data-id='" + id + "'");

                    JsonTbl2.H1 = GetSelector('CH1EPF decimalSet', ListaConceptos[i].H1, "data-id='" + id + "'");
                    JsonTbl2.H2 = GetSelector('CH2EPF decimalSet', ListaConceptos[i].H2, "data-id='" + id + "'");
                    JsonTbl2.H3 = GetSelector('CH3EPF decimalSet', ListaConceptos[i].H3, "data-id='" + id + "'");
                } else
                    if (i == 24) {
                        JsonTbl2.D1 = GetSelector('CD1EPFTotal ', ListaConceptos[i].D1, "data-id='" + id + "' disabled");
                        JsonTbl2.D2 = GetSelector('CD2EPFTotal ', ListaConceptos[i].D2, "data-id='" + id + "' disabled");
                        JsonTbl2.D3 = GetSelector('CD3EPFTotal ', ListaConceptos[i].D3, "data-id='" + id + "' disabled");

                        JsonTbl2.H1 = GetSelector('CH1EPFTotal ', ListaConceptos[i].H1, "data-id='" + id + "' disabled");
                        JsonTbl2.H2 = GetSelector('CH2EPFTotal ', ListaConceptos[i].H2, "data-id='" + id + "' disabled");
                        JsonTbl2.H3 = GetSelector('CH3EPFTotal ', ListaConceptos[i].H3, "data-id='" + id + "' disabled");
                    } else if (i == 26) {
                        JsonTbl2.D1 = GetSelector('CCUADRED1EPF decimalSet', ListaConceptos[24].D1 - ListaConceptos[24].H1, "data-id='" + id + "' disabled");
                        JsonTbl2.D2 = GetSelector('CCUADRED2EPF decimalSet', ListaConceptos[24].D2 - ListaConceptos[24].H2, "data-id='" + id + "' disabled");
                        JsonTbl2.D3 = GetSelector('CCUADRED3EPF decimalSet', ListaConceptos[24].D3 - ListaConceptos[24].H3, "data-id='" + id + "' disabled");

                        JsonTbl2.H1 = "&nbsp;";
                        JsonTbl2.H2 = "&nbsp;";
                        JsonTbl2.H3 = "&nbsp;";
                    }
                    else {
                        JsonTbl2.D1 = "&nbsp;";
                        JsonTbl2.D2 = "&nbsp;";
                        JsonTbl2.D3 = "&nbsp;";

                        JsonTbl2.H1 = "&nbsp;";
                        JsonTbl2.H2 = "&nbsp;";
                        JsonTbl2.H3 = "&nbsp;";
                    }


                Array.push(JsonTbl1);
                Array2.push(JsonTbl2);
            }

            fillTable(Array);
            fillTableAjuste(Array2);
        }

        function GetGrupo(id) {
            switch (id) {
                case 1:
                    return "ACTIVO";
                case 2:
                    return "ACTIVO FIJO-NETO";
                case 3:
                    return "PASIVO";
                case 4:
                    return "CAPITAL CONTABLE";
                default:
                    return "";
            }
        }

        function fillTable(dataSet) {
            tableDet = $('#tblPosicionesFinacieras').DataTable({
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
                    { "visible": false, "targets": 1 }
                ],
                "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(1, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group SubEncabezadoNoBorder noBorderLeftaRight"><td colspan="3"> <label>' + group + '<label></td></tr>'
                            );
                            last = group;
                        }
                    });
                },
                columns: [
                    { data: "id", "visible": false, "bSortable": false },
                    { data: "Grupo", "bSortable": false },
                    { data: "Concepto", "bSortable": false },
                    { data: "Inicial", "width": "90px", "bSortable": false }
                ],
                "paging": false,
                "info": false
            });

            var previous;

            tableDet.on('focusin', '.CValorInicialEPF', function () {
                previous = $(this).val();
            }).on('change', '.CValorInicialEPF', function () {
                var valorFormat = $(this).val();
                var elemento = $(this)
                var nf = new Intl.NumberFormat();
                var listaElementos = $('.CValorInicialEPF');
                $('.CD1EPF').trigger('change');;
                var SumActivo = 0;
                var SumPasivo = 0;
                var SumCapital = 0;

                for (var i = 0; i < listaElementos.length; i++) {
                    if (i <= 8) {
                        SumActivo += Math.round(Number(removeCommas($(listaElementos[i]).val())));
                    }
                    if (i > 8 && i <= 16) {
                        SumPasivo += Math.round(Number(removeCommas($(listaElementos[i]).val())));
                    }
                    if (i > 16 && i <= 24) {
                        SumCapital += Math.round(Number(removeCommas($(listaElementos[i]).val())));
                    }
                }
                $('.CSumaActivoEPF').val(nf.format(SumActivo));
                $('.CPasivoEPF').val(nf.format(SumPasivo));
                $('.CCapitalEPF').val(nf.format(SumCapital));
                $('.CCapitalContableEPF').val(nf.format(SumPasivo + SumCapital));

                var tempVal = removeCommas(valorFormat);
                elemento.val(nf.format(tempVal));
            });

            // setFixInputs();
        }

        function setFixInputs(dwe) {
            inputChange = $(".decimalSet");
            $.each(inputChange, function (i, e) {
                var nf = new Intl.NumberFormat();
                //  nf.format(SumActivo);
                var tempVal = Math.round($(e).val());
                $(e).val(nf.format(tempVal));
            });
            fnReloadCustomDecialEvent();
        }

        function sumar(elemento) {
            var SumActivo = 0;
            var SumPasivo = 0;
            var SumCapital = 0;

            var listaElementos = $('.CSaldoTotalEPF');
            var id = elemento.attr('data-id');
            var listaDatos = $("[data-id=" + id + "]");

            var ValorInicial = Number(removeCommas($(listaDatos[0]).val()));

            var D1 = Math.round(Number(removeCommas($(listaDatos[1]).val())));
            var H1 = Math.round(Number(removeCommas($(listaDatos[2]).val())));
            var D2 = Math.round(Number(removeCommas($(listaDatos[3]).val())));
            var H2 = Math.round(Number(removeCommas($(listaDatos[4]).val())));
            var D3 = Math.round(Number(removeCommas($(listaDatos[5]).val())));
            var H3 = Math.round(Number(removeCommas($(listaDatos[6]).val())));

            var Total = listaDatos[7];
            var Result = 0;

            if (id < 11) {
                Result = ValorInicial + D1 - H1 + D2 - H2 + D3 - H3;
            }
            else {

                Result = ValorInicial - D1 + H1 - D2 + H2 - D3 + H3;
            }


            var nf = new Intl.NumberFormat();
            $(Total).val(nf.format(Result));

            for (var i = 0; i < listaElementos.length; i++) {
                if (i <= 8) {
                    SumActivo += Number(removeCommas($(listaElementos[i]).val()));
                }
                if (i > 8 && i <= 16) {
                    SumPasivo += Number(removeCommas($(listaElementos[i]).val()));
                }
                if (i > 16 && i <= 24) {
                    SumCapital += Number(removeCommas($(listaElementos[i]).val()));
                }

            }

            $('.CSaldosTotalesActivoEPF').val(nf.format(SumActivo));
            $('.CSaldosTotalesPasivoEPF').val(nf.format(SumPasivo));
            $('.CSaldosTotalesCapitalContableEPF').val(nf.format(SumCapital));
            $('.CSaldosTotalesCapitalEPF').val(nf.format(SumPasivo + SumCapital));
            SumTotalesTable2();
        }
        function SumTotalesTable2() {

            var nf = new Intl.NumberFormat();

            ListaD1 = $('.CD1EPF');
            ListaH1 = $('.CH1EPF');
            ListaD2 = $('.CD2EPF');
            ListaH2 = $('.CH2EPF');
            ListaD3 = $('.CD3EPF');
            ListaH3 = $('.CH3EPF');


            var TotalD1 = GetSumaLineaByElemento(ListaD1);
            $('.CD1EPFTotal').val(nf.format(TotalD1));
            var TotalH1 = GetSumaLineaByElemento(ListaH1);
            $('.CH1EPFTotal').val(nf.format(TotalH1));
            var TotalD2 = GetSumaLineaByElemento(ListaD2);
            $('.CD2EPFTotal').val(nf.format(TotalD2));
            var TotalH2 = GetSumaLineaByElemento(ListaH2);
            $('.CH2EPFTotal').val(nf.format(TotalH2));
            var TotalD3 = GetSumaLineaByElemento(ListaD3);
            $('.CD3EPFTotal').val(nf.format(TotalD3));
            var TotalH3 = GetSumaLineaByElemento(ListaH3);
            $('.CH3EPFTotal').val(nf.format(TotalH3));
        }

        function GetSumaLineaByElemento(Lista) {
            var Suma = 0;
            for (var i = 0; i < Lista.length; i++) {
                var valueParcial = removeCommas($(Lista[i]).val());

                Suma += Math.round(Number(valueParcial));
            }
            return Suma;
        }

        function GetSelector(claseadd, value, extras) {
            return "<input type='text' class='form-control " + claseadd + "' value='" + value + "'" + extras + " >";
        }

        function fillTableAjuste(dataSet) {
            tlbSaldoAdjunto = $('#tblSaldoAjustado').DataTable({
                "bFilter": false,
                destroy: true,
                data: dataSet,
                "columnDefs": [
                    { "visible": false, "targets": 1 }
                ],
                "drawCallback": function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;
                    api.column(1, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group SubEncabezadoNoBorder noBorderLeftaRight"><td colspan="7"><label>&nbsp;</label></td></tr>'
                            );
                            last = group;
                        }
                    });
                },
                columns: [
                    { data: "id", "visible": false, "bSortable": false },
                    { data: "Grupo", "bSortable": false },
                    {
                        data: "D1", "width": "120px", "bSortable": false, "createdCell": function (td, cellData, rowData, row, col) {
                            if (row == 9 || row == 18 || row == 24) {
                                $(td).addClass('noBorders');
                            }
                        }
                    },
                    {
                        data: "H1", "width": "120px", "bSortable": false, "createdCell": function (td, cellData, rowData, row, col) {
                            if (row == 9 || row == 18 || row == 24) {
                                $(td).addClass('noBorders');
                            }
                        }
                    },
                    {
                        data: "D2", "width": "120px", "bSortable": false, "createdCell": function (td, cellData, rowData, row, col) {
                            if (row == 9 || row == 18 || row == 24) {
                                $(td).addClass('noBorders');
                            }
                        }
                    },
                    {
                        data: "H2", "width": "120px", "bSortable": false, "createdCell": function (td, cellData, rowData, row, col) {
                            if (row == 9 || row == 18 || row == 24) {
                                $(td).addClass('noBorders');
                            }
                        }
                    },
                    {
                        data: "D3", "width": "120px", "bSortable": false, "createdCell": function (td, cellData, rowData, row, col) {
                            if (row == 9 || row == 18 || row == 24) {
                                $(td).addClass('noBorders');
                            }
                        }
                    },
                    {
                        data: "H3", "width": "120px", "bSortable": false, "createdCell": function (td, cellData, rowData, row, col) {
                            if (row == 9 || row == 18 || row == 24) {
                                $(td).addClass('noBorders');
                            }
                        }
                    },

                    { data: "Saldo", "width": "120px", "bSortable": false },
                ],
                "paging": false,
                "info": false
            });
            tlbSaldoAdjunto.on('focusin', '.input', function () {

                previous = $(this).val();

            }).on('change', 'input', function () {
                elemento = $(this);
                var nf = new Intl.NumberFormat();
                sumar(elemento);
                var temp = elemento.val();
                var SnC = removeCommas(temp);

                elemento.val(nf.format(SnC))
            });

            SumTotalesTable2();
            setFixInputs('.decimalSet');
            $('input').addClass('dt-body-right');
        }

        $(document).on('change', '.CValorInicialEPF ', function () {
            var valor = redondear($(this).val());
            $(this).val(valor);
        });

        $(document).on('change', '.decimalSet', function () {
            var valor = redondear($(this).val());
            $(this).val(valor);
        });

        function removeCommas(str) {

            while (str.search(",") >= 0) {
                str = (str + "").replace(',', '');
            }
            return str;
        };

        function redondear(valor) {
            console.log('test');
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

        function removeCommas(str) {

            while (str.search(",") >= 0) {
                str = (str + "").replace(',', '');
            }
            return str;
        };

        function CallModal(titulo, mensaje) {
            TituloConfirmacion.text(titulo);
            MensajeConfirmacion.text(mensaje);
            MensajeConfirmacionModal.modal('show');
        }

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

            for (var i = 0; i < MesInicio; i++) {

                tituloMeses.push(months[i] + " " + (Number(periodo) + 1));
            }
            return tituloMeses;

        }
        int();
    };

    $(document).ready(function () {

        administrativo.proyecciones.EstadoPosicionesFinanciera = new EstadoPosicionesFinanciera();
    });
})();