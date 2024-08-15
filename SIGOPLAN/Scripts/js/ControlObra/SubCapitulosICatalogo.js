(function () {
    $.namespace('controlObra.SubCapitulosICatalogo');

    SubCapitulosICatalogo = function () {
        tblSubcapitulos = $("#tblSubcapitulos");
        btnBuscarSubcapitulos = $("#btnBuscarSubcapitulos");
        btnNuevoSubCapitulo = $("#btnNuevoSubCapitulo");
        btnGuardarSubcapitulo = $("#btnGuardarSubcapitulo");

        modalSubcapitulo = $('#modalSubcapitulo');

        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');

        let isEdit = false;
        const hoy = new Date();

        function init() {
            initTableSubcapitulos();
            initCbo();

            btnBuscarSubcapitulos.click(fnBuscarSubcapitulos);
            btnNuevoSubCapitulo.click(fnAbrirModalFaseNueva);
            btnGuardarSubcapitulo.click(fnGuardarSubCapitulo);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
        }

        function initTableSubcapitulos() {
            var groupColumn = 0;
            tblSubcapitulos = $("#tblSubcapitulos").DataTable({
                retrieve: true,
                paging: false,
                ajax: {
                    url: '/ControlObra/ControlObra/GetSubcapitulosN1Catalogo',
                    dataSrc: function (response) {
                        if (response.EMPTY) {
                            return [];
                        } else {
                            return response.items;
                        }
                    }
                },
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                sortable: false,
                initComplete: function (settings, json) {
                    tblSubcapitulos.on('click', '.btn-editar-subcapitulo', function () {
                        var rowData = tblSubcapitulos.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/ControlObra/ControlObra/GetSubcapituloN1',
                            data: { subcapituloID: rowData["id"] },
                            success: function (response) {
                                const data = response.subcapitulo;
                                const fechaInicio = new Date(moment(data.fechaInicio, "DD-MM-YYYY").format());
                                const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());
                                isEdit = true;

                                $("#btnGuardarSubcapitulo").val(rowData["id"]);
                                $("#txtSubcapitulo").val(data.subcapitulo);
                                $("#selectCapitulo").val(data.capituloID);
                                dpFechaInicio.datepicker("setDate", fechaInicio);
                                dpFechaFin.datepicker("setDate", fechaFin);
                                modalSubcapitulo.modal('show');
                            }
                        });
                    });

                    tblSubcapitulos.on('click', '.btn-eliminar-subcapitulo', function () {
                        var rowData = tblSubcapitulos.row($(this).closest('tr')).data();
                        $("#dialogEliminarSubcapitulo").dialog({
                            modal: true,
                            open: function () {
                                $("#txtEliminarSubcapitulo").text("¿Está seguro que desea eliminar el subcapitulo '" + rowData["subcapitulo"] + "'?");
                            },
                            buttons: {
                                "Aceptar": function () {
                                    var id = rowData["id"];
                                    $.ajax({
                                        url: '/ControlObra/ControlObra/RemoveSubcapituloN1',
                                        data: { subcapituloID: parseInt(id) },
                                        success: function () {
                                            recargarTodo();
                                            $("#dialogEliminarSubcapitulo").dialog("close");
                                        }
                                    });
                                },
                                "Cancelar": function () {
                                    $("#dialogEliminarSubcapitulo").dialog("close");
                                }
                            }
                        });
                    });
                },
                columns: [
                    { data: 'capitulo', sortable: false },
                    { data: 'subcapitulo', title: 'Subcapitulo', sortable: false },
                    { data: 'fechaInicio', title: 'Fecha Inicio', sortable: false },
                    { data: 'fechaFin', title: 'Fecha Fin', sortable: false },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-editar-subcapitulo btn btn-sm btn-warning" value="' + row.id + '" style=""><i class="fas fa-pencil-alt"></i></button>';
                            return html;
                        },
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-eliminar-subcapitulo btn btn-sm btn-danger" type="button" value="' + row.id + '" style=""><i class="fas fa-trash-alt"></i></button>';
                            return html;
                        },
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { visible: false, targets: groupColumn },
                    { width: 100, targets: 2 },
                    { width: 100, targets: 3 },
                    { width: 5, targets: 4 },
                    { width: 5, targets: 5 }
                ],
                order: [[groupColumn, 'asc']],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td style="background-color: #ffff99" colspan="5">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });
                }
            });
        }

        function initCbo() {
            $("#multiSelectCapitulos").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
            $("#selectCapitulo").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);

            convertToMultiselect('#multiSelectCapitulos');
        }

        function fnBuscarSubcapitulos() {
            const capitulos = getValoresMultiples('#multiSelectCapitulos');

            $.ajax({
                url: '/ControlObra/ControlObra/GetSubcapitulosN1Catalogo',
                datatype: "json",
                type: "POST",
                data: {
                    listCapitulosID: capitulos
                },
                success: function (response) {
                    const data = response.EMPTY ? [] : response.items;

                    tblSubcapitulos.clear();
                    tblSubcapitulos.rows.add(data);
                    tblSubcapitulos.draw();
                }
            });
        }

        function fnAbrirModalFaseNueva() {
            isEdit = false;
            $("#txtSubcapitulo").val("");
            $("#txtSubcapitulo").focus();
            dpFechaInicio.datepicker("setDate", hoy);
            dpFechaFin.datepicker("setDate", hoy);
            modalSubcapitulo.modal('show');
        }

        function fnGuardarSubCapitulo() {
            if (!validarGuardar()) {
                return;
            }

            if (isEdit) {
                editarSubcapitulo();
            } else {
                agregarSubcapitulo();
            }
        }

        function validarGuardar() {
            let esValida = false

            debugger;

            if ($("#txtSubcapitulo").val() != "" && $('#selectCapitulo').val() != "" && $('#dpFechaInicio').val() != "" && $('#dpFechaFin').val() != "") {
                esValida = true;
            }
            else {
                esValida = false;
            }

            return esValida;
        }

        function agregarSubcapitulo() {
            var request = new XMLHttpRequest();
            request.open("POST", "/ControlObra/ControlObra/GuardarSubcapituloN1");
            request.send(formData());
            request.onload = function (response) {
                if (request.status == 200) {
                    AlertaGeneral("Aviso", "Subcapitulo guardada correctamente.");
                    recargarTodo()
                }
            };
        }

        function formData() {
            let formData = new FormData();
            formData.append("subcapituloN1", JSON.stringify(getSubcapitulo()));
            return formData;
        }

        function getSubcapitulo() {
            return {
                subcapitulo: $('#txtSubcapitulo').val(),
                fechaInicio: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                fechaFin: moment(dpFechaFin.val(), "DD-MM-YYYY").format(),
                capitulo_id: $('#selectCapitulo').val(),
                estatus: true
            };
        }

        function editarSubcapitulo() {
            const url = 'UpdateSubcapituloN1';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    subcapituloID: $("#btnGuardarSubcapitulo").val(),
                    subcapitulo: $("#txtSubcapitulo").val(),
                    fechaInicio: dpFechaInicio.val(),
                    fechaFin: dpFechaFin.val(),
                    capituloID: $("#selectCapitulo").val()
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    recargarTodo();
                }
            });
        }

        function recargarTodo() {
            tblSubcapitulos.ajax.reload(null, false);
            modalSubcapitulo.modal('hide');
            initCbo();
        }

        init();
    };

    $(document).ready(function () {
        controlObra.SubCapitulosICatalogo = new SubCapitulosICatalogo();
    });
})();