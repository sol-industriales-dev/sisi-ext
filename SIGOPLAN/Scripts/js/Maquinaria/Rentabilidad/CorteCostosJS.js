(function () {

    $.namespace('maquinaria.rentabilidad.cortecostos');

    cortecostos = function () {

        let cuentasDescr = [];

        const comboAC = $('#comboAC');
        const comboTipo = $('#comboTipo');
        const cbTipoCorte = $('#cbTipoCorte');
        const inputCorte = $('#inputCorte');
        const chbTipoReporte = $("#chbTipoReporte");
        const cbConfiguracion = $("#cbConfiguracion");
        const comboGrupoK = $("#comboGrupoK");
        const comboModeloK = $("#comboModeloK");
        const cboMaquina = $("#cboMaquina");
        const cboHoraCorte = $("#cboHoraCorte");
        const inputDiaInicio = $("#inputDiaInicio");
        const inputDiaFinal = $("#inputDiaFinal");
        const botonBuscar = $('#botonBuscar');
        const comboDivision = $("#comboDivision");
        const comboResponsable = $("#comboResponsable"); 

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

        const tablaKubrixDetalleEco = $('#tablaKubrixDetalleEco');
        let dtTablaKubrixDetalleEco;

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

        const divTablaDetalles = $('#divTablaDetalles');
        const tablaDetalles = $('#tablaDetalles');
        let dtTablaDetalles;
        const botonTablaDetalle = $("#botonTablaDetalle");



        const getLstFchasCortes = new URL(window.location.origin + '/Rentabilidad/getLstFechasCortes');
        
        const getLstKubrix = new URL(window.location.origin + '/Rentabilidad/getLstKubrixCorte');
        const getLstKubrixDetalle = new URL(window.location.origin + '/Rentabilidad/getLstKubrixCorteDet');
        const getLstKubrixTablaDet = new URL(window.location.origin + '/Rentabilidad/getLstKubrixCorteDetTabla');

        const getLstCC = new URL(window.location.origin + '/Rentabilidad/getLstCC');
        const guardarLstCC = new URL(window.location.origin + '/Rentabilidad/guardarLstCC');
        const checkResponsable = new URL(window.location.origin + '/Rentabilidad/checkResponsable');
        const GetGrupoMaquinas = new URL(window.location.origin + '/Rentabilidad/getGrupoMaquinas');

        const GetCuentasDescr = new URL(window.location.origin + '/Rentabilidad/getCuentasDesc');

        let empresaActual = 2;
        
        let maquinaFiltro = "";
        let availableDates = [];
        let fechaInicioInput = new Date();
        
        let auxAdministrativoCentral = "";
        let auxAdministrativoProyectos = "";
        let auxOtros = "";
        let banderaTablaDetalle = false;
        

        //Detalles búsqueda
        let tipoReporte = 1;
        let tipoTabla = 0;
        let areasCuentaGlobal = [];
        let modelos = [];
        let fechaInicio;
        let fechaFin;
        let economico;
        let corteIDGlobal;
        //Filtros Detalle
        let subcuentaFiltro = "";
        let subsubcuentaFiltro = "";
        let divisionFiltro = "";
        let areaCuentaFiltro = "";
        let conciliacionFiltro = "";
        let economicoFiltro = "";
        //Filtros LstKubrixDet
        let divisionFiltroDetalle = "";
        let areaCuentaFiltroDetalle = "";
        //Columna y renglon para detalles
        let columnaGlobal = 1;
        let renglonGlobal = 0;
        let empresaGlobal = 0;

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
        let responsable = false;
        let numAreaCuenta = 0;
        let stringDivision = "TODAS";
        let stringAreaCuenta = "TODAS"
        var collapsedGroups = {};
        let usuarioID = 0;
        let fechaCorteGeneral = new Date();

        //
        const conceptos = ["Costo Total", "Depreciación", "Gastos de Operación", "Total" ];
        let divisiones = [];
        let areasCuenta = [];
        let areasCuentaDetalle = [];


        const getLstAnalisis = new URL(window.location.origin + '/Rentabilidad/getLstAnalisis');

        function init() {
            getEmpresaActual();
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

            setCuentasDescr();

            initTablaSubCuenta();
            initTablaSubSubCuenta();
            initTablaDivision();
            initTablaAreaCuenta();
            initTablaConciliacion();
            initTablaEconomico();
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
            cboMaquina.select2();
            comboAC.select2({ closeOnSelect: false });
            //cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', {}, false, "TODOS");
            //
            initTablaCC();

        }

        function initBotones()
        {
            divTablaSubSubCuenta.hide(); 
            divTablaDivision.hide();
            divTablaAreaCuenta.hide();
            divTablaConciliacion.hide();
            divTablaEconomico.hide();
            divTablaDetalles.hide();
            
            botonTablaSubSubCuenta.hide(); 
            botonTablaDivision.hide();
            botonTablaAreaCuenta.hide();
            botonTablaConciliacion.hide();
            botonTablaEconomico.hide();
            botonTablaDetalle.hide();            

            inputDiaInicio.hide();
        }

        function getEmpresaActual(){
            $.post("/Base/getEmpresa").then(function(response) { empresaActual = response; } );
        }

        function updateDatePicker()
        {
            setLstFechaCortes();
            inputCorte.datepicker("destroy");
            inputCorte.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",
                onSelect: function (dateText) {
                    $("#inputDiaFinalAnalisis").val(dateText);
                    inputDiaFinal.val(dateText);
                    cargarHorasCorte();
                },
                beforeShowDay: function(date)
                {
                    var auxArr = false;
                    $.each(availableDates, function( index, value ) {
                        if(date.getDate() == value.getDate() && date.getMonth() == value.getMonth() &&  date.getYear() == value.getYear()) { 
                            auxArr = true; 
                        }
                    });
                    return [auxArr, ""];
                }
            });
            inputCorte.val("");
            cboHoraCorte.clearCombo();
        }

        function initDatePickers()
        {            
            let yearActual = new Date().getFullYear();
            setLstFechaCortes();
            $("#inputDiaFinalAnalisis").datepicker({
                Button: false,
                dateFormat: "dd-mm-yy"
            });
            inputCorte.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",
                onSelect: function (dateText) {
                    $("#inputDiaFinalAnalisis").val(dateText);
                    inputDiaFinal.val(dateText);
                    cargarHorasCorte();
                },
                beforeShowDay: function(date)
                {
                    var auxArr = false;
                    $.each(availableDates, function( index, value ) {
                        if(date.getDate() == value.getDate() && date.getMonth() == value.getMonth() &&  date.getYear() == value.getYear()) { 
                            auxArr = true; 
                        }
                    });
                    return [auxArr, ""];
                }
            });
            inputDiaFinal.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy"
            });
            inputDiaFinal.datepicker("setDate", new Date());
            inputDiaInicio.datepicker({
                Button: false,
                dateFormat: "dd-mm-yy",

            });
            inputDiaInicio.datepicker("setDate", new Date(1,0,1));
            cboHoraCorte.clearCombo();
        }
        
        function agregarListeners()
        {
            //--> Tabla principal Detalle
            //--> Tabla principal Detalle
            modalDetallesK.on("hide.bs.modal", function() {
                dtTablaSubCuenta.clear().draw();
                dtTablaSubSubCuenta.clear().draw();
                dtTablaDivision.clear().draw();
                dtTablaAreaCuenta.clear().draw();
                dtTablaConciliacion.clear().draw();
                dtTablaEconomico.clear().draw();
                dtTablaDetalles.clear().draw();                

                divTablaSubCuenta.show();
                divTablaSubSubCuenta.hide();
                divTablaDivision.hide();
                divTablaAreaCuenta.hide();
                divTablaConciliacion.hide();
                divTablaEconomico.hide();
                divTablaDetalles.hide();

                botonTablaSubCuenta.show();                
                botonTablaSubSubCuenta.hide();
                botonTablaDivision.hide();
                botonTablaAreaCuenta.hide();
                botonTablaConciliacion.hide();
                botonTablaEconomico.hide();                
                botonTablaDetalle.hide();                
                
                botonTablaSubCuenta.prop("disabled", true);
                botonTablaSubSubCuenta.prop("disabled", true);
                botonTablaDivision.prop("disabled", true); 
                botonTablaAreaCuenta.prop("disabled", true); 
                botonTablaConciliacion.prop("disabled", true); 
                botonTablaEconomico.prop("disabled", true); 
                botonTablaDetalle.prop("disabled", true); 

                subcuentaFiltro = "";
                subsubcuentaFiltro = "";
                divisionFiltro = "";
                areaCuentaFiltro = "";
                conciliacionFiltro = "";
                economicoFiltro = "";
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
                    botonTablaDetalle.prop("disabled", true); 
                    
                    divTablaSubCuenta.show(500);
                    divTablaSubSubCuenta.hide(500);
                    divTablaDivision.hide(500);
                    divTablaAreaCuenta.hide(500);
                    divTablaConciliacion.hide(500);
                    divTablaEconomico.hide(500);
                    divTablaDetalles.hide(500);                    

                    botonTablaSubSubCuenta.hide();
                    botonTablaDivision.hide(); 
                    botonTablaAreaCuenta.hide(); 
                    botonTablaConciliacion.hide(); 
                    botonTablaEconomico.hide(); 
                    botonTablaDetalle.hide();

                    subcuentaFiltro = "";
                    subsubcuentaFiltro = "";
                    divisionFiltro = "";
                    areaCuentaFiltro = "";
                    conciliacionFiltro = "";
                    economicoFiltro = "";

                    empresaGlobal = 0;
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
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaSubSubCuenta.show(500);
                    divTablaDivision.hide(500);
                    divTablaAreaCuenta.hide(500);
                    divTablaConciliacion.hide(500);
                    divTablaEconomico.hide(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaDivision.hide(); 
                    botonTablaAreaCuenta.hide(); 
                    botonTablaConciliacion.hide(); 
                    botonTablaEconomico.hide(); 
                    botonTablaDetalle.hide(); 
                    subsubcuentaFiltro = "";
                    divisionFiltro = "";
                    areaCuentaFiltro = "";
                    conciliacionFiltro = "";
                    economicoFiltro = "";
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
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaDivision.show(500);
                    divTablaAreaCuenta.hide(500);
                    divTablaConciliacion.hide(500);
                    divTablaEconomico.hide(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaAreaCuenta.hide(); 
                    botonTablaConciliacion.hide(); 
                    botonTablaEconomico.hide(); 
                    botonTablaDetalle.hide(); 
                    divisionFiltro = "";
                    areaCuentaFiltro = "";
                    conciliacionFiltro = "";
                    economicoFiltro = "";
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
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaAreaCuenta.show(500);
                    divTablaConciliacion.hide(500);
                    divTablaEconomico.hide(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaConciliacion.hide(); 
                    botonTablaEconomico.hide(); 
                    botonTablaDetalle.hide();
                    
                    areaCuentaFiltro = "";
                    conciliacionFiltro = "";
                    economicoFiltro = "";
                }                 
            });
            botonTablaConciliacion.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubSubCuenta.is(":visible")) {
                    botonTablaConciliacion.prop("disabled", true);
                    botonTablaEconomico.prop("disabled", true);
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaConciliacion.show(500);
                    divTablaEconomico.hide(500);
                    divTablaDetalles.hide(500);                    
                     
                    botonTablaEconomico.hide(); 
                    botonTablaDetalle.hide();  

                    conciliacionFiltro = "";
                    economicoFiltro = "";
                }                 
            });
            botonTablaEconomico.click(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if(!divTablaSubSubCuenta.is(":visible")) {
                    botonTablaEconomico.prop("disabled", true);
                    botonTablaDetalle.prop("disabled", true);
                    
                    divTablaEconomico.show(500);
                    divTablaDetalles.hide(500);                    
                    
                    botonTablaDetalle.hide();  

                    economicoFiltro = "";
                }                 
            });

            chbTipoReporte.change(function(){
                if (dtTablaKubrixDetalle != null) {
                    var info = dtTablaKubrixDetalle.rows().data();
                    if(chbTipoReporte.is(":checked")) { 
                        if (botonBuscar.attr("data-tipoCorte") == 0) { initTablaKubrixDetalle(info, 1); }
                        else { initTablaKubrixDetalle(info, 3); }
                    }
                    else{ initTablaKubrixDetalle(info, 2); }
                }
            });
            botonBuscar.click(function (e) { 
                inputDiaFinal.change(); 
                setLstKubrix(e); 
            });

            //--> Tabla detalle Tipo Equipo

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

            cbTipoCorte.change(updateDatePicker);

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

            $("#chbAgrupacionDetalle").change(function(e) {
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                if($(this).is(":checked")) {                    
                    let rowData = dtTablaKubrixDetalleEco.data();
                    var detallesRaw = rowData.map(function(x) { return x.detalles});
                    var detalles = [].concat.apply([], detallesRaw);
                    detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                    setFormatoKubrixDetalles(e, detalles);
                }
                else {                    
                    let rowData = dtTablaKubrixDetalle.data();
                    var detallesRaw = rowData.map(function(x) { return x.detalles});
                    var detalles = [].concat.apply([], detallesRaw);
                    detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                    setFormatoKubrixDetallesEconomico(e, detalles);
                }
            });
            $("#chbDespliegueDetEco").change(function(e){
                e.preventDefault();
                e.stopPropagation();
                e.stopImmediatePropagation();
                let rowData = dtTablaKubrixDetalleEco.data();
                var detallesRaw = rowData.map(function(x) { return x.detalles});
                var detalles = [].concat.apply([], detallesRaw);
                detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                setFormatoKubrixDetallesEconomico(e, detalles);
            });
            //Tabla Kubrix Detalle Filtros
            $('#botonAtrasDetalle').click(function(){
                areasCuentaDetalle = areasCuenta;
                stringAreaCuenta = "TODAS";
                if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                //$("#chbAgrupacionDet").hide(500);
                $("#divTablaKubrixAreaCuenta").show(500);
                areaCuentaFiltroDetalle = "";
            });
            $('#chbTipoReporteDetalle').bootstrapToggle();
            $('#chbTipoReporteDetalle').change(function(){
                if (dtTablaKubrixDetalle != null) {
                    var info = dtTablaKubrixDetalle.rows().data();
                    if($('#chbTipoReporteDetalle').is(":checked")) { 
                        if (botonBuscar.attr("data-tipoCorte") == 0) { initTablaKubrixDetalle(info, 1); }
                        else { initTablaKubrixDetalle(info, 3); }
                    }
                    else{ initTablaKubrixDetalle(info, 2); }
                }
            });
            //Tabla Kubrix AC Filtros
            $('#botonAtrasAC').click(function(){
                areasCuenta = [];
                areasCuentaDetalle = [];
                stringDivision = "TODAS";
                if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                //$("#chbAgrupacionDet").hide(500);
                $("#divTablaKubrixDivision").show(500);
                divisionFiltroDetalle = "";
            });
            //Tabla Kubrix DetalleEco Filtros
            $('#botonAtrasDetEco').click(function(){
                areasCuenta = [];
                areasCuentaDetalle = [];
                stringAreaCuenta = "TODAS";
                if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                //$("#chbAgrupacionDet").hide(500);
                $("#divTablaKubrixAreaCuenta").show(500);
                areaCuentaFiltroDetalle = ""
            });
        }

        function fillCombos() {
            cboMaquina.select2();
            comboAC.select2({ closeOnSelect: false });
            //comboAC.append(new Option("SIN AREA CUENTA", "S/A", false, false)).trigger('change');
            comboGrupoK.select2();
            //cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: -1 });
            comboGrupoK.fillCombo('/Rentabilidad/fillComboGrupo', {}, false, "TODOS");            
            comboModeloK.select2({ closeOnSelect: false });
            comboModeloK.fillCombo('/Rentabilidad/fillComboModelo', { grupoID: -1 }, true);
            comboModeloK.find('option').get(0).remove();
            comboDivision.fillCombo('/Rentabilidad/fillComboDivision', {}, false, "TODAS");
            comboResponsable.fillCombo('/Rentabilidad/fillComboResponsable', {}, false, "TODOS");
            

            //convertToMultiselectSelectAll(comboModeloK);
            //comboModeloK.multiselect('selectAll', true).multiselect('updateButtonText');

            comboAC.fillCombo('cboObraKubrix', null, false, "TODOS");
            comboAC.find('option').get(0).remove();
            comboTipo.fillCombo('cboTipo', null, false, "TODOS");

            //Analisis
            comboACAnalisis.fillCombo('cboObra', null, false, "TODOS");
            comboTipoAnalisis.fillCombo('cboTipo', null, false, "");
            comboGrupo.multiselect();
            comboGrupo.multiselect('disable');

            comboModelo.multiselect();
            comboModelo.multiselect('disable');

            comboCC.multiselect();
            comboCC.multiselect('disable');

            comboGrupoK.select2();
            cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: -1 }, false, "TODOS");
            comboGrupoK.fillCombo('/Rentabilidad/fillComboGrupo', {}, false, "TODOS");


            //convertToMultiselectSelectAll(comboModeloK);
            //comboModeloK.multiselect('selectAll', true).multiselect('updateButtonText');
            comboGrupoK.change(cargarComboModeloK);
            comboModeloK.change(cargarMaquinariaModelo);

            comboDivision.fillCombo('/Rentabilidad/fillComboDivision', {}, false, "TODOS");
            comboResponsable.fillCombo('/Rentabilidad/fillComboResponsable', {}, false, "TODOS");

        }

        function cargarComboModeloK()
        {
            var grupo = comboGrupoK.val();
            if(grupo == "TODOS")
            {                
                comboModeloK.fillCombo('/Rentabilidad/fillComboModelo', { grupoID: -1 }, true);
                comboModeloK.select2('destroy').find('option').prop('selected', false).end().select2({ closeOnSelect: false });
                cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: -1 }, false, "TODOS");
            }
            else
            {
                comboModeloK.fillCombo('/Rentabilidad/fillComboModelo', { grupoID: grupo }, true);
                comboModeloK.select2('destroy').find('option').prop('selected', true).end().select2({ closeOnSelect: false });
                cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: grupo }, false, "TODOS");
            }
            comboModeloK.trigger({ type: 'select2:close' });
        }

        function cargarMaquinariaModelo()
        {
            var grupo = comboGrupoK.val();
            var modelo = comboModeloK.val();
            cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: grupo, modeloID: modelo }, false, "TODOS");
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

        function setCuentasDescr() {
            $.post(GetCuentasDescr, { })
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        // Operación exitosa.
                        cuentasDescr = response.cuentas;
                    } else {
                        // Operación no completada.
                    }
                }, function(error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                }
            );
        }

        function setLstFechaCortes() {
            $('#tablaKubrixDetalle tbody td').addClass("blurry");
            $.post(getLstFchasCortes, { tipoCorte: cbTipoCorte.val() })
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        // Operación exitosa.
                        availableDates = [];
                        for(let i = 0; i < response.fechas.length; i++)
                        {
                            availableDates.push(new Date(parseInt(response.fechas[i].substr(6))));
                        }
                        if(availableDates.length > 0){
                            inputCorte.datepicker("setDate", availableDates[availableDates.length - 1] );
                            cargarHorasCorte();
                        }
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function(error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.');
                }
            );
        }

        function setLstKubrix(e) {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            cambiarFechaInicio();
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();

            var array2 = [];

            if (dtTablaKubrixDetalle != null) { dtTablaKubrixDetalle.clear(); }
            if (dtTablaKubrixDetalleEco != null) { dtTablaKubrixDetalleEco.clear(); }
            if (dtTablaKubrixAreaCuenta != null) { dtTablaKubrixAreaCuenta.clear(); }
            if (dtTablaKubrixDivision != null) { dtTablaKubrixDivision.clear(); }

            tipoReporte = cbTipoCorte.val();
            areasCuentaGlobal = (responsable && comboAC.val().length < 1) ? array : (comboAC.val().length < 1 ? array2 : comboAC.val());
            modelos = comboModeloK.val();
            fechaInicio = inputDiaInicio.val();
            fechaFin = inputDiaFinal.val();
            economico = cboMaquina.val() == "TODOS" ? null : $("#cboMaquina option:selected").text().trim();
            corteIDGlobal = cboHoraCorte.val();

            $('#tablaKubrixDetalle tbody td').addClass("blurry");
            $.post(getLstKubrix, { 
                corteID: cboHoraCorte.val(), 
                modelos: comboModeloK.val(),
                areaCuenta: (responsable && comboAC.val().length < 1) ? array : (comboAC.val().length < 1 ? array2 : comboAC.val()), 
                economico: cboMaquina.val() == "TODOS" ? null : $("#cboMaquina option:selected").text().trim(),
                acumulado: cbConfiguracion.val(),
                fechaFin: inputDiaFinal.val(),
                tipoCorte : cbTipoCorte.val(),
                reporteCostos: true
            })
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        //// Operación exitosa.
                        //auxDatos = response.lst;     
                        //auxAdministrativoCentral = "";                        
                        //for(var i = 0; i < response.administrativoCentral.length; i++) { auxAdministrativoCentral += "- " + response.administrativoCentral[i] + "<br>" }
                        //auxAdministrativoProyectos = "";
                        //for(var i = 0; i < response.administrativoProyectos.length; i++) { auxAdministrativoProyectos += "- " + response.administrativoProyectos[i] + "<br>" }
                        //auxOtros = "";
                        //for(var i = 0; i < response.otros.length; i++) { auxOtros += "- " + response.otros[i] + "<br>" }
                        //var detallesRespuesta = [];
                        //for(var i = 0; i < auxDatos.length; i++) {
                        //    var aux1 = auxDatos[i].detalles;
                        //    if(aux1 != null)
                        //    {
                        //        detallesRespuesta = $.merge(detallesRespuesta, aux1);
                        //    }                            
                        //}
                        var detallesRespuesta = response.lst;
                        fechaCorteGeneral = new Date(parseInt(response.fecha.substr(6)));
                        botonBuscar.attr("data-corteID", cboHoraCorte.val());
                        botonBuscar.attr("data-obra", comboAC.val());
                        modelos: comboModeloK.val();
                        botonBuscar.attr("data-economico", cboMaquina.val() == "" ? null : $("#cboMaquina option:selected").text().trim());
                        botonBuscar.attr("data-fechaFin", inputCorte.val());
                        botonBuscar.attr("data-fechaInicio", inputDiaInicio.val());
                        botonBuscar.attr("data-tipoCorte", cbTipoCorte.val());
                        usuarioID = response.usuarioID;
                        maquinaFiltro = response.maquina;
                        numAreaCuenta = comboAC.val().length;
                        if(numAreaCuenta == 1 || cboMaquina.val() != "TODOS")
                        {
                            if($("#chbAgrupacionDetalle").is(":checked")) setFormatoKubrixDetalles(e, detallesRespuesta);
                            else setFormatoKubrixDetallesEconomico(e, detallesRespuesta);
                        }
                        else
                        {
                            if(comboResponsable.val() != "TODOS")
                            {
                                areasCuenta = ["ARRENDADORA"];
                                auxAreasCuenta = detallesRespuesta.map(function(element) { return element.areaCuenta.trim(); });
                                auxAreasCuenta = $.grep(auxAreasCuenta,function(el,index){ return (index == $.inArray(el,auxAreasCuenta) && el != null); });
                                auxAreasCuenta.sort(SortByAreaCuenta);
                                $.merge(areasCuenta, auxAreasCuenta);
                                areasCuentaDetalle = auxAreasCuenta;
                                var datos = FormatoDetalles(detallesRespuesta, areasCuenta, 2);
                                initTablaKubrixAreaCuenta(datos, areasCuenta, "ARRENDADORA");                            
                                if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                                $("#divTablaKubrixAreaCuenta").show(500);
                            }
                            else
                            {
                                divisiones = ["ARRENDADORA"];
                                auxDivisiones = detallesRespuesta.map(function(element) { return element.division; });
                                auxDivisiones = $.grep(auxDivisiones,function(el,index){ return (index == $.inArray(el,auxDivisiones) && el != null); });
                                $.merge(divisiones, auxDivisiones);
                                var datos = FormatoDetalles(detallesRespuesta, divisiones, 1);
                                initTablaKubrixDivision(datos, divisiones, "ARRENDADORA"); 
                                if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
                                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                                $("#divTablaKubrixDivision").show(500);
                            }
                        }    

                        //Reiniciar Tablas detalle
                        initTablaSubCuenta();
                        initTablaSubSubCuenta();
                        initTablaDivision();
                        initTablaAreaCuenta();
                        initTablaConciliacion();
                        initTablaEconomico();
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function(error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.');
                }
            );
        }
        
        function setLstKubrixTablaDet(tipo, columna, renglon, fecha, nombreRow, negativo, divisionCol, areaCuentaCol, economicoCol, semanal) {
            if (dtTablaDetalles != null) { dtTablaDetalles.clear(); }

            $.post(getLstKubrixTablaDet, {
                corteID: cboHoraCorte.val(), 
                modelos: modelos,
                areaCuenta: areasCuentaGlobal, 
                economico: economico,
                //fechaInicio: fechaInicio,
                fechaFin: fechaFin, 
                tipoCorte: tipoReporte,
                tipo: tipo,
                columna: columna - 1, 
                renglon: renglon,
                divisionCol: divisionCol,
                areaCuentaCol: areaCuentaCol,
                economicoCol: economicoCol,
                subcuentaFiltro: subcuentaFiltro,
                subsubcuentaFiltro: subsubcuentaFiltro,
                divisionFiltro: divisionFiltro,
                areaCuentaFiltro: areaCuentaFiltro,
                conciliacionFiltro: conciliacionFiltro,
                economicoFiltro: economicoFiltro,
                empresa: empresaGlobal,
                semanal: semanal,
                reporteCostos: true,
                acumulado: cbConfiguracion.val()
            })
                .then(function(response) {
                    if (response.success) {

                        // Operación exitosa.
                        var detalles = response.detalles;
                        var sumadetalles = 0;
                        for(var i = 0; i < detalles.length; i++) { sumadetalles += detalles[i].importe; }
                         
                        cargarTablaDetalle(detalles, (empresaActual == 2 ? economicoFiltro : ((numAreasCuenta == 1 && botonBuscar.attr("data-obra") != "") ? divisionFiltro : areaCuentaFiltro)), sumadetalles, semanal)
                        //cargarTablaDetalle(detalles, nombreColumna, total);                        
                    } else {
                        // Operación no completada.
                        AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                    AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".");
                }
            );
            return true;
        }

        function setLstKubrixDetalle(tipo, columna, renglon, fecha, nombreRow, negativo, divisionCol, areaCuentaCol, economicoCol) {
            if (dtTablaSubCuenta != null) { dtTablaSubCuenta.clear(); }
            if (dtTablaSubSubCuenta != null) { dtTablaSubSubCuenta.clear(); }
            if (dtTablaDivision != null) { dtTablaDivision.clear(); }
            if (dtTablaAreaCuenta != null) { dtTablaAreaCuenta.clear(); }
            if (dtTablaConciliacion != null) { dtTablaConciliacion.clear(); }
            if (dtTablaEconomico != null) { dtTablaEconomico.clear(); }
            if (dtTablaDetalles != null) { dtTablaDetalles.clear(); }

            $.post(getLstKubrixDetalle, {
                corteID: cboHoraCorte.val(), 
                modelos: modelos,
                areaCuenta: areasCuentaGlobal, 
                economico: economico,
                fechaInicio: fechaInicio,
                fechaFin: fechaFin, 
                tipoCorte: tipoReporte,
                tipo: tipo,
                columna: columna, 
                renglon: renglon,
                divisionCol: divisionCol,
                areaCuentaCol: areaCuentaCol,
                economicoCol: economicoCol,
                reporteCostos: true
            })
                .then(function(response) {
                    if (response.success) {

                        // Operación exitosa.
                        var detalles = response.detalles;
                        var sumadetalles = 0;
                        for(var i = 0; i < detalles.length; i++) { sumadetalles += detalles[i].importe; }
                        
                        cargarTablaSubCuenta(detalles, nombreRow, fecha, 1, negativo); 
                        columnaGlobal = columna;
                        renglonGlobal = renglon;
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

        function setLstAnalisisDetalle(cuenta, fechaMin, fechaMax, tipo, nombreColumna, total, tipoEquipoMayor) {
            $.post(getLstKubrixDetalle, {                 
                corteID: botonBuscar.attr("data-corteID"), 
                modelos: modelos, 
                economico: cboMaquina.val() == "" ? null : $("#cboMaquina option:selected").text().trim(), 
                fechaInicio: fechaMin, 
                fechaFin: fechaMax, 
                cuenta: cuenta, 
                tipo: tipo, 
                areaCuenta: botonBuscar.attr("data-obra"),
                tipoEquipoMayor: tipoEquipoMayor
            })
                .then(function(response) {                    
                    if (response.success) {
                        // Operación exitosa.
                        var detalles = response.lst;
                        var sumadetalles = 0;
                        for(var i = 0; i < detalles.length; i++)
                        {
                            sumadetalles += detalles[i].importe;
                        }
                        //cargarTablaSubCuenta(detalles, nombreColumna, total); 
                        mostrarSubdetalles(detalles, nombreColumna, total);                        
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

        //function setLstKubrixDetNivelUno(busq, nombreColumna, total) {
        //    $.post(getLstKubrixDetalle, { busq: busq })
        //        .then(function(response) {
        //            if (response.success) {
        //                // Operación exitosa.
        //                var detalles = response.lst;
        //                var sumadetalles = 0;
        //                for(var i = 0; i < detalles.length; i++)
        //                {
        //                    sumadetalles += detalles[i].importe;
        //                }
        //                cargarTablaSubSubCuenta(detalles, nombreColumna, total);
        //            } else {
        //                // Operación no completada.
        //                AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
        //            }
        //        }, function(error) {
        //            // Error al lanzar la petición.
        //            AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".");
        //        }
        //    );
        //}

        function cargarHorasCorte()
        {
            cboHoraCorte.fillCombo('/Rentabilidad/getHorasCorte', { fecha: inputCorte.val(), tipoCorte: cbTipoCorte.val() }, null, "");
            cboHoraCorte.find('option').get(0).remove();
        }

        function unique(array){
            return array.filter(function(el, index, arr) {
                return index === arr.indexOf(el);
            });
        }

        function setFormatoKubrixDetalles(e, datos) {
            //$("#chbAgrupacionDet").show(500);
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            let busq = getBusquedaDTODetalle();
            var lst = obtenerListaKubrix(datos, busq);  
            if (botonBuscar.attr("data-tipoCorte") == 0) { initTablaKubrixDetalle(lst, 1); }
            else { initTablaKubrixDetalle(lst, 3); }
            if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
            if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
            if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
            if(!$("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").show(500);
            $("#divTablaKubrixDetalle").show(500);
        }

        function setFormatoKubrixDetallesEconomico(e, datos) {
            //$("#comboAgrupacionDet").show(500);
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            let busq = getBusquedaDTODetalle();
            var lst = $("#chbDespliegueDetEco").is(":checked") ? obtenerListaKubrixEcoCompacto(datos, busq) : obtenerListaKubrixEconomico(datos, busq);
            initTablaKubrixDetalleEco(lst);
            if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
            if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
            if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
            if(!$("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").show(500);
            $("#divTablaKubrixDetalleEco").show(500);
        }
        
        function FormatoDetalles(source, separadores, tipo)
        {
            var datos = [];
            var detalles = [];
            var detalles2 = [];
            var auxseparadores = [];
            for(var i = 1; i <= 4; i++)
            {
                detalles = [];
                detalles2 = [];
                auxseparadores = [];
                switch(i)
                {
                    case 1:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0 && el != null); });
                        //if(tipo == 2) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (el.semana == 1); }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (el.semana == 1); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 2:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5000-10-") >= 0 && el != null); });     
                        //if(tipo == 2) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (el.semana == 1); }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (el.semana == 1); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 3:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5280-") >= 0 && el != null); }); 
                        //if(tipo == 2) $.each(detalles, function(index,element){ element.importe = element.importe * (-1)});
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (el.semana == 1); }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (el.semana == 1); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 4:
                        detalles = $.merge(detalles, datos[0].detalles);
                        detalles = $.merge(detalles, datos[1].detalles);
                        detalles = $.merge(detalles, datos[2].detalles);
                        detalles = $.merge(detalles, datos[3].detalles);
                        
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (el.semana == 1); }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += element.monto || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return ((el.semana == 1) && (tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j]));
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (el.semana == 1); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        detalles = [];
                        break;
                }
                var auxDatos = { descripcion: conceptos[i - 1], separadores: auxseparadores, detalles: detalles };
                datos.push(auxDatos);
                if(i == 2)
                {
                    detalles = [];
                    detalles2 = [];
                    auxseparadores = [];
                    detalles = $.grep(source, function(el,index){ return (el.cuenta == "1-4-0" && el != null); }); 
                    var totalImporte = 0;
                    var detallesTotal = $.grep(detalles, function(el,index){ return (el.semana == 1); }); 
                    if(detallesTotal.length > 0)
                    {
                        $.each(detallesTotal, function(index, element){
                            totalImporte += (element.monto) || 0;
                        });
                    }
                    auxseparadores.push(totalImporte);
                    for(var j = 1; j <= separadores.length; j++)
                    {
                        var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                            return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                        });
                        var auxseparadoresImporte = 0
                        var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (el.semana == 1); }); 
                        if(detallesTotalDetalle.length > 0)
                        {
                            $.each(detallesTotalDetalle, function(index, element){
                                auxseparadoresImporte += (element.monto) || 0;
                            });
                        }
                        auxseparadores.push(auxseparadoresImporte);
                    }
                    var auxDatos = { descripcion: "Costos Estimados", separadores: auxseparadores, detalles: detalles };
                    datos.push(auxDatos);
                }
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
                dom: '<f<t>>',
                data: data,
                columns: getKubrixColumnsDetalle(tipo),
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[0, 'asc']],
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaKubrixDetalle_filter input').addClass("form-control input-sm");
                },
                drawCallback: function( settings ) {    
                    //visibilidad botones
                    if(numAreaCuenta != 1 && cboMaquina.val() == "TODOS") $('#botonAtrasDetalle').css("display", "inline-block");
                    else $('#botonAtrasDetalle').css("display", "none");
                    $("#chbTipoReporteDetalle").parent().css("display", "inline-block");
                    $("#chbDespliegueDetEco").parent().css("display", "none");
                    //Detalles filtros
                    $("#divLblFiltrosDetalle")
                        .html('<b>División:</b> ' + (stringDivision == "TODAS" ? $("#comboDivision option:selected").text().trim() : stringDivision) + 
                        '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Area Cuenta:</b> ' + (stringAreaCuenta == "TODAS" ? ($("#comboAC option:selected").text().trim() == "" ? "TODAS" : $("#comboAC option:selected").toArray().map(function(item) { return item.text; }).join(", ")) : stringAreaCuenta) + 
                        '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + $("#cbConfiguracion option:selected").text().trim() 
                        + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputCorte.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));

                    CargarGraficaKubrix(data, tipo, "", true);

                    tablaKubrixDetalle.find('p.desplegable').unbind().click(function (e) {                        
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaKubrixDetalle.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const indexCol = $(this).parents("td").index();
                        const indexRow = $(this).closest('tr').index();
                        var negativo = false;
                        if (indexRow == 4 || indexRow == 5 || indexRow == 6 || indexRow == 8){ $.each(detalles, function(index, element){ negativo = true; }); }
                        const nombreColumna = $('#tablaKubrixDetalle thead tr th').eq($(this).parents("td").index()).text().trim();
                        var fechaMax = moment(botonBuscar.attr("data-fechaFin"), "DD-MM-YYYY").toDate();
                        var auxFecha = new Date(fechaMax);
                        var auxTipo = 0;
                        var numSemana = 1;
                        switch(nombreColumna)
                        {
                            case "Actual":
                            case "Empresa":
                                break;
                            case "Semana 2":
                                numSemana = 2;
                                auxFecha.setDate(fechaMax.getDate() - 7)
                                break;
                            case "Semana 3":
                                numSemana = 3;
                                auxFecha.setDate(fechaMax.getDate() - 14)
                                break;
                            case "Semana 4":
                                numSemana = 4;
                                auxFecha.setDate(fechaMax.getDate() - 21)
                                break;
                            case "Semana 5":
                                numSemana = 3;
                                auxFecha.setDate(fechaMax.getDate() - 28)
                                break;
                            case "EquipoMayor":
                                auxTipo = 1;
                                detalles = jQuery.grep(detalles, function( n, i ) { return n.tipo == 1;  });
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

                        ////if(auxTipo != 0) {setLstKubrixDetalle(4, auxTipo, indexRow, auxFecha, rowData.descripcion, negativo, divisionFiltroDetalle, areaCuentaFiltroDetalle, ""); tipoTabla = 4;}
                        ////else {setLstKubrixDetalle(3, numSemana, indexRow, auxFecha, rowData.descripcion, negativo, divisionFiltroDetalle, areaCuentaFiltroDetalle, ""); tipoTabla = 3;}
                        //////cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, auxFecha, auxTipo, true, numSemana, negativo); 
                        ////banderaTablaDetalle = true;

                        if(auxTipo != 0) { columnaGlobal = auxTipo; }
                        else columnaGlobal = numSemana;                        
                        renglonGlobal = indexRow

                        cargarTablaSubCuenta(detalles, rowData.descripcion, auxFecha, numSemana, negativo); 
                        banderaTablaDetalle = true;

                    });
                    $(".separador").click(function (e) {   
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();                      
                        if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                        if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                        if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                        if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                        //$("#chbAgrupacionDet").hide(500);
                        $("#divTablaKubrixAreaCuenta").show(500);
                    });
                    $('#tablaKubrixDetalle tbody td').addClass("blurry");
                    $('#tablaKubrixDetalle tbody').fadeIn(800);
                    setTimeout(function () {
                        $('#tablaKubrixDetalle tbody td').removeClass("blurry");
                    }, 600);
                    
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
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 1 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 1, 1);                                    
                                    break;
                                case "2":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO MENOR");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 2 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "3":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO TRANSPORTE CONSTRUPLAN");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 3 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "4":
                                    $("#tituloModal").text("ARRENDADORA - FLETES");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 4 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "5":
                                    $("#tituloModal").text("ARRENDADORA - OTR");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 5 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "6":
                                    $("#tituloModal").text("ARRENDADORA - ADMINISTRATIVO CENTRAL");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 6 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "7":
                                    $("#tituloModal").text("ARRENDADORA - OTROS");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 7 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "8":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO TRANSPORTE ARRENDADORA");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 8 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "9":
                                    $("#tituloModal").text("ARRENDADORA - ADMINISTRATIVO PROYECTOS");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 9 && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                default:
                                    $("#tituloModal").text("ARRENDADORA");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.semana == 1; });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                            }
                            //$("#botonNombreNivelCero").attr("data-ejercicioActual", ejercicioActual);
                            //$("#botonBuscarAnalisis").click();
                            //$("#comboTipoAnalisis").change();
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


            //Label filtros

        }

        function initTablaKubrixDetalleEco(data) {
            if (dtTablaKubrixDetalleEco != null) {
                dtTablaKubrixDetalleEco.destroy();
                tablaKubrixDetalleEco.empty();
                tablaKubrixDetalleEco.append('<thead class="bg-table-header"></thead>');
            }
            dtTablaKubrixDetalleEco = tablaKubrixDetalleEco.DataTable({
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
                order: [[0, 'asc']],
                dom: '<f<t>>',
                data: data,
                columns: getColumnasDetalleEco(),
                rowGroup: {
                    // Uses the 'row group' plugin
                    dataSrc: 'grupoMaquina',
                    startRender: function (rows, group) {
                        var collapsed = !collapsedGroups[group];

                        rows.nodes().each(function (r) {
                            r.style.display = collapsed ? 'none' : '';
                        });    
                        var data = rows.data();
                        if($("#chbDespliegueDetEco").is(":checked"))
                        {
                            var ingresos = data.map(function(x) { return x.ingresos; }).reduce(function(total, importe) { return total + importe; });
                            var costos = data.map(function(x) { return x.costos; }).reduce(function(total, importe) { return total + importe; });
                            var resultadoNeto = data.map(function(x) { return x.resultadoNeto; }).reduce(function(total, importe) { return total + importe; });
                            var margen = ingresos != 0 ? (resultadoNeto / ingresos) * 100 : 0;
                            var detalles = data.map(function(x) { return x.dealles; });
                            // Add category name to the <tr>. NOTE: Hardcoded colspan
                            return $('<tr/>')
                                .append('<td class="text-center">' + group + ' (' + rows.count() + ')</td><td class="text-center">' + getRowHTML(costos) + '</td><td class="text-center">' + getNumberHTML(resultadoNeto) + '</td>')
                                .attr('data-name', group)
                                .toggleClass('collapsed', collapsed);
                        }
                        else
                        {

                            var costoTotal = data.map(function(x) { return x.costoTotal; }).reduce(function(total, importe) { return total + importe; });
                            var depreciacion = data.map(function(x) { return x.depreciacion; }).reduce(function(total, importe) { return total + importe; });
                            var costosEstimados = data.map(function(x) { return x.costosEstimados; }).reduce(function(total, importe) { return total + importe; });

                            var gastosOperacion = data.map(function(x) { return x.gastosOperacion; }).reduce(function(total, importe) { return total + importe; });

                            var gastosProductosFinancieros = data.map(function(x) { return x.gastosProductosFinancieros; }).reduce(function(total, importe) { return total + importe; });

                            var otrosIngresos = data.map(function(x) { return x.otrosIngresos; }).reduce(function(total, importe) { return total + importe; });
                            var resultadoNeto = data.map(function(x) { return x.resultadoNeto; }).reduce(function(total, importe) { return total + importe; });
                            var detalles = data.map(function(x) { return x.dealles; });
                            // Add category name to the <tr>. NOTE: Hardcoded colspan
                            return $('<tr/>')
                                .append('<td class="text-center">' + group + ' (' + rows.count() + ')</td>' +

                                '<td class="text-center">' + getRowHTML(costoTotal) + '</td>' +
                                '<td class="text-center">' + getRowHTML(depreciacion) + '</td>' +
                                '<td class="text-center">' + getRowHTML(costosEstimados) + '</td>' +

                                '<td class="text-center">' + getRowHTML(gastosOperacion) + '</td>' +

                                '<td class="text-center">' + getRowHTML(gastosProductosFinancieros) + '</td>' +

                                '<td class="text-center">' + getRowHTML(otrosIngresos) + '</td>' +
                                '<td class="text-center">' + getNumberHTML(resultadoNeto) + '</td>')
                                .attr('data-name', group)
                                .toggleClass('collapsed', collapsed);
                        }
                    }
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                
                fnInitComplete: function (oSettings, json) {
                    $('div#tablaKubrixDetalleEco_filter input').addClass("form-control input-sm");
                },
                drawCallback: function( settings ) {
                    $('#tablaKubrixDetalleEco tbody').on('click', 'tr.group-start', function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        var name = $(this).data('name');
                        collapsedGroups[name] = !collapsedGroups[name];
                        dtTablaKubrixDetalleEco.draw(false);
                    });  
                    //Visibilidad botones
                    $("#chbTipoReporteDetalle").parent().css("display", "none");
                    $("#chbDespliegueDetEco").parent().css("display", "inline-block");

                    $("#divLblFiltrosDetalleEco")
                        .html('<b>División:</b> ' + (stringDivision == "TODAS" ? $("#comboDivision option:selected").text().trim() : stringDivision) + 
                        '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Area Cuenta:</b> ' + (stringAreaCuenta == "TODAS" ? ($("#comboAC option:selected").text().trim() == "" ? "TODAS" : $("#comboAC option:selected").toArray().map(function(item) { return item.text; }).join(", ")) : stringAreaCuenta) + 
                        '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + $("#cbConfiguracion option:selected").text().trim() 
                        + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputCorte.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));
                    

                    tablaKubrixDetalleEco.find('p.desplegable').unbind().click(function (e) {
                        
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaKubrixDetalleEco.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        var negativo = false;
                        const indexCol = $(this).parents("td").index();
                        const nombreColumna = $('#tablaKubrixDetalleEco thead tr th').eq($(this).parents("td").index()).text().trim();
                        economicoFiltro = rowData.descripcion;
                        var fechaMax = moment(botonBuscar.attr("data-fechaFin"), "DD-MM-YYYY").toDate();
                        var auxFecha = new Date(fechaMax);
                        var auxTipo = 0;
                        var numSemana = 1;
                        var tablaResumida = $("#chbDespliegueDetEco").is(":checked");
                        if(tablaResumida)
                        {
                            switch(indexCol)
                            {
                                case 1:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta.indexOf("5000-") >= 0 || el.cuenta.indexOf("5900-") >= 0 || el.cuenta.indexOf("5901-") >= 0
                                        || el.cuenta.indexOf("5280-") >= 0 || el.cuenta == "1-4-0"); });
                                    negativo = true;
                            }
                        }
                        else
                        {
                            switch(indexCol)
                            {
                                case 1:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0) ; });
                                    negativo = true;
                                    break;
                                case 2:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta.indexOf("5000-10-") >= 0 ; });
                                    negativo = true;
                                    break;
                                case 3:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta == "1-4-0" ; });
                                    negativo = true;
                                    break;
                                case 4:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta.indexOf("5280-") >= 0 ; });
                                    negativo = true;
                                    break;
                                case 5:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta.indexOf("5900-") >= 0; });
                                    break;
                                case 6:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta.indexOf("5901-") >= 0 ; });
                                    break;
                            }
                        }

                        //////cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, auxFecha, auxTipo, true, numSemana, negativo); 
                        ////setLstKubrixDetalle(5, 0, tablaResumida ? (indexCol - 2) : (indexCol - 1), auxFecha, rowData.descripcion, negativo, divisionFiltroDetalle, areaCuentaFiltroDetalle, rowData.descripcion);
                        ////banderaTablaDetalle = true;

                        columnaGlobal = 0;                        
                        renglonGlobal = tablaResumida ? (indexCol - 3) : (indexCol - 1);
                        cargarTablaSubCuenta(detalles, rowData.descripcion, auxFecha, numSemana, negativo); 
                        banderaTablaDetalle = true;
                    });
                },
            });
        }

        function getColumnasDetalleEco()
        {
            if($("#chbDespliegueDetEco").is(":checked"))
            {
                return [                   
                    { data: 'grupoMaquina', title: 'Grupo Maquina', visible: false },
                    { data: 'descripcion', title: 'Concepto' },
                    { data: 'costos', title: '<span class="tituloKubrixEc">Costos</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'resultadoNeto', title: '<span class="tituloKubrixEc">Total</span>', render: function (data, type, row) { return getNumberHTML(data); } },   
                ];
            }
            else
            {
                return[
                    { data: 'grupoMaquina', title: 'Grupo Maquina', visible: false },
                    { data: 'descripcion', title: 'Concepto' },
                    { data: 'costoTotal', title: '<span class="tituloKubrixEc">Costo Total</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'depreciacion', title: '<span class="tituloKubrixEc">Deprecia- ción</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'costosEstimados', title: '<span class="tituloKubrixEc">Costos Estimados</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'gastosOperacion', title: '<span class="tituloKubrixEc">Gastos de Operación</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'resultadoNeto', title: '<span class="tituloKubrixEc">Total</span>', render: function (data, type, row) { return getNumberHTML(data); } },
                ];
            }
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
                dom: '<f<text-center t>>',
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
                    $("#divLblFiltrosDivision")
                       .html('<b>División:</b> ' + (stringDivision == "TODAS" ? $("#comboDivision option:selected").text().trim() : stringDivision) + 
                       '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Area Cuenta:</b> ' + (stringAreaCuenta == "TODAS" ? ($("#comboAC option:selected").text().trim() == "" ? "TODAS" : $("#comboAC option:selected").toArray().map(function(item) { return item.text; }).join(", ")) : stringAreaCuenta) + 
                       '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + $("#cbConfiguracion option:selected").text().trim() 
                       + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputCorte.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));

                    tablaKubrixDivision.find('p.desplegable').unbind().click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaKubrixDivision.row($(this).parents('tr')).data();
                        const indexRow = $(this).closest('tr').index();
                        var detalles = rowData.detalles;
                        var negativo = false;
                        const nombreColumna = $('#tablaKubrixDivision thead tr th').eq($(this).parents("td").index()).text().trim();
                        var fechaMax = new Date(botonBuscar.attr("data-fechaFin"));
                        if(nombreColumna != nombreTotal){ detalles = jQuery.grep(detalles, function( n, i ) { return n.division.trim() == nombreColumna; }); }
                        if(indexRow ==  4 || indexRow == 5 || indexRow == 6 ||indexRow == 8){ negativo = true; }
                        divisionFiltroDetalle = nombreColumna;
                        ////////cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, fechaMax, 0, false, 1, negativo);
                        //////setLstKubrixDetalle(1, 0, indexRow, fechaMax, rowData.descripcion, negativo, nombreColumna, "", "");
                        //////banderaTablaDetalle = false;

                        columnaGlobal = 0;                        
                        renglonGlobal = indexRow;
                        cargarTablaSubCuenta(detalles, rowData.descripcion, fechaMax, 1, negativo); 
                        banderaTablaDetalle = false;
                    });
                    $('#tablaKubrixDivision tbody').fadeIn(800);
                    
                    $(".separador").click(function (e) {   
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        $.blockUI({ baseZ: 2000, message: 'Procesando...' });
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
                        areasCuentaDetalle = auxAreasCuenta;
                        var datos = FormatoDetalles(detalles, areasCuenta, 2);
                        if(nombreColumna == nombreTotal)
                        {
                            stringDivision = "TODAS";
                            initTablaKubrixAreaCuenta(datos, areasCuenta, nombreColumna);
                            if($("#chbAgrupacionDetalle").is(":checked")) setFormatoKubrixDetalles(e, detalles);
                            else setFormatoKubrixDetallesEconomico(e, detalles);  
                            divisionFiltroDetalle = "";
                        }else{
                            stringDivision = nombreColumna;
                            initTablaKubrixAreaCuenta(datos, areasCuenta, nombreColumna);                            
                            if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                            if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                            if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                            if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                            $("#divTablaKubrixAreaCuenta").show(500);
                            divisionFiltroDetalle = nombreColumna;
                        }
                        $.unblockUI();
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( dataIndex == 4 || dataIndex == 7 || dataIndex == 9 || dataIndex == 11 || dataIndex == 13 ||  dataIndex == 14){
                        $(row).addClass('resultado');
                    }
                },
            });
            $('[data-toggle="tooltip"]').tooltip();
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
                dom: '<f<t>>',
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
                    if(comboResponsable.val() == "TODOS") $('#botonAtrasAC').css("display", "inline-block");
                    else $('#botonAtrasAC').css("display", "none");

                    $("#divLblFiltrosAC")
                       .html('<b>División:</b> ' + (stringDivision == "TODAS" ? $("#comboDivision option:selected").text().trim() : stringDivision) + 
                       '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Area Cuenta:</b> ' + (stringAreaCuenta == "TODAS" ? ($("#comboAC option:selected").text().trim() == "" ? "TODAS" : $("#comboAC option:selected").toArray().map(function(item) { return item.text; }).join(", ")) : stringAreaCuenta) + 
                       '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + $("#cbConfiguracion option:selected").text().trim() 
                       + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputCorte.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));
                    
                    tablaKubrixAreaCuenta.find('p.desplegable').unbind().click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaKubrixAreaCuenta.row($(this).parents('tr')).data();
                        const indexRow = $(this).closest('tr').index();
                        var detalles = rowData.detalles;
                        var negativo = false;
                        if(indexRow ==  4 || indexRow == 5 || indexRow == 6 || indexRow == 8){ negativo = true; }
                        const nombreColumna = $('#tablaKubrixAreaCuenta thead tr th').eq($(this).parents("td").index()).text().trim();
                        var fechaMax = new Date(botonBuscar.attr("data-fechaFin"));
                        if(nombreColumna != nombreTotal){ detalles = jQuery.grep(detalles, function( n, i ) { return n.areaCuenta.trim() == nombreColumna; }); }
                        areaCuentaFiltroDetalle = nombreColumna;
                        ////////cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, fechaMax, 0, false, 1, negativo);  
                        //////setLstKubrixDetalle(2, 0, indexRow, fechaMax, rowData.descripcion, negativo, divisionFiltroDetalle, nombreColumna, "");
                        //////banderaTablaDetalle = false;
                        columnaGlobal = 0;                        
                        renglonGlobal = indexRow;
                        cargarTablaSubCuenta(detalles, rowData.descripcion, fechaMax, 1, negativo); 
                        banderaTablaDetalle = false;
                    });
                    $('#tablaKubrixAreaCuenta tbody').fadeIn(800);
                    
                    $(".separador").click(function (e) {   
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        $.blockUI({ baseZ: 2000, message: 'Procesando...' });
                        let rowData = dtTablaKubrixAreaCuenta.data();
                        const nombreColumna = $(this).text().trim();                        
                        var detallesRaw = rowData.map(function(x) { return x.detalles});
                        var detalles = [].concat.apply([], detallesRaw);
                        detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                        stringAreaCuenta = "TODAS";
                        if(nombreColumna != nombreTotal) {
                            stringAreaCuenta = nombreColumna;
                            detalles = $.grep(detalles, function( n, i ) { return n.areaCuenta != null && n.areaCuenta.trim() == nombreColumna; });
                            areasCuentaDetalle = [nombreColumna];
                            areaCuentaFiltroDetalle = nombreColumna;
                        }
                        var datos = detalles;
                        var semanas= [];
                        if($("#chbAgrupacionDetalle").is(":checked")) setFormatoKubrixDetalles(e, datos);
                        else setFormatoKubrixDetallesEconomico(e, datos);   
                        $.unblockUI();
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( dataIndex == 4 || dataIndex == 7 || dataIndex == 9 || dataIndex == 11 || dataIndex == 12 ||  dataIndex == 14){
                        $(row).addClass('resultado');
                    }
                },
            });
            $('[data-toggle="tooltip"]').tooltip();
            //switch tipo reporte

        }

        function obtenerListaAnalisis(detalles, idTipo)
        {
            var fechaMax = moment(botonBuscar.attr("data-fechaFin"), "DD-MM-YYYY").toDate();
            var rowData = [];            
            for(var i = 1; i <= 14; i++)
            {                
                var auxRowData = { tipo_mov: 0, descripcion: "", actual: 0, semana2: 0, semana3: 0, semana4: 0, semana5: 0, cfc: 0, cf: 0, mc: 0, pr: 0, tc: 0, car: 0, ex: 0, hdt: 0, otros: 0, detalles: [] }
                var auxDetallesNuevo = [];
                switch(i)
                {
                    case 1: 
                    //    var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 1; });
                    //    var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                    //    auxRowData.descripcion = "Ingresos Contabilizados"; 
                    //    auxRowData.tipo_mov = 1;
                    //    auxRowData.actual = costos.actual;
                    //    auxRowData.semana2 = costos.semana2;
                    //    auxRowData.semana3 = costos.semana3;
                    //    auxRowData.semana4 = costos.semana4;
                    //    auxRowData.semana5 = costos.semana5;
                    //    auxRowData.cfc = costos.cfc;
                    //    auxRowData.cf = costos.cf;
                    //    auxRowData.mc = costos.mc;
                    //    auxRowData.pr = costos.pr;
                    //    auxRowData.tc = costos.tc;
                    //    auxRowData.car = costos.car;
                    //    auxRowData.ex = costos.ex;
                    //    auxRowData.hdt = costos.hdt;
                    //    auxRowData.otros = costos.otros;                        
                    //    $.each(auxDetalles, function(i, n) { 
                    //        var item = n;
                    //        auxDetallesNuevo.push(n);
                    //    }); 
                    //    auxRowData.detalles = auxDetallesNuevo;
                    //    rowData.push(auxRowData);
                    //    break;
                    //case 2:                         
                    //    var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 2; });
                    //    var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                    //    auxRowData.descripcion = "Ingresos con Estimación";
                    //    auxRowData.tipo_mov = 2;
                    //    auxRowData.actual = costos.actual;
                    //    auxRowData.semana2 = costos.semana2;
                    //    auxRowData.semana3 = costos.semana3;
                    //    auxRowData.semana4 = costos.semana4;
                    //    auxRowData.semana5 = costos.semana5;
                    //    auxRowData.cfc = costos.cfc;
                    //    auxRowData.cf = costos.cf;
                    //    auxRowData.mc = costos.mc;
                    //    auxRowData.pr = costos.pr;
                    //    auxRowData.tc = costos.tc;
                    //    auxRowData.car = costos.car;
                    //    auxRowData.ex = costos.ex;
                    //    auxRowData.hdt = costos.hdt;
                    //    auxRowData.otros = costos.otros;
                    //    $.each(auxDetalles, function(i, n) { 
                    //        var item = n;
                    //        auxDetallesNuevo.push(n);
                    //    }); 
                    //    auxRowData.detalles = auxDetallesNuevo;
                    //    rowData.push(auxRowData);
                    //    break;
                    //case 3:                         
                    //    var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 3; });
                    //    var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                    //    auxRowData.descripcion = "Ingresos Pendientes por Generar"; 
                    //    auxRowData.tipo_mov = 3;
                    //    auxRowData.actual = costos.actual ;
                    //    auxRowData.semana2 = costos.semana2 ;
                    //    auxRowData.semana3 = costos.semana3 ;
                    //    auxRowData.semana4 = costos.semana4 ;
                    //    auxRowData.semana5 = costos.semana5 ;
                    //    auxRowData.cfc = costos.cfc ;
                    //    auxRowData.cf = costos.cf ;
                    //    auxRowData.mc = costos.mc ;
                    //    auxRowData.pr = costos.pr ;
                    //    auxRowData.tc = costos.tc ;
                    //    auxRowData.car = costos.car ;
                    //    auxRowData.ex = costos.ex;
                    //    auxRowData.hdt = costos.hdt;
                    //    auxRowData.otros = costos.otros ;
                    //    $.each(auxDetalles, function(i, n) { 
                    //        var item = n;
                    //        auxDetallesNuevo.push(n);
                    //    }); 
                    //    auxRowData.detalles = auxDetallesNuevo;
                    //    rowData.push(auxRowData);
                    //    break;
                    //case 4:                         
                    //    auxRowData.descripcion = "Total Ingresos"; 
                    //    auxRowData.tipo_mov = 4;
                    //    auxRowData.actual = rowData[0].actual + rowData[1].actual + rowData[2].actual;
                    //    auxRowData.semana2 = rowData[0].semana2 + rowData[1].semana2 + rowData[2].semana2;
                    //    auxRowData.semana3 = rowData[0].semana3 + rowData[1].semana3 + rowData[2].semana3;
                    //    auxRowData.semana4 = rowData[0].semana4 + rowData[1].semana4 + rowData[2].semana4;
                    //    auxRowData.semana5 = rowData[0].semana5 + rowData[1].semana5 + rowData[2].semana5;
                    //    auxRowData.cfc = rowData[0].cfc + rowData[1].cfc + rowData[2].cfc;
                    //    auxRowData.cf = rowData[0].cf + rowData[1].cf + rowData[2].cf;
                    //    auxRowData.mc = rowData[0].mc + rowData[1].mc + rowData[2].mc;
                    //    auxRowData.pr = rowData[0].pr + rowData[1].pr + rowData[2].pr;
                    //    auxRowData.tc = rowData[0].tc + rowData[1].tc + rowData[2].tc;
                    //    auxRowData.car = rowData[0].car + rowData[1].car + rowData[2].car;
                    //    auxRowData.ex = rowData[0].ex + rowData[1].ex + rowData[2].ex;
                    //    auxRowData.hdt = rowData[0].hdt + rowData[1].hdt + rowData[2].hdt;
                    //    auxRowData.otros = rowData[0].otros + rowData[1].otros + rowData[2].otros;
                    //    rowData.push(auxRowData);
                    //    break;
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
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            n.importe = n.importe;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData);
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
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            n.importe = n.importe;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData);

                        auxDetallesNuevo = [];
                        var auxRowData2 = { tipo_mov: 0, descripcion: "", actual: 0, semana2: 0, semana3: 0, semana4: 0, semana5: 0, cfc: 0, cf: 0, mc: 0, pr: 0, tc: 0, car: 0, ex: 0, hdt: 0, otros: 0, detalles: [] }
                        var auxDetalles2 = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 15; });
                        var costos2  = AsignarImportesAnalisis(auxDetalles2, fechaMax);
                        auxRowData2.descripcion = "Costo Estimado";
                        auxRowData2.tipo_mov = 15;
                        auxRowData2.actual = costos2.actual;
                        auxRowData2.semana2 = costos2.semana2;
                        auxRowData2.semana3 = costos2.semana3;
                        auxRowData2.semana4 = costos2.semana4;
                        auxRowData2.semana5 = costos2.semana5;
                        auxRowData2.cfc = costos2.cfc;
                        auxRowData2.cf = costos2.cf;
                        auxRowData2.mc = costos2.mc;
                        auxRowData2.pr = costos2.pr;
                        auxRowData2.tc = costos2.tc;
                        auxRowData2.car = costos2.car;
                        auxRowData2.ex = costos2.ex;
                        auxRowData2.hdt = costos2.hdt;
                        auxRowData2.otros = costos2.otros;                        
                        $.each(auxDetalles2, function(i, n) { 
                            var item = n;
                            n.importe = n.importe;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData2.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData2);
                        break;
                    case 7: 
                        auxRowData.descripcion = "Utilidad Bruta"; 
                        auxRowData.tipo_mov = 7;
                        auxRowData.actual = rowData[3].actual - rowData[4].actual - rowData[5].actual - rowData[6].actual;
                        auxRowData.semana2 = rowData[3].semana2 - rowData[4].semana2 - rowData[5].semana2 - rowData[6].semana2;
                        auxRowData.semana3 = rowData[3].semana3 - rowData[4].semana3 - rowData[5].semana3 - rowData[6].semana3;
                        auxRowData.semana4 = rowData[3].semana4 - rowData[4].semana4 - rowData[5].semana4 - rowData[6].semana4;
                        auxRowData.semana5 = rowData[3].semana5 - rowData[4].semana5 - rowData[5].semana5 - rowData[6].semana5;
                        auxRowData.cfc = rowData[3].cfc - rowData[4].cfc - rowData[5].cfc - rowData[6].cfc;
                        auxRowData.cf = rowData[3].cf - rowData[4].cf - rowData[5].cf - rowData[6].cf;
                        auxRowData.mc = rowData[3].mc - rowData[4].mc - rowData[5].mc - rowData[6].mc;
                        auxRowData.pr = rowData[3].pr - rowData[4].pr - rowData[5].pr - rowData[6].pr;
                        auxRowData.tc = rowData[3].tc - rowData[4].tc - rowData[5].tc - rowData[6].tc;
                        auxRowData.car = rowData[3].car - rowData[4].car - rowData[5].car - rowData[6].car;
                        auxRowData.ex = rowData[3].ex - rowData[4].ex - rowData[5].ex - rowData[6].ex;
                        auxRowData.hdt = rowData[3].hdt - rowData[4].hdt - rowData[5].hdt - rowData[6].hdt;
                        auxRowData.otros = rowData[3].otros - rowData[4].otros - rowData[5].otros - rowData[6].otros;
                        rowData.push(auxRowData);
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
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            n.importe = n.importe;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData);
                        break;
                    case 9: 
                        auxRowData.descripcion = "Resultado Antes Financieros"; 
                        auxRowData.tipo_mov = 9;
                        auxRowData.actual = rowData[7].actual - rowData[8].actual;
                        auxRowData.semana2 = rowData[7].semana2 - rowData[8].semana2;
                        auxRowData.semana3 = rowData[7].semana3 - rowData[8].semana3;
                        auxRowData.semana4 = rowData[7].semana4 - rowData[8].semana4;
                        auxRowData.semana5 = rowData[7].semana5 - rowData[8].semana5;
                        auxRowData.cfc = rowData[7].cfc - rowData[8].cfc;
                        auxRowData.cf = rowData[7].cf - rowData[8].cf;
                        auxRowData.mc = rowData[7].mc - rowData[8].mc;
                        auxRowData.pr = rowData[7].pr - rowData[8].pr;
                        auxRowData.tc = rowData[7].tc - rowData[8].tc;
                        auxRowData.car = rowData[7].car - rowData[8].car;
                        auxRowData.ex = rowData[7].ex - rowData[8].ex;
                        auxRowData.hdt = rowData[7].hdt - rowData[8].hdt;
                        auxRowData.otros = rowData[7].otros - rowData[8].otros;
                        rowData.push(auxRowData);
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
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData);
                        break;
                    case 11: 
                        auxRowData.descripcion = "Resultado con Financieros"; 
                        auxRowData.tipo_mov = 11;
                        auxRowData.actual = rowData[9].actual + rowData[10].actual;
                        auxRowData.semana2 = rowData[9].semana2 + rowData[10].semana2;
                        auxRowData.semana3 = rowData[9].semana3 + rowData[10].semana3;
                        auxRowData.semana4 = rowData[9].semana4 + rowData[10].semana4;
                        auxRowData.semana5 = rowData[9].semana5 + rowData[10].semana5;
                        auxRowData.cfc = rowData[9].cfc + rowData[10].cfc;
                        auxRowData.cf = rowData[9].cf + rowData[10].cf;
                        auxRowData.mc = rowData[9].mc + rowData[10].mc;
                        auxRowData.pr = rowData[9].pr + rowData[10].pr;
                        auxRowData.tc = rowData[9].tc + rowData[10].tc;
                        auxRowData.car = rowData[9].car + rowData[10].car;
                        auxRowData.ex = rowData[9].ex + rowData[10].ex;
                        auxRowData.hdt = rowData[9].hdt + rowData[10].hdt;
                        auxRowData.otros = rowData[9].otros + rowData[10].otros;
                        rowData.push(auxRowData);
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
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData);
                        break;
                    case 13: 
                        auxRowData.descripcion = "Total"; 
                        auxRowData.tipo_mov = 13;
                        auxRowData.actual = rowData[11].actual + rowData[12].actual;
                        auxRowData.semana2 = rowData[11].semana2 + rowData[12].semana2;
                        auxRowData.semana3 = rowData[11].semana3 + rowData[12].semana3;
                        auxRowData.semana4 = rowData[11].semana4 + rowData[12].semana4;
                        auxRowData.semana5 = rowData[11].semana5 + rowData[12].semana5;
                        auxRowData.cfc = rowData[11].cfc + rowData[12].cfc;
                        auxRowData.cf = rowData[11].cf + rowData[12].cf;
                        auxRowData.mc = rowData[11].mc + rowData[12].mc;
                        auxRowData.pr = rowData[11].pr + rowData[12].pr;
                        auxRowData.tc = rowData[11].tc + rowData[12].tc;
                        auxRowData.car = rowData[11].car + rowData[12].car;
                        auxRowData.ex = rowData[11].ex + rowData[12].ex;
                        auxRowData.hdt = rowData[11].hdt + rowData[12].hdt;
                        auxRowData.otros = rowData[11].otros + rowData[12].otros;
                        rowData.push(auxRowData);
                        break;
                    case 14: 
                        auxRowData.descripcion = "% de Margen"; 
                        auxRowData.tipo_mov = 14;
                        auxRowData.actual = rowData[3].actual != 0 ? rowData[13].actual / rowData[3].actual * (100) : 0;
                        auxRowData.semana2 = rowData[3].semana2 != 0 ? rowData[13].semana2 / rowData[3].semana2 * (100) : 0;
                        auxRowData.semana3 = rowData[3].semana3 != 0 ? rowData[13].semana3 / rowData[3].semana3 * (100) : 0;
                        auxRowData.semana4 = rowData[3].semana4 != 0 ? rowData[13].semana4 / rowData[3].semana4 * (100) : 0;
                        auxRowData.semana5 = rowData[3].semana5 != 0 ? rowData[13].semana5 / rowData[3].semana5 * (100) : 0;
                        auxRowData.cfc = rowData[3].cfc != 0 ? rowData[13].cfc / rowData[3].cfc * (100) : 0;
                        auxRowData.cf = rowData[3].cf != 0 ? rowData[13].cf / rowData[3].cf * (100) : 0;
                        auxRowData.mc = rowData[3].mc != 0 ? rowData[13].mc / rowData[3].mc * (100) : 0;
                        auxRowData.pr = rowData[3].pr != 0 ? rowData[13].pr / rowData[3].pr * (100) : 0;
                        auxRowData.tc = rowData[3].tc != 0 ? rowData[13].tc / rowData[3].tc * (100) : 0;
                        auxRowData.car = rowData[3].car != 0 ? rowData[13].car / rowData[3].car * (100) : 0;
                        auxRowData.ex = rowData[3].ex != 0 ? rowData[13].ex / rowData[3].ex * (100) : 0;
                        auxRowData.hdt = rowData[3].hdt != 0 ? rowData[13].hdt / rowData[3].hdt * (100) : 0;
                        auxRowData.otros = rowData[3].otros != 0 ? rowData[13].otros / rowData[3].otros * (100) : 0;
                        rowData.push(auxRowData);
                        break;
                    default: auxRowData.descripcion = ""; break;
                }
                
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
                        if (index == 7)
                            return getNumberHTML(importeFinal);
                        return getRowHTML(importeFinal);
                    }
                }                
                columnas.push(auxColumna);
            });
            return columnas
        }

        function getKubrixColumnsDetalle(tipo) {
            switch(tipo)
            {
                case 1:
                    return [
                        { data: 'descripcion', title: 'Concepto' }
                        , {
                            data: 'actual',
                            title: numAreaCuenta == 1 || botonBuscar.attr("data-economico") != "TODOS" ? 'Actual' : '<button class="btn btn-sm btn-light separador" title="Actual">Actual</button>',
                            render: function (data, type, row) {
                                var importeFinal = data;
                                if (row.tipo_mov == 4 || row.tipo_mov == 7 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
                                    return getNumberHTML(importeFinal);
                                if (row.tipo_mov == 14)
                                    return importeFinal.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %";
                                return getRowHTML(importeFinal);
                            }
                        }
                        , {
                            data: 'semana2',
                            title: 'Semana 2',
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
                            title: 'Semana 3',
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
                            title: 'Semana 4',
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
                            title: 'Semana 5',
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
                    break;
                case 2:
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
                            title: '<button class="btn btn-sm btn-light filtrado" data-filtro="7">Otros</button>',
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
                    break;
                case 3:
                    const monthNames = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" ];
                    var fechaMax = moment(botonBuscar.attr("data-fechaFin"), "DD-MM-YYYY").toDate();
                    var auxFechaMensual = new Date();

                    auxFechaMensual = new Date(fechaMax.getFullYear(), fechaMax.getMonth(), 1);
                    var mes1 = monthNames[auxFechaMensual.getMonth()] + '<br>' + auxFechaMensual.getFullYear();

                    if((fechaMax.getMonth() - 2) < 0) auxFechaMensual = new Date(fechaMax.getFullYear() - 1, 12 + (fechaMax.getMonth() - 1), 1);
                    else auxFechaMensual = new Date(fechaMax.getFullYear(), fechaMax.getMonth() - 1, 1);
                    var mes2 = monthNames[auxFechaMensual.getMonth()] + '<br>' + auxFechaMensual.getFullYear();

                    if((fechaMax.getMonth() - 3) < 0) auxFechaMensual = new Date(fechaMax.getFullYear() - 1, 12 + (fechaMax.getMonth() - 2), 1);
                    else auxFechaMensual = new Date(fechaMax.getFullYear(), fechaMax.getMonth() - 2, 1);
                    var mes3 = monthNames[auxFechaMensual.getMonth()] + '<br>' + auxFechaMensual.getFullYear();

                    if((fechaMax.getMonth() - 4) < 0) auxFechaMensual = new Date(fechaMax.getFullYear() - 1, 12 + (fechaMax.getMonth() - 3), 1);
                    else auxFechaMensual = new Date(fechaMax.getFullYear(), fechaMax.getMonth() - 3, 1);
                    var mes4 = monthNames[auxFechaMensual.getMonth()] + '<br>' + auxFechaMensual.getFullYear();

                    if((fechaMax.getMonth() - 5) < 0) auxFechaMensual = new Date(fechaMax.getFullYear() - 1, 12 + (fechaMax.getMonth() - 4), 1);
                    else auxFechaMensual = new Date(fechaMax.getFullYear(), fechaMax.getMonth() - 4, 1);
                    var mes5 = monthNames[auxFechaMensual.getMonth()] + '<br>' + auxFechaMensual.getFullYear();
                    return [
                        { data: 'descripcion', title: 'Concepto' }
                        , {
                            data: 'actual',
                            title: mes1,
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
                            data: 'semana2',
                            title: mes2,
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
                            title: mes3,
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
                            title: mes4,
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
                            title: mes5,
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
                    break;
                default:
                    break;
            }

            if(tipo == 1){

            }
            else{
               
                
            }
        }

        function getRowHTML(value) {
            var auxiliar = '<p' + (value != 0 ? ' class="desplegable">' : '>') + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
        }

        function getRowHTMLFecha(value, fecha, tipo, semanal, empresa) {
            var auxiliar = '<p data-fecha="' + fecha + '" data-empresa="' + empresa + '" data-tipo="' + tipo + '" data-semanal="' + semanal + '" class="desplegable">' + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
        }

        function getNumberHTML(value) {
            return '<p class="' + (value != 0 ? 'noDesplegable' : '') + (value < 0 ? ' Danger' : '') + '" >' + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
        }

        function getRowHTMLFiltrado(value) {
            var auxiliar = '<p' + (value != 0 ? ' class="filtrado">' : '>') + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
            return auxiliar;
        }       
        function getRowHTMLFiltradoFecha(value, fecha, tipo, semanal) {
            var auxiliar = '<p' + (value != 0 ? ' data-fecha="' + fecha + '" data-tipo="' + tipo + '" class="filtrado">' : '>') + parseFloat(value / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
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
            return { 
                obra: comboAC.val()                            
                , tipo: 0
                , lstMaquina: (cboMaquina.val().trim() != "" ? [$("#cboMaquina option:selected").text().trim()] : null)
                , min: new Date()
                , max: inputDiaFinal.val()
                , cta: 0
                , scta: 0
                ,tm: []
                , tipoReporte: tipoReporte
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
            if (dtTablaSubCuenta != null) {
                dtTablaSubCuenta.destroy();
            }
            var tipoCorteLocal = botonBuscar.attr("data-tipocorte") == "0";
            dtTablaSubCuenta = tablaSubCuenta.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'semanal', title: tipoCorteLocal ? 'Semana' : 'Mes', render: function(data, type, row) 
                    { 
                        return getRowHTMLFecha(data, row.fecha, row.tipo, 1, row.empresa); 
                    } 
                    },
                    { data: 'importe', title: 'Acumulado', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0, row.empresa); } }
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
                        var semanal = $(this).attr("data-semanal") == "1";
                        var exito = cargarTablaSubSubCuenta(rowData.detalles, rowData.descripcion, $(this).html(), fecha, tipo, semanal);
                        if(exito){
                            botonTablaSubSubCuenta.show();
                            botonTablaSubCuenta.prop("disabled", false);
                        }
                        subcuentaFiltro = rowData.id;
                        empresaGlobal = $(this).attr("data-empresa");
                    });
                    tablaSubCuenta.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
                        const rowData = dtTablaSubCuenta.row($(this).parents('tr')).data();
                        var datosGrafica = [];
                        var suma = 0;
                        for(var i = 0; i < 5; i++) {
                            suma = 0;
                            if(numSemana < 5)
                            {
                                let auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) {
                                    return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false ));
                                });
                                auxGrafica.forEach(function(n) { suma += n.importe; });                                
                            }
                            datosGrafica.push([(4 - i), suma]);
                            numSemana++;
                        }
                        datosGrafica.reverse();
                        CargarGraficaLineasAnalisis(datosGrafica);
                        $("#modalGrafica").modal("show");
                    });    
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        auxiliarSemanal = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="1" class="totalizador">$ ' + parseFloat(totalizador[0].semanal / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(4).footer()).html('TOTAL');
                        $(api.column(5).footer()).html(auxiliarSemanal);
                        $(api.column(6).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            var semanal = $(this).attr("data-semanal") == "1";
                            var exito = cargarTablaSubSubCuenta(totalizador[0].detalles, totalizador[0].descripcion, 0, fecha, tipo, semanal);
                            if(exito){
                                botonTablaSubSubCuenta.show();
                                botonTablaSubCuenta.prop("disabled", false);
                            }
                            empresaGlobal = 0;
                        });

                    }
                }
            });
        }

        function initTablaSubSubCuenta() {
            if (dtTablaSubSubCuenta != null) {
                dtTablaSubSubCuenta.destroy();
            }
            dtTablaSubSubCuenta = tablaSubSubCuenta.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                columns: [
                    //{ data: 'fecha', title: 'Fecha Póliza' },
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'scta', title: 'SubCuentaID', visible: false },
                    { data: 'sscta', title: 'SubSubCuentaID', visible: false },
                    { data: 'id', title: 'Cuenta' },
                    { data: 'descripcion', title: 'Descripción' },
                    { data: 'semanal', title: botonBuscar.attr("data-tipocorte") == "0" ? 'Semana' : 'Mes', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 1, 0); } },
                    { data: 'importe', title: 'Acumulado', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0, 0); }}
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
                        var semanal = $(this).attr("data-semanal") == "1";
                        //if(rowData.cta < 5000) {
                        var exito = cargarTablaDivision(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta, semanal);
                        if(exito){
                            botonTablaDivision.show();
                            botonTablaSubSubCuenta.prop("disabled", false);
                        }
                        subsubcuentaFiltro = rowData.id;
                        //}
                        //else {
                        //    cargarTablaDetalle(rowData.detalles, rowData.descripcion, $(this).html(), false);
                        //    botonTablaDetalle.show();
                        //    botonTablaSubSubCuenta.prop("disabled", false);
                        //}
                    });
                    tablaSubSubCuenta.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
                        const rowData = dtTablaSubSubCuenta.row($(this).parents('tr')).data();
                        var datosGrafica = [];
                        var suma = 0;
                        for(var i = 0; i < 5; i++) {
                            suma = 0;
                            if(numSemana < 5)
                            {
                                let auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) {
                                    return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false ));
                                });
                                auxGrafica.forEach(function(n) { suma += n.importe; });                                
                            }
                            datosGrafica.push([(4 - i), suma]);
                            numSemana++;
                        }
                        datosGrafica.reverse();
                        CargarGraficaLineasAnalisis(datosGrafica);
                        $("#modalGrafica").modal("show");
                    });   
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        auxiliarSemanal = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="1" class="totalizador">$ ' + parseFloat(totalizador[0].semanal / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(5).footer()).html('TOTAL');
                        $(api.column(6).footer()).html(auxiliarSemanal);
                        $(api.column(7).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            var semanal = $(this).attr("data-semanal") == "1";
                            //if(totalizador[0].cta < 5000) {
                            var exito = cargarTablaDivision(totalizador[0].detalles, totalizador[0].descripcion, 0, totalizador[0].cta, semanal);
                            if(exito){
                                botonTablaDivision.show();
                                botonTablaSubSubCuenta.prop("disabled", false);
                            }
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
            if (dtTablaDivision != null) {
                dtTablaDivision.destroy();
            }
            dtTablaDivision = tablaDivision.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'División' },
                    { data: 'semanal', title: botonBuscar.attr("data-tipocorte") == "0" ? 'Semana' : 'Mes', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 1, 0); } },
                    { data: 'importe', title: 'Acumulado', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0, 0); } }
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
                        var semanal = $(this).attr("data-semanal") == "1";
                        var numAreasCuenta = botonBuscar.attr("data-obra").split(',').length;
                        if(numAreasCuenta == 1 && botonBuscar.attr("data-obra") != "")
                        {
                            if(rowData.cta < 5000 && empresaActual == 2)
                            {
                                var exito = cargarTablaConciliacion(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta, semanal);
                                if(exito){
                                    botonTablaConciliacion.show();
                                    botonTablaDivision.prop("disabled", false);
                                }
                            }
                            else
                            {
                                if(empresaActual == 2)
                                {
                                    var exito = cargarTablaEconomico(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta, semanal);
                                    if(exito){
                                        botonTablaEconomico.show();
                                        botonTablaDivision.prop("disabled", false);
                                    }
                                }
                                else
                                {
                                    e.preventDefault();
                                    e.stopPropagation();
                                    e.stopImmediatePropagation();
                                    const rowData = dtTablaAreaCuenta.row($(this).parents('tr')).data();
                                    var semanal = $(this).attr("data-semanal") == "1";
                                    var division = rowData.descripcion == "TOTAL" ? "" : rowData.descripcion;
                                    //function setLstKubrixTablaDet(tipo, columna, renglon, fecha, nombreRow, negativo, divisionCol, areaCuentaCol, economicoCol, semanal)
                                    var exito = setLstKubrixTablaDet(3, columnaGlobal, renglonGlobal, rowData.fecha, "", false , division, "", "", semanal)
                                    //var exito = cargarTablaDetalle(rowData.detalles, rowData.descripcion, $(this).html(), semanal);
                                }
                            }
                        }
                        else
                        {
                            var exito = cargarTablaAreaCuenta(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta, semanal);
                            if(exito){
                                botonTablaAreaCuenta.show();
                                botonTablaDivision.prop("disabled", false);
                            }
                        }
                        divisionFiltro = rowData.descripcion;
                    });
                    tablaDivision.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
                        const rowData = dtTablaDivision.row($(this).parents('tr')).data();
                        var datosGrafica = [];
                        var suma = 0;
                        for(var i = 0; i < 5; i++) {
                            suma = 0;
                            if(numSemana < 5)
                            {
                                let auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) {
                                    return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false ));
                                });
                                auxGrafica.forEach(function(n) { suma += n.importe; });                                
                            }
                            datosGrafica.push([(4 - i), suma]);
                            numSemana++;
                        }
                        datosGrafica.reverse();
                        CargarGraficaLineasAnalisis(datosGrafica);
                        $("#modalGrafica").modal("show");
                    });   
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        auxiliarSemanal = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="1" class="totalizador">$ ' + parseFloat(totalizador[0].semanal / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(3).footer()).html('TOTAL');
                        $(api.column(4).footer()).html(auxiliarSemanal);
                        $(api.column(5).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var numAreasCuenta = botonBuscar.attr("data-obra").split(',').length;
                            var semanal = $(this).attr("data-semanal") == "1";
                            if(numAreasCuenta == 1 && botonBuscar.attr("data-obra") != "")
                            {
                                if(totalizador[0].cta < 5000 && empresaActual == 2)
                                {
                                    var exito = cargarTablaConciliacion(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta, semanal);
                                    if(exito){
                                        botonTablaConciliacion.show();
                                        botonTablaDivision.prop("disabled", false);
                                    }
                                }
                                else
                                {
                                    if(empresaActual == 2){
                                        var exito = cargarTablaEconomico(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta, semanal);
                                        if(exito){
                                            botonTablaEconomico.show();
                                            botonTablaDivision.prop("disabled", false);
                                        }
                                    }
                                    else
                                    {
                                        e.preventDefault();
                                        e.stopPropagation();
                                        e.stopImmediatePropagation();
                                        var fecha = $(this).attr("data-fecha");
                                        var tipo = $(this).attr("data-tipo");
                                        var semanal = $(this).attr("data-semanal") == "1";
                                        //economicoFiltro = totalizador[0].descripcion == "TOTAL" ? economicoFiltro = economicoFiltro : economicoFIltro = totalizador[0].descripcion;
                                        //var exito = cargarTablaDetalle(totalizador[0].detalles, totalizador[0].descripcion, 0, semanal);
                                        var exito = setLstKubrixTablaDet(3, columnaGlobal, renglonGlobal, totalizador[0].fecha, "", false , "", "", "", semanal)
                                    }
                                }
                            }
                            else
                            {
                                var exito = cargarTablaAreaCuenta(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta, semanal);
                                if(exito){
                                    botonTablaAreaCuenta.show();
                                    botonTablaDivision.prop("disabled", false);
                                }
                            }
                        });

                    }
                }
            });
        }

        function initTablaAreaCuenta() {
            if (dtTablaAreaCuenta != null) {
                dtTablaAreaCuenta.destroy();
            }
            dtTablaAreaCuenta = tablaAreaCuenta.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'Area Cuenta' },
                    { data: 'semanal', title: botonBuscar.attr("data-tipocorte") == "0" ? 'Semana' : 'Mes', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 1, 0); } },
                    { data: 'importe', title: 'Acumulado', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0, 0); } }
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
                        var semanal = $(this).attr("data-semanal") == "1";
                        //cargarTablaConciliacion(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta);
                        //botonTablaConciliacion.show();
                        //botonTablaAreaCuenta.prop("disabled", false);
                        if(rowData.cta < 5000 && empresaActual == 2)
                        {
                            var exito = cargarTablaConciliacion(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta, semanal);
                            if(exito){
                                botonTablaConciliacion.show();
                                botonTablaAreaCuenta.prop("disabled", false);
                            }
                        }
                        else
                        {
                            if(empresaActual == 2){
                                var exito = cargarTablaEconomico(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta, semanal);
                                if(exito){
                                    botonTablaEconomico.show();
                                    botonTablaAreaCuenta.prop("disabled", false);
                                }
                            }
                            else
                            {
                                e.preventDefault();
                                e.stopPropagation();
                                e.stopImmediatePropagation();
                                const rowData = dtTablaAreaCuenta.row($(this).parents('tr')).data();
                                var semanal = $(this).attr("data-semanal") == "1";
                                var areaCuenta = rowData.descripcion == "TOTAL" ? "" : rowData.descripcion;
                                //function setLstKubrixTablaDet(tipo, columna, renglon, fecha, nombreRow, negativo, divisionCol, areaCuentaCol, economicoCol, semanal)
                                var exito = setLstKubrixTablaDet(3, columnaGlobal, renglonGlobal, rowData.fecha, "", false , divisionFiltroDetalle, areaCuenta, "", semanal)
                                //var exito = cargarTablaDetalle(rowData.detalles, rowData.descripcion, $(this).html(), semanal);
                            }
                        }
                        areaCuentaFiltro = rowData.descripcion;
                    });
                    tablaAreaCuenta.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
                        const rowData = dtTablaAreaCuenta.row($(this).parents('tr')).data();
                        var datosGrafica = [];
                        var suma = 0;
                        for(var i = 0; i < 5; i++) {
                            suma = 0;
                            if(numSemana < 5)
                            {
                                let auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) {
                                    return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false ));
                                });
                                auxGrafica.forEach(function(n) { suma += n.importe; });                                
                            }
                            datosGrafica.push([(4 - i), suma]);
                            numSemana++;
                        }
                        datosGrafica.reverse();
                        CargarGraficaLineasAnalisis(datosGrafica);
                        $("#modalGrafica").modal("show");
                    });   
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        auxiliarSemanal = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="1" class="totalizador">$ ' + parseFloat(totalizador[0].semanal / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(3).footer()).html('TOTAL');
                        $(api.column(4).footer()).html(auxiliarSemanal);
                        $(api.column(5).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var semanal = $(this).attr("data-semanal") == "1";
                            if(totalizador[0].cta < 5000 && empresaActual == 2)
                            {
                                var exito = cargarTablaConciliacion(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta, semanal);
                                if(exito){
                                    botonTablaConciliacion.show();
                                    botonTablaAreaCuenta.prop("disabled", false);
                                }
                            }
                            else
                            {
                                if(empresaActual == 2){
                                    var exito = cargarTablaEconomico(totalizador[0].detalles, totalizador[0].descripcion, $(this).html(), totalizador[0].cta, semanal);
                                    if(exito){
                                        botonTablaEconomico.show();
                                        botonTablaAreaCuenta.prop("disabled", false);
                                    }
                                }
                                else
                                {
                                    e.preventDefault();
                                    e.stopPropagation();
                                    e.stopImmediatePropagation();
                                    var fecha = $(this).attr("data-fecha");
                                    var tipo = $(this).attr("data-tipo");
                                    var semanal = $(this).attr("data-semanal") == "1";
                                    //economicoFiltro = totalizador[0].descripcion == "TOTAL" ? economicoFiltro = economicoFiltro : economicoFIltro = totalizador[0].descripcion;
                                    //var exito = cargarTablaDetalle(totalizador[0].detalles, totalizador[0].descripcion, 0, semanal);
                                    var exito = setLstKubrixTablaDet(3, columnaGlobal, renglonGlobal, totalizador[0].fecha, "", false , divisionFiltroDetalle, "", "", semanal)
                                }
                            }
                        });
                    }
                }
            });
        }

        function initTablaConciliacion() {
            if (dtTablaConciliacion != null) {
                dtTablaConciliacion.destroy();
            }
            dtTablaConciliacion = tablaConciliacion.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'Conciliación' },
                    { data: 'semanal', title: botonBuscar.attr("data-tipocorte") == "0" ? 'Semana' : 'Mes', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 1, 0); } },
                    { data: 'importe', title: 'Acumulado', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0, 0); } }
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
                        var semanal = $(this).attr("data-semanal") == "1";
                        var exito = cargarTablaEconomico(rowData.detalles, rowData.descripcion, $(this).html(), rowData.cta, semanal);
                        if(exito){
                            botonTablaEconomico.show();
                            botonTablaConciliacion.prop("disabled", false);
                        }                        
                        conciliacionFiltro = rowData.descripcion;                        
                    });
                    tablaConciliacion.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
                        const rowData = dtTablaConciliacion.row($(this).parents('tr')).data();
                        var datosGrafica = [];
                        var suma = 0;
                        for(var i = 0; i < 5; i++) {
                            suma = 0;
                            if(numSemana < 5)
                            {
                                let auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) {
                                    return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false ));
                                });
                                auxGrafica.forEach(function(n) { suma += n.importe; });
                            }
                            datosGrafica.push([(4 - i), suma]);
                            numSemana++;
                        }
                        datosGrafica.reverse();
                        CargarGraficaLineasAnalisis(datosGrafica);
                        $("#modalGrafica").modal("show");
                    });   
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        auxiliarSemanal = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="1" data-semanal="1" class="totalizador">$ ' + parseFloat(totalizador[0].semanal / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(3).footer()).html('TOTAL');
                        $(api.column(4).footer()).html(auxiliarSemanal);
                        $(api.column(5).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            var semanal = $(this).attr("data-semanal") == "1";
                            var exito = cargarTablaEconomico(totalizador[0].detalles, totalizador[0].descripcion, 0, totalizador[0].cta, semanal);
                            if(exito){
                                botonTablaEconomico.show();
                                botonTablaConciliacion.prop("disabled", false);
                            }
                        });
                    }
                }
            });
        }

        function initTablaEconomico() {
            if (dtTablaEconomico != null) {
                dtTablaEconomico.destroy();
            }
            dtTablaEconomico = tablaEconomico.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: true,
                searching: true,
                autoWidth: true,
                columns: [
                    { data: 'grafica', title: '<i class="fas fa-chart-line"></i>' },
                    { data: 'cta', title: 'CuentaID', visible: false },
                    { data: 'id', title: 'ID', visible: false },
                    { data: 'descripcion', title: 'Económico' },
                    { data: 'semanal', title: botonBuscar.attr("data-tipocorte") == "0" ? 'Semana' : 'Mes', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 1, 0); } },
                    { data: 'importe', title: 'Acumulado', render: function(data, type, row) { return getRowHTMLFecha(data, row.fecha, row.tipo, 0, 0); } }
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
                        var semanal = $(this).attr("data-semanal") == "1";
                        var economico = rowData.descripcion == "TOTAL" ? "" : rowData.descripcion;
                        //function setLstKubrixTablaDet(tipo, columna, renglon, fecha, nombreRow, negativo, divisionCol, areaCuentaCol, economicoCol, semanal)
                        var exito = setLstKubrixTablaDet(3, columnaGlobal, renglonGlobal, rowData.fecha, "", false , divisionFiltroDetalle, areaCuentaFiltroDetalle, economico, semanal)
                        //var exito = cargarTablaDetalle(rowData.detalles, rowData.descripcion, $(this).html(), semanal);
                      
                    });
                    tablaEconomico.find('button.grafica').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
                        const rowData = dtTablaEconomico.row($(this).parents('tr')).data();
                        var datosGrafica = [];
                        var suma = 0;
                        for(var i = 0; i < 5; i++) {
                            suma = 0;
                            if(numSemana < 5)
                            {
                                let auxGrafica = jQuery.grep(rowData.detalles, function( n, i ) {
                                    return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false ));
                                });
                                auxGrafica.forEach(function(n) { suma += n.importe; });
                            }
                            datosGrafica.push([(4 - i), suma]);
                            numSemana++;
                        }
                        datosGrafica.reverse();
                        CargarGraficaLineasAnalisis(datosGrafica);
                        $("#modalGrafica").modal("show");
                    });   
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var totalizador = jQuery.grep(data, function( n, i ) {
                        return n.id === "-1";
                    });
                    if(totalizador.length > 0){
                        var api = this.api(), data;
                        auxiliar = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="0" class="totalizador">$ ' + parseFloat(totalizador[0].importe / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        auxiliarSemanal = '<p' + ' data-fecha="' + totalizador[0].fecha + '" data-tipo="0" data-semanal="1" class="totalizador">$ ' + parseFloat(totalizador[0].semanal / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '</p>';
                        $(api.column(3).footer()).html('TOTAL');
                        $(api.column(4).footer()).html(auxiliarSemanal);
                        $(api.column(5).footer()).html(auxiliar);
                        $(this).find('p.totalizador').click(function (e) {
                            e.preventDefault();
                            e.stopPropagation();
                            e.stopImmediatePropagation();
                            var fecha = $(this).attr("data-fecha");
                            var tipo = $(this).attr("data-tipo");
                            var semanal = $(this).attr("data-semanal") == "1";
                            //economicoFiltro = totalizador[0].descripcion == "TOTAL" ? economicoFiltro = economicoFiltro : economicoFIltro = totalizador[0].descripcion;
                            //var exito = cargarTablaDetalle(totalizador[0].detalles, totalizador[0].descripcion, 0, semanal);
                            var exito = setLstKubrixTablaDet(3, columnaGlobal, renglonGlobal, totalizador[0].fecha, "", false , divisionFiltroDetalle, areaCuentaFiltroDetalle, "", semanal)

                        });
                    }
                }
            });
        }

        function initTablaDetalles() {
            if (dtTablaDetalles != null) {
                dtTablaDetalles.destroy();
            }
            dtTablaDetalles = tablaDetalles.DataTable({
                language: dtDicEsp,
                destroy: true,                
                scrollY: "500px",
                scrollCollapse: true,
                paging: true,
                pageLength: 100,
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
        }

        function stripHtml(html)
        {
            var tmp = document.createElement("DIV");
            tmp.innerHTML = html;
            return tmp.textContent || tmp.innerText || "";
        }

        //--> Carga de tablas detalle principal
        function cargarTablaSubCuenta(detalles, nombreRow, fecha, numSemana, negativo) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return false;
            }
            const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
            const grouped = groupBy(detalles, function(detalle) { 
                var cuentaSplit = detalle.cuenta.split('-');
                var subcuenta = cuentaSplit[0] + "-" + cuentaSplit[1];
                var subcuentaDesc = cuentasDescr.find(x => x.Value === (subcuenta + "-" + "0"));
                if(subcuentaDesc == null)
                {
                    var a = 0;
                }
                return subcuentaDesc.Text + ((subcuenta ==  "5000-10" || subcuenta == "5900-3" || subcuenta == '5280-10') ? (detalle.empresa == 2 ? " ARRENDADORA" : " CONSTRUPLAN") : "")
            });

            dtTablaSubCuenta.clear();
            Array.from(grouped, function([key, value]) {
                var auxCuenta = value[0].cuenta.split('-');                
                const cta = parseInt(auxCuenta[0]);
                const scta = parseInt(auxCuenta[1]);
                const id = auxCuenta[0] + "-" + auxCuenta[1];
                const descripcion = key;
                const empresa = value[0].empresa;
                const importeDetalles = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeDetallesSemanal = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                const importeDetallesCancelados = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
                importeDetallesSemanal = $.merge(importeDetallesSemanal, importeDetallesCancelados);

                if(importeDetalles.length > 0){
                    var semanal = 0;
                    importeDetallesSemanal.length > 0 ? $.each(importeDetallesSemanal, function(i, n) { semanal += n * (negativo ? -1 : 1); }) : 0;
                    var importe = 0;
                    $.each(importeDetalles, function(i, n) { importe += n * (negativo ? -1 : 1); });
                    const grupo = { grafica: grafica, cta: cta, scta: scta, id: id, descripcion: descripcion, importe: importe, semanal: semanal, detalles: value, fecha: fecha, empresa: empresa };
                    dtTablaSubCuenta.row.add(grupo);
                }
            });
            const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
            var importeTotalizador = 0;
            $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
            var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
            var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
            $.merge(importeTotalizadorDetallesSemanal,auxImporteTotalDetSemanal);
            var importeTotalizadorSemanal = 0;
            $.each(importeTotalizadorDetallesSemanal, function(i, n) { importeTotalizadorSemanal += n * (negativo ? -1 : 1); });    
            const totalizador = { grafica: grafica, cta: detalles[0].cuenta.split('-')[0], scta: 0, id: "-1", descripcion: "TOTAL", semanal: importeTotalizadorSemanal, importe: importeTotalizador, detalles: detalles, empresa: detalles[0].empresa }
            dtTablaSubCuenta.row.add(totalizador);
            dtTablaSubCuenta.draw();
            $("#botonTablaSubCuenta strong").text(stripHtml(nombreRow).toUpperCase());
            botonTablaSubCuenta.attr("data-numSemana", numSemana);
            botonTablaSubCuenta.attr("data-negativo", negativo);
            botonTablaSubCuenta.attr("data-fecha", fecha);
            dtTablaSubCuenta.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
            modalDetallesK.modal('show');
            return true;
        }

        function cargarTablaSubSubCuenta(detalles, nombreColumna, total, fecha, tipo, semanal) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return false;
            }
            const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
            
            const numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
            const negativo = botonTablaSubCuenta.attr("data-negativo") == "true";

            if(detalles.length > 0){
                const grouped = groupBy(detalles, function(detalle) { return detalle.cuenta; });
                let fechaSemanal = new Date(botonTablaSubCuenta.attr("data-fecha"));
                if(botonBuscar.attr("data-tipoCorte") == "0") { fechaSemanal.setDate(fechaSemanal.getDate() - 7); }
                else{
                    fechaSemanal = new Date (fechaSemanal.getFullYear(), fechaSemanal.getMonth(), 1);
                    fechaSemanal.setDate(fechaSemanal.getDate() - 1);
                }
                dtTablaSubSubCuenta.clear();
                Array.from(grouped, function ([key, value]) {
                    var auxCuenta = value[0].cuenta.split('-');
                    const cta = parseInt(auxCuenta[0]);
                    const scta = parseInt(auxCuenta[1]);
                    const sscta = parseInt(auxCuenta[2]);
                    const id = value[0].cuenta;
                    const descripcion = cuentasDescr.find(x => x.Value === value[0].cuenta).Text;
                    const importeDetalles = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
                    importeDetallesSemanal = $.merge(importeDetallesSemanal, importeDetallesCancelados);

                    if(importeDetalles.length > 0){
                        var semanal = 0;
                        importeDetallesSemanal.length > 0 ? $.each(importeDetallesSemanal, function(i, n) { semanal += n * (negativo ? -1 : 1); }) : 0;
                        var importe = 0;
                        $.each(importeDetalles, function(i, n) { importe += n * (negativo ? -1 : 1); });                  
                        const grupo = { grafica: grafica, cta: cta, scta: scta, sscta: sscta, id: id, descripcion: descripcion, importe: importe, semanal: semanal, detalles: value, fecha: fecha, tipo: tipo };
                        dtTablaSubSubCuenta.row.add(grupo);
                    }
                });
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return ((n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
                $.merge(importeTotalizadorDetallesSemanal,auxImporteTotalDetSemanal);
                var importeTotalizadorSemanal = 0;
                $.each(importeTotalizadorDetallesSemanal, function(i, n) { importeTotalizadorSemanal += n * (negativo ? -1 : 1); });    
                const totalizador = { grafica: grafica, cta: detalles[0].cuenta.split('-')[0], scta: 0, sscta: 0, id: "-1", descripcion: "TOTAL", semanal: importeTotalizadorSemanal, importe: importeTotalizador, detalles: detalles }
                dtTablaSubSubCuenta.row.add(totalizador);
                $("#botonTablaSubSubCuenta strong").text(nombreColumna.toUpperCase());

                dtTablaSubSubCuenta.draw();
                dtTablaSubSubCuenta.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
                HideTablas();
                divTablaSubSubCuenta.show(500);
                return true;
            }
            else
            {
                AlertaGeneral("Aviso", "No se encontraron detalles con esas especificaciones.");
                return false;
            }
        }

        function cargarTablaDivision(detalles, nombreColumna, total, cuenta, semanal) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return false;
            }   
            const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
            const numSemana =  parseInt(botonTablaSubCuenta.attr("data-numSemana"));
            const negativo = botonTablaSubCuenta.attr("data-negativo") == "true";

            const grouped = groupBy(detalles, function(detalle) { return detalle.division; });
            if(detalles.length > 0){
                let fechaSemanal = new Date(botonTablaSubCuenta.attr("data-fecha"));
                if(botonBuscar.attr("data-tipoCorte") == "0") { fechaSemanal.setDate(fechaSemanal.getDate() - 7); }
                else{
                    fechaSemanal = new Date (fechaSemanal.getFullYear(), fechaSemanal.getMonth(), 1);
                    fechaSemanal.setDate(fechaSemanal.getDate() - 1);
                }
                dtTablaDivision.clear();
                Array.from(grouped, function ([key, value]) {
                    var auxCuenta = value[0].cuenta.split('-');
                    const cta = parseInt(auxCuenta[0]);
                    const id = value[0].division;
                    const descripcion = value[0].division;
                    const importeDetalles = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
                    importeDetallesSemanal = $.merge(importeDetallesSemanal, importeDetallesCancelados);

                    if(importeDetalles.length > 0){
                        var semanal = 0;
                        importeDetallesSemanal.length > 0 ? $.each(importeDetallesSemanal, function(i, n) { semanal += n * (negativo ? -1 : 1); }) : 0;
                        var importe = 0;
                        $.each(importeDetalles, function(i, n) { importe += n * (negativo ? -1 : 1); });             
                        const grupo = { grafica: grafica, cta: cta, id: id, descripcion: descripcion, importe: importe, semanal: semanal, detalles: value };
                        dtTablaDivision.row.add(grupo);
                    }
                });
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return ((n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
                $.merge(importeTotalizadorDetallesSemanal,auxImporteTotalDetSemanal);
                var importeTotalizadorSemanal = 0;
                $.each(importeTotalizadorDetallesSemanal, function(i, n) { importeTotalizadorSemanal += n * (negativo ? -1 : 1); });   
                const totalizador = { grafica: grafica, cta: detalles[0].cuenta.split('-')[0], id: "-1", descripcion: "TOTAL", semanal: importeTotalizadorSemanal, importe: importeTotalizador, detalles: detalles }
                dtTablaDivision.row.add(totalizador);
                $("#botonTablaDivision strong").text(nombreColumna.toUpperCase());

                dtTablaDivision.draw();
                dtTablaDivision.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
                HideTablas();
                divTablaDivision.show(500);
                return true;
            }
            else
            {
                AlertaGeneral("Aviso", "No se encontraron detalles con esas especificaciones.");
                return false;
            }
        }

        function cargarTablaAreaCuenta(detalles, nombreColumna, total, cuenta, semanal) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return false;
            }
            const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
            const numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
            const negativo = botonTablaSubCuenta.attr("data-negativo") == "true";
            
            const grouped = groupBy(detalles, function(detalle) { return detalle.areaCuenta; });
            if(detalles.length > 0){
                let fechaSemanal = new Date(botonTablaSubCuenta.attr("data-fecha"));
                if(botonBuscar.attr("data-tipoCorte") == "0") { fechaSemanal.setDate(fechaSemanal.getDate() - 7); }
                else{
                    fechaSemanal = new Date (fechaSemanal.getFullYear(), fechaSemanal.getMonth(), 1);
                    fechaSemanal.setDate(fechaSemanal.getDate() - 1);
                }
                dtTablaAreaCuenta.clear();
                Array.from(grouped, function ([key, value]) {
                    var auxCuenta = value[0].cuenta.split('-');
                    const cta = parseInt(auxCuenta[0]);
                    const id = value[0].areaCuenta;
                    const descripcion = value[0].areaCuenta;
                    const importeDetalles = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
                    importeDetallesSemanal = $.merge(importeDetallesSemanal, importeDetallesCancelados);

                    if(importeDetalles.length > 0){
                        var semanal = 0;
                        importeDetallesSemanal.length > 0 ? $.each(importeDetallesSemanal, function(i, n) { semanal += n * (negativo ? -1 : 1); }) : 0;
                        var importe = 0;
                        $.each(importeDetalles, function(i, n) { importe += n * (negativo ? -1 : 1); });                 
                        const grupo = { grafica: grafica, cta: cta,  id: id, descripcion: descripcion, importe: importe, semanal: semanal, detalles: value };
                        dtTablaAreaCuenta.row.add(grupo);
                    }
                });
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return ((n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
                $.merge(importeTotalizadorDetallesSemanal,auxImporteTotalDetSemanal);
                var importeTotalizadorSemanal = 0;
                $.each(importeTotalizadorDetallesSemanal, function(i, n) { importeTotalizadorSemanal += n * (negativo ? -1 : 1); });   
                const totalizador = { grafica: grafica, cta: detalles[0].cuenta.split('-')[0], id: "-1", descripcion: "TOTAL", semanal: importeTotalizadorSemanal, importe: importeTotalizador, detalles: detalles }
                dtTablaAreaCuenta.row.add(totalizador);
                $("#botonTablaAreaCuenta strong").text(nombreColumna.toUpperCase());

                dtTablaAreaCuenta.draw();
                dtTablaAreaCuenta.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
                HideTablas();
                divTablaAreaCuenta.show(500);
                return true;
            }
            else
            {
                AlertaGeneral("Aviso", "No se encontraron detalles con esas especificaciones.");
                return false;
            }
            //}

        }

        function cargarTablaConciliacion(detalles, nombreColumna, total, cuenta, semanal) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return false;
            }   
            const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
            const numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
            const negativo = botonTablaSubCuenta.attr("data-negativo") == "true";
            
            const grouped = groupBy(detalles, function(detalle) { return detalle.referencia; });
            if(detalles.length > 0){
                let fechaSemanal = new Date(botonTablaSubCuenta.attr("data-fecha"));
                if(botonBuscar.attr("data-tipoCorte") == "0") { fechaSemanal.setDate(fechaSemanal.getDate() - 7); }
                else{
                    fechaSemanal = new Date (fechaSemanal.getFullYear(), fechaSemanal.getMonth(), 1);
                    fechaSemanal.setDate(fechaSemanal.getDate() - 1);
                }
                dtTablaConciliacion.clear();
                Array.from(grouped, function ([key, value]) {
                    var auxCuenta = value[0].cuenta.split('-');
                    const cta = parseInt(auxCuenta[0]);
                    const id = value[0].referencia;
                    const descripcion = value[0].referencia;
                    const importeDetalles = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
                    importeDetallesSemanal = $.merge(importeDetallesSemanal, importeDetallesCancelados);

                    if(importeDetalles.length > 0){
                        var semanal = 0;
                        importeDetallesSemanal.length > 0 ? $.each(importeDetallesSemanal, function(i, n) { semanal += n * (negativo ? -1 : 1); }) : 0;
                        var importe = 0;
                        $.each(importeDetalles, function(i, n) { importe += n * (negativo ? -1 : 1); });                  
                        const grupo = { grafica: grafica, cta: cta, id: id, descripcion: descripcion, importe: importe, semanal: semanal, detalles: value };
                        dtTablaConciliacion.row.add(grupo);
                    }
                });
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return ((n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
                $.merge(importeTotalizadorDetallesSemanal,auxImporteTotalDetSemanal);
                var importeTotalizadorSemanal = 0;
                $.each(importeTotalizadorDetallesSemanal, function(i, n) { importeTotalizadorSemanal += n * (negativo ? -1 : 1); });   
                const totalizador = { grafica: grafica, cta: detalles[0].cuenta.split('-')[0], id: "-1", descripcion: "TOTAL", semanal: importeTotalizadorSemanal, importe: importeTotalizador, detalles: detalles }
                dtTablaConciliacion.row.add(totalizador);
                $("#botonTablaConciliacion strong").text(nombreColumna.toUpperCase());

                dtTablaConciliacion.draw();
                dtTablaConciliacion.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
                HideTablas();
                divTablaConciliacion.show(500);
                return true;
            }
            else
            {
                AlertaGeneral("Aviso", "No se encontraron detalles con esas especificaciones.");
                return false;
            }
        }

        function cargarTablaEconomico(detalles, nombreColumna, total, cuenta, semanal) {
            if (detalles == null || detalles.length == 0) {
                AlertaGeneral("Aviso", "Ocurrió un error al ver los detalles de este registros.");
                return false;
            }   
            const grafica = '<button class="btn btn-sm btn-primary grafica"><i class="fas fa-chart-line"></i></button>';
            const numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
            const negativo = botonTablaSubCuenta.attr("data-negativo") == "true";
            
            const grouped = groupBy(detalles, function(detalle) { return detalle.cc; });
            if(detalles.length > 0){
                let fechaSemanal = new Date(botonTablaSubCuenta.attr("data-fecha"));
                if(botonBuscar.attr("data-tipoCorte") == "0") { fechaSemanal.setDate(fechaSemanal.getDate() - 7); }
                else{
                    fechaSemanal = new Date (fechaSemanal.getFullYear(), fechaSemanal.getMonth(), 1);
                    fechaSemanal.setDate(fechaSemanal.getDate() - 1);
                }
                dtTablaEconomico.clear();
                Array.from(grouped, function ([key, value]) {
                    var auxCuenta = value[0].cuenta.split('-');
                    const cta = parseInt(auxCuenta[0]);
                    const id = value[0].cc;
                    const descripcion = value[0].cc;
                    const importeDetalles = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
                    importeDetallesSemanal = $.merge(importeDetallesSemanal, importeDetallesCancelados);

                    if(importeDetalles.length > 0){
                        var semanal = 0;
                        importeDetallesSemanal.length > 0 ? $.each(importeDetallesSemanal, function(i, n) { semanal += n * (negativo ? -1 : 1); }) : 0;
                        var importe = 0;
                        $.each(importeDetalles, function(i, n) { importe += n * (negativo ? -1 : 1); });                
                        const grupo = { grafica: grafica, cta: cta, id: id, descripcion: descripcion, importe: importe, semanal: semanal, detalles: value };
                        dtTablaEconomico.row.add(grupo);
                    }
                });
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return ((n.semana == numSemana || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) || (cbConfiguracion.val() == '0' ? n.semana == 6 : false )) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
                $.merge(importeTotalizadorDetallesSemanal,auxImporteTotalDetSemanal);
                var importeTotalizadorSemanal = 0;
                $.each(importeTotalizadorDetallesSemanal, function(i, n) { importeTotalizadorSemanal += n * (negativo ? -1 : 1); });   
                const totalizador = { grafica: grafica, cta: detalles[0].cuenta.split('-')[0], id: "-1", descripcion: "TOTAL", semanal: importeTotalizadorSemanal, importe: importeTotalizador, detalles: detalles }
                dtTablaEconomico.row.add(totalizador);
                $("#botonTablaEconomico strong").text(nombreColumna.toUpperCase());

                dtTablaEconomico.draw();
                dtTablaEconomico.rows(function (idx, data, node) { return data.id === "-1"; }).remove().draw();
                HideTablas();
                divTablaEconomico.show(500);
                botonTablaEconomico.show();
                return true;
            }
            else
            {
                AlertaGeneral("Aviso", "No se encontraron detalles con esas especificaciones.");
                return false;
            }
        }

        function cargarTablaDetalle(subdetalles, descripcion, total, semanal) {
            if(subdetalles.length > 0){
               
                const numSemana = parseInt(botonTablaSubCuenta.attr("data-numSemana"));
                const negativo = botonTablaSubCuenta.attr("data-negativo") == "true";
                var numAreasCuenta = botonBuscar.attr("data-obra").split(',').length;
                //subdetalles = $.grep(subdetalles, function(n, i){ return n.semana == numSemana; });
                subdetalles = subdetalles.map(function(x) {
                    return {
                        poliza: x.poliza,
                        noEco: x.noEco,
                        fecha: moment(x.fecha).toDate().toLocaleDateString('en-GB').Capitalize(),
                        descripcion: x.insumo_Desc,
                        importe: (x.importe * (negativo ? -1 : 1))
                    };
                });
                if(descripcion == null){ descripcion = "";}
                if(descripcion == ""){ descripcion = "TOTAL";}
                $("#botonTablaDetalle strong").text(descripcion.toUpperCase());
                dtTablaDetalles.clear().rows.add(subdetalles).draw();
                HideTablas();
                divTablaDetalles.show(500);    

                divTablaDetalles.show(500, function(){
                    dtTablaDetalles.columns.adjust()
                });
                botonTablaDetalle.show();
                if(empresaActual == 2){
                    botonTablaEconomico.prop("disabled", false);
                }
                else
                {
                    if(numAreasCuenta == 1 && botonBuscar.attr("data-obra") != "") 
                    {
                        botonTablaDivision.prop("disabled", false);
                    }
                    else
                    {
                        botonTablaAreaCuenta.prop("disabled", false);
                    }
                }
                return true;
            }
            else{ 
                AlertaGeneral("Alerta", "No se encontraron registros con los filtros seleccionados.")
                return false; 
            }
        }
        function HideTablas()
        {
            if(divTablaSubCuenta.is(":visible")) divTablaSubCuenta.hide(500);
            if(divTablaSubSubCuenta.is(":visible")) divTablaSubSubCuenta.hide(500);
            if(divTablaDivision.is(":visible")) divTablaDivision.hide(500);
            if(divTablaAreaCuenta.is(":visible")) divTablaAreaCuenta.hide(500);
            if(divTablaConciliacion.is(":visible")) divTablaConciliacion.hide(500);
            if(divTablaEconomico.is(":visible")) divTablaEconomico.hide(500);
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
        
        function getBusquedaDTO() {
            return {
                obra: comboACAnalisis.val()
                , tipo: $("#botonNombreNivelCero").attr("data-filtro")
                , fecha: inputDiaFinalAnalisis.val()
            };
        }

        //function setLstAnalisis(lista, tipo) {
        //    $.post(getLstAnalisis, { lista: lista, fecha: inputDiaFinalAnalisis.val(), ejercicioActual: ejercicioActual, tipo: parseInt(tipo) })
        //        .then(function (response) {
        //            if (response.success) {
        //                // Operación exitosa.
        //                auxDatos = response.lst;
        //                if(tipo == "1") {
        //                    cargarDatosTablaAnalisis(response.lst, 1);
        //                }
        //                else {
        //                    cargarDatosTablaAnalisis(response.lst, 0);
        //                }
        //                fechaMaxAnalisis = moment(response.fecha).toDate();
        //            } else {
        //                // Operación no completada.
        //                AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
        //            }
        //        }, function(error) {
        //            // Error al lanzar la petición.
        //            AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.');
        //        }
        //    );
        //}

        //function setLstKubrixDetNivelUno(busq, nombreColumna, total) {
        //    $.post(getLstKubrixDetalle, { busq: busq })
        //        .then(function(response) {
        //            if (response.success) {
        //                // Operación exitosa.
        //                var detalles = response.lst;
        //                var sumadetalles = 0;
        //                for(var i = 0; i < detalles.length; i++)
        //                {
        //                    sumadetalles += detalles[i].importe;
        //                }
        //                cargarDetScta(detalles, nombreColumna, total);                        
        //            } else {
        //                // Operación no completada.
        //                AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
        //            }
        //        }, function(error) {
        //            // Error al lanzar la petición.
        //            AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".");
        //        }
        //    );
        //}
        //function setLstKubrixDetNivelUnoFiltrado(busq, nombreColumna, total) {
        //    $.post(getLstKubrixDetalle, { busq: busq })
        //        .then(function(response) {
        //            if (response.success) {
        //                // Operación exitosa.
        //                var detalles = response.lst;
        //                var sumadetalles = 0;
        //                for(var i = 0; i < detalles.length; i++)
        //                {
        //                    sumadetalles += detalles[i].importe;
        //                }
        //                cargarDetSctaFiltrado(detalles, nombreColumna, total);
        //            } else {
        //                // Operación no completada.
        //                AlertaGeneral("Operación fallida", "No se pudo completar la operación: " + response.message);
        //            }
        //        }, function(error) {
        //            // Error al lanzar la petición.
        //            AlertaGeneral("Operación fallida", "Ocurrió un error al lanzar la petición al servidor: Error " + error.status + " - " + error.statusText + ".");
        //        }
        //    );
        //}

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
                        var fechaMax = moment(botonBuscar.attr("data-fechaFin"), "DD-MM-YYYY").toDate();
                        const p = $(this);
                        const rowData = dtTablaAnalisis.row(p.parents('tr')).data();
                        const td = p.parents("td");
                        columnaAnalisis = td.index();
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
                    if( data.tipo_mov == 5 || data.tipo_mov == 8 || data.tipo_mov == 10 || data.tipo_mov == 12 || data.tipo_mov == 14 || data.tipo_mov == 15) { $(row).addClass('resultado'); }
                }
            });
            if(equipoMayor == 1)
            {
                $("div.chkGrafica") .html('<input type="checkbox" checked data-toggle="toggle" data-on="Detalles" data-off="Gráfica" data-onstyle="success" data-offstyle="info" id="chbGrafica">' +
                    '&nbsp;&nbsp;&nbsp;<input type="checkbox" ' + (tipo == 0 ? "checked" : "") + ' data-toggle="toggle" data-on="80-20" data-off="Periodo" data-onstyle="success" data-offstyle="info" id="chb8020">');
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
                            if (row.tipo_mov == 4 || row.tipo_mov == 8 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13)
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
                        cargarDetSctaFiltrado(rowData.detalles, rowData.descripcion, $(this).html(), fecha, tipo);
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
                        const fecha = $(this).attr("data-fecha");
                        const tipo = $(this).attr("data-tipo");
                        const semanal = $(this).attr("data-semanal");
                        var dateMin = new Date(dateMax);
                        dateMin.setDate(dateMin.getDate() - 7);
                        var fechaMaximaAnalisis = moment(fecha).toDate().toLocaleDateString('en-GB').Capitalize();
                        var fechaMinimaAnalisis = semanal == 1 ? moment(dateMin).toDate().toLocaleDateString('en-GB').Capitalize() : botonBuscar.attr("data-fechaInicio");
                        var tipoEquipoMayor = ($("#chb8020").is(":checked") ? 0 : ($("#chb8020").length ? parseInt(botonBuscar.attr("data-tipoMayor")) - 1 : 0));
                        var busq = {
                            obra: $("#comboACAnalisis").val()
                            , lstMaquina: (cboMaquina.val().trim() != "" ? [$("#cboMaquina option:selected").text().trim()] : null)
                            , min: semanal == 1 ? moment(dateMin).toDate().toLocaleDateString('en-GB').Capitalize() : new Date()
                            , max: moment(fecha).toDate().toLocaleDateString('en-GB').Capitalize()
                            , cta: rowData.cta
                            , scta: rowData.scta
                            , sscta: rowData.sscta
                            , tipo: botonBuscar.attr("data-tipo")
                            , tm: [botonBuscar.attr("data-tipoMayor"), ($("#chb8020").is(":checked") ? 0 : ($("#chb8020").length ? 1 : 0))]
                        }
                        //setLstAnalisisDetalle(rowData.id, fechaMinimaAnalisis, fechaMaximaAnalisis, botonBuscar.attr("data-tipo"), rowData.descripcion, $(this).html(), tipoEquipoMayor);
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
                    const porcentajeNum = (acumulado / totalDetalle) * 100
                    const grupo = { cta: cta, scta: scta, id: id, grafica: grafica, descripcion: descripcion, importe: importe, detalles: value, acumulado: acumulado, porcentaje: porcentajeNum, tipo: tipo, fecha: fecha };
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
                    const grupo = { cta: cta, scta: scta, id: id, grafica: grafica, descripcion: descripcion, importe: importe, detalles: detallesFiltrados, acumulado: acumulado, porcentaje: porcentajeNum, fecha: dateMax, tipo: tipo/*, detallesSemanal: detallesFiltradosSemanal*/ };
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
            dtTablaDetallesA.draw();
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
            $("#botonNombreNivelUno strong").text("PERIODO: " + nombreColumna.toUpperCase());
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
                dtTablaDetallesA.columns.adjust()
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
            comboAC.fillComboAsync('cboObraKubrix', {divisionID: comboDivision.val(), responsableID: comboResponsable.val() }, false);
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
                searching: false,
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
            if(!responsable){
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

        function obtenerListaKubrixEconomico(listaRentabilidad, busq)
        {
            var listaUtilidad = [];
            listaRentabilidad = $.grep(listaRentabilidad,function(el,index){ return el != null; });            
            listaRentabilidadCostos = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("5000-") >= 0 || el.cuenta.indexOf("5900-") >= 0 || el.cuenta.indexOf("5901-") >= 0
                    || el.cuenta.indexOf("5280-") >= 0 || el.cuenta == "1-4-0") && (el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2; });
            var economicos = $.grep(listaRentabilidad,function(el,index){ return (el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )); }).map(function(element) { return element.cc; });
            economicos = unique(economicos).sort();
            var maquinasEstatus = SetMaquinasEstatus(economicos);
            var gruposMaquina = SetGrupoMaquinas();
            for(var i = 0; i < economicos.length; i++)
            {
                var detalles = $.grep(listaRentabilidad, function(el,index){ return el.cc == economicos[i]; });
                var estatus = maquinasEstatus.find(function (el) { return el.Value == economicos[i]; });

                var stringGrupo = "OTROS";
                var stringEstatus = "-1";
                var stringCC = "-1";
                var grupoMaquina = gruposMaquina.find(function (el) { return el.Text == economicos[i]; });
                if(grupoMaquina != null) stringGrupo = grupoMaquina.Value;
                if(estatus != null) {
                    stringEstatus = estatus.Text;
                    stringCC = estatus.Prefijo;
                }
                var item = { 
                    descripcion: economicos[i],
                    costoTotal: 0, depreciacion: 0, costosEstimados: 0, gastosOperacion: 0, gastosProductosFinancieros: 0, 
                    otrosIngresos: 0, resultadoNeto: 0, detalles: detalles, grupoMaquina: stringGrupo, estatus: stringEstatus, centro_costos: stringCC
                };

                //var ingresos = $.grep(listaRentabilidadIngresos, function(el,index){ return el.cc == economicos[i]; });
                //if (ingresos.length > 0) { $.each(ingresos, function(index, element){ item.ingresos += (element.monto) || 0; }); }
                
                var costoTotal = $.grep(listaRentabilidadCostos, function(el,index){ return el.cc == economicos[i] && el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0; });
                if (costoTotal.length > 0) { $.each(costoTotal, function(index, element){ item.costoTotal += (element.monto * (-1)) || 0; }); }

                var depreciacion = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5000-10") >= 0 && el.cc == economicos[i]; });
                if (depreciacion.length > 0) { $.each(depreciacion, function(index, element){ item.depreciacion += (element.monto) || 0; }); }

                var costosEstimados = $.grep(listaRentabilidad,function(el,index){ return el.cuenta == "1-4-0" && el.cc == economicos[i]; });
                if (costosEstimados.length > 0) { $.each(costosEstimados, function(index, element){ item.costosEstimados += (element.monto) || 0; }); }
               
                var gastosOperacion = $.grep(listaRentabilidad,function(el,index){ el.cuenta.indexOf("5280-") >= 0 && el.cc == economicos[i]; });
                if (gastosOperacion.length > 0) { $.each(gastosOperacion, function(index, element){ item.gastosOperacion += (element.monto) || 0; }); }

                var gastosProductosFinancieros = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5900-") >= 0 && el.cc == economicos[i]; });
                if (gastosProductosFinancieros.length > 0) { $.each(gastosProductosFinancieros, function(index, element){ item.gastosProductosFinancieros += (element.monto) || 0; }); }

                var otrosIngresos = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5901-") >= 0 && el.cc == economicos[i]; });
                if (otrosIngresos.length > 0) { $.each(otrosIngresos, function(index, element){ item.otrosIngresos += (element.monto) || 0; }); }
                item.resultadoNeto = item.resultadoFinancieros + item.otrosIngresos;

                listaUtilidad.push(item);                
            }
            listaUtilidad = listaUtilidad.sort((a, b) => (a.descripcion < b.descripcion) ? 1 : -1);
            return listaUtilidad.sort((a, b) => (a.grupoMaquina > b.grupoMaquina) ? 1 : -1);
        }

        function obtenerListaKubrixEcoCompacto(listaRentabilidad, busq)
        {
            var listaUtilidad = [];
            listaRentabilidad = $.grep(listaRentabilidad,function(el,index){ return el != null; });            
            listaRentabilidadCostos = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("5000-") >= 0 || el.cuenta.indexOf("5900-") >= 0 || el.cuenta.indexOf("5901-") >= 0
                    || el.cuenta.indexOf("5280-") >= 0 || el.cuenta == "1-4-0") && (el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2; });
            var economicos = $.grep(listaRentabilidad,function(el,index){ return (el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )); }).map(function(element) { return element.cc; });
            economicos = unique(economicos).sort();
            var maquinasEstatus = SetMaquinasEstatus(economicos);
            var gruposMaquina = SetGrupoMaquinas();
            for(var i = 0; i < economicos.length; i++)
            {
                var detalles = $.grep(listaRentabilidad, function(el,index){ return el.cc == economicos[i]; });
                var estatus = maquinasEstatus.find(function (el) { return el.Value == economicos[i]; });

                var stringGrupo = "OTROS";
                var stringEstatus = "-1";
                var stringCC = "-1";
                var grupoMaquina = gruposMaquina.find(function (el) { return el.Text == economicos[i]; });
                if(grupoMaquina != null) stringGrupo = grupoMaquina.Value;
                if(estatus != null) {
                    stringEstatus = estatus.Text;
                    stringCC = estatus.Prefijo;
                }
                var item = { 
                    descripcion: economicos[i], costos: 0, resultadoNeto: 0, margen: 0, detalles: detalles, grupoMaquina: stringGrupo, estatus: stringEstatus, centro_costos: stringCC
                };

                //var ingresos = $.grep(listaRentabilidadIngresos, function(el,index){ return el.cc == economicos[i]; });
                //if (ingresos.length > 0) { $.each(ingresos, function(index, element){ item.ingresos += (element.monto) || 0; }); }
                
                var costos = $.grep(listaRentabilidadCostos, function(el,index){ return el.cc == economicos[i]; });
                if (costos.length > 0) { $.each(costos, function(index, element){ item.costos += (element.monto * (-1)) || 0; }); }
                
                item.resultadoNeto = item.costos;
                item.margen = item.ingresos != 0 ? (item.resultadoNeto / item.ingresos) * 100 : 0;
                listaUtilidad.push(item);
            }
            listaUtilidad = listaUtilidad.sort((a, b) => (a.descripcion < b.descripcion) ? 1 : -1);
            return listaUtilidad.sort((a, b) => (a.grupoMaquina > b.grupoMaquina) ? 1 : -1);

        }

        function SetMaquinasEstatus(economicos)
        {
            var maquinasEstatus;
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/Rentabilidad/getEconomicoEstatus',
                data: { economicos: economicos },
                success: function (response) {
                    if (response.success) {
                        maquinasEstatus = response.maquinasEstatus;                         
                    } else {
                    }
                },
                error: function () {
                }
            });  
            return maquinasEstatus;
        }

        function SetGrupoMaquinas()
        {
            var maquinas = [];
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/Rentabilidad/getGrupoMaquinas',
                data: {  },
                success: function (response) {
                    if (response.success) {
                        maquinas = response.maquinas;
                        
                    } else {
                    }
                },
                error: function () {
                }
            });   
            return maquinas;
        }

        function obtenerListaKubrix(listaRentabilidad, busq)
        {
            var listaUtilidad = [];
            //--Ingresos Contabilizados--//


            //--Costo Total--//
            var elemento = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0; });
            var utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Costo Total", 5, false);
            listaUtilidad.push(utilidad);
            //--Depreciación--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5000-10") >= 0; }); 
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Depreciación", 6, false);
            listaUtilidad.push(utilidad);
            //--Costo Estimado--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cuenta == "1-4-0"; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Costo Estimado", 15, false);
            listaUtilidad.push(utilidad);
            //--Utilidad Bruta--//       
            //--Gastos de Operación--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5280-") >= 0; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Gastos de Operación", 8, false);
            listaUtilidad.push(utilidad);
            //--Gastos Antes de Finacieros--//           
            //--Gastos de Financieros--//
            //--Resultado con Financieros--//           
            //--Otros Ingresos--//
            //--Resultado Neto--//    
            utilidad =
            {
                tipo_mov: 13,
                descripcion: "Total",
                mayor: listaUtilidad[0].mayor + listaUtilidad[1].mayor + listaUtilidad[2].mayor + listaUtilidad[3].mayor,
                menor: listaUtilidad[0].menor + listaUtilidad[1].menor + listaUtilidad[2].menor + listaUtilidad[3].menor,
                transporteConstruplan: listaUtilidad[0].transporteConstruplan + listaUtilidad[1].transporteConstruplan + listaUtilidad[2].transporteConstruplan + listaUtilidad[3].transporteConstruplan,
                transporteArrendadora: listaUtilidad[0].transporteArrendadora + listaUtilidad[1].transporteArrendadora + listaUtilidad[2].transporteArrendadora + listaUtilidad[3].transporteArrendadora,
                administrativoCentral: listaUtilidad[0].administrativoCentral + listaUtilidad[1].administrativoCentral + listaUtilidad[2].administrativoCentral + listaUtilidad[3].administrativoCentral,
                administrativoProyectos: listaUtilidad[0].administrativoProyectos + listaUtilidad[1].administrativoProyectos + listaUtilidad[2].administrativoProyectos + listaUtilidad[3].administrativoProyectos,
                fletes: listaUtilidad[0].fletes + listaUtilidad[1].fletes + listaUtilidad[2].fletes + listaUtilidad[3].fletes,
                neumaticos: listaUtilidad[0].neumaticos + listaUtilidad[1].neumaticos + listaUtilidad[2].neumaticos + listaUtilidad[3].neumaticos,
                otros:listaUtilidad[0].otros + listaUtilidad[1].otros + listaUtilidad[2].otros + listaUtilidad[3].otros,
                actual:listaUtilidad[0].actual + listaUtilidad[1].actual + listaUtilidad[2].actual + listaUtilidad[3].actual,
                semana2: listaUtilidad[0].semana2 + listaUtilidad[1].semana2 + listaUtilidad[2].semana2 + listaUtilidad[3].semana2,
                semana3: listaUtilidad[0].semana3 + listaUtilidad[1].semana3 + listaUtilidad[2].semana3 + listaUtilidad[3].semana3,
                semana4 :listaUtilidad[0].semana4 + listaUtilidad[1].semana4 + listaUtilidad[2].semana4 + listaUtilidad[3].semana4,
                semana5: listaUtilidad[0].semana5 + listaUtilidad[1].semana5 + listaUtilidad[2].semana5 + listaUtilidad[3].semana5,
            };
            var auxNeto = utilidad;
            listaUtilidad.push(utilidad);
            //--% de Margen--//

            return listaUtilidad;
        }

        function AsignarImportesKubrix(importes, fecha, descripcion, tipoMov, negativo)
        {
            var costo = { 
                descripcion: descripcion, tipo_mov: tipoMov, mayor: 0, menor: 0,transporteConstruplan: 0, transporteArrendadora: 0,
                administrativoCentral: 0, administrativoProyectos: 0, fletes: 0, neumaticos: 0, otros: 0, total: 0, actual: 0,
                semana2: 0, semana3: 0, semana4: 0, semana5: 0, detalles: importes
            };

            var mayor = $.grep(importes,function(el,index){ return el.tipo == 1 && el.semana == 1; });
            if (mayor.length > 0) { $.each(mayor, function(index, element){ costo.mayor += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var menor = $.grep(importes,function(el,index){ return el.tipo == 2 && el.semana == 1; });
            if (menor.length > 0) { $.each(menor, function(index, element){ costo.menor += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var transporteConstruplan = $.grep(importes,function(el,index){ return el.tipo == 3 && el.semana == 1; });
            if (transporteConstruplan.length > 0) { $.each(transporteConstruplan, function(index, element){ costo.transporteConstruplan += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var transporteArrendadora = $.grep(importes,function(el,index){ return el.tipo == 8 && el.semana == 1; });
            if (transporteArrendadora.length > 0) { $.each(transporteArrendadora, function(index, element){ costo.transporteArrendadora += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var administrativoCentral = $.grep(importes,function(el,index){ return el.tipo == 6 && el.semana == 1; });
            if (administrativoCentral.length > 0) { $.each(administrativoCentral, function(index, element){ costo.administrativoCentral += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var administrativoProyectos = $.grep(importes,function(el,index){ return el.tipo == 9 && el.semana == 1; });
            if (administrativoProyectos.length > 0) { $.each(administrativoProyectos, function(index, element){ costo.administrativoProyectos += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var fletes = $.grep(importes,function(el,index){ return el.tipo == 4 && el.semana == 1; });
            if (fletes.length > 0) { $.each(fletes, function(index, element){ costo.fletes += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var neumaticos = $.grep(importes,function(el,index){ return el.tipo == 5 && el.semana == 1; });
            if (neumaticos.length > 0) { $.each(neumaticos, function(index, element){ costo.neumaticos += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var otros = $.grep(importes,function(el,index){ return el.tipo == 7 && el.semana == 1; });
            if (otros.length > 0) { $.each(otros, function(index, element){ costo.otros += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var total = $.grep(importes,function(el,index){ return el.semana == 1; });
            if (total.length > 0) { $.each(total, function(index, element){ costo.total += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
                    
            //Semanal
            var auxFecha = new Date(fecha);
            var actual = jQuery.grep(importes, function( n, i ) { return n.semana == 1; });
            if (actual.length > 0) { $.each(actual, function(index, element){ costo.actual += (element.monto * (negativo ? (-1) : 1)) || 0; });}
            var semana2 = jQuery.grep(importes, function( n, i ) { return n.semana == 2 });
            if (semana2.length > 0) { $.each(semana2, function(index, element){ costo.semana2 += (element.monto * (negativo ? (-1) : 1)) || 0; });}
            var semana3 = jQuery.grep(importes, function( n, i ) { return n.semana == 3 });
            if (semana3.length > 0) { $.each(semana3, function(index, element){ costo.semana3 += (element.monto * (negativo ? (-1) : 1)) || 0; });}
            var semana4 = jQuery.grep(importes, function( n, i ) { return n.semana == 4 });
            if (semana4.length > 0) { $.each(semana4, function(index, element){ costo.semana4 += (element.monto * (negativo ? (-1) : 1)) || 0; });}
            var semana5 = jQuery.grep(importes, function( n, i ) { return n.semana == 5 });
            if (semana5.length > 0) { $.each(semana5, function(index, element){ costo.semana5 += (element.monto * (negativo ? (-1) : 1)) || 0; });}
               
            costo.detalles = importes;
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
        
        init();
    };

    $(document).ready(function () {
        maquinaria.rentabilidad.cortecostos = new cortecostos();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();