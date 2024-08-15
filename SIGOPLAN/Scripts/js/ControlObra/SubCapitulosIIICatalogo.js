(function () {
    $.namespace('controlObra.SubCapitulosIIICatalogo');

    SubCapitulosIIICatalogo = function () {
        tblSubcapitulosN3 = $("#tblSubcapitulosN3");
        btnBuscarSubcapituloN3 = $("#btnBuscarSubcapituloN3");
        btnNuevoSubcapitulo = $("#btnNuevoSubcapitulo");
        modalSubcapitulo = $('#modalSubcapitulo');
        btnGuardarSubcapitulo = $("#btnGuardarSubcapitulo");

        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');

        let isEdit = false;
        let subcapitulos = [];
        const hoy = new Date();

        /* SUBCAPITULOS NIVEL II*/
        let detailRows = [];

        tblSubcapitulosN3.on('click', 'tr td.details-control', function () {
            debugger;
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
        tblSubcapitulosN3.on('draw', function () {
            debugger;
            $.each(detailRows, function (i, id) {
                $('#' + id + ' td.details-control').trigger('click');
            });
        });

        function format(data) {
            const subcapitulos = obtenerSubcapitulosNivel2(data.id);
            let subCapitulosN3 = [];
            let html = '';

            if (subcapitulos != undefined) {

                html = '<table id="tblNivel2"  class="table table-striped table-hover table-bordered tablaNivel2" style="width:97.8%;">';

                subcapitulos.forEach(function (subcapitulo) {

                    subCapitulosN3 = obtenerSubcapitulosNivel3(subcapitulo.id);

                    html += '<tr id="' + subcapitulo.id + '" data-toggle="collapse" data-target=".accordion' + subcapitulo.id + '">' +
                        '<td width="10px;" style="background-color: #ffff99; border-right: none;" class="details-control3"></td>' +
                        '<td style="background-color: #ffff99; border-right: none;" colspan="5">' + subcapitulo.subcapitulo + '</td>' +
                        '</tr>';

                    if (subCapitulosN3 != undefined) {

                        subCapitulosN3.forEach(function (subcapitulo) {
                            html += '<tr class="collapse accordion' + subcapitulo.subcapituloN2_id + '">' +
                                '<td width="10px;" style="border-right: none;"> </td>' +
                                '<td>' + subcapitulo.subcapitulo + '</td>' +
                                '<td  width="10px;">' + subcapitulo.fechaInicio + '</td>' +
                                '<td  width="10px;">' + subcapitulo.fechaFin + '</td>' +
                                '<td  width="5px;"> <button id="btnEditar" class="btn-editar-subcapitulo btn btn-sm btn-warning" type="button" value="' + subcapitulo.id + '" style=""><i class="fas fa-pencil-alt"></i></button></td>' +
                                '<td  width="5px;"> <button class="btn-eliminar-subcapitulo btn btn-sm btn-danger" type="button" value="' + subcapitulo.id + '" style=""><i class="fas fa-trash-alt"></i></button></td>' +
                                '</tr>';
                        });
                    }
                });
            }

            return html += '</table>';
        };
        /*FIN SUBCAPITULOS NIVEL II */


        /* SUBCAPITULOS NIVEL III*/
        //EDITAR NIVEL 3
        $(document).on("click", "#tblNivel2 tbody tr td button.btn-editar-subcapitulo", function () {
            const rowData = $(this).val();

            $.ajax({
                url: '/ControlObra/ControlObra/GetSubcapituloN3',
                data: { subcapituloID: rowData },
                success: function (response) {
                    const data = response.subcapitulo;
                    const fechaInicio = new Date(moment(data.fechaInicio, "DD-MM-YYYY").format());
                    const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());

                    isEdit = true;

                    $("#lblTitulo").text('Editar Subcapitulo');
                    $("#btnGuardarSubcapitulo").val(rowData);
                    $("#txtSubcapituloN3").val(data.subcapitulo);
                    $("#selectCapitulo").val(data.capitulo_id);
                    $("#selectSubcapituloN1").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN1List', { capituloID: data.capitulo_id }, false);
                    $("#selectSubcapituloN1").val(data.subcapituloN1_id);
                    $("#selectSubcapituloN2").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN2List', { subcapituloN1_id: data.subcapituloN1_id }, false);
                    $("#selectSubcapituloN2").val(data.subcapituloN2_id);
                    dpFechaInicio.datepicker("setDate", fechaInicio);
                    dpFechaFin.datepicker("setDate", fechaFin);
                    modalSubcapitulo.modal('show');
                }
            });
        });

        //ELIMINAR NIVEL 3
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
                            url: '/ControlObra/ControlObra/RemoveSubcapituloN3',
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
        /*FIN SUBCAPITULOS NIVEL III */


        function init() {
            initTableSubcapitulos();
            initCbo();

            btnBuscarSubcapituloN3.click(fnBuscarSubcapitulos);
            btnNuevoSubcapitulo.click(fnAbrirModal);
            btnGuardarSubcapitulo.click(fnGuardarSubcapitulo);

            $("#selectCapitulo").change(fnCapituloChange);
            $("#selectSubcapituloN1").change(fnSubCapituloN1Change);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
        }

        function initTableSubcapitulos() {
            const groupCapitulo = 1;

            tblSubcapitulosN3 = $("#tblSubcapitulosN3").DataTable({
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

        function obtenerSubcapitulosNivel3(subcapituloN2_id) {
            let nivel3 = [];

            $.ajax({
                type: "GET",
                async: false, //Esta linea tendrias que poner
                data: {
                    subcapituloN2_id: subcapituloN2_id
                },
                url: "GetSubcapitulosN3Catalogo",
                success: function (data) {
                    nivel3 = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return nivel3;
        }

        function fnCapituloChange() {
            const capituloID = $('#selectCapitulo').val() == "" ? -1 : $('#selectCapitulo').val();
            $("#selectSubcapituloN1").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN1List', { capituloID: capituloID }, false);

            fnSubCapituloN1Change();
        }

        function fnSubCapituloN1Change() {
            const subcapituloN1_id = $('#selectSubcapituloN1').val() == "" ? -1 : $('#selectSubcapituloN1').val();
            $("#selectSubcapituloN2").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN2List', { subcapituloN1_id: subcapituloN1_id }, false);
        }

        function fnBuscarSubcapitulos() {
            const capituloID = $('#SelectCapitulosFiltro').val() == "" ? -1 : $('#SelectCapitulosFiltro').val();

            $.post('/ControlObra/ControlObra/GetSubcapitulosN1Catalogo', { listCapitulosID: capituloID })
                .done(response => {
                    if (response.EMPTY) {
                        clearDt($("#tblSubcapitulosN3"));
                    } else {
                        subcapitulos = response.items;
                        AddRows($("#tblSubcapitulosN3"), subcapitulos)
                    }

                });
        }

        function fnAbrirModal() {
            isEdit = false;
            $("#lblTitulo").text('Nuevo Subcapitulo');
            $("#selectCapitulo").val("");
            $("#txtSubcapituloN3").val("");
            $("#txtSubcapituloN3").focus();
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

            if ($("#txtSubcapituloN3").val() != "" && $('#selectSubcapituloN2').val() != "" && $('#dpFechaInicio').val() != "" && $('#dpFechaFin').val() != "") {
                esValida = true;
            }
            else {
                esValida = false;
            }

            return esValida;
        }

        function agregarSubcapitulo() {
            var request = new XMLHttpRequest();
            request.open("POST", "/ControlObra/ControlObra/GuardarSubcapituloN3");
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
            formData.append("subcapituloN3", JSON.stringify(getSubcapitulo()));
            return formData;
        }

        function getSubcapitulo() {
            return {
                subcapitulo: $('#txtSubcapituloN3').val(),
                fechaInicio: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                fechaFin: moment(dpFechaFin.val(), "DD-MM-YYYY").format(),
                subcapituloN2_id: $('#selectSubcapituloN2').val(),
                estatus: true
            };
        }

        function editarSubcapitulo() {
            const url = 'UpdateSubcapituloN3';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {
                    subcapituloID: $("#btnGuardarSubcapitulo").val(),
                    subcapitulo: $("#txtSubcapituloN3").val(),
                    fechaInicio: dpFechaInicio.val(),
                    fechaFin: dpFechaFin.val(),
                    subcapituloN2_id: $("#selectSubcapituloN2").val()
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
        controlObra.SubCapitulosIIICatalogo = new SubCapitulosIIICatalogo();
    });
})();