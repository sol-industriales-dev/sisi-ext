(function () {
    $.namespace('maquinaria.inventario.FichaTecnica');
    FichaTecnica = function () {
        mensajes = {
            NOMBRE: 'Asignacion de Equipo',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        //Declarar variables
        const divFactura = $("#divFactura");
        const divPedimento = $("#divPedimento");
        const divpolizaSeguros = $("#divpolizaSeguros");
        const divtarjetaCirculacion = $("#divtarjetaCirculacion");
        const divPermisoEspecial = $("#divPermisoEspecial");
        const divCertificado = $("#divCertificado");
        const txtFiltroNoEconomico = $('#txtFiltroNoEconomico');
        const ireport = $("#report");
        const NoEconomico = $("#NoEconomico");
        const txtDescripcion = $("#txtDescripcion");
        const txtMarca = $("#txtMarca");
        const txtModelo = $("#txtModelo");
        const txtNoSerie = $("#txtNoSerie");
        const txtAnio = $("#txtAnio");
        const dateFechaCompra = $("#dateFechaCompra");
        const txtPropietario = $("#txtPropietario");
        const txtUbicacion = $("#txtUbicacion");
        const txtHorometroInicio = $("#txtHorometroInicio");
        const txtHorometroActual = $("#txtHorometroActual");
        const txtUltimoParo = $("#txtUltimoParo");
        const txtDesParo = $("#txtDesParo");
        const txtCostAdquisicion = $("#txtCostAdquisicion");
        const txtCostOverActivo = $("#txtCostOverActivo");
        const txtCostOverAplicado = $("#txtCostOverAplicado");
        const btnImprimir = $("#btnImprimir");
        const EconomicoDesripcion = originURL('/AltaBajaEquipos/EconomicoDesripcion');
        (() => {
            var economico = $.urlParam('idEconomico');
            if (economico != null) {
                FichaEconomico(economico);
            }
            txtFiltroNoEconomico.getAutocompleteValid(BuscarFicha, EconomicoCambio, null, EconomicoDesripcion.href)
            btnImprimir.click(clickImprimir);
            divFactura.click(downloadURI);
            divPedimento.click(downloadURI);
            divpolizaSeguros.click(downloadURI);
            divtarjetaCirculacion.click(downloadURI);
            divPermisoEspecial.click(downloadURI);
            divCertificado.click(downloadURI);
        })();
        function EconomicoCambio(e, ui) {
            if ((ui.item === null && $(this).val() != '')) {
            }
        }
        function BuscarFicha() {
            axios.post('/AltaBajaEquipos/BuscarFicha', { obj: txtFiltroNoEconomico.val() })
                .then(response => {
                    if (response.data.descripcion.length > 0) {
                        NoEconomico.val(response.data.noEconomico);
                        txtDescripcion.val(response.data.descripcion);
                        txtMarca.val(response.data.marca);
                        txtModelo.val(response.data.modelo);
                        txtNoSerie.val(response.data.noSerie);
                        txtAnio.val(response.data.anio);
                        dateFechaCompra.val(response.data.fechaCompra);
                        txtHorometroInicio.val(response.data.horometroInicio);
                        txtHorometroActual.val(response.data.horometroActual);
                        txtUltimoParo.val(response.data.fechaParo);
                        txtDesParo.val(response.data.detParo);
                        txtUbicacion.val(response.data.ubicacion);
                        txtCostAdquisicion.val(response.data.costoAdquisicion);
                        txtCostOverActivo.val(response.data.costoOverHaul);
                        txtCostOverAplicado.val(response.data.costoOverHaulAplicado);
                        response.data.factura ? SetInfoDocumento(divFactura, true, response.data.facturaID) : SetInfoDocumento(divFactura, false, 0);
                        response.data.pedimento ? SetInfoDocumento(divPedimento, true, response.data.pedimentoID) : SetInfoDocumento(divPedimento, false, 0);
                        response.data.poliza ? SetInfoDocumento(divpolizaSeguros, true, response.data.polizaID) : SetInfoDocumento(divpolizaSeguros, false, 0);
                        response.data.tarjetaCirculacion ? SetInfoDocumento(divtarjetaCirculacion, true, response.data.tarjetaCirculacionID) : SetInfoDocumento(divtarjetaCirculacion, false, 0);
                        response.data.permisoCarga ? SetInfoDocumento(divPermisoEspecial, true, response.data.permisoCargaID) : SetInfoDocumento(divPermisoEspecial, false, 0);
                        response.data.certificacion ? SetInfoDocumento(divCertificado, true, response.data.certificacionID) : SetInfoDocumento(divCertificado, false, 0);
                        txtPropietario.val("CONSTRUPLAN");
                        btnImprimir.show();
                    }
                });
        }

        function SetInfoDocumento(elemento, bandera, id) {
            if (bandera) {
                elemento.addClass('Activo');
                elemento.removeClass('Pendiente');
                divFactura.attr('data-id', id);
            }
            else {
                elemento.removeClass('Activo');
                elemento.addClass('Pendiente');
                elemento.removeAttr("data-id");
            }
        }

        function reset() {
            divFactura.removeClass('Activo');
            divFactura.removeClass('Pendiente');
            divFactura.removeAttr("data-id");

            divPedimento.removeClass('Activo');
            divPedimento.removeClass('Pendiente');
            divPedimento.removeAttr("data-id");

            divpolizaSeguros.removeClass('Activo');
            divpolizaSeguros.removeClass('Pendiente');
            divpolizaSeguros.removeAttr("data-id");

            divtarjetaCirculacion.removeClass('Activo');
            divtarjetaCirculacion.removeClass('Pendiente');
            divtarjetaCirculacion.removeAttr("data-id");

            divPermisoEspecial.removeClass('Activo');
            divPermisoEspecial.removeClass('Pendiente');
            divPermisoEspecial.removeAttr("data-id");

            divCertificado.removeClass('Activo');
            divCertificado.removeClass('Pendiente');
            divCertificado.removeAttr("data-id");

        }
        function FichaEconomico(idEconomico) {
            axios.post('/AltaBajaEquipos/GetFiltros', { obj: idEconomico })
                .then(response => {
                    let { success, noEconomico } = response.data;
                    if (success) {
                        txtFiltroNoEconomico.val(noEconomico);
                        BuscarFicha();
                    }
                });
        }
        function clickImprimir(e) {
            VerReporte();
            e.preventDefault();
        }
        function txtEconomicoData() {
            return txtFiltroNoEconomico.data().uiAutocomplete.selectedItem;
        }
        function VerReporte() {
            let economicoData = txtEconomicoData();
            axios.post('/CatMaquina/getMaquinaFichaTecnica', { id: +economicoData.id })
                .then(response => {
                    $.blockUI({ message: mensajes.PROCESANDO });
                    if (response.data.success) {
                        var path = "/Reportes/Vista.aspx?idReporte=25"
                        ireport.attr("src", path);
                        document.getElementById('report').onload = function () {
                            openCRModal();
                            $.unblockUI();
                        };
                    }
                }).catch(o_O => AlertaGeneral("Alerta", o_O.message));
        }
        function downloadURI() {
            elemento = $(this).attr('data-id');
            if (elemento != undefined) {
                var link = document.createElement("button");
                link.download = '/CatInventario/getFileDownload?id=' + elemento;
                link.href = '/CatInventario/getFileDownload?id=' + elemento;
                link.click();
                location.href = '/CatInventario/getFileDownload?id=' + elemento;
            }
            else {
                AlertaGeneral('Alerta', 'No hay archivos asignados para ese tipo de documento')
            }
        }
    };
    $(document).ready(function () {
        maquinaria.inventario.FichaTecnica = new FichaTecnica();
    });
})();