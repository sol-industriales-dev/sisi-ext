(function () {

    $.namespace('planActividades.areasCatalogo');

    areasCatalogo = function () {

        tblArea = $("#tblArea");

        btnNuevaArea = $("#btnNuevaArea");

        btnGuardarArea = $("#btnGuardarArea");

        btnEditarArea = $("#btnEditarArea");

        btnBuscar_Area = $("#btnBuscar_Area");

        inputReferencia = $('#inputReferencia');
        divGaleriaNuevaArea = $('#divGaleriaNuevaArea');

        inputReferenciaEditar = $('#inputReferenciaEditar');
        divGaleriaReferenciaEditar = $('#divGaleriaReferenciaEditar');
        mdlReferenciaEditar = $('#mdlReferenciaEditar');
        btnEditarReferencia = $('#btnEditarReferencia');

        $('a[href$="#tabArea"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        $('#tblArea').on('click', '.btn-editar-ref', function () {
            var areaID = $(this).val();

            $('#inputEditarAreaReferencia').val(areaID);

            mdlReferenciaEditar.modal('show');
        });

        function init() {
            initTableArea();

            initCbo();

            btnNuevaArea.click(fnAgregarArea);
            btnGuardarArea.click(fnGuardarArea);
            btnEditarArea.click(fnEditarArea);
            btnBuscar_Area.click(fnBuscarArea);

            inputReferencia.change(setGalery);
            inputReferenciaEditar.change(setGaleryEditar);

            btnEditarReferencia.click(fnEditarReferencia);
        }

        function fnAgregarArea() {
            $("#txtAreaDesc").val("");
            $("#selectAreaCuadrilla").val("");

            $("#dialogNuevaArea").modal('show');

            $("#txtAreaDesc").focus();
        }

        function fnEditarArea() {
            $.ajax({
                url: '/MAZDA/PlanActividades/EditarArea',
                datatype: "json",
                type: "POST",
                data: {
                    id: $("#btnEditarArea").val(),
                    desc: $("#txtEditAreaDesc").val(),
                    cuadrillaID: $("#selectEditAreaCuadrilla").val()
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información del área.");
                    recargarTodo();
                }
            });
        }

        function fnBuscarArea() {
            tblArea.ajax.reload(null, false);
            //$("#txtAreaCuadrilla").val("");
            //$("#txtArea").val("");
        }

        function fnGuardarArea() {
            if ($("#txtAreaDesc").val() != "" && $("#selectAreaCuadrilla").val() != "") {
                var request = new XMLHttpRequest();
                request.open("POST", "/MAZDA/PlanActividades/GuardarArea");
                request.send(formData());
                request.onload = function (response) {
                    if (request.status == 200) {
                        AlertaGeneral("Aviso", "Área guardada correctamente.");
                        $('#dialogNuevaArea').modal("hide");

                        recargarTodo();
                    }
                };

                //$.ajax({
                //    url: '/MAZDA/PlanActividades/GuardarArea',
                //    //dataType: 'json',
                //    data: {
                //        desc: $("#txtAreaDesc").val(),
                //        cuadrillaID: $("#selectAreaCuadrilla").val()
                //    },
                //    success: function (data) {
                //        AlertaGeneral("Información Guardada", "Se ha guardado la información del área.");
                //        recargarTodo();
                //    }
                //});
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }
        function formData() {
            let formData = new FormData();
            formData.append("area", JSON.stringify(getArea()));
            $.each(document.getElementById("inputReferencia").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }
        function getArea() {
            return {
                descripcion: $("#txtAreaDesc").val(),
                cuadrillaID: $("#selectAreaCuadrilla").val(),
                estatus: true
            };
        }

        function initTableArea() {
            tblArea = $("#tblArea").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/PlanActividades/GetAreas',
                    data: function (data) {
                        data.cuadrillaID = $("#txtAreaCuadrilla").val() != null && $("#txtAreaCuadrilla").val() != "" ? parseInt($("#txtAreaCuadrilla").val()) : 0,
                        data.area = $("#txtArea").val()
                    }
                },
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                },
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                //scrollX: "100%",
                "scrollY": "250px",
                "scrollCollapse": true,
                'initComplete': function (settings, json) {
                    tblArea.on('click', '.btn-editar-area', function () {
                        var rowData = tblArea.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/MAZDA/PlanActividades/GetArea',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                var data = response.data;

                                $("#btnEditarArea").val(rowData["id"]);

                                $("#txtEditAreaDesc").val("");
                                $("#selectEditAreaCuadrilla").val("");

                                $("#dialogEditarArea").modal('show');

                                $("#txtEditAreaDesc").val(data.descripcion);
                                $("#selectEditAreaCuadrilla").val(data.cuadrillaID != 0 ? data.cuadrillaID : "");
                            }
                        });
                    });

                    tblArea.on('click', '.btn-eliminar-area', function () {
                        var rowData = tblArea.row($(this).closest('tr')).data();
                        $("#dialogBajaArea").dialog({
                            modal: true,
                            open: function () {
                                $("#txtElimArea").text("¿Está seguro que desea eliminar el área '" + rowData["descripcion"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/MAZDA/PlanActividades/RemoveArea',
                                        data: { id: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogBajaArea").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogBajaArea").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'cuadrilla', title: 'Cuadrilla' },
                    { data: 'descripcion', title: 'Area' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-editar-area btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';
                            html += '<button class="btn-editar-ref btn btn-sm btn-primary" type="button" value="' + row.id + '" style="margin-left: 5px; margin-top: 2px;"><span class="fa fa-photo"></span></button>';

                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-eliminar-area btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": "_all" },
                    { "width": "90%", "targets": [1] },
                    { "width": "10%", "targets": [2, 3] }
                ]
            });
        }

        function initCbo() {
            $("#selectAreaCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
            $("#selectEditAreaCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
            $("#txtAreaCuadrilla").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
        }

        function recargarTodo() {
            tblArea.ajax.reload(null, false);

            $("#dialogNuevaArea").modal('hide');
            $("#dialogEditarArea").modal('hide');

            initCbo();
        }

        function setGalery() {
            let lstFotos = $(this)[0].files;
            $.each(lstFotos, function (i, e) {
                readURL(this);
            });
        }
        var readURL = function (input) {
            var reader = new FileReader();
            let item = $(document.createElement('div'));
            reader.onload = function (e) {
                item.addClass("mkr_SldItem");
                item.append(document.createElement('div'));
                item.find("div").addClass("thumbHolder");
                item.find(".thumbHolder").append(document.createElement("img"));
                item.find("img").attr("src", e.target.result);
                item.find("img").attr("width", "125px");
            }
            reader.readAsDataURL(input);
            divGaleriaNuevaArea.append(item);
        }

        function setGaleryEditar() {
            let lstFotos = $(this)[0].files;
            $.each(lstFotos, function (i, e) {
                readURLEditar(this);
            });
        }
        var readURLEditar = function (input) {
            var reader = new FileReader();
            let item = $(document.createElement('div'));
            reader.onload = function (e) {
                item.addClass("mkr_SldItem");
                item.append(document.createElement('div'));
                item.find("div").addClass("thumbHolder");
                item.find(".thumbHolder").append(document.createElement("img"));
                item.find("img").attr("src", e.target.result);
                item.find("img").attr("width", "125px");
            }
            reader.readAsDataURL(input);
            divGaleriaReferenciaEditar.append(item);
        }

        function fnEditarReferencia() {
            var request = new XMLHttpRequest();
            request.open("POST", "/MAZDA/PlanActividades/EditarReferenciaArea");
            request.send(formDataEditar());
            request.onload = function (response) {
                if (request.status == 200) {
                    AlertaGeneral("Aviso", "Referencia guardada correctamente.");
                    mdlReferenciaEditar.modal("hide");

                    inputReferenciaEditar.val("");
                    divGaleriaReferenciaEditar.empty();
                }
            };
        }

        function formDataEditar() {
            let formData = new FormData();
            formData.append("areaID", $('#inputEditarAreaReferencia').val());
            $.each(document.getElementById("inputReferenciaEditar").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }

        init();
    };

    $(document).ready(function () {
        planActividades.areasCatalogo = new areasCatalogo();
    });

})();