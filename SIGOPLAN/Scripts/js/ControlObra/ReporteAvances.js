(function () {
    $.namespace('controlObra.ReporteAvances');

    ReporteAvances = function () {
        tblActividades = $("#tblActividades");
        btnGuardarAvance = $("#btnGuardarAvance");
        btnGuardarAvanceSemanal = $("#btnGuardarAvanceSemanal");
        btnGuardarFacturacion = $("#btnGuardarFacturacion");
        dpFechaInicio = $('#dpFechaInicio');
        dpFechaFin = $('#dpFechaFin');
        dpFechaAvance = $('#dpFechaAvance');
        dpFechaAvanceSemanal = $('#dpFechaAvanceSemanal');
        dpFechaFacturacion = $('#dpFechaFacturacion');
        const txtFechaSaldo = $('#txtFechaSaldo');

        const hoy = new Date();
        let startDate, endDate;

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

        //tooltip concentrado
        $(document).on('mouseenter', ".toolConcentrado", function () {
            var $this = $(this);
            if (this.offsetWidth < this.scrollWidth && !$this.attr('title')) {
                $this.tooltip({
                    title: $this.text(),
                    placement: "bottom"
                });
                $this.tooltip('show');
            }
        });


        //PIINTAR RENGLON
        $(document).on("mouseenter", "tbody tr", function () {
            paintCells($(this).attr('data-actividadid'));
        });
        $(document).on("mouseleave", "tbody tr", function () {
            unpaintCells($(this).attr('data-actividadid'));
        });

        //calcular acumulado actual en avance
        $(document).on('change', 'input.inputVolumen', function () {
            const row = $(this).closest('tr');
            const acumAnt = row.find('.acumAnt').text(); //parseFloat(row.find('.PU').text().substring(2, row.find('.PU').text().lenght).replace(/,/g, ''));
            const volumen = $(this).val();

            row.find('.acumAct').text(formatValue((unmaskNumero(acumAnt) + +volumen).toFixed(4)));
        });

        //calcular acumulado actual en avance Facturado
        $(document).on('change', 'input.inputVolumenFacturado', function () {
            const row = $(this).closest('tr');
            const pu = unmaskNumero(row.find('.PU').text()); //parseFloat(row.find('.PU').text().substring(2, row.find('.PU').text().lenght).replace(/,/g, ''));
            const volumen = $(this).val();
            //totalFact += (+pu * +volumen).//(+pu * +volumen).toFixed(4);

            row.find('.inputImporteFacturado').text(maskNumero((+pu * +volumen)));

            $('#totalFact').text('');
            $('#totalFact').text(maskNumero(sumaTotalFacturado()));
        });

        const esActividadDiaria = function (element) {
            //revisa si es actividad diaria
            return element.tipoPeriodoAvance == 1;
        };

        const esActividadSemanal = function (element) {
            //revisa si es actividad semanal
            return element.tipoPeriodoAvance == 2;
        };

        function init() {
            initCbo();
            $("#SelectCapitulosFiltro").change(fnFechaInicio);
            btnGuardarAvance.click(crearObjectoAvance);
            btnGuardarAvanceSemanal.click(crearObjectoAvanceSemanal);
            btnGuardarFacturacion.click(crearObjectoFacturado);

            dpFechaInicio.datepicker({ dateFormat: 'dd/mm/yy' });
            dpFechaAvance.datepicker({ dateFormat: 'dd/mm/yy' });
            dpFechaAvanceSemanal.datepicker({ dateFormat: 'dd/mm/yy' });
            dpFechaFacturacion.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
            dpFechaFin.datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", hoy);
        }

        function initCbo() {
            $("#SelectCapitulosFiltro").fillCombo('/ControlObra/ControlObra/GetCapitulosList', null, false);
        }

        function CargarTablas() {
            const fechas = getBusq();
            const capitulo_id = $('#SelectCapitulosFiltro').val() == "" ? -1 : $('#SelectCapitulosFiltro').val();
            const fechaInicio = moment(fechas.min, "DD-MM-YYYY").format();
            const fechaFin = moment(fechas.max, "DD-MM-YYYY").format();

            obtenerSubcapitulosNivel1(capitulo_id, fechaInicio, fechaFin);
            obtenerConcentrado(capitulo_id, fechaInicio, fechaFin);
        }

        function fillTableConcentrado(concentrado) {
            limpiarTablaConcentrado();

            //TABLAS
            let tableConcentrado = makeTablaConcentrado();
            let bodyConcentrado = document.createElement('tbody');

            if (concentrado != undefined) {
                concentrado.forEach(function (subcapitulo) {
                    if (subcapitulo.subcapituloN3 == null)
                        bodyConcentrado.append(crearConcentradoRow(subcapitulo));
                });
            }

            $(tableConcentrado).append(bodyConcentrado);
        }

        function fnFillTables(subCapitulosN1, fechaInicio, fechaFin) {
            limpiarTabla();
            const subcapituloNI = subCapitulosN1;

            //TABLAS
            let table = makeTabla();
            let tableVolumen = makeTablaVolumenAvance();
            let tableVolumenPorcentaje = makeTablaVolumenPorcentaje();
            let tableVolumenFacturado = makeTablaVolumenFacturado();
            let tableVolumenXFacturar = makeTablaVolumenXFacturar();

            let tableImporte = makeTablaImporteAvance();
            let tableImportePorcentaje = makeTablaImportePorcentaje();
            let tableImporteFacturado = makeTablaFacturado();
            let tableImporteXFacturar = makeTablaXFacturar();

            let tableAvance = makeTablaAvancePresupuestoDiario();
            let tableAvanceSemanal = makeTablaAvancePresupuestoSemanal();
            let tableFacturado = makeTablaAvanceFacturado();

            let body = document.createElement('tbody');
            let bodyVolumen = document.createElement('tbody');
            let bodyVolumenPorcentaje = document.createElement('tbody');
            let bodyVolumenFacturado = document.createElement('tbody');
            let bodyVolumenXFacturar = document.createElement('tbody');

            let bodyImporte = document.createElement('tbody');
            let bodyImportePorcentaje = document.createElement('tbody');
            let bodyImporteFacturado = document.createElement('tbody');
            let bodyImporteXFacturar = document.createElement('tbody');

            let bodyAvance = document.createElement('tbody');
            let bodyAvanceSemanal = document.createElement('tbody');
            let bodyFacturado = document.createElement('tbody');

            if (subcapituloNI != undefined) {
                subcapituloNI.forEach(function (subcapituloNI) {

                    const actividadesN1 = obtenerActividades(subcapituloNI.id, -1, -1, fechaInicio, fechaFin);
                    const subCapitulosN2 = obtenerSubcapitulosNivel2(subcapituloNI.id);

                    //SUBCAPITULO I
                    body.append(crearCapitulosHeader(subcapituloNI, 1, 6));
                    bodyVolumen.append(crearCapitulosHeader(subcapituloNI, 1, 3));
                    bodyVolumenPorcentaje.append(crearCapitulosHeader(subcapituloNI, 1, 1));
                    bodyVolumenFacturado.append(crearCapitulosHeader(subcapituloNI, 1, 1));
                    bodyVolumenXFacturar.append(crearCapitulosHeader(subcapituloNI, 1, 1));

                    if (actividadesN1 != undefined) {
                        if (actividadesN1.some(esActividadDiaria))
                            bodyAvance.append(crearCapitulosHeader(subcapituloNI, 1, 7));

                        if (actividadesN1.some(esActividadSemanal))
                            bodyAvanceSemanal.append(crearCapitulosHeader(subcapituloNI, 1, 7));
                    } else {
                        bodyAvance.append(crearCapitulosHeader(subcapituloNI, 1, 7));
                        bodyAvanceSemanal.append(crearCapitulosHeader(subcapituloNI, 1, 7));
                    }

                    bodyFacturado.append(crearCapitulosHeader(subcapituloNI, 1, 8));

                    bodyImporte.append(crearCapitulosHeader(subcapituloNI, 1, 3));
                    bodyImportePorcentaje.append(crearCapitulosHeader(subcapituloNI, 1, 1));
                    bodyImporteFacturado.append(crearCapitulosHeader(subcapituloNI, 1, 1));
                    bodyImporteXFacturar.append(crearCapitulosHeader(subcapituloNI, 1, 1));

                    //ACTIVIDADES I
                    if (actividadesN1 != undefined) {
                        actividadesN1.forEach(function (actividad) {

                            body.append(crearActividadesRow(actividad));
                            bodyVolumen.append(crearVolumenAvanceRow(actividad));
                            bodyVolumenPorcentaje.append(crearVolumenPorcentajeRow(actividad));
                            bodyVolumenFacturado.append(crearVolumenFacturadoRow(actividad));
                            bodyVolumenXFacturar.append(crearVolumenXFacturarRow(actividad));

                            if (actividad.tipoPeriodoAvance == 1)
                                bodyAvance.append(crearAvanceRow(actividad));
                            else
                                bodyAvanceSemanal.append(crearAvanceSemanalRow(actividad));

                            bodyFacturado.append(crearFacturadoRow(actividad));

                            bodyImporte.append(crearImporteAvanceRow(actividad));
                            bodyImportePorcentaje.append(crearImportePorcentajeRow(actividad));
                            bodyImporteFacturado.append(crearImporteFacturadoRow(actividad));
                            bodyImporteXFacturar.append(crearXFacturarRow(actividad));
                        });
                    }

                    //SUBCAPITLO II
                    if (subCapitulosN2 != undefined) {
                        subCapitulosN2.forEach(function (subCapitulosN2) {
                            const actividadesN2 = obtenerActividades(-1, subCapitulosN2.id, -1, fechaInicio, fechaFin);
                            const subCapitulosN3 = obtenerSubcapitulosNivel3(subCapitulosN2.id);

                            body.append(crearCapitulosHeader(subCapitulosN2, 2, 6));
                            bodyVolumen.append(crearCapitulosHeader(subCapitulosN2, 2, 3));
                            bodyVolumenPorcentaje.append(crearCapitulosHeader(subCapitulosN2, 2, 1));
                            bodyVolumenFacturado.append(crearCapitulosHeader(subCapitulosN2, 2, 1));
                            bodyVolumenXFacturar.append(crearCapitulosHeader(subCapitulosN2, 2, 1));

                            if (actividadesN2 != undefined) {
                                if (actividadesN2.some(esActividadDiaria))
                                    bodyAvance.append(crearCapitulosHeader(subCapitulosN2, 2, 7));
                                if (actividadesN2.some(esActividadSemanal))
                                    bodyAvanceSemanal.append(crearCapitulosHeader(subCapitulosN2, 2, 7));
                            }
                            bodyFacturado.append(crearCapitulosHeader(subCapitulosN2, 2, 8));

                            bodyImporte.append(crearCapitulosHeader(subCapitulosN2, 2, 3));
                            bodyImportePorcentaje.append(crearCapitulosHeader(subCapitulosN2, 2, 1));
                            bodyImporteFacturado.append(crearCapitulosHeader(subcapituloNI, 2, 1));
                            bodyImporteXFacturar.append(crearCapitulosHeader(subcapituloNI, 2, 1));

                            //ACITIVDADES II
                            if (actividadesN2 != undefined) {
                                actividadesN2.forEach(function (actividad) {
                                    body.append(crearActividadesRow(actividad));
                                    bodyVolumen.append(crearVolumenAvanceRow(actividad));
                                    bodyVolumenPorcentaje.append(crearVolumenPorcentajeRow(actividad));
                                    bodyVolumenFacturado.append(crearVolumenFacturadoRow(actividad));
                                    bodyVolumenXFacturar.append(crearVolumenXFacturarRow(actividad));

                                    if (actividad.tipoPeriodoAvance == 1)
                                        bodyAvance.append(crearAvanceRow(actividad));
                                    else
                                        bodyAvanceSemanal.append(crearAvanceSemanalRow(actividad));

                                    bodyFacturado.append(crearFacturadoRow(actividad));

                                    bodyImporte.append(crearImporteAvanceRow(actividad));
                                    bodyImportePorcentaje.append(crearImportePorcentajeRow(actividad));
                                    bodyImporteFacturado.append(crearImporteFacturadoRow(actividad));
                                    bodyImporteXFacturar.append(crearXFacturarRow(actividad));
                                });
                            }

                            //SUBCAPITLO III
                            if (subCapitulosN3 != undefined) {
                                subCapitulosN3.forEach(function (subCapitulosN3) {
                                    const actividadesN3 = obtenerActividades(-1, -1, subCapitulosN3.id, fechaInicio, fechaFin);

                                    body.append(crearCapitulosHeader(subCapitulosN3, 3, 6));
                                    bodyVolumen.append(crearCapitulosHeader(subCapitulosN3, 3, 3));
                                    bodyVolumenPorcentaje.append(crearCapitulosHeader(subCapitulosN3, 3, 1));
                                    bodyVolumenFacturado.append(crearCapitulosHeader(subCapitulosN3, 3, 1));
                                    bodyVolumenXFacturar.append(crearCapitulosHeader(subCapitulosN3, 3, 1));

                                    if (actividadesN3 != undefined) {
                                        if (actividadesN3.some(esActividadDiaria))
                                            bodyAvance.append(crearCapitulosHeader(subCapitulosN3, 3, 7));
                                        if (actividadesN3.some(esActividadSemanal))
                                            bodyAvanceSemanal.append(crearCapitulosHeader(subCapitulosN3, 3, 7));
                                    }

                                    bodyFacturado.append(crearCapitulosHeader(subCapitulosN3, 3, 8));

                                    bodyImporte.append(crearCapitulosHeader(subCapitulosN3, 3, 3));
                                    bodyImportePorcentaje.append(crearCapitulosHeader(subCapitulosN3, 3, 1));
                                    bodyImporteFacturado.append(crearCapitulosHeader(subCapitulosN3, 3, 1));
                                    bodyImporteXFacturar.append(crearCapitulosHeader(subCapitulosN3, 3, 1));

                                    //ACITIVDADES III
                                    if (actividadesN3 != undefined) {
                                        actividadesN3.forEach(function (actividad) {
                                            body.append(crearActividadesRow(actividad));
                                            bodyVolumen.append(crearVolumenAvanceRow(actividad));
                                            bodyVolumenPorcentaje.append(crearVolumenPorcentajeRow(actividad));
                                            bodyVolumenFacturado.append(crearVolumenFacturadoRow(actividad));
                                            bodyVolumenXFacturar.append(crearVolumenXFacturarRow(actividad));

                                            if (actividad.tipoPeriodoAvance == 1)
                                                bodyAvance.append(crearAvanceRow(actividad));
                                            else
                                                bodyAvanceSemanal.append(crearAvanceSemanalRow(actividad));

                                            bodyFacturado.append(crearFacturadoRow(actividad));

                                            bodyImporte.append(crearImporteAvanceRow(actividad));
                                            bodyImportePorcentaje.append(crearImportePorcentajeRow(actividad));
                                            bodyImporteFacturado.append(crearImporteFacturadoRow(actividad));
                                            bodyImporteXFacturar.append(crearXFacturarRow(actividad));
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
            $(tableVolumenFacturado).append(bodyVolumenFacturado);
            $(tableVolumenXFacturar).append(bodyVolumenXFacturar);

            $(tableAvance).append(bodyAvance);
            $(tableAvanceSemanal).append(bodyAvanceSemanal);
            $(tableFacturado).append(bodyFacturado);

            $(tableImporte).append(bodyImporte);
            $(tableImportePorcentaje).append(bodyImportePorcentaje);
            $(tableImporteFacturado).append(bodyImporteFacturado);
            $(tableImporteXFacturar).append(bodyImporteXFacturar);
        }

        function obtenerSubcapitulosNivel1(capitulo_id, fechaInicio, fechaFin) {

            $.blockUI({ message: "Preparando información" });

            $.ajax({
                type: "Post",
                //async: false,
                data: {
                    listCapitulosID: capitulo_id
                },
                url: "GetSubcapitulosN1Catalogo",
                success: function (data) {
                    if (data.success) fnFillTables(data.items, fechaInicio, fechaFin);
                    else AlertaGeneral('Aviso', 'Ha ocurrido un error al cargar la información');
                    //nivel1 = data.items;
                    $.unblockUI();
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                    limpiarTabla();
                    $.unblockUI();
                }
            });
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

        function obtenerActividades(subcapitulosN1_id, subcapitulosN2_id, subcapitulosN3_id, fechaInicio, fechaFin) {
            let actividades = [];
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
                    actividades = data.items;
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                }
            });

            return actividades;
        }

        function obtenerConcentrado(capitulo_id, fechaInicio, fechaFin) {
            $.ajax({
                type: "POST",
                //async: false,
                data: {
                    capitulo_id: capitulo_id,
                    fechaInicio: fechaInicio,
                    fechaFin: fechaFin
                },
                url: "GetConcentradoReporte",
                success: function (data) {
                    if (data.success) fillTableConcentrado(data.items);
                    else limpiarTablaConcentrado();
                },
                error: function (objXMLHttpRequest) {
                    // console.log("error", objXMLHttpRequest);
                    limpiarTablaConcentrado();
                }
            });

        }

        function makeTablaConcentrado() {
            let table = $('#tblConcentrado');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thEmpty = document.createElement('th');
            thEmpty.textContent = '';

            let thPresupuesto = document.createElement('th');
            thPresupuesto.textContent = 'Presupuesto';

            let thEjecutado = document.createElement('th');
            thEjecutado.classList.add('text-center');
            thEjecutado.textContent = 'Ejecutado';

            let thXejecutar = document.createElement('th');
            thXejecutar.classList.add('text-center');
            thXejecutar.textContent = 'X Ejecturar';

            let thPorcentajeAvance = document.createElement('th');
            thPorcentajeAvance.classList.add('text-center');
            thPorcentajeAvance.textContent = '% Avance';

            let thImporteSemana = document.createElement('th');
            thImporteSemana.classList.add('text-center');
            thImporteSemana.textContent = 'Importe Semana';

            let thFacturado = document.createElement('th');
            thFacturado.classList.add('text-center');
            thFacturado.textContent = 'Facturado';

            let thxFacturar = document.createElement('th');
            thxFacturar.classList.add('text-center');
            thxFacturar.textContent = 'X Facturar';

            $(tr).append(thEmpty);
            $(tr).append(thPresupuesto);
            $(tr).append(thEjecutado);
            $(tr).append(thXejecutar);
            $(tr).append(thPorcentajeAvance);
            $(tr).append(thImporteSemana);
            $(tr).append(thFacturado);
            $(tr).append(thxFacturar);

            $(head).append(tr);
            $(table).append(head);

            return table;
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

            let thAvanceAnterior = document.createElement('th');
            thAvanceAnterior.classList.add('text-center');
            thAvanceAnterior.textContent = 'Acum.Anterior';

            let thAvanceSemana = document.createElement('th');
            thAvanceSemana.classList.add('text-center');
            thAvanceSemana.textContent = 'Esta Semana';

            let thAvanceAcumulado = document.createElement('th');
            thAvanceAcumulado.classList.add('text-center');
            thAvanceAcumulado.textContent = 'Acum.Total';

            $(tr).append(thAvanceAnterior);
            $(tr).append(thAvanceSemana);
            $(tr).append(thAvanceAcumulado);

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

        function makeTablaAvancePresupuestoDiario() {
            let table = $('#tblPresupuestoDiario');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let th = document.createElement('th');

            let thActividad = document.createElement('th');
            thActividad.textContent = 'Actividad';

            let thUnidad = document.createElement('th');
            thUnidad.textContent = 'Unidad';

            let thCantidad = document.createElement('th');
            thCantidad.classList.add('text-center');
            thCantidad.textContent = 'Cantidad';

            let thAcumAnt = document.createElement('th');
            thAcumAnt.classList.add('text-center');
            thAcumAnt.textContent = 'Acum.Anterior';

            let thAvancePeriodo = document.createElement('th');
            thAvancePeriodo.classList.add('text-center');
            thAvancePeriodo.textContent = 'Avance Periodo';

            let thAcumAct = document.createElement('th');
            thAcumAct.classList.add('text-center');
            thAcumAct.textContent = 'Acum.Actual';

            $(tr).append(th);
            $(tr).append(thActividad);
            $(tr).append(thUnidad);
            $(tr).append(thCantidad);
            $(tr).append(thAcumAnt);
            $(tr).append(thAvancePeriodo);
            $(tr).append(thAcumAct);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaAvancePresupuestoSemanal() {
            let table = $('#tblPresupuestoSemanal');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let th = document.createElement('th');

            let thActividad = document.createElement('th');
            thActividad.textContent = 'Actividad';

            let thUnidad = document.createElement('th');
            thUnidad.textContent = 'Unidad';

            let thCantidad = document.createElement('th');
            thCantidad.classList.add('text-center');
            thCantidad.textContent = 'Cantidad';

            let thAcumAnt = document.createElement('th');
            thAcumAnt.classList.add('text-center');
            thAcumAnt.textContent = 'Acum.Anterior';

            let thAvancePeriodo = document.createElement('th');
            thAvancePeriodo.classList.add('text-center');
            thAvancePeriodo.textContent = 'Avance Periodo';

            let thAcumAct = document.createElement('th');
            thAcumAct.classList.add('text-center');
            thAcumAct.textContent = 'Acum.Actual';

            $(tr).append(th);
            $(tr).append(thActividad);
            $(tr).append(thUnidad);
            $(tr).append(thCantidad);
            $(tr).append(thAcumAnt);
            $(tr).append(thAvancePeriodo);
            $(tr).append(thAcumAct);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function makeTablaAvanceFacturado() {
            let table = $('#tblFacturado');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let th = document.createElement('th');

            let thActividad = document.createElement('th');
            thActividad.textContent = 'Actividad';

            let thUnidad = document.createElement('th');
            thUnidad.textContent = 'Unidad';

            let thCantidad = document.createElement('th');
            thCantidad.classList.add('text-center');
            thCantidad.textContent = 'Cantidad';

            let thPrecioUnitario = document.createElement('th');
            thPrecioUnitario.classList.add('text-center');
            thPrecioUnitario.textContent = 'P.U';

            let thImporteContratado = document.createElement('th');
            thImporteContratado.classList.add('text-center');
            thImporteContratado.textContent = 'Importe';

            let thVolumenFacturado = document.createElement('th');
            thVolumenFacturado.classList.add('text-center');
            thVolumenFacturado.textContent = 'Volumen';

            let thImporteFacturado = document.createElement('th');
            thImporteFacturado.classList.add('text-center');
            thImporteFacturado.textContent = 'Facturado';

            $(tr).append(th);
            $(tr).append(thActividad);
            $(tr).append(thUnidad);
            $(tr).append(thCantidad);
            $(tr).append(thPrecioUnitario);
            $(tr).append(thImporteContratado);
            $(tr).append(thVolumenFacturado);
            $(tr).append(thImporteFacturado);

            $(head).append(tr);
            $(table).append(head);

            return table;
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

        function crearConcentradoRow(subcapitulo) {
            let tr = document.createElement('tr');
            tr.setAttribute('style', 'background-color:' + colorConcentradoROW(subcapitulo));

            let tdSubcapitulo = document.createElement('td');
            let divSubcapitulo = document.createElement('div');
            tdSubcapitulo.setAttribute('width', '5%');
            tdSubcapitulo.setAttribute('align', 'left');
            if (subcapitulo.subcapituloN2_id > 0) tdSubcapitulo.setAttribute('style', 'padding-left:25px');
            divSubcapitulo.append(subcapitulo.subcapituloN1 == null ? subcapitulo.subcapituloN2 == null ? subcapitulo.subcapituloN3 : subcapitulo.subcapituloN2 : subcapitulo.subcapituloN1);
            divSubcapitulo.classList.add('toolConcentrado');
            divSubcapitulo.classList.add('dw50');
            $(tdSubcapitulo).append(divSubcapitulo);

            let tdPresupuesto = document.createElement('td');
            tdPresupuesto.setAttribute('width', '2%');
            tdPresupuesto.setAttribute('align', 'right');
            tdPresupuesto.textContent = '$ ' + formatValue(subcapitulo.presupuesto);

            let tdEjecutado = document.createElement('td');
            tdEjecutado.setAttribute('width', '2%');
            tdEjecutado.setAttribute('align', 'right');
            tdEjecutado.textContent = '$ ' + formatValue(subcapitulo.ejecutado);

            let tdxEjecutar = document.createElement('td');
            tdxEjecutar.setAttribute('width', '2%');
            tdxEjecutar.setAttribute('align', 'right');
            tdxEjecutar.textContent = '$ ' + formatValue(subcapitulo.xEjecutar);

            let tdPorcentajeAvance = document.createElement('td');
            tdPorcentajeAvance.setAttribute('width', '2%');
            tdPorcentajeAvance.setAttribute('align', 'right');
            tdPorcentajeAvance.textContent = '% ' + formatValue(subcapitulo.porcentajeAvance.toFixed(4));

            let tdImporteSemana = document.createElement('td');
            tdImporteSemana.setAttribute('width', '2%');
            tdImporteSemana.setAttribute('align', 'right');
            tdImporteSemana.textContent = '$ ' + formatValue(subcapitulo.importeSemana);

            let thFacturado = document.createElement('td');
            thFacturado.setAttribute('width', '2%');
            thFacturado.setAttribute('align', 'right');
            thFacturado.textContent = '$ ' + formatValue(subcapitulo.facturado);

            let tdxFacturar = document.createElement('td');
            tdxFacturar.setAttribute('width', '2%');
            tdxFacturar.setAttribute('align', 'right');
            tdxFacturar.textContent = '$ ' + formatValue(subcapitulo.xFacturar);


            $(tr).append(tdSubcapitulo);
            $(tr).append(tdPresupuesto);
            $(tr).append(tdEjecutado);
            $(tr).append(tdxEjecutar);
            $(tr).append(tdPorcentajeAvance);
            $(tr).append(tdImporteSemana);
            $(tr).append(thFacturado);
            $(tr).append(tdxFacturar);

            return tr;
        }

        function crearAvanceRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let td = document.createElement('td');
            td.setAttribute('style', 'border-right: none');
            td.setAttribute('width', '1.5%');

            let tdActividad = document.createElement('td');
            let divActividad = document.createElement('div');
            tdActividad.setAttribute('width', '5%');
            tdActividad.setAttribute('style', 'border-left: none');
            divActividad.append(actividad.actividad);
            divActividad.classList.add('iffyTip');
            divActividad.classList.add('dwMax');
            $(tdActividad).append(divActividad);

            let tdUnidad = document.createElement('td');
            tdUnidad.setAttribute('width', '5.5%');
            tdUnidad.textContent = actividad.unidad;

            let tdCantidad = document.createElement('td');
            tdCantidad.classList.add('cantidadFact');
            tdCantidad.setAttribute('width', '5%');
            tdCantidad.setAttribute('align', 'right');
            tdCantidad.textContent = formatValue(actividad.cantidad);

            let tdAcumAnterior = document.createElement('td');
            tdAcumAnterior.classList.add('acumAnt');
            tdAcumAnterior.setAttribute('width', '5%');
            tdAcumAnterior.setAttribute('align', 'right');
            tdAcumAnterior.textContent = formatValue(actividad.acumActual);

            let tdVolumenAvanzado = document.createElement('td');
            let inputVolumen = document.createElement('input');
            inputVolumen.classList.add('inputVolumen');
            inputVolumen.setAttribute('style', 'background-color: #ccffcc; text-align: right; border: 0;');
            inputVolumen.setAttribute('value', formatValue(0));
            tdVolumenAvanzado.setAttribute('style', 'background-color: #ccffcc;');
            tdVolumenAvanzado.setAttribute('width', '7%');
            $(tdVolumenAvanzado).append(inputVolumen);

            let tdAcumActual = document.createElement('td');
            tdAcumActual.classList.add('acumAct');
            tdAcumActual.setAttribute('width', '5%');
            tdAcumActual.setAttribute('align', 'right');
            tdAcumActual.textContent = formatValue(0);

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdAcumAnterior);
            $(tr).append(tdVolumenAvanzado);
            $(tr).append(tdAcumActual);

            return tr;
        }

        function crearAvanceSemanalRow(actividad) {
            let tr = document.createElement('tr');
            tr.setAttribute('data-actividadID', actividad.actividad_id);

            let td = document.createElement('td');
            td.setAttribute('style', 'border-right: none');
            td.setAttribute('width', '1.5%');

            let tdActividad = document.createElement('td');
            let divActividad = document.createElement('div');
            tdActividad.setAttribute('width', '5%');
            tdActividad.setAttribute('style', 'border-left: none');
            divActividad.append(actividad.actividad);
            divActividad.classList.add('iffyTip');
            divActividad.classList.add('dwMax');
            $(tdActividad).append(divActividad);

            let tdUnidad = document.createElement('td');
            tdUnidad.setAttribute('width', '5.5%');
            tdUnidad.textContent = actividad.unidad;

            let tdCantidad = document.createElement('td');
            tdCantidad.classList.add('cantidadFact');
            tdCantidad.setAttribute('width', '5%');
            tdCantidad.setAttribute('align', 'right');
            tdCantidad.textContent = formatValue(actividad.cantidad);

            let tdAcumAnterior = document.createElement('td');
            tdAcumAnterior.classList.add('acumAnt');
            tdAcumAnterior.setAttribute('width', '5%');
            tdAcumAnterior.setAttribute('align', 'right');
            tdAcumAnterior.textContent = formatValue(actividad.acumActual);

            let tdVolumenAvanzado = document.createElement('td');
            let inputVolumen = document.createElement('input');
            inputVolumen.classList.add('inputVolumen');
            inputVolumen.setAttribute('style', 'background-color: #ccffcc; text-align: right; border: 0;');
            inputVolumen.setAttribute('value', formatValue(0));
            tdVolumenAvanzado.setAttribute('style', 'background-color: #ccffcc;');
            tdVolumenAvanzado.setAttribute('width', '7%');
            $(tdVolumenAvanzado).append(inputVolumen);

            let tdAcumActual = document.createElement('td');
            tdAcumActual.classList.add('acumAct');
            tdAcumActual.setAttribute('width', '5%');
            tdAcumActual.setAttribute('align', 'right');
            tdAcumActual.textContent = formatValue(0);

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdAcumAnterior);
            $(tr).append(tdVolumenAvanzado);
            $(tr).append(tdAcumActual);

            return tr;
        }

        function crearFacturadoRow(actividad) {
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
            divActividad.classList.add('wdFacturado');
            $(tdActividad).append(divActividad);

            let tdUnidad = document.createElement('td');
            tdUnidad.setAttribute('width', '5.5%');
            tdUnidad.textContent = actividad.unidad;

            let tdCantidad = document.createElement('td');
            tdCantidad.classList.add('cantidadFact');
            tdCantidad.setAttribute('width', '5%');
            tdCantidad.setAttribute('align', 'right');
            tdCantidad.textContent = formatValue(actividad.cantidad);

            let tdPrecioUnitario = document.createElement('td');
            tdPrecioUnitario.classList.add('PU');
            tdPrecioUnitario.setAttribute('width', '7%');
            tdPrecioUnitario.setAttribute('align', 'right');
            tdPrecioUnitario.textContent = maskNumero(actividad.actividadPU);

            let tdImporteContratado = document.createElement('td');
            tdImporteContratado.setAttribute('width', '7%');
            tdImporteContratado.setAttribute('align', 'right');
            tdImporteContratado.textContent = maskNumero(actividad.importeContratado);

            let tdVolumenFacturado = document.createElement('td');
            let inputVolumen = document.createElement('input');
            inputVolumen.classList.add('inputVolumenFacturado');
            inputVolumen.setAttribute('style', 'background-color: #ccffcc; text-align: right; border: 0;');
            inputVolumen.setAttribute('value', formatValue(0));
            tdVolumenFacturado.setAttribute('style', 'background-color: #ccffcc;');
            tdVolumenFacturado.setAttribute('width', '7%');
            $(tdVolumenFacturado).append(inputVolumen);

            let tdImporteFacturado = document.createElement('td');
            tdImporteFacturado.setAttribute('width', '7%');
            tdImporteFacturado.setAttribute('style', 'text-align: right;');
            tdImporteFacturado.classList.add('inputImporteFacturado');
            tdImporteFacturado.textContent = maskNumero(0);
            //let inputImporte = document.createElement('input');
            // inputImporte.classList.add('inputImporte');
            // inputImporte.setAttribute('style', 'background-color: #ccffcc;  text-align: right;');
            // inputImporte.setAttribute('value', formatValue(0));
            //$(tdImporteFacturado).append(inputImporte);

            $(tr).append(td);
            $(tr).append(tdActividad);
            $(tr).append(tdUnidad);
            $(tr).append(tdCantidad);
            $(tr).append(tdPrecioUnitario);
            $(tr).append(tdImporteContratado);
            $(tr).append(tdVolumenFacturado);
            $(tr).append(tdImporteFacturado);

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

        function colorConcentradoROW(subcapitulo) {
            let color = '';

            if (subcapitulo.subcapituloN1_id > 0) color = '#f2f2f2';
            else if (subcapitulo.capitulo_id > 0) color = '#d9d9d9';
            else color = 'white'

            return color;
        }

        function crearObjectoAvance() {
            let avance = new Object();
            avance = {
                capitulo_id: $('#SelectCapitulosFiltro').val(),
                fechaI: moment(dpFechaAvance.val(), "DD-MM-YYYY").format(),
                fechaF: moment(dpFechaAvance.val(), "DD-MM-YYYY").format(),
                periodoAvance: 1,
                autorizado: false,
                estatus: true
            };
            let avanceDetalle = [];
            let detalle;
            let total = 0;
            let avanceValido = true;

            $('#tblPresupuestoDiario').find('tbody tr[data-actividadid]').each(function (index) {
                const row = $(this).closest('tr');
                const actividad_id = parseInt(row.attr('data-actividadid'));
                const cantidadFact = unmaskNumero(row.find('.cantidadFact').text());
                const volumen = unmaskNumero(row.find('.inputVolumen').val());
                total += volumen;

                if (volumen > cantidadFact) {
                    avanceValido = false;
                    return false;
                }

                if (actividad_id > 0) {
                    detalle = new Object();
                    detalle.actividad_id = actividad_id;
                    detalle.cantidadAvance = volumen;
                    detalle.fechaI = moment(dpFechaAvance.val(), "DD-MM-YYYY").format();
                    detalle.fechaF = moment(dpFechaAvance.val(), "DD-MM-YYYY").format();
                    avanceDetalle.push(detalle);
                }
            });

            if (avanceValido) {
                if (total > 0) {
                    let avanceObj = new FormData();
                    avanceObj.append("avance", JSON.stringify(avance));
                    avanceObj.append("avanceDetalle", JSON.stringify(avanceDetalle));

                    guardarAvance(avanceObj);
                } else {
                    AlertaGeneral('Aviso', 'Debe ingresar una cantidad en Avance Periodo');
                }
            } else {
                AlertaGeneral('Aviso', 'No puede avanzar un volumen mayor al presupuesto.')
            }
        }

        function crearObjectoAvanceSemanal() {
            let avance = new Object();
            avance = {
                capitulo_id: $('#SelectCapitulosFiltro').val(),
                fechaI: moment(dpFechaAvanceSemanal.val(), "DD-MM-YYYY").format(),
                fechaF: moment(dpFechaAvanceSemanal.val(), "DD-MM-YYYY").format(),
                periodoAvance: 2,
                autorizado: false,
                estatus: true
            };
            let avanceDetalle = [];
            let detalle;
            let total = 0;
            let avanceValido = true;

            $('#tblPresupuestoSemanal').find('tbody tr[data-actividadid]').each(function (index) {
                const row = $(this).closest('tr');
                const actividad_id = parseInt(row.attr('data-actividadid'));
                const cantidadFact = unmaskNumero(row.find('.cantidadFact').text());
                const volumen = unmaskNumero(row.find('.inputVolumen').val());
                total += volumen;

                if (volumen > cantidadFact) {
                    avanceValido = false;
                    return false;
                }

                if (actividad_id > 0) {
                    detalle = new Object();
                    detalle.actividad_id = actividad_id;
                    detalle.cantidadAvance = volumen;
                    detalle.fechaI = moment(dpFechaAvanceSemanal.val(), "DD-MM-YYYY").format();
                    detalle.fechaF = moment(dpFechaAvanceSemanal.val(), "DD-MM-YYYY").format();
                    avanceDetalle.push(detalle);
                }
            });

            if (avanceValido) {
                if (total > 0) {
                    let avanceObj = new FormData();
                    avanceObj.append("avance", JSON.stringify(avance));
                    avanceObj.append("avanceDetalle", JSON.stringify(avanceDetalle));

                    guardarAvance(avanceObj);
                } else {
                    AlertaGeneral('Aviso', 'Debe ingresar una cantidad en Avance Periodo');
                }
            } else {
                AlertaGeneral('Aviso', 'No puede avanzar un volumen mayor al presupuesto.')
            }
        }

        function guardarAvance(avanceObj) {
            $.blockUI({ message: "Preparando información" });
            $.ajax({
                url: '/ControlObra/ControlObra/GuardarAvanceActividad',
                data: avanceObj,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                type: 'POST',
                success: function (data) {
                    if (data.success) {
                        AlertaGeneral("Aviso", "Avance guardado correctamente.");
                        $.unblockUI();
                        recargar();
                    } else {
                        AlertaGeneral("Aviso", "Ocurrió un error al guardar el avance.");
                        $.unblockUI();
                    }
                }
            });
        }

        function crearObjectoFacturado() {
            let facturado = new Object();
            facturado = {
                capitulo_id: $('#SelectCapitulosFiltro').val(),
                fecha: moment(dpFechaInicio.val(), "DD-MM-YYYY").format(),
                autorizado: false,
                estatus: true
            };
            let facturadoDetalle = [];
            let detalle;
            let total = 0;
            let facturaValido = true;

            $('#tblFacturado').find('tbody tr[data-actividadid]').each(function (index) {
                const row = $(this).closest('tr');
                const actividad_id = parseInt(row.attr('data-actividadid'));
                const cantidadFact = unmaskNumero(row.find('.cantidadFact').text());
                const volumen = unmaskNumero(row.find('.inputVolumenFacturado').val());
                const importe = unmaskNumero(row.find('.inputImporteFacturado').text());
                total += importe;

                if (volumen > cantidadFact) {
                    facturaValido = false;
                    return false;
                }

                if (actividad_id > 0) {
                    detalle = new Object();
                    detalle.actividad_id = actividad_id;
                    detalle.volumen = volumen;
                    detalle.importe = importe;
                    facturadoDetalle.push(detalle);
                }
            });

            if (facturaValido) {
                if (total > 0) {
                    let objFacturado = new FormData();
                    objFacturado.append("facturado", JSON.stringify(facturado));
                    objFacturado.append("facturadoDetalle", JSON.stringify(facturadoDetalle));

                    guardarAvanceFacturado(objFacturado);
                }
                else {
                    AlertaGeneral('Aviso', 'Debe ingresar una cantidad en volumen facturado');
                }
            }
            else {
                AlertaGeneral('Aviso', 'No puede facturar un volumen mayor al presupuesto.')
            }

        }

        function guardarAvanceFacturado(objFacturado) {
            $.blockUI({ message: "Preparando información" });

            $.ajax({
                url: '/ControlObra/ControlObra/GuardarFacturadoActividad',
                data: objFacturado,
                cache: false,
                contentType: false,
                processData: false,
                method: 'POST',
                type: 'POST',
                success: function (data) {
                    if (data.success) {
                        AlertaGeneral("Aviso", "Avance guardado correctamente.");
                        $.unblockUI();
                        recargar();
                    } else {
                        AlertaGeneral("Aviso", "Error");
                        $.unblockUI();
                    }
                }
            });
        }

        function sumaTotalFacturado() {
            let total = 0;

            $('#tblFacturado').find('tbody tr[data-actividadid]').each(function (index) {
                const row = $(this).closest('tr');
                const actividad_id = parseInt(row.attr('data-actividadid'));
                const volumen = unmaskNumero(row.find('.inputVolumenFacturado').val());
                const importe = unmaskNumero(row.find('.inputImporteFacturado').text());
                total += importe;

            });

            return total;
        }

        function recargar() {
            CargarTablas();
            fnFechaInicio();
        }

        function limpiarTabla() {
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

            $('#tblPresupuestoDiario thead tr').remove();
            $('#tblPresupuestoDiario thead').remove();
            $('#tblPresupuestoDiario tbody tr').remove();
            $('#tblPresupuestoDiario tbody').remove();

            $('#tblPresupuestoSemanal thead tr').remove();
            $('#tblPresupuestoSemanal thead').remove();
            $('#tblPresupuestoSemanal tbody tr').remove();
            $('#tblPresupuestoSemanal tbody').remove();

            $('#tblFacturado thead tr').remove();
            $('#tblFacturado thead').remove();
            $('#tblFacturado tbody tr').remove();
            $('#tblFacturado tbody').remove();
        }

        function limpiarTablaConcentrado() {
            $('#tblConcentrado thead tr').remove();
            $('#tblConcentrado thead').remove();
            $('#tblConcentrado tbody tr').remove();
            $('#tblConcentrado tbody').remove();
        }

        function fnFechaInicio() {
            $.ajax({
                url: '/ControlObra/ControlObra/GetFechasUltimoAvance',
                datatype: "json",
                type: "GET",
                data: {
                    capituloID: $("#SelectCapitulosFiltro").val()
                },
                success: function (response) {
                    const capitulo = response.capitulo;
                    const avanceDiario = response.avanceDiario;
                    const avanceSemanal = response.avanceSemanal;

                    $('#periodoFact').text(capitulo.periodoFact);

                    if (avanceDiario == null) {
                        dpFechaAvance.datepicker("setDate", capitulo.fechaInicio);
                    } else {
                        const fecha = moment(avanceDiario.fechaFin, "DD-MM-YYYY").add(1, 'days').format();
                        dpFechaAvance.datepicker("setDate", new Date(fecha));
                    }

                    if (avanceSemanal == null) {
                        const fecha = moment(capitulo.fechaInicio, "DD-MM-YYYY").add(6, 'days').format();
                        dpFechaAvanceSemanal.datepicker("setDate", new Date(fecha));
                    } else {
                        const fecha = moment(avanceSemanal.fechaFin, "DD-MM-YYYY").add(6, 'days').format();
                        dpFechaAvanceSemanal.datepicker("setDate", new Date(fecha));
                    }

                    txtFechaSaldo.datepicker({
                        firstDay: 0,
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        // minDate: $.toDate(capitulo.fechaInicio),
                        // maxDate: $.toDate(capitulo.fechaFin),
                        onSelect: function (dateText, inst) {
                            CargarTablas();
                            setSemanaGlobalSelecionada();
                        },
                        beforeShowDay: function (date) {
                            var cssClass = '';
                            if (date >= startDate && date <= endDate)
                                cssClass = 'ui-datepicker-current-day';
                            return [true, cssClass];
                        },
                        onChangeMonthYear: function (year, month, inst) {
                            selectCurrentWeek();
                        },
                        beforeShow: function () {
                            setTimeout(function () {
                                $('.ui-datepicker').css('z-index', 9999);
                            }, 0);
                        }
                    }).datepicker("setDate", new Date());
                    setSemanaGlobalSelecionada();
                    CargarTablas();
                }
            });
        }

        function paintCells(actividad_id) {
            if (actividad_id !== undefined && actividad_id.length > 0) {
                $(document).find(`tbody tr[data-actividadid=${actividad_id}]`).addClass("pintarGrupo");
            }
        }

        function unpaintCells(actividad_id, tr) {
            if (actividad_id !== undefined && actividad_id.length > 0) {
                $(document).find(`tbody tr[data-actividadid=${actividad_id}]`).removeClass("pintarGrupo");
            }
        }

        setSemanaGlobalSelecionada = () => {
            let date = txtFechaSaldo.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 5) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 9),
                diaStartNombre = startDate.toLocaleDateString("es-MX", { weekday: 'long' }).toUpperCase(),
                diaStartNumero = startDate.getDate(),
                mesStartNombre = startDate.toLocaleDateString("es-MX", { month: 'long' }).toUpperCase(),
                anioStart = startDate.getFullYear(),
                diaNombre = endDate.toLocaleDateString("es-MX", { weekday: 'long' }).toUpperCase(),
                diaNumero = endDate.getDate(),
                mesNombre = endDate.toLocaleDateString("es-MX", { month: 'long' }).toUpperCase(),
                anio = endDate.getFullYear();
            txtFechaSaldo.val(`${diaStartNombre}, ${diaStartNumero} DE ${mesStartNombre} AL ${diaNombre}, ${diaNumero} DE ${mesNombre} DE ${anio}`);
            selectCurrentWeek();
        }

        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                txtFechaSaldo.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }

        function getBusq() {
            let date = txtFechaSaldo.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 5) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 9);
            return {
                min: startDate.toLocaleDateString(),
                max: endDate.toLocaleDateString()
            };
        }

        init();
    };

    $(document).ready(function () {
        controlObra.ReporteAvances = new ReporteAvances();
    });
})();