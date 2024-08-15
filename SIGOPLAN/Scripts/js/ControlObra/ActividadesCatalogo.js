(function () {
    $.namespace('controlObra.ActividadesCatalogo');

    ActividadesCatalogo = function () {
        tblActividades = $("#tblActividades");
        btnBuscarActividades = $("#btnBuscarActividades");
        btnNuevaActividad = $("#btnNuevaActividad");
        btnGuardarActividad = $("#btnGuardarActividad");

        modalActividades = $('#modalActividades');

        let isEdit = false;
        const hoy = new Date();
        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');

        /* SUBCAPITULOS NIVEL II*/
        let detailRows = [];

        tblActividades.on('click', 'tr td.details-control', function () {
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
        tblActividades.on('draw', function () {
            debugger;
            $.each(detailRows, function (i, id) {
                $('#' + id + ' td.details-control').trigger('click');
            });
        });

        function format(data) {
            const actividadesN1 = obtenerActividades(data.id, -1, -1);
            const subcapitulos = obtenerSubcapitulosNivel2(data.id);
            let subCapitulosN3 = [];
            let table = '';
            let body = '';

            if (subcapitulos != undefined || actividadesN1 != undefined) {
                table = document.createElement('table');
                table.setAttribute('id', 'tblNivel2');
                table.setAttribute('style', 'width:100%');
                table.classList.add('table');
                table.classList.add('tablaNivel2');

                body = document.createElement('tbody');

                //Actividades
                if (actividadesN1 != undefined) {
                    actividadesN1.forEach(function (actividad) {
                        body.append(tablaActividades(actividad, '', '', '', ''));
                    });
                }

                //Subcapitulo II, III y Actividades
                if (subcapitulos != undefined) {

                    subcapitulos.forEach(function (subcapituloN2) {

                        const actividadesN2 = obtenerActividades(-1, subcapituloN2.id, -1);
                        subCapitulosN3 = obtenerSubcapitulosNivel3(subcapituloN2.id);

                        //SUBCAPITULO II
                        let tr = document.createElement('tr');
                        tr.setAttribute('id', subcapituloN2.id);
                        tr.setAttribute('data-toggle', 'collapse');
                        tr.setAttribute('data-target', '.accordion' + subcapituloN2.id);
                        tr.setAttribute('style', 'background-color: #ffff99');

                        let td = document.createElement('td');
                        td.setAttribute('style', 'border-right: none');
                        td.setAttribute('width', '40px');
                        td.classList.add('details-control3');

                        let tdSubcapitulo = document.createElement('td');
                        tdSubcapitulo.setAttribute('style', 'border-left: none');
                        tdSubcapitulo.setAttribute('colspan', '7');
                        tdSubcapitulo.textContent = subcapituloN2.subcapitulo

                        $(tr).append(td);
                        $(tr).append(tdSubcapitulo);

                        body.append(tr);

                        //ACTIVIDAD
                        if (actividadesN2 != undefined) {
                            actividadesN2.forEach(function (actividad) {
                                body.append(tablaActividades(actividad, 'collapse', 'accordion', actividad.subcapituloN2_id));
                            });
                        }

                        if (subCapitulosN3 != undefined) {
                            //SUBCAPITULO III
                            subCapitulosN3.forEach(function (subcapituloN3) {
                                const actividadesN3 = obtenerActividades(-1, -1, subcapituloN3.id);

                                let tr = document.createElement('tr');
                                tr.setAttribute('data-toggle', 'collapse');
                                tr.setAttribute('data-target', '.actividad' + subcapituloN3.id);
                                tr.classList.add('collapse');
                                tr.classList.add('accordion' + subcapituloN3.subcapituloN2_id);
                                tr.setAttribute('style', 'background-color: #9bc2e6');

                                let td = document.createElement('td');
                                td.setAttribute('style', 'border-right: none');
                                td.setAttribute('width', '40px');
                                td.classList.add('details-control3');

                                let tdSubcapitulo = document.createElement('td');
                                tdSubcapitulo.setAttribute('style', 'border-left: none');
                                tdSubcapitulo.setAttribute('colspan', '7');
                                tdSubcapitulo.textContent = subcapituloN3.subcapitulo

                                $(tr).append(td);
                                $(tr).append(tdSubcapitulo);
                                body.append(tr);

                                //ACTIVIDAD
                                if (actividadesN3 != undefined) {
                                    actividadesN3.forEach(function (actividad) {
                                        body.append(tablaActividades(actividad, 'collapse', 'actividad', actividad.subcapituloN3_id));
                                    });
                                }
                            });
                        }
                    });
                }
            }

            table.append(body);

            return table;
        };
        /*FIN SUBCAPITULOS NIVEL II */


        /* ACTIVIDADES */
        //EDITAR NIVEL 3
        $(document).on("click", "#tblNivel2 tbody tr td button.btn-editar-actividad", function () {
            const rowData = $(this).val();

            $.ajax({
                url: '/ControlObra/ControlObra/GetActividad',
                data: { actividadID: rowData },
                success: function (response) {
                    const data = response.actividad;
                    const fechaInicio = new Date(moment(data.fechaInicio, "DD-MM-YYYY").format());
                    const fechaFin = new Date(moment(data.fechaFin, "DD-MM-YYYY").format());

                    isEdit = true;
                    $("#lblTitulo").text('Editar Actividad');
                    $("#btnGuardarActividad").val(rowData);
                    $("#txtActividad").val(data.actividad);
                    $("#txtCantidad").val(formatValue(data.cantidad));
                    $("#selectCapitulo").val(data.capitulo_id);
                    $("#selectSubcapituloN1").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN1List', { capituloID: data.capitulo_id }, false);
                    $("#selectSubcapituloN1").val(data.subcapituloN1_id);
                    $("#selectSubcapituloN2").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN2List', { subcapituloN1_id: data.subcapituloN1_id }, false);
                    $("#selectSubcapituloN2").val(data.subcapituloN2_id == -1 ? "" : data.subcapituloN2_id);
                    $("#selectSubcapituloN3").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN3List', { subcapituloN2_id: data.subcapituloN2_id }, false);
                    $("#selectSubcapituloN3").val(data.subcapituloN3_id);
                    $("#selectActividadPadre").fillCombo('/ControlObra/ControlObra/GetActividadesList', { subcapitulosN1_id: data.subcapituloN1_id == null ? -1 : data.subcapituloN1_id, subcapitulosN2_id: data.subcapituloN2_id == null ? -1 : data.subcapituloN2_id, subcapitulosN3_id: data.subcapituloN3_id == null ? -1 : data.subcapituloN3_id }, false);
                    $("#selectActividadPadre").val(data.actividadPadre_id);
                    $("#selectUnidad").val(data.unidad_id);
                    $("#txtCostoUnidad").val(formatValue(data.costoUnidad));

                    dpFechaInicio.datepicker("setDate", fechaInicio);
                    dpFechaFin.datepicker("setDate", fechaFin);
                    modalActividades.modal('show');
                }
            });
        });

        //ELIMINAR NIVEL 3
        $(document).on("click", "#tblNivel2 tbody tr td button.btn-eliminar-actividad", function () {
            const rowData = $(this).val();

            $("#dialogEliminarActividad").dialog({
                modal: true,
                open: function () {
                    $("#txtEliminarActividad").text("¿Está seguro que desea eliminar la actividad ?");
                },
                buttons: {
                    "Aceptar": function () {
                        var id = rowData;
                        $.ajax({
                            url: '/ControlObra/ControlObra/RemoveActividad',
                            data: { actividadID: parseInt(id) },
                            success: function () {
                                recargarTodo();
                                $("#dialogEliminarActividad").dialog("close");
                            }
                        });
                    },
                    "Cancelar": function () {
                        $("#dialogEliminarActividad").dialog("close");
                    }
                }
            });
        });
        /*FIN ACTIVIDADES */

        function init() {
            initTableActividades();
            initCbo();

            btnBuscarActividades.click(fnBuscarActividades);
            btnNuevaActividad.click(fnAbrirModalActividadNueva);
            btnGuardarActividad.click(fnGuardarSActividad);

            $("#selectCapitulo").change(fnCapituloChange);
            $("#selectSubcapituloN1").change(fnSubCapituloN1Change);
            $("#selectSubcapituloN2").change(fnSubCapituloN2Change);
            $("#selectSubcapituloN3").change(fnSubCapituloN3Change);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
        }

        function initTableActividades() {
            const groupCapitulo = 1;

            tblActividades = $("#tblActividades").DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                scrollY: "800px",
                scrollCollapse: true,
                searching: false,
                sortable: false,
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
                    { width: 20, targets: 3 },
                    { width: 20, targets: 4 },
                    { width: 70, targets: 5 },
                    { width: 60, targets: 6 },
                    { width: 10, targets: 7 },
                    { width: 10, targets: 8 },
                ],
                order: [[groupCapitulo, 'asc']],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(groupCapitulo, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td style="background-color: #ffcc00" colspan="8">' + group + '</td></tr>'
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

            $("#selectUnidad").fillCombo('/ControlObra/ControlObra/GetUnidadesList', null, false);
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

        function obtenerActividades(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id) {
            let nivel2 = [];
            $.ajax({
                type: "GET",
                async: false,
                data: {
                    subcapitulosN1_id: subcapitulosN1_id,
                    subcapitulosN2_id: subcapitulosN2_id,
                    subcapitulosN3_id: subcapitulosN3_id
                },
                url: "GetActividadesCatalogo",
                success: function (data) {
                    nivel2 = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return nivel2;
        }

        function tablaActividades(actividad, claseColapse, claseExtra, subcapituloID) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.id);

            if (claseColapse != '') tr.classList.add(claseColapse);
            if (claseExtra != '') tr.classList.add(claseExtra + subcapituloID);

            let td = document.createElement('td');
            td.setAttribute('style', 'border-right: none');
            td.setAttribute('width', '1.5%');

            let tdActividad = document.createElement('td');
            tdActividad.setAttribute('width', '38.5%');
            tdActividad.setAttribute('style', 'border-left: none');
            tdActividad.textContent = actividad.actividad;

            let tdUnidad = document.createElement('td');
            tdUnidad.setAttribute('width', '2.5%');
            tdUnidad.textContent = actividad.unidad;

            let tdCantidad = document.createElement('td');
            tdCantidad.setAttribute('width', '2.5%');
            tdCantidad.setAttribute('align', 'right');
            tdCantidad.textContent = formatValue(actividad.cantidad);

            let tdFechaInicio = document.createElement('td');
            tdFechaInicio.setAttribute('width', '2%');
            tdFechaInicio.setAttribute('align', 'right');
            tdFechaInicio.textContent = actividad.fechaInicio;

            let tdFechaFin = document.createElement('td');
            tdFechaFin.setAttribute('width', '2%');
            tdFechaFin.setAttribute('align', 'right');
            tdFechaFin.textContent = actividad.fechaFin;

            let tdEditar = document.createElement('td');
            tdEditar.setAttribute('width', '2%');
            tdEditar.setAttribute('align', 'center');
            $(tdEditar).append(document.createElement('button'));
            $(tdEditar).find('button').addClass('btn-editar-actividad btn btn-sm btn-warning');
            $(tdEditar).find('button').attr('value', actividad.id);
            $(tdEditar).find('button').append(document.createElement('i'));
            $(tdEditar).find('button').find('i').addClass('fas fa-pencil-alt');

            let tdEliminar = document.createElement('td');
            tdEliminar.setAttribute('width', '2%');
            tdEliminar.setAttribute('align', 'center');
            $(tdEliminar).append(document.createElement('button'));
            $(tdEliminar).find('button').addClass('btn-eliminar-actividad btn btn-sm btn-danger');
            $(tdEliminar).find('button').attr('value', actividad.id);
            $(tdEliminar).find('button').append(document.createElement('i'));
            $(tdEliminar).find('button').find('i').addClass('fas fa-trash-alt');

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdFechaInicio);
            $(tr).append(tdFechaFin);
            $(tr).append(tdEditar);
            $(tr).append(tdEliminar);

            return tr;
        }

        function fnCapituloChange() {
            const capituloID = $('#selectCapitulo').val() == "" ? -1 : $('#selectCapitulo').val();
            $("#selectSubcapituloN1").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN1List', { capituloID: capituloID }, false);
            fnSubCapituloN1Change();
        }

        function fnSubCapituloN1Change() {
            const subcapituloN1_id = $('#selectSubcapituloN1').val() == "" ? -1 : $('#selectSubcapituloN1').val();
            $("#selectSubcapituloN2").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN2List', { subcapituloN1_id: subcapituloN1_id }, false);
            $("#selectActividadPadre").fillCombo('/ControlObra/ControlObra/GetActividadesList', { subcapitulosN1_id: subcapituloN1_id, subcapitulosN2_id: -1, subcapitulosN3_id: -1 }, false);

            fnSubCapituloN2Change();
        }

        function fnSubCapituloN2Change() {
            const subcapituloN2_id = $('#selectSubcapituloN2').val() == "" ? -1 : $('#selectSubcapituloN2').val();
            $("#selectSubcapituloN3").fillCombo('/ControlObra/ControlObra/GetSubcapitulosN3List', { subcapituloN2_id: subcapituloN2_id }, false);
            $("#selectActividadPadre").fillCombo('/ControlObra/ControlObra/GetActividadesList', { subcapitulosN1_id: -1, subcapitulosN2_id: subcapituloN2_id, subcapitulosN3_id: -1 }, false);

            fnSubCapituloN3Change()
        }

        function fnSubCapituloN3Change() {
            const subcapitulosN3_id = $('#selectSubcapituloN3').val() == "" ? -1 : $('#selectSubcapituloN3').val();
            $("#selectActividadPadre").fillCombo('/ControlObra/ControlObra/GetActividadesList', { subcapitulosN1_id: -1, subcapitulosN2_id: -1, subcapitulosN3_id: subcapitulosN3_id }, false);
        }

        function fnBuscarActividades() {
            const capituloID = $('#SelectCapitulosFiltro').val() == "" ? -1 : $('#SelectCapitulosFiltro').val();

            $.ajax({
                url: '/ControlObra/ControlObra/GetSubcapitulosN1Catalogo',
                datatype: "json",
                type: "GET",
                data: {
                    listCapitulosID: capituloID
                },
                success: function (response) {

                    if (response.EMPTY) {
                        clearDt($("#tblActividades"));
                    } else {
                        AddRows($("#tblActividades"), response.items)
                    }
                }
            });
        }

        function fnAbrirModalActividadNueva() {
            isEdit = false;
            $("#selectCapitulo").val("");
            $("#selectActividadPadre").val("");
            $("#selectUnidad").val("");
            $("#txtActividad").val("");
            $("#txtCantidad").val("");
            $("#txtCostoUnidad").val("");
            $("#txtActividad").focus();
            $("#lblTitulo").text('Nueva Actividad');
            dpFechaInicio.datepicker("setDate", hoy);
            dpFechaFin.datepicker("setDate", hoy);

            fnCapituloChange();
            modalActividades.modal('show');
        }

        function fnGuardarSActividad() {
            if (!validarGuardar()) {
                return;
            }

            if (isEdit) {
                fnEditarActividad();
            } else {
                fnAgregarActividad();
            }
        }

        function validarGuardar() {
            let esValida = false

            debugger;

            if ($("#txtActividad").val() != "" && $("#txtCantidad").val() != "" && $('#selectSubcapituloN1').val() != "" && $('#selectUnidad').val() != "" && $('#dpFechaInicio').val() != "" && $('#dpFechaFin').val() != "") {
                esValida = true;
            }
            else {
                esValida = false;
            }

            return esValida;
        }

        function fnAgregarActividad() {
            const actividad = $('#txtActividad').val();
            const cantidad = $("#txtCantidad").val();
            const fechaInicio = moment(dpFechaInicio.val(), "DD-MM-YYYY").format();
            const fechaFin = moment(dpFechaFin.val(), "DD-MM-YYYY").format();
            const subcapituloN1_id = $('#selectSubcapituloN3').val() == "" ? $('#selectSubcapituloN2').val() == "" ? $('#selectSubcapituloN1').val() : null : null;
            const subcapituloN2_id = $('#selectSubcapituloN3').val() == "" ? $('#selectSubcapituloN2').val() == "" ? null : $('#selectSubcapituloN2').val() : null;
            const subcapituloN3_id = $('#selectSubcapituloN3').val() == "" ? null : $('#selectSubcapituloN3').val();
            const actividadPadre_id = $('#selectActividadPadre').val() == "" ? null : $('#selectActividadPadre').val();
            const unidad_id = $('#selectUnidad').val();

            $.ajax({
                url: '/ControlObra/ControlObra/GuardarActividad',
                datatype: "json",
                type: "POST",
                data: {
                    actividad: actividad,
                    cantidad: cantidad,
                    unidad_id: unidad_id,
                    fechaInicio: fechaInicio,
                    fechaFin: fechaFin,
                    subcapitulosN1_id: subcapituloN1_id,
                    subcapitulosN2_id: subcapituloN2_id,
                    subcapitulosN3_id: subcapituloN3_id,
                    actividadPadre_id: actividadPadre_id,
                    estatus: true,
                    actividadPadreRequerida: false,
                    actividadTerminada: false
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    recargarTodo();
                },
                error: function (response) {
                }
            });
        }

        function fnEditarActividad() {
            const url = 'UpdateActividad';

            $.ajax({
                url: url,
                datatype: "json",
                type: "POST",
                data: {

                    actividadID: $("#btnGuardarActividad").val(),
                    actividad: $('#txtActividad').val(),
                    cantidad: $("#txtCantidad").val(),
                    unidad_id: $('#selectUnidad').val(),
                    fechaInicio: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                    fechaFin: moment(dpFechaFin.val(), "DD-MM-YYYY").format(),
                    subcapitulosN1_id: $('#selectSubcapituloN3').val() == "" ? $('#selectSubcapituloN2').val() == "" ? $('#selectSubcapituloN1').val() : null : null,
                    subcapitulosN2_id: $('#selectSubcapituloN3').val() == "" ? $('#selectSubcapituloN2').val() == "" ? null : $('#selectSubcapituloN2').val() : null,
                    subcapitulosN3_id: $('#selectSubcapituloN3').val() == "" ? null : $('#selectSubcapituloN3').val(),
                    actividadPadre_id: $('#selectActividadPadre').val() == "" ? null : $('#selectActividadPadre').val(),
                    actividadPadreRequerida: false
                },
                success: function (data) {
                    AlertaGeneral("Información Guardada", "Se ha guardado la información.");
                    recargarTodo();
                },
                error: function (response) {
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
            modalActividades.modal('hide');
            fnBuscarActividades();
        }

        init();
    }

    $(document).ready(function () {
        controlObra.ActividadesCatalogo = new ActividadesCatalogo();
    });
})();