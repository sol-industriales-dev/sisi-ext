(function () {

    $.namespace('maquinaria.captura.diaria.ReporteHorometro');

    ReporteHorometro = function () {
        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');
        btnBuscar = $("#btnBuscar");
        btnImprimir = $("#btnImprimir");
        tblHorometros = $('#tblHorometros');

        const hoy = new Date();

        function init() {
            initCboFiltros();
            initTableHorometros();
            btnBuscar.click(buscar);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
        }

        function initCboFiltros() {
            $("#multiCC").fillCombo('/CatObra/cboCentroCostosUsuarios', { est: true }, false, "Todos");
            convertToMultiselect('#multiCC');
        }

        function buscar() {
            const fechaSalida = moment(dpFechaInicio.val(), "DD-MM-YYYY").format();
            const fechaEntrega = moment(dpFechaFin.val(), "DD-MM-YYYY").format();
            let cc = getValoresMultiples('#multiCC');

            $.blockUI({ message: "Preparando información" });
            $.post("/Horometros/GetReporteHorometro", { cc: cc, fechaInicio: fechaSalida, fechaFin: fechaEntrega }, function (data) {
                if (data.listaHorometros.length > 0) {
                    tblHorometros.clear();
                    tblHorometros.rows.add(data.listaHorometros);
                    tblHorometros.draw();
                    btnImprimir.attr('disabled', false);
                } else {
                    tblHorometros.clear();
                    tblHorometros.draw();
                    btnImprimir.attr('disabled', true);
                }

            }).done(response => {
                $.unblockUI();
            });
        }

        btnImprimir.click(function (e) {
            $.blockUI({ message: "Preparando archivo a descargar" });
            $(this).download = '/Horometros/Download';
            $(this).href = '/Horometros/Download';
            location.href = '/Horometros/Download';
            $.unblockUI();
        });

        function initTableHorometros() {
            tblHorometros = $("#tblHorometros").DataTable({
                retrieve: true,
                paging: true,
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                initComplete: function (settings, json) {
                },
                columns: [
                    { data: 'fechaInicio', title: 'Fecha Inicio' },
                    { data: 'fechaFin', title: 'Fecha Fin' },
                    { data: 'noEconomico', title: 'Económico' },
                    { data: 'modelo', title: 'Modelo' },
                    { data: 'horometroInicial', title: 'Horometro Inicial' },
                    { data: 'horometroFinal', title: 'Horometro Final' },
                    { data: 'efectivo', title: 'Efectivo' },
                    { data: 'nombreObra', title: 'Descripción' }
                ],
                columnDefs: [
                ],
            });
        }

        init();

    };

    $(document).ready(function () {
        maquinaria.captura.diaria.ReporteHorometro = new ReporteHorometro();
    });
})();