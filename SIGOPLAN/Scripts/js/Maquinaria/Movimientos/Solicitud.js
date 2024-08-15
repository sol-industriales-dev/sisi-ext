$(document).ready(function () {

    limpiarSession();
    var tipoMaquinaria;

    $("#btnSolicita").on('click', function () {
        limpiar();
    });

    function limpiar() {
        localStorage['idGlobal'] = 1;
        $('#frmSolicitud')[0].reset();
        sessionStorage.clear;
        ConfirmacionGeneral("Confirmación", "  La solicitud fue creada correctamente", "bg-green");
        $("#tblMaquiSolicitud").bootgrid("clear");
        $("#tblListaMaquinas").bootgrid("clear");
        $('a[href^="#step-1"]').trigger('click');
        $('a[href^="#step-2"]').attr("disabled", "disabled");
        $('a[href^="#step-3"]').attr("disabled", "disabled");
        $('#btnSiguiente').prop("disabled", true);
        $('#btnSiguiente1').prop("disabled", true);
    }

    $('#slTipoM').on('change', function () {
        $("#slGrupoM").children().addClass('hidden');
        $('#slGrupoM option[value=0]').prop('selected', true);
        if ($(this).val() == 1) {
            $("#slGrupoM").children('.EquipoTransporte').removeClass('hidden');
        }
        if ($(this).val() == 2) {
            $("#slGrupoM").children('.EquipoMenor').removeClass('hidden');
        }
        if ($(this).val() == 3) {
            $("#slGrupoM").children('.MaquinariaMayor').removeClass('hidden');
        }
    });

    $('.tabs .tab-links a').on('click', function (e) {
        var currentAttrValue = $(this).attr('href');

        // Show/Hide Tabs
        $('.tabs ' + currentAttrValue).show().siblings().hide();

        // Change/remove current tab to active
        $(this).parent('li').addClass('active').siblings().removeClass('active');

        e.preventDefault();
    });

    String.prototype.compose = (function () {
        var re = /\{{(.+?)\}}/g;
        return function (o) {
            return this.replace(re, function (_, k) {
                return typeof o[k] != 'undefined' ? o[k] : '';
            });
        }
    }());


    $('#btnAddMaq').on('click', function () {

        if ($("#txtCantidad").val() > 0)
        {
            var tbody = $('#tblListaMaquinas').children('tbody');
            var table = tbody.length ? tbody : $('#myTable');
            var num = $("#txtCantidad").val();

            $("#tblListaMaquinas").bootgrid("append", añadirLista(table, num));
            $("tbody tr:first td:last-child button.programaEquivalente").addClass('hidden');
            $('#btnSiguiente1').prop("disabled", false);
            $("#txtDescripcion").val("");
            $("#txtCapacidad").val("0");
            $('#slModeloM option[value=0]').prop('selected', true);
            $('#slTipoM option[value=0]').prop('selected', true);
            $('#slGrupoM option[value=0]').prop('selected', true);
            ConfirmacionGeneral("Confirmación", " Se agregaron "+num+" regristros", "bg-green");

        }
        else
        {
            ConfirmacionGeneral("Alerta", " la cantidad debe ser mayor 0", "bg-red");
        }
    

    });

    localStorage['idGlobal'] = 1;
    function añadirLista(table, num) {
        var rowsLista = [];
        for (i = 0; i < num; i++) {
            var idGlobal = localStorage['idGlobal'];
            var objeto = { id: idGlobal, Tipo: $("#slTipoM option:selected").text(), Grupo: $("#slGrupoM option:selected").text(), Descripción: $("#txtDescripcion").val(), añadir: "", duplicar: "" }

            rowsLista.push(objeto);
            var count = localStorage['idGlobal'];
            count++;
            localStorage['idGlobal'] = count;
        }
        return rowsLista;
    };

    $('#btnAgregados').on('click', function () {
        $('#modalMaqSolicitada').modal('show');
    });

    $('#slTipoM').on('change', function () {
        if ($(this).val() != 1 || $(this).val() == 0) {
            $("#divTipoEncierro").addClass('hidden')
        }
        else {
            $("#divTipoEncierro").removeClass('hidden')
        }
    });

    var lista;
    $(".asiganaM").on('click', function () {
        lista = $(this).parent().parents('tr');
        lista.remove();
    });
    $(document).on('click', '.agregarPrograma', function () {
        limpia();
        var valor = $(this).parent().parent().children('.hidden').html();
        cargarDatos(valor, valor);
    });

    $(document).on('click', '.programaEquivalente', function () {
        limpia();
        var valor = $(this).parent().parent().children('.hidden').html();
        var anteriorValor = $(this).parents('tr').prev().children('td.hidden').html();

        cargarDatos(valor, anteriorValor);
    });

    $("#btnSiguiente2").on('click', function () {

        var tipo;
        var grupo;
        var descripcion;

        var tbody = $('#myTable1').children('tbody');
        var table = tbody.length ? tbody : $('#myTable1');
        var num = $("#txtCantidad").val();

        var row = '<tr>' +
             '<td class="hidden">{{id}}</td>' +
      '<td >{{Tipo}}</td>' +
      '<td >{{Grupo}}</td>' +
      '<td >{{Descripción}}</td>' +
      '<td>{{TiempoMeses}}</td>' +
      '<td>{{TiempoHoras}}</td>' +
      '<td>{{FechaObra}}</td>' +
      '<td>{{delete}}</td>' +
       '</tr>';
        var obj = JSON.parse(sessionStorage.getItem('modal'));
        if (obj != null) {
            $.each(obj, function (i, item) {
                $("td.hidden").each(function (i, itemtd) {

                    if (item.id == $(this).html()) {
                        var x = $(this).siblings('td[data-option="true"]');
                        tipo = $(x[0]).html();
                        grupo = $(x[1]).html();
                        descripcion = $(x[2]).html();
                    }
                });

                table.append(row.compose({
                    'id': item.id,
                    'Tipo': tipo,
                    'Grupo': grupo,
                    'Descripción': descripcion,
                    'TiempoMeses': item.meses,
                    'TiempoHoras': item.horas,
                    'FechaObra': item.fecha,
                    'delete': " <button type='button' class='btn btn-danger eliminarPeticion'>" +
                                                       " <span class='glyphicon glyphicon-remove'></span>" +
                                                  "  </button>"
                }));

            });
        }
    });

    var rows = [];

    $("#btnModalGuardar").on('click', function () {

        var valor = $("#idMaquina").val();
        var modal = { id: valor, meses: $("#txtTiempoMeses").val(), horas: $("#txtTiempoHoras").val(), fecha: $("#txtFechaObra").val(), radio: $('input[name=radioInline1]:checked').val() };

        var obj = [];

        obj = JSON.parse(sessionStorage.getItem('modal'));

        if (obj != null) {
            $.each(obj, function (i, item) {

                if (item.id == valor) {
                    obj = buscar(valor, obj);
                    obj.push(modal);
                    sessionStorage.setItem('modal', JSON.stringify(obj));
                    $('#btnSiguiente2').prop("disabled", false);

                }
                if (item.id != valor) {
                    obj = buscar(valor, obj);
                    obj.push(modal);
                    sessionStorage.setItem('modal', JSON.stringify(obj));
                    $('#btnSiguiente2').prop("disabled", false);
                }


            });
        }
        if (obj == null) {
            
            rows.push(modal);
            sessionStorage.setItem('modal', JSON.stringify(rows));
        }
        else {
            var obj = JSON.parse(sessionStorage.getItem('modal'));
            if (obj.length == 0)
            {
                obj = buscar(valor, obj);
                obj.push(modal);
                sessionStorage.setItem('modal', JSON.stringify(obj));
            }
        }

        crearTabla();
        $('#btnSiguiente2').prop("disabled", false);
    });

    init();
    function buscar(nameKey, myArray) {
       
        for (var i = 0; i < myArray.length; i++) {
            if (myArray[i].id === nameKey) {
                myArray.splice(i, 1);
            }
        }
        return myArray;
    }   
});


function crearTabla() {
    $("#tblMaquiSolicitud").bootgrid("clear");
    var obj = JSON.parse(sessionStorage.getItem('modal'));
    var insert = [];
    if (obj != null) {
        $.each(obj, function (i, item) {
            JSONINFO = {
                id: item.id, tipo: $("tr[data-row-id='" + (item.id - 1) + "']").find('Tipo').html(),
                grupo: $("tr[data-row-id='" + (item.id - 1) + "']").find('Grupo').html(), desc: $("tr[data-row-id='" + (item.id - 1) + "']").find('Desc').html(), meses: item.meses, horas: item.horas, fecha: item.fecha
            };
            insert.push(JSONINFO);
        });
    }
    $("#tblMaquiSolicitud").bootgrid("append", insert);
}

function limpia() {
    $("#idMaquina").val("");
    $("#txtTiempoMeses").val("");
    $("#txtTiempoHoras").val("");
    $("#txtFechaObra").val("");

}

function limpiarSession() {
    sessionStorage.setItem('modal', null);
    return '';
}


function cargarDatos(valor1, valor2) {
    var obj = JSON.parse(sessionStorage.getItem('modal'));

    if (obj != null) {
        $.each(obj, function (i, item) {
            if (item.id == valor2) {
                $("#txtTiempoMeses").val(item.meses);
                $("#txtTiempoHoras").val(item.horas);
                $("#txtFechaObra").val(item.fecha);
            }
        });
    }
    $("#idMaquina").val(valor1);

    $('#modalDatosPrograma').modal('show');
}

function init() {
    var d = new Date();
    var n = d.toLocaleDateString('es');
    $("#txtDateTime").val(n);
  
    var grid = $("#tblListaMaquinas").bootgrid({
        templates: {
            header: ""
        },
        formatters: {
            "Tipo": function (column, row) {
                return "<tipo>"+row.Tipo+"</tipo>";;
            },
            "Grupo": function (column, row) {
                return "<Grupo>" + row.Grupo + "</Grupo>";;
            },
            "Descripción": function (column, row) {
                return "<Desc>" + row.Descripción + "</Desc>";;
            },
            "añadir": function (column, row) {
                return "<button type='button' class='btn btn-success agregarPrograma' data-index=''>" +
                                "<i class='fa fa-clock-o fa-lg' aria-hidden='true'></i>" +
                           " </button>"
                ;
            },
            "duplicar": function (column, row) {
                return "<button type='button' class='btn btn-warning programaEquivalente'>" +
                               "<span class='glyphicon glyphicon-edit '></span> " +
                          " </button>"
                ;
            },

        }
    }).on("loaded.rs.jquery.bootgrid", function () {
        /* Executes after data is loaded and rendered */
        grid.find(".agregarPrograma").on("click", function (e) {
            limpia();
            var valor = $(this).parent().parent().children('.hidden').html();
            cargarDatos(valor, valor);

        });

        grid.find(".programaEquivalente").on('click', function (e) {
            limpia();
            var valor = $(this).parent().parent().children('.hidden').html();
            var anteriorValor = $(this).parents('tr').prev().children('td.hidden').html();

            cargarDatos(valor, anteriorValor);

        });
    });

    var gridSol = $("#tblMaquiSolicitud").bootgrid({
        templates: {
            header: ""
        },
        formatters: {
            "eliminar": function (column, row) {
                return "<button data-row-id=\"" + row.id + "\" type='button' class='btn btn-danger deleteRow'>" +
                               "<span class='glyphicon glyphicon-remove '></span> " +
                          " </button>"
                ;
            },
            "modificar": function (column, row) {
                return "<button data-row-id=\"" + row.id + "\" type='button' class='btn btn-warning editRow'>" +
                               "<span class='glyphicon glyphicon-edit '></span> " +
                          " </button>"
                ;
            }
        }

    }).on("loaded.rs.jquery.bootgrid", function () {
        gridSol.find(".deleteRow").on("click", function (e) {
            var valor = $(this).parent().parent().children('.hidden').html();
            var obj = JSON.parse(sessionStorage.getItem('modal'));
            sessionStorage.clear;
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].id === valor) {
                    obj.splice(i, 1);
                }
            }
            sessionStorage.setItem('modal', JSON.stringify(obj));
            crearTabla();
        });
        gridSol.find(".editRow").on("click", function (e) {
            var valor = $(this).parent().parent().children('.hidden').html();
            cargarDatos(valor, valor);
        });

    });

}