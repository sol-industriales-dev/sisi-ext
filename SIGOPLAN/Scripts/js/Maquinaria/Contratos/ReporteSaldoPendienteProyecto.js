(() => {
    $.namespace("Maquinaria.DocumentosPorPagar.ReporteSaldoPendienteProyecto");

    ReporteSaldoPendienteProyecto = function (){
        //#region CONST
        const cboProyecto = $('#cboProyecto');
        const cboDivision = $('#cboDivision');
        const btnBuscar = $('#btnBuscar');
        const tblAF_DxP_Divisiones_Proyecto = $('#tblAF_DxP_Divisiones_Proyecto');
        let dtDivisionesProyecto;
        //#endregion

        (function init() {
            fncListeners();
            initTablaDivisionesProyecto();
            fillCboDivisiones();
            fillCboProyectos();
        })();

        function fncListeners(){
            btnBuscar.click(function (e){
                fncObtenerListadoDivisiones();
            });

            cboDivision.change(function (e){
                fillCboProyectos();
            });
        }

        function fillCboProyectos(){
            cboProyecto.fillCombo('/Contratos/ObtenerCboCC', { lstDivisionID: cboDivision.val() }, true);
            cboProyecto.multiselect('enable');
            convertToMultiselectSelectAll(cboProyecto);
        }

        function fillCboDivisiones(){
            cboDivision.fillCombo('/Contratos/ObtenerCboDivisiones', {}, true);
            cboDivision.multiselect("enable");
            convertToMultiselectSelectAll(cboDivision);
        }

        function initTablaDivisionesProyecto() {
            dtDivisionesProyecto = tblAF_DxP_Divisiones_Proyecto.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                scrollY: '52vh',
                scrollCollapse: true,
                scrollX: true,
                "bLengthChange": false,
                "autoWidth": false,
                dom: 'Bfrtip',
                "order": [[2, "desc"]],
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte detalle Adeudos", "<center><h3>Reporte Detalle Adeudos </h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                        }
                    }
                ],
                columns: [
                    { data: "cc", title: "Proyecto" },
                    { data: "division", title: 'División' },
                    { 
                        data: "saldoPendiente", title: 'Saldo pendiente', 
                        render: (data, type, row) => {
                            return maskNumero(data.toFixed(2));
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function fncObtenerListadoDivisiones() {
            let objFiltro = fncObtenerFiltro();
            if (objFiltro != null){
                axios.post("ObtenerListadoDivisiones", objFiltro).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        dtDivisionesProyecto.clear();
                        dtDivisionesProyecto.rows.add(response.data.reporte).draw();
                    }else{
                        Alert2Warning(response.data.message);
                    }
                }).catch(error => Alert2Error(error.message));
            }else{
                Alert2Warning("Es necesario seleccionar un CC.");
            }
        }

        function fncObtenerFiltro(){
            let objFiltro = new Object();
            objFiltro = {
                lstCC: cboProyecto.val()
            };
            return objFiltro;
        }
    }

    $(() => Maquinaria.DocumentosPorPagar.ReporteSaldoPendienteProyecto = new ReporteSaldoPendienteProyecto())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();