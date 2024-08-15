(function () {

    $.namespace('maquinaria.overhaul.reportesadministradorcomponentes');

    reportesadministradorcomponentes = function () {
        mensajes = {
            NOMBRE: 'Componentes',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
            
        };
        let ultimoIDCorreo = 0;
    
        
        tabVidaUtil = $("#tabVidaUtil"),
        tabReparacionesPendientes = $("#tabReparacionesPendientes"),
        tabTiemposCRC = $("#tabTiemposCRC"),
        tabDesecho = $("#tabDesecho"),
        tabRemociones = $("#tabRemociones"),
        tabInventario = $("#tabInventario"),
        tabValorAlmacen = $("#tabValorAlmacen"),
        tabMaestro = $("#tabMaestro"),

        // Vida útil
        gridVidaUtil = $("#gridVidaUtil"),
        cboGrupoVida = $("#cboGrupoVida"),
        cboModeloVida = $("#cboModeloVida"),
        btnBuscarVida = $("#btnBuscarVida"),
        btnReporteVida = $("#btnReporteVida"),
        btnRptVidaRend = $("#btnRptVidaRend"),
        txtFIVida = $("#txtFIVida"),
        txtFFVida = $("#txtFFVida"),
        reporteVidaUtil = $("#reporteVidaUtil"),
        ireporteVidaUtil = $("#reporteVidaUtil > #reportViewerModal > #report"),

        //Reparaciones
        txtFechaInicioReparaciones = $("#txtFechaInicioReparaciones"),
        txtFechaFinReparaciones = $("#txtFechaFinReparaciones"),
        btnBuscarReparaciones = $("#btnBuscarReparaciones"),
        btnReporteReparaciones = $("#btnReporteReparaciones"),
        tblReparaciones = $("#tblReparaciones"),
        reporteReparaciones = $("#reporteReparaciones"),
        ireporteReparaciones = $("#reporteReparaciones > #reportViewerModal > #report"),
        txtSubconjuntoReparaciones = $("#txtSubconjuntoReparaciones"),
        txtNoCompReparaciones = $("#txtNoCompReparaciones"),
        cboObraReparaciones = $("#cboObraReparaciones"),
        txtEconomicoReparaciones = $("#txtEconomicoReparaciones"),
        cboProveedorReparaciones = $("#cboProveedorReparaciones"),
        txtCotizacionReparaciones = $("#txtCotizacionReparaciones"),
        //Tiempos CRC
        txtFechaInicioTiemposCRC = $("#txtFechaInicioTiemposCRC"),
        txtFechaFinTiemposCRC = $("#txtFechaFinTiemposCRC"),
        btnBuscarTiemposCRC = $("#btnBuscarTiemposCRC"),
        btnReporteTiemposCRC = $("#btnReporteTiemposCRC"),
        tblTiemposCRC = $("#tblTiemposCRC"),
        tblRastreoAdminTiemposCRC = $("#tblRastreoAdminTiemposCRC"),
        reporteTiemposCRC = $("#reporteTiemposCRC"),
        ireporteTiemposCRC = $("#reporteTiemposCRC > #reportViewerModal > #report"),
        txtSubconjuntoTiemposCRC = $("#txtSubconjuntoTiemposCRC"),
        txtNoCompTiemposCRC = $("#txtNoCompTiemposCRC"),
        cboObraTiemposCRC = $("#cboObraTiemposCRC"),
        txtEconomicoTiemposCRC = $("#txtEconomicoTiemposCRC"),
        cboProveedorTiemposCRC = $("#cboProveedorTiemposCRC"),
        txtCotizacionTiemposCRC = $("#txtCotizacionTiemposCRC"),
        txtCompradorReparaciones = $("#txtCompradorReparaciones"),
        cbotipoRastreoTiemposCRC = $("#cbotipoRastreoTiemposCRC"),
        //Desecho
        txtComponenteDesecho = $("#txtComponenteDesecho"),
        txtFechaInicioDesecho = $("#txtFechaInicioDesecho"),
        txtFechaFinDesecho = $("#txtFechaFinDesecho"),
        cboConjuntoDesecho = $("#cboConjuntoDesecho"),
        cboSubconjuntoDesecho = $("#cboSubconjuntoDesecho"),

        btnBuscarDesecho = $("#btnBuscarDesecho"),
        btnCorreosAlmacen = $("#btnCorreosAlmacen"),

        tblDesecho = $("#tblDesecho"),
        reporteDesecho = $("#reporteDesecho"),
        ireporteDesecho = $("#reporteDesecho > #reportViewerModal > #report"),
        // Remociones
        gridReportes = $("#gridReportes"),
        fsReporteRemocion = $("#fsReporteRemocion"),
        modalReporteRemocion = $("#modalReporteRemocion"),
        titleModal = $("#title-modal"),
        reporteRemocion = $("#reporteRemocion"),
        ireporteRemocion = $("#reporteRemocion > #reportViewerModal > #report"),
        botonAprobar = $("#dialogalertaGeneral .ui-button"),
        cboEstatusReporte = $("#cboEstatusReporte"),
        txtFiltroDescripcionComponenteRR = $("#txtFiltroDescripcionComponenteRR"),
        btnBuscarRR = $("#btnBuscarRR"),
        txtFiltroEconomicoRR = $("#txtFiltroEconomicoRR"),
        cboCCRR = $("#cboCCRR"),
        cboModeloRR = $("#cboModeloRR"),
        cboMotivoRemocion = $("#cboMotivoRemocion"),
        cboEstatusRR = $("#cboEstatusRR"),
        txtFechaInicioRR = $("#txtFechaInicioRR"),
        txtFechaFinRR = $("#txtFechaFinRR"),
        txtNoComponenteRR = $("#txtNoComponenteRR");
        var idReporteRR = 0;
        var banderaRR = 0;
        btnReporteRR = $("#btnReporteRR"),
        //Histórico Valor Almacén
        cboAnioValorAlmacen = $("#cboAnioValorAlmacen"),
        btnBuscarValorAlmacen = $("#btnBuscarValorAlmacen"),
        btnReporteValorAlmacen = $("#btnReporteValorAlmacen"),
        gridValorAlmacen = $("#gridValorAlmacen"),
        reporteValorAlmacen = $("#reporteValorAlmacen"),
        ireportValorAlmacen = $("#reporteValorAlmacen > #reportViewerModal > #report");
        //Listado Maestro
        cboCalendarioMaestro = $("#cboCalendarioMaestro"),
        btnBuscarMaestro = $("#btnBuscarMaestro"),
        //btnReporteMaestro = $("#btnReporteMaestro"),
        gridMaestro = $("#gridMaestro"),
        reporteMaestro = $("#reporteMaestro"),
        ireportMaestro = $("#reporteMaestro > #reportViewerModal > #report");
        gridDetallesModalMaestro = $("#gridDetallesModalMaestro"),
        modalDetallesMaestro = $("#modalDetallesMaestro");

        agregarCorreo = $("#agregarCorreo");
        ireport = $("#report");
        btnEnviar = $("#btnEnviar");
        ModalCorreosAlmacen = $("#ModalCorreosAlmacen"),

      

        //Component List
        cboLocacionComponent = $("#cboLocacionComponent"),
        noComponenteComponent = $("#noComponenteComponent"),
        cboConjuntoComponent = $("#cboConjuntoComponent"),
        cboSubconjuntoComponent = $("#cboSubconjuntoComponent"),
        btnBuscarComponent = $("#btnBuscarComponent"),
        btnReporteComponent = $("#btnReporteComponent"),
        cboModeloComponent = $("#cboModeloComponent"),
        cboObraComponent = $("#cboObraComponent"),
        gridComponent = $("#gridComponent");
        ireporteComponent = $("#reporteComponent > #reportViewerModal > #report"),

        //Inventario
        gridInventario = $("#gridInventario");
        cboLocacionInventario = $("#cboLocacionInventario"),
        noComponenteInventario = $("#noComponenteInventario"),
        cboConjuntoInventario = $("#cboConjuntoInventario"),
        cboSubconjuntoInventario = $("#cboSubconjuntoInventario"),
        fechaInicioInventario = $("#fechaInicioInventario"),
        fechaFinInventario = $("#fechaFinInventario"),
        btnBuscarInventario = $("#btnBuscarInventario"),
        btnReporteInventario = $("#btnReporteInventario"),
        reporteInventario = $("#reporteInventario"),
        ireporteInventario = $("#reporteInventario > #reportViewerModal > #report");


        var locacionesInventario = [];

        //Componentes en reparación
        cboLocacionCompReparacion = $("#cboLocacionCompReparacion"),
        cboGrupoCompReparacion = $("#cboGrupoCompReparacion"),
        cboModeloCompReparacion = $("#cboModeloCompReparacion"),
        subconjuntoCompReparacion = $("#subconjuntoCompReparacion"),
        btnBuscarCompReparacion = $("#btnBuscarCompReparacion"),
        btnReporteCompReparacion = $("#btnReporteCompReparacion"),
        gridCompReparacion = $("#gridCompReparacion"),
        reporteCompReparacion = $("#reporteCompReparacion"),
        ireporteCompReparacion = $("#reporteCompReparacion > #reportViewerModal > #report"),

        tblCorreos = $("#tblCorreos"); 
        
        
        function init() {
            // Vida útil            
            initGridVidaUtil();
            initTblCorreos();
            tblCorreos.bootgrid("clear");

            
            txtFIVida.datepicker().datepicker("setDate", new Date());
            txtFFVida.datepicker().datepicker("setDate", new Date());
            cboGrupoVida.fillCombo('/Overhaul/FillCboGrupoMaquinaComponentes', { obj: 0 });
            cboGrupoVida.change(cargarModeloVida);
            cboModeloVida.change(cargarGridVidaUtil);
            btnBuscarVida.click(cargarGridVidaUtil);
            btnReporteVida.click(ReporteVidaUtil);
            $("#reporteVidaUtil > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteVidaUtil > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteVidaUtil > #reportViewerModal").css("width", "0%");
                $("#reporteVidaUtil > #reportViewerModal").css("height", "0%");
            });

            //Conjuntos en reparación
            initGridCompReparacion();
            cboLocacionCompReparacion.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 2 });
            cboGrupoCompReparacion.fillCombo('/Overhaul/FillCboGrupoMaquinaComponentes', { obj: 0 });
            
            cboGrupoCompReparacion.change(cargarModeloCompReparacion);
            btnBuscarCompReparacion.click(cargarGridCompReparacion);
            btnReporteCompReparacion.click(ReporteCompReparacion);

            subconjuntoCompReparacion.getAutocomplete(SelectSubconjuntoCR, null, '/Overhaul/getSubConjuntos');

            $("#reporteCompReparacion > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteCompReparacion > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteCompReparacion > #reportViewerModal").css("width", "0%");
                $("#reporteCompReparacion > #reportViewerModal").css("height", "0%");
            });

            cboLocacionCompReparacion.select2();
            cboGrupoCompReparacion.select2();
            cboModeloCompReparacion.select2();

            //Conjuntos y subconjuntos
            initGridConjuntos();
            txtFechaInicioReparaciones.datepicker().datepicker("setDate", new Date());
            txtFechaFinReparaciones.datepicker().datepicker("setDate", new Date());
            btnReporteReparaciones.click(CargarRptReparacionesPend);
            $("#reporteReparaciones > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteReparaciones > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteReparaciones > #reportViewerModal").css("width", "0%");
                $("#reporteReparaciones > #reportViewerModal").css("height", "0%");
            });
            btnBuscarReparaciones.click(cargarTblReparaciones);

            
            txtSubconjuntoReparaciones.getAutocomplete(SelectSubconjuntoReparaciones, null, '/Overhaul/getSubConjuntos');
            txtNoCompReparaciones.getAutocomplete(SelectNoComponenteReparaciones, null, '/Overhaul/getNoComponente');
            cboObraReparaciones.fillCombo('/Overhaul/FillCboObraMaquina');
            txtEconomicoReparaciones.getAutocomplete(SelectEconomicoReparaciones, null, '/Overhaul/getEconomico');
            cboProveedorReparaciones.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 2 });
            //--// Autocompletado comprador
            txtCompradorReparaciones.getAutocomplete(SelectCompradorTiemposCRC, null, '/Overhaul/FillTxtComprador');
            //TiemposCRC
            initTblTiempos();
            initTblRastreoAdminTiemposCRC();
            txtFechaInicioTiemposCRC.datepicker().datepicker("setDate", new Date());
            txtFechaFinTiemposCRC.datepicker().datepicker("setDate", new Date());
            btnReporteTiemposCRC.click(CargarRptTiemposCRC);
            $("#reporteTiemposCRC > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteTiemposCRC > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteTiemposCRC > #reportViewerModal").css("width", "0%");
                $("#reporteTiemposCRC > #reportViewerModal").css("height", "0%");
            });

            btnEnviar.click(CargarRptTiemposCRCCorreo);
            $("#reporteTiemposCRC").click(function (e) {
                e.preventDefault();
                $("#reporteTiemposCRC ");
                $("#reporteTiemposCRC");
                $("#reporteTiemposCRC");
            });
           

            btnBuscarTiemposCRC.click(cargarTblTiemposCRC);
            txtSubconjuntoTiemposCRC.getAutocomplete(SelectSubconjuntoTiemposCRC, null, '/Overhaul/getSubConjuntos');
            txtNoCompTiemposCRC.getAutocomplete(SelectNoComponenteTiemposCRC, null, '/Overhaul/getNoComponente');
            cboObraTiemposCRC.fillCombo('/Overhaul/FillCboObraMaquina');
            txtEconomicoTiemposCRC.getAutocomplete(SelectEconomicoTiemposCRC, null, '/Overhaul/getEconomico');
            cboProveedorTiemposCRC.fillCombo('/Overhaul/FillCboLocacion', { tipoLocacion: 2 });
            cboProveedorTiemposCRC.select2();
            cbotipoRastreoTiemposCRC.change(cambiarTablaTiemposCRC);
            //Desecho
            IniciarTblDesecho();            
            cboConjuntoDesecho.fillCombo('/CatComponentes/FillCboConjunto_Componente', { idModelo: -1 });
            cboConjuntoDesecho.change(CargarSubDesecho);
            txtFechaInicioDesecho.datepicker().datepicker("setDate", new Date());
            txtFechaFinDesecho.datepicker().datepicker("setDate", new Date());
            txtComponenteDesecho.getAutocomplete(SelectComponenteDesecho, null, '/Overhaul/getNoComponente');
            btnBuscarDesecho.click(CargarTblDesecho);
            $("#reporteDesecho > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteDesecho > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteDesecho > #reportViewerModal").css("width", "0%");
                $("#reporteDesecho > #reportViewerModal").css("height", "0%");
            });
            // Remociones
            initGridRR();
            cboCCRR.fillCombo('/CatComponentes/FillCbo_CentroCostos', {}, true);
            cboCCRR.select2();
            cboModeloRR.fillCombo('/Overhaul/fillCboModelo', {}, true);
            cboModeloRR.select2();
            //tabRemociones.click(cargarGridRR);
            cboEstatusReporte.change(cargarGridRR);
            btnBuscarRR.click(cargarGridRR);
            btnReporteRR.click(abrirReportePorFiltros);
            var date = new Date();
            var firstDay = new Date(date.getFullYear(), date.getMonth(), 1);
            txtFechaInicioRR.datepicker().datepicker("setDate", firstDay);
            txtFechaFinRR.datepicker().datepicker("setDate", new Date());
            txtFiltroDescripcionComponenteRR.getAutocomplete(SelectSubconjuntoRR, null, '/Overhaul/getSubConjuntos');
            txtFiltroEconomicoRR.getAutocomplete(SelectEconomicoRR, null, '/Overhaul/getEconomico');
            //txtFiltroDescripcionComponenteRR.change(cargarGridRR);
            //txtFiltroEconomicoRR.change(cargarGridRR);
            //cboCCRR.change(cargarGridRR);
            //cboMotivoRemocion.change(cargarGridRR);
            $(document).on('click', "#modalEliminar #btnModalEliminar", function () {
                if ($("#ulNuevo .active a").text() == "Remociones") {
                    switch (banderaRR) {
                        case 0:
                            eliminarReporte();
                            break;
                        case 1:
                            verificarReporte();
                            break;
                        case 2:
                            aprobarReporte();
                            break;
                        default:
                            break;
                    }
                    cargarGridRR();
                }

            });
            $("#reporteRemocion > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteRemocion > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteRemocion > #reportViewerModal").css("width", "0%");
                $("#reporteRemocion > #reportViewerModal").css("height", "0%");
            });
            txtNoComponenteRR.getAutocomplete(SelectNoComponenteRemociones, null, '/Overhaul/getNoComponente');

            //Histórico Valor Almacén
            initGridValorAlmacen();
            cboAnioValorAlmacen.fillCombo('/Overhaul/FillCboAniosValorAlmacen');
            btnBuscarValorAlmacen.click(cargarGridValorAlmacen);
            btnReporteValorAlmacen.click(abrirReporteHistoricoAlmacen);
            $("#reporteValorAlmacen > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteValorAlmacen > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteValorAlmacen > #reportViewerModal").css("width", "0%");
                $("#reporteValorAlmacen > #reportViewerModal").css("height", "0%");
            });
            //Listado Maestro
            initGridMaestro();
            btnBuscarMaestro.click(cargarGridMaestro);
            cboCalendarioMaestro.fillCombo('/Overhaul/FillCboCalendarioReporteMaestro');
            $("#reporteMaestro > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteMaestro > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteMaestro > #reportViewerModal").css("width", "0%");
                $("#reporteMaestro > #reportViewerModal").css("height", "0%");
            });

            agregarCorreo.click(AgregarCorreo);
            btnEnviar.click(EnviarReporteAdministradorComponentes);  

         
                  


            //Component List
            intiGridComponent();
            cboLocacionComponent.fillCombo('/Overhaul/FillCboLocacionesComponentList', { modelosID: [] });
            cboModeloComponent.change(RecargarLocaciones);
            cboConjuntoComponent.change(cargarSubconjuntosComponent);
            noComponenteComponent.fillCombo('/Overhaul/FillCboComponentes');
            cboConjuntoComponent.fillCombo('/Overhaul/FillCboConjuntos');
            cboLocacionComponent.select2();
            noComponenteComponent.select2({ tags: true });
            btnBuscarComponent.click(cargarGridComponent);
            btnReporteComponent.click(CargarRptComponentList);
            cboModeloComponent.select2();
            cboModeloComponent.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: -1 }, false, "TODOS");
            cboModeloComponent.find('option').get(0).remove();
            cboObraComponent.fillCombo('/Overhaul/FillCboObraMaquina');
            $("#reporteComponent > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteComponent > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteComponent > #reportViewerModal").css("width", "0%");
                $("#reporteComponent > #reportViewerModal").css("height", "0%");
            });

            //Inventario
            intiGridInventario();
            cboConjuntoInventario.change(cargarSubconjuntosInventario);
            cboLocacionInventario.fillCombo('/Overhaul/FillCboAlmacenesInventario');
            noComponenteInventario.fillCombo('/Overhaul/FillCboComponentes');
            cboConjuntoInventario.fillCombo('/Overhaul/FillCboConjuntos');
            cboLocacionInventario.select2();
            noComponenteInventario.select2({ tags: true });
            fechaInicioInventario.datepicker();
            fechaFinInventario.datepicker();
            btnBuscarInventario.click(cargarGridInventario);
            btnReporteInventario.click(CargarRptInventario);
            $("#reporteInventario > #reportViewerModal > #btnCrvReporteEstandarCerrar").click(function (e) {
                e.preventDefault();
                $("#reporteInventario > #reportViewerModal > body").css("overflow", "auto");
                $("#reporteInventario > #reportViewerModal").css("width", "0%");
                $("#reporteInventario > #reportViewerModal").css("height", "0%");
            });
        }

        //Autocompletado

            //Reparaciones
        function SelectSubconjuntoReparaciones(eventppal, uippal) { txtSubconjuntoReparaciones.text(uippal.item.descripcion); }
        function SelectNoComponenteReparaciones(event, ui) { txtNoCompReparaciones.text(ui.item.noComponente); }
        function SelectEconomicoReparaciones(event, ui) { txtEconomicoReparaciones.text(ui.item.noComponente); }
            //TiemposCRC
        function SelectSubconjuntoTiemposCRC(eventppal, uippal) { txtSubconjuntoTiemposCRC.text(uippal.item.descripcion); }
        function SelectNoComponenteTiemposCRC(event, ui) { txtNoCompTiemposCRC.text(ui.item.noComponente); }
        function SelectEconomicoTiemposCRC(event, ui) { txtEconomicoTiemposCRC.text(ui.item.noComponente); }
            //Desecho
        function SelectComponenteDesecho(event, ui) { txtComponenteDesecho.text(ui.item.noComponente); }
        //Remociones
        function SelectNoComponenteRemociones(event, ui) { txtNoComponenteRR.text(ui.item.noComponente); }
        function SelectEconomicoRR(event, ui) { txtFiltroEconomicoRR.text(ui.item.noComponente); }
        //autocompletado Comprador
        function SelectCompradorTiemposCRC(event, ui) { txtCompradorReparaciones.text(ui.item.Text); }


        function initTblCorreos() {
            ultimoIDCorreo = 0;          
            
            tblCorreos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: { header: "" },
                sorting: false,  
                              
                formatters: { "eliminar": function (column, row) { 
                return "<button type='button' class='btn btn-sm btn-danger eliminar'><span class='glyphicon glyphicon-remove'></span></button>"; } }
            }).on("loaded.rs.jquery.bootgrid", 
            function () {      

                /* Executes after data is loaded and rendered */
                tblCorreos.find(".eliminar").parent().css("text-align", "center");
                tblCorreos.find(".eliminar").parent().css("width", "3%");
                tblCorreos.find(".eliminar").on("click", function(e) {  
                    e.preventDefault();  
                       e.stopPropagation();
                e.stopImmediatePropagation();                  
                 var rowID = parseInt($(this).parent().parent().attr('data-row-id'));
                tblCorreos.bootgrid("remove", [rowID]);
                });
            });             
        }



        function AgregarCorreo() {
            let correo = $("#txtCorreo").val().trim();
           
            if (correo == "") {
                Alert2Warning("Favor de introducir un correo");  
                return;               
            }
            if (Existe(correo) == false) {                
                if (correo != "" && validateEmail(correo)) {
                    var JSONINFO = [{ "id": ultimoIDCorreo, "correo": correo }];
                    console.log(JSONINFO)
                    tblCorreos.bootgrid("append", JSONINFO);
                    ultimoIDCorreo++;
                }
                else { AlertaGeneral("Alerta", "No se ha proporcionado un correo electrónico válido"); }
            }
            else{
                
                Alert2Warning("Este correo ya se encuentra agregado"); 
            }                      
        }

        function Existe(params) {
            let tr = $('#tblCorreos').find('tr')
            let existe = false;     
            for (let index = 0; index < tr.length; index++) {
                let td = $(tr[index]).find('td')
                if ( $(td[0]).text()==params) {
                    return existe=true;
              }
            }
            return existe;
        }

        function validateEmail(email) {
            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
            if (!emailReg.test(email)) {
                return false;
            } else {
                return true;
            }
        }


        function EnviarReporteAdministradorComponentes() {
            var path = "/Reportes/Vista.aspx?idReporte=173&index= "+cbotipoRastreoTiemposCRC.val() +"&tipo=1&inMemory=1";
            let correos = [];
            $('#tblCorreos tbody tr').each(function () {
                correos.push($(this).find('td:eq(0)').text());
            });
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                var path = "/Reportes/Vista.aspx?idReporte=180&index="+cbotipoRastreoTiemposCRC.val() +"&tipo=2&inMemory=2";
                ireport.attr("src", path);                
                document.getElementById('report').onload = function () {                                                           
                    $.ajax({                                                     
                        datatype: "json",
                        type: "POST",
                        url: '/Overhaul/EnviarReporteAdministradorComponentes',
                        data: {tipo: cbotipoRastreoTiemposCRC.val(), correos: correos }, 
                        success: function (response) {
                            Alert2Exito("El reporte se envio correctamente"),
                            ModalCorreosAlmacen.modal("hide");
                        },   
                        error: function () {
                            $.unblockUI();
                        }
                    });                
            };
        }
    }
        
        // Vida útil
        function cargarModeloVida() {
            if (cboGrupoVida.val() != "") {
                cboModeloVida.prop("disabled", false);
                cboModeloVida.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboGrupoVida.val() });
            }
            else {
                cboModeloVida.val("");
                cboModeloVida.prop("disabled", true);
            }
        }

        function initGridVidaUtil()
        {
            gridVidaUtil.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {}
            });
        }
        function cargarGridVidaUtil() {
            let grupo = (cboGrupoVida.val() == "" || cboGrupoVida.val() == null) ? -1 : cboGrupoVida.val();
            let modelo = (cboModeloVida.val() == "" || cboModeloVida.val() == null) ? -1 : cboModeloVida.val();
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarRemocionesVidaUtil",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    grupo: grupo,
                    modelo: cboModeloVida.val(),
                    fechaInicio: txtFIVida.val(),
                    fechaFin: txtFFVida.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if(response.remociones.length > 0) { btnReporteVida.prop("disabled", false); }
                        else { btnReporteVida.prop("disabled", true); }
                        gridVidaUtil.bootgrid("clear");
                        gridVidaUtil.bootgrid("append", response.remociones);
                        gridVidaUtil.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function ReporteVidaUtil() {
            $.blockUI({ message: mensajes.PROCESANDO });
            let grupo = (cboGrupoVida.val() == "" || cboGrupoVida.val() == null) ? -1 : cboGrupoVida.val();
            let modelo = (cboModeloVida.val() == "" || cboModeloVida.val() == null) ? -1 : cboModeloVida.val();

            $.ajax({
                url: '/Overhaul/getReporteVidaUtil',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    grupo: grupo,
                    modelo: modelo,
                    fechaInicio: txtFIVida.val(),
                    fechaFin: txtFFVida.val()
                }),
                success: function (response) {
                    ireporteVidaUtil.attr("src", "/Reportes/Vista.aspx?idReporte=163&modelo=" + $("#cboModeloVida option:selected").text() + "&grupo=" + $("#cboGrupoVida option:selected").text());

                    $(window).scrollTop(0);
                    $("#reporteVidaUtil > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteVidaUtil > #reportViewerModal").css("width", "100%");
                    $("#reporteVidaUtil > #reportViewerModal").css("height", "105%");
                    $("#reporteVidaUtil > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //Conjuntos y subconjuntos
        function initGridConjuntos() {
            tblReparaciones.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {}
            });
        }

        function cargarTblReparaciones() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarReporteReparaciones",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    fechaInicio: txtFechaInicioReparaciones.val(),
                    fechaFin: txtFechaFinReparaciones.val(),
                    subconjunto: txtSubconjuntoReparaciones.val().trim(),
                    noComponente: txtNoCompReparaciones.val().trim(),
                    obra: cboObraReparaciones.val(),
                    economico: txtEconomicoReparaciones.val().trim(),
                    proveedor: cboProveedorReparaciones.val(),
                    cotizacion: txtCotizacionReparaciones.val().trim(),
                    comprador: txtCompradorReparaciones.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if (response.reparaciones.length > 0) { btnReporteReparaciones.prop("disabled", false); }
                        else { btnReporteReparaciones.prop("disabled", true); }
                        tblReparaciones.bootgrid("clear");
                        tblReparaciones.bootgrid("append", response.reparaciones);
                        tblReparaciones.bootgrid('reload');

                        txtFechaInicioReparaciones.attr("data-info", txtFechaInicioReparaciones.val());
                        txtFechaFinReparaciones.attr("data-info", txtFechaFinReparaciones.val());
                        txtSubconjuntoReparaciones.attr("data-info", txtSubconjuntoReparaciones.val());
                        txtNoCompReparaciones.attr("data-info", txtNoCompReparaciones.val());
                        cboObraReparaciones.attr("data-info", cboObraReparaciones.val());
                        txtEconomicoReparaciones.attr("data-info", txtEconomicoReparaciones.val());
                        cboProveedorReparaciones.attr("data-info", cboProveedorReparaciones.val());
                        txtCotizacionReparaciones.attr("data-info", txtCotizacionReparaciones.val());                        
                    }
                    else { AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function CargarRptReparacionesPend() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetReporteReparaciones',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({
                    fechaInicio: txtFechaInicioReparaciones.attr("data-info"),
                    fechaFin: txtFechaFinReparaciones.attr("data-info"),
                    subconjunto: txtSubconjuntoReparaciones.attr("data-info").trim(),
                    noComponente: txtNoCompReparaciones.attr("data-info").trim(),
                    obra: cboObraReparaciones.val(),
                    economico: txtEconomicoReparaciones.attr("data-info").trim(),
                    proveedor: cboProveedorReparaciones.attr("data-info"),
                    cotizacion: txtCotizacionReparaciones.attr("data-info").trim()
                }),
                success: function (response) {
                    $.unblockUI();
                    ireporteReparaciones.attr("src", "/Reportes/Vista.aspx?idReporte=172");
                    $(window).scrollTop(0);
                    $("#reporteReparaciones > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteReparaciones > #reportViewerModal").css("width", "100%");
                    $("#reporteReparaciones > #reportViewerModal").css("height", "105%");
                    $("#reporteReparaciones > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //Tiempos CRC
        function initTblTiempos() {
            tblTiemposCRC.bootgrid({
                //headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "trasladoCRC": function (column, row) {
                        var numero = "";
                        if (row.trasladoCRC != -1)
                            numero = row.trasladoCRC.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "desarmado": function (column, row) {
                        var numero = "";
                        if (row.desarmado != -1)
                            numero = row.desarmado.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "autorizacion": function (column, row) {
                        var numero = "";
                        if (row.autorizacion != -1)
                            numero = row.autorizacion.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "armado": function (column, row) {
                        var numero = "";
                        if (row.armado != -1)
                            numero = row.armado.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "recoleccion": function (column, row) {
                        var numero = "";
                        if (row.recoleccion != -1)
                            numero = row.recoleccion.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "trasladoAlmacen": function (column, row) {
                        var numero = "";
                        if (row.trasladoAlmacen != -1)
                            numero = row.trasladoAlmacen.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "diasCRC": function (column, row) {
                        var numero = "";
                        if (row.diasCRC != -1)
                            numero = row.diasCRC.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "diasProceso": function (column, row) {
                        var numero = "";
                        if (row.diasProceso != -1)
                            numero = row.diasProceso.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "diasReparacion": function (column, row) {
                        var numero = "";
                        if (row.diasReparacion != -1)
                            numero = row.diasReparacion.toString();
                        return "<span'>" + numero + "</span>";
                    },
                }
            });
        }

        function initTblRastreoAdminTiemposCRC() {
            tblRastreoAdminTiemposCRC.bootgrid({
                //headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "trasladoCRC": function (column, row) {
                        var numero = "";
                        if (row.trasladoCRC != -1)
                            numero = row.trasladoCRC.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "desarmado": function (column, row) {
                        var numero = "";
                        if (row.desarmado != -1)
                            numero = row.desarmado.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "autorizacion": function (column, row) {
                        var numero = "";
                        if (row.autorizacion != -1)
                            numero = row.autorizacion.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "armado": function (column, row) {
                        var numero = "";
                        if (row.armado != -1)
                            numero = row.armado.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "recoleccion": function (column, row) {
                        var numero = "";
                        if (row.recoleccion != -1)
                            numero = row.recoleccion.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "trasladoAlmacen": function (column, row) {
                        var numero = "";
                        if (row.trasladoAlmacen != -1)
                            numero = row.trasladoAlmacen.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "diasCRC": function (column, row) {
                        var numero = "";
                        if (row.diasCRC != -1)
                            numero = row.diasCRC.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "diasProceso": function (column, row) {
                        var numero = "";
                        if (row.diasProceso != -1)
                            numero = row.diasProceso.toString();
                        return "<span'>" + numero + "</span>";
                    },
                    "diasReparacion": function (column, row) {
                        var numero = "";
                        if (row.diasReparacion != -1)
                            numero = row.diasReparacion.toString();
                        return "<span'>" + numero + "</span>";
                    },
                }
            });
        }

        function cambiarTablaTiemposCRC()
        {
            var tipo = cbotipoRastreoTiemposCRC.val();
            if (tipo == 1) {
                tblRastreoAdminTiemposCRC.parent().hide();
                tblTiemposCRC.parent().show();
            }
            if (tipo == 2) {
                tblTiemposCRC.parent().hide();
                tblRastreoAdminTiemposCRC.parent().show();
            }
        }


        function cargarTblTiemposCRC() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarReporteTiemposCRC",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    fechaInicio: txtFechaInicioTiemposCRC.val(),
                    fechaFin: txtFechaFinTiemposCRC.val(),
                    subconjunto: txtSubconjuntoTiemposCRC.val().trim(),
                    noComponente: txtNoCompTiemposCRC.val().trim(),
                    obra: cboObraTiemposCRC.val(),
                    economico: txtEconomicoTiemposCRC.val().trim(),
                    proveedor: cboProveedorTiemposCRC.val(),
                    cotizacion: txtCotizacionTiemposCRC.val().trim()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if (response.tiempos.length > 0) { btnReporteTiemposCRC.prop("disabled", false); }
                        else { btnReporteTiemposCRC.prop("disabled", true); }
                        tblTiemposCRC.bootgrid("clear");
                        tblTiemposCRC.bootgrid("append", response.tiempos);
                        tblTiemposCRC.bootgrid('reload');
                        tblRastreoAdminTiemposCRC.bootgrid("clear");
                        tblRastreoAdminTiemposCRC.bootgrid("append", response.tiempos);
                        tblRastreoAdminTiemposCRC.bootgrid('reload');

                        txtFechaInicioTiemposCRC.attr("data-info", txtFechaInicioTiemposCRC.val());
                        txtFechaFinTiemposCRC.attr("data-info", txtFechaFinTiemposCRC.val());
                        txtSubconjuntoTiemposCRC.attr("data-info", txtSubconjuntoTiemposCRC.val());
                        txtNoCompTiemposCRC.attr("data-info", txtNoCompTiemposCRC.val());
                        cboObraTiemposCRC.attr("data-info", cboObraTiemposCRC.val());
                        txtEconomicoTiemposCRC.attr("data-info", txtEconomicoTiemposCRC.val());
                        cboProveedorTiemposCRC.attr("data-info", cboProveedorTiemposCRC.val());
                        txtCotizacionTiemposCRC.attr("data-info", txtCotizacionTiemposCRC.val());
                    }
                    else { AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados"); }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function CargarRptTiemposCRCCorreo() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetReporteTiemposCRC',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({
                    fechaInicio: txtFechaInicioTiemposCRC.attr("data-info"),
                    fechaFin: txtFechaFinTiemposCRC.attr("data-info"),
                    subconjunto: txtSubconjuntoTiemposCRC.attr("data-info").trim(),
                    noComponente: txtNoCompTiemposCRC.attr("data-info").trim(),
                    obra: cboObraTiemposCRC.val(),
                    economico: txtEconomicoTiemposCRC.attr("data-info").trim(),
                    proveedor: cboProveedorTiemposCRC.val(),
                    cotizacion: txtCotizacionTiemposCRC.attr("data-info").trim()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (cbotipoRastreoTiemposCRC.val() == 1) { ireporteTiemposCRC.attr("src", "/Reportes/Vista.aspx?idReporte=173"); }
                    if (cbotipoRastreoTiemposCRC.val() == 2) { ireporteTiemposCRC.attr("src", "/Reportes/Vista.aspx?idReporte=180"); }                   
                    $("#reporteTiemposCRC");
                    $("#reporteTiemposCRC");
                    $("#reporteTiemposCRC");
                    $("#reporteTiemposCRC").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
  
        
        function CargarRptTiemposCRC() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetReporteTiemposCRC',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({
                    fechaInicio: txtFechaInicioTiemposCRC.attr("data-info"),
                    fechaFin: txtFechaFinTiemposCRC.attr("data-info"),
                    subconjunto: txtSubconjuntoTiemposCRC.attr("data-info").trim(),
                    noComponente: txtNoCompTiemposCRC.attr("data-info").trim(),
                    obra: cboObraTiemposCRC.val(),
                    economico: txtEconomicoTiemposCRC.attr("data-info").trim(),
                    proveedor: cboProveedorTiemposCRC.val(),
                    cotizacion: txtCotizacionTiemposCRC.attr("data-info").trim()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (cbotipoRastreoTiemposCRC.val() == 1) { ireporteTiemposCRC.attr("src", "/Reportes/Vista.aspx?idReporte=173"); }
                    if (cbotipoRastreoTiemposCRC.val() == 2) { ireporteTiemposCRC.attr("src", "/Reportes/Vista.aspx?idReporte=180"); }
                    $(window).scrollTop(0);
                    $("#reporteTiemposCRC > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteTiemposCRC > #reportViewerModal").css("width", "100%");
                    $("#reporteTiemposCRC > #reportViewerModal").css("height", "105%");
                    $("#reporteTiemposCRC > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //Desecho
        function IniciarTblDesecho() {
            tblDesecho.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {                    
                    "verReporte": function (column, row) {
                        return "<button type='button' class='btn btn-primary ver'  data-index='" + row.id + "'><span class='glyphicon glyphicon-eye-open'></span>  </button>";
                    },                    
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                tblDesecho.find(".ver").parent().css("width", "3%");
                tblDesecho.find(".ver").parent().css("text-align", "center");
                tblDesecho.find(".ver").on('click', function (e) {
                    e.stopPropagation();
                    e.stopImmediatePropagation();
                    abrirReporteDesecho($(this).attr("data-index"));
                });
            });
        }


        function CargarTblDesecho() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: '/Overhaul/CargarReportesDesecho',
                datatype: "json",
                type: "POST",
                data: {
                    fechaInicio: txtFechaInicioDesecho.val().trim(),
                    fechaFinal: txtFechaFinDesecho.val().trim(),
                    noComponente: txtComponenteDesecho.val().trim(),
                    conjunto: cboConjuntoDesecho.val(),
                    subconjunto: cboSubconjuntoDesecho.val()
                },
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {                        
                        tblDesecho.bootgrid("clear");
                        tblDesecho.bootgrid("append", response.reportes);
                        tblDesecho.bootgrid('reload');
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.MESSAGE);
                }
            });
        }

        function CargarSubDesecho() {
            if (cboConjuntoDesecho.val() != null && cboConjuntoDesecho.val() != "") {
                cboSubconjuntoDesecho.fillCombo('/CatComponentes/FillCboSubConjunto_Componente', { idConjunto: cboConjuntoDesecho.val(), idModelo: -1 });
                cboSubconjuntoDesecho.attr('disabled', false);
            }
            else {
                cboSubconjuntoDesecho.clearCombo();
                cboSubconjuntoDesecho.attr('disabled', true);
            }
        }

        function abrirReporteDesecho(idReporteDesecho) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReporteDesecho',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idReporteDesecho: idReporteDesecho }),
                success: function (response) {
                    ireporteDesecho.attr("src", "/Reportes/Vista.aspx?idReporte=164");
                    $(window).scrollTop(0);
                    $("#reporteDesecho > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteDesecho > #reportViewerModal").css("width", "100%");
                    $("#reporteDesecho > #reportViewerModal").css("height", "105%");
                    $("#reporteDesecho > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        // Remociones
        function SelectSubconjuntoRR(event2, ui2) { txtFiltroDescripcionComponenteRR.text(ui2.item.descripcion); }
        function initGridRR() {
            gridReportes.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "estatus": function (column, row) {
                        var estado = "";

                        switch (row.estatus) {
                            case 0:
                                estado = "INCOMPLETO";
                                break;
                            case 1:
                                estado = "BoVo SIN VERIFICAR";
                                break;
                            case 2:
                                estado = "BoVo VERIFICADO";
                                break;
                            case 3:
                                estado = "COMPLETO";
                                break;
                            case 4:
                                estado = "APROBADO";
                                break;
                            default:
                                break;
                        }
                        return "<span class=''> " + estado + "</span>";
                    },
                    "verReporte": function (column, row) {
                        return "<button type='button' class='btn btn-primary ver'  data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID + "' data-id='" + row.id + "' style='" + (row.estatus > 0 ? "" : "display:none") + "' >" +
                                       "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                    },
                    "aprobarReporte": function (column, row) {
                        return "<span class='' style='" + (row.estatus > 3 ? "" : "display:none") + "'> " + row.fechaAprobacionReporte + "</span>" +
                         "<button type='button' class='btn btn-success aprobar'  data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID + "' data-id='" + row.id + "' style='" + (row.estatus == 3 ? "" : "display:none") + "' >" +
                                       "<span class='glyphicon glyphicon-ok'></span>  </button>";
                    },
                    "completarReporte": function (column, row) {
                        return "<button type='button' class='btn btn-info completar'  data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID + "' data-id='" + row.id + "' style='" + (row.estatus < 3 ? "" : "display:none") + "' >" +
                                       "<span class='glyphicon glyphicon-file'></span>  </button>";
                    },
                    "eliminarReporte": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar'  data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID + "' data-id='" + row.id + "' style='" + (row.estatus < 2 ? "" : "display:none") + "' >" +
                                       "<span class='glyphicon glyphicon-remove'></span>  </button>";
                    },
                    "verificarReporte": function (column, row) {

                        return "<span class='' style='" + (row.estatus > 1 ? "" : "display:none") + "'> " + row.fechaAutorizacionBoVo + "</span>" +
                         //"<input class='form-control' id='tbFechaVerificacion' style='" + (row.estatus > 1 ? "" : "display:none") + "' data-fecha='" + row.fechaAutorizacionBoVo + "' disabled />" +

                        "<button type='button' class='btn btn-success verificar'  data-index='" + row.componenteRemovido + "' data-componente='" + row.noComponente + "' data-componenteid='" + row.componenteID + "' data-id='" + row.id + "' style='" + (row.estatus == 1 ? "" : "display:none") + "' >" +
                                       "<span class='glyphicon glyphicon-ok-circle'></span>  </button>";
                    },
                    "motivo": function (column, row) {
                        var stringMotivo = row.motivo == 0 ? "VIDA ÚTIL" : (row.motivo == 1 ? "FALLA" : (row.motivo == 2 ? "ESTRATEGIA" : "DESECHO"));
                        return "<span>" + stringMotivo + "</span>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {

                gridReportes.find(".ver").parent().css("width", "3%");
                gridReportes.find(".ver").parent().css("text-align", "center");
                gridReportes.find(".aprobar").parent().css("width", "3%");
                gridReportes.find(".aprobar").parent().css("text-align", "center");
                gridReportes.find(".completar").parent().css("width", "3%");
                gridReportes.find(".completar").parent().css("text-align", "center");
                gridReportes.find(".eliminar").parent().css("width", "3%");
                gridReportes.find(".eliminar").parent().css("text-align", "center");
                gridReportes.find(".verificar").parent().css("width", "3%");
                gridReportes.find(".verificar").parent().css("text-align", "center");

                gridReportes.find(".completar").on('click', function (e) {
                    window.location.href = "/Overhaul/Remocion?id=" + $(this).attr("data-componenteid");
                });

                gridReportes.find(".eliminar").on('click', function (e) {
                    idReporteRR = $(this).attr("data-id");
                    ConfirmacionEliminacion("Eliminar Reporte de Remoción", "¿Esta seguro que desea dar de baja el reporte de remoción para el componente \"" + $(this).attr("data-componente") + "\"?");

                });

                gridReportes.find(".ver").on('click', function (e) {
                    banderaRR = 0;
                    abrirReporte($(this).attr("data-id"));
                });

                gridReportes.find(".aprobar").on('click', function (e) {
                    banderaRR = 2;
                    idReporteRR = $(this).attr("data-id");
                    ConfirmacionEliminacion("Aprobación de reporte de remoción", "Se aprobará el reporte de remoción para el componente \"" + $(this).attr("data-componente") + "\", ¿Desea continuar?");
                });
                gridReportes.find(".verificar").on('click', function (e) {
                    banderaRR = 1;
                    idReporteRR = $(this).attr("data-id");
                    ConfirmacionEliminacion("Verificar", "Se aprobará el reporte de remoción para el componente \"" + $(this).attr("data-componente") + "\", ¿Desea continuar?");
                });
            });
        }
        function cargarGridRR() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarReportesRemocion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    estatus: parseInt(cboEstatusRR.val()),
                    descripcionComponente: txtFiltroDescripcionComponenteRR.val().trim(),
                    noEconomico: txtFiltroEconomicoRR.val() == "" ? -1 : txtFiltroEconomicoRR.val().trim(),
                    cc: cboCCRR.val(),
                    motivoRemocion: cboMotivoRemocion.val() == "" ? -1 : cboMotivoRemocion.val(),                    
                    fechaFinal: txtFechaFinRR.val(),
                    noComponente: txtNoComponenteRR.val().trim(),
                    fechaInicio: txtFechaInicioRR.val(),
                    modelos: cboModeloRR.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        if(response.reportes.length > 0) { btnReporteRR.prop("disabled", false); }
                        else { btnReporteRR.prop("disabled", true); }
                        gridReportes.bootgrid("clear");
                        gridReportes.bootgrid("append", response.reportes);
                        gridReportes.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function eliminarReporte() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/EliminarReportesRemocion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: idReporteRR }),
                success: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "El reporte se eliminó correctamente");
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function abrirReporte(idReporteRemocion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReporteRemocionComponente',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idReporteRemocion: idReporteRemocion, tipoVista: 0 }),
                success: function (response) {
                    ireporteRemocion.attr("src", response.html);
                    $(window).scrollTop(0);
                    $("#reporteRemocion > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteRemocion > #reportViewerModal").css("width", "100%");
                    $("#reporteRemocion > #reportViewerModal").css("height", "105%");
                    $("#reporteRemocion > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function abrirReportePorFiltros() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReportesRemocionComponenteGrupo',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    estatus: cboEstatusRR.val(),
                    descripcionComponente: txtFiltroDescripcionComponenteRR.val(),
                    noEconomico: txtFiltroEconomicoRR.val() == "" ? -1 : txtFiltroEconomicoRR.val(),
                    cc: cboCCRR.val(),
                    motivoRemocion: cboMotivoRemocion.val() == "" ? -1 : cboMotivoRemocion.val()     ,               
                    fechaInicio: txtFechaInicioRR.val(),
                    fechaFinal: txtFechaFinRR.val(),
                    modelos: cboModeloRR.val()
                }),
                success: function (response) {
                    ireporteRemocion.attr("src", "/Reportes/Vista.aspx?idReporte=151");

                    $(window).scrollTop(0);
                    $("#reporteRemocion > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteRemocion > #reportViewerModal").css("width", "100%");
                    $("#reporteRemocion > #reportViewerModal").css("height", "105%");
                    $("#reporteRemocion > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function aprobarReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/aprobarReporteRemocion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idReporte: idReporteRR }),
                success: function (response) {

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function verificarReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/verificarReporteRemocion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ idReporte: idReporteRR }),
                success: function (response) {

                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        // Histórico Valor Almacén
        function initGridValorAlmacen() {
            gridValorAlmacen.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "enero": function (column, row) { return "<span class='cantidad'>" + (row.enero == 0 ? "-" : row.enero) + "</span>"; },
                    "febrero": function (column, row) { return "<span class='cantidad'>" + (row.febrero == 0 ? "-" : row.febrero) + "</span>"; },
                    "marzo": function (column, row) { return "<span class='cantidad'>" + (row.marzo == 0 ? "-" : row.marzo) + "</span>"; },
                    "abril": function (column, row) { return "<span class='cantidad'>" + (row.abril == 0 ? "-" : row.abril) + "</span>"; },
                    "mayo": function (column, row) { return "<span class='cantidad'>" + (row.mayo == 0 ? "-" : row.mayo) + "</span>"; },
                    "junio": function (column, row) { return "<span class='cantidad'>" + (row.junio == 0 ? "-" : row.junio) + "</span>"; },
                    "julio": function (column, row) { return "<span class='cantidad'>" + (row.julio == 0 ? "-" : row.julio) + "</span>"; },
                    "agosto": function (column, row) { return "<span class='cantidad'>" + (row.agosto == 0 ? "-" : row.agosto) + "</span>"; },
                    "septiembre": function (column, row) { return "<span class='cantidad'>" + (row.septiembre == 0 ? "-" : row.septiembre) + "</span>"; },
                    "octubre": function (column, row) { return "<span class='cantidad'>" + (row.octubre == 0 ? "-" : row.octubre) + "</span>"; },
                    "noviembre": function (column, row) { return "<span class='cantidad'>" + (row.noviembre == 0 ? "-" : row.noviembre) + "</span>"; },
                    "diciembre": function (column, row) { return "<span class='cantidad'>" + (row.diciembre == 0 ? "-" : row.diciembre) + "</span>"; },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {

                //gridValorAlmacen.find(".inventarioLocacion").parent().css("width", "5%");
                //gridValorAlmacen.find(".inventarioLocacion").parent().css("text-align", "center");
                //gridValorAlmacen.find(".total").parent().css("text-align", "center");
                //gridInventario.find(".completar").on('click', function (e) {
                //});
            });
        }
        function cargarGridValorAlmacen() {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarReporteValorAlmacen",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    anioInicial: cboAnioValorAlmacen.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridValorAlmacen.bootgrid({
                            rowCount: -1,
                            templates: {
                                header: ""
                            }
                        });
                        gridValorAlmacen.bootgrid("clear");
                        gridValorAlmacen.bootgrid("append", response.valoralmacen);
                        gridValorAlmacen.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }


        function abrirReporteHistoricoAlmacen() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/getReporteHistoricoAlmacen',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    anio: cboAnioValorAlmacen.val()
                }),
                success: function (response) {
                    ireportValorAlmacen.attr("src", "/Reportes/Vista.aspx?idReporte=153");
                    $(window).scrollTop(0);
                    $("#reporteValorAlmacen > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteValorAlmacen > #reportViewerModal").css("width", "100%");
                    $("#reporteValorAlmacen > #reportViewerModal").css("height", "105%");
                    $("#reporteValorAlmacen > #reportViewerModal > #report").onload = function () {
                       

                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //Listado Maestro
        function initGridMaestro()
        {
            gridMaestro.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "causa":
                        function (column, row) {
                            return "<span class='cantidad'>" + row.tipo + "</span>";
                        },
                    "detalle":
                        function (column, row) {
                            return "<button type='button' class='btn btn-primary detalle' data-index='" + row.idPlaneacionOH + "'  data-obra='"
                                + row.obra + "' data-periodo='" + "X" + "' data-obramaquina='" + "X" + "' data-noeconomico='" + row.equipo + "' data-ritmo='" + row.ritmo
                                + "' data-hrscomp='" + row.horasComponente + "' data-target='" + row.target + "' data-fechapcr='" + row.fechaPCR + "' data-causa='" + row.tipo + "'>" +
                                        "<span class='glyphicon glyphicon-eye-open'></span>  </button>";
                        },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {

                gridMaestro.find(".detalle").parent().css("width", "3%");
                gridMaestro.find(".detalle").parent().css("text-align", "center");
                //gridMaestro.find(".total").parent().css("text-align", "center");
                gridMaestro.find(".detalle").on('click', function (e) {
                    let datosListado = {
                        obra: $(this).attr("data-obra").toUpperCase(),
                        periodo: $(this).attr("data-periodo").toUpperCase(),
                        fecha: new Date(),
                        obraMaquina: $(this).attr("data-obramaquina").toUpperCase(),
                        noEconomico: $(this).attr("data-noeconomico").toUpperCase(),
                        ritmo: $(this).attr("data-ritmo"),
                        horasComponente: $(this).attr("data-hrscomp"),
                        target: $(this).attr("data-target"),
                        proximoPCR: $(this).attr("data-fechapcr"),
                        cause: $(this).attr("data-causa").toUpperCase(),
                        elaboro: "-",
                        facilitador: "-",
                        reviso: "-"
                    }
                    CargarRptListadoMaestro($(this).attr("data-index"), datosListado);
                });
            });
        }
        function cargarGridMaestro()
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarReporteMaestro",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    idCalendario: cboCalendarioMaestro.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridMaestro.bootgrid({
                            rowCount: -1,
                            templates: {
                                header: ""
                            }
                        });
                        gridMaestro.bootgrid("clear");
                        gridMaestro.bootgrid("append", response.maestro);
                        gridMaestro.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function CargarRptListadoMaestro(idEvento, datos)
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetReporteListadoMaestro',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({ idEvento: idEvento, datos: datos }),
                success: function (response) {
                    $.unblockUI();
                    ireportMaestro.attr("src", response.html);
                    $(window).scrollTop(0);
                    $("#reporteMaestro > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteMaestro > #reportViewerModal").css("width", "100%");
                    $("#reporteMaestro > #reportViewerModal").css("height", "105%");
                    $("#reporteMaestro > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        
        function CargarDatosDetalleMaestro(id)
        {
            $.blockUI({ message: "Procesando..." });
            $.ajax({
                url: "/Overhaul/CargarDatosDetalleMaestro",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    idPlaneacionOH: id
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridDetallesModalMaestro.bootgrid({
                            rowCount: -1,
                            templates: {
                                header: ""
                            }
                        });
                        gridDetallesModalMaestro.bootgrid("clear");
                        gridDetallesModalMaestro.bootgrid("append", response.detalles);
                        gridDetallesModalMaestro.bootgrid('reload');
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //Component List
        function intiGridComponent() {
            gridComponent.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                sorting: false,
                formatters: {
                    "fechaRaw": function (column, row) {
                        return "<span class='fechaInstalacion'>" + row.fecha + "</span>";
                    },
                    "fechaProxRemocionRaw": function (column, row) {
                        return "<span class='fechaPCR'>" + row.fechaProxRemocion + "</span>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {

            });
        }
        function cargarGridComponent()
        {
            $.blockUI({ message: "Procesando..." });
            var locacion = cboLocacionComponent.val();
            var serie = noComponenteComponent.val();
            var conjunto = cboConjuntoComponent.val();
            var subconjunto = cboSubconjuntoComponent.val();
            $.ajax({
                url: "/Overhaul/CargarComponentList",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    locacion: locacion,
                    noComponente: serie,
                    conjunto: conjunto,
                    subconjunto: subconjunto,
                    modelo: cboModeloComponent.val(),
                    obra: cboObraComponent.val()
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridComponent .bootgrid("clear");
                        gridComponent.bootgrid("append", response.componentes);
                        gridComponent.bootgrid('reload');
                        if (response.componentes.length > 0) { btnReporteComponent.prop("disabled", false); }
                        else { btnReporteComponent.prop("disabled", true); }

                        cboLocacionComponent.attr("data-info", cboLocacionComponent.val());
                        noComponenteComponent.attr("data-info", noComponenteComponent.val());
                        cboConjuntoComponent.attr("data-info", cboConjuntoComponent.val());
                        cboSubconjuntoComponent.attr("data-info", cboSubconjuntoComponent.val());
                        cboModeloComponent.attr("data-info", cboModeloComponent.val());
                        cboObraComponent.attr("data-info", cboObraComponent.val());
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function cargarSubconjuntosComponent() {
            var conjunto = cboConjuntoComponent.val();
            if (conjunto == null || conjunto == "") {
                cboSubconjuntoComponent.clearCombo();
                cboSubconjuntoComponent.prop("disabled", true);
            }
            else {
                cboSubconjuntoComponent.fillCombo('/Overhaul/FillCboSubconjuntos', { conjunto: conjunto });
                cboSubconjuntoComponent.prop("disabled", false);
            }
        }
        function RecargarLocaciones()
        {
            var modelosID = cboModeloComponent.val();
            cboLocacionComponent.clearCombo();
            cboLocacionComponent.fillCombo('/Overhaul/FillCboLocacionesComponentList', { modelosID: modelosID });
        }
        function CargarRptComponentList() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetReporteComponentList',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({
                    locacion: cboLocacionComponent.attr("data-info"),
                    noComponente: noComponenteComponent.attr("data-info"),
                    conjunto: cboConjuntoComponent.attr("data-info"),
                    subconjunto: cboSubconjuntoComponent.attr("data-info"),
                    modelo: cboModeloComponent.attr("data-info") == "" ? null : cboModeloComponent.attr("data-info").split(','),
                    obra: cboObraComponent.attr("data-info")
                }),
                success: function (response) {
                    $.unblockUI();
                    ireporteComponent.attr("src", "/Reportes/Vista.aspx?idReporte=178");
                    $(window).scrollTop(0);
                    $("#reporteComponent > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteComponent > #reportViewerModal").css("width", "100%");
                    $("#reporteComponent > #reportViewerModal").css("height", "105%");
                    $("#reporteComponent > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //Inventario
        function intiGridInventario() {
            gridInventario.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                //sorting: false,
                formatters: {
                    "fechaRaw": function (column, row) {
                        return "<span class='fechaInstalacion'>" + row.fecha + "</span>";
                    },
                    "diasAlmacenado": function (column, row) {
                        return "<span class='diasAlmacenado'>" + (row.diasAlmacenado < 0 ? "--" : row.diasAlmacenado) + "</span>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {

            });
        }

        function cargarGridInventario() {
            $.blockUI({ message: "Procesando..." });
            var locacion = cboLocacionInventario.val();
            var serie = noComponenteInventario.val();
            var conjunto = cboConjuntoInventario.val();
            var subconjunto = cboSubconjuntoInventario.val();
            var fechaInicio = fechaInicioInventario.val();
            var fechaFin = fechaFinInventario.val();
            var modelo = cboModeloComponent.val();
            $.ajax({
                url: "/Overhaul/CargarInventarioComponentes",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    locacion: locacion,
                    noComponente: serie,
                    conjunto: conjunto,
                    subconjunto: subconjunto,
                    fechaInicio: fechaInicio,
                    fechaFin: fechaFin,
                    modelo: modelo
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridInventario.bootgrid("clear");
                        gridInventario.bootgrid("append", response.componentes);
                        gridInventario.bootgrid('reload');
                        if (response.componentes.length > 0) { btnReporteInventario.prop("disabled", false); }
                        else { btnReporteInventario.prop("disabled", true); }

                        cboLocacionInventario.attr("data-info", cboLocacionInventario.val());
                        noComponenteInventario.attr("data-info", noComponenteInventario.val());
                        cboConjuntoInventario.attr("data-info", cboConjuntoInventario.val());
                        cboSubconjuntoInventario.attr("data-info", cboSubconjuntoInventario.val());
                        fechaInicioInventario.attr("data-info", fechaInicioInventario.val());
                        fechaFinInventario.attr("data-info", fechaFinInventario.val());
                        cboModeloComponent.attr("data-info", cboModeloComponent.val());
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function cargarSubconjuntosInventario() {
            var conjunto = cboConjuntoInventario.val();
            if (conjunto == null || conjunto == "") {
                cboSubconjuntoInventario.clearCombo();
                cboSubconjuntoInventario.prop("disabled", true);
            }
            else {
                cboSubconjuntoInventario.fillCombo('/Overhaul/FillCboSubconjuntos', { conjunto: conjunto });
                cboSubconjuntoInventario.prop("disabled", false);
            }
        }

        function CargarRptInventario() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/Overhaul/GetReporteInventarioComponentes',
                type: 'POST',
                dataType: 'json',
                async: false,
                contentType: 'application/json',
                data: JSON.stringify({
                    locacion: cboLocacionInventario.attr("data-info"),
                    noComponente: noComponenteInventario.attr("data-info"),
                    conjunto: cboConjuntoInventario.attr("data-info"),
                    subconjunto: cboSubconjuntoInventario.attr("data-info"),
                    fechaInicio: fechaInicioInventario.attr("data-info"),
                    fechaFin: fechaFinInventario.attr("data-info"),
                    modelo: null
                }),
                success: function (response) {
                    $.unblockUI();
                    ireporteInventario.attr("src", "/Reportes/Vista.aspx?idReporte=179");
                    $(window).scrollTop(0);
                    $("#reporteInventario > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteInventario > #reportViewerModal").css("width", "100%");
                    $("#reporteInventario > #reportViewerModal").css("height", "105%");
                    $("#reporteInventario > #reportViewerModal > #report").onload = function () {
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        //Componentes en reparación

        function SelectSubconjuntoCR(event2, ui2) { subconjuntoCompReparacion.text(ui2.item.descripcion); }

        function cargarModeloCompReparacion() {
            if (cboGrupoCompReparacion.val() != "") {
                cboModeloCompReparacion.prop("disabled", false);
                cboModeloCompReparacion.fillCombo('/CatComponentes/FillCboModelo_Componente', { idGrupo: cboGrupoCompReparacion.val() });
            }
            else {
                cboModeloCompReparacion.val("");
                cboModeloCompReparacion.prop("disabled", true);
            }
        }

        function initGridCompReparacion()
        {
            gridCompReparacion.bootgrid({
                headerCssClass: '.bg-table-header',
                headerAlign: 'center',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "fechaRaw": function (column, row) {
                        return "<span class='fechaInstalacion'>" + row.fecha + "</span>";
                    },

                }
            }).on("loaded.rs.jquery.bootgrid", function () {

            });
        }


        //grid de reparacion
        function cargarGridCompReparacion() {
            $.blockUI({ message: "Procesando..." });
            var locacion = cboLocacionCompReparacion.val();
            var subconjunto = subconjuntoCompReparacion.val();
            var grupo = cboGrupoCompReparacion.val();
            var modelo = cboModeloCompReparacion.val();
            $.ajax({
                url: "/Overhaul/CargarCompReparacion",
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({
                    locacion: locacion,
                    subconjunto: subconjunto,
                    grupo: grupo,
                    modelo: modelo
                }),
                success: function (response) {
                    $.unblockUI();
                    if (response.success) {
                        gridCompReparacion.bootgrid("clear");
                        gridCompReparacion.bootgrid("append", response.data);
                        gridCompReparacion.bootgrid('reload');
                        if (response.data.length > 0)
                        {
                            btnReporteCompReparacion.prop("disabled", false);
                        }
                        else
                        {
                            btnReporteCompReparacion.prop("disabled", true);
                        }

                        cboLocacionCompReparacion.attr("data-info", cboLocacionCompReparacion.val());
                        cboGrupoCompReparacion.attr("data-info", cboGrupoCompReparacion.val());
                        cboModeloCompReparacion.attr("data-info", cboModeloCompReparacion.val());
                        subconjuntoCompReparacion.attr("data-info", subconjuntoCompReparacion.val());
                    }
                    else {
                        AlertaGeneral("Alerta", "No se obtuvieron registros con los filtros seleccionados");
                    }
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }

        function ReporteCompReparacion()
        {
            $.blockUI({ message: mensajes.PROCESANDO });
            
            $.ajax({
                url: '/Overhaul/GetReporteCompReparacion',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                
                success: function (response)
                {
                   
                    ireporteCompReparacion.attr("src", "/Reportes/Vista.aspx?idReporte=192");

                    $(window).scrollTop(0);

                    $("#reporteCompReparacion > #reportViewerModal > body").css("overflow", "hidden");
                    $("#reporteCompReparacion > #reportViewerModal").css("width", "100%");
                    $("#reporteCompReparacion > #reportViewerModal").css("height", "105%");
                    $("#reporteCompReparacion > #reportViewerModal > #report").onload = function () {
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
        maquinaria.overhaul.reportesadministradorcomponentes = new reportesadministradorcomponentes();
    });

})();