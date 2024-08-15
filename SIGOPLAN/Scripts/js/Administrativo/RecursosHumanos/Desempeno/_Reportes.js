(() => {
    $.namespace('RecursosHumanos.Desempeno.Reportes');
    
    Reportes = function () {
        const cboProcesos = $("#cboProcesos");
        const cboEvaluaciones = $("#cboEvaluaciones");
        const btnBuscar = $("#btnBuscar");
        const tblData = $("#tblData");
        const getDatos = new URL(window.location.origin + '/Desempeno/CargarTblPersonalEvaluado');
        let init = () => {
            cboProcesos.fillCombo('/Desempeno/getCboTodosLosProcesos', { });
            cboEvaluaciones.fillCombo('/Desempeno/getCboEvaluacionPorProceso', { idProceso: (cboProcesos.val()==''?0:cboProcesos.val()) });
            cboProcesos.change(function(){
                cboEvaluaciones.fillCombo('/Desempeno/getCboEvaluacionPorProceso', { idProceso: (cboProcesos.val()==''?0:cboProcesos.val()) });
            });
            btnBuscar.click(setDatos);
            initDataTblDetalle();
        }
        function initDataTblDetalle() {
            dtData = tblData.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": false,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Reporte de Evaluaciones", "<center><h3>Reporte de Evaluaciones</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        exportOptions: {
                            // columns: [':visible', 21]
                            columns: [0, 1, 2,3,4,5]
                        }
                    }
                ],
                initComplete: function (settings, json) {

                },
                footerCallback: function (row, data, start, end, display) {

                },
                columns: [
                    { title:'Proceso' ,data: 'proceso'}
                   ,{ title:'Periodo' ,data: 'periodo'}
                   ,{ title:'Empleado' ,data: 'empleado'}
                   ,{ title:'Evaluador' ,data: 'evaluador'}
                   ,{ title:'Estatus' ,data: 'strEstatus'}
                   ,{ title:'Porcentaje' ,data: 'porcentaje'}
                   
                ]
            });
        }
        async function setDatos() {
            try {
                if(cboProcesos.val()!='' && cboEvaluaciones.val()!=''){
                    dtData.clear().draw();
                    //var url = cboEmpresa.val()!=2?getDatosGeneralesEmpresa:getDatosGenerales;
                    response = await ejectFetchJson(getDatos, {proceso:cboProcesos.val(), periodo: cboEvaluaciones.val()});
                    if (response.success) {
                        var data = response.lst;
                        dtData.rows.add(data).draw();
                    } else {
                        AlertaGeneral(`Erro`, `No se encontraron datos.`);
                    }
                }
                else{
                    AlertaGeneral('Alerta','Debe seleccionar una evaluación!');
                }
                
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        init();
    }
    $(document).ready(() => {
        RecursosHumanos.Desempeno.Reportes = new Reportes();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...'}); })
    .ajaxStop(() => { $.unblockUI(); });
})();