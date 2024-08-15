(function () {

    $.namespace('maquinaria.MovimientoMaquinaria.Liberacion');

    Liberacion = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };

        mensajes = {
            NOMBRE: 'Liberacion',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        lblComentario = $("#lblComentario"),
        gAutorizar = (gtempAutorizar == "True" ? true : false);
        Trabajando = $("#Trabajando"),
        Pendientes = $("#Pendientes"),
        tblPendienteAutorizar = $("#tblPendienteAutorizar"),

        modalImpresion = $("#modalImpresion"),
        btnVerReporte = $("#btnVerReporte"),
        fechaInicio = $("#fechaInicio"),
        fechaFin = $("#fechaFin"),
        tbCCFiltro = $("#tbCCFiltro"),
        btnImprimiar = $("#btnImprimiar"),
        cboTipoFiltro = $("#cboTipoFiltro"),
        txtComentarioSolicitudModalComentario = $("#txtComentarioSolicitudModalComentario"),
        txtComentarioEstatusAutorizacion = $("#txtComentarioEstatusAutorizacion"),
        btnAutorizar = $("#btnAutorizar"),
        txtComentarioAutorizacion = $("#txtComentarioAutorizacion"),
        tbHorasParo = $("#tbHorasParo"),
        modalAutorizacion = $("#modalAutorizacion"),
        txtValidacion = $("#txtValidacion"),
        txtaComentarioParo = $("#txtaComentarioParo"),
        modalSolicitud = $("#modalSolicitud"),
        btnGuardarSolicitud = $("#btnGuardarSolicitud"),
        modalliberacion = $("#modalliberacion"),
        btnAceptarLiberacion = $("#btnAceptarLiberacion"),
        cboCentroCostos = $("#cboCentroCostos"),
        modalStandBy = $("#modalStandBy"),
        tbComentario = $("#tbComentario"), txtvalid = $("#txtvalid"),
        btnGuardarModal = $("#btnGuardarModal"),

        ireport = $("#report")
        tblEquiposAsignadosObra = $("#tblEquiposAsignadosObra");

        function init() {

            fechaInicio.datepicker().datepicker("setDate", new Date());
            fechaFin.datepicker().datepicker("setDate", new Date());
            initGrid(tblEquiposAsignadosObra);

            $(document).tooltip();
            btnVerReporte.click(verReporte);
            getTabla(centro_costos, tblEquiposAsignadosObra);
            btnImprimiar.click(verModalImprimir);
            btnGuardarModal.click(setStandBy);
            tbComentario.keypress(removeValid);
            btnAceptarLiberacion.click(Liberar);
            txtaComentarioParo.keydown(RemoveText);
            btnGuardarSolicitud.click(PrepareInfoSolictud);
            btnAutorizar.click(UpdateInfo);

            cboTipoFiltro.change(ReloadInfo);
        }

        function verModalImprimir() {
            tbCCFiltro.val('');
            fechaInicio.datepicker().datepicker("setDate", new Date());
            fechaFin.datepicker().datepicker("setDate", new Date());

            modalImpresion.modal('show');
        }

        function ReloadInfo() {


            switch ($(this).val()) {
                case "1":
                    {
                        Pendientes.addClass('hide');
                        Trabajando.removeClass('hide');
                        initGrid(tblEquiposAsignadosObra);
                        getTabla(centro_costos, tblEquiposAsignadosObra);
                    }
                    break;
                case "2":
                    {
                        Pendientes.removeClass('hide');
                        Trabajando.addClass('hide');
                        initGrid(tblPendienteAutorizar);
                        getTabla(centro_costos, tblPendienteAutorizar);
                    }
                    break;
                case "3":
                    {
                        Pendientes.addClass('hide');
                        Trabajando.removeClass('hide');
                        initGrid(tblEquiposAsignadosObra);
                        getTabla(centro_costos, tblEquiposAsignadosObra);
                    }
                default:

            }
        }

        function RemoveText() {
            txtValidacion.val('');
            tbHorasParo.val('');
            txtComentarioAutorizacion.val('');
        }

        function removeValid() {
            if (tbComentario != "") {
                txtvalid.text("");
            }
            else {
                txtvalid.text("*Debe Ingresar un comentario");
            }

        }
        function initGrid(selector) {
            selector.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "Liberar": function (column, row) {

                        return "<button type='button' class='btn btn-success Liberar' data-idAsignacion='" + row.idAsignacion + "'>" +
                                                  "<span class='glyphicon glyphicon-check'></span> " +
                                                         " </button>";
                    },
                    "Autorizacion": function (column, row) {
                        var disabled = "";

                        if (gAutorizar) {
                            //if (row.HasAutorizaciones == 1) {
                            //    disabled = "disabled";
                            //}
                            return "<button type='button' class='btn btn-primary Autoriza' data-idAsignacion='" + row.idAsignacion + "' data-idAutorizacionID='" + row.idAutorizacion + "'>" +
                                                 "<span class='glyphicon glyphicon-user'></span> Autorización" +
                                                        " </button>";
                        }
                        else {
                            //if (row.HasAutorizaciones == 0) {
                            //    disabled = "disabled";
                            //} else
                            //    if (row.HasAutorizaciones == 1) {
                            //        disabled = "";
                            //    }

                            return "<button type='button' class='btn btn-primary Solicita' data-idAsignacion='" + row.idAsignacion + "' >" +
                                                 "<span class='glyphicon glyphicon-share-alt'></span> Envio Solicitud" +
                                                        " </button>";
                        }

                    },
                    "Comentario": function (column, row) {
                        return "";
                        //return "<label class='txtComentarioStandBy'></label>";
                    },
                    "SetStandBy": function (column, row) {
                        var disab = "";
                        if ($("#cboTipoFiltro").val() == 2) {
                            disab = "disabled"
                        }
                        if (gAutorizar) {
                            disab = "disabled"
                        }

                        var select = "<select class='form-control cboEstatus' " + disab + ">";
                        select += "<option value='1' title='" + loadDescripcion(1) + "'>Trabajando</option>";
                        select += "<option value='2' title='" + loadDescripcion(2) + "'>Stand by A</option>";
                        select += "<option value='3' title='" + loadDescripcion(3) + "'>Stand by B</option>";
                        select += "<option value='4' title='" + loadDescripcion(4) + "'>Stand by C</option>";
                        select += "<option value='5' title='" + loadDescripcion(5) + "'>Stand by D</option>";
                        select += "<option value='6' title='" + loadDescripcion(6) + "'>Stand by E</option>";
                        select += "<option value='7' title='" + loadDescripcion(7) + "'>Liberar</option>";
                        select += "</select>";

                        return select;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                var rows = selector.bootgrid('getCurrentRows');
                fillComboTable(rows, selector);
                selector.find(".Liberar").on("click", function (e) {
                    var idAsignacion = $(this).attr('data-idAsignacion');

                });

                selector.find(".cboEstatus").on("change", function (e) {
                    var label = $(this).parents('tr').find('.txtComentarioStandBy');
                    var value = $(this).val();
                    var elemento = $(this).parents('tr').children().find('.Solicita');
                    elemento.prop('disabled', false);
                    loadDescripcion(value, label);
                });

                selector.find(".Autoriza").on("click", function (e) {
                    var idAutorizacion = $(this).attr('data-idAutorizacionID');
                    var idAsignacion = $(this).attr("data-idAsignacion");
                    btnAutorizar.attr("data-idAsignacion", idAsignacion);
                    btnAutorizar.attr("data-idAutorizacionID", idAutorizacion);
                    var EstatusMaquina = $(this).parent().parent().children().find('.cboEstatus').val();
                    if (EstatusMaquina != 7) {
                        GetInfoAutorizacionView(idAsignacion);
                    } else {
                        btnAceptarLiberacion.attr('data-idAsignacion', idAsignacion);
                        modalliberacion.modal('show');
                    }
                });

                selector.find(".Solicita").on("click", function (e) {

                    var idAsignacion = $(this).attr('data-idAsignacion');
                    var EstatusMaquina = $(this).parent().parent().children().find('.cboEstatus').val();

                    if (EstatusMaquina == "7") {
                        lblComentario.text('Motivo Liberacion');
                    }
                    else {
                        lblComentario.text('Comentario de paro maquina');
                    }
                    btnGuardarSolicitud.attr('data-idAsignacion', idAsignacion);
                    btnGuardarSolicitud.attr('data-estatusMaquinaria', EstatusMaquina);
                    txtaComentarioParo.val('');
                    modalSolicitud.modal('show');
                });

            });

        }


        function GetInfoAutorizacionView(idAsignacion) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/MovimientoMaquinaria/GetInfoAutorizacionView",
                data: { idAsignacion: idAsignacion },
                success: function (response) {
                    var res = response.Datos;
                    var tipoStandby = res.tipoStandBy;

                    var comentario = res.comentarioSolicitud;
                    txtComentarioSolicitudModalComentario.val(comentario);
                    txtComentarioEstatusAutorizacion.attr('data-idEstatus', tipoStandby);
                    txtComentarioEstatusAutorizacion.val(LoadTipoStandByDescr(tipoStandby) + ":\n" + loadDescripcion(tipoStandby));
                    modalAutorizacion.modal('show');
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });

        }

        function fillComboTable(data, elemento) {
            var dato = elemento.attr('id');
            if (data.length > 0) {
                $("#" + dato + " tbody tr").each(function (index) {
                    if (data[index].id != 0) {
                        $(this).find('select.cboEstatus').val(data[index].estatusMaquina);
                    }
                });
            }
        }

        function getTabla(obj, elemento) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/MovimientoMaquinaria/GetTablaMaquinariaAsignadaEnObra",
                data: { cc: obj, filtro: cboTipoFiltro.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.EquipoAsignado;
                    elemento.bootgrid("clear");
                    if (data.length > 0) {

                        elemento.bootgrid("append", data);
                        elemento.bootgrid('reload');
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadTipoStandByDescr(value) {

            switch (Number(value)) {
                case 1:
                    return "Trabajando";
                case 2:
                    return "Stand By A";
                case 3:
                    return "Stand By B";
                case 4:
                    return "Stand By C";
                case 5:
                    return "Stand By D";
                case 6:
                    return "Stand By E";
                case 7:
                    return "Liberar";
            }
        }

        function valid() {
            var state = true;
            if (!validarCampo(tbHorasParo)) { state = false; }
            if (!validarCampo(txtComentarioAutorizacion)) { state = false; }
            return state;
        }

        function UpdateInfo() {
            var idAutorizacion = btnAutorizar.attr("data-idAutorizacionID"); // btnAutorizar.attr("data-idAsignacion");
            var comentario = txtComentarioAutorizacion.val();
            var flag = true;

            var horasParo = tbHorasParo.val();
            //if (txtComentarioEstatusAutorizacion.val() == 1) {
            //    if (horasParo == 0) {
            //        flag = false;
            //    }
            //}

            if (txtComentarioEstatusAutorizacion.attr('data-idEstatus') != "1") {
                flag = valid();
                horasParo = 0;
            }
            else {
                horasParo = 0;
            }

            if (flag) {
                UpdateEstatus(idAutorizacion, comentario, horasParo);
            }
            else {
                // AlertaGeneral("Advertencia", "Debe capturar las horas de paro de la maquina.")
            }
        }


        function UpdateEstatus(idAutorizacion, comentario, horasParo) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/MovimientoMaquinaria/UpdateSetStandBy",
                data: { idAutorizacion: idAutorizacion, Comentario: comentario, horasParo: horasParo },
                success: function (response) {

                    modalAutorizacion.modal('hide');
                    RemoveText();
                    getTabla(centro_costos, tblPendienteAutorizar);
                    AlertaGeneral("confirmación", "El Registro fué actualizado correctamente");
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function PrepareInfoSolictud() {
            var idAsignacion = btnGuardarSolicitud.attr('data-idAsignacion');
            var idEstatus = btnGuardarSolicitud.attr('data-estatusMaquinaria');
            var Comentario = txtaComentarioParo.val();
            if (Comentario != "") {
                SolicituStandBy(idAsignacion, idEstatus, Comentario)
            }
            else {
                txtValidacion.text('Debe proporcionar un comentario.')
            }

        }

        function SolicituStandBy(obj, tipoAccion, Comentario) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/MovimientoMaquinaria/EnviarSolicitudStandBy",
                data: { idAsignacion: obj, tipoAccion: tipoAccion, Comentario: Comentario },
                success: function (response) {
                    modalSolicitud.modal('hide');
                    AlertaGeneral("confirmación", "Se ha enviado una solicitud");
                    getTabla(centro_costos, tblEquiposAsignadosObra);


                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setStandBy() {
            if (tbComentario.val() != "") {
                var idAsignacion = btnGuardarModal.attr('data-idAsignacion');
                var Comentario = tbComentario.val();
                UpdateEstatus(idAsignacion, 2, Comentario);
                modalStandBy.modal('hide');
                tbComentario.val('');
            }
            else {
                txtvalid.text("*Debe Ingresar un comentario");
            }

        }

        function Liberar() {

            var idAsignacion = $(this).attr('data-idAsignacion');
            UpdateEstatus(idAsignacion, 1, 0);
            modalliberacion.modal('hide');

        }

        function loadDescripcion(opcion) {
            var texto = ""
            switch (Number(opcion)) {
                case 1:
                    return "Trabajando";
                case 2:
                    return "Si el equipo está disponible para envio, está de flete para su retiro de la obra o proyecto";

                    break;
                case 3:

                    return "Si el equipo está en estatus de reparación estará más de 3 días en rehabilitación por un fallo no imputable a la operación del equipo";

                    break;
                case 4:

                    return "si el equipo esta sin utilización o falta de tramo por factores ajenos al área de produccion en un periodo mayor a 3 días(lluvias, huelgas, falta de permisos ambientales arranque de obra y paro de procesos por falta de suministros por parte del cliente)";

                    break;
                case 5:

                    return "Si el Equipo está en estatus de reparación y el daño fue causado en otra obra independientemente a que sea imputable a operación o no. Este daño debe ser detectado y evidenciado al recibir el equipo en sus primeras horas de operación en la obra o proyecto actual";

                    break;
                case 6:

                    return "En equipos que estén en back up sin que tengan demanda en otras obras o proyectos"

                    break;
                case 7:

                    return "Cuando la maquinaria no se ocupa por el periodo solicitado"

                    break;
            }
            //  labelDescripcion.text(texto);
        }

        function verReporte(e) {

            var CC = tbCCFiltro.val();
            var pFechaInico = fechaInicio.val();
            var pFechaFin = fechaFin.val();

            if (validarCampo(tbCCFiltro)) {
                modalImpresion.modal('hide');

                $.blockUI({ message: mensajes.PROCESANDO });

                var idReporte = "";

                var path = "/Reportes/Vista.aspx?idReporte=29&pCC=" + tbCCFiltro.val() + "&pFechaInicio=" + pFechaInico + "&pFechaFin=" + pFechaFin

                ireport.attr("src", path);

                document.getElementById('report').onload = function () {
                    $.unblockUI();
                    openCRModal();
                };
                e.preventDefault();
            }
            else {
                AlertaGeneral("Alerta", "Debe seleccionar un filtro", "bg-red");
            }

        }

        init();

    };

    $(document).ready(function () {
        maquinaria.MovimientoMaquinaria.Liberacion = new Liberacion();
    });
})();
