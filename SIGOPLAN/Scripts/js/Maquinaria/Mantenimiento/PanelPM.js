$(function () {
    $.namespace('MantenimientoPM.panelpm');
    panelpm = function () {
        mensajes = {
            PROCESANDO: 'Procesando...'
        };
        _idProyectado = 0;
        lstDicEs = {
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
        };

        var d = new Date();
        var paginaActual;
        var paginaActualpm1;
        var paginaActualpm2;
        var paginaActualpm3;
        var paginaActualpm4;
        var paginaActualJG;
        var paginaActualEX;
        var ModProy;
        var idPlaneador = 0;
        var idResponsable = 0;

        btnppalTabComponentes = $("#btnppalTabComponentes"),
            btnppalTabLubricantes = $("#btnppalTabLubricantes"),
            btnppalTabFiltros = $("#btnppalTabFiltros"),

            ireport = $("#report");
        //bansera programado 13/17/18 
        var BanderaProg = false;
        //modalIndicacionesCOnt modal inidcacion cambio contadores 11/07/18
        modalIndicacionesCOnt = $("#modalIndicacionesCOnt");
        //pantalla alta servicio
        cboTipoServ = $("#cboFiltroTipo");
        btnUnidad = $("#btnUnidad");
        //grids
        Actividadid = 0;
        btnRealizado = $("#btnRealizado");
        grid_ComponenteVin = $("#grid_ComponenteVin");
        grid_MiselaneoAceite = $("#grid_MiselaneoAceite");
        grid_MisAnticongelante = $("#grid_MisAnticongelante");
        txtbusquedamisc = $("#txtbusquedamisc");
        txtbusquedaComp = $("#txtbusquedaComp");
        modalSeguimientopms = $("#modalSeguimientopms");
        grid_GestorActividades = $("#grid_GestorActividades");//grid de actividades paso dos

        //botn guardado lubs SErv 13/07/18
        btnguardarLubObs = $("#btnguardarLubObs");
        //validadores problemas paginado bootgrid o yo? quien sae.. XD
        BanderapagComp = true;
        BanderapagFilt = true;
        BanderapagAceite = true;
        BanderapagAnti = true;
        Banderagirdpm1 = true;
        Banderagirdpm2 = true;
        Banderagirdpm3 = true;
        Banderagirdpm4 = true;
        BanderagirdJG = true;
        BanderagirdEX = true;
        btnActividadGestor = $("#btnActividadGestor");

        btnRefresh = $("#btnRefresh");
        vPM1 = $("#vPM1");
        vPM2 = $("#vPM2");
        vPM3 = $("#vPM3");
        vPM4 = $("#vPM4");
        vJG = $("#vJG");
        vDN0 = $("#vDN0");
        vDN1 = $("#vDN1");
        vDN2 = $("#vDN2");
        nelson = $("#nelson");
        vExtras = $("#vExtras");
        vDN = $("#vDN");
        vIN = $("#vIN");
        upFormato = $(".upFormato");
        btnGuardarDoc = $("#btnGuardarDoc");
        btnEliminarDoc = $("#btnEliminarDoc");
        calendar = $("#calendar");
        btnServicio = $("#btnServicio");
        modalNuevoServicio = $("#modalNuevoServicio");
        btnguardarMant = $("#btnguardarMant");
        fechaIniCurso = $("#fechaIniCurso");
        fechaIniMante = $("#fechaIniMante");
        fechaServProx = $("#fechaServProx");
        cboFiltroTipo = $("#cboFiltroTipo");
        cboFiltroNoEconomico = ("#cboFiltroNoEconomico");
        cboTipoManteniento = $("#cboTipoManteniento");
        archivoPM = $("#archivoPM");
        lblArchivo = $("#lblArchivo");
        archivoPMDescargar = $("#archivoPMDescargar");

        //Declarar variables
        NoEconomico = $("#NoEconomico");
        txtDescripcion = $("#txtDescripcion");
        txtMarca = $("#txtMarca");
        txtModelo = $("#txtModelo");
        txtNoSerie = $("#txtNoSerie");
        txtAnio = $("#txtAnio");
        dateFechaCompra = $("#dateFechaCompra");
        txtPropietario = $("#txtPropietario");
        txtUbicacion = $("#txtUbicacion");
        txtHorometroInicio = $("#txtHorometroInicio");
        txtHorometroActual = $("#txtHorometroActual");//horometro servicio  ACtual
        txtUltimoParo = $("#txtUltimoParo");
        txtDesParo = $("#txtDesParo");
        txtUltMantenimiento = $("#txtUltMantenimiento");
        txtProxMantenimiento = $("#txtProxMantenimiento");
        txtCostAdquisicion = $("#txtCostAdquisicion");
        txtCostOverActivo = $("#txtCostOverActivo");
        txtCostOverAplicado = $("#txtCostOverAplicado");
        btnImprimir = $("#btnImprimir");
        cboFiltroTipo = $("#cboFiltroTipo");
        cboFiltroGrupo = $("#cboFiltroGrupo");
        txtCostoHora = $("#txtCostoHora");
        btnAplicarFiltros = $("#btnAplicarFiltros");
        txtHorometro = $("#txtHorometro");
        fechaIniCurso = $("#fechaIniCurso");
        txtObservacion = $("#txtObservacion");
        btntablaPM = $(".btntablaPM");
        modaltablaPM = $("#modaltablaPM");
        tablapmTitulo = $("#tablapmTitulo");
        idmaquina = $("#idmaquina");
        idmantenimiento = ("#idmantenimiento");
        tablaPM2 = $("#tablaPM2");
        tablaPM3 = $("#tablaPM3");
        tablaPM4 = $("#tablaPM4");
        horometroMaquina = $("#horometroMaquina");
        horometroMaquinaVal = $("#horometroMaquinaVal");
        legendFicha = $("#legendFicha");
        btnTodosPM1 = $("#btnTodosPM1");//seleccionar todas las actividades del om
        btnTodosPM2 = $("#btnTodosPM2");
        btnTodosPM3 = $("#btnTodosPM3");
        btnTodosPM4 = $("#btnTodosPM4");
        HorometroAplico = $("#HorometroAplico");
        errorAceptar = $("#errorAceptar");
        timepicker = $("#timepicker");
        HorometroProyectado = $("#HorometroProyectado");
        tbNombreEmpleado = $("#tbNombreEmpleado");
        tbNombreCoordinador = $("#tbNombreCoordinador");
        tbPuestoEmpleado = $("#tbPuestoEmpleado");
        tbPuestoCoordinador = $("#tbPuestoCoordinador");
        fechaFinMante = $("#fechaFinMante");
        timepicker_6 = $("#timepicker_6");
        AltaActividades = $("#AltaActividades");
        btnTodosPM = $(".btnTodosPM");
        modalActividad = $("#modalActividad");
        grid_Actividad = $("#grid_Actividad");
        btnBuscar_Actividad = $("#btnBuscar_Actividad");
        txtDescripcionActividad = $("#txtDescripcionActividad");
        modalAltaActividad = $("#modalAltaActividad");
        btnNueva_Actividad = ("#btnNueva_Actividad");
        txtModaldescripcion = $("#txtModaldescripcion");
        btnGuardarActividad = $("#btnGuardarActividad");
        btnModalCancelarActividad = $("btnModalCancelarActividad");
        txtModaldescripcion = $("#txtModaldescripcion");
        btndialogalertaGeneral = $("#btndialogalertaGeneral");
        modalAlerta = $(".modalAlerta");
        modalVinculacionPM = $("#modalVinculacionPM");
        modalComponentes = $("#modalComponentes");
        modalAltaComponte = $("#modalAltaComponte");
        AltaComponentes = $("#AltaComponentes");
        btnNuevo_Componente = ("$btnNuevo_Componente");
        txtAltaCompDesc = ("#txtAltaCompDesc");
        txtAltaCompMin = ("#txtAltaCompDesc");
        txtAltaCompMax = ("#txtAltaCompDesc");
        txtDescripcionComp = ("#txtDescripcionComp");
        DescripcionActMod = "";
        idTipoActividadMod = 0;
        grid_Part = $("#grid_Part");
        btnGuardarParte = $("btnGuardarParte");
        txtAltaCompMin = $("txtAltaCompMin");
        txtAltaCompDesc = $("txtAltaCompDesc");
        txtAltaCompMax = $("txtAltaCompMax");
        modalVinCompAct = $("#modalVinCompAct");
        VinCompAct = $("#VinCompAct");
        tablaPM1 = $("#tablaPM1");
        txtmodeloVinc = $("#txtmodeloVinc");
        grid_pm1 = $("#grid_pm1");
        grid_pm2 = $("#grid_pm2");
        grid_pm3 = $("#grid_pm3");
        grid_pm4 = $("#grid_pm4");
        grid_JG = $("#grid_JG");
        grid_EX = $("#grid_EX");
        grid_DN0 = $("#grid_DN0");
        grid_DN1 = $("#grid_DN1");
        grid_DN2 = $("#grid_DN2");
        grid_MiselaneoFiltro = $("#grid_MiselaneoFiltro");
        btnActividadVinc = $("#btnActividadVinc");
        ActividadVinc = $("#ActividadVinc ");
        idmodelo = $("#idmodelo");
        idActividadVinc = $("#idActividadVinc");
        cboTipoActividad = $("#cboTipoActividad");
        cboAltaTipoActividad = $("#cboAltaTipoActividad");
        var idActividadModificar = 0;
        var HorometroUltMaq = 0;
        ArraytblM_ActvContPM = []
        var objPm;
        var contadorCiclopm1 = 0;
        var contadorCiclopm2 = 0;
        var contadorCiclopm3 = 0;
        var contadorCiclopm4 = 0;
        var banderaSelecTodos = false;
        var horometroActual = 0;
        var trabajoPorDiaManual;
        var trabajoPorSemana;
        var bUnidad = $("#bUnidad");
        var event = new Object();
        var hoy = new Date();
        var dd = hoy.getDate();
        var mm = hoy.getMonth() + 1;
        var yyyy = hoy.getFullYear();
        var trabajoPorDiaAutomatico;
        var deshacerFecha;
        var FechaLimiteSuperior;
        var FechaLimiteInferior;
        var FechalimiteSuperiorHrs;
        var FechalimiteInferiorHrs;
        var FechaSeleccionadaHrs;
        var FechaSeleccionada;
        var DeshabilitadoMes = false;

        let tblGridLubricantesAlta = $("#tblGridLubricantesAlta");

        /*MATUS*/
        btnPlanificacion = $("#btnPlanificacion");
        tblGridLubProx = $("#tblGridLubProx");
        cboObras = $("#cboObras");
        cboObras.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);

        selectReporte = $('#selectReporte');
        btnVerReporte = $("#btnVerReporte");

        const botonJOMAGALI = $('#botonJOMAGALI');

        modalProgramacionSemanal = $("#modalProgramacionSemanal");
        cboAreaCuentarpt = $("#cboAreaCuentarpt");
        btnSaveProgramar = $("#btnSaveProgramar");
        btnSaveFinalizar = $("#btnSaveFinalizar");
        dateIni = $("#dateIni"),
            dateFin = $("#dateFin");
        cboEconomicorpt = $("#cboEconomicorpt");
        cboAreaCuentarpt.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
        cboEconomicorpt.fillCombo('/Vehiculo/fillCboEconomico', null, false);
        cboFiltroGrupoVinc = $("#cboFiltroGrupoVinc"),
            cboFiltroModeloVinc = $("#cboFiltroModeloVinc");

        cboAreaCuentarpt.change(function (e) {
            cboEconomicorpt.fillCombo('/Vehiculo/fillCboEconomico', { cc: cboAreaCuentarpt.val() }, false);
        });
        /**/

        archivoPMDescargar.click(function (e) {
            descargarArchivo($("#MantenimientoSeguimiento").attr('idmant'));

        })

        $("#modalMiselaneos").on("shown.bs.modal", function () {
            tipoPM = $(".GenericaPM ul.nav").find('li.active a').text();
            if (tipoPM == "") {
                tipoPM = $(".GenericaPM ul.nav").find("li.active").children("div.btn-group").children(".active").attr("id-data");
            }
            if (tipoPM == "PM1") {
                //   $("#cboModalidad option[value='2']").hide();
                //   $("#cboModalidad option[value='3']").hide();
                //   $("#cboModalidad option[value='1']").show();
            } else if (tipoPM == "PM3") {
                //   $("#cboModalidad option[value='1']").show();
                //    $("#cboModalidad option[value='2']").hide();
                //   $("#cboModalidad option[value='3']").hide();
            } else if (tipoPM == "PM4") {
                //     $("#cboModalidad option[value='1']").show();
                //    $("#cboModalidad option[value='3']").hide();
            } else if (tipoPM == "Lubricantes" && Actividadid != 20) {
                //     $("#cboModalidad option[value='1']").hide();
                //     $("#cboModalidad option[value='2']").show();
                //    $("#cboModalidad option[value='3']").show();
            } else if (tipoPM == "Lubricantes" && Actividadid == 20) {
                //    $("#cboModalidad option[value='1']").hide();
                //    $("#cboModalidad option[value='2']").hide();
                //   $("#cboModalidad option[value='3']").show();
            }
            $("#TextModalViscosidad").attr("hidden", false);



            var tabla = $(".GenericaPM ul.nav").find('li.active a').text();
            if (tabla == "") {
                tabla = $(".GenericaPM ul.nav").find("li.active").children("div.btn-group").children(".active").attr("id-data");
            }
            if (tabla == "PM1" || tabla == "PM2" || tabla == "PM3" || tabla == "PM4") {
                // $("#cboModalidad option[value='2']").hide();
                //       $("#cboModalidad option[value='3']").hide();
                //      $("#cboModalidad option[value='1']").show();
                $("#cboModalidad").val(1).change();
            } else if (tabla == "Lubricantes" && Actividadid != 20) {
                //      $("#cboModalidad option[value='3']").hide();
                $("#cboModalidad").val(2).change();
                //      $("#cboModalidad option[value='2']").show();
            } else if (Actividadid == 20) {
                //      $("#cboModalidad option[value='2']").hide();
                $("#cboModalidad").val(3).change();
                //       $("#cboModalidad option[value='3']").show();
            }
        });

        function fnChangeCboFiltroModeloVinc() {

            vPM1.click();
            $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 1 }, '/MatenimientoPM/getCatActividad');//autocompletado
            $("#ActividadVinc").attr("disabled", false);
            if ($(this).val() != undefined) {
                BuscarInfoModelo($(this).val(), 1);
                $("#cboComponentes_Lubricantes").fillCombo('/matenimientopm/FillCboCatComponentesByModelo', { modeloID: $(this).val() });
            } else {
                LimpiartablasVinc();
            }

        }


        $('#btnstep2').click(function (e) {
            // $("#tblGridLubricantesAlta").columns.adjust().draw();
            // setTimeout(OprimirBotonAdjust, 50)
        });
        function OprimirBotonAdjust() {
            // $("#tblGridLubricantesAlta").columns.adjust().draw();
        }

        $("#btnstep3").click(function (e) {
            ConsultarActividadesExtras($("#cboFiltroNoEconomico").data().idModelo);
        });

        function fnChangeCboGrupoVinc() {
            $("#cboFiltroModeloVinc").fillCombo('/MatenimientoPM/FillCboModelo_Maquina', { idTipo: $("#cboFiltroGrupoVinc").val() });
            $("#cboFiltroModeloVinc").attr('disabled', false);
        }

        //MODAL ALTA DE MAQUINARIA
        cboFiltroGrupo.change(function (e) {

            FillCboNoEconomico();
        });
        $("#btnServicio").click(function () {
            modaltablaPM.modal("show");
        }); //Evento que despliega el modal para el alta del servicio.

        modaltablaPM.on("shown.bs.modal", function () {
            LimpiarFormularios();
            cboFiltroGrupo.attr('disabled', true);
            $("#txtNoEconomicoAlt").attr("disabled", true);
            $("#horometroMaquina").attr("disabled", true);
            $("#HorometroAplico").attr("disabled", true);
            $("#tbNombreEmpleado").attr("disabled", true);
            $("#tbPuestoEmpleado").attr("disabled", true);
            $("#ObservacionesMant").attr("disabled", true);
            $("#HorometroProyectado").attr("disabled", true);
            $("#fechaFinMante").attr("disabled", true);
            $("#fechaUltCal").attr("disabled", true);
            $("#MantenimientoProy").attr("disabled", true);
            $("#cboTipoMantenientoContador").attr("disabled", true);
            $("#fechaIniMante").attr("disabled", true);
            FillCboGrupo();

            $(cboFiltroNoEconomico).attr('disabled', true);
            var hoy = new Date();
            var dd = hoy.getDate();
            var mm = hoy.getMonth() + 1;
            var yyyy = hoy.getFullYear();
            $("#fechaIniMante").datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);
            $("#timepicker").timepicker();
            $("#btnstep1").click();
        });

        modaltablaPM.on('hide.bs.modal', function (e) {
            //$('#calendar').fullCalendar('removeEvents');
            //ConsultaPm(0);
            $.unblockUI();
        });

        btnguardarMant.click(function (e) {
            fnAltaEquipos();
        });



        function fnAltaEquipos() {
            if ($("#cboFiltroNoEconomico  option:selected").text() == "--Seleccione--" || $("#cboFiltroNoEconomico  option:selected").text() == "") {
                AlertaModal("PASO 1", "Indicar el No.Económico");
            } else if ($("#cboTipoMantenientoContador").val() == "") {
                AlertaModal("PASO 1", "Indicar el último tipo de Mantenimiento");
            } else if ($("#HorometroAplico").val() == "") {
                AlertaModal("PASO 1", "Indicar el último Horómetro del último Mantenimiento");
            } else if ($("#tbNombreCoordinador").val() == "") {
                AlertaModal("PASO 1", "Indicar el nombre del Coordinador del Servicio");
            } else if ($("#tbNombreEmpleado").val() == "") {
                AlertaModal("PASO 1", "Indicar el nombre del Responsable del Servicio");
                //paso dos validacio
            } else {
                //validacion paso 4
                var BanderaPaso4 = false;
                var BanderaPasoP4 = false;
                var longitud = $("#gridPaso4 >tbody >tr").length
                for (var i = 0; i <= (longitud - 1); i++) {
                    renglon = $("#gridPaso4 >tbody >tr")[i]
                    Hrsaplico = $(renglon).closest('tr').find('td:eq(1) .HorServ').val();
                    Perioricidad = $(renglon).closest('tr').find('td:eq(2) .perioricidad').val();
                    if (Hrsaplico == "") {
                        BanderaPaso4 = true;
                    } else if (Perioricidad == 0) {
                        BanderaPasoP4 = true;
                    }
                    if (BanderaPaso4 == true) {
                        AlertaModal("PASO 4", "Indicar Horas Actividades Extra en que se realizó el Servicio     '" + $(renglon).closest('tr').find('td:eq(0)').html() + "'");
                    } else if (BanderaPasoP4 == true) {
                        AlertaModal("PASO 4", "Vincular Periodicidad de la Actividad en Módulo de Agrupamiento  '" + $(renglon).closest('tr').find('td:eq(0)').html() + "'");
                    }
                }

                //validacion paso 3
                var BanderaPaso3 = false;
                var BanderaPasoP3 = false;
                var longitud = $("#gridPaso3 >tbody >tr").length
                for (var i = 0; i <= (longitud - 1); i++) {
                    renglon = $("#gridPaso3 >tbody >tr")[i]
                    Hrsaplico = $(renglon).closest('tr').find('td:eq(1) .HorServ').val();
                    Perioricidad = $(renglon).closest('tr').find('td:eq(2) .perioricidad').val();
                    if (Hrsaplico == "") {
                        BanderaPaso3 = true;
                    } else if (Perioricidad == 0) {
                        BanderaPasoP3 = true;
                    }
                    if (BanderaPaso3 == true) {
                        AlertaModal("PASO 3", "Indicar Horas Actividades Extra en que se realizó el Servicio     '" + $(renglon).closest('tr').find('td:eq(0)').html() + "'");
                    } else if (BanderaPasoP3 == true) {
                        AlertaModal("PASO 3", "Vincular Periodicidad de la Actividad en Módulo de Agrupamiento  '" + $(renglon).closest('tr').find('td:eq(0)').html() + "'");
                    }
                }
                //validacion paso 2
                var BanderaPaso2 = false;
                array = [];
                for (var i = 0; i < tblGridLubricantesAlta.data().length; i++) {
                    obj = {};
                    obj = tblGridLubricantesAlta.data()[i];
                    if (tblGridLubricantesAlta.data()[i].horServ == 0) {

                        BanderaPaso2 = true;
                        AlertaGeneral('Alerta', 'Falta hora de servicio del componente ' + tblGridLubricantesAlta.data()[i].componenteDesc);
                        break;

                    }
                    else {
                        array.push(obj);
                    }
                }

                if (BanderaPaso2 == false && BanderaPaso3 == false && BanderaPasoP3 == false && BanderaPaso4 == false && BanderaPasoP4 == false) {
                    EconomicoID = $("#txtNoEconomicoAlt").val();
                    TipoMatenimiento = $("#cboTipoMantenientoContador option:selected").val();
                    observaciones = $("#ObservacionesMant").val();
                    idMaquina = $("#cboFiltroNoEconomico option:selected").val();
                    TipoMantenimientoProyId = (parseInt(TipoMatenimiento));
                    objtblM_MatenimientoPm = {
                        id: 0, economicoID: EconomicoID, horometroUltCapturado: horometroMaquina.val(), fechaUltCapturado: $("#fechaUltCal").val(), tipoPM: TipoMatenimiento,
                        fechaPM: fechaIniMante.val(), horometroPM: HorometroAplico.val(), personalRealizo: $("#tbNombreEmpleado").attr("data-numempleado"), observaciones: observaciones,
                        horometroProy: $("#HorometroProyectado").val(), fechaProy: $("#fechaFinMante").val(), tipoMantenimientoProy: TipoMantenimientoProyId, idMaquina: idMaquina, planeador: $("#tbNombreCoordinador").attr("data-numempleado")
                    }
                    GuardadoJG(array);//Control de suministros
                    GuardadoAE();//control de actividades extras
                    GuardadoDN();//control de actividades DN's
                    GuardarMatenimientoObj();//guardado pendiente en valiacion 27/06/18 1246pm;
                    initTabla();
                    $(this).attr("disabled", false);
                }
            }
        }
        //MODAL ALTA DE MAQUINARIA FIN

        function init() {
            ///INICIALIZACION DE COMBOS FIJOS.
            $("#cboTipoServ").fillCombo('/MatenimientoPM/FillCboTipo_Maquina', { estatus: true });
            $("#cboTipoServ").val("1");
            //$("#cboTipoServ").attr('disabled', true);
            //////

            $("#cboTipoServ").change(function () {
                $("#cboFiltroGrupo").fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: $("#cboTipoServ").val() });
            });
            cboFiltroModeloVinc.change(fnChangeCboFiltroModeloVinc);
            cboFiltroGrupoVinc.change(fnChangeCboGrupoVinc);

            $("#cboModalidad").change(cargarDataComboModal);

            $("#tbNombreCoordinadorS").getAutocomplete(SelectCoordinador, null, '/MatenimientoPM/getEmpleados');
            $("#tbNombreCoordinador").getAutocomplete(SelectCoordinador, null, '/MatenimientoPM/getEmpleados');
            $("#tbNombreEmpleadoS").getAutocomplete(SelectEmpleado, null, '/MatenimientoPM/getEmpleados');

            $(".divTablaReporteSemanal").hide();
            btnVerReporte.hide(500);
            botonJOMAGALI.hide(500);
            initTablaReporteSemanal();
            btnVerReporte.click(verReporte);
            botonJOMAGALI.click(descargarJOMAGALI);
            btnPlanificacion.click(verModalPlaneacion);
            btnSaveFinalizar.click(saveAll);
            btnSaveProgramar.click(saveParcialPrograma);

            cboObras.change(reloadCalendarByObra);

            initGridtablaPM(grid_pm1);
            initGridMisFiltro(grid_MiselaneoFiltro);
            initGridMisAceite(grid_MiselaneoAceite);
            initGridCompoVin(grid_ComponenteVin);
            initGridtablaPM2(grid_pm2);
            initGridtablaPM3(grid_pm3);
            initGridtablaPM4(grid_pm4);
            initGridtablaJG(grid_JG);
            initGridtablaExt(grid_EX);
            initGridtablaDN0(grid_DN0);
            initGridtablaDN1(grid_DN1);
            initGridtablaDN2(grid_DN2);
            initGridCompoActPM($("#grid_ComponenteActGestor"));//ragulilar 070818 vinculacion
            initGridDNProx($("#gridDNProx"));
            initGridMisAnticongelante(grid_MisAnticongelante);
            initGridLubProy($("#gridLubProx"));
            $("tr a").css("padding", "0px");
            initGrid(grid_Actividad);
            initGridtablaPMGestor(grid_GestorActividades);//modulo segumiento pm 19/07/18
            initGridActProy($("#gridActProx"));
            //initGridgridGestorFormatos($("#grid_GestorFormatos"));//ragulilar 070818 gestor forrmatos
            txtDescripcionActividad.getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 0 }, '/MatenimientoPM/getCatActividad');//autocompletado alta actividad
            $("#txtActividadGestor").getAutocomplete(SelectActividad, { term: $("#txtActividadGestor").val(), idTipo: 0 }, '/MatenimientoPM/getCatActividad');//actividades pm
            initTabla();
            tbNombreEmpleado.getAutocomplete(SelectEmpleado, null, '/MatenimientoPM/getEmpleados');
            botnAgregar = "<button style='margin-bottom:35px' type='button' class='btn btn-success pull-right' id='btnServicio'>" +
                "<span class='glyphicon glyphicon-dashboard pull-left'></span>" + "Agregar Nuevo Servicio" +
                "</button>";
            btnTodosPM1.click(preSeleccionado);
            btnTodosPM2.click(preSeleccionado);
            btnTodosPM3.click(preSeleccionado);
            btnTodosPM4.click(preSeleccionado);
            btnGuardarDoc.click(SubirDocManejo);//subir Documento Modulo
            btnEliminarDoc.click(EliminarVincDoc);//subir Documento Modulo
            //$("#btnReporteMant").click(ReporteDeServicio);
            ConsultaPm(0);

            $("#ComponentesModalVin").on("shown.bs.modal", function () {

                grid_ComponenteVin.bootgrid("clear");
                ruta = '/MatenimientoPM/FillGridComponenteVin';
                loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                $("#TextModalVin").attr("hidden", false);
                if (Actividadid == 20) {
                    $("#txtbusquedaComp").attr("disabled", true);
                    $("#grid_ComponenteVin").bootgrid("search", "ANTICONGELANTE");
                } else {
                    $("#txtbusquedaComp").attr("disabled", false);
                    $("#grid_ComponenteVin").bootgrid("search", "");
                }
            });

            $("#modalObsActs").on("shown.bs.modal", function () {
                ruta = '/MatenimientoPM/FillGridComponenteRestrinccion';//restrinccion
                loadGrid(getFiltrosACtObject(), ruta, $("#grid_ComponenteActGestor"));
            });

            function getFiltrosACtObject() {
                idpm = $("#Pm_GestorActividades").attr("idpm");
                idTipoAct = 1;
                ConsultaModelobyMantenimiento($("#MantenimientoSeguimiento").attr("idmant"));
                //idAct
                return {
                    Id: 0,
                    modeloEquipoID: ModProy,
                    idActs: $(".txtActividadAltS").attr("idact"),
                    idTipoAct: idTipoAct,
                    idpm: idpm
                }
            }
            $("#ComponentesModalVin").on('hide.bs.modal', function (e) {
                $("#TextModalVin").html("");
                $("#txtbusquedaComp").val("");
                $("#grid_ComponenteVin").bootgrid("search", "");
                Actividadid = 0;
            });
            $("#modalMiselaneos").on("show.bs.modal", function () {
                grid_MiselaneoAceite.bootgrid("clear");
                grid_MiselaneoFiltro.bootgrid("clear");
                $("#grid_MisAnticongelante").bootgrid("clear");
                $("#cboModalidad").val(null).change();
                $("#cboModalidad").val(0).change();

            });
            $("#modalMiselaneos").on("hide.bs.modal", function () {
                $("#txtbusquedamisc").val("")
                $("#grid_MisAnticongelante").bootgrid("search", "");
                $("#grid_MiselaneoAceite").bootgrid("search", "");
                $("#grid_MiselaneoFiltro").bootgrid("search", "");
            });
            $("#cboFiltroNoEconomico").on('change', function () {
                // Store the current value on focus and on change
                LimpiarFormularios();
                relacionarcboTexto();
            });

            fechaIniMante.datepicker({
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 2000000000);
                    }, 0);
                }
            });

            modalVinCompAct.on('hide.bs.modal', function (e) {
                $("#cboFiltroModeloVinc ").val("").change();//restablece el filtro
            });

            $("#cboFiltroNoEconomico").change(function () {
                fncGetModeloEconomico($(this).val());
            });

            $("#modalObsActs").on('hide.bs.modal', function (e) {
                ////$.unblockUI();
                $("#ObservacionesMantAct").val("");
            });
            fechaIniCurso.datepicker().datepicker("setDate", dd + "/" + mm + "/" + yyyy);
            $(document).on('change', '#fechaIniMante', function () {
                banderaUltimoHorometro = true;
                ConsultarUltimoHorometro($("#fechaIniMante").val(), $("#cboFiltroNoEconomico").find('option:selected').text(), banderaUltimoHorometro);
                banderaUltimoHorometro = false;
            });
            $(document).on('click', "#btnModalAceptarEliminar", function () {
                idActividad = $(this).attr("data-index");
                ELiminarVinculacion(idActividad);
            });
            $("#modalIndicacionesCOnt").on("shown.bs.modal", function () {
                idMantProy = $("#btnguardarLubObs").attr("idobjproy");
                if (idMantProy == undefined) {
                    idMantProy = 0;
                }
                ////$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/CargaLubObs',
                    data: { idMantProy: idMantProy },
                    success: function (response) {
                        ////$.unblockUI();
                        if (response.success == true && response.ObjBitProyLub.length > 0) {
                            if (response.ObjBitProyLub[0].programado == true) {
                                $("#horomatroNoProg").val(response.ObjBitProyLub[0].Hrsaplico);
                                $(".horometroServTent").val($("#HorometroProyectadoS").val());
                                $("#ObservacionesMantLub").val(response.ObjBitProyLub[0].Observaciones);
                                $(".fechaServProxLub").val(FormateoFechaEdicion(response.ObjBitProyLub[0].FechaServicio));
                                $("#horomatroNoProg").val("N/D");
                                $("#cboProgramado").val(1);
                            } else {
                                $("#horomatroNoProg").val(response.ObjBitProyLub[0].Hrsaplico);
                                $("#horometroServTent").val($("#HorometroProyectadoS").val());
                                $("#ObservacionesMantLub").val(response.ObjBitProyLub[0].Observaciones);
                                $(".fechaServProxLub").val(FormateoFechaEdicion(response.ObjBitProyLub[0].FechaServicio));
                                $("#cboProgramado").val(2);
                            }
                        } else {
                            var myString = $("#fechaFinManteS").val();
                            var arr = myString.split('/');
                            $(".fechaServProxLub").css('z-index', 2000000000);
                            $(".fechaServProxLub").datepicker().datepicker("setDate", arr[0] + "/" + arr[1] + "/" + arr[2]);
                            $("#horometroServTent").val($("#HorometroProyectadoS").val());
                        }
                    },
                    error: function () {
                        ////$.unblockUI();
                    }
                });

            });
            $(document).on('change', "#cboProgramado", function () {
                var ProgramadoChk = $("#cboProgramado option:selected").val();
                if (ProgramadoChk == 2) {//no
                    $("#horomatroNoProg").attr("disabled", false);
                    $("#fechaServProxLub").attr("disabled", false);
                    $("#fechaServProxLub").datepicker("option", "maxDate", "+0m +0w");
                    $("#horomatroNoProg").val("");
                    BanderaProg = false;
                } else {
                    $("#horomatroNoProg").attr("disabled", true);
                    $("#fechaServProxLub").attr("disabled", true);
                    $("#horomatroNoProg").val("N/D");
                    BanderaProg = true;
                };
            });

            $(document).on('click', "#btnProgramServ", function () {
                ProgramaActividades();
            });

            $(document).on('change', '#cboTipoMantenientoContador', function () {
                tipoPm = $("#cboTipoMantenientoContador").val();
                if (tipoPm == 1) {
                    SeleccionarTodos(true, "#tablaPM1");
                    SeleccionarTodos(false, "#tablaPM2");
                    SeleccionarTodos(false, "#tablaPM3");
                    SeleccionarTodos(false, "#tablaPM4");
                    contadorCiclopm1 = 1;
                    contadorCiclopm2 = 0;
                    contadorCiclopm3 = 0;
                    contadorCiclopm4 = 0;
                } else if (tipoPm == 2) {
                    SeleccionarTodos(true, "#tablaPM1");
                    SeleccionarTodos(true, "#tablaPM2");
                    SeleccionarTodos(false, "#tablaPM3");
                    SeleccionarTodos(false, "#tablaPM4");
                    contadorCiclopm1 = 1;
                    contadorCiclopm2 = 1;
                    contadorCiclopm3 = 0;
                    contadorCiclopm4 = 0;
                } else if (tipoPm == 3) {
                    SeleccionarTodos(true, "#tablaPM1");
                    SeleccionarTodos(true, "#tablaPM2");
                    SeleccionarTodos(true, "#tablaPM3");
                    SeleccionarTodos(false, "#tablaPM4");
                    contadorCiclopm1 = 1;
                    contadorCiclopm2 = 1;
                    contadorCiclopm3 = 1;
                    contadorCiclopm4 = 0;
                } else if (tipoPm == 4) {
                    SeleccionarTodos(true, "#tablaPM1");
                    SeleccionarTodos(true, "#tablaPM2");
                    SeleccionarTodos(true, "#tablaPM3");
                    SeleccionarTodos(true, "#tablaPM4");
                    contadorCiclopm1 = 1;
                    contadorCiclopm2 = 1;
                    contadorCiclopm2 = 1;
                    contadorCiclopm3 = 1;
                }

                ValidacionCalculosHorometro();
            });
            $(document).on('change', '#Parte Select', function () {
                ValorMaxVida = $(this).val();
                vidahrsComponente = $(this.parentNode.parentNode).find('.vidahrsComponente input');
                vidahrsComponente.val(ValorMaxVida);
                $(".Contador").keypress();
            });
            $(document).on('keypress', '.Contador', function () {
                ValorMaxVida = $(this.parentNode).find('#Parte Select').val()//valor a conseiderar vida del compinente
                Contador = $(this).find('Input').val();//cuando se realizo el servicio del componente
                HorasTrabajadas = $(this.parentNode).find('.HorasTrabajadas input').val((parseFloat(horometroMaquina.val()).toFixed(2)) - (parseFloat(Contador).toFixed(2)))//horas trabajadas al horometro acutal maquina
                vidahrsComponente = $(this.parentNode).find('.vidahrsComponente input').val()
                vidaUtil = $(this.parentNode).find('.vidaUtil input').val(vidahrsComponente - HorasTrabajadas.val())
                indice = $(this.parentNode).find('#indice Label').html();
                //calculoEstatus($("#elemento" + indice + "").val(), $("#horasTrabajadas" + indice + "").val(), indice);
                estatus = $(this.parentNode).find('.estatus').empty();
                calculoEstatus(vidahrsComponente, HorasTrabajadas.val(), indice - 1, vidaUtil.val());//icono calculo
            });
            $(document).on('focusout', '.Contador', function () {
                ValorMaxVida = $(this.parentNode).find('#Parte Select').val()//valor a conseiderar vida del compinente
                Contador = $(this).find('Input').val();//cuando se realizo el servicio del componente
                HorasTrabajadas = $(this.parentNode).find('.HorasTrabajadas input').val((parseFloat(horometroMaquina.val()).toFixed(2)) - (parseFloat(Contador).toFixed(2)))//horas trabajadas al horometro acutal maquina
                vidahrsComponente = $(this.parentNode).find('.vidahrsComponente input').val()
                vidaUtil = $(this.parentNode).find('.vidaUtil input').val(vidahrsComponente - HorasTrabajadas.val())
                indice = $(this.parentNode).find('#indice Label').html();
                //calculoEstatus($("#elemento" + indice + "").val(), $("#horasTrabajadas" + indice + "").val(), indice);
                estatus = $(this.parentNode).find('.estatus').empty();
                calculoEstatus(vidahrsComponente, HorasTrabajadas.val(), indice - 1, vidaUtil.val());//icono calculo
            });
            $(document).on('focusout', '#HorometroAplico', function () {
                ValidacionCalculosHorometro();
                $(".txtHorometroUltLub").val($("#HorometroAplico").val());
            });
            $(document).on('click', '.fc-listYear-button', function () {
                $(".fc-scroller").removeAttr("style");
            });
            $('#HorometroAplico').bind("enterKey", function (e) {

            });
            $(document).on('click', '#errorAceptar', function () {
                //event.preventDefault()
            });
            $(document).on('click', '#btnCancelarFecha', function () {
                revertFunc();
            });



            //Modificar Nuevo

            btnActividadVinc.click(addNewActividad);

            ///
            $(document).on('click', '.tabs li', function () {
                var tabla = $(this).children("a").attr("href");
                var tablapm1 = $(".tab-content .panel #tablapm1");
                var tablapm2 = $(".tab-content .panel #tablapm2");
                var tablapm3 = $(".tab-content .panel #tablapm3");
                var tablapm4 = $(".tab-content .panel #tablapm4");
                if (tabla == undefined) {
                    tabla = $(this).children(".btn-group").children(".btn-default.active").attr("href");
                }
                if (tabla == "#tablapm1") {
                    tablapm1.removeClass("hide");
                    tablapm2.addClass("hide");
                    tablapm3.addClass("hide");
                    tablapm4.addClass("hide");
                } else if (tabla == "#tablapm2") {
                    tablapm1.addClass("hide");
                    tablapm2.removeClass("hide");
                    tablapm3.addClass("hide");
                    tablapm4.addClass("hide");
                } else if (tabla == "#tablapm3") {
                    tablapm1.addClass("hide");
                    tablapm2.addClass("hide");
                    tablapm3.removeClass("hide");
                    tablapm4.addClass("hide");
                } else if (tabla == "#tablapm4") {
                    tablapm1.addClass("hide");
                    tablapm2.addClass("hide");
                    tablapm3.addClass("hide");
                    tablapm4.removeClass("hide");
                } else {

                }
            });

            $("#btnActividadGestor").click(function () {
                IdActividadProy = $("#idActividadVinc").text();
                ObjAct = { idAct: IdActividadProy, idMant: $("#MantenimientoSeguimiento").attr('idmant'), Observaciones: "", aplicar: true, estatus: true, idPm: 0 };
                ////$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/GuardarActividadProy',
                    data: { ObjAct: ObjAct },
                    success: function (response) {
                        ////$.unblockUI();
                        pm = ReturnPm();
                        ConsultaActividadesProg(getModelo($("#MantenimientoSeguimiento").attr('idmant')), pm, $("#MantenimientoSeguimiento").attr('idmant'));

                    },
                    error: function () {
                        ////$.unblockUI();
                    }
                });
            });
            $("#btnguardarLubObs").click(function () {
                saveProyectados($(this).attr('idobjProy'));
            });
            $("#btnguardarAEObs").click(function () {
                id = $("#btnguardarAEObs").attr("idobj");
                ObjBitProyAE = {
                    id: id,
                    Hrsaplico: 0,
                    idAct: $("#ActividadServTent").attr("idAct"),
                    Vigencia: 0,
                    programado: true,
                    idMant: $("#MantenimientoSeguimiento").attr('idmant'),
                    Observaciones: $("#ObservacionesMantAct").val(),
                    FechaServicio: "",
                    estatus: true
                };
                //$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/GuardarBitProyAE2',
                    data: { ObjBitProyAE: ObjBitProyAE },
                    success: function (response) {
                        //$.unblockUI();
                        if (response.success == true) {
                            $("#modalActividadesCOnt").modal("hide");
                            ConfirmacionGeneralFC("Confirmación", "Info. Guardada", "bg-green");
                        }
                        CargaDeAEProyectado();
                        ProgressBarActProx();
                    },
                    error: function () {
                        //$.unblockUI();
                    }
                });
                //CargaDeAEProyectado();
                //ProgressBarActProx();
            });
            $("#btnguardarDNObs").click(function () {
                GuardadoMetOBsDn(true, $(this).attr("idobj"), 0);
            });
            $(document).on('click', '#btnPaso4', function () {

                btnSaveProgramar.addClass('hide');
                btnSaveFinalizar.removeClass('hide');

                TraerFormatos();
            });

            function setButton() {
                btnSaveProgramar.removeClass('hide');
                btnSaveFinalizar.addClass('hide');
            }
            $(document).on('click', '#btnPaso1', function () {
                setButton();
            });

            $(document).on('click', '#btnPaso2', function () {
                setButton();
                if (tblGridLubProxTbl != undefined) tblGridLubProxTbl.columns.adjust();
            });

            $(document).on('click', '#btnPaso3', function () {
                setButton();

            });



            $(document).on('click', '#btnPaso5', function () {
                if (gridFiltrosComponentes != undefined) gridFiltrosComponentes.columns.adjust();
            });

            $(document).on('click', '.btntablaPM', function () {
                horometroMaquina.val(txtHorometroActual.val());//pasar el horometro actual
                modaltablaPM.modal("show")
                $("#tablapmtitulo").val($("#cboFiltroNoEconomico option:selected").html())
                idmaquina.append($("#cboFiltroNoEconomico option:selected").val());
                $("#cboTipoMantenientoContador").fillCombo('/MatenimientoPM/FillCombotablaPM', { estatus: true });//carga ajax combo
                //cargar la ultima fecha correspondiente al ultimo horometro
                ConsultarFechaUltimoHorometro(txtHorometroActual.val(), $("#cboFiltroNoEconomico option:selected").html())
                //cargar ultimo horometro 
                var hoy = new Date();
                var dd = hoy.getDate();
                var mm = hoy.getMonth() + 1;
                var yyyy = hoy.getFullYear();
                var FechaHoy = (yyyy + "-" + mm + "-" + dd);
                ConsultarUltimoHorometro(FechaHoy, $("#tablapmtitulo").val());
            });
            $(document).on('click', '#AltaActividades', function () {
                modalActividad.modal("show");
            });
            $(document).on('click', '#VinculacionMaquinaria', function () {

            }); $(document).on('click', '#btnBuscar_Actividad', function () {
                BuscarActividad($("#txtDescripcionActividad").val(), $("#cboTipoActividad").val(), 0);
            });
            $(document).on('click', '#btnNueva_Actividad', function () {
                modalAltaActividad.modal("show");
                txtModaldescripcion.getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 0 }, '/MatenimientoPM/getCatActividad');//autocompletado
            });
            modalActividad.on("shown.bs.modal", function () {
                $("#cboTipoActividad").fillCombo('/MatenimientoPM/getTipoActividad', { estatus: true });//carga ajax combo
            });
            $("#btnguardarActObs").click(function () {
                pm = ReturnPm();
                objActvProy = { id: $("#btnguardarActObs").attr("idObj"), idAct: $("#ActividadServTent").attr("idact"), idMant: $("#MantenimientoSeguimiento").attr('idmant'), Observaciones: $("#ObservacionesMantAct").val(), aplicar: true, estatus: true, idPm: pm };
                GuardarActividadesProyectadas(objActvProy);
                ConsultaActividadesProg(ReturnModelo(), pm, $("#MantenimientoSeguimiento").attr('idmant'));
                $("#modalObsActs").modal("hide");
            });
            //modalSeguimientopms.on("shown.bs.modal", function () {
            //    idmant = $("#MantenimientoSeguimiento").attr("idmant");
            //    //$("#btnReporteMant").attr("idmant", idmant);

            //    $("#Pm_GestorActividades").html("");
            //    var cadena = $("#MantenimientoProyS").val();
            //    var idPM = 0;
            //    var modeloEquipoID = 0;
            //    inicio = 0,
            //        fin = 3,
            //        subCadena = cadena.substring(inicio, fin);
            //    $("#Pm_GestorActividades").append(subCadena);
            //    if (subCadena == "PM1") {
            //        idPM = 1;
            //    } else if ("PM2") {
            //        idPM = 2;
            //    } else if ("PM3") {
            //        idPM = 3;
            //    } else if ("PM4") {
            //        idPM = 4;
            //    }
            //    $("#Pm_GestorActividades").attr("idpm", idPM);
            //    modeloEquipoID = getModelo(idmantenimiento);///ReturnModelo();

            //    ConsultaActividadesProg(modeloEquipoID, idPM, idmantenimiento);
            //    //$("#btnPaso1").click();
            //    //27/07/18 09:92 PROGRESS BARR LUBRICANTES
            //    ProgressBarLub();
            //    //27/07/18 09:92 PROGRESS BARR ACTIVIDADES EXTRAS
            //    ProgressBarActProx();
            //    //02/08/18 carga dc

            //    CargaDeProyectado();
            //    CargaDeAEProyectado();
            //    CargaDeDNProyectado();
            //    ProgressBarDN();
            //    CargaDeFiltros(modeloEquipoID, idmantenimiento);

            //});

            function TraerFormatos() {
                idpm = $("#Pm_GestorActividades").attr("idpm");
                idTipoAct = 1;
                idmant = $("#MantenimientoSeguimiento").attr("idmant");
                //$("#btnReporteMant").attr("idmant", idmant);
                //$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/getFormato',
                    data: { id: idmant, idpm: idpm },
                    success: function (response) {
                        if (response.success == true && response.Actividad != "") {

                            getFormatosActividades(response.obj);

                            //$.unblockUI();
                        } else {
                            LimpiarFormularios();
                            AlertaModal("Error", "No existe Informacion de Actividades Extras Relacionados al Modelo   '" + modeloEquipoID + "'");
                            //modaltablaPM.modal("hide");
                        }
                    },
                    error: function () {
                        //$.unblockUI();
                    }
                });
            }


            $(document).on('click', '#btnModalCancelarActividad', function () {
                txtModaldescripcion.val("");
                BuscarActividad("", 0, 0);
            });

            $(document).on('click', '#btnGuardarActividad', function () {
                if (txtModaldescripcion.val() == "") {
                    AlertaModal("Error", "Indicar la Descripcion de la actividad");
                } else if (cboAltaTipoActividad.val() == "") {
                    AlertaModal("Error", "Indicar el tipo de Actividad a la que pertenece");
                } else {
                    idActividad = 0;
                    if (idActividadModificar != 0) {
                        idActividad = idActividadModificar;
                    }
                    GuardarNuevaActividad(txtModaldescripcion.val(), idActividad, cboAltaTipoActividad.val());
                    $('#modalAltaActividad').modal('toggle');
                }
            });
            $(document).on('click', '.btnEliminadoActividad', function () {
                ELiminarActividad($(this).attr("id"));
            });
            $(document).on('click', '#btndialogalertaGeneral', function () {
                $(".modalAlerta").modal("hide")
                modaltablaPM.modal("hide");
            });
            $(document).on('click', '#AltaComponentes', function () {
                modalComponentes.modal("show");
            });
            $(document).on('click', '#btnNuevo_Componente', function () {
                modalAltaComponte.modal("show");
            });
            $(document).on('click', '#btnGuardarParte', function () {
                GuardarParte($("#txtAltaCompMin").val(), $("#txtAltaCompDesc").val(), $("#txtAltaCompMax").val());
            });
            $(document).on('click', '#VinCompAct', function () {
                modalVinCompAct.modal("show");
            });
            $("#modalDocumento").on("shown.bs.modal", function () {
                $("#DoctoSubir").attr('hidden', false);
                $(document).on('click', '#btnVisualizarFormato', function () {
                    DescargaArchicoFormato($("#btnVisualizarFormato").attr("ruta"));
                });
            });
            $("#modalDocumento").on("hidden.bs.modal", function () {
                $("#DoctoSubir").html("");
            });
            modalVinCompAct.on("show.bs.modal", function () {
                $("#ActividadVinc").attr("disabled", true);
                idActividad = $("#idActividadVinc").text();

                $("#cboFiltroTipoVinc").fillCombo('/MatenimientoPM/FillCboTipo_Maquina', { estatus: true });
                $("#cboFiltroTipoVinc").val("1").change();

                $("#cboFiltroTipoVinc").change(function () {
                    $("#cboFiltroGrupoVinc").fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: $("#cboFiltroTipoVinc").val() });
                });

                //$("#cboFiltroTipoVinc").attr('disabled', true);
                $("#cboFiltroGrupoVinc").fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: $("#cboFiltroTipoVinc").val() });
                $("#cboFiltroModeloVinc").attr('disabled', true);
                $("#cboFiltroTipoVinc").val("1").change();
                vPM1.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 1 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                vPM2.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 1 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                vPM3.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 1 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                vPM4.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 1 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                vJG.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 2 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                vExtras.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 3 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                vDN0.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 4 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                vDN1.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 4 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                vDN2.click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active')
                    $("#ActividadVinc").val("");
                    $("#ActividadVinc").getAutocomplete(SelectActividad, { term: $("#ActividadVinc").val(), idTipo: 4 }, '/MatenimientoPM/getCatActividad');//autocompletado
                });
                $("#liAgrupaciones").click(function () {
                    $(".tabs #menu .btn-group .active").removeClass('active');


                    tblGroupComponentes.DataTable().draw(false);

                });
                $("#idActividadVinc").text("");

                fnLimpiarTablas();

            });


            function fnLimpiarTablas() {

                grid_pm1.bootgrid("clear");
                grid_pm2.bootgrid("clear");
                grid_pm3.bootgrid("clear");
                grid_pm4.bootgrid("clear");
                grid_JG.bootgrid("clear");
                grid_EX.bootgrid("clear");
                grid_DN0.bootgrid("clear");
                grid_DN1.bootgrid("clear");
                grid_DN2.bootgrid("clear");
                tblGroupComponentes.DataTable().clear().draw();
                tblComponentes_Lubricantes.DataTable().clear().draw();
                tblComponentes_Filtros.DataTable().clear().draw();
                cboGroupComponente.val('');
                cboLubricantes_Lubricantes.val('');
                tbPeriodo.val('');
                tbLitros.val('');
                cboFiltro_Filtros.val('');
                tbCantidadComponentes.val('');
                tblComponentes_Filtros.val('');

            }
            $("#cboTipoMantenientoContador").on("change", function () {
                $(".txtTipoUltimoServLu").val($("#cboTipoMantenientoContador option:selected").text());
                var auxModelo = ConsultarModeloEconomico($("#cboFiltroNoEconomico option:selected").text());
                ConsultarActividadesDN(auxModelo);
            });
            modalComponentes.on("shown.bs.modal", function () {
                CargaGridParte()
                $("#txtDescripcionComp").getAutocomplete(SelectActividad, null, '/MatenimientoPM/getCatComponente');//autocompletado
            });
            modalAltaComponte.on("shown.bs.modal", function () {
                $("#txtAltaCompDesc").getAutocomplete(SelectActividad, null, '/MatenimientoPM/getCatComponente');//autocompletado
            });

            $('a[href$="#ppalTabComponentes"]').on('shown.bs.tab', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });

            $('a[href$="#ppalTabFiltros"]').on('shown.bs.tab', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });

            $('a[href$="#ppalTabLubricantes"]').on('shown.bs.tab', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });

            modalAltaActividad.on('hidden.bs.modal', function (e) {
                DescripcionActMod = "";//seteo para cuando se modifico
                txtModaldescripcion.val("");
                BuscarActividad("", 0, 0);
                idTipoActividadMod = 0;//seteo para cuando se modig
                idActividadModificar = 0;
            });

            modalAltaActividad.on("shown.bs.modal", function () {
                $("#cboAltaTipoActividad").fillCombo('/MatenimientoPM/getTipoActividad', { estatus: true });//carga ajax combo
                if (idTipoActividadMod != 0) {
                    $("#cboAltaTipoActividad").val(idTipoActividadMod).change();
                }
            });
            modalActividad.on("shown.bs.modal", function () {
                ruta = '/MatenimientoPM/FillGridActividad';
                loadGrid(getFiltrosObject(), ruta, grid_Actividad);
            });
            $(document).ready(function () {
                calendar.fullCalendar({
                    eventClick: function (calEvent, jsEvent, view) {
                        if (calEvent.tipoMantenimientoActual != 3) {
                            modaltablaPM.modal("show");
                            idmaquina.append(calEvent.idMaquina);
                            idmantenimiento.append(calEvent.id);
                            $("#tablapmtitulo").val(calEvent.economicoID);
                            $("#cboTipoMantenientoContador").fillCombo('/MatenimientoPM/FillCombotablaPM', { estatus: true });//carga ajax combo
                        }
                        else {

                            //$.blockUI({
                            //    message: mensajes.PROCESANDO,
                            //    baseZ: 2000
                            //});

                            var idReporte = "85";
                            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + calEvent.id;

                            ireport.attr("src", path);
                            document.getElementById('report').onload = function () {
                                //$.unblockUI();
                                openCRModal();
                            };

                        }


                    },
                    editable: true,
                    eventDrop: function (event, delta, revertFunc) {
                        if (!confirm(" ¿Se procede a relizar Cambio De Fecha?" + event.start.format())) {
                            revertFunc();
                        } else {
                            ModificacionFecha(event.id, event.start.format())
                        }
                    }
                });
                $('#timepicker_6').timepicker({
                    showPeriod: true,
                    showLeadingZero: true
                })
                $(".fc-scroller").removeAttr("style");
                //raguilar pruebas 27/04/18     
                $("#grid_MiselaneoAceite-footer").addClass("hidden");//ocultar footer paginadr
                $("#grid_MiselaneoFiltro-footer").addClass("hidden");//ocultar footer paginadr
                ///funcionamiento step altaServicio
                var navListItems = $('div.setup-panel div a'),
                    allWells = $('.setup-content'),
                    allNextBtn = $('.nextBtn');
                allWells.hide();
                navListItems.click(function (e) {
                    e.preventDefault();
                    var $target = $($(this).attr('href')),
                        $item = $(this);
                    if (!$item.hasClass('disabled')) {
                        navListItems.removeClass('btn-success').addClass('btn-default');
                        $item.addClass('btn-success');
                        allWells.hide();
                        $target.show();
                        $target.find('input:eq(0)').focus();
                    }
                });
                $('div.setup-panel div a.btn-success').trigger('click');

            });
            $(".option").click(function () {
                $(this).find('span').toggleClass('inactive');
                $(this).toggleClass('active');
            });
            //$(".fc-toolbar").append(botnAgregar);//agregar boton nuevo servicio
            $("#btn-modal").click(function () {
                $("#summary-list").empty();
                $(".option").each(function () {
                    if (!$(this).children().hasClass('inactive'))
                        $("#summary-list").append("<li>" + $(this).text() + "</li>");
                });
                if ($("#summary-list").children().length == 0)
                    $("#summary-list").append("<li>No options selected</li>");
            });
            $("#btn-reset").click(function () {
                $(".option").each(function () {
                    $(this).children(
                    ).addClass('inactive');
                    $(this).removeClass('active');

                });
            });
            jQuery(function ($) {
                $.timepicker.regional['es'] = {
                    hourText: 'Hora',
                    minuteText: 'Minuto',
                    amPmText: ['AM', 'PM'],
                    closeButtonText: 'Aceptar',
                    nowButtonText: 'Ahora',
                    deselectButtonText: 'Deseleccionar'
                }
                $.timepicker.setDefaults($.timepicker.regional['es']);
            });
        }

        function fncGetModeloEconomico(idNoEconomico) {
            let obj = {};
            obj.idNoEconomico = idNoEconomico;
            axios.post('GetModeloEconomico', obj).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    $("#cboFiltroNoEconomico").data().idModelo = items;
                } else {
                    Alert2Error(message);
                }
            }).catch(error => Alert2Error(error.message));
        }

        /// Actualizacion de guardado de agrupacion.
        function setCatPM_CatActividadPM(objTipoActividad) {

            if (objTipoActividad.attr('id-idcattipoactividad') != undefined) {

                obj = {};
                obj.id = 0;
                obj.idAct = idActividadVinc.text();
                obj.idCatTipoActividad = objTipoActividad.attr('id-idcattipoactividad');
                obj.idPM = objTipoActividad.attr('data-idpm') != undefined ? objTipoActividad.attr('data-idpm') : 0;
                obj.orden = 0;
                obj.modeloEquipoID = cboFiltroModeloVinc.val();
                obj.estado = true;
                obj.leyenda = false;
                obj.perioricidad = 0;
                obj.idDN = objTipoActividad.attr('data-dn') != undefined ? objTipoActividad.attr('data-dn') : 0;
                obj.UsuarioCap = 0;
                obj.fechaCaptura = Date.now();
                VinculaNuevaActividad(obj);
            }
            else {
                AlertaGeneral('Alerta', 'El esquema de mantenimiento no es valido,Favor de intentar con uno nuevo.');
            }

        }
        function addNewActividad() {
            objTipoActividad = $("#btnCat").children('li.active').find('label.active');

            if (objTipoActividad == undefined || objTipoActividad.length == 0)
                objTipoActividad = $("#btnCat").children('li.active').find('a');

            if (objTipoActividad != undefined) {

                setCatPM_CatActividadPM(objTipoActividad);
            }
            else {
                AlertaGeneral('Alerta', 'El esquema de mantenimiento no es valido,Favor de intentar con uno nuevo.');
            }
        }

        ///

        btnRefresh.click(reloadTablas);
        function reloadTablas() {
            ConsultarModelo($("#cboFiltroNoEconomico option:selected").text())
        }

        function ConsultarJGEstructura(modeloEquipoID) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarJGEstructura',
                data: { modeloEquipoID: modeloEquipoID },
                success: function (response) {
                    //$.unblockUI();
                    $("#gridPaso2").bootgrid("clear");
                    $("#gridPaso2").bootgrid("append", response.Actividad);
                    ConsultarActividadesExtras(modeloEquipoID);
                    //$.unblockUI();

                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        $('.row setup-content').on('shown.bs.tab', function () {
            tblGridLubProxTbl.draw();
        });


        function verModalPlaneacion() {
            setFechas();
            modalProgramacionSemanal.modal('show');
        }

        function setFechas() {
            datePicker();
            var now = new Date(),
                year = now.getYear() + 1900;
            dateIni.datepicker().datepicker("setDate", "01/01/" + year);
            dateFin.datepicker().datepicker("setDate", new Date());

        }

        function datePicker() {
            var now = new Date(),
                year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
                from = $("#fechaIni")
                    .datepicker({
                        changeMonth: true,
                        changeYear: true,
                        numberOfMonths: 1,
                        defaultDate: new Date(year, 0, 1),
                        maxDate: new Date(year, 11, 31),

                        onChangeMonthYear: function (y, m, i) {
                            var d = i.selectedDay;
                            $(this).datepicker('setDate', new Date(y, m - 1, d));
                            $(this).trigger('change');
                        }

                    })
                    .on("change", function () {
                        to.datepicker("option", "minDate", getDate(this));
                    }),
                to = $("#fechaFin").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(),
                    maxDate: new Date(year, 11, 31),
                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));
                        $(this).trigger('change');
                    }
                })
                    .on("change", function () {
                        from.datepicker("option", "maxDate", getDate(this));
                    });

            function getDate(element) {
                var date;
                try {
                    date = $.datepicker.parseDate(dateFormat, element.value);
                } catch (error) {
                    date = null;
                }

                return date;
            }
        }

        selectReporte.change(alternarTablaReporteSemanal);
        const tablaReporteSemanal = $('#tablaReporteSemanal');
        const btnCargarReporteSemanal = $('#btnCargarReporteSemanal');
        const divTablaReporteSemanal = $('#divTablaReporteSemanal');
        let dtReporteSemanal;

        btnCargarReporteSemanal.click(() => {
            const areaCuenta = cboAreaCuentarpt.val().trim();
            const strFechaInicio = dateIni.val().trim();
            const strFechaFin = dateFin.val().trim();
            const economico = cboEconomicorpt.val().trim();

            if (datosInvalidosReporteSemanal(areaCuenta, strFechaInicio, strFechaFin)) {
                AlertaGeneral(`Aviso`, `Debe llenar todos los campos correctamente.`);
                return;
            }

            //$.blockUI({ message: 'Cargando datos...' });
            $.get('/MatenimientoPM/getPlaneacionSemanal', { areaCuenta, strFechaInicio, strFechaFin, economico })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        dtReporteSemanal.clear().rows.add(response.items).draw();
                        $(".comboComponentes").select2();
                        btnVerReporte.show(500);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                        btnVerReporte.hide(500);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    btnVerReporte.hide(500);
                }
                );

        });

        function datosInvalidosReporteSemanal(pAreaCuenta, strFechaInicio, strFechaFin) {
            return (pAreaCuenta == "" || strFechaInicio == "" || strFechaFin == "");
        }

        function initTablaReporteSemanal() {
            dtReporteSemanal = tablaReporteSemanal.DataTable({
                language: dtDicEsp,
                dom: '<t>',
                paging: false,
                order: [[2, "desc"]],
                columns: [
                    { data: 'economico', title: 'Económico' },
                    { data: 'tipoServicio', title: 'Tipo de Servicio' },
                    {
                        data: 'fechaProgramado', title: 'Fecha', visible: false,
                        render: function (data, type, row) {
                            // return moment(data).format('DD/MM/YYYY');
                            return moment(data).format('YYYY/MM/DD');
                        }
                    },
                    {
                        data: 'fechaProgramado', title: 'Fecha',
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    { data: 'horometroProgramado', title: 'Horómetro' },
                    {
                        data: 'id', title: 'Duración Estimada', render: function (data, type, row) {
                            return '<input type="number" min=0 class="form-control inputDuracion" value=0 data-index="' + data + '"></input>';
                        }
                    },
                    {
                        data: 'componentes', title: 'Duración Estimada', render: function (data, type, row) {
                            var html = '<select class="form-control comboComponentes" multiple="multiple">';
                            data.forEach(function (valor, indice, array) {
                                html += '<option value="' + valor.Value + '" data-prefijo="' + valor.Prefijo + '">' + valor.Text + '</option>'
                            });
                            html += '</select>';
                            return html;
                        }
                    },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function alternarTablaReporteSemanal() {
            switch (+selectReporte.val()) {
                case 1:
                    $(".divTablaReporteSemanal").show(500);
                    btnVerReporte.hide(500);
                    botonJOMAGALI.hide(500);
                    break;
                case 2:
                    btnVerReporte.show(500);
                    $(".divTablaReporteSemanal").hide(500);
                    botonJOMAGALI.hide(500);
                    break;
                case 3:
                    btnVerReporte.hide(500);
                    $(".divTablaReporteSemanal").hide(500);
                    botonJOMAGALI.show(500);
                    break;
                default:
                    btnVerReporte.hide(500);
                    $(".divTablaReporteSemanal").hide(500);
                    botonJOMAGALI.hide(500);
                    break;
            }
        }

        function getListaDuracionSemanal() {
            return tablaReporteSemanal.find('tbody tr')
                .toArray()
                .map(row => {
                    const $row = $(row);
                    const rowData = dtReporteSemanal.row($row).data();
                    return {
                        Text: rowData.economico,
                        Value: $row.find('.inputDuracion').val(),
                        componentes: $row.find('.comboComponentes').val(),
                        id: $row.find('.inputDuracion').attr("data-index"),
                    };
                });
        }

        function verReporte() {
            if (selectReporte.val() == 1) {
                verReportePlaneacionSemanal();
            }

            if (selectReporte.val() == 2) {
                var idReporte = "97";

                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pAreaCuenta=" + cboAreaCuentarpt.val() + "&pFechaInicioPm=" + dateIni.val() + "&pFechaFinPm=" + dateFin.val() + "&pEconomico1=" + cboEconomicorpt.val();
                ireport.attr("src", path);
                document.getElementById('report').onload = function () {
                    //$.unblockUI();
                    openCRModal();
                };
            }
        }

        function descargarJOMAGALI() {
            if (+cboAreaCuentarpt.val() == 0 || cboAreaCuentarpt.val() == undefined) {
                Alert2Warning('Debe seleccionar una área-cuenta.');
                return;
            }

            $('#variableAreaCuenta').val(cboAreaCuentarpt.val());
            $('#variableFechaInicio').val(dateIni.val());
            $('#variableFechaFin').val(dateFin.val());
            $('#variableEconomico').val(cboEconomicorpt.val());

            $("#formJOMAGALI").submit();
        }

        function verReportePlaneacionSemanal() {

            const pAreaCuenta = cboAreaCuentarpt.val().trim();
            const strFechaInicio = dateIni.val().trim();
            const strFechaFin = dateFin.val().trim();
            const pEconomico1 = cboEconomicorpt.val().trim();

            if (dtReporteSemanal.data().count() <= 0 || datosInvalidosReporteSemanal(pAreaCuenta, strFechaInicio, strFechaFin)) {
                AlertaGeneral(`Aviso`, `Datos inválidos para generar el reporte.`);
                return;
            }

            const listaDuracion = getListaDuracionSemanal();

            //$.blockUI({ message: 'Cargando...' });
            $.post('/MatenimientoPM/CargarHorasPlaneacionSemanal', { listaDuracion })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        //$.blockUI({ message: 'Generando reporte...' });
                        const path = `/Reportes/Vista.aspx?idReporte=96&pAreaCuenta=${pAreaCuenta}&pFechaInicioPm=${strFechaInicio}&pFechaFinPm=${strFechaFin}&pEconomico1=${pEconomico1}`;
                        ireport.attr("src", path);
                        document.getElementById('report').onload = function () {
                            dtReporteSemanal.clear().draw();
                            //$.unblockUI();
                            openCRModal();
                        };
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function GuardadoJG(array) {
            arrJG = [];
            for (var i = 0; i < array.length; i++) {

                objJG = {
                    id: 0,

                    idMisc: array[i].lubricanteID,
                    prueba: array[i].pruebaLub,
                    idComp: array[i].componenteID,
                    vidaActual: array[i].vidaActual,
                    Hrsaplico: array[i].horServ,
                    VidaRestante: array[i].vidaRest,
                    Vigencia: array[i].vidaLubricante,
                    Aplicado: true,
                    alta: true,
                    idAct: 0

                };
                arrJG.push(objJG);
            }
        };
        function GuardadoAE() {
            arrAE = [];
            var longitud = $("#gridPaso3 >tbody >tr").length
            for (var i = 0; i <= (longitud - 1); i++) {
                renglon = $("#gridPaso3 >tbody >tr")[i]
                Aplicado = true;

                idComp = $(renglon).closest('tr').find('td:eq(0) .Componente').attr("data-index");
                Hrsaplico = $(renglon).closest('tr').find('td:eq(1) .HorServ').val();
                idPerioricidad = $(renglon).closest('tr').find('td:eq(2) .perioricidad').attr('data-index');//id de la tabla tblM_CatPM_CatActividadPM para posteriormente tomar el id
                idAct = $(renglon).closest('tr').find('td:eq(2) .perioricidad').attr('data-idAct');
                vidaActual = $(renglon).closest('tr').find('td:eq(3) .TiempoTrans').val();
                VidaRestante = $(renglon).closest('tr').find('td:eq(4) .hrsRest').val();
                objAE = { id: 0, Aplicado: Aplicado, Hrsaplico: Hrsaplico, idPerioricidad: idPerioricidad, idAct: idAct, vidaActual: vidaActual, vidaRestante: VidaRestante, idComp: idComp, alta: true };
                arrAE.push(objAE);
            }
        };
        function GuardadoDN() {
            arrDN = [];
            var longitud = $("#gridPaso4 >tbody >tr").length
            for (var i = 0; i <= (longitud - 1); i++) {
                renglon = $("#gridPaso4 >tbody >tr")[i]
                Aplicado = true;
                Hrsaplico = $(renglon).closest('tr').find('td:eq(1) .HorServ').val();
                idPerioricidad = $(renglon).closest('tr').find('td:eq(2) .perioricidad').attr('data-index');//id de la tabla tblM_CatPM_CatActividadPM para posteriormente tomar el id
                idAct = $(renglon).closest('tr').find('td:eq(2) .perioricidad').attr('data-idAct');
                vidaActual = $(renglon).closest('tr').find('td:eq(3) .TiempoTrans').val();
                VidaRestante = $(renglon).closest('tr').find('td:eq(4) .hrsRest').val();
                objDN = { id: 0, Aplicado: Aplicado, Hrsaplico: Hrsaplico, idPerioricidad: idPerioricidad, idAct: idAct, vidaActual: vidaActual, vidaRestante: VidaRestante, alta: true };
                arrDN.push(objDN);
            }
        };
        function ProgramaActividades() {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ProgramaActividades',
                data: { idmantenimiento: idmantenimiento },
                success: function (response) {
                    //$.unblockUI();
                    $('#calendar').fullCalendar('removeEvents')
                    ConsultaPm(0);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        function ConsultarJGEstructura(modeloEquipoID) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultaInfoLubricantesAlta',
                data: { modeloEquipoID: modeloEquipoID },
                success: function (response) {
                    //$.unblockUI();
                    $(".services_panel").html("");
                    if (response.success == true) {
                        //pruebas JG 28/05/18
                        var html = '';
                        html += ' <div class="form-group">'
                        html += ' <div class="col-xs-12">'
                        html += ' <button type="button" class="btn btn-default pull-left" style="margin-bottom:0px; visibility:hidden">Horometro Actual.-</button>'
                        html += ' <button type="button" id="btnHorometro"  class="btn btn-danger pull-left" style="margin-bottom:0px; visibility:hidden""></button>'
                        html += ' </div>'
                        html += ' </div>'
                        html += ' <fieldset class="fieldset-custm">'
                        html += '  <legend class="legend-custm">Alta Control Lubricantes:</legend>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Horometro Actual:</span>'
                        html += '           <input type="text" class="form-control txtHorometroLubricantes" disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Horometro Último Servicio:</span>'
                        html += '           <input type="text" class="form-control txtHorometroUltLub" disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Tipo Último Servicio:</span>'
                        html += '           <input type="text" class="txtTipoUltimoServLu form-control " disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '<div class="col-lg-12">'
                        html += ' <table id="gridPaso2" class="table table-condensed table-hover table-striped text-center">'
                        html += '  <thead class="bg-table-header">'
                        html += '        <tr>'
                        html += '<th data-column-id="componente"  data-formatter="componente" data-header-align="center" data-sortable="false" data-width="1%">Componente Lubricante</th>'
                        html += '<th data-column-id="AceiteVin"  data-formatter="AceiteVin" data-align="center" data-header-align="center" data-sortable="false" data-width="30%">Suministro</th>'
                        html += '<th data-formatter="chkPrueba" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Tipo Prueba</th>'
                        html += '<th data-column-id="InputVida" data-formatter="InputVida" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Vida Util</th>'
                        html += '<th data-column-id="HorServ" data-formatter="HorServ" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Horometro Servicio</th>'
                        html += '<th data-formatter="EdadSum" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Vida Aceite</th>'
                        html += '<th data-formatter="VidaRest" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Vida Restante</th>'
                        html += '<th data-column-id="estatus" data-formatter="estatus" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Estatus</th>'
                        //html += '<th data-formatter="chkAplico" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Aplicó</th>'
                        html += '</tr>'
                        html += '</thead>'
                        html += '</table>'
                        html += '</div>'
                        html += ' </fieldset>'
                        html += '</div>'
                        html += '</div>'
                        //  $(".services_panel").append(html);
                        //  initGridPaso2($("#gridPaso2"));
                        //  $("#gridPaso2").bootgrid("clear");
                        //  $("#gridPaso2").bootgrid("append", response.Actividad);
                        setTableLubricantesAlta(response.dataSet);
                        ConsultarActividadesExtras(modeloEquipoID);

                        //$.unblockUI();
                    } else {
                        //$.unblockUI();
                        if ($("#cboFiltroNoEconomico  option:selected").text() != "--Seleccione--") {
                            AlertaModal("Error", "No existe Informacion de Lubricantes  Relacionados al Modelo  " + response.modelo + " ");
                        }
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        function ConsultarActividadesExtras(modeloEquipoID) {

            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarActividadesExtras',
                data: { modeloEquipoID: modeloEquipoID },
                success: function (response) {
                    //$.unblockUI();
                    $(".services_panel3").html("");
                    if (response.success == true && response.Actividad != "") {
                        var html = '';
                        html += ' <div class="form-group">'
                        html += '         <div class="row">'
                        html += ' <div class="col-xs-12">'
                        html += ' <button type="button" class="btn btn-default pull-left" style="margin-bottom:0px; visibility:hidden">Horometro Actual.-</button>'
                        html += ' <button type="button" id="btnHorometro2" class="btn btn-danger pull-left" style="margin-bottom:0px; visibility:hidden"></button>'
                        html += ' </div>'
                        html += ' </div>'
                        html += ' <fieldset class="fieldset-custm">'
                        html += '  <legend class="legend-custm">Alta Control Actividades Extras:</legend>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Horometro Actual:</span>'
                        html += '           <input type="text" class="form-control txtHorometroLubricantes" disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Horometro Último Servicio:</span>'
                        html += '           <input type="text" class="form-control txtHorometroUltLub" disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Tipo Último Servicio:</span>'
                        html += '           <input type="text" class="txtTipoUltimoServLu form-control " disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '<div class="col-lg-12">'
                        html += ' <table id="gridPaso3" class="table table-condensed table-hover table-striped text-center">'
                        html += '  <thead class="bg-table-header">'
                        html += '        <tr>'
                        html += '<th data-column-id="descripcion" data-align="center" data-header-align="center" data-sortable="false" data-width="1%">Actividad</th>'
                        html += '<th data-column-id="HorServ" data-formatter="HorServ" data-align="center" data-header-align="center" data-sortable="false" data-width="15%">Horometro Servicio</th>'
                        html += '<th data-formatter="perioricidad"  data-align="center" data-header-align="center" data-sortable="false" data-width="15%">Perioricidad</th>'
                        html += '<th data-column-id="TiempoTrans" data-formatter="TiempoTrans" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Horas Transcurrido</th>'
                        html += '<th data-column-id="hrsRest" data-formatter="hrsRest" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Horas Restantes</th>'
                        html += '<th data-column-id="estatus" data-formatter="estatus" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Estatus</th>'
                        //html += '<th  data-formatter="chkAplico" data-align="center" data-header-align="center" data-sortable="false" data-width="15%">Aplicó</th>'
                        html += '</tr>'
                        html += '</thead>'
                        html += '</table>'
                        html += '</div>'
                        html += ' </fieldset>'
                        html += '</div>'
                        html += '</div>'
                        $(".services_panel3").append(html);
                        initGridPaso3($("#gridPaso3"));
                        $("#gridPaso3").bootgrid("clear");
                        $("#gridPaso3").bootgrid("append", response.Actividad);
                        ConsultarActividadesDN(modeloEquipoID);
                        //$.unblockUI();
                    } else {
                        //consultar modelo
                        LimpiarFormularios();
                        AlertaModal("Error", "No existe Informacion de Actividades Extras Relacionados al Modelo   '" + response.modelo + "'");
                        //modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        function ConsultarActividadesDN(modeloEquipoID) {
            var auxIdPM = $("#cboTipoMantenientoContador").val();
            switch (auxIdPM) {
                case "2": case "6":
                    auxIdPM = 2;
                    break;
                case "4":
                    auxIdPM = 3;
                    break;
                case "8":
                    auxIdPM = 4;
                    break;
                default:
                    auxIdPM = 1;
                    break;
            }
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarActividadesDN',
                data: { modeloEquipoID: modeloEquipoID, idPM: auxIdPM },
                success: function (response) {
                    //$.unblockUI();
                    $(".services_panel4").html("");
                    if (response.success == true /*&& response.Actividad != ""*/) {
                        var html = '';
                        html += ' <div class="form-group">'
                        html += '         <div class="row">'
                        html += ' <div class="col-xs-12">'
                        html += ' <button type="button" class="btn btn-default pull-left" style="margin-bottom:0px; visibility:hidden">Horometro Actual.-</button>'
                        html += ' <button type="button" id="btnHorometro2" class="btn btn-danger pull-left" style="margin-bottom:0px; visibility:hidden"></button>'
                        html += ' </div>'
                        html += ' </div>'
                        html += ' <fieldset class="fieldset-custm">'
                        html += '  <legend class="legend-custm">Alta Control DNS:</legend>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Horómetro Actual:</span>'
                        html += '           <input type="text" class="form-control txtHorometroLubricantes" disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Horómetro Último Servicio:</span>'
                        html += '           <input type="text" class="form-control txtHorometroUltLub" disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '   <div class="col-lg-12">'
                        html += '       <div class="input-group">'
                        html += '           <span class="input-group-addon">Tipo Último Servicio:</span>'
                        html += '           <input type="text" class="txtTipoUltimoServLu form-control " disabled/>'
                        html += '                </div>'
                        html += '   </div>'
                        html += '</div>'
                        html += '<div class="row">'
                        html += '<div class="col-lg-12">'
                        html += ' <table id="gridPaso4" class="table table-condensed table-hover table-striped text-center">'
                        html += '  <thead class="bg-table-header">'
                        html += '        <tr>'
                        html += '<th data-column-id="descripcion" data-align="center" data-header-align="center" data-sortable="false" data-width="1%">Actividad</th>'
                        html += '<th data-column-id="HorServ" data-formatter="HorServ" data-align="center" data-header-align="center" data-sortable="false" data-width="15%">Horometro Servicio</th>'
                        html += '<th data-formatter="perioricidad"  data-align="center" data-header-align="center" data-sortable="false" data-width="15%">Perioricidad</th>'
                        html += '<th data-column-id="TiempoTrans" data-formatter="TiempoTrans" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Horas Transcurrido</th>'
                        html += '<th data-column-id="hrsRest" data-formatter="hrsRest" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Horas Restantes</th>'
                        html += '<th data-column-id="estatus" data-formatter="estatus" data-align="center" data-header-align="center" data-sortable="false" data-width="10%">Estatus</th>'
                        //html += '<th  data-formatter="chkAplico" data-align="center" data-header-align="center" data-sortable="false" data-width="15%">Aplicó</th>'
                        html += '</tr>'
                        html += '</thead>'
                        html += '</table>'
                        html += '</div>'
                        html += ' </fieldset>'
                        html += '</div>'
                        html += '</div>'
                        $(".services_panel4").append(html);
                        initGridPaso4($("#gridPaso4"));
                        $("#gridPaso4").bootgrid("clear");
                        $("#gridPaso4").bootgrid("append", response.Actividad);
                        //ConsultarActividadesDN(modeloEquipoID);
                        //$.unblockUI();
                    } else {
                        //consultar modelo
                        LimpiarFormularios();
                        AlertaModal("Error", "No existe Información de DN'S Relacionados al Modelo   '" + modeloEquipoID + "'");
                        //modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        function DescargaArchicoFormato(id) {
            window.location.href = '/MatenimientoPM/getFileDownload?id=' + id;
        };

        function EliminarVincDoc() {
            idActividad = btnGuardarDoc.attr("data-idFormato");
            modelo = $("#cboFiltroModeloVinc option:selected").val()
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                url: '/MatenimientoPM/EliminarVincDoc',
                type: 'POST',
                dataType: 'json',
                data: { idActividad: idActividad, modelo: modelo },
                success: function (response) {
                    if (response.success == true) {
                        $("#btnVisualizarFormato").attr("style", "display: none")//oculta boton de visualizacion
                        $("#nombreArchivo").html("");//vacia la etiqueta dle nombre del formto
                        document.getElementById("fFormato").value = "";//resete el input file name
                        RefrescadoEstatusVinc();//cambia color de estatus del formato
                        //$.unblockUI();
                    }
                    else {
                        ConfirmacionGeneral("Alerta", response.message, "bg-red");
                        //$.unblockUI();
                    }

                    $("#modalDocumento").modal("hide");
                },
                error: function (response) {
                    //$.unblockUI();
                    AlertaGeneral("Alerta", response.message);

                    $("#modalDocumento").modal("hide");
                }
            });
        }
        function ReporteDeServicio() {
            idmant = $(this).attr("idmant");
            //$.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = "85";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&fId=" + idmant;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                //$.unblockUI();
                openCRModal();
            };
            //alert("Roberto");
        }

        function SubirDocManejo() {//subir formato
            idActividad = btnGuardarDoc.attr("data-idFormato");
            modelo = $("#cboFiltroModeloVinc option:selected").val();
            formData = new FormData();
            file1 = document.getElementById("fFormato").files[0];
            formData.append("fFormato", file1);
            formData.append("idActividad", JSON.stringify(idActividad));
            var tabla = $(".GenericaPM ul.nav").find('li.active a').text()
            if (tabla == "") {
                tabla = $(".GenericaPM ul.nav").find("li.active").children("div.btn-group").children(".active").attr("id-data");
            }
            idCatTipoActividad = 0;
            idpm = 0;
            idDN = 0;
            if (tabla == "PM1") {
                idCatTipoActividad = 1;
                idpm = 1;
            } else if (tabla == "PM2") {
                idCatTipoActividad = 1;
                idpm = 2;
            } else if (tabla == "PM3") {
                idCatTipoActividad = 1;
                idpm = 3;
            } else if (tabla == "PM4") {
                idCatTipoActividad = 1;
                idpm = 4;
            } else if (tabla == "JG") {
                $("#vJG").click()
                idCatTipoActividad = 2;
                idpm = 0;
            } else if (tabla == "DN0") {
                idCatTipoActividad = 4;
                idDN = 1;
            } else if (tabla == "DN1") {
                idCatTipoActividad = 4;
                idDN = 2;
            } else if (tabla == "DN2") {
                idCatTipoActividad = 4;
                idDN = 3;
            } else if (tabla == "Extras") {
                idCatTipoActividad = 3;
            }
            formData.append("idDN", JSON.stringify(idDN));
            formData.append("idPM", JSON.stringify(idpm));
            formData.append("idCatTipoActividad", JSON.stringify(idCatTipoActividad));
            if (file1 != undefined) {
                formData.append("modelo", JSON.stringify(modelo));
                //formData.append("tipo", JSON.stringify(1));
                //$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: '/MatenimientoPM/SaveOrUpdate',
                    type: 'POST',
                    dataType: 'json',
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        if (response.success == true) {
                            $("#btnVisualizarFormato").attr("ruta", response.items);
                            $("#btnVisualizarFormato").removeAttr("style");
                            document.getElementById("fFormato").value = "";//resete el input file name
                            //ConfirmacionGeneralFC("Confirmación", "Se Autorizo Correctamente", "bg-green");
                            RefrescadoEstatusVinc();
                            //$.unblockUI();
                        }
                        else {
                            ConfirmacionGeneral("Alerta", response.message, "bg-red");
                            //$.unblockUI();
                        }

                        $("#modalDocumento").modal("hide");
                    },
                    error: function (response) {
                        //$.unblockUI();
                        AlertaGeneral("Alerta", response.message);

                        $("#modalDocumento").modal("hide");
                    }
                });
            }
        }

        function LimpiarFormularios() {
            $("#txtNoEconomicoAlt").val("");
            $("#horometroMaquina").val("");
            $("#HorometroAplico").val("");
            $("#tbNombreEmpleado").val("");
            $("#tbPuestoEmpleado").val("");
            $("#ObservacionesMant").val("");
            $("#HorometroProyectado").val("");
            $("#fechaFinMante").val("");
            $("#fechaUltCal").val("");
            $("#MantenimientoProy").val("");
            $("#tbNombreCoordinador").val("");
            $("#tbPuestoCoordinador").val("");
            $("#tbNombreEmpleado").val("");
            $("#tbPuestoEmpleado").val("");
            $("#archivoPM").val("");
            $("#cboTipoMantenientoContador").fillCombo('/MatenimientoPM/FillCombotablaPM', { estatus: true });//carga ajax combo
            $("#cboTipoManteniento").fillCombo('/MatenimientoPM/FillCombotablaPM', { estatus: true });
            // $("#tbNombreCoordinador").getAutocomplete(SelectCoordinador, null, '/MatenimientoPM/getEmpleados');
            // $("#tbNombreEmpleados").getAutocomplete(SelectEmpleado, null, '/MatenimientoPM/getEmpleados');
            idResponsable = 0;
            idPlaneador = 0;
        }

        function relacionarcboTexto() {
            $("#TextmodaltablaPM").html("");
            $("#TextmodaltablaPM").append("Unidad No Economico:  " + $("#cboFiltroNoEconomico").find('option:selected').text());//raguilar nombre de la unidad Set en el modal altaservicio 24/05/18
            $("#txtNoEconomicoAlt").val($("#cboFiltroNoEconomico").find('option:selected').text());

            //ConsultarFechaUltimoHorometro($("#HorometroMantenimiento").val(), $("#txtNoEconomicoAlt").val());//raparacion 27/06/18

            ConsultarModelo($("#cboFiltroNoEconomico").find('option:selected').text(), $("#fechaIniMante").val());
        }

        function ConsultarModelo(noEconmico, fechaIniMante) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/CargarDatosAltaPM',
                data: { noEconmico: noEconmico, fechaIniMante: fechaIniMante },
                success: function (response) {
                    ConsultarJGEstructura(response.modelo);
                    $("#horometro2").html("");
                    $("#horometro2").attr("hidden", false);
                    $("#horometro2").append($("#horometroMaquina").val());
                    $("#horometro2").attr("id-hrs", $("#horometroMaquina").val());
                    $("#btnHorometro").append($("#horometroMaquina").val());
                    $("#btnHorometro2").append($("#horometroMaquina").val());
                    //raguilar pruebas
                    // $(".txtHorometroLubricantes").val($("#horometroMaquina").val());
                    $(".txtHorometroLubricantes").val(response.horometro.HorometroAcumulado != undefined && response.horometro.HorometroAcumulado != null ? response.horometro.HorometroAcumulado : '');
                    //alert("jijo");
                    //ConsultarFechaUltimoHorometro($("#horometroMaquina").val(), $("#txtNoEconomicoAlt").val());
                    //--> ConsultaUltimoHorometro
                    if (response.horometro != null && response.horometro != 0) {
                        $("#horometroMaquina").val(response.horometro.HorometroAcumulado);
                        $("#fechaUltCal").val(FormateoFechaEdicion(response.horometro.Fecha));
                        $("#fechaIniMante").datepicker("option", "maxDate", new Date(FormateoFechaSet(response.horometro.Fecha)));
                        //if (bandera == false) {
                        $("#fechaIniMante").datepicker("setDate", FormateoFechaSet(response.horometro.Fecha));
                        //}
                        $("#HorometroAplico").attr("disabled", false);
                        $("#timepicker_6").attr("disabled", false);
                        $("#tbNombreEmpleado").attr("disabled", false);
                        $("#ObservacionesMant").attr("disabled", false);
                        $("#cboTipoMantenientoContador").attr("disabled", false);
                        $("#HorometroAplico").attr("disabled", false);
                        $("#fechaIniMante").attr("disabled", false);

                    } else {
                        $("#HorometroMantenimiento").val("");
                        $("#HorometroMantenimiento").attr("placeholder", "No hay Horometro Capturado Para La Fecha");
                        $("#fechaIniMante").datepicker("option", "maxDate", "+0m +0w");
                        $("#HorometroAplico").attr("disabled", true);
                        $("#timepicker_6").attr("disabled", true);
                        $("#tbNombreEmpleado").attr("disabled", true);
                        $("#tbPuestoEmpleado").attr("disabled", true);
                        $("#ObservacionesMant").attr("disabled", true);
                        $("#cboTipoMantenientoContador").attr("disabled", true);
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ConsultarModeloEconomico(noEconmico) {
            var modeloEconomico = 0;
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarModelo',
                data: { noEconmico: noEconmico },
                success: function (response) {
                    //$.unblockUI();
                    modeloEconomico = response.Actividad;
                },
                error: function () {
                    //$.unblockUI();
                }
            });
            return modeloEconomico;
        }

        var tipoPM;
        function VinculaNuevaActividad(obj) {
            //$("#divFormVinculacionActividades").block({ message: mensajes.PROCESANDO });
            tipoPM = $(".GenericaPM ul.nav").find('li.active a').text();
            $(".flex-column").find('li.active a').attr('href');
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/VinculaNuevaActividad',
                data: { obj: obj },
                success: function (response) {

                    if (response.Existente != true || response.Existente == undefined) {
                        if (tipoPM == "PM1") {
                            ActividadVinc.text = "";
                            $("#vPM1").click()
                        } else if (tipoPM == "PM2") {
                            ActividadVinc.text = "";
                            $("#vPM2").click()
                        } else if (tipoPM == "PM3") {
                            ActividadVinc.text = "";
                            $("#vPM3").click()
                        } else if (tipoPM == "PM4") {
                            ActividadVinc.text = "";
                            $("#vPM4").click()
                        } else if (tipoPM == "JG") {
                            ActividadVinc.text = "";
                            $("#vJG").click()
                        } else if (tipoPM == "Extras") {//raguilar habilitar actividades extras contables 17/05/18
                            ActividadVinc.text = "";
                            $("#vExtras").click()
                        }
                        if (response.Actividad != undefined) {
                            ActividadVinc.val("");
                            //AlertaGeneral('Confirmación', 'La actividad fue agregada correctamente.');
                            idmodelo = response.Actividad.modeloEquipoID;
                            idTipoAct = response.Actividad.idCatTipoActividad;
                            BuscarInfoModelo(idmodelo, idTipoAct);
                        }
                    } else {
                        AlertaGeneral("Accion Invalida", "La actividad ya se encuentra en el esquema seleccionado");
                    }
                    //$("#divFormVinculacionActividades").unblock();
                },
                error: function () {
                    AlertaGeneral("Error", response.message);
                    //$("#divFormVinculacionActividades").unblock();
                }
            });
        }
        function CargaGridParte() {
            initGrid(grid_Part);
            ruta = '/MatenimientoPM/FillGridParte';
            loadGrid(getFiltrosObject(), ruta, grid_Part);
        }
        function GuardarNuevaActividad(Actividad, id, idTipo) {
            objCatActividadPM = { id: id, descripcionActividad: Actividad, idCatTipoActividad: idTipo }
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/GuardarNuevaActividad',
                data: { objCatActividadPM: objCatActividadPM },
                success: function (response) {
                    //$.unblockUI();
                    ConfirmacionGeneralFC("Confirmación", response.Actividad, "bg-green");
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function getFiltrosMis() {
            //int modeloEquipoID, int idAct, int idCompVis, int idTipo, int idMis
            return {
                modeloEquipoID: $("#cboFiltroModeloVinc option:selected").val(),
                idAct: $("#btnModCompModalVin").attr("data-idvinc"),
                idCompVis: $("#btnModMis").attr("data-idviscomp"),
                idTipo: $("#cboModalidad option:selected").val()
            }
        }
        function getFiltrosObject() {
            var tabla = $(".GenericaPM ul.nav").find('li.active a').text()
            if (tabla == "") {
                tabla = $(".GenericaPM ul.nav").find("li.active").children("div.btn-group").children(".active").attr("id-data");
            }
            idTipoAct = 0;
            idpm = 0;
            if (tabla == "PM1") {
                idTipoAct = 1;
                idpm = 1;
            } else if (tabla == "PM2") {
                idTipoAct = 1;
                idpm = 2;
            } else if (tabla == "PM3") {
                idTipoAct = 1;
                idpm = 3;
            } else if (tabla == "PM4") {
                idTipoAct = 1;
                idpm = 4;
            } else if (tabla == "Lubricantes") {
                $("#vJG").click()
                idTipoAct = 2;
                idpm = 0;
            } else if (tabla == "DN0") {
                idTipoAct = 4;
                idDN = 4;
            } else if (tabla == "DN1") {
                idTipoAct = 4;
                idDN = 2;
            } else if (tabla == "DN2") {
                idTipoAct = 4;
                idDN = 3;
            }
            return {
                Id: 0,
                modeloEquipoID: $("#cboFiltroModeloVinc option:selected").val(),
                idActs: $("#btnModCompModalVin").attr("data-idvinc"),
                idTipoAct: idTipoAct,
                idpm: idpm
            }
        }
        function SelectActividadTipo(event, ui) {//autocompletado de actividades segun el tipo activo
            idActividadVinc.text(ui.item.id);
        }
        function SelectActividad(event, ui) {
            BuscarActividad(ui.item.value, 0, 0);
            idActividadVinc.text(ui.item.id);
        }
        function BuscarActividad(tmp, idCatTipoActividad, idActividad) {
            obj = { descripcionActividad: tmp, idCatTipoActividad: idCatTipoActividad, id: idActividad };
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/FillGridActividad',
                data: { obj: obj },
                async: false,
                success: function (tmp) {

                    grid_Actividad.bootgrid("clear");
                    grid_Actividad.bootgrid("append", tmp.rows);
                    DescripcionActMod = tmp.rows[0].descripcionActividad;
                    idTipoActividadMod = tmp.rows[0].idTipoActividad;
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function initGridtablaPMGestor(grid) {
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
                            return "<input type='checkbox' data-id='" + row.idobjProy + "' data-index='" + row.id + "'  class='form-control checkActExtra' style='margin-left:20px;' id-act='" + row.id + "'" + checked + ">";
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


                grid.find(".checkActExtra").on("click", function (e) {
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

                        //*Ver si de vuelve a poner.*/
                        //  ConsultarObservacionActividad(idObjAct);
                    }
                    $("#btnguardarActObs").attr("idObj", idObjAct);
                    // $("#modalObsActs").modal("show");
                });
                grid.find(".eliminar").on("click", function (e) {
                    objActvProy = { id: $(this).attr("data-id"), idAct: $(this).attr("data-index"), idMant: $("#MantenimientoSeguimiento").attr('idmant'), Observaciones: "", aplicar: true, estatus: true, idPm: ReturnPm() };
                    EliminarActividadProyectada(objActvProy)
                    //GuardarActividadesProyectadas(objActvProy);
                });
            });
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
        function ConsultarObservacionActividad(idObjAct) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarObservacionActividad',
                data: { idObjAct: idObjAct },
                success: function (response) {
                    //$.unblockUI();
                    $("#ObservacionesMantAct").val("");
                    $("#ObservacionesMantAct").val(response.objActvObs);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        };
        function GuardarActividadesProyectadas(objActvProy) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/GuardarActividadesMantProy',
                data: { objActvProy: objActvProy },
                success: function (response) {
                    //$.unblockUI();
                    ConsultaActividadesProg(response.mntto, response.idTipoPM, response.idMatn);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function EliminarActividadProyectada(objActvProy) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/EliminarActividadProy',
                data: { objActvProy: objActvProy },
                success: function (response) {
                    //$.unblockUI();
                    ConsultaActividadesProg(response.mntto, response.idTipoPM, response.idMatn);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ReturnModelo() {
            var retval;
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarModelo',
                data: { noEconmico: $("#cboFiltroNoEconomicoS option:selected").text() },
                success: function (response) {
                    //$.unblockUI();
                    retval = response.Actividad;
                },
                error: function () {
                    //$.unblockUI();
                }
            });
            return retval;
        }
        function ConsultaActividadesProg(modeloEquipoID, idPM, idmantenimiento) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarGestorPM',
                data: { modeloEquipoID: modeloEquipoID, idPM: idPM, idmantenimiento: idmantenimiento },
                success: function (response) {
                    //$.unblockUI();
                    $("#grid_GestorActividades").bootgrid("clear");
                    $("#grid_GestorActividades").bootgrid("append", response.ObjActProy);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
            $("#grid_GestorActividades-footer .pagination li.page-" + paginaActual + " .button").click();
        }
        function initGridtablaPM(grid) {
            grid.bootgrid({
                //headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "formato": function (column, row) {

                        if (row.idformato != null) {
                            if (row.idformato.nombreRuta == "") {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                    "</label>" +
                                    "<label class='btn btn-default btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                                    ;
                            } else {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label class='btn btn-default btn-off btn-sm active'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>F/A" +
                                    "</label>" +
                                    "<label class='btn btn-success btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato' data-url='" + row.idformato.id + "'" + "data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "'" + "nombre-archivo='" + row.idformato.nombreArchivo + "'name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                            }

                        } else {

                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm formato'>" +
                                "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-file' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                                ;

                        }

                    },
                    "leyenda": function (column, row) {
                        if (row.leyenda == true) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        return "<label  class='btn leyenda'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<input  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off'  " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    },
                    "update": function (column, row) {
                        return "<button type='button'  class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>"
                            ;
                    },
                    "deleteVin": function (column, row) {
                        return "<button type='button' class='btn btn-danger deleteVin' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "componente": function (column, row) {
                        if (row.Componente == "") {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        } else {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-default btn-off btn-sm active'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>C/A" +
                                "</label>" +
                                "<label class='btn btn-success btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".formato").on("click", function (e) {
                    if ($(this).find("#btnformato").attr("data-url") == undefined) {
                        $("#btnVisualizarFormato").hide();
                    }
                    $("#btnEliminarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#btnGuardarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#DoctoSubir").html("");
                    $("#nombreArchivo").html("");
                    $("#nombreArchivo").append($(this).find("#btnformato").attr("nombre-archivo"));
                    $("#nombreArchivo").attr("hidden", false);
                    $("#DoctoSubir").append($(this).find("#btnformato").attr("data-descrip"));
                    $("#btnVisualizarFormato").attr("ruta", $(this).find("#btnformato").attr("data-url"));
                    $("#modalDocumento").modal("show");
                });
                grid.find(".deleteVin").on("click", function (e) {
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
                grid.find(".componenteVin").on("click", function (e) {
                    $("#ComponentesModalVin").modal("show");
                    $("#TextModalVin").html("");
                    $("#TextModalVin").append($(this).find("#btnComp").attr("data-descrip"));
                    $("#btnModCompModalVin").attr("data-idVinc", $(this).find("#btnComp").attr('data-index'));//vinculacion 

                });
                grid.find(".leyenda").on("click", function (e) {
                    //  paginaActualpm1 = $("#grid_pm1").bootgrid("getCurrentPage");
                    //  Banderagirdpm1 = false;
                    modelo = $("#cboFiltroModeloVinc option:selected").val();
                    var idActividad = $(this).attr("data-index");
                    var chkbx = $(this).find("input");
                    if ($(chkbx).is(":checked")) {
                        tipoLeyenda(true, idActividad, modelo);
                    } else {
                        tipoLeyenda(false, idActividad, modelo);
                    }
                    RefrescadoEstatusVinc();
                });
            });
        }
        function EliminadoModal(titulo, mensaje, idAct) {
            if (!$("#modalEliminacion").is(':visible')) {
                var html = '<div id="modalEliminar" class="modal fade" role="dialog" data-backdrop="static">' +
                    '<div class="modal-dialog modal-dialog-fix modal-md" >' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-bg">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                    '&times;</button>' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body ajustar-texto">' +
                    '<h5 id="pMessage">' +
                    '</h5>' +
                    '<div class="row">' +
                    '<div id="icon" class="col-md-2">' +
                    '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
                    '</div>' +
                    '<div class="col-md-10">' +
                    '<h3>  ' + mensaje + '</h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a data-dismiss="modal" id="btnModalAceptarEliminar"   data-index= ' + idAct + ' class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '<a data-dismiss="modal" id="btnCancelarEliminar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }
        }
        function RefrescadoEstatusVinc() {
            grid_pm1.bootgrid("clear");
            var tabla = $(".GenericaPM ul.nav").find('li.active a').text()
            idTipoAct = 0;
            idpm = 0;
            if (tabla == "PM1") {
                idTipoAct = 1;
                idpm = 1;
            } else if (tabla == "PM2") {
                idTipoAct = 1;
                idpm = 2;
            } else if (tabla == "PM3") {
                idTipoAct = 1;
                idpm = 3;
            } else if (tabla == "PM4") {
                idTipoAct = 1;
                idpm = 4;
            } else if (tabla == "JG") {
                $("#vJG").click()
                idTipoAct = 2;
                idpm = 0;
            }
            idmodelo = $("#cboFiltroModeloVinc option:selected").val();
            BuscarInfoModelo(idmodelo, idTipoAct);
        }
        function tipoLeyenda(flagLeyenda, idActividad, modelo) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            obj = { leyenda: flagLeyenda, id: idActividad, modeloEquipoID: modelo }
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/tipoLeyenda',
                data: { obj: obj },
                success: function (response) {
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function initGridtablaPM2(grid) {
            grid.bootgrid({
                //headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "formato": function (column, row) {

                        if (row.idformato != null) {
                            if (row.idformato.nombreRuta == "") {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                    "</label>" +
                                    "<label class='btn btn-default btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                                    ;
                            } else {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label class='btn btn-default btn-off btn-sm active'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>F/A" +
                                    "</label>" +
                                    "<label class='btn btn-success btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato' data-url='" + row.idformato.id + "'" + "data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "'" + "nombre-archivo='" + row.idformato.nombreArchivo + "'name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                            }

                        } else {

                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm formato'>" +
                                "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-file' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                                ;

                        }
                    },
                    "leyenda": function (column, row) {
                        if (row.leyenda == true) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        return "<label  class='btn leyenda'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<input  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off'  " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    },
                    "update": function (column, row) {
                        return "<button type='button'  class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>"
                            ;
                    },
                    "deleteVin": function (column, row) {
                        return "<button type='button' class='btn btn-danger deleteVin' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "componente": function (column, row) {
                        if (row.Componente == "") {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        } else {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-default btn-off btn-sm active'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>C/A" +
                                "</label>" +
                                "<label class='btn btn-success btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".formato").on("click", function (e) {
                    if ($(this).find("#btnformato").attr("data-url") == undefined) {
                        $("#btnVisualizarFormato").hide();
                    }
                    $("#btnEliminarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#btnGuardarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#DoctoSubir").html("");
                    $("#nombreArchivo").html("");
                    $("#nombreArchivo").append($(this).find("#btnformato").attr("nombre-archivo"));
                    $("#nombreArchivo").attr("hidden", false);
                    $("#DoctoSubir").append($(this).find("#btnformato").attr("data-descrip"));
                    $("#btnVisualizarFormato").attr("ruta", $(this).find("#btnformato").attr("data-url"));
                    $("#modalDocumento").modal("show");
                });
                grid.find(".deleteVin").on("click", function (e) {
                    paginaActualpm2 = $("#grid_pm2").bootgrid("getCurrentPage");
                    Banderagirdpm2 = false;
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
                grid.find(".componenteVin").on("click", function (e) {
                    $("#ComponentesModalVin").modal("show");
                    $("#TextModalVin").html("");
                    $("#TextModalVin").append($(this).find("#btnComp").attr("data-descrip"));
                    $("#btnModCompModalVin").attr("data-idVinc", $(this).find("#btnComp").attr('data-index'));//vinculacion 

                });
                grid.find(".leyenda").on("click", function (e) {
                    paginaActualpm2 = $("#grid_pm2").bootgrid("getCurrentPage");
                    Banderagirdpm2 = false;
                    modelo = $("#cboFiltroModeloVinc option:selected").val();
                    var idActividad = $(this).attr("data-index");
                    var chkbx = $(this).find("input");
                    if ($(chkbx).is(":checked")) {
                        tipoLeyenda(true, idActividad, modelo);
                    } else {
                        tipoLeyenda(false, idActividad, modelo);
                    }
                    RefrescadoEstatusVinc();
                });
                Banderagirdpm2 = false;
            });
        }
        function initGridtablaPM3(grid) {
            grid.bootgrid({
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "formato": function (column, row) {
                        if (row.idformato != null) {
                            if (row.idformato.nombreRuta == "") {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                    "</label>" +
                                    "<label class='btn btn-default btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                                    ;
                            } else {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label class='btn btn-default btn-off btn-sm active'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>F/A" +
                                    "</label>" +
                                    "<label class='btn btn-success btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato' data-url='" + row.idformato.id + "'" + "data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "'" + "nombre-archivo='" + row.idformato.nombreArchivo + "'name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                            }

                        } else {

                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm formato'>" +
                                "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-file' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                                ;

                        }
                    },
                    "leyenda": function (column, row) {
                        if (row.leyenda == true) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        return "<label  class='btn leyenda'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<input  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off'  " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    },
                    "update": function (column, row) {
                        return "<button type='button'  class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>"
                            ;
                    },
                    "deleteVin": function (column, row) {
                        return "<button type='button' class='btn btn-danger deleteVin' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "componente": function (column, row) {
                        if (row.Componente == "") {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        } else {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-default btn-off btn-sm active'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>C/A" +
                                "</label>" +
                                "<label class='btn btn-success btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".formato").on("click", function (e) {
                    if ($(this).find("#btnformato").attr("data-url") == undefined) {
                        $("#btnVisualizarFormato").hide();
                    }
                    $("#btnEliminarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#btnGuardarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#DoctoSubir").html("");
                    $("#nombreArchivo").html("");
                    $("#nombreArchivo").append($(this).find("#btnformato").attr("nombre-archivo"));
                    $("#nombreArchivo").attr("hidden", false);
                    $("#DoctoSubir").append($(this).find("#btnformato").attr("data-descrip"));
                    $("#btnVisualizarFormato").attr("ruta", $(this).find("#btnformato").attr("data-url"));
                    $("#modalDocumento").modal("show");
                });
                grid.find(".deleteVin").on("click", function (e) {
                    Banderagirdpm3 = false;
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
                grid.find(".componenteVin").on("click", function (e) {
                    $("#ComponentesModalVin").modal("show");
                    $("#TextModalVin").html("");
                    $("#TextModalVin").append($(this).find("#btnComp").attr("data-descrip"));
                    $("#btnModCompModalVin").attr("data-idVinc", $(this).find("#btnComp").attr('data-index'));//vinculacion 

                });
                grid.find(".leyenda").on("click", function (e) {
                    Banderagirdpm3 = false;
                    modelo = $("#cboFiltroModeloVinc option:selected").val();
                    var idActividad = $(this).attr("data-index");
                    var chkbx = $(this).find("input");
                    if ($(chkbx).is(":checked")) {
                        tipoLeyenda(true, idActividad, modelo);
                    } else {
                        tipoLeyenda(false, idActividad, modelo);
                    }
                    RefrescadoEstatusVinc();
                });
                Banderagirdpm3 = false;
            });
        }
        function initGridtablaPM4(grid) {
            grid.bootgrid({
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "formato": function (column, row) {
                        if (row.idformato != null) {
                            if (row.idformato.nombreRuta == "") {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                    "</label>" +
                                    "<label class='btn btn-default btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                                    ;
                            } else {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label class='btn btn-default btn-off btn-sm active'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>F/A" +
                                    "</label>" +
                                    "<label class='btn btn-success btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato' data-url='" + row.idformato.id + "'" + "data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "'" + "nombre-archivo='" + row.idformato.nombreArchivo + "'name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                            }

                        } else {

                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm formato'>" +
                                "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-file' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                                ;

                        }
                    },
                    "leyenda": function (column, row) {
                        if (row.leyenda == true) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        return "<label  class='btn leyenda'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<input  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off'  " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    },
                    "update": function (column, row) {
                        return "<button type='button'  class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>"
                            ;
                    },
                    "deleteVin": function (column, row) {
                        return "<button type='button' class='btn btn-danger deleteVin' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "componente": function (column, row) {
                        if (row.Componente == "") {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        } else {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-default btn-off btn-sm active'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>C/A" +
                                "</label>" +
                                "<label class='btn btn-success btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".formato").on("click", function (e) {
                    if ($(this).find("#btnformato").attr("data-url") == undefined) {
                        $("#btnVisualizarFormato").hide();
                    }
                    $("#btnEliminarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#btnGuardarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#DoctoSubir").html("");
                    $("#nombreArchivo").html("");
                    $("#nombreArchivo").append($(this).find("#btnformato").attr("nombre-archivo"));
                    $("#nombreArchivo").attr("hidden", false);
                    $("#DoctoSubir").append($(this).find("#btnformato").attr("data-descrip"));
                    $("#btnVisualizarFormato").attr("ruta", $(this).find("#btnformato").attr("data-url"));
                    $("#modalDocumento").modal("show");
                });
                grid.find(".deleteVin").on("click", function (e) {
                    Banderagirdpm4 = false;
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
                grid.find(".componenteVin").on("click", function (e) {
                    $("#ComponentesModalVin").modal("show");
                    $("#TextModalVin").html("");
                    $("#TextModalVin").append($(this).find("#btnComp").attr("data-descrip"));
                    $("#btnModCompModalVin").attr("data-idVinc", $(this).find("#btnComp").attr('data-index'));//vinculacion 

                });
                grid.find(".leyenda").on("click", function (e) {
                    Banderagirdpm4 = false;
                    modelo = $("#cboFiltroModeloVinc option:selected").val();
                    var idActividad = $(this).attr("data-index");
                    var chkbx = $(this).find("input");
                    if ($(chkbx).is(":checked")) {
                        tipoLeyenda(true, idActividad, modelo);
                    } else {
                        tipoLeyenda(false, idActividad, modelo);
                    }
                    RefrescadoEstatusVinc();
                });
                Banderagirdpm4 = false;
            });
        }
        function initGridtablaJG(grid) {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>"
                            ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger deleteVin' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "componente": function (column, row) {
                        if (row.Componente == "") {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        } else {
                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-default btn-off btn-sm active'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>C/A" +
                                "</label>" +
                                "<label class='btn btn-success btn-on btn-sm componenteVin'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-wrench' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".componenteVin").on("click", function (e) {
                    $("#ComponentesModalVin").modal("show");
                    $("#TextModalVin").html("");
                    $("#TextModalVin").append($(this).find("#btnComp").attr("data-descrip"));
                    $("#btnModCompModalVin").attr("data-idVinc", $(this).find("#btnComp").attr('data-index'));//vinculacion 
                    Actividadid = $(this).find("#btnComp").attr('data-index');
                });
                grid.find(".deleteVin").on("click", function (e) {
                    BanderagirdJG = false;
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
                BanderagirdJG = false;
            });
        }
        function initGridtablaExt(grid) {
            grid.bootgrid({
                //headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "formato": function (column, row) {
                        if (row.idformato != null) {
                            if (row.idformato.nombreRuta == "") {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                    "</label>" +
                                    "<label class='btn btn-default btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                                    ;
                            } else {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label class='btn btn-default btn-off btn-sm active'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>F/A" +
                                    "</label>" +
                                    "<label class='btn btn-success btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato' data-url='" + row.idformato.id + "'" + "data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "'" + "nombre-archivo='" + row.idformato.nombreArchivo + "'name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                            }

                        } else {

                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm formato'>" +
                                "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-file' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                                ;

                        }
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger delete' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "Perioricidad": function (column, row) {
                        if (row.perioricidad != null) {
                            return "<input type='number' data-index='" + row.id + "' value='" + row.perioricidad + "' class='form-control bfh-number txtEdad'>";
                        }
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number txtEdad'>";
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".txtEdad").on("change", function (e) {
                    if ($(this).val() != "") {
                        id = $(this).attr("data-index");
                        AsignarEdadAct(id, $(this).val());
                    }
                });
                grid.find(".formato").on("click", function (e) {
                    if ($(this).find("#btnformato").attr("data-url") == undefined) {
                        $("#btnVisualizarFormato").hide();
                    }
                    $("#btnGuardarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#DoctoSubir").html("");
                    $("#nombreArchivo").html("");
                    $("#nombreArchivo").append($(this).find("#btnformato").attr("nombre-archivo"));
                    $("#nombreArchivo").attr("hidden", false);
                    $("#DoctoSubir").append($(this).find("#btnformato").attr("data-descrip"));
                    $("#btnVisualizarFormato").attr("ruta", $(this).find("#btnformato").attr("data-url"));
                    $("#modalDocumento").modal("show");
                });
                grid.find(".delete").on("click", function (e) {
                    BanderagirdEX = false;
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
                BanderagirdEX = false;
            });
        }
        function initGridtablaDN0(grid) {
            grid.bootgrid({
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "formato": function (column, row) {
                        if (row.idformato != null) {
                            if (row.idformato.nombreRuta == "") {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                    "</label>" +
                                    "<label class='btn btn-default btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                                    ;
                            } else {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label class='btn btn-default btn-off btn-sm active'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>F/A" +
                                    "</label>" +
                                    "<label class='btn btn-success btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato' data-url='" + row.idformato.id + "'" + "data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "'" + "nombre-archivo='" + row.idformato.nombreArchivo + "'name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                            }

                        } else {

                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm formato'>" +
                                "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-file' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                                ;

                        }
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger delete' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "Perioricidad": function (column, row) {
                        if (row.perioricidad != null) {
                            return "<input type='number' data-index='" + row.id + "' value='" + row.perioricidad + "' class='form-control bfh-number txtEdad'>";
                        }
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number txtEdad'>";
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".txtEdad").on("change", function (e) {
                    if ($(this).val() != "") {
                        id = $(this).attr("data-index");
                        //AsignarEdad(id, $(this).val());
                        AsignarEdadAct(id, $(this).val());
                    }
                });
                grid.find(".formato").on("click", function (e) {
                    if ($(this).find("#btnformato").attr("data-url") == undefined) {
                        $("#btnVisualizarFormato").hide();
                    }
                    $("#btnGuardarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#DoctoSubir").html("");
                    $("#nombreArchivo").html("");
                    $("#nombreArchivo").append($(this).find("#btnformato").attr("nombre-archivo"));
                    $("#nombreArchivo").attr("hidden", false);
                    $("#DoctoSubir").append($(this).find("#btnformato").attr("data-descrip"));
                    $("#btnVisualizarFormato").attr("ruta", $(this).find("#btnformato").attr("data-url"));
                    $("#modalDocumento").modal("show");
                });
                grid.find(".delete").on("click", function (e) {
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
            });
        }
        function initGridtablaDN1(grid) {
            grid.bootgrid({
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "formato": function (column, row) {
                        if (row.idformato != null) {
                            if (row.idformato.nombreRuta == "") {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                    "</label>" +
                                    "<label class='btn btn-default btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                                    ;
                            } else {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label class='btn btn-default btn-off btn-sm active'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>F/A" +
                                    "</label>" +
                                    "<label class='btn btn-success btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato' data-url='" + row.idformato.id + "'" + "data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "'" + "nombre-archivo='" + row.idformato.nombreArchivo + "'name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                            }

                        } else {

                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm formato'>" +
                                "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-file' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                                ;

                        }
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger delete' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "Perioricidad": function (column, row) {
                        if (row.perioricidad != null) {
                            //return "<input type='number' data-index='" + row.id + "' value='" + row.asignado.vida + "' class='form-control bfh-number txtEdadAceite'>";
                            return "<input type='number' data-index='" + row.id + "' value='" + row.perioricidad + "' class='form-control bfh-number txtEdad'>";
                        }
                        //return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number txtEdadAceite'>";
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number txtEdad'>";
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".txtEdad").on("change", function (e) {
                    if ($(this).val() != "") {
                        id = $(this).attr("data-index");
                        //AsignarEdad(id, $(this).val());
                        AsignarEdadAct(id, $(this).val());
                    }
                });
                grid.find(".formato").on("click", function (e) {
                    if ($(this).find("#btnformato").attr("data-url") == undefined) {
                        $("#btnVisualizarFormato").hide();
                    }
                    $("#btnGuardarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#DoctoSubir").html("");
                    $("#nombreArchivo").html("");
                    $("#nombreArchivo").append($(this).find("#btnformato").attr("nombre-archivo"));
                    $("#nombreArchivo").attr("hidden", false);
                    $("#DoctoSubir").append($(this).find("#btnformato").attr("data-descrip"));
                    $("#btnVisualizarFormato").attr("ruta", $(this).find("#btnformato").attr("data-url"));
                    $("#modalDocumento").modal("show");
                });
                grid.find(".delete").on("click", function (e) {
                    //var idActividad = $(this).attr("data-index");
                    //ELiminarVinculacion(idActividad);
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
            });
        }
        function initGridtablaDN2(grid) {
            grid.bootgrid({
                //headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "formato": function (column, row) {
                        if (row.idformato != null) {
                            if (row.idformato.nombreRuta == "") {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                    "</label>" +
                                    "<label class='btn btn-default btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                                    ;
                            } else {
                                return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                    "<label class='btn btn-default btn-off btn-sm active'>" +
                                    "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>F/A" +
                                    "</label>" +
                                    "<label class='btn btn-success btn-on btn-sm formato'>" +
                                    "<input type='radio' value='1'  id='btnformato' data-url='" + row.idformato.id + "'" + "data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "'" + "nombre-archivo='" + row.idformato.nombreArchivo + "'name='multifeatured_module[module_id][status]' checked='checked'>" +
                                    "<i class='fa fa-file' size='8px;'></i>" +
                                    "</label>" +
                                    "</div>"
                            }

                        } else {

                            return "<div class='btn-group' id='status' data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm formato'>" +
                                "<input type='radio' value='1'  id='btnformato'  data-index='" + row.idAct + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='fa fa-file' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                                ;

                        }
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger delete' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "Perioricidad": function (column, row) {
                        if (row.perioricidad != null) {

                            return "<input type='number' data-index='" + row.id + "' value='" + row.perioricidad + "' class='form-control bfh-number txtEdad'>";
                        }
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number txtEdad'>";
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".txtEdad").on("change", function (e) {
                    if ($(this).val() != "") {
                        id = $(this).attr("data-index");
                        //AsignarEdad(id, $(this).val());
                        AsignarEdadAct(id, $(this).val());
                    }
                });
                grid.find(".formato").on("click", function (e) {
                    if ($(this).find("#btnformato").attr("data-url") == undefined) {
                        $("#btnVisualizarFormato").hide();
                    }
                    $("#btnGuardarDoc").attr("data-idformato", $(this).find("#btnformato").attr('data-index'));
                    $("#DoctoSubir").html("");
                    $("#nombreArchivo").html("");
                    $("#nombreArchivo").append($(this).find("#btnformato").attr("nombre-archivo"));
                    $("#nombreArchivo").attr("hidden", false);
                    $("#DoctoSubir").append($(this).find("#btnformato").attr("data-descrip"));
                    $("#btnVisualizarFormato").attr("ruta", $(this).find("#btnformato").attr("data-url"));
                    $("#modalDocumento").modal("show");
                });
                grid.find(".delete").on("click", function (e) {
                    EliminadoModal("Alerta", "Se perdera la informacion Relacionada Al Modelo ¿Desea Continuar?", $(this).attr("data-index"));//idactividad a eliminar
                });
            });
        }
        function initGridMisAnticongelante(grid) {
            grid.bootgrid({
                rowCount: [5],
                headerCssClass: '.bg-table-header',
                align: 'left',
                templates: {
                    header: ""
                },
                formatters: {
                    "check-asignar": function (column, row) {
                        if (row.asignado != null) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        id = 0;//id registro misc
                        if (row.asignado != null) {
                            id = row.asignado.id;
                        }
                        return "<label  class='btn chkfiltro'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'data-id='" + id + "' >" +
                            "<input  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off' " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    },
                    "Componente": function (column, row) {
                        return row.Componente;
                    },
                    "orden": function (column, row) {
                        return row.Componente.id;
                    },
                    "edad": function (column, row) {
                        if (row.asignado != null) {
                            return "<input type='number' data-index='" + row.asignado.id + "' value='" + row.asignado.vida + "' class='form-control bfh-number txtEdadAceite'>";
                        }
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number txtEdadAceite' disabled>";
                    },
                    "cantidad": function (column, row) {
                        if (row.asignado != null) {
                            return "<input type='number' data-index='" + row.cantidad[0].id + "' value='" + row.cantidad[0].cantidad + "' class='form-control bfh-number txtCantidad'>";
                        } else {
                            return "<input type='number'  class='form-control bfh-number txtCantidad' disabled>";
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".chkfiltro").on("click", function (e) {
                    var chkbx = $(this).find("input");
                    if (chkbx.is(":checked")) {
                        $(this).closest("tr").find("td:eq(3) .txtEdadAceite").attr("disabled", false);
                        id = 0,
                            ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = true;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis };
                        VincularMis(obj);
                        ruta = '/MatenimientoPM/FillGridComponenteVin';
                        loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                        $("#TextModalVin").attr("hidden", false);
                        ruta = '/MatenimientoPM/FillGridMiselaneo';
                        //$.blockUI({ message: mensajes.PROCESANDO });
                        loadGrid(getFiltrosMis(), ruta, grid_MisAnticongelante);
                        //$.unblockUI();
                    } else {
                        $(this).closest("tr").find("td:eq(3) .txtEdadAceite").attr("disabled", false);
                        id = $(this).attr("data-id")
                        ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = false;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis };
                        VincularMis(obj);
                        //$.unblockUI();
                        ruta = '/MatenimientoPM/FillGridComponenteVin';
                        loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                        ruta = '/MatenimientoPM/FillGridMiselaneo';
                        loadGrid(getFiltrosMis(), ruta, grid_MisAnticongelante);
                        //$.unblockUI();
                    }
                });
                grid.find(".txtEdadAceite").on("change", function (e) {
                    if ($(this).val() != "") {
                        id = $(this).attr("data-index");
                        AsignarEdad(id, $(this).val());
                    }
                });
                grid.find(".txtCantidad").on("change", function (e) {
                    if ($(this).val() != "") {
                        id = $(this).attr("data-index");
                        AsignarCantidad(id, $(this).val());
                    }
                });
            });
        }
        //truco para el redireccionamiento 
        function initGridMisAceite(grid) {
            grid.bootgrid({
                rowCount: [4],
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "check-asignar": function (column, row) {
                        if (row.asignado != null) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        id = 0;
                        if (row.asignado != null) {
                            id = row.asignado.id;
                        }
                        return "<label  class='btn chkfiltro'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'data-id='" + id + "' >" +
                            "<input  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off' " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    },
                    "Componente": function (column, row) {
                        return row.Componente;
                    },
                    "orden": function (column, row) {
                        return row.Componente.id;
                    },
                    "edad": function (column, row) {
                        if (row.asignado != null) {
                            return "<input type='number' data-index='" + row.asignado.id + "' value='" + row.asignado.vida + "' class='form-control bfh-number txtEdadAceite'>";
                        }
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number txtEdadAceite' disabled>";
                    },
                    "cantidad": function (column, row) {
                        if (row.asignado != null) {
                            return "<input type='number' data-index='" + row.cantidad[0].id + "' value='" + row.cantidad[0].cantidad + "' class='form-control bfh-number txtCantidad'>";
                        } else {
                            return "<input type='number'  class='form-control bfh-number txtCantidad' disabled>";
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".chkfiltro").on("click", function (e) {
                    var chkbx = $(this).find("input");
                    if (chkbx.is(":checked")) {
                        $(this).closest("tr").find("td:eq(3) .txtEdadAceite").attr("disabled", false);
                        id = 0,
                            ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = true;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis };
                        VincularMis(obj);
                        ruta = '/MatenimientoPM/FillGridComponenteVin';
                        loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                        $("#TextModalVin").attr("hidden", false);
                        ruta = '/MatenimientoPM/FillGridMiselaneo';
                        //$.blockUI({ message: mensajes.PROCESANDO });
                        loadGrid(getFiltrosMis(), ruta, grid_MiselaneoAceite);
                        //$.unblockUI();
                    } else {
                        $(this).closest("tr").find("td:eq(3) .txtEdadAceite").attr("disabled", false);
                        id = $(this).attr("data-id")
                        ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = false;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis };
                        VincularMis(obj);
                        //$.unblockUI();
                        ruta = '/MatenimientoPM/FillGridComponenteVin';
                        loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                        ruta = '/MatenimientoPM/FillGridMiselaneo';
                        loadGrid(getFiltrosMis(), ruta, grid_MiselaneoAceite);
                    }
                });
                grid.find(".txtEdadAceite").on("change", function (e) {
                    if ($(this).val() != "") {
                        id = $(this).attr("data-index");
                        AsignarEdad(id, $(this).val());
                    }
                });
                grid.find(".txtCantidad").on("change", function (e) {
                    if ($(this).val() != "") {
                        id = $(this).attr("data-index");
                        AsignarCantidad(id, $(this).val());
                    }
                });
            });
        }
        function AsignarCantidad(idMis, cantidad) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/VincularCantidadMis',
                data: { id: id, cantidad: cantidad },
                success: function (response) {
                    if (response.success == true) {
                        //$.unblockUI();
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        };
        function AsignarEdad(idMis, edad) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/VincularEdadMis',
                data: { id: id, edad: edad },
                success: function (response) {
                    if (response.success == true) {
                        //$.unblockUI();
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        };
        function AsignarEdadAct(idMis, edad) {
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/VincularEdadAct',
                data: { id: id, edad: edad },
                success: function (response) {
                    if (response.success == true) {
                        AlertaGeneral('Confirmación', 'El registro se actualizo correctamente');
                    }
                    else {
                        AlertaGeneral('Alerta', 'No Fue posible actualizar la informacion favor de volver a intentarlo');
                    }
                },
                error: function () {
                    AlertaGeneral('Alerta', 'Ocurrio un error inesperado favor de contactar a sistemas.');
                    //$.unblockUI();
                }
            });
        };
        function initGridMisFiltro(grid) {
            grid.bootgrid({
                rowCount: [4],
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "check-asignar": function (column, row) {
                        if (row.asignado != null) {
                            return "Asignado"
                        } else {
                            return "Pendiente"
                        }
                    },
                    "btnAsignarFiltro": function (column, row) {

                        if (row.asignado != null) {
                            id = row.asignado.id;
                            return "<button type='button' class='btn btn-danger setFiltros' data-index='" + row.id + "' data-id='" + id + "' data-estatus='" + false + "' >" +
                                "<span class='glyphicon glyphicon-remove'></span> " +
                                " </button>";
                        }
                        else {

                            return "<button type='button' class='btn btn-success setFiltros' data-index='" + row.id + "' data-id='" + 0 + "' data-estatus='" + true + "' >" +
                                "<span class='glyphicon glyphicon-plus'></span> " +
                                " </button>";
                        }


                    },
                    "marca": function (column, row) {
                        if (row.marca == 1) {
                            row.marca = "Caterpillar"
                        } else if (row.marca == 2) {
                            row.marca = "Donaldson"
                        } else if (row.marca == 3) {
                            row.marca = "Atlas Copco"
                        }
                        return row.marca
                    },
                    "sintetico": function (column, row) {
                        if (row.sintetico == true) {
                            row.sintetico = "Si"
                        } else if (row.sintetico == false) {
                            row.sintetico = "No"
                        }
                        return row.sintetico
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    },
                    "edad": function (column, row) {
                        if (row.asignado != null) {
                            return "<input type='number' data-index='" + row.asignado.id + "' value='" + row.asignado.vida + "' class='form-control bfh-number txtEdadAceiteFiltros'>";
                        }
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number txtEdadAceiteFiltros'>";
                    },
                    "cantidad": function (column, row) {
                        if (row.asignado != null) {
                            return "<input type='number' data-index='" + row.cantidad[0].id + "' value='" + row.cantidad[0].cantidad + "' class='form-control bfh-number txtCantidadFiltros'>";
                        } else {
                            return "<input type='number'  class='form-control bfh-number txtCantidadFiltros'>";
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".setFiltros").on("click", function (e) {
                    var chkbx = $(this).attr('data-estatus') == "false" ? false : true;
                    if (chkbx) {
                        // $(this).closest("tr").find("td:eq(5) .txtEdadAceite").attr("disabled", false);

                        vida = $(this).parents('tr').children().find('.txtEdadAceiteFiltros').val();
                        cantidad = $(this).parents('tr').children().find('.txtCantidadFiltros').val();
                        id = 0,
                            ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = true;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis, vida: vida, cantidad: cantidad };
                        VincularMis(obj);
                    } else {
                        //$(this).closest("tr").find("td:eq(3) .txtEdadAceite").attr("disabled", false);
                        id = $(this).attr("data-id")
                        vida = $(this).parents('tr').children().find('.txtEdadAceiteFiltros').val();
                        cantidad = $(this).parents('tr').children().find('.txtCantidadFiltros').val();
                        ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = false;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis, vida: vida, cantidad: cantidad };
                        VincularMis(obj);
                    }
                });
            });
        }
        var contadorfilt = 0;
        function initGridLubHis(grid) {
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
                            return ("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                        } else if (Patron < 250 && Patron >= 0) {
                            return ("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                        } else if (Patron < 0) {
                            return ("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                        }
                    },
                    "EdadSum": function (column, row) {
                        return row.vidaA;
                    },
                    "AceiteVin": function (column, row) {

                        if (row.AceiteVin != 0) {
                            return row.AceiteVin[0].descripcion[0].nomeclatura;
                        }
                        else
                            return "";

                    },
                    "prueba": function (column, row) {
                        if (row.prueba == true) {
                            return 'SI';
                        } else {
                            return 'NO';
                        }
                    },
                    "ObservacionesLubis": function (column, row) {
                        return ("<button type='button'  style='color:gray;'> <span class='fa fa-comments'></span></button>");
                    },
                    //"ProxLub": function (column, row) {
                    //    return ("<a href='#gridLubProx'><button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-down'></span></button></a>");
                    //},
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
            });
        }
        function initGridLubProy(grid) {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                    footer: ""
                },
                formatters: {
                    "Icon": function (column, row) {
                        return '<i class="' + row.Icon + ' fa-3x"  style="color:black;"></i>'
                    },
                    "componente": function (column, row) {
                        return row.componente[0];
                    },
                    "InputVida": function (column, row) {
                        return "<input type='number'  class='form-control bfh-number InputVida'disabled>";
                    },
                    "VidaRest": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number VidaRest'disabled>";
                    },
                    "HorServ": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number HorServ'disabled>";
                    },
                    "estatus": function (column, row) {
                        return "<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>";
                    },
                    "EdadSum": function (column, row) {

                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number EdadSum'disabled>";
                    },
                    "AceiteVin": function (column, row) {
                        html = '';
                        html += '<div class="SelectSum">';
                        html += '<select class="form-control" id="cboAceiteVin" style="margin-top:4px;" title="Seleccione:">'
                        html += '<option value="0" selected disabled hidden>Tipo De Aceite:</option>'
                        $.each(row.AceiteVin, function (i, e) {
                            if (e.edadSuministro != 0) {
                                html += '<option  id-comp="' + row.idComponente.idCompVis + '"   id-misc="' + e.edadSuministro[0].idMis + '"  data-edad="' + e.edadSuministro[0].vida + '"  value="' + e.descripcion[0].id + '">' + e.descripcion[0].nomeclatura + '</option>'
                            }
                        });
                        html += '</select>'
                        html += '</div>';
                        return html;
                    }, "chkPrueba": function (column, row) {
                        if (row.asignado != null) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        id = 0;
                        if (row.asignado != null) {
                            id = row.asignado.id;
                        }
                        return "<label  class='btn chkPrueba'   data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'data-id='" + id + "' >" +
                            "<input type='checkbox'   class='form-control ' style='margin-left:20px;' " + checked + ">" +
                            " </label>"
                            ;

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
                        return "<button type='button' class='chkAplico' disabled style='color:gray;'> <span class='fa fa-comments'></span></button>";
                    },
                    "ProxServ": function (column, row) {
                        return "<input type='number'  class='form-control bfh-number InputProx'disabled>";
                    },
                    "HisLub": function (column, row) {
                        return ("<a href='#gridLubhis'><button type='button' class='HisLub' style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-up'></span></button></a>");
                    },
                    "estatus": function (column, row) {
                    },
                    "Aplicar": function (column, row) {
                        return "<input type='checkbox'  class='form-control aplicar' style='margin-left:20px;' >";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".chkfiltro").on("click", function (e) {
                    var chkbx = $(this).find("input");

                    if (chkbx.is(":checked")) {
                        $(this).closest("tr").find("td:eq(4) .txtEdadAceite").attr("disabled", false);
                        id = 0,
                            ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = true;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis };
                        VincularMis(obj);
                        ruta = '/MatenimientoPM/FillGridComponenteVin';
                        loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                        $("#TextModalVin").attr("hidden", false);
                        ruta = '/MatenimientoPM/FillGridMiselaneo';
                        //$.blockUI({ message: mensajes.PROCESANDO });
                        loadGrid(getFiltrosMis(), ruta, grid_MiselaneoFiltro);
                    } else {
                        $(this).closest("tr").find("td:eq(2) .txtEdadAceite").attr("disabled", false);
                        id = $(this).attr("data-id")
                        ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = false;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis };
                        VincularMis(obj);
                        ruta = '/MatenimientoPM/FillGridComponenteVin';
                        loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                        ruta = '/MatenimientoPM/FillGridMiselaneo';
                        loadGrid(getFiltrosMis(), ruta, grid_MiselaneoFiltro);
                    }
                });
                //HisLub
                grid.find(".HisLub").on("click", function (e) {
                    var etop = $('.seccionLubHis').offset().top;
                    $(window).scrollTop(etop);
                });
                grid.find(".chkPrueba").on("click", function (e) {
                    var chkbx = $(this).find("input");
                    if (chkbx.is(":checked")) {
                        $("#btnguardarLubObs").attr("TipoPrueba", true);
                        $(this).closest("tr").find("td:eq(3) .InputVida").attr("disabled", false);
                        if ("" != $(this).closest("tr").find("td:eq(3) .InputVida").val()) {
                            $(this).closest("tr").find("td:eq(4) .HorServ").attr("disabled", false);
                            $(this).closest("tr").find("td:eq(3) .InputVida").val("");
                        }
                    } else {
                        $("#btnguardarLubObs").attr("TipoPrueba", false);
                        $(this).closest("tr").find("td:eq(3) .InputVida").attr("disabled", true);
                        $(this).closest("tr").find("td:eq(3) .InputVida").val("");

                        varSelector = $(this).closest("tr").find('#cboAceiteVin');
                        edadAnterior = $(varSelector).find('option:selected').attr('data-edad');

                        $(this).closest("tr").find("td:eq(3) .InputVida").val(edadAnterior);
                        //edadIn = $(renglon).closest('tr').find('td:eq(1)  #cboAceiteVin option:selected').attr('data-edad');
                        HorometroActual = $("#btnHorometro").html();
                        HorometroServicio = $(this).closest("tr").find("td:eq(4) .HorServ").val();
                        HorasTranscurridas = HorometroActual - HorometroServicio;
                        $(this).closest("tr").find("td:eq(5) .EdadSum ").val(HorasTranscurridas.toFixed(2));
                        Patron = $(this).closest("tr").find("td:eq(3) .InputVida").val() - $(this).closest("tr").find("td:eq(6) .EdadSum ").val();
                        $(this).closest("tr").find("td:eq(6) .VidaRest").val(Patron.toFixed(2));
                        if (Patron >= 250) {
                            $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                        } else if (Patron < 250 && Patron >= 0) {
                            $(this).closest("tr").find("td:eq(7)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                        } else if (Patron < 0) {
                            $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                        }
                    }
                });
                grid.find(".chkAplico").on("click", function (e) {
                    $("#btnguardarLubObs").attr('idobjProy', $(this).attr('id'))
                    TipoPrueba = false;
                    prueba = $(this).closest("tr").find("td:eq(4)  input");
                    $("#btnguardarLubObs").attr("vigencia", $(this).closest("tr").find("td:eq(3) input").val());
                    $("#lblControlLub").attr("idcomp", $(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").attr('id-comp'));
                    $("#lblControlLub").attr("idmis", $(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").attr('id-misc'));
                    $(".NoEcoServTent").val($(".txtNoEconomicoAltS").val());
                    $("#LubServTent").val($(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").text());
                    $("#CompServTent").val($(this).closest("tr").find("td:eq(0)").text());
                    $("#LubServTent").val();
                    modalIndicacionesCOnt.modal("show");
                });
                grid.find(".InputVida").on("change", function (e) {
                    HorometroActual = $("#btnHorometro").html();
                    HorometroServicio = $(this).closest("tr").find("td:eq(4) .HorServ").val();
                    HorasTranscurridas = HorometroActual - HorometroServicio;
                    $(this).closest("tr").find("td:eq(5) .EdadSum ").val(HorasTranscurridas.toFixed(2));
                    Patron = $(this).closest("tr").find("td:eq(3) .InputVida").val() - $(this).closest("tr").find("td:eq(5) .EdadSum ").val();
                    $(this).closest("tr").find("td:eq(6) .VidaRest").val(Patron.toFixed(2));
                    if (Patron >= 250) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                    } else if (Patron < 250 && Patron >= 0) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                    } else if (Patron < 0) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                    }
                    Proyhr = (parseInt($(this).closest("tr").find("td:eq(3) .InputVida").val()) + parseInt($(".txtHorometroLubricantes").val()));
                    $(this).closest("tr").find("td:eq(4) .InputProx").val(Proyhr.toFixed(2));
                });
                grid.find(".SelectSum").on("change", function (e) {

                    $(this).closest("tr").find("td:eq(8) input").attr("disabled", false);

                    $(this).closest("tr").find("td:eq(2) label").attr("disabled", false);
                    $(this).closest("tr").find("td:eq(3) .InputVida").val($(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").attr('data-edad'));
                    idmisc = ($(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").attr("id-misc"))
                    $(this).closest("tr").find("td:eq(3) .InputVida").attr('idmisc', idmisc);
                    Proyhr = (parseInt($(this).closest("tr").find("td:eq(3) .InputVida").val()) + parseInt($(".txtHorometroLubricantes").val()));

                });
                grid.find(".HorServ").on("change", function (e) {
                    HorometroActual = $("#btnHorometro").html();
                    HorometroServicio = $(this).closest("tr").find("td:eq(4) .HorServ").val();
                    HorasTranscurridas = HorometroActual - HorometroServicio;
                    $(this).closest("tr").find("td:eq(5) .EdadSum ").val(HorasTranscurridas.toFixed(2));
                    Patron = $(this).closest("tr").find("td:eq(3) .InputVida").val() - $(this).closest("tr").find("td:eq(5) .EdadSum ").val();
                    $(this).closest("tr").find("td:eq(6) .VidaRest").val(Patron.toFixed(2));
                    var a = $(this).closest("tr").find("td:eq(7)").html("");
                    if (Patron >= 250) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                    } else if (Patron < 250 && Patron >= 0) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                    } else if (Patron < 0) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                    }
                });
                grid.find(".aplicar").on("click", function (e) {
                    Checked = $(this).closest('tr').find('td:eq(7) input.aplicar');
                    Aplicado = true;
                    prueba = false;
                    if (Checked.is(":checked")) {
                        var chkbx = $(this).closest("tr").find("td:eq(2) input");
                        if (chkbx.is(":checked")) {
                            prueba = true;
                        } else {
                            prueba = false;
                        }
                        $(this).closest('tr').find('td:eq(4) .chkAplico').attr('disabled', false);
                        objLubBitProy = {
                            Vigencia: $(this).closest("tr").find("td:eq(3) input").val(),
                            idComp: $(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").attr('id-comp'),
                            idMisc: $(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").attr('id-misc'),
                            prueba: prueba,
                            programado: true,
                            idMant: $("#MantenimientoSeguimiento").attr("idMant"),
                            Observaciones: "",
                            FechaServicio: $("#fechaServProxLub").val(),
                            estatus: true
                        };
                        //$.blockUI({ message: mensajes.PROCESANDO });
                        $.ajax({
                            //async: true,
                            datatype: "json",
                            type: "POST",
                            url: '/MatenimientoPM/GuardarBitProyLub',
                            data: { ObjBitProyLub: objLubBitProy },
                            success: function (response) {
                                //$.unblockUI();
                                if (response.success == true) {
                                    ConsultaModelobyMantenimiento(response.ObjBitProyLub.idMant);
                                    ConsultagridLubProx(ModProy);
                                    //$.unblockUI();
                                }
                            },
                            error: function () {
                                //$.unblockUI();
                            }
                        });
                    } else {
                        $(this).closest('tr').find('td:eq(4) .chkAplico').attr('disabled', true);
                        //cambiar estatus al registro
                        $.ajax({
                            //async: true,
                            datatype: "json",
                            type: "POST",
                            url: '/MatenimientoPM/DeshabilitarLubProy',
                            data: { id: $(this).attr("id") },
                            success: function (response) {
                                //$.unblockUI();
                                ConsultagridLubProx(response.obj);
                            },
                            error: function () {
                                //$.unblockUI();
                            }
                        });
                    };
                });
                // ProgressBarLub();

            });
            CargaDeProyectado();
        }
        function ConsultaModelobyMantenimiento(id) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultaModelobyMantenimiento',
                data: { id: id },
                success: function (response) {
                    //$.unblockUI();
                    ModProy = response.obj;
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function GuardadoObjProyLub(id) {
            if ($("#btnguardarLubObs").attr("vigencia") == "") {
                vigencia = 0;
            } else {
                vigencia = $("#btnguardarLubObs").attr("vigencia");
            }
            var ProgramadoChk = $("#cboProgramado option:selected").val();
            if (ProgramadoChk == 1) {
                BanderaProg = true;
            } else {
                BanderaProg = false;
            }


            objLubBitProy = { id: id, Hrsaplico: $("#horomatroNoProg").val(), Vigencia: vigencia, idComp: $("#lblControlLub").attr("idcomp"), idMisc: $("#lblControlLub").attr("idmis"), prueba: $("#btnguardarLubObs").attr("TipoPrueba"), programado: BanderaProg, idMant: $("#MantenimientoSeguimiento").attr("idMant"), Observaciones: $("#ObservacionesMantLub").val(), FechaServicio: $("#fechaServProxLub").val(), estatus: true };
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/GuardarBitProyLub',
                data: { ObjBitProyLub: objLubBitProy },
                success: function (response) {
                    //$.unblockUI();
                    if (response.success == true) {
                        $("#modalIndicacionesCOnt").modal("hide");
                        ConfirmacionGeneralFC("Confirmación", "Info. Guardada", "bg-green");
                    }
                    ProgressBarLub();
                    CargaDeProyectado();
                },
                error: function () {
                    //$.unblockUI();
                }
            });
            //ProgressBarLub();
            //CargaDeProyectado();
        }
        function saveProyectados(idProyectado) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/GuardarBitProyLub2',
                data: { id: idProyectado, Comentario: $("#ObservacionesMantLub").val() },
                success: function (response) {
                    //$.unblockUI();
                    if (response.success == true) {
                        $("#modalIndicacionesCOnt").modal("hide");
                        ConfirmacionGeneralFC("Confirmación", "Info. Guardada", "bg-green");
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }


        function CargaDeProyectado() {
            if (idmantenimiento != "#idmantenimiento") {
                //$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/CargaDeProyectado',
                    data: { idmantenimiento: idmantenimiento },
                    success: function (response) {
                        //$.unblockUI();
                        if (response.items.length != 0) {
                            response.items.forEach(function (valor, indice, array) {
                                var longitud = $("#gridLubProx >tbody >tr").length
                                for (var i = 0; i <= (longitud - 1); i++) {
                                    renglon = $("#gridLubProx >tbody >tr")[i]
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
                        //$.unblockUI();
                    }
                });
            }
        };
        function ProgressBarDN() {
            arrVidasDNProx = [];
            var LongDN = $("#gridDNHis tbody tr");
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
            var Long = $("#gridDNProx tbody tr");
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
        function ProgressBarActProx() {
            arrVidasAct = [];
            var LongAct = $("#gridActhis tbody tr");
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
            var Long = $("#gridActProx tbody tr");
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
        function ProgressBarLub() {
            arrVidas = [];
            var Long = $("#gridLubhis tbody tr")
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
            var Long = $("#gridLubProx tbody tr")
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
        function ConsultGridDNProy(modeloID) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarActividadesDN',
                data: { modeloEquipoID: modeloID },
                success: function (response) {
                    if (response.success == true && response.Actividad != "") {

                        $("#gridDNProx").bootgrid("clear");
                        $("#gridDNProx").bootgrid("append", response.Actividad);
                        //$.unblockUI();
                    } else {
                        LimpiarFormularios();
                        AlertaModal("Error", "No existe Informacion de DN'S Relacionados al Modelo   '" + modeloEquipoID + "'");
                        //modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ConsultagridAEProx(modeloID) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarActividadesExtras',
                data: { modeloEquipoID: modeloID },
                success: function (response) {
                    if (response.success == true && response.Actividad != "") {

                        $("#gridActProx").bootgrid("clear");
                        $("#gridActProx").bootgrid("append", response.Actividad);
                        //$.unblockUI();
                    } else {
                        LimpiarFormularios();
                        AlertaModal("Error", "No existe Informacion de Actividades Extras Relacionados al Modelo   '" + modeloEquipoID + "'");
                        //modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ConsultagridLubProx(modeloID) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarJGEstructura',
                data: { modeloEquipoID: modeloID },
                success: function (response) {
                    if (response.success == true) {

                        $("#gridLubProx").bootgrid("clear");
                        $("#gridLubProx").bootgrid("append", response.Actividad);
                        //$.unblockUI();
                    } else {
                        //$.unblockUI();
                        if ($("#cboFiltroNoEconomico  option:selected").text() != "--Seleccione--") {
                            LimpiarFormularios();
                            AlertaModal("Error", "No existe Informacion de Lubricantes  Relacionados al Modelo'" + modeloEquipoID + "'");
                            //modaltablaPM.modal("hide");
                        }
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        /*matus*/
        function saveAll() {
            arrayDataLubricantes = new Array();
            arrayDataActividadesExtra = new Array();
            arrayDataDN = new Array();
            arrayDataDet = new Array();

            gridFiltrosComponentes.data().each(function (value, index) {
                var dataItem = {};

                dataItem.id = 0;
                dataItem.idAct = 0;
                dataItem.modeloEquipoID = value.modeloEquipoID;
                dataItem.idCompVis = value.idCompVis;
                dataItem.programado = value.programado;
                dataItem.cantidad = value.cantidad;
                dataItem.modelo = value.modelo;
                dataItem.idFiltro = value.idFiltro;
                dataItem.tipoPMid = 0;

                arrayDataDet.push(dataItem);
            });

            tblGridLubProxTbl.data().each(function (value, index) {
                proyectado = value.proyectado;


                if (proyectado != null) {
                    arrayDataLubricantes.push(value.proyectado);
                }
                else {
                    objProyectado = {};

                    objProyectado.id = 0;
                    objProyectado.Hrsaplico = value.objHis.hrsAplico;
                    objProyectado.Vigencia = value.objHis.vidaA;
                    objProyectado.idComp = value.objHis.idComp;

                    objProyectado.idMisc = value.objHis.AceiteVin[0].suministroID;
                    objProyectado.prueba = value.objHis.prueba;
                    objProyectado.programado = false;
                    objProyectado.idMant = value.idmantenimiento;
                    objProyectado.Observaciones = "";
                    objProyectado.FechaServicio = d;
                    objProyectado.UsuarioCap = 0;
                    objProyectado.fechaCaptura = d;
                    objProyectado.estatus = false;
                    objProyectado.aplicado = false;
                    objProyectado.idAct = value.objComponente.idAct;

                    arrayDataLubricantes.push(objProyectado);
                }

            });

            tblGridActProxTbl.data().each(function (value, index) {
                proyectado = value.proyectado;
                if (proyectado != null) {
                    arrayDataActividadesExtra.push(value.proyectado);
                }
            });
            tblgridDNProxTbl.data().each(function (value, index) {
                proyectado = value.proyectado;
                if (proyectado != null) {
                    arrayDataDN.push(value.proyectado);
                }
            });
            // DownloadArchivos();
            sendInfoSave(idPlaneador, idResponsable, arrayDataLubricantes, arrayDataActividadesExtra, arrayDataDN, 2, arrayDataDet);
        }

        function downloadURI(elementos) {
            var link = document.createElement("button");
            link.download = '/matenimientopm/getDocumentos?id=' + elementos;
            link.href = '/matenimientopm/getDocumentos?id=' + elementos;
            link.click();
            location.href = '/matenimientopm/getDocumentos?id=' + elementos;
        }

        function DownloadArchivos() {
            var ids = "";
            for (var i = 0; i < GridFormatosTbl.data().length; i++) {
                if (GridFormatosTbl.data()[i].aplica) { ids += GridFormatosTbl.data()[i].idformato.id.toString() + ","; }
            }
            if (ids != "") {
                ids = ids.substring(0, ids.length - 1);
                downloadURI(ids);
            }
        }

        function saveParcialPrograma() {

            arrayDataLubricantes = new Array();
            arrayDataActividadesExtra = new Array();
            arrayDataDN = new Array();
            arrayDataDet = new Array();

            gridFiltrosComponentes.data().each(function (value, index) {
                var dataItem = {};

                dataItem.id = 0;
                dataItem.idAct = value.idAct;
                dataItem.modeloEquipoID = value.modeloEquipoID;
                dataItem.idCompVis = value.idCompVis;
                dataItem.programado = value.programado;
                dataItem.cantidad = value.cantidad;
                dataItem.modelo = value.modelo;
                dataItem.idFiltro = value.idFiltro;
                dataItem.tipoPMid = 0;

                arrayDataDet.push(dataItem);
            });
            tblGridLubProxTbl.data().each(function (value, index) {
                proyectado = value.proyectado;
                if (proyectado != null) {
                    arrayDataLubricantes.push(value.proyectado);
                }
                else {
                    objProyectado = {};

                    objProyectado.id = 0;
                    objProyectado.Hrsaplico = value.objHis.hrsAplico;
                    objProyectado.Vigencia = value.objHis.vidaA;
                    objProyectado.idComp = value.objHis.idComp;

                    objProyectado.idMisc = value.objHis.AceiteVin[0].suministroID;
                    objProyectado.prueba = value.objHis.prueba;
                    objProyectado.programado = false;
                    objProyectado.idMant = value.idmantenimiento;
                    objProyectado.Observaciones = "";
                    objProyectado.FechaServicio = d;
                    objProyectado.UsuarioCap = 0;
                    objProyectado.fechaCaptura = d;
                    objProyectado.estatus = false;
                    objProyectado.aplicado = false;
                    objProyectado.idAct = value.componente.objComponente.idAct;

                    arrayDataLubricantes.push(objProyectado);
                }

            });
            tblGridActProxTbl.data().each(function (value, index) {
                proyectado = value.proyectado;
                if (proyectado != null) {
                    arrayDataActividadesExtra.push(value.proyectado);
                }

            });
            tblgridDNProxTbl.data().each(function (value, index) {
                proyectado = value.proyectado;
                if (proyectado != null) {
                    arrayDataDN.push(value.proyectado);
                }
            });

            sendInfoSave(idPlaneador, idResponsable, arrayDataLubricantes, arrayDataActividadesExtra, arrayDataDN, 1, arrayDataDet);
        }

        function sendInfoSave(planeador, responsable, tblGridLubProxTbl, tblGridActProxTbl, tblgridDNProxTbl, tipoGuardardo, arrayDataDet) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            const archivoAdjunto = archivoPM.get(0).files[0];
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/sendInfoSave',
                data: { idMantenimiento: $("#MantenimientoSeguimiento").attr('idmant'), planeador: planeador, responsable: responsable, tblGridLubProxTbl: tblGridLubProxTbl, tblGridActProxTbl: tblGridActProxTbl, tblgridDNProxTbl: tblgridDNProxTbl, tipoGuardardo: tipoGuardardo, tlbDetACtividades: arrayDataDet },
                success: function (response) {
                    //$.unblockUI();
                    if (response.success) {
                        if (archivoAdjunto != "") {
                            fncGuardarDocPM();
                        }

                        AlertaModal("Confirmacion", "Se Guardo Correctamente");
                        //$.unblockUI();
                        if (tipoGuardardo != 1) {
                            DownloadArchivos();
                        }
                        ConsultaPm(0);

                    } else {
                        AlertaModal("Alerta", "Ocurrio un error en el guardado favor de llamar al area de sistemas.");
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        function fncGuardarDocPM() {
            let objDocumentoPM = fncObjGuardarDocPM();
            axios.post("GuardarDocumentoPM", objDocumentoPM, { headers: { 'Content-Type': 'multipart/form-data' } }).then(response => {
                let { success, items, message } = response.data;
            }).catch(error => Alert2Error(error.message));
        }

        function descargarArchivo(idArchivo) {
            if (idArchivo > 0) {
                location.href = `DescargarArchivo?idArchivo=${idArchivo}`;
            }
        }
        function GetArchivosAdjuntos(idmantenimiento) {
            var idArchivo = parseInt(idmantenimiento);
            axios.post("GetArchivosAdjuntos", { idArchivo: idArchivo }).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    archivoPMDescargar.show();
                } else {
                    archivoPMDescargar.hide();
                    // Alert2Error(message)
                }
            }).catch(error => Alert2Error(error.message));
        }
        function fncObjGuardarDocPM() {
            const archivoAdjunto = archivoPM.get(0).files[0];
            let objDocumentoPM = new Object();
            objDocumentoPM = {
                idMantenimiento: $("#MantenimientoSeguimiento").attr('idmant')
            }

            let formData = new FormData();
            formData.set('objFile', archivoAdjunto);
            formData.set('objDocumentoPM', JSON.stringify(objDocumentoPM));
            return formData;
        }

        function getActividadesDNs(modeloID, idMantenimiento) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/getActividadesDNS',
                data: { modeloEquipoID: modeloID, idMantenimiento: idMantenimiento },
                success: function (response) {
                    if (response.success == true) {
                        initGridDNhis($("#gridDNHis"));
                        $("#gridDNHis").bootgrid("clear");
                        $("#gridDNHis").bootgrid("append", response.objActividadesDNHis);

                        setTableGridDNs(response.objActividadesDN);
                        //$.unblockUI();
                    } else {
                        LimpiarFormularios();
                        AlertaModal("Error", "No existe Informacion de DN'S Relacionados al Modelo   '" + modeloID + "'");
                        //modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        function getActividadesExtras(modeloID, maquinariaID) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/getActividadesExtras',
                data: { modeloEquipoID: modeloID, idMantenimiento: maquinariaID },
                success: function (response) {
                    if (response.success == true && response.objActividadesExtras != "") {
                        initGridActHis($("#gridActhis"));
                        $("#gridActhis").bootgrid("clear");
                        $("#gridActhis").bootgrid("append", response.objActividadesExtrashis);

                        setTableGridActividadesExtra(response.objActividadesExtras)

                        //$.unblockUI();
                    } else {
                        LimpiarFormularios();
                        AlertaModal("Error", "No existe Informacion de Actividades Extras Relacionados al Modelo   '" + modeloID + "'");
                        //modaltablaPM.modal("hide");
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });

        }

        function getActividadesLubricantes(modeloID, idmantenimiento) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarJGEstructuraLubricantes',
                data: { modeloEquipoID: modeloID, idmantenimiento: idmantenimiento },
                async: false,
                success: function (response) {
                    setTableTlbGridLub(response.dataSetGridLubProx); //Carga de tabla de Datatable.

                    initGridLubHis($("#gridLubhis")); //Carga inicializa bootgrid.
                    $("#gridLubhis").bootgrid("clear");
                    $("#gridLubhis").bootgrid("append", response.JGHis);
                    //$.unblockUI();

                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        /////Renderizado de Tablas.

        ///Tabla de Lubricantes Paso 1
        function setTableTlbGridLub(dataSet) {
            tblGridLubProxTbl = $("#tblGridLubProx").DataTable({
                language: lstDicEs,
                destroy: true,
                scrollY: false,
                paging: false,
                ordering: false,
                info: false,
                data: dataSet,
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
                            id = 0;
                            if (rowData.proyectado != null) {
                                id = rowData.proyectado.id;
                            }


                            html = "<button type='button' class='chkAplico'  data-idPro ='" + id + "' data-componente='" + rowData.componente + "' style='color:gray;'> <span class='fa fa-comments'></span></button>";
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
                                if (rowData.proyectado.Vigencia == 0) { vidaUtil = parseFloat($('option:selected', $(td).parents('tr').find('.cboAceiteVin')).attr("data-edad")); }
                                else { vidaUtil = rowData.proyectado.Vigencia; }
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
                                "<input type='checkbox' data-componenteIndex='" + rowData.objComponente.idCompVis + "'   class='form-control programarLubricantes' style='margin-left:20px;' " + checked + ">" +
                                " </label>";

                            $(td).append(html);
                        }
                    },

                ],
                "createdRow": function (row, data, index) {

                    objProyectado = data.proyectado;
                    setCboAceites = $(row).find('.cboAceiteVin');
                    setIsPrueba = $(row).find('.checkedTipoPrueba');
                    setInputVida = $(row).find('.InputVida');
                    setIsProgramado = $(row).find('.programarLubricantes');

                    if (objProyectado != null) {
                        $(setCboAceites).val(objProyectado.idMisc);

                        $(setIsPrueba).prop("checked", objProyectado.prueba);
                        if (objProyectado.Vigencia == 0) { $(setInputVida).val(parseFloat($('option:selected', $(row).find('.cboAceiteVin')).attr("data-edad"))); }
                        else { $(setInputVida).val(objProyectado.Vigencia); }
                        $(setIsProgramado).prop("checked", objProyectado.programado);
                        if (objProyectado.prueba)
                            $(setInputVida).attr('disabled', false)
                        else
                            $(setInputVida).attr('disabled', true)
                    }
                    else {
                        objHist = data.objHis;
                        objHistAceites = objHist.AceiteVin != 0 ? objHist.AceiteVin[0].edadSuministro[0] : 0;
                        $(setCboAceites).val(objHistAceites.idMis);
                        $(setIsPrueba).prop("checked", objHist.prueba);
                        if (objHist.vidaA == 0) { $(setInputVida).val(parseFloat($('option:selected', $(row).find('.cboAceiteVin')).attr("data-edad"))); }
                        else { $(setInputVida).val(objHist.vidaA); }
                        $(setIsProgramado).prop("checked", false);
                        if (objHist.prueba)
                            $(setInputVida).attr('disabled', false)
                        else
                            $(setInputVida).attr('disabled', true)
                    }
                },
                initComplete: function (settings, json) {
                    tblGridLubProx.on('change', '.InputVida', function () {
                        componenteId = $('option:selected', $(this).parents('tr').find('.cboAceiteVin')).attr('id-comp');
                        suministroId = $('option:selected', $(this).parents('tr').find('.cboAceiteVin')).attr('id-misc');
                        vidaUtil = $(this).parents('tr').find('.InputVida').val();
                        prueba = $(this).parents('tr').find('.checkedTipoPrueba').is(':checked');
                        programado = $(this).parents('tr').find('.programarLubricantes').is(':checked');
                        $('.chkAplico').trigger('change');
                        index = setProgramado(componenteId, suministroId, programado, prueba, vidaUtil);
                        setTableTlbGridLub(index);

                    });

                    tblGridLubProx.on('change', '.programarLubricantes', function () {
                        componenteId = $('option:selected', $(this).parents('tr').find('.cboAceiteVin')).attr('id-comp');
                        suministroId = $('option:selected', $(this).parents('tr').find('.cboAceiteVin')).attr('id-misc');
                        vidaUtil = $(this).parents('tr').find('.InputVida').val();

                        prueba = $(this).parents('tr').find('.checkedTipoPrueba').is(':checked');
                        programado = $(this).is(':checked');

                        index = setProgramado(componenteId, suministroId, programado, prueba, vidaUtil);
                        //   setTableTlbGridLub(index);
                    });

                    tblGridLubProx.on('change', '.cboAceiteVin', function () {
                        vidaUtil = $(this).find('option:selected').attr('data-edad');
                        componenteId = $('option:selected', this).attr('id-comp');
                        suministroId = $('option:selected', this).attr('id-misc');
                        prueba = $(this).parents('tr').find('.checkedTipoPrueba').is(':checked');
                        programado = $(this).parents('tr').find('.programarLubricantes').is(':checked');


                        index = setProgramado(componenteId, suministroId, programado, prueba, vidaUtil);
                        setTableTlbGridLub(index);


                    });

                    tblGridLubProx.on('change', '.checkedTipoPrueba', function () {
                        componenteId = $('option:selected', $(this).parents('tr').find('.cboAceiteVin')).attr('id-comp');
                        suministroId = $('option:selected', $(this).parents('tr').find('.cboAceiteVin')).attr('id-misc');
                        prueba = $(this).is(':checked');
                        vidaUtil = $(this).parents('tr').find('.InputVida').attr('data-edad');
                        $(this).parents('tr').find('.InputVida').prop('disabled', prueba ? false : true);
                        programado = $(this).parents('tr').find('.programarLubricantes').is(':checked');


                        index = setProgramado(componenteId, suministroId, programado, prueba, vidaUtil);
                        //  setTableTlbGridLub(index);
                    });

                    tblGridLubProx.on('click', '.chkAplico', function () {

                        $("#CompServTent").val($(this).attr('data-componente'));
                        $("#LubServTent").val($('option:selected', $(this).parents('tr').find('.cboAceiteVin')).text());
                        $("#btnguardarLubObs").attr('idobjProy', $(this).attr('data-idpro'));
                        modalIndicacionesCOnt.modal("show");
                    })
                    //tblGridLubProx.draw();//.draw();
                },
            });
            tblGridLubProxTbl.draw();

        }

        function getFormatosActividades(dataSet) {
            var Tamaño = ($(window).width() * 53) / 1366;
            tamañoY = Tamaño + 'vh';
            GridFormatosTbl = $("#tblGridFormatos").DataTable({
                language: lstDicEs,
                "bFilter": false,
                destroy: true,
                scrollY: tamañoY,
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                columns: [
                    {
                        data: "descripcion", width: '120px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).text(cellData);
                        }
                    },
                    {
                        data: "idformato", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            $(td).text(cellData.nombreArchivo);
                        }
                    },
                    {
                        data: "id", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            html = '';
                            $(td).text('');
                            html = "<label  class='btn chkPrueba' >" +
                                "<input type='checkbox'   class='form-control programarActividad' style='margin-left:20px;' data-id=" + cellData + ">" +
                                " </label>";
                            $(td).append(html);
                        }
                    }
                ],
                "paging": true,
                "info": false,
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
                    $("#tblGridFormatos").on('change', '.programarActividad', function () {
                        var id = $(this).attr('data-id');
                        var flag = $(this).is(':checked');


                        for (var i = 0; i < GridFormatosTbl.data().length; i++) {

                            if (GridFormatosTbl.data()[i].id == id) {
                                GridFormatosTbl.data()[i].aplica = flag;
                            }
                        }

                    });

                }
            });
            GridFormatosTbl.draw();

        }

        ///Tabla de Actividades Extra Paso 1
        function setTableGridActividadesExtra(dataSet) {
            tblGridActProxTbl = $("#tblGridActProx").DataTable({
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
                            id = 0;
                            if (rowData.proyectado != null) {
                                id = rowData.proyectado.id;
                            }

                            html = "<button type='button' class='chkAplico' data-id='" + id + "' data-Actividad = '" + rowData.actividad + "' style='color:gray;'> <span class='fa fa-comments'></span></button>";

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
                                "<input type='checkbox'   class='form-control programarActividad' style='margin-left:20px;' " + checked + " data-id='" + rowData.id + "' data-Actividad ='" + rowData.actividad + "'>" +
                                " </label>";

                            $(td).append(html);
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
                "paging": true,
                "info": false,
                initComplete: function (settings, json) {
                    $("#tblGridActProx").on('change', '.programarActividad', function () {
                        var id = $(this).attr('data-id');
                        var flag = $(this).is(':checked');


                        for (var i = 0; i < tblGridActProxTbl.data().length; i++) {

                            if (tblGridActProxTbl.data()[i].id == id) {
                                proyectado = tblGridActProxTbl.data()[i].proyectado;
                                if (proyectado != null) {


                                    tblGridActProxTbl.data()[i].proyectado.programado = flag;

                                    if (proyectado.id == 0) {
                                        var obj = {};
                                        obj.Hrsaplico = $("#horometroMaquinaS").val();
                                        obj.idAct = tblGridActProxTbl.data()[i].idAct;
                                        obj.Vigencia = tblGridActProxTbl.data()[i].perioricidad;
                                        obj.programado = flag;
                                        obj.idMant = tblGridActProxTbl.data()[i].idMant;
                                        obj.Observaciones = tblGridActProxTbl.data()[i].Observaciones;

                                        obj.FechaServicio = $("#fechaIniManteS").val();
                                        obj.UsuarioCap = tblGridActProxTbl.data()[i].UsuarioCap;
                                        obj.fechaCaptura = tblGridActProxTbl.data()[i].fechaCaptura;
                                        obj.aplicado = false;
                                        obj.estatus = true;
                                        tblGridActProxTbl.data()[i].proyectado = obj;
                                    }

                                }
                                else {
                                    var obj = {};

                                    obj.id = 0;
                                    obj.Hrsaplico = $("#horometroMaquinaS").val();
                                    obj.idAct = tblGridActProxTbl.data()[i].idAct;
                                    obj.Vigencia = tblGridActProxTbl.data()[i].perioricidad;
                                    obj.programado = flag;
                                    obj.idMant = tblGridActProxTbl.data()[i].idMant;
                                    obj.Observaciones = tblGridActProxTbl.data()[i].Observaciones;

                                    obj.FechaServicio = $("#fechaIniManteS").val();
                                    obj.UsuarioCap = tblGridActProxTbl.data()[i].UsuarioCap;
                                    obj.fechaCaptura = tblGridActProxTbl.data()[i].fechaCaptura;
                                    obj.aplicado = false;
                                    obj.estatus = true;
                                    tblGridActProxTbl.data()[i].proyectado = obj;

                                }
                            }
                        }

                    });
                    $("#tblGridActProx").on('click', '.chkAplico', function () {
                        var id = $(this).attr('data-id');
                        if (id != 0) {
                            $("#ObservacionesMantAct").val('');
                            $("#ActividadServTent").val($(this).attr('data-Actividad'));
                            $("#btnguardarAEObs").attr("idobj", id);
                            $("#modalActividadesCOnt").modal("show");
                        }


                    });
                }
            });
            tblGridActProxTbl.draw();

        }

        ///Tabla ed Actividades de DNS Paso 2
        function setTableGridDNs(dataSet) {
            tblgridDNProxTbl = $("#tblgridDNProx").DataTable({
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
                            html = "<button type='button' class='chkAplico'  style='color:gray;'> <span class='fa fa-comments'></span></button>";
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
                    },
                    {
                        data: "programar", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            checked = "";
                            html = '';
                            html = "<label  class='btn chkPrueba' >" +
                                "<input type='checkbox'   class='form-control programarActividad' style='margin-left:20px;' " + checked + " data-id ='" + rowData.id + "'>" +
                                " </label>";
                            $(td).append(html);
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


                "paging": true,
                "info": false,
                initComplete: function (settings, json) {
                    $("#tblgridDNProx").on('change', '.programarActividad', function () {
                        var id = $(this).attr('data-id');
                        var flag = $(this).is(':checked');

                        for (var i = 0; i < tblgridDNProxTbl.data().length; i++) {

                            if (tblgridDNProxTbl.data()[i].id == id) {
                                proyectado = tblgridDNProxTbl.data()[i].proyectado;
                                if (proyectado != null) {

                                    tblgridDNProxTbl.data()[i].proyectado.Hrsaplico = $("#horometroMaquinaS").val();
                                    tblgridDNProxTbl.data()[i].proyectado.idAct = tblgridDNProxTbl.data()[i].idAct;
                                    tblgridDNProxTbl.data()[i].proyectado.Vigencia = tblgridDNProxTbl.data()[i].vidaUtil;
                                    tblgridDNProxTbl.data()[i].proyectado.programado = flag;  //tblgridDNProxTbl.data()[i].idAct;
                                    tblgridDNProxTbl.data()[i].proyectado.idMant = tblgridDNProxTbl.data()[i].idMant;
                                    tblgridDNProxTbl.data()[i].proyectado.Observaciones = tblgridDNProxTbl.data()[i].Observaciones;
                                    tblgridDNProxTbl.data()[i].proyectado.estatus = true;
                                    tblgridDNProxTbl.data()[i].FechaServicio = $("#fechaIniManteS").val();
                                    tblgridDNProxTbl.data()[i].UsuarioCap = tblgridDNProxTbl.data()[i].UsuarioCap;
                                    tblgridDNProxTbl.data()[i].aplicado = false;
                                }
                                else {
                                    var obj = {};

                                    obj.id = 0;
                                    obj.Hrsaplico = $("#horometroMaquinaS").val();
                                    obj.idAct = tblgridDNProxTbl.data()[i].idAct;
                                    obj.Vigencia = tblgridDNProxTbl.data()[i].vidaUtil;
                                    obj.programado = flag;
                                    obj.idMant = tblgridDNProxTbl.data()[i].idMant;
                                    obj.Observaciones = tblgridDNProxTbl.data()[i].Observaciones;
                                    obj.estatus = true;
                                    obj.FechaServicio = $("#fechaIniManteS").val();
                                    obj.UsuarioCap = tblgridDNProxTbl.data()[i].UsuarioCap;
                                    obj.fechaCaptura = tblgridDNProxTbl.data()[i].fechaCaptura;
                                    obj.aplicado = false;
                                    tblgridDNProxTbl.data()[i].proyectado = obj;

                                }
                            }
                        }

                    });

                }
            });

            tblgridDNProxTbl.draw();
        }

        ///Se encarga de agregar o actualizar los datos de la tabla de Lubricantes.
        function setProgramado(componenteId, suministroid, programado, prueba, vidaUtil) {

            var array = new Array();
            for (var i = 0; i < tblGridLubProxTbl.data().length; i++) {

                if (tblGridLubProxTbl.data()[i].idComponente == componenteId) {
                    proyectado = tblGridLubProxTbl.data()[i].proyectado;
                    objHis = tblGridLubProxTbl.data()[i].objHis;
                    if (proyectado != null) {

                        tblGridLubProxTbl.data()[i].proyectado.idMisc = suministroid;
                        tblGridLubProxTbl.data()[i].proyectado.programado = programado;
                        tblGridLubProxTbl.data()[i].proyectado.prueba = prueba;
                        tblGridLubProxTbl.data()[i].proyectado.Vigencia = vidaUtil;

                    }
                    else {
                        arraySuministros = tblGridLubProxTbl.data()[i].Suministros;

                        for (var s = 0; s < arraySuministros.length; s++) {
                            if (arraySuministros[s].suministroID == suministroid) {
                                var obj = {};
                                objSuministros = arraySuministros[s].edadSuministro[0];
                                obj.FechaServicio = new Date().getDate() + "/" + (new Date().getMonth() + 1) + "/" + new Date().getFullYear();
                                obj.Hrsaplico = tblGridLubProxTbl.data()[i].objHis.hrsAplico;
                                obj.Observaciones = '';
                                obj.UsuarioCap = objSuministros.UsuarioCap;
                                obj.Vigencia = tblGridLubProxTbl.data()[i].objHis.vidaA;
                                obj.estatus = true;
                                obj.fechaCaptura = obj.FechaServicio;
                                obj.id = 0;
                                obj.idComp = componenteId;
                                obj.idMisc = suministroid;
                                obj.programado = programado,
                                    obj.prueba = prueba;
                                tblGridLubProxTbl.data()[i].proyectado = obj;
                            }
                        }
                    }
                }

                array.push(tblGridLubProxTbl.data()[i]);
            }
            return array;
        }

        ///Se encarga de renderizar la vida consumida y la vida restante de de las tablas.
        function setPorcentajesVida(vidaUtil, hrsAplico) {
            Estatus = "";
            Barra = "";

            horometroActual = Number($("#paso2 .txtHrT:eq(0)").val());
            VidaAceite = horometroActual - hrsAplico;
            VidaRestante = vidaUtil - VidaAceite;
            Vida = (VidaAceite * 100) / vidaUtil;
            Patron = VidaRestante;
            Estatus = "";

            if (Patron >= 250) {
                Estatus = "<button type='button'  style='color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" +
                    "<button type='button'  style='font-size: large; color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                    "<div class='progress-bar progress-bar-success' style='width:" + Vida +
                    "%;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            } else if (Patron < 250 && Patron >= 0) {
                Estatus = "<button type='button'  style='color:orange; ' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                    "<div class='progress-bar progress-bar-warning' style='width:" + Vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            } else if (Patron < 0) {
                Estatus = "<button type='button'  style='color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full  fa-spin'></span></button>" + "<button type='button'  style='color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                    "<div class='progress-bar progress-bar-danger' style='width:" + Vida + "%; font-size:15px;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
            }

            //console.log("-----------------------------------------");
            //console.log("Horometro actual: " + horometroActual);
            //console.log("Vida aceite: " + VidaAceite);
            //console.log("Vida restante: " + VidaRestante.toFixed(2));
            //console.log("Vida: " + Vida);
            //console.log("-----------------------------------------");

            return objReturn = {
                Estatus: Estatus,
                Barra: Barra
            };
        }

        function setPorcentajesVidaTemp(vidaUtil, hrsAplico) {
            Estatus = "";
            Barra = "";

            horometroActual = Number($(".txtHorometroUltLub").val());
            //horometroActual = Number($("#horometroMaquina").val());
            VidaAceite = horometroActual - hrsAplico;
            VidaRestante = vidaUtil - VidaAceite;
            Vida = (VidaAceite * 100) / vidaUtil;
            Patron = VidaRestante;
            Estatus = "";

            if (Patron >= 250) {
                Estatus = "<button type='button'  style='color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>" + "<button type='button'  style='font-size: large; color:black;' id='btndrop'>" + VidaRestante.toFixed(2) + "</button>";
                Barra = "<div class='progress' style='margin-bottom:0px;'>" +
                    "<div class='progress-bar progress-bar-success' style='width:" + Vida + "%;color: black; font-weight: bold;margin-bottom:0px;margin-top:0px'>" + Vida.toFixed(2) + "%" + "</div>";
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
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        function initGridPaso2(grid) {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                    footer: ""
                },
                formatters: {
                    "Icon": function (column, row) {
                        return '<i class="' + row.Icon + ' fa-3x"  style="color:black;"></i>'
                    },
                    "componente": function (column, row) {
                        return "<input type='text'  value='" + row.componente[0] + "' data-index='" + row.idComponente.idCompVis + "' class='Componente' disabled>";
                    },
                    "InputVida": function (column, row) {
                        return "<input type='number'  class='form-control bfh-number InputVida'disabled>";
                    },
                    "VidaRest": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number VidaRest'disabled>";
                    },
                    "HorServ": function (column, row) {

                        return "<input type='number' tabindex='" + row.id + "'  data-index='" + row.id + "' class='form-control bfh-number HorServ'disabled>";
                    },
                    "estatus": function (column, row) {
                        return "<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>";
                    },
                    "EdadSum": function (column, row) {

                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number EdadSum'disabled>";
                    },
                    "AceiteVin": function (column, row) {
                        html = '';
                        html += '<div class="SelectSum">';
                        html += '<select class="form-control" id="cboAceiteVin" style="margin-top:4px;" title="Seleccione:">'
                        html += '<option value="0" selected disabled hidden>Tipo De Aceite:</option>'
                        $.each(row.AceiteVin, function (i, e) {
                            if (e.edadSuministro != 0) {
                                html += '<option  id-idAct="' + e.edadSuministro[0].idAct + '"  id-misc="' + e.edadSuministro[0].idMis + '"  data-edad="' + e.edadSuministro[0].vida + '"  value="' + e.descripcion[0].id + '">' + e.descripcion[0].nomeclatura + '</option>'
                            }
                        });
                        html += '</select>'
                        html += '</div>';
                        return html;
                    },
                    "chkPrueba": function (column, row) {
                        if (row.asignado != null) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        id = 0;
                        if (row.asignado != null) {
                            id = row.asignado.id;
                        }
                        return "<label  class='btn chkPrueba'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "'data-id='" + id + "' >" +
                            "<input  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off' " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
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
                            "<input class='chkAplico'  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off' " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".chkfiltro").on("click", function (e) {
                    var chkbx = $(this).find("input");
                    if (chkbx.is(":checked")) {
                        $(this).closest("tr").find("td:eq(4) .txtEdadAceite").attr("disabled", false);
                        id = 0,
                            ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = true;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis };
                        VincularMis(obj);
                        ruta = '/MatenimientoPM/FillGridComponenteVin';
                        loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                        $("#TextModalVin").attr("hidden", false);
                        ruta = '/MatenimientoPM/FillGridMiselaneo';
                        //$.blockUI({ message: mensajes.PROCESANDO });
                        //   loadGrid(getFiltrosMis(), ruta, grid_MiselaneoFiltro);
                    } else {
                        $(this).closest("tr").find("td:eq(2) .txtEdadAceite").attr("disabled", false);
                        id = $(this).attr("data-id")
                        ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                        idAct = $("#btnModCompModalVin").attr("data-idvinc");
                        estado = false;
                        idCompVis = $("#btnModMis").attr("data-idviscomp");
                        idTipo = $("#cboModalidad option:selected").val();
                        idMis = $(this).attr("data-index");
                        obj = { id: id, ModeloEquipoID: ModeloEquipoID, idAct: idAct, estado: estado, idCompVis: idCompVis, idTipo: idTipo, idMis: idMis };
                        VincularMis(obj);
                        ruta = '/MatenimientoPM/FillGridComponenteVin';
                        loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                        ruta = '/MatenimientoPM/FillGridMiselaneo';
                        //    loadGrid(getFiltrosMis(), ruta, grid_MiselaneoFiltro);
                    }
                });
                grid.find(".chkPrueba").on("click", function (e) {
                    var chkbx = $(this).find("input");
                    if (chkbx.is(":checked")) {
                        $(this).closest("tr").find("td:eq(3) .InputVida").attr("disabled", false);
                        //habilitar horometro vida
                        //verificar   que el cambio de select sea diferente
                        if ("" != $(this).closest("tr").find("td:eq(3) .InputVida").val()) {
                            $(this).closest("tr").find("td:eq(4) .HorServ").attr("disabled", false);
                        }
                    } else {
                        $(this).closest("tr").find("td:eq(3) .InputVida").attr("disabled", true);
                        $(this).closest("tr").find("td:eq(3) .InputVida").val("");

                        varSelector = $(this).closest("tr").find('#cboAceiteVin');
                        edadAnterior = $(varSelector).find('option:selected').attr('data-edad');

                        $(this).closest("tr").find("td:eq(3) .InputVida").val(edadAnterior);
                        HorometroActual = $("#btnHorometro").html();
                        HorometroServicio = $(this).closest("tr").find("td:eq(4) .HorServ").val();
                        HorasTranscurridas = HorometroActual - HorometroServicio;
                        $(this).closest("tr").find("td:eq(5) .EdadSum ").val(HorasTranscurridas.toFixed(2));
                        Patron = $(this).closest("tr").find("td:eq(3) .InputVida").val() - $(this).closest("tr").find("td:eq(6) .EdadSum ").val();
                        $(this).closest("tr").find("td:eq(6) .VidaRest").val(Patron);
                        var a = $(this).closest("tr").find("td:eq(7)").html("");
                        if (Patron >= 250) {
                            $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                        } else if (Patron < 250 && Patron >= 0) {
                            $(this).closest("tr").find("td:eq(7)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                        } else if (Patron < 0) {
                            $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                        }
                    }
                });
                grid.find(".InputVida").on("change", function (e) {
                    HorometroActual = $("#btnHorometro").html();
                    HorometroServicio = $(this).closest("tr").find("td:eq(4) .HorServ").val();
                    HorasTranscurridas = HorometroActual - HorometroServicio;
                    $(this).closest("tr").find("td:eq(5) .EdadSum ").val(HorasTranscurridas.toFixed(2));
                    Patron = $(this).closest("tr").find("td:eq(3) .InputVida").val() - $(this).closest("tr").find("td:eq(5) .EdadSum ").val();
                    $(this).closest("tr").find("td:eq(6) .VidaRest").val(Patron.toFixed(2));
                    var a = $(this).closest("tr").find("td:eq(7)").html("");
                    if (Patron >= 250) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                    } else if (Patron < 250 && Patron >= 0) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                    } else if (Patron < 0) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                    }
                });
                grid.find(".SelectSum").on("change", function (e) {

                    selectorVidaUtil = $(this).closest("tr").find("td:eq(3) .InputVida");


                    $(this).closest("tr").find("td:eq(3) .InputVida").val();
                    $(this).closest("tr").find("td:eq(3) .InputVida").val($(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").attr('data-edad'));
                    idmisc = ($(this).closest("tr").find("td:eq(1) #cboAceiteVin option:selected").attr("id-misc"))
                    $(this).closest("tr").find("td:eq(3) .InputVida").attr('idmisc', idmisc);
                    $(this).closest("tr").find("td:eq(4) .HorServ").attr("disabled", false);

                    $(selectorVidaUtil).trigger('change');
                });
                grid.find(".HorServ").on("change", function (e) {
                    HorometroActual = $("#btnHorometro").html();
                    HorometroServicio = $(this).closest("tr").find("td:eq(4) .HorServ").val();
                    HorasTranscurridas = HorometroActual - HorometroServicio;
                    $(this).closest("tr").find("td:eq(5) .EdadSum ").val(HorasTranscurridas.toFixed(2));
                    Patron = $(this).closest("tr").find("td:eq(3) .InputVida").val() - $(this).closest("tr").find("td:eq(5) .EdadSum ").val();
                    $(this).closest("tr").find("td:eq(6) .VidaRest").val(Patron.toFixed(2));
                    var a = $(this).closest("tr").find("td:eq(7)").html("");
                    if (Patron >= 250) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                    } else if (Patron < 250 && Patron >= 0) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                    } else if (Patron < 0) {
                        $(this).closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                    }
                });
            });
        }

        function setTableLubricantesAlta(dataSet) {

            tblGridLubricantesAlta = $("#tblLubricantesAlta").DataTable({
                language: lstDicEs,
                "bFilter": false,
                destroy: true,
                scrollY: "300px",
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                autoWidth: false,
                columns: [
                    {
                        data: "componenteDesc", width: '120px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            html = '';
                            html = "<input class='form-control bfh-number tbComponenteID' data-componenteID='" + rowData.componenteID + "' disabled value='" + cellData + "' >";
                            $(td).append(html);
                        }
                    },
                    {
                        data: "lubricantes", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            var html = '';
                            html += '<div class="SelectSum">';
                            html += '<select class="form-control cboLubricanteAlta" style="margin-top:4px;" title="Seleccione:">'
                            $.each(rowData.lubricantes, function (i, e) {

                                html += '<option  value="' + e.lubricanteID + '"  data-edadAceite="' + e.edadAceite + '" >' + e.descripcion + '</option>'

                            });
                            html += '</select>'
                            html += '</div>';

                            $(td).append(html);
                        }
                    },
                    {
                        data: "pruebaLub", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            checked = (cellData ? 'checked' : "")
                            html = '';
                            html = "<label  class='btn chkPrueba' >" +
                                "<input type='checkbox' class='form-control chckPruebaLubricante' style='margin-left:20px;' " + checked + ">" +
                                " </label>";
                            $(td).append(html);
                        }
                    },
                    {
                        data: "vidaLubricante", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = '';
                            html = "<input type='number' class='form-control bfh-number tbVidaLubricante' value='" + cellData + "' disabled >";
                            $(td).append(html);
                        }

                    },
                    {
                        data: "horServ", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = '';
                            html = "<input type='number' class='form-control bfh-number horaServicio' value='" + cellData + "'>";
                            $(td).append(html);

                        }
                    },
                    {
                        data: "vidaActual", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = '';

                            html = "<input type='number' class='form-control bfh-number tbVidaActual' value='" + cellData + "' disabled>";
                            $(td).append(html);

                        }
                    },
                    {
                        data: "vidaRest", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            html = '';
                            html = "<input type='number' class='form-control bfh-number vidaRestante' value='" + cellData + "'  disabled>";
                            $(td).append(html);

                        }
                    },
                    {
                        data: "estatus", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            vidaUtil = rowData.vidaLubricante;
                            hrsAplico = rowData.horServ;
                            Estatus = setPorcentajesVidaTemp(vidaUtil, hrsAplico).Estatus;
                            $(td).append(Estatus);
                        }
                    }

                ],
                "createdRow": function (row, data, index) {
                    if (data.lubricanteID == undefined)
                        $(row).find('.cboLubricanteAlta').val(data.lubricantes[0].lubricanteID);
                    else
                        $(row).find('.cboLubricanteAlta').val(data.lubricanteID);

                },
                "paging": true,
                "info": false,
                initComplete: function (settings, json) {

                },
                drawCallback: function () {

                    $("#tblLubricantesAlta").find(".chckPruebaLubricante").on("click", function (e) {
                        var chkbx = $(this);
                        if (chkbx.is(":checked")) {
                            $(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").attr("disabled", false);
                            //habilitar horometro vida
                            //verificar   que el cambio de select sea diferente
                            if ("" != $(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").val()) {
                                $(this).parent().closest("tr").find("td:eq(4) .horaServicio").attr("disabled", false);
                            }
                        } else {
                            $(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").attr("disabled", true);
                            $(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").val("");

                            varSelector = $(this).parent().closest("tr").find('.cboLubricanteAlta');
                            edadAnterior = $(varSelector).find('option:selected').attr('data-edadaceite');

                            $(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").val(edadAnterior);
                            HorometroActual = $(".txtHorometroUltLub:eq(0)").val();
                            HorometroServicio = $(this).parent().closest("tr").find("td:eq(4) .horaServicio").val();
                            HorasTranscurridas = HorometroActual - HorometroServicio;
                            $(this).parent().closest("tr").find("td:eq(5) .tbVidaActual ").val(HorasTranscurridas.toFixed(2));
                            Patron = $(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").val() - $(this).parent().closest("tr").find("td:eq(5) .tbVidaActual").val();

                            $(this).parent().closest("tr").find("td:eq(6) .vidaRestante").val(Patron);
                            var a = $(this).parent().closest("tr").find("td:eq(7)").html("");
                            if (Patron >= 250) {
                                $(this).parent().closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
                            } else if (Patron < 250 && Patron >= 0) {
                                $(this).parent().closest("tr").find("td:eq(7)").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
                            } else if (Patron < 0) {
                                $(this).parent().closest("tr").find("td:eq(7)").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
                            }
                        }
                    });
                    $("#tblLubricantesAlta").find(".tbVidaLubricante").on("change", function (e) {
                        HorometroActual = $(".txtHorometroUltLub").val();
                        horServ = $(this).parents('tr').find('.horaServicio').val();
                        prueba = $(this).parents('tr').find('.chckPruebaLubricante').is(':checked');
                        componenteID = $(this).parents('tr').find('.tbComponenteID').attr('data-componenteID');
                        lubricanteID = $('option:selected', $(this).parents('tr').find('.cboLubricanteAlta')).val();
                        vidaLubricante = $(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").val();
                        vidaRestante = Math.round((vidaLubricante - (HorometroActual - horServ)) * 100) / 100;

                        setDataAltaLubricantes(componenteID, prueba, horServ, vidaLubricante, lubricanteID, vidaRestante);
                        $("#tblLubricantesAlta").fixedHeader.adjust();



                        //vidaRestante = Math.round((parseInt($(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").val()) - parseInt($(this).parent().closest("tr").find("td:eq(5) .tbVidaActual").val())) * 100) / 100;
                        //$(this).parent().closest("tr").find("td:eq(6) .vidaRestante").val(vidaRestante);
                    });
                }
            });
        }

        $("#tblLubricantesAlta").on('change', '.cboLubricanteAlta', function () {

            horServ = $(this).parents('tr').find('.horaServicio').val();
            prueba = $(this).parents('tr').find('.chckPruebaLubricante').is(':checked');
            componenteID = $(this).parents('tr').find('.tbComponenteID').attr('data-componenteID');
            lubricanteID = $('option:selected', $(this).parents('tr').find('.cboLubricanteAlta')).val();
            vidaLubricante = $('option:selected', $(this).parents('tr').find('.cboLubricanteAlta')).attr('data-edadaceite');
            vidaRestante = Math.round((parseInt($(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").val()) - (parseInt($(".txtHorometroLubricantes:eq(0)").val()) - parseInt($(this).parent().closest("tr").find("td:eq(4) .horaServicio").val()))) * 100) / 100;

            setDataAltaLubricantes(componenteID, prueba, horServ, vidaLubricante, lubricanteID, vidaRestante);
        });

        $("#tblLubricantesAlta").on('change', '.horaServicio', function () {
            HorometroActual = $(".txtHorometroUltLub").val();
            horServ = $(this).parents('tr').find('.horaServicio').val();
            prueba = $(this).parents('tr').find('.chckPruebaLubricante').is(':checked');
            componenteID = $(this).parents('tr').find('.tbComponenteID').attr('data-componenteID');
            lubricanteID = $('option:selected', $(this).parents('tr').find('.cboLubricanteAlta')).val();
            vidaLubricante = $(this).parent().closest("tr").find("td:eq(3) .tbVidaLubricante").val();
            vidaRestante = Math.round((vidaLubricante - (HorometroActual - horServ)) * 100) / 100;

            setDataAltaLubricantes(componenteID, prueba, horServ, vidaLubricante, lubricanteID, vidaRestante);
            //table.fixedHeader.adjust();
        });

        function setDataAltaLubricantes(componenteID, prueba, horServ, vidaLubricante, lubricanteID, vidaRestante) {

            HorometroActual = $(".txtHorometroUltLub").val();
            var array = new Array();
            for (var i = 0; i < tblGridLubricantesAlta.data().length; i++) {
                obj = {};
                obj = tblGridLubricantesAlta.data()[i];
                if (tblGridLubricantesAlta.data()[i].componenteID == componenteID) {

                    obj.pruebaLub = prueba;
                    obj.horServ = horServ;
                    obj.vidaActual = Math.round((HorometroActual - horServ) * 100) / 100;
                    obj.vidaRest = vidaRestante;
                    obj.vidaLubricante = vidaLubricante;
                    obj.lubricanteID = lubricanteID;
                    obj.asignado = prueba;
                    array.push(obj);
                }
                else {
                    array.push(obj);
                }
            }
            setTableLubricantesAlta(array);
        }

        function initGridActProy(grid) {
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
                        return "<input type='number'   data-idAct='" + row.idAct + "'  data-index='" + row.id + "'  value='" + row.perioricidad + "' class='form-control bfh-number perioricidad' disabled>";
                    },
                    "TiempoTrans": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number TiempoTrans' disabled>";
                    },
                    "hrsRest": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number hrsRest' disabled>";
                    },
                    "estatus": function (column, row) {
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
                        return "<button type='button' class='chkAplico' disabled  style='color:gray;'> <span class='fa fa-comments'></span></button>";
                    },
                    "ProxServ": function (column, row) {
                        return "<input type='number'  class='form-control bfh-number InputProx'disabled>";
                    },
                    "HisLub": function (column, row) {
                        return ("<a href='#gridActhis'><button type='button' class='HisLub' style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-up'></span></button></a>");
                    },
                    "Aplicar": function (column, row) {
                        return "<input type='checkbox' class='form-control aplicar' style='margin-left:20px;' id-act='" + row.idAct + "' >";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".HorServ").on("change", function (e) {
                    //HorometroActual = $("#btnHorometro2").html();
                    HorometroActual = $(".services_panel3:eq(0) .txtHorometroUltLub:eq(0)").val();
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
                grid.find(".aplicar").on("click", function (e) {
                    Checked = $(this).closest('tr').find('td:eq(5) input.aplicar');
                    Aplicado = true;
                    if (Checked.is(":checked")) {
                        $(this).closest('tr').find('td:eq(2) button.chkAplico').attr('disabled', false);
                        ObjBitProyAE = {
                            Hrsaplico: 0,
                            idAct: $(this).closest('tr').find('td:eq(5) input.aplicar').attr("id-act"),
                            Vigencia: 0,
                            programado: true,
                            idMant: $("#MantenimientoSeguimiento").attr('idmant'),
                            Observaciones: "",
                            FechaServicio: "",
                            UsuarioCap: "",
                            estatus: true
                        };
                        //$.blockUI({ message: mensajes.PROCESANDO });
                        $.ajax({
                            async: false,
                            datatype: "json",
                            type: "POST",
                            url: '/MatenimientoPM/GuardarBitProyAE2',
                            data: { ObjBitProyAE: ObjBitProyAE },
                            success: function (response) {
                                //$.unblockUI();
                                if (response.success == true) {
                                    ConsultaModelobyMantenimiento(response.ObjBitProyAE.idMant);
                                    ConsultagridAEProx(ModProy);
                                }
                            },
                            error: function () {
                                //$.unblockUI();
                            }
                        });
                    } else {
                        $(this).closest('tr').find('td:eq(2) button.chkAplico').attr('disabled', true);
                        id = $(this).closest('tr').find('td:eq(5) input').attr('idobj');
                        //cambiar estatus al registro
                        $.ajax({
                            //async: true,
                            datatype: "json",
                            type: "POST",
                            url: '/MatenimientoPM/DeshabilitarACProy',
                            data: { id: id },
                            success: function (response) {
                                //$.unblockUI();
                                ConsultagridAEProx(response.obj);
                                CargaDeAEProyectado();
                                ProgressBarActProx();
                            },
                            error: function () {
                                //$.unblockUI();
                            }
                        });
                    };
                });
                grid.find(".chkAplico").on("click", function (e) {
                    $("#ActividadServTent").val($(this).closest("tr").find("td:eq(0)").text());
                    $("#ActividadServTent").attr("idAct", $(this).closest('tr').find('td:eq(5) input.aplicar').attr("id-act"));
                    $("#btnguardarAEObs").attr("idobj", $(this).closest('tr').find('td:eq(5) input.aplicar').attr("idobj"));
                    $("#modalActividadesCOnt").modal("show");
                });
                CargaDeAEProyectado();
                ProgressBarActProx();
            });
        }
        function CargaDeDNProyectado() {
            if (idmantenimiento != "#idmantenimiento") {
                //$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/CargaDeDNProyectado',
                    data: { idmantenimiento: idmantenimiento },
                    success: function (response) {
                        //$.unblockUI();
                        if (response.items.length != 0) {
                            response.items.forEach(function (valor, indice, array) {
                                var longitud = $("#gridDNProx >tbody >tr").length
                                for (var i = 0; i <= (longitud - 1); i++) {
                                    renglon = $("#gridDNProx >tbody >tr")[i]
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
                        //$.unblockUI();
                    }
                });
            }
        };
        function CargaDeAEProyectado() {
            if (idmantenimiento != "#idmantenimiento") {
                //$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/CargaDeAEProyectado',
                    data: { idmantenimiento: idmantenimiento },
                    success: function (response) {
                        //$.unblockUI();
                        if (response.items.length != 0) {
                            response.items.forEach(function (valor, indice, array) {
                                var longitud = $("#gridActProx >tbody >tr").length
                                for (var i = 0; i <= (longitud - 1); i++) {
                                    renglon = $("#gridActProx >tbody >tr")[i]
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
                        //$.unblockUI();
                    }
                });
            }
        };
        function initGridActHis(grid) {
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
                        return ("<button type='button'  style='color:gray;'> <span class='fa fa-comments'></span></button>");
                    },
                    //"ProxAct": function (column, row) {
                    //    return ("<a href='#gridActProx'><button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-down'></span></button></a>");
                    //},
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".HorServ").on("change", function (e) {
                    HorometroActual = $("#btnHorometro2").html();
                    //HorometroActual = $(".services_panel3:eq(0) .txtHorometroUltLub:eq(0)").val();
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
        function initGridPaso3(grid) {
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
                        return "<input type='number'   data-idAct='" + row.idAct + "'  data-index='" + row.id + "'  value='" + row.perioricidad + "' class='form-control bfh-number perioricidad' disabled>";
                    },
                    "TiempoTrans": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number TiempoTrans' disabled>";
                    },
                    "hrsRest": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number hrsRest' disabled>";
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
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".HorServ").on("change", function (e) {
                    //HorometroActual = $("#btnHorometro2").html();
                    HorometroActual = $(".services_panel3:eq(0) .txtHorometroUltLub:eq(0)").val();
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
        //initGridDNProx
        function initGridDNProx(grid) {
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
                        return "<input type='number'   data-idAct='" + row.idAct + "'  data-index='" + row.id + "'  value='" + row.perioricidad + "' class='form-control bfh-number perioricidad' disabled>";
                    },
                    "TiempoTrans": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number TiempoTrans' disabled>";
                    },
                    "hrsRest": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number hrsRest' disabled>";
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
                        return "<button type='button' class='chkAplico' disabled style='color:gray;'> <span class='fa fa-comments'></span></button>";

                    },
                    "ProxServ": function (column, row) {
                        return "<input type='number'  class='form-control bfh-number InputProx'disabled>";
                    },
                    "HisLub": function (column, row) {
                        return ("<a href='#gridDNHis'><button type='button' class='HisLub' style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-up'></span></button></a>");
                    },
                    "Aplicar": function (column, row) {
                        return "<input type='checkbox' class='form-control aplicar' style='margin-left:20px;' id-act=" + row.idAct + ">";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".HorServ").on("change", function (e) {
                    //HorometroActual = $("#btnHorometro2").html();
                    HorometroActual = $(".services_panel4:eq(0) .txtHorometroUltLub:eq(0)").val();
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
                grid.find(".chkAplico").on("click", function (e) {
                    $("#DNServTent").val($(this).closest("tr").find("td:eq(0)").text());
                    $("#DNServTent").attr("idAct", $(this).closest("tr").find("td:eq(5) input").attr("id-act")),
                        $("#modalDNCOnt").modal("show");
                });
                grid.find(".aplicar").on("click", function (e) {
                    Checked = $(this).closest('tr').find('td:eq(5) input.aplicar');
                    Aplicado = true;
                    if (Checked.is(":checked")) {
                        $(this).closest('tr').find('td:eq(2) .chkAplico').attr('disabled', false);
                        //alert("si");
                        GuardadoMetOBsDn(false, $(this).closest("tr").find("td:eq(5) input").attr("id-act"));
                    } else {
                        $(this).closest('tr').find('td:eq(2) .chkAplico').attr('disabled', true);
                        id = $(this).closest('tr').find('td:eq(5) input').attr('idobj');
                        $.ajax({
                            //async: true,
                            datatype: "json",
                            type: "POST",
                            //url: '/MatenimientoPM/DeshabilitarACProy',
                            url: '/MatenimientoPM/DeshabilitarDNProy',
                            data: { id: id },
                            success: function (response) {
                                //$.unblockUI();
                                ConsultGridDNProy(response.obj);
                                ProgressBarDN();
                                CargaDeDNProyectado();
                                //ConsultagridAEProx(response.obj);
                            },
                            error: function () {
                                //$.unblockUI();
                            }
                        });
                    };
                });
                //ProgressBarDN();
                //CargaDeDNProyectado();
            });
        }
        function GuardadoMetOBsDn(flag, idACt) {
            if (flag == false) {
                ObjBitProyDN = {
                    id: id,
                    Hrsaplico: 0,
                    idAct: idACt,
                    Vigencia: 0,
                    programado: true,//necesita modificarse
                    idMant: $("#MantenimientoSeguimiento").attr('idmant'),
                    Observaciones: $("#ObservacionesMantDN").val(),
                    FechaServicio: "",
                    estatus: true
                };
            } else {
                ObjBitProyDN = {
                    id: $("#btnguardarDNObs").attr("idobj"),
                    Hrsaplico: 0,
                    idAct: $("#DNServTent").attr("idAct"),
                    Vigencia: 0,
                    programado: true,//necesita modificarse
                    idMant: $("#MantenimientoSeguimiento").attr('idmant'),
                    Observaciones: $("#ObservacionesMantDN").val(),
                    FechaServicio: "",
                    estatus: true
                };
            }
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/GuardarBitProyDN',
                data: { ObjBitProyDN: ObjBitProyDN },
                success: function (response) {
                    $("#modalDNCOnt").modal("hide");
                    if (flag == true) {
                        ConfirmacionGeneralFC("Confirmación", "Info. Guardada", "bg-green");
                        $("#ObservacionesMantDN").val("");
                    }
                    CargaDeDNProyectado();
                    ProgressBarDN();
                },
                error: function () {
                    //$.unblockUI();
                }
            });
            //CargaDeDNProyectado();
            //ProgressBarDN();
        }
        function initGridDNhis(grid) {
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
                        return ("<button type='button'  style='color:gray;'> <span class='fa fa-comments'></span></button>");
                    },
                    //"ProxAct": function (column, row) {
                    //    return ("<a href='#gridDNProx'><button type='button'  style='font-size: 2em; color:gray;'> <span class='fa fa-arrow-down'></span></button></a>");
                    //},
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".HorServ").on("change", function (e) {
                    HorometroActual = $("#btnHorometro2").html();
                    //HorometroActual = $(".services_panel3:eq(0) .txtHorometroUltLub:eq(0)").val();
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
        function initGridPaso4(grid) {
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
                        return "<input type='number'   data-idAct='" + row.idAct + "'  data-index='" + row.id + "'  value='" + row.perioricidad + "' class='form-control bfh-number perioricidad' disabled>";
                    },
                    "TiempoTrans": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number TiempoTrans' disabled>";
                    },
                    "hrsRest": function (column, row) {
                        return "<input type='number'  data-index='" + row.id + "' class='form-control bfh-number hrsRest' disabled>";
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
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".HorServ").on("change", function (e) {
                    //HorometroActual = $("#btnHorometro2").html();
                    HorometroActual = $(".services_panel3:eq(0) .txtHorometroUltLub:eq(0)").val();
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
        function SelectModelo(id, idtipo) {
            BuscarInfoModelo(id, idtipo);
            idmodelo.append(id);
        }
        function BuscarInfoModelo(idModelo, idtipo) {
            //$("#modalVinCompAct").block({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/getActividadModelo',
                data: { idModelo: idModelo, idtipo: idtipo },
                async: false,
                success: function (tmp) {
                    PM1 = [];
                    PM2 = [];
                    PM3 = [];
                    PM4 = [];
                    JG = [];
                    EX = [];
                    DN0 = [];
                    DN1 = [];
                    DN2 = [];
                    IN = [];
                    tmp.rows.forEach(function (valor, indice, array) {
                        if (valor.idTipo == 1) {
                            if (valor.PM == 1) {
                                PM1.push(valor);
                            } else if (valor.PM == 2) {
                                PM2.push(valor);
                            } else if (valor.PM == 3) {
                                PM3.push(valor);
                            } else if (valor.PM == 4) {
                                PM4.push(valor);
                            }
                        } else if (valor.idTipo == 2) { //jomagali
                            JG.push(valor);
                        } else if (valor.idTipo == 3) {//extras
                            EX.push(valor);
                        } else if (valor.idTipo == 4) {//DN
                            if (valor.DN == 1) {
                                DN0.push(valor);
                            } else if (valor.DN == 2) {
                                DN1.push(valor);
                            } else if (valor.DN == 3) {
                                DN2.push(valor);
                            }
                        } else if (valor.idTipo == 5) {//Indicacione
                            IN.push(valor);
                        }
                    });
                    grid_pm1.bootgrid("clear");
                    grid_pm1.bootgrid("append", PM1);
                    grid_pm2.bootgrid("clear");
                    grid_pm2.bootgrid("append", PM2);
                    grid_pm3.bootgrid("clear");
                    grid_pm3.bootgrid("append", PM3);
                    grid_pm4.bootgrid("clear");
                    grid_pm4.bootgrid("append", PM4);
                    grid_JG.bootgrid("clear");
                    grid_JG.bootgrid("append", JG);
                    grid_EX.bootgrid("clear");
                    grid_EX.bootgrid("append", EX);
                    grid_DN0.bootgrid("clear");
                    grid_DN0.bootgrid("append", DN0);
                    grid_DN1.bootgrid("clear");
                    grid_DN1.bootgrid("append", DN1);
                    grid_DN2.bootgrid("clear");
                    grid_DN2.bootgrid("append", DN2);
                    $("tr a").css("padding", "0px");
                    $("tr a").css("padding", "0px");
                    tblGroupComponentes.DataTable().clear().draw();
                    tblComponentes_Lubricantes.DataTable().clear().draw();
                    tblComponentes_Filtros.DataTable().clear().draw();
                    //$("#modalVinCompAct").unblock();
                },
                error: function () {
                    //$("#modalVinCompAct").unblock();
                }
            });
        }

        function LimpiartablasVinc() {
            grid_pm1.bootgrid("clear");
            grid_pm2.bootgrid("clear");
            grid_pm3.bootgrid("clear");
            grid_pm4.bootgrid("clear");
            grid_JG.bootgrid("clear");
            grid_EX.bootgrid("clear");
            tblGroupComponentes.DataTable().clear().draw();
            tblComponentes_Lubricantes.DataTable().clear().draw();
            tblComponentes_Filtros.DataTable().clear().draw();
        }
        function initGrid(grid) {
            grid.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "update": function (column, row) {
                        return "<button type='button' class='btn btn-warning modificar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-edit '></span> " +
                            " </button>"
                            ;
                    },
                    "delete": function (column, row) {
                        return "<button type='button' class='btn btn-danger eliminar' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<span class='glyphicon glyphicon-remove'></span> " +
                            " </button>"
                            ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */
                grid_Actividad.find(".modificar").on("click", function (e) {
                    idActividadModificar = $(this).attr("data-index");
                    BuscarActividad("", 0, idActividadModificar);
                    modalAltaActividad.modal("show");
                    $("#txtModaldescripcion").val(DescripcionActMod);
                    $("#cboAltaTipoActividad").fillCombo('/MatenimientoPM/getTipoActividad', { estatus: true });//carga ajax combo

                });
                grid_Actividad.find(".eliminar").on("click", function (e) {
                    var idActividad = $(this).attr("data-index");
                    ConfirmacionEliminadoActividad("Eliminado de Actividad", "¿Desea Eliminar Actividad?", idActividad);
                });
            });
        }
        function initGridCompoVin(grid) {
            grid.bootgrid({
                rowCount: [5],
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "check-asignar": function (column, row) {
                        if (row.asignadoMis != null) {
                            if (row.asignadoMis.estado == false) {
                                row.asignadoMis = null;
                            }
                        }
                        if (row.asignado != null) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        return "<label  class='btn chkAsignar'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<input  style='width:20px;  height:20px;  margin-top:0'  type='checkbox' autocomplete='off' " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    }, "miscelaneos": function (column, row) {
                        if (row.asignado != null && row.asignadoMis == null) {
                            return "<div class='btn-group'  id='status'  data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm miscelaneos'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='glyphicon glyphicon-oil' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        } else if (row.asignado != null && row.asignadoMis != null) {
                            return "<div class='btn-group'  id='status'  data-toggle='buttons'>" +
                                "<label  class='btn btn-default btn-off btn-sm active'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>M/A" +
                                "</label>" +
                                "<label class='btn btn-success btn-on btn-sm miscelaneos'>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='glyphicon glyphicon-oil' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                        else {
                            return "<div class='btn-group'  id='status'  data-toggle='buttons'>" +
                                "<label  class='btn btn-danger btn-off btn-sm active' style='color: white;' disabled>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>N/A" +
                                "</label>" +
                                "<label class='btn btn-default btn-on btn-sm miscelaneos' disabled>" +
                                "<input type='radio' value='1'   id='btnComp' data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='glyphicon glyphicon-oil' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".miscelaneos").on("click", function (e) {
                    $("#modalMiselaneos").modal("show");
                    $("#ComponentesModalVin").modal('hide');

                    $("#TextModalViscosidad").html("");
                    $("#TextModalViscosidad").append("Vinculacion " + $("#TextModalVin").html() + "  " + $(this).find("input").attr("data-descrip"));
                    $("#btnModMis").attr("data-idVisComp", $(this).find("input").attr("data-index"));//vinculacion 


                });
                grid.find(".chkAsignar").on("click", function (e) {
                    var tabla = $(".GenericaPM ul.nav").find('li.active a').text()
                    if (tabla == "") {
                        tabla = $(".GenericaPM ul.nav").find("li.active").children("div.btn-group").children(".active").attr("id-data");
                    }
                    idCatTipoActividad = 0;
                    idpm = 0;
                    if (tabla == "PM1") {
                        idCatTipoActividad = 1;
                        idpm = 1;
                    } else if (tabla == "PM2") {
                        idCatTipoActividad = 1;
                        idpm = 2;
                    } else if (tabla == "PM3") {
                        idCatTipoActividad = 1;
                        idpm = 3;
                    } else if (tabla == "PM4") {
                        idCatTipoActividad = 1;
                        idpm = 4;
                    } else if (tabla == "Lubricantes") {
                        $("#vJG").click()
                        idCatTipoActividad = 2;
                        idpm = 0;
                    } else if (tabla == "DN0") {
                        idCatTipoActividad = 4;
                        idDN = 4;
                    } else if (tabla == "DN1") {
                        idCatTipoActividad = 4;
                        idDN = 2;
                    } else if (tabla == "DN2") {
                        idCatTipoActividad = 4;
                        idDN = 3;
                    }
                    var chkbx = $(this).find("input");
                    id = chkbx.attr("data-index");
                    ModeloEquipoID = $("#cboFiltroModeloVinc option:selected").val();
                    idComponente = $(this).attr("data-index");
                    idact = $("#btnModCompModalVin").attr("data-idvinc");
                    if (chkbx.is(":checked")) {
                        $(this).closest("tr").find("td:eq(3) div#status.btn-group label").attr("disabled", false);
                        VincularComponete(id, ModeloEquipoID, idComponente, idact, true, idpm, idCatTipoActividad);//vincula componente con actividad
                        // paginaActualComp = $("#grid_ComponenteVin").bootgrid("getCurrentPage");
                        BanderapagComp = false;
                    } else {
                        $(this).closest("tr").find("td:eq(3) div#status.btn-group label").attr("disabled", true);
                        VincularComponete(id, ModeloEquipoID, idComponente, idact, false, idpm, idCatTipoActividad);//vincula componente con actividad
                        //  paginaActualComp = $("#grid_ComponenteVin").bootgrid("getCurrentPage");
                        BanderapagComp = false;
                    }
                    ruta = '/MatenimientoPM/FillGridComponenteVin';
                    loadGrid(getFiltrosObject(), ruta, grid_ComponenteVin);
                });
            });
        }
        function initGridCompoActPM(grid) {
            grid.bootgrid({
                rowCount: [5],
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "check-asignar": function (column, row) {
                        if (row.asignadoMis != null) {
                            if (row.asignadoMis.estado == false) {
                                row.asignadoMis = null;
                            }
                        }
                        if (row.asignado != null && row.pmActual == true) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        return "<label  class='btn chkAsignar'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<input  style='width:30px;  height:30px; margin-top:0px;'   class='form-control aplicar' type='checkbox'  " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    }, "miscelaneos": function (column, row) {
                        if (row.asignadoMis == null) {
                            return "<button type='button' disabled style='margin-left:10px' class='btn btn-default'>" +
                                "<span class='fa fa-ban'></span> " +
                                " </button>";
                        } else if (row.asignado != null && row.asignadoMis != null) {
                            return "<div class='btn-group'  id='status'  data-toggle='buttons'>" +
                                "<label  class='btn btn-default btn-off btn-sm active'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>M/A" +
                                "</label>" +
                                "<label class='btn btn-success btn-on btn-sm miscelaneos'>" +
                                "<input type='radio' value='1'    data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='glyphicon glyphicon-oil' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                        else {
                            return "N/A";
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".miscelaneos").on("click", function (e) {
                    $("#modalMiselaneos").modal("show");
                    $("#TextModalViscosidad").html("");
                    $("#TextModalViscosidad").append("Vinculacion " + $("#TextModalVin").html() + "  " + $(this).find("input").attr("data-descrip"));
                    $("#btnModMis").attr("data-idVisComp", $(this).find("input").attr("data-index"));//vinculacion 
                });
                grid.find(".chkAsignar").on("click", function (e) {
                    var chkbx = $(this).find("input");


                });
                //excluirNoComponentes();
            });

        }

        function initGridgridGestorFormatos(grid) {
            grid.bootgrid({
                rowCount: [4],
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "check-asignar": function (column, row) {
                        if (row.asignadoMis != null) {
                            if (row.asignadoMis.estado == false) {
                                row.asignadoMis = null;
                            }
                        }
                        if (row.asignado != null) {
                            checked = "checked=checked";
                        } else {
                            checked = "";
                        }
                        return "<label  class='btn chkAsignar'  data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "'data-estatus='" + row.estatus + "' >" +
                            "<input  style='width:30px;  height:30px; margin-top:0px;'   class='form-control aplicar' type='checkbox'  " + checked + "></span> " +
                            "<span></span> " +
                            " </label>"
                            ;
                    }, "miscelaneos": function (column, row) {
                        if (row.asignadoMis == null) {
                            return "<button type='button' disabled style='margin-left:10px' class='btn btn-default'>" +
                                "<span class='fa fa-ban'></span> " +
                                " </button>";
                        } else if (row.asignado != null && row.asignadoMis != null) {
                            return "<div class='btn-group'  id='status'  data-toggle='buttons'>" +
                                "<label  class='btn btn-default btn-off btn-sm active'>" +
                                "<input type='radio' value='0' name='multifeatured_module[module_id][status]'>M/A" +
                                "</label>" +
                                "<label class='btn btn-success btn-on btn-sm miscelaneos'>" +
                                "<input type='radio' value='1'    data-index='" + row.id + "'" + "data-descrip='" + row.descripcion + "' name='multifeatured_module[module_id][status]' checked='checked'>" +
                                "<i class='glyphicon glyphicon-oil' size='8px;'></i>" +
                                "</label>" +
                                "</div>"
                        }
                        else {
                            return "N/A";
                        }
                    }, "nombre": function (column, row) {
                        return row.idformato.nombreArchivo;
                    }, "Aplicar": function (column, row) {
                        return "<input type='checkbox' class='form-control aplicar' style='margin-left:20px;' id-act=" + row.idAct + ">";
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                grid.find(".chkAsignar").on("click", function (e) {

                });
            });
        }
        //truco para el redireccionamiento 
        var contadorcomp = 0;

        function BuscarComponenteVinculado(ModeloEquipoID, idComponente, idacts) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            objVincularComponete = { id: 0, ModeloEquipoID: ModeloEquipoID, idAct: idacts, estado: true, idCompVis: idComponente };
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/VincularComponete',
                data: { objVincularComponete: objVincularComponete },
                success: function (response) {
                    //$.unblockUI();
                    BuscarActividad("", 0, 0);
                },
                error: function () {
                    //$.unblockUI();
                }
            });

        };
        //vinculacion de miscelaneos
        function VincularMis(obj) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/VincularMis',
                data: { obj: obj },
                success: function (response) {
                    $("#cboModalidad").trigger('change');
                },
                error: function () {
                    //$.unblockUI();
                }
            });

        };
        function VincularComponete(id, ModeloEquipoID, idComponente, idacts, estatus, idpm, idCatTipoActividad) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            objVincularComponete = { id: 0, ModeloEquipoID: ModeloEquipoID, idAct: idacts, estado: estatus, idCompVis: idComponente, idpm: idpm, idCatTipoActividad: idCatTipoActividad };
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/VincularComponete',
                data: { objVincularComponete: objVincularComponete },
                success: function (response) {
                    //$.unblockUI();
                    BuscarActividad("", 0, 0);
                },
                error: function () {
                    //$.unblockUI();
                }
            });

        };
        function ELiminarVinculacion(id) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            var tabla = $(".GenericaPM ul.nav").find('li.active a').text();
            var tabla = $(".GenericaPM ul.nav").find('li.active a').text();
            if (tabla == "") {
                tabla = $(".GenericaPM ul.nav").find("li.active").children("div.btn-group").children(".active").attr("id-data");
            }
            idTipoAct = 0;
            idpm = 0;
            idDN = 0;
            if (tabla == "PM1") {
                idTipoAct = 1;
                idpm = 1;
            } else if (tabla == "PM2") {
                idTipoAct = 1;
                idpm = 2;
            } else if (tabla == "PM3") {
                idTipoAct = 1;
                idpm = 3;
            } else if (tabla == "PM4") {
                idTipoAct = 1;
                idpm = 4;
            } else if (tabla == "DN0") {
                idTipoAct = 4;
                idDN = 1;
            } else if (tabla == "DN1") {
                idTipoAct = 1;
                idDN = 2;
            } else if (tabla == "DN2") {
                idTipoAct = 1;
                idDN = 3;
            } else if (tabla == "JG") {
                $("#vJG").click()
                idTipoAct = 2;
                idpm = 0;
            }
            obj = { id: id, idCatTipoActividad: idTipoAct, idPM: idpm, idDN: idDN };
            var idVinc = parseInt(id);
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/EliminarVinculacion',
                data: obj,
                success: function (response) {
                    var tabla = $(".GenericaPM ul.nav").find('li.active a').text()
                    idTipoAct = 0;
                    if (tabla == "PM1") {
                        idTipoAct = 1;
                    } else if (tabla == "PM2") {
                        idTipoAct = 1;
                    } else if (tabla == "PM3") {
                        idTipoAct = 1;
                    } else if (tabla == "PM4") {
                        idTipoAct = 1;
                    } else if (tabla == "JG") {
                        $("#vJG").click()
                        idTipoAct = 2;
                    }
                    idmodelo = $("#cboFiltroModeloVinc option:selected").val();
                    BuscarInfoModelo(idmodelo, idTipoAct);
                    btnActividadVinc.click();
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ELiminarActividad(idActividad) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            objCatActividadPM = { id: idActividad };
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ELiminarActividad',
                data: { objCatActividadPM: objCatActividadPM },
                success: function (response) {
                    //$.unblockUI();
                    BuscarActividad("", 0, 0);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ProyeccionProxFechaServicio() {
            horometroServicio = $("#HorometroAplico").val();
            fechaMantenimientoActual = fechaIniMante.val();
            getDataRitmo(EconomicoID, true);
            var hrsPromDiaria = 0;
            if (trabajoPorDiaManual != null) {
                hrsPromDiaria = trabajoPorDiaManual;
            } else {
                hrsPromDiaria = trabajoPorDiaAutomatico;
            }
            var factor = 250;
            var hrsServProx = (parseInt(horometroServicio) + 250);
            var from = fechaMantenimientoActual.split("/");
            var fechaProyectadaProximo = new Date(from[2], from[1] - 1, from[0]);
            var proyeccionFechaServDias = factor / hrsPromDiaria;
            var fechaproxServicio = sumarDias(fechaProyectadaProximo, proyeccionFechaServDias);
            var date = new Date(fechaproxServicio),
                yr = date.getFullYear(),
                month = date.getMonth() < 9 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1),
                day = date.getDate() < 9 ? '0' + date.getDate() : date.getDate()
            return fechaproxServicio = day + '/' + month + '/' + yr;
        }
        function SelectEmpleado(event, ui) {
            tbNombreEmpleado.text(ui.item.value);
            tbNombreEmpleado.attr('data-NumEmpleado', ui.item.id)
            SetInfoEmpleado(ui.item.id);
            idResponsable = ui.item.id;
        }
        function SetInfoEmpleado(idEmplado) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetSingleUsuario',
                data: { id: idEmplado },
                async: false,
                success: function (response) {
                    if (response.success) {
                        tbPuestoEmpleado.val(response.Puesto.toLowerCase());
                        tbPuestoEmpleado.attr('data-CC', response.CCEmpleado);
                        //pruebaraguuilar 03/07/18
                        $("#tbPuestoEmpleadoS").val(response.Puesto.toLowerCase());
                        $("#tbPuestoEmpleadoS").attr('data-CC', response.CCEmpleado);

                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function SelectCoordinador(event, ui) {
            tbNombreCoordinador.text(ui.item.value);
            tbNombreCoordinador.attr('data-NumEmpleado', ui.item.id)
            SetInfoCoordinador(ui.item.id);
            idPlaneador = ui.item.id;
        }
        function SetInfoCoordinador(idEmplado) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/ResguardoEquipo/GetSingleUsuario',
                data: { id: idEmplado },
                async: false,
                success: function (response) {
                    if (response.success) {
                        tbPuestoCoordinador.val(response.Puesto.toLowerCase());
                        tbPuestoCoordinador.attr('data-CC', response.CCEmpleado);
                        //prueba 03/07/18
                        $("#tbPuestoCoordinadorS").val(response.Puesto.toLowerCase());
                        $("#tbPuestoCoordinadorS").attr('data-CC', response.CCEmpleado);
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        //validacion calculos horometro fechas
        function ValidacionCalculosHorometro() {
            if (HorometroAplico.val() != "") {
                EconomicoID = $("#txtNoEconomicoAlt").val();
                TraerFechasIntarvaluo($("#fechaIniMante").val(), EconomicoID);
                var HorometroPMrealizo = HorometroAplico.val();
                var ultimoHorometroRegistrado = $("#horometroMaquina").val();
                var Valormin = parseFloat(HorometroPMrealizo) - parseFloat(FechalimiteInferiorHrs);
                if (parseFloat(HorometroPMrealizo) > parseFloat(ultimoHorometroRegistrado)) {//no puede ser mayor al horometro actual
                    AlertaModal("Error", "El horómetro no puede ser Mayor al Último Horómetro Capturado " + parseFloat(ultimoHorometroRegistrado).toFixed(2));
                }
                else if ((parseFloat(HorometroPMrealizo) > (parseFloat(FechaSeleccionadaHrs + 250))) && FechaSeleccionadaHrs != 0) {//no puede ser mayor  a mas 25
                    AlertaModal("Error", "El horometro registrado no puede ser Mayor a 25 horas " + (parseFloat(FechaSeleccionadaHrs + 250).toFixed(2)));
                } else if ((parseFloat(HorometroPMrealizo) < (parseFloat(FechaSeleccionadaHrs - 250))) && FechaSeleccionadaHrs != 0) {//no puede ser menor a 25
                    AlertaModal("Error", "El horometro registrado no puede ser menor a 25 horas " + (parseFloat(FechaSeleccionadaHrs - 250).toFixed(2)));
                } else {
                    HorometroProyectado.val((parseFloat(HorometroPMrealizo)) + 250);
                    fechaFinMante.val(ProyeccionProxFechaServicio(EconomicoID));
                    TipoMantenimientoActual = $("#cboTipoMantenientoContador option:selected").val();
                    if (TipoMantenimientoActual != "") {
                        if (TipoMantenimientoActual == 8) {
                            $("#MantenimientoProy").val($('#cboTipoMantenientoContador  option:eq("' + 1 + '")').text());
                        } else {
                            $("#MantenimientoProy").val($('#cboTipoMantenientoContador  option:eq("' + (parseInt(TipoMantenimientoActual) + 1) + '")').text());
                        }
                    } else {

                        AlertaModal("Error", "Seleccione el tipo del último Mantenimiento");
                    }
                }
            }
        };
        function TraerFechasIntarvaluo(fecha, EconomicoID) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarIntervaloFecha',
                data: { fecha: fecha, EconomicoID: EconomicoID },
                success: function (response) {
                    //$.unblockUI();


                    FechaLimiteInferior = response.items.FechaLimiteInferior.Fecha
                    FechaLimiteSuperior = response.items.FechaLimiteSuperior.Fecha;
                    FechalimiteSuperiorHrs = response.items.FechaLimiteSuperior.Horometro;
                    FechalimiteInferiorHrs = response.items.FechaLimiteInferior.Horometro;
                    FechaLimiteInferior = FormateoFechaEdicion(FechaLimiteInferior);
                    if (response.items.FechaSeleccionada.Fecha != undefined) {
                        FechaSeleccionada = response.items.FechaSeleccionada.Fecha
                        FechaSeleccionada = FormateoFechaEdicion(FechaSeleccionada);
                        FechaSeleccionadaHrs = response.items.FechaSeleccionada.Horometro
                    } else {
                        FechaSeleccionadaHrs = 0;//SI NO HAY HOROMETRO DE ESA FECHA
                    }
                    if (FechaLimiteSuperior != undefined) {
                        FechaLimiteSuperior = FormateoFechaEdicion(FechaLimiteSuperior);
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ConsultarFechaUltimoHorometro(Horometro, EconomicoID) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarFechaUltimoHorometro',
                data: { Horometro: Horometro, EconomicoID: EconomicoID },
                success: function (response) {
                    //$.unblockUI();
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function preSeleccionado() {
            btnTipo = $(this).attr("id");
            var tablaGenerica = "";
            if (btnTipo == 'btnTodosPM1') {
                tablaGenerica = "#tablaPM1";
            } else if (btnTipo == 'btnTodosPM2') {
                tablaGenerica = "#tablaPM2";
            } else if (btnTipo == 'btnTodosPM3') {
                tablaGenerica = "#tablaPM3";
            } else if (btnTipo == 'btnTodosPM4') {
                tablaGenerica = "#tablaPM4";
            }
            if (contadorCiclopm1 == 0 && btnTipo == 'btnTodosPM1') {
                banderaSelecTodospm1 = true;
                contadorCiclopm1 = 1;
            } else if (contadorCiclopm1 == 1 && btnTipo == 'btnTodosPM1') {
                banderaSelecTodospm1 = false;
                contadorCiclopm1 = 0;
            } else if (contadorCiclopm2 == 0 && btnTipo == 'btnTodosPM2') {
                banderaSelecTodospm2 = true;
                contadorCiclopm2 = 1;
            } else if (contadorCiclopm2 == 1 && btnTipo == 'btnTodosPM2') {
                banderaSelecTodospm2 = false;
                contadorCiclopm2 = 0;
            } else if (contadorCiclopm3 == 0 && btnTipo == 'btnTodosPM3') {
                banderaSelecTodospm3 = true;
                contadorCiclopm3 = 1;
            } else if (contadorCiclopm3 == 1 && btnTipo == 'btnTodosPM3') {
                banderaSelecTodospm3 = false;
                contadorCiclopm3 = 0;
            } else if (contadorCiclopm4 == 0 && btnTipo == 'btnTodosPM4') {
                banderaSelecTodospm4 = true;
                contadorCiclopm4 = 1;
            } else if (contadorCiclopm4 == 1 && btnTipo == 'btnTodosPM4') {
                banderaSelecTodospm4 = false;
                contadorCiclopm4 = 0;
            }
            if (btnTipo == 'btnTodosPM1') {
                if (banderaSelecTodospm1) {
                    checkadopm1 = true;
                } else {
                    checkadopm1 = false;
                }
                SeleccionarTodos(checkadopm1, tablaGenerica);
            } else if (btnTipo == 'btnTodosPM2') {
                if (banderaSelecTodospm2) {
                    checkadopm2 = true;
                } else {
                    checkadopm2 = false;
                }
                SeleccionarTodos(checkadopm2, tablaGenerica);

            } else if (btnTipo == 'btnTodosPM3') {
                if (banderaSelecTodospm3) {
                    checkadopm3 = true;
                } else {
                    checkadopm3 = false;
                }
                SeleccionarTodos(checkadopm3, tablaGenerica);

            } else if (btnTipo == 'btnTodosPM4') {
                if (banderaSelecTodospm4) {
                    checkadopm4 = true;
                } else {
                    checkadopm4 = false;
                }
                SeleccionarTodos(checkadopm4, tablaGenerica);
            }
        }
        function getConsultaUltHrsByEcoo(obj) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultaUltHrsByEco',
                data: { EconomicoID: obj },
                success: function (response) {
                    //$.unblockUI();
                    horometroActual = (response.items.HorometroAcumulado);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function sumarDias(fecha, dias) {
            fecha.setDate(fecha.getDate() + dias);
            return fecha;
        }
        function GuardarParte(AltaCompMin, AltaCompDesc, AltaCompMax) {
            objCatparte = { id: 0, descripcion: AltaCompDesc, vidaUtilMin: AltaCompMin, vidaUtilMax: AltaCompMax };
            $.ajax({
                url: '/MatenimientoPM/GuardarParte',
                type: "POST",
                datatype: "json",
                async: false,
                data: { objCatparte: objCatparte },
                success: function (response) {
                    modalAltaComponte.modal("hide");
                    ConfirmacionGeneralFC("Confirmación", response.Actividad, "bg-green");
                    CargaGridParte()
                },
                error: function () {
                }
            });
        }
        function ConfirmacionEliminadoActividad(titulo, mensaje, btnAceptar) {
            if (!$("#modalEliminar").is(':visible')) {
                var html = '<div id="modalEliminar" class="modal fade" role="dialog" data-backdrop="static">' +
                    '<div class="modal-dialog modal-dialog-fix modal-md" >' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-bg">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                    '&times;</button>' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body ajustar-texto">' +
                    '<h5 id="pMessage">' +
                    '</h5>' +
                    '<div class="row">' +
                    '<div id="icon" class="col-md-2">' +
                    '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
                    '</div>' +
                    '<div class="col-md-10">' +
                    '<h3>  ' + mensaje + '</h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a data-dismiss="modal" id="' + btnAceptar + '" class="btn btn-primary btn-sm btnEliminadoActividad"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '<a data-dismiss="modal" id="btnEliminadoCancelar"  class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-remove"></span> Cancelar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }
        }
        function ConsultaPm(idMaquina) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            objMantenimiento = { idMaquina: idMaquina };
            $.ajax({
                url: '/MatenimientoPM/AjustarFullcalendarFront',
                type: "POST",
                datatype: "json",
                //async: false,
                data: { objMantenimiento: objMantenimiento, objObra: cboObras.val() },
                success: function (response) {
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', response.d)
                    returnData(response.d);
                    //$.unblockUI();
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ConsultarUltimoHorometro(fechaIniMante, EconomicoID, bandera) {//sobre carga cambio de numero economico
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarUltimoHorometro',
                data: { fechaIniMante: fechaIniMante, EconomicoID: EconomicoID },
                success: function (response) {
                    //$.unblockUI();
                    if (response.items != 0) {
                        $("#horometroMaquina").val(response.items.HorometroAcumulado);
                        $("#fechaUltCal").val(FormateoFechaEdicion(response.items.Fecha));
                        $("#fechaIniMante").datepicker("option", "maxDate", new Date(FormateoFechaSet(response.items.Fecha)));
                        if (bandera == false) {
                            $("#fechaIniMante").datepicker("setDate", FormateoFechaSet(response.items.Fecha));
                        }
                        $("#HorometroAplico").attr("disabled", false);
                        $("#timepicker_6").attr("disabled", false);
                        $("#tbNombreEmpleado").attr("disabled", false);
                        $("#ObservacionesMant").attr("disabled", false);
                        $("#cboTipoMantenientoContador").attr("disabled", false);
                        $("#HorometroAplico").attr("disabled", false);
                        $("#fechaIniMante").attr("disabled", false);

                    } else {
                        $("#HorometroMantenimiento").val("");
                        $("#HorometroMantenimiento").attr("placeholder", "No hay Horometro Capturado Para La Fecha");
                        $("#fechaIniMante").datepicker("option", "maxDate", "+0m +0w");
                        $("#HorometroAplico").attr("disabled", true);
                        $("#timepicker_6").attr("disabled", true);
                        $("#tbNombreEmpleado").attr("disabled", true);
                        $("#tbPuestoEmpleado").attr("disabled", true);
                        $("#ObservacionesMant").attr("disabled", true);
                        $("#cboTipoMantenientoContador").attr("disabled", true);
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function restaFechas(f1, f2) {
            var aFecha1 = f1.split('-');
            var aFecha2 = f2.split('-');
            var fFecha1 = Date.UTC(aFecha1[0], aFecha1[1] - 1, aFecha1[2]);
            var fFecha2 = Date.UTC(aFecha2[0], aFecha2[1] - 1, aFecha2[2]);
            var dif = fFecha2 - fFecha1;
            var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
            return dias;
        }
        function FormateoFechaEdicion(Fecha) {
            if (Fecha) {
                var dateString = Fecha.substr(6);
                var currentTime = new Date(parseInt(dateString));
                var month = currentTime.getMonth() + 1;
                var day = currentTime.getDate();
                var year = currentTime.getFullYear();
                var date = day + "/" + month + "/" + year;
                return date;
            } else {
                return '';
            }
        }
        function FormateoFechaSet(Fecha) {
            var dateString = Fecha.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = year + "," + month + "," + day;
            return date;
        }
        function FormateoFechaEdicionCalendario(Fecha) {
            var dateString = Fecha.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = year + "-" + month + "-" + day;
            return date;
        }
        function FormateoFechaEdicionCalendarioStart(Fecha) {
            var dateString = Fecha.substr(6);
            var currentTime = new Date(parseInt(dateString));
            var month = currentTime.getMonth() + 1;
            if (month < 10) {
                month = ("0" + month);
            }
            var time = currentTime.toLocaleTimeString().toLowerCase();
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = year + "-" + month + "-" + day + " " + time;
            return date;
        }
        function rrecorrertablaPM1() {
            var nomeclaturaLong = $("#tablaPM1 tbody tr")
            var longitud = nomeclaturaLong.length
            for (var i = 0; i <= longitud; i++) {
                var renglon = nomeclaturaLong[i]
                id = 0;
                idActividad = $(renglon).find('td#indice').attr('id-Actividad');
                Actual = $(renglon).find('td.chk input').prop('checked'),
                    idMaquina = idmaquina.html()
                idMantenimientoPm = 0;
                idParteVidaUtil = $(renglon).find('td#Parte option:selected').attr('idparte');
                Contador = $(renglon).find('td.Contador input').val()
                if (idParteVidaUtil == undefined) {
                    idParteVidaUtil = null
                }
                objtblM_ActvContPM = {
                    id: id, idActividad: idActividad, Contador: Contador, Actual: Actual, idMaquina: idMaquina,
                    idMantenimientoPm: idMantenimientoPm, idParteVidaUtil: idParteVidaUtil
                }
                ArraytblM_ActvContPM.push(objtblM_ActvContPM);
            }
        }
        $("#txtbusquedamisc").keyup(function () {
            var optionCbo = $("#cboModalidad option:selected").val();
            if (optionCbo == 1) {//filtro
                $("#grid_MiselaneoFiltro").bootgrid("search", $("#txtbusquedamisc").val().toUpperCase());
            } else if (optionCbo == 2) {//aceite
                $("#grid_MiselaneoAceite").bootgrid("search", $("#txtbusquedamisc").val().toUpperCase());
            } else if (optionCbo == 3) {//anticongelante
                $("#grid_MisAnticongelante").bootgrid("search", $("#txtbusquedamisc").val().toUpperCase());
            }
        });
        $("#txtbusquedaComp").keyup(function () {
            $("#grid_ComponenteVin").bootgrid("search", $("#txtbusquedaComp").val().toUpperCase());
        });
        function rrecorrertablaPM2() {
            var nomeclaturaLong = $("#tablaPM2 tbody tr")
            var longitud = nomeclaturaLong.length
            for (var i = 0; i <= longitud; i++) {
                var renglon = nomeclaturaLong[i]
                id = 0;
                idActividad = $(renglon).find('td#indice').attr('id-Actividad');
                Actual = $(renglon).find('td.chk input').prop('checked'),
                    idMaquina = idmaquina.html()
                idMantenimientoPm = 0;
                idParteVidaUtil = $(renglon).find('td#Parte option:selected').attr('idparte');
                Contador = $(renglon).find('td.Contador input').val()
                if (idParteVidaUtil == undefined) {
                    idParteVidaUtil = null
                }
                objtblM_ActvContPM = {
                    id: id, idActividad: idActividad, Contador: Contador, Actual: Actual, idMaquina: idMaquina,
                    idMantenimientoPm: idMantenimientoPm, idParteVidaUtil: idParteVidaUtil
                }
                ArraytblM_ActvContPM.push(objtblM_ActvContPM);
            }
        }
        function rrecorrertablaPM3() {
            var nomeclaturaLong = $("#tablaPM3 tbody tr")
            var longitud = nomeclaturaLong.length
            for (var i = 0; i <= longitud; i++) {
                var renglon = nomeclaturaLong[i]
                id = 0;
                idActividad = $(renglon).find('td#indice').attr('id-Actividad');
                Actual = $(renglon).find('td.chk input').prop('checked'),
                    idMaquina = idmaquina.html()
                idMantenimientoPm = 0;
                idParteVidaUtil = $(renglon).find('td#Parte option:selected').attr('idparte');
                Contador = $(renglon).find('td.Contador input').val()
                if (idParteVidaUtil == undefined) {
                    idParteVidaUtil = null
                }
                objtblM_ActvContPM = {
                    id: id, idActividad: idActividad, Contador: Contador, Actual: Actual, idMaquina: idMaquina,
                    idMantenimientoPm: idMantenimientoPm, idParteVidaUtil: idParteVidaUtil
                }
                ArraytblM_ActvContPM.push(objtblM_ActvContPM);
            }
        }
        function rrecorrertablaPM4() {
            var nomeclaturaLong = $("#tablaPM4 tbody tr")
            var longitud = nomeclaturaLong.length
            for (var i = 0; i <= longitud; i++) {
                var renglon = nomeclaturaLong[i]
                id = 0;
                idActividad = $(renglon).find('td#indice').attr('id-Actividad');
                Actual = $(renglon).find('td.chk input').prop('checked'),
                    idMaquina = idmaquina.html()
                idMantenimientoPm = 0;
                idParteVidaUtil = $(renglon).find('td#Parte option:selected').attr('idparte');
                Contador = $(renglon).find('td.Contador input').val()
                if (idParteVidaUtil == undefined) {
                    idParteVidaUtil = null
                }
                objtblM_ActvContPM = {
                    id: id, idActividad: idActividad, Contador: Contador, Actual: Actual, idMaquina: idMaquina,
                    idMantenimientoPm: idMantenimientoPm, idParteVidaUtil: idParteVidaUtil
                }
                ArraytblM_ActvContPM.push(objtblM_ActvContPM);
            }
        }
        function returnData(param) {
            objPm = param;
        }
        function FillCboGrupo() {
            clearCbo();
            if ($("#cboTipoServ").val() != "") {
                cboFiltroGrupo.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: $("#cboTipoServ").val() });
            }
            else {
                cboFiltroGrupo.clearCombo();
                cboFiltroGrupo.attr('disabled', true);
            }
        }
        function FillCboNoEconomico() {
            if ($(cboFiltroGrupo).val() != "") {
                //modaltablaPM.block({ message: mensajes.PROCESANDO });
                $(cboFiltroNoEconomico).fillCombo('/MatenimientoPM/fillCboEconomicos', { idGrupo: cboFiltroGrupo.val(), idTipo: $("#cboTipoServ").val() });
                //modaltablaPM.unblock();
                $(cboFiltroNoEconomico).attr('disabled', false);
            }
            else {
                $(cboFiltroNoEconomico).clearCombo();
                $(cboFiltroNoEconomico).attr('disabled', true);
            }
        }
        function getDataRitmo(obj, ritmo) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/horometros/getDataRitmo',
                data: { obj: obj },
                success: function (response) {
                    //$.unblockUI();
                    var data = response.dataRitmo;
                    if (response.dataRitmo != null && (response.dataRitmo.horasDiarias != 0 && response.dataRitmo.horasSemana != 0)) {
                        trabajoPorDiaManual = (data.horasDiarias);
                        trabajoPorSemana = (data.horasSemana);
                    }
                    else {
                        trabajoPorDiaManual = null;
                        ConsultarRitmoAutomatico(obj);
                    }
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ConsultarRitmoAutomatico(obj) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarRitmoAutomatico',
                data: { EconomicoID: obj },
                success: function (response) {
                    //$.unblockUI();
                    trabajoPorDiaAutomatico = response.items;
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function SeleccionarTodos(checkado, tablaGenerica) {
            var nomeclaturaLong = $("" + tablaGenerica + " tbody tr")
            var longitud = nomeclaturaLong.length
            for (var i = 0; i <= longitud; i++) {
                var renglon = nomeclaturaLong[i]
                try {
                    var check = renglon.children[8].children[0]
                    check.checked = checkado
                } catch (e) {
                }
            }
        }
        function RenglonPM(valor, indice, tablaPM) {
            renglonPM1 =
                "<tr>"
                + "<td id='indice' id-Actividad=" + valor.id + ">"
                + "<label id='idActividad" + valor.id + "'>" + (indice + 1) + "</label>"
                + "</td>"
                + "<td id='descripcionActividad>"
                + "<textarea readonly style='width:100%'>" + valor.descripcion[0] + "</textarea>"
                + "</td>"
                + "<td id='Parte'>"
                + "<select id='Tipo" + indice + "' style='width:100%'>"
                + "</select>"
                + "</td>"
                + "<td class='Contador'>"
                + "<input id='horometroUltimoParte" + indice + "' tabindex=" + (indice + 1) + " type='number' style='border:1px solid black; width:110px;'  >"
                + "</td>"
                + "<td class='HorasTrabajadas'>"
                + "<input  id='horasTrabajadas" + indice + "' disabled type='number' style='border:1px solid black; width:110px;'  >"
                + "</td>"
                + "<td class='vidahrsComponente'>"
                + "<input  id='elemento" + indice + "' disabled type='number' style='border:1px solid black; width:110px;'  >"
                + "</td>"
                + "<td class='vidaUtil'>"
                + "<input  id='util" + indice + "'  disabled type='number' style='border:1px solid black; width:110px;'  >"
                + "</td>"

                + "<td class='estatus' id='estatus" + indice + "' >"

                + "</td>"
                + "<td  class='chk'  id='" + valor.tipoMantenimiento[0] + "'>"
                + "<input type='checkbox' class='form-control'  type='text'>"
                + "</td>"
                + "</tr>"
            tablaPM.append(renglonPM1)
        }
        function MetodoInsertSelect(valor, indice) {
            flagHorometro = false;
            $("#Tipo" + indice + "").append($('<option>',
                {
                    value: valor.factorConvencional[0],
                    text: "Convencional"
                }));
            valor.factorExtra.forEach(function (valorExtra, indiceE, arrayE) {
                $("#Tipo" + indice + "").append($('<option>',
                    {
                        idparte: valorExtra.id,
                        prefijo: valor.tipoMantenimiento[0],
                        prefijo: valorExtra.vidaUtilMin,
                        value: valorExtra.vidaUtilMax,
                        text: valorExtra.descripcion
                    }));

                if (valor.Contador[0] != null) {
                    if (valor.Contador[0].idParteVidaUtil == valorExtra.id) {
                        $('select#Tipo' + indice + '  option[idparte="' + valor.Contador[0].idParteVidaUtil + '"]').prop("selected", true);
                        $("#horometroUltimoParte" + indice + "").val(valor.Contador[0].Contador);
                        flagHorometro = true;
                    }
                }
            });
            if (flagHorometro == true) {
                $("#horasTrabajadas" + indice + "").val((horometroMaquina.val()) - $("#horometroUltimoParte" + indice + "").val());
            } else {

                if (valor.Contador == undefined || valor.Contador.length == 0) {
                    $("#horometroUltimoParte" + indice + "").val(0);
                } else {
                    $("#horometroUltimoParte" + indice + "").val(valor.Contador[0].Contador);
                }
                $("#horasTrabajadas" + indice + "").val((horometroMaquina.val()) - $("#horometroUltimoParte" + indice + "").val());
            }
            $("#elemento" + indice + "").val($("select#Tipo" + indice + "").val())//vida util parte
            $("#util" + indice + "").val(($("#elemento" + indice + "").val() - $("#horasTrabajadas" + indice + "").val()))
            calculoEstatus($("#elemento" + indice + "").val(), $("#horasTrabajadas" + indice + "").val(), indice, $("#util" + indice + "").val());
        }
        function calculoEstatus(elemento, hrsTrabajadas, indice, vidautil) {
            if (vidautil > 25 && vidautil <= (1 / 4 * elemento)) {//indiacion de un cuarto
                $("#estatus" + indice + "").append("<button type='button' style='font-size: 2em; color:red;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-quarter'></span></button>");
            } else if (vidautil > (1 / 4 * elemento) && vidautil <= (2 / 4 * elemento)) {//indicacion de 1/4 a 2/4 
                $("#estatus" + indice + "").append("<button type='button' style='font-size: 2em; color:orange;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-half'></span></button>");
            } else if (vidautil > (2 / 4 * elemento) && vidautil <= (3 / 4 * elemento)) {//indicacion de 2/4 a 3/4 
                $("#estatus" + indice + "").append("<button type='button' style='font-size: 2em; color:green;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-three-quarters'></span></button>");
                //} else if (vidautil > (3 / 4 * elemento) && vidautil < (4 / 4 * elemento)) {//indicacion de 2/4 a 3/4 
                //    $("#estatus" + indice + "").append("<button type='button' style='font-size: 2em; color:green;' id='btndrop' class='rotar'> <span class='fa fa-thermometer-three-quarters'></span></button>");
            } else if (vidautil > (3 / 4 * elemento) && vidautil < (4 / 4 * elemento)) {//indicacion de 2/4 a 3/4 
                $("#estatus" + indice + "").append("<button type='button'  style='font-size: 2em; color:green;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
            } else if (vidautil > (3 / 4 * elemento) && vidautil == elemento) {//indicacion de 2/4 a 3/4 
                $("#estatus" + indice + "").append("<button type='button'  style='font-size: 2em; color:blue;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-full'></span></button>");
            } else if (vidautil >= -25 && vidautil <= 25) {//indicacion de 2/4 a 3/4 
                $("#estatus" + indice + "").append("<button type='button'  style='font-size: 2em; color:orange;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
            } else if (vidautil < -25) {//indicacion de 2/4 a 3/4 
                $("#estatus" + indice + "").append("<button type='button'  style='font-size: 2em; color:red;' id='btndrop' class='rotar' > <span class='fa fa-thermometer-empty  fa-spin'></span></button>");
            }
        }
        function clearCbo() {
            $(cboFiltroNoEconomico).clearCombo();
            $(cboFiltroNoEconomico).attr('disabled', true);
            $(cboFiltroGrupo).attr('disabled', false);
        }
        function BuscarFicha() {
            //modalNuevoServicio.block({ message: mensajes.PROCESANDO });
            //$.blockUI({ message: mensajes.Enviando });
            $.ajax({
                url: '/MatenimientoPM/BuscarFicha',
                type: "POST",
                datatype: "json",
                data: { obj: $("#cboFiltroNoEconomico option:selected").html() },
                success: function (response) {
                    //$.unblockUI();
                    if (response.descripcion.length > 0) {
                        cboFiltroNoEconomico
                        NoEconomico.val(response.noEconomico);
                        txtDescripcion.val(response.descripcion);
                        txtMarca.val(response.marca);
                        txtModelo.val(response.modelo);
                        txtNoSerie.val(response.noSerie);
                        txtAnio.val(response.anio);
                        dateFechaCompra.val(response.fechaCompra);
                        txtHorometroInicio.val(response.horometroInicio);
                        txtUltimoParo.val(response.fechaParo);
                        txtDesParo.val(response.detParo);
                        txtUbicacion.val(response.ubicacion);
                        txtCostAdquisicion.val(response.costoAdquisicion);
                        txtCostOverActivo.val(response.costoOverHaul);
                        txtHorometroActual.val(response.horometroActual);
                        txtCostOverAplicado.val(response.costoOverHaulAplicado);
                        txtPropietario.val("CONSTRUPLAN");
                        btnImprimir.show();
                        //modalNuevoServicio.unblock();
                        btntablaPM.click();
                    }
                    else {
                        AlertaGeneral("Alerta", "No. Economico no encontrado");
                    }
                },
                error: function () {
                    //modalNuevoServicio.unblock();
                }
            });
        }
        function GuardarMatenimientoObj() {
            //modaltablaPM.block({ message: mensajes.PROCESANDO });
            //$.blockUI({ message: mensajes.Enviando });

            $.ajax({
                url: ' /MatenimientoPM/GuardarPm',
                type: "POST",
                datatype: "json",
                data: { objMantenimiento: objtblM_MatenimientoPm, arrJG: arrJG, arrAE: arrAE, arrDN: arrDN },
                success: function (response) {
                    ConfirmacionGeneralFC("Confirmación", "   Se a guardado Servicio  ", "bg-green");
                    $("#btnstep1").click();
                    $("#modaltablaPM").modal("hide");
                    //modaltablaPM.unblock();
                },
                error: function () {
                    //modaltablaPM.unblock();
                }
            });
        }
        function AlertaModal(titulo, mensaje) {
            if (!$("#modalEliminacion").is(':visible')) {
                var html = '<div id="modalAlerta" class="modal" role="dialog" data-backdrop="static">' +
                    '<div class="modal-dialog modal-dialog-fix modal-lg" >' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-bg">' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body ajustar-texto">' +
                    '<h5 id="pMessage">' +
                    '</h5>' +
                    '<div class="row">' +
                    '<div id="icon" class="col-md-2">' +
                    '<span class="glyphicon glyphicon-warning-sign alert-warning-span" style="font-size:40px;" aria-hidden="true"></span>' +
                    '</div>' +
                    '<div class="col-md-10">' +
                    '<h3>  ' + mensaje + '</h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a data-dismiss="modal" id="errorAceptar" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }
        }
        function ModificacionFecha(idMaquina, FechaUpdate) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ModificacionFecha',
                data: { idMaquina: idMaquina, FechaUpdate: FechaUpdate },
                success: function (response) {
                    //$.unblockUI();
                    $('#calendar').fullCalendar('removeEvents')
                    ConsultaPm(0);
                    //$.unblockUI();
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function ConfirmacionGeneralFC(titulo, mensaje, color) {
            if (!$("#dialogalertaGeneral").is(':visible')) {
                var html = '<div id="dialogalertaGeneral" class="modal fade modalAlerta" role="dialog">' +
                    '<div class="modal-dialog">' +
                    '<div class="modal-content">' +
                    '<div class="modal-header text-center modal-md">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">' +
                    '&times;</button>' +
                    '<h4  class="modal-title">' + titulo + '</h4>' +
                    '</div>' +
                    '<div class="modal-body">' +
                    '<div class="container">' +
                    '<div class="row">' +
                    '<div class="col-lg-12">' +
                    '<h3> <span  class="glyphicon glyphicon-ok-circle ' + color + '" aria-hidden="true" style="font-size:40px;  display: inline-block;   width: 20%; vertical-align:top;"></span> <label style="position: fixed;">' + mensaje + '</label></h3>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                    '<a id="btndialogalertaGeneral" class="btn btn-primary btn-sm"><span class="glyphicon glyphicon-ok"></span> Aceptar</a>' +
                    '</div>' +
                    '</div>' +
                    '</div></div>';

                var _this = $(html);
                _this.modal("show");
            }

        }
        function FormateoFechaModificacionHorario(fecha) {
            var currentTime = new Date(fecha);
            var month = currentTime.getMonth() + 1;
            if (month < 10) {
                month = ("0" + month);
            }
            var time = currentTime.toLocaleTimeString().toLowerCase();
            var day = currentTime.getDate();
            var year = currentTime.getFullYear();
            var date = year + "-" + month + "-" + day + " " + time;
            return date;
        }
        function ModificacionHorarioServicio(idMaquina, inicio, fin) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                //(int idMaquina, DateTime inicio, DateTime fin)
                url: '/MatenimientoPM/ModificacionHorarioServicio',
                data: { idMaquina: idMaquina, inicio: FormateoFechaModificacionHorario(inicio), fin: FormateoFechaModificacionHorario(fin) },
                success: function (response) {
                    //$.unblockUI();
                    $('#calendar').fullCalendar('removeEvents')
                    ConsultaPm(0);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }
        function alerta(variable) {
            //  alert(variable.id)
        }
        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            $("#wrapper").toggleClass("active");
        });
        function initTabla() {
            calendar.fullCalendar({
                header: {
                    left: 'prev,next today miboton,',
                    center: 'title',
                    //right: 'month,agendaWeek,agendaDay,listWeek'
                    //right: 'listYear,month,agendaWeek,agendaDay'
                    right: 'listYear,month,agendaWeek'
                },
                timeFormat: 'H(:mm)',
                eventLimit: true,
                timezone: 'local',
                buttonText: {
                    today: 'Hoy',
                    month: 'Mes',
                    week: 'Semana',
                    day: 'Dia',
                    list: 'Agenda'
                },
                locate: 'ISO',
                //defaultView: 'month',
                displayEventTime: true,//muestra fecha
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
                //ddefaultDate: '2018-03-12',
                //defaultDate: Moment,
                //navLinks: true, // can click day/week names to navigate views
                editable: true,
                disableDragging: true,
                //eventLimit: true, // for all non-agenda views
                //views: {
                //    agenda: {
                //        eventLimit: 6 // adjust to 6 only for agendaWeek/agendaDay
                //    },
                events: [
                ],
                eventClick: function (calEvent, jsEvent, view) {

                    if (calEvent.estadoMantenimiento == 3) {
                        economicoiD = calEvent.title;
                        idmantenimiento = calEvent.idMantenimiento;
                        GetArchivosAdjuntos(idmantenimiento);
                        RenderizadoServicio(idmantenimiento, economicoiD);
                        $("#lblArchivo").hide();
                        $("#archivoPM").hide();
                        $("#btnSaveProgramar").hide();
                        $("#btnPaso1").click();

                    } else {
                        economicoiD = calEvent.title;
                        idmantenimiento = calEvent.idMantenimiento;
                        $("#grid_LubHis").bootgrid("clear");
                        GetArchivosAdjuntos(idmantenimiento);
                        RenderizadoServicio(idmantenimiento, economicoiD);
                        $("#lblArchivo").show();
                        $("#archivoPM").show();
                        $("#btnSaveProgramar").show();
                        $("#btnPaso1").click();
                    }


                },
                //editable: true,
                eventDrop: function (event, delta, revertFunc) {
                    alert(event.title + " was dropped on " + event.start.format());
                    if (!confirm(" ¿Se procede a relizar Cambio De Fecha?" + event.start.format())) {
                        revertFunc();
                    } else {
                        ModificacionFecha(event.idMantenimiento, event.start.format(), event.end)
                    }
                },
                eventResize: function (event, delta, revertFunc) {
                    ModificacionHorarioServicio(event.idMantenimiento, event.end._d, event.start._d);
                },
                eventRender: function (event, element) {
                    element.find('.fc-title').append(" <label style='text-align:center' class='pull-right !important'>   " + event.description + "<label>");
                    element.find('.fc-time').html("");
                },

            });
            //renderizado srvico 05/07/18 11 am
            function RenderizadoServicio(idmantenimiento, economicoiD) {
                //$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/RenderizadoServicio',
                    data: { idmantenimiento: idmantenimiento, economicoiD: economicoiD },
                    success: function (response) {
                        if (response.success) {
                            $("#MantenimientoSeguimiento").html("");
                            $("#MantenimientoSeguimiento").append("Programación de Actividades  " + response.Mantenimiento.economicoID);
                            $("#MantenimientoSeguimiento").attr("idMant", idmantenimiento);

                            $(".txtNoEconomicoAltS").val(response.Mantenimiento.economicoID);
                            $(".NoEcoServTent").val(response.Mantenimiento.economicoID);
                            $(".txtUltCap").val(response.Mantenimiento.horometroUltCapturado);
                            $("#horometroMaquinaS").val(response.Mantenimiento.horometroUltCapturado);
                            //asginacop

                            $("#tbNombreCoordinadorS").val(response.CoodinadorPM.nombre + " " + response.CoodinadorPM.ape_paterno + " " + response.CoodinadorPM.ape_materno);

                            // $("#tbNombreCoordinadorS").attr('disabled', true);
                            $("#tbPuestoCoordinadorS").val(response.ResponsablePM.descripcion);

                            $("#tbNombreEmpleadoS").val(response.ResponsablePM.nombre + " " + response.ResponsablePM.ape_paterno + " " + response.ResponsablePM.ape_materno);
                            // $("#tbNombreEmpleadoS").attr('disabled', true);
                            $("#tbPuestoEmpleadoS").val(response.CoodinadorPM.descripcion);

                            $(".txtHorometroLubricantes").val(response.Mantenimiento.horometroUltCapturado);
                            $("#fechaUltCalS").val(FormateoFechaEdicion(response.Mantenimiento.fechaUltCapturado));
                            $("#cboTipoMantenientoContadorS").fillCombo('/MatenimientoPM/FillCombotablaPM', { estatus: true });//carga ajax combo
                            $("#cboTipoMantenientoContadorS").val(response.Mantenimiento.tipoPM).change();
                            $("#cboTipoMantenientoContadorS").attr('disabled', true);
                            $("#fechaIniManteS").val(FormateoFechaEdicion(response.Mantenimiento.fechaPM));
                            $("#fechaIniManteS").attr('disabled', true);
                            $("#HorometroAplicoS").val(response.Mantenimiento.horometroPM);
                            $("#HorometroAplicoS").attr('disabled', true);
                            $("#ObservacionesMantS").val(response.Mantenimiento.observaciones);
                            $("#MantenimientoProyS").val(response.Mantenimiento);
                            $("#MantenimientoProyS").attr('disabled', true);
                            $("#fechaFinManteS").val(FormateoFechaEdicion(response.Mantenimiento.fechaProy));
                            $("#fechaFinManteS").attr('disabled', true);
                            $("#HorometroProyectadoS").val(response.Mantenimiento.horometroProy);
                            $(".fechaServProxLub").val(FormateoFechaEdicion(response.Mantenimiento.fechaProy));
                            $(".horometroServTent").val($("#HorometroProyectadoS").val());
                            $("#HorometroProyectadoS").attr('disabled', true);
                            TipoMantenimientoActual = $("#cboTipoMantenientoContadorS option:selected").val();
                            if (TipoMantenimientoActual != "") {
                                if (TipoMantenimientoActual == 8) {
                                    $("#MantenimientoProyS").val($('#cboTipoMantenientoContadorS  option:eq("' + 1 + '")').text());
                                } else {
                                    $("#MantenimientoProyS").val($('#cboTipoMantenientoContadorS  option:eq("' + (parseInt(TipoMantenimientoActual) + 1) + '")').text());
                                }
                            }
                            idCor = response.Mantenimiento.planeador;
                            idEmp = response.Mantenimiento.personalRealizo;

                            // INFO MAQUINA
                            $("#cboFiltroGrupoS").val(response.maquina.grupoMaquinariaID).change();
                            $("#cboFiltroGrupoS").attr('disabled', true);
                            $(".txtTipoUltimoServLu").val($("#cboTipoMantenientoContador option:selected").text());
                            modeloID = response.maquina.modeloEquipoID;
                            $(".txtServT").val($("#MantenimientoProyS").val());
                            $(".txtHrT").val($("#HorometroProyectadoS").val());
                            $(".txtFechaT").val($("#fechaFinManteS").val());
                            //ConsultarJGEstructuraLubricantes
                            setTableTlbGridLub(response.dataSetGridLubProx); //Carga de tabla de Datatable.
                            initGridLubHis($("#gridLubhis")); //Carga inicializa bootgrid.
                            $("#gridLubhis").bootgrid("clear");
                            $("#gridLubhis").bootgrid("append", response.JGHis);
                            //GetActividadesExtra
                            if (response.objActividadesExtras != "") {
                                initGridActHis($("#gridActhis"));
                                $("#gridActhis").bootgrid("clear");
                                $("#gridActhis").bootgrid("append", response.objActividadesExtrashis);
                                setTableGridActividadesExtra(response.objActividadesExtras)
                            } else {
                                LimpiarFormularios();
                                AlertaModal("Error", "No existe Informacion de Actividades Extras Relacionados al Modelo   '" + modeloID + "'");
                                //modaltablaPM.modal("hide");
                            }
                            initGridDNhis($("#gridDNHis"));
                            $("#gridDNHis").bootgrid("clear");
                            $("#gridDNHis").bootgrid("append", response.objActividadesDNHis);
                            setTableGridDNs(response.objActividadesDN);

                            //Show Modal
                            idmant = idmantenimiento;
                            //$("#btnReporteMant").attr("idmant", idmant);

                            $("#Pm_GestorActividades").html("");
                            var cadena = $("#MantenimientoProyS").val();
                            var idPM = 0;
                            //var modeloEquipoID = 0;
                            inicio = 0, fin = 3, subCadena = cadena.substring(inicio, fin);
                            $("#Pm_GestorActividades").append(subCadena);
                            if (subCadena == "PM1") { idPM = 1; } else if ("PM2") { idPM = 2; } else if ("PM3") { idPM = 3; } else if ("PM4") { idPM = 4; }
                            $("#Pm_GestorActividades").attr("idpm", idPM);

                            //-->ConsultaActividadesProg(modeloID, idPM, idmantenimiento);
                            $("#grid_GestorActividades").bootgrid("clear");
                            $("#grid_GestorActividades").bootgrid("append", response.ObjActProy);


                            //CargaDeProyectado();
                            if (response.CargaDeProyectado.length != 0) {
                                response.CargaDeProyectado.forEach(function (valor, indice, array) {
                                    var longitud = $("#gridLubProx >tbody >tr").length
                                    for (var i = 0; i <= (longitud - 1); i++) {
                                        renglon = $("#gridLubProx >tbody >tr")[i]
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
                            //CargaDeAEProyectado();
                            if (response.CargaDeAEProyectado.length != 0) {
                                response.CargaDeAEProyectado.forEach(function (valor, indice, array) {
                                    var longitud = $("#gridActProx >tbody >tr").length
                                    for (var i = 0; i <= (longitud - 1); i++) {
                                        renglon = $("#gridActProx >tbody >tr")[i]
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
                            //CargaDeDNProyectado();
                            if (response.CargaDeDNProyectado.length != 0) {
                                response.CargaDeDNProyectado.forEach(function (valor, indice, array) {
                                    var longitud = $("#gridDNProx >tbody >tr").length
                                    for (var i = 0; i <= (longitud - 1); i++) {
                                        renglon = $("#gridDNProx >tbody >tr")[i]
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
                            //ProgressBarLub();
                            //ProgressBarActProx();
                            //ProgressBarDN();
                            //CargaDeFiltros(modeloID, idmantenimiento);
                            setGridFiltros(response.CargaDeFiltros);

                            modalSeguimientopms.modal("show");
                        }
                        else {

                        }
                        //$.unblockUI();
                    },
                    error: function () {
                        //$.unblockUI();
                    }
                });

            }
            var modeloID = "";
            function InfoMaquina(economicoiD, idMantenimiento) {
                //$.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    async: false,
                    datatype: "json",
                    type: "POST",
                    url: '/MatenimientoPM/ConsultarIDmaquinaria',
                    data: { EconomicoiD: economicoiD },
                    success: function (result) {
                        $("#cboFiltroGrupoS").val(result.items.grupoMaquinariaID).change();
                        $("#cboFiltroGrupoS").attr('disabled', true);
                        $(".txtTipoUltimoServLu").val($("#cboTipoMantenientoContador option:selected").text());
                        modeloID = result.items.modeloID;

                        //$.unblockUI();
                    },
                    error: function () {
                        //$.unblockUI();
                    }
                });
                getActividadesLubricantes(modeloID, idMantenimiento);
                getActividadesExtras(modeloID, idMantenimiento);
                getActividadesDNs(modeloID, idMantenimiento);
            }


            function FillCboGrupoS() {
                clearCbo();
                if ($("#cboTipoServS").val() != "") {
                    $("#cboFiltroGrupoS").fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: $("#cboTipoServS").val() });
                }
                else {
                    $("#cboFiltroGrupoS").clearCombo();
                    $("#cboFiltroGrupoS").attr('disabled', true);
                }
            }
        }


        function reloadCalendarByObra() {
            ConsultaPm(0);
        }

        function cargarDataComboModal() {
            Idmodalidad = $("#cboModalidad option:selected").val();
            if (Idmodalidad == 1) {
                $("#grid_MiselaneoFiltro").removeClass("hidden");
                $("#grid_MiselaneoFiltro-footer").removeClass("hidden");
                $("#grid_MiselaneoAceite").addClass("hidden");
                $("#grid_MiselaneoAceite-footer").addClass("hidden");
                $("#grid_MisAnticongelante").addClass("hidden");
                $("#grid_MisAnticongelante-footer").addClass("hidden");
                ruta = '/MatenimientoPM/FillGridMiselaneo';
                grid_MiselaneoFiltro.bootgrid("clear");
                loadGrid(getFiltrosMis(), ruta, grid_MiselaneoFiltro);
            } else if (Idmodalidad == 2) {
                $("#grid_MiselaneoAceite").removeClass("hidden");
                $("#grid_MiselaneoAceite-footer").removeClass("hidden");
                $("#grid_MiselaneoFiltro").addClass("hidden");
                $("#grid_MiselaneoFiltro-footer").addClass("hidden");
                $("#grid_MisAnticongelante").addClass("hidden");
                $("#grid_MisAnticongelante-footer").addClass("hidden");


                ruta = '/MatenimientoPM/FillGridMiselaneo';
                grid_MiselaneoAceite.bootgrid("clear");
                loadGrid(getFiltrosMis(), ruta, grid_MiselaneoAceite);
            } else if (Idmodalidad == 3) {
                $("#grid_MiselaneoFiltro").addClass("hidden");
                $("#grid_MiselaneoFiltro-footer").addClass("hidden");
                $("#grid_MiselaneoAceite").addClass("hidden");
                $("#grid_MiselaneoAceite-footer").addClass("hidden");
                $("#grid_MisAnticongelante-footer").removeClass("hidden");
                $("#grid_MisAnticongelante").removeClass("hidden");
                ruta = '/MatenimientoPM/FillGridMiselaneo';
                grid_MisAnticongelante.bootgrid("clear");
                loadGrid(getFiltrosMis(), ruta, grid_MisAnticongelante);
            }
        }
        function getModelo(idmantenimiento) {
            var retval;
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/ConsultarModeloEquipoPM',
                data: { idMantenimiento: idmantenimiento },
                success: function (response) {
                    //$.unblockUI();
                    retval = response.Actividad;
                },
                error: function () {
                    //$.unblockUI();
                }
            });
            return retval;
        }
        function CargaDeFiltros(idModelo, idmantenimiento) {
            //$.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                async: false,
                datatype: "json",
                type: "POST",
                url: '/MatenimientoPM/getFiltrosMiselaneos',
                data: { idModelo: idModelo, idmantenimiento: idmantenimiento },
                success: function (response) {
                    //$.unblockUI();
                    setGridFiltros(response.resultData);
                },
                error: function () {
                    //$.unblockUI();
                }
            });
        }

        function setGridFiltros(dataSet) {
            gridFiltrosComponentes = $("#tblFiltrosComponentes").DataTable({
                language: lstDicEs,
                "bFilter": false,
                destroy: true,
                scrollY: "300px",
                scrollCollapse: true,
                paging: false,
                data: dataSet,
                dom: '<t>p',
                columns: [
                    { data: "componente", width: '120px', },
                    { data: "modelo", width: '40px' },
                    {
                        data: "cantidad", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {

                            $(td).text('');
                            html = '';
                            html = "<input type='number' class='form-control tbCantidadFiltros'  disabled value='" + cellData + "'>";
                            $(td).append(html);
                        }
                    },
                    {
                        data: "chkProgramado", width: '40px',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).text('');
                            checked = (rowData ? 'checked' : "")
                            html = '';
                            html = "<label  class='btn chkPrueba' >" +
                                "<input type='checkbox'  data-row='" + row + "'  data-idMantto =' " + rowData.idMant + "'class='form-control programarFiltros' style='margin-left:20px;' " + checked + ">" +
                                " </label>";
                            $(td).append(html);
                        }
                    },

                ],
                "paging": true,
                "info": false,
                "createdRow": function (row, data, index) {
                    filtroIsProgramado = $(row).find('.programarFiltros');
                    $(filtroIsProgramado).prop("checked", data.programado);
                },
                initComplete: function (settings, json) {
                    $("#tblFiltrosComponentes").on('change', '.programarFiltros', function () {
                        programado = $(this).is(':checked');
                        rowDat = $(this).attr('data-row');
                        setProgramadoFiltro(programado, rowDat);
                    });
                }
            });
        }

        function setProgramadoFiltro(programado, index) {
            gridFiltrosComponentes.data()[index].programado = programado;
        }

        init();

    }
    $(document).ready(function () {
        MantenimientoPM.panelpm = new panelpm();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);


});