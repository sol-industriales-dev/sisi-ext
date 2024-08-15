(function () {
    $.namespace('maquinaria.Reportes.RepConsumoLubricantes');
    RepConsumoLubricantes = function () {
        mensajes = { PROCESANDO: 'Procesando...' };

        btnImprimirDetalle = $("#btnImprimirDetalle"),
            modalDetalle = $("#modalDetalle"),
            tblDetalleLubricantes = $("#tblDetalleLubricantes"),
            cboTiposLubricantes = $("#cboTiposLubricantes");
        dpBusqFecha = $("#dpBusqFecha");
        cboBusqCC = $("#cboBusqCC");
        txtCCNombre = $("#txtCCNombre");
        cboBusqTurno = $("#cboBusqTurno");
        dpBusqInicio = $("#dpBusqInicio");
        dpBusqFin = $("#dpBusqFin");
        cboBusqEconomico = $("#cboBusqEconomico");
        btnBuscar = $("#btnBuscar");
        btnImprimir = $("#btnImprimir");
        tblLubricante = $("#tblLubricante");
        ireport = $("#report");
        const mdlLubricante = $('#mdlLubricante');
        let dtlubricante;
        const btnLubricantesTotales = $('#btnLubricantesTotales');
        const tblCanLubricates = $('#tblCanLubricates');
        function init() {

            initElementos();
            initTabla();
            btnBuscar.click(tblConsumoMaqAceiteLubricante);
            btnImprimir.click(verReporte);
            btnImprimirDetalle.click(verReporteDetalle);
            btnLubricantesTotales.click(function (e) {
                tblCatidadLubricante();
            });


            grid();


            //   cboBusqEconomico.change(GetLubricantesEquipo);

        }

        function initElementos() {
            var now = new Date(),
                year = now.getYear() + 1900;
            dpBusqInicio.datepicker().datepicker("setDate", new Date(year, now.getMonth(), 1));
            dpBusqFin.datepicker().datepicker("setDate", new Date(year, now.getMonth() + 1, 0));
            cboBusqCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
            cboBusqEconomico.fillCombo('/Horometros/cboModalEconomico', { obj: $("#cboBusqCC").val() == undefined ? 0 : $("#cboBusqCC").val() });
            cboBusqCC.change(setCC);
        }

        function setCC() {
            var cc = $("#cboBusqCC").val();
            if (cc == "--Seleccione--") {
                cc = "";
                txtCCNombre.val("");
                cboBusqEconomico.fillCombo('/Horometros/cboModalEconomico', { obj: cc == undefined ? "" : cc });
            }
            else {
                txtCCNombre.val(cboBusqCC.val());
                cboBusqEconomico.fillCombo('/Horometros/cboModalEconomico', { obj: cc == undefined ? "" : cc });
            }
        }

        function initTabla() {
            tblLubricante.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                rowCount: -1,
                templates: {
                    header: ""
                },
                formatters: {
                    "VerDetalle": function (column, row) {
                        return "<button type='button' class='btn btn-warning verDetalle' data-index='" + row.ECONOMICO + "' data-fechaInicio='" + dpBusqInicio.val() + "' data-fechaFin='" + dpBusqFin.val() + "'>" +
                            "<span class='glyphicon glyphicon-eye-open'></span> " +
                            " </button>";
                    },
                    "MOTOR": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.MOTOR + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.motorVal + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "MOTOR2": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.MOTOR2 + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.motor2Val + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "TRANS": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.TRANS + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.transVal + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "HCO": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.HCO + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.hcoVal + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "DIF": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.DIF + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.difVal + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "MF": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.MF + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.mfVal + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "DIR": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.DIR + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.dirVal + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "GRASA": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.GRASA + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.GrasaVal + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "OTROS1": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.otros1Des + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.otros1Val + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "OTROS2": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.otros2Des + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.otros2Val + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "OTROS3": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.otros3Des + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.otros3Val + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "OTROS4": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.otros4Des + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.otros4Val + "</label></div>" +
                            " </div>";

                        return data;
                    }

                }
            }).on("loaded.rs.jquery.bootgrid", function () {

                tblLubricante.find(".verDetalle").on("click", function (e) {
                    var Economico = $(this).attr('data-index');
                    var FechaInico = $(this).attr('data-fechaInicio');
                    var FechaFin = $(this).attr('data-fechaFin');
                    var CentroCostos = cboBusqCC.val();
                    verDetalle(Economico, FechaInico, FechaFin, CentroCostos);

                });
            });

        }
        // let motor = "";
        // let dir = "";
        // let valMotor = 0;
        // let valdir = 0;
        // let total = 0;

        // motor = row.MOTOR;
        // dir = row.DIR;
        // valMotor = row.motorVal;
        // valdir = row.dirVal;

        // if (motor == dir) {
        //     total = valMotor + valdir;
        // }
        // else{
        //     total = valMotor;
        // }


        // return total;
        function grid() {
            tblDetalleLubricantes.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "MOTOR": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.motorDes + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.MOTOR + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "TRANS": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.transDes + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.TRANS + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "HCO": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.hcoDes + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.HCO + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "DIF": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.dirDes + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.DIF + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "MF": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.mfDes + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.MF + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "DIR": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.dirDes + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.DIR + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "GRASA": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.grasaDes + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.GRASA + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "otros1": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.otros1Des + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.otros1 + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "otros2": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.otros2Des + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.otros2 + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "otros3": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.otros3Des + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.otros3 + "</label></div>" +
                            " </div>";

                        return data;
                    },
                    "otros4": function (column, row) {

                        var data = "<div class='row text-center'> " +
                            " <div class='col-lg-12' style='background: #EB984E;'><lable>" + row.otros4Des + "</label></div>" +
                            " <div class='col-lg-12'><lable>" + row.otros4 + "</label></div>" +
                            " </div>";

                        return data;
                    }


                }
            });
        }
        function verDetalle(Economico, FechaInico, FechaFin, CC) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/AceitesLubricantes/getDetalleLubricantes",
                type: "POST",
                datatype: "json",
                data: { cc: CC, inicio: FechaInico, fin: FechaFin, economico: Economico },
                success: function (response) {

                    modalDetalle.modal('show');

                    tblDetalleLubricantes.bootgrid("clear");
                    tblDetalleLubricantes.bootgrid("append", response.rptList);
                    tblDetalleLubricantes.bootgrid('reload');
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function tblConsumoMaqAceiteLubricante() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/AceitesLubricantes/tblRepMaqAceiteLubricante",
                type: "POST",
                datatype: "json",
                data: { cc: $("#cboBusqCC").val(), turno: cboBusqTurno.val(), inicio: dpBusqInicio.val(), fin: dpBusqFin.val(), economico: cboBusqEconomico.val() },
                success: function (response) {
                    if (response.success) {
                        tblLubricante.bootgrid("clear");
                        if (response.lstMaq.length > 0) {
                            tblLubricante.bootgrid("append", response.lstMaq);
                            btnImprimir.removeClass("hidden");
                        } else {
                            btnImprimir.addClass("hidden");
                        }
                    }
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function verReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = "50";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function verReporteDetalle() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = "63";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        //#region TABLA DINAMICA 
        function tblCatidadLubricante() {
            $.blockUI({ message: mensajes.PROCESANDO });

            $.ajax({
                url: "/AceitesLubricantes/CatidadLubricante",
                type: "POST",
                datatype: "json",
                data: { cc: $("#cboBusqCC").val(), turno: cboBusqTurno.val(), inicio: dpBusqInicio.val(), fin: dpBusqFin.val(), economico: cboBusqEconomico.val() },
                success: function (response) {
                    let { success, items } = response;
                    console.log(response)
                    if (success) {
                        //AddRows(tblCanLubricates, items);
                        initCantLubricante(items);
                        mdlLubricante.modal("show");
                    }
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function ColumnasDinamicas(data) {

            var columnas =
                [
                    { data: "noEconomico", title: "Economico" }
                ];
            $.each(data[0].lstAceites, function (key, value) {
                var auxColumna = {
                    data: "lstAceites", title: "<th>" + value.insumo + "</th>",
                    render: function (data) {
                        if (data[key] == undefined) {
                            return 0;
                        }
                        else {
                            return parseInt(data[key].total);
                        }
                    },
                }
                columnas.push(auxColumna);

            });
            var columns = { data: "TotalFila", title: "TOTAL GENERAL" }
            columnas.push(columns);



            return columnas
        }


        function initCantLubricante(data) {
            if (dtlubricante != null) {
                dtlubricante.destroy();
            }
            tblCanLubricates.empty();
            dtlubricante = tblCanLubricates.DataTable({
                language: dtDicEsp,
                ordering: false,
                paging: false,
                ordering: true,
                bFilter: false,
                info: true,
                dom: 'Bfrtip',
                data: data,
                buttons: parametrosImpresion("Lista de totales", "<center><h3>Lista de totales</h3></center>"),
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,

                    }
                ],

                columns: ColumnasDinamicas(data), data,


            });
        }
        //#endregion       

        init();
    };

    $(document).ready(function () {
        maquinaria.Reportes.RepConsumoLubricantes = new RepConsumoLubricantes();
    });
})();