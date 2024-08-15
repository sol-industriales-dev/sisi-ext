$(function () {
    $.namespace('recursoshumanos.finiquito.conceptosFiniquito');
    conceptosFiniquito = function () {
        _Eliminar = 0;
        mensajes = {
            PROCESANDO: 'Procesando...'
        };

        txtConcepto = $("#txtConcepto");
        txtDetalle = $("#txtDetalle");
        txtOperador = $("#txtOperador");

        txtEditConcepto = $("#txtEditConcepto");
        txtEditDetalle = $("#txtEditDetalle");
        txtEditOperador = $("#txtEditOperador");

        txtElimConcepto = $("#txtElimConcepto");
        txtElimDetalle = $("#txtElimDetalle");
        txtElimOperador = $("#txtElimOperador");

        btnNuevoConcepto = $("#btnNuevoConcepto");

        function init() {
            initTable();
            initCbo();

            btnNuevoConcepto.click(fnAgregarConcepto);
        }

        function fnAgregarConcepto() {
            if (txtOperador.val() != "") {
                $.ajax({
                    url: '/Finiquito/GuardarConcepto',
                    type: 'POST',
                    data: { concepto: txtConcepto.val(), detalle: txtDetalle.val(), operador: txtOperador.val() == 1 ? true : false },
                    success: function (response) {
                        txtConcepto.val("");
                        txtDetalle.val("");
                        txtOperador.val("");
                        tblDataFiniquitosConceptos.ajax.reload(null, false);

                        AlertaGeneral("Información Guardada", "Se ha guardado la información del concepto nuevo.");
                    }
                });
            } else {
                AlertaGeneral("Alerta", "Seleccione un operador.");
            }
        }

        function initCbo() {
            txtOperador.fillCombo('/Finiquito/FillComboOperador', null, false, null);
            txtEditOperador.fillCombo('/Finiquito/FillComboOperador', null, false, null);
            txtElimOperador.fillCombo('/Finiquito/FillComboOperador', null, false, null);
        }

        function initTable() {
            tblDataFiniquitosConceptos = $("#tblDataFiniquitosConceptos").DataTable({
                retrieve: true,
                ajax: {
                    url: '/Finiquito/GetConceptos',
                    dataSrc: 'data',
                    data: function (d) {
                        //d.clave = (txtClave.val() != 0 && !isNaN(txtClave.val())) ? unmaskNumero(txtClave.val()) : 0,
                        //d.nombre = txtNombre.val(),
                        //d.cc = txtCC.val()
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
                    tblDataFiniquitosConceptos.on('click', '.btn-editar', function () {
                        var rowData = tblDataFiniquitosConceptos.row($(this).closest('tr')).data();
                        $("#dialogEditarConcepto").dialog({
                            position: { my: "top+50", at: "top", of: window },
                            width: '30%',
                            modal: true,
                            open: function () {
                                var id = rowData["id"];
                                $.ajax({
                                    url: '/Finiquito/GetDetalleConcepto',
                                    data: { id: id },
                                    success: function (response) {
                                        var data = response.data;

                                        txtEditConcepto.val(data.concepto);
                                        txtEditDetalle.val(data.detalle);
                                        txtEditOperador.val((data.operador == true || data.operador == "True") ? 1 : 0);
                                    }
                                });
                            },
                            buttons: {
                                "Guardar": function () {
                                    if (txtEditOperador != "") {
                                        $.ajax({
                                            url: '/Finiquito/UpdateConcepto',
                                            data: { id: rowData["id"], concepto: txtEditConcepto.val(), detalle: txtEditDetalle.val(), operador: txtEditOperador.val() == 1 ? true : false },
                                            success: function (response) {
                                                tblDataFiniquitosConceptos.ajax.reload(null, false);
                                                $("#dialogEditarConcepto").dialog("close");
                                                AlertaGeneral("Información Guardada", "Se ha actualizado la información del concepto.");
                                            }
                                        });
                                    } else {
                                        AlertaGeneral("Alerta", "Seleccione un operador.");
                                    }
                                },
                                "Cerrar": function () {
                                    $("#dialogEditarConcepto").dialog("close");
                                }
                            }
                        });
                    });

                    tblDataFiniquitosConceptos.on('click', '.btn-eliminar', function () {
                        var rowData = tblDataFiniquitosConceptos.row($(this).closest('tr')).data();
                        $("#dialogEliminarConcepto").dialog({
                            position: { my: "top+50", at: "top", of: window },
                            width: '30%',
                            modal: true,
                            open: function () {
                                var id = rowData["id"];
                                $.ajax({
                                    url: '/Finiquito/GetDetalleConcepto',
                                    data: { id: id },
                                    success: function (response) {
                                        var data = response.data;

                                        txtElimConcepto.val(data.concepto);
                                        txtElimDetalle.val(data.detalle);
                                        txtElimOperador.val((data.operador == true || data.operador == "True") ? 1 : 0);
                                    }
                                });
                            },
                            buttons: {
                                "Eliminar": function () {
                                    $.ajax({
                                        url: '/Finiquito/RemoveConcepto',
                                        data: { id: rowData["id"] },
                                        success: function (response) {
                                            tblDataFiniquitosConceptos.ajax.reload(null, false);
                                            $("#dialogEliminarConcepto").dialog("close");
                                            AlertaGeneral("Concepto Eliminado", "Se ha eliminado la información del concepto.");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $(this).dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    {
                        data: 'concepto', title: "Concepto", render: function (data, type, row, meta) {
                            if (row.operador == "True" || row.operador == true) {
                                return "(+) " + row.concepto;
                            } else {
                                return "(-) " + row.concepto;
                            }
                        }
                    },
                    { data: 'detalle', title: "Detalle" },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-editar btn btn-warning btn-sm glyphicon glyphicon-cog" type="button" value="' + row.id + '" style="margin-right: 5px;"></button>';
                            html += '<button class="btn-eliminar btn btn-danger btn-sm glyphicon glyphicon-remove" type="button" value="' + row.id + '" style="margin-right: 5px;"></button>';
                            return html;
                        },
                        title: ""
                    }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2] },
                    { "width": "50%", "targets": [0] },
                    { "width": "40%", "targets": [1] },
                    { "width": "10%", "targets": [2] },
                ]
            });
        }

        init();
    }

    $(document).ready(function () {
        recursoshumanos.finiquito.conceptosFiniquito = new conceptosFiniquito();
    });
});