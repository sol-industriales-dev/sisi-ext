$(function () {
    $.namespace('MantenimientoPM.AgrupacionComponenteFiltroPM');
    AgrupacionComponenteFiltroPM = function () {


        tbFiltrosAuto = $("#tbFiltrosAuto"),
            cboFiltroModeloVinc = $("#cboFiltroModeloVinc"),
            cboGroupComponente_Filtros = $("#cboGroupComponente_Filtros"),
            btnAddFiltro = $("#btnAddFiltro"),
            cboFiltro_Filtros = $("#cboFiltro_Filtros"),
            tbCantidadComponentes = $("#tbCantidadComponentes"),
            tblComponentes_Filtros = $("#tblComponentes_Filtros");


        function init() {
            //cboFiltro_Filtros.fillCombo('/matenimientopm/FillCboCatFiltros')
            cboFiltroModeloVinc.change(filltblFiltros);
            initTbl(tblComponentes_Filtros, objColumns());

            tbFiltrosAuto.getAutocomplete(SelectFiltro, null, '/matenimientopm/FillCboCatFiltros');

        }

        function SelectFiltro(event, ui) {
            tbFiltrosAuto.text(ui.item.value);
            tbFiltrosAuto.attr('data-filtroID', ui.item.id)
        }

        function removeRow(id) {
            var Data = [];
            Data = tblComponentes_Filtros.DataTable().data();
            DeleteRow(id)
        }

        function DeleteRow(id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MatenimientoPM/DeleteFiltrosComponente',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: id, modeloID: cboFiltroModeloVinc.val() }),
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");

                    filltblFiltros();
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        btnAddFiltro.click(SaveOrUpdateAgrupacionComponenteFiltro);

        function objColumns() {

            var columns = [
                {
                    data: "componenteID", createdCell: function (td, cellData, rowData, row, col) {
                        $(td).text('');
                        $(td).append($("#cboGroupComponente_Filtros option[value=" + cellData + "]").text());

                    }
                },
                {
                    data: "descripcion", createdCell: function (td, cellData, rowData, row, col) {


                    }
                },
                {
                    data: "insumo", createdCell: function (td, cellData, rowData, row, col) {


                    }
                },
                {
                    data: "Codigo", createdCell: function (td, cellData, rowData, row, col) {


                    }
                },
                {
                    data: "cantidad"
                },
                {
                    data: "id", createdCell: function (td, cellData, rowData, row, col) {
                        $(td).text('');
                        $(td).append('<button type="button" data-id="' + cellData + '" class="btn btn-default btn-block btn-sm btn-delete">Quitar</button>');
                    }
                }
            ];

            return columns;
        }

        function filltblFiltros() {
            $.blockUI({ message: mensajes.PROCESANDO });
            cboGroupComponente_Filtros.fillCombo('/matenimientopm/FillCboCatComponentesByModelo', { modeloID: cboFiltroModeloVinc.val() });

            $.ajax({
                url: '/matenimientopm/FillTblComponenteFiltro',
                type: 'POST',
                dataType: 'json',
                data: { modeloID: cboGroupModeloMaquinaria.val() != undefined ? (cboGroupModeloMaquinaria.val() == "" ? 0 : cboGroupModeloMaquinaria.val()) : 0 },
                success: function (response) {
                    if (response.success) {
                        AddRows(tblComponentes_Filtros, response.dataSet);
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function getObjFiltros() {
            obj = {};

            obj.id = 0;
            obj.modeloID = $("#cboFiltroModeloVinc").val();
            obj.componenteID = cboGroupComponente_Filtros.val();
            obj.filtroID = tbFiltrosAuto.attr('data-filtroID'),// cboFiltro_Filtros.val();
                obj.estatus = true;
            obj.usuariosCaptura = 0;
            obj.fechaCaptura = new Date();
            obj.cantidad = tbCantidadComponentes.val();

            return obj;
        }

        function initTbl(tblSet, columnsObj) {
            tblSet.DataTable({
                destroy: true,
                scrollCollapse: true,
                bFilter: true,
                paging: false,
                info: false,
                scrollY: '50vh',
                initComplete: function (settings, json) {

                },
                columns: columnsObj,
                columnDefs: [
                    { className: "dt-center", targets: "_all" }
                ]
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }


        tblComponentes_Filtros.on('click', '.btn-delete', function () {
            removeRow($(this).attr("data-id"))
        });
        function SaveOrUpdateAgrupacionComponenteFiltro() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MatenimientoPM/SaveOrUpdateComponenteFiltro',
                type: 'POST',
                dataType: 'json',
                data: { obj: getObjFiltros() },
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    filltblFiltros();
                    tbFiltrosAuto.val('');
                    tbCantidadComponentes.val('');
                    tbFiltrosAuto.removeAttr('data-filtroID');


                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        init();
    }
    $(document).ready(function () {
        MantenimientoPM.AgrupacionComponenteFiltroPM = new AgrupacionComponenteFiltroPM();
    });
});