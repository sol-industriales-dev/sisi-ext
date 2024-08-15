(() => {

    $.namespace('FileManager.EnvioDocumento');

EnvioDocumento = function () {

    let tblEnvios = $("#tblEnvios");
    let btnGuardarGestor = $("#btnModalGuardar_Gestor");
    let cboDocumento = $("#cboDocumento");
    let txtDescripcion = $("#txtDescripcion");
    let btnBuscar = $("#btnBuscar_Envio");


    function init() {
        IniciarTblEnvios();
        CargarTblEnvios();
        $('#modalGestor').on('hidden.bs.modal', function () {
            CargarTblEnvios();
        });
        cboDocumento.fillCombo("/FileManager/FileManager/FillCboTipoDocEnvio");
        btnBuscar.click(CargarTblEnvios);
        $(document).on('click', "#btnModalEliminar", function (e) {
            e.preventDefault();
            EliminarDocumento(btnGuardarGestor.attr("data-envioid"));
        });

    }

    function IniciarTblEnvios()
    {
        tblEnvios = $("#tblEnvios").DataTable({
            language: {
                "sProcessing": "Procesando...", "sLengthMenu": "Mostrar _MENU_ registros", "sZeroRecords": "No se encontraron resultados", 
                "sEmptyTable": "Ningún dato disponible en esta tabla", "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros", "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "", "sSearch": "Buscar:", "sUrl": "", "sInfoThousands": ",", "sLoadingRecords": "Cargando...", "oPaginate": {
                    "sFirst": "Primero", "sLast": "Último", "sNext": "Siguiente", "sPrevious": "Anterior" },
                "oAria": { "sSortAscending": ": Activar para ordenar la columna de manera ascendente", "sSortDescending": ": Activar para ordenar la columna de manera descendente" }
            },
            searching: false, scrollCollapse: true, select: { style: 'os', selector: 'td:first-child' },
            drawCallback: function (settings) {
                var gestor = $(".btn-enviar");
                $.each(gestor, function (i, e) {
                    $(this).click(clickGestor);
                });

                var eliminar = $(".btn-eliminar");
                $.each(eliminar, function (i, e) {
                    $(this).click(clickEliminar);
                });
            },
            columns: [
                
                { data: 'tipoDocumentoDescripcion', title: 'TIPO DE DOCUMENTO' },
                { data: 'descripcion', title: 'DESCRIPCIÓN' },
                { data: 'fecha', title: 'FECHA' },
                {
                    sortable: false,
                    "render": function (data, type, row, meta) {
                        var html = '<button class="btn-enviar btn btn-sm btn-primary glyphicon glyphicon-folder-open" type="button" data-index="' + row.documentoID + '" data-tipodocumento="' +
                            row.tipoDocumento + ' " data-envioid="' + row.id + '"></button>';                            
                        return html;
                    },
                    title: "GUARDAR"
                },
                {
                    sortable: false,
                    "render": function (data, type, row, meta) {
                        var html = '<button class="btn-eliminar btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" data-descripcion="' + row.descripcion + '" data-tipodocumento="' +
                            row.tipoDocumentoDescripcion + ' " data-envioid="' + row.id + '"></button>';                            
                        return html;
                    },
                    title: "ELIMINAR"
                },
            ],
            columnDefs: [
                { "className": "dt-center", "targets": [0, 1, 2, 3, 4] },
                { "orderable": false, "targets": [3] },
                { "width": "10%", "targets": [3, 4] },
                { "width": "15%", "targets": [2] },
                { "width": "30%", "targets": [0] },
                { "width": "45%", "targets": [1] }

            ],
            order: [[0, 'asc'], [1, 'asc']],                
        });
    }

    function CargarTblEnvios()
    {
        $.blockUI({ message: "Procesando...", baseZ: 2000 });
        $.ajax({
            url: '/FileManager/FileManager/CargarTblEnvios',
            datatype: "json",
            type: "POST",
            data: {
                tipoDocumento: cboDocumento.val(), 
                descripcion: txtDescripcion.val().trim() 
            },
            success: function (response) {
                $.unblockUI();
                if (response.success) {
                    tblEnvios.clear();
                    tblEnvios.rows.add(response.documentos);
                    tblEnvios.draw();
                }
            },
            error: function (response) {
                $.unblockUI();
                AlertaGeneral("Alerta", response.MESSAGE);
            }
        });
    }

    function clickGestor() {

        $.blockUI({ message: "Cargando información..." });
        let tipoDocumento = $(this).data("tipodocumento");
        let indexClickG = $(this).data("index");
        btnGuardarGestor.attr("data-index", $(this).data("index"));
        btnGuardarGestor.attr("data-envioid", $(this).data("envioid"));
        btnGuardarGestor.attr("data-tipoDocumento", tipoDocumento);   
        let empresa = 0;
        $.ajax({
            url: '/FileManager/FileManager/GetEmpresaReporte',
            datatype: "json",
            type: "POST",
            data: { envioID: $(this).data("envioid") },
            success: function (response) {
                $.unblockUI();
                if (response.success) {
                    empresa = response.empresa  
                    switch(tipoDocumento.trim())
                    {
                        case "16":
                            $.blockUI({ message: "Cargando información..." });
                            var path = "/Reportes/Vista.aspx?idReporte=104&plantillaID=" + indexClickG + "&inMemory=1&empresa=" + empresa;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {
                                $.unblockUI();
                                $("#modalGestor").modal("show");
                            };    
                            break;
                        case "17":
                            $.blockUI({ message: "Cargando información..." });
                            $.ajax({
                                url: '/SolicitudEquipo/GetReportePorEmpresa',
                                type: "POST",
                                datatype: "json",
                                data: { obj: indexClickG, empresa: empresa },
                                success: function (response) {

                                    var idReporte = response.idReporte;
                                    var path = "/Reportes/Vista.aspx?idReporte=" + response.idReporte + "&pCC=" + response.CC + "&inMemory=1";
                                    $("#report").attr("src", path);
                                    document.getElementById('report').onload = function () {
                                        $("#modalGestor").modal("show");
                                        $.unblockUI();
                                    };

                                },
                                error: function () {
                                    $.unblockUI();
                                }
                            });
                            
                            break;
                        case "18":
                            $.blockUI({ message: "Cargando información..." });
                            var path = "/Reportes/Vista.aspx?idReporte=4&minuta=" + indexClickG + "&inMemory=1&empresa=" + empresa;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {
                                var path = "/Reportes/Vista.aspx?idReporte=5&minuta=" + indexClickG + "&inMemory=1&empresa=" + empresa;
                                $("#report").attr("src", path);
                                document.getElementById('report').onload = function () {
                                    $.unblockUI();
                                    $("#modalGestor").modal("show");
                                };
                            };
                            break;
                        case "19":
                            $.blockUI({ message: "Cargando información..." });
                            var path = "/Reportes/Vista.aspx?idReporte=78&id=" + indexClickG + "&inMemory=1&isCRModal=false&empresa=" + empresa;
                            $("#report").attr("src", path);
                            document.getElementById('report').onload = function () {
                                $.unblockUI();
                                $("#modalGestor").modal("show");
                            }; 
                            break;
                        default:
                            $.unblockUI();
                            break;
                    }
                }
            },
            error: function (response) {
                $.unblockUI();
                AlertaGeneral("Alerta", response.MESSAGE);
            }
        });
    }

    function clickEliminar()
    {
        let tipoDocumento = $(this).data("tipodocumento");
        let descripcion = $(this).data("descripcion")
        btnGuardarGestor.attr("data-descripcion", descripcion);
        btnGuardarGestor.attr("data-envioid", $(this).data("envioid"));
        btnGuardarGestor.attr("data-tipoDocumento", tipoDocumento);   
        ConfirmacionEliminacion("Desecho", "¿Está seguro que desea eliminar el registro "+ tipoDocumento.trim() + ": " + descripcion + "?");
    }

    function EliminarDocumento(index)
    {
        $.blockUI({ message: "Procesando...", baseZ: 2000 });
        $.ajax({
            url: '/FileManager/FileManager/EliminarEnvioDoc',
            datatype: "json",
            type: "POST",
            data: {
                index: index
            },
            success: function (response) {
                $.unblockUI();
                if (response.exito) {
                    AlertaGeneral("Alerta", "Se ha eliminado el registro");
                    CargarTblEnvios();
                }
                else
                {
                    AlertaGeneral("Alerta", "Ocurrió un error al tratar de eliminar el registro: " + response.MESSAGE);
                }
            },
            error: function (response) {
                $.unblockUI();
                AlertaGeneral("Alerta", response.MESSAGE);
            }
        });
    }

init();

};
$(document).ready(() => FileManager.EnvioDocumento = new EnvioDocumento());
})();