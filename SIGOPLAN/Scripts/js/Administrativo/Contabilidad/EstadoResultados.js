(() => {
    $.namespace('Administrativo.Contabilidad.EstadoResultados');
    EstadoResultados = function () {
        //#region Selectores
        const report = $('#report');

        const selectEmpresa = $('#selectEmpresa');
        const inputMes = $('#inputMes');
        const selectCentroCosto = $('#selectCentroCosto');
        const tablaEstadoResultados = $('#tablaEstadoResultados');
        const botonBuscar = $('#botonBuscar');
        const botonReporte = $('#botonReporte');
        const modalIngreso = $('#modalIngreso');
        const modalCosto = $('#modalCosto');
        const modalGasto = $('#modalGasto');
        const tablaIngreso = $('#tablaIngreso');
        const tablaCosto = $('#tablaCosto');
        const tablaGasto = $('#tablaGasto');

        const graficaIngresoColumnas = $("#graficaIngresoColumnas");
        const graficaIngresoLineas = $("#graficaIngresoLineas");
        const lblGraficaIngreso = $("#lblGraficaIngreso");
        const graficaCostoColumnas = $("#graficaCostoColumnas");        
        const graficaCostoLineas = $("#graficaCostoLineas");
        const lblGraficaCosto = $("#lblGraficaCosto");
        //#endregion

        let dtTablaEstadoResultados;
        let dtTablaIngreso;
        let dtTablaCosto;
        let dtTablaGasto;

        let listaEmpresas;
        let fecha;
        let listaCC;
        let mesGlobal;
        let anioGlobal;

        //#region Variables Date
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        let listaMeses = [
            { mes: 1, nombre: 'ENERO' },
            { mes: 2, nombre: 'FEBRERO' },
            { mes: 3, nombre: 'MARZO' },
            { mes: 4, nombre: 'ABRIL' },
            { mes: 5, nombre: 'MAYO' },
            { mes: 6, nombre: 'JUNIO' },
            { mes: 7, nombre: 'JULIO' },
            { mes: 8, nombre: 'AGOSTO' },
            { mes: 9, nombre: 'SEPTIEMBRE' },
            { mes: 10, nombre: 'OCTUBRE' },
            { mes: 11, nombre: 'NOVIEMBRE' },
            { mes: 12, nombre: 'DICIEMBRE' }
        ];
        let funcionMonto = function (data, type, row, meta) {
            if (type === 'display') {
                //let montoMillares = Math.trunc(data / 1000);
                let montoMillares = data;

                return montoMillares >= 0 ? convertirMontoMillares(montoMillares) : `<p style="color: red; margin-bottom: 0px;">${('-' + (convertirMontoMillares(montoMillares).replace('-', '')))}</p>`;
            } else {
                return data;
            }
        };
        let funcionPorcentaje = function (data, type, row, meta) {
            if (type === 'display') {
                return data >= 0 ? data + '%' : `<p style="color: red; margin-bottom: 0px;">${data}%</p>`;
            } else {
                return data;
            }
        };
        let funcionConcepto = function (data, type, row, meta) {
            if (type === 'display') {
                if (row.renglonEnlace) {
                    let tiposModal = [
                        { tipoDetalle: 0, nombre: '' },
                        { tipoDetalle: 1, nombre: 'Ingreso' },
                        { tipoDetalle: 2, nombre: 'Costo' },
                        { tipoDetalle: 3, nombre: 'Gasto' }
                    ];
                    let modal = tiposModal.filter((x) => x.tipoDetalle == row.tipoDetalle);
                    let modalNombre = modal.length > 0 ? modal[0].nombre : '';

                    return `<a data-target="#modal${modalNombre}" data-toggle="modal${modalNombre}" href="#modal${modalNombre}" nombre-detalle="${modalNombre}">${row.concepto}</a>`;
                } else {
                    return data;
                }
            } else {
                return data;
            }
        };

        (function init() {
            
            //initTablaCosto();
            //initTablaGasto();
            initMonthPicker(inputMes);

            selectCentroCosto.fillCombo('FillComboCC', null, false, 'Todos');
            convertToMultiselect('#selectCentroCosto');
            convertToMultiselect('#selectEmpresa');

            botonBuscar.click(cargarEstadoResultados);
            botonReporte.click(cargarReporte);
        })();

        function initTablaEstadoResultados() {
            dtTablaEstadoResultados = tablaEstadoResultados.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                scrollY: '60vh',
                scrollCollapse: true,
                createdRow: function (row, rowData) {
                    if (rowData.flagGrupoReporte) {
                        let celda = $(row).find('td');

                        celda.css('font-weight', '800');
                        celda.css('border-top', '2px solid black');
                    }
                },
                columns: [
                    { data: 'ordenReporte' },
                    { data: 'concepto' },
                    {
                        data: 'montoMesAnioActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                let montoMillares = Math.trunc(data / 1000);

                                return montoMillares >= 0 ? convertirMontoMillares(montoMillares) : `<p style="color: red; margin-bottom: 0px;">${('-' + (convertirMontoMillares(montoMillares).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'porcentajeMesAnioActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? data + '%' : `<p style="color: red; margin-bottom: 0px;">${data}%</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'montoMesAnioAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                let montoMillares = Math.trunc(data / 1000);

                                return montoMillares >= 0 ? convertirMontoMillares(montoMillares) : `<p style="color: red; margin-bottom: 0px;">${('-' + (convertirMontoMillares(montoMillares).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'porcentajeMesAnioAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? data + '%' : `<p style="color: red; margin-bottom: 0px;">${data}%</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'variaciones', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                let montoMillares = Math.trunc(data / 1000);

                                return montoMillares >= 0 ? convertirMontoMillares(montoMillares) : `<p style="color: red; margin-bottom: 0px;">${('-' + (convertirMontoMillares(montoMillares).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'montoMesAcumuladoAnioActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                let montoMillares = Math.trunc(data / 1000);

                                return montoMillares >= 0 ? convertirMontoMillares(montoMillares) : `<p style="color: red; margin-bottom: 0px;">${('-' + (convertirMontoMillares(montoMillares).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'porcentajeMesAcumuladoAnioActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? data + '%' : `<p style="color: red; margin-bottom: 0px;">${data}%</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'montoMesAcumuladoAnioAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                let montoMillares = Math.trunc(data / 1000);

                                return montoMillares >= 0 ? convertirMontoMillares(montoMillares) : `<p style="color: red; margin-bottom: 0px;">${('-' + (convertirMontoMillares(montoMillares).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'porcentajeMesAcumuladoAnioAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? data + '%' : `<p style="color: red; margin-bottom: 0px;">${data}%</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'variacionesAcumulado', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                let montoMillares = Math.trunc(data / 1000);

                                return montoMillares >= 0 ? convertirMontoMillares(montoMillares) : `<p style="color: red; margin-bottom: 0px;">${('-' + (convertirMontoMillares(montoMillares).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { targets: [0], visible: false }
                ]
            });
        }

        function initTablaEstadoResultadosDinamica(columnas, datos, cantidadEmpresas, mesNombre, anio) {
            if (dtTablaEstadoResultados != null) {
                dtTablaEstadoResultados.destroy();
                tablaEstadoResultados.empty();
            }

            //#region Generar HTML para la tabla con columnas agrupadas
            tablaEstadoResultados.append('<thead></thead>');
            tablaEstadoResultados.find('thead').append('<tr></tr>');
            tablaEstadoResultados.find('thead').append('<tr></tr>');

            //#region Renglón Agrupado
            if (cantidadEmpresas == 1) {
                let agrupacion = 4 * cantidadEmpresas;
                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');

                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');
                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');
                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');

                // tablaEstadoResultados.find('thead tr:eq(0) th:eq(2)').addClass('text-center').attr('colspan', agrupacion + 1).text(`${mesNombre} ${anio}`);
                // tablaEstadoResultados.find('thead tr:eq(0) th:eq(3)').addClass('text-center').attr('colspan', agrupacion + 1).text(`${mesNombre} ${anio - 1}`);
                tablaEstadoResultados.find('thead tr:eq(0) th:eq(2)').addClass('text-center').attr('colspan', agrupacion + 1).text(`MENSUAL`);
                tablaEstadoResultados.find('thead tr:eq(0) th:eq(3)').addClass('text-center').attr('colspan', agrupacion + 1).text(`ACUMULADO`);

                let posicionCelda = 0;

                columnas.forEach(columna => {
                    tablaEstadoResultados.find('thead tr:eq(1)').append('<th></th>');
                    tablaEstadoResultados.find(`thead tr:eq(1) th:eq(${posicionCelda})`).text(posicionCelda > 1 ? columna.tituloPrueba : '');

                    posicionCelda++;
                });

                tablaEstadoResultados.find('thead tr:eq(0) th:eq(1)').attr('rowspan', '2').text('CONCEPTOS');
                tablaEstadoResultados.find('thead tr:eq(1) th:eq(0)').remove();
            } else {
                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');// Contador
                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');// Conceptos
                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');// Empresas

                // let col = 0;
                // for (let index = 0; index < cantidadEmpresas; index++) {
                //     tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');//Empresas

                //     col++;
                // }
                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');//Consolidado
                tablaEstadoResultados.find('thead tr:eq(0)').append('<th></th>');//Consolidado anterior


                tablaEstadoResultados.find('thead tr:eq(0) th:eq(2)').addClass('text-center').attr('colspan', cantidadEmpresas * 2).text(`EMPRESAS`);
                tablaEstadoResultados.find('thead tr:eq(0) th:eq(3)').addClass('text-center').attr('colspan', cantidadEmpresas).text(`CONSOLIDADO`);
                tablaEstadoResultados.find('thead tr:eq(0) th:eq(4)').addClass('text-center').attr('colspan', cantidadEmpresas).text(`${mesNombre} ${anio - 1}`);

                let posicionCelda = 0;

                columnas.forEach(columna => {
                    tablaEstadoResultados.find('thead tr:eq(1)').append('<th></th>');
                    tablaEstadoResultados.find(`thead tr:eq(1) th:eq(${posicionCelda})`).text(posicionCelda > 1 ? columna.tituloPrueba : '');

                    posicionCelda++;
                });

                tablaEstadoResultados.find('thead tr:eq(0) th:eq(1)').attr('rowspan', '2').text('CONCEPTOS');
                tablaEstadoResultados.find('thead tr:eq(1) th:eq(0)').remove();
            }
            //#endregion
            //#endregion

            dtTablaEstadoResultados = tablaEstadoResultados.DataTable({
                destroy: true,
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                scrollY: '84vh',
                scrollX: false,
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaEstadoResultados.on('click', 'a', function () {
                        let rowData = dtTablaEstadoResultados.row($(this).closest('tr')).data();
                        let modal = $(this).attr('data-target');
                        let nombreDetalle = $(this).attr('nombre-detalle');

                        cargarInformacionModal(modal, nombreDetalle, rowData);
                    });
                },
                createdRow: function (row, rowData) {
                    if (rowData.flagGrupoReporte) {
                        let celda = $(row).find('td');

                        celda.css('font-weight', '800');
                        celda.css('border-top', '2px solid black');
                    } else if (rowData.renglonEnlace) {
                        let celda = $(row).find('td');

                        $(celda[0]).css('font-weight', '700');
                        $(celda[0]).css('text-decoration', 'underline');
                    }
                },
                columns: columnas,
                data: datos,
                columnDefs: [
                    { className: "dt-left", targets: [1] },
                    { className: "dt-center", targets: "_all" },
                    { targets: [0], visible: false }
                ]
            });
        }

        function initTablaIngreso() {
            if (dtTablaIngreso != null) {
                dtTablaIngreso.destroy();
            }
            tablaIngreso.empty();
            tablaIngreso.append('<thead></thead>');     
            tablaIngreso.append('<tbody></tbody>'); 
            tablaIngreso.append('<tfoot><tr><th colspan="1" style="text-align:right">Total:</th><th></th><th></th><th></th><th></th><th></th><th></th><th></th><th></th></tr></tfoot>'); 

            dtTablaIngreso = tablaIngreso.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                ordering: false,
                createdRow: function (row, rowData) {
                    let celdas = $(row).find('td');

                    if (rowData.renglonSubTitulo) {
                        celdas.css('font-weight', '700');
                        celdas.css('font-size', '15px');
                        celdas.css('font-style', 'italic');
                        celdas.css('text-align', 'left');
                    } else if (rowData.renglonGrupo) {
                        celdas.css('font-weight', '700');
                        celdas.css('border-top', '2px solid black');
                        celdas.css('text-align', 'right');
                        celdas.css('font-style', 'italic');
                    } else if (rowData.renglonEnlace) {
                        $(celdas[0]).css('font-weight', '700');
                        $(celdas[0]).css('text-decoration', 'underline');
                        $(celdas[0]).css('text-align', 'left');
                    } else {
                        celdas.css('text-align', 'middle');
                    }
                },
                columns: [
                    //{ data: 'divisionID', title: '', visible: false },
                    { data: 'divisionDescr' },
                    {
                        data: 'mensualActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'porcentajeMensualActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumeroNM(data) + '%' : '<p style="color: red;">' + (maskNumeroNM(data)) + '%</p>'
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'acumuladoActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'porcentajeAcumuladoActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumeroNM(data) + '%' : '<p style="color: red;">' + (maskNumeroNM(data)) + '%</p>'
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'mensualAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'porcentajeMensualAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumeroNM(data) + '%' : '<p style="color: red;">' + (maskNumeroNM(data)) + '%</p>'
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'acumuladoAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'porcentajeAcumuladoAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumeroNM(data) + '%' : '<p style="color: red;">' + (maskNumeroNM(data)) + '%</p>'
                            } else {
                                return data;
                            }
                        }
                    },
                ],
                columnDefs: [
                    { className: 'dt-center', targets: [1,2,3,4,5,6,7,8] },
                    { width: "30%", targets: 0 }
                ],
                initComplete: function (settings, json) {
                    tablaIngreso.find('thead').append('<tr></tr>');
                    let mesAnio = getMesAnio();
                    tablaIngreso.find('thead tr:eq(0) th:eq(0)').addClass('text-center').text(`DIVISION`);
                    tablaIngreso.find('thead tr:eq(0) th:eq(1)').addClass('text-center').text(`${mesAnio.mesNombre} ${mesAnio.anio}`);
                    tablaIngreso.find('thead tr:eq(0) th:eq(2)').addClass('text-center').text(`%`);
                    tablaIngreso.find('thead tr:eq(0) th:eq(3)').addClass('text-center').text(`ACUMULADO ${mesAnio.anio}`);
                    tablaIngreso.find('thead tr:eq(0) th:eq(4)').addClass('text-center').text(`%`);
                    tablaIngreso.find('thead tr:eq(0) th:eq(5)').addClass('text-center').text(`${mesAnio.mesNombre} ${mesAnio.anio - 1}`);
                    tablaIngreso.find('thead tr:eq(0) th:eq(6)').addClass('text-center').text(`%`);
                    tablaIngreso.find('thead tr:eq(0) th:eq(7)').addClass('text-center').text(`ACUMULADO ${mesAnio.anio - 1}`);
                    tablaIngreso.find('thead tr:eq(0) th:eq(8)').addClass('text-center').text(`%`);
                },
                footerCallback: function ( row, data, start, end, display ) {                
                    var api = this.api(), data;
 
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '')*1 : typeof i === 'number' ? i : 0;
                    };
 
                    // Total over all pages
                    total1 = api.column(1).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total2 = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total3 = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total4 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total5 = api.column(5).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total6 = api.column(6).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total7 = api.column(7).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total8 = api.column(8).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
  
                    // Update footer
                    $(api.column(1).footer()).html(total1 >= 0 ? '$' + maskNumero2D(total1) : `<p style="color: red;">${('-' + ('$' + maskNumero2D(total1).replace('-', '')))}</p>`);
                    $(api.column(2).footer()).html(total2 >= 0 ? parseFloat(total2).toFixed(2) + '%' : '<p style="color: red;">' + (parseFloat(total2).toFixed(2).replace('-', '')) + '%</p>');
                    $(api.column(3).footer()).html(total3 >= 0 ? '$' + maskNumero2D(total3) : `<p style="color: red;">${('-' + ('$' + maskNumero2D(total3).replace('-', '')))}</p>`);
                    $(api.column(4).footer()).html(total4 >= 0 ? parseFloat(total4).toFixed(2) + '%' : '<p style="color: red;">' + (parseFloat(total4).toFixed(2).replace('-', '')) + '%</p>');
                    $(api.column(5).footer()).html(total5 >= 0 ? '$' + maskNumero2D(total5) : `<p style="color: red;">${('-' + ('$' + maskNumero2D(total5).replace('-', '')))}</p>`);
                    $(api.column(6).footer()).html(total6 >= 0 ? parseFloat(total6).toFixed(2) + '%' : '<p style="color: red;">' + (parseFloat(total6).toFixed(2).replace('-', '')) + '%</p>');
                    $(api.column(7).footer()).html(total7 >= 0 ? '$' + maskNumero2D(total7) : `<p style="color: red;">${('-' + ('$' + maskNumero2D(total7).replace('-', '')))}</p>`);
                    $(api.column(8).footer()).html(total8 >= 0 ? parseFloat(total8).toFixed(2) + '%' : '<p style="color: red;">' + (parseFloat(total8).toFixed(2).replace('-', '')) + '%</p>');
                }
            });
        }

        function initTablaCosto() {
            if (dtTablaCosto != null) {
                dtTablaCosto.destroy();
            }
            tablaCosto.empty();
            tablaCosto.append('<thead></thead>');     
            tablaCosto.append('<tbody></tbody>'); 
            tablaCosto.append('<tfoot><tr><th colspan="1" style="text-align:right">Total:</th><th></th><th></th><th></th><th></th></tr></tfoot>'); 

            dtTablaCosto = tablaCosto.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                ordering: false,
                createdRow: function (row, rowData) {
                    let celdas = $(row).find('td');

                    if (rowData.renglonSubTitulo) {
                        celdas.css('font-weight', '700');
                        celdas.css('font-size', '15px');
                        celdas.css('font-style', 'italic');
                        celdas.css('text-align', 'left');
                    } else if (rowData.renglonGrupo) {
                        celdas.css('font-weight', '700');
                        celdas.css('border-top', '2px solid black');
                        celdas.css('text-align', 'right');
                        celdas.css('font-style', 'italic');
                    } else if (rowData.renglonEnlace) {
                        $(celdas[0]).css('font-weight', '700');
                        $(celdas[0]).css('text-decoration', 'underline');
                        $(celdas[0]).css('text-align', 'left');
                    } else {
                        celdas.css('text-align', 'middle');
                    }
                },
                columns: [
                    { data: 'divisionDescr' },
                    {
                        data: 'mensualActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'acumuladoActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'mensualAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'acumuladoAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: [1,2,3,4] },
                    { width: "30%", targets: 0 }
                ],
                initComplete: function (settings, json) {
                    tablaCosto.find('thead').append('<tr></tr>');
                    let mesAnio = getMesAnio();
                    tablaCosto.find('thead tr:eq(0) th:eq(0)').addClass('text-center').text(`CONCEPTO`);
                    tablaCosto.find('thead tr:eq(0) th:eq(1)').addClass('text-center').text(`${mesAnio.mesNombre} ${mesAnio.anio}`);
                    tablaCosto.find('thead tr:eq(0) th:eq(2)').addClass('text-center').text(`ACUMULADO ${mesAnio.anio}`);
                    tablaCosto.find('thead tr:eq(0) th:eq(3)').addClass('text-center').text(`${mesAnio.mesNombre} ${mesAnio.anio - 1}`);
                    tablaCosto.find('thead tr:eq(0) th:eq(4)').addClass('text-center').text(`ACUMULADO ${mesAnio.anio - 1}`);
                },
                footerCallback: function ( row, data, start, end, display ) {                
                    var api = this.api(), data;
 
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '')*1 : typeof i === 'number' ? i : 0;
                    };
 
                    // Total over all pages
                    total1 = api.column(1).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total2 = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total3 = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total4 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
  
                    // Update footer
                    $(api.column(1).footer()).html(total1 >= 0 ? maskNumero(total1) : `<p style="color: red;">${('-' + (maskNumero(total1).replace('-', '')))}</p>`);
                    $(api.column(2).footer()).html(total2 >= 0 ? maskNumero(total2) : `<p style="color: red;">${('-' + (maskNumero(total2).replace('-', '')))}</p>`);
                    $(api.column(3).footer()).html(total3 >= 0 ? maskNumero(total3) : `<p style="color: red;">${('-' + (maskNumero(total3).replace('-', '')))}</p>`);
                    $(api.column(4).footer()).html(total4 >= 0 ? maskNumero(total4) : `<p style="color: red;">${('-' + (maskNumero(total4).replace('-', '')))}</p>`);
                }
            });
        }

        function initTablaGasto() {
            if (dtTablaGasto != null) {
                dtTablaGasto.destroy();
            }
            tablaGasto.empty();
            tablaGasto.append('<thead></thead>');     
            tablaGasto.append('<tbody></tbody>'); 
            tablaGasto.append('<tfoot><tr><th colspan="1" style="text-align:right">Total:</th><th></th><th></th><th></th><th></th></tr></tfoot>'); 

            dtTablaGasto = tablaGasto.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                ordering: false,
                createdRow: function (row, rowData) {
                    let celdas = $(row).find('td');

                    if (rowData.renglonSubTitulo) {
                        celdas.css('font-weight', '700');
                        celdas.css('font-size', '15px');
                        celdas.css('font-style', 'italic');
                        celdas.css('text-align', 'left');
                    } else if (rowData.renglonGrupo) {
                        celdas.css('font-weight', '700');
                        celdas.css('border-top', '2px solid black');
                        celdas.css('text-align', 'right');
                        celdas.css('font-style', 'italic');
                    } else if (rowData.renglonEnlace) {
                        $(celdas[0]).css('font-weight', '700');
                        $(celdas[0]).css('text-decoration', 'underline');
                        $(celdas[0]).css('text-align', 'left');
                    } else {
                        celdas.css('text-align', 'middle');
                    }
                },
                columns: [
                    { data: 'divisionDescr' },
                    {
                        data: 'mensualActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'acumuladoActual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'mensualAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'acumuladoAnterior', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: [1,2,3,4] },
                    { width: "30%", targets: 0 }
                ],
                initComplete: function (settings, json) {
                    tablaGasto.find('thead').append('<tr></tr>');
                    let mesAnio = getMesAnio();
                    tablaGasto.find('thead tr:eq(0) th:eq(0)').addClass('text-center').text(`CONCEPTO`);
                    tablaGasto.find('thead tr:eq(0) th:eq(1)').addClass('text-center').text(`${mesAnio.mesNombre} ${mesAnio.anio}`);
                    tablaGasto.find('thead tr:eq(0) th:eq(2)').addClass('text-center').text(`ACUMULADO ${mesAnio.anio}`);
                    tablaGasto.find('thead tr:eq(0) th:eq(3)').addClass('text-center').text(`${mesAnio.mesNombre} ${mesAnio.anio - 1}`);
                    tablaGasto.find('thead tr:eq(0) th:eq(4)').addClass('text-center').text(`ACUMULADO ${mesAnio.anio - 1}`);
                },
                footerCallback: function ( row, data, start, end, display ) {                
                    var api = this.api(), data;
 
                    // Remove the formatting to get integer data for summation
                    var intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '')*1 : typeof i === 'number' ? i : 0;
                    };
 
                    // Total over all pages
                    total1 = api.column(1).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total2 = api.column(2).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total3 = api.column(3).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
                    total4 = api.column(4).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 );
  
                    // Update footer
                    $(api.column(1).footer()).html(total1 >= 0 ? maskNumero(total1) : `<p style="color: red;">${('-' + (maskNumero(total1).replace('-', '')))}</p>`);
                    $(api.column(2).footer()).html(total2 >= 0 ? maskNumero(total2) : `<p style="color: red;">${('-' + (maskNumero(total2).replace('-', '')))}</p>`);
                    $(api.column(3).footer()).html(total3 >= 0 ? maskNumero(total3) : `<p style="color: red;">${('-' + (maskNumero(total3).replace('-', '')))}</p>`);
                    $(api.column(4).footer()).html(total4 >= 0 ? maskNumero(total4) : `<p style="color: red;">${('-' + (maskNumero(total4).replace('-', '')))}</p>`);
                }
            });

        }

        function cargarInformacionModal(modal, nombreDetalle, rowData) {
            
            axios.post('GetEstadoResultadoDetalle', { listaEmpresas, fechaAnioMes: fecha, listaCC, tipoBusqueda: rowData.detalleID })
            .then(response => {
                if (response.data.success) {                        
                    if (rowData.detalleID == 1) { 
                        initTablaIngreso(); 
                        CargarGraficasIngresos(response.data.dataGraficas);
                    }
                    if (rowData.detalleID == 2) { 
                        initTablaCosto(); 
                        CargarGraficasCostos(response.data.dataGraficas);
                    }
                    if (rowData.detalleID == 3) { initTablaGasto(); }
                    AddRows($('#tabla' + nombreDetalle), response.data.data);
                    
                } 
                else {
                    AlertaGeneral(`Alerta`, response.data.message);
                    botonReporte.prop('disabled', true);
                }
            }).catch(error => {
                AlertaGeneral(`Alerta`, error.message);
                botonReporte.prop('disabled', true);
            });            

            $(modal).modal('show');
        }

        function cargarDatosPruebaDetalle() {
            AddRows(tablaIngreso, [
                { proyecto: 'MINERÍA A CIELO ABIERTO', mes: '1', acumulado: '1' },
                { proyecto: 'DIVISIÓN ALIMENTOS', mes: '2', acumulado: '2' },
                { proyecto: 'CONSTRUCCIÓN PESADA', mes: '3', acumulado: '3' },
                { proyecto: '', mes: '4', acumulado: '4', renglonGrupo: true }
            ]);
            AddRows(tablaCosto, [
                { concepto: 'MATERIALES P/CONSTRUCCIÓN', anioActualMes: '1', anioActualAcumulado: '1', anioAnteriorMes: '1', anioAnteriorAcumulado: '1' },
                { concepto: 'REFACCIONES PARA EQUIPOS', anioActualMes: '2', anioActualAcumulado: '2', anioAnteriorMes: '2', anioAnteriorAcumulado: '2' },
                { concepto: 'LLANTAS PARA EQUIPOS', anioActualMes: '3', anioActualAcumulado: '3', anioAnteriorMes: '3', anioAnteriorAcumulado: '3' },
                { concepto: '', anioActualMes: '4', anioActualAcumulado: '4', anioAnteriorMes: '4', anioAnteriorAcumulado: '4', renglonGrupo: true }
            ]);
            AddRows(tablaGasto, [
                { concepto: 'SEGUROS Y FINANZAS', anioActualMes: '1', anioActualAcumulado: '1', anioAnteriorMes: '1', anioAnteriorAcumulado: '1' },
                { concepto: 'HONORARIOS Y ASESORIAS', anioActualMes: '2', anioActualAcumulado: '2', anioAnteriorMes: '2', anioAnteriorAcumulado: '2' },
                { concepto: 'SERVICIOS', anioActualMes: '3', anioActualAcumulado: '3', anioAnteriorMes: '3', anioAnteriorAcumulado: '3' },
                { concepto: '', anioActualMes: '4', anioActualAcumulado: '4', anioAnteriorMes: '4', anioAnteriorAcumulado: '4', renglonGrupo: true }
            ]);
        }

        function initMonthPicker(input) {
            $(input).datepicker({
                dateFormat: "mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                maxDate: fechaActual,
                showAnim: showAnim,
                closeText: "Aceptar",
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                        $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                    }
                },
                beforeShow: function (input, inst) {
                    inst.dpDiv.addClass('month_year_datepicker')

                    if ((datestr = $(this).val()).length > 0) {
                        year = datestr.substring(datestr.length - 4, datestr.length);
                        month = datestr.substring(0, 2);
                        $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                        $(this).datepicker('setDate', new Date(year, month - 1, 1));
                        $(".ui-datepicker-calendar").hide();
                    }
                }
            }).datepicker("setDate", fechaActual);
        }

        function cargarEstadoResultados() {
            listaEmpresas = getValoresMultiples('#selectEmpresa');
            let mes = inputMes.val();
            let listaStringMes = mes.split('/');
            mesGlobal = parseInt(listaStringMes[0]) || 0;
            anioGLobal = parseInt(listaStringMes[1]) || 0
            fecha = '01' + '/' + listaStringMes[0] + '/' + listaStringMes[1];
            listaCC = getValoresMultiples('#selectCentroCosto');

            axios.post('CalcularEstadoResultados', { listaEmpresas, fechaAnioMes: fecha, listaCC })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        let mesNombre = listaMeses.filter((x) => x.mes == parseInt(listaStringMes[0]))[0].nombre;
                        let anio = parseInt(listaStringMes[1]);
                        let columnas = response.data.listaColumnas.map(x => {
                            return {
                                data: x.Item1,
                                tituloPrueba: x.Item2, // title: x.Item2,
                                render: x.Item3 == 1 ? funcionMonto : x.Item3 == 2 ? funcionPorcentaje : funcionConcepto //Tipo de columna
                            }
                        });

                        initTablaEstadoResultadosDinamica(columnas, response.data.listaDatosDataTable, listaEmpresas.length, mesNombre, anio);

                        botonReporte.prop('disabled', false);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                        botonReporte.prop('disabled', true);
                    }
                }).catch(error => {
                    AlertaGeneral(`Alerta`, error.message);
                    botonReporte.prop('disabled', true);
                });
        }

        function cargarReporte() {
            $.blockUI({ message: 'Generando reporte...' });
            var path = '/Reportes/Vista.aspx?idReporte=233';
            report.attr('src', path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function inicializarTablaDinamica(datos) {
            // let columnas = [{ data: 'ordenReporte' }, { data: 'concepto' }];

            // //#region Mes
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `montoMesAnioActual_${dato.empresaDesc}`,
            //         render: funcionMonto
            //     });
            // });
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `porcentajeMesAnioActual_${dato.empresaDesc}`,
            //         render: funcionPorcentaje
            //     });
            // });
            // //#endregion

            // //#region Mes Año Anterior
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `montoMesAnioAnterior_${dato.empresaDesc}`,
            //         render: funcionMonto
            //     });
            // });
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `porcentajeMesAnioAnterior_${dato.empresaDesc}`,
            //         render: funcionPorcentaje
            //     });
            // });
            // //#endregion

            // //#region Variaciones
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `variaciones_${dato.empresaDesc}`,
            //         render: funcionMonto
            //     });
            // });
            // //#endregion

            // //#region Acumulado Mes
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `montoMesAcumuladoAnioActual_${dato.empresaDesc}`,
            //         render: funcionMonto
            //     });
            // });
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `porcentajeMesAcumuladoAnioActual_${dato.empresaDesc}`,
            //         render: funcionPorcentaje
            //     });
            // });
            // //#endregion

            // //#region Acumulado Mes Año Anterior
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `montoMesAcumuladoAnioAnterior_${dato.empresaDesc}`,
            //         render: funcionMonto
            //     });
            // });
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `porcentajeMesAcumuladoAnioAnterior_${dato.empresaDesc}`,
            //         render: funcionPorcentaje
            //     });
            // });
            // //#endregion

            // //#region Acumulado Variaciones
            // datos.forEach(dato => {
            //     columnas.push({
            //         data: `variacionesAcumulado_${dato.empresaDesc}`,
            //         render: funcionMonto
            //     });
            // });
            // //#endregion

            initTablaEstadoResultadosDinamica(columnas);
        }

        function convertirMontoMillares(numeroInt) {
            return '$' + (Math.trunc(parseFloat(numeroInt))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        }

        function getMesAnio() {
            let listaMeses = [
                { mes: 1, nombre: 'ENERO' },
                { mes: 2, nombre: 'FEBRERO' },
                { mes: 3, nombre: 'MARZO' },
                { mes: 4, nombre: 'ABRIL' },
                { mes: 5, nombre: 'MAYO' },
                { mes: 6, nombre: 'JUNIO' },
                { mes: 7, nombre: 'JULIO' },
                { mes: 8, nombre: 'AGOSTO' },
                { mes: 9, nombre: 'SEPTIEMBRE' },
                { mes: 10, nombre: 'OCTUBRE' },
                { mes: 11, nombre: 'NOVIEMBRE' },
                { mes: 12, nombre: 'DICIEMBRE' }
            ];
            let mes = inputMes.val();
            let listaStringMes = mes.split('/');
            let mesNombre = listaMeses.filter((x) => x.mes == parseInt(listaStringMes[0]))[0].nombre;
            let anio = parseInt(listaStringMes[1]);

            return { mesNombre, anio };
        }

        function CargarGraficasIngresos(data)
        {            
            let listaMeses = ['ENERO', 'FEBRERO', 'MARZO', 'ABRIL', 'MAYO','JUNIO', 'JULIO', 'AGOSTO', 'SEPTIEMBRE', 'OCTUBRE', 'NOVIEMBRE', 'DICIEMBRE'];
            listaMeses = listaMeses.slice(0, mesGlobal);
            let listaConceptos = $.map(data, function(valor) { return valor.concepto; }).filter(function(valor, index, arreglo) { return index === arreglo.indexOf(valor); });

            let seriesColumnasFinal = [];
            for (var i = 1; i <= listaMeses.length; i++) {
                var auxSerie = $.grep(data, function(valor, index) { return valor.mes == i; });

                let auxDatosSerie = [];
                for(var j = 0; j < listaConceptos.length; j++)
                {
                    var auxDatoObjeto = $.grep(auxSerie, function(valor, index) { return valor.concepto == listaConceptos[j]; });
                    var auxDatoMonto = $.map(auxDatoObjeto, function(valor) { return valor.monto; }).reduce((pv,cv) => { return pv + (parseFloat(cv) || 0); }, 0);
                    auxDatosSerie.push(auxDatoMonto * (-1));
                }
                let serie = { name: listaMeses[i - 1], data: auxDatosSerie, visible: 'true' };
                seriesColumnasFinal.push(serie)
            }            
            grGraficaIngresoColumna = Highcharts.chart('graficaIngresoColumnas', {
                chart: { type: 'column' },
                title: { text: '' },
                xAxis: {
                    categories: listaConceptos,
                    crosshair: true
                },
                yAxis: {
                    title: { text: '$ (MNX)' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>${point.y:.1f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: seriesColumnasFinal,
                credits: { enabled: false }
            });
            let seriesLineasFinal = [];
            let auxDatosSerie = [];

            for(var j = 1; j <= listaMeses.length; j++)
            {
                var auxDatoObjeto = $.grep(data, function(valor, index) { return valor.mes == j; });
                var auxDatoMonto = $.map(auxDatoObjeto, function(valor) { return valor.monto; }).reduce((pv,cv) => { return pv + (parseFloat(cv) || 0); }, 0);
                auxDatosSerie.push(auxDatoMonto * (-1));
            }

            if (listaMeses.length == 1) {
                let nuevoMeses = ["inicio", "ENERO"];
                listaMeses = nuevoMeses;
                let valorEnero = auxDatosSerie[0];
                auxDatosSerie = new Array();
                auxDatosSerie.push(0);
                auxDatosSerie.push(valorEnero);
            }
          
            let serie = { 
                name: "TOTAL", 
                data: auxDatosSerie, 
                visible: 'true', 
                regression: true,
                tooltip: {
                    valueDecimals: 2
                },
                regressionSettings: {
                    type: 'linear',
                    //color:  '#1111cc',
                    dashStyle: "shortdash",
                    name: "Tendencia"
                },
            };
            seriesLineasFinal.push(serie)
            
            grGraficaIngresoLineas = Highcharts.chart('graficaIngresoLineas', {
                title: { text: '' },
                yAxis: { title: { text: '$ (MNX)' } },
                xAxis: { categories: listaMeses },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                plotOptions: { series: { label: { connectorAllowed: false } }
                },
                series: seriesLineasFinal,
                responsive: {
                    rules: [{
                        condition: { maxWidth: 500 },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                },  
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>${point.y:.1f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                credits: { enabled: false }
            });            
        }
        function CargarGraficasCostos(data)
        {            
            let listaMeses = ['ENERO', 'FEBRERO', 'MARZO', 'ABRIL', 'MAYO','JUNIO', 'JULIO', 'AGOSTO', 'SEPTIEMBRE', 'OCTUBRE', 'NOVIEMBRE', 'DICIEMBRE'];
            listaMeses = listaMeses.slice(0, mesGlobal);
            let listaConceptos = $.map(data, function(valor) { return valor.concepto; }).filter(function(valor, index, arreglo) { return index === arreglo.indexOf(valor); });

            let seriesColumnasFinal = [];
            for (var i = 1; i <= listaMeses.length; i++) {
                var auxSerie = $.grep(data, function(valor, index) { return valor.mes == i; });

                let auxDatosSerie = [];
                for(var j = 0; j < listaConceptos.length; j++)
                {
                    var auxDatoObjeto = $.grep(auxSerie, function(valor, index) { return valor.concepto == listaConceptos[j]; });
                    var auxDatoMonto = $.map(auxDatoObjeto, function(valor) { return valor.monto; }).reduce((pv,cv) => { return pv + (parseFloat(cv) || 0); }, 0);
                    auxDatosSerie.push(auxDatoMonto);
                }
                let serie = { name: listaMeses[i - 1], data: auxDatosSerie, visible: 'true' };
                seriesColumnasFinal.push(serie)
            }            
            grGraficaCostoColumna = Highcharts.chart('graficaCostoColumnas', {
                chart: { type: 'column' },
                title: { text: '' },
                xAxis: {
                    categories: listaConceptos,
                    crosshair: true
                },
                yAxis: {
                    title: { text: '$ (MNX)' }
                },
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>${point.y:.1f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0
                    }
                },
                series: seriesColumnasFinal,
                credits: { enabled: false }
            });
            let seriesLineasFinal = [];
            let auxDatosSerie = [];
            
            for(var j = 1; j <= listaMeses.length; j++)
            {
                var auxDatoObjeto = $.grep(data, function(valor, index) { return valor.mes == j; });
                var auxDatoMonto = $.map(auxDatoObjeto, function(valor) { return valor.monto; }).reduce((pv,cv) => { return pv + (parseFloat(cv) || 0); }, 0);
                auxDatosSerie.push(auxDatoMonto);
            }
            
            if (listaMeses.length == 1) {
                let nuevoMeses = ["inicio", "ENERO"];
                listaMeses = nuevoMeses;
                let valorEnero = auxDatosSerie[0];
                auxDatosSerie = new Array();
                auxDatosSerie.push(0);
                auxDatosSerie.push(valorEnero);
            }

            let serie = { 
                name: "TOTAL", 
                data: auxDatosSerie, 
                visible: 'true', 
                regression: true,
                regressionSettings: {
                    color:  'rgba(223, 83, 83, .9)',
                    name: "Tendencia",
                    tooltip: {
                        enabled: false
                    },
                }, 
            };
            seriesLineasFinal.push(serie)
            
            grGraficaCostoLineas = Highcharts.chart('graficaCostoLineas', {
                title: { text: '' },
                yAxis: { title: { text: '$ (MNX)' } },
                xAxis: { categories: listaMeses },
                legend: {
                    align: 'center',
                    verticalAlign: 'bottom',
                    layout: 'vertical',
                },
                plotOptions: { series: { label: { connectorAllowed: false } }
                },
                series: seriesLineasFinal,
                responsive: {
                    rules: [{
                        condition: { maxWidth: 500 },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                },  
                tooltip: {
                    headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                    pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                        '<td style="padding:0"><b>${point.y:.1f}</b></td></tr>',
                    footerFormat: '</table>',
                    shared: true,
                    useHTML: true
                },                
                credits: { enabled: false }
            });    
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Contabilidad.EstadoResultados = new EstadoResultados())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();