$(function () {
    $.namespace('MantenimientoPM.EjecucionPM');
    EjecucionPM = function () {
        _idMantenimiento = 0;
        _horometroEjecucionPM = 0;
        _idEconomico = 0;

        btnEjecucionPM = $('#btnEjecucionPM');
        btnAltaFiltro = $('#btnAltaFiltro');
        modalAltaFiltro = $('#modalAltaFiltro');
        btnGuardarAltaFiltro = $("#btnGuardarAltaFiltro");
        modalEjecucionPM = $('#modalEjecucionPM');
        modalEjecucionPMdetalle = $('#modalEjecucionPMdetalle');
        tblMantenimientosProg = $('#tblMantenimientosProg');

        const tblGridLubProxEjecPM = $('#tblGridLubProxEjecPm');
        let dtGridLubProxEjecPM;
        tblGridActProxEjecPm = $('#tblGridActProxEjecPm');
        tblgridDNProxEjecPM = $('#tblgridDNProxEjecPm');

        grid_GestorActividadesEjecPM = $("#grid_GestorActividadesEjecPM");

        //Propiedades Objeto Ejecutado General
        horometroMaquinaSEjecPM = $('#horometroMaquinaSEjecPM');
        fechaUltCalSEjecPM = $('#fechaUltCalSEjecPM');
        cboTipoMantenientoContadorSEjecPM = $('#cboTipoMantenientoContadorSEjecPM');
        fechaIniManteSEjecPM = $('#fechaIniManteSEjecPM');
        HorometroAplicoSEjecPM = $('#HorometroAplicoSEjecPM');
        tbNombreCoordinadorSEjecPM = $('#tbNombreCoordinadorSEjecPM');
        tbPuestoCoordinadorSEjecPM = $('#tbPuestoCoordinadorSEjecPM');
        tbNombreEmpleadoSEjecPM = $('#tbNombreEmpleadoSEjecPM');
        tbPuestoEmpleadoSEjecPM = $('#tbPuestoEmpleadoSEjecPM');
        ObservacionesMantSEjecPM = $('#ObservacionesMantSEjecPM');
        HorometroProyectadoSEjecPM = $('#HorometroProyectadoSEjecPM');
        fechaFinManteSEjecPM = $('#fechaFinManteSEjecPM');
        MantenimientoProySEjecPM = $('#MantenimientoProySEjecPM');



        //---

        tblGridFormatosEjecPm = $('#tblGridFormatosEjecPm');

        var paginaActual;


        //HorometroAplicoSEjecPM.on('change', function () {
        //    _horometroEjecucionPM = +($(this).val());
        //    //$('#tblMantenimientosProg').find('tbody tr .btn-detalle-pm[value="' + _idMantenimiento + '"]').click();
        //    //HorometroAplicoSEjecPM.val(_horometroEjecucionPM);
        //    $('.txtUltCap').val(_horometroEjecucionPM);
        //    _horometroEjecucionPM = 0;
        //});

        btnEjecucionPM.on('click', function () {
            tblMantenimientosProg.ajax.reload(null, false);
            modalEjecucionPM.modal('show');
        });

        btnAltaFiltro.on('click', function () {
            $("#cboMarcaAltaFiltro").fillCombo("/MatenimientoPM/fillCboMarcaFiltro");
            //tblMantenimientosProg.ajax.reload(null, false);
            $("#txtDescripcionAltaFiltro").val("");
            $("#cboMarcaAltaFiltro").val("");
            $("#cboSinteticoAltaFiltro").val("");
            $("#txtModeloAltaFiltro").val("");
            modalAltaFiltro.modal('show');
        });

        btnGuardarAltaFiltro.on('click', function () {
            if (ValidarAltaFiltro()) {
                GuardarFiltro();
            }
            else {
                ConfirmacionGeneral("Alerta", "Faltan registros por proporcionar");
            }
        });

        $('a[href$="#Lubricantes"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        $('a[href$="#ActExt"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        $('a[href$="#DN"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        $('a[href$="#LubricantesEjecPm"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        $('a[href$="#ActExtEjecPm"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
        $('a[href$="#DNEjecPm"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        $('#btnGuardarEjecutado').on('click', function () {
            modalEjecucionPMdetalle.modal("hide");
            saveEjecutado();
        });

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable
                .tables({ visible: true, api: true })
                .columns.adjust();
        })

        function init() {
            initTableMantenimientosProg();
            initGridtablaPMGestorEjecPM(grid_GestorActividadesEjecPM);

            $("#tbNombreCoordinadorSEjecPM").getAutocomplete(SelectCoordinador, null, '/MatenimientoPM/getEmpleados');
            $("#tbNombreEmpleadoSEjecPM").getAutocomplete(SelectResponsable, null, '/MatenimientoPM/getEmpleados');
            HorometroAplicoSEjecPM.change(SetProximoPM);
            fechaIniManteSEjecPM.change(setFechaPM);

            $(document).on('click', '#btnEjecPMPaso2', function () {
                if (dtGridLubProxEjecPM != null) dtGridLubProxEjecPM.columns.adjust().draw();
                // alert("si");
            });

        }

        function SetProximoPM() {
            _horometroEjecucionPM = +($(this).val());
            //$('#tblMantenimientosProg').find('tbody tr .btn-detalle-pm[value="' + _idMantenimiento + '"]').click();
            //HorometroAplicoSEjecPM.val(_horometroEjecucionPM);
            $('.txtUltCap').val(_horometroEjecucionPM);
            _horometroEjecucionPM = 0;
            InfoMaquinaEjecPM(_idEconomico, _idMantenimiento)
        }
        function setFechaPM() {
            $('.txtFechaT ').val($(this).val());
        }



        function SelectCoordinador(event, ui) {
            tbNombreCoordinadorSEjecPM.text(ui.item.value);
            tbNombreCoordinadorSEjecPM.attr('data-NumEmpleado', ui.item.id)
            SetInfoCoordinador(ui.item.id);
        }
        function SelectResponsable(event, ui) {
            tbNombreEmpleadoSEjecPM.text(ui.item.value);
            tbNombreEmpleadoSEjecPM.attr('data-NumEmpleado', ui.item.id)
            SetInfoResponsable(ui.item.id);
        }
        function SetInfoCoordinador(idEmplado) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetSingleUsuario',
                data: { id: idEmplado },
                //async: false,
                success: function (response) {
                    if (response.success) {
                        tbPuestoCoordinadorSEjecPM.val(response.Puesto.toLowerCase());
                        tbPuestoCoordinadorSEjecPM.attr('data-CC', response.CCEmpleado);
                        //prueba 03/07/18
                        $("#tbPuestoCoordinadorSEjecPM").val(response.Puesto.toLowerCase());
                        $("#tbPuestoCoordinadorSEjecPM").attr('data-CC', response.CCEmpleado);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetInfoResponsable(idEmplado) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetSingleUsuario',
                data: { id: idEmplado },
                //async: false,
                success: function (response) {
                    if (response.success) {
                        tbPuestoEmpleadoSEjecPM.val(response.Puesto.toLowerCase());
                        tbPuestoEmpleadoSEjecPM.attr('data-CC', response.CCEmpleado);
                        //prueba 03/07/18
                        $("#tbPuestoEmpleadoSEjecPM").val(response.Puesto.toLowerCase());
                        $("#tbPuestoEmpleadoSEjecPM").attr('data-CC', response.CCEmpleado);
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function initTableMantenimientosProg() {
            tblMantenimientosProg = tblMantenimientosProg.DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/MatenimientoPM/GetMantenimientosProg',
                    data: function (data) {
                        data.cc = $("#cboObras").val()
                    }
                },
                "language": dtDicEsp,
                "aaSorting": [1, 'desc'],
                rowId: 'id',
                "scrollCollapse": true,
                'initComplete': function (settings, json) {
                    tblMantenimientosProg.on('click', '.btn-detalle-pm', function () {
                        var rowData = tblMantenimientosProg.row($(this).closest('tr')).data();

                        modalEjecucionPMdetalle.modal("show");
                        economicoiD = rowData["economicoID"];
                        idmantenimiento = rowData["id"];
                        _idMantenimiento = rowData["id"];
                        _idEconomico = rowData["economicoID"];
                        RenderizadoServicioEjecPM(idmantenimiento, economicoiD);
                    });

                    tblMantenimientosProg.on('click', '.btn-reporte-pm', function () {
                        var rowData = tblMantenimientosProg.row($(this).closest('tr')).data();

                        $.blockUI({
                            message: mensajes.PROCESANDO,
                            baseZ: 2000
                        });

                        var idReporte = "85";
                        var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + rowData["id"];

                        ireport.attr("src", path);
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    });
                },
                columns: [
                    { data: 'economicoID', title: 'CC' },
                    { data: 'fechaPM', title: 'Fecha PM' },
                    { data: 'horometroPM', title: 'Horómetro PM' },
                    { data: 'observaciones', title: 'Observaciones' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-detalle-pm btn btn-sm btn-primary glyphicon glyphicon-eye-open" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Detalle"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-reporte-pm btn btn-sm btn-warning glyphicon glyphicon-floppy-save" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Reporte"
                    }
                ]
            });
        }

        //Cargado de tablas del modal
        function RenderizadoServicioEjecPM(idmantenimiento, economicoiD) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/RenderizadoServicio',
                data: { idmantenimiento: idmantenimiento, economicoiD: economicoiD },
                success: function (response) {
                    $(".txtNoEconomicoAltS").val(response.Mantenimiento.economicoID);
                    $(".NoEcoServTent").val(response.Mantenimiento.economicoID);
                    $(".txtUltCap").val(response.Mantenimiento.horometroUltCapturado);

                    $("#horometroMaquinaSEjecPM").val(response.Mantenimiento.horometroUltCapturado);
                    $("#tbNombreCoordinadorSEjecPM").val(response.CoodinadorPM.nombre + " " + response.CoodinadorPM.ape_paterno + "" + response.CoodinadorPM.ape_materno);

                    $("#tbPuestoCoordinadorSEjecPM").val(response.CoodinadorPM.descripcion);
                    $("#tbNombreEmpleadoSEjecPM").val(response.ResponsablePM.nombre + " " + response.ResponsablePM.ape_paterno + "" + response.ResponsablePM.ape_materno);

                    $("#tbPuestoEmpleadoSEjecPM").val(response.ResponsablePM.descripcion);

                    $(".txtHorometroLubricantes").val(response.Mantenimiento.horometroUltCapturado);

                    $("#fechaUltCalSEjecPM").val(FormateoFechaEdicion(response.Mantenimiento.fechaUltCapturado));
                    $("#cboTipoMantenientoContadorSEjecPM").fillCombo('/MatenimientoPM/FillCombotablaPM', { estatus: true });//carga ajax combo
                    $("#cboTipoMantenientoContadorSEjecPM").val(response.Mantenimiento.tipoPM).change();
                    $("#cboTipoMantenientoContadorSEjecPM").attr('disabled', true);
                    $("#fechaIniManteSEjecPM").val(FormateoFechaEdicion(response.Mantenimiento.fechaPM));
                    //  $("#fechaIniManteSEjecPM").attr('disabled', true);
                    $("#HorometroAplicoSEjecPM").val(response.Mantenimiento.horometroPM + 250);
                    //$("#HorometroAplicoSEjecPM").change();
                    //getActividadesExtrasEjecPM(modeloID, idMantenimiento);
                    initGridActHisEjecPM($("#gridActhisEjecPm"));
                    $("#gridActhisEjecPm").bootgrid("clear");
                    $("#gridActhisEjecPm").bootgrid("append", response.objActividadesExtrashis);
                    setTableGridActividadesExtraEjecPM(response.objActividadesExtras);

                    //getActividadesDNsEjecPM(modeloID, idMantenimiento);
                    initGridDNhisEjecPm($("#gridDNHisEjecPm"));
                    $("#gridDNHisEjecPm").bootgrid("clear");
                    $("#gridDNHisEjecPm").bootgrid("append", response.objActividadesDNHis);
                    setTableGridDNsEjecPM(response.objActividadesDN);

                    //$("#HorometroAplicoSEjecPM").attr('disabled', true);
                    $("#ObservacionesMantSEjecPM").val(response.Mantenimiento.observaciones);
                    //$("#ObservacionesMantSEjecPM").attr('disabled', true);
                    $("#MantenimientoProySEjecPM").val(response.Mantenimiento);
                    $("#MantenimientoProySEjecPM").attr('disabled', true);
                    $("#fechaFinManteSEjecPM").val(FormateoFechaEdicion(response.Mantenimiento.fechaProy));
                    $("#fechaFinManteSEjecPM").attr('disabled', true);
                    $("#HorometroProyectadoSEjecPM").val(response.Mantenimiento.horometroProy);

                    $(".fechaServProxLub").val(FormateoFechaEdicion(response.Mantenimiento.fechaProy));
                    $(".horometroServTent").val($("#HorometroProyectadoSEjecPM").val());

                    $("#HorometroProyectadoSEjecPM").attr('disabled', true);

                    TipoMantenimientoActual = $("#cboTipoMantenientoContadorSEjecPM option:selected").val();
                    if (TipoMantenimientoActual != "") {
                        if (TipoMantenimientoActual == 8) {
                            $("#MantenimientoProySEjecPM").val($('#cboTipoMantenientoContadorSEjecPM  option:eq("' + 1 + '")').text());
                        } else {
                            $("#MantenimientoProySEjecPM").val($('#cboTipoMantenientoContadorSEjecPM  option:eq("' + (parseInt(TipoMantenimientoActual) + 1) + '")').text());
                        }
                    }
                    idCor = response.Mantenimiento.planeador;
                    idEmp = response.Mantenimiento.personalRealizo;
                    $(".txtServT").val($("#MantenimientoProySEjecPM").val());
                    $(".txtHrT").val($("#HorometroProyectadoSEjecPM").val());
                    $(".txtFechaT").val($("#fechaFinManteSEjecPM").val());


                    //InfoMaquinaEjecPM(economicoiD, idmantenimiento)
                    $("#cboFiltroGrupoS").val(response.maquina.grupoMaquinariaID).change();
                    $("#cboFiltroGrupoS").attr('disabled', true);
                    $(".txtTipoUltimoServLu").val($("#cboTipoMantenientoContadorSEjecPM option:selected").text());
                    modeloID = response.maquina.modeloEquipoID;
                    //loadDataProgramaActividadesLubEjecPM(modeloID, idMantenimiento);
                    //let arr = jQuery.grep(response.dataSetGridLubProx, function (a) {
                    //    return a.objHis !== null;
                    //});
                    setTableTlbGridLubEjecPM(response.dataSetGridLubProx);
                    initGridLubHisEjecPM($("#gridLubhisEjecPm"));
                    $("#gridLubhisEjecPm").bootgrid("clear");

                    var cadena = $("#MantenimientoProySEjecPM").val();
                    var idPM = 0;

                    inicio = 0;
                    fin = 3;

                    subCadena = cadena.substring(inicio, fin);

                    if (subCadena == "PM1") { idPM = 1; } else if ("PM2") { idPM = 2; } else if ("PM3") { idPM = 3; } else if ("PM4") { idPM = 4; }

                    $("#Pm_GestorActividadesEjecPM").html("");
                    $("#Pm_GestorActividadesEjecPM").append(subCadena);
                    $("#Pm_GestorActividadesEjecPM").attr("idpm", idPM);


                    //var retMod = ReturnModeloEjecPM();
                    //ConsultaActividadesProgEjecPM(modeloID, idPM, idmantenimiento);
                    $("#grid_GestorActividadesEjecPM").bootgrid("clear");
                    $("#grid_GestorActividadesEjecPM").bootgrid("append", response.ObjActProy);
                    //ProgressBarLubEjecPM();
                    //ProgressBarActProxEjecPM();
                    //CargaDeProyectadoEjecPM(idmantenimiento);
                    if (response.CargaDeProyectado.length != 0) {
                        response.CargaDeProyectado.forEach(function (valor, indice, array) {
                            var longitud = $("#gridLubProxEjecPm >tbody >tr").length
                            for (var i = 0; i <= (longitud - 1) ; i++) {
                                renglon = $("#gridLubProxEjecPm >tbody >tr")[i]
                                idCOmp = $(renglon).closest('tr').find('td:eq(1)  #cboAceiteVin  option:eq(1)').attr("id-comp");
                                if (valor.idComp == idCOmp) {
                                    $(renglon).closest('tr').find('td:eq(1)  #cboAceiteVin').val(valor.idMisc);//asignar el valor
                                    if (valor.prueba == true) {
                                        var chkbx = $(renglon).closest("tr").find("td:eq(2) Input").prop('checked', true);
                                        $(renglon).closest("tr").find("td:eq(3) .InputVida").val(valor.Vigencia);
                                        $(renglon).closest("tr").find("td:eq(3) .InputVida").attr('disabled', false);
                                    } else {
                                        edadIn = $(renglon).closest('tr').find('td:eq(1)  #cboAceiteVin option:selected').attr('data-edad');
                                        $(renglon).closest("tr").find("td:eq(3) .InputVida").val(edadIn);
                                    }
                                    $(renglon).closest("tr").find("td:eq(4) button").attr('disabled', false);
                                    $(renglon).closest("tr").find("td:eq(4) Input").prop('checked', true);//si trae valores lo checka "aplico"

                                    if (valor.Observaciones != null) {
                                        $(renglon).closest("tr").find("td:eq(4) button").attr('style', 'font-size: 2em; color:#da6a1a');
                                    }
                                    $(renglon).closest("tr").find("td:eq(4) button").attr('id', valor.id);

                                    $(renglon).closest("tr").find("td:eq(7) Input").prop('checked', true);
                                    $(renglon).closest("tr").find("td:eq(7) Input").attr('id', valor.id);
                                }
                            }
                        });
                    }
                    //CargaDeAEProyectadoEjecPM(idmantenimiento);
                    if (response.CargaDeAEProyectado.length != 0) {
                        response.CargaDeAEProyectado.forEach(function (valor, indice, array) {
                            var longitud = $("#gridActProxEjecPm >tbody >tr").length
                            for (var i = 0; i <= (longitud - 1) ; i++) {
                                renglon = $("#gridActProxEjecPm >tbody >tr")[i]
                                idAct = $(renglon).closest('tr').find('td:eq(5) input.aplicar').attr("id-act")
                                $(renglon).closest('tr').find('td:eq(5) input.aplicar').attr("idobj", valor.id);
                                if (valor.idAct == idAct) {
                                    $(renglon).closest("tr").find("td:eq(2) button").attr('disabled', false);
                                    $(renglon).closest('tr').find('td:eq(5) input.aplicar').prop('checked', true);
                                    if (valor.Observaciones != null) {
                                        $(renglon).closest("tr").find("td:eq(2) button").attr('style', 'font-size: 2em; color:#da6a1a');
                                    }
                                }
                            }
                        });
                    }
                    //CargaDeDNProyectadoEjecPM(idmantenimiento);
                    if (response.CargaDeDNProyectado.length != 0) {
                        response.CargaDeDNProyectado.forEach(function (valor, indice, array) {
                            var longitud = $("#gridDNProxEjecPm >tbody >tr").length
                            for (var i = 0; i <= (longitud - 1) ; i++) {
                                renglon = $("#gridDNProxEjecPm >tbody >tr")[i]
                                idAct = $(renglon).closest('tr').find('td:eq(5) input.aplicar').attr("id-act")
                                $(renglon).closest('tr').find('td:eq(5) input.aplicar').attr("idobj", valor.id);
                                $("#btnguardarDNObs").attr("idobj", valor.id);
                                if (valor.idAct == idAct) {
                                    $(renglon).closest("tr").find("td:eq(2) button").attr('disabled', false);
                                    $(renglon).closest('tr').find('td:eq(5) input.aplicar').prop('checked', true);
                                    if (valor.Observaciones != null) {
                                        $(renglon).closest("tr").find("td:eq(2) button").attr('style', 'font-size: 2em; color:#da6a1a');
                                    } else {
                                        $(renglon).closest("tr").find("td:eq(2) button").attr('style', 'font-size: 2em; color:gray');
                                    }
                                }
                            }
                        });
                    }
                    //ProgressBarDNEjecPM();

                    //TraerFormatosEjecPM(idmantenimiento, idPM);
                    if (response.success == true && response.objFormato != "") {
                        getFormatosActividadesEjecPM(response.objFormato);
                    }
                    $("#btnEjecPMPaso1").click();

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }
        var modeloID = "";
        function InfoMaquinaEjecPM(economicoiD, idMantenimiento) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarIDmaquinaria',
                data: { EconomicoiD: economicoiD },
                success: function (result) {
                    $("#cboFiltroGrupoS").val(result.items.grupoMaquinariaID).change();
                    $("#cboFiltroGrupoS").attr('disabled', true);
                    $(".txtTipoUltimoServLu").val($("#cboTipoMantenientoContadorSEjecPM option:selected").text());
                    modeloID = result.items.modeloID;
                    loadDataProgramaActividadesLubEjecPM(modeloID, idMantenimiento);

                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });

            getActividadesExtrasEjecPM(modeloID, idMantenimiento);

            getActividadesDNsEjecPM(modeloID, idMantenimiento);
        }
        function FormateoFechaEdicion(Fecha) {
            var dateString = Fecha.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = day + "/" + month + "/" + year;
            return date;
        }
        function loadDataProgramaActividadesLubEjecPM(modeloID, idmantenimiento) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarJGEstructuraLubricantes',
                data: { modeloEquipoID: modeloID, idmantenimiento: idmantenimiento },
                //async: false,
                success: function (response) {
                    let arr = jQuery.grep(response.dataSetGridLubProx, function (a) {
                        return a.objHis !== null;
                    });
                    setTableTlbGridLubEjecPM(arr);

                    initGridLubHisEjecPM($("#gridLubhisEjecPm"));
                    $("#gridLubhisEjecPm").bootgrid("clear");
                    //$("#gridLubhisEjecPm").bootgrid("append", response.JGHis);
                    $.unblockUI();

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function setTableTlbGridLubEjecPM(dataSet) {
            dtGridLubProxEjecPM = $("#tblGridLubProxEjecPm").DataTable({
                language: lstDicEs,
                destroy: true,
                scrollY: false,
                paging: false,
                ordering: false,
                info: false,
                data: dataSet,
                columnDefs: [
                    { className: "dt-center", "targets": [0, 1, 3, 4, 5, 6, 7, 8] }
                ],
                columns: [
                    {
                        data: "componente", width: '120px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).text(cellData)

                        }
                    },
                    {
                        data: "Suministros", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = '';
                            html += '<div class="SelectSum">';
                            html += '<select class="form-control cboAceiteVin" id="cboAceiteVin" style="margin-top:4px;" title="Seleccione:">'
                            html += '<option value="0" selected disabled hidden>Tipo De Aceite:</option>'
                            $.each(cellData, function (i, e) {
                                if (e.edadSuministro.length != 0) {
                                    html += '<option  id-comp="' + e.componenteID + '"   id-misc="' + e.edadSuministro[0].idMis + '"  data-edad="' + e.edadSuministro[0].vida + '"  value="' + e.descripcion[0].id + '">' + e.descripcion[0].nomeclatura + '</option>'
                                }
                            });
                            html += '</select>'
                            html += '</div>';
                            $(td).append(html);

                        }
                    },
                    {
                        data: "TipoPrueba", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            checked = "";
                            html = '';
                            html = "<label  class='btn chkPrueba' >" +
                                "<input type='checkbox'   class='form-control checkedTipoPrueba' style='margin-left:20px;' " + checked + ">" +
                                " </label>";

                            $(td).append(html);
                        }
                    },
                    {
                        data: "VidaUtil", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = '';

                            html = "<input type='number' class='form-control bfh-number InputVida'disabled>";
                            $(td).append(html);
                        }
                    },
                    {
                        data: "Info", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            html = '';
                            html = "<label  class='btn chkAplico'  data-index='" + row + "'" + "'data-id='" + row + "' >" +
                                "<input class='chkAplico'  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off' " + checked + "></span> " +
                                "<span></span> " +
                                " </label>";
                            $(td).append(html);
                        }
                    },
                    {
                        data: "VidaConsumida", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            if (rowData.proyectado == null) {
                                setCboAceites = $(td).parents('tr').find('.cboAceiteVin');
                                $(setCboAceites).val(rowData.proyectado.idMisc);
                                if (rowData.objHist.vidaA == 0) { vidaUtil = parseFloat($('option:selected', $(td).parents('tr').find('.cboAceiteVin')).attr("data-edad")); }
                                else { vidaUtil = rowData.objHis.vidaA; }
                                //vidaUtil = rowData.objHis.vidaA;
                                hrsAplico = rowData.objHis.hrsAplico;
                                $(td).attr('data-aplico', hrsAplico);
                                Barra = setPorcentajesVida(vidaUtil, hrsAplico).Barra; //TODO
                                $(td).append(Barra);
                                //console.log();
                            }
                            else {
                                setCboAceites = $(td).parents('tr').find('.cboAceiteVin');
                                $(setCboAceites).val(rowData.proyectado.idMisc);
                                if (rowData.proyectado.Vigencia == 0) { vidaUtil = parseFloat($('option:selected', $(td).parents('tr').find('.cboAceiteVin')).attr("data-edad")); }
                                else { vidaUtil = rowData.proyectado.Vigencia; }
                                //vidaUtil = rowData.proyectado.Vigencia;
                                hrsAplico = rowData.proyectado.Hrsaplico;
                                $(td).attr('data-aplico', hrsAplico);
                                Barra = setPorcentajesVida(vidaUtil, hrsAplico).Barra;
                                $(td).append(Barra);

                            }

                        }
                    },
                    {
                        data: "VidaRestante", width: '150px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            if (rowData.proyectado == null) {
                                vidaUtil = rowData.objHis.vidaA;
                                hrsAplico = rowData.objHis.hrsAplico;
                                $(td).attr('data-aplico', hrsAplico);
                                Estatus = setPorcentajesVida(vidaUtil, hrsAplico).Estatus;
                                $(td).append(Estatus);

                            }
                            else {

                                vidaUtil = rowData.proyectado.Vigencia;
                                hrsAplico = rowData.proyectado.Hrsaplico;
                                $(td).attr('data-aplico', hrsAplico);
                                Estatus = setPorcentajesVida(vidaUtil, hrsAplico).Estatus;
                                $(td).append(Estatus);

                            }
                        }
                    },
                    {
                        data: "Programar", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            checked = "";
                            html = '';
                            html = "<label  class='btn chkPrueba' >" +
                                "<input type='checkbox' data-componenteIndex='" + rowData.objComponente.idCompVis + "' disabled  class='programarLubricantes' style=' width:20px;  height:20px; margin-left:20px;' " + checked + ">" +
                                " </label>";

                            $(td).append(html);
                        }
                    },
                    {
                        sortable: false, width: '40px',
                        "render": function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            checked = "";
                            var html = '';
                            html = "<label class='btn checkEjecutado'>" +
                                "    <input type='checkbox' data-componenteIndex='" + rowData.objComponente.idCompVis + "' class='ejecutadoLubricantes' style=' width:20px;  height:20px; margin-left:20px;' " + checked + ">" +
                                "</label>";

                            return html;
                        }
                    }


                ],
                "createdRow": function (row, data, index) {

                    objProyectado = data.proyectado;

                    setCboAceites = $(row).find('.cboAceiteVin');
                    setIsPrueba = $(row).find('.checkedTipoPrueba');
                    setInputVida = $(row).find('.InputVida');
                    setIsProgramado = $(row).find('.programarLubricantes');
                    setIsEjecutado = $(row).find('.ejecutadoLubricantes');

                    if (objProyectado != null) {
                        $(setCboAceites).val(objProyectado.idMisc);
                        $(setIsPrueba).prop("checked", objProyectado.prueba);
                        $(setInputVida).val(objProyectado.Vigencia);
                        $(setIsProgramado).prop("checked", objProyectado.programado);
                        $(setIsEjecutado).prop("checked", objProyectado.programado);
                        data.proyectado.aplicado = objProyectado.programado;
                    }
                    else {
                        objHist = data.objHis;
                        objHistAceites = objHist.AceiteVin[0].edadSuministro[0]
                        $(setCboAceites).val(objHistAceites.idMis);
                        $(setIsPrueba).prop("checked", objHist.prueba);
                        $(setInputVida).val(objHist.vidaA);
                        $(setIsProgramado).prop("checked", false);
                        $(setIsEjecutado).prop("checked", false);
                        data.proyectado.aplicado = false;
                    }
                },
                initComplete: function (settings, json) {
                    tblGridLubProxEjecPM.on('click', '.ejecutadoLubricantes', function () {
                        var rowData = dtGridLubProxEjecPM.row($(this).closest('tr')).data();

                        rowData.proyectado.aplicado = $(this).is(":checked");
                    });
                },
            });
            dtGridLubProxEjecPM.draw();

        }
        function initGridLubHisEjecPM(grid) {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                    footer: ""
                },
                formatters: {
                    "componente": function (column, row) {
                        return row.componente[0];
                    },
                    "InputVida": function (column, row) {
                        return ($(".txtHorometroLubricantes").val() - row.hrsAplico).toFixed(2);
                    },
                    "VidaRest": function (column, row) {
                        aplico = "";
                        if (row.aplico == true) {
                            return ((row.vidaA + row.hrsAplico) - ($(".txtHorometroLubricantes").val())).toFixed(2);
                        } else {
                            return (row.vidaA - ($(".txtHorometroLubricantes").val() - row.hrsAplico)).toFixed(2);
                        }
                    },
                    "HorServ": function (column, row) {
                        return row.hrsAplico;
                    },
                    "estatus": function (column, row) {
                        aplico = "";
                        HorometroActual = $(".txtHorometroLubricantes").val();
                        if (row.aplico == true) {
                            HorometroServicio = (((row.hrsAplico + row.vidaA))).toFixed(2);
                            HorasTranscurridas = ($(".txtHorometroLubricantes").val() - (row.hrsAplico + row.vidaA)).toFixed(2);
                            Patron = (row.vidaA - ($(".txtHorometroLubricantes").val() - (row.hrsAplico + row.vidaA))).toFixed(2);
                        } else {
                            HorometroServicio = row.hrsAplico;
                            HorasTranscurridas = ($(".txtHorometroLubricantes").val() - row.hrsAplico).toFixed(2);
                            Patron = (row.vidaA - ($(".txtHorometroLubricantes").val() - row.hrsAplico)).toFixed(2);
                        }
                        if (Patron >= 250) {
                            return ("<button type='button'  style='font-size: larger; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                        } else if (Patron < 250 && Patron >= 0) {
                            return ("<button type='button' style='font-size: larger; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                        } else if (Patron < 0) {
                            return ("<button type='button'  style='font-size: larger; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                        }
                    },
                    "EdadSum": function (column, row) {
                        return row.vidaA;
                    },
                    "AceiteVin": function (column, row) {
                        return row.AceiteVin[0].descripcion[0].nomeclatura;
                    },
                    "prueba": function (column, row) {
                        if (row.prueba == true) {
                            return 'SI';
                        } else {
                            return 'NO';
                        }
                    },
                    "ObservacionesLubis": function (column, row) {
                        return ("<button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-comments'></span></button>");
                    },
                    "ProxLub": function (column, row) {
                        return ("<a href='#gridLubProx'><button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-down'></span></button></a>");
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
            });
        }
        function getActividadesExtrasEjecPM(modeloID, maquinariaID) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/getActividadesExtras',
                data: { modeloEquipoID: modeloID, idMantenimiento: maquinariaID },
                success: function (response) {
                    if (response.success == true && response.objActividadesExtras != "") {
                        initGridActHisEjecPM($("#gridActhisEjecPm"));
                        $("#gridActhisEjecPm").bootgrid("clear");
                        $("#gridActhisEjecPm").bootgrid("append", response.objActividadesExtrashis);

                        setTableGridActividadesExtraEjecPM(response.objActividadesExtras)

                        $.unblockUI();
                    } else {
                        AlertaModal("Error", "No existe Informacion de Actividades Extras Relacionados al Modelo   '" + modeloEquipoID + "'");
                        modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });

        }
        function getActividadesDNsEjecPM(modeloID, idMantenimiento) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/getActividadesDNS',
                data: { modeloEquipoID: modeloID, idMantenimiento: idMantenimiento },
                success: function (response) {
                    if (response.success == true/* && response.objActividadesDN != ""*/) {
                        initGridDNhisEjecPm($("#gridDNHisEjecPm"));
                        $("#gridDNHisEjecPm").bootgrid("clear");
                        $("#gridDNHisEjecPm").bootgrid("append", response.objActividadesDNHis);

                        setTableGridDNsEjecPM(response.objActividadesDN);
                        $.unblockUI();
                    } else {
                        AlertaModal("Error", "No existe Informacion de DN'S Relacionados al Modelo   '" + modeloID + "'");
                        modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function initGridActHisEjecPM(grid) {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                    footer: ""
                },
                formatters: {
                    "marca": function (column, row) {
                        if (row.marca == 1) {
                            row.marca = "Caterpillar"
                        } else if (row.marca == 2) {
                            row.marca = "Donaldson"
                        }
                        return row.marca
                    },
                    "HorServ": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number HorServ'>";
                    },
                    "TiempoTrans": function (column, row) {
                        return ($(".txtHorometroLubricantes").val() - (row.Hrsaplico)).toFixed(2);
                    },
                    "hrsRest": function (column, row) {
                        return (row.perioricidad - (($(".txtHorometroLubricantes").val() - (row.Hrsaplico)))).toFixed(2);
                    },
                    "chkAplico": function (column, row) {
                        if (row.asignado != null) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        id = 0;
                        if (row.asignado != null) {
                            id = row.asignado.id;
                        }
                        return "<label  class='btn chkAplico'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'data-id='" + id + "' >" +
                            "<input  class='chkAplico' style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off' " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    },
                    "ObservacionesActhis": function (column, row) {
                        return ("<button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-comments'></span></button>");
                    },
                    "ProxAct": function (column, row) {
                        return ("<a href='#gridActProx'><button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-down'></span></button></a>");
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".HorServ").on("change", function (e) {
                    HorometroActual = $("#btnHorometro2").html();
                    HorometroServicio = $(this).closest("tr").find("td:eq(1) .HorServ ").val();
                    HorasTranscurridas = HorometroActual - HorometroServicio;
                    $(this).closest("tr").find("td:eq(3) .TiempoTrans").val(HorasTranscurridas.toFixed(2));
                    Patron = $(this).closest("tr").find("td:eq(2) .perioricidad").val() - $(this).closest("tr").find("td:eq(3) .TiempoTrans").val();
                    $(this).closest("tr").find("td:eq(4) .hrsRest").val(Patron.toFixed(2));
                    var a = $(this).closest("tr").find("td:eq(5)").html("");
                    if (Patron >= 250) {
                        $(this).closest("tr").find("td:eq(5)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                    } else if (Patron < 250 && Patron >= 0) {
                        $(this).closest("tr").find("td:eq(5)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                    } else if (Patron < 0) {
                        $(this).closest("tr").find("td:eq(5)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                    }
                });

            });
        }
        function setTableGridActividadesExtraEjecPM(dataSet) {
            tblGridActProxEjecPm = $("#tblGridActProxEjecPm").DataTable({
                language: lstDicEs,
                "bFilter": false,
                destroy: true,
                scrollY: "300px",
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                columns: [
                    {
                        data: "actividad", width: '120px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).text(cellData)

                        }
                    },
                    {
                        data: "vidaUtil", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = '';
                            html = "<input type='number'   data-idAct='" + row.idAct + "'  data-index='" + row.id + "'  value='" + cellData + "' class='form-control bfh-number perioricidad' disabled>";
                            $(td).append(html);
                        }
                    },
                    {
                        data: "info", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            html = '';

                            html = "<button type='button' class='chkAplico'  style='font-size: 2em; color:gray;'> <span class='fa fa-comments'></span></button>";

                            $(td).append(html);
                        }
                    },
                    {
                        data: "vidaConsumida", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            vidaUtil = rowData.vidaUtil;
                            hrsAplico = rowData.hrsAplico;

                            Barra = setPorcentajesVida(vidaUtil, hrsAplico).Barra;
                            $(td).append(Barra);
                        }

                    },
                    {
                        data: "vidaRestante", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            vidaUtil = rowData.vidaUtil;
                            hrsAplico = rowData.hrsAplico;
                            Estatus = setPorcentajesVida(vidaUtil, hrsAplico).Estatus;
                            $(td).append(Estatus);
                        }
                    },
                    {
                        data: "programar", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            checked = "";
                            html = '';
                            html = "<label  class='btn chkPrueba' >" +
                                "<input type='checkbox'   class='form-control programarActividad' disabled style='margin-left:20px;' " + checked + ">" +
                                " </label>";

                            $(td).append(html);
                        }
                    },
                    {
                        sortable: false, width: '40px',
                        "render": function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            checked = "";
                            var html = '';
                            html = "<label  class='btn checkEjecutado' >" +
                                "    <input type='checkbox' class='form-control ejecutadoActividad' style='margin-left:20px;' " + checked + ">" +
                                "</label>";

                            return html;
                        }
                    },

                ],
                "createdRow": function (row, data, index) {
                    proyectado = data.proyectado;
                    setProgramarActividad = $(row).find('.programarActividad');

                    if (proyectado != null) {
                        $(setProgramarActividad).prop('checked', proyectado.programado);
                    }
                    else {
                        $(setProgramarActividad).prop('checked', false);
                    }
                },
                initComplete: function (settings, json) {
                    tblGridActProxEjecPm.on('click', '.ejecutadoActividad', function () {
                        var rowData = tblGridActProxEjecPm.row($(this).closest('tr')).data();

                        rowData.proyectado.aplicado = $(this).is(":checked");
                    });
                },
                "paging": true,
                "info": false
            });

            tblGridActProxEjecPm.draw();
        }
        function initGridDNhisEjecPm(grid) {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                    footer: ""
                },
                formatters: {
                    "marca": function (column, row) {
                        if (row.marca == 1) {
                            row.marca = "Caterpillar"
                        } else if (row.marca == 2) {
                            row.marca = "Donaldson"
                        }
                        return row.marca
                    },
                    "HorServ": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number HorServ'>";
                    },
                    "perioricidad": function (column, row) {
                        return row.perioricidad;
                    },
                    "TiempoTrans": function (column, row) {
                        return ($(".txtHorometroLubricantes").val() - (row.Hrsaplico)).toFixed(2);
                    },
                    "hrsRest": function (column, row) {
                        return ((row.perioricidad + row.Hrsaplico) - ($(".txtHorometroLubricantes").val())).toFixed(2);
                    },
                    "estatus": function (column, row) {
                        return "<button type='button'  style='font-size: 2em; color:red' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>";
                    },
                    "chkAplico": function (column, row) {
                        if (row.asignado != null) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        id = 0;
                        if (row.asignado != null) {
                            id = row.asignado.id;
                        }
                        return "<label  class='btn chkAplico'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'data-id='" + id + "' >" +
                            "<input  class='chkAplico' style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off' " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    },
                    "ObservacionesActhis": function (column, row) {
                        return ("<button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-comments'></span></button>");
                    },
                    "ProxAct": function (column, row) {
                        return ("<a href='#gridDNProx'><button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-down'></span></button></a>");
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".HorServ").on("change", function (e) {
                    HorometroActual = $("#btnHorometro2").html();
                    HorometroServicio = $(this).closest("tr").find("td:eq(1) .HorServ ").val();
                    HorasTranscurridas = HorometroActual - HorometroServicio;
                    $(this).closest("tr").find("td:eq(3) .TiempoTrans").val(HorasTranscurridas.toFixed(2));
                    Patron = $(this).closest("tr").find("td:eq(2) .perioricidad").val() - $(this).closest("tr").find("td:eq(3) .TiempoTrans").val();
                    $(this).closest("tr").find("td:eq(4) .hrsRest").val(Patron.toFixed(2));
                    var a = $(this).closest("tr").find("td:eq(5)").html("");
                    if (Patron >= 250) {
                        $(this).closest("tr").find("td:eq(5)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                    } else if (Patron < 250 && Patron >= 0) {
                        $(this).closest("tr").find("td:eq(5)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                    } else if (Patron < 0) {
                        $(this).closest("tr").find("td:eq(5)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                    }
                });
            });
        }
        function setTableGridDNsEjecPM(dataSet) {
            tblgridDNProxEjecPM = $("#tblgridDNProxEjecPm").DataTable({
                language: lstDicEs,
                "bFilter": false,
                destroy: true,
                scrollY: "300px",
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                columns: [
                    {
                        data: "actividad", width: '120px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).text(cellData)

                        }
                    },
                    {
                        data: "vidaUtil", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = '';
                            html = "<input type='number'   data-idAct='" + row.idAct + "'  data-index='" + row.id + "'  value='" + cellData + "' class='form-control bfh-number perioricidad' disabled>";
                            $(td).append(html);
                        }
                    },
                    {
                        data: "info", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            html = '';

                            html = "<button type='button' class='chkAplico' disabled  style='font-size: 2em; color:gray;'> <span class='fa fa-comments'></span></button>";

                            $(td).append(html);
                        }
                    },
                    {
                        data: "vidaConsumida", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            vidaUtil = rowData.vidaUtil;
                            hrsAplico = rowData.Hrsaplico;

                            Barra = setPorcentajesVida(vidaUtil, hrsAplico).Barra;
                            $(td).append(Barra);
                        }

                    },
                    {
                        data: "vidaRestante", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            vidaUtil = rowData.vidaUtil;
                            hrsAplico = rowData.Hrsaplico;
                            Estatus = setPorcentajesVida(vidaUtil, hrsAplico).Estatus;
                            $(td).append(Estatus);
                        }
                    }, {
                        data: "programar", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            checked = "";
                            html = '';
                            html = "<label  class='btn chkPrueba' >" +
                                "<input type='checkbox'   class='form-control programarActividad' disabled style='margin-left:20px;' " + checked + ">" +
                                " </label>";
                            $(td).append(html);
                        }
                    },

                    {
                        sortable: false, width: '40px',
                        "render": function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            checked = "";
                            var html = '';
                            html = "<label  class='btn checkEjecutado' >" +
                                "    <input type='checkbox' class='form-control ejecutadoActividad' style='margin-left:20px;' " + checked + ">" +
                                "</label>";

                            return html;
                        }
                    }


                ],
                "createdRow": function (row, data, index) {
                    proyectado = data.proyectado;
                    setProgramarActividad = $(row).find('.programarActividad');

                    if (proyectado != null) {
                        $(setProgramarActividad).prop('checked', proyectado.programado);
                    }
                    else {
                        $(setProgramarActividad).prop('checked', false);
                    }
                },
                initComplete: function (settings, json) {
                    tblgridDNProxEjecPM.on('click', '.ejecutadoActividad', function () {
                        var rowData = tblgridDNProxEjecPM.row($(this).closest('tr')).data();

                        rowData.proyectado.aplicado = $(this).is(":checked");
                    });

                },
                "paging": true,
                "info": false
            });

            tblgridDNProxEjecPM.draw();
        }
        function setPorcentajesVida(vidaUtil, hrsAplico) {

            Estatus = "";
            Barra = "";

            horometroActual = $("#HorometroAplicoSEjecPM").val();// _horometroEjecucionPM > 0 ? _horometroEjecucionPM : +($("#horometroMaquinaSEjecPM").val());
            VidaAceite = horometroActual - hrsAplico;
            VidaRestante = vidaUtil - VidaAceite;
            Vida = (VidaAceite * 100) / vidaUtil;
            Patron = VidaRestante;
            Estatus = "";

            if (Patron >= 250) {
                Estatus = "<button type='button'  style=' color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                    "<div class='progress-bar progress-bar-success' style='width:" + Vida + "%; color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            } else if (Patron < 250 && Patron >= 0) {
                Estatus = "<button type='button'  style='color:orange; ' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                    "<div class='progress-bar progress-bar-warning' style='width:" + Vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            } else if (Patron < 0) {
                Estatus = "<button type='button'  style='color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full  fa-spin'></span></button>" + "<button type='button'  style='color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                    "<div class='progress-bar progress-bar-danger' style='width:" + Vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            }

            return objReturn = {

                Estatus: Estatus,
                Barra: Barra
            };

        }
        function ConsultaActividadesProgEjecPM(modeloEquipoID, idPM, idmantenimiento) {
            $.blockUI({ message: mensajes.PROCESANDO });

            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarGestorPM',
                data: { modeloEquipoID: modeloEquipoID, idPM: idPM, idmantenimiento: idmantenimiento },
                success: function (response) {
                    $.unblockUI();
                    $("#grid_GestorActividadesEjecPM").bootgrid("clear");
                    $("#grid_GestorActividadesEjecPM").bootgrid("append", response.ObjActProy);
                },
                error: function () {
                    $.unblockUI();
                }
            });

            $("#grid_GestorActividadesEjecPM-footer .pagination li.page-" + paginaActual + " .button").click();
        }

        function ReturnModeloEjecPM() {
            var retval;
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarModelo',
                data: { noEconmico: $($(".txtNoEconomicoAltS")[0]).val() },
                success: function (response) {
                    $.unblockUI();
                    retval = response.Actividad;
                },
                error: function () {
                    $.unblockUI();
                }
            });
            return retval;
        }
        function ProgressBarLubEjecPM() {
            arrVidas = [];
            var Long = $("#gridLubhisEjecPm tbody tr")
            var longitud = Long.length
            for (var i = 0; i <= longitud - 1; i++) {
                var renglon = Long[i];
                VidaRestante = $(renglon).find('td:eq(6)').html();
                VidaAceite = $(renglon).find('td:eq(3)').html();
                EdadSuministro = $(renglon).find('td:eq(5)').html();
                Vida = (VidaAceite * 100) / EdadSuministro;
                //regla de tres para el porcentaje
                Componente = $(renglon).find('td:eq(0)').html();
                objComponente = { componente: Componente, vida: Vida, VidaRestante: VidaRestante };
                arrVidas.push(objComponente);

            }
            var Long = $("#gridLubProxEjecPm tbody tr")
            var longitud = Long.length
            for (var i = 0; i <= longitud; i++) {
                var renglon = Long[i];
                Componente1 = $(renglon).find('td:eq(0)').html();
                $.each(arrVidas, function (index, value) {
                    if (value.componente == Componente1) {
                        vida = value.vida;


                        Patron = value.VidaRestante;
                        Estatus = "";
                        if (Patron >= 250) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='font-size: 2em; color:black; width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-success' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";
                        } else if (Patron < 250 && Patron >= 0) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:orange; ' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='font-size: 2em; color:black;  width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-warning' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";
                        } else if (Patron < 0) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full  fa-spin'></span></button>" + "<button type='button'  style='font-size: 2em; color:black;width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-danger' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";
                        }
                        $(renglon).find('td:eq(5)').html("");
                        $(renglon).find('td:eq(5)').append(Barra);
                        $(renglon).find('td:eq(6)').html("");

                        $(renglon).find('td:eq(6)').append(Estatus);
                    }
                });
            }
        }
        function ProgressBarActProxEjecPM() {
            arrVidasAct = [];
            var LongAct = $("#gridActhisEjecPm tbody tr");
            var longitud = LongAct.length;
            for (var i = 0; i <= longitud - 1; i++) {
                var renglon = LongAct[i];
                VidaRestante = $(renglon).find('td:eq(4)').html();
                VidaAceite = $(renglon).find('td:eq(1)').html();
                EdadSuministro = $(renglon).find('td:eq(3)').html();
                Vida = (VidaAceite * 100) / EdadSuministro;
                //regla de tres para el porcentaje
                Actividad = $(renglon).find('td:eq(0)').html();
                objActividad = { Actividad: Actividad, vida: Vida, VidaRestante: VidaRestante };
                arrVidasAct.push(objActividad);
            }
            var Long = $("#gridActProxEjecPm tbody tr");
            var longitud = Long.length
            for (var i = 0; i <= longitud; i++) {
                var renglon = Long[i];
                Actividad1 = $(renglon).find('td:eq(0)').html();
                $.each(arrVidasAct, function (index, value) {
                    if (value.Actividad == Actividad1) {
                        vida = value.vida;
                        Patron = value.VidaRestante;
                        Estatus = "";
                        if (Patron >= 250) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='font-size: 2em; color:black; width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-success' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";
                        } else if (Patron < 250 && Patron >= 0) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:orange; ' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='font-size: 2em; color:black;  width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-warning' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";
                        } else if (Patron < 0) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full  fa-spin'></span></button>" + "<button type='button'  style='font-size: 2em; color:black;width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-danger' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";
                        }
                        $(renglon).find('td:eq(3)').html("");
                        $(renglon).find('td:eq(3)').append(Barra);
                        $(renglon).find('td:eq(4)').html("");
                        $(renglon).find('td:eq(4)').append(Estatus);
                    }

                });
            }
        }
        function CargaDeProyectadoEjecPM(idmantenimiento) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/CargaDeProyectado',
                data: { idmantenimiento: idmantenimiento },
                success: function (response) {
                    $.unblockUI();
                    if (response.items.length != 0) {
                        response.items.forEach(function (valor, indice, array) {
                            var longitud = $("#gridLubProxEjecPm >tbody >tr").length
                            for (var i = 0; i <= (longitud - 1); i++) {
                                renglon = $("#gridLubProxEjecPm >tbody >tr")[i]
                                idCOmp = $(renglon).closest('tr').find('td:eq(1)  #cboAceiteVin  option:eq(1)').attr("id-comp");
                                if (valor.idComp == idCOmp) {
                                    $(renglon).closest('tr').find('td:eq(1)  #cboAceiteVin').val(valor.idMisc);//asignar el valor
                                    if (valor.prueba == true) {
                                        var chkbx = $(renglon).closest("tr").find("td:eq(2) Input").prop('checked', true);
                                        $(renglon).closest("tr").find("td:eq(3) .InputVida").val(valor.Vigencia);
                                        $(renglon).closest("tr").find("td:eq(3) .InputVida").attr('disabled', false);
                                    } else {
                                        edadIn = $(renglon).closest('tr').find('td:eq(1)  #cboAceiteVin option:selected').attr('data-edad');
                                        $(renglon).closest("tr").find("td:eq(3) .InputVida").val(edadIn);
                                    }
                                    $(renglon).closest("tr").find("td:eq(4) button").attr('disabled', false);
                                    $(renglon).closest("tr").find("td:eq(4) Input").prop('checked', true);//si trae valores lo checka "aplico"

                                    if (valor.Observaciones != null) {
                                        $(renglon).closest("tr").find("td:eq(4) button").attr('style', 'font-size: 2em; color:#da6a1a');
                                    }
                                    $(renglon).closest("tr").find("td:eq(4) button").attr('id', valor.id);

                                    $(renglon).closest("tr").find("td:eq(7) Input").prop('checked', true);
                                    $(renglon).closest("tr").find("td:eq(7) Input").attr('id', valor.id);
                                }
                            }
                        });
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        };
        function CargaDeAEProyectadoEjecPM(idmantenimiento) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/CargaDeAEProyectado',
                data: { idmantenimiento: idmantenimiento },
                success: function (response) {
                    $.unblockUI();
                    if (response.items.length != 0) {
                        response.items.forEach(function (valor, indice, array) {
                            var longitud = $("#gridActProxEjecPm >tbody >tr").length
                            for (var i = 0; i <= (longitud - 1); i++) {
                                renglon = $("#gridActProxEjecPm >tbody >tr")[i]
                                idAct = $(renglon).closest('tr').find('td:eq(5) input.aplicar').attr("id-act")
                                $(renglon).closest('tr').find('td:eq(5) input.aplicar').attr("idobj", valor.id);
                                if (valor.idAct == idAct) {
                                    $(renglon).closest("tr").find("td:eq(2) button").attr('disabled', false);
                                    $(renglon).closest('tr').find('td:eq(5) input.aplicar').prop('checked', true);
                                    if (valor.Observaciones != null) {
                                        $(renglon).closest("tr").find("td:eq(2) button").attr('style', 'font-size: 2em; color:#da6a1a');
                                    }
                                }
                            }
                        });
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        };
        function CargaDeDNProyectadoEjecPM(idmantenimiento) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/CargaDeDNProyectado',
                data: { idmantenimiento: idmantenimiento },
                success: function (response) {
                    $.unblockUI();
                    if (response.items.length != 0) {
                        response.items.forEach(function (valor, indice, array) {
                            var longitud = $("#gridDNProxEjecPm >tbody >tr").length
                            for (var i = 0; i <= (longitud - 1); i++) {
                                renglon = $("#gridDNProxEjecPm >tbody >tr")[i]
                                idAct = $(renglon).closest('tr').find('td:eq(5) input.aplicar').attr("id-act")
                                $(renglon).closest('tr').find('td:eq(5) input.aplicar').attr("idobj", valor.id);
                                $("#btnguardarDNObs").attr("idobj", valor.id);
                                if (valor.idAct == idAct) {
                                    $(renglon).closest("tr").find("td:eq(2) button").attr('disabled', false);
                                    $(renglon).closest('tr').find('td:eq(5) input.aplicar').prop('checked', true);
                                    if (valor.Observaciones != null) {
                                        $(renglon).closest("tr").find("td:eq(2) button").attr('style', 'font-size: 2em; color:#da6a1a');
                                    } else {
                                        $(renglon).closest("tr").find("td:eq(2) button").attr('style', 'font-size: 2em; color:gray');
                                    }
                                }
                            }
                        });
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function ProgressBarDNEjecPM() {
            arrVidasDNProx = [];
            var LongDN = $("#gridDNHisEjecPm tbody tr");
            var longitud = LongDN.length;
            for (var i = 0; i <= longitud - 1; i++) {
                var renglon = LongDN[i];
                VidaRestante = $(renglon).find('td:eq(4)').html();
                VidaAceite = $(renglon).find('td:eq(1)').html();
                EdadSuministro = $(renglon).find('td:eq(3)').html();
                Vida = (VidaAceite * 100) / EdadSuministro;
                Actividad = $(renglon).find('td:eq(0)').html();
                objDNProx = { Actividad: Actividad, vida: Vida, VidaRestante: VidaRestante };
                arrVidasDNProx.push(objDNProx);
            };
            var Long = $("#gridDNProxEjecPm tbody tr");
            var longitud = Long.length;
            for (var i = 0; i <= longitud; i++) {
                var renglon = Long[i];
                Actividad1 = $(renglon).find('td:eq(0)').html();
                $.each(arrVidasDNProx, function (index, value) {
                    if (value.Actividad == Actividad1) {
                        vida = value.vida;
                        Patron = value.VidaRestante;
                        Estatus = "";
                        if (Patron >= 250) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='font-size: 2em; color:black; width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-success' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";
                        } else if (Patron < 250 && Patron >= 0) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:orange; ' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='font-size: 2em; color:black;  width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-warning' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";

                        } else if (Patron < 0) {
                            Estatus = "<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full  fa-spin'></span></button>" + "<button type='button'  style='font-size: 2em; color:black;width:150px;height:50px' id='btndrop'>" + value.VidaRestante + "</button>";
                            Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                                "<div class='progress-bar progress-bar-danger' style='width:" + value.vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + value.vida.toFixed(2) + "%" + "</div>";
                        }
                        $(renglon).find('td:eq(3)').html("");
                        $(renglon).find('td:eq(3)').append(Barra);
                        $(renglon).find('td:eq(4)').html("")
                        $(renglon).find('td:eq(4)').append(Estatus);
                    }

                });
            }
        }
        function initGridtablaPMGestorEjecPM(grid) {
            grid.bootgrid({
                rowCount: [5],
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "Aplicar": function (column, row) {
                        if (row.aplicar != false) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        if (row.PM != 0) {
                            return "<input type='checkbox' data-id='" + row.idobjProy + "' data-index='" + row.id + "'  class='form-control chkAplico' style='width:40px;  height:40px; margin-left:20px;' id-act='" + row.id + "'" + checked + ">";
                        } else {
                            return "<button type='button' disabled class='btn btn-default'>" +
                                "<span class='fa fa-ban'></span> " +
                                "</button>";
                        }
                    },
                    "Quitar": function (column, row) {
                        if (row.PM != 0) {
                            return "<button type='button' disabled class='btn btn-default'>" +
                                "<span class='fa fa-ban'></span> " +
                                "</button>";
                        } else {
                            return "<button type='button' data-id='" + row.idobjProy + "' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                                "<span class='glyphicon glyphicon-remove'></span> " +
                                "</button>";
                        }
                    },
                    "ObservacionesActs": function (column, row) {
                        if (row.id == 1 || row.id == 16 || row.id == 59) {//seteado e
                            if ((row.Observacion == null || row.Observacion == "") && row.aplicar == true) {
                                return ("<button type='button'   class='ObsActs' style='font-size: 2em; color:gray;'  idAct='" + row.id + "'  idobjProy='" + row.idobjProy + "'  idobs='" + row.Observacion + "'> <span class='fa fa-cog'></span></button>");
                            } else if (row.aplicar == false) {
                                return ("<button type='button' disabled class='ObsActs' style='font-size: 2em; color:gray;' idAct='" + row.id + "'  idobjProy='" + row.idobjProy + "' idobs='" + row.Observacion + "'> <span class='fa fa-cog'></span></button>");
                            } else {
                                return ("<button type='button'  disabled class='btn btn-default ObsActs' style='font-size: 2em; color:#da6a1a;'  idAct='" + row.id + "'   idobjProy='" + row.idobjProy + "'> <span class='fa fa-cog'></span></button>");
                            }
                        } else {
                            return "<button type='button' disabled class='btn btn-default ObsActs'>" +
                                "<span class='fa fa-ban'></span> " +
                                " </button>";
                        }
                    },
                    "descripcion": function (column, row) {
                        return "<label  class='btn ' data-id='" + row.id + "' >" +
                            row.descripcion +
                            " </label>"
                    },
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".chkAplico").on("click", function (e) {
                    pm = ReturnPm();

                    if ($(this).is(":checked")) {
                        $(this).closest("tr").find("td:eq(2) .ObsActs").attr("disabled", false);
                        objActvProy = { id: $(this).attr("data-id"), idAct: $(this).attr("data-index"), idMant: $("#MantenimientoSeguimiento").attr('idmant'), Observaciones: "", aplicar: true, estatus: $(this).is(":checked"), idPm: pm };
                        GuardarActividadesProyectadas(objActvProy);
                    } else {
                        $(this).closest("tr").find("td:eq(2) .ObsActs").attr("disabled", true);
                        objActvProy = { id: $(this).attr("data-id"), idAct: $(this).attr("data-index"), idMant: $("#MantenimientoSeguimiento").attr('idmant'), Observaciones: "", aplicar: false, estatus: $(this).is(":checked"), idPm: pm };
                        GuardarActividadesProyectadas(objActvProy);
                    }
                });
                grid.find(".ObsActs").on("click", function (e) {
                    $(".txtActividadAltS").val($(this).closest("tr").find("td:eq(0) label").html());
                    $(".txtActividadAltS").attr("idAct", $(this).closest("tr").find("td:eq(0) label").attr("data-id"));
                    $(".txtActividadAltS").attr("idAct", $(this).closest("tr").find("td:eq(0) label").attr("data-id"));
                    idObjAct = $(this).attr("idobjproy");
                    idobs = $(this).attr("idobs");
                    if (idObjAct != 0 && idobs == undefined) {
                        ConsultarObservacionActividad(idObjAct);
                    }
                    $("#btnguardarActObs").attr("idObj", idObjAct);
                    $("#modalObsActs").modal("show");
                });
                grid.find(".eliminar").on("click", function (e) {
                    objActvProy = { id: $(this).attr("data-id"), idAct: $(this).attr("data-index"), idMant: $("#MantenimientoSeguimiento").attr('idmant'), Observaciones: "", aplicar: true, estatus: true, idPm: ReturnPm() };
                    GuardarActividadesProyectadas(objActvProy);
                });
            });
        }
        function TraerFormatosEjecPM(idmant, idpm) {
            //$("#btnReporteMant").attr("idmant", idmant);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/getFormato',
                data: { id: idmant, idpm: idpm },
                success: function (response) {
                    if (response.success == true && response.Actividad != "") {
                        getFormatosActividadesEjecPM(response.obj);

                        $.unblockUI();
                    } else {
                        AlertaModal("Error", "No existe Información de Actividades Extras Relacionados al Modelo   '" + modeloEquipoID + "'");
                        modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        function getFormatosActividadesEjecPM(dataSet) {
            var Tamaño = ($(window).width() * 53) / 1366;
            tamañoY = Tamaño + 'vh';
            tblGridFormatosEjecPm = $("#tblGridFormatosEjecPm").DataTable({
                language: lstDicEs,
                "bFilter": false,
                destroy: true,
                scrollY: tamañoY,
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                initComplete: function (settings, json) {
                    tblGridFormatosEjecPm.on('click', '.custom-file-upload', function () {
                        var rowData = tblGridFormatosEjecPm.row($(this).closest('tr')).data();

                        $(this).next('input').click();
                    });

                    tblGridFormatosEjecPm.on('change', '.inputFormatoMantenimiento', function () {
                        var rowData = tblGridFormatosEjecPm.row($(this).closest('tr')).data();
                    });
                },
                columns: [
                    {
                        data: "descripcion",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).text(cellData);
                        }
                    },
                    {
                        data: "idformato",
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).text(cellData.nombreArchivo);
                        }
                    },
                    {
                        data: "id",
                        createdCell: function (td, cellData, rowData, row, col) {
                            var html = '';
                            $(td).text('');
                            checked = "";

                            html += '<div class="custom-file-upload" style="text-align: center;">';
                            html += '   <label class="custom-file-upload">';
                            html += '       <i class="glyphicon glyphicon-floppy-open"></i>';
                            html += '   </label>';
                            html += '   <input type="file" class="form-control inputFormatoMantenimiento" accept=".pdf, .xls, .xlsx" multiple id-actividad="' + rowData.idAct + '">';
                            html += '</div>';

                            $(td).append(html);
                        }
                    }
                ],
                "paging": true,
                "info": false
            });
        }
        //---

        function saveEjecutado() {
            var fechaUltCapturado = $.datepicker.parseDate("dd/mm/yy", fechaUltCalSEjecPM.val());
            var fechaPM = $.datepicker.parseDate("dd/mm/yy", fechaIniManteSEjecPM.val());
            var fechaProy = $.datepicker.parseDate("dd/mm/yy", fechaFinManteSEjecPM.val());
            var captura = new Date();

            objGeneral = {
                economicoID: modalEjecucionPMdetalle.find('.txtNoEconomicoAltS').val(),
                horometroUltCapturado: horometroMaquinaSEjecPM.val(),
                fechaUltCapturado: fechaUltCapturado,
                tipoPM: cboTipoMantenientoContadorSEjecPM.val(),
                //tipomantenimiento: cboTipoMantenientoContadorSEjecPM.find('option:selected').text().trim(),
                fechaPM: fechaPM,
                horometroPM: HorometroAplicoSEjecPM.val(),
                observaciones: ObservacionesMantSEjecPM.val(),
                horometroProy: HorometroProyectadoSEjecPM.val(),
                fechaProy: fechaProy,
                tipoMantenimientoProy: cboTipoMantenientoContadorSEjecPM.find('option:contains(' + MantenimientoProySEjecPM.val() + ')').val(),
                actual: 1,
                estatus: true,
                horometroPMEjecutado: HorometroAplicoSEjecPM.val(),
                estadoMantenimiento: 3,
                personalRealizo: tbNombreEmpleadoSEjecPM.attr("data-NumEmpleado"),
            };

            arrayDataLubricantes = new Array();
            arrayDataActividadesExtra = new Array();
            arrayDataDN = new Array();

            let lub = [];
            dtGridLubProxEjecPM.data().each(function (value, index) {
                proyectado = value.proyectado;

                if (proyectado != null) {
                    lub.push(JSON.stringify(value.proyectado));
                }
            });
            let lubArray = "[" + lub + "]";

            let actExt = [];
            tblGridActProxEjecPm.data().each(function (value, index) {
                proyectado = value.proyectado;
                if (proyectado != null) {
                    actExt.push(JSON.stringify(value.proyectado));
                }
            });
            let actExtArray = "[" + actExt + "]";

            let dnArr = [];
            tblgridDNProxEjecPM.data().each(function (value, index) {
                proyectado = value.proyectado;
                if (proyectado != null) {
                    dnArr.push(JSON.stringify(value.proyectado));
                }
            });
            let dnArray = "[" + dnArr + "]";

            let formData = new FormData();

            formData.append('objGeneral', JSON.stringify(objGeneral));
            formData.append('idMantenimiento', _idMantenimiento);
            formData.append('lubricantes[]', lubArray);
            formData.append('actividadesExtra[]', actExtArray);
            formData.append('dns[]', dnArray);

            var file1 = document.getElementById("upLoadArchivos").files[0];

            formData.append("files[]", file1);

            //$('#tblGridFormatosEjecPm').find('tbody tr').each(function (value, index) {
            //    var archs = $(this).find('.inputFormatoMantenimiento')[0].files

            //    $.each(archs, function (i, file) {
            //        formData.append("files[]", file);
            //    });
            //});

            //sendEjecutadoSave(objGeneral, arrayDataLubricantes, arrayDataActividadesExtra, arrayDataDN);
            sendEjecutadoSave(formData);
        }
        //function sendEjecutadoSave(objGeneral, tblGridLubProxTbl, tblGridActProxTbl, tblgridDNProxTbl) {
        function sendEjecutadoSave(formData) {
            $.blockUI({ message: mensajes.PROCESANDO });

            var request = new XMLHttpRequest();
            request.open("POST", "/MatenimientoPM/sendEjecutadoSave");
            request.send(formData);
            request.onload = function (response) {
                if (request.status == 200) {
                    $.unblockUI();
                    AlertaGeneral('Información Guardada', 'Se ha capturado la información del P.M.');
                    tblMantenimientosProg.ajax.reload();
                    modalEjecucionPMdetalle.modal('hide');
                    modalEjecucionPM.modal('hide');
                    $("#cboObras").trigger('change');
                } else {
                    $.unblockUI();
                    AlertaGeneral('Alerta', 'No se pudo guardar la información.');
                    tblMantenimientosProg.ajax.reload();
                    modalEjecucionPMdetalle.modal('hide');
                    modalEjecucionPM.modal('hide');
                }
            };
        }

        function ValidarAltaFiltro() {
            var estatus = true;
            if ($("#txtDescripcionAltaFiltro").val().trim() == "" || $("#txtDescripcionAltaFiltro").val() == null) { estatus = false; }
            if ($("#cboMarcaAltaFiltro").val() == "" || $("#cboMarcaAltaFiltro").val() == null) { estatus = false; }
            if ($("#txtModeloAltaFiltro").val().trim() == "" || $("#txtModeloAltaFiltro").val() == null) { estatus = false; }
            if ($("#cboSinteticoAltaFiltro").val() == "" || $("#cboSinteticoAltaFiltro").val() == null) { estatus = false; }
            return estatus;
        }

        function GuardarFiltro() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/GuardarFiltro',
                data: { obj: getDatosFiltro() },
                success: function (response) {
                    $.unblockUI();
                    if (response.noExisteFiltro) {
                        modalAltaFiltro.modal('hide');
                        ConfirmacionGeneral("Guardado", "Se guardaron los datos con éxito");
                    }
                    else {
                        AlertaGeneral("Alerta", "Ya existe un registro con esa descripción");
                    }
                },
                error: function () {
                    $.unblockUI();
                    AlertaGeneral("Alerta", "La operación no pudo ser completada");
                }
            });
        }

        function getDatosFiltro() {
            return {
                id: 0,
                descripcion: $("#txtDescripcionAltaFiltro").val().trim(),
                marca: $("#cboMarcaAltaFiltro").val(),
                sintetico: $("#cboSinteticoAltaFiltro").val() == "1" ? true : false,
                modelo: $("#txtModeloAltaFiltro").val().trim(),
                estado: true
            }
        }

        function ReturnPm() {
            var pm;
            pm = $("#Pm_GestorActividades").html();
            if (pm == "PM1") {
                pm = 1;
            } else if (pm == "PM2") {
                pm = 2;
            } else if (pm == "PM3") {
                pm = 3;
            } else if (pm == "PM4") {
                pm = 4;
            }
            return pm;
        }

        function GuardarActividadesProyectadas(objActvProy) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/GuardarActividadesMantProy',
                data: { objActvProy: objActvProy },
                success: function (response) {
                    $.unblockUI();
                    ConsultaActividadesProg(response.mntto, response.idTipoPM, response.idMatn);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function ConsultaActividadesProg(modeloEquipoID, idPM, idmantenimiento) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                //async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarGestorPM',
                data: { modeloEquipoID: modeloEquipoID, idPM: idPM, idmantenimiento: idmantenimiento },
                success: function (response) {
                    $.unblockUI();
                    $("#grid_GestorActividadesEjecPM").bootgrid("clear");
                    $("#grid_GestorActividadesEjecPM").bootgrid("append", response.ObjActProy);
                },
                error: function () {
                    $.unblockUI();
                }
            });
            $("#grid_GestorActividades-footer .pagination li.page-" + paginaActual + " .button").click();
        }

        init();
    }
    $(document).ready(function () {
        MantenimientoPM.EjecucionPM = new EjecucionPM();
    });
});