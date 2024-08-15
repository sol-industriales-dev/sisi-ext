(function () {

    $.namespace('Maquinaria.Rentabilidad.Kubrix');

    Kubrix = function () {
        const comboAC = $('#comboAC');
        const comboTipo = $('#comboTipo');
        const inputDiaFinal = $('#inputDiaFinal');
        const inputDiaInicio = $('#inputDiaInicio');
        const chbTipoReporte = $("#chbTipoReporte");
        const cbConfiguracion = $("#cbConfiguracion");
        const comboGrupoK = $("#comboGrupoK");
        const comboModeloK = $("#comboModeloK");        
        const cboMaquina = $("#cboMaquina");
        const botonBuscar = $('#botonBuscar');  
        const comboDivision = $("#comboDivision");
        const comboResponsable = $("#comboResponsable"); 
        //const cboTipoIntervalo = $("#cboTipoIntervalo");

        //catalogo cc
        const botonCatalogoCC = $("#botonCatalogoCC");
        const modalCatalogoCC = $("#modalCatalogoCC");
        const btnGuardarCC = $("#btnGuardarCC");        
        const tablaCC = $("#tablaCC");
        let dtTablaCC;        
              
        //// Tabla Kubrix
        const tablaKubrixDivision = $('#tablaKubrixDivision');
        let dtTablaKubrixDivision;

        const tablaKubrixAreaCuenta = $('#tablaKubrixAreaCuenta');
        let dtTablaKubrixAreaCuenta;

        const tablaKubrixDetalle = $('#tablaKubrixDetalle');
        let dtTablaKubrixDetalle;
        
        //// Detalles
        const modalDetallesK = $('#modalDetallesK');

        const divTablaSubCuenta = $('#divTablaSubCuenta');
        const tablaSubCuenta = $('#tablaSubCuenta');
        let dtTablaSubCuenta;
        const botonTablaSubCuenta = $("#botonTablaSubCuenta");

        const divTablaSubSubCuenta = $('#divTablaSubSubCuenta');
        const tablaSubSubCuenta = $('#tablaSubSubCuenta');
        let dtTablaSubSubCuenta;
        const botonTablaSubSubCuenta = $("#botonTablaSubSubCuenta");      
        
        const divTablaDivision = $('#divTablaDivision');
        const tablaDivision = $('#tablaDivision');
        let dtTablaDivision;
        const botonTablaDivision = $("#botonTablaDivision");   
        
        const divTablaAreaCuenta = $('#divTablaAreaCuenta');
        const tablaAreaCuenta = $('#tablaAreaCuenta');
        let dtTablaAreaCuenta;
        const botonTablaAreaCuenta = $("#botonTablaAreaCuenta");

        const divTablaConciliacion = $('#divTablaConciliacion');
        const tablaConciliacion = $('#tablaConciliacion');
        let dtTablaConciliacion;
        const botonTablaConciliacion = $("#botonTablaConciliacion");

        const divTablaEconomico = $('#divTablaEconomico');
        const tablaEconomico = $('#tablaEconomico');
        let dtTablaEconomico;
        const botonTablaEconomico = $("#botonTablaEconomico");

        const divTablaPoliza = $('#divTablaPoliza');
        const tablaPoliza = $('#tablaPoliza');
        let dtTablaPoliza;
        const botonTablaPoliza = $("#botonTablaPoliza");

        const divTablaDetalles = $('#divTablaDetalles');
        const tablaDetalles = $('#tablaDetalles');
        let dtTablaDetalles;
        const botonTablaDetalle = $("#botonTablaDetalle");


        const getLstKubrix = new URL(window.location.origin + '/Rentabilidad/getLstKubrix');
        const FormatoKubrixDetalles = new URL(window.location.origin + '/Rentabilidad/FormatoKubrixDetalles');

        let tipoReporte = 1;
        let modelos = [];

        //Analisis
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
        const tablaDetallesA = $('#tablaDetallesA');
        let dtTablaDetallesA;
        const botonNombreNivelUno = $("#botonNombreNivelUno");        

        ////Tabla Nivel Dos
        const divTablaNivelDos = $('#divTablaNivelDos');
        const tablaSubdetalles = $('#tablaSubdetalles');
        let dtTablaSubdetalles;        
        const botonNombreNivelDos = $("#botonNombreNivelDos");

        const divTablaNivelTres = $('#divTablaNivelTres');
        const tablaSubdetallesIngresos = $('#tablaSubdetallesIngresos');
        let dtTablaSubdetallesIngresos;
        const botonNombreNivelTres = $("#botonNombreNivelTres");

        //Grafica
        const divGrafica = $("#divGrafica");
        const divGraficaDetalle = $("#divGraficaDetalle");
        const graficaLineas = $("#graficaLineas");
        let grGrafica;

        let dateMax = new Date();
        let columnaAnalisis = 0;
        let dataGraficaDetalle = [ 0, 0, 0, 0, 0 ];
        let fechaInicioInput = new Date();
        let auxAdministrativoCentral = "";
        let auxAdministrativoProyectos = "";
        let auxOtros = "";
        const conceptos = ["Ingresos Contabilizados", "Ingresos con Estimación", "Ingresos Pendientes por Generar",
            "Total Ingresos", "Costo Total", "Depreciación", "Utilidad Bruta", "Gastos de Operación", "Resultado Antes Finacieros",
            "Gastos y Productos Financieros", "Resultado con Financieros", "Otros Ingresos", "Resultado Neto", "% de Margen"];
        let divisiones = [];
        let areasCuenta = [];
        let numAreaCuenta = 0;
        let responsable = false;
        let tipoIntervaloGlobal = false;

        const getLstAnalisis = new URL(window.location.origin + '/Rentabilidad/getLstAnalisis');
        const getLstCC = new URL(window.location.origin + '/Rentabilidad/getLstCC');
        const guardarLstCC = new URL(window.location.origin + '/Rentabilidad/guardarLstCC');
        const checkResponsable = new URL(window.location.origin + '/Rentabilidad/checkResponsable');

        function init() {
            setResponsable();
            initBotones();
            modalAnalisis.draggable({
                stop: function () {
                    var l = (100 * parseFloat($(this).css("left")) / parseFloat($(this).parent().css("width"))) + "%";
                    var t = (100 * parseFloat($(this).css("top")) / parseFloat($(this).parent().css("height"))) + "%";
                    $(this).css("left", l);
                    $(this).css("top", t);
                }
            });
            modalDetallesK.draggable({
                stop: function () {
                    var l = (100 * parseFloat($(this).css("left")) / parseFloat($(this).parent().css("width"))) + "%";
                    var t = (100 * parseFloat($(this).css("top")) / parseFloat($(this).parent().css("height"))) + "%";
                    $(this).css("left", l);
                    $(this).css("top", t);
                }
            });
            initDatePickers();
            fillCombos();
            agregarListeners();

            initTablaSubCuenta();
            initTablaSubSubCuenta();
            initTablaDivision();
            initTablaAreaCuenta();
            initTablaConciliacion();
            initTablaEconomico();
            initTablaPoliza();
            initTablaDetalles();

            setVisibles();
            $("#modalGrafica").draggable({
                stop: function () {
                    var l = (100 * parseFloat($(this).css("left")) / parseFloat($(this).parent().css("width"))) + "%";
                    var t = (100 * parseFloat($(this).css("top")) / parseFloat($(this).parent().css("height"))) + "%";
                    $(this).css("left", l);
                    $(this).css("top", t);
                }
            });
            initTablaSctaDetalles();
            initTablaDetallesA();
            initTablaSubdetalles();

            comboGrupoK.change(cargarComboModeloK);
            comboModeloK.change(cargarMaquinariaModelo);

            //
            initTablaCC();

            //cboTipoIntervalo.change(RecargarTablaDetalle);
            
        }

        function initBotones()
        {
            divTablaSubSubCuenta.hide(); 
            divTablaDivision.hide();
            divTablaAreaCuenta.hide();
            divTablaConciliacion.hide();
            divTablaEconomico.hide();
            divTablaPoliza.hide();
            divTablaDetalles.hide();
            
            botonTablaSubSubCuenta.hide(); 
            botonTablaDivision.hide();
            botonTablaAreaCuenta.hide();
            botonTablaConciliacion.hide();
            botonTablaEconomico.hide();
            botonTablaPoliza.hide();
            botonTablaDetalle.hide();            

            inputDiaInicio.hide();
        }

        function initDatePickers()
        {
            $("#inputDiaFinalAnalisis").datepicker({
                Button: false,
                dateFormat: "dd-mm-yy"
            });
            inputDiaFinal.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",
                onSelect: function (dateText) {
                    $("#inputDiaFinalAnalisis").val(dateText);
                }
            });
            inputDiaFinal.datepicker("setDate", new Date());
            inputDiaInicio.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",

            });
            inputDiaInicio.datepicker("setDate", new Date(1,0,1));

            $("#inputDiaFinalAnalisis").datepicker("setDate", new Date());
        }

        function agregarListeners()
        {
            //--> Tabla principal Detalle
            modalDetallesK.on("hide.bs.modal", function() {
                dtTablaSubCuenta.clear().draw();
                dtTablaSubSubCuenta.clear().draw();
                dtTablaDivision.clear().draw();
                dtTablaAreaCuenta.clear().draw();
                dtTablaConciliacion.clear().draw();
                dtTablaEconomico.clear().draw();
                dtTablaPoliza.clear().draw();
                dtTablaDetalles.clear().draw();                

                divTablaSubCuenta.show();
                divTablaSubSubCuenta.hide();
                divTablaDivision.hide();
                divTablaAreaCuenta.hide();
                divTablaConciliacion.hide();
                divTablaEconomico.hide();
                divTablaPoliza.hide();
                divTablaDetalles.hide();

                botonTablaSubCuenta.show();                
                botonTablaSubSubCuenta.hide();
                botonTablaDivision.hide();
                botonTablaAreaCuenta.hide();
                botonTablaConciliacion.hide();
                botonTablaEconomico.hide();   
                botonTablaPoliza.hide(); 
                botonTablaDetalle.hide();                
                
                botonTablaSubCuenta.prop("disabled", true);
                botonTablaSubSubCuenta.prop("disabled", true);
                botonTablaDivision.prop("disabled", true); 
                botonTablaAreaCuenta.prop("disabled", true); 
                botonTablaConciliacion.prop("disabled", true); 
                botonTablaEconomico.prop("disabled", true); 
                botonTablaPoliza.prop("disabled", true); 
                botonTablaDetalle.prop("disabled", true);                 
            });
            botonTablaSubCuenta.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubCuenta.is(":visible")) {
                    botonTablaSubCuenta.prop("disabled", true);
                    botonTablaSubSubCuenta.prop("disabled", true);
                    botonTablaDivision.prop("disabled", true); 
                    botonTablaAreaCuenta.prop("disabled", true); 
                    botonTablaConciliacion.prop("disabled", true); 
                    botonTablaEconomico.prop("disabled", true); 
                    botonTablaPoliza.prop("disabled", true); 
                    botonTablaDetalle.prop("disabled", true); 
                    
                    divTablaSubCuenta.show(500);
                    divTablaSubSubCuenta.hide(500);
                    divTablaDivision.hide(500);
                    divTablaAreaCuenta.hide(500);
                    divTablaConciliacion.hide(500);
                    divTablaEconomico.hide(500);
                    divTablaPoliza.hide(500);
                    divTablaDetalles.hide(500);                    

                    botonTablaSubSubCuenta.hide();
                    botonTablaDivision.hide(); 
                    botonTablaAreaCuenta.hide(); 
                    botonTablaConciliacion.hide(); 
                    botonTablaEconomico.hide(); 
                    botonTablaPoliza.hide(); 
                    botonTablaDetalle.hide();
                }                 
            });
            botonTablaSubSubCuenta.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubSubCuenta.is(":visible")) {
                    botonTablaSubSubCuenta.prop("disabled", true);
                    botonTablaDivision.prop("disabled", true);
                    botonTablaAreaCuenta.prop("disabled", true);
                    botonTablaConciliacion.prop("disabled", true);
                    botonTablaEconomico.prop("disabled", true);
                    botonTablaPoliza.prop("disabled", true);
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaSubSubCuenta.show(500);
                    divTablaDivision.hide(500);
                    divTablaAreaCuenta.hide(500);
                    divTablaConciliacion.hide(500);
                    divTablaEconomico.hide(500);
                    divTablaPoliza.hide(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaDivision.hide(); 
                    botonTablaAreaCuenta.hide(); 
                    botonTablaConciliacion.hide(); 
                    botonTablaEconomico.hide(); 
                    botonTablaPoliza.hide(); 
                    botonTablaDetalle.hide();                    
                }                 
            });
            botonTablaDivision.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubSubCuenta.is(":visible")) {
                    botonTablaDivision.prop("disabled", true);
                    botonTablaAreaCuenta.prop("disabled", true);
                    botonTablaConciliacion.prop("disabled", true);
                    botonTablaEconomico.prop("disabled", true);
                    botonTablaPoliza.prop("disabled", true);
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaDivision.show(500);
                    divTablaAreaCuenta.hide(500);
                    divTablaConciliacion.hide(500);
                    divTablaEconomico.hide(500);
                    divTablaPoliza.hide(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaAreaCuenta.hide(); 
                    botonTablaConciliacion.hide(); 
                    botonTablaEconomico.hide(); 
                    botonTablaPoliza.hide(); 
                    botonTablaDetalle.hide();                    
                }                 
            });
            botonTablaAreaCuenta.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubSubCuenta.is(":visible")) {
                    botonTablaAreaCuenta.prop("disabled", true);
                    botonTablaConciliacion.prop("disabled", true);
                    botonTablaEconomico.prop("disabled", true);
                    botonTablaPoliza.prop("disabled", true);
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaAreaCuenta.show(500);
                    divTablaConciliacion.hide(500);
                    divTablaEconomico.hide(500);
                    divTablaPoliza.hide(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaConciliacion.hide(); 
                    botonTablaEconomico.hide(); 
                    botonTablaPoliza.hide();
                    botonTablaDetalle.hide();                    
                }                 
            });
            botonTablaConciliacion.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubSubCuenta.is(":visible")) {
                    botonTablaConciliacion.prop("disabled", true);
                    botonTablaEconomico.prop("disabled", true);
                    botonTablaPoliza.prop("disabled", true);
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaConciliacion.show(500);
                    divTablaEconomico.hide(500);
                    divTablaPoliza.hide(500);
                    divTablaDetalles.hide(500);                    
                     
                    botonTablaEconomico.hide(); 
                    botonTablaPoliza.hide(); 
                    botonTablaDetalle.hide();                    
                }                 
            });
            botonTablaEconomico.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubSubCuenta.is(":visible")) {
                    botonTablaEconomico.prop("disabled", true);
                    botonTablaPoliza.prop("disabled", true);
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaEconomico.show(500);
                    divTablaPoliza.hide(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaPoliza.hide();
                    botonTablaDetalle.hide();                    
                }                 
            });
            botonTablaPoliza.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubSubCuenta.is(":visible")) {
                    botonTablaPoliza.prop("disabled", true);
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaPoliza.show(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaDetalle.hide();                    
                }                 
            });
            chbTipoReporte.change(function(){
                if (dtTablaKubrixDetalle != null) {
                    var info = dtTablaKubrixDetalle.rows().data();
                    if(chbTipoReporte.is(":checked")) { 
                        initTablaKubrixDetalle(info, 1); 
                    }
                    else{ initTablaKubrixDetalle(info, 2); }
                }
            });
            botonBuscar.click(function (e) { setLstKubrix(e); });
            
            //Analisis
            //botonBuscarAnalisis.click(setLstAnalisis);

            modalDetalles.on("hide.bs.modal", function() {
                dtTablaSctaDetalles.clear().draw();
                dtTablaDetallesA.clear().draw();
                dtTablaSubdetalles.clear().draw();
                dtTablaSubdetallesIngresos.clear().draw();
                divTablaNivelCero.show();
                botonNombreNivelCero.show();
                divTablaNivelUno.hide();
                botonNombreNivelUno.hide();
                divTablaNivelDos.hide();
                botonNombreNivelDos.hide();
                divTablaNivelTres.hide();
                botonNombreNivelTres.hide();
                botonNombreNivelCero.prop("disabled", true);
                botonNombreNivelUno.prop("disabled", true);
                botonNombreNivelDos.prop("disabled", true); 
                botonNombreNivelTres.prop("disabled", true); 
            });
            botonNombreNivelUno.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaNivelUno.is(":visible")) {
                    botonNombreNivelUno.prop("disabled", true);
                    botonNombreNivelDos.prop("disabled", true);
                    botonNombreNivelTres.prop("disabled", true);
                    divTablaNivelDos.hide(500);
                    divTablaNivelTres.hide(500);
                    divTablaNivelUno.show(500);
                    
                    botonNombreNivelDos.hide();       
                    botonNombreNivelTres.hide(); 
                }                 
            });
            botonNombreNivelCero.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaNivelCero.is(":visible")) {
                    botonNombreNivelCero.prop("disabled", true);
                    botonNombreNivelUno.prop("disabled", true);
                    botonNombreNivelDos.prop("disabled", true); 
                    botonNombreNivelTres.prop("disabled", true); 
                    divTablaNivelUno.hide(500);
                    divTablaNivelDos.hide(500);
                    divTablaNivelTres.hide(500);
                    divTablaNivelCero.show(500);
                    botonNombreNivelUno.hide();
                    botonNombreNivelDos.hide();     
                    botonNombreNivelTres.hide();  
                }                 
            });

            botonNombreNivelTres.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaNivelCero.is(":visible")) {
                    botonNombreNivelDos.prop("disabled", true); 
                    botonNombreNivelTres.prop("disabled", true); 
                    divTablaNivelUno.hide(500);
                    divTablaNivelDos.hide(500);
                    divTablaNivelTres.show(500);
                    divTablaNivelCero.hide(500);
                    botonNombreNivelDos.hide();                   
                }                 
            });
            modalAnalisis.on('shown.bs.modal', function (e) {
                dtTablaAnalisis.columns.adjust();                
            });
            $("#botonCerrarGrafica").click(function(e){
                $("#modalGrafica").modal("hide");
            });

            cbConfiguracion.change(recargarFechasTipoRep);

            //Division y responsable

            comboDivision.change(cargarAC);
            comboResponsable.change(cargarAC);

            //Catalogo CC
            botonCatalogoCC.click(cargarModalCC);
            btnGuardarCC.click(guardarCambiosCC);

            //-- comboAC multiselect custom --//
            comboAC.next(".select2-container").css("display", "none");
            $("#spanComboAC").click(function(e){
                comboAC.next(".select2-container").css("display", "block");
                comboAC.siblings("span").find(".select2-selection__rendered")[0].click();
            });
            comboAC.on('select2:close', function (e)
            {
                comboAC.next(".select2-container").css("display", "none");
                var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                if(seleccionados.length == 0) $("#spanComboAC").text("TODOS");
                else {
                    if (seleccionados.length  == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                    else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
                }                
            });
            comboAC.on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) { return; }
                evt.params.originalEvent.stopPropagation();
            });

            //-- comboModeloK multiselect custom --//      
            comboModeloK.next(".select2-container").css("display", "none");
            $("#spanComboModeloK").click(function(e){
                comboModeloK.next(".select2-container").css("display", "block");
                comboModeloK.siblings("span").find(".select2-selection__rendered")[0].click();
            });
            comboModeloK.on('select2:close', function (e)
            {
                comboModeloK.next(".select2-container").css("display", "none");
                var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                if(seleccionados.length == 0) $("#spanComboModeloK").text("TODOS");
                else {
                    if (seleccionados.length  == 1) $("#spanComboModeloK").text($(seleccionados[0]).text().slice(1));
                    else $("#spanComboModeloK").text(seleccionados.length.toString() + " Seleccionados");
                }                
            });
            comboModeloK.on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) { return; }
                evt.params.originalEvent.stopPropagation();
            });

        }
        
        function recargarFechasTipoRep() {
            var tipoReporte = cbConfiguracion.val();
            if(inputDiaInicio.is(":visible"))
            {
                if(inputDiaInicio.val() == "") { fechaInicioInput = new Date(); }
                else { fechaInicioInput = inputDiaInicio.datepicker('getDate') }
            }
            let yearActual = new Date().getFullYear();
            if(inputDiaFinal.val() != "") { yearActual = inputDiaFinal.datepicker('getDate').getFullYear(); }
            switch(tipoReporte)
            {
                case "0":
                    inputDiaInicio.datepicker("setDate", new Date(1,0,1));
                    inputDiaFinal.css("width", "100%");
                    inputDiaInicio.hide();                    
                    break;
                case "1":
                    inputDiaInicio.datepicker("setDate", new Date(yearActual,0,1));
                    inputDiaFinal.css("width", "100%");
                    inputDiaInicio.hide();                    
                    break;
                case "2":
                    inputDiaInicio.datepicker("setDate", fechaInicioInput);
                    inputDiaFinal.css("width", "49%");
                    inputDiaInicio.show();                    
                    break;
            }
        }

        function cambiarFechaInicio()
        {
            var tipoReporte = cbConfiguracion.val();
            let yearActual = new Date().getFullYear();
            if(inputDiaFinal.val() != "") { yearActual = inputDiaFinal.datepicker('getDate').getFullYear(); }
            if(tipoReporte == "0") { inputDiaInicio.datepicker("setDate", new Date(1,0,1)); }
            else { if(tipoReporte == "1") { inputDiaInicio.datepicker("setDate", new Date(yearActual,0,1)); } }
        }

        function fillCombos() {
            cboMaquina.select2();
            comboAC.select2({ closeOnSelect: false });
            //comboAC.append(new Option("SIN AREA CUENTA", "S/A", false, false)).trigger('change');
            comboGrupoK.select2();
            cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: -1 });
            comboGrupoK.fillCombo('/Rentabilidad/fillComboGrupo', {}, false, "TODOS");            
            comboModeloK.select2({ closeOnSelect: false });
            comboModeloK.fillCombo('/Rentabilidad/fillComboModelo', { grupoID: -1 }, true);
            comboModeloK.find('option').get(0).remove();
            comboDivision.fillCombo('/Rentabilidad/fillComboDivision', {}, false, "TODOS");
            comboResponsable.fillCombo('/Rentabilidad/fillComboResponsable', {}, false, "TODOS");

            //convertToMultiselectSelectAll(comboModeloK);
            //comboModeloK.multiselect('selectAll', true).multiselect('updateButtonText');

            comboAC.fillCombo('cboObraKubrix', null, false, "TODOS");
            comboAC.find('option').get(0).remove();
            comboTipo.fillCombo('cboTipo', null, false, "TODOS");

            //Analisis
            //comboACAnalisis.fillCombo('cboObra', null, false, "TODOS");
            comboTipoAnalisis.fillCombo('cboTipo', null, false, "");
            comboGrupo.multiselect();
            comboGrupo.multiselect('disable');

            comboModelo.multiselect();
            comboModelo.multiselect('disable');

            comboCC.multiselect();
            comboCC.multiselect('disable');
        }

        function cargarComboModeloK()
        {
            var grupo = comboGrupoK.val();
            if(grupo == "TODOS")
            {
                
                comboModeloK.fillCombo('/Rentabilidad/fillComboModelo', { grupoID: -1 }, true);
                comboModeloK.select2('destroy').find('option').prop('selected', false).end().select2({ closeOnSelect: false });
                cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: -1 });
            }
            else
            {
                comboModeloK.fillCombo('/Rentabilidad/fillComboModelo', { grupoID: grupo }, true);
                comboModeloK.select2('destroy').find('option').prop('selected', true).end().select2({ closeOnSelect: false });
                cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: grupo });
            }
            comboModeloK.trigger({ type: 'select2:close' });
        }

        function cargarMaquinariaModelo()
        {
            var grupo = comboGrupoK.val();
            var modelo = comboModeloK.val();
            cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: grupo, modeloID: modelo });
        }

        function setResponsable()
        {
            $.post(checkResponsable, { responsableID: comboResponsable.val() })
                .then(function (response) {
                    if (response.success) {
                        //if(response.responsable)
                        //{
                        //    botonCatalogoCC.hide();
                        //    comboResponsable.parent().hide();
                        //}
                        // Operación exitosa.
                        responsable = response.responsable;
                    } else {
                        // Operación no completada.
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                }
            );
        }

        function setLstKubrix(e) {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            cambiarFechaInicio();
            let busq = getBusquedaDTOK();
            //$('#tablaKubrixDetalle tbody td').addClass("blurry");
            $.post(getLstKubrix, { busq: busq })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        //auxDatos = response.lst;                       

                        auxAdministrativoCentral = "";                        
                        for(var i = 0; i < response.administrativoCentral.length; i++) { auxAdministrativoCentral += "- " + response.administrativoCentral[i] + "<br>" }
                        auxAdministrativoProyectos = "";
                        for(var i = 0; i < response.administrativoProyectos.length; i++) { auxAdministrativoProyectos += "- " + response.administrativoProyectos[i] + "<br>" }
                        auxOtros = "";
                        for(var i = 0; i < response.otros.length; i++) { auxOtros += "- " + response.otros[i] + "<br>" }
                        //initTablaKubrixDivision(datos, divisiones, "ARRENDADORA"); 
                        botonBuscar.attr("data-obra", comboAC.val());
                        botonBuscar.attr("data-responsable", comboResponsable.val());
                        botonBuscar.attr("data-fechaInicio", inputDiaInicio.datepicker('getDate'));
                        botonBuscar.attr("data-fechaFin", inputDiaFinal.datepicker('getDate'));
                        botonBuscar.attr("data-economico",  cboMaquina.val() == "" ? null : $("#cboMaquina option:selected").text().trim());
                        modelos = comboModeloK.val();
                        numAreaCuenta = comboAC.val().length;
                        if(numAreaCuenta == 1 || cboMaquina.val() != "")
                        {
                            setFormatoKubrixDetalles(response.lst, true);
                        }
                        else
                        {
                            if(comboResponsable.val() != "TODOS")
                            {
                                areasCuenta = ["ARRENDADORA"];
                                auxAreasCuenta = response.lst.map(function(element) { return element.areaCuenta.trim(); });
                                auxAreasCuenta = $.grep(auxAreasCuenta,function(el,index){ return (index == $.inArray(el,auxAreasCuenta) && el != null); });
                                auxAreasCuenta.sort(SortByAreaCuenta);
                                $.merge(areasCuenta, auxAreasCuenta);
                                var datos = FormatoDetalles(response.lst, areasCuenta, 2, true);
                                initTablaKubrixAreaCuenta(datos, areasCuenta, "ARRENDADORA");                            
                                if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                                $("#divTablaKubrixAreaCuenta").show(500);
                            }
                            else
                            {
                                divisiones = ["ARRENDADORA"];
                                auxDivisiones = response.lst.map(function(element) { return element.division; });
                                auxDivisiones = $.grep(auxDivisiones,function(el,index){ return (index == $.inArray(el,auxDivisiones) && el != null); });
                                $.merge(divisiones, auxDivisiones);
                                var datos = FormatoDetalles(response.lst, divisiones, 1, true);
                                initTablaKubrixDivision(datos, divisiones, "ARRENDADORA"); 
                            }
                        }  
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
        function setFormatoKubrixDetalles(datos, formatoSignos) {
            let busq = getBusquedaDTODetalle();
            var lst = obtenerListaKubrix(datos, busq, formatoSignos);  
            initTablaKubrixDetalle(lst, 1);
        }

        function FormatoDetalles(source, separadores, tipo, inicial)
        {
            var datos = [];
            var detalles = [];
            var detalles2 = [];
            var auxseparadores = [];
            for(var i = 1; i <= 14; i++)
            {
                detalles = [];
                detalles2 = [];
                auxseparadores = []
                switch(i)
                {
                    case 1:
                        detalles =  $.grep(source, function(el,index){ return (el.cta == 4000 && el != null); }); 
                        if(inicial == 1) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = detalles; 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = auxseparadoresImporteCuenta 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 2:
                        detalles = $.grep(source, function(el,index){ return ((el.tipoInsumo == "1-1" || el.tipoInsumo == "1-3") && el != null); }); 
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 3:
                        detalles = $.grep(source, function(el,index){ return (el.tipoInsumo == "1-2" && el != null); });  
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 4:
                        detalles = $.merge(detalles, datos[0].detalles);
                        detalles = $.merge(detalles, datos[1].detalles);
                        detalles = $.merge(detalles, datos[2].detalles);
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporte = 0
                            if(auxseparadoresImporteCuenta.length > 0)
                            {
                                $.each(auxseparadoresImporteCuenta, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        detalles = [];
                        break;
                    case 5:
                        detalles = $.grep(source, function(el,index){ return (el.cta == 5000 && el.tipoInsumo != "5000-10" && el != null); });
                        //if(tipo == 2) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 6:
                        detalles = $.grep(source, function(el,index){ return (el.tipoInsumo == "5000-10" && el != null); });    
                        //if(tipo == 2) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 7:
                        detalles = $.merge(detalles, datos[0].detalles);
                        detalles = $.merge(detalles, datos[1].detalles);
                        detalles = $.merge(detalles, datos[2].detalles);
                        detalles2 = $.merge(detalles2, datos[4].detalles);
                        detalles2 = $.merge(detalles2, datos[5].detalles);

                        var totalImporte = 0;
                        var detallesTotal = detalles
                        var detalles2Total = detalles2
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        if(detalles2Total.length > 0)
                        {
                            $.each(detalles2Total, function(index, element){
                                totalImporte -= element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporteCuenta2 = $.grep(detalles2, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporte = 0
                            if(auxseparadoresImporteCuenta.length > 0)
                            {
                                $.each(auxseparadoresImporteCuenta, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            if(auxseparadoresImporteCuenta2.length > 0)
                            {
                                $.each(auxseparadoresImporteCuenta2, function(index, element){
                                    auxseparadoresImporte -= element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        detalles = [];
                        break;
                    case 8:
                        detalles = $.grep(source, function(el,index){ return (el.cta == 5280 && el != null); });  
                        //if(tipo == 2) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 9:
                        detalles = $.merge(detalles, datos[0].detalles);
                        detalles = $.merge(detalles, datos[1].detalles);
                        detalles = $.merge(detalles, datos[2].detalles);

                        detalles2 = $.merge(detalles2, datos[4].detalles);
                        detalles2 = $.merge(detalles2, datos[5].detalles);
                        detalles2 = $.merge(detalles2, datos[7].detalles);
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        var detallesTotal2 = detalles2
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        if(detallesTotal2.length > 0)
                        {
                            $.each(detallesTotal2, function(index, element){
                                totalImporte -= element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporteCuenta2 = $.grep(detalles2, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporte = 0;
                            if(auxseparadoresImporteCuenta.length > 0)
                            {
                                $.each(auxseparadoresImporteCuenta, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            if(auxseparadoresImporteCuenta2.length > 0)
                            {
                                $.each(auxseparadoresImporteCuenta2, function(index, element){
                                    auxseparadoresImporte -= element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        detalles = [];
                        break;
                    case 10:
                        detalles = $.grep(source, function(el,index){ return ((el.cta == 4900 || el.cta == 5900) && el != null); });  
                        if(inicial == 1) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 11:
                        detalles = $.merge(detalles, datos[0].detalles);
                        detalles = $.merge(detalles, datos[1].detalles);
                        detalles = $.merge(detalles, datos[2].detalles);
                        detalles = $.merge(detalles, datos[9].detalles);

                        detalles2 = $.merge(detalles2, datos[4].detalles);
                        detalles2 = $.merge(detalles2, datos[5].detalles);
                        detalles2 = $.merge(detalles2, datos[7].detalles);
                                                
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        var detallesTotal2 = detalles2
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        if(detallesTotal2.length > 0)
                        {
                            $.each(detallesTotal2, function(index, element){
                                totalImporte -= element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporteCuenta2 = $.grep(detalles2, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            if(auxseparadoresImporteCuenta2.length > 0)
                            {
                                $.each(auxseparadoresImporteCuenta2, function(index, element){
                                    auxseparadoresImporte -= element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        detalles = [];
                        break;
                    case 12:
                        detalles = $.grep(source, function(el,index){ return ((el.cta == 4901 || el.cta == 5901) && el != null); });    
                        if(inicial == 1) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 13:
                        detalles = $.merge(detalles, datos[0].detalles);
                        detalles = $.merge(detalles, datos[1].detalles);
                        detalles = $.merge(detalles, datos[2].detalles);
                        detalles = $.merge(detalles, datos[9].detalles);
                        detalles = $.merge(detalles, datos[11].detalles);

                        detalles2 = $.merge(detalles2, datos[4].detalles);
                        detalles2 = $.merge(detalles2, datos[5].detalles);
                        detalles2 = $.merge(detalles2, datos[7].detalles);
                        
                        var totalImporte = 0;
                        var detallesTotal = detalles
                        var detallesTotal2 = detalles2
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.importe || 0;
                            });
                        }
                        if(detallesTotal2.length > 0)
                        {
                            $.each(detallesTotal2, function(index, element){
                                totalImporte -= element.importe || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporteCuenta2 = $.grep(detalles2, function (el, index){
                                return ((tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle =auxseparadoresImporteCuenta
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.importe || 0;
                                });
                            }
                            if(auxseparadoresImporteCuenta2.length > 0)
                            {
                                $.each(auxseparadoresImporteCuenta2, function(index, element){
                                    auxseparadoresImporte -= element.importe || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        detalles = [];
                        break;
                    case 14:
                        detalles = []; 
                        for(var j = 0; j <= separadores.length; j++)
                        {
                            var divisor = datos[3].separadores[j];
                            auxseparadoresImporte = 0;
                            if(Math.abs(divisor) > 1000)
                            {
                                auxseparadoresImporte = (datos[12].separadores[j] / datos[3].separadores[j]) * 100;
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                }
                var auxDatos = { descripcion: conceptos[i - 1], separadores: auxseparadores, detalles: detalles };
                datos.push(auxDatos);
            }
            return datos;
        }
                                
        function initTablaKubrixDetalle(data, tipo) {
            if (dtTablaKubrixDetalle != null) {
                dtTablaKubrixDetalle.destroy();
                tablaKubrixDetalle.empty();
                tablaKubrixDetalle.append('<thead class="bg-table-header"></thead>');
            }
            dtTablaKubrixDetalle = tablaKubrixDetalle.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                destroy: true,
                paging: false,
                autoWidth: false,
                searching: true,
                ordering: false,
                dom: '<<"col-xs-12 col-sm-12 col-md-12 col-lg-12 text-center divLblFiltros"><"col-xs-6 col-sm-6 col-md-3 col-lg-3 no-padding divCbTipoReporte"><"col-xs-6 col-sm-6 col-md-3 col-lg-3 no-padding divCbTipoIntervalo">' + (numAreaCuenta != 1 && cboMaquina.val() == "" ? '<"col-xs-6 col-sm-6 col-md-3 col-lg-3 no-padding divBotonAtrasDetalle">' : '') + 'f<t>>',
                data: data,
                columns: getKubrixColumnsDetalle(data, tipo),
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[0, 'asc']],
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaKubrixDetalle_filter input').addClass("form-control input-sm");
                },
                drawCallback: function( settings ) {                 
                    CargarGraficaKubrix(data, tipo, "", true);
                    if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
                    if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                    $("#divTablaKubrixDetalle").show(500);
                    $(".separador").click(function (e) {   
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();                      
                        if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                        if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                        $("#divTablaKubrixAreaCuenta").show(500);
                    });
                    tablaKubrixDetalle.find('p.desplegable').unbind().click(function (e) {
                        
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();

                        const rowData = dtTablaKubrixDetalle.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const nombreColumna = $('#tablaKubrixDetalle thead tr th').eq($(this).parents("td").index()).text().trim();

                        var fechaMax = new Date(botonBuscar.attr("data-fechaFin"));
                        var auxFecha = new Date(botonBuscar.attr("data-fechaFin"));
                        var auxTipo = 0;
                        switch(nombreColumna)
                        {
                            case "Actual":
                            case "Empresa":
                                break;
                            case "Semana 2":
                                auxFecha.setDate(fechaMax.getDate() - 7);
                                detalles = jQuery.grep(detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
                                break;
                            case "Semana 3":
                                auxFecha.setDate(fechaMax.getDate() - 14)                          
                                detalles = jQuery.grep(detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
                                break;
                            case "Semana 4":
                                auxFecha.setDate(fechaMax.getDate() - 21)
                                detalles = jQuery.grep(detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
                                break;
                            case "Semana 5":
                                auxFecha.setDate(fechaMax.getDate() - 28)
                                detalles = jQuery.grep(detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
                                break;
                            case "Mes 2":
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                detalles = jQuery.grep(detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
                                break;
                            case "Mes 3":
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                detalles = jQuery.grep(detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
                                break;
                            case "Mes 4":
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                detalles = jQuery.grep(detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
                                break;
                            case "Mes 5":
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1);
                                auxFecha.setDate(auxFecha.getDate() - 1);
                                detalles = jQuery.grep(detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
                                break;
                            case "EquipoMayor":
                                auxTipo = 1;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 1; });
                                break;
                            case "EquipoMenor":
                                auxTipo = 2;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 2; });
                                break;
                            case "EquipoTransporteConstruplan":
                                auxTipo = 3;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 3; });
                                break;
                            case "EquipoTransporteArrendadora":
                                auxTipo = 8;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 8; });
                                break;
                            case "Fletes":
                                auxTipo = 4;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 4; });
                                break;
                            case "OTR":
                                auxTipo = 5;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 5; });
                                break;
                            case "AdminCentral":
                                auxTipo = 6;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 6; });
                                break;
                            case "AdminProyectos":
                                auxTipo = 9;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 9; });
                                break;
                            case "Otros":
                                auxTipo = 7;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 7; });
                                break;
                            default:
                                break;
                        }
                        cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, auxFecha, auxTipo)                        
                    });
                    //$('#tablaKubrixDetalle tbody td').addClass("blurry");
                    //$('#tablaKubrixDetalle tbody').fadeIn(800);
                    //setTimeout(function () {
                    //    $('#tablaKubrixDetalle tbody td').removeClass("blurry");
                    //}, 600);
                    
                    $(".filtrado").click(function (e) {   
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaKubrixDetalle.data();
                        var detallesRaw = rowData.map(function(x) { return x.detalles});
                        var detalles = [].concat.apply([], detallesRaw); 
                        if (dtTablaKubrixDetalle.data().any()) {
                            var idTipo = $(this).attr("data-filtro");
                            botonBuscar.attr("data-tipo", idTipo);
                            $("#botonNombreNivelCero").attr("data-filtro", idTipo);
                            switch (idTipo) {
                                case "1":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO MAYOR");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 1, 1);                                    
                                    break;
                                case "2":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO MENOR");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 2; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "3":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO TRANSPORTE CONSTRUPLAN");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 3; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "4":
                                    $("#tituloModal").text("ARRENDADORA - ADMINISTRATIVO CENTRAL");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 4; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "5":
                                    $("#tituloModal").text("ARRENDADORA - FLETES");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 5; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "6":
                                    $("#tituloModal").text("ARRENDADORA - OTR");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 6; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "7":
                                    $("#tituloModal").text("ARRENDADORA - OTROS");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 7; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "8":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO TRANSPORTE ARRENDADORA");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 8; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "9":
                                    $("#tituloModal").text("ARRENDADORA - ADMINISTRATIVO PROYECTOS");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 9; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                default:
                                    $("#tituloModal").text("ARRENDADORA");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                            }
                        }
                    });
                    $('#tablaKubrixDetalle thead').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        CargarGraficaKubrix(data, tipo, "", true);
                    });
                    $('#tablaKubrixDetalle tbody').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var auxData = dtTablaKubrixDetalle.row(this).data();
                        CargarGraficaKubrix(data, tipo, auxData.descripcion, false);
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( data.tipo_mov == 4 || data.tipo_mov == 7 || data.tipo_mov == 9 || data.tipo_mov == 11 || data.tipo_mov == 13 ||  data.tipo_mov == 14){
                        $(row).addClass('resultado');
                    }
                }
            });
            $('[data-toggle="tooltip"]').tooltip();
            //switch tipo reporte
            $("div.divCbTipoReporte")
                .html('<input type="checkbox" ' + (tipo == 1 ? 'checked' : '') + ' data-toggle="toggle" data-on="Tipo Equipo" data-off="Periodo" data-onstyle="success" data-offstyle="info" id="chbTipoReporte">');
            $('div.divCbTipoReporte input').bootstrapToggle();
            $('div.divCbTipoReporte #chbTipoReporte').change(function(){
                if (dtTablaKubrixDetalle != null) {
                    var info = dtTablaKubrixDetalle.rows().data();
                    if($(this).is(":checked")) { initTablaKubrixDetalle(info, 1); }
                    else{ initTablaKubrixDetalle(info, 2); }
                }
            });

            $("div.divCbTipoIntervalo")
                .html('<input type="checkbox" ' + (tipoIntervaloGlobal == true ? 'checked' : '') + ' data-toggle="toggle" data-on="Semanal" data-off="Mensual" data-onstyle="success" data-offstyle="info" id="cboTipoIntervalo">');
            $('div.divCbTipoIntervalo input').bootstrapToggle();
            $('div.divCbTipoIntervalo #cboTipoIntervalo').change(function(){
                if (dtTablaKubrixDetalle != null) {
                    
                    tipoIntervaloGlobal = !tipoIntervaloGlobal;
                    RecargarTablaDetalle;
                }
            });
            $("div.divBotonAtrasDetalle")
                .html('<button class="btn btn-primary" id="botonAtras"> <i class="fas fa-level-up-alt"></i> </button>');
            $('div.divBotonAtrasDetalle #botonAtras').click(function(){
                if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                $("#divTablaKubrixAreaCuenta").show(500);
            });
            $('div.divCbTipoReporte .toggle').css("margin-top", "0");
            $('div.divCbTipoReporte .toggle').css("margin-bottom", "5px");

            //Label filtros
            $("div.divLblFiltrosDetalle")
                .html('<b>Area Cuenta:</b> ' + $("#comboAC option:selected").text().trim() + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + $("#cbConfiguracion option:selected").text().trim() 
                + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputDiaFinal.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));
            $('div.divLblFiltrosDetalle').css("font-size", "20px");
        }

        function initTablaKubrixDivision(data, separador, nombreTotal) {
            tipo = 1;
            if (dtTablaKubrixDivision != null) { dtTablaKubrixDivision.destroy(); }
            tablaKubrixDivision.empty();
            tablaKubrixDivision.append('<thead class="bg-table-header"></thead>');
            dtTablaKubrixDivision = tablaKubrixDivision.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                destroy: true,
                paging: false,
                autoWidth: false,
                searching: true,
                ordering: false,
                dom: '<<"col-xs-12 col-sm-12 col-md-12 col-lg-12 text-center divLblFiltrosDivision">f<t>>',
                data: data,
                columns: getKubrixColumns(data, separador),
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[0, 'asc']],
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaKubrixDivision_filter input').addClass("form-control input-sm");
                },
                drawCallback: function( settings ) {                 
                    CargarGraficaKubrix(data, tipo, "", true);
                    if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
                    if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                    $("#divTablaKubrixDivision").show(500);
                    tablaKubrixDivision.find('p.desplegable').unbind().click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaKubrixDivision.row($(this).parents('tr')).data();
                        var indexRow = dtTablaKubrixDivision.row($(this).parents('tr')).index();
                        var detalles = rowData.detalles;
                        const nombreColumna = $('#tablaKubrixDivision thead tr th').eq($(this).parents("td").index()).text().trim();
                        var fechaMax = new Date(botonBuscar.attr("data-fechaFin"));
                        if(nombreColumna != nombreTotal){ detalles = jQuery.grep(detalles, function( n, i ) { return n.division.trim() == nombreColumna; }); }
                        //if(indexRow !=  4 && indexRow != 5 && indexRow != 7){ $.each(detalles, function(index, element){ element.importe = element.importe * (-1)}); }
                        cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, fechaMax, 0);                     
                    });
                    $('#tablaKubrixDivision tbody').fadeIn(800);
                    
                    $(".separador").click(function (e) {   
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let rowData = dtTablaKubrixDivision.data();
                        const nombreColumna = $(this).text().trim();
                        var detallesRaw = rowData.map(function(x) { return x.detalles});
                        var detalles = [].concat.apply([], detallesRaw);
                        detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                        if(nombreColumna != nombreTotal) detalles = $.grep(detalles, function( n, i ) { return n.division == nombreColumna; });
                        areasCuenta = [nombreColumna];
                        auxAreasCuenta = detalles.map(function(element) { return element.areaCuenta.trim(); });
                        auxAreasCuenta = $.grep(auxAreasCuenta,function(el,index){ return (index == $.inArray(el,auxAreasCuenta) && el != null); });
                        auxAreasCuenta.sort(SortByAreaCuenta);
                        $.merge(areasCuenta, auxAreasCuenta);
                        var datos = FormatoDetalles(detalles, areasCuenta, 2, false);
                        if(nombreColumna == nombreTotal)
                        {
                            initTablaKubrixAreaCuenta(datos, areasCuenta, nombreColumna);
                            setFormatoKubrixDetalles(detalles);                            
                            if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                            if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
                            $("#divTablaKubrixDetalle").show(500);
                        }else{
                            initTablaKubrixAreaCuenta(datos, areasCuenta, nombreColumna);                            
                            if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                            if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                            $("#divTablaKubrixAreaCuenta").show(500);
                        }
                    });
                    $('#tablaKubrixDivision thead').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        CargarGraficaKubrix(data, tipo, "", true);
                    });
                    $('#tablaKubrixDivision tbody').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var auxData = dtTablaKubrixDivision.row(this).data();
                        CargarGraficaKubrix(data, tipo, auxData.descripcion, false);
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( dataIndex == 3 || dataIndex == 6 || dataIndex == 8 || dataIndex == 10 || dataIndex == 12 ||  dataIndex == 13){
                        $(row).addClass('resultado');
                    }
                },
            });
            $('[data-toggle="tooltip"]').tooltip();
            //Label filtros
            $("div.divLblFiltrosDivision")
                .html('<b>Area Cuenta:</b> ' + $("#comboAC option:selected").text().trim() + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + $("#cbConfiguracion option:selected").text().trim() 
                + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputDiaFinal.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));
            $('div.divLblFiltrosDivision').css("font-size", "20px");
        }

        function initTablaKubrixAreaCuenta(data, separador, nombreTotal) {
            tipo = 1;
            if (dtTablaKubrixAreaCuenta != null) { dtTablaKubrixAreaCuenta.destroy(); }
            tablaKubrixAreaCuenta.empty();
            tablaKubrixAreaCuenta.append('<thead class="bg-table-header"></thead>');
            dtTablaKubrixAreaCuenta = tablaKubrixAreaCuenta.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                destroy: true,
                paging: false,
                autoWidth: false,
                searching: true,
                ordering: false,
                dom: '<<"col-xs-12 col-sm-12 col-md-12 col-lg-12 text-center divLblFiltrosAreaCuenta">' + (comboResponsable.val() == "TODOS" ? '<"col-xs-6 col-sm-6 col-md-3 col-lg-3 no-padding divBotonAtrasAreaCuenta">' : '') + 'f<t>>',
                data: data,
                columns: getKubrixColumns(data, separador),
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[0, 'asc']],
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaKubrixAreaCuenta_filter input').addClass("form-control input-sm");
                },
                drawCallback: function( settings ) {                 
                    CargarGraficaKubrix(data, tipo, "", true);
                    tablaKubrixAreaCuenta.find('p.desplegable').unbind().click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaKubrixAreaCuenta.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const nombreColumna = $('#tablaKubrixAreaCuenta thead tr th').eq($(this).parents("td").index()).text().trim();
                        var fechaMax = new Date(botonBuscar.attr("data-fechaFin"));
                        if(nombreColumna != nombreTotal){ detalles = jQuery.grep(detalles, function( n, i ) { return n.areaCuenta.trim() == nombreColumna; }); }
                        
                        cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, fechaMax, 0);                     
                    });
                    $('#tablaKubrixAreaCuenta tbody').fadeIn(800);
                    
                    $(".separador").click(function (e) {   
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let rowData = dtTablaKubrixAreaCuenta.data();
                        const nombreColumna = $(this).text().trim();
                        var detallesRaw = rowData.map(function(x) { return x.detalles});
                        var detalles = [].concat.apply([], detallesRaw);
                        detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                        if(nombreColumna != nombreTotal) detalles = $.grep(detalles, function( n, i ) { return n.areaCuenta == nombreColumna; });
                        var datos = detalles;
                        var semanas= [];
                        setFormatoKubrixDetalles(datos, false);
                        //initTablaKubrix(tablaKubrixAreaCuenta, dtTablaKubrixAreaCuenta, "tablaKubrixAreaCuenta", datos, areasCuenta, 2);
                            
                        if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                        if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
                        $("#divTablaKubrixDetalle").show(500);
                    });
                    $('#tablaKubrixAreaCuenta thead').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        CargarGraficaKubrix(data, tipo, "", true);
                    });
                    $('#tablaKubrixAreaCuenta tbody').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var auxData = dtTablaKubrixAreaCuenta.row(this).data();
                        CargarGraficaKubrix(data, tipo, auxData.descripcion, false);
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( dataIndex == 3 || dataIndex == 6 || dataIndex == 8 || dataIndex == 10 || dataIndex == 12 ||  dataIndex == 13){
                        $(row).addClass('resultado');
                    }
                },
            });
            $('[data-toggle="tooltip"]').tooltip();
            //switch tipo reporte
            $("div.divBotonAtrasAreaCuenta")
                .html('<button class="btn btn-primary" id="botonAtras"> <i class="fas fa-level-up-alt"></i> </button>');
            $('div.divBotonAtrasAreaCuenta #botonAtras').click(function(){
                if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                $("#divTablaKubrixDivision").show(500);
            });
            //Label filtros
            $("div.divLblFiltrosAreaCuenta")
                .html('<b>Area Cuenta:</b> ' + $("#comboAC option:selected").text().trim() + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + $("#cbConfiguracion option:selected").text().trim() 
                + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputDiaFinal.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));
            $('div.divLblFiltrosAreaCuenta').css("font-size", "20px");
        }

        function getKubrixColumnsDetalle(datos, tipo) {
            if(tipo == 1){
                return [
                    { data: 'descripcion', title: 'Concepto' }
                    , {
                        data: 'actual',
                        title: numAreaCuenta == 1 || botonBuscar.attr("data-economico") != null ? 'Actual' : '<button class="btn btn-sm btn-light separador" title="Actual">Actual</button>',
                        render: function (data, type, row, meta) {
                            var indexColumna = meta.row + 1;
                            var importeFinal = data
                            if (indexColumna == 4 || indexColumna == 7 || indexColumna == 9 || indexColumna == 11 || indexColumna == 13)
                                return getNumberHTML(importeFinal);
                            if (indexColumna == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'semana2',
                        title: tipoIntervaloGlobal == 1 ? 'Semana 2' : 'Mes 2',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'semana3',
                        title: tipoIntervaloGlobal == 1 ? 'Semana 3' : 'Mes 3',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'semana4',
                        title: tipoIntervaloGlobal == 1 ? 'Semana 4' : 'Mes 4',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'semana5',
                        title: tipoIntervaloGlobal == 1 ? 'Semana 5' : 'Mes 5',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                ];
            }
            else{
                return [
                    { data: 'descripcion', title: 'Concepto' }
                    , {
                        data: 'total',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="0">Empresa</button>',                    
                        render: function (data, type, row) {                        
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , {
                        data: 'mayor',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="1">Equipo<br>Mayor</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'menor',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="2">Equipo<br>Menor</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'transporteConstruplan',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="3">Equipo<br>Transporte<br>Construplan</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'transporteArrendadora',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="8">Equipo<br>Transporte<br>Arrendadora</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }

                    , {
                        data: 'fletes',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="4">Fletes</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'neumaticos',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="5">OTR</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'administrativoCentral',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="6" data-toggle="tooltip" data-placement="bottom" data-html="true" title="'+ auxAdministrativoCentral +'">Admin<br>Central</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'administrativoProyectos',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="9" data-toggle="tooltip" data-placement="bottom" data-html="true" title="'+ auxAdministrativoProyectos +'">Admin<br>Proyectos</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                    , {
                        data: 'otros',
                        title: '<button class="btn btn-sm btn-light filtrado" data-filtro="7"data-toggle="tooltip" data-placement="bottom" data-html="true" title="'+ auxOtros +'">Otros</button>',
                        render: function (data, type, row) {
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                            return getRowHTML(importeFinal);
                        }
                    }
                ];
            }
        }
        
        function obtenerListaAnalisis(detalles, idTipo)
        {
            var rowData = [];
            for(var i = 1; i <= 14; i++)
            {                
                var auxRowData = { tipo_mov: 0, descripcion: "", actual: 0, semana2: 0, semana3: 0, semana4: 0, semana5: 0, cfc: 0, cf: 0, mc: 0, pr: 0, tc: 0, car: 0, otros: 0, detalles: [] }
                var fechaMax = new Date(botonBuscar.attr("data-fechaFin"));
                switch(i)
                {
                    case 1: 
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 1; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Ingresos Contabilizados"; 
                        auxRowData.tipo_mov = 1;
                        auxRowData.actual = costos.actual;
                        auxRowData.semana2 = costos.semana2;
                        auxRowData.semana3 = costos.semana3;
                        auxRowData.semana4 = costos.semana4;
                        auxRowData.semana5 = costos.semana5;
                        auxRowData.cfc = costos.cfc;
                        auxRowData.cf = costos.cf;
                        auxRowData.mc = costos.mc;
                        auxRowData.pr = costos.pr;
                        auxRowData.tc = costos.tc;
                        auxRowData.car = costos.car;
                        auxRowData.ex = costos.ex;
                        auxRowData.hdt = costos.hdt;
                        auxRowData.otros = costos.otros;
                        //$.each(auxDetalles, function(key, value) {
                        //    key.importe = key.importe * (-1);
                        //});
                        auxRowData.detalles = auxDetalles;
                        break;
                    case 2:                         
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 2; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Ingresos con Estimación";
                        auxRowData.tipo_mov = 2;
                        auxRowData.actual = costos.actual;
                        auxRowData.semana2 = costos.semana2;
                        auxRowData.semana3 = costos.semana3;
                        auxRowData.semana4 = costos.semana4;
                        auxRowData.semana5 = costos.semana5;
                        auxRowData.cfc = costos.cfc;
                        auxRowData.cf = costos.cf;
                        auxRowData.mc = costos.mc;
                        auxRowData.pr = costos.pr;
                        auxRowData.tc = costos.tc;
                        auxRowData.car = costos.car;
                        auxRowData.ex = costos.ex;
                        auxRowData.hdt = costos.hdt;
                        auxRowData.otros = costos.otros;
                        auxRowData.detalles = auxDetalles;
                        break;
                    case 3:                         
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 3; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Ingresos Pendientes por Generar"; 
                        auxRowData.tipo_mov = 3;
                        auxRowData.actual = costos.actual ;
                        auxRowData.semana2 = costos.semana2 ;
                        auxRowData.semana3 = costos.semana3 ;
                        auxRowData.semana4 = costos.semana4 ;
                        auxRowData.semana5 = costos.semana5 ;
                        auxRowData.cfc = costos.cfc ;
                        auxRowData.cf = costos.cf ;
                        auxRowData.mc = costos.mc ;
                        auxRowData.pr = costos.pr ;
                        auxRowData.tc = costos.tc ;
                        auxRowData.car = costos.car ;
                        auxRowData.ex = costos.ex;
                        auxRowData.hdt = costos.hdt;
                        auxRowData.otros = costos.otros ;
                        auxRowData.detalles = auxDetalles;
                        break;
                    case 4:                         
                        auxRowData.descripcion = "Total Ingresos"; 
                        auxRowData.tipo_mov = 4;
                        auxRowData.actual = rowData[0].actual + rowData[1].actual + rowData[2].actual;
                        auxRowData.semana2 = rowData[0].semana2 + rowData[1].semana2 + rowData[2].semana2;
                        auxRowData.semana3 = rowData[0].semana3 + rowData[1].semana3 + rowData[2].semana3;
                        auxRowData.semana4 = rowData[0].semana4 + rowData[1].semana4 + rowData[2].semana4;
                        auxRowData.semana5 = rowData[0].semana5 + rowData[1].semana5 + rowData[2].semana5;
                        auxRowData.cfc = rowData[0].cfc + rowData[1].cfc + rowData[2].cfc;
                        auxRowData.cf = rowData[0].cf + rowData[1].cf + rowData[2].cf;
                        auxRowData.mc = rowData[0].mc + rowData[1].mc + rowData[2].mc;
                        auxRowData.pr = rowData[0].pr + rowData[1].pr + rowData[2].pr;
                        auxRowData.tc = rowData[0].tc + rowData[1].tc + rowData[2].tc;
                        auxRowData.car = rowData[0].car + rowData[1].car + rowData[2].car;
                        auxRowData.ex = rowData[0].ex + rowData[1].ex + rowData[2].ex;
                        auxRowData.hdt = rowData[0].hdt + rowData[1].hdt + rowData[2].hdt;
                        auxRowData.otros = rowData[0].otros + rowData[1].otros + rowData[2].otros;
                        break;
                    case 5:                         
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 5; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Costo Total";
                        auxRowData.tipo_mov = 5;
                        auxRowData.actual = costos.actual;
                        auxRowData.semana2 = costos.semana2;
                        auxRowData.semana3 = costos.semana3;
                        auxRowData.semana4 = costos.semana4;
                        auxRowData.semana5 = costos.semana5;
                        auxRowData.cfc = costos.cfc;
                        auxRowData.cf = costos.cf;
                        auxRowData.mc = costos.mc;
                        auxRowData.pr = costos.pr;
                        auxRowData.tc = costos.tc;
                        auxRowData.car = costos.car;
                        auxRowData.ex = costos.ex;
                        auxRowData.hdt = costos.hdt;
                        auxRowData.otros = costos.otros;
                        auxRowData.detalles = auxDetalles;
                        break;
                    case 6:                         
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 6; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Depreciación";
                        auxRowData.tipo_mov = 6;
                        auxRowData.actual = costos.actual;
                        auxRowData.semana2 = costos.semana2;
                        auxRowData.semana3 = costos.semana3;
                        auxRowData.semana4 = costos.semana4;
                        auxRowData.semana5 = costos.semana5;
                        auxRowData.cfc = costos.cfc;
                        auxRowData.cf = costos.cf;
                        auxRowData.mc = costos.mc;
                        auxRowData.pr = costos.pr;
                        auxRowData.tc = costos.tc;
                        auxRowData.car = costos.car;
                        auxRowData.ex = costos.ex;
                        auxRowData.hdt = costos.hdt;
                        auxRowData.otros = costos.otros;
                        auxRowData.detalles = auxDetalles;
                        break;
                    case 7: 
                        auxRowData.descripcion = "Utilidad Bruta"; 
                        auxRowData.tipo_mov = 7;
                        auxRowData.actual = rowData[3].actual - rowData[4].actual - rowData[5].actual;
                        auxRowData.semana2 = rowData[3].semana2 - rowData[4].semana2 - rowData[5].semana2;
                        auxRowData.semana3 = rowData[3].semana3 - rowData[4].semana3 - rowData[5].semana3;
                        auxRowData.semana4 = rowData[3].semana4 - rowData[4].semana4 - rowData[5].semana4;
                        auxRowData.semana5 = rowData[3].semana5 - rowData[4].semana5 - rowData[5].semana5;
                        auxRowData.cfc = rowData[3].cfc - rowData[4].cfc - rowData[5].cfc;
                        auxRowData.cf = rowData[3].cf - rowData[4].cf - rowData[5].cf;
                        auxRowData.mc = rowData[3].mc - rowData[4].mc - rowData[5].mc;
                        auxRowData.pr = rowData[3].pr - rowData[4].pr - rowData[5].pr;
                        auxRowData.tc = rowData[3].tc - rowData[4].tc - rowData[5].tc;
                        auxRowData.car = rowData[3].car - rowData[4].car - rowData[5].car;
                        auxRowData.ex = rowData[3].ex - rowData[4].ex - rowData[5].ex;
                        auxRowData.hdt = rowData[3].hdt - rowData[4].hdt - rowData[5].hdt;
                        auxRowData.otros = rowData[3].otros - rowData[4].otros - rowData[5].otros;
                        break;
                    case 8:                         
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 8; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Gastos de Operación"; 
                        auxRowData.tipo_mov = 8;
                        auxRowData.actual = costos.actual;
                        auxRowData.semana2 = costos.semana2;
                        auxRowData.semana3 = costos.semana3;
                        auxRowData.semana4 = costos.semana4;
                        auxRowData.semana5 = costos.semana5;
                        auxRowData.cfc = costos.cfc;
                        auxRowData.cf = costos.cf;
                        auxRowData.mc = costos.mc;
                        auxRowData.pr = costos.pr;
                        auxRowData.tc = costos.tc;
                        auxRowData.car = costos.car;
                        auxRowData.ex = costos.ex;
                        auxRowData.hdt = costos.hdt;
                        auxRowData.otros = costos.otros;
                        auxRowData.detalles = auxDetalles;
                        break;
                    case 9: 
                        auxRowData.descripcion = "Resultado Antes Financieros"; 
                        auxRowData.tipo_mov = 9;
                        auxRowData.actual = rowData[6].actual - rowData[7].actual;
                        auxRowData.semana2 = rowData[6].semana2 - rowData[7].semana2;
                        auxRowData.semana3 = rowData[6].semana3 - rowData[7].semana3;
                        auxRowData.semana4 = rowData[6].semana4 - rowData[7].semana4;
                        auxRowData.semana5 = rowData[6].semana5 - rowData[7].semana5;
                        auxRowData.cfc = rowData[6].cfc - rowData[7].cfc;
                        auxRowData.cf = rowData[6].cf - rowData[7].cf;
                        auxRowData.mc = rowData[6].mc - rowData[7].mc;
                        auxRowData.pr = rowData[6].pr - rowData[7].pr;
                        auxRowData.tc = rowData[6].tc - rowData[7].tc;
                        auxRowData.car = rowData[6].car - rowData[7].car;
                        auxRowData.ex = rowData[6].ex - rowData[7].ex;
                        auxRowData.hdt = rowData[6].hdt - rowData[7].hdt;
                        auxRowData.otros = rowData[6].otros - rowData[7].otros;
                        break;
                    case 10:                         
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 10; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Gastos Financieros"; 
                        auxRowData.tipo_mov = 10;
                        auxRowData.actual = costos.actual;
                        auxRowData.semana2 = costos.semana2;
                        auxRowData.semana3 = costos.semana3;
                        auxRowData.semana4 = costos.semana4;
                        auxRowData.semana5 = costos.semana5;
                        auxRowData.cfc = costos.cfc;
                        auxRowData.cf = costos.cf;
                        auxRowData.mc = costos.mc;
                        auxRowData.pr = costos.pr;
                        auxRowData.tc = costos.tc;
                        auxRowData.car = costos.car;
                        auxRowData.ex = costos.ex;
                        auxRowData.hdt = costos.hdt;
                        auxRowData.otros = costos.otros;
                        auxRowData.detalles = auxDetalles;
                        break;
                    case 11: 
                        auxRowData.descripcion = "Resultado con Financieros"; 
                        auxRowData.tipo_mov = 11;
                        auxRowData.actual = rowData[8].actual + rowData[9].actual;
                        auxRowData.semana2 = rowData[8].semana2 + rowData[9].semana2;
                        auxRowData.semana3 = rowData[8].semana3 + rowData[9].semana3;
                        auxRowData.semana4 = rowData[8].semana4 + rowData[9].semana4;
                        auxRowData.semana5 = rowData[8].semana5 + rowData[9].semana5;
                        auxRowData.cfc = rowData[8].cfc + rowData[9].cfc;
                        auxRowData.cf = rowData[8].cf + rowData[9].cf;
                        auxRowData.mc = rowData[8].mc + rowData[9].mc;
                        auxRowData.pr = rowData[8].pr + rowData[9].pr;
                        auxRowData.tc = rowData[8].tc + rowData[9].tc;
                        auxRowData.car = rowData[8].car + rowData[9].car;
                        auxRowData.ex = rowData[8].ex + rowData[9].ex;
                        auxRowData.hdt = rowData[8].hdt + rowData[9].hdt;
                        auxRowData.otros = rowData[8].otros + rowData[9].otros;
                        break;
                    case 12:                         
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 12; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Otros Ingresos"; 
                        auxRowData.tipo_mov = 12;
                        auxRowData.actual = costos.actual;
                        auxRowData.semana2 = costos.semana2;
                        auxRowData.semana3 = costos.semana3;
                        auxRowData.semana4 = costos.semana4;
                        auxRowData.semana5 = costos.semana5;
                        auxRowData.cfc = costos.cfc;
                        auxRowData.cf = costos.cf;
                        auxRowData.mc = costos.mc;
                        auxRowData.pr = costos.pr;
                        auxRowData.tc = costos.tc;
                        auxRowData.car = costos.car;
                        auxRowData.ex = costos.ex;
                        auxRowData.hdt = costos.hdt;
                        auxRowData.otros = costos.otros;
                        $.each(auxDetalles, function(key, value) {
                            key.importe = key.importe;
                        });
                        auxRowData.detalles = auxDetalles;
                        break;
                    case 13: 
                        auxRowData.descripcion = "Resultado Neto"; 
                        auxRowData.tipo_mov = 13;
                        auxRowData.actual = rowData[10].actual + rowData[11].actual;
                        auxRowData.semana2 = rowData[10].semana2 + rowData[11].semana2;
                        auxRowData.semana3 = rowData[10].semana3 + rowData[11].semana3;
                        auxRowData.semana4 = rowData[10].semana4 + rowData[11].semana4;
                        auxRowData.semana5 = rowData[10].semana5 + rowData[11].semana5;
                        auxRowData.cfc = rowData[10].cfc + rowData[11].cfc;
                        auxRowData.cf = rowData[10].cf + rowData[11].cf;
                        auxRowData.mc = rowData[10].mc + rowData[11].mc;
                        auxRowData.pr = rowData[10].pr + rowData[11].pr;
                        auxRowData.tc = rowData[10].tc + rowData[11].tc;
                        auxRowData.car = rowData[10].car + rowData[11].car;
                        auxRowData.ex = rowData[10].ex + rowData[11].ex;
                        auxRowData.hdt = rowData[10].hdt + rowData[11].hdt;
                        auxRowData.otros = rowData[10].otros + rowData[11].otros;
                        break;
                    case 14: 
                        auxRowData.descripcion = "% de Margen"; 
                        auxRowData.tipo_mov = 14;
                        auxRowData.actual = rowData[0].actual != 0 ? rowData[12].actual / rowData[0].actual * (100) : 0;
                        auxRowData.semana2 = rowData[0].semana2 != 0 ? rowData[12].semana2 / rowData[0].semana2 * (100) : 0;
                        auxRowData.semana3 = rowData[0].semana3 != 0 ? rowData[12].semana3 / rowData[0].semana3 * (100) : 0;
                        auxRowData.semana4 = rowData[0].semana4 != 0 ? rowData[12].semana4 / rowData[0].semana4 * (100) : 0;
                        auxRowData.semana5 = rowData[0].semana5 != 0 ? rowData[12].semana5 / rowData[0].semana5 * (100) : 0;
                        auxRowData.cfc = rowData[0].cfc != 0 ? rowData[12].cfc / rowData[0].cfc * (100) : 0;
                        auxRowData.cf = rowData[0].cf != 0 ? rowData[12].cf / rowData[0].cf * (100) : 0;
                        auxRowData.mc = rowData[0].mc != 0 ? rowData[12].mc / rowData[0].mc * (100) : 0;
                        auxRowData.pr = rowData[0].pr != 0 ? rowData[12].pr / rowData[0].pr * (100) : 0;
                        auxRowData.tc = rowData[0].tc != 0 ? rowData[12].tc / rowData[0].tc * (100) : 0;
                        auxRowData.car = rowData[0].car != 0 ? rowData[12].car / rowData[0].car * (100) : 0;
                        auxRowData.ex = rowData[0].ex != 0 ? rowData[12].ex / rowData[0].ex * (100) : 0;
                        auxRowData.hdt = rowData[0].hdt != 0 ? rowData[12].hdt / rowData[0].hdt * (100) : 0;
                        auxRowData.otros = rowData[0].otros != 0 ? rowData[12].otros / rowData[0].otros * (100) : 0;
                        break;
                    default: auxRowData.descripcion = ""; break;
                }
                rowData.push(auxRowData);
            }
            return rowData;
        }

        function AsignarImportesAnalisis(importes, fecha)
        {
            var costos = {actual: 0, semana2: 0, semana3: 0, semana4: 0, semana5: 0, cfc: 0, cf: 0, mc: 0, pr: 0, tc: 0, car: 0, ex: 0, hdt: 0, otros: 0};
            //80-20
            var cfc = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("CFC-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= fecha; }); 
            if (cfc.length > 0) { for(var i = 0; i < cfc.length; i++) { costos.cfc += cfc[i].importe; } }
            var cf = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("CF-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= fecha; });            
            if (cf.length > 0) { for(var i = 0; i < cf.length; i++) { costos.cf += cf[i].importe; } }
            var mc = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("MC-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= fecha; });            
            if (mc.length > 0) { for(var i = 0; i < mc.length; i++) { costos.mc += mc[i].importe; } }
            var pr = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("PR-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= fecha; });            
            if (pr.length > 0) { for(var i = 0; i < pr.length; i++) { costos.pr += pr[i].importe; } }
            var tc = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("TC-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= fecha; });            
            if (tc.length > 0) { for(var i = 0; i < tc.length; i++) { costos.tc += tc[i].importe; } }
            var car = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("CAR-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= fecha; });            
            if (car.length > 0) { for(var i = 0; i < car.length; i++) { costos.car += car[i].importe; } }
            var ex = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("EX-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= fecha; }); 
            if (ex.length > 0) { for(var i = 0; i < ex.length; i++) { costos.ex += ex[i].importe; } }
            var hdt = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("HDT-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= fecha; }); 
            if (hdt.length > 0) { for(var i = 0; i < hdt.length; i++) { costos.dt += hdt[i].importe; } }
            var otros = jQuery.grep(importes, function( n, i ) { return n.noEco.indexOf("CFC-") == -1 && n.noEco.indexOf("CF-") == -1 && n.noEco.indexOf("MC-") == -1 && n.noEco.indexOf("PR-") == -1 && 
                n.noEco.indexOf("TC-") == -1 && n.noEco.indexOf("CAR-") == -1 && n.noEco.indexOf("EX-") == -1 && n.noEco.indexOf("HDT-") == -1 && new Date(parseInt(n.fecha.substr(6))) <= fecha; });            
            if (otros.length > 0) { for(var i = 0; i < otros.length; i++) { costos.otros += otros[i].importe; } }
            //Semanal
            var auxFechaAnalisis = new Date(fecha);
            var actual = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= fecha; });
            if (actual.length > 0) { for(var i = 0; i < actual.length; i++) { costos.actual += actual[i].importe; } }
            auxFechaAnalisis.setDate(auxFechaAnalisis.getDate() - 7);
            var semana2 = jQuery.grep(importes, function( n, i ) { return (new Date(parseInt(n.fecha.substr(6)))) <= auxFechaAnalisis; });            
            if (semana2.length > 0) { for(var i = 0; i < semana2.length; i++) { costos.semana2 += semana2[i].importe; } }
            auxFechaAnalisis.setDate(auxFechaAnalisis.getDate() - 7);
            var semana3 = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFechaAnalisis; });            
            if (semana3.length > 0) { for(var i = 0; i < semana3.length; i++) { costos.semana3 += semana3[i].importe; } }
            auxFechaAnalisis.setDate(auxFechaAnalisis.getDate() - 7);
            var semana4 = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFechaAnalisis; });            
            if (semana4.length > 0) { for(var i = 0; i < semana4.length; i++) { costos.semana4 += semana4[i].importe; } }
            auxFechaAnalisis.setDate(auxFechaAnalisis.getDate() - 7);
            var semana5 = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFechaAnalisis; });            
            if (semana5.length > 0) { for(var i = 0; i < semana5.length; i++) { costos.semana5 += semana5[i].importe; } }
            return costos;
            
        }

        function getKubrixColumns(data, separador) {
            var columnas = [{ data: 'descripcion', title: 'Concepto' }];
            $.each(separador, function (key, value){
                var auxColumna = {
                    data: "separadores",
                    title: '<button class="btn btn-sm btn-light separador" data-toggle="tooltip" data-placement="bottom" title="' + value.trim() + '">' + value + '</button>',
                    render: function (data, type, row, meta) {
                        var importeFinal = data[key];
                        var index = meta.row + 1;
                        //if(index == 1 || index == 2 || index == 3 || index == 4 || index == 7 || index == 9 || index == 10 || 
                        //    index == 11 || index == 12 || index == 13){ importeFinal = importeFinal * (-1); }
                        if (index == 4 || index == 7 || index == 9 || index == 11 || index == 13)
                            return getNumberHTML(importeFinal);
                        if (index == 14)
                            return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                        return getRowHTML(importeFinal);
                    }
                }                
                columnas.push(auxColumna);
            });
            return columnas
        }

        function getRowHTML(value) {
            var auxiliar = '<p' + /*(value != 0 ? */' class="desplegable">'/* : '>')*/ + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
        }

        function getRowHTMLFecha(value, fecha, tipo, semanal) {
            var auxiliar = '<p' + /*(value != 0 ? */' data-fecha="' + fecha + '" data-tipo="' + tipo + '" data-semanal="' + semanal + '" class="desplegable">' /*: '>')*/ + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
        }

        function getNumberHTML(value) {
            return '<p class="' + /*(value != 0 ? */'noDesplegable'/* : '')*/ + (value < 0 ? ' Danger' : '') + '" >' + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
        }

        function getRowHTMLFiltrado(value) {
            var auxiliar = '<p' + /*(value != 0 ? */' class="filtrado">'/* : '>')*/ + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
        }       
        function getRowHTMLFiltradoFecha(value, fecha, tipo, semanal) {
            var auxiliar = '<p' + /*(value != 0 ? */' data-fecha="' + fecha + '" data-tipo="' + tipo + '" class="filtrado">'/* : '>')*/ + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
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

        function getBusquedaDTOK() {
            var notSelected = $("#comboAC").find('option').not(':selected');
            var Selected = $("#comboAC").find("option:selected");
            var array = notSelected.map(function () {
                return this.value;
            }).get();
            var arrayEnkontrolSelected = Selected.map(function () {
                return $(this).attr("data-Prefijo");
            }).get();
            var arrayEnkontrolNotSelected = notSelected.map(function () {
                return $(this).attr("data-Prefijo");
            }).get();
            return { 
                obra: (responsable && comboAC.val().length < 1) ? array : comboAC.val()
                , fechaInicio: inputDiaInicio.val()
                , fechaFin: inputDiaFinal.val()
                , cta: 0
                , scta: 0
                , sscta: 0
                , economico: cboMaquina.val() == "" ? null : $("#cboMaquina option:selected").text().trim()
                , lstModelo: comboModeloK.val()
                , tipoEquipo: 0
                , tipoEquipoMayor : 0
                , ccEnkontrol: (responsable && comboAC.val().length < 1) ? arrayEnkontrolNotSelected : arrayEnkontrolSelected
                , tipoIntervalo: tipoIntervaloGlobal == 1
            };
        }

        function getBusquedaDTODetalle() {
            return { 
                fechaFin: botonBuscar.attr("data-fechaFin")
            };
        }

        function abrirDetalleEquipo()
        {
            var idTipo = $(this).attr("data-id");
            switch (idTipo) {
                case 1:
                case 2:
                case 3:
                    $("#comboTipoAnalisis").val(idTipo);
                    break;
                default:
                    $("#comboTipoAnalisis").val("TODOS");
                    break;
            }
            if (dtTablaKubrixDetalle.data().any())
                modalAnalisis.modal("show");
        }

        //--> Inicializadores de tablas detalle principal
        function initTablaSubCuenta() {

            dtTablaSubCuenta = tablaSubCuenta.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0); } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[0, 'asc'], [1, 'asc']],
                drawCallback: function () {
                    tablaSubCuenta.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaSubCuenta.row($(this).parents('tr')).data();
                        var fecha = $(this).attr("data-fecha");
                        var tipo = $(this).attr("data-tipo");
                        cargarTablaSubSubCuenta(rowData.detalles, rowData.descripcion, $(this).html(), fecha, tipo);
                        botonTablaSubSubCuenta.show();
                        botonTablaSubCuenta.prop("disabled", false);
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(3).footer()).html('TOTAL');
                        $(api.column(4).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            cargarTablaSubSubCuenta(totalizador[0].detalles, totalizador[0].descripcion, 0, fecha, tipo);
                            botonTablaSubSubCuenta.show();
                            botonTablaSubCuenta.prop("disabled", false);
                        });

                    }
                }
            });
        }

        function initTablaSubSubCuenta() {

            dtTablaSubSubCuenta = tablaSubSubCuenta.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    //{ data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'sscta', title: 'SubSubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0); }}
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                order: [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                drawCallback: function () {
                    tablaSubSubCuenta.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaSubSubCuenta.row($(this).parents('tr')).data();
                        //if(rowData.cta < 5000) {
                        cargarTablaDivision(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                        botonTablaDivision.show();
                        botonTablaSubSubCuenta.prop("disabled", false);
                        //}
                        //else {
                        //    cargarTablaDetalle(rowData.detalles, rowData.descripcion, $(this).html(), false);
                        //    botonTablaDetalle.show();
                        //    botonTablaSubSubCuenta.prop("disabled", false);
                        //}
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(4).footer()).html('TOTAL');
                        $(api.column(5).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            //if(totalizador[0].cta < 5000) {
                            cargarTablaDivision(totalizador[0].detalles, totalizador[0].descripcion, 0, totalizador[0].cta);
                            botonTablaDivision.show();
                            botonTablaSubSubCuenta.prop("disabled", false);
                            //}
                            //else {
                            //    cargarTablaDetalle(totalizador[0].detalles, totalizador[0].descripcion, 0, false);
                            //    botonTablaDetalle.show();
                            //    botonTablaSubSubCuenta.prop("disabled", false);
                            //}
                        });
                    }
                }
            });
        }

        function initTablaDivision() {

            dtTablaDivision = tablaDivision.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'División' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0); } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[2, 'desc'], [1, 'asc']],
                drawCallback: function () {
                    tablaDivision.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();                        
                        const rowData = dtTablaDivision.row($(this).parents('tr')).data();
                        var numAreasCuenta = botonBuscar.attr("data-obra").split(',').length;
                        if(numAreasCuenta == 1 && botonBuscar.attr("data-obra") != "")
                        {
                            if(rowData.cta < 5000)
                            {
                                cargarTablaConciliacion(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                                botonTablaConciliacion.show();
                                botonTablaDivision.prop("disabled", false);
                            }
                            else
                            {
                                cargarTablaEconomico(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                                botonTablaEconomico.show();
                                botonTablaDivision.prop("disabled", false);
                            }
                        }
                        else
                        {
                            cargarTablaAreaCuenta(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                            botonTablaAreaCuenta.show();
                            botonTablaDivision.prop("disabled", false);
                        }
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(2).footer()).html('TOTAL');
                        $(api.column(3).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var numAreasCuenta = botonBuscar.attr("data-obra").split(',').length;
                            if(numAreasCuenta == 1 && botonBuscar.attr("data-obra") != "")
                            {
                                if(totalizador[0].cta < 5000)
                                {
                                    cargarTablaConciliacion(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta);
                                    botonTablaConciliacion.show();
                                    botonTablaDivision.prop("disabled", false);
                                }
                                else
                                {
                                    cargarTablaEconomico(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta);
                                    botonTablaEconomico.show();
                                    botonTablaDivision.prop("disabled", false);
                                }
                            }
                            else
                            {
                                cargarTablaAreaCuenta(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta);
                                botonTablaAreaCuenta.show();
                                botonTablaDivision.prop("disabled", false);
                            }
                        });

                    }
                }
            });
        }

        function initTablaAreaCuenta() {

            dtTablaAreaCuenta = tablaAreaCuenta.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'Area Cuenta' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0); } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[2, 'desc'], [1, 'asc']],
                drawCallback: function () {
                    tablaAreaCuenta.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaAreaCuenta.row($(this).parents('tr')).data();
                        //cargarTablaConciliacion(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                        //botonTablaConciliacion.show();
                        //botonTablaAreaCuenta.prop("disabled", false);
                        if(rowData.cta < 5000)
                        {
                            cargarTablaConciliacion(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                            botonTablaConciliacion.show();
                            botonTablaAreaCuenta.prop("disabled", false);
                        }
                        else
                        {
                            cargarTablaEconomico(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                            botonTablaEconomico.show();
                            botonTablaAreaCuenta.prop("disabled", false);
                        }
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(2).footer()).html('TOTAL');
                        $(api.column(3).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            if(totalizador[0].cta < 5000)
                            {
                                cargarTablaConciliacion(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta);
                                botonTablaConciliacion.show();
                                botonTablaAreaCuenta.prop("disabled", false);
                            }
                            else
                            {
                                cargarTablaEconomico(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta);
                                botonTablaEconomico.show();
                                botonTablaAreaCuenta.prop("disabled", false);
                            }
                        });
                    }
                }
            });
        }

        function initTablaConciliacion() {

            dtTablaConciliacion = tablaConciliacion.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'Conciliación' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0); } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[2, 'desc'], [1, 'asc']],
                drawCallback: function () {
                    tablaConciliacion.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaConciliacion.row($(this).parents('tr')).data();
                        cargarTablaEconomico(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                        botonTablaEconomico.show();
                        botonTablaConciliacion.prop("disabled", false);
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(2).footer()).html('TOTAL');
                        $(api.column(3).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            cargarTablaEconomico(totalizador[0].detalles, totalizador[0].descripcion, 0, totalizador[0].cta);
                            botonTablaEconomico.show();
                            botonTablaConciliacion.prop("disabled", false);
                        });
                    }
                }
            });
        }

        function initTablaEconomico() {
            dtTablaEconomico = tablaEconomico.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'Económico' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0); } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[2, 'desc'], [1, 'asc']],
                drawCallback: function () {
                    tablaEconomico.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaEconomico.row($(this).parents('tr')).data();
                        cargarTablaPoliza(rowData.detalles, rowData.descripcion, $(this).html());
                        botonTablaPoliza.show();
                        botonTablaEconomico.prop("disabled", false);
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(2).footer()).html('TOTAL');
                        $(api.column(3).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            cargarTablaPoliza(totalizador[0].detalles, totalizador[0].descripcion, 0);
                            botonTablaPoliza.show();
                            botonTablaEconomico.prop("disabled", false);
                        });
                    }
                }
            });
        }

        function initTablaPoliza() {
            dtTablaPoliza = tablaPoliza.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'Poliza' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0); } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[2, 'desc'], [1, 'asc']],
                drawCallback: function () {
                    tablaPoliza.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaPoliza.row($(this).parents('tr')).data();
                        cargarTablaDetalle(rowData.detalles, rowData.descripcion, $(this).html());
                        botonTablaDetalle.show();
                        botonTablaPoliza.prop("disabled", false);
                    });
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(2).footer()).html('TOTAL');
                        $(api.column(3).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            cargarTablaDetalle(totalizador[0].detalles, totalizador[0].descripcion, 0);
                            botonTablaDetalle.show();
                            botonTablaPoliza.prop("disabled", false);
                        });
                    }
                }
            });
        }

        function initTablaDetalles() {

            dtTablaDetalles = tablaDetalles.DataTable({
                language: dtDicEsp,
                destroy: true,                
                scrollY: "500px",
                scrollCollapse: true,
                paging: true,
                pageLength: 100,
                columns: [
                    { data: 'poliza', title: 'Poliza' },
                    { data: 'linea', title: 'Linea' },
                    { data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'noEco', title: 'CC' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return "<p>" + maskNumero(data) + "</p>" } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: "15%", "targets": [0, 1] },
                    { width: "25%", "targets": [2, 3] },
                    { width: "20%", "targets": [4] }
                ],
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = (api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 ))
                    $(api.column(4).footer()).html('TOTAL');
                    $(api.column(5).footer()).html('$' + parseFloat(total).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                }
            });
        }

        function stripHtml(html)
        {
            var tmp = document.createElement("DIV");
            tmp.innerHTML = html;
            return tmp.textContent || tmp.innerText || "";
        }

        //--> Carga de tablas detalle principal
        function cargarTablaSubCuenta(detalles, nombreRow, total, nombreColumna, fecha, tipo) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return;
            }
            const grouped = groupBy(detalles, function(detalle) { return detalle.tipoInsumo_Desc + " " + (detalle.empresa == 2 ? "ARRENDADORA" : "CONSTRUPLAN"); });

            dtTablaSubCuenta.clear();
            Array.from(grouped, function([key, value]) {
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const id = value[0].tipoInsumo;
                const descripcion = (value[0].tipoInsumo == "5000-10" || value[0].tipoInsumo == "5900-3") ? key : value[0].tipoInsumo_Desc;
                const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
                const grupo = { cta: cta, scta: scta, id: id, descripcion: descripcion, importe: importe, detalles: value, fecha: fecha, tipo: tipo };
                dtTablaSubCuenta.row.add(grupo);
            });
            const importeTotalizador = detalles.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
            const totalizador = { cta: detalles[0].cta, scta: 0, id: "-1", descripcion: "TOTAL", importe: importeTotalizador, detalles: detalles, fecha: fecha, tipo: tipo }
            dtTablaSubCuenta.row.add(totalizador);
            dtTablaSubCuenta.draw();
            $("#botonTablaSubCuenta strong").text(stripHtml(nombreRow).toUpperCase());
            dtTablaSubCuenta.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            modalDetallesK.modal('show');
        }

        function cargarTablaSubSubCuenta(detalles, nombreColumna, total, fecha, tipo) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return;
            }
            const grouped = groupBy(detalles, function(detalle) { return detalle.grupoInsumo; });

            dtTablaSubSubCuenta.clear();
            Array.from(grouped, function ([key, value]) {
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const sscta = parseInt(auxCuenta[2]);
                const id = value[0].grupoInsumo;
                const descripcion = value[0].grupoInsumo_Desc;
                const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                const grupo = { cta: cta, scta: scta, sscta: sscta, id: id, descripcion: descripcion, importe: importe, detalles: value, fecha: fecha, tipo: tipo };
                dtTablaSubSubCuenta.row.add(grupo);
            });
            const importeTotalizador = detalles.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
            const totalizador = { cta: detalles[0].cta, scta: 0, sscta: 0,  id: "-1", descripcion: "TOTAL", importe: importeTotalizador, detalles: detalles, fecha: fecha, tipo: tipo }
            dtTablaSubSubCuenta.row.add(totalizador);
            $("#botonTablaSubSubCuenta strong").text(nombreColumna.toUpperCase());

            dtTablaSubSubCuenta.draw();
            dtTablaSubSubCuenta.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            HideTablas();
            divTablaSubSubCuenta.show(500);
        }

        function cargarTablaDivision(detalles, nombreColumna, total, cuenta) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return;
            }   
            const grouped = groupBy(detalles, function(detalle) { return detalle.division; });
            dtTablaDivision.clear();
            Array.from(grouped, function ([key, value]) {
                const cta = value[0].cta;
                const id = value[0].division;
                const descripcion = value[0].division;
                const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                const grupo = { cta: cta, id: id, descripcion: descripcion, importe: importe, detalles: value };
                dtTablaDivision.row.add(grupo);
            });
            const importeTotalizador = detalles.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
            const totalizador = { cta: detalles[0].cta, id: "-1", descripcion: "TOTAL", importe: importeTotalizador, detalles: detalles }
            dtTablaDivision.row.add(totalizador);
            $("#botonTablaDivision strong").text(nombreColumna.toUpperCase());

            dtTablaDivision.draw();
            dtTablaDivision.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            HideTablas();
            divTablaDivision.show(500);
        }

        function cargarTablaAreaCuenta(detalles, nombreColumna, total, cuenta) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return;
            }
            
            //if(cuenta < 5000)
            //{
            //    const grouped = groupBy(detalles, function(detalle) { return detalle.referencia; });

            //    dtTablaAreaCuenta.clear();
            //    Array.from(grouped, function ([key, value]) {
            //        const id = value[0].referencia;
            //        const descripcion = value[0].referencia;
            //        const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
            //        const grupo = { id: id, descripcion: descripcion, importe: importe, detalles: value };
            //        dtTablaAreaCuenta.row.add(grupo);
            //    });
            //    const importeTotalizador = detalles.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
            //    const totalizador = { id: "-1", descripcion: "TOTAL", importe: importeTotalizador, detalles: detalles }
            //    dtTablaAreaCuenta.row.add(totalizador);
            //    $("#botonTablaAreaCuenta strong").text(nombreColumna.toUpperCase());

            //    dtTablaAreaCuenta.draw();
            //    dtTablaAreaCuenta.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            //    divTablaDivision.hide(500);
            //    divTablaAreaCuenta.show(500);
            //}
            //else
            //{
            const grouped = groupBy(detalles, function(detalle) { return detalle.areaCuenta; });

            dtTablaAreaCuenta.clear();
            Array.from(grouped, function ([key, value]) {
                const cta = value[0].cta;
                const id = value[0].areaCuenta;
                const descripcion = value[0].areaCuenta;
                const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                const grupo = { cta: cta, id: id, descripcion: descripcion, importe: importe, detalles: value };
                dtTablaAreaCuenta.row.add(grupo);
            });
            const importeTotalizador = detalles.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
            const totalizador = { cta: detalles[0].cta, id: "-1", descripcion: "TOTAL", importe: importeTotalizador, detalles: detalles }
            dtTablaAreaCuenta.row.add(totalizador);
            $("#botonTablaAreaCuenta strong").text(nombreColumna.toUpperCase());

            dtTablaAreaCuenta.draw();
            dtTablaAreaCuenta.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            HideTablas();
            divTablaAreaCuenta.show(500);
            //}

        }

        function cargarTablaConciliacion(detalles, nombreColumna, total, cuenta) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return;
            }   
            const grouped = groupBy(detalles, function(detalle) { return detalle.referencia; });
            dtTablaConciliacion.clear();
            Array.from(grouped, function ([key, value]) {
                const cta = value[0].cta;
                const id = value[0].referencia;
                const descripcion = value[0].referencia;
                const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                const grupo = { cta: cta, id: id, descripcion: descripcion, importe: importe, detalles: value };
                dtTablaConciliacion.row.add(grupo);
            });
            const importeTotalizador = detalles.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
            const totalizador = { cta: detalles[0].cta, id: "-1", descripcion: "TOTAL", importe: importeTotalizador, detalles: detalles }
            dtTablaConciliacion.row.add(totalizador);
            $("#botonTablaConciliacion strong").text(nombreColumna.toUpperCase());

            dtTablaConciliacion.draw();
            dtTablaConciliacion.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            HideTablas();
            divTablaConciliacion.show(500);
        }

        function cargarTablaEconomico(detalles, nombreColumna, total, cuenta) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return;
            }   
            const grouped = groupBy(detalles, function(detalle) { return detalle.noEco; });
            dtTablaEconomico.clear();
            Array.from(grouped, function ([key, value]) {
                const cta = value[0].cta;
                const id = value[0].noEco;
                const descripcion = value[0].noEco;
                const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                const grupo = { cta: cta, id: id, descripcion: descripcion, importe: importe, detalles: value };
                dtTablaEconomico.row.add(grupo);
            });
            const importeTotalizador = detalles.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
            const totalizador = { cta: detalles[0].cta, id: "-1", descripcion: "TOTAL", importe: importeTotalizador, detalles: detalles }
            dtTablaEconomico.row.add(totalizador);
            $("#botonTablaEconomico strong").text(nombreColumna.toUpperCase());

            dtTablaEconomico.draw();
            dtTablaEconomico.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            HideTablas();
            divTablaEconomico.show(500);
        }

        function cargarTablaPoliza(detalles, nombreColumna, total, cuenta) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return;
            }   
            const grouped = groupBy(detalles, function(detalle) { return detalle.poliza; });
            dtTablaPoliza.clear();
            Array.from(grouped, function ([key, value]) {
                const cta = value[0].cta;
                const id = value[0].poliza;
                const descripcion = value[0].poliza;
                const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                const grupo = { cta: cta, id: id, descripcion: descripcion, importe: importe, detalles: value };
                dtTablaPoliza.row.add(grupo);
            });
            const importeTotalizador = detalles.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe});
            const totalizador = { cta: detalles[0].cta, id: "-1", descripcion: "TOTAL", importe: importeTotalizador, detalles: detalles }
            dtTablaPoliza.row.add(totalizador);
            $("#botonTablaPoliza strong").text(nombreColumna.toUpperCase());

            dtTablaPoliza.draw();
            dtTablaPoliza.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            HideTablas();
            divTablaPoliza.show(500);
        }

        function cargarTablaDetalle(subdetalles, descripcion, total) {
            subdetalles = subdetalles.map(function(x) {
                return {
                    poliza: x.poliza,
                    linea: x.linea,
                    noEco: x.noEco,
                    fecha: moment(x.fecha).toDate().toLocaleDateString('en-GB').Capitalize(),
                    descripcion: x.insumo_Desc,
                    importe: x.importe
                };
            });
            if(descripcion == null){ descripcion = "";}
            $("#botonTablaDetalle strong").text(descripcion.toUpperCase());
            dtTablaDetalles.clear().rows.add(subdetalles).draw();
            HideTablas();
            divTablaDetalles.show(500);    

            divTablaDetalles.show(500, function(){
                dtTablaDetalles.columns.adjust()
            });
        }

        function HideTablas()
        {
            if(divTablaSubCuenta.is(":visible")) divTablaSubCuenta.hide(500);
            if(divTablaSubSubCuenta.is(":visible")) divTablaSubSubCuenta.hide(500);
            if(divTablaDivision.is(":visible")) divTablaDivision.hide(500);
            if(divTablaAreaCuenta.is(":visible")) divTablaAreaCuenta.hide(500);
            if(divTablaConciliacion.is(":visible")) divTablaConciliacion.hide(500);
            if(divTablaEconomico.is(":visible")) divTablaEconomico.hide(500);
            if(divTablaPoliza.is(":visible")) divTablaPoliza.hide(500);
            if(divTablaDetalles.is(":visible")) divTablaDetalles.hide(500);
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
        
        function cargarDatosTablaAnalisis(data, tipo, equipoMayor) {            
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
                    CargarGraficaLineas(data, tipo, "", true);
                    tablaAnalisis.find('p.desplegable').unbind().click(function () {                        
                        const p = $(this);
                        var fechaMax = new Date(botonBuscar.attr("data-fechaFin"));
                        const rowData = dtTablaAnalisis.row(p.parents('tr')).data();
                        const td = p.parents("td");
                        columnaAnalisis = td.index() - 2;
                        botonBuscar.attr("data-tipoMayor", columnaAnalisis);
                        const nombreColumna = rowData.descripcion.trim();
                        var auxFecha = new Date(fechaMax);
                        cargarSubCuenta(rowData.detalles, nombreColumna, p.html(), td.index(), tipo, auxFecha);
                        dtTablaSctaDetalles.columns.adjust();
                        botonNombreNivelCero.click();
                    });
                    $('#tablaAnalisis tbody').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var auxData = dtTablaAnalisis.row(this).data();
                        CargarGraficaLineas(data, tipo, auxData.descripcion, false);
                    });
                    $('#tablaAnalisis thead').on('click', 'tr', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        CargarGraficaLineas(data, tipo, "", true);
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( data.tipo_mov == 4 || data.tipo_mov == 7 || data.tipo_mov == 9 || data.tipo_mov == 11 || data.tipo_mov == 13 || data.tipo_mov == 14) { $(row).addClass('resultado'); }
                }
            });
            if(equipoMayor == 1)
            {
                $("div.chkGrafica") .html('<input type="checkbox" checked data-toggle="toggle" data-on="Detalles" data-off="Gráfica" data-onstyle="success" data-offstyle="info" id="chbGrafica">' +
                    '&nbsp;&nbsp;&nbsp;<input type="checkbox" ' + (tipo == 0 ? "checked" : "") + ' data-toggle="toggle" data-on="80-20" data-off="Semanal" data-onstyle="success" data-offstyle="info" id="chb8020">');
            }
            else { $("div.chkGrafica").html('<input type="checkbox" checked data-toggle="toggle" data-on="Detalles" data-off="Gráfica" data-onstyle="success" data-offstyle="info" id="chbGrafica">'); }            
            $('div.chkGrafica input').bootstrapToggle();
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
                if($(this).is(":checked")) { cargarDatosTablaAnalisis(datosTabla, 0, 1); }
                else { cargarDatosTablaAnalisis(datosTabla, 1, 1); }
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
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'semana2', title: '<span data-toggle="tooltip" title="Semana 2" data-placement="bottom">Semana 2</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'semana3', title: '<span data-toggle="tooltip" title="Semana 3" data-placement="bottom">Semana 3</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'semana4', title: '<span data-toggle="tooltip" title="Semana 4" data-placement="bottom">Semana 4</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'semana5', title: '<span data-toggle="tooltip" title="Semana 5" data-placement="bottom">Semana 5</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
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
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'cfc', title: '<span data-toggle="tooltip" title="CFC" data-placement="bottom">CFC</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'cf', title: '<span data-toggle="tooltip" title="CF" data-placement="bottom">CF</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'mc', title: '<span data-toggle="tooltip" title="MC" data-placement="bottom">MC</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'pr', title: '<span data-toggle="tooltip" title="PR" data-placement="bottom">PR</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'tc', title: '<span data-toggle="tooltip" title="TC" data-placement="bottom">TC</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'car', title: '<span data-toggle="tooltip" title="CAR" data-placement="bottom">CAR</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'ex', title: '<span data-toggle="tooltip" title="EX" data-placement="bottom">EX</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'hdt', title: '<span data-toggle="tooltip" title="HDT" data-placement="bottom">HDT</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                    , { data: 'otros', title: '<span data-toggle="tooltip" title="Otros" data-placement="bottom">Otros</span>',                         
                        render: function (data, type, row) { 
                            var importeFinal = data
                            if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                return getNumberHTML(importeFinal);
                            if (row.tipo_mov == 14)
                                return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                            return getRowHTML(importeFinal);
                        }  
                    }
                ];
            }
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
                    { data: 'importe', title: 'Semana', render: function (data, type, row) { return getRowHTMLFiltradoFecha(data, row.fecha, row.tipo); } },
                    { data: 'acumulado', title: 'Acumulado', render: function (data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0); } },
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
                        const rowData = dtTablaSctaDetalles.row($(this).parents('tr')).data();
                        var fecha = $(this).attr("data-fecha");
                        var tipo = $(this).attr("data-tipo");
                        cargarDetScta(rowData.detalles, rowData.descripcion, $(this).html(), fecha, tipo);
                        botonNombreNivelUno.show();
                        botonNombreNivelCero.prop("disabled", false);
                    });
                    tablaSctaDetalles.find('p.filtrado').click(function (e) {
                        const rowData = dtTablaSctaDetalles.row($(this).parents('tr')).data();
                        var fecha = $(this).attr("data-fecha");
                        var tipo = $(this).attr("data-tipo");
                        cargarDetSctaFiltrado(rowData.detallesSemana, rowData.descripcion, $(this).html(), fecha, tipo);
                        botonNombreNivelUno.show();  
                        botonNombreNivelCero.prop("disabled", false);
                    });
                    tablaSctaDetalles.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaSctaDetalles.row($(this).parents('tr')).data();
                        const fechaMaxGrafica = $(this).parents('tr').find('.desplegable').attr('data-fecha');
                        var datosGrafica = [];

                        var dateMaxGrafica = new Date(fechaMaxGrafica);
                        var suma = 0;
                        for(var i = 0; i < 5; i++) {
                            suma = 0;
                            auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) {
                                return new Date(parseInt(n.fecha.substr(6))) <= dateMaxGrafica;
                            });
                            dateMaxGrafica.setDate(dateMaxGrafica.getDate() - 7);
                            auxGrafica.forEach(function(n) { suma += n.importe; });
                            datosGrafica.push([(4 - i), suma]);
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
                    total = (api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalAcumulado = (api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalPorcentaje = api.column(7).data().reduce( function (a, b) { return intVal(parseFloat(a, 10)) + intVal(parseFloat(b, 10)); }, 0 );
                    $(api.column(4).footer()).html('TOTAL');
                    $(api.column(5).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(6).footer()).html('$' + parseFloat(totalAcumulado).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(7).footer()).html(parseFloat(totalPorcentaje).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '%');
                }
            });
            $("#tablaSctaDetalles.inputTitulo").html('<span id="tituloSctaDetalles">' +  + '</span>');
        }

        function initTablaDetallesA() {
            dtTablaDetallesA = tablaDetallesA.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                dom: '<"col-xs-12 col-sm-12 col-md-6 col-lg-6 inputTitulo"><f<t>>',
                columns: [
                    //{ data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'sscta', title: 'SubSubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta', visible: false },
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, row.semanal); } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaDetallesA_filter input').addClass("form-control input-sm");
                },
                drawCallback: function () {
                    tablaDetallesA.find('p.desplegable').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaDetallesA.row($(this).parents('tr')).data();
                        mostrarSubdetalles(rowData.detalles, rowData.descripcion, $(this).html()); 
                        botonNombreNivelDos.show();
                        botonNombreNivelUno.prop("disabled", false);
                    });                    
                    tablaDetallesA.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaDetallesA.row($(this).parents('tr')).data();
                        const fechaMaxGrafica = $(this).parents('tr').find('.desplegable').attr('data-fecha');
                        const semanal = $(this).parents('tr').find('.desplegable').attr("data-semanal");
                        var datosGrafica = [];

                        var dateMaxGrafica = new Date(fechaMaxGrafica);
                        var dateMinGrafica = new Date(dateMaxGrafica);
                        dateMinGrafica.setDate(dateMinGrafica.getDate() - 7);
                        var suma = 0;
                        if(semanal == 1) {
                            for(var i = 0; i < 5; i++) {
                                suma = 0;
                                auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= dateMaxGrafica && new Date(parseInt(n.fecha.substr(6))) > dateMinGrafica; });                                
                                dateMaxGrafica.setDate(dateMaxGrafica.getDate() - 7);
                                dateMinGrafica.setDate(dateMinGrafica.getDate() - 7);
                                auxGrafica.forEach(function(n) { suma += n.importe; });
                                datosGrafica.push([(4 - i), suma]);
                            }
                        }
                        else {
                            for(var i = 0; i < 5; i++) {
                                suma = 0;
                                auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= dateMaxGrafica; });                                
                                dateMaxGrafica.setDate(dateMaxGrafica.getDate() - 7);
                                auxGrafica.forEach(function(n) { suma += n.importe; });
                                datosGrafica.push([(4 - i), suma]);
                            }
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
            $("#tablaDetallesA.inputTitulo").html('<span id="tituloDetalles">' +  + '</span>');
        }

        function initTablaSubdetalles() {
            dtTablaSubdetalles = tablaSubdetalles.DataTable({
                language: dtDicEsp,
                destroy: true,      
                paging: true,
                pageLength: 100,
                dom: '<"col-xs-12 col-sm-12 col-md-6 col-lg-6 inputTitulo"><f<t>>',
                columns: [
                    { data: 'poliza', title: 'Poliza' },
                    { data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'noEco', title: 'CC' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'importe', title: 'Importe', render: function(data, type, row) { return "<p>" + maskNumero(data) + "</p>" } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: "15%", "targets": [0, 1] },
                    { width: "25%", "targets": [2, 3] },
                    { width: "20%", "targets": [4] }
                ],
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaSubdetalles_filter input').addClass("form-control input-sm");
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    total = (api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 ))
                    $(api.column(3).footer()).html('TOTAL');
                    $(api.column(4).footer()).html('$' + parseFloat(total).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                }
            });
            $("#tablaSubdetalles.inputTitulo").html('<span id="tituloSubDetalles"></span>');
        }
        
        function cargarSubCuenta(detalles, nombreColumna, total, columna, tipo, fecha) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral('Aviso', 'Ocurrió un error al ver los detalles de este registros.');
                return;
            }
            let detallesFiltrados;
            if(tipo == 1) {         
                dtTablaSctaDetalles.column(5).visible(false);
                dateMax = new Date(fecha);
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
                    case 8: // EX
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("EX-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("EX");
                        break;
                    case 9: // HDT
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("HDT-") >= 0 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("HDT");
                        break;
                    case 10: // OTROS
                        detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                            return n.noEco.indexOf("CFC-") == -1 && n.noEco.indexOf("CF-") == -1 && n.noEco.indexOf("MC-") == -1 && n.noEco.indexOf("PR-") == -1 
                                && n.noEco.indexOf("TC-") == -1 && n.noEco.indexOf("CAR-") == -1 && n.noEco.indexOf("EX-") == -1 && n.noEco.indexOf("HDT-") == -1 && new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                        });
                        $("#tituloSctaDetalles").text("Otros");
                        break;
                    default:
                        $("#tituloSctaDetalles").text("");
                        return;
                }
                const grouped = groupBy(detallesFiltrados, function(detallesFiltrados) { return detallesFiltrados.tipoInsumo; });
                const totalDetalle = detallesFiltrados.length > 0 ? detallesFiltrados.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; }) : 0;
                dtTablaSctaDetalles.clear();
                Array.from(grouped, function([key, value]) {
                    var auxCuenta = value[0].tipoInsumo.split('-');
                    const cta = value[0].cta;
                    const scta = parseInt(auxCuenta[1]);
                    const id = value[0].tipoInsumo;
                    const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
                    const descripcion = value[0].tipoInsumo_Desc;
                    const importe = detallesFiltrados.length > 0 ? detallesFiltrados.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; }) : 0;
                    const acumulado = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                    const porcentajeNum = (acumulado / totalDetalle) * 100;
                    const grupo = { cta: cta, scta: scta, id: id, grafica: grafica, descripcion: descripcion, importe: importe, detalles: value, detallesSemana: value, acumulado: acumulado, porcentaje: porcentajeNum, tipo: tipo, fecha: fecha };
                    dtTablaSctaDetalles.row.add(grupo);
                });
                dtTablaSctaDetalles.draw();
                $("#botonNombreNivelCero strong").text(nombreColumna.toUpperCase());
            }
            else {
                dtTablaSctaDetalles.column(5).visible(true);
                dateMax = new Date(fecha);
                switch (columna) {
                    case 1: // Actual
                        $("#tituloSctaDetalles").text("Actual");
                        break;
                    case 2: // Semana 2
                        dateMax.setDate(dateMax.getDate() - 7);
                        $("#tituloSctaDetalles").text("Semana 2");
                        break;
                    case 3: // Semana 3
                        dateMax.setDate(dateMax.getDate() - 14);
                        $("#tituloSctaDetalles").text("Semana 3");
                        break;
                    case 4: // Semana 4
                        dateMax.setDate(dateMax.getDate() - 21);
                        $("#tituloSctaDetalles").text("Semana 4");
                        break;
                    case 5: // Semana 5
                        dateMax.setDate(dateMax.getDate() - 28);
                        $("#tituloSctaDetalles").text("Semana 5");
                        break;
                    default:
                        return;
                }
                var dateMin = new Date(dateMax);
                dateMin.setDate(dateMin.getDate() - 7);
                const grouped = groupBy(detalles, function(detalle) { return detalle.tipoInsumo; });
                detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
                    return new Date(parseInt(n.fecha.substr(6))) <= dateMax;
                });
                const totalDetalle = detallesFiltrados.length > 0 ? detallesFiltrados.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; }) : 0;      
                dtTablaSctaDetalles.clear();
                Array.from(grouped, function([key, value])  {                    
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
                    const importe = detallesFiltradosSemanal.length > 0 ? detallesFiltradosSemanal.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; }) : 0;
                    const acumulado = detallesFiltrados.length > 0 ? detallesFiltrados.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; }) : 0;
                    const porcentajeNum = (importe / totalDetalle) * 100
                    const grupo = { cta: cta, scta: scta, id: id, grafica: grafica, descripcion: descripcion, importe: importe, detalles: detallesFiltrados, detallesSemana: detallesFiltradosSemanal, acumulado: acumulado, porcentaje: porcentajeNum, fecha: dateMax, tipo: tipo/*, detallesSemanal: detallesFiltradosSemanal*/ };
                    dtTablaSctaDetalles.row.add(grupo);
                });
            }
            dtTablaSctaDetalles.draw();
            $("#botonNombreNivelCero strong").text(nombreColumna.toUpperCase());            
        }
        
        function cargarDetScta(detalles, nombreColumna, total, fecha, tipo) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral('Aviso', 'Ocurrió un error al ver los detalles de este registros.');
                return;
            }
            const grouped = groupBy(detalles, function(detalle) { return detalle.grupoInsumo; });
            dtTablaDetallesA.clear();
            Array.from(grouped, function([key, value])  {
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const sscta = parseInt(auxCuenta[2]);
                const id = value[0].grupoInsumo;
                const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
                const descripcion = value[0].grupoInsumo_Desc;
                const importe = value.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                const grupo = { cta: cta, scta: scta, sscta: sscta, id: id, grafica: grafica, descripcion: descripcion, importe: importe, detalles: value, fecha: fecha, tipo: tipo, semanal: 0 };
                dtTablaDetallesA.row.add(grupo);
            });
            $("#botonNombreNivelUno strong").text("ACUMULADO: " + nombreColumna.toUpperCase());
            $("#tituloDetalles").text($("#tituloSctaDetalles").text());
            dtTablaDetalles.draw();
            divTablaNivelCero.hide(500);
            divTablaNivelUno.show(500);
        }

        function cargarDetSctaFiltrado(detalles, nombreColumna, total, fecha, tipo) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral('Aviso', 'Ocurrió un error al ver los detalles de este registros.');
                return;
            }
            dateMax = new Date(fecha);
            var dateMin = new Date(dateMax);
            dateMin.setDate(dateMin.getDate() - 7);            
            //var detallesFiltrados = jQuery.grep(detalles, function( n, i ) {
            //    return new Date(parseInt(n.fecha.substr(6))) <= dateMax && new Date(parseInt(n.fecha.substr(6))) > dateMin;
            //});
            const grouped = groupBy(detalles, function(detalle) { return detalle.grupoInsumo; });
            dtTablaDetallesA.clear();
            Array.from(grouped, function([key, value])  {
                var detallesFiltrados = jQuery.grep(value, function( n, i ) {
                    return new Date(parseInt(n.fecha.substr(6))) <= dateMax && new Date(parseInt(n.fecha.substr(6))) > dateMin;
                });
                var auxCuenta = value[0].grupoInsumo.split('-');
                const cta = value[0].cta;
                const scta = parseInt(auxCuenta[1]);
                const sscta = parseInt(auxCuenta[2]);
                const id = value[0].grupoInsumo;
                const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
                const descripcion = value[0].grupoInsumo_Desc;
                const importe = detallesFiltrados.map(function(x) { return x.importe; }).reduce(function(total, importe) { return total + importe; });
                const grupo = { cta: cta, scta: scta, sscta: sscta, id: id, grafica: grafica, descripcion: descripcion, importe: importe, detalles: value, fecha: fecha, tipo: tipo, semanal: 1 };
                dtTablaDetallesA.row.add(grupo);
            });
            $("#botonNombreNivelUno strong").text("SEMANAL: " + nombreColumna.toUpperCase());
            $("#tituloDetalles").text($("#tituloSctaDetalles").text());
            dtTablaDetallesA.draw();
            divTablaNivelCero.hide(500);
            divTablaNivelUno.show(500);
        }
        
        function mostrarSubdetalles(subdetalles, descripcion, total) {
            subdetalles = subdetalles.map(function(x) {
                return {
                    poliza: x.poliza,
                    noEco: x.noEco,
                    fecha: moment(x.fecha).toDate().toLocaleDateString('en-GB').Capitalize(),
                    descripcion: x.insumo_Desc,
                    importe: x.importe
                };
            });
            $("#botonNombreNivelDos strong").text(descripcion.toUpperCase());
            $("#tituloSubDetalles").text($("#tituloDetalles").text());
            dtTablaSubdetalles.clear().rows.add(subdetalles).draw();
            divTablaNivelUno.hide(500);
            divTablaNivelDos.show(500);    

            divTablaNivelDos.show(500, function(){
                dtTablaSubdetallesK.columns.adjust()
            });
        }

        function CargarGraficaKubrix(data, tipo, serie, inicio)
        {
            $("#lbGraficaKubrix").text(serie);
            if(tipo == 2)
            {
                var infoGrafica = [];
                for(var i = 0; i < data.length; i++)
                {
                    if(inicio || data[i].descripcion == serie) { 
                        auxInfoGraf = { 
                            name: data[i].descripcion, 
                            data: [data[i].mayor, data[i].menor, data[i].transporteConstruplan, data[i].transporteArrendadora, data[i].fletes, data[i].neumaticos, data[i].administrativoCentral, data[i].administrativoProyectos, data[i].otros],
                            tooltip: {
                                valueDecimals: 2
                            },
                        };
                        infoGrafica.push(auxInfoGraf);
                    }
                }
                grGrafica = Highcharts.chart('graficaKubrix', {
                    chart: { height: 530, type: 'bar' },
                    marginBottom: 100,
                    title: { text: '' },                
                    xAxis: { categories: ['Equipo Mayor', 'Equipo Menor', 'Equipo Transporte COnstruplan', 'Equipo Transporte Arrendadora', 'Fletes', 'OTR', 'Administrativo Central', 'Administrativo Proyectos', 'Otros'] },
                    yAxis: { title: { text: '' } },
                    legend: {
                        align: 'center',
                        verticalAlign: 'bottom',
                        x: 0,
                        y: 0,
                        width:400,
                        itemWidth:200,
                        itemStyle: { width:190 }
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
                                    verticalAlign: (inicio ? 'top' : 'bottom')
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
                    if(inicio || data[i].descripcion == serie) { 
                        auxInfoGraf = { 
                            name: data[i].descripcion,
                            data: [[0, data[i].semana5], [1, data[i].semana4], [2, data[i].semana3], [3, data[i].semana2], [4, data[i].actual]], 
                            regression: !inicio,
                            tooltip: {
                                valueDecimals: 2
                            },
                            regressionSettings: {
                                type: 'linear',
                                color:  '#1111cc',
                                dashStyle: "shortdash",
                                name: "Tendencia"
                            },
                        };
                        infoGrafica.push(auxInfoGraf);
                    }
                }
                grGrafica = Highcharts.chart('graficaKubrix', {
                    chart: { height: 530, type: 'line' },
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
                        itemStyle: { width:190 }
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
                                    verticalAlign: (inicio ? 'top' : 'bottom')
                                }
                            }
                        }]
                    },
                    credits: { enabled: false }
                });
            }
        }



        function CargarGraficaLineas(data, tipo, serie, inicio)
        {
            if(tipo == 1)
            {
                var infoGrafica = [];
                for(var i = 0; i < data.length; i++)
                {
                    if(inicio || data[i].descripcion == serie) { 
                        auxInfoGraf = { 
                            name: data[i].descripcion, 
                            data: [data[i].cfc, data[i].cf, data[i].mc, data[i].pr, data[i].tc, data[i].car, data[i].ex, data[i].hdt, data[i].otros],
                            tooltip: {
                                valueDecimals: 2
                            },
                        };
                        infoGrafica.push(auxInfoGraf);
                    }
                }
                grGrafica = Highcharts.chart('graficaLineas', {
                    chart: { height: 530, type: 'bar' },
                    marginBottom: 100,
                    title: { text: '' },                
                    xAxis: { categories: ['CFC', 'CF', 'MC', 'PR', 'TC', 'CAR', 'EX', 'HDT', 'Otros'] },
                    yAxis: { title: { text: '' } },
                    legend: {
                        align: 'center',
                        verticalAlign: 'bottom',
                        x: 0,
                        y: 0,
                        width:400,
                        itemWidth:200,
                        itemStyle: { width:190 }
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
                    if(inicio || data[i].descripcion == serie) { 
                        auxInfoGraf = { 
                            name: data[i].descripcion,
                            data: [[0, data[i].semana5], [1, data[i].semana4], [2, data[i].semana3], [3, data[i].semana2], [4, data[i].actual]], 
                            regression: !inicio,
                            tooltip: {
                                valueDecimals: 2
                            },
                            regressionSettings: {
                                type: 'linear',
                                color:  '#1111cc',
                                dashStyle: "shortdash",
                                name: "Tendencia"
                            },
                        };
                        infoGrafica.push(auxInfoGraf);
                    }
                }
                grGrafica = Highcharts.chart('graficaLineas', {
                    chart: { height: 530, type: 'line' },
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
                        itemStyle: { width:190 }
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
            auxInfoGraf = { 
                name: "", 
                data: data, 
                regression: true,
                tooltip: {
                    valueDecimals: 2
                },
                regressionSettings: {
                    type: 'linear',
                    //color:  '#1111cc',
                    dashStyle: "shortdash",
                    name: "Tendencia"
                },
            };
            infoGrafica.push(auxInfoGraf);
            grGraficaAnalisis = Highcharts.chart('graficaLineasDetalle', {
                chart: { height: 530, type: 'line' },
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
                    itemStyle: { width:190 }
                },
                plotOptions: {
                    series: { label: { connectorAllowed: false }, color:  '#1111cc' }
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

        //Division y responsable

        function cargarAC()
        {
            setResponsable();
            comboAC.fillCombo('cboObraKubrix', {divisionID: comboDivision.val(), responsableID: comboResponsable.val() }, false);
            comboAC.find('option').get(0).remove();
            if(comboDivision.val() == "TODOS" && comboResponsable.val() == "TODOS") comboAC.find('option').prop('selected', false).change();
            else comboAC.find('option').prop('selected', true).change();
            comboAC.trigger({ type: 'select2:close' });
        }      

        //Catalogo CC

        function initTablaCC()
        {
            dtTablaCC = tablaCC.DataTable({
                language: dtDicEsp,
                destroy: true,      
                paging: false,

                columns: [
                    { data: null, title: ' ', render: function(data, type, row) { return '<input type="checkbox" class="checkbox-cc" ' + (row.guardado ? 'checked' : '') + ' style="height:40px;width:40px;">' }},
                    { data: 'areaCuenta', title: 'Centro Costos' },
                    { data: 'descripcion', title: 'Descripcion' },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": [1, 2] },
                    { className: "dt-combobox", "targets": [0] },
                ],
                order: [[ 1, 'asc' ]]
            });
        }

        function cargarModalCC()
        {
            $.post(getLstCC, { })
                .then(function (response) {
                    if (response.success) {
                        dtTablaCC.clear().rows.add(response.lstCC).draw();
                        // Operación exitosa.
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.');
                }
            );
            modalCatalogoCC.modal("show");
        }

        function guardarCambiosCC()
        {
            $("#tablaCC_filter").find("input").val("").change();
            dtTablaCC.search('').draw();
            var arregloCC = getArregloCC();
            $.post(guardarLstCC, { listaCC: arregloCC })
                .then(function (response) {
                    if (response.success) {
                        // Operación exitosa.
                        AlertaGeneral('Operación exitosa', 'Se guardaron los cambios');
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.');
                }
            );
            modalCatalogoCC.modal("show");
        }

        function getArregloCC()
        {
            var ccsStr = []
            var ccs = $(".checkbox-cc:checked").parent().parent().children(".sorting_1");
            for(let i = 0; i < ccs.length; i++)
            {
                ccsStr.push(ccs[i].innerHTML);
            }
            return ccsStr;
        }

        function obtenerListaKubrix(listaRentabilidad, busq, formatoSignos)
        {
            var listaUtilidad = [];
            //--Ingresos Contabilizados--//
            var elemento = $.grep(listaRentabilidad,function(el,index){ return el.cta == 4000; });
            var utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Ingresos Contabilizados", 1, formatoSignos);
            listaUtilidad.push(utilidad);
            //--Ingresos con Estimación--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return (el.tipoInsumo == "1-1" || el.tipoInsumo == "1-3"); });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Ingresos con Estimación", 2, false);
            listaUtilidad.push(utilidad);
            //--Ingresos Pendientes por Generar--//
            elemento =  $.grep(listaRentabilidad,function(el,index){ return el.tipoInsumo == "1-2"; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Ingresos Pendientes por Generar", 3, false);
            listaUtilidad.push(utilidad);
            //--Subtotal--//
            var subtotal =  $.grep(listaRentabilidad,function(el,index){ return el.tipo_mov < 3; });
            utilidad = 
            {
                tipo_mov: 4,
                descripcion: "Total Ingresos",
                mayor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].mayor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].mayor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].mayor,
                menor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].menor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].menor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].menor,
                transporteConstruplan: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].transporteConstruplan + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].transporteConstruplan + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].transporteConstruplan,
                transporteArrendadora: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].transporteArrendadora + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].transporteArrendadora + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].transporteArrendadora,
                administrativoCentral: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].administrativoCentral + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].administrativoCentral + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].administrativoCentral,
                administrativoProyectos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].administrativoProyectos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].administrativoProyectos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].administrativoProyectos,
                fletes: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].fletes + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].fletes + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].fletes,
                neumaticos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].neumaticos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].neumaticos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].neumaticos,
                otros: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].otros + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].otros + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].otros,
                total: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].total + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].total + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].total,
                actual: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].actual + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].actual + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].actual,
                semana2: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].semana2 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].semana2 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].semana2,
                semana3: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].semana3 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].semana3 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].semana3,
                semana4: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].semana4 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].semana4 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].semana4,
                semana5: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 1; })[0].semana5 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 2; })[0].semana5 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 3; })[0].semana5,
            };
            var auxUtilidad = utilidad;
            listaUtilidad.push(utilidad);

            //--Costo Total--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cta == 5000 && el.tipoInsumo != "5000-10"; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Costo Total", 5, false);
            listaUtilidad.push(utilidad);
            //--Depreciación--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cta == 5000 && el.tipoInsumo == "5000-10"; }); 
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Depreciación", 6, false);
            listaUtilidad.push(utilidad);
        //--Utilidad Bruta--//           
            utilidad = 
            {
                tipo_mov: 7,
                descripcion: "Utilidad Bruta",
                mayor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].mayor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].mayor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].mayor,
                menor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].menor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].menor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].menor,
                transporteConstruplan: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].transporteConstruplan - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].transporteConstruplan - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].transporteConstruplan,
                transporteArrendadora: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].transporteArrendadora - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].transporteArrendadora - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].transporteArrendadora,
                administrativoCentral: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].administrativoCentral - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].administrativoCentral - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].administrativoCentral,
                administrativoProyectos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].administrativoProyectos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].administrativoProyectos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].administrativoProyectos,
                fletes: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].fletes - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].fletes - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].fletes,
                neumaticos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].neumaticos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].neumaticos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].neumaticos,
                otros: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].otros - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].otros - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].otros,
                total: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].total - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].total - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].total,
                actual: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].actual - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].actual - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].actual,
                semana2: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].semana2 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].semana2 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].semana2,
                semana3: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].semana3 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].semana3 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].semana3,
                semana4: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].semana4 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].semana4 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].semana4,
                semana5: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].semana5 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].semana5 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].semana5,
            };
            listaUtilidad.push(utilidad);
            //--Gastos de Operación--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cta == 5280; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Gastos de Operación", 8, false);
            listaUtilidad.push(utilidad);
            //--Gastos Antes de Finacieros--//           
            utilidad =
            {
                tipo_mov: 9,
                descripcion: "Resultado Antes Finacieros",
                mayor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].mayor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].mayor,
                menor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].menor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].menor,
                transporteConstruplan: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].transporteConstruplan - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].transporteConstruplan,
                transporteArrendadora: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].transporteArrendadora - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].transporteArrendadora,
                administrativoCentral: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].administrativoCentral - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].administrativoCentral,
                administrativoProyectos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].administrativoProyectos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].administrativoProyectos,
                fletes: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].fletes - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].fletes,
                neumaticos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].neumaticos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].neumaticos,
                otros: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].otros - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].otros,
                total: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].total - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].total,
                actual: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].actual - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].actual,
                semana2: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].semana2 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].semana2,
                semana3: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].semana3 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].semana3,
                semana4: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].semana4 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].semana4,
                semana5: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].semana5 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].semana5,
    
            };
            listaUtilidad.push(utilidad);
        //--Gastos de Financieros--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cta == 5900 || el.cta == 4900; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Gastos y Productos Financieros", 10, formatoSignos);
            listaUtilidad.push(utilidad);
        //--Resultado con Financieros--//           
            utilidad =
            {
                tipo_mov: 11,
                descripcion: "Resultado con Financieros",
                mayor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].mayor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].mayor,
                menor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].menor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].menor,
                transporteConstruplan: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].transporteConstruplan + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].transporteConstruplan,
                transporteArrendadora: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].transporteArrendadora + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].transporteArrendadora,
                administrativoCentral: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].administrativoCentral + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].administrativoCentral,
                administrativoProyectos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].administrativoProyectos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].administrativoProyectos,
                fletes: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].fletes + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].fletes,
                neumaticos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].neumaticos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].neumaticos,
                otros: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].otros + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].otros,
                total: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].total + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].total,
                actual: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].actual + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].actual,
                semana2: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].semana2 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].semana2,
                semana3: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].semana3 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].semana3,
                semana4: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].semana4 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].semana4,
                semana5: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].semana5 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].semana5,
            };
            listaUtilidad.push(utilidad);
        //--Otros Ingresos--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cta == 5901 || el.cta == 4901; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Otros Ingresos", 12, formatoSignos);
            listaUtilidad.push(utilidad);
        //--Resultado Neto--//           
            utilidad =
            {
                tipo_mov: 13,
                descripcion: "Resultado Neto",
                mayor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].mayor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].mayor,
                menor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].menor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].menor,
                transporteConstruplan: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].transporteConstruplan + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].transporteConstruplan,
                transporteArrendadora: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].transporteArrendadora + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].transporteArrendadora,
                administrativoCentral: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].administrativoCentral + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].administrativoCentral,
                administrativoProyectos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].administrativoProyectos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].administrativoProyectos,
                fletes: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].fletes + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].fletes,
                neumaticos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].neumaticos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].neumaticos,
                otros: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].otros + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].otros,
                total: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].total + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].total,
                actual: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].actual + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].actual,
                semana2: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].semana2 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].semana2,
                semana3: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].semana3 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].semana3,
                semana4: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].semana4 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].semana4,
                semana5: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 11; })[0].semana5 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 12; })[0].semana5,
                };
            var auxNeto = utilidad;
            listaUtilidad.push(utilidad);
        //--% de Margen--//
            utilidad =
            {
                tipo_mov: 14,
                descripcion: "% de Margen",
                mayor: auxUtilidad.mayor != 0 ? (auxNeto.mayor / auxUtilidad.mayor) * 100 : 0,
                menor: auxUtilidad.menor != 0 ? (auxNeto.menor / auxUtilidad.menor) * 100 : 0,
                transporteConstruplan: auxUtilidad.transporteConstruplan != 0 ? (auxNeto.transporteConstruplan / auxUtilidad.transporteConstruplan) * 100 : 0,
                transporteArrendadora: auxUtilidad.transporteArrendadora != 0 ? (auxNeto.transporteArrendadora / auxUtilidad.transporteArrendadora) * 100 : 0,
                administrativoCentral: auxUtilidad.administrativoCentral != 0 ? (auxNeto.administrativoCentral / auxUtilidad.administrativoCentral) * 100 : 0,
                administrativoProyectos: auxUtilidad.administrativoProyectos != 0 ? (auxNeto.administrativoProyectos / auxUtilidad.administrativoProyectos) * 100 : 0,
                fletes: auxUtilidad.fletes != 0 ? (auxNeto.fletes / auxUtilidad.fletes) * 100 : 0,
                neumaticos: auxUtilidad.neumaticos != 0 ? (auxNeto.neumaticos / auxUtilidad.neumaticos) * 100 : 0,
                otros: auxUtilidad.otros != 0 ? (auxNeto.otros / auxUtilidad.otros) * 100 : 0,
                total: auxUtilidad.total != 0 ? (auxNeto.total / auxUtilidad.total) * 100 : 0,
                actual: auxUtilidad.actual != 0 ? (auxNeto.actual / auxUtilidad.actual) * 100 : 0,
                semana2: auxUtilidad.semana2 != 0 ? (auxNeto.semana2 / auxUtilidad.semana2) * 100 : 0,
                semana3: auxUtilidad.semana3 != 0 ? (auxNeto.semana3 / auxUtilidad.semana3) * 100 : 0,
                semana4: auxUtilidad.semana4 != 0 ? (auxNeto.semana4 / auxUtilidad.semana4) * 100 : 0,
                semana5: auxUtilidad.semana5 != 0 ? (auxNeto.semana5 / auxUtilidad.semana5) * 100 : 0,
                };
            listaUtilidad.push(utilidad);
            return listaUtilidad;
        }

        function AsignarImportesKubrix(importes, fecha, descripcion, tipoMov, cambioSigno)
        {
            var costo = { 
                descripcion: descripcion, tipo_mov: tipoMov, mayor: 0, menor: 0,transporteConstruplan: 0, transporteArrendadora: 0,
                administrativoCentral: 0, administrativoProyectos: 0, fletes: 0, neumaticos: 0, otros: 0, total: 0, actual: 0,
                semana2: 0, semana3: 0, semana4: 0, semana5: 0, detalles: importes
            };

            var mayor = $.grep(importes,function(el,index){ return el.tipo == 1; });
            if (mayor.length > 0) { $.each(mayor, function(index, element){ costo.mayor += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            var menor = $.grep(importes,function(el,index){ return el.tipo == 2; });
            if (menor.length > 0) { $.each(menor, function(index, element){ costo.menor += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            var transporteConstruplan = $.grep(importes,function(el,index){ return el.tipo == 3; });
            if (transporteConstruplan.length > 0) { $.each(transporteConstruplan, function(index, element){ costo.transporteConstruplan += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            var transporteArrendadora = $.grep(importes,function(el,index){ return el.tipo == 8; });
            if (transporteArrendadora.length > 0) { $.each(transporteArrendadora, function(index, element){ costo.transporteArrendadora += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            var administrativoCentral = $.grep(importes,function(el,index){ return el.tipo == 6; });
            if (administrativoCentral.length > 0) { $.each(administrativoCentral, function(index, element){ costo.administrativoCentral += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            var administrativoProyectos = $.grep(importes,function(el,index){ return el.tipo == 9; });
            if (administrativoProyectos.length > 0) { $.each(administrativoProyectos, function(index, element){ costo.administrativoProyectos += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            var fletes = $.grep(importes,function(el,index){ return el.tipo == 4; });
            if (fletes.length > 0) { $.each(fletes, function(index, element){ costo.fletes += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            var neumaticos = $.grep(importes,function(el,index){ return el.tipo == 5; });
            if (neumaticos.length > 0) { $.each(neumaticos, function(index, element){ costo.neumaticos += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            var otros = $.grep(importes,function(el,index){ return el.tipo == 7; });
            if (otros.length > 0) { $.each(otros, function(index, element){ costo.otros += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
            if (importes.length > 0) { $.each(importes, function(index, element){ costo.total += (cambioSigno ? element.importe * (-1) : element.importe) || 0; }); }
                    
                //Semanal
            var auxFecha = new Date(fecha);
            var actual = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
            if (actual.length > 0) { $.each(actual, function(index, element){ costo.actual += (cambioSigno ? element.importe * (-1) : element.importe) || 0; });}
            
            if(tipoIntervaloGlobal == 1) { auxFecha.setDate(auxFecha.getDate() - 7); }
            else { 
                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1); 
                auxFecha.setDate(auxFecha.getDate() - 1);
            }
            var semana2 = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
            if (semana2.length > 0) { $.each(semana2, function(index, element){ costo.semana2 += (cambioSigno ? element.importe * (-1) : element.importe) || 0; });}
            
            if(tipoIntervaloGlobal == 1) { auxFecha.setDate(auxFecha.getDate() - 7); }
            else { 
                auxFecha.setDate(auxFecha.getDate() - 1);
                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1); 
                auxFecha.setDate(auxFecha.getDate() - 1);
            }            
            var semana3 = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
            if (semana3.length > 0) { $.each(semana3, function(index, element){ costo.semana3 += (cambioSigno ? element.importe * (-1) : element.importe) || 0; });}
            
            if(tipoIntervaloGlobal == 1) { auxFecha.setDate(auxFecha.getDate() - 7); }
            else { 
                auxFecha.setDate(auxFecha.getDate() - 1);
                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1); 
                auxFecha.setDate(auxFecha.getDate() - 1);
            }
            var semana4 = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
            if (semana4.length > 0) { $.each(semana4, function(index, element){ costo.semana4 += (cambioSigno ? element.importe * (-1) : element.importe) || 0; });}
            
            if(tipoIntervaloGlobal == 1) { auxFecha.setDate(auxFecha.getDate() - 7); }
            else { 
                auxFecha.setDate(auxFecha.getDate() - 1);
                auxFecha = new Date(auxFecha.getFullYear(), auxFecha.getMonth(), 1); 
                auxFecha.setDate(auxFecha.getDate() - 1);
            }
            var semana5 = jQuery.grep(importes, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) <= auxFecha; });
            if (semana5.length > 0) { $.each(semana5, function(index, element){ costo.semana5 += (cambioSigno ? element.importe * (-1) : element.importe) || 0; });}
            
            //auxFecha.setDate(auxFecha.getDate() - 7);
               
            costo.detalles = importes;
            if (cambioSigno) $.each(costo.detalles, function(index, element){ element.importe = element.importe * (-1); });
            $.each(costo.detalles, function(index, element){ element.tipo_mov = tipoMov; });
            return costo;
        }

        function SortByAreaCuenta(a, b){
            var aArea = parseInt(a.split('-')[0]) || -1;
            var aCuenta = parseInt(a.split('-')[1]) || -1;
            var bArea = parseInt(b.split('-')[0]) || -1;
            var bCuenta = parseInt(b.split('-')[1]) || -1;
            return ((aArea < bArea) ? -1 : ((aArea > bArea) ? 1 : ((aCuenta < bCuenta) ? -1 : ((aCuenta > bCuenta) ? 1 : 0))));
        }   

        function RecargarTablaDetalle(e)
        {
            if($("#divTablaKubrixDetalle").is(":visible")) {
                let rowData = dtTablaKubrixDivision.data();
                var detallesRaw = rowData.map(function(x) { return x.detalles});
                var detalles = [].concat.apply([], detallesRaw);
                detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                setFormatoKubrixDetalles(detalles, false);
            }
        }

        init();
    };

$(document).ready(function () {
    Maquinaria.Rentabilidad.Kubrix = new Kubrix();
}).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();


