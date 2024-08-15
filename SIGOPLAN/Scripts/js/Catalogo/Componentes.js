$(function () {
    $("#btnNuevo").on('click', function () {
        $('#modalAltaComponente').modal('show');
    });
    $("#btnModalGuardar").on('click', function () {
        $('#modalAltaComponente').modal('hide');
        limpiar();
        ConfirmacionGeneral("Confirmación", "  El Componente agregado correctamente", "bg-green");
    });

    function limpiar() {
        $('#frmComponentes')[0].reset();
    }

    $("#stModalGrupoMaquina").on('change', function () {
        $("#txtModalIdObra").val("");
        $("#stModalModeloMaquina").children().addClass('hidden');
        $("#stModalPrefMaquinaria").children().addClass('hidden');

        $('#stModalModeloMaquina option[value=0]').prop('selected', true);
        $('#stModalModeloMaquina option[value=0]').removeClass('hidden');
        $('#stModalPrefMaquinaria option[value=0]').prop('selected', true);
        $('#stModalPrefMaquinaria option[value=0]').removeClass('hidden');

        var valor = $(this).val();
        $("#stModalModeloMaquina").find("[data-grupo='" + valor + "']").removeClass('hidden');
    });

    $("#stModalModeloMaquina").on('change', function () {
        $("#stModalPrefMaquinaria").children().addClass('hidden');
        $('#stModalPrefMaquinaria option[value=0]').prop('selected', true);
        $('#stModalPrefMaquinaria option[value=0]').removeClass('hidden');

        var valor = $(this).val();
        $("#stModalPrefMaquinaria").find("[data-modelo='" + valor + "']").removeClass('hidden');
    });

    $("#stModalPrefMaquinaria").on('change', function () {
        var valor = $(this).val();
        if(valor!="0")
        {
            var pref = $("#stModalPrefMaquinaria").val() == "0" ? '' : $("#stModalPrefMaquinaria").val();
            var cc = $("#stModalCC").val() == "0" ? '' : $("#stModalCC").val();
            var conjunto = $("#stModalConjunto").val() == "0" ? '' : $("#stModalConjunto").val();
            var subConjunto = $("#stModalSubConjunto").val() == "0" ? '' : $("#stModalSubConjunto").val();
            var posicion = $("#stModalPosicion").val() == "0" ? '' : $("#stModalPosicion").val();

            var concatenado = pref + cc + conjunto + subConjunto + posicion;
            $("#txtModalIdObra").val(concatenado);
        }
    });

    $("#stModalCC").on('change', function () {
        var valor = $(this).val();
        if (valor != "0") {
            var pref = $("#stModalPrefMaquinaria").val() == "0" ? '' : $("#stModalPrefMaquinaria").val();
            var cc = $("#stModalCC").val() == "0" ? '' : $("#stModalCC").val();
            var conjunto = $("#stModalConjunto").val() == "0" ? '' : $("#stModalConjunto").val();
            var subConjunto = $("#stModalSubConjunto").val() == "0" ? '' : $("#stModalSubConjunto").val();
            var posicion = $("#stModalPosicion").val() == "0" ? '' : $("#stModalPosicion").val();

            var concatenado = pref + cc + conjunto + subConjunto + posicion;
            $("#txtModalIdObra").val(concatenado);
        }
    });

    $("#stModalConjunto").on('change', function () {
        var valor = $(this).val();
        if (valor != "0") {
            var pref = $("#stModalPrefMaquinaria").val() == "0" ? '' : $("#stModalPrefMaquinaria").val();
            var cc = $("#stModalCC").val() == "0" ? '' : $("#stModalCC").val();
            var conjunto = $("#stModalConjunto").val() == "0" ? '' : $("#stModalConjunto").val();
            var subConjunto = $("#stModalSubConjunto").val() == "0" ? '' : $("#stModalSubConjunto").val();
            var posicion = $("#stModalPosicion").val() == "0" ? '' : $("#stModalPosicion").val() + "-001";

            var concatenado = pref + cc + conjunto + subConjunto + posicion;
            $("#txtModalIdObra").val(concatenado);
        }
    });
    $("#stModalSubConjunto").on('change', function () {
        var valor = $(this).val();
        if (valor != "0") {
            var pref = $("#stModalPrefMaquinaria").val() == "0" ? '' : $("#stModalPrefMaquinaria").val();
            var cc = $("#stModalCC").val() == "0" ? '' : $("#stModalCC").val();
            var conjunto = $("#stModalConjunto").val() == "0" ? '' : $("#stModalConjunto").val();
            var subConjunto = $("#stModalSubConjunto").val() == "0" ? '' : $("#stModalSubConjunto").val();
            var posicion = $("#stModalPosicion").val() == "0" ? '' : $("#stModalPosicion").val() + "-001";

            var concatenado = pref + cc + conjunto + subConjunto + posicion;
            $("#txtModalIdObra").val(concatenado + "-001");
        }
    });
    $("#stModalPosicion").on('change', function () {
        var valor = $(this).val();
        if (valor != "0") {
            var pref = $("#stModalPrefMaquinaria").val() == "0" ? '' : $("#stModalPrefMaquinaria").val();
            var cc = $("#stModalCC").val() == "0" ? '' : $("#stModalCC").val();
            var conjunto = $("#stModalConjunto").val() == "0" ? '' : $("#stModalConjunto").val();
            var subConjunto = $("#stModalSubConjunto").val() == "0" ? '' : $("#stModalSubConjunto").val();
            var posicion = $("#stModalPosicion").val() == "0" ? '' : $("#stModalPosicion").val()+"-001";

            var concatenado = pref + cc + conjunto + subConjunto + posicion;
            $("#txtModalIdObra").val(concatenado);
        }
        else
        {
            var pref = $("#stModalPrefMaquinaria").val() == "0" ? '' : $("#stModalPrefMaquinaria").val();
            var cc = $("#stModalCC").val() == "0" ? '' : $("#stModalCC").val();
            var conjunto = $("#stModalConjunto").val() == "0" ? '' : $("#stModalConjunto").val();
            var subConjunto = $("#stModalSubConjunto").val() == "0" ? '' : $("#stModalSubConjunto").val();
            var posicion = $("#stModalPosicion").val() == "0" ? '' : $("#stModalPosicion").val();

            var concatenado = pref + cc + conjunto + subConjunto;
            $("#txtModalIdObra").val(concatenado + "-001");
        }


    });

    $("#btnBuscar").on('click', function () {
        $("#Table").bootgrid("clear");
        JSONINFO = [{ noComp: "H4CNBBBV-001", conjunto: "BOMBA", subconjunto: "VENTILADOR", descp: "Bomba para CFC 777F" }
        ];
        $("#Table").bootgrid("append", JSONINFO);
    });



    init();
});

function init() {
    $("#stModalModeloMaquina").children().addClass('hidden');
    $('#stModalModeloMaquina option[value=0]').prop('selected', true);
    $("#stModalPrefMaquinaria").children().addClass('hidden');
    $('#stModalPrefMaquinaria option[value=0]').prop('selected', true);


    $("#Table").bootgrid({
        headerCssClass: '.bg-table-header',
        align: 'center',
        templates: {
            header: ""
        }
    });
}