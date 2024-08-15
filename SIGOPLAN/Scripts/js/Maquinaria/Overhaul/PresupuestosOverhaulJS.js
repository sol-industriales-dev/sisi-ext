(function () {

    $.namespace('maquinaria.overhaul.presupuesto');

    presupuesto = function () {
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        //Avance
        tabAvance = $("#tabAvance"),
        cboModeloAvance = $("#cboModeloAvance"),
        tblAvance = $("#tblAvance"),
        cboAnioAvance = $("#cboAnioAvance"),
        cboCCAvance = $("#cboCCAvance"),
        cboEstatusAvance = $("#cboEstatusAvance"),
        cboConjuntoAvance = $("#cboConjuntoAvance"),
        cboSubconjuntoAvance = $("#cboSubconjuntoAvance"),
        cboProgramadoAvance = $("#cboProgramadoAvance");
        tblAvanceDetalle = $('#tblAvanceDetalle');
        btnBuscar = $('#btnBuscar');

        /* Nuevo Presupuesto*/
        const cboSubconjunto = $("#cboSubconjunto");
        const txtComponenteAvance = $("#txtComponenteAvance");

        //let obrasAvance = [];
        //let presupuestoID = 0;
        //let cerrado = false;
        //let respuesta = [];
        //let tipoUsuario = 6;
        //let dtAvance;
        //let dtAvanceDetalle;
        //let dtAvanceGeneral;

        function init() {
            //PermisosBotones();

            //initTblInversion();
            //cboObraInversion.fillCombo('/Overhaul/FillCboObraMaquina', {});

            //cboObraInversion.change(function (e) {
            //    e.preventDefault();
            //    e.stopPropagation();
            //    e.stopImmediatePropagation();
            //    cargarTblInversion();
            //});

            //tabPresupuesto.click(IniciartblPresupuesto);
            //btnGuardarAumento.click(GuardarAumento);
            //IniciarTblArchivosHistorial();            
            //IniciartblAvanceGeneral();
            ////IniciartblAvanceGeneralAtrasado();
            IniciartblAvance();
            //IniciartblAvanceDetalle();
            //Autorizacion            


            //Avance

            cboModeloAvance.fillCombo('/CatComponentes/FillCboModelo_Componente');
            cboAnioAvance.fillCombo('/Overhaul/fillCboAnioPresupuesto');
            cboAnioAvance.find('option').get(0).remove();
            //cboCCAvance.fillCombo('/Overhaul/FillCboObraMaquinaAC', {}, true);
            cboConjuntoAvance.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: -1 });
            cboSubconjunto.fillCombo('/Overhaul/FillCboSubconjuntos', { conjunto: -1 });
            
            //cboConjuntoAvance.change(CboSubConjuntoCargar);
            //convertToMultiselectSelectAll(cboCCAvance);
            //cboCCAvance.multiselect('selectAll', false).multiselect('updateButtonText');
            //IniciartblAvance();
            //cboModeloAvance.change(CargartblAvance);
            //cboAnioAvance.change(function (){
            //    CargarAvanceGeneral();
            //    CargartblAvance();
            //});
            
            //cboCCAvance.change(CargartblAvance);
            //tabAvance.click(function (){
            //    CargarAvanceGeneral();
            //    CargartblAvance();
            //});
            btnBuscar.click(buscarPresupuesto);
            cboSubconjunto.select2();
            cboModeloAvance.select2();
        }

        //function PermisosBotones() {
        //    $.blockUI({
        //        message: mensajes.PROCESANDO,
        //        baseZ: 2000
        //    });
        //    $.ajax({
        //        url: "/Overhaul/PermisosBotonesAdminComp",
        //        type: 'POST',
        //        dataType: 'json',
        //        contentType: 'application/json',
        //        //async: false,
        //        success: function (response) {
        //            $.unblockUI();
        //            tipoUsuario = response.tipoUsuario;
        //            if (response.tipoUsuario == 1 || response.tipoUsuario == 2 || response.tipoUsuario == 7) {
        //                tabPresupuesto.css("display", "block");
        //            }
        //            else { tabPresupuesto.css("display", "none"); }
        //        },
        //        error: function (response) {
        //            $.unblockUI();
        //            AlertaGeneral("Alerta", response.message);
        //        }
        //    });
        //}

        //function CboSubConjuntoCargar()
        //{
        //    if (cboConjuntoAvance.val() != null && cboConjuntoAvance.val() != "") {
        //        cboSubconjuntoAvance.fillCombo('/CatComponentes/FillCboSubConjunto_Componente', { idConjunto: cboConjuntoAvance.val(), idModelo: -1 });
        //        cboSubconjuntoAvance.attr('disabled', false);
        //    }
        //    else {
        //        cboSubconjuntoAvance.clearCombo();
        //        cboSubconjuntoAvance.attr('disabled', true);
        //    }
        //}



        //Avance
        //function AutorizarPresupuesto() {
        //    var tipoActual = btnModalAceptar.attr("data-tipo");
        //    $.blockUI({ message: "Procesando..." });
        //    $.ajax({
        //        url: '/Overhaul/AutorizarPresupuesto',
        //        datatype: "json",
        //        type: "POST",
        //        data: {
        //            presupuestoID: btnModalAceptar.attr("data-index"),
        //            obra: btnModalAceptar.attr("data-obra"),
        //            anio: btnModalAceptar.attr("data-anio"),
        //            modelo: btnModalAceptar.attr("data-modelo"),
        //            tipo: btnModalAceptar.attr("data-tipo")
        //        },
        //        success: function (response) {
        //            $.unblockUI();
        //            if (response.success) {
        //                if (response.exito)
        //                {
        //                    modalAutorizacion.modal("hide");
        //                    AlertaGeneral("Éxito", "Se ha realizado la operación con éxito");
        //                    CargartblAutorizacion();
        //                }
        //            }
        //        },
        //        error: function (response) {
        //            console.log(response);
        //            $.unblockUI();
        //            AlertaGeneral("Alerta", response.MESSAGE);
        //        }
        //    });
        //}

        function IniciartblAvance() {
            dtAvance = tblAvance.DataTable({
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '35vh',
                scrollCollapse: true,
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'noComponente', title: 'Componente' },
                    { data: 'modeloID', title: 'modeloID', visible: false },
                    { data: 'modelo', title: 'Modelo' },
                    { data: 'subconjuntoID', title: 'subconjuntoID', visible: false },
                    { data: 'subconjunto', title: 'Subconjunto' },
                    { data: 'presupuestoInicial', title: 'Presupuesto' },
                    { data: 'erogado', title: 'Erogado' },
                    { data: 'presupuestado', title: 'presupuestado', visible: false },
                ],
            }).columns.adjust();
        }

        //function cargarDetallesAvance(presupuestoID, obraID)
        //{
        //    $.blockUI({ message: "Procesando..." });
        //    $.ajax({
        //        url: '/Overhaul/CargarTblModalAutorizacion',
        //        datatype: "json",
        //        async: false,
        //        type: "POST",
        //        data: {
        //            presupuestoID: presupuestoID,
        //            obra: obraID,
        //            subconjunto: cboSubconjuntoAvance.val()
        //        },
        //        success: function (response) {
        //            $.unblockUI();
        //            if (response.success) {
        //                respuesta = response.data;
        //                tblAvance.columns.adjust();
        //            }
        //        }
        //    });            
        //}

        function format(d) {
            let html = "";
            let total = 0;
            let totalReal = 0;

            for (let i = 0; i < d.length; i++)
            {
                //total += d[i].costo;
                //totalReal += d[i].costoReal;
                html +=
                    '<tr class="childRow">' +
                        '<td>' + d[i].noEconomico + '</td>' +
                        '<td>' + d[i].noComponente + '</td>' +
                        '<td>' + d[i].subconjunto + '</td>' +
                        '<td>' + d[i].target + '</td>' +
                        '<td>' + d[i].horasCiclo + '</td>' +
                        '<td>' + d[i].programado + '</td>' +
                        '<td>' + '$' + parseFloat(d[i].costo, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString() + '</td>' +
                        '<td>' + '$' + parseFloat(d[i].costoReal, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString() + '</td>' +
                    '</tr>';                
            }
            return '<div class="slider">' +
                '<table id="tablaAvanceDetalle" class="table table-condensed table-hover table-striped text-center bootgrid-table tablaAvanceDet">' +
                    '<thead class="bg-table-header">' +
                        '<tr class="childRow">' +
                        '<th>Económico</th>' + 
                        '<th>Serie</th>' +
                        '<th>Subconjunto</th>' +
                        '<th>Target</th>' +
                        '<th>Hrs. comp.</th>' +
                        '<th>Programado</th>' +
                        '<th>Costo Presupuesto</th>' +
                        '<th>Costo Real</th>' +
                    '</thead>' +
                    '<tbody>' +
                    html +
                    '</tbody>' +
                    '<tfoot>' +
                        '<tr>' +
                            '<th colspan="6" style="text-align:right">Total:</th>' +
                            '<th></th>' +
                            '<th></th>' +
                        '</tr>' +
                    '</tfoot>' +
                '</table>' +
            '</div>';            
        }

        //function CargartblAvance() {
        //    $.blockUI({ message: "Procesando..." });
        //    $.ajax({
        //        url: '/Overhaul/CargarTblAvance',
        //        datatype: "json",
        //        type: "POST",
        //        data: {
        //            obras: cboCCAvance.val(),
        //            anio: cboAnioAvance.val(),
        //            modeloID: cboModeloAvance.val(),
        //            estatus: cboEstatusAvance.val()
        //        },
        //        success: function (response) {
        //            $.unblockUI();
        //            if (response.success) {
        //                dtAvance.clear();
        //                dtAvance.rows.add(response.data);
        //                dtAvance.draw(false);
        //                dtAvance.columns.adjust();
        //            }
        //        }
        //    });
        //}

        //Tabla Reporte Inversion

        //function initTblInversion() {
        //    var labelsEspeciales = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
        //    dttblInversion = tblInversion.DataTable({
        //        language: dtDicEsp,
        //        destroy: true,
        //        autoWidth: true,
        //        searching: false, paging: false, info: false,
        //        rowGroup: {
        //            startRender: function (rows, group) {
        //                if (labelsEspeciales.indexOf(group) > -1) {
        //                    return $('<span style="font-weight:bold;background-color:' + (group == "Overhaul General" ? 'green;color:white' :
        //                        (group == "Cambio de Motor" ? 'blue;color:white' : (group == "Componentes Desfasados" ? 'orange;color:white' :
        //                        (group == "Falla" ? 'red;color:white' : '#f3f3f3')))) + '">' + group + '</span>');
        //                }
        //                else { return " "; }
        //            },
        //            dataSrc: ["mes"]
        //        },
        //        columns: [
        //            { data: 'mes', title: 'Mes' },
        //            { data: 'numMes', title: 'Mes' },
        //            { data: 'equipo', title: 'Equipo' },
        //            { data: 'componente', title: 'Componente' },
        //            { data: 'subconjunto', title: 'Subconjunto' },
        //            { data: 'horasComponente', title: 'Horas Componente' },
        //            { data: 'target', title: 'Target' },
        //            { data: 'proximoPCR', title: 'proximoPCR' },
        //            {
        //                data: 'presupuesto',
        //                title: 'Presupuesto',
        //                render: function (data, type, row) {
        //                    return '<p>$ ' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
        //                }
        //            },
        //            {
        //                data: 'erogado',
        //                title: 'Erogado',
        //                render: function (data, type, row) {
        //                    return '<p>$ ' + parseFloat(data).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
        //                }
        //            },
        //            { data: 'tipoParo', title: 'Tipo Paro' },
        //            { data: 'numTipoParo', title: 'Tipo Paro' },
        //            { data: 'paroID', title: 'Tipo Paro' },
        //        ],
        //        columnDefs: [
        //            { className: "dt-center", targets: "_all" },
        //            { targets: [0, 1, 11, 12], visible: false },
        //            { orderable: false, targets: "_all" }
        //        ],
        //        order: [[1, 'asc'], [12, 'asc'], [2, 'asc']],
        //        drawCallback: function () {
        //            tblInversion.find('p.desplegable').click(function (e) {
        //                e.preventDefault();
        //                e.stopPropagation();
        //                e.stopImmediatePropagation();

        //            });
        //        },
        //        rowCallback: function (row, data, index, full) {
        //            switch (data.numTipoParo) {
        //                case 0:
        //                    $('td', row).eq(8).css('background-color', 'rgb(92, 184, 92)');
        //                    $('td', row).eq(8).css('color', 'white');
        //                    break;
        //                case 1:
        //                    $('td', row).eq(8).css('background-color', 'rgb(32, 77, 116)');
        //                    $('td', row).eq(8).css('color', 'white');
        //                    break;
        //                case 2:
        //                    $('td', row).eq(8).css('background-color', '#ff8c1a');
        //                    $('td', row).eq(8).css('color', 'white');
        //                    break;
        //                default:
        //                    break;
        //            }
        //            if (data.paroTerminado) {
        //                $('td', row).eq(0).css('background-color', '#696969');
        //                $('td', row).eq(0).css('color', 'white');
        //                //$('td', row).eq(6).css('background-color', '#696969');
        //                //$('td', row).eq(6).css('color', 'white');
        //            }
        //            if (data.fechaRemocion != '--') {
        //                $('td', row).eq(1).css('background-color', '#696969');
        //                $('td', row).eq(2).css('background-color', '#696969');
        //                $('td', row).eq(3).css('background-color', '#696969');
        //                $('td', row).eq(4).css('background-color', '#696969');
        //                $('td', row).eq(5).css('background-color', '#696969');
        //                $('td', row).eq(6).css('background-color', '#696969');
        //                $('td', row).eq(7).css('background-color', '#696969');
        //                $('td', row).eq(1).css('color', 'white');
        //                $('td', row).eq(2).css('color', 'white');
        //                $('td', row).eq(3).css('color', 'white');
        //                $('td', row).eq(4).css('color', 'white');
        //                $('td', row).eq(5).css('color', 'white');
        //                $('td', row).eq(6).css('color', 'white');
        //                $('td', row).eq(7).css('color', 'white');
        //            }
        //        },
        //        "footerCallback": function (row, data, start, end, display) {
        //            var api = this.api(), data;

        //            // Remove the formatting to get integer data for summation
        //            var intVal = function (i) {
        //                return typeof i === 'string' ?
        //                    i.replace(/[\$,]/g, '') * 1 :
        //                    typeof i === 'number' ?
        //                    i : 0;
        //            };

        //            // Total over all pages
        //            total = api
        //                .column(8)
        //                .data()
        //                .reduce(function (a, b) {
        //                    return intVal(a) + intVal(b);
        //                }, 0);


        //            // Update footer
        //            $(api.column(3).footer()).html(
        //                '$' + parseFloat(total).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' total'
        //            );
        //        }
        //    });

        //}

        //function MergeGridCells() {
        //    var dimension_cells = new Array();
        //    var dimension_col = 1;
        //    var columnCount = $("#tblInversion tr:first th").length;
        //    // first_instance holds the first instance of identical td
        //    var first_instance = null;
        //    var first_instance_tipo = null;
        //    var rowspan = 1;
        //    var rowspan_tipo = 1;
        //    // iterate through rows
        //    $("#tblInversion").find('tr').each(function () {
        //        // find the td of the correct column (determined by the dimension_col set above)
        //        var dimension_td = $(this).find('td:nth-child(1)');
        //        var dimension_td_tipo = $(this).find('td:nth-child(9)');
        //        if (first_instance == null) {
        //            // must be the first row
        //            first_instance = dimension_td;
        //            first_instance_tipo = dimension_td_tipo;
        //        }
        //        else {
        //            if (dimension_td.text() == first_instance.text()) {
        //                // the current td is identical to the previous
        //                // remove the current td
        //                dimension_td.remove();
        //                ++rowspan;
        //                // increment the rowspan attribute of the first instance
        //                first_instance.attr('rowspan', rowspan);
        //                if (dimension_td_tipo.text() == first_instance_tipo.text()) {
        //                    // the current td is identical to the previous
        //                    // remove the current td
        //                    dimension_td_tipo.remove();
        //                    ++rowspan_tipo;
        //                    // increment the rowspan attribute of the first instance
        //                    first_instance_tipo.attr('rowspan', rowspan_tipo);
        //                }
        //                else {
        //                    // this cell is different from the last
        //                    first_instance_tipo = dimension_td_tipo;
        //                    rowspan_tipo = 1;
        //                }
        //            }
        //            else {
        //                $(this).find('td').css("border-top-width", "thick");
        //                // this cell is different from the last
        //                first_instance = dimension_td;
        //                rowspan = 1;
        //                first_instance_tipo = dimension_td_tipo;
        //                rowspan_tipo = 1;
        //            }
        //        }
        //    });
        //}

        //function cargarTblInversion() {
        //    $.blockUI({ message: mensajes.PROCESANDO });

        //    $.ajax({
        //        url: "/Overhaul/cargarTblInversionObra",
        //        type: 'POST',
        //        dataType: 'json',
        //        contentType: 'application/json',
        //        //async: false,
        //        data: JSON.stringify({
        //            modelo: cboModeloPresupuesto.val(),
        //            obra: cboObraInversion.val(),
        //            anio: cboAnioPresupuesto.val()
        //        }),
        //        success: function (response) {
        //            $.unblockUI();
        //            dttblInversion.clear();
        //            dttblInversion.rows.add(response.detalles);
        //            dttblInversion.draw();
        //            MergeGridCells();
        //        },
        //        error: function (response) {
        //            $.unblockUI();
        //            AlertaGeneral("Alerta", response.message);
        //        }
        //    });
        //}

        //const CargarAvanceDetallle = function (idDetalle){
        //    $.blockUI({ message: "Procesando..." });
        //    $.ajax({
        //        url: '/Overhaul/CargarTblAvanceDetalle',
        //        datatype: "json",
        //        type: "POST",
        //        data: {
        //            idDetalle:idDetalle
        //        },
        //        success: function (response) {
        //            $.unblockUI();
        //            if (response.success) {
        //                dtAvanceDetalle.clear();
        //                dtAvanceDetalle.rows.add(response.data);
        //                dtAvanceDetalle.draw(false);
        //                dtAvanceDetalle.columns.adjust();
        //            }
        //        }
        //    });
        //}

        //const IniciartblAvanceDetalle = function () {
        //    dtAvanceDetalle = tblAvanceDetalle.DataTable({
        //        retrieve: true,
        //        searching: false,
        //        paging: false,
        //        scrollY: '35vh',
        //        scrollCollapse: true,
        //        autoWidth: false,
        //        select: {
        //            style: 'os',
        //            selector: 'td:first-child'
        //        },

        //        "createdRow": function( row, data, dataIndex ) {
        //            $(row).addClass( 'rowPadre' );                    
        //        },
        //        columns: [

        //            { data: 'componenteID', title: 'No Serie' },
        //            { data: 'maquinaID', title: 'Económico' },
        //            // { data: 'costoSugerido', title: 'costoSugerido' },
        //            { data: 'costoPresupuesto', title: 'Costo Presupuestado' },
        //            { data: 'horasCiclo', title: 'Horas Ciclo' },
        //            { data: 'horasAcumuladas', title: 'Horas Acumuladas' },
        //            // { data: 'presupuestoID', title: 'presupuestoID' },
        //            // { data: 'estado', title: 'estado' },
        //            // { data: 'subconjuntoID', title: 'subconjuntoID' },
        //            { data: 'obra', title: 'Obra' },
        //            // { data: 'vida', title: 'vida' },
        //            { data: 'costoReal', title: 'Costo Real' },
        //            { data: 'fecha', title: 'Fecha',
        //                render: function (data, type, row) {
        //                    return moment(data).format('DD/MM/YYYY');
        //                }
        //            },
        //            // { data: 'tipo', title: 'tipo' },
        //            // { data: 'comentarioAumento', title: 'comentarioAumento' },
        //            // { data: 'programado', title: 'programado' },
        //            // { data: 'esServicio', title: 'esServicio' }
                      
        //        ],
          
         
        //    }).columns.adjust();
        //}


        //const CargarAvanceGeneral = function (){
        //    $.blockUI({ message: "Procesando..." });
        //    $.ajax({
        //        url: '/Overhaul/CargarAvanceGeneral',
        //        datatype: "json",
        //        type: "POST",
        //        data: {
        //            obras: cboCCAvance.val(),
        //            anio: cboAnioAvance.val(),
        //            modeloID: cboModeloAvance.val(),
        //            estatus: cboEstatusAvance.val()
        //        },
        //        success: function (response) {
        //            $.unblockUI();
        //            if (response.success) {
        //                dtAvanceGeneral.clear();
        //                dtAvanceGeneral.rows.add(response.data);
        //                dtAvanceGeneral.draw(false);
        //                dtAvanceGeneral.columns.adjust();

        //                //dtAvanceGeneralAtrasado.clear();
        //                //dtAvanceGeneralAtrasado.rows.add(response.dataAtrasados);
        //                //dtAvanceGeneralAtrasado.draw(false);
        //                //dtAvanceGeneralAtrasado.columns.adjust();
        //            }
        //        }
        //    });
        //}

        //const IniciartblAvanceGeneral = function () {
        //    dtAvanceGeneral = tblAvanceGeneral.DataTable({
        //        retrieve: true,
        //        searching: false,
        //        paging: false,
        //        scrollY: '35vh',
        //        scrollCollapse: true,
        //        autoWidth: false,
        //        select: {
        //            style: 'os',
        //            selector: 'td:first-child'
        //        },

        //        "createdRow": function( row, data, dataIndex ) {
        //            $(row).addClass( 'rowPadre' );                    
        //        },
        //        columns: [

          
        //            { data: 'Descripcion', title: 'Descripcion' },
        //            { data: 'obra', title: 'Obra' },
        //            { data: 'cc', title: 'Centro de costo' },
        //            { data: 'presupuesto', title: 'Presupuesto', render: $.fn.dataTable.render.number(',', '.', 2)},
        //            { data: 'avance', title: 'Avance Presupuesto', render: $.fn.dataTable.render.number(',', '.', 2)},
        //            { data: 'avanceErogado', title: 'Avance Erogado', render: $.fn.dataTable.render.number(',', '.', 2)},
        //            { data: 'bolsaRestante', title: 'Bolsa Restante', render: $.fn.dataTable.render.number(',', '.', 2)},
                
        //        ],
          
         
        //    }).columns.adjust();
        //}

        //const IniciartblAvanceGeneralAtrasado = function () {
        //    dtAvanceGeneralAtrasado = tblAvanceGeneralAtrasado.DataTable({
        //        retrieve: true,
        //        searching: false,
        //        paging: false,
        //        scrollY: '35vh',
        //        scrollCollapse: true,
        //        autoWidth: false,
        //        select: {
        //            style: 'os',
        //            selector: 'td:first-child'
        //        },

        //        "createdRow": function( row, data, dataIndex ) {
        //            $(row).addClass( 'rowPadre' );                    
        //        },
        //        columns: [

          
        //            { data: 'Descripcion', title: 'Descripcion' },
        //            { data: 'obra', title: 'obra' },
        //            { data: 'cc', title: 'Centro de costo' },
        //            { data: 'presupuesto', title: 'presupuesto', render: $.fn.dataTable.render.number(',', '.', 2)},
        //            { data: 'avance', title: 'avance presupuesto', render: $.fn.dataTable.render.number(',', '.', 2)},
        //            { data: 'avanceErogado', title: 'avance Erogado', render: $.fn.dataTable.render.number(',', '.', 2)},
        //            { data: 'bolsaRestante', title: 'bolsa restante', render: $.fn.dataTable.render.number(',', '.', 2)},
                
        //        ],
          
         
        //    }).columns.adjust();
        //}

        const buscarPresupuesto = function () {
            let anio = cboAnioAvance.val();
            let modelo = cboModeloAvance.val();
            let subconjunto = cboSubconjunto.val();
            let noComponente = txtComponenteAvance.val();
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarPresupuestoPorComponente',
                datatype: "json",
                type: "POST",
                data: {
                    anio: anio,
                    modelo: modelo,
                    subconjunto: subconjunto,
                    noComponente: noComponente
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        dtAvance.clear();
                        dtAvance.rows.add(response.data);
                        dtAvance.draw(false);
                        dtAvance.columns.adjust();
                    }
                }
            });
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.presupuesto = new presupuesto();
    });
})();


