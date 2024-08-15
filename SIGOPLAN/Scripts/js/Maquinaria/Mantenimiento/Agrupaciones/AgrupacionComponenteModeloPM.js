$(function () {
    $.namespace('MantenimientoPM.AgrupacionComponenteModeloPM');
    AgrupacionComponenteModeloPM = function () {

        cboGroupGrupoMaquinaria = $("#cboFiltroGrupoVinc"),
        cboGroupModeloMaquinaria = $("#cboFiltroModeloVinc"),
        cboGroupComponente = $("#cboGroupComponente"),
        btnAgregarComponente = $("#btnAgregarComponente"),
        tblGroupComponentes = $("#tblGroupComponentes");

        function initCompontenteModeloPM() {

            cboGroupComponente.fillCombo('/matenimientopm/FillCboCatComponentes');
            btnAgregarComponente.click(addRow);
            initTblGrupoModeloComponente();
            cboGroupModeloMaquinaria.change(filltblComponentes);

        }

        function filltblComponentes() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/matenimientopm/filltblAgrupacionComponenteModelo',
                type: 'POST',
                dataType: 'json',
                data: { modeloID: cboGroupModeloMaquinaria.val() != undefined ? (cboGroupModeloMaquinaria.val() == "" ? 0 : cboGroupModeloMaquinaria.val()) : 0 },
                success: function (response) {
                    if (response.success) {
                        AddRows(tblGroupComponentes, response.dataSet);
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function addRow() {

            var Data = [];
            obj = {};
            Data = tblGroupComponentes.DataTable().data();

            obj.id = 0;
            obj.modelo = $("#cboFiltroModeloVinc option:selected").text();
            obj.componente = $("#cboGroupComponente option:selected").text();
            obj.componenteID = cboGroupComponente.val();
            obj.modeloID = cboGroupModeloMaquinaria.val();
            obj.estatus = true;
            obj.usuariosCaptura = 0;
            obj.fechaCaptura = new Date();

            if (Data.count() == 0) {
                saveOrUpdate(obj);
            }
            else {
                var result = jQuery.grep(Data, function (n, i) {
                    return (n.componenteID === cboGroupComponente.val());
                });

                if (result.length == 0) {
                    saveOrUpdate(obj);
                }
                else {
                    AlertaGeneral('Alerta', 'El componente ya se encuentra agregado')
                }
            }
        }

        function removeRow(id) {
            var Data = [];
            Data = tblGroupComponentes.DataTable().data();
            DeleteAgrupacionComponenteModelo(id)
        }

        function initTblGrupoModeloComponente() {
            tblGroupComponentes.DataTable({
                destroy: true,
                scrollCollapse: true,
                bFilter: true,
                paging: false,
                info: false,
                scrollY: '50vh',
                initComplete: function (settings, json) {
                    tblGroupComponentes.on('click', '.btn-delete', function () {
                        removeRow($(this).attr("data-id"))
                    });
                },
                columns: [
                    { data: "modelo" },
                    { data: "componente" },
                    {
                        data: "id", createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');

                            $(td).append('<button type="button" class="btn btn-default btn-block btn-sm btn-delete" data-id=' + rowData.id + '  data-componenteID=' + rowData.componenteID + '>X</button>');

                        }
                    }
                ],
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

        function saveOrUpdate(getInfoObject) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MatenimientoPM/SaveOrUpdateAgrupacionComponenteModelo',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: getInfoObject }),

                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    $("#cboComponentes_Lubricantes").fillCombo('/matenimientopm/FillCboCatComponentesByModelo', { modeloID: cboFiltroModeloVinc.val() });
                    $("#cboGroupComponente_Filtros").fillCombo('/matenimientopm/FillCboCatComponentesByModelo', { modeloID: cboFiltroModeloVinc.val() });
                    filltblComponentes();
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function DeleteAgrupacionComponenteModelo(getInfoObject) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MatenimientoPM/DeleteAgrupacionComponenteModelo',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: getInfoObject }),
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    
                    filltblComponentes();
                    $("#cboComponentes_Lubricantes").fillCombo('/matenimientopm/FillCboCatComponentesByModelo', { modeloID: cboFiltroModeloVinc.val() });
                    $("#cboGroupComponente_Filtros").fillCombo('/matenimientopm/FillCboCatComponentesByModelo', { modeloID: cboFiltroModeloVinc.val() });
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        initCompontenteModeloPM();
    }
    $(document).ready(function () {
        MantenimientoPM.AgrupacionComponenteModeloPM = new AgrupacionComponenteModeloPM();
    });
});