(function () {

    $.namespace('maquinaria.inventario.Solicitud.Asignaciones');

    Asignaciones = function () {

        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };

        mensajes = {
            NOMBRE: 'Asignacion de Equipo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        idSolicitudDetalle = 0;
        tbObservaciones = $("#tbObservaciones"),
        tbDescripcionModal = $("#tbDescripcionModal"),
        modalConfirmacionCancelacion = $("#modalConfirmacionCancelacion"),
        btnCancelarSolicitud = $("#btnCancelarSolicitud"),
        tbFolioSolicitud = $("#tbFolioSolicitud"),
        btnGuardar = $("#btnGuardar"),
        btnRegresar = $("#btnRegresar"),
        btnSiguiente = $(".btnSiguiente"),
        divAsignacionEquipo = $("#divAsignacionEquipo"),
        tituloModal = $("#tituloModal"),
        modalListaEquiposAsignados = $("#modalListaEquiposAsignados"),
        btnEquipoAsignado = $("#btnEquipoAsignado");
        tbHoras = $("#tbHoras");
        tbTipoID = $("#tbTipoID"),
        tbGrupoId = $("#tbGrupoId"),
        tbModeloEquipo = $("#tbModeloEquipo"),
        tbFechaInicio = $("#tbFechaInicio"),
        tbFechaFin = $("#tbFechaFin"),
        tbTipoPrioridad = $("#tbTipoPrioridad"),
        divSolicitudesAutorizados = $("#divSolicitudesAutorizados"),
        divListaEquiposAsignar = $("#divListaEquiposAsignar"),
        tbCantidadRegistros = $("#tbCantidadRegistros"),
        tblSolicitudesAutorizadas = $("#tblSolicitudesAutorizadas"),
        tblDetalleSolicitud = $("#tblDetalleSolicitud"),
        txtFechaPromesa = $("#txtFechaPromesa"),
        tblListaEconomicos = $("#tblListaEconomicos"),
        btnRentaCompra = $("#btnRentaCompra"),
        tblEconomicosAsignar = $("#tblEconomicosAsignar");
        ireport = $("#report");
        var data;
        var listtaEconomicos = [];
        var EconomicosXAsignar = [];
        var Centro_Costos;

        function init() {
            btnCancelarSolicitud.click(cancelarSolicitud);
            bootG('/SolicitudEquipo/GetDataSolicitudesAutorizadas');
            btnEquipoAsignado.click(verEconomicosAsignados);
            btnSiguiente.click(GetReporte);
            btnRegresar.click(backPage);
            btnGuardar.click(saveOrUpdate);
            txtFechaPromesa.datepicker().datepicker("setDate", new Date());
            btnRentaCompra.click(addRentaCompra);
        }

        function backPage() {
            divListaEquiposAsignar.removeClass('hidden');
            divAsignacionEquipo.addClass('hidden');
        }

        function GetReporte() {
            if ($(this).parent().hasClass('btnModal')) {
                modalListaEquiposAsignados.modal('hide');
            }
            if (listtaEconomicos.length >= 0) {
                divListaEquiposAsignar.addClass('hidden');
                divAsignacionEquipo.removeClass('hidden');
                var cc = btnSiguiente.attr('data-CentroCostos');
                var idSolicitud = btnSiguiente.attr('data-idSolicitud');

                fillReporte(cc, idSolicitud);
            }
            else {
                AlertaGeneral("Precaución", "Debe agregar los economicos");
            }

        }


        function cancelarSolicitud() {
            if (tbDescripcionModal.val() != "") {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/SolicitudEquipo/CancelacionSolicitudes',
                    type: "POST",
                    datatype: "json",
                    data: { idSolicitud: btnCancelarSolicitud.attr('data-idSolicitud'), descripcion: tbDescripcionModal.val() },
                    success: function (response) {
                        bootG('/SolicitudEquipo/GetDataSolicitudesAutorizadas');
                        ConfirmacionGeneral("Confirmación", "Su solicitud se cancelo correctamente");
                        modalConfirmacionCancelacion.modal('hide');
                        $.unblockUI();
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }
            else {
                ConfirmacionGeneral("Alerta", "Debe agregar un comentario");
            }

        }
        function fillReporte(cc, idSolicitud) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/GetReporteAsignacion',
                type: "POST",
                datatype: "json",
                data: { obj: listtaEconomicos, idSolicitud: idSolicitud },
                success: function (response) {
                    var path = "/Reportes/Vista.aspx?idReporte=12&pCC=" + cc;
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                    };
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }
        function verEconomicosAsignados() {
            //$(".tbFechaPromesa").datepicker().datepicker("setDate", new Date());
            tituloModal.text("Lista de Equipos asignados");
            tblListaEconomicos.bootgrid("clear");
            tblListaEconomicos.bootgrid("append", listtaEconomicos);
            modalListaEquiposAsignados.modal('show');
        }
        //2
        function iniciarGrid() {
            tblSolicitudesAutorizadas.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {

                    "Cancelar": function (column, row) {
                        return "<button type='button' class='btn btn-danger Cancelar' data-id='" + row.id + "'  data-CentroCostos = '" + row.CentroCostos + "' data-Folio='" + row.Folio + "'  >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                                   " </button>"
                        ;
                    },
                    "VerSolicitud": function (column, row) {
                        return "<button type='button' class='btn btn-primary verSolicitud' data-id='" + row.id + "'  data-CentroCostos = '" + row.CentroCostos + "' data-Folio='" + row.Folio + "'  >" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                                   " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblSolicitudesAutorizadas.find(".verSolicitud").on("click", function (e) {
                    var obj = $(this).attr('data-id')
                    var Centro_Costos = $(this).attr('data-CentroCostos');
                    var idSolicitud = $(this).attr('data-id');
                    var Folio = $(this).attr('data-Folio');
                    btnSiguiente.attr("data-CentroCostos", Centro_Costos);
                    btnSiguiente.attr('data-idSolicitud', idSolicitud);
                    btnSiguiente.attr('data-Folio', Folio);
                    $.ajax({
                        url: '/SolicitudEquipo/GetListaDetalleSolicitud',
                        type: "POST",
                        datatype: "json",
                        data: { obj: Number(obj) },
                        success: function (response) {
                            $.unblockUI();
                            data = response.DetalleSolicitud;
                            if (response.Agregados.length > 0) {

                                var array = response.Agregados;
                                for (var i = 0; i < array.length ; i++) {
                                    var objEconomico = {};
                                    objEconomico.pFechaInicio = array[i].pFechaInicio;
                                    objEconomico.pFechaFin = array[i].pFechaFin;
                                    objEconomico.pHoras = array[i].pHoras;
                                    objEconomico.CC = array[i].CC;
                                    objEconomico.pTipoPrioridad = array[i].pTipoPrioridad;
                                    objEconomico.id = array[i].id;
                                    objEconomico.idEconomico = array[i].idEconomico;
                                    objEconomico.Tipo = array[i].Tipo;
                                    objEconomico.Modelo = array[i].Modelo;
                                    objEconomico.Grupo = array[i].Grupo;
                                    objEconomico.Descripcion = array[i].Descripcion;
                                    objEconomico.localizacion = array[i].localizacion;
                                    objEconomico.Economico = array[i].Economico;
                                    objEconomico.Marca = array[i].Marca;
                                    objEconomico.Serie = array[i].Serie;
                                    objEconomico.CCOrigen = array[i].CCOrigen;
                                    objEconomico.CCDestino = array[i].CCDestino;
                                    objEconomico.idsolicitud = array[i].idsolicitud;
                                    objEconomico.Folio = array[i].Folio;
                                    objEconomico.estatus = array[i].estatus;
                                    objEconomico.FechaPromesa = array[i].FechaPromesa;
                                    listtaEconomicos.push(objEconomico);
                                }

                                setEconomico("Asignados", null);
                            }
                            else {
                                setEconomico(false, null);
                            }

                            divListaEquiposAsignar.removeClass('hidden');
                            divSolicitudesAutorizados.addClass('hidden');
                        },
                        error: function () {
                            $.unblockUI();
                        }
                    });

                });

                tblSolicitudesAutorizadas.find(".Cancelar").on("click", function (e) {
                    var obj = $(this).attr('data-id')
                    var Centro_Costos = $(this).attr('data-CentroCostos');
                    var idSolicitud = $(this).attr('data-id');
                    var Folio = $(this).attr('data-Folio');
                    btnCancelarSolicitud.attr('data-idSolicitud', idSolicitud);
                    tbDescripcionModal.val('');
                    modalConfirmacionCancelacion.modal('show');

                });

            });

            tblListaEconomicos.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {

                    "Economico": function (column, row) {
                        if (row.TipoEquipo == 1) {
                            return "<select class='form-control CompraRenta' data-idRegistro='" + row.id + "'>" +
                                   "<option value='RENTA' >RENTA</option>" +
                                   "<option value='COMPRA'>COMPRA</option>" +
                                   "<option value='RENTA OPCION COMPRA'>RENTA/COMPRA</option>" +
                                    "</select>";

                        } else {
                            return "<input type='text' value='" + row.Economico + "' class='CompraRenta' readonly data-idRegistro='" + row.id + "'>"
                        }

                    },
                    "FechaPromesa": function (column, row) {
                        return "<input type='text' value='" + row.FechaPromesa + "' class='form-control tbFechaPromesa' readonly data-idRegistro='" + row.id + "'>"

                    },
                    "Remove": function (column, row) {
                        return "<button type='button' class='btn btn-danger Remove' data-id='" + row.idEconomico + "' data-idRegistro='" + row.id + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                                   " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                $(".tbFechaPromesa").datepicker();
                tblListaEconomicos.find(".Remove").on("click", function (e) {
                    var idEconomico = $(this).attr('data-id');
                    var idRegistro = $(this).attr('data-idRegistro')
                    var objEconomico = {};
                    var obj;
                    for (var i = 0; i < listtaEconomicos.length ; i++) {
                        if (listtaEconomicos[i].id == idRegistro) {
                            obj = listtaEconomicos[i];

                            tbTipoID.val(obj.Tipo);
                            tbGrupoId.val(obj.Grupo);
                            tbModeloEquipo.val(obj.Modelo);
                            tbFechaInicio.val(obj.pFechaInicio);
                            tbFechaFin.val(obj.pFechaFin);
                            tbHoras.val(obj.pHoras);
                            tbTipoPrioridad.val(obj.pTipoPrioridad);

                            tbFolioSolicitud.val(obj.Folio);
                            var tempD = {};
                            tempD.Folio = obj.Folio;
                            tempD.Tipo = obj.Tipo;
                            tempD.Grupo = obj.Grupo;
                            tempD.Modelo = obj.Modelo;
                            tempD.pFechaInicio = obj.pFechaInicio;
                            tempD.pFechaFin = obj.pFechaFin;
                            tempD.pHoras = obj.pHoras;
                            tempD.pTipoPrioridad = obj.pTipoPrioridad;
                            tempD.tbCantidadRegistros = data.length + 1;
                            tempD.id = obj.id;
                            tempD.SolicitudDetalleId = obj.SolicitudDetalleId;
                            tempD.FechaPromesa = obj.FechaPromesa;
                            data.push(tempD);

                        }
                    }



                    tbCantidadRegistros.val(data.length);
                    tbCantidadRegistros.attr('idDetalleSolicitud', obj.id);


                    listtaEconomicos = $.grep(listtaEconomicos,
                      function (o, i) { return o.idEconomico == Number(idEconomico) && o.id == obj.id; },
                   true);

                    if (listtaEconomicos.length >= 0) {
                        setEconomico("Asignados", null);
                        tblListaEconomicos.bootgrid("clear");
                        tblListaEconomicos.bootgrid("append", listtaEconomicos);
                    }


                });

                tblListaEconomicos.find(".CompraRenta").on("change", function (e) {

                    var identificador = $(this).attr('data-idRegistro');
                    var Economico = $(this).val();
                    var FechaPromesa = $(this).parent().parent().children().find('.tbFechaPromesa').val();

                    for (var i = 0; i < listtaEconomicos.length ; i++) {
                        if (listtaEconomicos[i].id == identificador) {
                            listtaEconomicos[i].Economico = Economico;
                            listtaEconomicos[i].FechaPromesa = FechaPromesa
                        }
                    }
                });

                tblListaEconomicos.find(".tbFechaPromesa").on("change", function (e) {

                    var identificador = $(this).attr('data-idRegistro');
                    var Economico = $(this).parent().parent().children().find('.CompraRenta').val();
                    var FechaPromesa = $(this).val();

                    for (var i = 0; i < listtaEconomicos.length ; i++) {
                        if (listtaEconomicos[i].id == identificador) {
                            listtaEconomicos[i].Economico = Economico;
                            listtaEconomicos[i].FechaPromesa = FechaPromesa
                        }
                    }
                });


            });

            tblEconomicosAsignar.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',

                formatters: {
                    "Asignar": function (column, row) {
                        return "<button type='button' class='btn btn-success asignar' data-id='" + row.id + "' data-Economico='" + row.Economico + "' " +
                               "data-grupoDes='" + row.Grupo + "' " + "data-MarcaDes='" + row.Marca + "'" + "data-ModeloDes='" + row.Modelo + "' " +
                                "data-SerieDes='" + row.Serie + "' " + "data-TipoDes='" + row.Tipo + "' " + "data-localizacion='" + row.CC + "' " +
                                "data-SolicitudID='" + row.solicitudEquipoID + "' " + "data-Folio='" + row.Folio + "' " + "data-fechaInicio='" + row.pFechaInicio + "' " +
                                "data-fechaFin='" + row.pFechaFin + "' " + "data-Horas='" + row.pHoras + "'" + "data-CC='" + row.CC + "' " + "data-idEconomico ='" + row.idEconomico + "' " +
                                "data-CCOrigen='" + row.CCOrigen + "'>" +
                            "<span class='glyphicon glyphicon-plus'></span> " +
                                   " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                tblEconomicosAsignar.find(".asignar").on("click", function (e) {
                    $(this).attr('data-Economico');
                    var objEconomico = {};
                    var objeto;
                    var objTemp;
                    var Flag = false;
                    if (data.length > 0) {
                        objTemp = data[1];
                    }

                    if (data.length > 0) {
                        objeto = data[0];

                        objEconomico.pFechaInicio = objeto.pFechaInicio;
                        objEconomico.pFechaFin = objeto.pFechaFin;
                        objEconomico.pHoras = objeto.pHoras;
                        objEconomico.CC = objeto.CC;
                        objEconomico.Folio = objeto.Folio;
                        objEconomico.pTipoPrioridad = objeto.pTipoPrioridad;
                        objEconomico.id = objeto.id;
                        objEconomico.Comentario = objeto.Comentario;
                    }

                    if (objTemp != null) {
                        if (objeto.Tipo == objTemp.Tipo && objeto.Grupo == objTemp.Grupo)
                        { Flag = true; }
                    }
                    data.shift();


                    objEconomico.idEconomico = $(this).attr('data-idEconomico');
                    objEconomico.Tipo = $(this).attr('data-TipoDes');
                    objEconomico.Modelo = $(this).attr('data-ModeloDes');
                    objEconomico.Grupo = $(this).attr('data-grupoDes');
                    objEconomico.Descripcion = $(this).attr('data-grupoDes');
                    objEconomico.localizacion = $(this).attr('data-localizacion');
                    objEconomico.Economico = $(this).attr('data-Economico');
                    objEconomico.Marca = $(this).attr('data-MarcaDes');
                    objEconomico.Serie = $(this).attr('data-SerieDes');
                    objEconomico.CCOrigen = $(this).attr('data-CCOrigen');
                    objEconomico.CCDestino = btnSiguiente.attr("data-centrocostos");
                    objEconomico.idsolicitud = btnSiguiente.attr("data-idsolicitud");
                    objEconomico.estatus = 1;
                    objEconomico.FechaPromesa = txtFechaPromesa.val();
                    objEconomico.TipoEquipo = 0;
                    listtaEconomicos.push(objEconomico);
                    setEconomico(Flag, $(this).attr('data-Economico'));
                });

            });
        }


        function Cancelar(id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/CancelarSolicitud',
                type: "POST",
                datatype: "json",
                data: { obj: id },
                success: function (response) {
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function addRentaCompra() {


            if (tbCantidadRegistros.val() != 0) {


                var objEconomico = {};
                var objeto;
                var objTemp;
                var Flag = false;
                if (data.length > 0) {
                    objTemp = data[1];
                }

                if (data.length > 0) {
                    objeto = data[0];

                    objEconomico.pFechaInicio = objeto.pFechaInicio;
                    objEconomico.pFechaFin = objeto.pFechaFin;
                    objEconomico.pHoras = objeto.pHoras;
                    objEconomico.CC = objeto.CC;
                    objEconomico.Folio = objeto.Folio;
                    objEconomico.pTipoPrioridad = objeto.pTipoPrioridad;
                    objEconomico.id = objeto.id;
                }

                if (objTemp != null) {
                    if (objeto.Tipo == objTemp.Tipo && objeto.Grupo == objTemp.Grupo)
                    { Flag = true; }
                }
                data.shift();
                objEconomico.idEconomico = 0;
                objEconomico.Tipo = tbTipoID.val();
                objEconomico.Modelo = tbModeloEquipo.val();
                objEconomico.Grupo = tbGrupoId.val();
                objEconomico.Descripcion = tbGrupoId.val();
                objEconomico.localizacion = "";
                objEconomico.Economico = "Renta";
                objEconomico.Marca = "";
                objEconomico.Serie = "";
                objEconomico.CCOrigen = "";
                objEconomico.CCDestino = btnSiguiente.attr("data-centrocostos");
                objEconomico.idsolicitud = btnSiguiente.attr("data-idsolicitud");
                objEconomico.estatus = 1;
                objEconomico.FechaPromesa = txtFechaPromesa.val();
                objEconomico.TipoEquipo = 1;
                listtaEconomicos.push(objEconomico);
                setEconomico(false, 0);
            }
            else {
                ConfirmacionGeneral("Confirmación", "No puede agregar mas registros de los solicitados");
            }
        }

        function fillEconomicosDelete(objeto, listtaEconomicos) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/getListaEconomicosNo',
                type: "POST",
                datatype: "json",
                data: { objeto: objeto, obj: listtaEconomicos },
                success: function (response) {
                    $.unblockUI();
                    var data = response.listaEconomicos;
                    EconomicosXAsignar = response.listaEconomicos;
                    tblEconomicosAsignar.bootgrid("clear");
                    tblEconomicosAsignar.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setInput(obj) {
            tbTipoID.val(obj.Tipo);
            tbGrupoId.val(obj.Grupo);
            tbModeloEquipo.val(obj.Modelo);
            tbFechaInicio.val(obj.pFechaInicio);
            tbFechaFin.val(obj.pFechaFin);
            tbHoras.val(obj.pHoras);
            tbTipoPrioridad.val(obj.pTipoPrioridad);
            tbCantidadRegistros.attr('idDetalleSolicitud', obj.id);
            var Folio = btnSiguiente.attr('data-Folio');
            tbFolioSolicitud.val(obj.Folio);
            tbObservaciones.val(obj.Comentario);

        }



        function setEconomico(Flag, economico) {
            if (data.length > 0) {
                var objeto = data[0];
                setInput(objeto);
                if (Flag == true) {

                    EconomicosXAsignar = $.grep(EconomicosXAsignar,
                      function (o, i) { return o.Economico == economico; },
                   true);

                    tblEconomicosAsignar.bootgrid("clear");
                    tblEconomicosAsignar.bootgrid("append", EconomicosXAsignar);

                } else if (Flag == false) {
                    fillEconomicos(objeto);
                } else if (Flag == "Asignados") {
                    fillEconomicosDelete(objeto, listtaEconomicos);
                }

                tbCantidadRegistros.val(data.length);

            }
            else {
                tbTipoID.val("");
                tbGrupoId.val("");
                tbModeloEquipo.val("");
                tbFechaInicio.val("");
                tbFechaFin.val("");
                tbHoras.val("");
                tbTipoPrioridad.val("");
                tblEconomicosAsignar.bootgrid("clear");
                tblListaEconomicos.bootgrid("clear");
                tblListaEconomicos.bootgrid("append", listtaEconomicos);
                modalListaEquiposAsignados.modal('show');
                tbCantidadRegistros.val(data.length);
                tbFolioSolicitud.val("");
            }

        }

        function fillEconomicos(objeto) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/getListaEconomicos',
                type: "POST",
                datatype: "json",
                data: objeto,
                success: function (response) {
                    $.unblockUI();
                    var data = response.listaEconomicos;
                    EconomicosXAsignar = response.listaEconomicos;
                    tblEconomicosAsignar.bootgrid("clear");
                    tblEconomicosAsignar.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }



        function fillGrid() {
            tblDetalleSolicitud.bootgrid("clear");
            tblDetalleSolicitud.bootgrid("append", data);
        }
        //1
        function bootG(url) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: url,
                type: "POST",
                datatype: "json",
                contentType: 'application/json',
                success: function (response) {
                    $.unblockUI();
                    var SolicitudesPendientes = response.Pendientes;
                    tbCantidadRegistros.val(SolicitudesPendientes.length);
                    tblSolicitudesAutorizadas.bootgrid("clear");
                    tblSolicitudesAutorizadas.bootgrid("append", SolicitudesPendientes);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function saveOrUpdate() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/SaveOrUpdateAsignacion',
                type: 'POST',
                dataType: 'json',
                data: { obj: listtaEconomicos },
                success: function (response) {
                    if (response.success == true) {
                        if (data.length > 0) {
                            ConfirmacionGeneral("Confirmación", "Se hizo un guardado parcial puede continuar mas adelante.", "bg-green");
                        } else {
                            ConfirmacionGeneral("Confirmación", "La maquinaria se asigno correctamente ", "bg-green");
                        }

                        Redireccion();
                        $.unblockUI();
                    }
                    else {
                        ConfirmacionGeneral("Alerta", response.message, "bg-red");
                        $.unblockUI();
                    }

                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function Redireccion() {
            divListaEquiposAsignar.addClass('hidden');
            divSolicitudesAutorizados.removeClass('hidden');
            divAsignacionEquipo.addClass('hidden');
            data;
            listtaEconomicos = [];
            EconomicosXAsignar = [];
            Centro_Costos;

            tblListaEconomicos.bootgrid("clear");

            tblEconomicosAsignar.bootgrid("clear");
            bootG('/SolicitudEquipo/GetDataSolicitudesAutorizadas');
        }

        iniciarGrid();
        init();
    };

    $(document).ready(function () {
        maquinaria.inventario.Solicitud.Asignaciones = new Asignaciones();
    });
})();


