(function () {
    $.namespace('controlObra.FasesCatalogo');

    FaseCatalogo = function () {
        tblFases = $("#tblFases");
        btnBuscarFases = $("#btnBuscarFases");
        btnNuevaFase = $("#btnNuevaFase");
        btnGuardarFaseNueva = $("#btnGuardarFaseNueva");
        btnGuardarFaseEdit = $("#btnGuardarFaseEdit");
        dialogFase = $('#dialogFase');
        dialogFaseEdit = $('#dialogFaseEdit');
        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');
        dpFechaInicioEdit = $('#dpFechaInicioEdit');
        dpFechaFinEdit = $('#dpFechaFinEdit');

        const hoy = new Date();

        function init() {
            initTableFases();
            initCbo();
            btnBuscarFases.click(fnBuscarFases);
            btnNuevaFase.click(fnAbrirModalFaseNueva);
            btnGuardarFaseNueva.click(fnAgregarFase);
            btnGuardarFaseEdit.click(fnEditarFase);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaInicioEdit.datepicker({ dateFormat: 'dd/mm/yy' });
            dpFechaFinEdit.datepicker({ dateFormat: 'dd/mm/yy' });

        }

        function initTableFases() {
            var groupColumn = 0;
            tblFases = $("#tblFases").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/ControlObra/ControlObra/GetFasesCatalogo',
                    dataSrc: function (response) {
                        return response.items;
                    }
                },
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                sortable: false,
                initComplete: function (settings, json) {
                    tblFases.on('click', '.btn-editar-fase', function () {
                        var rowData = tblFases.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/ControlObra/ControlObra/GetFase',
                            data: { faseID: rowData["id"] },
                            success: function (response) {
                                const data = response.fase;
                                const fechaInicio = new Date(moment(data.fechaInicio, "DD-MM-YYYY").format());
                                const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());

                                $("#btnGuardarFaseEdit").val(rowData["id"]);
                                $("#txtFaseEdit").val("");

                                dialogFaseEdit.modal('show');

                                $("#txtFaseEdit").val(data.fase);
                                $("#selectProyectoEdit").val(data.proyectoID);
                                dpFechaInicioEdit.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaInicio);
                                dpFechaFinEdit.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaFin);
                            }
                        });
                    });

                    tblFases.on('click', '.btn-eliminar-fase', function () {
                        var rowData = tblFases.row($(this).closest('tr')).data();
                        $("#dialogEliminarFase").dialog({
                            modal: true,
                            open: function () {
                                $("#txtEliminarFase").text("¿Está seguro que desea eliminar la fase '" + rowData["fase"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/ControlObra/ControlObra/RemoveFase',
                                        data: { faseId: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogEliminarFase").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogEliminarFase").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'proyecto', sortable: false },
                    { data: 'fase', title: 'Fase', sortable: false },
                    { data: 'fechaInicio', title: 'Fecha Inicio', sortable: false },
                    { data: 'fechaFin', title: 'Fecha Fin', sortable: false },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-editar-fase btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-eliminar-fase btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { visible: false, targets: groupColumn },
                    { "width": "900px", "targets": 1 },
                    { "width": "10px", "targets": 2 },
                    { "width": "10px", "targets": 3 },
                    { "width": "5px", "targets": 4 },
                    { "width": "5px", "targets": 5 }
                ],
                order: [[groupColumn, 'asc']],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td style="background-color: #ffff99" colspan="11">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });
                }
            });
        }

        function initCbo() {
            $("#multiSelectProyectos").fillCombo('/ControlObra/ControlObra/GetProyectosList', null, false);
            $("#selectProyecto").fillCombo('/ControlObra/ControlObra/GetProyectosList', null, false);
            $("#selectProyectoEdit").fillCombo('/ControlObra/ControlObra/GetProyectosList', null, false);
            convertToMultiselect('#multiSelectProyectos');
        }

        function fnBuscarFases() {
            var arrProyectos = getValoresMultiples('#multiSelectProyectos');

            $.ajax({
                url: '/ControlObra/ControlObra/GetFasesCatalogo',
                datatype: "json",
                type: "POST",
                data: {
                    listProyectoID: arrProyectos
                },
                success: function (response) {
                    tblFases.clear();
                    tblFases.rows.add(response.items);
                    tblFases.draw();
                }
            });
        }

        function fnAbrirModalFaseNueva() {
            dialogFase.modal('show');
            $("#txtFaseNueva").val("");
            dpFechaInicio.datepicker("setDate", hoy);
            dpFechaFin.datepicker("setDate", hoy);
            $("#txtFaseNueva").focus();
        }

        function fnAgregarFase() {
            if ($("#txtFaseNueva").val() != "" && $('#dpFechaInicio').val() != null && $('#dpFechaFin').val() != "" && $('#selectProyecto').val() != "") {
                var request = new XMLHttpRequest();
                request.open("POST", "/ControlObra/ControlObra/GuardarFase");
                request.send(formData());
                request.onload = function (response) {
                    if (request.status == 200) {
                        AlertaGeneral("Aviso", "Fase guardada correctamente.");
                        recargarTodo()
                    }
                };
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }

        function formData() {
            let formData = new FormData();
            formData.append("fase", JSON.stringify(getFase()));
            return formData;
        }

        function getFase() {
            return {

                fase: $('#txtFaseNueva').val(),
                fechaInicio: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                fechaFin: moment(dpFechaFin.val(), "DD-MM-YYYY").format(),
                proyectoID: $('#selectProyecto').val(),
                estatus: true
            };
        }

        function fnEditarFase() {
            const url = 'UpdateFase';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    faseID: $("#btnGuardarFaseEdit").val(),
                    fase: $("#txtFaseEdit").val(),
                    fechaInicio: dpFechaInicioEdit.val(),
                    fechaFin: dpFechaFinEdit.val(),
                    proyectoID: $("#selectProyectoEdit").val(),
                    estatus: true
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    recargarTodo();
                }
            });
        }

        function recargarTodo() {
            tblFases.ajax.reload(null, false);
            dialogFase.modal('hide');
            dialogFaseEdit.modal('hide');
            initCbo();
        }

        init();
    };

    $(document).ready(function () {
        controlObra.FaseCatalogo = new FaseCatalogo();
    });
})();