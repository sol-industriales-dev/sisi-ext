(function () {

    $.namespace('planActividades.equiposCatalogo');

    equiposCatalogo = function () {

        tblEquipo = $("#tblEquipo");
        btnNuevoEquipo = $("#btnNuevoEquipo");
        btnGuardarEquipo = $("#btnGuardarEquipo");
        btnEditarEquipo = $("#btnEditarEquipo");
        btnBuscar_Equipo = $("#btnBuscar_Equipo");

        inputReferencia = $('#inputReferencia');
        divGaleriaNuevoEquipo = $('#divGaleriaNuevoEquipo');
        dialogNuevoEquipo = $('#dialogNuevoEquipo');

        inputReferenciaEditar = $('#inputReferenciaEditar');
        divGaleriaReferenciaEditar = $('#divGaleriaReferenciaEditar');
        mdlReferenciaEditar = $('#mdlReferenciaEditar');
        btnEditarReferencia = $('#btnEditarReferencia');

        $('a[href$="#tabEquipo"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        $('#tblEquipo').on('click', '.btn-editar-ref', function () {
            var equipoID = $(this).val();

            $('#inputEditarEquipoReferencia').val(equipoID);

            mdlReferenciaEditar.modal('show');
        });

        function init() {
            //initTableEquipo();
            initCbo();
            btnNuevoEquipo.click(fnAgregarEquipo);
            btnGuardarEquipo.click(fnGuardarEquipo);
            btnEditarEquipo.click(fnEditarEquipo);
            btnBuscar_Equipo.click(fnBuscarEquipo);
            inputReferencia.change(setGalery);
            inputReferenciaEditar.change(setGaleryEditar);
            btnEditarReferencia.click(fnEditarReferencia);
            $("#multiSelectCuadrillas").change(fnMultiCuaChange);
            $("#multiSelectArea").change(fnMultiAreaChange);
            initCboFiltros();
            initTableEquipo();
        }

        $('#selectEquipoNuevoArea').on('change', function () {
            $("#selectEquipoNuevoSubArea").fillCombo('/MAZDA/PlanActividades/GetSubAreasList',{ areaID: $('#selectEquipoNuevoArea').val()}, false);
        });

        $('#selectEquipoEditArea').on('change', function () {
            debugger;
            $("#selectEquipoEditSubarea").fillCombo('/MAZDA/PlanActividades/GetSubAreasList', { areaID: $('#selectEquipoEditArea').val() }, false);
        });

        $('#selectEquipoNuevoCuadrilla').on('change', function () {
            $("#selectEquipoNuevoArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: $('#selectEquipoNuevoCuadrilla').val() }, false);
            $("#selectEquipoNuevoSubArea").fillCombo('/MAZDA/PlanActividades/GetSubAreasList', { areaID: -1 }, false);
        });

        $('#selectEquipoEditCuadrilla').on('change', function () {
            $("#selectEquipoEditArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: $('#selectEquipoEditCuadrilla').val() }, false);
            $("#selectEquipoEditSubarea").fillCombo('/MAZDA/PlanActividades/GetSubAreasList', { areaID: -1 }, false);
        });

        function fnMultiCuaChange() {
            $("#multiSelectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: getValoresMultiples('#multiSelectCuadrillas') }, false, "Todos");
            convertToMultiselect('#multiSelectArea');

            $("#multiSelectSubAreas").fillCombo('/MAZDA/PlanActividades/GetSubAreasList', { areaID: getValoresMultiples('#multiSelectArea'), cuadrillaID: getValoresMultiples('#multiSelectCuadrillas') }, false, "Todos");
            convertToMultiselect('#multiSelectSubAreas');
        }

        function fnMultiAreaChange() {
            $("#multiSelectSubAreas").fillCombo('/MAZDA/PlanActividades/GetSubAreasList', { areaID: getValoresMultiples('#multiSelectArea') }, false, "Todos");
            convertToMultiselect('#multiSelectSubAreas');
        }

        function fnAgregarEquipo() {
            $("#txtEquipoNuevoDescripcion").val("");
            $("#txtEquipoNuevoCaracteristicas").val("");
            $("#txtEquipoNuevoModelo").val("");
            $("#txtEquipoNuevoTonelaje").val("");
            $("#selectEquipoNuevoArea").val("");
            $("#selectEquipoNuevoSubArea").val("");
            $("#txtEquipoNuevoCantidad").val("");
            $("#dialogNuevoEquipo").modal('show');
            $("#txtEquipoNuevoDescripcion").focus();
        }

        function fnEditarEquipo() {
            const url = 'EditarEquipo';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    id: $("#btnEditarEquipo").val(),
                    descripcion: $("#txtEquipoEditDescripcion").val(),
                    caracteristicas: $("#txtEquipoEditCaracteristicas").val(),
                    modelo: $("#txtEquipoEditModelo").val(),
                    tonelaje: $("#txtEquipoEditTonelaje").val(),
                    subAreaID: $("#selectEquipoEditSubarea").val() != null && $("#selectEquipoEditSubarea").val() != "" ? $("#selectEquipoEditSubarea").val() : 0,
                    subArea: $("#selectEquipoEditSubarea").val() != null && $("#selectEquipoEditSubarea").val() != "" ? $("#selectEquipoEditSubarea").find('option:selected').text().trim() : "",
                    cantidad: $("#txtEquipoEditCantidad").val(),
                    estatus: true
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información del equipo.");
                    recargarTodo();
                }
            });
        }

        function fnEditarReferencia() {
            var request = new XMLHttpRequest();
            request.open("POST", "/MAZDA/PlanActividades/EditarReferenciaEquipo");
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

        function fnBuscarEquipo() {
            var arrCuadrillas = getValoresMultiples('#multiSelectCuadrillas');
            var arrAreas = getValoresMultiples('#multiSelectArea');
            var arrSubAreas = getTextosMultiples('#multiSelectSubAreas');

            $.ajax({
                url: '/MAZDA/PlanActividades/GetEquiposCatalogo',
                datatype: "json",
                type: "POST",
                data: {
                    arrCuadrillas: arrCuadrillas,
                    arrAreas: arrAreas,
                    arrSubAreas: arrSubAreas
                },
                success: function (response) {             
                    tblEquipo.clear();
                    tblEquipo.rows.add(response.data);
                    tblEquipo.draw();
                }
            });
        }

        function fnGuardarEquipo() {
            if ($("#txtEquipoNuevoDescripcion").val() != "") {
                var request = new XMLHttpRequest();
                request.open("POST", "/MAZDA/PlanActividades/GuardarEquipo");
                request.send(formData());
                request.onload = function (response) {
                    if (request.status == 200) {
                        AlertaGeneral("Aviso", "Equipo guardado correctamente.");
                        dialogNuevoEquipo.modal("hide");

                        inputReferencia.val("");
                        divGaleriaNuevoEquipo.empty();
                        recargarTodo()
                    }
                };
            } else {
                AlertaGeneral("Alerta", "Falta información.");
            }
        }

        function initTableEquipo(data) {
            var arrCuadrillas = getValoresMultiples('#multiSelectCuadrillas');
            var arrAreas = getValoresMultiples('#multiSelectArea');
            var arrSubAreas = getTextosMultiples('#multiSelectSubAreas');

            tblEquipo = $("#tblEquipo").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/PlanActividades/GetEquiposCatalogo',
                    data: function (data) {
                        data.arrCuadrillas = arrCuadrillas,
                        data.arrAreas = arrAreas,
                        data.arrSubAreas = arrSubAreas
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
                    tblEquipo.on('click', '.btn-editar-equi', function () {
                        var rowData = tblEquipo.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/MAZDA/PlanActividades/GetEquipoMAZDA',
                            data: { id: rowData["id"] },
                            success: function (response) {
                                var data = response.data;

                                $("#btnEditarEquipo").val(rowData["id"]);

                                $("#txtEquipoEditDescripcion").val("");
                                $("#txtEquipoEditCaracteristicas").val("");
                                $("#txtEquipoEditModelo").val("");
                                $("#txtEquipoEditTonelaje").val("");
                                $("#selectEquipoEditCuadrilla").val("");
                                $("#selectEquipoEditArea").val("");
                                $("#selectEquipoEditSubarea").val("");
                                $("#txtEquipoEditCantidad").val("");

                                $("#dialogEditarEquipo").modal('show');

                                $("#txtEquipoEditDescripcion").val(data.descripcion);
                                $("#txtEquipoEditCaracteristicas").val(data.caracteristicas);
                                $("#txtEquipoEditModelo").val(data.modelo);
                                $("#txtEquipoEditTonelaje").val(data.tonelaje);
                                $("#selectEquipoEditCuadrilla").val(data.cuadrillaID != 0 ? data.cuadrillaID : "");
                                $("#selectEquipoEditArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { cuadrillasID: data.cuadrillaID != 0 ? data.cuadrillaID : "" }, false);
                                $("#selectEquipoEditArea").val(data.areaID != 0 ? data.areaID : "");
                                $("#selectEquipoEditSubarea").fillCombo('/MAZDA/PlanActividades/GetSubAreasList', { areaID: data.areaID != 0 ? data.areaID : "" }, false);
                                $("#selectEquipoEditSubarea").val(data.subAreaID != 0 ? data.subAreaID : "");
                                $("#txtEquipoEditCantidad").val(data.cantidad);                               
                            }
                        });
                    });

                    tblEquipo.on('click', '.btn-eliminar-equi', function () {
                        var rowData = tblEquipo.row($(this).closest('tr')).data();

                        $("#dialogBajaEquipo").dialog({
                            modal: true,
                            open: function () {
                                $("#txtElimEquipo").text("¿Está seguro que desea eliminar el equipo '" + rowData["descripcion"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/MAZDA/PlanActividades/RemoveEquipo',
                                        data: { id: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogBajaEquipo").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogBajaEquipo").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'descripcion', title: 'Equipo' },
                    { data: 'caracteristicas', title: 'Tipo/Caracteristicas' },
                    { data: 'modelo', title: 'Modelo'},
                    { data: 'tonelaje', title: 'Tonelaje' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'subArea', title: 'Subárea' },                 
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-editar-equi btn btn-sm btn-warning glyphicon glyphicon-cog" type="button" value="' + row.id + '" style=""></button>';
                            html += '<button class="btn-editar-ref btn btn-sm btn-primary" type="button" value="' + row.id + '" style="margin-left: 5px;"><span class="fa fa-photo"></span></button>';

                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';

                            html += '<button class="btn-eliminar-equi btn btn-sm btn-danger glyphicon glyphicon-remove" type="button" value="' + row.id + '" style=""></button>';

                            return html;
                        },
                        title: "Eliminar"
                    }
                ]
                //columnDefs: [
                //    { "className": "dt-center", "targets": [0, 1, 2, 3, 4] },
                //    { "width": "5%", "targets": [3, 5] }
                //]
            });
        }

        function initCboFiltros() {
            $("#multiSelectCuadrillas").fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', { est: true }, false, "Todos");
            convertToMultiselect('#multiSelectCuadrillas');

            $("#multiSelectArea").fillCombo('/MAZDA/PlanActividades/GetAreasList', { est: true }, false, "Todos");
            convertToMultiselect('#multiSelectArea');

            $("#multiSelectSubAreas").fillCombo('/MAZDA/PlanActividades/GetSubAreasList', { est: true}, false, "Todos");
            convertToMultiselect('#multiSelectSubAreas');
        }

        function initCbo() {          
            $('#selectEquipoNuevoCuadrilla').fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);        
            $('#selectEquipoEditCuadrilla').fillCombo('/MAZDA/PlanActividades/GetCuadrillasList', null, false);       
        }

        function recargarTodo() {
            tblEquipo.ajax.reload(null, false);
            $("#dialogNuevoEquipo").modal('hide');
            $("#dialogEditarEquipo").modal('hide');
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
            divGaleriaNuevoEquipo.append(item);
        }
        function formData() {
            let formData = new FormData();
            formData.append("equipo", JSON.stringify(getEquipo()));
            $.each(document.getElementById("inputReferencia").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }
        function getEquipo() {
            return {

                descripcion: $('#txtEquipoNuevoDescripcion').val(),
                caracteristicas: $('#txtEquipoNuevoCaracteristicas').val(),
                modelo: $('#txtEquipoNuevoModelo').val(),
                tonelaje: $('#txtEquipoNuevoTonelaje').val(),
                subAreaID: $('#selectEquipoNuevoSubArea').val() != "" ? $('#selectEquipoNuevoSubArea').val() : 0,
                subArea: $('#selectEquipoNuevoSubArea').val() != "" ? $('#selectEquipoNuevoSubArea').find('option:selected').text().trim() : "",
                cantidad: $('#txtEquipoNuevoCantidad').val(),
                //cuadrillaID: $('#selectEquipoNuevoCuadrilla').val() != "" ? $('#selectEquipoNuevoCuadrilla').val() : 0,
                //periodo: $('#selectEquipoNuevoPeriodo').val() != "" ? $('#selectEquipoNuevoPeriodo').val() : 0,
                estatus: true
            };
        }

        //Editar
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
        function formDataEditar() {
            let formData = new FormData();
            formData.append("equipoID", $('#inputEditarEquipoReferencia').val());
            $.each(document.getElementById("inputReferenciaEditar").files, function (i, file) {
                formData.append("files[]", file);
            });
            return formData;
        }

        init();
    };

    $(document).ready(function () {
        planActividades.equiposCatalogo = new equiposCatalogo();
    });

})();