$(function () {


    $("#btnRegresar").on('click', function () {
        $("#divAutorizaciones").addClass('hidden');
        $("#divPendientesAutorizacion").removeClass('hidden');

    });

    $("#btnAutoriza").on('click', function () {
        ConfirmacionGeneral("Confirmación", "Se autorizó correctamente la solicitud","bg-green");
    });
    $("#btnCancelar").on('click', function () {
        ConfirmacionGeneral("Confirmación", "Se rechazo correctamente la solicitud", "bg-red");
    });
    init();
});


function init()
{
    var grid = $("#grid-basic").bootgrid({
        headerCssClass: '.bg-table-header',
        align: 'center',
        templates: {
            header: ""
        },
        formatters: {
            "commands": function (column, row) {
                return "<button type='button' data-row-id=\"" + row.folio + "\" class='btn btn-info verSol'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> Ver Solicitud" +
                           " </button>"

                ;
            }
        }
    }).on("loaded.rs.jquery.bootgrid", function () {
        /* Executes after data is loaded and rendered */
        grid.find(".verSol").on("click", function (e) {
            $("#divAutorizaciones").removeClass('hidden');
            $("#divPendientesAutorizacion").addClass('hidden');
            var c = $(this).attr('data-row-id');
            $("#txtFolio").text(c);
        });
    });
}