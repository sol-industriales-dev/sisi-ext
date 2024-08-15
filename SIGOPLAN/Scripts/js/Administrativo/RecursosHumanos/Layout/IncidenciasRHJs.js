$(function () {

    $.namespace('recursoshumanos.Layout.AltasRH');

    AltasRH = function () {


        mensajes = {
            PROCESANDO: 'Procesando...'
        };

        selected = false;
        tblEmpleadosNomina = $("#tblEmpleadosNomina"),
        btnBuscar = $("#btnBuscar"),
        btnExportar = $("#btnExportar"),
        fechaIni = $("#fechaIni"),
        fechaFin = $("#fechaFin"),
        btnSeleccionar = $("#btnSeleccionar"),
        cboCC = $("#cboCC");

        function init() {
            cboCC.fillCombo('/Administrativo/ReportesRH/FillComboCC', { est: true }, false, "Todos");
            convertToMultiselect("#cboCC");

            fechaIni.datepicker().datepicker("setDate", new Date());
            fechaFin.datepicker().datepicker("setDate", new Date());

            btnBuscar.click(loadTabla);
            btnExportar.click(exportData);
            btnSeleccionar.click(selectAll);
        }

        function getFiltrosObject() {
            return {
                cc: getValoresMultiples("#cboCC"),
                fechaInicio: fechaIni.val(),
                fechaFin: fechaFin.val()
            }
        }
        

        function selectAll() {

            if (!selected)
            {
                selected = true;
                $("input:checkbox[name=cckEmpleados]").prop('checked', true);
            }
            else {
                selected = false;
                $("input:checkbox[name=cckEmpleados]").prop('checked', false);
            }
            
        }

        function loadTabla() {
            loadGrid(getFiltrosObject(), "/Administrativo/LayoutRH/fillTableLayoutIncidenciasRH", tblEmpleadosNomina);
        }

        function exportData() {

            var array = [];
            $("input:checkbox[name=cckEmpleados]:checked").each(function () {
                array.push($(this).attr('data-cveEmpleado'));
            });

            if (array.length > 0) {
                getFileDownload(array);
            }
            else {
                AlertaGeneral("Alerta","Debe seleccionar almenos un registro para exportar!");
            }
           // window.location = '/Administrativo/LayoutRH/getFileDownload';
        }

        function getFileDownload(array) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Administrativo/LayoutRH/ExportInformacion',
                type: "POST",
                datatype: "json",
                data: { empleados: array },
                success: function (response) {
                    $.unblockUI();
                    
                    window.location = '/Administrativo/LayoutRH/getFileDownloadIncidencias';
                    AlertaGeneral("Confirmación", "¡Archivo Generado Correctamente!");
                    
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }


        function initGrid() {

            tblEmpleadosNomina.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "seleccion": function (column, row) {
                        return "<input type='checkbox' style=\"width: 15px;height: 15px;\" name='cckEmpleados' data-cveEmpleado='" + row.EMP_CLAVE + "'>";
                    }

                }
            });
        }
        initGrid();
        init();
    }

    $(document).ready(function () {
        recursoshumanos.Layout.AltasRH = new AltasRH();
    });
});