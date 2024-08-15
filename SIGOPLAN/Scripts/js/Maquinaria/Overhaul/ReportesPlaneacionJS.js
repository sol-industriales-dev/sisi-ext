(function () {

    $.namespace('maquinaria.overhaul.reportesplaneacion');

    reportesplaneacion = function () {
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        tabCalendario = $("#tabCalendario"),
        tabDisponibilidad = $("#tabDisponibilidad"),
        tabPrecision = $("#tabPrecision"),
        //Reporte Inversion
        cboCalendarioInversion = $("#cboCalendarioInversion"),
        cboAnio = $("#cboAnio"),
        btnBuscarInversion = $("#btnBuscarInversion"),
        btnReporteInversion = $("#btnReporteInversion"),
        tblInversion = $("#tblInversion"),
        reporteInversion = $("#reporteInversion"),
        ireporteInversion = $("#reporteInversion > #reportViewerModal > #report");
        //Reporte Calendario
        cboCCCalendario = $("#cboCCCalendario"),
        cboAnioCalendario = $("#cboAnioCalendario"),
        btnBuscarCalendario = $("#btnBuscarCalendario"),
        btnReporteCalendario = $("#btnReporteCalendario"),
        tblCalendario = $("#tblCalendario"),
        reporteCalendario = $("#reporteCalendario"),
        ireporteCalendario = $("#reporteCalendario > #reportViewerModal > #report");
        //Reporte Disponibilidad
        cboCCKpi = $("#cboCCKpi"),
        cboAnioKpi = $("#cboAnioKpi"),
        btnBuscarKpi = $("#btnBuscarKpi"),
        btnReporteKpi = $("#btnReporteKpi"),
        tblKpi = $("#tblKpi"),
        reporteDisponibilidad = $("#reporteDisponibilidad"),
        ireporteDisponibilidad = $("#reporteDisponibilidad > #reportViewerModal > #report");
        //Reporte Precisión
        txtFechaInicio = $("#txtFechaInicio"),
        txtFechaFin = $("#txtFechaFin"),
        cboTipo = $("#cboTipo"),
        btnBuscarPrecision = $("#btnBuscarPrecision"),
        btnReportePrecision = $("#btnReportePrecision"),
        tblPrecision = $("#tblPrecision"),
        reportePrecision = $("#reportePrecision"),
        ireportePrecision = $("#reportePrecision > #reportViewerModal > #report");

        let dttblInversion;
        let dtPrecision;
        
        function init() {
            //Reporte Inversion
            cboCalendarioInversion.fillCombo("/Overhaul/CargarCalendarios", { anio: cboAnio.val() });
            initTblInversion();
            btnBuscarInversion.click(cargarTblInversion);
            cboAnio.change(cargarCalendarios);
            $("#reporteCalendario > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteCalendario > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteCalendario > #reportViewerModal").css("width", "0%");
                $("#reporteCalendario > #reportViewerModal").css("height", "0%");
            });
            //Reporte Calendario
            cboCCCalendario.fillCombo('/Overhaul/FillCboObraMaquina');
            cboAnioCalendario.fillCombo('/Overhaul/fillCboAnioPresupuesto');
            initTblCalendario();
            cargarTblCalendario();
            btnBuscarCalendario.click(cargarTblCalendario);
            btnReporteInversion.click(GetReporteInversion);
            $("#reporteCalendario > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteCalendario > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteCalendario > #reportViewerModal").css("width", "0%");
                $("#reporteCalendario > #reportViewerModal").css("height", "0%");
            });
            btnReporteCalendario.click(GetReporteCalendario);
            //Reporte Disponibilidad
            cboCCKpi.fillCombo('/Overhaul/FillCboObraMaquina');
            cboAnioKpi.fillCombo('/Overhaul/fillCboAnioPresupuesto');
            initTblKpi();
            $("#reporteDisponibilidad > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteDisponibilidad > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteDisponibilidad > #reportViewerModal").css("width", "0%");
                $("#reporteDisponibilidad > #reportViewerModal").css("height", "0%");
            });
            //cargarTblKpi();
            btnBuscarKpi.click(cargarTblKpi);
            btnReporteKpi.click(GetReporteDisponibilidad);
            //Reporte Precisión
            // IniciarTblPrecision();
            initDataTable_Precision();
         
            txtFechaInicio.datepicker().datepicker("setDate", new Date());
            txtFechaFin.datepicker().datepicker("setDate", new Date());
            $("#reportePrecision > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reportePrecision > #reportViewerModal > body").css("overflow", "auto");
                $("#reportePrecision > #reportViewerModal").css("width", "0%");
                $("#reportePrecision > #reportViewerModal").css("height", "0%");
            });
            btnReportePrecision.click(GetReportePrecisión);
            btnBuscarPrecision.click(fncCargarTablaPrecision);
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
                    { data: 'tipoParo', title: 'Tipo Paro' },
                    { data: 'numTipoParo', title: 'Tipo Paro' },
                    { data: 'paroID', title: 'Tipo Paro' },
                ],
                columnDefs: [
                    { className: "dt-center", targets: "_all" },
                    { targets: [0, 1, 9, 10], visible: false },
                    { orderable: false, targets: "_all" }
                ],
                order: [[1, 'asc'], [10, 'asc'], [2, 'asc']],
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
                            $('td', row).eq(6).css('background-color', 'rgb(92, 184, 92)');
                            $('td', row).eq(6).css('color', 'white');
                            break;
                        case 1:
                            $('td', row).eq(6).css('background-color', 'rgb(32, 77, 116)');
                            $('td', row).eq(6).css('color', 'white');
                            break;
                        case 2:
                            $('td', row).eq(6).css('background-color', '#ff8c1a');
                            $('td', row).eq(6).css('color', 'white');
                            break;
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
                        $('td', row).eq(1).css('color', 'white');
                        $('td', row).eq(2).css('color', 'white');
                        $('td', row).eq(3).css('color', 'white');
                        $('td', row).eq(4).css('color', 'white');
                        $('td', row).eq(5).css('color', 'white');
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
                var dimension_td_tipo = $(this).find('td:nth-child(7)');
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
            $.blockUI({ message: mensajes.PROCESANDO });

            $.ajax({
                url: "/Overhaul/cargarTblInversion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    calendarioID: cboCalendarioInversion.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    dttblInversion.clear();
                    dttblInversion.rows.add(response.detalles);
                    dttblInversion.draw();
                    MergeGridCells();
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
                url: "/Overhaul/GetReporteCalendarioInversion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    calendarioID: cboCalendarioInversion.val()
                }),
                success: function (response) {
                    ireporteInversion.attr("src", "/Reportes/Vista.aspx?idReporte=186");
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

        function initTblCalendario()
        {
            tblCalendario.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {

                    "calendario": function (column, row) {
                        return "<span class=\"CCName\"> " + row.nombre.replace(/\-/g, ' ') + " </span>";
                    },
                    "estatus": function (column, row) {
                        return "<span class=\"CCName\"> " + (row.estatus == 0 ? "ACTIVO" : "TERMINADO") + " </span>";
                    },
                    "reporte": function (column, row) {
                        return "<button type='button' class='btn btn-primary detalle' data-index='" +
                            row.id + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span></button>";
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblCalendario.find(".detalle").parent().css("text-align", "center");
                tblCalendario.find(".detalle").parent().css("width", "3%");
                tblCalendario.find(".detalle").on("click", function (e) {

                });
            });
        }

        function cargarTblCalendario()
        {
            $.blockUI({ message: mensajes.PROCESANDO } );

            $.ajax({
                url: "/Overhaul/cargarTblCalendarioEjec",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    obra: cboCCCalendario.val(),
                    anio: cboAnioCalendario.val()
                }),
                success: function (response) {
                    $.unblockUI();

                    tblCalendario.bootgrid("clear");
                    tblCalendario.bootgrid("append", response.calendarios);
                    tblCalendario.bootgrid('reload');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function cargarTblKpi() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Overhaul/CargarTblDispOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    obra: cboCCKpi.val(),
                    anio: cboAnioKpi.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    tblKpi.bootgrid("clear");
                    tblKpi.bootgrid("append", response.overhauls);
                    tblKpi.bootgrid('reload');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetReporteDisponibilidad()
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Overhaul/GetReporteDisponibilidad",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    obra: cboCCKpi.val(),
                    anio: cboAnioKpi.val()
                }),
                success: function (response) {
                    ireporteDisponibilidad.attr("src", "/Reportes/Vista.aspx?idReporte=159&obra=" + cboCCKpi.val() + "&anio=" + cboAnioKpi.val());
                    $(window).scrollTop(0);
                    $("#reporteDisponibilidad > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteDisponibilidad > #reportViewerModal").css("width", "100%");
                    $("#reporteDisponibilidad > #reportViewerModal").css("height", "105%");
                    $("#reporteDisponibilidad > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function initTblKpi() {
            tblKpi.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
            });
        }
        
        function GetReporteCalendario() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Overhaul/GetReporteCalenEjecOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    obra: cboCCCalendario.val(),
                    anio: cboAnioCalendario.val()
                }),
                success: function (response) {
                    ireporteCalendario.attr("src", "/Reportes/Vista.aspx?idReporte=160&obra=" + cboCCCalendario.val() + "&anio=" + cboAnioCalendario.val());
                    $(window).scrollTop(0);
                    $("#reporteCalendario > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteCalendario > #reportViewerModal").css("width", "100%");
                    $("#reporteCalendario > #reportViewerModal").css("height", "105%");
                    $("#reporteCalendario > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GetReportePrecisión() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/Overhaul/GetReportePrecisionOverhaul",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    fechaInicio: txtFechaInicio.val(),
                    fechaFin: txtFechaFin.val(),
                    tipo: cboTipo.val()
                }),
                success: function (response) {
                    ireportePrecision.attr("src", "/Reportes/Vista.aspx?idReporte=161");
                    $(window).scrollTop(0);
                    $("#reportePrecision > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reportePrecision > #reportViewerModal").css("width", "100%");
                    $("#reportePrecision > #reportViewerModal").css("height", "105%");
                    $("#reportePrecision > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        // function IniciarTblPrecision()
        // {
        //     tblPrecision.bootgrid({
        //         headerCssClass: '.bg-table-header',
        //         align: 'center',
        //         rowCount: -1,
        //         templates: {
        //             header: ""
        //         },
        //         sorting: false,
        //     });
        // }
 
        // function cargarTblPrecision() {
        //     $.blockUI({ message: mensajes.PROCESANDO });
        //     $.ajax({
        //         url: "/Overhaul/CargarTblPrecOH",
        //         type: 'POST',
        //         dataType: 'json',
        //         contentType: 'application/json',
        //         //async: false,
        //         data: JSON.stringify({
        //             fechaInicio: txtFechaInicio.val(),
        //             fechaFin: txtFechaFin.val()
        //         }),
        //         success: function (response) {
        //             $.unblockUI();
        //             console.log(response.data)
        //             tblPrecision.bootgrid("clear");
        //             tblPrecision.bootgrid("append", response.data);
        //             tblPrecision.bootgrid('reload');
        //         },
        //         error: function (response) {
        //             $.unblockUI();
        //             AlertaGeneral("Alerta", response.message);
        //         }
        //     });
        // }

        var fncCargarTablaPrecision = function () {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/Overhaul/CargarTblPrecOH",
                contentType: 'application/json',
                data: JSON.stringify({
                                fechaInicio: txtFechaInicio.val(),
                                fechaFin: txtFechaFin.val(),
                                tipo: cboTipo.val()
                            }),
                success: function (response) {
                    if (response.success) {
                        console.log(response.data)
                        dtPrecision.clear();
                        dtPrecision.rows.add(response.data);
                        dtPrecision.draw();
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }

        var initDataTable_Precision = function () {
            dtPrecision = tblPrecision.DataTable({
                language: dtDicEsp,
                ordering: true,
                paging: true,
                searching: true,
                bFilter: true,
                info: false,
                columns: [
                    { data: 'economico', title: 'Económico' },
                    { data: 'tipo', title: 'Tipo Overhaul' },
                    { data: 'lstComponentes', title: 'Componentes',render: (data, type, row) => {
                            let html='';
                           for (let index = 0; index < data.length; index++) {
                               html += '<span style="text-align:center;">'+data[index]+'</span><br>'
                           }
                        return html;
                        }
                    },
                    { data: 'HorasCiclo', title: 'Horas Ciclo',render: (data, type, row) => {
                            let html='';
                           for (let index = 0; index < data.length; index++) {
                               html += '<span style="text-align:center;">'+data[index]+'</span><br>'
                           }
                        return html;
                        }
                    },
                    { data: 'diasDG', title: 'Dias DG' },
                    { data: 'diasReales', title: 'Dias Reales' },
                    { data: 'precicion', title: 'Precision' ,render: (data, type, row) => {
                            let html='';
                                html=  data == ""|| data =="N/A" ? "N/A" : parseFloat(data).toFixed(2) + ' %'
                            return html;
                        }
                    },
                    // {
                    //     data: 'lstCC', title: 'Centro Costo', render: (data, type, row) => {
    
                    //         let html = "";
                    //         for (let i = 0; i < row.lstCC.length; i++) {
                    //             html += "<span class='btn btn-primary displayCC'><i class='fab fa-creative-commons-nd'>" + row.lstCC[i].cc + "</i></span>";
                    //         }
                    //         return html;
                    //     }
                    // },
                    // {
                    //     title: "Estatus",
                    //     render: function (data, type, row) {
                    //         let activo;
                    //         row.esActivo ? activo = "Activo" : activo = "Desactivado";
                    //         return activo;
                    //     }, visible: false
                    // },
                    // {
                    //     render: function (data, type, row) {
                    //         let btnEliminar = "";
                    //         row.esActivo ?
                    //             btnEliminar = `<button class='btn-eliminar btn btn-success eliminarAgrupacion' data-esActivo="1" data-id="${row.id}">` +
                    //             `<i class="fas fa-toggle-on"></i></button>` :
                    //             btnEliminar = `<button class='btn-eliminar btn btn-danger eliminarAgrupacion' data-esActivo="0" data-id="${row.id}">` +
                    //             `<i class="fas fa-trash"></i></button>`;
    
                    //         return `<button class='btn-editar btn btn-warning editarAgrupacion' data-id="${row.id}">` +
                    //             `<i class='fas fa-pencil-alt'></i>` +
                    //             `</button>&nbsp;` + btnEliminar;
                    //     }
                    // }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { "width": "10%", "targets": [0, 2, 3] }
                ],
                rowCallback: function (row, data, index, full) {
                    switch (data.tipo) {
                        case "FALLO":
                            $($(row).find("td")[1]).css("background-color","#E60026");
                            $($(row).find("td")[1]).css("color","white");
                            break;
                        case "OVERHAUL GENERAL":
                            $($(row).find("td")[1]).css("background-color","#00b050");
                            $($(row).find("td")[1]).css("color","white");
                            break;
                        case "CAMBIO DE MOTOR":
                            $($(row).find("td")[1]).css("background-color","#0000ff");
                            $($(row).find("td")[1]).css("color","white");
                            break;
                        case "COMPONENTES DESFASADOS":
                            $($(row).find("td")[1]).css("background-color","#ffc000");
                            $($(row).find("td")[1]).css("color","white");
                        default:
                            break;
                    }
                    
                          
                    
                },
                initComplete: function (settings, json) {
                  
                }
            });
        }
      
        function cargarCalendarios()
        {
            cboCalendarioInversion.fillCombo("/Overhaul/CargarCalendarios", { anio: cboAnio.val() });
        }

        init();
    };


        



    $(document).ready(function () {
        maquinaria.overhaul.reportesplaneacion = new reportesplaneacion();
    }).ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });
})();