(function () {

    $.namespace('maquinaria.rentabilidad.cortecplan');
    cortecplan = function () {
    //#region VARIABLE
                let cuentasDescr = [];
                var estimado = ["1-1-0", "1-2-1", "1-2-2", "1-2-3", "1-3-1", "1-3-2", "1-4-0"];

                const comboAC = $('#comboAC');
                const comboTipo = $('#comboTipo');
                const cbTipoCorte = $('#cbTipoCorte');
                const inputCorte = $('#inputCorte');
                const chbTipoReporte = $("#chbTipoReporte");
                const cbConfiguracion = $("#cbConfiguracion");
                const chkConfiguracion = $("#chkConfiguracion");
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

                var lstDetalle = [];

                const getLstFchasCortes = new URL(window.location.origin + '/Rentabilidad/getLstFechasCortes');
                
                const getLstKubrix = new URL(window.location.origin + '/Rentabilidad/getLstKubrixCorteConstruplan');
                const getLstKubrixDetalle = new URL(window.location.origin + '/Rentabilidad/getLstKubrixCorteDet');
                const getLstKubrixTablaDet = new URL(window.location.origin + '/Rentabilidad/getLstKubrixCorteDetTabla');
                const getLstKubrixCostoEstimado = new URL(window.location.origin + '/Rentabilidad/getLstKubrixCorteCostoEstimado');

                const getLstCC = new URL(window.location.origin + '/Rentabilidad/getLstCC');
                const guardarLstCC = new URL(window.location.origin + '/Rentabilidad/guardarLstCC');
                const checkResponsable = new URL(window.location.origin + '/Rentabilidad/checkResponsable');
                const GetGrupoMaquinas = new URL(window.location.origin + '/Rentabilidad/getGrupoMaquinas');

                const GetMaquinasEstatus = new URL(window.location.origin + '/Rentabilidad/getEconomicoEstatus');

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
                let economicoFiltroDetEco = "";
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
                const conceptos = [
                    "Ingresos Contabilizados"
                    , "<p class='guardarLinea' data-tipoGuardado='1'>Ingresos con Estimación</p>"
                    , "<p class='guardarLinea' data-tipoGuardado='2'>Ingresos Pendientes por Generar</p>"
                    , "Total Ingresos"
                    , "Costo Total"
                    , "Depreciación"
                    , "Financieros"
                    , "<p class='guardarLinea' data-tipoGuardado='3'>Costos Estimados</p>"
                    , "Resultado de Operación"
                    , "Gastos de Operación"
                    , "Resultado Antes de Efecto Cambiario Neto"
                    , "Efecto Cambiario Neto"
                    , "Resultado con Efecto Cambiario"
                    , /*"Otros Ingresos", "Resultado Neto",*/ "% de Margen"
                    , "ARRENDADORA"
                    , "Resultado con Efecto semanal"];
                let divisiones = [];
                let areasCuenta = [];
                let areasCuentaDetalle = [];


                const getLstAnalisis = new URL(window.location.origin + '/Rentabilidad/getLstAnalisis');


                //Guardar linea
                const botonGuardarLinea = $("#botonGuardarLinea");
                const botonCerrarCostoEst = $("#botonCerrarCostoEst");
                const agregarCostoEst = $("#agregarCostoEst");

                const tablaCostosEstimados = $("#tablaCostosEstimados");
                let dtTablaCostosEstimados;

                //Cuentas por pagar
                const tablaCXP = $("#tablaCXP");
                let dtTablaCXP;

                const tablaCXPFacturas = $("#tablaCXPFacturas");
                let dtTablaCXPFacturas;

                const tablaCXPAC = $("#tablaCXPAC");
                let dtTablaCXPAC;

                //Cuentas por cobrar
                const tablaCXC = $("#tablaCXC");
                let dtTablaCXC;

                const tablaCXCFacturas = $("#tablaCXCFacturas");
                let dtTablaCXCFacturas;

                const tablaCXCAC = $("#tablaCXCAC");
                let dtTablaCXCAC;

                //Guardar Archivos Estimados
                const btncargarArchivo = $("#btncargarArchivo");
                const inCargarArchivo = $("#inCargarArchivo");
                let estadoConfirmacion = 0;
    //#endregion 
        function init() {
            // obtenerDapper();
          //#region FUNCIONES DENTRO DEL INIT
                getEmpresaActual();
                setResponsable();
                setCuentasDescr();
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
                //Guardar Linea
                initTablaCostosEstimados();
                $("#acGuardarLinea").select2();
                $("#acGuardarLinea").fillCombo("cboObraKubrix");
                $("#fechaGuardarLinea").datepicker().datepicker('setDate', new Date());

                //Cuentas por Pagar
                initTablaCXP();
                initTablaCXPFacturas();
                initTablaCXPAC();
                //Cuentas por Cobrar
                initTablaCXC();
                initTablaCXCFacturas();
                initTablaCXCAC();

                //Subir Archivo Estimados
                btncargarArchivo.click(function (e) {
                    e.preventDefault();
                    estadoConfirmacion = 2;
                    ConfirmacionEliminacion("Alerta", "¿Está seguro que desea eliminar los costos estimados registrados y reemplazarlos?");
                    //inCargarArchivo.click();
                });
                inCargarArchivo.change(function (e) {
                    SubirArchivoEstimados(e);
                });
                $(document).on('click', "#btnModalEliminar", function (e) {
                    e.preventDefault();
                    if (estadoConfirmacion == 1) EliminarLinea();
                    if (estadoConfirmacion == 2) inCargarArchivo.click();
                });
          //#endregion
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
            $.post("/Base/getEmpresa").then(function(response) { empresaActual = response; /*cbConfiguracion.val(response == 2 ? 1 : 0);*/ } );
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
        function obtenerDapper(){
            axios.post('obtenerDapper')
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    let { success, items} = response.data;
                    if (success) {
                        
                    }
                });
        }
        function agregarListeners()
        {
            //-->Recarga de detalles actual / acumulado
            chkConfiguracion.change(function(e){    
                if (dtTablaKubrixDetalle != null) {
                    var datos = dtTablaKubrixDetalle.rows().data();
                    var detallesRespuesta = [];
                    for(var i = 0; i < datos.length; i++) {
                        var aux1 = datos[i].detalles;
                        if(aux1 != null)
                        {
                            detallesRespuesta = $.merge(detallesRespuesta, aux1);
                        }                            
                    }                    
                    let busq = getBusquedaDTODetalle();
                    var lst = obtenerListaKubrix(distinctArrayBy(detallesRespuesta, "id"), busq,lstDetalle);  
                    if (botonBuscar.attr("data-tipoCorte") == 0) { initTablaKubrixDetalle(lst, 1); }
                    else { initTablaKubrixDetalle(lst, 3); }
                }
                if (dtTablaKubrixDetalleEco != null) {
                    var datos = dtTablaKubrixDetalleEco.rows().data();
                    var detallesRespuesta = [];
                    for(var i = 0; i < datos.length; i++) {
                        var aux1 = datos[i].detalles;
                        if(aux1 != null)
                        {
                            detallesRespuesta = $.merge(detallesRespuesta, aux1);
                        }                            
                    }
                    let busq = getBusquedaDTODetalle();
                    var lst = $("#chbDespliegueDetEco").is(":checked") ? obtenerListaKubrixEcoCompacto(distinctArrayBy(detallesRespuesta, "id"), busq) : obtenerListaKubrixEconomico(distinctArrayBy(detallesRespuesta, "id"), busq);
                    initTablaKubrixDetalleEco(lst);
                }

                if (dtTablaKubrixDivision != null) {
                    var datos = dtTablaKubrixDivision.rows().data();
                    var detallesRespuesta = [];
                    for(var i = 0; i < datos.length; i++) {
                        var aux1 = datos[i].detalles;
                        if(aux1 != null)
                        {
                            detallesRespuesta = $.merge(detallesRespuesta, aux1);
                        }                            
                    }
                    divisiones = getDivisiones(detallesRespuesta);
                    //$.merge(divisiones, auxDivisiones);
                    var datosFinal = FormatoDetalles(distinctArrayBy(detallesRespuesta, "id"), divisiones, 1,lstDetalle,1);
                    initTablaKubrixDivision(datosFinal, divisiones, "CONSTRUPLAN"); 
                }

                if (dtTablaKubrixAreaCuenta != null && $("#divTablaKubrixAreaCuenta").is(":visible")) {
                    var datos = dtTablaKubrixAreaCuenta.rows().data();
                    var detallesRespuesta = [];
                    for(var i = 0; i < datos.length; i++) {
                        var aux1 = datos[i].detalles;
                        if(aux1 != null)
                        {
                            detallesRespuesta = $.merge(detallesRespuesta, aux1);
                        }                            
                    }
                    var nombreColumna = $(dtTablaKubrixAreaCuenta.column(1).header()).text().trim();
                    var datosFinal = FormatoDetalles(distinctArrayBy(detallesRespuesta, "id"), areasCuenta, 2,lstDetalle,1);
                    initTablaKubrixAreaCuenta(datosFinal, areasCuenta, nombreColumna);

                }             
            });
            //<--
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
            $("#btnCerrarCostosEstimados").click(function(e){
                $("#modalCostosEstimados").modal("hide");
            });
            $("#botonCerrarGuardarLinea").click(function(e){
                $("#modalGuardarLinea").modal("hide");
            });
            //cbConfiguracion.change(recargarFechasTipoRep);
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
                //var lstDetalleFiltro = $.grep(lstDetalle,function(el,index){ return ( && el != null); });

                if($(this).is(":checked")) {                    
                    let rowData = dtTablaKubrixDetalleEco.data();
                    var detallesRaw = rowData.map(function(x) { return x.detalles});
                    var detalles = [].concat.apply([], detallesRaw);
                    //detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                    setFormatoKubrixDetalles(e, detalles);
                }
                else {                    
                    let rowData = dtTablaKubrixDetalle.data();
                    var detallesRaw = rowData.map(function(x) { return x.detalles});
                    var detalles = [].concat.apply([], detallesRaw);
                    //detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
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
                //detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                setFormatoKubrixDetallesEconomico(e, detalles);
            });
            //Tabla Kubrix Detalle Filtros
            $('#botonAtrasDetalle').click(function(){
                areasCuentaDetalle = areasCuenta;
                recargarTotalizadoresCX();
                stringAreaCuenta = "TODAS";
                if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                //$("#chbAgrupacionDet").hide(500);
                $("#divTablaKubrixAreaCuenta").show(500);
                areaCuentaFiltroDetalle = "";
                economicoFiltro = "";
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
                recargarTotalizadoresCX();
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
                recargarTotalizadoresCX();
                stringAreaCuenta = "TODAS";
                if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                //$("#chbAgrupacionDet").hide(500);
                $("#divTablaKubrixAreaCuenta").show(500);
                areaCuentaFiltroDetalle = ""
            });
            //Guarda linea
            botonGuardarLinea.click(guardarLinea);
            botonCerrarCostoEst.click(cerrarCostoEst);
            agregarCostoEst.click(AbrirModalGuardarLinea);
            $("#modalCostosEstimados").on('shown.bs.modal', function (e) {
                dtTablaCostosEstimados.columns.adjust();                
            });
            //Cuentas por pagar
            $(".pCXP").click(AbrirModalCXP);
            $("#modalCXP").on('shown.bs.modal', function (e) {
                dtTablaCXP.columns.adjust();                
            });
            $("#modalCXPFacturas").on('shown.bs.modal', function (e) {
                dtTablaCXPFacturas.columns.adjust();                
            });
            $("#botonCerrarCXPFacturas").click(function(e){
                $("#modalCXPFacturas").modal("hide");
            });
            $("#modalCXPAC").on('shown.bs.modal', function (e) {
                dtTablaCXPAC.columns.adjust();                
            });
            $("#botonCerrarCXPAC").click(function(e){
                $("#modalCXPAC").modal("hide");
            });
            //Cuentas por Cobrar
            $(".pCXC").click(AbrirModalCXC);
            $("#modalCXC").on('shown.bs.modal', function (e) {
                dtTablaCXC.columns.adjust();                
            });
            $("#modalCXCFacturas").on('shown.bs.modal', function (e) {
                dtTablaCXCFacturas.columns.adjust();                
            });
            $("#botonCerrarCXCFacturas").click(function(e){
                $("#modalCXCFacturas").modal("hide");
            });
            $("#modalCXCAC").on('shown.bs.modal', function (e) {
                dtTablaCXCAC.columns.adjust();                
            });
            $("#botonCerrarCXCAC").click(function(e){
                $("#modalCXCAC").modal("hide");
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
            //var tipoReporte = cbConfiguracion.val();
            var tipoReporte = 0;
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
        function setLstKubrixCostoEstimado()
        {
            $.post(getLstKubrixCostoEstimado, { corteID: botonBuscar.attr("data-corteid") })
                .then(function (response) {
                    if (response.success) {

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

            if (dtTablaSubCuenta != null) { dtTablaSubCuenta.clear(); }
            if (dtTablaSubSubCuenta != null) { dtTablaSubSubCuenta.clear(); }
            if (dtTablaDivision != null) { dtTablaDivision.clear(); }
            if (dtTablaAreaCuenta != null) { dtTablaAreaCuenta.clear(); }
            if (dtTablaConciliacion != null) { dtTablaConciliacion.clear(); }
            if (dtTablaEconomico != null) { dtTablaEconomico.clear(); }
            if (dtTablaDetalles != null) { dtTablaDetalles.clear(); }

            if (dtTablaAnalisis != null) { dtTablaAnalisis.clear(); }
            if (dtTablaSctaDetalles != null) { dtTablaSctaDetalles.clear(); }
            if (dtTablaDetallesA != null) { dtTablaDetallesA.clear(); }
            if (dtTablaSubdetalles != null) { dtTablaSubdetalles.clear(); }
            if (dtTablaSubdetallesIngresos != null) { dtTablaSubdetallesIngresos.clear(); }
            if (dtTablaCostosEstimados != null) { dtTablaCostosEstimados.clear(); }
            if (dtTablaCXP != null) { dtTablaCXP.clear(); }
            if (dtTablaCXPFacturas != null) { dtTablaCXPFacturas.clear(); }
            if (dtTablaCXPAC != null) { dtTablaCXPAC.clear(); }
            if (dtTablaCXC != null) { dtTablaCXC.clear(); }
            if (dtTablaCXCFacturas != null) { dtTablaCXCFacturas.clear(); }
            if (dtTablaCXCAC != null) { dtTablaCXCAC.clear(); }   
            
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
                areaCuenta: areasCuentaGlobal, 
                economico: cboMaquina.val() == "TODOS" ? null : $("#cboMaquina option:selected").text().trim(),
                //fechaInicio: inputDiaInicio.val(),
                fechaFin: inputDiaFinal.val(),
                tipoCorte : cbTipoCorte.val(),
                //acumulado: cbConfiguracion.val()
                acumulado: 0
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
                        var detallesRespuesta = response.lst;
                         lstDetalle = response.lstDetalle;
                        //for(var i = 0; i < auxDatos.length; i++) {
                        //    var aux1 = auxDatos[i].detalles;
                        //    if(aux1 != null)
                        //    {
                        //        detallesRespuesta = $.merge(detallesRespuesta, aux1);
                        //    }                            
                        //}
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
                                areasCuenta = ["CONSTRUPLAN"];
                                auxAreasCuenta = detallesRespuesta.map(function(element) { return element.areaCuenta.trim(); });
                                auxAreasCuenta = $.grep(auxAreasCuenta,function(el,index){ return (index == $.inArray(el,auxAreasCuenta) && el != null); });
                                auxAreasCuenta.sort(SortByAreaCuenta);
                                $.merge(areasCuenta, auxAreasCuenta);
                                areasCuentaDetalle = auxAreasCuenta;
                                recargarTotalizadoresCX();
                                var datos = FormatoDetalles(detallesRespuesta, areasCuenta, 2,lstDetalle,1);
                                initTablaKubrixAreaCuenta(datos, areasCuenta, "CONSTRUPLAN");                            
                                if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
                                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                                $("#divTablaKubrixAreaCuenta").show(500);
                                //Cargar Costos Estimados
                                //var costosEst = $.grep(detallesRespuesta, function( n, i ) { return n.grupoInsumo == "1-4-0";  });
                                //cargarTablaCostosEstimados();
                            }
                            else
                            {
                                divisiones = getDivisiones(detallesRespuesta);
                                //$.merge(divisiones, auxDivisiones);
                                var datos = FormatoDetalles(detallesRespuesta, divisiones, 1,lstDetalle,1);
                                initTablaKubrixDivision(datos, divisiones, "CONSTRUPLAN"); 
                                if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
                                if($("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").hide(500);
                                if($("#divTablaKubrixDetalle").is(":visible")) $("#divTablaKubrixDetalle").hide(500);
                                if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
                                $("#divTablaKubrixDivision").show(500);
                                //Cargar Costos Estimados
                                //var costosEst = $.grep(detallesRespuesta, function( n, i ) { return n.grupoInsumo == "1-4-0";  });
                                //cargarTablaCostosEstimados();
                            }
                        }    
                        // CXP
                        var detallesCXP = response.CXP;
                        var totalCXP = 0;
                        $.each(detallesCXP, function(i, n) { totalCXP += n.monto; });
                        $(".pCXP").text(parseFloat(totalCXP / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") );                        
                        cargarTablaCXP(response.CXP, fechaCorteGeneral);
                        //CXC
                        var detallesCXC = response.CXC;
                        var totalCXC = 0;
                        $.each(detallesCXC, function(i, n) { totalCXC += n.monto; });
                        $(".pCXC").text(parseFloat(totalCXC / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") );                        
                        cargarTablaCXC(response.CXC, fechaCorteGeneral);
                        $("#divTablaCuentasDiv").css("display", "block");
                        //Check Costos Estimados
                        if(CheckCostoEstCerrado()) { $("#mensajeCostosEst").css("display", "none"); }
                        else { $("#mensajeCostosEst").css("display", "block"); }
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
        function getDivisiones(detallesRespuesta)
        {
            let divisionesData = ["CONSTRUPLAN"];
            auxDivisiones = detallesRespuesta.map(function(element) { return element.division; });
            auxDivisiones = $.grep(auxDivisiones,function(el,index){ return (index == $.inArray(el,auxDivisiones) && el != null); });
            if(jQuery.inArray( "ADMINISTRACION", auxDivisiones ) != -1) { 
                auxDivisiones = jQuery.grep(auxDivisiones, function(value) { return value != 'ADMINISTRACION'; });
                auxDivisiones.push('ADMINISTRACION');
            }
            if(jQuery.inArray( "ADMINISTRACION ARRENDADORA", auxDivisiones ) != -1) { 
                auxDivisiones = jQuery.grep(auxDivisiones, function(value) { return value != 'ADMINISTRACION ARRENDADORA'; }); 
                auxDivisiones.push('ADMINISTRACION ARRENDADORA');
            }
            if(jQuery.inArray( "FLETES", auxDivisiones ) != -1) { 
                auxDivisiones = jQuery.grep(auxDivisiones, function(value) { return value != 'FLETES'; }); 
                auxDivisiones.push('FLETES');
            }
            if(jQuery.inArray( "LLANTAS OTR", auxDivisiones ) != -1) { 
                auxDivisiones = jQuery.grep(auxDivisiones, function(value) { return value != 'LLANTAS OTR'; });
                auxDivisiones.push('LLANTAS OTR');
            }
            if(jQuery.inArray( "SIN DIVISION", auxDivisiones ) != -1) { 
                auxDivisiones = jQuery.grep(auxDivisiones, function(value) { return value != 'SIN DIVISION'; });
                auxDivisiones.push('SIN DIVISION');
            }

            $.merge(divisionesData, auxDivisiones);
            return divisionesData;
        }
        function setLstKubrixTablaDet(tipo, columna, renglon, fecha, nombreRow, negativo, divisionCol, areaCuentaCol, economicoCol, semanal) {
            if (dtTablaDetalles != null) { dtTablaDetalles.clear(); }

            var tablaDetalleEco = $("#divTablaKubrixDetalleEco").is(":visible");            

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
                economicoFiltro: tablaDetalleEco ? economicoFiltroDetEco : economicoFiltro,
                empresa: empresaGlobal,
                semanal: semanal, 
                //acumulado: cbConfiguracion.val()
                acumulado: chkConfiguracion.is(":checked") ? 0 : 1
            })
                .then(function(response) {
                    if (response.success) {

                        // Operación exitosa.
                        var detalles = response.detalles;
                        var numAreasCuenta = botonBuscar.attr("data-obra").split(',').length;
                        var sumadetalles = 0;
                        for(var i = 0; i < detalles.length; i++) { sumadetalles += detalles[i].importe; }
                        //numAreasCuenta == 1 && botonBuscar.attr("data-obra") != ""
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
                economicoCol: economicoCol
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
            var lst = obtenerListaKubrix(datos, busq,lstDetalle);  
            if (botonBuscar.attr("data-tipoCorte") == 0) { initTablaKubrixDetalle(lst, 1); }
            else { initTablaKubrixDetalle(lst, 3); }
            if($("#divTablaKubrixDivision").is(":visible")) $("#divTablaKubrixDivision").hide(500);
            if($("#divTablaKubrixDetalleEco").is(":visible")) $("#divTablaKubrixDetalleEco").hide(500);
            if($("#divTablaKubrixAreaCuenta").is(":visible")) $("#divTablaKubrixAreaCuenta").hide(500);
            if(!$("#contenedorDetalles").is(":visible")) $("#contenedorDetalles").show(500);
            $("#divTablaKubrixDetalle").show(500);
            //Cargar Costos Estimados
            //var costosEst = $.grep(datos, function( n, i ) { return n.grupoInsumo == "1-4-0";  });
            //cargarTablaCostosEstimados(costosEst);
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
            //Cargar Costos Estimados
            //var costosEst = $.grep(datos, function( n, i ) { return n.grupoInsumo == "1-4-0";  });
            //cargarTablaCostosEstimados(costosEst);
        }
        function FormatoDetalles(source, separadores, tipo,lstDetalle,TipoTabla,division)
        {
            var datos = [];
            var detalles = [];
            var detalles2 = [];
            var auxseparadores = [];
            for(var i = 1; i <= 16; i++)
            {
                const numSemana = 1;
                detalles = [];
                detalles2 = [];
                auxseparadores = [];
                switch(i)
                {
                    case 1:
                        detalles =  $.grep(source, function(el,index){ return (el.cuenta.indexOf("4000-") >= 0 && el.cuenta.indexOf("4000-8-") < 0 && el != null); }); 
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
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
                                var data = tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                                return data;
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 2:
                        detalles = $.grep(source, function(el,index){ return ((el.cuenta == "1-1-0" || el.cuenta == "1-3-1" || el.cuenta == "1-3-2") && el != null); }); 
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
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
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 3:
                        detalles = $.grep(source, function(el,index){ return ((el.cuenta == "1-2-1" || el.cuenta == "1-2-2" || el.cuenta == "1-2-3") && el != null); });  
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
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
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 4:
                        var totalImporte = 0;
                        for(var j = 0; j < 3; j ++)
                        {
                            if(datos[j].separadores.length > 1)
                            {
                                $.each(datos[j].separadores, function(index, element){
                                    if(index > 0) totalImporte += element || 0;
                                });
                            }
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = 0;
                            for(var k = 0; k < 3; k ++)
                            {
                                if(datos[k].separadores.length > j)
                                {
                                    auxTotalSep += datos[k].separadores[j];
                                }
                            }
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
                        break;
                    case 5:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0 && el != null); });
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){  return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto * (-1)) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 6:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5000-10-") >= 0 && el != null); });    
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto * (-1)) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 7:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5900-") >= 0 && el.cuenta.indexOf("5900-1-") < 0 && el != null); });    
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto * (-1)) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 8:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta == "1-4-0" && el != null); });  
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });                         
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto * (-1)) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 9:
                        var totalImporte = datos[3].separadores[0];
                        totalImporte -= datos[4].separadores[0];
                        totalImporte -= datos[5].separadores[0];
                        totalImporte -= datos[6].separadores[0];
                        totalImporte -= datos[7].separadores[0];
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = datos[3].separadores[j];
                            for(var k = 4; k <= 7; k ++)
                            {
                                if(datos[k].separadores.length > j)
                                {
                                    auxTotalSep -= datos[k].separadores[j];
                                }
                            }
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
                        break;
                    case 10:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5280-") >= 0 && el != null); });  
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto * (-1)) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 11:
                        var totalImporte = datos[8].separadores[0];
                        totalImporte -= datos[9].separadores[0];
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = datos[8].separadores[j];
                            auxTotalSep -= datos[9].separadores[j];
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
                        break;
                    case 12:
                        detalles = $.grep(source, function(el,index){ return ((el.cuenta.indexOf("4900-1-") >= 0 || el.cuenta.indexOf("5900-1-") >= 0) && el != null); });  
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
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
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 13:
                        var totalImporte = datos[10].separadores[0];
                        totalImporte += datos[11].separadores[0];
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = datos[10].separadores[j];
                            auxTotalSep += datos[11].separadores[j];
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
                        break;
                  //#region 
                        //case 14:
                        //detalles = $.grep(source, function(el,index){ return ((el.cuenta.indexOf("4901-") >= 0 || el.cuenta.indexOf("4000-8-") >= 0 || el.cuenta.indexOf("5901-") >= 0) && el != null); });   
                        //var totalImporte = 0;
                        //var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (chkConfiguracion.is(":checked") ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                        //if(detallesTotal.length > 0)
                        //{
                        //    $.each(detallesTotal, function(index, element){
                        //        totalImporte += element.monto || 0;
                        //    });
                        //}
                        //auxseparadores.push(totalImporte);
                        //for(var j = 1; j <= separadores.length; j++)
                        //{
                        //    var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                        //        return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                        //    });
                        //    var auxseparadoresImporte = 0
                        //    var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (chkConfiguracion.is(":checked") ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                        //    if(detallesTotalDetalle.length > 0)
                        //    {
                        //        $.each(detallesTotalDetalle, function(index, element){
                        //            auxseparadoresImporte += element.monto || 0;
                        //        });
                        //    }
                        //    auxseparadores.push(auxseparadoresImporte);
                        //}
                    //    break;
                    //case 15:
                        //var totalImporte = datos[12].separadores[0];
                        //totalImporte += datos[13].separadores[0];
                        //auxseparadores.push(totalImporte);
                        //for(var j = 1; j <= separadores.length; j++)
                        //{
                        //    var auxTotalSep = datos[12].separadores[j];
                        //    auxTotalSep += datos[13].separadores[j];
                        //    auxseparadores.push(auxTotalSep);
                        //}
                        //detalles = [];
                        //detalles = [];
                        //break;
                        //#endregion
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
                        //SWITCH
                    case 15:
                        //#region ALGO QUE NO SIRVE
                         if (TipoTabla === 1) {
                            let detallesTotal = [];                                           // return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2;
                            let detallesTotal2 = [];                                           // return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2;
                            var TotalArrendadora = $.grep(lstDetalle, function(n,index){ return (chkConfiguracion.is(":checked") ? (n.semana > 1 && n.acumulado == true)  : (n.semana == 1 && n.acumulado == false)) ; });
                          
                            var totalImporte = 0;
                            if(TotalArrendadora.length > 0)
                            {
                                $.each(TotalArrendadora, function(index, element){
                                    totalImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(totalImporte);

                            for (let j = 1; j < separadores.length; j++) {
                                let auxseparadoresImporte = 0;
                                var auxseparadoresImporteCuenta = $.grep(TotalArrendadora, function (el, index){
                                    return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                                });
                                var nuevalista = $.grep(auxseparadoresImporteCuenta, function(i, n) { 
                                        i.monto = (i.monto);
                                    return i;
                                });
                                if(nuevalista.length > 0)
                                {
                                    nuevalista.forEach(element => {
                                        auxseparadoresImporte += element.monto;
                                    });
                                }
                              
                                    auxseparadores.push(auxseparadoresImporte);
                            }
                         }
                         else if(TipoTabla === 2){

                            var lstFormato = $.grep(lstDetalle, function(n,index){ return division != "CONSTRUPLAN" ? (n.division == division) : n.division ; });
                            // return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2;
                            var TotalArrendadora = $.grep(lstFormato, function(n,index){ return (chkConfiguracion.is(":checked") ? (n.semana > 1 && n.acumulado == true)  : (n.semana == 1 && n.acumulado == false)); });
                            var totalImporte = 0;
                            $.each(TotalArrendadora, function(index, element){
                             
                                    totalImporte += element.monto;

                            });
                                auxseparadores.push(totalImporte);
                            for (let j = 1; j < separadores.length; j++) {
                             
                                let auxseparadoresImporte = 0;
                                var auxseparadoresImporteCuenta = $.grep(TotalArrendadora, function (el, index){
                                    return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : (el.areaCuenta.split(' ')[0]) == separadores[j].split(' ')[0] && el.acumulado == false);
                                });
                                var nuevalista = $.grep(auxseparadoresImporteCuenta, function(i, n) { 
                                        i.monto = (i.monto);
                                    return i;
                                });
                                if(nuevalista.length > 0)
                                {
                                    nuevalista.forEach(element => {
                                        auxseparadoresImporte += element.monto;
                                    });
                                }
                                    auxseparadores.push(auxseparadoresImporte);
                            }
                         }
                        //#endregion
                       
                       
                        break;
                    case 16:
                        detalles = source;  
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(n,index){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov > 0; });
                       
                       
                       
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto);
                            });
                        }
                        auxseparadores.push(totalImporte * (-1));
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detallesTotal, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta) == separadores[j];
                            });
                            var auxseparadoresImporte = 0;

                            var nuevalista = $.grep(auxseparadoresImporteCuenta, function(i, n) { 
                                if (i.monto < 0) {
                                    i.monto = (i.monto);
                                }
                                return i;
                            });
                            if(nuevalista.length > 0)
                            {
                                nuevalista.forEach(element => {
                                    auxseparadoresImporte += element.monto;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte  * -1);
                        }


                        break;
                }
                var auxDatos = { descripcion: conceptos[i - 1], separadores: auxseparadores, detalles: detalles };
                
                datos.push(auxDatos);
                detalles = [];
            }
            return datos;
        }

        function FormatoDetallesArrendadora(source, separadores, tipo)
        {
            var datos = [];
            var detalles = [];
            var detalles2 = [];
            var auxseparadores = [];
            for(var i = 1; i <= 14; i++)
            {
                detalles = [];
                detalles2 = [];
                auxseparadores = [];
                switch(i)
                {
                    case 1:
                        detalles =  $.grep(source, function(el,index){ return (el.cuenta.indexOf("4000-") >= 0 && el.cuenta.indexOf("4000-8-") < 0 && el != null); }); 
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
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
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 2:
                        detalles = $.grep(source, function(el,index){ return ((el.cuenta == "1-1-0" || el.cuenta == "1-3-1" || el.cuenta == "1-3-2") && el != null); }); 
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
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
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 3:
                        detalles = $.grep(source, function(el,index){ return ((el.cuenta == "1-2-1" || el.cuenta == "1-2-2" || el.cuenta == "1-2-3") && el != null); });  
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
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
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 4:
                        var totalImporte = 0;
                        for(var j = 0; j < 3; j ++)
                        {
                            if(datos[j].separadores.length > 1)
                            {
                                $.each(datos[j].separadores, function(index, element){
                                    if(index > 0) totalImporte += element || 0;
                                });
                            }
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = 0;
                            for(var k = 0; k < 3; k ++)
                            {
                                if(datos[k].separadores.length > j)
                                {
                                    auxTotalSep += datos[k].separadores[j];
                                }
                            }
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
                        break;
                    case 5:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0 && el != null); });
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto * (-1)) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 6:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5000-10-") >= 0 && el != null); });    
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto * (-1)) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 7:
                        var totalImporte = datos[3].separadores[0];
                        totalImporte -= datos[4].separadores[0];
                        totalImporte -= datos[5].separadores[0];
                        totalImporte -= datos[6].separadores[0];
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = datos[3].separadores[j];
                            for(var k = 4; k <= 6; k ++)
                            {
                                if(datos[k].separadores.length > j)
                                {
                                    auxTotalSep -= datos[k].separadores[j];
                                }
                            }
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
                        break;
                    case 8:
                        detalles = $.grep(source, function(el,index){ return (el.cuenta.indexOf("5280-") >= 0 && el != null); });  
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                        if(detallesTotal.length > 0)
                        {
                            $.each(detallesTotal, function(index, element){
                                totalImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += (element.monto * (-1)) || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 9:
                        var totalImporte = datos[7].separadores[0];
                        totalImporte -= datos[8].separadores[0];
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = datos[7].separadores[j];
                            auxTotalSep -= datos[8].separadores[j];
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
                        break;
                    case 10:
                        detalles = $.grep(source, function(el,index){ return ((el.cuenta.indexOf("4900-") >= 0 || el.cuenta.indexOf("5900-") >= 0) && el != null); });  
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
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
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 11:
                        var totalImporte = datos[9].separadores[0];
                        totalImporte += datos[10].separadores[0];
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = datos[9].separadores[j];
                            auxTotalSep += datos[10].separadores[j];
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
                        break;
                    case 12:
                        detalles = $.grep(source, function(el,index){ return ((el.cuenta.indexOf("4901-") >= 0 || el.cuenta.indexOf("4000-8-") >= 0 || el.cuenta.indexOf("5901-") >= 0) && el != null); });   
                        var totalImporte = 0;
                        var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
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
                                return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                            });
                            var auxseparadoresImporte = 0
                            var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                            if(detallesTotalDetalle.length > 0)
                            {
                                $.each(detallesTotalDetalle, function(index, element){
                                    auxseparadoresImporte += element.monto || 0;
                                });
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                    case 13:
                        var totalImporte = datos[11].separadores[0];
                        totalImporte += datos[12].separadores[0];
                        auxseparadores.push(totalImporte);
                        for(var j = 1; j <= separadores.length; j++)
                        {
                            var auxTotalSep = datos[11].separadores[j];
                            auxTotalSep += datos[12].separadores[j];
                            auxseparadores.push(auxTotalSep);
                        }
                        detalles = [];
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
                                auxseparadoresImporte = (datos[13].separadores[j] / datos[3].separadores[j]) * 100;
                            }
                            auxseparadores.push(auxseparadoresImporte);
                        }
                        break;
                }
                var auxDatos = { descripcion: conceptos[i - 1], separadores: auxseparadores, detalles: detalles };
                datos.push(auxDatos);
                if(i == 6)
                {
                    detalles = [];
                    detalles2 = [];
                    auxseparadores = [];
                    detalles = $.grep(source, function(el,index){ return (el.cuenta == "1-4-0" && el != null); });  
                    var totalImporte = 0;
                    var detallesTotal = $.grep(detalles, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                    if(detallesTotal.length > 0)
                    {
                        $.each(detallesTotal, function(index, element){
                            totalImporte += (element.monto * (-1)) || 0;
                        });
                    }
                    auxseparadores.push(totalImporte);
                    for(var j = 1; j <= separadores.length; j++)
                    {
                        var auxseparadoresImporteCuenta = $.grep(detalles, function (el, index){
                            return tipo == 1 ? (el.division == null ? "" : el.division.trim()) == separadores[j] : (el.areaCuenta == null ? "" : el.areaCuenta.trim()) == separadores[j];
                        });
                        var auxseparadoresImporte = 0
                        var detallesTotalDetalle = $.grep(auxseparadoresImporteCuenta, function(el,index){ return ((el.semana == 1 || (cbConfiguracion.val() == '0' ? el.semana == 6 : false )) && el.tipoMov < 2); }); 
                        if(detallesTotalDetalle.length > 0)
                        {
                            $.each(detallesTotalDetalle, function(index, element){
                                auxseparadoresImporte += (element.monto * (-1)) || 0;
                            });
                        }
                        auxseparadores.push(auxseparadoresImporte);
                    }
                    var auxDatos = { descripcion: conceptos[14], separadores: auxseparadores, detalles: detalles };
                    datos.push(auxDatos);
                }
                detalles = [];
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
                        '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + chkConfiguracion.text().trim() 
                        + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputCorte.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));

                    CargarGraficaKubrix(data, tipo, "", true);

                    //Cargar Costos Estimados
                    //var costosEst = $.grep(data, function( n, i ) { return n.grupoInsumo == "1-4-0";  });
                    //cargarTablaCostosEstimados(costosEst);

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
                                numSemana = 5;
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
                                numSemana = indexCol;
                                auxFecha.setDate(fechaMax.getDate() - (indexCol * 7))
                                break;
                        }
                        //if(auxTipo != 0) {setLstKubrixDetalle(4, auxTipo, indexRow, auxFecha, rowData.descripcion, negativo, divisionFiltroDetalle, areaCuentaFiltroDetalle, "");}
                        //else setLstKubrixDetalle(3, numSemana, indexRow, auxFecha, rowData.descripcion, negativo, divisionFiltroDetalle, areaCuentaFiltroDetalle, "");
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
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 1 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 1, 1);                                    
                                    break;
                                case "2":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO MENOR");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 2 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "3":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO TRANSPORTE CONSTRUPLAN");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 3 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "4":
                                    $("#tituloModal").text("ARRENDADORA - FLETES");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 4 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "5":
                                    $("#tituloModal").text("ARRENDADORA - OTR");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 5 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "6":
                                    $("#tituloModal").text("ARRENDADORA - ADMINISTRATIVO CENTRAL");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 6 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "7":
                                    $("#tituloModal").text("ARRENDADORA - OTROS");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 7 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "8":
                                    $("#tituloModal").text("ARRENDADORA - EQUIPO TRANSPORTE ARRENDADORA");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 8 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                case "9":
                                    $("#tituloModal").text("ARRENDADORA - ADMINISTRATIVO PROYECTOS");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && n.tipo == 9 && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
                                    info = obtenerListaAnalisis(detalles, idTipo);
                                    cargarDatosTablaAnalisis(info, 0, 0);
                                    break;
                                default:
                                    $("#tituloModal").text("ARRENDADORA");
                                    detalles = jQuery.grep(detalles, function( n, i ) { return n != null && (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado)); });
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
                    $("#tablaKubrixDetalle p.guardarLinea").click(function(e){
                        var tipoGuardado = $(this).attr("data-tipoGuardado");
                        AbrirModalCostosEstimados(tipoGuardado);
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( data.tipo_mov == 4 || data.tipo_mov == 9 || data.tipo_mov == 9 || data.tipo_mov == 11 || data.tipo_mov == 13 ||  data.tipo_mov == 15 ||  data.tipo_mov == 16){
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
                                .append('<td class="text-center">' + group + ' (' + rows.count() + ')</td><td class="text-center">' + getRowHTML(ingresos) + '</td><td class="text-center">' + getRowHTML(costos) + '</td><td class="text-center">' + getNumberHTML(resultadoNeto) + '</td><td class="text-center">' + margen.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %" + '</td>')
                                .attr('data-name', group)
                                .toggleClass('collapsed', collapsed);
                        }
                        else
                        {
                            var ingresosContabilizados = data.map(function(x) { return x.ingresosContabilizados; }).reduce(function(total, importe) { return total + importe; });
                            var ingresosconEstimacion = data.map(function(x) { return x.ingresosconEstimacion; }).reduce(function(total, importe) { return total + importe; });
                            var ingresosPendientesGenerar = data.map(function(x) { return x.ingresosPendientesGenerar; }).reduce(function(total, importe) { return total + importe; });
                            var totalIngresos = data.map(function(x) { return x.totalIngresos; }).reduce(function(total, importe) { return total + importe; });

                            var costoTotal = data.map(function(x) { return x.costoTotal; }).reduce(function(total, importe) { return total + importe; });
                            var depreciacion = data.map(function(x) { return x.depreciacion; }).reduce(function(total, importe) { return total + importe; });
                            var costosEstimados = data.map(function(x) { return x.costosEstimados; }).reduce(function(total, importe) { return total + importe; });
                            var utilidadBruta = data.map(function(x) { return x.utilidadBruta; }).reduce(function(total, importe) { return total + importe; });

                            var gastosOperacion = data.map(function(x) { return x.gastosOperacion; }).reduce(function(total, importe) { return total + importe; });
                            var resultadoAntesFinacieros = data.map(function(x) { return x.resultadoAntesFinacieros; }).reduce(function(total, importe) { return total + importe; });

                            var gastosProductosFinancieros = data.map(function(x) { return x.gastosProductosFinancieros; }).reduce(function(total, importe) { return total + importe; });
                            var resultadoFinancieros = data.map(function(x) { return x.resultadoFinancieros; }).reduce(function(total, importe) { return total + importe; });

                            //var otrosIngresos = data.map(function(x) { return x.otrosIngresos; }).reduce(function(total, importe) { return total + importe; });
                            //var resultadoNeto = data.map(function(x) { return x.resultadoNeto; }).reduce(function(total, importe) { return total + importe; });
                            //var margen = totalIngresos != 0 ? (resultadoNeto / totalIngresos) * 100 : 0;
                            var margen = totalIngresos != 0 ? (resultadoFinancieros / totalIngresos) * 100 : 0;
                            var detalles = data.map(function(x) { return x.dealles; });
                            // Add category name to the <tr>. NOTE: Hardcoded colspan
                            return $('<tr/>')
                                .append('<td class="text-center">' + group + ' (' + rows.count() + ')</td>' +
                                '<td class="text-center">' + getRowHTML(ingresosContabilizados) + '</td>' +
                                '<td class="text-center">' + getRowHTML(ingresosconEstimacion) + '</td>' +
                                '<td class="text-center">' + getRowHTML(ingresosPendientesGenerar) + '</td>' +
                                '<td class="text-center">' + getNumberHTML(totalIngresos) + '</td>' +

                                '<td class="text-center">' + getRowHTML(costoTotal) + '</td>' +
                                '<td class="text-center">' + getRowHTML(depreciacion) + '</td>' +
                                '<td class="text-center">' + getRowHTML(costosEstimados) + '</td>' +
                                '<td class="text-center">' + getNumberHTML(utilidadBruta) + '</td>' +

                                '<td class="text-center">' + getRowHTML(gastosOperacion) + '</td>' +
                                '<td class="text-center">' + getNumberHTML(resultadoAntesFinacieros) + '</td>' +

                                '<td class="text-center">' + getRowHTML(gastosProductosFinancieros) + '</td>' +
                                '<td class="text-center">' + getNumberHTML(resultadoFinancieros) + '</td>' +

                                //'<td class="text-center">' + getRowHTML(otrosIngresos) + '</td>' +
                                //'<td class="text-center">' + getNumberHTML(resultadoNeto) + '</td>' +
                                '<td class="text-center">' + margen.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %" + '</td>')
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
                        '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + chkConfiguracion.text().trim() 
                        + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputCorte.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));
                    
                    //Cargar Costos Estimados
                    //var costosEst = $.grep(data, function( n, i ) { return n.grupoInsumo == "1-4-0";  });
                    //cargarTablaCostosEstimados(costosEst);

                    tablaKubrixDetalleEco.find('p.desplegable').unbind().click(function (e) {
                        
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaKubrixDetalleEco.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        var negativo = false;
                        const indexCol = $(this).parents("td").index();
                        const nombreColumna = $('#tablaKubrixDetalleEco thead tr th').eq($(this).parents("td").index()).text().trim();
                        economicoFiltroDetEco = rowData.descripcion;
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
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta.indexOf("4000-") >= 0 || el.cuenta.indexOf("4900-") >= 0 || el.cuenta.indexOf("4901-") >= 0 
                                         || el.cuenta == "1-1-0" || el.cuenta == "1-2-1" || el.cuenta == "1-2-2" || el.cuenta == "1-2-3" || el.cuenta == "1-3-1" || el.cuenta == "1-3-2"); });
                                    break;
                                case 2:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta.indexOf("5000-") >= 0 || el.cuenta.indexOf("5900-") >= 0 || el.cuenta.indexOf("5901-") >= 0
                                        || el.cuenta.indexOf("5280-") >= 0 || el.cuenta == "1-4-0"); });
                                    negativo = true;
                                    break;
                            }
                        }
                        else
                        {
                            switch(indexCol)
                            {
                                case 1:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta.indexOf("4000-") >= 0 && el.cuenta.indexOf("4000-8-") < 0; });
                                    break;
                                case 2:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta == "1-1-0" || el.cuenta == "1-3-1" || el.cuenta == "1-3-2") ; });
                                    break;
                                case 3:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta == "1-2-1" || el.cuenta == "1-2-2" || el.cuenta == "1-2-3") ; });
                                    break;
                                case 5:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0) ; });
                                    negativo = true;
                                    break;
                                case 6:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta.indexOf("5000-10-") >= 0 ; });
                                    negativo = true;
                                    break;
                                case 7:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta == "1-4-0" ; });
                                    negativo = true;
                                    break;
                                case 9:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return el.cuenta.indexOf("5280-") >= 0 ; });
                                    negativo = true;
                                    break;
                                case 11:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta.indexOf("4900-") >= 0 || el.cuenta.indexOf("5900-") >= 0) ; });
                                    break;
                                case 13:
                                    detalles = jQuery.grep(detalles, function( el, i ) { return (el.cuenta.indexOf("4901-") >= 0 || el.cuenta.indexOf("4000-8-") >= 0 || el.cuenta.indexOf("5901-") >= 0) ; });
                                    break;
                            }
                        }
                        //cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, auxFecha, auxTipo, true, numSemana, negativo); 
                        //setLstKubrixDetalle(5, 0, tablaResumida ? (indexCol - 3) : (indexCol - 1), auxFecha, rowData.descripcion, negativo, divisionFiltroDetalle, areaCuentaFiltroDetalle, rowData.descripcion);
                        columnaGlobal = 0;                        
                        renglonGlobal = tablaResumida ? (indexCol - 3) : (indexCol - 1);
                        cargarTablaSubCuenta(detalles, rowData.descripcion, auxFecha, numSemana, negativo); 
                        banderaTablaDetalle = true;
                    });
                    $("#tablaKubrixDetalleEco p.guardarLinea").click(function(e){
                        var tipoGuardado = $(this).attr("data-tipoGuardado");
                        AbrirModalCostosEstimados(tipoGuardado);
                    });
                },
            });
        }
        function getColumnasDetalleEco()
        {
            var obras = botonBuscar.attr("data-obra").split(",");
            if($("#chbDespliegueDetEco").is(":checked"))
            {
                return [                   
                    { data: 'grupoMaquina', title: 'Grupo Maquina', visible: false },
                    { data: 'descripcion', title: 'Concepto', render: function (data, type, row) { 
                        var estatusString = row.centro_costos;
                        var colorString = "";
                        switch(row.estatus)
                        {
                            case "0": colorString = "#ff6600"; break;
                            case "1": colorString = "#009900"; break;
                            case "2": colorString = "#ff6600"; break;
                            default: { colorString = "white"; estatusString = "N/A"; break; }
                        }
                        if(jQuery.inArray(row.centro_costos, obras) == -1 && row.centro_costos != "-1" && botonBuscar.attr("data-obra") != "") { colorString = "#808080"; }
                        return '<span class="dotAcotacion" style="font-size:9px;background-color:' + colorString + ';vertical-align: middle;margin-bottom: 5px;">' + estatusString + '</span>' + '  ' + data; }
                    },
                    { data: 'ingresos', title:  '<span class="tituloKubrixEc">Ingresos</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'costos', title: '<span class="tituloKubrixEc">Costos</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'resultadoNeto', title: '<span class="tituloKubrixEc">Resultado Neto</span>', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'margen', title: '<span class="tituloKubrixEc">% de Margen</span>', render: function (data, type, row) { return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %"; } },
                ];
            }
            else
            {
                return[
                    { data: 'grupoMaquina', title: 'Grupo Maquina', visible: false },
                    { data: 'descripcion', title: 'Concepto', render: function (data, type, row) { 
                        var estatusString = "";
                        var colorString = "";
                        switch(row.estatus)
                        {
                            case "0": colorString = "#ff6600"; break;
                            case "1": colorString = "#009900"; break;
                            case "2": colorString = "#ffcc00"; break;
                            default: colorString = "white"; break;
                        }
                        if(jQuery.inArray(row.centro_costos, obras) == -1 && row.centro_costos != "-1" && botonBuscar.attr("data-obra") != "") { colorString = "#808080"; }
                        return '<span class="dotAcotacion" style="font-size:9px;background-color:' + colorString + ';vertical-align: middle;margin-bottom: 5px;">' + estatusString + '</span>' + '  ' + data; }
                    },
                    { data: 'ingresosContabilizados', title:  '<span class="tituloKubrixEc">Ingresos Contab.</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'ingresosconEstimacion', title: '<span class="tituloKubrixEc">Ingresos con Estimación</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'ingresosPendientesGenerar', title: '<span class="tituloKubrixEc">Ingresos Pendientes Generar</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'totalIngresos', title: '<span class="tituloKubrixEc">Total Ingresos</span>', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'costoTotal', title: '<span class="tituloKubrixEc">Costo Total</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'depreciacion', title: '<span class="tituloKubrixEc">Deprecia- ción</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'costosEstimados', title: '<span class="tituloKubrixEc">Costos Estimados</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'utilidadBruta', title: '<span class="tituloKubrixEc">Utilidad Bruta</span>', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'gastosOperacion', title: '<span class="tituloKubrixEc">Gastos de Operación</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'resultadoAntesFinacieros', title: '<span class="tituloKubrixEc">Resultado Antes de Finacieros</span>', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'gastosProductosFinancieros', title: '<span class="tituloKubrixEc">Efecto Cambiario Neto</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    { data: 'resultadoFinancieros', title: '<span class="tituloKubrixEc">Resultado con Efecto Cambiario</span>', render: function (data, type, row) { return getNumberHTML(data); } },
                    //{ data: 'otrosIngresos', title: '<span class="tituloKubrixEc">Otros Ingresos</span>', render: function (data, type, row) { return getRowHTML(data); } },
                    //{ data: 'resultadoNeto', title: '<span class="tituloKubrixEc">Resultado Neto</span>', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'margen', title: '<span class="tituloKubrixEc">% de Margen</span>', render: function (data, type, row) { return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %"; } },
                  //CAMPOS DATOS
                    { data: 'ARRENDADORA', title: '<span class="tituloKubrixEc">ARRENDADORA</span>', render: function (data, type, row) { return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %"; } },
                    { data: 'resultadoConEfectoSemanal', title: '<span class="tituloKubrixEc">Resultado con Efecto semanal</span>', render: function (data, type, row) { return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + " %"; } },
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
                       '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + chkConfiguracion.text().trim() 
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
                        //cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, fechaMax, 0, false, 1, negativo);
                        divisionFiltroDetalle = nombreColumna;
                        //setLstKubrixDetalle(1, 0, indexRow, fechaMax, rowData.descripcion, negativo, nombreColumna, "", "");
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
                            // AQUI OBTENEMOS EL DATA DE LA TABLA DETALLE
                        const nombreColumna = $(this).text();                        
                        var detallesRaw = rowData.map(function(x) { return x.detalles});
                           //TE HACE UN DETALLE SEGUN LA CANTIDAD DE DETALLES QUE TENGA EL OBJETO SEPARADO
                        var detalles = distinctArrayBy([].concat.apply([], detallesRaw), "id");
                           
                        //detalles = getUnique(detalles);
                        //$.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                        if(nombreColumna != nombreTotal) detalles = $.grep(detalles, function( n, i ) { return n.division == nombreColumna; });
                        areasCuenta = [nombreColumna];
                        auxAreasCuenta = detalles.map(function(element) { return element.areaCuenta.trim(); });
                        auxAreasCuenta = getUnique(auxAreasCuenta);
                        auxAreasCuenta.sort(SortByAreaCuenta);
                        $.merge(areasCuenta, auxAreasCuenta); 
                        areasCuentaDetalle = auxAreasCuenta;
                        recargarTotalizadoresCX();
                        var datos = FormatoDetalles(detalles, areasCuenta, 2,lstDetalle,2,nombreColumna);
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
                        //Cargar Costos Estimados
                        //var costosEst = $.grep(detalles, function( n, i ) { return n.grupoInsumo == "1-4-0";  });
                        //cargarTablaCostosEstimados(costosEst);
                        $.unblockUI();
                    });
                    $("#tablaKubrixDivision p.guardarLinea").click(function(e){
                        var tipoGuardado = $(this).attr("data-tipoGuardado");
                        AbrirModalCostosEstimados(tipoGuardado);
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if( dataIndex == 3 || dataIndex == 8 || dataIndex == 10 || dataIndex == 12 || dataIndex == 14 ||  dataIndex == 15){
                        $(row).addClass('resultado');
                    }
                },
                rowCallback: function( row, data, index ) {
                    if ((index == 0 || index == 4 || index == 5 || index ==7 || index == 8 || index == 9 || index == 10 || index == 11 || index == 12 || index == 14) && cbTipoCorte.val() == 10) {
                        $(row).hide();
                    }
                },
            });
            $('[data-toggle="tooltip"]').tooltip();
        }
        function getUnique(array){
            var u = {}, a = [];
            for(var i = 0, l = array.length; i < l; ++i){
                if(u.hasOwnProperty(array[i])) {
                    continue;
                }
                a.push(array[i]);
                u[array[i]] = 1;
            }
            return a;
        }

        function distinctArrayBy(arr, propName) {
            var result = arr.reduce(function (arr1, e1) {
                var matches = arr1.filter(function (e2) {
                    return e1[propName] == e2[propName];
                })
                if (matches.length == 0)
                    arr1.push(e1)
                return arr1;
            }, []);

            return result;
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
                       '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Ejercicio:</b> ' + chkConfiguracion.text().trim() 
                       + '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Corte a:</b> ' + inputCorte.val().trim() + (cboMaquina.val() == "" ? '' : '&nbsp;&nbsp;<b>/</b>&nbsp;&nbsp;<b>Economico:</b> ' + $("#cboMaquina option:selected").text().trim()));

                    //Cargar Costos Estimados
                    //var costosEst = $.grep(data, function( n, i ) { return n.grupoInsumo == "1-4-0";  });
                    //cargarTablaCostosEstimados(costosEst);

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
                        
                        //cargarTablaSubCuenta(detalles, rowData.descripcion, rowData.importe, nombreColumna, fechaMax, 0, false, 1, negativo);  
                        areaCuentaFiltroDetalle = nombreColumna;
                        //setLstKubrixDetalle(2, 0, indexRow, fechaMax, rowData.descripcion, negativo, divisionFiltroDetalle, nombreColumna, "");
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
                        const nombreColumna = $(this).text();                        
                        var detallesRaw = rowData.map(function(x) { return x.detalles});
                        var detalles = distinctArrayBy([].concat.apply([], detallesRaw), "id");
                        //detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
                        stringAreaCuenta = "TODAS";
                        if(nombreColumna != nombreTotal) {
                            stringAreaCuenta = nombreColumna;
                            detalles = $.grep(detalles, function( n, i ) { return n.areaCuenta != null && n.areaCuenta.trim() == nombreColumna; });
                            areasCuentaDetalle = [nombreColumna];
                            recargarTotalizadoresCX();
                            areaCuentaFiltroDetalle = nombreColumna;
                        }
                        var datos = detalles;
                        var semanas= [];
                        if($("#chbAgrupacionDetalle").is(":checked")) setFormatoKubrixDetalles(e, datos);
                        else setFormatoKubrixDetallesEconomico(e, datos);   
                        $.unblockUI();
                    });
                    $("#tablaKubrixAreaCuenta p.guardarLinea").click(function(e){
                        var tipoGuardado = $(this).attr("data-tipoGuardado");
                        AbrirModalCostosEstimados(tipoGuardado);
                    });
                },
                createdRow: function( row, data, dataIndex){
                    if(dataIndex == 3 || dataIndex == 8 || dataIndex == 10 || dataIndex == 12 || dataIndex == 14 ||  dataIndex == 15){
                        $(row).addClass('resultado');
                    }
                },
            });
            $('[data-toggle="tooltip"]').tooltip();
            //switch tipo reporte
            let tr = $('#tablaKubrixDetalle').find('tr')
            let td = $(tr[16]).find('td');
            
            for (let index = 1; index < td.length; index++) {
                $(td[index]).find('p').removeClass();
            }
        }
        function obtenerListaAnalisis(detalles, idTipo)
        {
            var fechaMax = moment(botonBuscar.attr("data-fechaFin"), "DD-MM-YYYY").toDate();
            var rowData = [];            
            for(var i = 1; i <= 16; i++)
            {                
                var auxRowData = { tipo_mov: 0, descripcion: "", actual: 0, semana2: 0, semana3: 0, semana4: 0, semana5: 0, cfc: 0, cf: 0, mc: 0, pr: 0, tc: 0, car: 0, ex: 0, hdt: 0, otros: 0, detalles: [] }
                var auxDetallesNuevo = [];
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
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData);
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
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData);
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
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            auxDetallesNuevo.push(n);
                        }); 
                        auxRowData.detalles = auxDetallesNuevo;
                        rowData.push(auxRowData);
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
                        rowData.push(auxRowData);
                        break;
                    case 5:                         
                        var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 5; });
                        var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                        auxRowData.descripcion = "Costo Total";
                        auxRowData.tipo_mov = 5;
                        auxRowData.actual = costos.actual * (-1);
                        auxRowData.semana2 = costos.semana2 * (-1);
                        auxRowData.semana3 = costos.semana3 * (-1);
                        auxRowData.semana4 = costos.semana4 * (-1);
                        auxRowData.semana5 = costos.semana5 * (-1);
                        auxRowData.cfc = costos.cfc * (-1);
                        auxRowData.cf = costos.cf * (-1);
                        auxRowData.mc = costos.mc * (-1);
                        auxRowData.pr = costos.pr * (-1);
                        auxRowData.tc = costos.tc * (-1);
                        auxRowData.car = costos.car * (-1);
                        auxRowData.ex = costos.ex * (-1);
                        auxRowData.hdt = costos.hdt * (-1);
                        auxRowData.otros = costos.otros * (-1);
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            n.importe = n.importe * (-1);
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
                        auxRowData.actual = costos.actual * (-1);
                        auxRowData.semana2 = costos.semana2 * (-1);
                        auxRowData.semana3 = costos.semana3 * (-1);
                        auxRowData.semana4 = costos.semana4 * (-1);
                        auxRowData.semana5 = costos.semana5 * (-1);
                        auxRowData.cfc = costos.cfc * (-1);
                        auxRowData.cf = costos.cf * (-1);
                        auxRowData.mc = costos.mc * (-1);
                        auxRowData.pr = costos.pr * (-1);
                        auxRowData.tc = costos.tc * (-1);
                        auxRowData.car = costos.car * (-1);
                        auxRowData.ex = costos.ex * (-1);
                        auxRowData.hdt = costos.hdt * (-1);
                        auxRowData.otros = costos.otros * (-1);
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            n.importe = n.importe * (-1);
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
                        auxRowData2.actual = costos2.actual * (-1);
                        auxRowData2.semana2 = costos2.semana2 * (-1);
                        auxRowData2.semana3 = costos2.semana3 * (-1);
                        auxRowData2.semana4 = costos2.semana4 * (-1);
                        auxRowData2.semana5 = costos2.semana5 * (-1);
                        auxRowData2.cfc = costos2.cfc * (-1);
                        auxRowData2.cf = costos2.cf * (-1);
                        auxRowData2.mc = costos2.mc * (-1);
                        auxRowData2.pr = costos2.pr * (-1);
                        auxRowData2.tc = costos2.tc * (-1);
                        auxRowData2.car = costos2.car * (-1);
                        auxRowData2.ex = costos2.ex * (-1);
                        auxRowData2.hdt = costos2.hdt * (-1);
                        auxRowData2.otros = costos2.otros * (-1);                        
                        $.each(auxDetalles2, function(i, n) { 
                            var item = n;
                            n.importe = n.importe * (-1);
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
                        auxRowData.actual = costos.actual * (-1);
                        auxRowData.semana2 = costos.semana2 * (-1);
                        auxRowData.semana3 = costos.semana3 * (-1);
                        auxRowData.semana4 = costos.semana4 * (-1);
                        auxRowData.semana5 = costos.semana5 * (-1);
                        auxRowData.cfc = costos.cfc * (-1);
                        auxRowData.cf = costos.cf * (-1);
                        auxRowData.mc = costos.mc * (-1);
                        auxRowData.pr = costos.pr * (-1);
                        auxRowData.tc = costos.tc * (-1);
                        auxRowData.car = costos.car * (-1);
                        auxRowData.ex = costos.ex * (-1);
                        auxRowData.hdt = costos.hdt * (-1);
                        auxRowData.otros = costos.otros * (-1);
                        $.each(auxDetalles, function(i, n) { 
                            var item = n;
                            n.importe = n.importe * (-1);
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
          //#region CODIGO COMENTADO
                    //case 12:                         
                    //    var auxDetalles = jQuery.grep(detalles, function(n, index) { return n != null && n.tipo_mov == 12; });
                    //    var costos  = AsignarImportesAnalisis(auxDetalles, fechaMax);
                    //    auxRowData.descripcion = "Otros Ingresos"; 
                    //    auxRowData.tipo_mov = 12;
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
                    //case 13: 
                    //    auxRowData.descripcion = "Resultado Neto"; 
                    //    auxRowData.tipo_mov = 13;
                    //    auxRowData.actual = rowData[11].actual + rowData[12].actual;
                    //    auxRowData.semana2 = rowData[11].semana2 + rowData[12].semana2;
                    //    auxRowData.semana3 = rowData[11].semana3 + rowData[12].semana3;
                    //    auxRowData.semana4 = rowData[11].semana4 + rowData[12].semana4;
                    //    auxRowData.semana5 = rowData[11].semana5 + rowData[12].semana5;
                    //    auxRowData.cfc = rowData[11].cfc + rowData[12].cfc;
                    //    auxRowData.cf = rowData[11].cf + rowData[12].cf;
                    //    auxRowData.mc = rowData[11].mc + rowData[12].mc;
                    //    auxRowData.pr = rowData[11].pr + rowData[12].pr;
                    //    auxRowData.tc = rowData[11].tc + rowData[12].tc;
                    //    auxRowData.car = rowData[11].car + rowData[12].car;
                    //    auxRowData.ex = rowData[11].ex + rowData[12].ex;
                    //    auxRowData.hdt = rowData[11].hdt + rowData[12].hdt;
                    //    auxRowData.otros = rowData[11].otros + rowData[12].otros;
                    //    rowData.push(auxRowData);
                    //    break;
                    //case 14: 
                    //    auxRowData.descripcion = "% de Margen"; 
                    //    auxRowData.tipo_mov = 14;
                    //    auxRowData.actual = rowData[3].actual != 0 ? rowData[13].actual / rowData[3].actual * (100) : 0;
                    //    auxRowData.semana2 = rowData[3].semana2 != 0 ? rowData[13].semana2 / rowData[3].semana2 * (100) : 0;
                    //    auxRowData.semana3 = rowData[3].semana3 != 0 ? rowData[13].semana3 / rowData[3].semana3 * (100) : 0;
                    //    auxRowData.semana4 = rowData[3].semana4 != 0 ? rowData[13].semana4 / rowData[3].semana4 * (100) : 0;
                    //    auxRowData.semana5 = rowData[3].semana5 != 0 ? rowData[13].semana5 / rowData[3].semana5 * (100) : 0;
                    //    auxRowData.cfc = rowData[3].cfc != 0 ? rowData[13].cfc / rowData[3].cfc * (100) : 0;
                    //    auxRowData.cf = rowData[3].cf != 0 ? rowData[13].cf / rowData[3].cf * (100) : 0;
                    //    auxRowData.mc = rowData[3].mc != 0 ? rowData[13].mc / rowData[3].mc * (100) : 0;
                    //    auxRowData.pr = rowData[3].pr != 0 ? rowData[13].pr / rowData[3].pr * (100) : 0;
                    //    auxRowData.tc = rowData[3].tc != 0 ? rowData[13].tc / rowData[3].tc * (100) : 0;
                    //    auxRowData.car = rowData[3].car != 0 ? rowData[13].car / rowData[3].car * (100) : 0;
                    //    auxRowData.ex = rowData[3].ex != 0 ? rowData[13].ex / rowData[3].ex * (100) : 0;
                    //    auxRowData.hdt = rowData[3].hdt != 0 ? rowData[13].hdt / rowData[3].hdt * (100) : 0;
                    //    auxRowData.otros = rowData[3].otros != 0 ? rowData[13].otros / rowData[3].otros * (100) : 0;
                    //    rowData.push(auxRowData);
                    //    break;
          //#endregion
                    case 14: 
                        auxRowData.descripcion = "% de Margen"; 
                        auxRowData.tipo_mov = 14;
                        auxRowData.actual = rowData[3].actual != 0 ? rowData[11].actual / rowData[3].actual * (100) : 0;
                        auxRowData.semana2 = rowData[3].semana2 != 0 ? rowData[11].semana2 / rowData[3].semana2 * (100) : 0;
                        auxRowData.semana3 = rowData[3].semana3 != 0 ? rowData[11].semana3 / rowData[3].semana3 * (100) : 0;
                        auxRowData.semana4 = rowData[3].semana4 != 0 ? rowData[11].semana4 / rowData[3].semana4 * (100) : 0;
                        auxRowData.semana5 = rowData[3].semana5 != 0 ? rowData[11].semana5 / rowData[3].semana5 * (100) : 0;
                        auxRowData.cfc = rowData[3].cfc != 0 ? rowData[11].cfc / rowData[3].cfc * (100) : 0;
                        auxRowData.cf = rowData[3].cf != 0 ? rowData[11].cf / rowData[3].cf * (100) : 0;
                        auxRowData.mc = rowData[3].mc != 0 ? rowData[11].mc / rowData[3].mc * (100) : 0;
                        auxRowData.pr = rowData[3].pr != 0 ? rowData[11].pr / rowData[3].pr * (100) : 0;
                        auxRowData.tc = rowData[3].tc != 0 ? rowData[11].tc / rowData[3].tc * (100) : 0;
                        auxRowData.car = rowData[3].car != 0 ? rowData[11].car / rowData[3].car * (100) : 0;
                        auxRowData.ex = rowData[3].ex != 0 ? rowData[11].ex / rowData[3].ex * (100) : 0;
                        auxRowData.hdt = rowData[3].hdt != 0 ? rowData[11].hdt / rowData[3].hdt * (100) : 0;
                        auxRowData.otros = rowData[3].otros != 0 ? rowData[11].otros / rowData[3].otros * (100) : 0;
                        rowData.push(auxRowData);
                        break;
                    default: auxRowData.descripcion = ""; break;
                    //SWITNU
                    case 15:
                        auxRowData.descripcion = "ARRENDADORA"; 
                        auxRowData.tipo_mov = 15;
                        rowData.push(auxRowData);
         
                        break;
                    case 16:
                        auxRowData.descripcion = "Resultado con Efecto semanal"; 
                        auxRowData.tipo_mov = 16;
                        rowData.push(auxRowData);
                    break;
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
                        //if(index == 1 || index == 2 || index == 3 || index == 4 || index == 7 || index == 9 || index == 10 || 
                        //    index == 11 || index == 12 || index == 13){ importeFinal = importeFinal * (-1); }
                        if (index == 4 || index == 9 || index == 11 || index == 13 || index == 15 || index == 16)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15|| row.tipo_mov == 16)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15|| row.tipo_mov == 16)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15|| row.tipo_mov == 16)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15|| row.tipo_mov == 16)
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
                                if (row.tipo_mov == 4 || row.tipo_mov == 9 || row.tipo_mov == 11 || row.tipo_mov == 13 || row.tipo_mov == 15|| row.tipo_mov == 16)
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
        //INIT TABLA
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
                    { data: 'scta', title: 'Cuenta' , render: function(data, type, row) 
                    { 
                        //IFRENDER
                        if (type === 'display') {
                            return row.id;
                        } else {
                            return data;
                        }

                        // let html =`${data}`;
                        // return  html;
                    } 
                    },
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
                order: [[1, 'asc']],
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
                                    return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado));
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
                    { data: 'sscta', title: 'Cuenta' , render: function(data, type, row) 
                    { 
                        //IFRENDER
                            if (type === 'display') {
                                return row.id;
                            } else {
                                return data;
                            }
                        } 
                    },
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
                                    return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado));
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
                                if(empresaActual == 2){
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
                                    var fecha = $(this).attr("data-fecha");
                                    var tipo = $(this).attr("data-tipo");
                                    var semanal = $(this).attr("data-semanal") == "1";
                                    var exito = setLstKubrixTablaDet(3, columnaGlobal, renglonGlobal, rowData.fecha, "", false , "", "", "", semanal)
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

                        //divisionFiltro = rowData.descripcion;
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
                                    return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado));
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
                                    return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado));
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
                                    return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado));
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
                                    return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado));
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
                    { data: 'fecha', title: 'Fecha Póliza' , render: function(data, type, row) 
                    { 
                        //IFRENDER
	                        if (type === 'display') {
	                            return data;
	                        } else {
	                            return data.toString('YYYYMMdd');
	                        }
	                    } 
                    },
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
        //AQUI DEBO
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
                const importeDetalles = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeDetallesSemanal = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                const importeDetallesCancelados = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
                importeDetallesSemanal = $.merge(importeDetallesSemanal, importeDetallesCancelados);
                //semanal
                if(importeDetalles.length > 0){
                    var semanal = 0;
                    importeDetallesSemanal.length > 0 ? $.each(importeDetallesSemanal, function(i, n) { semanal += n * (negativo ? -1 : 1); }) : 0;
                    var importe = 0;
                    $.each(importeDetalles, function(i, n) { importe += n * (negativo ? -1 : 1); });
                    const grupo = { grafica: grafica, cta: cta, scta: parseInt(scta), id: id, descripcion: descripcion, importe: importe, semanal: semanal, detalles: value, fecha: fecha, empresa: empresa };
                    dtTablaSubCuenta.row.add(grupo);
                }
            });
            const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
            var importeTotalizador = 0;
            $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
            var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
            var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
            $.merge(importeTotalizadorDetallesSemanal,auxImporteTotalDetSemanal);
            var importeTotalizadorSemanal = 0;
            $.each(importeTotalizadorDetallesSemanal, function(i, n) { importeTotalizadorSemanal += n * (negativo ? -1 : 1); });    
            const totalizador = { grafica: grafica ,cta: detalles[0].cuenta.split('-')[0], scta: 0, id: "-1", descripcion: "TOTAL", semanal: importeTotalizadorSemanal, importe: importeTotalizador, detalles: detalles, empresa: detalles[0].empresa }
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
                    const importeDetalles = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
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
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
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
                    const importeDetalles = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
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
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
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
                    const importeDetalles = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
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
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
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
                    const importeDetalles = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
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
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
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
                    const importeDetalles = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                    var importeDetallesSemanal = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 1; }).map(function(x) { return x.monto; });
                    const importeDetallesCancelados = $.grep(value, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov === 2; }).map(function(x) { return x.monto; });
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
                const importeTotalizadorDetalles = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov < 2; }).map(function(x) { return x.monto; });
                var importeTotalizador = 0;
                $.each(importeTotalizadorDetalles, function(i, n) { importeTotalizador += n * (negativo ? -1 : 1); });     
                var importeTotalizadorDetallesSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 1; }).map(function(x) { return x.monto; });
                var auxImporteTotalDetSemanal = $.grep(detalles, function(n, i){ return (chkConfiguracion.is(":checked") ? (n.semana == numSemana || n.semana == 6) : (n.semana == numSemana && !n.acumulado)) && n.tipoMov == 2; }).map(function(x) { return x.monto; });
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
                    if( data.tipo_mov == 4 || data.tipo_mov == 8 || data.tipo_mov == 10 || data.tipo_mov == 12 || data.tipo_mov == 14 || data.tipo_mov == 15) { $(row).addClass('resultado'); }
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
            if(serie.indexOf("<") != -1) $("#lbGraficaKubrix").text($(serie).text());
            else $("#lbGraficaKubrix").text(serie);
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
                    xAxis: { categories: ['Equipo Mayor', 'Equipo Menor', 'Equipo Transporte Construplan', 'Equipo Transporte Arrendadora', 'Fletes', 'OTR', 'Administrativo Central', 'Administrativo Proyectos', 'Otros'] },
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

            comboAC.next(".select2-container").css("display", "none");
            var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
            if(seleccionados.length == 0) $("#spanComboAC").text("TODOS");
            else {
                if (seleccionados.length  == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
            }   
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

            listaRentabilidad1 = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("4000-") >= 0 && el.cuenta.indexOf("4000-8-") < 0 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });
            listaRentabilidad2 = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta == "1-1-0" || el.cuenta == "1-3-1" || el.cuenta == "1-3-2") && (el.semana == 1 || el.semana == 6) && el.tipoMov < 2; });
            listaRentabilidad3 = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta == "1-2-1" || el.cuenta == "1-2-2" || el.cuenta == "1-2-3") && (el.semana == 1 || el.semana == 6) && el.tipoMov < 2;});
            listaRentabilidad4 = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });
            listaRentabilidad5 = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5000-10") >= 0 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });
            listaRentabilidad6 = $.grep(listaRentabilidad,function(el,index){ return el.cuenta == "1-4-0" && (el.semana == 1 || el.semana == 6) && el.tipoMov < 2; });
            listaRentabilidad7 = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5280-") >= 0 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });
            listaRentabilidad8 = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("4900-") >= 0 || el.cuenta.indexOf("5900-") >= 0) && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });
            listaRentabilidad9 = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("4901-") >= 0 || el.cuenta.indexOf("4000-8-") >= 0 || el.cuenta.indexOf("5901-") >= 0) && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });

            var economicos = $.grep(listaRentabilidad,function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); }).map(function(element) { return element.cc; });
            economicos = unique(economicos).sort();
            var maquinasEstatus = SetMaquinasEstatus(economicos);
            var gruposMaquina = SetGrupoMaquinas();

            var estatus = maquinasEstatus.find(function (el) { return el.Value == economicos[i]; });

            for(var i = 0; i < economicos.length; i++)
            {
                var detalles = $.grep(listaRentabilidad,function(el,index){ return el.cc == economicos[i]; });
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
                    descripcion: economicos[i], ingresosContabilizados: 0, ingresosconEstimacion: 0, ingresosPendientesGenerar: 0, totalIngresos: 0, 
                    costoTotal: 0, depreciacion: 0, costosEstimados: 0, utilidadBruta: 0, gastosOperacion: 0, resultadoAntesFinacieros: 0, gastosProductosFinancieros: 0, 
                    resultadoFinancieros: 0, otrosIngresos: 0, resultadoNeto: 0, margen: 0, detalles: detalles, grupoMaquina: stringGrupo, estatus: stringEstatus, centro_costos: stringCC,
                };

                var ingresosContabilizados = $.grep(listaRentabilidad1, function(el,index){ return el.cc == economicos[i]; });
                if (ingresosContabilizados.length > 0) { $.each(ingresosContabilizados, function(index, element){ item.ingresosContabilizados += (element.monto) || 0; }); }
                var ingresosconEstimacion = $.grep(listaRentabilidad2, function(el,index){ return el.cc == economicos[i]; });
                if (ingresosconEstimacion.length > 0) { $.each(ingresosconEstimacion, function(index, element){ item.ingresosconEstimacion += (element.monto) || 0; }); }
                var ingresosPendientesGenerar = $.grep(listaRentabilidad3, function(el,index){ return el.cc == economicos[i]; });
                if (ingresosPendientesGenerar.length > 0) { $.each(ingresosPendientesGenerar, function(index, element){ item.ingresosPendientesGenerar += (element.monto) || 0; }); }
                item.totalIngresos = item.ingresosContabilizados + item.ingresosconEstimacion + item.ingresosPendientesGenerar;
                var costoTotal = $.grep(listaRentabilidad4, function(el,index){ return el.cc == economicos[i]; });
                if (costoTotal.length > 0) { $.each(costoTotal, function(index, element){ item.costoTotal += (element.monto * (-1)) || 0; }); }
                var depreciacion = $.grep(listaRentabilidad5, function(el,index){ return el.cc == economicos[i]; });
                if (depreciacion.length > 0) { $.each(depreciacion, function(index, element){ item.depreciacion += (element.monto * (-1)) || 0; }); }
                var costosEstimados = $.grep(listaRentabilidad6, function(el,index){ return el.cc == economicos[i]; });
                if (costosEstimados.length > 0) { $.each(costosEstimados, function(index, element){ item.costosEstimados += (element.monto * (-1)) || 0; }); }
                item.utilidadBruta = item.totalIngresos - item.costoTotal - item.depreciacion - item.costosEstimados;
                var gastosOperacion = $.grep(listaRentabilidad7, function(el,index){ return el.cc == economicos[i]; });
                if (gastosOperacion.length > 0) { $.each(gastosOperacion, function(index, element){ item.gastosOperacion += (element.monto * (-1)) || 0; }); }
                item.resultadoAntesFinacieros = item.utilidadBruta - item.gastosOperacion;
                var gastosProductosFinancieros = $.grep(listaRentabilidad8, function(el,index){ return el.cc == economicos[i]; });
                if (gastosProductosFinancieros.length > 0) { $.each(gastosProductosFinancieros, function(index, element){ item.gastosProductosFinancieros += (element.monto) || 0; }); }
                item.resultadoFinancieros = item.resultadoAntesFinacieros + item.gastosProductosFinancieros;
                var otrosIngresos = $.grep(listaRentabilidad9, function(el,index){ return el.cc == economicos[i]; });
                if (otrosIngresos.length > 0) { $.each(otrosIngresos, function(index, element){ item.otrosIngresos += (element.monto) || 0; }); }
                item.resultadoNeto = item.resultadoFinancieros + item.otrosIngresos;
                item.margen = item.totalIngresos != 0 ? (item.resultadoNeto / item.totalIngresos) * 100 : 0;
                listaUtilidad.push(item);
            }
            listaUtilidad = listaUtilidad.sort((a, b) => (a.descripcion < b.descripcion) ? 1 : -1);
            return listaUtilidad.sort((a, b) => (a.grupoMaquina > b.grupoMaquina) ? 1 : -1);
        }
        function obtenerListaKubrixEcoCompacto(listaRentabilidad, busq)
        {
            var listaUtilidad = [];
            listaRentabilidad = $.grep(listaRentabilidad,function(el,index){ return el != null; });
            listaRentabilidadIngresos = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("4000-") >= 0 || el.cuenta.indexOf("4900-") >= 0 || el.cuenta.indexOf("4901-") >= 0 ) 
                && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });
            listaRentabilidadIngresosEstimados = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta == "1-1-0" || el.cuenta == "1-2-1" || el.cuenta == "1-2-2" || el.cuenta == "1-2-3" || el.cuenta == "1-3-1" || el.cuenta == "1-3-2") 
                && (el.semana == 1 || el.semana == 6) && el.tipoMov < 2; });
            
            listaRentabilidadIngresos = $.merge(listaRentabilidadIngresos, listaRentabilidadIngresosEstimados);

            listaRentabilidadCostos = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("5000-") >= 0 || el.cuenta.indexOf("5900-") >= 0 || el.cuenta.indexOf("5901-") >= 0
                || el.cuenta.indexOf("5280-") >= 0) && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)) && el.tipoMov < 2; });
            listaRentabilidadCostosEstimados = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta == "1-4-0") 
                && (el.semana == 1 || el.semana == 6) && el.tipoMov < 2; });
            
            listaRentabilidadCostos = $.merge(listaRentabilidadCostos, listaRentabilidadCostosEstimados);

            var economicos = $.grep(listaRentabilidad,function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); }).map(function(element) { return element.cc; });
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
                    descripcion: economicos[i], ingresos: 0, costos: 0, resultadoNeto: 0, margen: 0, detalles: detalles, grupoMaquina: stringGrupo, estatus: stringEstatus, centro_costos: stringCC
                };

                var ingresos = $.grep(listaRentabilidadIngresos, function(el,index){ return el.cc == economicos[i]; });
                if (ingresos.length > 0) { $.each(ingresos, function(index, element){ item.ingresos += (element.monto) || 0; }); }
                
                var costos = $.grep(listaRentabilidadCostos, function(el,index){ return el.cc == economicos[i]; });
                if (costos.length > 0) { $.each(costos, function(index, element){ item.costos += (element.monto * (-1)) || 0; }); }
                
                item.resultadoNeto = item.ingresos - item.costos;
                item.margen = item.ingresos != 0 ? (item.resultadoNeto / item.ingresos) * 100 : 0;
                listaUtilidad.push(item);
            }
            listaUtilidad = listaUtilidad.sort((a, b) => (a.descripcion < b.descripcion) ? 1 : -1);
            return listaUtilidad.sort((a, b) => (a.grupoMaquina > b.grupoMaquina) ? 1 : -1);
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
        function obtenerListaKubrix(listaRentabilidad, busq,lstDetalle)
        {
            var listaUtilidad = [];
            //--Ingresos Contabilizados--//
            var elemento = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("4000-") >= 0 && el.cuenta.indexOf("4000-8-") < 0 ; });
            var utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Ingresos Contabilizados", 1, false);
            listaUtilidad.push(utilidad);
            //--Ingresos con Estimación--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta == "1-1-0" || el.cuenta == "1-3-1" || el.cuenta == "1-3-2"); });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "<p class='guardarLinea' data-tipoGuardado='1'>Ingresos con Estimación</p>", 2, false);
            listaUtilidad.push(utilidad);
            //--Ingresos Pendientes por Generar--//
            elemento =  $.grep(listaRentabilidad,function(el,index){ return(el.cuenta == "1-2-1" || el.cuenta == "1-2-2" || el.cuenta == "1-2-3"); });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "<p class='guardarLinea' data-tipoGuardado='2'>Ingresos Pendientes por Generar</p>", 3, false);
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
            elemento = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("5000-") >= 0 && el.cuenta.indexOf("5000-10-") < 0); });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Costo Total", 5, true);
            listaUtilidad.push(utilidad);
            //--Depreciación--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5000-10") >= 0; }); 
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Depreciación", 6, true);
            listaUtilidad.push(utilidad);
            //--Financieros--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("5900-") >= 0 && el.cuenta.indexOf("5900-1-") < 0); });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Financieros", 7, true);
            listaUtilidad.push(utilidad);
            //--Costo Estimado--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cuenta == "1-4-0"; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "<p class='guardarLinea' data-tipoGuardado='3'>Costo Estimado</p>", 8, true);
            listaUtilidad.push(utilidad);
            //--Utilidad Bruta--//           
            utilidad = 
            {
                tipo_mov: 9,
                descripcion: "Resultado de Operación",
                mayor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].mayor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].mayor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].mayor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].mayor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].mayor,
                menor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].menor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].menor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].menor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].menor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].menor,
                transporteConstruplan: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].transporteConstruplan - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].transporteConstruplan - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].transporteConstruplan - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].transporteConstruplan - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].transporteConstruplan,
                transporteArrendadora: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].transporteArrendadora - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].transporteArrendadora - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].transporteArrendadora - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].transporteArrendadora - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].transporteArrendadora,
                administrativoCentral: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].administrativoCentral - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].administrativoCentral - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].administrativoCentral - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].administrativoCentral - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].administrativoCentral,
                administrativoProyectos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].administrativoProyectos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].administrativoProyectos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].administrativoProyectos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].administrativoProyectos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].administrativoProyectos,
                fletes: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].fletes - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].fletes - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].fletes - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].fletes - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].fletes,
                neumaticos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].neumaticos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].neumaticos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].neumaticos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].neumaticos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].neumaticos,
                otros: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].otros - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].otros - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].otros - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].otros - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].otros,
                total: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].total - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].total - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].total - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].total - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].total,
                actual: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].actual - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].actual - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].actual - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].actual - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].actual,
                semana2: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].semana2 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].semana2 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].semana2 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].semana2 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].semana2,
                semana3: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].semana3 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].semana3 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].semana3 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].semana3 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].semana3,
                semana4: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].semana4 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].semana4 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].semana4 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].semana4 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].semana4,
                semana5: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 4; })[0].semana5 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 5; })[0].semana5 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 6; })[0].semana5 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 7; })[0].semana5 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 8; })[0].semana5,
            };
            listaUtilidad.push(utilidad);
            //--Gastos de Operación--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return el.cuenta.indexOf("5280-") >= 0; });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Gastos de Operación", 10, true);
            listaUtilidad.push(utilidad);
            //--Gastos Antes de Finacieros--//           
            utilidad =
            {
                tipo_mov: 11,
                descripcion: "Resultado Antes de Efecto Cambiario Neto",
                mayor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].mayor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].mayor,
                menor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].menor - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].menor,
                transporteConstruplan: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].transporteConstruplan - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].transporteConstruplan,
                transporteArrendadora: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].transporteArrendadora - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].transporteArrendadora,
                administrativoCentral: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].administrativoCentral - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].administrativoCentral,
                administrativoProyectos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].administrativoProyectos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].administrativoProyectos,
                fletes: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].fletes - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].fletes,
                neumaticos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].neumaticos - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].neumaticos,
                otros: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].otros - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].otros,
                total: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].total - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].total,
                actual: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].actual - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].actual,
                semana2: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].semana2 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].semana2,
                semana3: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].semana3 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].semana3,
                semana4: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].semana4 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].semana4,
                semana5: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 9; })[0].semana5 - $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 10; })[0].semana5,
    
            };
            listaUtilidad.push(utilidad);
            //--Gastos de Financieros--//
            elemento = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("4900-1-") >= 0 || el.cuenta.indexOf("5900-1-") >= 0); });
            utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Efecto Cambiario Neto", 12, false);
            listaUtilidad.push(utilidad);
            //--Resultado con Financieros--//           
            utilidad =
            {
                tipo_mov: 13,
                descripcion: "Resultado con Efecto Cambiario",
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
            listaUtilidad.push(utilidad);
            //--Otros Ingresos--//
            //elemento = $.grep(listaRentabilidad,function(el,index){ return (el.cuenta.indexOf("4901-") >= 0 || el.cuenta.indexOf("4000-8") >= 0 || el.cuenta.indexOf("5901-") >= 0); });
            //utilidad = AsignarImportesKubrix(elemento, busq.fechaFin, "Otros Ingresos", 14, false);
            //listaUtilidad.push(utilidad);
            ////--Resultado Neto--//           
            //utilidad =
            //{
            //    tipo_mov: 15,
            //    descripcion: "Resultado Neto",
            //    mayor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].mayor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].mayor,
            //    menor: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].menor + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].menor,
            //    transporteConstruplan: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].transporteConstruplan + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].transporteConstruplan,
            //    transporteArrendadora: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].transporteArrendadora + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].transporteArrendadora,
            //    administrativoCentral: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].administrativoCentral + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].administrativoCentral,
            //    administrativoProyectos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].administrativoProyectos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].administrativoProyectos,
            //    fletes: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].fletes + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].fletes,
            //    neumaticos: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].neumaticos + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].neumaticos,
            //    otros: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].otros + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].otros,
            //    total: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].total + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].total,
            //    actual: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].actual + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].actual,
            //    semana2: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].semana2 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].semana2,
            //    semana3: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].semana3 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].semana3,
            //    semana4: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].semana4 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].semana4,
            //    semana5: $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 13; })[0].semana5 + $.grep(listaUtilidad, function(el,index){ return el.tipo_mov == 14; })[0].semana5,
            //};
            var auxNeto = utilidad;
            //listaUtilidad.push(utilidad);
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
            
            var uti =0;
            var semana1 = 0;
            lstDetalle.forEach(index=>{ 
                if (index.semana == 1) {
                    semana1 += index.monto;
                }
             });
            var semana2 = 0;
             lstDetalle.forEach(index=>{ 
                if (index.semana == 2) {
                    semana2 += index.monto;
                }
             });
            var semana3 = 0;
             lstDetalle.forEach(index=>{ 
                if (index.semana == 3) {
                    semana3 += index.monto;
                }
             });
            var semana4 = 0;
             lstDetalle.forEach(index=>{ 
                if (index.semana == 4) {
                    semana4 += index.monto;
                }
             });
            var semana5 = 0;
             lstDetalle.forEach(index=>{ 
                if (index.semana == 5) {
                    semana5 += index.monto;
                }
             });

            var ARRENDADORA =
            {
                tipo_mov: 15,
                actual: semana1 != 0 ? (semana1 * -1) : 0,
                semana2: semana2 != 0 ? (semana2 * -1) : 0,
                semana3: semana3 != 0 ? (semana3 * -1) : 0,
                semana4: semana4 != 0 ? (semana4 * -1) : 0,
                semana5: semana5 != 0 ? (semana5 * -1) : 0,
                descripcion: "ARRENDADORA",
            }
            listaUtilidad.push(ARRENDADORA);
            //CAMPO
            detalles = listaRentabilidad;  
            var totalImporte1 = 0;
            var detallesTotal1 = $.grep(detalles, function(n,index){ return (chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 1) : (n.semana == 1 && !n.acumulado)) && n.tipoMov > 0;  });
            $.each(detallesTotal1, function(index, element){
                totalImporte1 += (element.monto);
            });

            var totalImporte2 = 0;
            var detallesTotal2 = $.grep(detalles, function(n,index){ return (chkConfiguracion.is(":checked") ? (n.semana == 2 || n.semana == 2) : (n.semana == 2 && !n.acumulado)) && n.tipoMov > 0;  });
            $.each(detallesTotal2, function(index, element){
                totalImporte2 += (element.monto);
            });

            var totalImporte3 = 0;
            var detallesTotal3 = $.grep(detalles, function(n,index){ return (chkConfiguracion.is(":checked") ? (n.semana == 3 || n.semana == 3) : (n.semana == 3 && !n.acumulado)) && n.tipoMov > 0;  });
            $.each(detallesTotal3, function(index, element){
                totalImporte3 += (element.monto);
            });

            var totalImporte4 = 0;
            var detallesTotal4 = $.grep(detalles, function(n,index){ return (chkConfiguracion.is(":checked") ? (n.semana == 4 || n.semana == 4) : (n.semana == 4 && !n.acumulado)) && n.tipoMov > 0;  });
            $.each(detallesTotal4, function(index, element){
                totalImporte4 += (element.monto);
            });

            var totalImporte5 = 0;
            var detallesTotal5 = $.grep(detalles, function(n,index){ return (chkConfiguracion.is(":checked") ? (n.semana == 5 || n.semana == 5) : (n.semana == 5 && !n.acumulado)) && n.tipoMov > 0;  });
            $.each(detallesTotal5, function(index, element){
                totalImporte5 += (element.monto);
            });



            var ResultadoSemanal =
            {
                tipo_mov: 16,
                actual: totalImporte1 != 0 ? (totalImporte1) : 0,
                semana2: totalImporte2 != 0 ? (totalImporte2) : 0,
                semana3: totalImporte3 != 0 ? (totalImporte3) : 0,
                semana4: totalImporte4 != 0 ? (totalImporte4) : 0,
                semana5: totalImporte5 != 0 ? (totalImporte5) : 0,
                descripcion: "Resultado con Efecto semanal",
            }
            listaUtilidad.push(ResultadoSemanal);


            return listaUtilidad;
        }
        function AsignarImportesKubrix(importes, fecha, descripcion, tipoMov, negativo)
        {
            var costo = { 
                descripcion: descripcion, tipo_mov: tipoMov, mayor: 0, menor: 0,transporteConstruplan: 0, transporteArrendadora: 0,
                administrativoCentral: 0, administrativoProyectos: 0, fletes: 0, neumaticos: 0, otros: 0, total: 0, actual: 0,
                semana2: 0, semana3: 0, semana4: 0, semana5: 0, detalles: importes
            };
            var auxImportes = $.grep(importes,function(el,index){ return el.tipoMov < 2; });

            var mayor = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 1 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (mayor.length > 0) { $.each(mayor, function(index, element){ costo.mayor += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var menor = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 2 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (menor.length > 0) { $.each(menor, function(index, element){ costo.menor += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var transporteConstruplan = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 3 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (transporteConstruplan.length > 0) { $.each(transporteConstruplan, function(index, element){ costo.transporteConstruplan += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var transporteArrendadora = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 8 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (transporteArrendadora.length > 0) { $.each(transporteArrendadora, function(index, element){ costo.transporteArrendadora += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var administrativoCentral = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 6 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (administrativoCentral.length > 0) { $.each(administrativoCentral, function(index, element){ costo.administrativoCentral += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var administrativoProyectos = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 9 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (administrativoProyectos.length > 0) { $.each(administrativoProyectos, function(index, element){ costo.administrativoProyectos += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var fletes = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 4 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (fletes.length > 0) { $.each(fletes, function(index, element){ costo.fletes += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var neumaticos = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 5 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (neumaticos.length > 0) { $.each(neumaticos, function(index, element){ costo.neumaticos += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var otros = $.grep(auxImportes,function(el,index){ return el.tipoEquipo == 7 && (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (otros.length > 0) { $.each(otros, function(index, element){ costo.otros += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
            var total = $.grep(auxImportes,function(el,index){ return (chkConfiguracion.is(":checked") ? (el.semana == 1 || el.semana == 6) : (el.semana == 1 && !el.acumulado)); });
            if (total.length > 0) { $.each(total, function(index, element){ costo.total += (element.monto * (negativo ? (-1) : 1)) || 0; }); }
                    
            //Semanal
            var auxFecha = new Date(fecha);
            var actual = jQuery.grep(auxImportes, function( n, i ) { return chkConfiguracion.is(":checked") ? (n.semana == 1 || n.semana == 6) : (n.semana == 1 && !n.acumulado); });
            if (actual.length > 0) { $.each(actual, function(index, element){ costo.actual += (element.monto * (negativo ? (-1) : 1)) || 0; });}
            var semana2 = jQuery.grep(auxImportes, function( n, i ) { return chkConfiguracion.is(":checked") ? (n.semana == 2 || n.semana == 6) : (n.semana == 2 && !n.acumulado); });
            if (semana2.length > 0) { $.each(semana2, function(index, element){ costo.semana2 += (element.monto * (negativo ? (-1) : 1)) || 0; });}
            var semana3 = jQuery.grep(auxImportes, function( n, i ) { return chkConfiguracion.is(":checked") ? (n.semana == 3 || n.semana == 6) : (n.semana == 3 && !n.acumulado); });
            if (semana3.length > 0) { $.each(semana3, function(index, element){ costo.semana3 += (element.monto * (negativo ? (-1) : 1)) || 0; });}
            var semana4 = jQuery.grep(auxImportes, function( n, i ) { return chkConfiguracion.is(":checked") ? (n.semana == 4 || n.semana == 6) : (n.semana == 4 && !n.acumulado); });
            if (semana4.length > 0) { $.each(semana4, function(index, element){ costo.semana4 += (element.monto * (negativo ? (-1) : 1)) || 0; });}
            var semana5 = jQuery.grep(auxImportes, function( n, i ) { return chkConfiguracion.is(":checked") ? (n.semana == 5 || n.semana == 6) : (n.semana == 5 && !n.acumulado); });
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
        //Guardar Linea
        function guardarLinea()
        {
            var errores = validarGuardadoLinea();
            tipoGuardado = botonGuardarLinea.attr("data-tipoGuardado");
            if(errores.length > 0)
            {
                $.each(errores, function(index, element){ 
                    switch(element)
                    {
                        case 1: $("#conceptoGuardarLinea").addClass("has-error"); break;
                        case 2: $("#montoGuardarLinea").addClass("has-error"); break;
                        case 3: $("#ccGuardarLinea").addClass("has-error"); break;
                        case 4: $("#acGuardarLinea").addClass("has-error"); break;
                        case 5: $("#fechaGuardarLinea").addClass("has-error"); break;
                        case 6: $("#conciliacionGuardarLinea").addClass("has-error"); break;
                        case 7: $("#empresaGuardarLinea").addClass("has-error"); break;
                    }
                });
            }
            else
            {
                const id = botonGuardarLinea.attr("data-id");
                const corteID = botonGuardarLinea.attr("data-corteID");
                const concepto = $("#conceptoGuardarLinea").val();
                const monto = $("#montoGuardarLinea").val();
                const cc = $("#ccGuardarLinea").val();
                const ac = $("#acGuardarLinea").val();
                const fecha = $("#fechaGuardarLinea").val();
                const conciliacion = $("#conciliacionGuardarLinea").val();
                const empresa = $("#empresaGuardarLinea").val();
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/Rentabilidad/GuardarLineaCorte',
                    data: { id: id, corteID: corteID, concepto: concepto, monto: monto, cc: cc, ac: ac, fecha: fecha, conciliacion: conciliacion, empresa: empresa, tipoGuardado: tipoGuardado },
                    success: function (response) {
                        if (response.success) {
                            if(response.exito) { 
                                AlertaGeneral('Éxito', 'Se completo el guardado con éxito'); 
                                $(".formGuardarLinea").val("");
                                cargarTablaCostosEstimados(tipoGuardado);
                                $("#modalGuardarLinea").modal("hide");
                            }                        
                        } 
                        else { AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message); }
                    },
                    error: function (error) { AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.'); }
                });   
            }            
        }
        function EliminarLinea()
        {
            const id = botonGuardarLinea.attr("data-id");
            const tipoGuardado = botonGuardarLinea.attr("data-tipoGuardado");
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/Rentabilidad/EliminarLineaCorte',
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        if(response.exito) { 
                            AlertaGeneral('Éxito', 'Se completo la eliminación con éxito'); 
                            botonGuardarLinea.attr("data-id", 0);
                            cargarTablaCostosEstimados(tipoGuardado);
                        }                        
                    } 
                    else { AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message); }
                },
                error: function (error) { AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.'); }
            });  
        }
        function validarGuardadoLinea()
        {
            $(".formGuardarLinea").removeClass("has-error");
            var errores = [];
            if($("#conceptoGuardarLinea").val() == "") errores.push(1);
            if($("#montoGuardarLinea").val() == "") errores.push(2);
            if($("#ccGuardarLinea").val() == "") errores.push(3);
            if($("#acGuardarLinea").val() == "") errores.push(4);
            if($("#fechaGuardarLinea").val() == "") errores.push(5);
            if($("#conciliacionGuardarLinea").val() == "") errores.push(6);
            if($("#empresaGuardarLinea").val() == "") errores.push(7);
            return errores;
        }
        function AbrirModalCostosEstimados(tipoGuardado)
        {
            if(usuarioID == 1176 || usuarioID == 6032 || usuarioID == 3841){
                switch(tipoGuardado)
                {
                    case "1":
                        $("#tituloGuardadoLinea").text("Ingresos con Estimación");
                        break;
                    case "2":
                        $("#tituloGuardadoLinea").text("Ingresos Pendientes por Generar");
                        break;
                    case "3":
                        $("#tituloGuardadoLinea").text("Costos Estimados");
                        break;
                    default:
                        $("#tituloGuardadoLinea").text("Costos Estimados");
                        break;

                }
                if(CheckCostoEstCerrado()) { botonCerrarCostoEst.prop("disabled", true); }
                else { botonCerrarCostoEst.prop("disabled", false); }
                cargarTablaCostosEstimados(tipoGuardado);
                botonGuardarLinea.attr("data-tipoGuardado", tipoGuardado);
                $("#modalCostosEstimados").modal("show");
            }
        }
        function AbrirModalEditarLinea(detalle)
        {
            if(usuarioID == 1176 || usuarioID == 6032 || usuarioID == 3841){
                $(".formGuardarLinea").removeClass("has-error");
                $(".formGuardarLinea").val("");   
                
                botonGuardarLinea.attr("data-id", detalle.id);
                botonGuardarLinea.attr("data-corteID", detalle.corteID);
                $("#conceptoGuardarLinea").val(detalle.concepto);
                $("#montoGuardarLinea").val(detalle.monto);
                $("#ccGuardarLinea").val(detalle.cc);
                $("#acGuardarLinea").val(detalle.areaCuenta.split(" ")[0]);
                $("#fechaGuardarLinea").datepicker("setDate", detalle.fecha);
                $("#conciliacionGuardarLinea").val(detalle.conciliacion);
                $("#empresaGuardarLinea").val(detalle.empresa);               

                $("#modalGuardarLinea").modal("show");
            }
        }
        function AbrirModalGuardarLinea()
        {
            if(usuarioID == 1176 || usuarioID == 6032 || usuarioID == 3841){
                $(".formGuardarLinea").removeClass("has-error");
                $(".formGuardarLinea").val("");   

                botonGuardarLinea.attr("data-id", 0);
                botonGuardarLinea.attr("data-corteID", botonBuscar.attr("data-corteID"));
                
                $("#modalGuardarLinea").modal("show");
            }
        }
        function initTablaCostosEstimados()
        {
            dtTablaCostosEstimados = tablaCostosEstimados.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { data: 'id', title: 'id', visible: false },
                    { data: 'corteID', title: 'corteID', visible: false },
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'monto', title: 'Monto', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'cc', title: 'CC' },
                    { data: 'areaCuenta', title: 'Area Cuenta' },
                    { data: 'fecha', title: 'Fecha', render: function(data, type, row) { return data.toLocaleDateString('en-GB').Capitalize() } },
                    { data: 'conciliacion', title: 'Conciliación' },
                    { data: 'empresa', title: 'Empresa', render: function (data, type, row) { return data == 1 ? "CONSTRUPLAN" : "ARRENDADORA"; } },
                    { title: 'Editar', render: function (data, type, row) { return "<button class='btn btn-sm btn-warning editar' data-index='" + row.id + "'><i class='fas fa-edit'></i></button>"; } },
                    { title: 'Eliminar', render: function (data, type, row) { return "<button class='btn btn-sm btn-danger eliminar' data-index='" + row.id + "'><i class='fas fa-minus-square'></i></button>"; } },

                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                order: [[7, 'desc']],
                drawCallback: function () {                 
                    tablaCostosEstimados.find('button.editar').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const detalle = dtTablaCostosEstimados.row($(this).parents('tr')).data();
                        AbrirModalEditarLinea(detalle);       
                    });     
                    tablaCostosEstimados.find('button.eliminar').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        botonGuardarLinea.attr("data-id", $(this).attr("data-index"));
                        estadoConfirmacion = 1;
                        ConfirmacionEliminacion("Eliminar", "¿Está seguro que desea eliminar el registro?");
                    }); 
                }                
            });
        }
        function cargarTablaCostosEstimados(tipoGuardado) {
            $.post(getLstKubrixCostoEstimado, { corteID: botonBuscar.attr("data-corteid"), tipoGuardado: tipoGuardado })
                .then(function (response) {
                    if (response.success) {
                        var detalles = response.detalles;
                        if(detalles.length > 0){
                            detalles = detalles.map(function(x) {
                                return {
                                    id: x.id,
                                    corteID: x.corteID,
                                    concepto: x.insumo_Desc,
                                    monto: x.importe * (-1),
                                    cc: x.cc,
                                    areaCuenta: x.areaCuenta,
                                    fecha: new Date(parseInt(x.fecha.substr(6))),
                                    conciliacion: x.referencia,
                                    empresa: x.empresa
                                };
                            });
                            dtTablaCostosEstimados.clear().rows.add(detalles).draw();
                            return true;
                        }
                        return false
                    } else {
                        // Operación no completada.
                    }
                }, function(error) {
                    // Error al lanzar la petición.
                }
            );


        }
        function CheckCostoEstCerrado()
        {
            const corteID = botonBuscar.attr("data-corteID");
            bandera = false;
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/Rentabilidad/CheckCostoEstimadoCerrado',
                data: { corteID: corteID },
                success: function (response) {
                    if (response.success) {
                        if(response.cerrado) { bandera = true; }    
                    } 
                },
                error: function (error) {  }
            });  
            return bandera;
        }
        function cerrarCostoEst()
        {
            const corteID = botonBuscar.attr("data-corteID");
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/Rentabilidad/CerrarCostoEst',
                data: { corteID: corteID },
                success: function (response) {
                    if (response.success) {
                        if(response.exito) { 
                            AlertaGeneral('Éxito', 'Se ha cerrado el costo estimado con éxito');
                            botonCerrarCostoEst.prop("disabled", true);
                        }                        
                    } 
                    else { AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message); }
                },
                error: function (error) { AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status +' - ' + error.statusText + '.'); }
            });   
        }
        function initTablaCXP() {
            dtTablaCXP = tablaCXP.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verCC' data.proveedor='" + row.proveedor + "'><i class='fas fa-layer-group'></i></button>"; } },
                    { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verFacturas'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'proveedor', title: 'Proveedor' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return getNumberHTML(data); } },
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
                //order: [[9, 'desc']],
                drawCallback: function () {                 
                    tablaCXP.find('button.verFacturas').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaCXP.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        if(areasCuentaDetalle.length > 0) {
                            detalles = $.grep(detalles,function(el,index){ 
                                return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                            });
                        }
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXPFacturas(detalles, fechaCorteGeneral);
                        $("#modalCXPFacturas").modal("show");
       
                    });     
                    tablaCXP.find('button.verCC').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaCXP.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXPAC(detalles, fechaCorteGeneral);
                        $("#modalCXPAC").modal("show");
       
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
                    totalPorVencer = (api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencido15 = (api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencido30 = (api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencido60 = (api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencido90 = (api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencidoMas = (api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    total = (api.column(9).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalPorcentaje = (api.column(10).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    $(api.column(2).footer()).html('TOTAL');
                    $(api.column(3).footer()).html('$' + parseFloat(totalPorVencer).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(4).footer()).html('$' + parseFloat(totalVencido15).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(5).footer()).html('$' + parseFloat(totalVencido30).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(6).footer()).html('$' + parseFloat(totalVencido60).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(7).footer()).html('$' + parseFloat(totalVencido90).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(8).footer()).html('$' + parseFloat(totalVencidoMas).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(9).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(10).footer()).html(parseFloat(totalPorcentaje).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '%');
                }
            });
        }
        function cargarTablaCXP(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }  
            else{
                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function(i, n) { totalDetalles += n.monto; }); 
                const grouped = groupBy(detalles, function(detalle) { return detalle.responsable; });
                dtTablaCXP.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const proveedor = value[0].responsable;
                    var total = 0;
                    var detallesFiltrados = value;
                    if(areasCuentaDetalle.length > 0) {
                        detallesFiltrados = $.grep(detallesFiltrados,function(el,index){ 
                            return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                        });
                    }
                    $.each(detallesFiltrados, function(i, n) { total += n.monto; }); 
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function(i, n) { porVencer += n.monto; });  
                    const vencido15Detalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function(i, n) { vencido15 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0)  $.each(vencido30Detalles, function(i, n) { vencido30 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function(i, n) { vencido60 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function(i, n) { vencido90 += n.monto; });  
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function(i, n) { vencidoMas += n.monto; });                  
                    const grupo = { proveedor: proveedor, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, total: total, porcentaje: porcentaje, detalles: value };
                    auxDatosFinales.push(grupo);
                });
                auxDatosFinales.sort((a, b) => b.total - a.total);

                var grupoFinal = { proveedor: "OTROS PROVEEDORES", porVencer: 0, vencido15: 0, vencido30: 0, vencido60: 0, vencido90: 0, vencidoMas: 0, total: 0, porcentaje: 0, detalles: [] };
                for(var i = 0; i < auxDatosFinales.length; i++)
                {
                    
                    if(i < 15) datosFinales.push(auxDatosFinales[i]);
                    else
                    {
                        grupoFinal.porVencer += auxDatosFinales[i].porVencer;
                        grupoFinal.vencido15+= auxDatosFinales[i].vencido15; 
                        grupoFinal.vencido30 += auxDatosFinales[i].vencido30;
                        grupoFinal.vencido60 += auxDatosFinales[i].vencido60;
                        grupoFinal.vencido90 += auxDatosFinales[i].vencido90;
                        grupoFinal.vencidoMas += auxDatosFinales[i].vencidoMas;
                        grupoFinal.total += auxDatosFinales[i].total;
                    }
                }
                grupoFinal.porcentaje = totalDetalles > 0 ? (grupoFinal.total * 100) / totalDetalles : 0;
                datosFinales.push(grupoFinal);
                dtTablaCXP.rows.add(datosFinales);
                dtTablaCXP.draw();
                return true;
            }
        }
        function initTablaCXPFacturas() {
            dtTablaCXPFacturas = tablaCXPFacturas.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { data: 'factura', title: 'Factura' },
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'fecha', title: 'Fecha', render: function (data, type, row) { return moment(data).toDate().toLocaleDateString('en-GB').Capitalize() } },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },

                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                //order: [[9, 'desc']],
                drawCallback: function () {                 
                 
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    totalPorVencer = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido15 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido30 = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido60 = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido90 = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencidoMas = api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total = api.column(9).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(2).footer()).html('TOTAL');
                    $(api.column(3).footer()).html('$' + parseFloat(totalPorVencer).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(4).footer()).html('$' + parseFloat(totalVencido15).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(5).footer()).html('$' + parseFloat(totalVencido30).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(6).footer()).html('$' + parseFloat(totalVencido60).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(7).footer()).html('$' + parseFloat(totalVencido90).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(8).footer()).html('$' + parseFloat(totalVencidoMas).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(9).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                }
            });
        }
        function cargarTablaCXPFacturas(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }  
            else{
                dtTablaCXPFacturas.clear();
                var datosFinales = [];
                $.each(detalles, function(i, n) { 
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const factura = n.factura;
                    const concepto = n.concepto;
                    const fecha = new Date(parseInt(n.fecha.substr(6)));
                    var porVencer = 0; 
                    if(fecha > fechaFin) porVencer = n.monto;
                    var vencido15 = 0;
                    if(fecha > fechaInicio && fecha <= fechaFin) vencido15 = n.monto;
                    var vencido30 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    if(fecha > fechaInicio && fecha <= fechaFin) vencido30 = n.monto;
                    var vencido60 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    if(fecha > fechaInicio && fecha <= fechaFin) vencido60 = n.monto;
                    var vencido90 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    if(fecha > fechaInicio && fecha <= fechaFin) vencido90 = n.monto;
                    var vencidoMas = 0;
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    if(fecha < fechaInicio) vencidoMas = n.monto;
                    var total = n.monto;
                    const group = { factura: factura, concepto: concepto, fecha: fecha, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, total: total };
                    datosFinales.push(group);
                }); 
                datosFinales.sort((a, b) => b.total - a.total);
                dtTablaCXPFacturas.rows.add(datosFinales);
                dtTablaCXPFacturas.draw();
                return true;
            }
        }
        function initTablaCXPAC() {
            dtTablaCXPAC = tablaCXPAC.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { data: 'descripcionCC', title: 'Descripcion CC' },
                    { data: 'porVencer', title: 'Concepto', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                //order: [[9, 'desc']],
                drawCallback: function () {                 
                 
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    totalPorVencer = api.column(1).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido15 = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido30 = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido60 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido90 = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencidoMas = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(0).footer()).html('TOTAL');
                    $(api.column(1).footer()).html('$' + parseFloat(totalPorVencer).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(2).footer()).html('$' + parseFloat(totalVencido15).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(3).footer()).html('$' + parseFloat(totalVencido30).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(4).footer()).html('$' + parseFloat(totalVencido60).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(5).footer()).html('$' + parseFloat(totalVencido90).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(6).footer()).html('$' + parseFloat(totalVencidoMas).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(7).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                }
            });
        }
        function cargarTablaCXPAC(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }  
            else{
                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function(i, n) { totalDetalles += n.monto; }); 
                const grouped = groupBy(detalles, function(detalle) { return detalle.areaCuenta; });
                dtTablaCXPAC.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const descripcionCC = value[0].areaCuenta + " " + value[0].areaCuentaDesc;
                    var total = 0;
                    $.each(value, function(i, n) { total += n.monto; }); 
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function(i, n) { porVencer += n.monto; });  
                    const vencido15Detalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function(i, n) { vencido15 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0)  $.each(vencido30Detalles, function(i, n) { vencido30 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function(i, n) { vencido60 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function(i, n) { vencido90 += n.monto; });  
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function(i, n) { vencidoMas += n.monto; });                  
                    const grupo = { descripcionCC: descripcionCC, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, total: total, porcentaje: porcentaje };
                    datosFinales.push(grupo);
                });
                datosFinales.sort((a, b) => b.total - a.total);                
                dtTablaCXPAC.rows.add(datosFinales);
                dtTablaCXPAC.draw();
                return true;
            }
        }
        function initTablaCXC() {
            dtTablaCXC = tablaCXC.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verCC' data-cliente='" + row.cliente + "'><i class='fas fa-layer-group'></i></button>"; } },
                    { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verFacturas' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'cliente', title: 'Cliente' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return getNumberHTML(data); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return getNumberHTML(data); } },
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
                //order: [[9, 'desc']],
                drawCallback: function () {                 
                    tablaCXC.find('button.verFacturas').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaCXC.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        if(areasCuentaDetalle.length > 0) {
                            detalles = $.grep(detalles,function(el,index){ 
                                return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                            });
                        }
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCFacturas(detalles, fechaCorteGeneral);
                        $("#modalCXCFacturas").modal("show");
       
                    });     
                    tablaCXC.find('button.verCC').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaCXC.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCAC(detalles, fechaCorteGeneral);
                        $("#modalCXCAC").modal("show");
       
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
                    totalPorVencer = (api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencido15 = (api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencido30 = (api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencido60 = (api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencido90 = (api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalVencidoMas = (api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    total = (api.column(9).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    totalPorcentaje = (api.column(10).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    $(api.column(2).footer()).html('TOTAL');
                    $(api.column(3).footer()).html('$' + parseFloat(totalPorVencer).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(4).footer()).html('$' + parseFloat(totalVencido15).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(5).footer()).html('$' + parseFloat(totalVencido30).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(6).footer()).html('$' + parseFloat(totalVencido60).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(7).footer()).html('$' + parseFloat(totalVencido90).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(8).footer()).html('$' + parseFloat(totalVencidoMas).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(9).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(10).footer()).html(parseFloat(totalPorcentaje).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString() + '%');
                }
            });
        }
        function cargarTablaCXC(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }  
            else{
                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function(i, n) { totalDetalles += n.monto; }); 
                const grouped = groupBy(detalles, function(detalle) { return detalle.responsable; });
                dtTablaCXC.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const cliente = value[0].responsable;
                    var total = 0;
                    var detallesFiltrados = value;
                    if(areasCuentaDetalle.length > 0) {
                        detallesFiltrados = $.grep(detallesFiltrados,function(el,index){ 
                            return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                        });
                    }
                    $.each(detallesFiltrados, function(i, n) { total += n.monto; }); 
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function(i, n) { porVencer += n.monto; });  
                    const vencido15Detalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function(i, n) { vencido15 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0)  $.each(vencido30Detalles, function(i, n) { vencido30 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function(i, n) { vencido60 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function(i, n) { vencido90 += n.monto; });  
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(detallesFiltrados, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function(i, n) { vencidoMas += n.monto; });                  
                    const grupo = { cliente: cliente, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, total: total, porcentaje: porcentaje, detalles: value };
                    auxDatosFinales.push(grupo);
                });
                auxDatosFinales.sort((a, b) => b.total - a.total);

                var grupoFinal = { cliente: "OTROS CLIENTES", porVencer: 0, vencido15: 0, vencido30: 0, vencido60: 0, vencido90: 0, vencidoMas: 0, total: 0, porcentaje: 0, detalles: [] };
                for(var i = 0; i < auxDatosFinales.length; i++)
                {
                    
                    if(i < 15) datosFinales.push(auxDatosFinales[i]);
                    else
                    {
                        grupoFinal.porVencer += auxDatosFinales[i].porVencer;
                        grupoFinal.vencido15+= auxDatosFinales[i].vencido15; 
                        grupoFinal.vencido30 += auxDatosFinales[i].vencido30;
                        grupoFinal.vencido60 += auxDatosFinales[i].vencido60;
                        grupoFinal.vencido90 += auxDatosFinales[i].vencido90;
                        grupoFinal.vencidoMas += auxDatosFinales[i].vencidoMas;
                        grupoFinal.total += auxDatosFinales[i].total;
                    }
                }
                grupoFinal.porcentaje = totalDetalles > 0 ? (grupoFinal.total * 100) / totalDetalles : 0;
                datosFinales.push(grupoFinal);
                dtTablaCXC.rows.add(datosFinales);
                dtTablaCXC.draw();
                return true;
            }
        }
        function initTablaCXCFacturas() {
            dtTablaCXCFacturas = tablaCXCFacturas.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { data: 'factura', title: 'Factura' },
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'fecha', title: 'Fecha', render: function (data, type, row) { return moment(data).toDate().toLocaleDateString('en-GB').Capitalize() } },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },

                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                //order: [[9, 'desc']],
                drawCallback: function () {                 
                 
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    totalPorVencer = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido15 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido30 = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido60 = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido90 = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencidoMas = api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total = api.column(9).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(2).footer()).html('TOTAL');
                    $(api.column(3).footer()).html('$' + parseFloat(totalPorVencer).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(4).footer()).html('$' + parseFloat(totalVencido15).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(5).footer()).html('$' + parseFloat(totalVencido30).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(6).footer()).html('$' + parseFloat(totalVencido60).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(7).footer()).html('$' + parseFloat(totalVencido90).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(8).footer()).html('$' + parseFloat(totalVencidoMas).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(9).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                }
            });
        }
        function cargarTablaCXCFacturas(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }  
            else{
                dtTablaCXCFacturas.clear();
                var datosFinales = [];
                $.each(detalles, function(i, n) { 
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const factura = n.factura;
                    const concepto = n.concepto;
                    const fecha = new Date(parseInt(n.fecha.substr(6)));
                    var porVencer = 0; 
                    if(fecha > fechaFin) porVencer = n.monto;
                    var vencido15 = 0;
                    if(fecha > fechaInicio && fecha <= fechaFin) vencido15 = n.monto;
                    var vencido30 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    if(fecha > fechaInicio && fecha <= fechaFin) vencido30 = n.monto;
                    var vencido60 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    if(fecha > fechaInicio && fecha <= fechaFin) vencido60 = n.monto;
                    var vencido90 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    if(fecha > fechaInicio && fecha <= fechaFin) vencido90 = n.monto;
                    var vencidoMas = 0;
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    if(fecha < fechaInicio) vencidoMas = n.monto;
                    var total = n.monto;
                    const group = { factura: factura, concepto: concepto, fecha: fecha, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, total: total };
                    datosFinales.push(group);
                }); 
                datosFinales.sort((a, b) => b.total - a.total);
                dtTablaCXCFacturas.rows.add(datosFinales);
                dtTablaCXCFacturas.draw();
                return true;
            }
        }
        function initTablaCXCAC() {
            dtTablaCXCAC = tablaCXCAC.DataTable({
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { data: 'descripcionCC', title: 'Descripcion CC' },
                    { data: 'porVencer', title: 'Concepto', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return parseFloat(data).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"); } },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                //order: [[9, 'desc']],
                drawCallback: function () {                 
                 
                },
                footerCallback: function ( row, data, start, end, display ) {                    
                    var api = this.api(), data;
                    var intVal = function ( i ) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '')*1 :
                            typeof i === 'number' ?
                            i : 0;
                    };
                    totalPorVencer = api.column(1).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido15 = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido30 = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido60 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencido90 = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    totalVencidoMas = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    $(api.column(0).footer()).html('TOTAL');
                    $(api.column(1).footer()).html('$' + parseFloat(totalPorVencer).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(2).footer()).html('$' + parseFloat(totalVencido15).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(3).footer()).html('$' + parseFloat(totalVencido30).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(4).footer()).html('$' + parseFloat(totalVencido60).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(5).footer()).html('$' + parseFloat(totalVencido90).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(6).footer()).html('$' + parseFloat(totalVencidoMas).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                    $(api.column(7).footer()).html('$' + parseFloat(total).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,").toString());
                }
            });
        }
        function cargarTablaCXCAC(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }  
            else{
                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function(i, n) { totalDetalles += n.monto; }); 
                const grouped = groupBy(detalles, function(detalle) { return detalle.areaCuenta; });
                dtTablaCXCAC.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const descripcionCC = value[0].areaCuenta + " " + value[0].areaCuentaDesc;
                    var total = 0;
                    $.each(value, function(i, n) { total += n.monto; }); 
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function(i, n) { porVencer += n.monto; });  
                    const vencido15Detalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function(i, n) { vencido15 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0)  $.each(vencido30Detalles, function(i, n) { vencido30 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function(i, n) { vencido60 += n.monto; });  
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function(i, n) { vencido90 += n.monto; });  
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(value, function( n, i ) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function(i, n) { vencidoMas += n.monto; });                  
                    const grupo = { descripcionCC: descripcionCC, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, total: total, porcentaje: porcentaje };
                    datosFinales.push(grupo);
                });
                datosFinales.sort((a, b) => b.total - a.total);                
                dtTablaCXCAC.rows.add(datosFinales);
                dtTablaCXCAC.draw();
                return true;
            }
        }
        function AbrirModalCXP()
        {
            let rowData = dtTablaCXP.data();
            var detallesRaw = rowData.map(function(x) { return x.detalles});
            var detalles = [].concat.apply([], detallesRaw);
            detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
            cargarTablaCXP(detalles, fechaCorteGeneral);
            $("#modalCXP").modal("show");
        }
        function AbrirModalCXC()
        {
            let rowData = dtTablaCXC.data();
            var detallesRaw = rowData.map(function(x) { return x.detalles});
            var detalles = [].concat.apply([], detallesRaw);
            detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
            cargarTablaCXC(detalles, fechaCorteGeneral);
            $("#modalCXC").modal("show");
        }
        function recargarTotalizadoresCX()
        {
            let rowData = dtTablaCXP.data();
            var detallesRaw = rowData.map(function(x) { return x.detalles});
            var detalles = [].concat.apply([], detallesRaw);
            detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
            if(areasCuentaDetalle.length > 0) {
                detalles = $.grep(detalles,function(el,index){ 
                    return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                });
            }
            var totalizador = 0;
            $.each(detalles, function(i, n) { totalizador += n.monto; }); 
            $(".pCXP").text(parseFloat(totalizador / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") ); 

            rowData = dtTablaCXC.data();
            detallesRaw = rowData.map(function(x) { return x.detalles});
            detalles = [].concat.apply([], detallesRaw);
            detalles = $.grep(detalles,function(el,index){ return (index == $.inArray(el,detalles) && el != null); });
            if(areasCuentaDetalle.length > 0) {
                detalles = $.grep(detalles,function(el,index){ 
                    return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                });
            }
            totalizador = 0;
            $.each(detalles, function(i, n) { totalizador += n.monto; }); 
            $(".pCXC").text(parseFloat(totalizador / 1000).toFixed(0).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") ); 
        }
        //Subir Archivo Estimados
        function SubirArchivoEstimados(e) {
            e.preventDefault();
            if (document.getElementById("inCargarArchivo").files[0] == null) {
                //$("#pathArchivo").text("  NINGÚN ARCHIVO SELECCIONADO");
                //btnSubirArchivo.prop("disabled", true);
            }
            else {
                var ext = document.getElementById("inCargarArchivo").files[0].name.match(/\.(.+)$/)[1];
                ext = ext.toLowerCase();
                if (ext == 'xls' || ext == 'xlsx' || ext == 'csv') {
                    size = document.getElementById("inCargarArchivo").files[0].size;
                    //if (size > 20971520) {
                    //    AlertaGeneral("Alerta", "Archivo sobrepasa los 20MB");
                    //}
                    //else {
                    if (size <= 0) {
                        AlertaGeneral("Alerta", "Archivo vacío");
                    }
                    else {
                        guardarArchivoEstimados(e);
                    }
                    //}
                }
                else {
                    AlertaGeneral("Alerta", "Sólo se aceptan archivos Excel");
                }
            }
        }
        function guardarArchivoEstimados(e) {
            e.preventDefault();
            if (botonBuscar.attr("data-corteid") != null) {
                $.blockUI({ message: "Procesando...", baseZ: 2000 });
                var formData = new FormData();
                var request = new XMLHttpRequest();
                var file = document.getElementById("inCargarArchivo").files[0];
                formData.append("archivoEstimados", file);
                formData.append("corteID", botonBuscar.attr("data-corteid"));
                if (file != undefined) { $.blockUI({ message: 'Cargando archivo... Espere un momento', baseZ: 2000 }); }
                $.ajax({
                    type: "POST",
                    url: '/Rentabilidad/GuardarEstimados',
                    data: formData,
                    dataType: 'json',
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        $.unblockUI();
                        inCargarArchivo.val("");
                        switch(response.exito)
                        {
                            case 0:
                                AlertaGeneral("Alerta", "Ocurrio un error al leer los datos, favor de revisar el formato del archivo");
                                break;
                            case 1:
                                AlertaGeneral("Éxtio", "Se guardaron correctamente los estimados");
                                break;
                            case 2:
                                AlertaGeneral("Alerta", "Ya existe registro de estimados para este corte");
                        }
                    },
                    error: function (response) {
                        $.unblockUI();
                        inCargarArchivo.val("");
                        AlertaGeneral("Alerta", "Se encontró un error al tratar de guardar el archivo");
                    }
                });
            }
            else {
                inCargarArchivo.val("");
                AlertaGeneral("Alerta", "Favor de cargar un corte antes de subir ");
            }
        }

        init();
    };

    $(document).ready(function () {
        maquinaria.rentabilidad.cortecplan = new cortecplan();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();