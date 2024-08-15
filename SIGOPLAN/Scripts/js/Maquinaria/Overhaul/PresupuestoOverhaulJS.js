(function () {

    $.namespace('maquinaria.overhaul.presupuesto');

    presupuesto = function () {
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        btnCrearPresupuesto = $("#btnCrearPresupuesto"),
        //Presupuesto
        ulNuevo = $("#ulNuevo"),
        tabPresupuesto = $("#tabPresupuesto"),
        cboModeloPresupuesto = $("#cboModeloPresupuesto"),
        tblPresupuesto = $("#tblPresupuesto"),
        cboAnioPresupuesto = $("#cboAnioPresupuesto"),
        modalDetallesComponente = $("#modalDetallesComponente");
        let obrasPresupuesto = [];
        //--// Modal Aumento
        modalAumento = $("#modalAumento"),
        txtAumento = $("#txtAumento"),
        txtComentario = $("#txtComentario"),
        btnGuardarAumento = $("#btnGuardarAumento"),
        //Autorizacion
        cboModeloAutorizacion = $("#cboModeloAutorizacion"),
        cboCCAutorizacion = $("#cboCCAutorizacion"),
        tblAutorizacion = $("#tblAutorizacion"),
        cboAnioAutorizacion = $("#cboAnioAutorizacion");
        let obrasAutorizacion = [];
        tabAutorizacion = $("#tabAutorizacion"),
        btnModalAceptar = $("#btnModalAceptar"),
        modalAutorizacion = $("#modalAutorizacion"),
        tituloModal = $("#title-modal"),
        tblModalAutorizacion = $("#tblModalAutorizacion"),
        tblPresAutorizacion = $("#tblPresAutorizacion"),
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
        let obrasAvance = [];
        let presupuestoID = 0;
        let cerrado = false;
        let respuesta = [];
        let tipoUsuario = 6;
        let dtAvance;
        let dtAvanceDetalle;
        //Reporte Inversion
        const modalReporteInversion = $("#modalReporteInversion");
        const cboObraInversion = $("#cboObraInversion");
        const tblInversion = $("#tblInversion");
        const btnPresCalendario = $("#btnPresCalendario");
        tblAvanceGeneral = $('#tblAvanceGeneral');
        //tblAvanceGeneralAtrasado = $('#tblAvanceGeneralAtrasado');
        let dtAvanceGeneral;
        let btnReporte =$('#btnReporte');
        const reporteAvance = $("#reporteAvance");
        ireporteAvance = $("#reporteAvance > #reportViewerModal > #report");

        function init() {
            PermisosBotones();
            cboModeloPresupuesto.fillCombo('/CatComponentes/FillCboModelo_Componente');
            cboAnioPresupuesto.fillCombo('/Overhaul/fillCboAnioPresupuesto');
            IniciartblPresupuesto();
            cboModeloPresupuesto.change(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                CargartblPresupuesto(e);
            });
            cboAnioPresupuesto.change(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                CargartblPresupuesto(e);
            });

            btnPresCalendario.click(function (e) {
                cboObraInversion.val('');
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                cargarTblInversion();
                modalReporteInversion.modal("show");
            });
            btnReporte.click(function (){
                CargarReporteAvanceGeneral();
            });
            initTblInversion();
            cboObraInversion.fillCombo('/Overhaul/FillCboObraMaquina', {});

            cboObraInversion.change(function (e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                cargarTblInversion();
            });

            tabPresupuesto.click(IniciartblPresupuesto);
            btnGuardarAumento.click(GuardarAumento);
            IniciarTblArchivosHistorial();            
            IniciartblAvanceGeneral();
            //IniciartblAvanceGeneralAtrasado();
            IniciartblAvance();
            IniciartblAvanceDetalle();
            //Autorizacion            
            cboModeloAutorizacion.fillCombo('/CatComponentes/FillCboModelo_Componente');
            cboAnioAutorizacion.fillCombo('/Overhaul/fillCboAnioPresupuesto');
            cboCCAutorizacion.fillCombo('/Overhaul/FillCboObraMaquinaAC', {}, true);
            convertToMultiselectSelectAll(cboCCAutorizacion);
            cboCCAutorizacion.multiselect('selectAll', false).multiselect('updateButtonText');
            IniciartblAutorizacion();
            cboModeloAutorizacion.change(CargartblAutorizacion);
            cboAnioAutorizacion.change(CargartblAutorizacion);
            tabAutorizacion.click(CargartblAutorizacion);
            cboCCAutorizacion.change(CargartblAutorizacion);
            btnModalAceptar.click(AutorizarPresupuesto);
            InitTblPresAutorizacion();
            CargartblAutorizacion();

            //Avance
            cboModeloAvance.fillCombo('/CatComponentes/FillCboModelo_Componente');
            cboAnioAvance.fillCombo('/Overhaul/fillCboAnioPresupuesto');
            cboCCAvance.fillCombo('/Overhaul/FillCboObraMaquinaAC', {}, true);
            cboConjuntoAvance.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: -1 });
            cboConjuntoAvance.change(CboSubConjuntoCargar);
            convertToMultiselectSelectAll(cboCCAvance);
            cboCCAvance.multiselect('selectAll', false).multiselect('updateButtonText');
            IniciartblAvance();
            cboModeloAvance.change(CargartblAvance);
            cboAnioAvance.change(function (){
                CargarAvanceGeneral();
                CargartblAvance();
            });
            
            cboCCAvance.change(CargartblAvance);
            tabAvance.click(function (){
                CargarAvanceGeneral();
                CargartblAvance();
            });
        }

        function PermisosBotones() {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/PermisosBotonesAdminComp",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                success: function (response) {
                    $.unblockUI();
                    tipoUsuario = response.tipoUsuario;
                    if (response.tipoUsuario == 1 || response.tipoUsuario == 2 || response.tipoUsuario == 7) {
                        tabPresupuesto.css("display", "block");
                    }
                    else { tabPresupuesto.css("display", "none"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function CboSubConjuntoCargar()
        {
            if (cboConjuntoAvance.val() != null && cboConjuntoAvance.val() != "") {
                cboSubconjuntoAvance.fillCombo('/CatComponentes/FillCboSubConjunto_Componente', { idConjunto: cboConjuntoAvance.val(), idModelo: -1 });
                cboSubconjuntoAvance.attr('disabled', false);
            }
            else {
                cboSubconjuntoAvance.clearCombo();
                cboSubconjuntoAvance.attr('disabled', true);
            }
        }
        function IniciartblPresupuesto() {            
            tblPresupuesto = $("#tblPresupuesto").DataTable({
                language: {
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
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '75vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                "drawCallback": function (settings, json) {
                    tblPresupuesto.on("click", ".btn-accion", function (e) {
                        var UpdateCell = $(this).parent('td').parent().children().eq(4);
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        CargarDetallesPresupuesto($(this).attr("data-index"), $(this).attr("data-tipo"), cboModeloPresupuesto.val(), UpdateCell, $(this).attr("data-subconjunto"), cboAnioPresupuesto.val(), 0, $(this).attr("data-esServicio"));
                        tblPresupuesto.columns.adjust();
                    });


                    tblPresupuesto.on("click", ".btn-iniciar", function (e) {                        
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var UpdateCell = $(this).parent('td').parent().children().eq(4);
                        if (presupuestoID > 0) { CerrarPresupuesto(presupuestoID); }
                        else { IniciarPresupuesto($(this).attr("data-modelo"), $(this).attr("data-anio")); }
                    });
                },
                columns: [
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'id', title: 'id' },
                    { data: 'indicador', title: 'Indicador' },
                    { data: 'cantidad', title: '#' },
                    { data: 'costo', title: '$' },
                    { data: 'desgloce', title: 'Desgloce' },
                ],
                //rowsGroup: [ "descripcion:name" ],
                "columnDefs": [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] },
                    { "visible": false, "targets": 1 },
                    { "orderable": false, "targets": 1 },
                    { "width": "0%", "targets": [1] },
                    { "width": "15%", "targets": [2] },
                    { "width": "10%", "targets": [0, 3, 4] },
                    { "width": "75%", "targets": [5] },
                ],
                "order": [[1, 'asc']],
            });
        }

        function IniciarPresupuesto(modelo, anio)
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/IniciarPresupuesto',
                datatype: "json",
                type: "POST",
                data: {
                    modelo: modelo,
                    anio: anio
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.presupuestoID > 0) {
                        presupuestoID = response.presupuestoID;
                        AlertaGeneral("Éxito", "Se ha iniciado el presupuesto con éxito");
                        cboModeloPresupuesto.change();
                    }
                },
                error: function (response) {
                    console.log(response);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }

        function CerrarPresupuesto(presupuestoID) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CerrarPresupuesto',
                datatype: "json",
                type: "POST",
                data: {
                    presupuestoID: presupuestoID
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.exito) {
                        AlertaGeneral("Éxito", "Se ha cerrado el presupuesto con éxito");
                        cboModeloPresupuesto.change();
                    }
                },
                error: function (response) {
                    console.log(response);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }

        function CargartblPresupuesto(e) {
            $("#tblPresupuesto").off("change", ".cbo-obra");
            if (cboAnioPresupuesto.val() != "" && cboModeloPresupuesto.val() != "") {
                tblPresupuesto.clear();
                tblPresupuesto.row.add(
                    {
                        id: 0,
                        descripcion: cboModeloPresupuesto.val() == "" ? "" : $("#cboModeloPresupuesto option:selected").text(),
                        indicador: '<div class="col-xs-12 col-sm-12 col-md-2 col-lg-12" style="overflow-x: visible !important;overflow-y: visible !important;">' +
                            '<label>Obra:     </label><input id="chkall" type="checkbox" checked>Seleccionar Todas<br/>' +
                            '<span id="spanComboAC" class="form-control" style="width:300px;">NINGUNO SELECCIONADO</span>' +
                            '<select class="form-control cbo-obra" id="cboObraPresupuesto" data-container="body" multiple="multiple"></select></div>' +
                            
                            '<span style="padding: 12px 12px;"><hr></span>' +
                            '<span style="padding: 12px 12px;">TOTAL PRESUPUESTO</span>' +
                            '<span style="padding: 12px 12px;"><hr></span>' +
                            '<span style="padding: 12px 12px;"># DE MÁQUINAS</span>' +
                            '<span style="padding: 12px 12px;"><hr></span>' +
                            '<span style="padding: 12px 12px;"># DE COMPONENTES</span>',
                        cantidad: '<div class="col-xs-12 col-sm-12 col-md-2 col-lg-12" style="visibility:hidden;height:50px;"> <label>Obra:</label><select class="form-control" ></select></div>' +
                            '<span style="padding: 12px 1px;"><hr></span>' +
                            '<span id="numTotalPresupuesto"></span>' +
                            '<hr>' +
                            '<span id="numMaquinasTotal"></span>' +
                            '<hr>' +
                            '<span id="numComponentesTotal"></span>',
                        costo: "",
                        desgloce: '<button data-modelo="' + cboModeloPresupuesto.val() + '" data-anio="' + cboAnioPresupuesto.val() + '"  class="btn btn-primary btn-todos btn-iniciar"></button>'
                    }
                ).draw(false);
                $("#cboObraPresupuesto").fillCombo('/Overhaul/FillCboObraMaquina', {}, true);
                //select2
                $("#cboObraPresupuesto").select2();
                $("#cboObraPresupuesto").select2({ width: '250px' });
                //Todas las obras
                var selectedItems = [];
                var allOptions = $("#cboObraPresupuesto option");
                allOptions.each(function () {
                    selectedItems.push($(this).val());
                });
                //Cargar todas las obras
                $("#cboObraPresupuesto").val(selectedItems).trigger("change");
                //Indicar cuantas obras estan seleccionadas
                var seleccionados = $("#cboObraPresupuesto").siblings("span").find(".select2-selection__choice");
                if (seleccionados.length == 0) $("#spanComboAC").text("NINGUNO SELECCIONADO");
                else {
                    if (seleccionados.length == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                    else $("#spanComboAC").text(seleccionados.length.toString() + " SELECCIONADOS");
                }
                //Ocultar combo
                $("#cboObraPresupuesto").next(".select2-container").css("display", "none");
                //Desplegar combo si se da click en totalizador de obras
                $("#spanComboAC").click(function (e) {
                    $("#cboObraPresupuesto").next(".select2-container").css("display", "block");
                    $("#cboObraPresupuesto").siblings("span").find(".select2-selection__rendered")[0].click();
                });
                //ocultar combo al cerrar lista
                $("#cboObraPresupuesto").on('select2:close', function (e) {
                    $("#cboObraPresupuesto").next(".select2-container").css("display", "none");
                    var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                    if (seleccionados.length == 0) {
                        $("#spanComboAC").text("NINGUNO SELECCIONADO");
                        $("#numComponentesTotal").text(0);
                        $("#numMaquinasTotal").text(0);
                        $("#numTotalPresupuesto").text("$0.00");
                    }
                    else {
                        if (seleccionados.length == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                        else $("#spanComboAC").text(seleccionados.length.toString() + " SELECCIONADOS");
                    }
                });
                $("#cboObraPresupuesto").on("select2:unselect", function (evt) {
                    if (!evt.params.originalEvent) { return; }
                    evt.params.originalEvent.stopPropagation();
                });
                //Check seleccionar todas
                $("#chkall").click(function () {
                    if ($("#chkall").is(':checked')) {
                        $("#cboObraPresupuesto").val(selectedItems).trigger("change").trigger("select2:close");
                    } else {
                        $("#cboObraPresupuesto").val('').trigger("change").trigger("select2:close");
                        $("#numComponentesTotal").text(0);
                        $("#numMaquinasTotal").text(0);
                        $("#numTotalPresupuesto").text("$0.00");
                    }
                });


                obrasPresupuesto = $("#cboObraPresupuesto").val();
                CargarTablaPresupuesto(obrasPresupuesto);
                
                tblPresupuesto.columns.adjust();


                tblPresupuesto.on("change", ".cbo-obra", function (e) {
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    var firstElement = $('tbody > tr').first();
                    tblPresupuesto.rows(firstElement.nextAll('tr')).remove().draw();
                    CargarTablaPresupuesto($("#cboObraPresupuesto").val());
                });
            }
        }
        function CargarTablaPresupuesto(obrasID)
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarTblPresupuesto',
                datatype: "json",
                type: "POST",
                data: {
                    obras: obrasID,
                    anio: cboAnioPresupuesto.val(),
                    modeloID: cboModeloPresupuesto.val()
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        presupuestoID = response.presupuestoID;
                        cerrado = response.cerrado;
                        var totalPresupuesto = 0;
                        if (presupuestoID > 0)
                        {
                            $(".btn-iniciar").text("Cerrar Presupuesto");
                        }
                        else
                        {
                            $(".btn-iniciar").text("Iniciar Presupuesto");
                        }
                        if (cerrado) { $(".btn-iniciar").prop("disabled", true); }
                        else { $(".btn-iniciar").prop("disabled", false); }
                        var presupuesto = response.data;
                        var numComponentes = 0;                        
                        for(let i = 0; i < presupuesto.length; i++)
                        {
                            numComponentes += presupuesto[i].maquinasComponentes.length;
                            var htmlObrasNombre = "";
                            var htmlObrasValor = "";
                            var htmlObrasCosto = "";
                            var indicadoresVida = "";
                            var cantidadVida = "";
                            var costoVida = "";
                            for (let j = 0; j < presupuesto[i].obras.length; j++)
                            {
                                htmlObrasValor += '<span><hr></span>' +
                                                  '<div style="height:30px;">' +
                                                  '<span>' + presupuesto[i].obras[j].Valor + '</span>' +
                                                  '</div>';
                                                  
                                htmlObrasNombre += '<span><hr></span>' +
                                                   '<button data-index="' + presupuesto[i].obras[j].Propiedad + '" class="btn btn-xs btn-warning btn-obra btn-accion" style="border:0;height:30px" data-esServicio ="' + presupuesto[i].esServicio + '" data-subconjunto="' +
                                                   presupuesto[i].subconjuntoID + '" ' + (presupuesto[i].obras[j].Autorizado ? 'disabled' : '') + '># ' + presupuesto[i].obras[j].Nombre.toUpperCase() + '</button>';

                                htmlObrasCosto += '<span><hr></span>' +
                                                  '<div style="height:30px;text-align:right;font-size:13px;font-weight:bold;">' +
                                                  '<span>$ ' + presupuesto[i].obras[j].Costo.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</span>' +
                                                  '</div>';
                            }
                            for (let j = 0; j < presupuesto[i].vida.length; j++)
                            {
                                indicadoresVida += (presupuesto[i].vida[j] > 0 ?
                                    ('<hr>' +
                                    '<button data-index="' + $("#cboObraPresupuesto").val() + '" data-tipo="' + j + '" data-esServicio ="' + presupuesto[i].esServicio + '" data-subconjunto="' + presupuesto[i].subconjuntoID + '" class="btn btn-xs bg-danger btn-vida btn-accion" style="border:0;height:30px">VIDA ' + j + '</button>') : "");

                                cantidadVida += (presupuesto[i].vida[j] > 0 ?
                                    ('<hr>' +
                                    '<div style="height:30px;">' +
                                    '<span>' + presupuesto[i].vida[j] + '</span>' +
                                    '</div>') : "");
                                costoVida += (presupuesto[i].vida[j] > 0 ?
                                    ('<hr>' +
                                    '<div style="height:30px;text-align:right;font-size:13px;font-weight:bold;">' +
                                    //'<span>$ ' + "0" + '</span>' +
                                    '<span>$ ' + presupuesto[i].costoVida[j].toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</span>' +
                                    '</div>') : "");                                    
                            }
                            var agregar = {
                                "descripcion": presupuesto[i].subconjunto,
                                "id": presupuesto[i].subconjuntoID,
                                "indicador":
                                    '<button data-index="' + $("#cboObraPresupuesto").val() + '" data-esServicio ="' + presupuesto[i].esServicio + '" data-subconjunto="' + presupuesto[i].subconjuntoID + '" class="btn btn-xs btn-success btn-todos btn-accion" style="border:0;height:30px"># A PRESUPUESTAR</button>' +
                                    htmlObrasNombre + indicadoresVida,
                                "cantidad":
                                    '<div style="height:30px;">' +
                                    '<span>' + presupuesto[i].maquinasComponentes.length + '</span>' +
                                    '</div>' +
                                    htmlObrasValor + cantidadVida,
                                "costo":
                                    '<div style="height:30px;text-align:right;font-size:13px;font-weight:bold;">' +
                                    '<span>$ ' + presupuesto[i].costoTotal.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</span>' +
                                    '</div>' +
                                    htmlObrasCosto + costoVida,
                                "desgloce": "" 
                            };
                            totalPresupuesto += presupuesto[i].costoTotal;
                            tblPresupuesto.row.add(agregar);
                        }
                        tblPresupuesto.draw(false);
                        let numMaquinas = $.map(presupuesto, function (presupuesto) {
                            return presupuesto.maquinasComponentes;
                        });
                        //numMaquinas = $.map(numMaquinas, function (numMaquinas) {
                        //    return numMaquinas.locacion;
                        //});
                        $("#numComponentesTotal").text(numComponentes);
                        $("#numMaquinasTotal").text(unico(numMaquinas).length);
                        $("#numTotalPresupuesto").text("$" + totalPresupuesto.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    }
                },
                error: function (response) {
                    console.log(response);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }
        function unico(array) {
            return $.grep(array, function (el, index) {
                return index === $.inArray(el, array);
            });
        }
        function SumArray(numArray) {
            var sum = 0;
            for (var i = 0; i < numArray.length; i++) {
                if (!isNaN(numArray[i])) {
                    sum += numArray[i];
                }
            }
            return sum;
        }
        function CargarDetallesPresupuesto(obra, vida, modelo, updateCell, subconjunto, anio, tipo, esServicio) {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/GetDetallePresupuesto',
                datatype: "json",
                type: "POST",
                data: {
                    obras: (obra == "" || obra == null) ? obrasPresupuesto : obra,
                    vidas: vida,
                    modelo: modelo,
                    anio: anio,
                    subConjunto: subconjunto,
                    presupuestoID: presupuestoID,
                    esServicio: esServicio
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        var infoPresupuestos = "";
                        for (let i = 0; i < response.data.length; i++)
                        {
                            infoPresupuestos +=
                            '<tr style="background-color:' + response.data[i].color + ';">' +
                                '<td>' + response.data[i].noEconomico + '</td>' +
                                '<td>' + response.data[i].horometroCiclo.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</td>' +
                                '<td>' + response.data[i].horometroAcumulado.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</td>' +
                                '<td class="btn btn-info historialComponente" style="font-size:12px;" data-index="' + response.data[i].componenteID + '">' + response.data[i].noComponente + '</td>' +
                                (tipo == 0 ? '<td class="btn details-control-shown center-block" style="display: table-cell" data-componenteid="' + response.data[i].componenteID + '" ' + (presupuestoID == 0 || !response.data[i].guardado ? "disabled" : "") + ' data-input="number' + subconjunto + i + '"></td>' : '') +
                                '<td style="text-align:center;">' + response.data[i].costoSugerido + '</td>' +
                                '<td class="btn details-control center-block" style="display: table-cell;" data-componenteid="' + response.data[i].componenteID + '" ' + (presupuestoID == 0 || !response.data[i].guardado ? "disabled" : "") + ' data-input="number' + subconjunto + i + '"></td>' +
                                '<td style="white-space: nowrap;">' +
                                        '<input class="form-control" type="number" id="number' + subconjunto + i + '" value="' + response.data[i].costo + '" style="display:inline-block;width:100% !important;font-size:12px;height: 20px;" disabled />' +
                                '</td>' +
                                (tipo == 0 ? (!response.data[i].guardado ? ('<td><button class="btn btn-primary btn-sm glyphicon glyphicon-ok guardarPresupuesto" data-componenteid="' + response.data[i].componenteID + '" data-esServicio="' + response.data[i].esServicio + '" data-maquinaid="' + response.data[i].maquinaID + '" data-costo="' + response.data[i].costo + '" data-eventoid="' + response.data[i].eventoID + '" data-vidas="' + response.data[i].vida + '" ' + (presupuestoID == 0 ? "disabled" : "") + '></button></td>')
                                    : '<td><button class="btn btn-sm btn-primary glyphicon glyphicon glyphicon-ok disabled"></button></td>') : '') +
                            '</tr>';
                        }
                        if (response.data.length == 0)
                        {
                            infoPresupuestos += '<tr><td colspan="9" class="no-results">No se encontró información</td></tr>';
                        }

                        var tabla =
                        '<div class="blockTablaDetalles" style="font-size:12px;">' +
                            '<table class="table text-center display compact tablaDetalles" aria-busy="false">' +
                                '<thead class="bg-table-header">' +
                                    '<tr style="background-color: inherit;">' +
                                        '<th data-column-id="noEconomico">' +
                                            'Económico' +
                                        '</th>' +
                                        '<th data-column-id="horCiclo">' +
                                            'Hor. Ciclo' +
                                        '</th>' +
                                        '<th data-column-id="horAcumulado">' +
                                            'Hor. Acum.' +
                                        '</th>' +
                                        '<th data-column-id="serie">' +
                                            'SerieComponente' +
                                        '</th>' +
                                        (tipo == 0 ? '<th data-column-id="reduccion"></th>' : '') +
                                        '<th data-column-id="costoSugerido">' +
                                            'Costo Sugerido' +
                                        '</th>' +
                                        '<th data-column-id="aumento"></th>' +
                                        '<th data-column-id="aumCosto">' +
                                            'Costo Presupuesto' +
                                        '</th>' +
                                        (tipo == 0 ? '<th data-column-id="guardarPresupuesto">Incluir</th>' : '') +
                                    '</tr>' +
                                '</thead>' +
                                '<tbody>' +
                                    infoPresupuestos +
                                '</tbody>' +
                            '</table>' +
                        '</div>';
                        $(".blockTablaDetalles").css("display", "none");
                        updateCell.html(tabla)
                    }
                    $(".guardarPresupuesto").click(function (event) {
                        event.stopPropagation();
                        event.stopImmediatePropagation();
                        GuardarCosto($(this).attr("data-componenteid"), $(this).attr("data-maquinaid"), $(this).attr("data-costo"), $(this).attr("data-eventoid"), $(this).parent().prev().children("input").val(), $(this).parent(), $(this).attr("data-vidas"), $(this).attr("data-esServicio"));
                    });
                    $(".tablaDetalles").on('click', 'td.details-control-shown', function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        $("#lbAumento").text("Reducción Presupuesto");
                        CargarComentarioAumento($(this).attr("data-componenteID"));
                        btnGuardarAumento.attr("data-index", presupuestoID);
                        btnGuardarAumento.attr("data-componenteid", $(this).attr("data-componenteID"));
                        btnGuardarAumento.attr("data-tipo", 0);
                        btnGuardarAumento.attr("data-input", $(this).attr("data-input"));
                        txtAumento.parent().parent().css("display", "block");
                        btnGuardarAumento.parent().css("display", "block");

                        modalAumento.appendTo("body").modal('show');
      
                    });
                    $(".tablaDetalles").on('click', 'td.details-control', function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        $("#lbAumento").text("Aumento Presupuesto");
                        CargarComentarioAumento($(this).attr("data-componenteID"));
                        btnGuardarAumento.attr("data-index", presupuestoID);
                        btnGuardarAumento.attr("data-componenteid", $(this).attr("data-componenteID"));
                        btnGuardarAumento.attr("data-tipo", 1);
                        btnGuardarAumento.attr("data-input", $(this).attr("data-input"));
                        if (tipo == 0) {
                            txtAumento.parent().parent().css("display", "block");
                            btnGuardarAumento.parent().css("display", "block");
                        }
                        else {
                            txtAumento.parent().parent().css("display", "none");
                            btnGuardarAumento.parent().css("display", "none");
                        }

                        modalAumento.appendTo("body").modal('show');
                    });
                    $(".tablaDetalles").on('click', 'td.historialComponente', function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        $("#lgHistorial").text("Historial componente " + $(this).text());
                        cargarGridHistorialComponente($(this).attr("data-index"), $("#gridDetallesHistorial"));
                        modalDetallesComponente.appendTo("body").modal('show');
                    });
                    $(".tablaDetalles").DataTable({
                        retrieve: true,
                        searching: false,
                        paging: false,
                        scrollY: '35vh',
                        scrollCollapse: true,
                        autoWidth: false,
                        select: {
                            style: 'os',
                            selector: 'td:first-child'
                        },
                        "columnDefs": [
                          { "orderable": false, "targets": [4, 6, 8] }
                        ]
                    });
                    
                },
                error: function (response) {
                    console.log(response);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }
        function GuardarCosto(componenteID, maquinaID, costoSugerido, eventoID, costo, parent, vidas, esServicio)
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/GuardarCostoPresupuesto',
                datatype: "json",
                type: "POST",
                data: {
                    componenteID: componenteID == null ? '' : componenteID,
                    maquinaID: maquinaID == null ? '' : maquinaID,
                    costo: costo == null ? '' : costo,
                    eventoID: eventoID == null ? '' : eventoID,
                    costoSugerido: costoSugerido == null ? '' : costoSugerido,
                    modelo: cboModeloPresupuesto.val() == null ? '' : cboModeloPresupuesto.val(),
                    anio: cboAnioPresupuesto.val() == null ? '' : cboAnioPresupuesto.val(),
                    presupuestoID: presupuestoID == null ? '' : presupuestoID,
                    vidas: vidas == null ? '' : vidas,
                    esServicio: esServicio
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.presupuestoID != 0)
                    {
                        presupuestoID = response.presupuestoID;
                        parent.parent().css("background-color", "#b4e4b4");
                        AlertaGeneral("Éxito", "Se guardó el presupuesto con éxito");
                        $("td.details-control-shown").removeAttr("disabled");
                        $("td.details-control").removeAttr("disabled");
                        $("td.guardarPresupuesto").removeAttr("disabled");
                    }
                },
                error: function (response) {
                    console.log(response);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }
        function cargarGridHistorialComponente(idComponente, grid) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/ModalComponentesHistorial",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ idComponente: idComponente }),
                success: function (response) {
                    $.unblockUI();
                    grid.bootgrid({
                        templates: {
                            header: ""
                        },
                        rowCount: -1,
                        sorting: false,
                        formatters: {
                            "reciclado": function (column, row) {
                                return '<span class="reciclado ' + (!row.reciclado ? '' : 'glyphicon glyphicon-ok') + '"> </span>';
                            },
                            "archivos": function (column, row) {
                                return "<button type='button' class='btn btn-warning archivos' data-trackID='" + row.id + "'><span class='fa fa-file-pdf'></span>  </button>"
                            }
                        }
                    }).on("loaded.rs.jquery.bootgrid", function () {
                        grid.find(".archivos").parent().css("text-align", "center");
                        grid.find(".archivos").parent().css("width", "3%");
                        grid.find(".archivos").on("click", function (e) {
                            trackID = $(this).attr("data-trackID");
                            cargarTblArchivosHistorial($(this).attr("data-trackID"), $("#tblArchivosHistorial"));
                            $("#modalArchivoHistorial").appendTo("body").modal('show');
                        });
                    });

                    grid.bootgrid("clear");
                    grid.bootgrid("append", response.historial);
                    grid.bootgrid('reload');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function cargarTblArchivosHistorial(idTrack, tabla) {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/cargarGridArchivosCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idTrack: idTrack }),
                success: function (response) {
                    $.unblockUI();
                    tabla.bootgrid("clear");
                    tabla.bootgrid("append", response.archivos);
                    tabla.bootgrid('reload');
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function IniciarTblArchivosHistorial() {
            $("#tblArchivosHistorial").bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "fecha": function (column, row) {
                        var fecha = row.FechaCreacion.substring(0, 2) + "/" + row.FechaCreacion.substring(2, 4) + "/" + row.FechaCreacion.substring(4, 8);
                        return "<span class='estatus'> " + fecha + " </span>";
                    },
                    "descargar": function (column, row) {
                        return "<button type='button' class='btn btn-primary descargar' data-index='" + row.id + "' >" +
                            "<span class='glyphicon glyphicon-ok'></span></button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                $("#tblArchivosHistorial").find(".eliminar").parent().css("text-align", "center");
                $("#tblArchivosHistorial").find(".eliminar").parent().css("width", "3%");
                $("#tblArchivosHistorial").find(".descargar").parent().css("text-align", "center");
                $("#tblArchivosHistorial").find(".descargar").parent().css("width", "3%");
                $("#tblArchivosHistorial").find(".descargar").on("click", function (e) {
                    descargarArchivoHistorial($(this).attr("data-index"));
                });
            });
        }

        function descargarArchivoHistorial(idArchivo) {
            window.location.href = "/Overhaul/DescargarArchivoCRC?idTrack=" + trackID + "&idArchivo=" + idArchivo;
        }

        function CargarComentarioAumento(componenteID)
        {
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: "/Overhaul/ComentarioAumPresupuesto",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({ presupuestoID: presupuestoID, componenteID: componenteID }),
                success: function (response) {
                    $.unblockUI();
                    if (response.comentario != "") {
                        txtAumento.val('');
                        txtComentario.val(response.comentario);
                    }
                    else
                    {
                        AlertaGeneral("Alerta", "Se ha producido un error al tratar de actualizar el costo");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function GuardarAumento()
        {
            if(txtAumento.val() != "" && txtComentario.val() != ""){
                $.blockUI({
                    message: mensajes.PROCESANDO,
                    baseZ: 2000
                });
                $.ajax({
                    url: "/Overhaul/GuardarAumentoPresupuesto",
                    type: 'POST',
                    dataType: 'json',
                    contentType: 'application/json',
                    //async: false,
                    data: JSON.stringify({ aumento: txtAumento.val(), comentario: txtComentario.val(), presupuestoID: btnGuardarAumento.attr("data-index"), componenteID: btnGuardarAumento.attr("data-componenteid"), tipo: btnGuardarAumento.attr("data-tipo") }),
                    success: function (response) {
                        $.unblockUI();
                        if (response.costo >= 0) {
                            AlertaGeneral("Alerta", "Se ha actualizado el costo");
                            modalAumento.modal("hide");
                            $("#" + btnGuardarAumento.attr("data-input")).val(response.costo);
                            $("#" + btnGuardarAumento.attr("data-input")).parent().parent().css("background-color", "#b4e4b4");
                            txtAumento.val('');
                            txtComentario.val('');
                        }
                        else
                        {
                            AlertaGeneral("Alerta", "Se ha producido un error al tratar de actualizar el costo");
                        }
                    },
                    error: function (response) {
                        $.unblockUI();
                        AlertaGeneral("Alerta", response.message);
                    }
                });
            }
            else
            {
                AlertaGeneral("Alerta", "Se requieren todos los datos del formulario");
            }
        }

        //Autorizacion
        function IniciartblAutorizacion() {
            tblAutorizacion = $("#tblAutorizacion").DataTable({
                language: {
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
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '35vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                "drawCallback": function (settings, json) {
                    tblAutorizacion.on("click", ".enviar", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnModalAceptar.attr("data-index", $(this).attr("data-index"));
                        btnModalAceptar.attr("data-anio", $(this).attr("data-anio"));
                        btnModalAceptar.attr("data-modelo", $(this).attr("data-modelo"));
                        //btnModalAceptar.attr("data-obra", $(this).attr("data-obra"));
                        btnModalAceptar.attr("data-modeloDes", $(this).attr("data-modeloDes"));
                        btnModalAceptar.attr("data-tipo", 0);
                        tituloModal.text("Envío de Presupuesto");
                        CargarTblPresAutorizacion($(this).attr("data-modeloDes"));
                        modalAutorizacion.appendTo("body").modal('show');
                    });
                    tblAutorizacion.on("click", ".vobo1", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnModalAceptar.attr("data-index", $(this).attr("data-index"));
                        btnModalAceptar.attr("data-anio", $(this).attr("data-anio"));
                        btnModalAceptar.attr("data-modelo", $(this).attr("data-modelo"));
                        //btnModalAceptar.attr("data-obra", $(this).attr("data-obra"));
                        btnModalAceptar.attr("data-modeloDes", $(this).attr("data-modeloDes"));
                        btnModalAceptar.attr("data-tipo", 1);
                        tituloModal.text("VoBo Presupuesto");
                        CargarTblPresAutorizacion($(this).attr("data-modeloDes"));
                        modalAutorizacion.appendTo("body").modal('show');
                    });
                    tblAutorizacion.on("click", ".vobo2", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnModalAceptar.attr("data-index", $(this).attr("data-index"));
                        btnModalAceptar.attr("data-anio", $(this).attr("data-anio"));
                        btnModalAceptar.attr("data-modelo", $(this).attr("data-modelo"));
                        //btnModalAceptar.attr("data-obra", $(this).attr("data-obra"));
                        btnModalAceptar.attr("data-modeloDes", $(this).attr("data-modeloDes"));
                        btnModalAceptar.attr("data-tipo", 2);
                        tituloModal.text("VoBo Presupuesto");
                        CargarTblPresAutorizacion($(this).attr("data-modeloDes"));
                        modalAutorizacion.appendTo("body").modal('show');
                    });
                    tblAutorizacion.on("click", ".vobo3", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnModalAceptar.attr("data-index", $(this).attr("data-index"));
                        btnModalAceptar.attr("data-anio", $(this).attr("data-anio"));
                        btnModalAceptar.attr("data-modelo", $(this).attr("data-modelo"));
                        //btnModalAceptar.attr("data-obra", $(this).attr("data-obra"));
                        btnModalAceptar.attr("data-modeloDes", $(this).attr("data-modeloDes"));
                        btnModalAceptar.attr("data-tipo", 3);
                        tituloModal.text("VoBo Presupuesto");
                        CargarTblPresAutorizacion($(this).attr("data-modeloDes"));
                        modalAutorizacion.appendTo("body").modal('show');
                    });
                    tblAutorizacion.on("click", ".autorizar", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        btnModalAceptar.attr("data-index", $(this).attr("data-index"));
                        btnModalAceptar.attr("data-anio", $(this).attr("data-anio"));
                        btnModalAceptar.attr("data-modelo", $(this).attr("data-modelo"));
                        //btnModalAceptar.attr("data-obra", $(this).attr("data-obra"));
                        btnModalAceptar.attr("data-modeloDes", $(this).attr("data-modeloDes"));
                        btnModalAceptar.attr("data-tipo", 4);
                        tituloModal.text("Autorización de Presupuesto");
                        CargarTblPresAutorizacion($(this).attr("data-modeloDes"));
                        modalAutorizacion.appendTo("body").modal('show');
                    });
                },
                columns: [
                    { data: 'modelo', title: 'Modelo' },
                    { data: 'anio', title: 'Año' },
                    //{ data: 'obra', title: 'Obra' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            if (row.estado < 2)
                                return '<button type="button" class="btn btn-primary enviar" ' + (row.cerrado ? ((tipoUsuario == 1) ? '' : 'disabled') : 'disabled')
                                    //+ ' data-index="' + row.presupuestoID + '" data-anio="' + row.anio + '" data-obra="' + row.obraID + '" data-modelo="' + row.modeloID
                                    + ' data-index="' + row.presupuestoID + '" data-anio="' + row.anio + '" data-modelo="' + row.modeloID
                                    + '" data-modeloDes="' + row.modelo + '"><span class="glyphicon glyphicon-play"></span></button>';
                            else {
                                return '<span>' + row.fechaEnvio + '</span>'
                            }
                        },
                        title: "Enviar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            if (row.estado < 3)
                                return '<button type="button" class="btn btn-primary vobo1" ' + (row.estado == 2 ? ((tipoUsuario == 2) ? '' : 'disabled') : 'disabled')
                                    //+ ' data-index="' + row.presupuestoID + '" data-anio="' + row.anio + '" data-obra="' + row.obraID + '" data-modelo="' + row.modeloID
                                    + ' data-index="' + row.presupuestoID + '" data-anio="' + row.anio + '" data-modelo="' + row.modeloID
                                    + '" data-modeloDes="' + row.modelo + '"><span class="glyphicon glyphicon-play"></span></button>';
                            else {
                                return '<span>' + row.fechaVoBo1 + '</span>'
                            }
                        },
                        title: 'VoBo 1'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            if (row.estado < 4)
                                return '<button type="button" class="btn btn-primary vobo2" ' + (row.estado == 3 ? ((tipoUsuario == 8) ? '' : 'disabled') : 'disabled')
                                    //+ ' data-index="' + row.presupuestoID + '" data-anio="' + row.anio + '" data-obra="' + row.obraID + '" data-modelo="' + row.modeloID
                                    + ' data-index="' + row.presupuestoID + '" data-anio="' + row.anio + '" data-modelo="' + row.modeloID
                                    + '" data-modeloDes="' + row.modelo + '"><span class="glyphicon glyphicon-play"></span></button>';
                            else {
                                return '<span>' + row.fechaVoBo2 + '</span>'
                            }
                        },
                        title: 'VoBo 2'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            if (row.estado < 5)
                                return '<button type="button" class="btn btn-primary vobo3" ' + (row.estado == 4 ? ((tipoUsuario == 4) ? '' : 'disabled') : 'disabled')
                                    //+ ' data-index="' + row.presupuestoID + '" data-anio="' + row.anio + '" data-obra="' + row.obraID + '" data-modelo="' + row.modeloID
                                    + ' data-index="' + row.presupuestoID + '" data-anio="' + row.anio + '" data-modelo="' + row.modeloID
                                    + '" data-modeloDes="' + row.modelo + '"><span class="glyphicon glyphicon-play"></span></button>';
                            else {
                                return '<span>' + row.fechaVoBo3 + '</span>'
                            }
                        },
                        title: 'VoBo 3'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            if (row.estado < 6)
                                return '<button type="button" class="btn btn-success autorizar" '
                                    + (row.estado == 5 ? ((tipoUsuario == 5 || tipoUsuario == 7) ? '' : 'disabled') : 'disabled') + ' data-index="' + row.presupuestoID + '" data-anio="'
                                    //+ row.anio + '" data-obra="' + row.obraID + '" data-modelo="' + row.modeloID + '" data-modeloDes="' + row.modelo
                                    + row.anio + '" data-modelo="' + row.modeloID + '" data-modeloDes="' + row.modelo
                                    + '"><span class="glyphicon glyphicon-play"></span></button>';
                            else {
                                return '<span>' + row.fechaAutorizacion + '</span>'
                            }
                        },
                        title: 'Autorizar'
                    }
                ],
                //rowsGroup: [ "descripcion:name" ],
                "columnDefs": [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5, 6] },
                    //{ "visible": false, "targets": 0 },
                    //{ "orderable": false, "targets": 0 },
                    { "width": "7%", "targets": [2, 3, 4, 5, 6] },
                    { "width": "20%", "targets": [0] },
                    { "width": "20%", "targets": [1] },
                ],
                "order": [[0, 'asc']],
            });
        }

        function CargartblAutorizacion() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarTblAutorizacion',
                datatype: "json",
                type: "POST",
                data: {
                    obras: cboCCAutorizacion.val(),
                    anio: cboAnioAutorizacion.val(),
                    modeloID: cboModeloAutorizacion.val()
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        tipoUsuario = response.tipoUsuario;
                        tblAutorizacion.clear();
                        tblAutorizacion.rows.add(response.data);
                        tblAutorizacion.draw();
                        tblAutorizacion.columns.adjust();                        
                    }
                }
            });
            
        }

        function initTblModalAuto()
        {
            var groupColumn = 4;
            tblModalAutorizacion = $("#tblModalAutorizacion").DataTable({
                language: {
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
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '35vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                "drawCallback": function (settings, json) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td colspan="7" style="background-color:black !important;color:white !important;"><b>' + moment(group).format('DD/MM/YYYY')
                                + '</b></td></tr>'
                            );
                            last = group;
                        }
                    });
                },
                "footerCallback": function ( row, data, start, end, display ) {
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(5).footer()).html('$' + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                },
                columns: [
                    { data: 'noEconomico', title: 'Equipo' },
                    { data: 'subconjunto', title: 'Componente' },
                    { data: 'horasCiclo', title: 'Horas Comp. al corte' },
                    { data: 'target', title: 'target' },
                    {
                        data: 'fechaID',
                        render: function (data, type, row, meta) {
                            return moment(data).format('DD/MM/YYYY');
                        },
                        title: 'Próximo PCR'
                    },
                    {
                        data: 'costo',
                        render: $.fn.dataTable.render.number( ',', '.', 2 ),
                        title: 'Presupuesto'
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            switch (row.causa)
                            {
                                case 0:
                                    return "OVERHAUL GENERAL";
                                    break;
                                case 1:
                                    return "CAMBIO DE MOTOR";
                                    break;
                                case 2:
                                    return "COMPONENTE DESFASADO";
                                    break;
                                default:
                                    return "";
                            }
                        },
                        title: 'Resumen de paro'
                    }
                ],
                //rowsGroup: [ "descripcion:name" ],
                "columnDefs": [
                    //{ "visible": false, "targets": [0] },
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] },
                    //{ "visible": false, "targets": 0 },
                    ////{ "orderable": false, "targets": 0 },
                    //{ "width": "7%", "targets": [3, 4, 5, 6] },
                    //{ "width": "20%", "targets": [0] },
                    //{ "width": "20%", "targets": [1] },
                    //{ "width": "32%", "targets": [2] }
                ],
                "order": [[4, 'asc'], [0, 'asc']],
            });
        }

        function cargarTblModalAuto(presupuestoID, obra)
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarTblModalAutorizacion',
                datatype: "json",
                type: "POST",
                data: {
                    presupuestoID: presupuestoID,
                    obra: obra
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        tblModalAutorizacion.clear();
                        tblModalAutorizacion.rows.add(response.data);
                        tblModalAutorizacion.draw();
                        tblModalAutorizacion.columns.adjust();
                    }
                }
            });
        }
        function InitTblPresAutorizacion() {
            let modelo = btnModalAceptar.attr("data-modelo");
            let anio = btnModalAceptar.attr("data-anio");
            tblPresAutorizacion = $("#tblPresAutorizacion").DataTable({
                language: {
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
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '60vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },
                "drawCallback": function (settings, json) {
                    tblPresAutorizacion.on("click", ".btn-accion-auto", function (e) {
                        var UpdateCell = $(this).parent('td').parent().children().eq(4);
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        CargarDetallesPresupuesto($(this).attr("data-index"), $(this).attr("data-tipo"), btnModalAceptar.attr("data-modelo"), UpdateCell, $(this).attr("data-subconjunto"), $(this).attr("data-anio"), 1, $(this).attr("data-esServicio"));
                    });

                    tblPresAutorizacion.on("change", ".cbo-obra", function (e) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();


                        CargarTablaPresupuestoAuto($("#cboObraPresupuestoAuto").val(), btnModalAceptar.attr("data-modelo"), btnModalAceptar.attr("data-anio"));
                    });
                },
                columns: [
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'id', title: 'id' },
                    { data: 'indicador', title: 'Indicador' },
                    { data: 'cantidad', title: '#' },
                    { data: 'costo', title: '$' },
                    { data: 'desgloce', title: 'Desgloce' },
                ],
                "columnDefs": [
                    { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 5] },
                    { "visible": false, "targets": 1 },
                    { "orderable": false, "targets": 1 },
                    { "width": "15%", "targets": [2] },
                    { "width": "10%", "targets": [0, 3, 4] }
                ],
                "order": [[1, 'asc']],
            });
        }


        function CargarTblPresAutorizacion(modeloMaquina) {            
            let modelo = btnModalAceptar.attr("data-modelo");
            let anio = btnModalAceptar.attr("data-anio");
            let modeloDes = btnModalAceptar.attr("data-modeloDes");
            let obra = btnModalAceptar.attr("data-obra");
            if (anio != "" && modelo != "") {
                tblPresAutorizacion.clear();
                tblPresAutorizacion.row.add(
                    {
                        id: 0,
                        descripcion: modeloMaquina,
                        indicador: '<div class="col-xs-12 col-sm-12 col-md-2 col-lg-12" style="overflow-x: visible !important;overflow-y: visible !important;">' +
                            '<label>Obra:     </label><input id="chkallAuto" type="checkbox" checked>Seleccionar Todas<br/>' +
                            '<span id="spanComboACAuto" class="form-control" style="width:300px;">NINGUNO SELECCIONADO</span>' +
                            '<select class="form-control cbo-obra" id="cboObraPresupuestoAuto" data-container="body" multiple="multiple"></select></div>' +

                            '<span style="padding: 12px 12px;"><hr></span>' +
                            '<span style="padding: 12px 12px;"><hr></span>' +
                            '<span style="padding: 12px 12px;">TOTAL PRESUPUESTO</span>' +
                            '<span style="padding: 12px 12px;"><hr></span>' +
                            '<span style="padding: 12px 12px;"># DE MÁQUINAS</span>' +
                            '<span style="padding: 12px 12px;"><hr></span>' +
                            '<span style="padding: 12px 12px;"># DE COMPONENTES</span>',
                        cantidad: '<div class="col-xs-12 col-sm-12 col-md-2 col-lg-12" style="visibility:hidden;height:50px;"> <label>Obra:</label><select class="form-control" ></select></div>' +
                            '<span style="padding: 12px 1px;"><hr></span>' +
                            '<span style="padding: 12px 12px;"><hr></span>' +
                            '<span id="numTotalPresupuestoAuto"></span>' +
                            '<hr>' +
                            '<span id="numMaquinasTotalAuto"></span>' +
                            '<hr>' +
                            '<span id="numComponentesTotalAuto"></span>',
                        costo: "",
                        desgloce: ''
                    }
                ).draw(false);
                $("#cboObraPresupuestoAuto").fillCombo('/Overhaul/FillCboObraMaquina', {}, true);
                //select2
                $("#cboObraPresupuestoAuto").select2();
                $("#cboObraPresupuestoAuto").select2({ width: '250px' });
                //Todas las obras
                var selectedItems = [];
                var allOptions = $("#cboObraPresupuestoAuto option");
                allOptions.each(function () {
                    selectedItems.push($(this).val());
                });
                //Cargar todas las obras
                $("#cboObraPresupuestoAuto").val(selectedItems).trigger("change");
                //Indicar cuantas obras estan seleccionadas
                var seleccionados = $("#cboObraPresupuestoAuto").siblings("span").find(".select2-selection__choice");
                if (seleccionados.length == 0) $("#spanComboAC").text("NINGUNO SELECCIONADO");
                else {
                    if (seleccionados.length == 1) $("#spanComboACAuto").text($(seleccionados[0]).text().slice(1));
                    else $("#spanComboACAuto").text(seleccionados.length.toString() + " SELECCIONADOS");
                }
                //Ocultar combo
                $("#cboObraPresupuestoAuto").next(".select2-container").css("display", "none");
                //Desplegar combo si se da click en totalizador de obras
                $("#spanComboACAuto").click(function (e) {
                    $("#cboObraPresupuestoAuto").next(".select2-container").css("display", "block");
                    $("#cboObraPresupuestoAuto").siblings("span").find(".select2-selection__rendered")[0].click();
                });
                //ocultar combo al cerrar lista
                $("#cboObraPresupuestoAuto").on('select2:close', function (e) {
                    $("#cboObraPresupuestoAuto").next(".select2-container").css("display", "none");
                    var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                    if (seleccionados.length == 0) {
                        $("#spanComboACAuto").text("NINGUNO SELECCIONADO");
                        $("#numComponentesTotalAuto").text(0);
                        $("#numMaquinasTotalAuto").text(0);
                        $("#numTotalPresupuestoAuto").text("$0.00");
                    }
                    else {
                        if (seleccionados.length == 1) $("#spanComboACAuto").text($(seleccionados[0]).text().slice(1));
                        else $("#spanComboACAuto").text(seleccionados.length.toString() + " SELECCIONADOS");
                    }
                });
                $("#cboObraPresupuestoAuto").on("select2:unselect", function (evt) {
                    if (!evt.params.originalEvent) { return; }
                    evt.params.originalEvent.stopPropagation();
                });
                //Check seleccionar todas
                $("#chkallAuto").click(function () {
                    if ($("#chkallAuto").is(':checked')) {
                        $("#cboObraPresupuestoAuto").val(selectedItems).trigger("change").trigger("select2:close");
                    } else {
                        $("#cboObraPresupuestoAuto").val('').trigger("change").trigger("select2:close");
                        $("#numComponentesTotalAuto").text(0);
                        $("#numMaquinasTotalAuto").text(0);
                        $("#numTotalPresupuestoAuto").text("$0.00");
                    }
                });
                obrasPresupuesto = $("#cboObraPresupuestoAuto").val();
                //CargarTablaPresupuestoAuto(obrasPresupuesto, modelo, anio);


                
                tblPresAutorizacion.columns.adjust();
            }
        }
        function CargarTablaPresupuestoAuto(obrasID, modelo, anio)
        {
            var firstElement = $('#tblPresAutorizacion tbody > tr').first().nextAll('tr');
            tblPresAutorizacion.rows(firstElement).remove().draw();
            $.blockUI({
                message: mensajes.PROCESANDO,
                baseZ: 2000
            });
            $.ajax({
                url: '/Overhaul/CargarTblPresupuesto',
                datatype: "json",
                type: "POST",
                data: {
                    anio: anio,
                    modeloID: modelo,
                    obras: obrasID
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        presupuestoID = response.presupuestoID;
                        var presupuesto = response.data;
                        var numComponentes = 0;
                        var totalPresupuesto = 0;
                        for(let i = 0; i < presupuesto.length; i++)
                        {
                            numComponentes += presupuesto[i].maquinasComponentes.length;
                            var htmlObrasNombre = "";
                            var htmlObrasValor = "";
                            var htmlObrasCosto = "";
                            var indicadoresVida = "";
                            var cantidadVida = "";
                            var costoVida = "";
                            for (let j = 0; j < presupuesto[i].obras.length; j++)
                            {
                                htmlObrasValor += '<span><hr></span>' +
                                                    '<div style="height:30px;">' +
                                                    '<span>' + presupuesto[i].obras[j].Valor + '</span>' +
                                                    '</div>';
                                                  
                                htmlObrasNombre += '<span><hr></span>' +
                                                    '<button data-esServicio ="' + presupuesto[i].esServicio + '" data-index="' + presupuesto[i].obras[j].Propiedad + '" class="btn btn-xs btn-warning btn-obra btn-accion-auto" style="border:0;height:30px" data-esServicio ="' + presupuesto[i].esServicio + '" data-anio="' + anio + '" data-subconjunto="' +
                                                    presupuesto[i].subconjuntoID + '" ' + (presupuesto[i].obras[j].Autorizado ? 'disabled' : '') + '># ' + presupuesto[i].obras[j].Nombre.toUpperCase() + '</button>';

                                htmlObrasCosto += '<span><hr></span>' +
                                                  '<div style="height:30px;text-align:right;font-size:12px;font-weight:bold;">' +
                                                  '<span>$ ' + presupuesto[i].obras[j].Costo.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</span>' +
                                                  '</div>';
                            }
                            for (let j = 0; j < presupuesto[i].vida.length; j++) {
                                indicadoresVida += (presupuesto[i].vida[j] > 0 ?
                                    ('<hr>' +
                                    '<button data-index="' + presupuesto[i].obras.map(function (x) { return x.Propiedad }) + '" data-esServicio ="' + presupuesto[i].esServicio + '" data-tipo="' + j + '" data-anio="' + anio + '" data-subconjunto="' + presupuesto[i].subconjuntoID + '" class="btn btn-xs bg-danger btn-vida btn-accion-auto" style="border:0;height:30px">VIDA ' + j + '</button>') : "");

                                cantidadVida += (presupuesto[i].vida[j] > 0 ?
                                    ('<hr>' +
                                    '<div style="height:30px;">' +
                                    '<span>' + presupuesto[i].vida[j] + '</span>' +
                                    '</div>') : "");
                                costoVida += (presupuesto[i].vida[j] > 0 ?
                                    ('<hr>' +
                                    '<div style="height:30px;text-align:right;font-size:12px;font-weight:bold;">' +
                                    //'<span>$ ' + "0" + '</span>' +
                                    '<span>$ ' + presupuesto[i].costoVida[j].toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</span>' +
                                    '</div>') : "");
                            }
                            var agregar = {
                                "descripcion": presupuesto[i].subconjunto,
                                "id": presupuesto[i].subconjuntoID,
                                "indicador":
                                    '<button data-index="' + presupuesto[i].obras.map(function (x) { return x.Propiedad }) + '" data-esServicio ="' + presupuesto[i].esServicio + '" data-anio="' + anio + '" data-subconjunto="' + presupuesto[i].subconjuntoID + '" class="btn btn-xs btn-success btn-todos btn-accion-auto" style="border:0;height:30px"># A PRESUPUESTAR</button>' +
                                    htmlObrasNombre + indicadoresVida,
                                "cantidad":
                                    '<div style="height:30px;">' +
                                    '<span>' + presupuesto[i].maquinasComponentes.length + '</span>' +
                                    '</div>' +
                                    htmlObrasValor + cantidadVida,
                                "costo":
                                    '<div style="height:30px;text-align:right;font-size:12px;font-weight:bold;">' +
                                    '<span>$ ' + presupuesto[i].costoTotal.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</span>' +
                                    '</div>' +
                                    htmlObrasCosto + costoVida,
                                "desgloce": ""
                            };
                            totalPresupuesto += presupuesto[i].costoTotal;
                            tblPresAutorizacion.row.add(agregar);
                        }
                        tblPresAutorizacion.draw(false);

                        //numMaquinas = $.map(numMaquinas, function (numMaquinas) {
                        //    return numMaquinas.locacion;
                        //});
                        $("#numComponentesTotalAuto").text(numComponentes);
                        $("#numMaquinasTotalAuto").text(unico(response.maquinas).length);
                        $("#numTotalPresupuestoAuto").text("$" + totalPresupuesto.toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    }
                },
                error: function (response) {
                    console.log(response);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }

        //Avance
        function AutorizarPresupuesto() {
            var tipoActual = btnModalAceptar.attr("data-tipo");
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/AutorizarPresupuesto',
                datatype: "json",
                type: "POST",
                data: {
                    presupuestoID: btnModalAceptar.attr("data-index"),
                    obra: btnModalAceptar.attr("data-obra"),
                    anio: btnModalAceptar.attr("data-anio"),
                    modelo: btnModalAceptar.attr("data-modelo"),
                    tipo: btnModalAceptar.attr("data-tipo")
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if (response.exito)
                        {
                            modalAutorizacion.modal("hide");
                            AlertaGeneral("Éxito", "Se ha realizado la operación con éxito");
                            CargartblAutorizacion();
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }

        function IniciartblAvance() {
            dtAvance = tblAvance.DataTable({
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '35vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },

                "createdRow": function( row, data, dataIndex ) {
                    $(row).addClass( 'rowPadre' );                    
                },
                columns: [

                    { data: 'modelo', title: 'Modelo' },
                    // { data: 'anio', title: 'Año' },
                    //{ data: 'obra', title: 'Obra' },
                    {
                        data: 'costo',
                        render: $.fn.dataTable.render.number(',', '.', 2),
                        title: 'Presupuesto USD',
                    },
                    {
                        data: 'presupuesto',
                        render: $.fn.dataTable.render.number(',', '.', 2),
                        title: 'Erogado USD ',
                       
                    },
                    
                    {
                        title:'Detalle',
                        render: function (data, type, row) {
                            return `<button class='btn-editar btn btn-warning verDetalleAvance' data-toggle="modal" data-target="#mdlCrearAgrupacion" data-id="${row.id}">` +
                                `<i class='fas fa-pencil-alt'></i>` +
                                `</button>&nbsp;`;
                        }
                    }                  
                ],
                initComplete: function (settings, json) {
                    tblAvance.on("click", ".verDetalleAvance", function () {
                        const rowData = dtAvance.row($(this).closest("tr")).data();
                        CargarAvanceDetallle(rowData.presupuestoID);
                });
                }
             
            }).columns.adjust();
        }

        function cargarDetallesAvance(presupuestoID, obraID)
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarTblModalAutorizacion',
                datatype: "json",
                async: false,
                type: "POST",
                data: {
                    presupuestoID: presupuestoID,
                    obra: obraID,
                    subconjunto: cboSubconjuntoAvance.val()
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        respuesta = response.data;
                        tblAvance.columns.adjust();
                    }
                }
            });            
        }

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

        function CargartblAvance() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarTblAvance',
                datatype: "json",
                type: "POST",
                data: {
                    obras: cboCCAvance.val(),
                    anio: cboAnioAvance.val(),
                    modeloID: cboModeloAvance.val(),
                    estatus: cboEstatusAvance.val()
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

        //Tabla Reporte Inversion

        function initTblInversion() {
            var labelsEspeciales = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];
            dttblInversion = tblInversion.DataTable({
                language: dtDicEsp,
                destroy: true,
                autoWidth: true,
                searching: false, paging: false, info: false,
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
                    switch (data.numTipoParo) {
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
                        default:
                            break;
                    }
                    if (data.paroTerminado) {
                        $('td', row).eq(0).css('background-color', '#696969');
                        $('td', row).eq(0).css('color', 'white');
                        //$('td', row).eq(6).css('background-color', '#696969');
                        //$('td', row).eq(6).css('color', 'white');
                    }
                    if (data.fechaRemocion != '--') {
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
                "footerCallback": function (row, data, start, end, display) {
                    var api = this.api(), data;

                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                            i : 0;
                    };

                    // Total over all pages
                    total = api
                        .column(8)
                        .data()
                        .reduce(function (a, b) {
                            return intVal(a) + intVal(b);
                        }, 0);


                    // Update footer
                    $(api.column(3).footer()).html(
                        '$' + parseFloat(total).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + ' total'
                    );
                }
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
            $.blockUI({ message: mensajes.PROCESANDO });

            $.ajax({
                url: "/Overhaul/cargarTblInversionObra",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                //async: false,
                data: JSON.stringify({
                    modelo: cboModeloPresupuesto.val(),
                    obra: cboObraInversion.val(),
                    anio: cboAnioPresupuesto.val()
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

        const CargarAvanceDetallle = function (idDetalle){
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarTblAvanceDetalle',
                datatype: "json",
                type: "POST",
                data: {
                    idDetalle:idDetalle
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        dtAvanceDetalle.clear();
                        dtAvanceDetalle.rows.add(response.data);
                        dtAvanceDetalle.draw(false);
                        dtAvanceDetalle.columns.adjust();
                    }
                }
            });
        }

        const IniciartblAvanceDetalle = function () {
            dtAvanceDetalle = tblAvanceDetalle.DataTable({
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '35vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },

                "createdRow": function( row, data, dataIndex ) {
                    $(row).addClass( 'rowPadre' );                    
                },
                columns: [

                    { data: 'componenteID', title: 'No Serie' },
                    { data: 'maquinaID', title: 'Económico' },
                    // { data: 'costoSugerido', title: 'costoSugerido' },
                    { data: 'costoPresupuesto', title: 'Costo Presupuestado' },
                    { data: 'horasCiclo', title: 'Horas Ciclo' },
                    { data: 'horasAcumuladas', title: 'Horas Acumuladas' },
                    // { data: 'presupuestoID', title: 'presupuestoID' },
                    // { data: 'estado', title: 'estado' },
                    // { data: 'subconjuntoID', title: 'subconjuntoID' },
                    { data: 'obra', title: 'Obra' },
                    // { data: 'vida', title: 'vida' },
                    { data: 'costoReal', title: 'Costo Real' },
                    { data: 'fecha', title: 'Fecha',
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    // { data: 'tipo', title: 'tipo' },
                    // { data: 'comentarioAumento', title: 'comentarioAumento' },
                    // { data: 'programado', title: 'programado' },
                    // { data: 'esServicio', title: 'esServicio' }
                      
                ],
          
         
            }).columns.adjust();
        }


        const CargarAvanceGeneral = function (){
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarAvanceGeneral',
                datatype: "json",
                type: "POST",
                data: {
                    obras: cboCCAvance.val(),
                    anio: cboAnioAvance.val(),
                    modeloID: cboModeloAvance.val(),
                    estatus: cboEstatusAvance.val()
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        dtAvanceGeneral.clear();
                        dtAvanceGeneral.rows.add(response.data);
                        dtAvanceGeneral.draw(false);
                        dtAvanceGeneral.columns.adjust();

                        //dtAvanceGeneralAtrasado.clear();
                        //dtAvanceGeneralAtrasado.rows.add(response.dataAtrasados);
                        //dtAvanceGeneralAtrasado.draw(false);
                        //dtAvanceGeneralAtrasado.columns.adjust();
                    }
                }
            });
        }

        const CargarReporteAvanceGeneral = function ()
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarReporteAvanceGeneral',
                datatype: "json",
                type: "POST",
                data: {
                    obras: cboCCAvance.val(),
                    anio: cboAnioAvance.val(),
                    modeloID: cboModeloAvance.val(),
                    estatus: cboEstatusAvance.val()
                },
                success: function (response) {
                    ireporteAvance.attr("src", "/Reportes/Vista.aspx?idReporte=210");
                    $(window).scrollTop(0);
                    $("#reporteAvance > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteAvance > #reportViewerModal").css("width", "100%");
                    $("#reporteAvance > #reportViewerModal").css("height", "105%");
                    $("#reporteAvance > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        const IniciartblAvanceGeneral = function () {
            dtAvanceGeneral = tblAvanceGeneral.DataTable({
                retrieve: true,
                searching: false,
                paging: false,
                scrollY: '35vh',
                scrollCollapse: true,
                autoWidth: false,
                select: {
                    style: 'os',
                    selector: 'td:first-child'
                },

                "createdRow": function( row, data, dataIndex ) {
                    $(row).addClass( 'rowPadre' );                    
                },
                columns: [

          
                    { data: 'Descripcion', title: 'Descripcion' },
                    { data: 'obra', title: 'Obra' },
                    { data: 'cc', title: 'Centro de costo' },
                    { data: 'presupuesto', title: 'Presupuesto', render: $.fn.dataTable.render.number(',', '.', 2)},
                    { data: 'avance', title: 'Avance Presupuesto', render: $.fn.dataTable.render.number(',', '.', 2)},
                    { data: 'avanceErogado', title: 'Avance Erogado', render: $.fn.dataTable.render.number(',', '.', 2)},
                    { data: 'bolsaRestante', title: 'Bolsa Restante', render: $.fn.dataTable.render.number(',', '.', 2)},
                
                ],
          
         
            }).columns.adjust();
        }

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

        init();
    };

    $(document).ready(function () {
        maquinaria.overhaul.presupuesto = new presupuesto();
    });
})();


