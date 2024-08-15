(function () {
    $.namespace('CuentasPorCobrar.CuentasPorCobrar.ControlEstimaciones');
    controlestimaciones = function () {
        const comboAC = $('#comboAC');
        const cbTipoCorte = $('#cbTipoCorte');
        const inputCorte = $('#inputCorte');
        const botonBuscar = $('#botonBuscar');
        const comboDivision = $("#comboDivision");
        const comboResponsable = $("#comboResponsable");
        const botonDescargarExcel = $('#botonDescargarExcel');
        var lstDetalle = [];

        const getLstCXC = new URL(window.location.origin + '/CuentasPorCobrar/CuentasPorCobrar/getCXC');

        const getLstCC = new URL(window.location.origin + '/CuentasPorCobrar/CuentasPorCobrar/getLstCC');
        const checkResponsable = new URL(window.location.origin + '/CuentasPorCobrar/CuentasPorCobrar/checkResponsable');
        const comboCC = $('#comboCC');

        let fechaCorteGeneral = new Date();

        const tablaCXC = $("#tablaCXC");
        let dtTablaCXC;

        const tablaCXCFacturas = $("#tablaCXCFacturas");
        let dtTablaCXCFacturas;

        const tablaCXCAC = $("#tablaCXCAC");
        let dtTablaCXCAC;

        const titleModalCXCFacturas = $('#titleModalCXCFacturas');

        const tblCXCCc = $('#tblCXCCc');
        let dtCXCCc;

        const modalCXCClientes = $('#modalCXCClientes');
        const titleModalCXCClientes = $('#titleModalCXCClientes');
        const titleClienteModalCXCClientes = $('#titleClienteModalCXCClientes');
        const titleClienteModalCXCClientes2 = $('#titleClienteModalCXCClientes2');
        const tblCXCCcClientes = $('#tblCXCCcClientes');
        let dtCXCCcClientes

        //#region FILTROS
        const chkCETipoTabla = $('#chkCETipoTabla');

        const divShowCC = $('#divShowCC');
        const divShowClientes = $('#divShowClientes');
        //#endregion

        function init() {
            setResponsable();

            inputCorte.datepicker().datepicker('setDate', new Date());

            fillCombos();
            agregarListeners();
            initTablaCXC();
            initTablaCXCFacturas();
            initTablaCXCAC();
            initTblCXCCc();
            initTblCXCCcClientes();

            //OCULTAR UNO DE LOS DOS DIVS
            divShowClientes.hide();
        }

        function agregarListeners() {
            botonBuscar.click(function (e) {
                setLstCXC(e);
            });
            comboDivision.change(cargarAC);
            comboResponsable.change(cargarAC);

            comboAC.next(".select2-container").css("display", "none");
            $("#spanComboAC").click(function (e) {
                comboAC.next(".select2-container").css("display", "block");
                comboAC.siblings("span").find(".select2-selection__rendered")[0].click();
            });
            comboAC.on('select2:close', function (e) {
                comboAC.next(".select2-container").css("display", "none");
                var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
                if (seleccionados.length == 0) $("#spanComboAC").text("TODOS");
                else {
                    if (seleccionados.length == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                    else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
                }
            });
            comboAC.on("select2:unselect", function (evt) {
                if (!evt.params.originalEvent) { return; }
                evt.params.originalEvent.stopPropagation();
            });


            $("#modalCXCFacturas").on('shown.bs.modal', function (e) {
                dtTablaCXCFacturas.columns.adjust();
            });
            $("#botonCerrarCXCFacturas").click(function (e) {
                $("#modalCXCFacturas").modal("hide");
            });
            $("#modalCXCAC").on('shown.bs.modal', function (e) {
                dtTablaCXCAC.columns.adjust();
            });
            $("#botonCerrarCXCAC").click(function (e) {
                $("#modalCXCAC").modal("hide");
            });

            chkCETipoTabla.on("change", function () {
                if ($(this).prop("checked")) {
                    divShowCC.show();
                    divShowClientes.hide();
                } else {
                    divShowCC.hide();
                    divShowClientes.show();
                    dtTablaCXC.columns.adjust();
                }
            });

            botonDescargarExcel.on('click', function () {
                location.href = `DescargarExcelCXC`;
            });
        }

        function fillCombos() {
            comboAC.select2({ closeOnSelect: false });
            // comboDivision.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboDivision', {}, false, "TODAS");
            comboResponsable.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboResponsable', {}, false, "TODOS");

            comboAC.fillCombo('cboObraEstimados', null, false, "TODOS");
            comboAC.find('option').get(0).remove();

            comboCC.multiselect();
            comboCC.multiselect('disable');

            // comboDivision.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboDivision', {}, false, "TODOS");
            comboResponsable.fillCombo('/CuentasPorCobrar/CuentasPorCobrar/fillComboResponsable', {}, false, "TODOS");
        }

        function setResponsable() {
            $.post(checkResponsable, { responsableID: comboResponsable.val() })
                .then(function (response) {
                    if (response.success) {
                        responsable = response.responsable;
                    } else {
                        // Operación no completada.
                    }
                }, function (error) {
                    // Error al lanzar la petición.
                }
                );
        }

        function setLstCXC(e) {
            e.preventDefault();
            e.stopPropagation();
            e.stopImmediatePropagation();
            var notSelected = $("#comboAC").find('option').not(':selected');
            var array = notSelected.map(function () {
                return this.value;
            }).get();

            var array2 = [];
            areasCuentaGlobal = comboAC.val().length > 0 ? comboAC.val() : ((responsable || comboDivision.val().length > 0) ? array : array2);

            if (areasCuentaGlobal.length == 1 && areasCuentaGlobal[0] == "TODOS") {
                areasCuentaGlobal = [];
            }

            $('#tablaKubrixDetalle tbody td').addClass("blurry");
            $.post(getLstCXC, {
                fecha: inputCorte.val(),
                areaCuenta: areasCuentaGlobal,
            })
                .then(function (response) {
                    //$.unblockUI;
                    if (response.success) {
                        //CXC
                        var detallesCXC = response.CXC;
                        var totalCXC = 0;
                        $.each(detallesCXC, function (i, n) { totalCXC += n.monto; });
                        $(".pCXC").text(parseFloat(totalCXC / 1000).toFixed(2));
                        fechaCorteGeneral = inputCorte.datepicker('getDate');

                        cargarTablaCXC(response.CXC, fechaCorteGeneral);
                        cargarTablaCXCCc(response.CXC, fechaCorteGeneral);

                        botonDescargarExcel.css('display', response.CXC.length > 0 ? 'inline-block' : 'none');
                    } else {
                        // Operación no completada.
                        AlertaGeneral('Operación fallida', 'No se pudo completar la operación: ' + response.message);
                    }
                }, function (error) {
                    //$.unblockUI;
                    // Error al lanzar la petición.
                    AlertaGeneral('Operación fallida', 'Ocurrió un error al lanzar la petición al servidor: Error ' + error.status + ' - ' + error.statusText + '.');
                }
                );
        }

        function initTablaCXC() {
            dtTablaCXC = tablaCXC.DataTable({
                dom: 'B<lfr>tip',
                buttons: [ 
                    'excelHtml5', 
                ],
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verCC' data-cliente='" + row.cliente + "'><i class='fas fa-layer-group'></i></button>"; } },
                    // { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verFacturas' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'cliente', title: 'Cliente' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { title: 'Pagado', render: function () { return getNumberHTML("0.00"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    {
                        data: 'porcentaje',
                        title: '%',
                        render: function (data, type, row) {
                            return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                drawCallback: function () {
                    tablaCXC.find('button.verFacturas').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaCXC.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCFacturas(detalles, fechaCorteGeneral, null);
                        $("#modalCXCFacturas").modal("show");

                    });
                    tablaCXC.find('button.verCC').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        const rowData = dtTablaCXC.row($(this).parents('tr')).data();
                        var detalles = rowData.detalles;
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCAC(detalles, fechaCorteGeneral);
                        titleClienteModalCXCClientes.text(rowData.cliente);
                        titleClienteModalCXCClientes2.text(rowData.cliente);

                        $("#modalCXCAC").modal("show");

                    });
                },
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = (api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0)) / 1000;
                    totalVencido15 = (api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0)) / 1000;
                    totalVencido30 = (api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0)) / 1000;
                    totalVencido60 = (api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0)) / 1000;
                    totalVencido90 = (api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0)) / 1000;
                    totalVencidoMas = (api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0)) / 1000;
                    // pronostico = (api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0)) / 1000;
                    //programado = (api.column(10).data().reduce( function (a, b) { return intVal(a) + intVal(b); }, 0 )) / 1000;
                    total = (api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0)) / 1000;
                    totalPorcentaje = (api.column(9).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0));
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    // $(api.column(8).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    // $(api.column(9).footer()).html('$0.00');
                    $(api.column(8).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                    $(api.column(9).footer()).html(parseFloat(totalPorcentaje).toFixed(2).toString() + '%');
                }
            }).columns.adjust();
        }

        function cargarTablaCXC(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }
            else {
                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function (i, n) { totalDetalles += n.monto; });
                const grouped = groupBy(detalles, function (detalle) { return detalle.responsable; });
                dtTablaCXC.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const cliente = value[0].responsable;
                    var total = 0;
                    var detallesFiltrados = value;
                    //if(areasCuentaDetalle.length > 0) {
                    //    detallesFiltrados = $.grep(detallesFiltrados,function(el,index){ 
                    //        return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                    //    });
                    //}
                    $.each(detallesFiltrados, function (i, n) { total += n.monto; });
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function (i, n) { porVencer += n.monto; });
                    const vencido15Detalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function (i, n) { vencido15 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0) $.each(vencido30Detalles, function (i, n) { vencido30 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function (i, n) { vencido60 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function (i, n) { vencido90 += n.monto; });
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function (i, n) { vencidoMas += n.monto; });
                    const pronosticoDetalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    let valPronostico = 0;
                    $.each(detallesFiltrados, function (i, n) { valPronostico += n.montoPronosticado; });
                    // console.log(value);
                    const grupo = { cliente: cliente, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, montoPronosticado: valPronostico, total: total, porcentaje: porcentaje, detalles: value };
                    auxDatosFinales.push(grupo);
                });
                auxDatosFinales.sort((a, b) => b.total - a.total);
                dtTablaCXC.rows.add(auxDatosFinales);
                dtTablaCXC.draw();
                return true;
            }
        }

        function initTablaCXCFacturas() {
            dtTablaCXCFacturas = tablaCXCFacturas.DataTable({
                buttons: [ 
                    'excelHtml5', 
                ],
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<Bf<t>>',
                columns: [
                    { data: 'factura', title: 'Factura' },
                    { data: 'concepto', title: 'Concepto' },
                    { data: 'fecha', title: 'Fecha', render: function (data, type, row) { return moment(data).toDate().toLocaleDateString('en-GB').Capitalize() } },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, type, row) { return maskNumero(parseFloat(data ?? 0).toFixed(2); } },
                    // { title: 'Pagado', render: function () { return getNumberHTML("0.00"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-success capturarConvenio' title='Agregar convenio' data-cliente='" + row.cliente + "'><i class='fas fa-plus'></i></button>"; } },

                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                //order: [[9, 'desc']],
                drawCallback: function () {
                    tablaCXCFacturas.find('.capturarConvenio').click(function (e) {
                        const rowData = dtTablaCXCFacturas.row($(this).parents('tr')).data();
                        fncCapturarConvenio(rowData.datafactura.numcte, rowData.factura)
                    });
                },
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido15 = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido30 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido60 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido90 = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencidoMas = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    // pronostico = api.column(9).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total = api.column(9).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    $(api.column(2).footer()).html('TOTAL');
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(8).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    // $(api.column(9).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    // $(api.column(10).footer()).html('$0.00');
                    $(api.column(9).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                }
            });
        }

        function fncCapturarConvenio(numcte, factura) {
            document.location.href = `/CuentasPorCobrar/CuentasPorCobrar/Convenios?cargarModal=${1}&clienteID=${numcte}&facturaID=${factura}`;
        }

        function cargarTablaCXCFacturas(detalles, fechaCorte, cc) {
            // console.log(detalles);

            if (detalles == null && cc == null) {
                return false;
            }
            else {
                dtTablaCXCFacturas.clear();
                var datosFinales = [];
                $.each(detalles, function (i, n) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const factura = n.factura;
                    const concepto = n.concepto;
                    const fecha = new Date(parseInt(n.fecha.substr(6)));
                    var porVencer = 0;
                    if (fecha > fechaFin) porVencer = n.monto;
                    var vencido15 = 0;
                    if (fecha > fechaInicio && fecha <= fechaFin) vencido15 = n.monto;
                    var vencido30 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    if (fecha > fechaInicio && fecha <= fechaFin) vencido30 = n.monto;
                    var vencido60 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    if (fecha > fechaInicio && fecha <= fechaFin) vencido60 = n.monto;
                    var vencido90 = 0;
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    if (fecha > fechaInicio && fecha <= fechaFin) vencido90 = n.monto;
                    var vencidoMas = 0;
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    if (fecha < fechaInicio) vencidoMas = n.monto;
                    var total = n.monto;
                    var valPronostico = n.montoPronosticado;
                    const group = { factura: factura, concepto: concepto, fecha: fecha, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, montoPronosticado: valPronostico, total: total, datafactura: n };

                    if (n.areaCuenta == cc) {
                        datosFinales.push(group);

                    }
                });
                datosFinales.sort((a, b) => b.total - a.total);
                dtTablaCXCFacturas.rows.add(datosFinales);
                dtTablaCXCFacturas.draw();
                return true;
            }
        }

        function initTablaCXCAC() {
            dtTablaCXCAC = tablaCXCAC.DataTable({
                dom: 'B<lfr>tip',
                buttons: [ 
                    'excelHtml5', 
                ],
                language: {
                    sInfo: "Mostrando _TOTAL_ registro(s)",
                    sSearch: "Filtrar:",
                    sZeroRecords: "No se encontraron resultados",
                    sEmptyTable: "Ningún dato disponible en esta tabla",
                },
                searching: true,
                autoWidth: true,
                scrollY: "450px",
                scrollCollapse: true,
                paging: false,
                ordering: false,
                dom: '<f<t>>',
                columns: [
                    { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary facturasXCC' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'descripcionCC', title: 'Descripcion CC' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { title: 'Pagado', render: function () { return getNumberHTML("0.00"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                ],
                //order: [[9, 'desc']],
                drawCallback: function () {
                    tablaCXCAC.on('click', '.facturasXCC', function () {
                        let rowData = dtTablaCXCAC.row($(this).closest('tr')).data();

                        var detalles = rowData.detalles;
                        console.log(detalles)
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCFacturas(detalles, fechaCorteGeneral, rowData.cc);
                        titleModalCXCFacturas.text("CC: " + rowData.descripcionCC);
                        titleClienteModalCXCClientes.text(rowData.cliente);
                        $("#modalCXCFacturas").modal("show");
                    });
                },
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido15 = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido30 = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido60 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido90 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencidoMas = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    // pronostico = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    // $(api.column(7).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    // $(api.column(8).footer()).html('$0.00');
                    $(api.column(8).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                }
            });
        }

        function cargarTablaCXCAC(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }
            else {
                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function (i, n) { totalDetalles += n.monto; });
                const grouped = groupBy(detalles, function (detalle) { return detalle.areaCuenta; });
                dtTablaCXCAC.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const descripcionCC = value[0].areaCuenta + " " + value[0].areaCuentaDesc;
                    var total = 0;
                    $.each(value, function (i, n) { total += n.monto; });
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function (i, n) { porVencer += n.monto; });
                    const vencido15Detalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function (i, n) { vencido15 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0) $.each(vencido30Detalles, function (i, n) { vencido30 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function (i, n) { vencido60 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function (i, n) { vencido90 += n.monto; });
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function (i, n) { vencidoMas += n.monto; });
                    let valPronostico = 0;
                    $.each(value, function (i, n) { valPronostico += n.montoPronosticado; });
                    const grupo = { descripcionCC: descripcionCC, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, montoPronosticado: valPronostico, total: total, porcentaje: porcentaje, cc: value[0].areaCuenta, detalles: detalles };
                    datosFinales.push(grupo);
                });
                datosFinales.sort((a, b) => b.total - a.total);
                dtTablaCXCAC.rows.add(datosFinales);
                dtTablaCXCAC.draw();
                return true;
            }
        }

        function cargarTablaCXCCc(detalles, fechaCorte) {
            if (detalles == null) {
                return false;
            }
            else {
                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function (i, n) { totalDetalles += n.monto; });
                const grouped = groupBy(detalles, function (detalle) { return detalle.areaCuenta; });
                dtCXCCc.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const descripcionCC = value[0].areaCuenta + " " + value[0].areaCuentaDesc;
                    var total = 0;
                    $.each(value, function (i, n) { total += n.monto; });
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function (i, n) { porVencer += n.monto; });
                    const vencido15Detalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function (i, n) { vencido15 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0) $.each(vencido30Detalles, function (i, n) { vencido30 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function (i, n) { vencido60 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function (i, n) { vencido90 += n.monto; });
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(value, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function (i, n) { vencidoMas += n.monto; });
                    let valPronostico = 0;
                    $.each(value, function (i, n) { valPronostico += n.montoPronosticado; });

                    const grupo = { descripcionCC: descripcionCC, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, montoPronosticado: valPronostico, total: total, porcentaje: porcentaje, cc: value[0].areaCuenta, detalles: value };
                    datosFinales.push(grupo);
                });
                datosFinales.sort((a, b) => b.total - a.total);
                dtCXCCc.rows.add(datosFinales);
                dtCXCCc.draw();
                return true;
            }
        }

        function recargarTotalizadoresCX() {
            let rowData = dtTablaCXP.data();
            var detallesRaw = rowData.map(function (x) { return x.detalles });
            var detalles = [].concat.apply([], detallesRaw);
            detalles = $.grep(detalles, function (el, index) { return (index == $.inArray(el, detalles) && el != null); });
            if (areasCuentaDetalle.length > 0) {
                detalles = $.grep(detalles, function (el, index) {
                    return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0);
                });
            }
            var totalizador = 0;
            $.each(detalles, function (i, n) { totalizador += n.monto; });
            $(".pCXP").text(parseFloat(totalizador / 1000).toFixed(2));

            rowData = dtTablaCXC.data();
            detallesRaw = rowData.map(function (x) { return x.detalles });
            detalles = [].concat.apply([], detallesRaw);
            detalles = $.grep(detalles, function (el, index) { return (index == $.inArray(el, detalles) && el != null); });
            if (areasCuentaDetalle.length > 0) {
                detalles = $.grep(detalles, function (el, index) {
                    return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0);
                });
            }
            totalizador = 0;
            $.each(detalles, function (i, n) { totalizador += n.monto; });
            $(".pCXC").text(parseFloat(totalizador / 1000).toFixed(2));
        }

        function cargarAC() {
            setResponsable();
            comboAC.fillComboAsync('cboObraEstimados', { divisionID: comboDivision.val(), responsableID: comboResponsable.val() }, false, "TODOS");
            comboAC.find('option').get(0).remove();
            if (comboDivision.val() == "TODOS" && comboResponsable.val() == "TODOS") comboAC.find('option').prop('selected', false).change();
            else comboAC.find('option').prop('selected', true).change();
            comboAC.trigger({ type: 'select2:close' });

            comboAC.next(".select2-container").css("display", "none");
            var seleccionados = $(this).siblings("span").find(".select2-selection__choice");
            if (seleccionados.length == 0) $("#spanComboAC").text("TODOS");
            else {
                if (seleccionados.length == 1) $("#spanComboAC").text($(seleccionados[0]).text().slice(1));
                else $("#spanComboAC").text(seleccionados.length.toString() + " Seleccionados");
            }
        }

        function groupBy(list, keyGetter) {
            const map = new Map();
            list.forEach(function (item) {
                const key = keyGetter(item);
                const collection = map.get(key);
                if (!collection) {
                    map.set(key, [item]);
                } else {
                    collection.push(item);
                }
            });
            return map;
        }

        function getNumberHTML(value) {
            return '<p class="' + (value != 0 ? 'noDesplegable' : '') + (value < 0 ? ' Danger' : '') + '" >' + parseFloat(value / 1000).toFixed(2) + '</p>';
        }

        function initTblCXCCc() {
            dtCXCCc = tblCXCCc.DataTable({
                dom: 'B<lfr>tip',
                buttons: [ 
                    'excelHtml5', 
                ],
                language: dtDicEsp,
                destroy: false,
                paging: true,
                ordering: true,
                searching: true,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary detalleClientes' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'descripcionCC', title: 'Descripcion CC' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, type, row) { return maskNumero(parseFloat(data ?? 0).toFixed(2); } },
                    // { title: 'Pagado', render: function () { return getNumberHTML("0.00"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                ],
                initComplete: function (settings, json) {
                    tblCXCCc.on('click', '.detalleClientes', function () {
                        let rowData = dtCXCCc.row($(this).closest('tr')).data();
                        cargarTablaCXCClientes(rowData.detalles, fechaCorteGeneral, rowData.cc);
                        modalCXCClientes.modal("show");
                        titleModalCXCClientes.text(rowData.descripcionCC);
                        titleClienteModalCXCClientes.text(rowData.cliente);
                    });
                    tblCXCCc.on('click', '.classBtn', function () {
                        let rowData = dtCXCCc.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido15 = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido30 = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido60 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido90 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencidoMas = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    // pronostico = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    // $(api.column(7).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    // $(api.column(8).footer()).html('$0.00');
                    $(api.column(8).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                }
            });
        }

        function initTblCXCCcClientes() {
            dtCXCCcClientes = tblCXCCcClientes.DataTable({
                dom: 'B<lfr>tip',
                buttons: [ 
                    'excelHtml5', 
                ],
                language: dtDicEsp,
                destroy: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: false,
                info: false,
                columns: [
                    //render: function (data, type, row) { }
                    { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary detalleFacturas' data-cliente='" + row.cliente + "'><i class='fas fa-layer-group'></i></button>"; } },
                    // { title: '', render: function (data, type, row) { return "<button class='btn btn-sm btn-primary verFacturas' data-cliente='" + row.cliente + "'><i class='fas fa-clipboard-list'></i></button>"; } },
                    { data: 'cliente', title: 'Cliente' },
                    { data: 'porVencer', title: 'Por Vencer', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido15', title: 'Vencido 15 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido30', title: 'Vencido 30 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido60', title: 'Vencido 60 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencido90', title: 'Vencido 90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    { data: 'vencidoMas', title: 'Vencido +90 Días', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    // { data: 'montoPronosticado', title: 'Pronóstico', render: function (data, type, row) { return maskNumero(parseFloat(data ?? 0).toFixed(2); } },
                    // { title: 'Pagado', render: function () { return getNumberHTML("0.00"); } },
                    { data: 'total', title: 'Total', render: function (data, type, row) { return maskNumero(parseFloat(data).toFixed(2)); } },
                    {
                        data: 'porcentaje',
                        title: '%',
                        render: function (data, type, row) {
                            return data.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + "%";
                        }
                    }
                ],
                initComplete: function (settings, json) {
                    tblCXCCcClientes.on('click', '.detalleFacturas', function () {
                        let rowData = dtCXCCcClientes.row($(this).closest('tr')).data();

                        var detalles = rowData.detalles;
                        console.log(detalles)
                        const proveedor = $(this).attr("data-proveedor");
                        cargarTablaCXCFacturas(detalles, fechaCorteGeneral, rowData.cc);
                        // titleModalCXCFacturas.text("Cliente: " + rowData.cliente);
                        titleModalCXCFacturas.text("CC: " + rowData.descripcionCC);
                        titleClienteModalCXCClientes.text(rowData.cliente);
                        titleClienteModalCXCClientes2.text(rowData.cliente);
                        $("#modalCXCFacturas").modal("show");

                    });
                    tblCXCCcClientes.on('click', '.classBtn', function () {
                        let rowData = dtCXCCcClientes.row($(this).closest('tr')).data();
                        //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                    });
                },
                columnDefs: [
                    { className: 'dt-center', 'targets': '_all' }
                ],
                footerCallback: function (row, data, start, end, display) {
                    var api = this.api(), data;
                    var intVal = function (i) {
                        return typeof i === 'string' ?
                            i.replace(/[\$,]/g, '') * 1 :
                            typeof i === 'number' ?
                                i : 0;
                    };
                    totalPorVencer = api.column(2).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido15 = api.column(3).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido30 = api.column(4).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido60 = api.column(5).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencido90 = api.column(6).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    totalVencidoMas = api.column(7).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    // pronostico = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    total = api.column(8).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);
                    $(api.column(1).footer()).html('TOTAL');
                    $(api.column(2).footer()).html(maskNumero(parseFloat(totalPorVencer).toFixed(2).toString()));
                    $(api.column(3).footer()).html(maskNumero(parseFloat(totalVencido15).toFixed(2).toString()));
                    $(api.column(4).footer()).html(maskNumero(parseFloat(totalVencido30).toFixed(2).toString()));
                    $(api.column(5).footer()).html(maskNumero(parseFloat(totalVencido60).toFixed(2).toString()));
                    $(api.column(6).footer()).html(maskNumero(parseFloat(totalVencido90).toFixed(2).toString()));
                    $(api.column(7).footer()).html(maskNumero(parseFloat(totalVencidoMas).toFixed(2).toString()));
                    // $(api.column(8).footer()).html(maskNumero(parseFloat(pronostico).toFixed(2).toString()));
                    // $(api.column(9).footer()).html('$0.00');
                    $(api.column(8).footer()).html(maskNumero(parseFloat(total).toFixed(2).toString()));
                    $(api.column(9).footer()).html(parseFloat(totalPorcentaje).toFixed(2).toString() + '%');
                }
            });
        }

        function cargarTablaCXCClientes(detalles, fechaCorte, clienteCC) {
            if (detalles == null) {
                return false;
            }
            else {
                var datosFinales = [];
                var auxDatosFinales = [];
                var totalDetalles = 0;
                $.each(detalles, function (i, n) { totalDetalles += n.monto; });
                const grouped = groupBy(detalles, function (detalle) { return detalle.responsable; });
                dtCXCCcClientes.clear();
                Array.from(grouped, function ([key, value]) {
                    var fechaFin = new Date();
                    fechaFin.setDate(fechaCorte.getDate());
                    var fechaInicio = new Date();
                    fechaInicio.setDate(fechaCorte.getDate() - 15);
                    const cliente = value[0].responsable;
                    let ccDet = value[0].areaCuenta;
                    let descripcionCC = value[0].areaCuenta + " " + value[0].areaCuentaDesc;
                    var total = 0;
                    var detallesFiltrados = value;
                    //if(areasCuentaDetalle.length > 0) {
                    //    detallesFiltrados = $.grep(detallesFiltrados,function(el,index){ 
                    //        return (el != null && $.inArray((el.areaCuenta + " " + el.areaCuentaDesc), areasCuentaDetalle) >= 0); 
                    //    });
                    //}
                    $.each(detallesFiltrados, function (i, n) { total += n.monto; });
                    const porcentaje = totalDetalles > 0 ? (total * 100) / totalDetalles : 0;
                    const porVencerDetalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaFin; });
                    var porVencer = 0;
                    if (porVencerDetalles.length > 0) $.each(porVencerDetalles, function (i, n) { porVencer += n.monto; });
                    const vencido15Detalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido15 = 0;
                    if (vencido15Detalles.length > 0) $.each(vencido15Detalles, function (i, n) { vencido15 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 15);
                    fechaInicio.setDate(fechaCorte.getDate() - 30);
                    const vencido30Detalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido30 = 0;
                    if (vencido30Detalles.length > 0) $.each(vencido30Detalles, function (i, n) { vencido30 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 30);
                    fechaInicio.setDate(fechaCorte.getDate() - 60);
                    const vencido60Detalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido60 = 0;
                    if (vencido60Detalles.length > 0) $.each(vencido60Detalles, function (i, n) { vencido60 += n.monto; });
                    fechaFin.setDate(fechaCorte.getDate() - 60);
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencido90Detalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) > fechaInicio && new Date(parseInt(n.fecha.substr(6))) <= fechaFin; });
                    var vencido90 = 0;
                    if (vencido90Detalles.length > 0) $.each(vencido90Detalles, function (i, n) { vencido90 += n.monto; });
                    fechaInicio.setDate(fechaCorte.getDate() - 90);
                    const vencidoMasDetalles = jQuery.grep(detallesFiltrados, function (n, i) { return new Date(parseInt(n.fecha.substr(6))) < fechaInicio; });
                    var vencidoMas = 0;
                    if (vencidoMasDetalles.length > 0) $.each(vencidoMasDetalles, function (i, n) { vencidoMas += n.monto; });
                    let valPronostico = 0;
                    $.each(detallesFiltrados, function (i, n) { valPronostico += n.montoPronosticado; });
                    const grupo = { descripcionCC: descripcionCC, cliente: cliente, porVencer: porVencer, vencido15: vencido15, vencido30: vencido30, vencido60: vencido60, vencido90: vencido90, vencidoMas: vencidoMas, montoPronosticado: valPronostico, total: total, porcentaje: porcentaje, cc: ccDet, detalles: value };

                    auxDatosFinales.push(grupo);

                    // if (cc == clienteCC) {
                    //     auxDatosFinales.push(grupo);

                    // }
                });
                auxDatosFinales.sort((a, b) => b.total - a.total);
                dtCXCCcClientes.rows.add(auxDatosFinales);
                dtCXCCcClientes.draw();
                return true;
            }
        }

        init();
    };

    $(document).ready(function () {
        CuentasPorCobrar.CuentasPorCobrar.ControlEstimaciones = new controlestimaciones();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();