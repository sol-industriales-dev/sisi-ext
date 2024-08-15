$(function () {
    //$('.verSolicitud').on('click', function () {
    //    $("#divVisFolios").addClass('hidden');
    //    $("#divSolicitudes").removeClass('hidden');
    //});

    $("#btnRegresar").on('click', function () {
        $("#divVisFolios").removeClass('hidden');
        $("#divSolicitudes").addClass('hidden');
    });

    $("button.agregarMaquinaria").on('click', function () {
        $('#modalAsignaMaquinaria').modal('show');
    });
    $("button.verMaquinaria").on('click', function () {
        var lista = $(this).parent().parents('tr');
        //alert(Test);
    });
    $("#btnAutoriza").on('click', function () {
        $("#divVisFolios").removeClass('hidden');
        $("#divSolicitudes").addClass('hidden');
        ConfirmacionGeneral("Confirmación", "Se guardo correctamente la información", "bg-green");
    });



    init();
});
function init() {

    var grid1 = $("#tblSolicitudes").bootgrid({
        headerCssClass: '.bg-table-header',
        align: 'center',
        templates: {
            header: ""
        },
        formatters: {
            "commands": function (column, row) {
                return "<button type='button' class='btn btn-info verSolicitud'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span> Ver Solicitud" +
                           " </button>"
                ;
            }
        }
    }).on("loaded.rs.jquery.bootgrid", function () {
        /* Executes after data is loaded and rendered */
        grid1.find(".verSolicitud").on("click", function (e) {
            sessionStorage.clear;
            lista = [{ noEco: "EX-R70", des: "EXCAVADORA", noSerie: 'A11023', localizacion: "DEPOSITO DE JALES SECOS III", Asignar: "" },
            { noEco: "EX-R71", des: "EXCAVADORA", noSerie: 'A10703', localizacion: "DEPOSITO DE JALES SECOS III", Asignar: "" },
            { noEco: "EX-20", des: "EXCAVADORA", noSerie: 'CAT0325DPA3R00684', localizacion: "DEPOSITO DE JALES SECOS III", Asignar: '' },
            { noEco: "EX-21", des: "EXCAVADORA", noSerie: 'CAT0336DLM4T01134', localizacion: "DEPOSITO DE JALES SECOS III", Asignar: '' },
            { noEco: "EX-22", des: "EXCAVADORA", noSerie: 'CAT0390DKWAP00358', localizacion: "MINADO LA COLORADA", Asignar: '' },
            { noEco: "EX-23", des: "EXCAVADORA", noSerie: 'CAT0385CAEDA00551', localizacion: "MINADO NOCHEBUENA II", Asignar: '' },
            { noEco: "EX-26", des: "EXCAVADORA", noSerie: '70921', localizacion: "CANAL DE DESCARGA OHL" }
            ];
            sessionStorage.setItem('maquinaria', JSON.stringify(lista));

            agregarGrid();
            $("#divVisFolios").addClass('hidden');
            $("#divSolicitudes").removeClass('hidden');
        });
    });

    var grid = $("#tblListado").bootgrid({
        headerCssClass: '.bg-table-header',
        align: 'center',
        templates: {
            header: ""
        },
        formatters: {
            "asigna": function (column, row) {
                return "<button type='button' class='btn btn-success agregarMaquinaria'>" +
                                "<span class='glyphicon glyphicon-plus'></span>" +
                           " </button>"
                ;
            },
            "ver": function (column, row) {
                return "<button type='button' class='btn btn-info verSolicitud'>" +
                                "<span class='glyphicon glyphicon-eye-open'></span>" +
                           " </button>"
                ;
            }
        }
    }).on("loaded.rs.jquery.bootgrid", function () {
        /* Executes after data is loaded and rendered */
        grid.find(".agregarMaquinaria").on("click", function (e) {
            $('#modalAsignaMaquinaria').modal('show');
        });
    });

    var tblMaquinaria = $("#tblMaquinaria").bootgrid({
        headerCssClass: '.bg-table-header',
        align: 'center',
        templates: {
            header: ""
        },
        formatters: {
            "Asignar": function (column, row) {
                return "<button type='button' data-row-id=\"" + row.noEco + "\" class='btn btn-success btnModalAsignar'>" +
                                "<span class='glyphicon glyphicon-plus'></span>" +
                           " </button>";
            }
        }
    }).on("loaded.rs.jquery.bootgrid", function () {
        tblMaquinaria.find(".btnModalAsignar").on("click", function (e) {
            $("#tblMaquinaria").bootgrid("clear");
            indice = $(this).attr("data-row-id");
            $("#tblMaquinaria").bootgrid("append", listaReturn(indice));
        });
    });
}

function agregarGrid() {
    $("#tblMaquinaria").bootgrid("clear");
    listadoMaquinaria = JSON.parse(sessionStorage.getItem('maquinaria'));
    $("#tblMaquinaria").bootgrid("append", listadoMaquinaria);
}
function listaReturn(indice) {


    var listaMaquinaria = JSON.parse(sessionStorage.getItem('maquinaria'));

    for (var i = 0; i < listaMaquinaria.length; i++) {

        if (listaMaquinaria[i].noEco === indice) {
            listaMaquinaria.splice(i, 1);
        }
    }
    sessionStorage.setItem('maquinaria', JSON.stringify(listaMaquinaria));
    return listaMaquinaria;
}



function listadoMaquinaria() {
    lista = [{ noEco: "EX-R70", des: "EXCAVADORA", noSerie: 'A11023', localizacion: "DEPOSITO DE JALES SECOS III",Asignar:""},
              { noEco: "EX-R71", des: "EXCAVADORA", noSerie: 'A10703', localizacion: "DEPOSITO DE JALES SECOS III",Asignar:"" },
              { noEco: "EX-20", des: "EXCAVADORA", noSerie: 'CAT0325DPA3R00684', localizacion: "DEPOSITO DE JALES SECOS III",Asignar:'' },
              { noEco: "EX-21", des: "EXCAVADORA", noSerie: 'CAT0336DLM4T01134', localizacion: "DEPOSITO DE JALES SECOS III",Asignar:'' },
              { noEco: "EX-22", des: "EXCAVADORA", noSerie: 'CAT0390DKWAP00358', localizacion: "MINADO LA COLORADA",Asignar:'' },
              { noEco: "EX-23", des: "EXCAVADORA", noSerie: 'CAT0385CAEDA00551', localizacion: "MINADO NOCHEBUENA II",Asignar:'' },
              { noEco: "EX-26", des: "EXCAVADORA", noSerie: '70921', localizacion: "CANAL DE DESCARGA OHL" }
    ];

    return lista;
}