 //<reference path="AsignacionMaquinariaJS.js" />
(function () {

    $.namespace('maquinaria.Inventario.SolicitudesEquipo.AsignacionMaquinaria');

    AsignacionMaquinaria = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        idSolicitudGlobal = 0;


        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        idSolicitudDetalle = 0;

        ireport = $("#report ")
        btnVerReporte = $("#btnVerReporte"),
        lblfolio = $("#lblfolio"),
        cboTipoFiltro = $("#cboTipoFiltro");
        lblTitleLengedSolicitudes = $("#lblTitleLengedSolicitudes"),
        btnEliminarRegistro = $("#btnEliminarRegistro"),
        modalEliminarRegistro = $("#modalEliminarRegistro"),
        btnGuardarSolicitud = $("#btnGuardarSolicitud"),
        btnGuardarAsignacion = $("#btnGuardarAsignacion"),
        BntRegresar = $("#BntRegresar"),
        divDetalle = $("#divDetalle"),
        divPrincipal = $("#divPrincipal"),
        btnGuardarTodos = $("#btnGuardarTodos"),
        tblRastreoAsignados = $("#tblRastreoAsignados"),
        tblEquiposAsignados = $("#tblEquiposAsignados"),
        tblDetalleSolicitud = $("#tblDetalleSolicitud"),
        tblSolicitudesAutorizadas = $("#tblSolicitudesAutorizadas");

        function init() {

            lblTitleLengedSolicitudes.text('SOLICITUDES CON MAQUINARIA PENDIENTE DE ASIGNAR');
            setGrid();
            bootG('/SolicitudEquipo/GetDataSolicitudesAutorizadasAsignacion');
            eventListers();

        }

        function eventListers() {
            btnGuardarTodos.click(AsignarTodos);
            BntRegresar.click(Regresar);
            btnGuardarAsignacion.click(GuardadoParcial);
            btnGuardarSolicitud.click(GuardarAsignados);
            btnEliminarRegistro.click(EliminarDetalle);
            cboTipoFiltro.change(ChangeFiltro);
            btnVerReporte.click(VerReportes);
        }

        function ChangeFiltro() {
            bootG('/SolicitudEquipo/GetDataSolicitudesAutorizadasAsignacion');
        }

        function VerReportes() {
            CentroCostosGlobal = $(this).attr('data-CC');
            LoadReporte(idSolicitudGlobal, CentroCostosGlobal);
        }

        function LoadReporte(idSolicitud, CC) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/GetReporteNuevAutorizacion',
                type: "POST",
                datatype: "json",
                data: { obj: idSolicitud },
                success: function (response) {
                    var path = "/Reportes/Vista.aspx?idReporte=12&pCC=" + CC;
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function EliminarRegistroSolicitud() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/EliminarRegistroSolicitud',
                type: "POST",
                datatype: "json",
                data: { objDetalleID: btnEliminarRegistro.attr('data-id'), solicitudID: idSolicitudGlobal },
                success: function (response) {
                    GetSolicitdDetalleTable(idSolicitudGlobal);

                    AlertaGeneral('Confirmacion', 'Los registros fueron eliminados correctamente');

                    btnEliminarRegistro.removeAttr('data-id');
                    modalEliminarRegistro.modal('hide');
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function Regresar() {
            btnVerReporte.removeAttr('data-CC');
            divDetalle.addClass('hide');
            divPrincipal.removeClass('hide');
            lblfolio.text('');
        }

        function bootG(url) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                data: { filtro: cboTipoFiltro.val() },
                success: function (response) {
                    $.unblockUI();
                    idSolicitudGlobal = 0;
                    var SolicitudesPendientes = response.Pendientes;
                    tblSolicitudesAutorizadas.bootgrid("clear");
                    tblSolicitudesAutorizadas.bootgrid("append", SolicitudesPendientes);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setGrid() {
            tblSolicitudesAutorizadas.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "VerSolicitud": function (column, row) {

                        ClassColor = 'btn-primary'
                        if (row.hasAsignacion == 1) {
                            ClassColor = 'btn-success'
                        }

                        return "<button type='button' class='btn " + ClassColor + " verSolicitud' data-id='" + row.id + "' data-tipoAsignacion='" + row.tipoAsignacion + "' >" +
                            "<span class='glyphicon glyphicon-ok'></span> " +
                                   " </button>";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblSolicitudesAutorizadas.find(".verSolicitud").on("click", function (e) {
                    var tipo = $(this).attr('data-tipoAsignacion');

                    if (cboTipoFiltro.val() == "1" || cboTipoFiltro == "3")
                    {
                        $('a[href="#divAsignacionPendiente"]').trigger('click');
                    }
                    else {
                        $('a[href="#divAsigacionesRealizadas"]').trigger('click');
                    }

                    if (tipo == "1") {
                        var idSolicitud = $(this).attr('data-id'); // id Solicitud 
                        idSolicitudGlobal = Number(idSolicitud);
                        GetSolicitdDetalleTable(Number(idSolicitud));
                    }
                    else {
                        var idSolicitud = $(this).attr('data-id'); // id Solicitud 
                        idSolicitudGlobal = Number(idSolicitud);
                        GetSolicitdDetalleTableReemplazo(Number(idSolicitud));
                        
                    }
                });
            });
            tblDetalleSolicitud.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                formatters: {
                    "Economicos": function (column, row) {
                        return row.Economico;
                    },
                    "Asignar": function (column, row) {
                        return "<button type='button' class='btn btn-success Asignar' data-id='" + row.id + "' data-tipoAsignacion='" + row.tipoAsignacion + "' >" +
                            "<span class='glyphicon glyphicon-plus'></span> " +
                                   " </button>";
                    },
                    "FechaPromesa": function (column, row) {
                        return "<input type='text' class='form-control tbFechaPromesa' data-id='" + row.id + "'>"
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                $(".tbFechaPromesa").datepicker();
                var rows = tblDetalleSolicitud.bootgrid('getCurrentRows');

                fillComboTable(rows);
                /* Executes after data is loaded and rendered */
                tblDetalleSolicitud.find(".Asignar").on("click", function (e) {
                    var tipoAsignacion = $(this).attr('data-tipoAsignacion');
                    var idSolicitudDetalle = $(this).attr('data-id'); // id Solicitud Detalle
                    var GetEconomico = $('select[data-id="' + idSolicitudDetalle + '"]').find("option:selected").text();
                    var fechaPromesa = $('input.tbFechaPromesa[data-id$="' + idSolicitudDetalle + '"]').val()
                    dataListaAsignados = [];

                    var dataDetalleSolicitud = $('#tblDetalleSolicitud').bootgrid().data('.rs.jquery.bootgrid').rows;
                    dataListaAsignados = $("#tblEquiposAsignados").bootgrid().data('.rs.jquery.bootgrid').rows;
                    var objDataListAsignados = {};
                    objSolicitudAsignado = $.grep(dataDetalleSolicitud,
                                  function (o, i) { return o.id != Number(idSolicitudDetalle); },
                                  true);
                    temp = objSolicitudAsignado[0];
                    if (temp.idNoEconomico != 0 && (temp.pFechaObra != "" && temp.pFechaObra != '01/01/0001')) {
                        var DataSend = [];
                        var objSave = {};

                        objSave.id = idSolicitudDetalle;
                        objSave.idNoEconomico = temp.idNoEconomico;
                        objSave.pFechaInicio = temp.pFechaInicio;
                        objSave.pFechaFin = temp.pFechaFin;
                        objSave.pFechaObra = temp.pFechaObra;
                        objSave.tipoAsignacion = Number(tipoAsignacion);
                        DataSend.push(objSave);

                        GuardarAsignados(DataSend, tipoAsignacion);
                        AlertaGeneral('Confirmación', 'Se asigno correctamente');
                    }
                    else {
                        AlertaGeneral('Alerta', 'Se debe seleccionar uno de los campos validos');
                    }

                });

                tblDetalleSolicitud.find(".tbFechaPromesa").on("change", function (e) {
                    var idSolicitudDetalle = $(this).attr('data-id'); // id Solicitud Detalle
                    var ListaRows = tblDetalleSolicitud.bootgrid().data('.rs.jquery.bootgrid').rows;
                    var currentValue = $(this).val();
                    for (var i = 0; i < ListaRows.length; i++) {
                        if (ListaRows[i].id == idSolicitudDetalle) {
                            ListaRows[i].pFechaObra = currentValue;
                        }
                    }
                });

                /* Executes after data is loaded and rendered */
                tblDetalleSolicitud.find(".clsEconomico").on("change", function (e) {
                    var CurrentElemento = $(this);
                    var idSolicitudDetalle = Number($(this).attr('data-id'));
                    var currentValue = $(CurrentElemento).val();
                    var listaEconomicos = $(".clsEconomico")
                    var count = 0;
                    $.get('/SolicitudEquipo/getAsginaciones', { economicoID: CurrentElemento.val() })
                        .then(response => {
                            if (response.success) {
                                // Operación exitosa.
                                if(response.asignacionExiste)
                                {
                                    CurrentElemento.val(0);
                                    return AlertaGeneral('Alerta','El equipo no puede ser asignado, ya que se encuentra con controles pendientes, terminar el proceso para poder continuar con el proceso.');
                                }
                                else if(response.redireccionamientoVenta)
                                {
                                    CurrentElemento.val(0);
                                    return AlertaGeneral('Alerta','El equipo no puede ser asignado, ya que se encuentra marcado como equipo para venta.');
                                }
                                else{
                                   return true;
                                }
                            } else {
                                // Operación no completada.
                                AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                            }
                        }, error => {
                            // Error al lanzar la petición.
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        ).then(value=>{
                            if(value)
                            { $.each(listaEconomicos, function (index, value) {
                                var indexValue = $(value).val();
        
                                if (indexValue != "0") {
                                    if ( Number(indexValue) > 3) {
                                        if (indexValue == currentValue) {
                                            count++;
                                        }
                                    }
                                }
                            });
                            var CurrentText = CurrentElemento.find("option:selected").text();
                            $("#list option:selected").text();
                            if (count >= 2) {
                                AlertaGeneral("Alerta", "Ya selecciono el economico " + CurrentText + " , favor de seleccionar uno diferente");
                                $(this).val('');
                            }
                            else {
                                var ListaRows = tblDetalleSolicitud.bootgrid().data('.rs.jquery.bootgrid').rows;
        
                                for (var i = 0; i < ListaRows.length; i++) {
                                    if (ListaRows[i].id == idSolicitudDetalle) {
                                        ListaRows[i].idNoEconomico = currentValue;
                                    }
                                }
                            }
                        }
                        });
                });

                /* Executes after data is loaded and rendered */
                tblDetalleSolicitud.find(".Eliminar").on("click", function (e) {
                    var idSolicitudDetalle = $(this).attr('data-id'); // id Solicitud Detalle

                    btnEliminarRegistro.attr('data-id', idSolicitudDetalle);
                    modalEliminarRegistro.modal('show');

                });
            });

            tblEquiposAsignados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "Reemplazo": function (column, row) {
                        return "<button type='button' class='btn btn-danger Reemplazo' data-id='" + row.id + "' data-tipoAsignacion='" + row.tipoAsignacion + "'>" +
                             "<span class='glyphicon glyphicon-remove'></span> " +
                                    " </button>";
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblEquiposAsignados.find(".Reemplazo").on("click", function (e) {
                    idSolicitudDetalle = $(this).attr('data-id'); // id Solicitud Detalle
                    btnEliminarRegistro.attr('data-id', idSolicitudDetalle);
                    modalEliminarRegistro.modal('show');

                    //  EliminarDetalle(idSolicitudDetalle);

                });
            });

            tblRastreoAsignados.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "Rastreo": function (column, row) {
                        var barEstatus1 = "";
                        var barEstatus2 = "";
                        var barEstatus3 = "";
                        var barEstatus4 = "";
                        var barEstatus5 = "";


                        if (row.Estado >= 1) {
                            barEstatus1 = " <div class=\"progress-bar progress-bar-danger\" role=\"progressbar\" style=\"width:14%\"> " +
                                                    "Asignado" +
                                                    " </div> ";
                        }
                        if (row.Estado >= 2) {
                            barEstatus2 = " <div class=\"progress-bar progress-bar-warning\" role=\"progressbar\" style=\"width:19%\"> " +
                                                    "Calidad envío" +
                                                    " </div> ";
                        }
                        if (row.Estado >= 3) {
                            barEstatus3 = " <div class=\"progress-bar progress-bar-success\" role=\"progressbar\" style=\"width:19%\"> " +
                                                    "Control envío" +
                                                    " </div> ";
                        }
                        if (row.Estado >= 4) {
                            barEstatus4 = " <div class=\"progress-bar progress-bar-info\" role=\"progressbar\" style=\"width:25%\"> " +
                                                    "Calidad recepción" +
                                                    " </div> ";
                        }
                        if (row.Estado >= 5) {
                            barEstatus5 = " <div class=\"progress-bar progress-bar-primary\" role=\"progressbar\" style=\"width:23%\"> " +
                                                   "Control recepción" +
                                                    " </div> ";
                        }

                        return barEstatus1 + barEstatus2 + barEstatus3 + barEstatus4 + barEstatus5;
                    }

                }
            });

        }

        function EliminarDetalle() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/EliminarAsignacion',
                type: "POST",
                datatype: "json",
                data: { objDetalleID: btnEliminarRegistro.attr('data-id'), solicitudID: idSolicitudGlobal },
                success: function (response) {

                    if (response.success == true) {
                        dataListaAsignados = $("#tblEquiposAsignados").bootgrid().data('.rs.jquery.bootgrid').rows;
                        objSolicitudAsignado = $.grep(dataListaAsignados,
                                      function (o, i) { return o.id == Number(idSolicitudDetalle); },
                                      true);
                        tblEquiposAsignados.bootgrid("clear");
                        tblEquiposAsignados.bootgrid("append", objSolicitudAsignado);
                        modalEliminarRegistro.modal('hide');
                        GetSolicitdDetalleTable(idSolicitudGlobal);
                        AlertaGeneral('Confirmacion', 'Los registros fueron eliminados correctamente');
                    }
                    else {
                        AlertaGeneral('Alerta', 'El registro que desea eliminar no puede ser eliminado ya que contiene un control de calidad y/o envio');
                    }

                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function GuardadoParcial() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/GuardarParcialAsignaciones',
                type: "POST",
                datatype: "json",
                data: { objParcial: GetObjParcial(), solicitudID: idSolicitudGlobal },
                success: function (response) {

                    AlertaGeneral('Confirmacion', 'Se Guardo Correctamente')
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function GuardarAsignados(DataSend, tipoAsignacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/GuardarAsignaciones',
                type: "POST",
                datatype: "json",
                data: { objAsignados: DataSend, solicitudID: idSolicitudGlobal },
                success: function (response) {
                    if (tipoAsignacion == "1") {
                        GetSolicitdDetalleTable(idSolicitudGlobal);
                    }
                    else {
                        GetSolicitdDetalleTableReemplazo(idSolicitudGlobal);
                    }
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function GetObjAsignados() {
            var DataSend = [];
            var DataGrid = tblEquiposAsignados.bootgrid().data('.rs.jquery.bootgrid').rows;
            for (var i = 0; i < DataGrid.length; i++) {
                var objTemp = {};
                var objSave = {};

                objTemp = DataGrid[i];

                objSave.id = objTemp.id;
                objSave.idNoEconomico = objTemp.idNoEconomico;
                objSave.pFechaObra = objTemp.FechaPromesa;
                DataSend.push(objSave);


            }
            return DataSend;
        }

        function GetObjParcial() {

            var DataSend = [];
            var DataGrid = tblDetalleSolicitud.bootgrid().data('.rs.jquery.bootgrid').rows;
            for (var i = 0; i < DataGrid.length; i++) {
                var objTemp = {};
                var objSave = {};

                objTemp = DataGrid[i];
                if (objTemp.idNoEconomico != 0) {
                    objSave.id = objTemp.id;
                    objSave.idNoEconomico = objTemp.idNoEconomico;
                    objSave.pFechaInicio = objTemp.pFechaInicio;
                    objSave.pFechaFin = objTemp.pFechaFin;
                    objSave.pFechaObra = objTemp.pFechaObra;
                    objSave.tipoAsignacion = objTemp.tipoAsignacion;
                    DataSend.push(objSave);
                }

            }
            return DataSend;
        }

        function AsignarTodos() {
            $.blockUI({ message: mensajes.PROCESANDO });
            dataListaAsignados = [];
            dataDetalleSolicitud = [];
            var DataSend = [];
            var dataDetalleSolicitud = $('#tblDetalleSolicitud').bootgrid().data('.rs.jquery.bootgrid').rows;
            dataListaAsignados = $("#tblEquiposAsignados").bootgrid().data('.rs.jquery.bootgrid').rows;
            var TipoAsignacion;
            objSolicitudAsignado = $.grep(dataDetalleSolicitud,
                             function (o, i) { return o.idNoEconomico == ""; },
                          true);
            for (var i = 0; i < objSolicitudAsignado.length; i++) {
                var objDataListAsignados = {};
                temp = objSolicitudAsignado[i];

                if (temp.idNoEconomico != 0 && (temp.pFechaObra != "" && temp.pFechaObra != '01/01/0001')) {

                    var GetEconomico = $('select[data-id="' + temp.id + '"]').find("option:selected").text();
                    var fechaPromesa = $('input.tbFechaPromesa[data-id$="' + temp.id + '"]').val();
                    var objSave = {};

                    objSave.id = temp.id;
                    objSave.idNoEconomico = temp.idNoEconomico;
                    objSave.pFechaInicio = temp.pFechaInicio;
                    objSave.pFechaFin = temp.pFechaFin;
                    objSave.pFechaObra = fechaPromesa;
                    objSave.tipoAsignacion = temp.tipoAsignacion;
                    DataSend.push(objSave);
                    TipoAsignacion = temp.tipoAsignacion;
                }
            }
            GuardarAsignados(DataSend, TipoAsignacion);

            AlertaGeneral('Confirmacion', 'Fueron asignados todos los registros con numero economico y fecha promesa');
            $.unblockUI();

        }

        function fillComboTable(data) {
            if (data.length > 0) {
                $("#tblDetalleSolicitud tbody tr").each(function (index) {
                    if (data[index].id != 0) {
                        $(this).find('select.clsEconomico').val(data[index].idNoEconomico);
                        $(this).find('.tbFechaPromesa').val(data[index].pFechaObra);
                    } else {
                        $("#tblDetalleSolicitud").find('select.clsEconomico').val('');
                    }
                });
            }
        }

        function GetSolicitdDetalleTable(idSolicitudobj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/FillDetalleSolictudAsignacion',
                type: "POST",
                datatype: "json",
                data: { obj: idSolicitudobj },
                success: function (response) {

                    lblfolio.text(response.folio);
                    var dataDetalleSolicitud = response.dataDetalleSolicitud;

                    btnVerReporte.attr('data-CC', response.CC);
                    var AsignacionesRastreo = response.AsignacionesRastreo;
                    var AsignadosRaw = response.AsignadosRaw;

                    tblDetalleSolicitud.bootgrid("clear");
                    tblDetalleSolicitud.bootgrid("append", dataDetalleSolicitud);

                    tblEquiposAsignados.bootgrid("clear");
                    tblEquiposAsignados.bootgrid("append", AsignadosRaw);

                    tblRastreoAsignados.bootgrid('clear');
                    tblRastreoAsignados.bootgrid('append', AsignacionesRastreo)

                    divDetalle.removeClass('hide');
                    divPrincipal.addClass('hide');
                    $.unblockUI();

                },
                error: function (e) {
                    $.unblockUI();
                }
            });
        }
        function GetSolicitdDetalleTableReemplazo(idSolicitudobj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/FillDetalleReemplazo',
                type: "POST",
                datatype: "json",
                data: { obj: idSolicitudobj },
                success: function (response) {

                     lblfolio.text(response.folio);
                    var dataDetalleSolicitud = response.dataDetalleSolicitud;

                    var AsignadosRaw = response.AsignadosRaw;

                    tblDetalleSolicitud.bootgrid("clear");
                    tblDetalleSolicitud.bootgrid("append", dataDetalleSolicitud);

                    tblEquiposAsignados.bootgrid("clear");
                    tblEquiposAsignados.bootgrid("append", AsignadosRaw);

                    divDetalle.removeClass('hide');
                    divPrincipal.addClass('hide');
                    $.unblockUI();

                },
                error: function (e) {
                    $.unblockUI();
                }
            });
        }

        //#region Tabla Asignacion./*
        function initTablaAsignaciones() {
            dtTablaBancos = tablaBancos.DataTable({
                language: dtDicEsp,
                destroy: true,
                paging: false,
                searching: false,
                columns: [
                    { data: 'banco', title: 'Banco' },
                    { data: 'descripcion', title: 'Descripcion' },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-success editar"><i class="fas fa-tools"></i> editar</button>` },
                    { data: 'id', render: (data, type, row) => `<button class="btn btn-danger eliminar"><i class="fas fa-trash"></i> eliminar</button>` }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function (settings) {

                    tablaBancos.find('.editar').click(function () {
                        let data = dtTablaBancos.row($(this).parents('tr')).data();
                        idBanco = data.id;
                        inputBanco.val(data.banco);
                        inputDescripcion.val(data.descripcion);
                        modalNuevoBanco.modal('show');
                    }); 

                    tablaBancos.find('.eliminar').click(function () {
                        let data = dtTablaBancos.row($(this).parents('tr')).data();
                        idBanco = data.id;
                        modalEliminarBanco.modal('show');
                    });

                }
            });
        }
        //#endregion */

        init();
    };

    $(document).ready(function () {

        maquinaria.Inventario.SolicitudesEquipo.AsignacionMaquinaria = new AsignacionMaquinaria();
    });
})();

