(function () {

    $.namespace('Maquinaria.Rentabilidad.Analisis');

    Analisis = function () {
        const modalAnalisis = $("#modalAnalisis");

        const comboACAnalisis = $('#comboACAnalisis');
        const comboTipoAnalisis = $('#comboTipoAnalisis');
        const comboGrupo = $('#comboGrupo');
        const comboModelo = $('#comboModelo');
        const comboCC = $('#comboCC');
        const inputDiaInicialAnalisis = $('#inputDiaInicialAnalisis');
        const inputDiaFinalAnalisis = $('#inputDiaFinalAnalisis');
        const botonBuscarAnalisis = $('#botonBuscarAnalisis');

        //// Tabla Analisis
        const divTablaAnalisis = $("#divTablaAnalisis");
        const tablaAnalisis = $('#tablaAnalisis');
        let dtTablaAnalisis;

        const modalDetalles = $('#modalDetalles');
        const divTablasDetalle = $("#divTablasDetalle");

        ////Tabla Nivel Cero
        const divTablaNivelCero = $('#divTablaNivelCero');
        const tablaSctaDetalles = $('#tablaSctaDetalles');
        let dtTablaSctaDetalles;        
        const botonNombreNivelCero = $("#botonNombreNivelCero");

        ////Tabla Nivel Uno
        const divTablaNivelUno = $('#divTablaNivelUno');
        const tablaDetalles = $('#tablaDetalles');
        let dtTablaDetalles;
        const botonNombreNivelUno = $("#botonNombreNivelUno");        

        ////Tabla Nivel Dos
        const divTablaNivelDos = $('#divTablaNivelDos');
        const tablaSubdetalles = $('#tablaSubdetalles');
        let dtTablaSubdetalles;        
        const botonNombreNivelDos = $("#botonNombreNivelDos");

        //Grafica
        const divGrafica = $("#divGrafica");
        const divGraficaDetalle = $("#divGraficaDetalle");
        const graficaLineas = $("#graficaLineas");
        let grGrafica;

        let fechaMaxAnalisis = new Date();
        let dateMax = new Date();
        let columnaAnalisis = 0;

        const getLstAnalisis = new URL(window.location.origin + '/Rentabilidad/getLstAnalisis');
        const getLstKubrixDetalle = new URL(window.location.origin + '/Rentabilidad/getLstKubrixDetalle');

        function init() {
            //setWeekPickers();
            setVisibles();
            $("#modalGrafica").draggable({
                stop: function () {
                    var l = (100 * parseFloat($(this).css("left")) / parseFloat($(this).parent().css("width"))) + "%";
                    var t = (100 * parseFloat($(this).css("top")) / parseFloat($(this).parent().css("height"))) + "%";
                    $(this).css("left", l);
                    $(this).css("top", t);
                }
            });
            fillCombos();
            agregarListeners();

            initTablaSctaDetalles();
            initTablaDetalles();
            initTablaSubdetalles();
        }

        function agregarListeners()
        {
            comboACAnalisis.change(cargarMaquinas);
            comboTipoAnalisis.change(cargarGrupos);
            comboGrupo.change(cargarModelos)
            comboModelo.change(cargarMaquinas);
            botonBuscarAnalisis.click(setLstAnalisis);

            modalDetalles.on("hide.bs.modal", function() {
                dtTablaSctaDetalles.clear().draw();
                dtTablaDetalles.clear().draw();
                dtTablaSubdetalles.clear().draw();
                divTablaNivelCero.show();
                botonNombreNivelCero.show();
                divTablaNivelUno.hide();
                botonNombreNivelUno.hide();
                divTablaNivelDos.hide();
                botonNombreNivelDos.hide();
            });
            botonNombreNivelUno.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaNivelUno.is(":visible")) {
                divTablaNivelDos.hide(500);
                    divTablaNivelUno.show(500);
                    botonNombreNivelDos.hide();
                }                 
            });
            botonNombreNivelCero.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaNivelCero.is(":visible")) {
                    divTablaNivelUno.hide(500);
                    divTablaNivelDos.hide(500);
                    divTablaNivelCero.show(500);
                    botonNombreNivelUno.hide();
                    botonNombreNivelDos.hide();
                }                 
            });
            modalAnalisis.on('shown.bs.modal', function (e) {
                dtTablaAnalisis.columns.adjust();                
            });
            //$("#btnMover").click(function(e){
            //    e.preventDefault();
            //    e.stopImmediatePropagation();
            //    if(divTablaAnalisis.is(":visible")){
            //        $(this).html('<i class="glyphicon glyphicon-chevron-right"></i>');
            //        divTablasDetalle.removeClass('col-md-5');
            //        divTablasDetalle.removeClass('col-lg-5');
            //        divTablasDetalle.addClass('col-md-6');
            //        divTablasDetalle.addClass('col-lg-6');
            //        dtTablaSctaDetalles.column(3).visible(true);
            //        dtTablaDetalles.column(4).visible(true);
            //        divGraficaDetalle.show();
            //    }
            //    else {
            //        $(this).html('<i class="glyphicon glyphicon-chevron-left"></i>');
            //        divTablasDetalle.addClass('col-md-5');
            //        divTablasDetalle.addClass('col-lg-5');
            //        divTablasDetalle.removeClass('col-md-6');
            //        divTablasDetalle.removeClass('col-lg-6');
            //        dtTablaSctaDetalles.column(3).visible(false);
            //        dtTablaDetalles.column(4).visible(false);
            //        divGraficaDetalle.hide();
            //    }
            //    divTablaAnalisis.animate({width:'toggle'},350);
            //});
            $("#botonCerrarGrafica").click(function(e){
                $("#modalGrafica").modal("hide");
            });
        }

        function setWeekPickers()
        {
            inputDiaInicialAnalisis.val(moment().day(3).locale('es').format("D MMMM YYYY"));
            inputDiaFinalAnalisis.val(moment().add(7, 'days').day(2).locale('es').format("D MMMM YYYY"));
            $(".weekPicker").datepicker({
                firstDay: 3,
                todayHighlight: true,
                dateFormat: "d mm yy",
                onSelect: function (dateText) {
                    var date = moment(dateText, "D MM YYYY");
                    if (date.day() >= 3) {  
                        var firstDate = date.day(3).locale('es').format("D MMMM YYYY");
                        var lastDate = date.add(7, 'days').day(2).locale('es').format("D MMMM YYYY");
                    }
                    else {
                        var firstDate = date.add(-7, 'days').day(3).locale('es').format("D MMMM YYYY");
                        var lastDate = date.add(7, 'days').day(2).locale('es').format("D MMMM YYYY");
                    }
                    var lastDate = date.day(2).locale('es').format("D MMMM YYYY");
                    inputDiaInicialAnalisis.val(firstDate);
                    inputDiaFinalAnalisis.val(lastDate);
                },
            });
        }

        function setVisibles()
        {
            divTablasDetalle.hide();
            divGrafica.show();
            divTablaNivelCero.show(); 
            divTablaNivelUno.hide(); 
            botonNombreNivelUno.hide();
            divTablaNivelDos.hide();
            botonNombreNivelDos.hide();  
            divGraficaDetalle.hide();
        }

        function fillCombos() {
            comboACAnalisis.fillCombo('cboObra', null, false, "TODOS");
            comboTipoAnalisis.fillCombo('cboTipo', null, false, "TODOS");
            comboGrupo.multiselect();
            comboGrupo.multiselect('disable');

            comboModelo.multiselect();
            comboModelo.multiselect('disable');

            comboCC.multiselect();
            comboCC.multiselect('disable');
        }

        function cargarMaquinas() {
            let busq = getBusquedaDTO();
            //$.blockUI({ message: 'Procesando...' });
            comboCC.fillComboAsync('cboMaquina', { busq: busq }, false, 'Todos', function() { 
                comboCC.multiselect('destroy'); 
                convertToMultiselect('#comboCC'); 
                //$.unblockUI(); 
            });
        }

        function cargarGrupos() {
            const busq = getBusquedaDTO();
            if (busq.tipo == 0) {
                comboGrupo.multiselect('deselectAll');
                comboGrupo.multiselect('disable');
                comboModelo.multiselect('deselectAll');
                comboModelo.multiselect('disable');
            } else {
                //$.blockUI({ message: 'Procesando...' });
                comboGrupo.fillComboAsync('cboGrupo', { busq: busq }, false, 'Todos', function() {
                    comboGrupo.multiselect('destroy');
                    convertToMultiselect('#comboGrupo');
                    //$.unblockUI();
                });
            }
        }

        function cargarModelos() {
            let busq = getBusquedaDTO();

            //$.blockUI({ message: 'Procesando...' });
            comboModelo.fillComboAsync('cboModelo', { busq: busq }, false, 'Todos', function() {
                comboModelo.multiselect('destroy');
                convertToMultiselect('#comboModelo');
                //$.unblockUI();
            });
        }

        function getBusquedaDTO() {
            return {
                obra: comboACAnalisis.val()
                , tipo: $("#botonNombreNivelCero").attr("data-filtro")
                , lstGrupo: getValoresMultiples('#comboGrupo')
                , lstModelo: getValoresMultiples('#comboModelo')
                , lstMaquina: getValoresMultiples('#comboCC')
                , fecha: inputDiaFinalAnalisis.val()
                //, max: inputMesFinal.val()
            };
        }

        function setLstAnalisis() {
            let busq = getBusquedaDTO();
            if (busq.tipoReporte == 0) {
                AlertaGeneral('Aviso', 'Debe seleccionar el tipo de Rentabilidad');
                return;
            }
            $.post(getLstAnalisis, { busq: busq })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        auxDatos = response.lst;
                        if(busq.tipo == 1) {
                            cargarDatosTablaAnalisis(response.lst, 1);
                        }
                        else {
                            cargarDatosTablaAnalisis(response.lst, 0);
                        }
                        fechaMaxAnalisis = moment(response.fecha).toDate();
                        } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.');
                }
            );
        }

        function setLstKubrixDetNivelUno(busq, nombreColumna, total) {
            $.post(getLstKubrixDetalle, { busq: busq })
                .then(function(response) {
                    if (response.success) {
                        // Operación exitosa.
                        var detalles = response.lst;
                        var sumadetalles = 0;
                        for(var i = 0; i < detalles.length; i++)
                        {
                            sumadetalles += detalles[i].importe;
                        }
                        cargarDetScta(detalles, nombreColumna, total);
                    } else {
                        // Operación no completada.
                        AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                    AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".");
                }
            );
        }
        function setLstKubrixDetNivelUnoFiltrado(busq, nombreColumna, total) {
            $.post(getLstKubrixDetalle, { busq: busq })
                .then(function(response) {
                    if (response.success) {
                        // Operación exitosa.
                        var detalles = response.lst;
                        var sumadetalles = 0;
                        for(var i = 0; i < detalles.length; i++)
                        {
                            sumadetalles += detalles[i].importe;
                        }
                        cargarDetSctaFiltrado(detalles, nombreColumna, total);
                    } else {
                        // Operación no completada.
                        AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                    AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".");
                }
            );
        }

        function cargarDatosTablaAnalisis(data, tipo) {
            setVisibles();
            if (dtTablaAnalisis != null) {
                dtTablaAnalisis.destroy();
                tablaAnalisis.empty();
                tablaAnalisis.append('<thead class="bg-table-header"></thead>');
            }
            var detallesData = $.map(data, function( n ) {
                return n.detalles;
            });
            dtTablaAnalisis = tablaAnalisis.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                dom: '<<"col-xs-12 col-sm-12 col-md-6 col-lg-6 chkGrafica">f<t>>',
                destroy: true,
                scrollCollapse: true,
                autoWidth: false,
                scrollX: true,
                ordering: false,
                bProcessing: true,
                paging: false,
                columns: getAnalisisColumns(tipo),
                data: data,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { visible: false, "targets": 0 },
                ],
                order: [[0, 'asc']],
                select: {
                    style: 'single'
                },
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaAnalisis_filter input').addClass("form-control input-sm");
                },
                drawCallback: function(settings) {       
                    CargarGraficaLineas(data, tipo);
                    tablaAnalisis.find('p.desplegable').unbind().click(function () {                        
                        const p = $(this);
                        const rowData = dtTablaAnalisis.row(p.parents('tr')).data();
                        const td = p.parents("td");
                        columnaAnalisis = td.index();
                        const nombreColumna = rowData.descripcion.trim();

                        cargarSubCuenta(rowData.detalles, nombreColumna, p.html(), td.index(), tipo);
                        dtTablaSctaDetalles.columns.adjust();
                        botonNombreNivelCero.click();
                    });
                    $('#tablaAnalisis tbody').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var auxData = dtTablaAnalisis.row(this).data();
                        var seriesLength = grGrafica.series.length;
                        for(var i = seriesLength - 1; i > -1; i--) {
                            if(grGrafica.series[i].name != auxData.descripcion) { grGrafica.series[i].hide(); }
                            else { grGrafica.series[i].show(); }
                        }
                    });
                    $('#tablaAnalisis thead').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var seriesLength = grGrafica.series.length;
                        for(var i = seriesLength - 1; i > -1; i--) { grGrafica.series[i].show(); }
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( data.tipo_mov == 4 || data.tipo_mov == 6 || data.tipo_mov == 8 || data.tipo_mov == 10 || data.tipo_mov == 12) { $(row).addClass('resultado'); }
                }
            });
            $("div.chkGrafica").html('<input type="checkbox" checked data-toggle="toggle" data-on="Detalles" data-off="Gráfica" data-onstyle="success" data-offstyle="info" id="chbGrafica">&nbsp;&nbsp;&nbsp;<input type="checkbox" ' + (tipo == 0 ? "checked" : "") + ' data-toggle="toggle" data-on="80-20" data-off="Semanal" data-onstyle="success" data-offstyle="info" id="chb8020" hidden>');
            
            $('div.chkGrafica input').bootstrapToggle();
            /*if(tipo == 1)
            {
                $('div.chkGrafica #chb8020').checked = false;
            }*/
            $('div.chkGrafica #chbGrafica').change(function(){
                if($(this).is(":checked"))
                {
                    divGrafica.show();
                    divTablasDetalle.hide();
                }
                else
                {
                    divGrafica.hide();
                    divTablasDetalle.show();
                }
            });
            $('div.chkGrafica #chb8020').change(function() {
                var datosTabla = dtTablaAnalisis.rows().data();
                if($(this).is(":checked"))
                {                    
                    cargarDatosTablaAnalisis(datosTabla, 0);
                }
                else
                {
                    cargarDatosTablaAnalisis(datosTabla, 1);
                }
            });
            $('[data-toggle="tooltip"]').tooltip();
            modalAnalisis.modal("show");
            dtTablaAnalisis.columns.adjust();
        }

        function getAnalisisColumns(tipo) {
            if(tipo == 0){
                return [
                    { data: 'tipo_mov', title: 'Descripción' }
                    , { data: 'descripcion', title: 'Descripción' }
                    , { data: 'actual', title: '<span data-toggle="tooltip" title="Actual" data-placement="bottom">Actual</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'semana2', title: '<span data-toggle="tooltip" title="Semana 2" data-placement="bottom">Semana 2</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'semana3', title: '<span data-toggle="tooltip" title="Semana 3" data-placement="bottom">Semana 3</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'semana4', title: '<span data-toggle="tooltip" title="Semana 4" data-placement="bottom">Semana 4</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'semana5', title: '<span data-toggle="tooltip" title="Semana 5" data-placement="bottom">Semana 5</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                ];
            }
            else
            {
                return [
                    { data: 'tipo_mov', title: 'Descripción' }
                    , { data: 'descripcion', title: 'Descripción' }
                    , { data: 'actual', title: '<span data-toggle="tooltip" title="Actual" data-placement="bottom">Total</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'cfc', title: '<span data-toggle="tooltip" title="CFC" data-placement="bottom">CFC</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'cf', title: '<span data-toggle="tooltip" title="CF" data-placement="bottom">CF</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'mc', title: '<span data-toggle="tooltip" title="MC" data-placement="bottom">MC</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'pr', title: '<span data-toggle="tooltip" title="PR" data-placement="bottom">PR</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'tc', title: '<span data-toggle="tooltip" title="TC" data-placement="bottom">TC</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'car', title: '<span data-toggle="tooltip" title="CAR" data-placement="bottom">CAR</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'otros', title: '<span data-toggle="tooltip" title="Otros" data-placement="bottom">Otros</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 6 || row.tipo_mov == 8 || row.tipo_mov == 10 || row.tipo_mov == 12)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 13)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                ];
            }
        }

        function getRowHTML(value) {
            var auxiliar = '<p' + (value != 0 ? ' class="desplegable">' : '>') + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
        }

        function getNumberHTML(value) {
            return '<p class="' + (value != 0 ? 'noDesplegable' : '') + (value < 0 ? ' Danger' : '') + '" >' + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
        }

        function getRowHTMLFiltrado(value) {
            var auxiliar = '<p' + (value != 0 ? ' class="filtrado">' : '>') + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
        }


        

        function initTablaSctaDetalles() {

            dtTablaSctaDetalles = tablaSctaDetalles.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                dom: '<"col-xs-12 col-sm-12 col-md-6 col-lg-6 inputTitulo"><f<t>>',
                columns: [
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta', visible: false },
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'importe', title: 'Semana', render: function (data, type, row) { return getRowHTMLFiltrado(data); } },
                    { data: 'acumulado', title: 'Acumulado', render: function (data, type, row) { return getRowHTML(data); } },
                    { 
                        data: 'porcentaje', 
                        title: '%',
                        render: function (data, type, row) { 
                            return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                        }   
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[0, 'asc'], [1, 'asc']],
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaSctaDetalles_filter input').addClass("form-control input-sm");
                },
                drawCallback: function () {                    
                    tablaSctaDetalles.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaSctaDetalles.row($(this).parents('tr')).data();
                        const listaMaquinas = rowData.detalles.map(function(x) { return x.noEco; });
                        var auxFecha = $("#inputDiaFinalAnalisis").datepicker('getDate');
                        switch($("#tituloSctaDetalles").text())
                        {
                            case "Semana 2":
                                auxFecha.setDate(auxFecha.getDate() - 7);
                                break;
                            case "Semana 3":
                                auxFecha.setDate(auxFecha.getDate() - 14);
                                break;
                            case "Semana 4":
                                auxFecha.setDate(auxFecha.getDate() - 21);
                                break;
                            case "Semana 5":
                                auxFecha.setDate(auxFecha.getDate() - 28);
                                break;
                        }
                        var busq = {
                            obra: $("#comboACAnalisis").val()
                            , tipo: botonNombreNivelCero.attr("data-filtro")
                            , lstGrupo: getValoresMultiples('#comboGrupo')
                            , lstModelo: getValoresMultiples('#comboModelo')
                            , lstMaquina: listaMaquinas
                            , min: new Date()
                            , max: auxFecha.toLocaleDateString().Capitalize()
                            , cta: rowData.cta
                            , scta: rowData.scta
                            , tm: [0]
                            ,// tipoReporte: tipoReporte
                        }

                        setLstKubrixDetNivelUno(busq, rowData.descripcion, $(this).html());
                        botonNombreNivelUno.show();
                    });
                    tablaSctaDetalles.find('p.filtrado').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaSctaDetalles.row($(this).parents('tr')).data();
                        var busq = {
                            obra: $("#comboACAnalisis").val()
                            , tipo: botonNombreNivelCero.attr("data-filtro")
                            , lstGrupo: getValoresMultiples('#comboGrupo')
                            , lstModelo: getValoresMultiples('#comboModelo')
                            , lstMaquina: getValoresMultiples('#comboCC')
                            , min: new Date()
                            , max: $("#inputDiaFinalAnalisis").val()
                            , cta: rowData.cta
                            , scta: rowData.scta
                            , tm: [0]
                            ,// tipoReporte: tipoReporte
                        }

                        setLstKubrixDetNivelUnoFiltrado(busq, rowData.descripcion, $(this).html());
                        botonNombreNivelUno.show();

                        //e.preventDefault();
                        //e.stopPropagation();
                        //e.stopImmediatePropagation();                        
                        //const rowData = dtTablaSctaDetalles.row($(this).parents('tr')).data();
                        //cargarDetSctaFiltrado(rowData.detalles, rowData.descripcion, $(this).html());
                        //botonNombreNivelUno.show();                        
                    });
                    tablaSctaDetalles.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();

                    });
                    
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = (api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalAcumulado = (api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalPorcentaje = api.column(7).data().reduce( function (a, b) { return intVal(parseFloat(a, 10)) + intVal(parseFloat(b, 10)); }, 0 );
                    $(api.column(4).footer()).html('TOTAL');
                    $(api.column(5).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(6).footer()).html('$' + parseFloat(totalAcumulado).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(7).footer()).html(parseFloat(totalPorcentaje).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '%');
                }
            });
            $("div.inputTitulo").html('<span id="tituloSctaDetalles">' +  + '</span>');
        }

        function initTablaDetalles() {
            dtTablaDetalles = tablaDetalles.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    //{ data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'sscta', title: 'SubSubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta', visible: false },
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'importe', title: 'Importe', render: (data, type, row) => getRowHTML(data) }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function () {
                    tablaDetalles.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaDetalles.row($(this).parents('tr')).data();

                        mostrarSubdetalles(rowData.detalles, rowData.descripcion, rowData.importe);
                        botonNombreNivelDos.show();
                    });
                    tablaDetalles.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaDetalles.row($(this).parents('tr')).data();

                        var datosGrafica = [];
                        var subdetalles = rowData.detalles.map(function(x) {
                            return {
                                fechaRaw: moment(x.fecha).toDate(),
                                fecha: moment(x.fecha).toDate().toLocaleDateString().Capitalize(),
                                descripcion: x.insumo_Desc,
                                importe: x.importe
                            };
                        });

                        dateMax = fechaMaxAnalisis;
                        var suma = 0;
                        for(var i = 0; i < 5; i++) {
                            suma = 0;
                            auxGrafica = jQuery.grep(subdetalles, function( n, i ) {
                                return n.fechaRaw <= dateMax && n.fechaRaw > dateMax.setDate(dateMax.getDate() - 7);
                            });
                            auxGrafica.forEach(function(n) { suma += parseFloat(n.importe) || 0; });
                            datosGrafica.push(suma);
                        }
                        datosGrafica.reverse();
                        CargarGraficaLineasAnalisis(datosGrafica);
                        $("#modalGrafica").modal("show");
                    });

                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = (api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    $(api.column(5).footer()).html('TOTAL');
                    $(api.column(6).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                }
            });
        }
        
        function initTablaSubdetalles() {

            dtTablaSubdetalles = tablaSubdetalles.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    { data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'importe', title: 'Importe', render: function (data, type, row) { return '<p>' + maskNumero(data) + '</p>'; } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html('$' + parseFloat(total).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                }
            });
        }

        function groupBy(list, keyGetter) {
            const map = new Map();
            list.forEach(function(item) {
                const key = keyGetter(item);
                const collection = map.get(key);
                if (!collection) {
                    map.set(key, [item]);
                } else {
                    collection.push(item);
                }
            });
            return map;
        }

        function dateFormat(dateObject) {
            var d = new Date(dateObject);
            var day = d.getDate();
            var month = d.getMonth() + 1;
            var year = d.getFullYear();
            if (day < 10) {
                day = "0" + day;
            }
            if (month < 10) {
                month = "0" + month;
            }
            var date = day + "/" + month + "/" + year;

            return date;
        };

        function cargarSubCuenta(detalles, nombreColumna, total, columna, tipo) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral('Aviso', 'Ocurrió un error al ver los detalles de este registros.');
                return;
            }
            let detallesFiltrados;
            if(tipo == 1) {         
                dtTablaSctaDetalles.column(5).visible(false);
                dateMax = new Date(fechaMaxAnalisis);
                switch (columna) {
                    case 1: // Total
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("Total");
                        break;
                    case 2: // CFC
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("CFC-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("CFC");
                        break;
                    case 3: // CF
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("CF-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("CF");
                        break;
                    case 4: // MC
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("MC-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("MC");
                        break;
                    case 5: // PR
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("PR-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("PR");
                        break;
                    case 6: // TC
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("TC-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("TC");
                        break;
                    case 7: // CAR
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("CAR-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("CAR");
                        break;
                    case 8: // OTROS
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("CFC-") == -1 && n.noEco.indexOf("CF-") == -1 && n.noEco.indexOf("MC-") == -1
                                && n.noEco.indexOf("PR-") == -1 && n.noEco.indexOf("TC-") == -1 && n.noEco.indexOf("CAR-") == -1 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("Otros");
                        break;
                    default:
                        $("#tituloSctaDetalles").text("");
                        return;
                }
                const grouped = groupBy(detallesFiltrados, detallesFiltrados => detallesFiltrados.tipoInsumo);
                const totalDetalle = detallesFiltrados.length > 0 ? detallesFiltrados.map(x => x.importe).reduce((total, importe) => total + importe) : 0;
                dtTablaSctaDetalles.clear();
                Array.from(grouped, ([key, value]) => {
                    var auxCuenta = value[0].tipoInsumo.split('-');
                    const cta = value[0].cta;
                    const scta = parseInt(auxCuenta[1]);
                    const id = value[0].tipoInsumo;
                    const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
                    const descripcion = value[0].tipoInsumo_Desc;
                    const importe = detallesFiltrados.length > 0 ? detallesFiltrados.map(x => x.importe).reduce((total, importe) => total + importe) : 0;
                    const acumulado = value.map(x => x.importe).reduce((total, importe) => total + importe);
                    const porcentajeNum = (acumulado / totalDetalle) * 100
                    const grupo = { cta: cta, scta: scta, id: id, grafica: grafica, descripcion: descripcion, importe: importe, detalles: value, acumulado: acumulado, porcentaje: porcentajeNum };
                    dtTablaSctaDetalles.row.add(grupo);
                });
                dtTablaSctaDetalles.draw();
                $("#botonNombreNivelCero strong").text(nombreColumna.toUpperCase());
            }
            else {
                dtTablaSctaDetalles.column(5).visible(true);
                dateMax = new Date(fechaMaxAnalisis);
                switch (columna) {
                    case 1: // Actual
                        $("#tituloSctaDetalles").text("Actual");
                        break;
                    case 2: // Semana 2
                        var dateMax = dateMax.setDate(dateMax.getDate() - 7);
                        $("#tituloSctaDetalles").text("Semana 2");
                        break;
                    case 3: // Semana 3
                        var dateMax = dateMax.setDate(dateMax.getDate() - 14);
                        $("#tituloSctaDetalles").text("Semana 3");
                        break;
                    case 4: // Semana 4
                        var dateMax = dateMax.setDate(dateMax.getDate() - 21);
                        $("#tituloSctaDetalles").text("Semana 4");
                        break;
                    case 5: // Semana 5
                        var dateMax = dateMax.setDate(dateMax.getDate() - 28);
                        $("#tituloSctaDetalles").text("Semana 5");
                        break;
                    default:
                        return;
                }
                var dateMin = new Date(dateMax);
                var dateMin = dateMin.setDate(dateMin.getDate() - 7);
                const grouped = groupBy(detalles, detalle => detalle.tipoInsumo);
                detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                    return new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                });
                const totalDetalle = detallesFiltrados.length > 0 ? detallesFiltrados.map(x => x.importe).reduce((total, importe) => total + importe) : 0;

                dtTablaSctaDetalles.clear();
                Array.from(grouped, ([key, value]) => {
                    detallesFiltrados = jQuery.grep(value, function( n, i ) {
                        return new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                    });
                    detallesFiltradosSemanal = jQuery.grep(value, function( n, i ) {
                        return new Date(parseInt(n.fecha.substr(6))) <= dateMax && new Date(parseInt(n.fecha.substr(6))) > dateMin;
                    });
                    var auxCuenta = value[0].tipoInsumo.split('-');
                    const cta = value[0].cta;
                    const scta = parseInt(auxCuenta[1]);
                    const id = value[0].grupoInsumo;
                    const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
                    const descripcion = value[0].tipoInsumo_Desc;
                    const importe = detallesFiltradosSemanal.length > 0 ? detallesFiltradosSemanal.map(x => x.importe).reduce((total, importe) => total + importe) : 0;
                    const acumulado = detallesFiltrados.length > 0 ? detallesFiltrados.map(x => x.importe).reduce((total, importe) => total + importe) : 0;
                    const porcentajeNum = (importe / totalDetalle) * 100
                    const grupo = { cta: cta, scta: scta, id: id, grafica: grafica, descripcion: descripcion, importe: importe, detalles: value, acumulado: acumulado, porcentaje: porcentajeNum };
                    dtTablaSctaDetalles.row.add(grupo);
                });
            }
            dtTablaSctaDetalles.draw();
            $("#botonNombreNivelCero strong").text(nombreColumna.toUpperCase());            
        }


        
        function cargarDetScta(detalles, nombreColumna, total) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral('Aviso', 'Ocurrió un error al ver los detalles de este registros.');
                return;
            }
            const grouped = groupBy(detalles, detalle => detalle.grupoInsumo);
            dtTablaDetalles.clear();
            Array.from(grouped, ([key, value]) => {
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const sscta = parseInt(auxCuenta[2]);
                const id = value[0].grupoInsumo;
                const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
                const descripcion = value[0].grupoInsumo_Desc;
                const importe = value.map(x => x.importe).reduce((total, importe) => total + importe);
                const grupo = { cta, scta, sscta, id, grafica, descripcion, importe, detalles: value };
                dtTablaDetalles.row.add(grupo);
            });
            $("#botonNombreNivelUno strong").text("ACUMULADO: " + nombreColumna.toUpperCase());
            dtTablaDetalles.draw();
            divTablaNivelCero.hide(500);
            divTablaNivelUno.show(500);
        }

        function cargarDetSctaFiltrado(detalles, nombreColumna, total) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral('Aviso', 'Ocurrió un error al ver los detalles de este registros.');
                return;
            }
            dateMax = new Date(fechaMaxAnalisis);
            switch (columnaAnalisis) {
                case 1: // Actual
                    break;
                case 2: // Semana 2
                    var dateMax = dateMax.setDate(dateMax.getDate() - 7);
                    break;
                case 3: // Semana 3
                    var dateMax = dateMax.setDate(dateMax.getDate() - 14);
                    break;
                case 4: // Semana 4
                    var dateMax = dateMax.setDate(dateMax.getDate() - 21);
                    break;
                case 5: // Semana 5
                    var dateMax = dateMax.setDate(dateMax.getDate() - 28);
                    break;
                default:
                    return;
            }
            var dateMin = new Date(dateMax);
            var dateMin = dateMin.setDate(dateMin.getDate() - 7);
            var detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                return new Date(parseInt(n.fecha.substr(6))) <= dateMax && new Date(parseInt(n.fecha.substr(6))) > dateMin;
            });
            const grouped = groupBy(detallesFiltrados, detalle => detalle.grupoInsumo);
            dtTablaDetalles.clear();
            Array.from(grouped, ([key, value]) => {
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const sscta = parseInt(auxCuenta[2]);
                const id = value[0].grupoInsumo;
                const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
                const descripcion = value[0].grupoInsumo_Desc;
                const importe = value.map(x => x.importe).reduce((total, importe) => total + importe);
                const grupo = { cta, scta, sscta, id, grafica, descripcion, importe, detalles: value };
                dtTablaDetalles.row.add(grupo);
            });
            $("#botonNombreNivelUno strong").text(nombreColumna.toUpperCase());
            dtTablaDetalles.draw();
            divTablaNivelCero.hide(500);
            divTablaNivelUno.show(500);
        }

        function mostrarSubdetalles(subdetalles, descripcion, total) {

            subdetalles = subdetalles.map(x => {
                return {
                    fecha: moment(x.fecha).toDate().toLocaleDateString().Capitalize(),
                    descripcion: x.insumo_Desc,
                    importe: x.importe
                };
            });
            $("#botonNombreNivelDos strong").text(descripcion.toUpperCase());
            dtTablaSubdetalles.clear().rows.add(subdetalles).draw();
            divTablaNivelUno.hide(500);
            divTablaNivelDos.show(500);
        }

        function CargarGraficaLineas(data, tipo)
        {
            if(tipo == 1)
            {
                var infoGrafica = [];
                for(var i = 0; i < data.length; i++)
                {
                    auxInfoGraf = { name: data[i].descripcion, data: [data[i].cfc, data[i].cf, data[i].mc, data[i].pr, data[i].tc, data[i].car, data[i].otros] };
                    infoGrafica.push(auxInfoGraf);
                }
                grGrafica = Highcharts.chart('graficaLineas', {
                    chart: {
                        height: 530,
                        type: 'line'
                    },
                    marginBottom: 100,
                    title: { text: '' },                
                    xAxis: { categories: ['CFC', 'CF', 'MC', 'PR', 'TC', 'CAR', 'Otros'] },
                    yAxis: { title: { text: '' } },
                    legend: {
                        align: 'center',
                        verticalAlign: 'bottom',
                        x: 0,
                        y: 0,
                        width:400,
                        itemWidth:200,
                        itemStyle: {
                            width:190
                        }
                    },
                    plotOptions: {
                        series: { label: { connectorAllowed: false } }
                    },
                    series: infoGrafica,
                    responsive: {
                        rules: [{
                            condition: { maxWidth: 500 },
                            chartOptions: {
                                legend: {
                                    layout: 'horizontal',
                                    align: 'center',
                                    verticalAlign: 'bottom'
                                }
                            }
                        }]
                    },
                    credits: { enabled: false }
                });
            }
            else
            {
                var infoGrafica = [];
                for(var i = 0; i < data.length; i++)
                {
                    auxInfoGraf = {name: data[i].descripcion, data: [data[i].semana5, data[i].semana4, data[i].semana3, data[i].semana2, data[i].actual]};
                    infoGrafica.push(auxInfoGraf);
                }
                grGrafica = Highcharts.chart('graficaLineas', {
                    chart: {
                        height: 530,
                        type: 'line'
                    },
                    marginBottom: 100,
                    title: { text: '' },                
                    xAxis: { categories: ['Semana 5', 'Semana 4', 'Semana 3', 'Semana 2', 'Actual'] },
                    yAxis: { title: { text: '' } },
                    legend: {
                        align: 'center',
                        verticalAlign: 'bottom',
                        x: 0,
                        y: 0,
                        width:400,
                        itemWidth:200,
                        itemStyle: {
                            width:190
                        }
                    },
                    plotOptions: {
                        series: { label: { connectorAllowed: false } }
                    },
                    series: infoGrafica,
                    responsive: {
                        rules: [{
                            condition: { maxWidth: 500 },
                            chartOptions: {
                                legend: {
                                    layout: 'horizontal',
                                    align: 'center',
                                    verticalAlign: 'bottom'
                                }
                            }
                        }]
                    },
                    credits: { enabled: false }
                });
            }
        }

        function CargarGraficaLineasAnalisis(data)
        {
            var infoGrafica = [];
            auxInfoGraf = {name: "", data: data};
            infoGrafica.push(auxInfoGraf);
            grGraficaAnalisis = Highcharts.chart('graficaLineasDetalle', {
                chart: {
                    height: 530,
                    type: 'line'
                },
                title: { text: '' },                
                xAxis: { categories: ['Semana 5', 'Semana 4', 'Semana 3', 'Semana 2', 'Actual'] },
                yAxis: { title: { text: '' } },
                legend: {
                    align: 'center',
                    verticalAlign: 'bottom',
                    x: 0,
                    y: 0,
                    width:400,
                    itemWidth:200,
                    itemStyle: {
                        width:190
                    }
                },
                plotOptions: {
                    series: { label: { connectorAllowed: false } }
                },
                series: infoGrafica,
                responsive: {
                    rules: [{
                        condition: { maxWidth: 500 },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                },
                credits: { enabled: false }
            });
        }

        init();
    };

    $(document).ready(function () {
        Maquinaria.Rentabilidad.Analisis = new Analisis();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();


