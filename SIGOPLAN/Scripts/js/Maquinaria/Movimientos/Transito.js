$(function () {
    
    $('input[name=radioTipoControl]').on('change', function () {
        var valor = $(this).val();
        if(valor==0)
        {
            $("#txtFechaEnvio").removeClass('hidden');
            $("#txtFechaRecepcion").addClass('hidden');
            $("#lblTipoEnvio").text("Lugar de envio");
            $("#txtFolio").attr("placeholder", "000001");
            $('#txtFolio').prop("disabled", true);
        }
        if (valor == 1) {
            $("#txtFechaEnvio").addClass('hidden');
            $("#txtFechaRecepcion").removeClass('hidden');
            $("#lblTipoEnvio").text("Lugar de recepción");
            $("#txtFolio").attr("placeholder", "Ingrese folio");
            $('#txtFolio').prop("disabled", false);
        }
    });
    $("#btnGuardar").on('click', function () {
        limpiar();
        ConfirmacionGeneral("Confirmación", "  Se guardo correctamente la información", "bg-green");
    });
    

})


function limpiar() {
    $('#frmTrafico')[0].reset();
    $('a[href^="#step-1"]').trigger('click');
    $('a[href^="#step-2"]').attr("disabled", "disabled");
    $('a[href^="#step-3"]').attr("disabled", "disabled");
}