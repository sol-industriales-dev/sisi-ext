(function () {
    $.namespace('maquinaria.catalogo.BajaActivoFijo');
    BajaActivoFijo = function () {
        mensajes = {
            NOMBRE: 'Inventario',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        cboTipo = $("#cboTipo");
        dpInicio = $("#dpInicio");
        dpFin = $("#dpFin");
        tblInventario = $("#tblInventario");
        btnBuscar = $("#btnBuscar");
        btnImprimir = $("#btnImprimir");
        ireport = $("#report");

        lblTtlNoAsignado = $("#lblTtlNoAsignado");
        lblTtlExterna = $("#lblTtlExterna");
        lblTtlInterna = $("#lblTtlInterna");
        lblTtlTermino = $("#lblTtlTermino");
        lblTtlSiniestro = $("#lblTtlSiniestro");
        lblTtlRobo = $("#lblTtlRobo");
        lblP2NoAsignado = $("#lblP2NoAsignado");
        lblP2Externa = $("#lblP2Externa");
        lblP2Interna = $("#lblP2Interna");
        lblP2Termino = $("#lblP2Termino");
        lblP2Siniestro = $("#lblP2Siniestro");
        lblP2Robo = $("#lblP2Robo");

        function init() {
            initElementos();
            initTblInventario();
            btnBuscar.click(cargarMaquinaria);
            btnImprimir.click(verReporte);
        }

        function initElementos() {
            cboTipo.fillCombo('/CatMaquina/FillCboTipoBaja', null, false, null);
            dpInicio.datepicker().datepicker("setDate", new Date());
            dpFin.datepicker().datepicker("setDate", new Date());
        }

        function initTblInventario() {
            tblInventario.bootgrid({
                headerCssClass: '.bg-table-header',
                align: 'center',
                templates: {
                    header: ""
                },
                formatters: {
                    "Promedio": function (column, row) {
                        return row.Promedio;
                    },
                    "NoAsignado": function (column, row) {
                        return row.NoAsignado ? '<span class="glyphicon glyphicon glyphicon-ok-sign"></span>' : "";
                    },
                    "VentaInterna": function (column, row) {
                        return row.VentaInterna ? '<span class="glyphicon glyphicon glyphicon-ok-sign"></span>' : "";
                    },
                    "VentaExterna": function (column, row) {
                        return row.VentaExterna ? '<span class="glyphicon glyphicon glyphicon-ok-sign"></span>' : "";
                    },
                    "TerminoVida": function (column, row) {
                        return row.TerminoVida ? '<span class="glyphicon glyphicon glyphicon-ok-sign"></span>' : "";
                    },
                    "Siniestro": function (column, row) {
                        return row.Siniestro ? '<span class="glyphicon glyphicon glyphicon-ok-sign"></span>' : "";
                    },
                    "Robo": function (column, row) {
                        return row.Robo ? '<span class="glyphicon glyphicon glyphicon-ok-sign"></span>' : "";
                    },

                }
            });
        }

        function cargarMaquinaria() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/CatInventario/cargarMaquinariaActivoFijo',
                data: { inicio: dpInicio.val(), fin: dpFin.val(), tipo: cboTipo.val() == "" ? 0 : cboTipo.val() },
                success: function (response) {
                    if (response.success) {
                        tblInventario.bootgrid("clear");
                        if (response.lstBajaMaquina.length > 0) {
                            tblInventario.bootgrid("append", response.lstBajaMaquina);
                            response.lstBajaMaquina.length > 0 ? btnImprimir.removeClass("hidden") : btnImprimir.addClass("hidden");
                            setTotalRelativo(response.lstContador, response.lstRelativo)
                        }
                    }
                    $.unblockUI();
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function setTotalRelativo(cont, rel) {
            lblTtlNoAsignado.text(cont.NoAsignado);
            lblTtlExterna.text(cont.VentaInterna);
            lblTtlInterna.text(cont.VentaExterna);
            lblTtlTermino.text(cont.TerminoVida);
            lblTtlSiniestro.text(cont.Siniestro);
            lblTtlRobo.text(cont.Robo);
            lblP2NoAsignado.text(rel.NoAsignado);
            lblP2Externa.text(rel.VentaInterna);
            lblP2Interna.text(rel.VentaExterna);
            lblP2Termino.text(rel.TerminoVida);
            lblP2Siniestro.text(rel.Siniestro);
            lblP2Robo.text(rel.Robo);
        }

        function verReporte() {
            $.blockUI({ message: mensajes.PROCESANDO });
            var idReporte = "52";
            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte;
            ireport.attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        init();
    };
    $(document).ready(function () {
        maquinaria.catalogo.BajaActivoFijo = new BajaActivoFijo();
    });
})();
