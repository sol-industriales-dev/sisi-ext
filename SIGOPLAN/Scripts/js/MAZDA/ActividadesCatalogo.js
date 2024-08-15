(function () {

    $.namespace('planActividades.actividadesCatalogo');

    actividadesCatalogo = function () {

        tblActividad = $("#tblActividad");

        btnNuevaActividad = $("#btnNuevaActividad");

        btnGuardarActividad = $("#btnGuardarActividad");

        btnEditarActividad = $("#btnEditarActividad");

        btnBuscar_Actividad = $("#btnBuscar_Actividad");

        $('a[href$="#tabActividad"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function init() {
            initTableActividad();

            initCbo();

            btnNuevaActividad.click(fnAgregarActividad);

            btnGuardarActividad.click(fnGuardarActividad);

            btnEditarActividad.click(fnEditarActividad);

            btnBuscar_Actividad.click(fnBuscarActividad);
        }

        function fnAgregarActividad() {
            $("#txtActividadDesc").val("");
            $("#txtActividadDescripcion").val("");
            $("#selectActividadCuadrilla").val("");
            $("#selectActividadArea").val("");
            $("#selectActividadPeriodo").val("");

            $("#dialogNuevaActividad").modal('show');

            $("#txtActividadDesc").focus();
        }

        function fnEditarActividad() {
            $.ajax({
                url: '/MAZDA/PlanActividades/EditarActividad',
                datatype: "json",
                type: "POST",
                data: {
                    id: $("#btnEditarActividad").val(),
                    desc: $("#txtEditActividadDesc").val(),
                    descripcion: $('#txtEditActividadDescripcion').val(),
                    cuadrillaID: $("#selectEditActividadCuadrilla").val(),
                    area: $("#selectEditActividadArea").find('option:selected').text(),
                    periodo: $("#selectEditActividadPeriodo").val()
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información del área.");
                    recargarTodo();
                }
            });
        }

        function fnBuscarActividad() {
            tblActividad.ajax.reload(null, false);
            //$("#txtActividadCuadrilla").val("");
            //$("#txtActividadPeriodo").val("");
            //$("#txtActividadArea").val("");
            //$("#txtActividad").val("");
        }

        function fnGuardarActividad() {
            if ($("#selectActividadCuadrilla").val() != "" && $("#txtActividadDesc").val() != "" && $("#selectActividadArea").val() != "" && $("#selectActividadPeriodo").val() != "") {
                $.ajax({
                    url: '/MAZDA/PlanActividades/GuardarActividad',
                    //dataType: 'json',
                    data: {
                        desc: $("#txtActividadDesc").val(),
                        descripcion: $('#txtActividadDescripcion').val(),
                        cuadrillaID: $("#selectActividadCuadrilla").val(),
                        area: $("#selectActividadArea").find('option:selected').text(),
                        periodo: $("#selectActividadPeriodo").val()
                    },
                    success: function (data) {
                        AlertaGeneral("Información Guardada", "Se ha guardado la información de la actividad.");
                        recargarTodo();
                    }
                });
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }

        function initTableActividad() {
            tblActividad = $("#tblActividad").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/PlanActividades/GetActividades',
                    data: function (data) {
                        data.cuadrillaID = $("#txtActividadCuadrilla").val() != null && $("#txtActividadCuadrilla").val() != "" ? parseInt($("#txtActividadCuadrilla").val()) : 0,
                        data.periodo = $("#txtActividadPeriodo").val() != null && $("#txtActividadPeriodo").val() != "" ? parseInt($("#txtActividadPeriodo").val()) : 0,
                        data.area = $("#txtActividadArea").val(),
                        data.actividad = $("#txtActividad").val()
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
                    tblActividad.on('click', '.btn-editar-act', function () {
                        var rowData = tblActividad.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/MAZDA/PlanActividades/GetActividad',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                var data = response.data;

                                $("#btnEditarActividad").val(rowData["id"]);

                                $("#txtEditActividadDesc").val("");
                                $("#selectEditActividadCuadrilla").val("");
                                $("#selectEditActividadArea").val("");
                                $("#selectEditActividadPeriodo").val("");

                                $("#dialogEditarActividad").modal('show');

                                $("#txtEditActividadDesc").val(data.descripcion);
                                $("#txtEditActividadDescripcion").val(data.detalle);
                                $("#selectEditActividadCuadrilla").val(data.cuadrillaID != 0 ? data.cuadrillaID : "");

                                //$('#test option').filter(function () { return $(this).html() == "B"; }).val();

                                //var opcion = $("#selectEditActividadArea").find('option[text="' + data.area + '"]').val();
                                var opcion = $('#selectEditActividadArea option').filter(function () { return $(this).html() == data.area; }).val();

                                $("#selectEditActividadArea").val(opcion);

                                $("#selectEditActividadPeriodo").val(data.periodo);
                            }
                        });
                    });

                    tblActividad.on('click', '.btn-eliminar-act', function () {
                        var rowData = tblActividad.row($(this).closest('tr')).data();
                        $("#dialogBajaActividad").dialog({
                            modal: true,
                            open: function () {
                                $("#txtElimActividad").text("¿Está seguro que desea eliminar la actividad '" + rowData["descripcion"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/MAZDA/PlanActividades/RemoveActividad',
                                        data: { id: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogBajaActividad").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogBajaActividad").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'cuadrilla', title: 'Cuadrilla' },
                    { data: 'periodo', title: 'Periodo' },
                    { data: 'area', title: 'Area' },
                    { data: 'descripcion', title: 'Actividad' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-editar-act btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-eliminar-act btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] },
                    {
                        "render": function (data, type, row) {
                            switch (data) {
                                case 1:
                                    return 'MENSUAL';
                                    break;
                                case 2:
                                    return 'BIMESTRAL';
                                    break;
                                case 3:
                                    return 'TRIMESTRAL';
                                    break;
                                case 4:
                                    return 'SEMESTRAL';
                                    break;
                                case 5:
                                    return 'ANUAL';
                                    break;
                                default:
                                    return '';
                                    break;
                            }
                        },
                        "targets": [1]
                    },
                    { "width": "90%", "targets": [3] }
                ]
            });
        }

        function initCbo() {
            $("#selectActividadCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
            $("#selectActividadArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', null, false);

            $("#selectEditActividadCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
            $("#selectEditActividadArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', null, false);

            $("#txtActividadCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
        }

        function recargarTodo() {
            tblActividad.ajax.reload(null, false);

            $("#dialogNuevaActividad").modal('hide');

            $("#dialogEditarActividad").modal('hide');

            initCbo();
        }

        init();
    };

    $(document).ready(function () {
        planActividades.actividadesCatalogo = new actividadesCatalogo();
    });

})();