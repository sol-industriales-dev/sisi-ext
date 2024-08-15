(function () {

    $.namespace('planActividades.cuadrillasCatalogo');

    cuadrillasCatalogo = function () {

        tblCuadrilla = $("#tblCuadrilla");

        btnNuevaCuadrilla = $("#btnNuevaCuadrilla");
        btnGuardarCuadrilla = $("#btnGuardarCuadrilla");
        btnEditarCuadrilla = $("#btnEditarCuadrilla");
        btnBuscar_Cuadrilla = $("#btnBuscar_Cuadrilla");

        $("#tblDataUsuariosCuadrilla").on({
            click: function () {
                var row = $(this).closest('tr');

                if ($("#tblDataUsuariosCuadrilla tbody tr").length <= 1) {
                    row.remove();

                    var html = '';

                    html += '<tr>';
                    html += '    <td class="text-center">';
                    html += '        <select class="form-control usuarioCuadrilla"></select>';
                    html += '    </td>';
                    html += '    <td class="text-center">';
                    html += '        <button class="btn btn-sm btn-danger btn-eliminar-usuarioCuadrilla"><span class="glyphicon glyphicon-remove"></span></button>';
                    html += '    </td>';
                    html += '</tr>';

                    $(html).appendTo($("#tblDataUsuariosCuadrilla tbody"));

                    recargarListasUsuarios();
                } else {
                    row.remove();
                }
            }
        }, ".btn-eliminar-usuarioCuadrilla");

        $("#tblDataEditUsuariosCuadrilla").on({
            click: function () {
                var row = $(this).closest('tr');

                if ($("#tblDataEditUsuariosCuadrilla tbody tr").length <= 1) {
                    row.remove();

                    var html = '';

                    html += '<tr>';
                    html += '    <td class="text-center">';
                    html += '        <select class="form-control usuarioCuadrillaEdit"></select>';
                    html += '    </td>';
                    html += '    <td class="text-center">';
                    html += '        <button class="btn btn-sm btn-danger btn-eliminar-usuarioCuadrillaEdit"><span class="glyphicon glyphicon-remove"></span></button>';
                    html += '    </td>';
                    html += '</tr>';

                    $(html).appendTo($("#tblDataEditUsuariosCuadrilla tbody"));

                    recargarListasUsuariosEdit();
                } else {
                    row.remove();
                }
            }
        }, ".btn-eliminar-usuarioCuadrillaEdit");

        $("#tblDataUsuariosCuadrilla").on({
            change: function () {
                var row = $(this).closest('tr');

                var index = $('tr').index(row);
                var lastIndex = $('tr').index($('#tblDataUsuariosCuadrilla tbody tr:last'));

                if (index == lastIndex) {
                    var html = '';

                    html += '<tr>';
                    html += '    <td class="text-center">';
                    html += '        <select class="form-control usuarioCuadrilla"></select>';
                    html += '    </td>';
                    html += '    <td class="text-center">';
                    html += '        <button class="btn btn-sm btn-danger btn-eliminar-usuarioCuadrilla"><span class="glyphicon glyphicon-remove"></span></button>';
                    html += '    </td>';
                    html += '</tr>';

                    $(html).appendTo($("#tblDataUsuariosCuadrilla tbody"));

                    recargarListasUsuarios();
                }
            }
        }, ".usuarioCuadrilla");

        $("#tblDataEditUsuariosCuadrilla").on({
            change: function () {
                var row = $(this).closest('tr');

                var index = $('tr').index(row);
                var lastIndex = $('tr').index($('#tblDataEditUsuariosCuadrilla tbody tr:last'));

                if (index == lastIndex) {
                    var html = '';

                    html += '<tr>';
                    html += '    <td class="text-center">';
                    html += '        <select class="form-control usuarioCuadrillaEdit"></select>';
                    html += '    </td>';
                    html += '    <td class="text-center">';
                    html += '        <button class="btn btn-sm btn-danger btn-eliminar-usuarioCuadrillaEdit"><span class="glyphicon glyphicon-remove"></span></button>';
                    html += '    </td>';
                    html += '</tr>';

                    $(html).appendTo($("#tblDataEditUsuariosCuadrilla tbody"));

                    recargarListasUsuariosEdit();
                }
            }
        }, ".usuarioCuadrillaEdit");

        $('a[href$="#tabCuadrilla"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function init() {
            initTableCuadrilla();

            initCbo();

            btnNuevaCuadrilla.click(fnAgregarCuadrilla);
            btnGuardarCuadrilla.click(fnGuardarCuadrilla);
            btnEditarCuadrilla.click(fnEditarCuadrilla);
            btnBuscar_Cuadrilla.click(fnBuscarCuadrilla);
        }

        function fnAgregarCuadrilla() {
            $("#txtCuadrillaDesc").val("");

            $("#tblDataUsuariosCuadrilla tbody tr").remove();
            var html = '';

            html += '<tr>';
            html += '    <td class="text-center">';
            html += '        <select class="form-control usuarioCuadrilla"></select>';
            html += '    </td>';
            html += '    <td class="text-center">';
            html += '        <button class="btn btn-sm btn-danger btn-eliminar-usuarioCuadrilla"><span class="glyphicon glyphicon-remove"></span></button>';
            html += '    </td>';
            html += '</tr>';

            $(html).appendTo($("#tblDataUsuariosCuadrilla tbody"));

            recargarListasUsuarios();

            $("#dialogNuevaCuadrilla").modal('show');

            $("#txtCuadrillaDesc").focus();
        }
        function fnEditarCuadrilla() {
            var arr = new Array();

            $("#tblDataEditUsuariosCuadrilla tbody tr").each(function (index) {
                var row = $(this);

                if (row.find('.usuarioCuadrillaEdit').val() != "") {
                    var obj = { id: parseInt(row.find('.usuarioCuadrillaEdit').val()) };

                    arr.push(obj);
                }
            });

            $.ajax({
                url: '/MAZDA/PlanActividades/EditarCuadrilla',
                datatype: "json",
                type: "POST",
                data: {
                    id: $("#btnEditarCuadrilla").val(),
                    desc: $("#txtEditCuadrillaDesc").val(),
                    personal: arr
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información de la cuadrilla.");
                    recargarTodo();
                }
            });
        }
        function fnBuscarCuadrilla() {
            tblCuadrilla.ajax.reload(null, false);
            //$("#txtCuadrilla").val("");
            //$("#txtPersonal").val("");
        }
        function fnGuardarCuadrilla() {
            if ($("#txtCuadrillaDesc").val() != "") {
                var arr = new Array();

                $("#tblDataUsuariosCuadrilla tbody tr").each(function (index) {
                    var row = $(this);

                    if (row.find('.usuarioCuadrilla').val() != "") {
                        var obj = { id: parseInt(row.find('.usuarioCuadrilla').val()) };

                        arr.push(obj);
                    }
                });

                $.ajax({
                    url: '/MAZDA/PlanActividades/GuardarCuadrilla',
                    datatype: "json",
                    type: "POST",
                    data: {
                        desc: $("#txtCuadrillaDesc").val(),
                        personal: arr
                    },
                    success: function (data) {
                        AlertaGeneral("Información Guardada", "Se ha guardado la información de la cuadrilla.");
                        recargarTodo();
                    }
                });
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }

        function initTableCuadrilla() {
            tblCuadrilla = $("#tblCuadrilla").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/PlanActividades/GetCuadrillas',
                    data: function (data) {
                        data.cuadrillaID = $("#txtCuadrilla").val() != null && $("#txtCuadrilla").val() != "" ? $("#txtCuadrilla").val() : 0,
                        data.personal = $("#txtPersonal").val()
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
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                //scrollX: "100%",
                "scrollY": "250px",
                "scrollCollapse": true,
                'initComplete': function (settings, json) {
                    tblCuadrilla.on('click', '.btn-editar-cua', function () {
                        var rowData = tblCuadrilla.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/MAZDA/PlanActividades/GetCuadrilla',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                var data = response.data;

                                $("#btnEditarCuadrilla").val(rowData["id"]);

                                $("#txtEditCuadrillaDesc").val("");
                                $("#tblDataEditUsuariosCuadrilla tbody tr").remove();

                                $("#dialogEditarCuadrilla").modal('show');

                                $("#txtEditCuadrillaDesc").val(data.descripcion);

                                for (i = 0; i < data.personalLista.length; i++) {
                                    var html = '';

                                    html += '<tr>';
                                    html += '    <td class="text-center">';
                                    html += '        <select class="form-control usuarioCuadrillaEdit"></select>';
                                    html += '    </td>';
                                    html += '    <td class="text-center">';
                                    html += '        <button class="btn btn-sm btn-danger btn-eliminar-usuarioCuadrillaEdit"><span class="glyphicon glyphicon-remove"></span></button>';
                                    html += '    </td>';
                                    html += '</tr>';

                                    $(html).appendTo($("#tblDataEditUsuariosCuadrilla tbody"));

                                    $(".usuarioCuadrillaEdit:last").fillCombo('/MAZDA/PlanActividades/GetUsuariosList', null, false);

                                    $(".usuarioCuadrillaEdit:last").val(data.personalLista[i].id);
                                }

                                var html2 = '';

                                html2 += '<tr>';
                                html2 += '    <td class="text-center">';
                                html2 += '        <select class="form-control usuarioCuadrillaEdit"></select>';
                                html2 += '    </td>';
                                html2 += '    <td class="text-center">';
                                html2 += '        <button class="btn btn-sm btn-danger btn-eliminar-usuarioCuadrillaEdit"><span class="glyphicon glyphicon-remove"></span></button>';
                                html2 += '    </td>';
                                html2 += '</tr>';

                                $(html2).appendTo($("#tblDataEditUsuariosCuadrilla tbody"));

                                $(".usuarioCuadrillaEdit:last").fillCombo('/MAZDA/PlanActividades/GetUsuariosList', null, false);
                            }
                        });
                    });

                    tblCuadrilla.on('click', '.btn-eliminar-cua', function () {
                        var rowData = tblCuadrilla.row($(this).closest('tr')).data();
                        $("#dialogBajaCuadrilla").dialog({
                            modal: true,
                            open: function () {
                                $("#txtElimCuadrilla").text("¿Está seguro que desea eliminar la cuadrilla '" + rowData["descripcion"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/MAZDA/PlanActividades/RemoveCuadrilla',
                                        data: { id: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogBajaCuadrilla").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogBajaCuadrilla").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'descripcion', title: 'Cuadrilla' },
                    { data: 'personal', title: 'Personal' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-editar-cua btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-eliminar-cua btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": "_all" }
                    //{ "width": "90%", "targets": [0] }
                ]
            });
        }

        function initCbo() {
            $(".usuarioCuadrilla").fillCombo('/MAZDA/PlanActividades/GetUsuariosList', null, false);

            $("#txtCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
        }

        function recargarTodo() {
            tblCuadrilla.ajax.reload(null, false);

            $("#dialogNuevaCuadrilla").modal('hide');

            $("#dialogEditarCuadrilla").modal('hide');

            initCbo();
        }

        function recargarListasUsuarios() {
            $(".usuarioCuadrilla:last").fillCombo('/MAZDA/PlanActividades/GetUsuariosList', null, false);
        }

        function recargarListasUsuariosEdit() {
            $(".usuarioCuadrillaEdit:last").fillCombo('/MAZDA/PlanActividades/GetUsuariosList', null, false);
        }

        init();
    };

    $(document).ready(function () {
        planActividades.cuadrillasCatalogo = new cuadrillasCatalogo();
    });

})();