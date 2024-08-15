$(function () {
    $.namespace('MantenimientoPM.agrupacionComponenteLubricante');
    agrupacionComponenteLubricante = function () {
        tbPeriodo = $("#tbPeriodo"),
            tbLitros = $("#tbLitros"),
            cboComponentes_Lubricantes = $("#cboComponentes_Lubricantes"),
            cboLubricantes_Lubricantes = $("#cboLubricantes_Lubricantes"),
            btnAdd_Lubricantes = $("#btnAdd_Lubricantes"),
            tblComponentes_Lubricantes = $("#tblComponentes_Lubricantes"),
            cboFiltroModeloVinc = $("#cboFiltroModeloVinc");

        mensajes = {
            NOMBRE: 'Agrupacion Componentes Lubricantes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        function init() {

            cboLubricantes_Lubricantes.fillCombo('/matenimientopm/FillCboCatLubricantes');
            initTblA(tblComponentes_Lubricantes, objColumns());
            cboFiltroModeloVinc.change(filltblComponentes);
        }

        btnAdd_Lubricantes.on('click', function () {
            addData();
        });

        function objColumns() {

            var columns = [
                {
                    data: "componenteID", createdCell: function (td, cellData, rowData, row, col) {
                        $(td).text('');
                        $(td).append($("#cboComponentes_Lubricantes option[value=" + cellData + "]").text());

                    }
                },
                {
                    data: "lubricanteID", createdCell: function (td, cellData, rowData, row, col) {
                        $(td).text('');
                        $(td).append($("#cboLubricantes_Lubricantes option[value=" + cellData + "]").text());

                    }
                },
                {
                    data: "vidaLubricante", createdCell: function (td, cellData, rowData, row, col) {


                    }
                },
                { data: "cantidadLitros" },
                {
                    data: "id", createdCell: function (td, cellData, rowData, row, col) {
                        $(td).text('');

                        $(td).append('<button type="button" data-id="' + cellData + '" class="btn btn-default btn-block btn-sm btn-delete">Quitar</button>');

                    }
                }
            ];
            return columns;
        }

        function filltblComponentes() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/matenimientopm/fillTblComponenteLubricante',
                type: 'POST',
                dataType: 'json',
                data: { modeloID: cboFiltroModeloVinc.val() != undefined ? (cboFiltroModeloVinc.val() == "" ? 0 : cboFiltroModeloVinc.val()) : 0 },
                success: function (response) {
                    AddRows(tblComponentes_Lubricantes, response.dataSet);
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                }
            });

        }

        function addData() {

            var Data = [];
            obj = getObjInfo();

            if (obj != undefined) {
                saveOrUpdateAgrupacionComponete(obj)
            }

        }

        function getObjInfo() {

            obj = {};

            obj.id = 0;
            obj.componenteID = cboComponentes_Lubricantes.val();
            obj.lubricanteID = cboLubricantes_Lubricantes.val();
            obj.modeloID = cboFiltroModeloVinc.val()
            obj.vidaLubricante = tbPeriodo.val();
            obj.cantidadLitros = tbLitros.val();
            obj.usuarioID = 0
            obj.fechaCaptura = new Date();
            obj.estatus = true;

            return obj;
        }

        function initTblA(tblSet, columnsObj) {
            tblSet.DataTable({
                destroy: true,
                scrollCollapse: true,
                bFilter: true,
                paging: false,
                info: false,
                scrollY: '50vh',
                initComplete: function (settings, json) {
                    //tblSet.on('click', '.btn-delete', function () {
                    //    removeRow($(this).attr("data-id"))
                    //});
                },
                columns: columnsObj,
                columnDefs: [
                    { className: "dt-center", targets: "_all" }
                ]
            });
        }

        tblComponentes_Lubricantes.on('click', '.btn-delete', function () {
            removeRow($(this).attr("data-id"))
        });


        function removeRow(id) {
            var Data = [];
            Data = tblComponentes_Lubricantes.DataTable().data();
            DeleteRow(id)
        }

        function DeleteRow(id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MatenimientoPM/DeleteLubricanteComponente',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: id, modeloID: cboFiltroModeloVinc.val() }),
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

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function saveOrUpdateAgrupacionComponete(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/MatenimientoPM/SaveOrUpdateComponenteLubricante',
                type: 'POST',
                dataType: 'json',
                data: { obj: obj },
                success: function (response) {
                    ConfirmacionGeneral("Confirmación", response.message, "bg-green");
                    filltblComponentes();
                    $.unblockUI();
                },
                error: function (response) {
                    AlertaGeneral("Alerta", response.message);
                    $.unblockUI();
                }
            });
        }

        init();
    }
    $(document).ready(function () {
        MantenimientoPM.agrupacionComponenteLubricante = new agrupacionComponenteLubricante();
    });
});