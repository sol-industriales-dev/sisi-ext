
(function () {

    $.namespace('maquinaria.movimientoMaquinaria.ReporteStandBY');

    ReporteStandBY = function () {
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Autorizacion de Solicitudes Reemplazo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };

        listaTabla = [];

        txtCC = $("#txtCC"),
        divGerente = $("#divGerente"),
        lblElaboro2 = $("#lblElaboro2"),
        lblElaboro1 = $("#lblElaboro1"),

        BntRegresar = $("#BntRegresar"),
        ppal = $("#ppal"),
        vistaReporte = $("#vistaReporte");
        report2 = $("#report2");
        ireport = $("#report");
        tblEquiposStandBy = $("#tblEquiposStandBy"),
        cboEstatusFiltro = $("#cboEstatusFiltro"),
        tbFechaInicio = $("#tbFechaInicio"),
        tbFechaFin = $("#tbFechaFin");

        function init() {
            tbFechaInicio.datepicker().datepicker("setDate", new Date());
            tbFechaFin.datepicker().datepicker("setDate", new Date());
            loadTable();
            initGrid();
            BntRegresar.click(Regresar);
            vistaReporte.addClass('hide');
            cboEstatusFiltro.change(setFiltros);
            txtCC.fillCombo('/MovimientoMaquinaria/cboGetCentroCostos');
            txtCC.change(filtroCC);
        }

        function filtroCC() {

            if (txtCC.val() != "") {
                var ListaTemp = $.grep(listaTabla,
                 function (o, i) { return o.cc == txtCC.val() },
             false);
                tblEquiposStandBy.bootgrid("clear");
                tblEquiposStandBy.bootgrid("append", ListaTemp);
            }
            else {
                tblEquiposStandBy.bootgrid("clear");
                tblEquiposStandBy.bootgrid("append", listaTabla);
            }

        }

        function setFiltros() {
            loadTable();
        }

        function Regresar() {
            vistaReporte.addClass('hide');
            ppal.removeClass('hide');
        }

        function loadTable() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/ValidarStandBy",
                type: "POST",
                datatype: "json",
                data: { filtro: cboEstatusFiltro.val(), fechaInicio: tbFechaInicio.val(), fechaFin: tbFechaFin.val() },
                success: function (response) {
                    $.unblockUI();
                    var data = response.StandyByLista;
                    listaTabla = data;
                    tblEquiposStandBy.bootgrid("clear");
                    tblEquiposStandBy.bootgrid("append", data);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function LoadReporte() {
            var idReporte = 47;
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pID=" + 5;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();

            };
        }

        function initGrid() {

            tblEquiposStandBy.bootgrid({
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
                    "btnValida": function (column, row) {
                        if (row.validaV || row.validaVg) {
                            return "<button type='button' class='btn btn-info btnValida' data-id='" + row.id + "' data-CC='" + row.CC + "' data-EstatusGerente = '" + row.estatus + "' " +
                                " data-Elabora1 = '" + row.Elabora1 + "' data-Elabora2 = '" + row.Elabora2 + "' data-autoriza ='" + row.validaVg + "' data-desauto ='" + row.desauto + "' >" +
                              "<span class='glyphicon glyphicon-eye-open'></span> " +
                                     " </button>"
                            ;
                        }
                    }
                }
            }).on("loaded.rs.jquery.bootgrid", function () {
                /* Executes after data is loaded and rendered */

                tblEquiposStandBy.find(".btnValida").on("click", function (e) {
                    var pID = $(this).attr('data-id');
                    var Gerente = $(this).attr('data-autoriza');
                    var desauto = $(this).attr('data-desauto');

                    var nombreElabora1 = $(this).attr('data-Elabora1');
                    var nombreElabora2 = $(this).attr('data-Elabora2');
                    lblElaboro1.text(nombreElabora1);
                    lblElaboro2.text(nombreElabora2);
                    var pEstatusGerente = $(this).attr('data-EstatusGerente');
                    var idReporte = 47;
                    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pID=" + pID + "&size=1";
                    report2.attr("src", path);
                    document.getElementById('report2').onload = function () {
                        $.unblockUI();
                        vistaReporte.removeClass('hide');
                        ppal.addClass('hide');
                        setBtnAutorizaciones(pEstatusGerente, pID, Gerente, desauto);

                    };
                });


                tblEquiposStandBy.find(".btnReporte").on("click", function (e) {
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

        $(document).on('click', "#btnAutorizacion", function () {
            var id = $(this).attr('data-id');
            saveOrUpdate(Number(id), 1);
        });

        $(document).on('click', "#btnDesAutorizacion", function () {
            var id = $(this).attr('data-id');
            saveOrUpdate(Number(id), 2);
        });

        function saveOrUpdate(id, flag) {

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: "/MovimientoMaquinaria/saveOrUpdateAutorizacionStandby",
                type: "POST",
                datatype: "json",
                data: { obj: id, estatus: flag },
                success: function (response) {
                    $.unblockUI();
                    setBtnAutorizaciones(flag, id);
                    var idReporte = 47;
                    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&pID=" + id + "&size=1";
                    report2.attr("src", path);
                    document.getElementById('report2').onload = function () {
                        $.unblockUI();
                        loadTable();

                    };
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setBtnAutorizaciones(estatus, pID, gerente, desauto) {

            elemento = $("#btnGerente");
            elemento.children().remove();

            if (estatus == "0") {
                if (gerente == "true") {
                    var btnsControl = "<div class='row'> <div class='col-lg-12 col-xs-12' id='divAccionesAutorizacion'> <div class='col-xs-6'><button class='form-control btn btn-block colorAutoriza' id='btnAutorizacion' data-id='" + pID + "' >VALIDAR</button></div>" +
                         "</div></div>"

                    elemento.append(btnsControl);
                }
                elemento.next().addClass('panel-footer-Pendiente').html("Pendiente");
                elemento.next().removeClass('panel-footer-Rechazo');
                elemento.next().removeClass('panel-footer-Autoriza');
            } else if (estatus == "1") {

                if (desauto == "true")
                {
                    var btnsControl = "<div class='row'> <div class='col-lg-12 col-xs-12' id='divAccionesAutorizacion'> <div class='col-xs-6'><button class='form-control btn btn-block colorAutoriza' id='btnDesAutorizacion' data-id='" + pID + "' >DESAUTORIZAR</button></div>" +
                             "</div></div>"

                    elemento.append(btnsControl);
                }
                else {
                    elemento.children().remove();
                    elemento.next().removeClass('panel-footer-Rechazo');
                    elemento.next().removeClass('panel-footer-Pendiente');
                    elemento.next().addClass('panel-footer-Autoriza').html("Autorizado");
                    elemento.removeClass('btn btn-block');
                    elemento.attr('data-Autorizado', true);
                    elemento.removeClass('bg-primary');
                    elemento.removeClass('noPadding');
                }
              

            } else if (estatus == "2") {
                elemento.children().remove();
                elemento.next().removeClass('panel-footer-Autoriza');
                elemento.next().removeClass('panel-footer-Pendiente');
                elemento.next().addClass('panel-footer-Rechazo').html("Rechazado");
                elemento.removeClass('noPadding');
                elemento.attr('data-Autorizado', false);
            }

        }


        init();
    };

    $(document).ready(function () {
        maquinaria.movimientoMaquinaria.ReporteStandBY = new ReporteStandBY();
    });
})();

