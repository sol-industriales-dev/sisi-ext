
(function () {

    $.namespace('maquinaria.Rastreo.ReporteTiemposSolicitud');

    ReporteTiemposSolicitud = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        ireport = $("#report"),
        btnImprimirReporte = $("#btnImprimirReporte")
        tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas"),
        cboListaCC = $("#cboListaCC");
        mensajes = {
            NOMBRE: 'Reporte de tiempo de solicitudes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        tbFechaIncio = $("#tbFechaIncio"),
        tbFechaFin = $("#tbFechaFin"),
        tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas").DataTable({});

        function init() {

            cboListaCC.fillCombo('/MovimientoMaquinaria/cboGetCentroCostos', null, false, "Todos");
            convertToMultiselect("#cboListaCC")

            btnImprimirReporte.click(verReporte);
            // datePicker();
            var now = new Date(),
            year = now.getYear() + 1900;
            tbFechaIncio.datepicker().datepicker("setDate", "01/01/" + year);
            tbFechaFin.datepicker().datepicker("setDate", new Date());

            loadTabla();
            cboListaCC.change(loadTabla);
            tbFechaIncio.change(loadTabla);
            tbFechaFin.change(loadTabla);
        }

        function verReporte(e) {

            $.blockUI({ message: mensajes.PROCESANDO });
            //var idReporte = "";

            var Periodo = "Periodo: del " + tbFechaIncio.val() + " Al " + tbFechaFin.val();
            var path = "/Reportes/Vista.aspx?idReporte=54&pPeriodo=" + Periodo;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
            e.preventDefault();
        }

        function loadTabla() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Rastreos/RptListaEquiposAsignados",
                type: "POST",
                datatype: "json",
                data: { CentroCostos: getValoresMultiples("#cboListaCC"), pFechaInicio: tbFechaIncio.val(), pFechaFin: tbFechaFin.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.DataTables == undefined ? null : response.DataTables;
                    SetDataInTables(data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetDataInTables(dataSet) {
            tblSolcitudesAprobadas.clear().draw();
            tblSolcitudesAprobadas = $("#tblSolcitudesAprobadas").DataTable({
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del START al END de un total de TOTAL registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de MAX registros)",
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
                responsive: true,
                "bFilter": false,
                destroy: true,
                select: true,
                scrollY: '50vh',
                data: dataSet,
                columns: [

                     {
                         data: "folio"
                     },
                     {
                         data: "FechaAdministrador"
                     },
                     {
                         data: "FechaGerente"
                     },
                     {
                         data: "DirectorArea"
                     }, {
                         data: "DirectorDivision"
                     },
                     {
                         data: "DirectorGeneral"
                     },
                    {
                        data: "Asigno"
                    },
                     {
                         data: "TiempoTotal"
                     }
                ],
                "paging": false,
                "info": false

            });
        }

        function datePicker() {
            var now = new Date(),
            year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
              from = $("#fechaIni")
                .datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(year, 00, 01),
                    maxDate: new Date(year, 11, 31),

                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));
                        $(this).trigger('change');
                    }

                })
                .on("change", function () {
                    to.datepicker("option", "minDate", getDate(this));
                }),
              to = $("#fechaFin").datepicker({
                  changeMonth: true,
                  changeYear: true,
                  numberOfMonths: 1,
                  defaultDate: new Date(),
                  maxDate: new Date(year, 11, 31),
                  onChangeMonthYear: function (y, m, i) {
                      var d = i.selectedDay;
                      $(this).datepicker('setDate', new Date(y, m - 1, d));
                      $(this).trigger('change');
                  }
              })
              .on("change", function () {
                  from.datepicker("option", "maxDate", getDate(this));
              });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }

        init();
    };

    $(document).ready(function () {

        maquinaria.Rastreo.ReporteTiemposSolicitud = new ReporteTiemposSolicitud();
    });
})();

