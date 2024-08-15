(function () {
    $.namespace('sigoplan.seguimientoacuerdos.reporteactividades');
    reporteactividades = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        },
        _ID = 0,
        cboDepartamento = $("#cboDepartamento"),
        cboEstatus = $("#cboEstatus"),
        txtFechaInicio = $("#txtFechaInicio"),
        txtFechaFin = $("#txtFechaFin"),
        btnCargar = $("#btnCargar"),
        tblData = $("#tblData");
        function init() 
        {
            cboDepartamento.fillCombo('/SeguimientoAcuerdos/FillComboDepartamentos', { est: true }, false, "Todos");
            convertToMultiselect("#cboDepartamento");
            btnCargar.click(fnCargar);
            txtFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' });
            txtFechaFin.datepicker({ dateFormat: 'dd/mm/yy' });
            initTable();
        }
        function fnCargar() {
            $.blockUI({ message: 'Cargando información...' });
            tblData.ajax.reload(null, false);
        }
        function initTable() {
            tblData = $("#tblData").DataTable({
                ajax: {
                    url: '/SeguimientoAcuerdos/getReporteActividades',
                    dataSrc: 'dataMain',
                    type: "POST",
                    data: function (d) {
                        d.Departamentos = JSON.stringify(getValoresMultiples("#cboDepartamento")),
                        d.FechaInicio = txtFechaInicio.val(),
                        d.FechaFin = txtFechaFin.val(),
                        d.Estatus = cboEstatus.val()
                    }
                },
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                columns: [
                    { data: 'actividadID' },
                    { data: 'actividad' },
                    { data: 'responsable' },
                    { data: 'fechaInicio' },
                    { data: 'fechaFin' },
                    { data: 'prioridad' },
                    { data: 'departamento' },
                    { data: 'minuta' },
                    { data: 'creadorMinuta' }
                ],
                columnDefs: [
                    { targets: 0, "visible": false }
                ],
                dom: 'Bfrtip',
                "drawCallback": function (settings) {
                    $.unblockUI();
                },
                buttons: parametrosImpresion("Reporte de actividades", "<center><h3>Reporte de actividades (" + $("#cboEstatus option:selected").text() + ") <br/>del " + txtFechaInicio.val() + " al " + txtFechaFin.val() + "</h3></center>")
            });
        }
        init();
    };
    $(document).ready(function () {
        sigoplan.seguimientoacuerdos.reporteactividades = new reporteactividades();
    });
})();
