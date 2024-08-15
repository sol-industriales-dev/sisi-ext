$(function () {
    $.namespace('encuestas.gestionTelefonica');
    gestionTelefonica = function () {
        _Eliminar = 0;
        mensajes = {
            PROCESANDO: 'Procesando...'
        };

        _encuestaID = 0;

        tblDataEncuestas = $("#tblDataEncuestas");
        tblDataUsuariosNoti = $("#tblDataUsuariosNoti");

        txtTitulo = $("#txtTitulo");
        txtDescripcion = $("#txtDescripcion");
        txtDepartamento = $("#txtDepartamento");

        btnAplicarFiltros = $("#btnAplicarFiltros");

        //txtIgNumeroEmpleado = $("#txtIgNumeroEmpleado");
        lblIgNombre = $("#lblIgNombre");

        btnAgregarUsuarioNoti = $("#btnAgregarUsuarioNoti");

        btnGuardar = $("#btnGuardar");

        $("#tblDataUsuariosNoti").on({
            click: function () {
                var row = $(this).closest('tr');
                row.remove();
            }
        }, ".btn-quitar-usuario");

        $("#tblDataUsuariosNoti").on({
            click: function () {
                var div = $(this).closest('div');

                var html = "";

                if ($(this).attr("data-check") == "true") {
                    html += '<button class="btn-usuario-telefonica btn btn-sm btn" type="button" data-check=false style="margin-top: 5px; margin-bottom: 5px;">No</button>';
                } else {
                    html += '<button class="btn-usuario-telefonica btn btn-sm btn-success" type="button" data-check=true style="margin-top: 5px; margin-bottom: 5px;">Sí</button>';
                }

                $(this).remove();

                $(html).appendTo($(div));
            }
        }, ".btn-usuario-telefonica");

        $("#tblDataUsuariosNoti").on({
            click: function () {
                var div = $(this).closest('div');

                var html = "";

                if ($(this).attr("data-check") == "true") {
                    html += '<button class="btn-usuario-notificacion btn btn-sm btn" type="button" data-check=false style="margin-top: 5px; margin-bottom: 5px;">No</button>';
                } else {
                    html += '<button class="btn-usuario-notificacion btn btn-sm btn-success" type="button" data-check=true style="margin-top: 5px; margin-bottom: 5px;">Sí</button>';
                }

                $(this).remove();

                $(html).appendTo($(div));
            }
        }, ".btn-usuario-notificacion");

        function init() {
            btnGuardar.click(fnGuardarUsuariosCheck);
            btnAplicarFiltros.click(fnBuscar);
            //txtIgNumeroEmpleado.change(selectEmpleado);

            btnAgregarUsuarioNoti.click(fnAgregarUsuarioNoti);

            initTable();

            setAutoComplete();
        }

        function fnBuscar() {
            tblDataEncuestas.page(0);
            tblDataEncuestas.ajax.reload(null, false);

            txtTitulo.val("");
            txtDescripcion.val("");
            txtDepartamento.val("");
        }

        function fnGuardarUsuariosCheck() {
            var arr = new Array();

            $("#tblDataUsuariosNoti tbody tr").each(function (index) {
                var row = $(this);
                var obj = {};

                obj = {
                    id: row.attr("id"),
                    contestaTelefonica: row.find('.btn-usuario-telefonica').attr("data-check") == "true" ? true : false,
                    recibeNotificacion: row.find('.btn-usuario-notificacion').attr("data-check") == "true" ? true : false
                }
                //arr.push(parseInt(row.attr("id")));
                arr.push(obj);
            });

            $.ajax({
                datatype: "json",
                url: '/Encuestas/Encuesta/GuardarUsuariosCheck',
                type: 'POST',
                data: { encuestaID: _encuestaID, usuarios: arr },
                success: function (response) {
                    if (response.success) {
                        $("#dialogNotificacionUsuarios").modal("hide");
                        AlertaGeneral("Información Guardada", "Se ha guardado la información de la encuesta.");
                        tblDataEncuestas.ajax.reload(null, false);
                    } else {
                        AlertaGeneral("Alerta", "Error al guardar la información de la encuesta.");
                        tblDataEncuestas.ajax.reload(null, false);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setAutoComplete() {
            //lblIgNombre.getAutocomplete(setIdEmpleado, null, '/Administrativo/FormatoCambio/getEmpleados');
            lblIgNombre.getAutocomplete(setIdEmpleado, null, '/Encuestas/Encuesta/getUsuarios');

            txtDepartamento.getAutocomplete(funSelDep, { dep: txtDepartamento.val() }, '/Encuestas/Encuesta/getDepartamentosList');
        }

        function setIdEmpleado(event, ui) {
            //txtIgNumeroEmpleado.val(ui.item.id);
            //txtIgNumeroEmpleado.trigger('change');
        }

        function fnAsignClaveVobo(event, ui) {
            $.ajax({
                datatype: "json",
                url: '/Administrativo/Finiquito/getEmpleado',
                data: { id: ui.item.id },
                success: function (response) {
                    var data = response.items;
                    lblIgNombre.data("claveEmpleado", data.claveEmpleado);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function funSelDep(event, ui) {
            txtDepartamento.text(ui.item.value);
        }

        function selectEmpleado() {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/Finiquito/getEmpleado',
                data: { id: txtIgNumeroEmpleado.val() },
                async: false,
                success: function (response) {
                    if (response.success) {
                        viewDatosEmpleados(response.items);
                    }
                    else {
                        AlertaGeneral('Alerta', 'No se encontró el número de usuario');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function viewDatosEmpleados(objEmpleado) {
            lblIgNombre.val(objEmpleado.nombre + " " + objEmpleado.ape_paterno + " " + objEmpleado.ape_materno);
        }

        function fnAgregarUsuarioNoti() {
            //var claveEmpleado = txtIgNumeroEmpleado.val();
            var nombre = lblIgNombre.val();

            $.ajax({
                datatype: "json",
                url: '/Encuestas/Encuesta/checkUsuario',
                data: { nombre: nombre },
                success: function (response) {
                    if (response.obj != null) {
                        var flag = false;

                        $("#tblDataUsuariosNoti tbody tr").each(function (index) {
                            var row = $(this);

                            if (response.obj.id == row.attr("id")) {
                                flag = true;
                            }
                        });

                        if (flag == false) {
                            var obj = response.obj;

                            $.ajax({
                                url: '/Encuestas/Encuesta/getEncuestaCheck',
                                data: { encuestaID: _encuestaID },
                                success: function (response) {
                                    var data = response.data;

                                    addRowUsuario(obj, data);
                                    lblIgNombre.val("");
                                    //txtIgNumeroEmpleado.val("");
                                }
                            });
                        } else {
                            AlertaGeneral("Alerta", "No se puede repetir el usuario.");
                            lblIgNombre.val("");
                            //txtIgNumeroEmpleado.val("");
                        }
                    } else {
                        AlertaGeneral("Alerta", "No se encuentra el usuario.");
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function initTable() {
            tblDataEncuestas = $("#tblDataEncuestas").DataTable({
                retrieve: true,
                ajax: {
                    url: '/Encuestas/Encuesta/getEncuestas',
                    //dataSrc: 'data',
                    data: function (d) {
                        d.titulo = txtTitulo.val();
                        d.descripcion = txtDescripcion.val();
                        d.departamento = txtDepartamento.val();
                    }
                },
                language: dtDicEsp,
                rowId: 'id',
                scrollX: "100%",
                //"scrollY": datatableHeight(window.screen.height),
                "scrollCollapse": true,
                "deferRender": true,
                "order": [2, 'asc'],
                'initComplete': function (settings, json) {
                    tblDataEncuestas.on('click', '.btn-telefonica-green', function () {
                        $(this).removeClass('btn-telefonica-green btn-success');
                        $(this).addClass('btn-telefonica-red');
                        $(this).text('No');

                        var rowData = tblDataEncuestas.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/Encuestas/Encuesta/UpdateTelefonica',
                            data: { encuestaID: rowData["id"], telefonica: false },
                            success: function (response) {
                                tblDataEncuestas.ajax.reload(null, false);

                                //AlertaGeneral("Encuesta Actualizada", "Se ha actualizado la información de la encuesta.");
                            }
                        });
                    });

                    tblDataEncuestas.on('click', '.btn-telefonica-red', function () {
                        $(this).removeClass('btn-telefonica-red');
                        $(this).addClass('btn-telefonica-green btn-success');
                        $(this).text('Sí');

                        var rowData = tblDataEncuestas.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/Encuestas/Encuesta/UpdateTelefonica',
                            data: { encuestaID: rowData["id"], telefonica: true },
                            success: function (response) {
                                tblDataEncuestas.ajax.reload(null, false);

                                //AlertaGeneral("Encuesta Actualizada", "Se ha actualizado la información de la encuesta.");
                            }
                        });
                    });

                    tblDataEncuestas.on('click', '.btn-notificacion-green', function () {
                        $(this).removeClass('btn-notificacion-green btn-success');
                        $(this).addClass('btn-notificacion-red');
                        $(this).text('No');

                        var rowData = tblDataEncuestas.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/Encuestas/Encuesta/UpdateNotificacion',
                            data: { encuestaID: rowData["id"], notificacion: false },
                            success: function (response) {
                                tblDataEncuestas.ajax.reload(null, false);

                                AlertaGeneral("Encuesta Actualizada", "Se ha actualizado la información de la encuesta.");
                            }
                        });
                    });

                    tblDataEncuestas.on('click', '.btn-notificacion-red', function () {
                        $(this).removeClass('btn-notificacion-red');
                        $(this).addClass('btn-notificacion-green btn-success');
                        $(this).text('Sí');

                        var rowData = tblDataEncuestas.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/Encuestas/Encuesta/UpdateNotificacion',
                            data: { encuestaID: rowData["id"], notificacion: true },
                            success: function (response) {
                                tblDataEncuestas.ajax.reload(null, false);

                                AlertaGeneral("Encuesta Actualizada", "Se ha actualizado la información de la encuesta.");
                            }
                        });
                    });

                    tblDataEncuestas.on('click', '.btn-papel-green', function () {
                        $(this).removeClass('btn-papel-green btn-success');
                        $(this).addClass('btn-papel-red');
                        $(this).text('No');

                        var rowData = tblDataEncuestas.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/Encuestas/Encuesta/UpdatePapel',
                            data: { encuestaID: rowData["id"], papel: false },
                            success: function (response) {
                                tblDataEncuestas.ajax.reload(null, false);

                                AlertaGeneral("Encuesta Actualizada", "Se ha actualizado la información de la encuesta.");
                            }
                        });
                    });

                    tblDataEncuestas.on('click', '.btn-papel-red', function () {
                        $(this).removeClass('btn-papel-red');
                        $(this).addClass('btn-papel-green btn-success');
                        $(this).text('Sí');

                        var rowData = tblDataEncuestas.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/Encuestas/Encuesta/UpdatePapel',
                            data: { encuestaID: rowData["id"], papel: true },
                            success: function (response) {
                                tblDataEncuestas.ajax.reload(null, false);

                                AlertaGeneral("Encuesta Actualizada", "Se ha actualizado la información de la encuesta.");
                            }
                        });
                    });

                    tblDataEncuestas.on('click', '.btn-usuarios-noti', function () {
                        var rowData = tblDataEncuestas.row($(this).closest('tr')).data();

                        _encuestaID = rowData["id"];

                        $.ajax({
                            url: '/Encuestas/Encuesta/getUsuariosCheck',
                            data: { encuestaID: rowData["id"] },
                            success: function (response) {
                                var obj = response.obj;

                                $.ajax({
                                    url: '/Encuestas/Encuesta/getEncuestaCheck',
                                    data: { encuestaID: rowData["id"] },
                                    success: function (response) {
                                        var data = response.data;

                                        $("#dialogNotificacionUsuarios").modal("show");
                                        lblIgNombre.val("");
                                        //txtIgNumeroEmpleado.val("");

                                        addRowUsuarios(obj, data);
                                    }
                                });
                            }
                        });
                    });
                },
                columns: [
                    { data: 'titulo', title: "Título" },
                    { data: 'descripcion', title: "Descripción" },
                    { data: 'departamento', title: "Departamento" },
                    {
                        data: 'telefonica', title: "Telefónica", render: function (data, type, row, meta) {
                            if (row.telefonica == true) {
                                return '<button class="btn-telefonica-green btn btn-sm btn-success" type="button" value="' + row.id + '">Sí</button>';
                            } else {
                                return '<button class="btn-telefonica-red btn btn-sm" type="button" value="' + row.id + '">No</button>';
                            }
                        }
                    },
                    {
                        data: 'notificacion', title: "Notificación", render: function (data, type, row, meta) {
                            if (row.notificacion == true) {
                                return '<button class="btn-notificacion-green btn btn-sm btn-success" type="button" value="' + row.id + '">Sí</button>';
                            } else {
                                return '<button class="btn-notificacion-red btn btn-sm" type="button" value="' + row.id + '">No</button>';
                            }
                        }
                    },
                    {
                        data: 'papel', title: "Papel", render: function (data, type, row, meta) {
                            if (row.papel == true) {
                                return '<button class="btn-papel-green btn btn-sm btn-success" type="button" value="' + row.id + '">Sí</button>';
                            } else {
                                return '<button class="btn-papel-red btn btn-sm" type="button" value="' + row.id + '">No</button>';
                            }
                        }
                    },
                    {
                        title: "Usuarios", render: function (data, type, row, meta) {
                            if (row.telefonica == true || row.notificacion == true) {
                                return '<button class="btn-usuarios-noti btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '"></button>';
                            } else {
                                return '';
                            }
                        }
                    }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6] }
                ]
            });
        }

        //function initTableUsuariosNoti() {
        //    tblDataUsuariosNoti = $("#tblDataUsuariosNoti").DataTable({
        //        retrieve: true,
        //        ajax: {
        //            url: '/Encuestas/Encuesta/getUsuariosNoti',
        //            //dataSrc: 'data',
        //            data: function (d) {
        //                d.encuestaID = 0;
        //            }
        //        },
        //        "language": {
        //            "sProcessing": "Procesando...",
        //            "sLengthMenu": "Mostrar _MENU_ registros",
        //            "sZeroRecords": "No se encontraron resultados",
        //            "sEmptyTable": "Ningún dato disponible en esta tabla",
        //            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        //            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        //            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        //            "sInfoPostFix": "",
        //            "sSearch": "Buscar:",
        //            "sUrl": "",
        //            "sInfoThousands": ",",
        //            "sLoadingRecords": "Cargando...",
        //            "oPaginate": {
        //                "sFirst": "Primero",
        //                "sLast": "Último",
        //                "sNext": "Siguiente",
        //                "sPrevious": "Anterior"
        //            },
        //            "oAria": {
        //                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
        //                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        //            }
        //        },
        //        rowId: 'id',
        //        scrollX: "100%",
        //        //"scrollY": datatableHeight(window.screen.height),
        //        "scrollCollapse": true,
        //        "deferRender": true,
        //        "order": [2, 'asc'],
        //        'initComplete': function (settings, json) {

        //        },
        //        columns: [
        //            { data: 'nombre', title: "Nombre" },
        //            { data: 'puesto', title: "Puesto" },
        //            { data: 'correo', title: "Correo" },
        //            {
        //                render: function (data, type, row, meta) {
        //                    return '<button class="btn-quitar-usuario btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '"></button>';
        //                }
        //            }
        //        ],
        //        columnDefs: [
        //            { "className": "dt-center", "targets": [0, 1, 2, 3] }
        //        ]
        //    });
        //}

        function addRowUsuarios(arr, encuesta) {
            $("#tblDataUsuariosNoti tbody tr").remove();

            for (i = 0; i < arr.length; i++) {
                var html = "";

                html += '<tr  id="' + arr[i].id + '">';
                html += '   <td class="text-center nombre">';
                html += '       <div class="col-lg-12">';
                html += '           ' + arr[i].nombre + " " + arr[i].apellidoPaterno + " " + arr[i].apellidoMaterno;
                html += '       </div>';
                html += '   </td>';
                html += '   <td class="text-center puesto">';
                html += '       <div class="col-lg-12">';
                html += '           ' + (arr[i].puestoDescripcion != 'Temp' ? arr[i].puestoDescripcion : '');
                html += '       </div>';
                html += '   </td>';
                html += '   <td class="text-center correo">';
                html += '       <div class="col-lg-12">';
                html += '           ' + arr[i].correo;
                html += '       </div>';
                html += '   </td>';

                if (encuesta.telefonica != false) {
                    if (arr[i].contestaTelefonica == true) {
                        html += '   <td class="text-center usuario-telefonica">';
                        html += '       <div class="col-lg-12">';
                        html += '           <button class="btn-usuario-telefonica btn btn-sm btn-success" type="button" value="' + arr[i].id + '" data-check=true style="margin-top: 5px; margin-bottom: 5px;">Sí</button>';
                        html += '       </div>';
                        html += '   </td>';
                    } else {
                        html += '   <td class="text-center usuario-telefonica">';
                        html += '       <div class="col-lg-12">';
                        html += '           <button class="btn-usuario-telefonica btn btn-sm btn" type="button" value="' + arr[i].id + '" data-check=false style="margin-top: 5px; margin-bottom: 5px;">No</button>';
                        html += '       </div>';
                        html += '   </td>';
                    }
                } else {
                    html += '   <td class="text-center usuario-telefonica">';
                    html += '       <div class="col-lg-12">';
                    html += '           <button class="btn-usuario-telefonica btn btn-sm btn" type="button" value="' + arr[i].id + '" data-check=false style="margin-top: 5px; margin-bottom: 5px;" disabled>No</button>';
                    html += '       </div>';
                    html += '   </td>';
                }

                if (encuesta.notificacion != false) {
                    if (arr[i].recibeNotificacion == true) {
                        html += '   <td class="text-center usuario-notificacion">';
                        html += '       <div class="col-lg-12">';
                        html += '           <button class="btn-usuario-notificacion btn btn-sm btn-success" type="button" value="' + arr[i].id + '" data-check=true style="margin-top: 5px; margin-bottom: 5px;">Sí</button>';
                        html += '       </div>';
                        html += '   </td>';
                    } else {
                        html += '   <td class="text-center usuario-notificacion">';
                        html += '       <div class="col-lg-12">';
                        html += '           <button class="btn-usuario-notificacion btn btn-sm btn" type="button" value="' + arr[i].id + '" data-check=false style="margin-top: 5px; margin-bottom: 5px;">No</button>';
                        html += '       </div>';
                        html += '   </td>';
                    }
                } else {
                    html += '   <td class="text-center usuario-notificacion">';
                    html += '       <div class="col-lg-12">';
                    html += '           <button class="btn-usuario-notificacion btn btn-sm btn" type="button" value="' + arr[i].id + '" data-check=false style="margin-top: 5px; margin-bottom: 5px;" disabled>No</button>';
                    html += '       </div>';
                    html += '   </td>';
                }

                html += '   <td class="text-center quitar-usuario">';
                html += '       <div class="col-lg-12">';
                html += '           <button class="btn-quitar-usuario btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + arr[i].id + '" style="margin-top: 5px; margin-bottom: 5px;"></button>';
                html += '       </div>';
                html += '   </td>';
                html += '</tr>';

                $(html).appendTo($("#tblDataUsuariosNoti tbody"));
            }
        }

        function addRowUsuario(obj, encuesta) {
            var html = "";

            html += '<tr  id="' + obj.id + '">';
            html += '   <td class="text-center nombre">';
            html += '       <div class="col-lg-12">';
            html += '           ' + obj.nombre + " " + obj.apellidoPaterno + " " + obj.apellidoMaterno;
            html += '       </div>';
            html += '   </td>';
            html += '   <td class="text-center puesto">';
            html += '       <div class="col-lg-12">';
            html += '           ' + obj.puestoDescripcion;
            html += '       </div>';
            html += '   </td>';
            html += '   <td class="text-center correo">';
            html += '       <div class="col-lg-12">';
            html += '           ' + obj.correo;
            html += '       </div>';
            html += '   </td>';

            if (encuesta.telefonica != false) {
                html += '   <td class="text-center usuario-telefonica">';
                html += '       <div class="col-lg-12">';
                html += '           <button class="btn-usuario-telefonica btn btn-sm btn" type="button" value="' + obj.id + '" data-check=false style="margin-top: 5px; margin-bottom: 5px;">No</button>';
                html += '       </div>';
                html += '   </td>';
            } else {
                html += '   <td class="text-center usuario-telefonica">';
                html += '       <div class="col-lg-12">';
                html += '           <button class="btn-usuario-telefonica btn btn-sm btn" type="button" value="' + obj.id + '" data-check=false style="margin-top: 5px; margin-bottom: 5px;" disabled>No</button>';
                html += '       </div>';
                html += '   </td>';
            }

            if (encuesta.notificacion != false) {
                html += '   <td class="text-center usuario-notificacion">';
                html += '       <div class="col-lg-12">';
                html += '           <button class="btn-usuario-notificacion btn btn-sm btn" type="button" value="' + obj.id + '" data-check=false style="margin-top: 5px; margin-bottom: 5px;">No</button>';
                html += '       </div>';
                html += '   </td>';
            } else {
                html += '   <td class="text-center usuario-notificacion">';
                html += '       <div class="col-lg-12">';
                html += '           <button class="btn-usuario-notificacion btn btn-sm btn" type="button" value="' + obj.id + '" data-check=false style="margin-top: 5px; margin-bottom: 5px;" disabled>No</button>';
                html += '       </div>';
                html += '   </td>';
            }

            html += '   <td class="text-center quitar-usuario">';
            html += '       <div class="col-lg-12">';
            html += '           <button class="btn-quitar-usuario btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + obj.id + '" style="margin-top: 5px; margin-bottom: 5px;"></button>';
            html += '       </div>';
            html += '   </td>';
            html += '</tr>';

            $(html).appendTo($("#tblDataUsuariosNoti tbody"));
        }

        init();
    }

    $(document).ready(function () {
        encuestas.gestionTelefonica = new gestionTelefonica();
    });
});