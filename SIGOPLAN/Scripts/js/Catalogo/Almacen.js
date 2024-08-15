$(function () {
    $("#btnNuevo").on('click', function () {
        $('#modalAltaAlmacen').modal('show');
    });

    //$("#btnBuscar").on('click', function () {
    //    $('#divFiltroBusqueda').removeClass('hidden');
    //});

    $("#btnModalGuardar").on('click', function () {
        $('#modalAltaAlmacen').modal('hide');
        limpiar();
        ConfirmacionGeneral("Confirmación", "  El almacén fue agregado correctamente", "bg-green");
    });

    function limpiar() {
        $('#frmAlmacen')[0].reset();
    }
    $("#btnBuscar").on('click', function () {
        $("#Table").bootgrid("clear");
        JSONINFO = [{ noAlmacen: "1", descrip: "ALMACEN GENERAL ", localizacion: "PERIFERICO PTE. #770" },
        { noAlmacen: "2", descrip: "ALMACEN DE LENTO MOVIMIENTO Y OBSOLETOS ", localizacion: "PERIFERICO PTE. #770" },
        { noAlmacen: "70", descrip: "ALMACEN MONTAJE ELECTRICO OHL", localizacion: "EMPALME, SON." },
        { noAlmacen: "114", descrip: "PATIOS NOCHE BUENA", localizacion: "MINA NOCHE BUENA" },
        { noAlmacen: "132", descrip: "PROYECTO SAN JULIAN", localizacion: "GUADALUPE Y CALVO, CHIHUAHUA" },
        { noAlmacen: "146", descrip: "ALMACEN MINADO LA COLORADA", localizacion: "LA COLORADA, SON." },
        { noAlmacen: "152", descrip: "DEPOSITO DE JALES SECOS III", localizacion: "OAXACA, OAX." },
        { noAlmacen: "214", descrip: "ALMACEN CANAL DE DESCARGA OHL", localizacion: "EMPALME, SON." },
        { noAlmacen: "323", descrip: "ALMACEN PRESA PILARES", localizacion: "ALAMOS, SONORA" },
        { noAlmacen: "342", descrip: "ALMACEN SIDUR Y GRIEGA", localizacion: "CABORCA SON." },
        { noAlmacen: "900", descrip: "1 VIRTUAL CENTRAL", localizacion: "PERIFERICO PONIENTE #770" },

        { noAlmacen: "902", descrip: "114 VIRTUAL NOCHE BUENA", localizacion: "EJIDO JUAN ALVAREZ SON." },
        { noAlmacen: "910", descrip: "ALMACEN DE INSUMOS REMANENTES", localizacion: "PERIFERICO PTE. #770" },
        { noAlmacen: "917", descrip: "VIRTUAL MONTAJE ELECTRICO OHL", localizacion: "EMPALME, SON." },
        { noAlmacen: "927", descrip: "VIRTUAL PROYECTO SAN JULIAN", localizacion: "GUADALUPE Y CALVO, CHIHUAHUA" },
        { noAlmacen: "930", descrip: "VIRTUAL PRESA PILARES", localizacion: "ALAMOS, SONORA" },
        { noAlmacen: "944", descrip: "VIRTUAL MINADO LA COLORADA", localizacion: "LA COLORADA, SON." },
        { noAlmacen: "947", descrip: "VIRTUAL SIDUR Y GRIEGA", localizacion: "CABORCA SON." },
        { noAlmacen: "948", descrip: "VIRTUAL DEPOSITO DE JALES SECOS III", localizacion: "OAXACA, OAX." },
        { noAlmacen: "949", descrip: "VIRTUAL TERRACERIAS HERRADURA PLD-2", localizacion: "CABORCA, SON." },
        { noAlmacen: "950", descrip: "VIRTUAL PRESA DE JALES \"SAN CARLOS\"", localizacion: "ZACATECAS, ZAC." }];
        $("#Table").bootgrid("append", JSONINFO);
    });


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