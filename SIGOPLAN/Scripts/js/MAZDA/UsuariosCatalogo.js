(function () {

    $.namespace('planActividades.usuariosCatalogo');

    usuariosCatalogo = function () {

        tblUsuario = $("#tblUsuario");

        btnNuevoUsuario = $("#btnNuevoUsuario");

        btnGuardarUsuario = $("#btnGuardarUsuario");

        btnEditarUsuario = $("#btnEditarUsuario");

        btnBuscar_Usuario = $("#btnBuscar_Usuario");

        $('a[href$="#tabUsuario"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function init() {
            initTableUsuario();

            initCbo();

            btnNuevoUsuario.click(fnAgregarUsuario);

            btnGuardarUsuario.click(fnGuardarUsuario);

            btnEditarUsuario.click(fnEditarUsuario);

            btnBuscar_Usuario.click(fnBuscarUsuario);
        }

        function fnAgregarUsuario() {
            $("#txtUsuarioNuevoNombre").val("");
            $("#txtUsuarioApeP").val("");
            $("#txtUsuarioApeM").val("");
            $("#txtUsuarioCorreo").val("");
            $("#txtUsuarioUsu").val("");
            //$("#txtUsuarioContrasena").val("");
            $("#selectUsuarioCuadrilla").val("");

            $("#dialogNuevoUsuario").modal('show');

            $("#txtUsuarioNuevoNombre").focus();
        }

        function fnEditarUsuario() {
            $.ajax({
                url: '/MAZDA/PlanActividades/EditarUsuario',
                datatype: "json",
                type: "POST",
                data: {
                    id: $("#btnEditarUsuario").val(),
                    nombre: $("#txtUsuarioEditNombre").val(),
                    apellidoPaterno: $("#txtUsuarioEditApeP").val(),
                    apellidoMaterno: $("#txtUsuarioEditApeM").val(),
                    correo: $("#txtUsuarioEditCorreo").val(),
                    usuario: $("#txtUsuarioEditUsu").val(),
                    cuadrillaID: $("#selectUsuarioEditCuadrilla").val() != null && $("#selectUsuarioEditCuadrilla").val() != "" ? $("#selectUsuarioEditCuadrilla").val() : 0
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información del usuario.");
                    recargarTodo();
                }
            });
        }

        function fnBuscarUsuario() {
            tblUsuario.ajax.reload(null, false);
            //$("#txtUsuarioNombre").val("");
            //$("#txtUsuarioCuadrilla").val("");
        }

        function fnGuardarUsuario() {
            if ($("#txtUsuarioNuevoNombre").val() != "" && $("#txtUsuarioCorreo").val() != "" && $("#txtUsuarioUsu").val() != "") {
                $.ajax({
                    url: '/MAZDA/PlanActividades/GuardarUsuario',
                    data: {
                        nombre: $("#txtUsuarioNuevoNombre").val(),
                        apellidoPaterno: $("#txtUsuarioApeP").val(),
                        apellidoMaterno: $("#txtUsuarioApeM").val(),
                        correo: $("#txtUsuarioCorreo").val(),
                        usuario: $("#txtUsuarioUsu").val(),
                        //contrasena: $("#txtUsuarioContrasena").val(),
                        cuadrillaID: $("#selectUsuarioCuadrilla").val() != null && $("#selectUsuarioCuadrilla").val() != "" ? $("#selectUsuarioCuadrilla").val() : 0
                    },
                    success: function (data) {
                        AlertaGeneral("Información Guardada", "Se ha guardado la información del usuario.");
                        recargarTodo();
                    }
                });
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }

        function initTableUsuario() {
            tblUsuario = $("#tblUsuario").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/PlanActividades/GetUsuariosMAZDA',
                    data: function (data) {
                        data.usuario = $("#txtUsuarioNombre").val(),
                        data.cuadrillaID = $("#txtUsuarioCuadrilla").val() != null && $("#txtUsuarioCuadrilla").val() != "" ? parseInt($("#txtUsuarioCuadrilla").val()) : 0
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
                    tblUsuario.on('click', '.btn-editar-usu', function () {
                        var rowData = tblUsuario.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/MAZDA/PlanActividades/GetUsuarioMAZDA',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                var data = response.data;

                                $("#btnEditarUsuario").val(rowData["id"]);

                                $("#txtUsuarioEditNombre").val("");
                                $("#txtUsuarioEditApeP").val("");
                                $("#txtUsuarioEditApeM").val("");
                                $("#txtUsuarioEditCorreo").val("");
                                $("#txtUsuarioEditUsu").val("");
                                $("#selectUsuarioEditCuadrilla").val("");

                                $("#dialogEditarUsuario").modal('show');

                                $("#txtUsuarioEditNombre").val(data.nombre);
                                $("#txtUsuarioEditApeP").val(data.apellidoPaterno);
                                $("#txtUsuarioEditApeM").val(data.apellidoMaterno);
                                $("#txtUsuarioEditCorreo").val(data.correo);
                                $("#txtUsuarioEditUsu").val(data.nombreUsuario);
                                $("#selectUsuarioEditCuadrilla").val(data.cuadrillaID != 0 ? data.cuadrillaID : "");
                            }
                        });
                    });

                    tblUsuario.on('click', '.btn-eliminar-usu', function () {
                        var rowData = tblUsuario.row($(this).closest('tr')).data();

                        $("#dialogBajaUsuario").dialog({
                            modal: true,
                            open: function () {
                                $("#txtElimUsuario").text("¿Está seguro que desea eliminar el usuario de '" + rowData["nombre"] + " " + rowData["apellidoPaterno"] + " " + rowData["apellidoMaterno"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/MAZDA/PlanActividades/RemoveUsuario',
                                        data: { id: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogBajaUsuario").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogBajaUsuario").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'nombreCompleto', title: 'Nombre' },
                    { data: 'correo', title: 'Correo' },
                    { data: 'nombreUsuario', title: 'Usuario' },
                    { data: 'cuadrilla', title: 'Cuadrilla' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-editar-usu btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-eliminar-usu btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] },
                    { "width": "90%", "targets": [0] }
                ]
            });
        }

        function initCbo() {
            $("#selectUsuarioCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);

            $("#selectUsuarioEditCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);

            $("#txtUsuarioCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
        }

        function recargarTodo() {
            tblUsuario.ajax.reload(null, false);

            $("#dialogNuevoUsuario").modal('hide');

            $("#dialogEditarUsuario").modal('hide');

            initCbo();
        }

        init();
    };

    $(document).ready(function () {
        planActividades.usuariosCatalogo = new usuariosCatalogo();
    });

})();