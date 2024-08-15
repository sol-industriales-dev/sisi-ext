(function () {
    $.namespace('planActividades.subareasCatalogo');

    subareasCatalogo = function () {
        tblSubArea = $("#tblSubArea");
        dialogSubArea = $('#dialogSubArea');
        dialogSubAreaEdit = $('#dialogSubAreaEdit');
        btnBuscarAubArea = $("#btnBuscarAubArea");
        btnNuevaSubArea = $("#btnNuevaSubArea");
        btnGuardarSubArea = $("#btnGuardarSubArea");
        btnEditarSubArea = $("#btnEditarSubArea");
        divGaleriaSubArea = $('#divGaleriaSubArea');
        inputReferencia = $('#inputReferencia');
        mdlReferenciaEditar = $('#mdlReferenciaEditar');
        inputReferenciaEditar = $('#inputReferenciaEditar');
        divGaleriaReferenciaEditar = $('#divGaleriaReferenciaEditar');
        btnEditarReferencia = $('#btnEditarReferencia');

        function init() {
            initTableSubArea();
            initCbo();
            btnBuscarAubArea.click(fnBuscarSubArea);
            btnNuevaSubArea.click(fnAgregarSubArea);
            btnGuardarSubArea.click(fnGuardarSubArea);
            btnEditarSubArea.click(fnEditarSubArea);
            $("#multiSelectCuadrillas").change(fnMultiCuaChange);
            initCboFiltros();
            inputReferencia.change(setGalery);
            inputReferenciaEditar.change(setGaleryEditar);
            btnEditarReferencia.click(fnEditarReferencia);
        }

        $('#selectCuadrillaNuevo').on('change', function () {
            $("#selectAreaNuevo").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: $('#selectCuadrillaNuevo').val() }, false);
        });

        $('#selectCuadrillaEdit').on('change', function () {
            $("#selectAreaEdit").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: $('#selectCuadrillaEdit').val() }, false);
        });

        $('#tblSubArea').on('click', '.btn-editar-ref', function () {
            var subAreaID = $(this).val();

            $('#inputEditarSubareaReferencia').val(subAreaID);

            mdlReferenciaEditar.modal('show');
        });

        function initTableSubArea() {
            tblSubArea = $("#tblSubArea").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/PlanActividades/GetSubAreasCatalogo'
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
                scrollY: "250px",
                scrollCollapse: true,
                searching: false,
                initComplete: function (settings, json) {
                    tblSubArea.on('click', '.btn-editar-subarea', function () {
                        var rowData = tblSubArea.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/MAZDA/PlanActividades/GetSubArea',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                var data = response.data;

                                $("#btnEditarSubArea").val(rowData["id"]);
                                $("#txtSubAreaEdit").val("");
                                $("#selectCuadrillaEdit").val("");
                                $("#selectAreaEdit").val("");

                                dialogSubAreaEdit.modal('show');

                                $("#txtSubAreaEdit").val(data.descripcion);
                                $("#selectCuadrillaEdit").val(data.cuadrillaID != 0 ? data.cuadrillaID : "");
                                $("#selectAreaEdit").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: data.cuadrillaID != 0 ? data.cuadrillaID : "" }, false);
                                $("#selectAreaEdit").val(data.areaID != 0 ? data.areaID : "");
                            }
                        });
                    });

                    tblSubArea.on('click', '.btn-eliminar-subarea', function () {
                        var rowData = tblSubArea.row($(this).closest('tr')).data();
                        $("#dialogBajaSubArea").dialog({
                            modal: true,
                            open: function () {
                                $("#txtElimSubArea").text("¿Está seguro que desea eliminar la subárea '" + rowData["descripcion"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/MAZDA/PlanActividades/RemoveSubArea',
                                        data: { id: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogBajaSubArea").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogBajaSubArea").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'cuadrilla', title: 'Cuadrilla' },
                    { data: 'area', title: 'Área' },
                    { data: 'descripcion', title: 'Subárea' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-editar-subarea btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';
                            html += '<button class="btn-editar-ref btn btn-sm btn-primary" type="button" value="' + row.id + '" style="margin-left: 5px;"> <i class="far fa-images"></i></button>';

                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-eliminar-subarea btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Eliminar"
                    }
                ]
            });
        }

        function initCboFiltros() {
            $("#multiSelectCuadrillas").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', { est: true }, false, "Todos");
            convertToMultiselect('#multiSelectCuadrillas');

            $("#multiSelectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { est: true }, false, "Todos");
            convertToMultiselect('#multiSelectArea');
        }

        function initCbo() {
            $("#selectCuadrillaNuevo").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
            $("#selectCuadrillaEdit").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);
        }

        function fnMultiCuaChange() {
            $("#multiSelectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: getValoresMultiples('#multiSelectCuadrillas') }, false, "Todos");
            convertToMultiselect('#multiSelectArea');
        }

        function fnBuscarSubArea() {
            var arrCuadrillas = getValoresMultiples('#multiSelectCuadrillas');
            var arrAreas = getValoresMultiples('#multiSelectArea');

            $.ajax({
                url: '/MAZDA/PlanActividades/GetSubAreasCatalogo',
                datatype: "json",
                type: "POST",
                data: {
                    arrCuadrillas: arrCuadrillas,
                    arrAreas: arrAreas
                },
                success: function (response) {
                    tblSubArea.clear();
                    tblSubArea.rows.add(response.data);
                    tblSubArea.draw();
                }
            });
        }

        function fnAgregarSubArea() {
            $("#txtSubAreaNuevo").val("");
            $("#selectCuadrillaNuevo").val("");
            $("#selectAreaNuevo").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: -1 }, false);

            dialogSubArea.modal('show');

            $("#txtSubAreaNuevo").focus();
        }

        function fnGuardarSubArea() {
            if ($("#txtSubAreaNuevo").val() != "" && ($('#selectAreaNuevo').val() != null && $('#selectAreaNuevo').val() != "")) {
                var request = new XMLHttpRequest();
                request.open("POST", "/MAZDA/PlanActividades/GuardarSubArea");
                request.send(formData());
                request.onload = function (response) {
                    if (request.status == 200) {
                        AlertaGeneral("Aviso", "Subárea guardada correctamente.");
                        dialogSubArea.modal("hide");
                        inputReferencia.val("");
                        divGaleriaSubArea.empty();
                        recargarTodo()
                    }
                };
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }

        function formData() {
            let formData = new FormData();
            formData.append("subArea", getSubArea());
            $.each(document.getElementById("inputReferencia").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }

        function getSubArea() {
            return {
                descripcion: $('#txtSubAreaNuevo').val(),
                areaID: $('#selectAreaNuevo').val() != "" ? $('#selectAreaNuevo').val() : 0,
                estatus: true
            };
        }

        function recargarTodo() {
            tblSubArea.ajax.reload(null, false);
            dialogSubArea.modal('hide');
            dialogSubAreaEdit.modal('hide');
            initCbo();
            $("#selectAreaNuevo").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: -1 }, false);
        }

        function fnEditarSubArea() {
            const url = 'EditarSubArea';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    id: $("#btnEditarSubArea").val(),
                    descripcion: $("#txtSubAreaEdit").val(),
                    areaID: $("#selectAreaEdit").val() != null && $("#selectAreaEdit").val() != "" ? $("#selectAreaEdit").val() : 0,
                    estatus: true
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    recargarTodo();
                }
            });
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
            divGaleriaSubArea.append(item);
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
            request.open("POST", "/MAZDA/PlanActividades/EditarReferenciaSubarea");
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
            formData.append("subareaID", $('#inputEditarSubareaReferencia').val());
            $.each(document.getElementById("inputReferenciaEditar").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }

        init();
    };

    $(document).ready(function () {
        planActividades.subareasCatalogo = new subareasCatalogo();
    });


})();