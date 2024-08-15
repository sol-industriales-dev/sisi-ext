$(function () {
    $("#btnNuevo").on('click', function () {
        $('#modalAltaTalleres').modal('show');
    });
    $("#btnBuscar").on('click', function () {
        $("#Table").bootgrid("clear");
        JSONINFO = [{ cc: "010", descr: "TALLER" },
                    { cc: "070", descr: "MONTAJE ELECTRICO OHL" },
                    { cc: "139", descr: "PRESA DE JALES SAN JULIAN" },
                    { cc: "146", descr: "MINADO LA COLORADA" },
                    { cc: "148", descr: "MINADO NOCHEBUENA II" },
                    { cc: "151", descr: "PATIOS NOCHEBUENA VI" },
                    { cc: "152", descr: "DEPOSITO DE JALES SECOS III" },
                    { cc: "153", descr: "TERRACERIAS HERRADURA PLD-2" },
                    { cc: "154", descr: "PRESA DE JALES \"SAN CARLOS\"" },
                    { cc: "214", descr: "CANAL DE DESCARGA OHL" },
                    { cc: "342", descr: "SIDUR Y GRIEGA" },
                    { cc: "343", descr: "RECARPETEO JOSE S HEALY" },
                    { cc: "323", descr: "PRESA PILARES" }];
        $("#Table").bootgrid("append", JSONINFO);
    });

    $("#btnModalGuardar").on('click', function () {
        $('#modalAltaTalleres').modal('hide');
        limpiar();
        ConfirmacionGeneral("Confirmación", "  El taller fue agregado correctamente", "bg-green");
    });

    function limpiar() {
        $('#frmTaller')[0].reset();
    }

    init();
});



function init() {
    $("#Table").bootgrid({
        headerCssClass: '.bg-table-header',
        align: 'center',
        templates: {
            header: ""
        }
    });
}