(() => {
    $.namespace('CuadroComparativo.OrdenCompra.DashboardCalificaciones');
    DashboardCalificaciones = function () {
        //#region selectores
        //Filtro
        const txtFechaInicio = $('#txtFechaInicio');
        const txtFechaFinal = $('#txtFechaFinal');
        const cboProveedores = $('#cboProveedores');
        const cboCompradores = $('#cboCompradores');
        const btnFiltrar = $('#btnFiltrar');

        //Graficas
        const gpxPastel_OptimoVsNoOptimo = $('#gpxPastel_OptimoVsNoOptimo');
        const gpxPastel_Top10ProvOptimos = $('#gpxPastel_Top10ProvOptimos');
        const gpxPastel_Top10ProvNoOptimos = $('#gpxPastel_Top10ProvNoOptimos');
        //#endregion

        //#region Modales
        const mdlDetalleGpxPastel = $('#mdlDetalleGpxPastel');
        const mdlDetalleGpxPastelTitulo = $('#mdlDetalleGpxPastelTitulo');
        const tblDetalleGpxPastel = $('#tblDetalleGpxPastel');

        const mdlDetalleTop10ProvOptimos = $('#mdlDetalleTop10ProvOptimos');
        const tblDetalleTop10ProvOptimos = $('#tblDetalleTop10ProvOptimos');

        const mdlDetalleTop10ProvNoOptimos = $('#mdlDetalleTop10ProvNoOptimos');
        const tblDetalleTop10ProvNoOptimos = $('#tblDetalleTop10ProvNoOptimos');

        const mdlDetalleCalificaciones = $('#mdlDetalleCalificaciones');
        const tblDetalleCalificaciones = $('#tblDetalleCalificaciones');
        //#endregion

        let detalleNoOptimos = [];
        let detalleOptimos = [];
        let detalleTop10ProveedoresOptimos = [];
        let detalleTop10ProveedoresNoOptimos = [];
        let detalleCalificaciones = [];

        //#region eventos
        btnFiltrar.on('click', function () {
            ConsultaDashboard();
        });

        mdlDetalleGpxPastel.on('shown.bs.modal', function () {
            tblDetalleGpxPastel.DataTable().columns.adjust().draw();
        });

        mdlDetalleTop10ProvOptimos.on('shown.bs.modal', function () {
            tblDetalleTop10ProvOptimos.DataTable().columns.adjust().draw();
        });

        mdlDetalleTop10ProvNoOptimos.on('shown.bs.modal', function () {
            tblDetalleTop10ProvNoOptimos.DataTable().columns.adjust().draw();
        });

        mdlDetalleCalificaciones.on('shown.bs.modal', function () {
            tblDetalleCalificaciones.DataTable().columns.adjust().draw();
        });

        txtFechaInicio.attr("autocomplete", "off");
        txtFechaFinal.attr("autocomplete", "off");
        //#endregion

        (function init() {
            initFechas();
            initGpxPastel_OptimoVsNoOptimo(null);
            initGpxPastel_Top10ProvOptimos(null);
            initGpxPastel_Top10ProvNoOptimos(null);
            initGpxPastel_Calificaciones(null);
            initGpxBarras_ProvNoOptimos(null);
            initGpxBarras_Compradores(null);
            initTablaDetalle();
            initTablaDetalleTop10ProveedoresOptimos();
            initTablaDetalleTop10ProveedoresNoOptimos();
            initTablaDetalleCalificaciones();
            Listener();
        })();

        function Listener() {
            cboProveedores.fillCombo('FillCboProveedores', null, false, "Todos");
            convertToMultiselect('#cboProveedores');

            cboCompradores.fillCombo('FillCboCompradores', null, false, "Todos");
            convertToMultiselect('#cboCompradores');
        }

        function initFechas() {
            txtFechaInicio.datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: '2021:2050'
            }).datepicker('setDate', new Date());

            txtFechaFinal.datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: '2021:2050'
            }).datepicker('setDate', new Date());
        }

        //#region graficas
        function initGpxPastel_OptimoVsNoOptimo(datos) {
            Highcharts.chart('gpxPastel_OptimoVsNoOptimo', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    pltoShadow: false,
                    type: 'pie'
                },
                title: {
                    text: ''
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            formatter: function () {
                                return this.point.name + ': ' + this.point.y + '%';
                            }
                            // enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Valor',
                    colorByPoint: true,
                    data: datos,
                    events: {
                        click: function (event) {
                            //#region SE OBTIENE EL DETALLE
                            // function fncGetDetallesProveedoresOptimosVsNoOptimos() {
                            let obj = new Object();
                            obj.fechaInicio = moment(txtFechaInicio.val(), 'DD/MM/YYYY').toISOString(true);
                            obj.fechaFin = moment(txtFechaFinal.val(), 'DD/MM/YYYY').toISOString(true);
                            obj.proveedores = cboProveedores.val();
                            obj.compradores = cboCompradores.val();
                            axios.post("GetDetallesProveedoresOptimosVsNoOptimos", obj).then(response => {
                                let { success, items, message } = response.data;
                                if (success) {
                                    //#region FILL DATATABLE
                                    AddRows(tblDetalleGpxPastel, items);
                                    let titulo = event.point.options.name == 'Optimas' ? 'Compras optimas' : 'Compras no optimas';
                                    mdlDetalleGpxPastel.modal('show');
                                    mdlDetalleGpxPastelTitulo.html(titulo);
                                    //#endregion
                                } else {
                                    Alert2Error(message);
                                }
                            }).catch(error => Alert2Error(error.message));
                            // }
                            //#endregion
                            // AddRows(tblDetalleGpxPastel, event.point.options.name == 'Optimas' ? detalleOptimos : detalleNoOptimos);
                            // let titulo = event.point.options.name == 'Optimas' ? 'Compras optimas' : 'Compras no optimas';
                            // mdlDetalleGpxPastel.modal('show');
                            // mdlDetalleGpxPastelTitulo.html(titulo);
                        }
                    }
                }],
                credits: {
                    enabled: false
                }
            });
        }

        function initGpxPastel_Top10ProvOptimos(datos) {
            Highcharts.chart('gpxPastel_Top10ProvOptimos', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    pltoShadow: false,
                    type: 'pie'
                },
                title: {
                    text: ''
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            formatter: function () {
                                return this.point.name + ': ' + this.point.y;
                            }
                        },
                        // dataLabels: {
                        //     enabled: true
                        // },
                        showInLegend: false
                    }
                },
                series: [{
                    name: 'Valor',
                    colorByPoint: true,
                    data: datos,
                    events: {
                        click: function (event) {
                            let obj = new Object();
                            obj.fechaInicio = moment(txtFechaInicio.val(), 'DD/MM/YYYY').toISOString(true);
                            obj.fechaFin = moment(txtFechaFinal.val(), 'DD/MM/YYYY').toISOString(true);
                            obj.proveedores = cboProveedores.val();
                            obj.compradores = cboCompradores.val();
                            axios.post("GetDetallesTop10ProvOptimos", obj).then(response => {
                                let { success, items, message } = response.data;
                                if (success) {
                                    //#region FILL DATATABLE
                                    AddRows(tblDetalleTop10ProvOptimos, items);
                                    mdlDetalleTop10ProvOptimos.modal("show");
                                    //#endregion
                                } else {
                                    Alert2Error(message);
                                }
                            }).catch(error => Alert2Error(error.message));
                            // AddRows(tblDetalleTop10ProvOptimos, detalleTop10ProveedoresOptimos);
                            // mdlDetalleTop10ProvOptimos.modal("show");
                        }
                    }
                }],
                credits: {
                    enabled: false
                }
            });
        }

        function initGpxPastel_Top10ProvNoOptimos(datos) {
            Highcharts.chart('gpxPastel_Top10ProvNoOptimos', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    pltoShadow: false,
                    type: 'pie'
                },
                title: {
                    text: ''
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:1f}%</b>'
                },
                accessibility: {
                    point: {
                        valueSuffix: '%'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            formatter: function () {
                                return this.point.name + ': ' + this.point.y;
                            }
                        },
                        // dataLabels: {
                        //     enabled: true
                        // },
                        showInLegend: false
                    }
                },
                series: [{
                    name: 'Valor',
                    colorByPoint: true,
                    data: datos,
                    events: {
                        click: function (event) {
                            // function fncGetDetallesProveedoresOptimosVsNoOptimos() {
                            let obj = new Object();
                            obj.fechaInicio = moment(txtFechaInicio.val(), 'DD/MM/YYYY').toISOString(true);
                            obj.fechaFin = moment(txtFechaFinal.val(), 'DD/MM/YYYY').toISOString(true);
                            obj.proveedores = cboProveedores.val();
                            obj.compradores = cboCompradores.val();
                            axios.post("GetDetallesTop10ProvNoOptimos", obj).then(response => {
                                let { success, items, message } = response.data;
                                if (success) {
                                    //#region FILL DATATABLE
                                    AddRows(tblDetalleTop10ProvNoOptimos, items);
                                    mdlDetalleTop10ProvNoOptimos.modal("show");
                                    //#endregion
                                } else {
                                    Alert2Error(message);
                                }
                            }).catch(error => Alert2Error(error.message));
                            // }
                            // AddRows(tblDetalleTop10ProvNoOptimos, detalleTop10ProveedoresNoOptimos);
                            // mdlDetalleTop10ProvNoOptimos.modal("show");
                        }
                    }
                }],
                credits: {
                    enabled: false
                }
            });
        }

        function initGpxPastel_Calificaciones(lstComprasOptimas, lstComprasMedias, lstComprasNoOptimas) {
            let lstNombres = [];
            lstNombres.push("Optimas");
            lstNombres.push("Medias");
            lstNombres.push("No optimas");

            let lstValores = [];
            lstValores.push(lstComprasOptimas);
            lstValores.push(lstComprasMedias);
            lstValores.push(lstComprasNoOptimas);

            let objData = new Object();
            let lstData = [];
            for (let i = 0; i < 3; i++) {
                objData = {
                    name: lstNombres[i],
                    y: parseFloat(lstValores[i]),
                    sliced: true,
                    selected: true
                }
                lstData.push(objData);
            }
            Highcharts.chart('gpxPastel_Calificaciones', {
                chart: {
                    type: 'pie'
                },
                title: {
                    text: ''
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                // xAxis: {
                //     categories: ['Optimas', 'Medias', 'No optimas'],
                //     labels: {
                //         x: -10
                //     }
                // },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                series: [{
                    name: "Valor",
                    data: lstData,
                    events: {
                        click: function (event) {
                            let obj = new Object();
                            obj.fechaInicio = moment(txtFechaInicio.val(), 'DD/MM/YYYY').toISOString(true);
                            obj.fechaFin = moment(txtFechaFinal.val(), 'DD/MM/YYYY').toISOString(true);
                            obj.proveedores = cboProveedores.val();
                            obj.compradores = cboCompradores.val();
                            axios.post("GetDetallesCalificaciones", obj).then(response => {
                                let { success, items, message } = response.data;
                                if (success) {
                                    //#region FILL DATATABLE
                                    AddRows(tblDetalleCalificaciones, items);
                                    mdlDetalleCalificaciones.modal("show");
                                    //#endregion
                                } else {
                                    Alert2Error(message);
                                }
                            }).catch(error => Alert2Error(error.message));
                            // AddRows(tblDetalleCalificaciones, detalleCalificaciones);
                            // mdlDetalleCalificaciones.modal("show");
                        }
                    }
                }],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                align: 'center',
                                verticalAlign: 'bottom',
                                layout: 'horizontal'
                            },
                            yAxis: {
                                labels: {
                                    align: 'left',
                                    x: 0,
                                    y: -5
                                },
                                title: {
                                    text: null
                                }
                            },
                            subtitle: {
                                text: null
                            },
                            credits: {
                                enabled: false
                            }
                        }
                    }]
                },
                credits: {
                    enabled: false
                }
            });
        }

        function initGpxBarras_ProvNoOptimos(datos) {
            Highcharts.chart('gpxBarras_ProvNoOptimos', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                xAxis: {
                    categories: datos == null ? "" : datos[0].categorias,
                    crosshair: true
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                series: [{
                    name: datos == null ? "" : datos[0].serie1Descripcion,
                    data: datos == null ? "" : datos[0].serie1
                }, {
                    name: datos == null ? "" : datos[0].serie2Descripcion,
                    data: datos == null ? "" : datos[0].serie2
                }, {
                    name: datos == null ? "" : datos[0].serie3Descripcion,
                    data: datos == null ? "" : datos[0].serie3
                }],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                align: 'center',
                                verticalAlign: 'bottom',
                                layout: 'horizontal'
                            },
                            yAxis: {
                                labels: {
                                    align: 'left',
                                    x: 0,
                                    y: -5
                                },
                                title: {
                                    text: null
                                }
                            },
                            subtitle: {
                                text: null
                            },
                            credits: {
                                enabled: false
                            }
                        }
                    }]
                },
                credits: {
                    enabled: false
                }
            });
        }

        function initGpxBarras_Compradores(datos) {
            Highcharts.chart('gpxBarras_Compradores', {
                chart: {
                    type: 'column'
                },
                title: {
                    text: ''
                },
                legend: {
                    align: 'right',
                    verticalAlign: 'middle',
                    layout: 'vertical'
                },
                xAxis: {
                    categories: datos == null ? "" : datos[0].categorias,
                    crosshair: true
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: ''
                    }
                },
                series: [{
                    name: datos == null ? "" : datos[0].serie1Descripcion,
                    data: datos == null ? "" : datos[0].serie1
                }, {
                    name: datos == null ? "" : datos[0].serie2Descripcion,
                    data: datos == null ? "" : datos[0].serie2
                }, {
                    name: datos == null ? "" : datos[0].serie3Descripcion,
                    data: datos == null ? "" : datos[0].serie3
                }],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                align: 'center',
                                verticalAlign: 'bottom',
                                layout: 'horizontal'
                            },
                            yAxis: {
                                labels: {
                                    align: 'left',
                                    x: 0,
                                    y: -5
                                },
                                title: {
                                    text: null
                                }
                            },
                            subtitle: {
                                text: null
                            },
                            credits: {
                                enabled: false
                            }
                        }
                    }]
                },
                credits: {
                    enabled: false
                }
            });
        }
        //#endregion

        //#region tablas
        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function initTablaDetalle() {
            // destroy: false,
            //     paging: false,
            //         ordering: false,
            //             searching: false,
            //                 bFilter: false,
            tblDetalleGpxPastel.DataTable({
                ordering: true,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: true,
                // scrollX: true,
                // scrollY: '45vh',
                scrollCollapse: true,
                // lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'NumeroOrdenCompra', title: 'Orden compra' },
                    { data: 'CC', title: 'CC' },
                    { data: 'NumeroRequisicion', title: 'Requisición' },
                    { data: 'Folio', title: 'Folio' },
                    { data: 'NumeroProveedor', title: 'Proveedor' },
                    // { data: 'NombreProveedor', title: 'Nombre del proveedor' },
                    { data: 'Calificacion.Calificacion', title: 'Calificación' }
                ],

                columnDefs: [
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            return row.NumeroOrdenCompra == 0 ? '' : data;
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                    // $(row).find('td').eq(0).addClass('text-center');
                    // $(row).find('td').eq(0).nextAll().addClass('text-right');
                },

                drawCallback: function (settings) {
                },

                headerCallback: function (thead, data, start, end, display) {
                    // $(thead).addClass('bg-table-header');
                },

                footerCallback: function (tfoot, data, start, end, display) {
                    // $(tfoot).find('th').addClass('text-right');
                    // $(tfoot).find('th').eq(8).html(maskNumero(totalesTabulador.SumaLibros));
                }
            });
        }

        function initTablaDetalleTop10ProveedoresOptimos() {
            tblDetalleTop10ProvOptimos.DataTable({
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollCollapse: true,
                columns: [
                    { data: 'proveedor', title: 'Proveedor' },
                    { data: 'cantOC', title: 'Cant. Compras' },
                ],
                columnDefs: [{ targets: [0], }],
            });
        }

        function initTablaDetalleTop10ProveedoresNoOptimos() {
            tblDetalleTop10ProvNoOptimos.DataTable({
                ordering: true,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: true,
                scrollCollapse: true,
                columns: [
                    { data: 'proveedor', title: 'Proveedor' },
                    { data: 'cantOC', title: 'Cant. Compras' },
                ],
                columnDefs: [{ targets: [0], }],
            });
        }

        function initTablaDetalleCalificaciones() {
            tblDetalleCalificaciones.DataTable({
                ordering: true,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: true,
                scrollCollapse: true,
                columns: [
                    { data: 'oc', title: 'Orden compra' },
                    { data: 'requisicion', title: 'Requisición' },
                    { data: 'cc', title: 'CC' },
                    { data: 'proveedor', title: 'Proveedor' },
                    {
                        data: "fecha", title: "Fecha",
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    }
                ],
                columnDefs: [{ targets: [0], }],
            });
        }
        //#endregion

        //#region server
        function ConsultaDashboard() {
            let filtro = fncValidarFiltro();
            if (filtro) {
                let fechaInicio = moment(txtFechaInicio.val(), 'DD/MM/YYYY').toISOString(true);
                let fechaFinal = moment(txtFechaFinal.val(), 'DD/MM/YYYY').toISOString(true);
                $.post('/Enkontrol/OrdenCompra/ConsultaDashboard',
                    {
                        fechaInicio: fechaInicio,
                        fechaFin: fechaFinal,
                        proveedores: cboProveedores.val(),
                        compradores: cboCompradores.val()
                    }).then(response => {
                        if (response.success) {
                            console.log(response);
                            initGpxPastel_OptimoVsNoOptimo(response.seriesOptimoVsNoOptimo);
                            initGpxPastel_Top10ProvOptimos(response.seriesTop10ProvOptimos);
                            initGpxPastel_Top10ProvNoOptimos(response.seriesTop10ProvNoOptimos);
                            initGpxBarras_ProvNoOptimos(response.lstGpx_Proveedores);
                            initGpxPastel_Calificaciones(response.lstComprasOptimas, response.lstComprasMedias, response.lstComprasNoOptimas);
                            initGpxBarras_Compradores(response.lstGpx_Compradores);

                            // detalleNoOptimos = response.pastel_noOptimoDetalle;
                            // detalleTop10ProveedoresOptimos = response.lstDetalleTop10ProveedoresOptimos;
                            // detalleOptimos = response.pastel_optimaDetalle;
                            // detalleTop10ProveedoresNoOptimos = response.lstDetalleTop10ProveedoresNoOptimos;
                            // detalleCalificaciones = response.lstDetalleCalificaciones;
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    });
            }
        }

        function fncValidarFiltro() {
            let filtro = true;
            let strMensajeError = "";

            txtFechaInicio.val() != "" ? true : strMensajeError += "Es necesario seleccionar fecha de inicio.";
            txtFechaFinal.val() != "" ? true : strMensajeError += "<br>Es necesario seleccionar fecha final.";
            cboProveedores.val() != "" ? true : strMensajeError += "<br>Es necesario seleccionar al menos un proveedor.";
            cboCompradores.val() != "" ? true : strMensajeError += "<br>Es necesario seleccionar al menos un comprador.";

            if (strMensajeError != "") {
                Alert2Warning(strMensajeError);
                filtro = false;
            }

            return filtro;
        }
        //#endregion
    }

    $(document).ready(() => CuadroComparativo.OrdenCompra.DashboardCalificaciones = new DashboardCalificaciones())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();