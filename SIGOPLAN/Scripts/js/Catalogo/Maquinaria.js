$(function () {
    init();

    $("button.reset").on('click', function () {
        limpiar();
    });

    $("#btnGuardar").on('click', function () {
        $('#modalAltaMaquinaria').modal('hide');
        limpiar();
        ConfirmacionGeneral("Confirmación", "  La maquinaria fue guardada correctamente","bg-green");
    });

    $("#btnNuevo").on('click', function () {
        $('#modalAltaMaquinaria').modal('show');
    });

    $("#btnBuscar").on('click', function () {
        $("#grid-basic").bootgrid("clear");
        JSONINFO = [{ grupo: "Equipo Mayor", noEco: "CFC-01", descrip: "CAMION FUERA DE CARRETERA", modelo: "77F" },
                    { grupo: "Equipo Mayor", noEco: "CFC-02", descrip: "CAMION FUERA DE CARRETERA", modelo: "77F" }];
        $("#grid-basic").bootgrid("append", JSONINFO);
    });

    $('#slTipoM').on('change', function () {
        $("#slGrupoM").children().addClass('hidden');
        $('#slGrupoM option[value=0]').prop('selected', true);
        generarEconomico(0);
        var valor = $(this).val();
        if (valor == "3")
        {
            $("#divIsTP").removeClass('hidden');
            $("#divIsTP1").removeClass('hidden');
        }
        else {
            $("#divIsTP").addClass('hidden');
            $("#divIsTP1").addClass('hidden');
        }
        $("#slGrupoM").find("[data-maquina='" + valor + "']").removeClass('hidden');

    });

    $('#slGrupoM').on('change', function () {
        $("#slModelo").children().addClass('hidden');
        $('#slModelo option[value=0]').prop('selected', true);
        var valor = $(this).val();
        if ($("#slTipoRenta").val() == "0")
        {
            var economico = generarEconomico(parseInt(valor));
            $("#txtnoEco").val(economico+"-01");
        }
        else {
            var economico = generarEconomico(parseInt(valor));
            $("#txtnoEco").val(economico + "R-01");
        }
        $("#slModelo").find("[data-modelo='" + valor + "']").removeClass('hidden');

    });

    $("#slTipoRenta").on('change', function () {
        
        var valor = $('#slGrupoM').val();
        if ($(this).val() == "0") {
            if (valor == "0")
            {
                return false;
            }
            var economico = generarEconomico(parseInt(valor));
            $("#txtnoEco").val(economico + "-01");
        }
        else {
            if (valor == "0") {
                return false;
            }
            var economico = generarEconomico(parseInt(valor));
            $("#txtnoEco").val(economico + "R-01");
        }
    });


    function limpiar()
    {
        $('#frmMaquinaria')[0].reset();
        $('a[href^="#step-1"]').trigger('click');
        $('a[href^="#step-2"]').attr("disabled", "disabled");
        $('a[href^="#step-3"]').attr("disabled", "disabled");
    }

    function generarEconomico(val)
    {


        var economico;
        switch (val) {
            case 0:
                economico = "";
                break;
            case 1:
                economico = "PU";
                break;
            case 2:
                economico = "CFC";
                break;
            case 3:
                economico = "CF";
                break;
            case 4:
                economico = "CAR";
                break;
            case 5:
                economico = "EM-CAR";
                break;
            case 6:
                economico = "BL";
                break;
            case 7:
                economico = "CAM"
                break;
           
            case 8:
                economico = "OR";
                break;
            case 9:
                economico = "CS";
                break;

        }
        return economico;
      
    }

    $('#demoTipoFiltro').on('change', function () {
        $("#demoGrupoFiltro").children().addClass('hidden');
        $('#demoGrupoFiltro option[value=0]').prop('selected', true);
        $('#demoGrupoFiltro option[value=0]').removeClass('hidden');
        $("#demoEcoFiltro").children().addClass('hidden');
        $('#demoEcoFiltro option[value=0]').prop('selected', true);

        if ($(this).val() == 1) {
            $("#demoGrupoFiltro").children('.equipoMayor').removeClass('hidden');
        }
        if ($(this).val() == 2) {
            $("#demoGrupoFiltro").children('.equipoMenor').removeClass('hidden');
        }
        if ($(this).val() == 3) {
            $("#demoGrupoFiltro").children('.Transporte').removeClass('hidden');
        }
        if ($(this).val() == 0) {
            $("#demoGrupoFiltro").children().removeClass('hidden');
            $("#demoEcoFiltro").children().removeClass('hidden');
        }

    });

    $('#demoGrupoFiltro').on('change', function () {

        $("#demoEcoFiltro").children().addClass('hidden');
        $('#demoEcoFiltro option[value=0]').prop('selected', true);

        var valor = $(this).val();
        $("#demoEcoFiltro").find("[data-tipocamion='" + valor + "']").removeClass('hidden');

        if ($(this).val() == 0) {
            $("#demoEcoFiltro").children().removeClass('hidden');
        }
    });




    $("#example").on('click', function () {
        console.log("click");
    });

});

function init() {
    $("#slGrupoM").children().addClass('hidden');
 
    $("#grid-basic").bootgrid({
        headerCssClass: '.bg-table-header',
        align: 'center',
        templates: {
            header: ""
        },
        formatter: null
    });

}