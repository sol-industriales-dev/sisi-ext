(function () {

    $.namespace('maquinaria.MovimientoMaquinaria.Standby');

    Standby = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };

        var StandbyOBj;
        btnRegresar = $("#btnRegresar"),
        btnNuevo = $("#btnNuevo"),
        tblEquiposStandBylist = $("#tblEquiposStandBylist"),
        divPrincipal = $("#divPrincipal"),
        divCaptura = $("#divCaptura"),
        ireport = $("#report "),
        btnVerReporte = $("#btnVerReporte"),
        modalImpresion = $("#modalImpresion");
        fechaInicio = $("#fechaInicio");
        fechaFin = $("#fechaFin"),
        tbCCFiltro = $("#tbCCFiltro"),
        btnGuardarStandBy = $("#btnGuardarStandBy"),
        tbfechaIni = $("#tbfechaIni"),
        tbfechaFin = $("#tbfechaFin"),
        btnImprimir = $("#btnImprimir"),
        cboListaCC = $("#cboListaCC"),
        tblEquiposStandBy = $("#tblEquiposStandBy"),
        tblEquiposObra = $("#tblEquiposObra");
        

        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        var now = new Date(),
         y = now.getYear() + 1900;
        m = now.getMonth();
        d = now.getDate();

        function init() {

            btnNuevo.click(nuevaConciliacion);
            LOADFECHAS();
            loadTableppal();
            cboListaCC.fillCombo('/MovimientoMaquinaria/cboGetCentroCostos');
            cboListaCC.change(LoadTabla);
            btnVerReporte.click(verReporte);
            btnImprimir.click(verModalImprimir);

            dateNow = new Date();
            var day = dateNow.getDay(),
                diff = dateNow.getDate() - day + (day == 0 ? 0 : 1);
            FechaInicio = new Date(dateNow.setDate(diff));
            var dayFin = new Date(FechaInicio);
            difff = dayFin.getDate() + (2 + 7 - dayFin.getDay()) % 7;
            FechaFin = new Date(dayFin.setDate(difff));

            $("#tbfechaIni").datepicker().datepicker("setDate", FechaInicio);
            $("#tbfechaFin").datepicker().datepicker("setDate", FechaFin);

   //         $("#tbfechaIni").datepicker("option", "minDate", FechaInicio);
     //       $("#tbfechaIni").datepicker("option", "maxDate", FechaInicio);

           // $("#tbfechaFin").datepicker("option", "minDate", FechaFin);
         //   $("#tbfechaFin").datepicker("option", "maxDate", FechaFin);
            divCaptura.addClass('hide');
            btnRegresar.click(back);

        }
        function nuevaConciliacion() {
            cboListaCC.prop('disabled', false);
            LOADFECHAS();
            divCaptura.removeClass('hide');
            divPrincipal.addClass('hide');
            tblEquiposObra.bootgrid("clear");
            tblEquiposStandBy.bootgrid("clear");
            loadTableppal();
        }


        function back() {
            divPrincipal.removeClass('hide');
            divCaptura.addClass('hide');
            tblEquiposObra.bootgrid("clear");
            tblEquiposStandBy.bootgrid("clear");

        }

        function loadTableppal() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/FechaInicioFin",
                type: "POST",
                datatype: "json",
                success: function (response) {
                    $.unblockUI();
                    tbfechaFin.val(response.FechaFin);
                    tbfechaIni.val(response.FechaInicio);
                    fechaInicio.val(response.FechaInicio);
                    fechaFin.val(response.FechaFin);

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LOADFECHAS() {

            $.ajax({
                url: "/MovimientoMaquinaria/ValidarStandByLista",
                type: "POST",
                datatype: "json",
                success: function (response) {
                    var data = response.StandyByLista;
                    listaTabla = data;
                    tblEquiposStandBylist.bootgrid("clear");
                    tblEquiposStandBylist.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
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

        $(document).on('click', "#btnAsignarTodo", function () {
            listaFechas = $(".setFechaStandby");
            temptblEquiposObraData = [];

            var tblEquiposObraData = $('#tblEquiposObra').bootgrid().data('.rs.jquery.bootgrid').rows;
            var tblEquiposStanByData = $("#tblEquiposStandBy").bootgrid().data('.rs.jquery.bootgrid').rows;

            temptblEquiposObraData = $.grep(tblEquiposObraData,
                  function (o, i) { return o.FechaStandBy == undefined; },
               true);

            for (var i = 0; i < temptblEquiposObraData.length; i++) {
                var objDataE = {};
                temp = temptblEquiposObraData[i];
                tblEquiposStanByData.push(temp);
            }

            tblEquiposObraData = $.grep(tblEquiposObraData,
                  function (o, i) { return o.FechaStandBy != undefined; },
               true);

            tblEquiposObra.bootgrid("clear");
            tblEquiposObra.bootgrid("append", tblEquiposObraData);
            tblEquiposStandBy.bootgrid("clear");
            tblEquiposStandBy.bootgrid("append", tblEquiposStanByData);
            AlertaGeneral('Confirmación', 'Los registros fueron agregados para su guardado.');


        });


        $(document).on('click', "#btnGuardarStandBy", function () {

            objStandby = getObject();
            if (StandbyOBj != null) {

                FechaCaptura = new Date(Number(StandbyOBj.FechaCaptura.split('(')[1].split(')')[0]));
                FechaFin = new Date(Number(StandbyOBj.FechaFin.split('(')[1].split(')')[0]));
                FechaInicio = new Date(Number(StandbyOBj.FechaInicio.split('(')[1].split(')')[0]));
                objStandby = StandbyOBj;

                objStandby.FechaCaptura = GetFechaFormat(FechaCaptura);
                objStandby.FechaFin = GetFechaFormat(FechaFin);
                objStandby.FechaInicio = GetFechaFormat(FechaInicio);
            }

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/GuardarStandBy",
                type: "POST",
                datatype: "json",
                data: { standByDet: GetDataSave(), standByObj: objStandby },
                success: function (response) {
                    $.unblockUI();
                 
                    var idReporte = response.id;
                  
                    var CentroCostos = response.CC;
                    var Asunto = response.Asunto;
                    var Correos = response.Correos;
                    var path = "/Reportes/Vista.aspx?idReporte=" + 47 + "&pID=" + idReporte + "&inMemory=1";
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.ajax({
                            url: "/MovimientoMaquinaria/SendCorreoStanby",
                            type: "POST",
                            datatype: "json",
                            data: { cc: CentroCostos, asunto: Asunto, Correos: Correos },
                            success: function (response) {
                                $.unblockUI();
                                if (response.success) {
                                    tblEquiposObra.bootgrid("clear");
                                    tblEquiposStandBy.bootgrid("clear");
                                    cboListaCC.val('');
                                    $('a[href="#ListaEconomicosObra"]').trigger('click');
                                    AlertaGeneral('Confirmación', 'El Registro fue creado correctamente');
                                }
                                else {
                                    AlertaGeneral('Error', 'Ocurrio un error al momento de guardar la información');
                                }

                            },
                            error: function () {
                                $.unblockUI();
                            }
                        });
                    };
                },
                error: function () {
                    $.unblockUI();
                }
            });
        });

        function GetFechaFormat(fecha) {
            var curr_date = fecha.getDate();

            var curr_month = fecha.getMonth();

            var curr_year = fecha.getFullYear();


            return curr_date + "-" + curr_month + "-" + curr_year;
        }

        function getObject() {
            return {
                cc: cboListaCC.val(),
                fechaInicio: $("#tbfechaIni").val(),
                fechaFin: $("#tbfechaFin").val()
            }
        }

        function verModalImprimir() {

            dateNow = new Date();
            var day = dateNow.getDay(),
                diff = dateNow.getDate() - day + (day == 0 ? -6 : 3);
            FechaInicio = new Date(dateNow.setDate(diff));
            var dayFin = new Date(FechaInicio);
            difff = dayFin.getDate() + (2 + 7 - dayFin.getDay()) % 7;
            FechaFin = new Date(dayFin.setDate(difff));
            tbCCFiltro.val(cboListaCC.val());
            fechaInicio.datepicker().datepicker("setDate", new Date(FechaInicio));
            fechaFin.datepicker().datepicker("setDate", new Date(FechaFin));

            modalImpresion.modal('show');
        }

        function GetDataSave() {

            var arrayd = new Array();
            var tblEquiposObraData = $('#tblEquiposStandBy').bootgrid().data('.rs.jquery.bootgrid').rows;

            for (var i = 0; i < tblEquiposObraData.length; i++) {
                var objJson = {};
                var tempJson = tblEquiposObraData[i];

                objJson.id = tempJson.id;
                objJson.noEconomicoID = tempJson.noEconomicoID;
                objJson.FechaStandBy = tempJson.FechaStandBy;
                objJson.cc = tempJson.cc;
                objJson.Economico = tempJson.Economico;
                objJson.TipoConsideracion = tempJson.TipoConsideracion;
                arrayd.push(objJson);
            }

            return arrayd;
        }


        function LoadTabla() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/GetMaquinariaEnObraStandby",
                type: "POST",
                datatype: "json",
                data: { CC: cboListaCC.val(), fechaInicio: tbfechaIni.val(), fechaFinal: tbfechaFin.val() },
                success: function (response) {
                    $.unblockUI();
                    var listaEquiposObra = response.listaEquiposObra;

                    tblEquiposObra.bootgrid("clear");
                    tblEquiposStandBy.bootgrid("clear");
                    tblEquiposObra.bootgrid("append", listaEquiposObra);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function datePicker() {
            var now = new Date(),
            year = now.getYear() + 1900;
            $.datepicker.setDefaults($.datepicker.regional["es"]);
            var dateFormat = "dd/mm/yy",
              from = $("#tbfechaIni")
                .datepicker({
                    changeMonth: true,
                    changeYear: true,
                    numberOfMonths: 1,
                    defaultDate: new Date(year, 00, 01),
                    maxDate: new Date(),

                    onChangeMonthYear: function (y, m, i) {
                        var d = i.selectedDay;
                        $(this).datepicker('setDate', new Date(y, m - 1, d));
                        $(this).trigger('change');
                    }

                })
                .on("change", function () {
                    to.datepicker("option", "minDate", getDate(this));
                }),
              to = $("#tbfechaFin").datepicker({
                  changeMonth: true,
                  changeYear: true,
                  numberOfMonths: 1,
                  defaultDate: new Date(),
                  maxDate: new Date(),
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

        function initGrid() {
            tblEquiposStandBy.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header:
                "</div>" +
                "<div class='col-lg-12 col-md-8 col-xs-12 no-padding'> <button type='button' class='btn btn-primary pull-right' id='btnGuardarStandBy' style='margin-bottom: 5px;'> <span class='glyphicon glyphicon-floppy-disk'></span> Guardar</button> </div>" +
                    "</div>"
                },
                formatters: {
                    "setFechaStandbyA": function (column, row) {
                        if (row.FechaStandBy != undefined) {
                            return "<input type='text' class='form-control setFechaStandbyA' data-id=" + row.noEconomicoID + " value='" + row.FechaStandBy + "' />";
                        } else {
                            return "<input type='text' class='form-control setFechaStandbyA' data-id=" + row.noEconomicoID + "  />";
                        }
                    },
                    "TipoConsideracion": function (column, row) {
                        return "<select class='form-control cboTipoConsideracionS'  data-id=" + row.noEconomicoID + "> " +
                                    "<option value='1'>A</option>" +
                                    "<option value='2'>B</option>" +
                                    "<option value='3'>C</option>" +
                                    "<option value='4'>D</option>" +
                                    "<option value='5'>E</option>" +
                                "</select>";
                    },
                    "removeStandBy": function (column, row) {
                        return "<button type='button' class='btn btn-danger removeStandBy' data-id='" + row.noEconomicoID + "' >" +
                         "<span class='glyphicon glyphicon-remove '></span> " +
                    " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                SetConsideracionStandBy();
                $(".setFechaStandbyA").datepicker().datepicker();
                tblEquiposStandBy.find(".setFechaStandbyA").on("change", function (e) {

                    var noEconomicoID = Number($(this).attr('data-id'));
                    var tblEquiposObraData = $('#tblEquiposStandBy').bootgrid().data('.rs.jquery.bootgrid').rows;
                    Elemento = $(this);
                    var data = $.grep(tblEquiposObraData,
                      function (o, i) {
                          if (o.noEconomicoID == Number(noEconomicoID)) {
                              $('#tblEquiposStandBy').bootgrid().data('.rs.jquery.bootgrid').rows[i].FechaStandBy = $(Elemento).val();
                          }
                      },
                      true);

                });

                tblEquiposStandBy.find(".removeStandBy").on("click", function (e) {


                    var noEconomicoID = Number($(this).attr('data-id'));

                    if (StandbyOBj != null) {
                        DeleteRowBD(StandbyOBj.id, noEconomicoID);
                    }

                    var tblEquiposObraData = $('#tblEquiposObra').bootgrid().data('.rs.jquery.bootgrid').rows;
                    var tblEquiposStanByData = $("#tblEquiposStandBy").bootgrid().data('.rs.jquery.bootgrid').rows;
                    var objData = {};

                    objEconomico2 = $.grep(tblEquiposStanByData,
                                 function (o, i) {
                                     return o.noEconomicoID != Number(noEconomicoID);
                                 },
                                 true);

                    temp = objEconomico2[0];

                    tblEquiposObraData.push(temp);

                    tblEquiposStanByData = $.grep(tblEquiposStanByData,
                              function (o, i) { return o.noEconomicoID == Number(noEconomicoID); },
                              true);

                    tblEquiposObra.bootgrid("clear");
                    tblEquiposObra.bootgrid("append", tblEquiposObraData);

                    tblEquiposStandBy.bootgrid("clear");
                    tblEquiposStandBy.bootgrid("append", tblEquiposStanByData);

                });

                tblEquiposStandBy.find(".cboTipoConsideracionS").on("click", function (e) {

                    var noEconomicoID = Number($(this).attr('data-id'));
                    var tblEquiposObraData = $('#tblEquiposStandBy').bootgrid().data('.rs.jquery.bootgrid').rows;
                    Elemento = $(this);
                    var data = $.grep(tblEquiposObraData,
                      function (o, i) {
                          if (o.noEconomicoID == Number(noEconomicoID)) {
                              $('#tblEquiposStandBy').bootgrid().data('.rs.jquery.bootgrid').rows[i].TipoConsideracion = Number($(Elemento).val());
                          }
                      },
                      true);
                });

            });

            tblEquiposObra.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: "<div id=\"{{ctx.id}}\" class=\"{{css.header}}\"><div class=\"row\"><div class=\"col-sm-12 actionBar\"><button type='button' class='btn btn-success' id='btnAsignarTodo'> <span class='glyphicon glyphicon-plus'></span>Asignar Todo</button> <p class=\"{{css.search}}\"></p><p class=\"{{css.actions}}\"></p></div></div></div>"
                },
                formatters: {
                    "setFechaStandby": function (column, row) {
                        if (row.FechaStandBy != undefined) {
                            return "<input type='text' class='form-control setFechaStandby' data-id=" + row.noEconomicoID + "  value='" + row.FechaStandBy + "'/>";
                        }
                        else {
                            return "<input type='text' class='form-control setFechaStandby' data-id=" + row.noEconomicoID + " />";
                        }

                    },
                    "TipoConsideracion": function (column, row) {
                        return "<select class='form-control cboTipoConsideracion' data-id='" + row.noEconomicoID + "'> " +
                                    "<option value='1'>A</option>" +
                                    "<option value='2'>B</option>" +
                                    "<option value='3'>C</option>" +
                                    "<option value='4'>D</option>" +
                                    "<option value='5'>E</option>" +
                                "</select>";
                    },
                    "setStandby": function (column, row) {
                        return "<button type='button' class='btn btn-success setStandby' data-id='" + row.noEconomicoID + "' >" +
                         "<span class='glyphicon glyphicon-plus '></span> " +
                    " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                SetConsideracion();
                $(".setFechaStandby").datepicker().datepicker();

                dateNow = new Date();
                var day = dateNow.getDay(),
                    diff = dateNow.getDate() - day + (day == 0 ? 3 : 3);
                FechaInicio = new Date(dateNow.setDate(diff));
                var dayFin = new Date(FechaInicio);
                difff = dayFin.getDate() + (2 + 7 - dayFin.getDay()) % 7;
                FechaFin = new Date(dayFin.setDate(difff));

                y = FechaInicio.getFullYear();
                m = FechaInicio.getMonth() + 1;
                d = FechaInicio.getDate();
                tblEquiposObra.find(".setFechaStandby").on("change", function (e) {
                    var noEconomicoID = Number($(this).attr('data-id'));
                    var tblEquiposObraData = $('#tblEquiposObra').bootgrid().data('.rs.jquery.bootgrid').rows;
                    Elemento = $(this);
                    var data = $.grep(tblEquiposObraData,
                      function (o, i) {
                          if (o.noEconomicoID == Number(noEconomicoID)) {
                              $('#tblEquiposObra').bootgrid().data('.rs.jquery.bootgrid').rows[i].FechaStandBy = $(Elemento).val();
                          }
                      },
                      true);
                });

                tblEquiposObra.find(".cboTipoConsideracion").on("change", function (e) {
                    var noEconomicoID = Number($(this).attr('data-id'));
                    var tblEquiposObraData = $('#tblEquiposObra').bootgrid().data('.rs.jquery.bootgrid').rows;
                    Elemento = $(this);
                    var data = $.grep(tblEquiposObraData,
                      function (o, i) {
                          if (o.noEconomicoID == Number(noEconomicoID)) {
                              $('#tblEquiposObra').bootgrid().data('.rs.jquery.bootgrid').rows[i].TipoConsideracion = Number($(Elemento).val());
                          }
                      },
                      true);
                });

                tblEquiposObra.find(".setStandby").on("click", function (e) {
                    var noEconomicoID = Number($(this).attr('data-id'));

                    if ($(this).parents('tr').find('.setFechaStandby').val() != "") {


                        var tblEquiposObraData = $('#tblEquiposObra').bootgrid().data('.rs.jquery.bootgrid').rows;
                        var tblEquiposStanByData = $("#tblEquiposStandBy").bootgrid().data('.rs.jquery.bootgrid').rows;
                        var objData = {};

                        objEconomico = $.grep(tblEquiposObraData,
                                     function (o, i) {
                                         return o.noEconomicoID != Number(noEconomicoID);
                                     },
                                     true);

                        temp = objEconomico[0];

                        tblEquiposStanByData.push(temp);

                        tblEquiposObraData = $.grep(tblEquiposObraData,
                                  function (o, i) { return o.noEconomicoID == Number(noEconomicoID); },
                                  true);

                        tblEquiposObra.bootgrid("clear");
                        tblEquiposObra.bootgrid("append", tblEquiposObraData);

                        tblEquiposStandBy.bootgrid("clear");
                        tblEquiposStandBy.bootgrid("append", tblEquiposStanByData);

                        AlertaGeneral('Confirmacion', 'El registro fue agregado correctamente esta listo para ser guardado.')
                    } else {
                        AlertaGeneral('Confirmacion', 'Debe seleccionar una fecha para poder agregar un equipo')
                    }
                });

            });

        }

        function DeleteRowBD(idStanby, idEconomico) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/DeleteRow",
                type: "POST",
                datatype: "json",
                data: { objStanby: idStanby, objEconomico: idEconomico },
                success: function (response) {
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function SetConsideracion() {
            var tblEquiposObraData = $('#tblEquiposObra').bootgrid().data('.rs.jquery.bootgrid').rows;

            for (var i = 0; i < tblEquiposObraData.length; i++) {

                if (tblEquiposObraData[i].TipoConsideracion != 1) {
                    $("select[data-id='" + tblEquiposObraData[i].noEconomicoID + "']").val(tblEquiposObraData[i].TipoConsideracion)
                }
            }
        }

        function SetConsideracionStandBy() {
            var tblEquiposObraData = $('#tblEquiposStandBy').bootgrid().data('.rs.jquery.bootgrid').rows;

            for (var i = 0; i < tblEquiposObraData.length; i++) {

                if (tblEquiposObraData[i].TipoConsideracion != 1) {
                    $("select[data-id='" + tblEquiposObraData[i].noEconomicoID + "']").val(tblEquiposObraData[i].TipoConsideracion)
                }
            }
        }

        function initGrid2() {

            tblEquiposStandBylist.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: "",
                },
                formatters: {
                    "btnReporte": function (column, row) {

                        return "<button type='button' class='btn btn-primary btnReporte' data-id='" + row.id + "' data-CC='" + row.CC + "' >" +
                           "<span class='glyphicon glyphicon glyphicon-print'></span> " +
                                  " </button>";
                    },
                    "btnEdit": function (column, row) {

                        return "<button type='button' class='btn btn-warning btnEdit' data-id='" + row.id + "' data-CC='" + row.CC + "' data-EstatusGerente = '" + row.estatus + "' " +
                            " data-Elabora1 = '" + row.Elabora1 + "' data-Elabora2 = '" + row.Elabora2 + "' data-autoriza ='" + row.validaVg + "' >" +
                          "<span class='glyphicon glyphicon-edit'></span> " +
                                 " </button>"
                        ;
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */

                tblEquiposStandBylist.find(".btnEdit").on("click", function (e) {

                    var pID = $(this).attr('data-id');
                    setValidacion(pID);
                });
                tblEquiposStandBylist.find(".btnReporte").on("click", function (e) {
                    var pID = $(this).attr('data-id');
                    var idReporte = 47;
                    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pID=" + pID + "&size=2";
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                });
            });
        }


        function setValidacion(idStandby) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/GetMaquinariaEnObraStandbyEdit",
                type: "POST",
                datatype: "json",
                data: { obj: idStandby },
                success: function (response) {
                    $.unblockUI();
                    CC = response.CC;
                    StandbyOBj = response.StandbyOBj;
                    var listaEquiposObra = response.listaEquiposObra;
                    var listaEquiposStandby = response.listaEquiposObraStandby;
                    tblEquiposObra.bootgrid("clear");

                    tblEquiposStandBy.bootgrid("clear");

                    if (listaEquiposStandby != undefined) {
                        tblEquiposStandBy.bootgrid("append", listaEquiposStandby);
                    }
                    if (listaEquiposObra != undefined) {
                        tblEquiposObra.bootgrid("append", listaEquiposObra);
                    }

                    if (StandbyOBj != undefined) {
                        cboListaCC.val(StandbyOBj.CC)
                        cboListaCC.prop('disabled', true);
                    }

                    divCaptura.removeClass('hide');
                    divPrincipal.addClass('hide');
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        init();
        initGrid();
        initGrid2();

    };

    $(document).ready(function () {

        maquinaria.MovimientoMaquinaria.Standby = new Standby();

    });
})();

