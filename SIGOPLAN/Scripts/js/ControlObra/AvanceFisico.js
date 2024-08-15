(function () {
    $.namespace('controlObra.AvanceFisico');

    avanceFisico = function () {
        btnFiltrarProyecto = $("#btnFiltrarProyecto");
        tblAvanceDetalle = $("#tblAvanceDetalle");
        btnImprimir = $("#btnImprimir");
        report = $("#report");

        let avanceDetalle = [];

        function init() {
            initCbo();
            initTableAvanceDetalle();
            btnImprimir.click(verReporte);
            btnFiltrarProyecto.click(filtrarProyecto);
        }

        function initTableAvanceDetalle() {
            var groupColumn = 0;

            tblAvanceDetalle = $("#tblAvanceDetalle").DataTable({
                retrieve: true,
                paging: false,
                data: avanceDetalle,
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                searching: false,
                sortable: false,
                initComplete: function (settings, json) {
                },
                columns: [
                    {
                        data: 'fase', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'faseActividad', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'unidad', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'cantidad', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'faseActInicio', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            const date = new Date(parseInt(cellData.substr(6)));
                            const fecha = (date.getUTCDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear())
                            $(td).html(fecha);
                        }
                    },
                    {
                        data: 'faseActFin', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            const date = new Date(parseInt(cellData.substr(6)));
                            const fecha = (date.getUTCDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear())
                            $(td).html(fecha);
                        }
                    },
                    {
                        data: 'acumAnterior', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'avancePeriodo', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'acumActual', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'avancePeriodoPor', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'avanceAcumActual', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'observaciones', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    }
                ],
                columnDefs: [
                    { visible: false, targets: groupColumn },
                    { "width": "800px", "targets": 1 },
                    { "width": "10px", "targets": 2 },
                    { "width": "10px", "targets": 3 },
                    { "width": "10px", "targets": 4 },
                    { "width": "10px", "targets": 5 },
                    { "width": "10px", "targets": 6 },
                    { "width": "10px", "targets": 7 },
                    { "width": "10px", "targets": 8 },
                    { "width": "10px", "targets": 9 },
                    { "width": "10px", "targets": 10 },
                    { "width": "30px", "targets": 9 },


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
            $("#selectProyectos").fillCombo('/ControlObra/ControlObra/GetProyectosList', null, false);
        }

        function verReporte() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=102`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function filtrarProyecto() {
            debugger;
            const proyectoID = $('#selectProyectos').val() == "" ? -1 : $('#selectProyectos').val();
            $.post('/ControlObra/ControlObra/GetAvanceFisicoDetalle', { proyectoID: proyectoID })
                .done(response => {
                    debugger;

                    if (response.EMPTY) {
                        clearDt($("#tblAvanceDetalle"));
                    } else {
                        avanceDetalle = response.items;
                        AddRows($("#tblAvanceDetalle"), avanceDetalle)
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

        init();
    };

    $(document).ready(function () {
        controlObra.AvanceFisico = new avanceFisico();
    });
})();