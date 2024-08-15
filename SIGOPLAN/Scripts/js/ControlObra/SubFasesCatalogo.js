(function () {
    $.namespace('controlObra.SubFasesCatalogo');

    SubFasesCatalogo = function () {
        tblSubFases = $("#tblSubFases");
        btnBuscarSubFase = $("#btnBuscarSubFase");
        btnNuevaSubFase = $("#btnNuevaSubFase");
        btnGuardarSubFaseNueva = $("#btnGuardarSubFaseNueva");
        btnGuardarSubFaseEdit = $("#btnGuardarSubFaseEdit");

        dialogSubFase = $('#dialogSubFase');
        dialogSubFaseEdit = $('#dialogSubFaseEdit');

        let subFases = [];
        const hoy = new Date();
        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');
        dpFechaInicioEdit = $('#dpFechaInicioEdit');
        dpFechaFinEdit = $('#dpFechaFinEdit');

        function init() {
            initTableSubFases();
            initCbo();

            btnBuscarSubFase.click(fnBuscarSubFases);
            btnNuevaSubFase.click(fnAbrirModalSubFase);
            btnGuardarSubFaseNueva.click(fnAgregarSubFase);
            btnGuardarSubFaseEdit.click(fnEditarSubFase);

            $("#SelectProyectoFiltro").change(fnProyectoFiltroChange);
            $("#selectProyecto").change(fnProyectoChange);
            $("#selectProyectoEdit").change(fnProyectoEditChange);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaInicioEdit.datepicker({ dateFormat: 'dd/mm/yy' });
            dpFechaFinEdit.datepicker({ dateFormat: 'dd/mm/yy' });
        }

        function initTableSubFases() {
            const gruopProyecto = 0;
            const groupFase = 1;

            tblSubFases = $("#tblSubFases").DataTable({
                retrieve: true,
                paging: false,
                data: subFases,
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                sortable: false,
                rowId: 'id',
                initComplete: function (settings, json) {
                    tblSubFases.on('click', '.btn-editar-subFase', function () {
                        var rowData = tblSubFases.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/ControlObra/ControlObra/GetSubFase',
                            data: { subFaseID: rowData["id"] },
                            success: function (response) {
                                const data = response.subfase;
                                const fechaInicio = new Date(moment(data.fechaInicio, "DD-MM-YYYY").format());
                                const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());

                                $("#btnGuardarSubFaseEdit").val(rowData["id"]);
                                $("#txtSubFaseEdit").val("");

                                dialogSubFaseEdit.modal('show');

                                $("#txtSubFaseEdit").val(data.subFase);
                                dpFechaInicioEdit.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaInicio);
                                dpFechaFinEdit.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", fechaFin);
                                $("#selectProyectoEdit").val(data.proyectoID);
                                $("#selectFaseEdit").fillCombo('/ControlObra/ControlObra/GetFasesList', { proyectoID: data.proyectoID }, false);
                                $("#selectFaseEdit").val(data.faseID);
                            }
                        });
                    });

                    tblSubFases.on('click', '.btn-eliminar-subFase', function () {
                        var rowData = tblSubFases.row($(this).closest('tr')).data();
                        $("#dialogEliminarSubFase").dialog({
                            modal: true,
                            open: function () {
                                $("#txtEliminarSubFase").text("¿Está seguro que desea eliminar la Subfase '" + rowData["subFase"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/ControlObra/ControlObra/RemoveSubFase',
                                        data: { subFaseID: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogEliminarSubFase").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogEliminarSubFase").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    {
                        data: 'proyecto', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'fase', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'subFase', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'fechaInicio', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'fechaFin', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-editar-subFase btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-eliminar-subFase btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';
                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { visible: false, targets: gruopProyecto },
                    { visible: false, targets: groupFase },
                    { "width": "800px", "targets": 2 },
                    { "width": "10px", "targets": 3 },
                    { "width": "10px", "targets": 4 },
                    { "width": "5px", "targets": 5 },
                    { "width": "5px", "targets": 6 },
                ],
                order: [[gruopProyecto, 'asc']],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(gruopProyecto, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td style="background-color: #ffcc00" colspan="7">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });

                    api.column(groupFase, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td style="background-color: #f8cbad" colspan="7">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });

                }
            });
        }

        function initCbo() {
            $("#SelectProyectoFiltro").fillCombo('/ControlObra/ControlObra/GetProyectosList', null, false);
            $("#selectProyecto").fillCombo('/ControlObra/ControlObra/GetProyectosList', null, false);
            $("#selectProyectoEdit").fillCombo('/ControlObra/ControlObra/GetProyectosList', null, false);
        }

        function fnProyectoFiltroChange() {
            const proyectoID = $('#SelectProyectoFiltro').val() == "" ? -1 : $('#SelectProyectoFiltro').val();
            $("#SelectFaseFiltro").fillCombo('/ControlObra/ControlObra/GetFasesList', { proyectoID: proyectoID }, false);
        }

        function fnProyectoChange() {
            const proyectoID = $('#selectProyecto').val() == "" ? -1 : $('#selectProyecto').val();
            $("#selectFase").fillCombo('/ControlObra/ControlObra/GetFasesList', { proyectoID: proyectoID }, false);
        }

        function fnProyectoEditChange() {
            const proyectoID = $('#selectProyectoEdit').val() == "" ? -1 : $('#selectProyectoEdit').val();
            $("#selectFaseEdit").fillCombo('/ControlObra/ControlObra/GetFasesList', { proyectoID: proyectoID }, false);
        }

        function fnBuscarSubFases() {
            debugger;
            const faseID = $('#SelectFaseFiltro').val() == "" ? -1 : $('#SelectFaseFiltro').val();
            $.post('/ControlObra/ControlObra/GetSubFasesCataogo', { faseID: faseID })
                .done(response => {
                    debugger;

                    if (response.EMPTY) {
                        clearDt($("#tblSubFases"));
                    } else {
                        subFases = response.items;
                        AddRows($("#tblSubFases"), subFases)
                    }

                });
        }

        function fnAbrirModalSubFase() {
            dialogSubFase.modal('show');
            $("#txtSubFaseNueva").val("");
            dpFechaInicio.datepicker("setDate", hoy);
            dpFechaFin.datepicker("setDate", hoy);
            $("#txtSubFaseNueva").focus();
        }

        function fnAgregarSubFase() {
            if ($("#txtSubFaseNueva").val() != "" && $('#dpFechaInicio').val() != null && $('#dpFechaFin').val() != "" && $('#selectFase').val() != "") {
                var request = new XMLHttpRequest();
                request.open("POST", "/ControlObra/ControlObra/GuardarSubFase");
                request.send(formData());
                request.onload = function (response) {
                    if (request.status == 200) {
                        AlertaGeneral("Aviso", "SubFase guardada correctamente.");
                        recargarTodo()
                    }
                };
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }

        function formData() {
            let formData = new FormData();
            formData.append("subfase", JSON.stringify(getSubFase()));
            return formData;
        }

        function getSubFase() {
            return {
                subFase: $('#txtSubFaseNueva').val(),
                fechaInicio: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                fechaFin: moment(dpFechaFin.val(), "DD-MM-YYYY").format(),
                faseID: $('#selectFase').val(),
                estatus: true
            };
        }


        function fnEditarSubFase() {
            const url = 'UpdateSubFase';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    subFaseID: $("#btnGuardarSubFaseEdit").val(),
                    subFase: $("#txtSubFaseEdit").val(),
                    fechaInicio: dpFechaInicioEdit.val(),
                    fechaFin: dpFechaFinEdit.val(),
                    faseID: $("#selectFaseEdit").val(),
                    estatus: true
                },
                success: function (data) {
                    debugger;
                    AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    recargarTodo();
                }
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }

        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }

        function clearDt(tbl) {
            dt = tbl.DataTable();
            dt
                .clear()
                .draw();
        }

        function recargarTodo() {
            dialogSubFase.modal('hide');
            dialogSubFaseEdit.modal('hide');
            fnBuscarSubFases();
            initCbo();
        }


        init();
    };


    $(document).ready(function () {
        controlObra.SubFasesCatalogo = new SubFasesCatalogo();
    });
})();