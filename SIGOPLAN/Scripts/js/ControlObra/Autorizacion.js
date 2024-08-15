(function () {
    $.namespace('controlObra.Autorizacion');

    Autorizacion = function () {
        tblAvances = $("#tblAvances");
        modalDetalle = $('#modalDetalle');

        //AUTORIZACION
        $(document).on("click", "#modalDetalle div.modal-body div.row div.col-md-4 div.flex-container div.panel-primary div.panel-heading button.auth", function () {
            debugger;
            autorizar($(this).val(), $(this).attr('data-avanceid'))
        });

        //tooltip actividad
        $(document).on('mouseenter', ".iffyTip", function () {
            var $this = $(this);
            if (this.offsetWidth < this.scrollWidth && !$this.attr('title')) {
                $this.tooltip({
                    title: $this.text(),
                    placement: "bottom"
                });
                $this.tooltip('show');
            }
        });

        function init() {
            initCbo();
            initTableCapitulos();

            $("#selectCapituloFiltro").change(fnBuscarAvances);

        }

        function initCbo() {
            $("#selectCapituloFiltro").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
        }

        function initTableCapitulos() {
            tblAvances = $("#tblAvances").DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                scrollY: "500px",
                scrollCollapse: true,
                searching: false,
                initComplete: function (settings, json) {
                    tblAvances.on('click', '.btn-autorizar-avance', function () {
                        $.blockUI({ message: "Preparando información" });
                        var rowData = tblAvances.row($(this).closest('tr')).data();

                        $.ajax({
                            url: '/ControlObra/ControlObra/GetAvancesAutorizar',
                            data: { actividadAvanceID: rowData["avance_id"], capituloID: rowData["capitulo_id"] },
                            success: function (response) {
                                if (response.success) modalAvanceDetalle(rowData["avance_id"], rowData["capitulo_id"], rowData["nombreAutorizante"]);
                                else AlertaGeneral('Aviso', 'Ha ocurrido un error al cargar la información');

                                $.unblockUI();
                            }
                        });
                    });
                },
                columns: [
                    { data: 'capitulo', title: 'Capitulo' },
                    { data: 'fechaInicio', title: 'Fecha Inicio' },
                    { data: 'fechaFin', title: 'Fecha Fin' },
                    {
                        sortable: false,
                        "render": function (data, type, row, meta) {
                            var html = '';
                            html += '<button class="btn-autorizar-avance btn btn-sm btn-primary" type="button" value="' + row.avance_id + '" style=""><i class="far fa-user"></i></button>';
                            return html;
                        },
                        title: "Autorizar"
                    }
                ],
                columnDefs: [
                    { width: 100, targets: 1 },
                    { width: 100, targets: 2 },
                    { width: 5, targets: 3 }
                ],
            });
        }

        function fnBuscarAvances() {
            const capituloID = $('#selectCapituloFiltro').val() == "" ? -1 : $('#selectCapituloFiltro').val();

            $.ajax({
                url: '/ControlObra/ControlObra/GetAvancesAutorizar',
                datatype: "json",
                type: "GET",
                data: {
                    capituloID: capituloID
                },
                success: function (response) {

                    if (response.EMPTY) {
                        clearDt($("#tblAvances"));
                    } else {

                        AddRows($("#tblAvances"), response.items);
                    }
                }
            });
        }

        function modalAvanceDetalle(avance_id, capitulo_id, nombreAutorizante) {
            limpiarModal();

            const subcapituloNI = obtenerSubcapitulosNivel1(capitulo_id);

            // TABLA AVANCE
            let table = makeTabla();
            let tableVolumen = makeTablaVolumenAvance();
            let tableVolumenPorcentaje = makeTablaVolumenPorcentaje();
            // let tableVolumenFacturado = makeTablaVolumenFacturado();
            // let tableVolumenXFacturar = makeTablaVolumenXFacturar();

            let tableImporte = makeTablaImporteAvance();
            let tableImportePorcentaje = makeTablaImportePorcentaje();
            // let tableImporteFacturado = makeTablaFacturado();
            // let tableImporteXFacturar = makeTablaXFacturar();

            let body = document.createElement('tbody');
            let bodyVolumen = document.createElement('tbody');
            let bodyVolumenPorcentaje = document.createElement('tbody');
            // let bodyVolumenFacturado = document.createElement('tbody');
            // let bodyVolumenXFacturar = document.createElement('tbody');

            let bodyImporte = document.createElement('tbody');
            let bodyImportePorcentaje = document.createElement('tbody');
            // let bodyImporteFacturado = document.createElement('tbody');
            // let bodyImporteXFacturar = document.createElement('tbody');

            if (subcapituloNI != undefined) {
                subcapituloNI.forEach(function (subcapituloNI) {
                    const actividadesN1 = obtenerActividades(subcapituloNI.id, -1, -1, avance_id);
                    const subCapitulosN2 = obtenerSubcapitulosNivel2(subcapituloNI.id);

                    //SUBCAPITULO I
                    body.append(crearCapitulosHeader(subcapituloNI, 1, 6));
                    bodyVolumen.append(crearCapitulosHeader(subcapituloNI, 1, 3));
                    bodyVolumenPorcentaje.append(crearCapitulosHeader(subcapituloNI, 1, 1));
                    // bodyVolumenFacturado.append(crearCapitulosHeader(subcapituloNI, 1, 1));
                    // bodyVolumenXFacturar.append(crearCapitulosHeader(subcapituloNI, 1, 1));

                    bodyImporte.append(crearCapitulosHeader(subcapituloNI, 1, 3));
                    bodyImportePorcentaje.append(crearCapitulosHeader(subcapituloNI, 1, 1));
                    // bodyImporteFacturado.append(crearCapitulosHeader(subcapituloNI, 1, 1));
                    // bodyImporteXFacturar.append(crearCapitulosHeader(subcapituloNI, 1, 1));

                    //ACTIVIDADES I
                    if (actividadesN1 != undefined) {
                        actividadesN1.forEach(function (actividad) {
                            body.append(crearActividadesRow(actividad));
                            bodyVolumen.append(crearVolumenAvanceRow(actividad));
                            bodyVolumenPorcentaje.append(crearVolumenPorcentajeRow(actividad));
                            // bodyVolumenFacturado.append(crearVolumenFacturadoRow(actividad));
                            // bodyVolumenXFacturar.append(crearVolumenXFacturarRow(actividad));

                            bodyImporte.append(crearImporteAvanceRow(actividad));
                            bodyImportePorcentaje.append(crearImportePorcentajeRow(actividad));
                            // bodyImporteFacturado.append(crearImporteFacturadoRow(actividad));
                            // bodyImporteXFacturar.append(crearXFacturarRow(actividad));
                        });
                    }

                    //SUBCAPITLO II
                    if (subCapitulosN2 != undefined) {
                        subCapitulosN2.forEach(function (subCapitulosN2) {
                            const actividadesN2 = obtenerActividades(-1, subCapitulosN2.id, -1, avance_id);
                            const subCapitulosN3 = obtenerSubcapitulosNivel3(subCapitulosN2.id);

                            body.append(crearCapitulosHeader(subCapitulosN2, 2, 6));
                            bodyVolumen.append(crearCapitulosHeader(subCapitulosN2, 2, 3));
                            bodyVolumenPorcentaje.append(crearCapitulosHeader(subCapitulosN2, 2, 1));
                            // bodyVolumenFacturado.append(crearCapitulosHeader(subCapitulosN2, 2, 1));
                            // bodyVolumenXFacturar.append(crearCapitulosHeader(subCapitulosN2, 2, 1));

                            bodyImporte.append(crearCapitulosHeader(subCapitulosN2, 2, 3));
                            bodyImportePorcentaje.append(crearCapitulosHeader(subCapitulosN2, 2, 1));
                            // bodyImporteFacturado.append(crearCapitulosHeader(subcapituloNI, 2, 1));
                            // bodyImporteXFacturar.append(crearCapitulosHeader(subcapituloNI, 2, 1));

                            //ACITIVDADES II
                            if (actividadesN2 != undefined) {
                                actividadesN2.forEach(function (actividad) {
                                    body.append(crearActividadesRow(actividad));
                                    bodyVolumen.append(crearVolumenAvanceRow(actividad));
                                    bodyVolumenPorcentaje.append(crearVolumenPorcentajeRow(actividad));
                                    // bodyVolumenFacturado.append(crearVolumenFacturadoRow(actividad));
                                    // bodyVolumenXFacturar.append(crearVolumenXFacturarRow(actividad));

                                    bodyImporte.append(crearImporteAvanceRow(actividad));
                                    bodyImportePorcentaje.append(crearImportePorcentajeRow(actividad));
                                    // bodyImporteFacturado.append(crearImporteFacturadoRow(actividad));
                                    // bodyImporteXFacturar.append(crearXFacturarRow(actividad));
                                });
                            }

                            //SUBCAPITLO III
                            if (subCapitulosN3 != undefined) {
                                subCapitulosN3.forEach(function (subCapitulosN3) {
                                    const actividadesN3 = obtenerActividades(-1, -1, subCapitulosN3.id, avance_id);

                                    body.append(crearCapitulosHeader(subCapitulosN3, 3, 6));
                                    bodyVolumen.append(crearCapitulosHeader(subCapitulosN3, 3, 3));
                                    bodyVolumenPorcentaje.append(crearCapitulosHeader(subCapitulosN3, 3, 1));
                                    // bodyVolumenFacturado.append(crearCapitulosHeader(subCapitulosN3, 3, 1));
                                    // bodyVolumenXFacturar.append(crearCapitulosHeader(subCapitulosN3, 3, 1));

                                    bodyImporte.append(crearCapitulosHeader(subCapitulosN3, 3, 3));
                                    bodyImportePorcentaje.append(crearCapitulosHeader(subCapitulosN3, 3, 1));
                                    // bodyImporteFacturado.append(crearCapitulosHeader(subCapitulosN3, 3, 1));
                                    // bodyImporteXFacturar.append(crearCapitulosHeader(subCapitulosN3, 3, 1));

                                    //ACITIVDADES III
                                    if (actividadesN3 != undefined) {
                                        actividadesN3.forEach(function (actividad) {
                                            body.append(crearActividadesRow(actividad));
                                            bodyVolumen.append(crearVolumenAvanceRow(actividad));
                                            bodyVolumenPorcentaje.append(crearVolumenPorcentajeRow(actividad));
                                            // bodyVolumenFacturado.append(crearVolumenFacturadoRow(actividad));
                                            // bodyVolumenXFacturar.append(crearVolumenXFacturarRow(actividad));

                                            bodyImporte.append(crearImporteAvanceRow(actividad));
                                            bodyImportePorcentaje.append(crearImportePorcentajeRow(actividad));
                                            // bodyImporteFacturado.append(crearImporteFacturadoRow(actividad));
                                            // bodyImporteXFacturar.append(crearXFacturarRow(actividad));
                                        });
                                    }
                                });
                            }
                        });
                    }
                });
            }

            $(table).append(body);
            $(tableVolumen).append(bodyVolumen);
            $(tableVolumenPorcentaje).append(bodyVolumenPorcentaje);
            // $(tableVolumenFacturado).append(bodyVolumenFacturado);
            // $(tableVolumenXFacturar).append(bodyVolumenXFacturar);

            $(tableImporte).append(bodyImporte);
            $(tableImportePorcentaje).append(bodyImportePorcentaje);
            // $(tableImporteFacturado).append(bodyImporteFacturado);
            // $(tableImporteXFacturar).append(bodyImporteXFacturar);

            modalDetalle.find('.modal-body').append(setAutorizacion(nombreAutorizante, avance_id));
            modalDetalle.modal('show');
        }

        function makeTabla() {
            let table = $('#tblAvanceDetalle');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let th = document.createElement('th');

            let thActividad = document.createElement('th');
            thActividad.textContent = 'Actividad';

            let thUnidad = document.createElement('th');
            thUnidad.textContent = 'Unidad';

            let thCantidad = document.createElement('th');
            thCantidad.classList.add('text-right');
            thCantidad.textContent = 'Cantidad';

            let thPrecioUnitario = document.createElement('th');
            thPrecioUnitario.classList.add('text-right');
            thPrecioUnitario.textContent = 'P.U';

            let thImporteContratado = document.createElement('th');
            thImporteContratado.classList.add('text-right');
            thImporteContratado.textContent = 'Importe';

            $(tr).append(th);
            $(tr).append(thActividad);
            $(tr).append(thUnidad);
            $(tr).append(thCantidad);
            $(tr).append(thPrecioUnitario);
            $(tr).append(thImporteContratado);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaVolumenAvance() {
            let table = $('#tblVolumenAvance');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thAcumAnt = document.createElement('th');
            thAcumAnt.classList.add('text-center');
            thAcumAnt.textContent = 'Acum.Anterior';

            let thAcumSemana = document.createElement('th');
            thAcumSemana.classList.add('text-center');
            thAcumSemana.textContent = 'Esta Semana';

            let thAcumAct = document.createElement('th');
            thAcumAct.classList.add('text-center');
            thAcumAct.textContent = 'Acum.Total';

            $(tr).append(thAcumAnt);
            $(tr).append(thAcumSemana);
            $(tr).append(thAcumAct);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaVolumenPorcentaje() {
            let table = $('#tblVolumenPorcentaje');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thAvance = document.createElement('th');
            thAvance.classList.add('text-center');
            thAvance.textContent = '%Actividad';

            $(tr).append(thAvance);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaVolumenFacturado() {
            let table = $('#tblVolumenFacturado');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thVolumen = document.createElement('th');
            thVolumen.classList.add('text-center');
            thVolumen.textContent = 'Volumen';

            $(tr).append(thVolumen);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaVolumenXFacturar() {
            let table = $('#tblVolumenXFacturar');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thVolumen = document.createElement('th');
            thVolumen.classList.add('text-center');
            thVolumen.textContent = 'Volumen';

            $(tr).append(thVolumen);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaImporteAvance() {
            let table = $('#tblImporteAvance');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thAcumAnt = document.createElement('th');
            thAcumAnt.classList.add('text-center');
            thAcumAnt.textContent = 'Acum.Anterior';

            let thAcumSemana = document.createElement('th');
            thAcumSemana.classList.add('text-center');
            thAcumSemana.textContent = 'Esta Semana';

            let thAcumAct = document.createElement('th');
            thAcumAct.classList.add('text-center');
            thAcumAct.textContent = 'Acum.Total';

            $(tr).append(thAcumAnt);
            $(tr).append(thAcumSemana);
            $(tr).append(thAcumAct);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaImportePorcentaje() {
            let table = $('#tblImportePorcentaje');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thAvance = document.createElement('th');
            thAvance.classList.add('text-center');
            thAvance.textContent = '%Actividad';

            $(tr).append(thAvance);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaFacturado() {
            let table = $('#tblImporteFacturado');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thImporte = document.createElement('th');
            thImporte.classList.add('text-center');
            thImporte.textContent = 'Importe';

            $(tr).append(thImporte);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaXFacturar() {
            let table = $('#tblImporteXFacturar');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thImporte = document.createElement('th');
            thImporte.classList.add('text-center');
            thImporte.textContent = 'Importe';

            $(tr).append(thImporte);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function obtenerSubcapitulosNivel1(capitulo_id) {
            let nivel1 = [];
            $.ajax({
                type: "GET",
                async: false,
                data: {
                    listCapitulosID: capitulo_id
                },
                url: "GetSubcapitulosN1Catalogo",
                success: function (data) {
                    nivel1 = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return nivel1;
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

        function obtenerActividades(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, actividadAvance_id) {
            let actividades = [];
            $.ajax({
                type: "GET",
                async: false,
                data: {
                    subcapitulosN1_id: subcapitulosN1_id,
                    subcapitulosN2_id: subcapitulosN2_id,
                    subcapitulosN3_id: subcapitulosN3_id,
                    actividadAvance_id: actividadAvance_id
                },
                url: "GetActividadAvanceDetalleAutorizar",
                success: function (data) {
                    actividades = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return actividades;
        }

        function crearCapitulosHeader(subcapitulo, subcapituloTipo, colSpan) {
            let tr = document.createElement('tr');
            tr.setAttribute('id', subcapitulo.id);
            tr.setAttribute('style', 'background-color:' + colorROW(subcapituloTipo));

            let tdSubcapitulo = document.createElement('td');
            tdSubcapitulo.setAttribute('style', 'border-right: none');
            tdSubcapitulo.setAttribute('style', 'border-left: none');
            tdSubcapitulo.setAttribute('colspan', colSpan);

            if (colSpan < 6) {
                tdSubcapitulo.setAttribute('style', 'height: 21px');
            } else {
                tdSubcapitulo.textContent = subcapitulo.subcapitulo;
            }


            $(tr).append(tdSubcapitulo);

            return tr;
        }

        function crearActividadesRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let td = document.createElement('td');
            td.setAttribute('style', 'border-right: none');
            td.setAttribute('width', '2%');

            let tdActividad = document.createElement('td');
            let divActividad = document.createElement('div');
            tdActividad.setAttribute('width', '5%');
            tdActividad.setAttribute('style', 'border-left: none');
            divActividad.append(actividad.actividad);
            divActividad.classList.add('iffyTip');
            divActividad.classList.add('wd100');
            $(tdActividad).append(divActividad);

            let tdUnidad = document.createElement('td');
            tdUnidad.setAttribute('width', '5.5%');
            tdUnidad.textContent = actividad.unidad;

            let tdCantidad = document.createElement('td');
            tdCantidad.setAttribute('width', '5%');
            tdCantidad.setAttribute('align', 'right');
            tdCantidad.textContent = formatValue(actividad.cantidad);

            let tdPrecioUnitario = document.createElement('td');
            tdPrecioUnitario.setAttribute('width', '7%');
            tdPrecioUnitario.setAttribute('align', 'right');
            tdPrecioUnitario.textContent = '$ ' + formatValue(actividad.actividadPU);

            let tdImporteContratado = document.createElement('td');
            tdImporteContratado.setAttribute('width', '7%');
            tdImporteContratado.setAttribute('align', 'right');
            tdImporteContratado.textContent = '$ ' + formatValue(actividad.importeContratado);

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdPrecioUnitario);
            $(tr).append(tdImporteContratado);

            return tr;
        }

        function crearVolumenAvanceRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let tdAcumAnterior = document.createElement('td');
            tdAcumAnterior.setAttribute('align', 'right');
            tdAcumAnterior.textContent = formatValue(actividad.acumAnterior);

            let tdAcumSemana = document.createElement('td');
            tdAcumSemana.setAttribute('style', 'background-color: #ccffcc;');
            tdAcumSemana.setAttribute('align', 'right');
            tdAcumSemana.textContent = formatValue(actividad.avancePeriodo);

            let tdAcumActual = document.createElement('td');
            tdAcumActual.setAttribute('align', 'right');
            tdAcumActual.textContent = formatValue(actividad.acumActual);


            $(tr).append(tdAcumAnterior);
            $(tr).append(tdAcumSemana);
            $(tr).append(tdAcumActual);

            return tr;
        }

        function crearVolumenPorcentajeRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let tdImporteAvanceAcumulado = document.createElement('td');
            tdImporteAvanceAcumulado.setAttribute('width', '2%');
            tdImporteAvanceAcumulado.setAttribute('align', 'right');
            tdImporteAvanceAcumulado.textContent = '% ' + formatValue(actividad.avanceAcumuladoPorcentaje);

            $(tr).append(tdImporteAvanceAcumulado);

            return tr;
        }

        function crearVolumenFacturadoRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let tdVolumen = document.createElement('td');
            tdVolumen.setAttribute('width', '2%');
            tdVolumen.setAttribute('align', 'right');
            tdVolumen.textContent = formatValue(actividad.volumenFacturado);

            $(tr).append(tdVolumen);

            return tr;
        }

        function crearVolumenXFacturarRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let tdVolumen = document.createElement('td');
            tdVolumen.setAttribute('width', '2%');
            tdVolumen.setAttribute('align', 'right');
            tdVolumen.textContent = formatValue(actividad.volumenxFacturar);

            $(tr).append(tdVolumen);

            return tr;
        }

        function crearImporteAvanceRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let tdImporteAvanceAnterior = document.createElement('td');
            tdImporteAvanceAnterior.setAttribute('width', '2%');
            tdImporteAvanceAnterior.setAttribute('align', 'right');
            tdImporteAvanceAnterior.textContent = '$ ' + formatValue(actividad.importeAvanceAnt);

            let tdAvanceSemana = document.createElement('td');
            tdAvanceSemana.setAttribute('style', 'background-color: #ccffcc;');
            tdAvanceSemana.setAttribute('width', '2%');
            tdAvanceSemana.setAttribute('align', 'right');
            tdAvanceSemana.textContent = '$' + formatValue(actividad.importeAvancePeriodo);

            let tdImporteAvanceAcumulado = document.createElement('td');
            tdImporteAvanceAcumulado.setAttribute('width', '2%');
            tdImporteAvanceAcumulado.setAttribute('align', 'right');
            tdImporteAvanceAcumulado.textContent = '$ ' + formatValue(actividad.importeAvanceAcumulado);

            $(tr).append(tdImporteAvanceAnterior);
            $(tr).append(tdAvanceSemana);
            $(tr).append(tdImporteAvanceAcumulado);

            return tr;
        }

        function crearImportePorcentajeRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let tdImportePorcentaje = document.createElement('td');
            tdImportePorcentaje.setAttribute('width', '2%');
            tdImportePorcentaje.setAttribute('align', 'right');
            tdImportePorcentaje.textContent = '% ' + formatValue(actividad.avanceAcumuladoPorcentaje);

            $(tr).append(tdImportePorcentaje);

            return tr;
        }

        function crearImporteFacturadoRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let tdImporte = document.createElement('td');
            tdImporte.setAttribute('width', '2%');
            tdImporte.setAttribute('align', 'right');
            tdImporte.textContent = '$' + formatValue(actividad.importeFacturado);

            $(tr).append(tdImporte);

            return tr;
        }

        function crearXFacturarRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let tdImporte = document.createElement('td');
            tdImporte.setAttribute('width', '2%');
            tdImporte.setAttribute('align', 'right');
            tdImporte.textContent = '$' + formatValue(actividad.importexFacturar);

            $(tr).append(tdImporte);

            return tr;
        }

        function colorROW(subcapituloTipo) {
            let color = '';
            switch (subcapituloTipo) {
                case 1:
                    color = '#f8cbad'
                    break;
                case 2:
                    color = '#ffff99'
                    break;
                case 3:
                    color = '#9bc2e6'
                    break;
                default:
                    color = 'white'
                    break;
            }

            return color;
        }

        function setAutorizacion(nombre, avance_id) {
            //divAuth.html();

            let div = `
                    <div class="row divAutorizacion" style="padding-top: 20px;">
                        <div class="col-md-4"></div>
                        <div class="col-md-4">
                            <div class="flex-container text-center" >
                                <div class="panel panel-primary flex-item">
                                    <div class="panel-heading"> Autorizante en espera 
                                        <button class='btn btn-danger btn-xs pull-left auth' value='3' data-avanceid='${avance_id}'><i class='fa fa-times'></i> </button>
                                        <button class='btn btn-success btn-xs pull-right auth' value='2' data-avanceid='${avance_id}'><i class='fa fa-check'></i></button>
                                    </div>
                                    <div class="panel-body">
                                        <label>${nombre}</label>
                                    </div>
                                </div>        
                            </div>
                        </div>
                        <div class="col-md-4"></div>
                    </div>
                    `;
            //divAuth.html(div);
            return div;
        }

        function autorizar(autorizar, avance_id) {
            let res = $.post("/ControlObra/ControlObra/GuardarAutorizacion", { autorizacion: autorizar == 2 ? true : false, avance_id: avance_id });
            res.done(function (response) {
                if (response.success) {
                    AlertaGeneral("Aviso", "Se Guardo correctamente la información");
                    limpiar();
                }
                else {
                    AlertaGeneral("Aviso", response.error);
                    limpiar()
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
            modalDetalle.modal('hide');
            fnBuscarAvances();
        }

        function limpiarModal() {
            modalDetalle.find('.divAutorizacion').remove();
            $('#tblAvanceDetalle thead tr').remove();
            $('#tblAvanceDetalle thead').remove();
            $('#tblAvanceDetalle tbody tr').remove();
            $('#tblAvanceDetalle tbody').remove();

            $('#tblVolumenAvance thead tr').remove();
            $('#tblVolumenAvance thead').remove();
            $('#tblVolumenAvance tbody tr').remove();
            $('#tblVolumenAvance tbody').remove();

            $('#tblImporteAvance thead tr').remove();
            $('#tblImporteAvance thead').remove();
            $('#tblImporteAvance tbody tr').remove();
            $('#tblImporteAvance tbody').remove();

            $('#tblVolumenPorcentaje thead tr').remove();
            $('#tblVolumenPorcentaje thead').remove();
            $('#tblVolumenPorcentaje tbody tr').remove();
            $('#tblVolumenPorcentaje tbody').remove();

            $('#tblImportePorcentaje thead tr').remove();
            $('#tblImportePorcentaje thead').remove();
            $('#tblImportePorcentaje tbody tr').remove();
            $('#tblImportePorcentaje tbody').remove();

            $('#tblVolumenFacturado thead tr').remove();
            $('#tblVolumenFacturado thead').remove();
            $('#tblVolumenFacturado tbody tr').remove();
            $('#tblVolumenFacturado tbody').remove();

            $('#tblImporteFacturado thead tr').remove();
            $('#tblImporteFacturado thead').remove();
            $('#tblImporteFacturado tbody tr').remove();
            $('#tblImporteFacturado tbody').remove();

            $('#tblVolumenXFacturar thead tr').remove();
            $('#tblVolumenXFacturar thead').remove();
            $('#tblVolumenXFacturar tbody tr').remove();
            $('#tblVolumenXFacturar tbody').remove();

            $('#tblImporteXFacturar thead tr').remove();
            $('#tblImporteXFacturar thead').remove();
            $('#tblImporteXFacturar tbody tr').remove();
            $('#tblImporteXFacturar tbody').remove();
        }

        init();
    };

    $(document).ready(function () {
        controlObra.Autorizacion = new Autorizacion();
    });
})();