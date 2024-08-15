(function () {
    $.namespace('controlObra.ProyectosCatalogo');

    ProyectoCatalogo = function () {
        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');
        dpFechaInicioEdit = $('#dpFechaInicioEdit');
        dpFechaFinEdit = $('#dpFechaFinEdit');
        tblProyectos = $("#tblProyectos");
        dialogProyecto = $('#dialogProyecto');
        dialogProyectoEdit = $('#dialogProyectoEdit');
        btnNuevoProyecto = $("#btnNuevoProyecto");
        btnGuardarProyectoNuevo = $("#btnGuardarProyectoNuevo");
        btnGuardarProyectoEdit = $("#btnGuardarProyectoEdit");
        const hoy = new Date();

        function init() {
            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaInicioEdit.datepicker({ dateFormat: 'dd/mm/yy' });
            dpFechaFinEdit.datepicker({ dateFormat: 'dd/mm/yy' });

            btnNuevoProyecto.click(fnAbrirModalProyectoNuevo);
            btnGuardarProyectoNuevo.click(fnAgregarProyecto);
            btnGuardarProyectoEdit.click(fnEditarProyecto);
            initTableProyectos();

        }

        function fnAbrirModalProyectoNuevo() {
            dialogProyecto.modal('show');
            $("#txtProyectoNuevo").val("");
            dpFechaInicio.datepicker("setDate", hoy);
            dpFechaFin.datepicker("setDate", hoy);
            $("#txtProyectoNuevo").focus();
        }

        function fnAgregarProyecto() {
            if ($("#txtProyectoNuevo").val() != "" && ($('#dpFechaInicio').val() != null && $('#dpFechaFin').val() != "")) {
                var request = new XMLHttpRequest();
                request.open("POST", "/ControlObra/ControlObra/GuardarProyecto");
                request.send(formData());
                request.onload = function (response) {
                    if (request.status == 200) {
                        AlertaGeneral("Aviso", "Proyecto guardado correctamente.");
                        recargarTodo()
                    }
                };
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }

        function formData() {
            let formData = new FormData();
            formData.append("proyecto", JSON.stringify(getProyecto()));
            return formData;
        }

        function getProyecto() {
            return {

                proyecto: $('#txtProyectoNuevo').val(),
                fechaInicio: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                fechaFin: moment(dpFechaFin.val(), "DD-MM-YYYY").format(),
                estatus: true
            };
        }

        function fnEditarProyecto() {
            const url = 'UpdateProyecto';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    proyectoID: $("#btnGuardarProyectoEdit").val(),
                    proyecto: $("#txtProyectoEdit").val(),
                    fechaInicio: dpFechaInicioEdit.val(),
                    fechaFin: dpFechaFinEdit.val(),
                    estatus: true
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    recargarTodo();
                }
            });
        }

        function initTableProyectos() {
            tblProyectos = $("#tblProyectos").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/ControlObra/ControlObra/GetProyectosCatalogo',
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
                    tblProyectos.on('click', '.btn-editar-proyecto', function () {
                        var rowData = tblProyectos.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/ControlObra/ControlObra/GetProyecto',
                            data: { proyectoID: rowData["id"] },
                            success: function (response) {
                                const data = response.Proyecto;
                                const fechaInicio = new Date(moment(data.fechaInicio, "DD-MM-YYYY").format());
                                const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());

                                $("#btnGuardarProyectoEdit").val(rowData["id"]);
                                $("#txtProyectoEdit").val("");

                                dialogProyectoEdit.modal('show');

                                $("#txtProyectoEdit").val(data.proyecto);
                                dpFechaInicioEdit.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaInicio);
                                dpFechaFinEdit.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaFin);
                            }
                        });
                    });

                    tblProyectos.on('click', '.btn-eliminar-proyecto', function () {
                        var rowData = tblProyectos.row($(this).closest('tr')).data();
                        $("#dialogEliminarProyecto").dialog({
                            modal: true,
                            open: function () {
                                $("#txtEliminarProyecto").text("¿Está seguro que desea eliminar el proyecto '" + rowData["proyecto"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/ControlObra/ControlObra/RemoveProyecto',
                                        data: { proyectoID: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogEliminarProyecto").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogEliminarProyecto").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'proyecto', title: 'Proyecto' },
                    { data: 'fechaInicio', title: 'Fecha Inicio' },
                    { data: 'fechaFin', title: 'Fecha Fin' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-editar-proyecto btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-eliminar-proyecto btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: "Eliminar"
                    }
                ]
            });
        }

        function recargarTodo() {
            tblProyectos.ajax.reload(null, false);
            dialogProyecto.modal('hide');
            dialogProyectoEdit.modal('hide');
        }

        init();
    };

    $(document).ready(function () {
        controlObra.ProyectoCatalogo = new ProyectoCatalogo();
    });
})();