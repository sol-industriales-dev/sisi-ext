
(function () {

    $.namespace('maquinaria.MovimientosInternos');

    MovimientosInternos = function () {
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

        var idGlobal = 0;
        tbFecha = $("#tbFecha"),
            cboEnvio = $("#cboEnvio"),
            cboRecepcion = $("#cboRecepcion"),
            tbHorometros = $("#tbHorometros"),
            tbCombustible = $("#tbCombustible"),
            tbFolio = $("#tbFolio"),
            cboEconomicos = $("#cboEconomicos"),
            tbDescripcion = $("#tbDescripcion"),
            tbMarca = $("#tbMarca"),
            tbModelo = $("#tbModelo"),
            tbSerie = $("#tbSerie"),
            tbComentario = $("#tbComentario"),
            tbBaterias = $("#tbBaterias"),
            tbMarca2 = $("#tbMarca2"),
            tbSerie2 = $("#tbSerie2"),
            tbRegistro = $("#tbRegistro"),
            tbGuardarControl = $("#tbGuardarControl");

        function init() {
            //  loadFolio();
            tbFecha.datepicker().datepicker("setDate", new Date());
            cboEnvio.change(LoadEconomicos);
            cboEconomicos.change(LoadDataEconomico);
            //cboEconomicos.fillCombo('/MovimientosInternos/FillCboEconomicos', { obj: Number(cboEnvio.val()) });
            cboEnvio.fillCombo('/MovimientosInternos/FillCboCentroCostos', { TipoCbo: Number(1) });
            cboRecepcion.fillCombo('/MovimientosInternos/FillCboEconomicosRecepcion', { obj: cboEnvio.val() });

            tbGuardarControl.click(GuardarInfomarcion);
            tbFolio.prop('disabled', true);
            tbFecha.prop('disabled', true);
            tbDescripcion.prop('disabled', true);
            tbMarca.prop('disabled', true);
            tbModelo.prop('disabled', true);
            tbSerie.prop('disabled', true);
            cboRecepcion.change(validadIgual);

        }

        function validadIgual() {
            if (cboRecepcion.val() == cboEnvio.val()) {
                cboRecepcion.val('0');
            }
        }

        function LoadEconomicos() {
            loadFolio();
            cboRecepcion.val('0');
            cboEconomicos.fillCombo('/MovimientosInternos/FillCboEconomicos', { cc: cboEnvio.val() }, null, null, () => {
                cboEconomicos.select2();
            });
            cboRecepcion.fillCombo('/MovimientosInternos/FillCboEconomicosRecepcion', { obj: cboEnvio.val() });
        }


        function LoadDataEconomico() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/MovimientosInternos/DataEconomico",
                data: { id: Number(cboEconomicos.val()) },
                success: function (response) {
                    $.unblockUI();
                    tbDescripcion.val(response.Descripcion);
                    tbMarca.val(response.Marca);
                    tbModelo.val(response.Modelo);
                    tbSerie.val(response.Serie);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function loadFolio() {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/MovimientosInternos/LoadFolio",
                success: function (response) {
                    $.unblockUI();
                    tbFolio.val(response.Folio);
                },
                error: function () {
                    $.unblockUI();
                }
            });
        }

        function GuardarInfomarcion() {
            state = true
            cboEconomicos.addClass('required');
            if (!validarCampo(cboEconomicos)) { state = false; }
            if (!validarCampo(tbHorometros)) { state = false; }
            if (!validarCampo(cboRecepcion)) { state = false }
            if (state) {
                $.blockUI({ message: mensajes.PROCESANDO });
                $.ajax({
                    url: "/MovimientosInternos/GuardarActualizar",
                    type: "POST",
                    datatype: "json",
                    data: { obj: GetDataInfo() },
                    success: function (response) {
                        $.unblockUI();
                        ConfirmacionGeneralAccion("Confirmación", "El registro se guardo correctamente");
                    },
                    error: function () {
                        $.unblockUI();
                    }
                });
            }

        }

        function GetDataInfo() {

            return {
                id: idGlobal,
                Envio: cboEnvio.val(),
                Destino: cboRecepcion.val(),
                Horometro: tbHorometros.val(),
                Combustible: tbCombustible.val(),
                Folio: tbFolio.val(),
                EconomicoID: cboEconomicos.val(),
                Estatus: $('input[name=optradio]:checked').val(),
                Comentario: tbComentario.val(),
                Bateria: tbBaterias.val(),
                Marca2: tbMarca2.val(),
                Serie2: tbSerie2.val(),
                Registro: tbRegistro.val()
            }
        }

        init();
    };

    $(document).ready(function () {

        maquinaria.MovimientosInternos = new MovimientosInternos();
    });
})();

