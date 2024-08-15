(function () {
    $.namespace('controlObra.UnidadesCatalogo');

    UnidadesCatalogo = function () {
        tblUnidades = $("#tblUnidades");
        btnNuevaUnidad = $("#btnNuevaUnidad");
        btnGuardarUnidad = $("#btnGuardarUnidad");

        mdlUnidad = $('#mdlUnidad');
        let lbl = document.getElementById('lblTitulo');
        let isEdit = false;

        function init() {
            initTableUnidades();
            btnNuevaUnidad.click(fnAbrirModalUnidad);
            btnGuardarUnidad.click(fnGuardarUnidad);
        }

        function initTableUnidades() {
            tblUnidades = $("#tblUnidades").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/ControlObra/ControlObra/GetUnidadesCatalogo',
                    dataSrc: function (response) {
                        return response.items;
                    }
                },
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                scrollY: "250px",
                scrollCollapse: true,
                searching: false,
                initComplete: function (settings, json) {

                    tblUnidades.on('click', '.btn-editar-unidad', function () {
                        var rowData = tblUnidades.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/ControlObra/ControlObra/GetUnidad',
                            data: { unidadID: rowData["id"] },
                            success: function (response) {
                                const data = response.unidad;
                                isEdit = true;

                                $("#btnGuardarUnidad").val(rowData["id"]);
                                $("#txtUnidad").val("");
                                $("#txtUnidad").val(data.unidad);
                                lbl.innerText = 'Editar Unidad';

                                mdlUnidad.modal('show');
                            }
                        });
                    });

                    tblUnidades.on('click', '.btn-eliminar-unidad', function () {
                        var rowData = tblUnidades.row($(this).closest('tr')).data();
                        $("#dialogEliminarUnidad").dialog({
                            modal: true,
                            open: function () {
                                $("#txtEliminarUnidad").text("¿Está seguro que desea eliminar la unidad '" + rowData["unidad"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/ControlObra/ControlObra/RemoveUnidad',
                                        data: { unidadID: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogEliminarUnidad").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogEliminarUnidad").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'unidad', title: 'Unidad' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-editar-unidad btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-eliminar-unidad btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { "width": "800px", "targets": 0 },
                    { "width": "5px", "targets": 1 },
                    { "width": "5px", "targets": 2 }
                ],
            });
        }

        function fnAbrirModalUnidad() {
            isEdit = false;

            $("#txtUnidad").val("");
            $("#txtUnidad").focus();
            lbl.innerText = 'Nueva Unidad';
            mdlUnidad.modal('show');
        }

        function fnGuardarUnidad() {
            if (!validarGuardar) {
                return;
            }

            if (isEdit) {
                editarUnidad();
            } else {
                agregarUnidad();
            }
        }

        function validarGuardar() {
            const esValida = false


            if ($("#txtUnidad").val() != "") {
                esValida = true;
            }
            else {
                esValida = false;
            }

            return esValida;
        }

        function agregarUnidad() {
            $.post('/ControlObra/ControlObra/GuardarUnidad', {
                unidad: $("#txtUnidad").val()

            })
                .done(respuesta => {
                    if (respuesta.success) {
                        AlertaGeneral("Aviso", "Unidad guardada correctamente.");
                        recargarTodo();
                    } else {
                        AlertaGeneral("Aviso", respuesta.error);
                    }
                });
        }

        function editarUnidad() {
            $.post('/ControlObra/ControlObra/EditarUnidad', {
                unidadID: $("#btnGuardarUnidad").val(),
                unidad: $("#txtUnidad").val()
            })
                .done(respuesta => {
                    if (respuesta.success) {
                        AlertaGeneral("Aviso", "Unidad actualizado correctamente.");
                        recargarTodo();
                    } else {
                        AlertaGeneral("Aviso", respuesta.error);
                    }
                });
        }

        function recargarTodo() {
            mdlUnidad.modal('hide');
            tblUnidades.ajax.reload(null, false);
        }

        init();
    };

    $(document).ready(function () {
        controlObra.UnidadesCatalogo = new UnidadesCatalogo();
    });
})();