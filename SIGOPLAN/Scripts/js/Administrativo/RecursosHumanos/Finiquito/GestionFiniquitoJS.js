$(function () {
    $.namespace('recursoshumanos.finiquito.gestFiniquito');
    gestFiniquito = function () {
        _Eliminar = 0;
        mensajes = {
            PROCESANDO: 'Procesando...'
        };

        $("#tblDataAut").on("click", "#btnAutFiniquito", function () {
            fnAutFiniquito(1, $("#btnAutFiniquito").data("objetoid"));
        });

        $("#tblDataAut").on("click", "#btnRechFiniquito", function () {
            fnAutFiniquito(2, $("#btnRechFiniquito").data("objetoid"));
        });
        tblDataFiniquitos = $("#tblDataFiniquitos");
        tblDataFiniquitosAut = $("#tblDataFiniquitosAut");
        tblDataFiniquitosRech = $("#tblDataFiniquitosRech");

        txtClave = $("#txtClave");
        txtNombre = $("#txtNombre");
        txtCC = $("#txtCC");

        txtClave2 = $("#txtClave2");
        txtNombre2 = $("#txtNombre2");
        txtCC2 = $("#txtCC2");

        txtClave3 = $("#txtClave3");
        txtNombre3 = $("#txtNombre3");
        txtCC3 = $("#txtCC3");

        btnAplicarFiltros = $("#btnAplicarFiltros");
        btnAplicarFiltros2 = $("#btnAplicarFiltros2");
        btnAplicarFiltros3 = $("#btnAplicarFiltros3");

        ireport = $("#report");

        $('a[href$="#tabFinPendientes"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        $('a[href$="#tabFinAutorizados"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        $('a[href$="#tabFinRechazados"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function init() {
            initTable();
            initTableAutorizados();
            initTableRechazados();

            btnAplicarFiltros.click(fnBuscar);
            btnAplicarFiltros2.click(fnBuscar2);
            btnAplicarFiltros3.click(fnBuscar3);

            setAutoComplete();
        }

        function fnBuscar() {
            tblDataFiniquitos.page(0);
            tblDataFiniquitos.ajax.reload(null, false);
            txtClave.val("");
            txtNombre.val("");
            txtCC.val("");
        }

        function fnBuscar2() {
            tblDataFiniquitosAut.page(0);
            tblDataFiniquitosAut.ajax.reload(null, false);
            txtClave2.val("");
            txtNombre2.val("");
            txtCC2.val("");
        }

        function fnBuscar3() {
            tblDataFiniquitosRech.page(0);
            tblDataFiniquitosRech.ajax.reload(null, false);
            txtClave3.val("");
            txtNombre3.val("");
            txtCC3.val("");
        }

        function setAutoComplete() {
            //lblIgNombre.getAutocomplete(setIdEmpleado, null, '/Administrativo/FormatoCambio/getEmpleados');
            txtCC.getAutocomplete(funSelCC, null, '/Administrativo/FormatoCambio/getCC');
            txtCC2.getAutocomplete(funSelCC2, null, '/Administrativo/FormatoCambio/getCC');
            txtCC3.getAutocomplete(funSelCC3, null, '/Administrativo/FormatoCambio/getCC');
        }

        function funSelCC(event, ui) {
            txtCC.text(ui.item.value);
        }

        function funSelCC2(event, ui) {
            txtCC2.text(ui.item.value);
        }

        function funSelCC3(event, ui) {
            txtCC3.text(ui.item.value);
        }

        function initTable() {
            tblDataFiniquitos = $("#tblDataFiniquitos").DataTable({
                retrieve: true,
                ajax: {
                    url: '/Finiquito/getFiniquitos',
                    dataSrc: 'data',
                    data: function (d) {
                        d.clave = (txtClave.val() != 0 && !isNaN(txtClave.val())) ? unmaskNumero(txtClave.val()) : 0,
                            d.nombre = txtNombre.val(),
                            d.cc = txtCC.val(),
                            d.aut = 1
                        //d.fam = txtFam.val(),
                        //d.lin = txtLin.val()
                    }
                },
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
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
                rowId: 'id',
                scrollX: "100%",
                //"scrollY": datatableHeight(window.screen.height),
                "scrollCollapse": true,
                "deferRender": true,
                "order": [0, 'asc'],
                'initComplete': function (settings, json) {
                    tblDataFiniquitos.on('click', '.btn-detalle', function () {
                        var rowData = tblDataFiniquitos.row($(this).closest('tr')).data();
                        $("#dialogVerFiniquito").dialog({
                            position: { my: "top+50", at: "top", of: window },
                            width: '90%',
                            modal: true,
                            open: function () {
                                var id = rowData["id"];
                                $.ajax({
                                    url: '/Finiquito/GetDetalleFin',
                                    data: { id: id },
                                    success: function (response) {
                                        //addRowReqVer(arr, 1);
                                        //fnResultadosReqVer();
                                        var data = response.data;

                                        fnDetalleFiniquito(data);
                                    }
                                });
                            },
                            buttons: {
                                "Cerrar": function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    });

                    tblDataFiniquitos.on('click', '.btn-excel', function (e) {
                        var rowData = tblDataFiniquitos.row($(this).closest('tr')).data();
                        verReporte(e, null, rowData["id"]);
                    });

                    tblDataFiniquitos.on('click', '.btn-aut', function () {
                        var rowData = tblDataFiniquitos.row($(this).closest('tr')).data();

                        $("#dialogAutFiniquito").dialog({
                            position: { my: "top+50", at: "top", of: window },
                            width: '70%',
                            modal: true,
                            open: function () {
                                $.ajax({
                                    url: '/Finiquito/GetAutorizaciones',
                                    data: { finiquitoID: rowData["id"] },
                                    success: function (response) {
                                        addRowAut(response.data);
                                    }
                                });
                            },
                            buttons: {
                                "Cerrar": function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'claveEmpleado', title: "Clave del Empleado" },
                    { data: 'nombreCompleto', title: "Nombre Completo" },
                    { data: 'fechaAlta', title: "Fecha Ingreso" },
                    { data: 'fechaBaja', title: "Fecha Egreso" },
                    { data: 'cc', title: "Centro de Costos" },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-detalle btn btn-info glyphicon glyphicon-eye-open" type="button" value="' + row.id + '" style="margin-right: 5px;"></button>';
                            html += '<button class="btn-excel btn btn-primary glyphicon glyphicon-print" type="button" value="' + row.id + '" style="margin-right: 5px;"></button>';
                            html += '<button class="btn-aut btn btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '"></button>';
                            return html;
                        },
                        title: ""
                    }
                ],
                columnDefs: [
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        "targets": [2, 3]
                    },
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] }
                ]
            });
        }

        function initTableAutorizados() {
            tblDataFiniquitosAut = $("#tblDataFiniquitosAut").DataTable({
                retrieve: true,
                ajax: {
                    url: '/Finiquito/getFiniquitos',
                    dataSrc: 'data',
                    data: function (d) {
                        d.clave = (txtClave2.val() != 0 && !isNaN(txtClave2.val())) ? unmaskNumero(txtClave2.val()) : 0,
                            d.nombre = txtNombre2.val(),
                            d.cc = txtCC2.val(),
                            d.aut = 2
                    }
                },
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
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
                rowId: 'id',
                scrollX: "100%",
                //"scrollY": datatableHeight(window.screen.height),
                "scrollCollapse": true,
                "deferRender": true,
                "order": [0, 'asc'],
                'initComplete': function (settings, json) {
                    tblDataFiniquitosAut.on('click', '.btn-detalle', function () {
                        var rowData = tblDataFiniquitosAut.row($(this).closest('tr')).data();
                        $("#dialogVerFiniquito").dialog({
                            position: { my: "top+50", at: "top", of: window },
                            width: '90%',
                            modal: true,
                            open: function () {
                                var id = rowData["id"];
                                $.ajax({
                                    url: '/Finiquito/GetDetalleFin',
                                    data: { id: id },
                                    success: function (response) {
                                        var data = response.data;

                                        fnDetalleFiniquito(data);
                                    }
                                });
                            },
                            buttons: {
                                "Cerrar": function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    });

                    tblDataFiniquitosAut.on('click', '.btn-excel', function (e) {
                        var rowData = tblDataFiniquitosAut.row($(this).closest('tr')).data();
                        verReporte(e, null, rowData["id"]);
                    });
                },
                columns: [
                    { data: 'claveEmpleado', title: "Clave del Empleado" },
                    { data: 'nombreCompleto', title: "Nombre Completo" },
                    { data: 'fechaAlta', title: "Fecha Ingreso" },
                    { data: 'fechaBaja', title: "Fecha Egreso" },
                    { data: 'cc', title: "Centro de Costos" },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-detalle btn btn-info glyphicon glyphicon-eye-open" type="button" value="' + row.id + '" style="margin-right: 5px;"></button>';
                            html += '<button class="btn-excel btn btn-primary glyphicon glyphicon-print" type="button" value="' + row.id + '"></button>';
                            return html;
                        },
                        title: ""
                    }
                ],
                columnDefs: [
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        "targets": [2, 3]
                    },
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] }
                ]
            });
        }

        function initTableRechazados() {
            tblDataFiniquitosRech = $("#tblDataFiniquitosRech").DataTable({
                retrieve: true,
                ajax: {
                    url: '/Finiquito/getFiniquitos',
                    dataSrc: 'data',
                    data: function (d) {
                        d.clave = (txtClave3.val() != 0 && !isNaN(txtClave3.val())) ? unmaskNumero(txtClave3.val()) : 0,
                            d.nombre = txtNombre3.val(),
                            d.cc = txtCC3.val(),
                            d.aut = 3
                    }
                },
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
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
                rowId: 'id',
                scrollX: "100%",
                //"scrollY": datatableHeight(window.screen.height),
                "scrollCollapse": true,
                "deferRender": true,
                "order": [0, 'asc'],
                'initComplete': function (settings, json) {
                    tblDataFiniquitosRech.on('click', '.btn-detalle', function () {
                        var rowData = tblDataFiniquitosRech.row($(this).closest('tr')).data();
                        $("#dialogVerFiniquito").dialog({
                            position: { my: "top+50", at: "top", of: window },
                            width: '90%',
                            modal: true,
                            open: function () {
                                var id = rowData["id"];
                                $.ajax({
                                    url: '/Finiquito/GetDetalleFin',
                                    data: { id: id },
                                    success: function (response) {
                                        var data = response.data;

                                        fnDetalleFiniquito(data);
                                    }
                                });
                            },
                            buttons: {
                                "Cerrar": function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    });

                    tblDataFiniquitosRech.on('click', '.btn-excel', function (e) {
                        var rowData = tblDataFiniquitosRech.row($(this).closest('tr')).data();
                        verReporte(e, null, rowData["id"]);
                    });
                },
                columns: [
                    { data: 'claveEmpleado', title: "Clave del Empleado" },
                    { data: 'nombreCompleto', title: "Nombre Completo" },
                    { data: 'fechaAlta', title: "Fecha Ingreso" },
                    { data: 'fechaBaja', title: "Fecha Egreso" },
                    { data: 'cc', title: "Centro de Costos" },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-detalle btn btn-info glyphicon glyphicon-eye-open" type="button" value="' + row.id + '" style="margin-right: 5px;"></button>';
                            html += '<button class="btn-excel btn btn-primary glyphicon glyphicon-print" type="button" value="' + row.id + '"></button>';
                            return html;
                        },
                        title: ""
                    }
                ],
                columnDefs: [
                    {
                        "render": function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        "targets": [2, 3]
                    },
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] }
                ]
            });
        }

        function fnDetalleFiniquito(data) {
            $("#txtIgNumeroEmpleado").val(data.claveEmpleado);
            $("#lblIgNombre").val(data.nombre + " " + data.ape_paterno + " " + data.ape_materno);
            $("#lblIgIngreso").val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.fechaAlta.substr(6)))));
            $("#lblIgEgreso").val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.fechaBaja.substr(6)))));

            if (data.tipoNominaID == 1) {
                var bonoMensual = (data.bono / 7) * 30.4;
                var sueldoMensual = (data.salarioBase / 7) * 30.4;
                var complementoMensual = (data.complemento / 7) * 30.4;

                $("#lblIgSueldoMensual").val(maskNumero(unmaskNumero(sueldoMensual.toString()) + unmaskNumero(complementoMensual.toString()) + unmaskNumero(bonoMensual.toString())));
            }
            else if (data.tipoNominaID == 4) {
                var bonoMensual = data.bono * 2;
                var sueldoMensual = data.salarioBase * 2;
                var complementoMensual = data.complemento * 2;

                $("#lblIgSueldoMensual").val(maskNumero(unmaskNumero(sueldoMensual.toString()) + unmaskNumero(complementoMensual.toString()) + unmaskNumero(bonoMensual.toString())));
            }

            $("#lblIgPuesto").val(data.puesto);
            $("#lblIgCentro").val(data.cc);

            $("#tblDataDetalleFin tbody tr").remove();

            var detalle = data.detalle;

            for (i = 0; i < detalle.length; i++) {
                switch (detalle[i].conceptoID) {
                    case 1:
                        var html = "";
                        html += '<tr>';
                        html += '   <td>';
                        html += '       <div class="col-lg-12">';
                        html += '           <input type="text" class="form-control col-lg-3" id="" value="AGUINALDO" style="border: none; box-shadow: none; background-color: white;" readonly />';
                        html += '       </div>';
                        html += '   </td>';
                        html += '   <td>';
                        html += '       <div class="col-lg-12 text-right">';
                        html += '           <input type="text" id="aguiFijo" class="form-control text-center inputTablaFiniquito" value="' + detalle[i].operacion1 + '" readonly /> x';
                        html += '           <input type="text" id="aguiDiasTrans" class="form-control text-center inputTablaFiniquito" value="' + detalle[i].operacion2 + '" readonly /> =';
                        html += '           <input type="text" id="aguiRes1" class="form-control text-center inputTablaFiniquito" value="' + detalle[i].operacion3 + '" readonly /> x';
                        html += '           <input type="text" id="aguiSalDiario" class="form-control text-right inputTablaFiniquito aguiSalDiario" value="$' + detalle[i].operacion4 + '" readonly /> =';
                        html += '       </div>';
                        html += '   </td>';
                        html += '   <td>';
                        html += '       <div class="col-lg-12">';
                        html += '           <input type="text" id="aguiRes2" class="form-control text-right resultado" value="$' + detalle[i].resultado + '" style="border: none; box-shadow: none; background-color: white;" readonly />';
                        html += '       </div>';
                        html += '   </td>';
                        html += '</tr>';
                        $(html).appendTo($("#tblDataDetalleFin tbody"));
                        break;
                    case 2:
                        var html = "";
                        html += '<tr>';
                        html += '   <td>';
                        html += '       <div class="col-lg-12">';
                        html += '           <input type="text" class="form-control" value="' + detalle[i].conceptoNombre + (detalle[i].conceptoInfo || "") + '" style="border: none; box-shadow: none; background-color: white;" readonly />';
                        html += '       </div>';
                        html += '   </td>';
                        html += '   <td>';
                        html += '       <div class="col-lg-12 text-right vacInput">';
                        html += '           <input type="text" id="" class="form-control text-center inputTablaFiniquito vacFijo" value="' + detalle[i].operacion1 + '" /> x';
                        html += '           <input type="text" id="" class="form-control text-center inputTablaFiniquito vacDiasTrans" value="' + detalle[i].operacion2 + '" /> =';
                        html += '           <input type="text" id="" class="form-control text-center inputTablaFiniquito vacRes1" value="' + detalle[i].operacion3 + '" readonly /> x';
                        html += '           <input type="text" id="aguiSalDiario" class="form-control text-right inputTablaFiniquito aguiSalDiario" value="$' + detalle[i].operacion4 + '" readonly /> =';
                        html += '       </div>';
                        html += '   </td>';
                        html += '   <td>';
                        html += '       <div class="col-lg-12">';
                        html += '          <input type="text" class="form-control text-right resultado" value="$' + detalle[i].resultado + '" style="border: none; box-shadow: none; background-color: white;" readonly />';
                        html += '       </div>';
                        html += '   </td>';
                        html += '</tr>';
                        $(html).appendTo($("#tblDataDetalleFin tbody"));
                        break;
                    default:
                        var html = "";
                        html += '<tr id="' + detalle[i].id + '">';
                        html += '   <td>';
                        html += '       <div class="col-lg-12">';
                        html += '           <input type="text" class="form-control" value="' + detalle[i].conceptoNombre + " " + (detalle[i].conceptoInfo || "") + '" style="border: none; box-shadow: none; background-color: white;" readonly />';
                        html += '       </div>';
                        html += '   </td>';
                        html += '   <td>';
                        html += '       <div class="col-lg-12">';
                        html += '           <input type="text" class="form-control text-right conceptoDetalle" value="' + (detalle[i].conceptoDetalle || "") + '" style="box-shadow: none;" />';
                        html += '       </div>';
                        html += '   </td>';
                        html += '   <td>';
                        html += '       <div class="col-lg-12">';
                        html += '           <input type="text" class="form-control text-right resultado" value="$' + detalle[i].resultado + '" style="box-shadow: none;" />';
                        html += '       </div>';
                        html += '   </td>';
                        html += '</tr>';
                        $(html).appendTo($("#tblDataDetalleFin tbody"));
                        break;
                }

                //var html = "";
                //html += '<tr  id="' + detalle[i].id + '">';
                //html += '   <td class="text-center concepto">';
                //html += '       ' + arr[i].cod;
                //html += '   </td>';
                //html += '   <td class="text-center detalle">';
                //html += '       ' + arr[i].cant;
                //html += '   </td>';
                //html += '   <td class="text-center resultado">';
                //html += '       ' + arr[i].existencia;
                //html += '   </td>';
                //html += '</tr>';
                //$(html).appendTo($("#tblDataDetalleFin tbody"));
            }

            $("#inputTotalFiniquito").val(maskNumero(data.total));
        }

        function addRowAut(arr) {
            $("#tblDataAut tbody tr").remove();

            for (i = 0; i < arr.length; i++) {
                var html = "";
                html += '<tr  id="' + arr[i].id + '">';
                html += '   <td class="text-center orden">';
                html += '       ' + arr[i].orden;
                html += '   </td>';
                html += '   <td class="text-center nombre">';
                html += '       ' + arr[i].nombre + " " + arr[i].ape_paterno + " " + arr[i].ape_materno;
                html += '   </td>';
                html += '   <td class="text-center puesto">';
                html += '       ' + arr[i].puesto;
                html += '   </td>';
                html += '   <td class="text-center estado">';
                html += '       ' + arr[i].estadoAut;
                html += '   </td>';

                if (arr[i].checkUsuario == true) {
                    html += '   <td class="text-center autorizacion">';
                    html += '       <button id="btnAutFiniquito" type="button" class="btn btn-success btn-xs btn-aut-finiquito" role="button" data-autorizaID="' + arr[i].id + '" data-userAutorizaID="' + arr[i].usuarioID + '" data-objetoid="' + arr[i].finiquitoID + '" style="margin-top: 5px; margin-bottom: 5px;"><i class= "icon glyphicon glyphicon-ok"></i> Firmar</button>';
                    html += '       <button id="btnRechFiniquito" type="button" class="btn btn-danger btn-xs btn-rech-finiquito" role="button" data-autorizaID="' + arr[i].id + '" data-userAutorizaID="' + arr[i].usuarioID + '" data-objetoid="' + arr[i].finiquitoID + '" style="margin-top: 5px; margin-bottom: 5px;"><i class= "icon glyphicon glyphicon-remove"></i> Rechazar</button>';
                    html += '   </td>';
                } else {
                    html += '   <td class="text-center autorizacion">';
                    html += '       ';
                    html += '   </td>';
                }

                html += '</tr>';
                $(html).appendTo($("#tblDataAut tbody"));
            }
        }

        function fnAutFiniquito(aut, finiquitoID) {
            $.ajax({
                url: '/Finiquito/AutorizaFiniquito',
                data: { aut: aut, finiquitoID: finiquitoID },
                success: function (response) {
                    var data = response.data;

                    $("#dialogAutFiniquito").dialog("close");

                    tblDataFiniquitos.ajax.reload(null, false);

                    AlertaGeneral("Información Guardada", "Se ha guardado la información con respecto al finiquito.");

                    if (data != null) {
                        verReporteCorreo(data.id, data.claveEmpleado, data.autorizoID);
                    }
                }
            });
        }

        function verReporte(e, selector, data) {

            $.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = "77";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + e.target.value;

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();
                openCRModal();

            };
            e.preventDefault();
        }

        function verReporteCorreo(idFiniquito, claveEmpleado, autorizoID) {

            //$.blockUI({ message: mensajes.PROCESANDO });

            var idReporte = "77";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + idFiniquito + "&inMemory=1";

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {
                $.ajax({
                    datatype: "json",
                    type: "POST",
                    url: '/Finiquito/EnviarCorreos',
                    data: { empleadoClave: claveEmpleado, usuarioID: autorizoID },
                    success: function (response) {

                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
                //$.unblockUI();
                //openCRModal();

            };
        }

        init();
    }

    $(document).ready(function () {
        recursoshumanos.finiquito.gestFiniquito = new gestFiniquito();
    });
});