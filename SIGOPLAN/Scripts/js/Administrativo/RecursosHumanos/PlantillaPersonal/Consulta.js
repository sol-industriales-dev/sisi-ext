(function () {
    $.namespace('sigoplan.rh.plantillapersonal');
    plantillapersonal = function () {
        cboCC = $("#cboCC"),
            txtFechaInicio = $("#txtFechaInicio"),
            txtFechaFin = $("#txtFechaFin"),
            btnBuscar = $("#btnBuscar"),
            btnExportar = $("#btnExportar"),
            tblData = $("#tblData");
        function init() {
            initTable();
            cboCC.fillCombo('/Administrativo/PlantillaPersonal/FillComboCC', { plantilla: true }, false);
            btnBuscar.click(fnCargar);
            btnExportar.click(fnReporte);
        }
        function initTable() {
            tblData = $("#tblData").DataTable({
                ajax: {
                    url: '/Administrativo/PlantillaPersonal/GetPlantillasEK',
                    dataSrc: 'dataMain',
                    data: function (d) {
                        d.cc = (cboCC.val() == undefined ? '' : cboCC.val())
                    }
                },
                scrollCollapse: true,
                bFilter: true,
                paging: false,
                info: false,
                scrollY: '50vh',
                columns: [
                    { data: 'fechaInicio' },
                    { data: 'fechaFin' },
                    { data: 'id' },
                    { data: 'puesto' },
                    { data: 'departamento' },
                    { data: 'nomina' },
                    { data: 'personalOriginal', width: "150px" },
                    { data: 'personalActual', width: "150px" },
                    { data: 'sueldoBase', width: "150px" },
                    { data: 'sueldoComplemento', width: "150px" },
                    { data: 'sueldoTotal', width: "150px" }
                ],
                columnDefs: [
                    { targets: 0, "visible": false },
                    { targets: 1, "visible": false }
                ],
                drawCallback: function (settings) {
                    $("#tblData").DataTable().columns().every(function (colIdx, tableLpp, rowLoop) {
                        $(this.footer()).html('');

                        let totalOriginal = 0;
                        let totalActual = 0;

                        switch (colIdx) {
                            case 6:
                                for (let x = 0; x < this.data().length; x++) {
                                    totalOriginal += this.data()[x];
                                }
                                $(this.footer()).html(totalOriginal);
                                break;
                            case 7:
                                for (let x = 0; x < this.data().length; x++) {
                                    totalActual += this.data()[x];
                                }
                                $(this.footer()).html(totalActual);
                                break;
                        }
                    });
                },
                rowCallback: function (row, data) {
                    txtFechaInicio.val(data.fechaInicio);
                    txtFechaFin.val((data.fechaFin == null ? "" : data.fechaFin));
                },
                dom: 'Bfrtip',
                buttons: parametrosImpresion("Consulta Plantilla Personal " + $("#cboCC option:selected").text(), "<center><h3>Consulta Plantilla Personal " + $("#cboCC option:selected").text() + " </h3></center>"),
                buttons: [
                    {
                        extend: 'excel'
                    },
                ]
            });

        }
        function fnReporte() {
            if (cboCC.val() == undefined) {
                AlertaGeneral("Alerta", "Debe seleccionar un CC");
            }
            else {
                $.blockUI({ message: "Cargando información..." });

                var idReporte = "11";

                var path = "/Reportes/Vista.aspx?idReporte=108&cc=" + cboCC.val() + "&ccNombre=" + $("#cboCC option:selected").text();

                $("#report").attr("src", path);

                document.getElementById('report').onload = function () {

                    $.unblockUI();
                    openCRModal();

                };
            }
        }
        function fnCargar() {
            tblData.ajax.reload(null, false);
        }

        init();
    };
    $(document).ready(function () {
        sigoplan.rh.plantillapersonal = new plantillapersonal();
    });
})();
