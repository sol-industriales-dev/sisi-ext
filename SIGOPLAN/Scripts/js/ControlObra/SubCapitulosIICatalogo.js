(function () {
    $.namespace('controlObra.SubCapitulosIICatalogo');

    SubCapitulosIICatalogo = function () {
        tblSubcapitulosN2 = $("#tblSubcapitulosN2");
        btnBuscarSubcapituloN2 = $("#btnBuscarSubcapituloN2");
        btnNuevoSubcapitulo = $("#btnNuevoSubcapitulo");
        modalSubcapitulo = $('#modalSubcapitulo');
        btnGuardarSubcapitulo = $("#btnGuardarSubcapitulo");

        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');

        let isEdit = false;
        let subcapitulos = [];
        const hoy = new Date();

        // Array to track the ids of the details displayed rows
        var detailRows = [];

        tblSubcapitulosN2.on('click', 'tr td.details-control', function () {
            var tr = $(this).closest('tr');
            var row = dt.row(tr);
            var idx = $.inArray(tr.attr('id'), detailRows);

            if (row.child.isShown()) {
                tr.removeClass('details');
                row.child.hide();

                // Remove from the 'open' array
                detailRows.splice(idx, 1);
            }
            else {
                tr.addClass('details');
                row.child(format(row.data())).show();

                // Add to the 'open' array
                if (idx === -1) {
                    detailRows.push(tr.attr('id'));
                }
            }
        });

        // On each draw, loop over the `detailRows` array and show any child rows
        tblSubcapitulosN2.on('draw', function () {
            $.each(detailRows, function (i, id) {
                $('#' + id + ' td.details-control').trigger('click');
            });
        });

        //EDITAR NIVEL 2
        $(document).on("click", "#tblNivel2 tbody tr td button.btn-editar-subcapitulo", function () {
            const rowData = $(this).val();

            $.ajax({
                url: '/ControlObra/ControlObra/GetSubcapituloN2',
                data: { subcapituloID: rowData },
                success: function (response) {
                    const data = response.subcapitulo;
                    const fechaInicio = new Date(moment(data.fechaInicio, "DD-MM-YYYY").format());
                    const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());

                    isEdit = true;

                    $("#lblTitulo").text('Editar Subcapitulo');
                    $("#btnGuardarSubcapitulo").val(rowData);
                    $("#txtSubcapituloN2").val(data.subcapitulo);
                    $("#selectCapitulo").val(data.capitulo_id);
                    $("#selectSubcapituloN1").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN1List', { capituloID: data.capitulo_id }, false);
                    $("#selectSubcapituloN1").val(data.subcapituloN1_id);
                    dpFechaInicio.datepicker("setDate", fechaInicio);
                    dpFechaFin.datepicker("setDate", fechaFin);
                    modalSubcapitulo.modal('show');
                }
            });
        });

        //ELIMINAR NIVEL 2
        $(document).on("click", "#tblNivel2 tbody tr td button.btn-eliminar-subcapitulo", function () {
            const rowData = $(this).val();

            $("#dialogEliminarSubcapitulo").dialog({
                modal: true,
                open: function () {
                    $("#txtEliminarSubcapitulo").text("¿Está seguro que desea eliminar el subcapitulo ?");
                },
                buttons: {
                    "Aceptar": function () {
                        var id = rowData;
                        $.ajax({
                            url: '/ControlObra/ControlObra/RemoveSubcapituloN2',
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

        function format(data) {
            const subcapitulos = obtenerSubcapitulosNivel2(data.id);
            let html = '';

            if (subcapitulos != undefined) {
                html = '<table id="tblNivel2"  class="table table-striped table-hover table-bordered tablaNivel2" style="width:97.8%;">';

                subcapitulos.forEach(function (subcapitulo) {
                    html += '<tr>' +
                        '<td>' + subcapitulo.subcapitulo + '</td>' +
                        '<td width="10px;">' + subcapitulo.fechaInicio + '</td>' +
                        '<td width="10px;">' + subcapitulo.fechaFin + '</td>' +
                        '<td width="5px;"> <button id="btnEditar" class="btn-editar-subcapitulo btn btn-sm btn-warning" type="button" value="' + subcapitulo.id + '" style=""><i class="fas fa-pencil-alt"></i></button></td>' +
                        '<td width="5px;"> <button class="btn-eliminar-subcapitulo btn btn-sm btn-danger" type="button" value="' + subcapitulo.id + '" style=""><i class="fas fa-trash-alt"></i></button></td>' +
                        '</tr>';
                });
            }

            return html += '</table>';
        };

        function init() {
            initTableSubcapitulos();
            initCbo();

            btnBuscarSubcapituloN2.click(fnBuscarSubcapitulos);
            btnNuevoSubcapitulo.click(fnAbrirModal);
            btnGuardarSubcapitulo.click(fnGuardarSubcapitulo);

            $("#selectCapitulo").change(fnCapituloChange);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
        }

        function initTableSubcapitulos() {
            const groupCapitulo = 1;

            tblSubcapitulosN2 = $("#tblSubcapitulosN2").DataTable({
                retrieve: true,
                paging: false,
                data: subcapitulos,
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                scrollY: "900px",
                scrollCollapse: true,
                searching: false,
                sortable: false,
                rowId: 'id',
                initComplete: function (settings, json) {
                },
                columns: [
                    {
                        className: 'details-control',
                        defaultContent: '',
                        data: null,
                        orderable: false
                    },
                    {
                        data: 'capitulo', sortable: false, className: 'table-cell-edit',
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        data: 'subcapitulo', sortable: false,
                        createdCell: function (td, cellData, rowData, row, col) {
                            $(td).html(cellData);
                        }
                    },
                    {
                        defaultContent: '', orderable: false
                    },
                    {
                        defaultContent: '', orderable: false
                    },
                    {
                        sortable: false,
                        defaultContent: '',
                        title: "Editar"
                    },
                    {
                        sortable: false,
                        defaultContent: '',
                        title: "Eliminar"
                    }
                ],
                columnDefs: [
                    { width: 5, targets: 0 },
                    { visible: false, targets: groupCapitulo },
                    { width: 10, targets: 3 },
                    { width: 10, targets: 4 },
                    { width: 5, targets: 5 },
                    { width: 5, targets: 6 },
                ],
                order: [[groupCapitulo, 'asc']],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(groupCapitulo, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td style="background-color: #ffcc00" colspan="6">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });
                }
            });
        }

        function initCbo() {
            $("#SelectCapitulosFiltro").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
            $("#selectCapitulo").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
        }

        function obtenerSubcapitulosNivel2(subcapituloN1_id) {
            let nivel2 = [];
            $.ajax({
                type: "GET",
                async: false, //Esta linea tendrias que poner
                data: {
                    subcapituloN1_id: subcapituloN1_id
                },
                url: "GetSubcapitulosN2Catalogo",
                success: function (data) {
                    nivel2 = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return nivel2;
        }

        function fnCapituloChange() {
            const capituloID = $('#selectCapitulo').val() == "" ? -1 : $('#selectCapitulo').val();
            $("#selectSubcapituloN1").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN1List', { capituloID: capituloID }, false);
        }

        function fnBuscarSubcapitulos() {
            //const subcapituloN1_id = $('#SelectSubcapituloN1Filtro').val() == "" ? -1 : $('#SelectSubcapituloN1Filtro').val();
            const capituloID = $('#SelectCapitulosFiltro').val() == "" ? -1 : $('#SelectCapitulosFiltro').val();

            $.post('/ControlObra/ControlObra/GetSubcapitulosN1Catalogo', { listCapitulosID: capituloID })
                .done(response => {
                    if (response.EMPTY) {
                        clearDt($("#tblSubcapitulosN2"));
                    } else {
                        subcapitulos = response.items;
                        AddRows($("#tblSubcapitulosN2"), subcapitulos)
                    }

                });
        }

        function fnAbrirModal() {
            isEdit = false;
            $("#lblTitulo").text('Nuevo Subcapitulo');
            $("#selectCapitulo").val("");
            $("#txtSubcapituloN2").val("");
            $("#txtSubcapituloN2").focus();
            dpFechaInicio.datepicker("setDate", hoy);
            dpFechaFin.datepicker("setDate", hoy);
            fnCapituloChange();
            modalSubcapitulo.modal('show');
        }

        function fnGuardarSubcapitulo() {
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

            if ($("#txtSubcapituloN2").val() != "" && $('#selectSubcapituloN1').val() != "" && $('#dpFechaInicio').val() != "" && $('#dpFechaFin').val() != "") {
                esValida = true;
            }
            else {
                esValida = false;
            }

            return esValida;
        }

        function agregarSubcapitulo() {
            var request = new XMLHttpRequest();
            request.open("POST", "/ControlObra/ControlObra/GuardarSubcapituloN2");
            request.send(formData());
            request.onload = function (response) {
                debugger;
                if (request.status == 200) {
                    AlertaGeneral("Aviso", "subcapitulo guardado correctamente.");
                    recargarTodo()
                }
            };
        }

        function formData() {
            let formData = new FormData();
            formData.append("subcapituloN2", JSON.stringify(getSubcapitulo()));
            return formData;
        }

        function getSubcapitulo() {
            return {
                subcapitulo: $('#txtSubcapituloN2').val(),
                fechaInicio: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                fechaFin: moment(dpFechaFin.val(), "DD-MM-YYYY").format(),
                subcapituloN1_id: $('#selectSubcapituloN1').val(),
                estatus: true
            };
        }

        function editarSubcapitulo() {
            const url = 'UpdateSubcapituloN2';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    subcapituloID: $("#btnGuardarSubcapitulo").val(),
                    subcapitulo: $("#txtSubcapituloN2").val(),
                    fechaInicio: dpFechaInicio.val(),
                    fechaFin: dpFechaFin.val(),
                    subcapituloN1_id: $("#selectSubcapituloN1").val()
                },
                success: function (data) {
                    debugger;
                    AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    recargarTodo();
                }
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }

        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }

        function clearDt(tbl) {
            dt = tbl.DataTable();
            dt
                .clear()
                .draw();
        }

        function recargarTodo() {
            modalSubcapitulo.modal('hide');
            fnBuscarSubcapitulos();
            $("#selectCapitulo").val("");
            fnCapituloChange();
        }


        init();
    };


    $(document).ready(function () {
        controlObra.SubCapitulosIICatalogo = new SubCapitulosIICatalogo();
    });
})();