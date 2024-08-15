(function () {
    $.namespace('controlObra.ReporteDiario');

    ReporteDiario = function () {
        tblActividades = $("#tblActividades");
        btnBuscarActividades = $("#btnBuscarActividades");

        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');
        const hoy = new Date();

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
            $.each(detailRows, function (i, id) {
                $('#' + id + ' td.details-control').trigger('click');
            });
        });

        function format(data) {
            const fechaInicio = moment(dpFechaInicio.val(), "DD-MM-YYYY").format();
            const fechaFin = fechaInicio;
            const actividadesN1 = obtenerActividades(data.id, -1, -1, fechaInicio, fechaFin);
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

                        const actividadesN2 = obtenerActividades(-1, subcapituloN2.id, -1, fechaInicio, fechaFin);
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
                        tdSubcapitulo.setAttribute('colspan', '8');
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

                                const actividadesN3 = obtenerActividades(-1, -1, subcapituloN3.id, fechaInicio, fechaFin);

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
                                tdSubcapitulo.setAttribute('colspan', '8');
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

        function init() {
            initTableActividades();
            initCbo();

            btnBuscarActividades.click(fnBuscarActividades);

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
                        data: 'capitulo', sortable: false,
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
                        defaultContent: '', orderable: false
                    },
                    {
                        defaultContent: '', orderable: false
                    },
                    {
                        defaultContent: '', orderable: false
                    }
                ],
                columnDefs: [
                    { width: 5, targets: 0 },
                    { visible: false, targets: groupCapitulo },
                    { width: 740, targets: 2 },
                    { width: 40, targets: 3 },
                    { width: 30, targets: 4 },
                    { width: 70, targets: 5 },
                    { width: 70, targets: 6 },
                    { width: 100, targets: 7 },
                    { width: 100, targets: 8 },
                    { width: 120, targets: 9 }
                ],
                order: [[groupCapitulo, 'asc']],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    api.column(groupCapitulo, { page: 'current' }).data().each(function (group, i) {
                        if (last !== group) {
                            $(rows).eq(i).before(
                                '<tr class="group"><td style="background-color: #ffcc00" colspan="9">' + group + '</td></tr>'
                            );

                            last = group;
                        }
                    });
                }
            });
        }

        function initCbo() {
            $("#SelectCapitulosFiltro").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
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

        function tablaActividades(actividad, claseColapse, claseExtra, subcapituloID) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.id);

            if (claseColapse != '') tr.classList.add(claseColapse);
            if (claseExtra != '') tr.classList.add(claseExtra + subcapituloID);

            let td = document.createElement('td');
            td.setAttribute('style', 'border-right: none');
            td.setAttribute('width', '2.2%');

            let tdActividad = document.createElement('td');
            tdActividad.setAttribute('width', '35%');
            tdActividad.setAttribute('style', 'border-left: none');
            tdActividad.textContent = actividad.actividad;

            let tdUnidad = document.createElement('td');
            tdUnidad.setAttribute('width', '4%');
            tdUnidad.textContent = actividad.unidad;

            let tdCantidad = document.createElement('td');
            tdCantidad.setAttribute('width', '4%');
            tdCantidad.setAttribute('align', 'right');
            tdCantidad.textContent = formatValue(actividad.cantidad);

            let tdFechaInicio = document.createElement('td');
            tdFechaInicio.setAttribute('width', '4%');
            tdFechaInicio.setAttribute('align', 'center');
            tdFechaInicio.textContent = actividad.fechaInicio;

            let tdFechaFin = document.createElement('td');
            tdFechaFin.setAttribute('width', '4%');
            tdFechaFin.setAttribute('align', 'center');
            tdFechaFin.textContent = actividad.fechaInicio;

            let tdAcumAnterior = document.createElement('td');
            tdAcumAnterior.setAttribute('width', '7%');
            tdAcumAnterior.setAttribute('align', 'right');
            tdAcumAnterior.textContent = formatValue(actividad.acumAnterior);

            let tdAcumActual = document.createElement('td');
            tdAcumActual.setAttribute('width', '7%');
            tdAcumActual.setAttribute('align', 'right');
            tdAcumActual.textContent = formatValue(actividad.acumActual);

            let tdAvanceActualPorcentaje = document.createElement('td');
            tdAvanceActualPorcentaje.setAttribute('width', '7%');
            tdAvanceActualPorcentaje.setAttribute('align', 'right');
            tdAvanceActualPorcentaje.textContent = formatValue(actividad.avanceAcumuladoPorcentaje) + '%';

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdFechaInicio);
            $(tr).append(tdFechaFin);
            $(tr).append(tdAcumAnterior);
            $(tr).append(tdAcumActual);
            $(tr).append(tdAvanceActualPorcentaje);

            return tr;
        }

        function obtenerActividades(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, fechaInicio, fechaFin) {

            debugger;
            let nivel2 = [];
            $.ajax({
                type: "GET",
                async: false,
                data: {
                    subcapitulosN1_id: subcapitulosN1_id,
                    subcapitulosN2_id: subcapitulosN2_id,
                    subcapitulosN3_id: subcapitulosN3_id,
                    fechaInicio: fechaInicio,
                    fechaFin: fechaFin
                },
                url: "GetActividadAvanceReporte",
                success: function (data) {
                    nivel2 = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return nivel2;
        }

        function fnBuscarActividades() {
            debugger;
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

        function limpiar() {
            fnBuscarActividades();
        }

        init();
    };

    $(document).ready(function () {
        controlObra.ReporteDiario = new ReporteDiario();
    });
})();