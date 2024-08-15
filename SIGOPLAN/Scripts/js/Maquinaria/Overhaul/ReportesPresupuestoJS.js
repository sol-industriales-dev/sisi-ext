(function () {

    $.namespace('maquinaria.overhaul.reportespresupuesto');

    reportespresupuesto = function () {
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        //Reporte Inversion
        //cboCalendarioInversion = $("#cboCalendarioInversion"),
        cboModeloInversion = $("#cboModeloInversion"), 
        cboObraInversion = $("#cboObraInversion"),
        cboAnioInversion = $("#cboAnioInversion"),
        btnBuscarInversion = $("#btnBuscarInversion"),
        btnReporteInversion = $("#btnReporteInversion"),
        tblInversion = $("#tblInversion"),
        reporteInversion = $("#reporteInversion"),
        ireporteInversion = $("#reporteInversion > #reportViewerModal > #report");

        const year = $('#year');
        const pAutorizado = $('#pAutorizado');
        const pProgramado = $('#pProgramado');
        const eProgramado = $('#eProgramado');
        const eNoProgramado = $('#eNoProgramado');
        const pTotal = $('#pTotal');
        const eTotal = $('#eTotal');
        const bolsa = $('#bolsa');

        let dttblInversion;
        
        function init() {

            //Reporte Inversion
            //cboCalendarioInversion.fillCombo('/Overhaul/CargarCalendarios');
            cboModeloInversion.select2();
            cboModeloInversion.fillCombo('/Overhaul/CargarModelosRptInversion', {}, true);
            cboObraInversion.select2();
            cboObraInversion.fillCombo('/Overhaul/CargarObrasRptInversion', {}, true);
            cboAnioInversion.select2();
            cboAnioInversion.fillCombo('/Overhaul/CargarAnioRptInversion', {}, true);
            initTblInversion();
            btnBuscarInversion.click(cargarTblInversion);
            btnReporteInversion.click(GetReporteInversion);
            $("#reporteCalendario > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteCalendario > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteCalendario > #reportViewerModal").css("width", "0%");
                $("#reporteCalendario > #reportViewerModal").css("height", "0%");
            });
        }

        function initTblInversion() {
            var labelsEspeciales = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
            dttblInversion = tblInversion.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: true,
                autoWidth: true,
                fixedHeader: {
                    header: true,
                    footer: true
                },
                rowGroup: {
                    startRender: function (rows, group) {
                        if (labelsEspeciales.indexOf(group) > -1) {
                            return $('<span style="font-weight:bold;background-color:' + (group == "Overhaul General" ? 'green;color:white' :
                                (group == "Cambio de Motor" ? 'blue;color:white' : (group == "Componentes Desfasados" ? 'orange;color:white' :
                                (group == "Falla" ? 'red;color:white' : '#f3f3f3')))) + '">' + group + '</span>');
                        }
                        else { return " "; }                            
                    },
                    dataSrc: ["mes"]
                },
                columns: [
                    { data: 'mes', title: 'Mes' },
                    { data: 'numMes', title: 'Mes' },
                    { data: 'equipo', title: 'Equipo' },
                    { data: 'componente', title: 'Componente' },
                    { data: 'subconjunto', title: 'Subconjunto' },
                    { data: 'horasComponente', title: 'Horas Componente' },
                    { data: 'target', title: 'Target' },
                    { data: 'proximoPCR', title: 'proximoPCR' },
                    {
                        data: 'presupuesto',
                        title: 'Presupuesto',
                        render: function (data, type, row) {
                            return '<p>$ ' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        }
                    },
                    {
                        data: 'erogado',
                        title: 'Erogado',
                        render: function (data, type, row) {
                            return '<p>$ ' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        }
                    },
                    { data: 'tipoParo', title: 'Tipo Paro' },
                    { data: 'numTipoParo', title: 'Tipo Paro' },
                    { data: 'paroID', title: 'Tipo Paro' },
                ],
                columnDefs: [
                    { className: "dt-center", targets: "_all" },
                    { targets: [0, 1, 11, 12], visible: false },
                    { orderable: false, targets: "_all" }
                ],
                order: [[1, 'asc'], [12, 'asc'], [2, 'asc']],
                drawCallback: function () {
                    tblInversion.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();

                    });
                },
                rowCallback: function (row, data, index, full) {
                    switch (data.numTipoParo)
                    {
                        case 0:
                            $('td', row).eq(8).css('background-color', 'rgb(92, 184, 92)');
                            $('td', row).eq(8).css('color', 'white');
                            break;
                        case 1:
                            $('td', row).eq(8).css('background-color', 'rgb(32, 77, 116)');
                            $('td', row).eq(8).css('color', 'white');
                            break;
                        case 2:
                            $('td', row).eq(8).css('background-color', '#ff8c1a');
                            $('td', row).eq(8).css('color', 'white');
                            break;
                        case 3:
                            $('td', row).eq(8).css('background-color', '#e42c2c');
                            $('td', row).eq(8).css('color', 'white');
                        default:
                            break;
                    }
                    if (data.paroTerminado)
                    {
                        $('td', row).eq(0).css('background-color', '#696969');
                        $('td', row).eq(0).css('color', 'white');
                        //$('td', row).eq(6).css('background-color', '#696969');
                        //$('td', row).eq(6).css('color', 'white');
                    }
                    if (data.fechaRemocion != '--')
                    {
                        $('td', row).eq(1).css('background-color', '#696969');
                        $('td', row).eq(2).css('background-color', '#696969');
                        $('td', row).eq(3).css('background-color', '#696969');
                        $('td', row).eq(4).css('background-color', '#696969');
                        $('td', row).eq(5).css('background-color', '#696969');
                        $('td', row).eq(6).css('background-color', '#696969');
                        $('td', row).eq(7).css('background-color', '#696969');
                        $('td', row).eq(1).css('color', 'white');
                        $('td', row).eq(2).css('color', 'white');
                        $('td', row).eq(3).css('color', 'white');
                        $('td', row).eq(4).css('color', 'white');
                        $('td', row).eq(5).css('color', 'white');
                        $('td', row).eq(6).css('color', 'white');
                        $('td', row).eq(7).css('color', 'white');
                    }
                },
            });
            
        }

        function MergeGridCells() {
            var dimension_cells = new Array();
            var dimension_col = 1;
            var columnCount = $("#tblInversion tr:first th").length;
            // first_instance holds the first instance of identical td
            var first_instance = null;
            var first_instance_tipo = null;
            var rowspan = 1;
            var rowspan_tipo = 1;
            // iterate through rows
            $("#tblInversion").find('tr').each(function () {
                // find the td of the correct column (determined by the dimension_col set above)
                var dimension_td = $(this).find('td:nth-child(1)');
                var dimension_td_tipo = $(this).find('td:nth-child(9)');
                if (first_instance == null) {
                    // must be the first row
                    first_instance = dimension_td;
                    first_instance_tipo = dimension_td_tipo;
                }
                else {
                    if (dimension_td.text() == first_instance.text()) {                        
                        // the current td is identical to the previous
                        // remove the current td
                        dimension_td.remove();
                        ++rowspan;
                        // increment the rowspan attribute of the first instance
                        first_instance.attr('rowspan', rowspan);                        
                        if (dimension_td_tipo.text() == first_instance_tipo.text()) {
                            // the current td is identical to the previous
                            // remove the current td
                            dimension_td_tipo.remove();
                            ++rowspan_tipo;
                            // increment the rowspan attribute of the first instance
                            first_instance_tipo.attr('rowspan', rowspan_tipo);
                        }
                        else {
                            // this cell is different from the last
                            first_instance_tipo = dimension_td_tipo;
                            rowspan_tipo = 1;
                        }                        
                    }
                    else {
                        $(this).find('td').css("border-top-width", "thick");
                        // this cell is different from the last
                        first_instance = dimension_td;
                        rowspan = 1;
                        first_instance_tipo = dimension_td_tipo;
                        rowspan_tipo = 1;
                    }
                }
            });
        }

        function cargarTblInversion() {
            console.log()
            $.blockUI({ message: mensajes.PROCESANDO });

            $.ajax({
                url: "/Overhaul/cargarTblInversionFiltrada",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    obras: cboObraInversion.val(), 
                    modelos: cboModeloInversion.val(),
                    anio: cboAnioInversion.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    dttblInversion.clear();
                    dttblInversion.rows.add(response.detalles);
                    dttblInversion.draw();
                    MergeGridCells();

                    pAutorizado.text(maskNumero(response.pAutorizado));
                    pProgramado.text(maskNumero(response.pProgramado));
                    eProgramado.text(maskNumero(response.eProgramado));
                    eNoProgramado.text(maskNumero(response.eNoProgramado));
                    pTotal.text(maskNumero(response.pTotal));
                    eTotal.text(maskNumero(response.eTotal));
                    bolsa.text(maskNumero(response.bolsa));
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetReporteInversion() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Overhaul/GetReporteInversion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    obras: cboObraInversion.val(),
                    modelos: cboModeloInversion.val(),
                    anio: cboAnioInversion.val()
                }),
                success: function (response) {
                    ireporteInversion.attr("src", "/Reportes/Vista.aspx?idReporte=185");
                    $(window).scrollTop(0);
                    $("#reporteInversion > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteInversion > #reportViewerModal").css("width", "100%");
                    $("#reporteInversion > #reportViewerModal").css("height", "105%");
                    $("#reporteInversion > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        
        init();
    };
    $(document).ready(function () {
        maquinaria.overhaul.reportespresupuesto = new reportespresupuesto();
    });
})();