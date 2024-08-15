(function () {
    $.namespace('maquinaria.catalogo.BajaMaquinaria');
    BajaMaquinaria = function () {
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

        function init() {
            initElementos();
            btnBuscar.click(cargarMaquinaria);
            btnImprimir.click(verReporte);
        }

        function initElementos() {
            cboTipo.fillCombo('/CatMaquina/FillCboTipoBaja', null, false, null);
            dpInicio.datepicker().datepicker("setDate", new Date());
            dpFin.datepicker().datepicker("setDate", new Date());
        }

        function cargarMaquinaria() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/CatInventario/cargarMaquinaria',
                data: { inicio: dpInicio.val(), fin: dpFin.val(), tipo: cboTipo.val() == "" ? 0 : cboTipo.val() },
                success: function (response) {
                    if (response.success) {
                        tblInventario.bootgrid("clear");
                        if (response.lstBajaMaquina.length > 0) {
                            tblInventario.bootgrid("append", response.lstBajaMaquina);
                            response.lstBajaMaquina.length > 0 ? btnImprimir.removeClass("hidden") : btnImprimir.addClass("hidden");
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
            var idReporte = "48";
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
        maquinaria.catalogo.BajaMaquinaria = new BajaMaquinaria();
    });
})();
