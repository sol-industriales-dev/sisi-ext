(() => {
    $.namespace('ControlPresupuestal.ControlPresupuestal');
    ControlPresupuestal = function () {
        //#region Selectores
        const selectAreaCuenta = $('#selectAreaCuenta');
        const selectTipo = $('#selectTipo');
        const selectGrupo = $('#selectGrupo');
        const selectModelo = $('#selectModelo');
        const selectCC = $('#selectCC');
        const inputFechaInicial = $('#inputFechaInicial');
        const inputFechaFinal = $('#inputFechaFinal');
        const botonBuscar = $('#botonBuscar');
        const cboCostoPor = $('#cboCostoPor');
        const botonBuscarConsultadoEmpresaDiv = $('#botonBuscarConsultadoEmpresaDiv');
        const botonBuscarActualEmpresaDiv = $('#botonBuscarActualEmpresaDiv');
        const botonBuscar12EmpresaDiv = $('#botonBuscar12EmpresaDiv');
        const botonBuscar24EmpresaDiv = $('#botonBuscar24EmpresaDiv');
        const tablaControlPresupuestal = $('#tablaControlPresupuestal');
        const modalDetalleAgrupado = $('#modalDetalleAgrupado');
        const tablaDetalleAgrupado = $('#tablaDetalleAgrupado');
        const modalDetalleMovimientos = $('#modalDetalleMovimientos');
        const tablaDetalleMovimientos = $('#tablaDetalleMovimientos');
        const mdlEconomico = $('#mdlEconomico');

        const botonBuscarConsultadoEmpresaDiv_Agrupado = $('#botonBuscarConsultadoEmpresaDiv_Agrupado');
        const botonBuscarActualEmpresaDiv_Agrupado = $('#botonBuscarActualEmpresaDiv_Agrupado');
        const botonBuscar12EmpresaDiv_Agrupado = $('#botonBuscar12EmpresaDiv_Agrupado');
        const botonBuscar24EmpresaDiv_Agrupado = $('#botonBuscar24EmpresaDiv_Agrupado');

        const botonBuscarConsultadoEmpresaDiv_Economico = $('#botonBuscarConsultadoEmpresaDiv_Economico');
        const botonBuscarActualEmpresaDiv_Economico = $('#botonBuscarActualEmpresaDiv_Economico');
        const botonBuscar12EmpresaDiv_Economico = $('#botonBuscar12EmpresaDiv_Economico');
        const botonBuscar24EmpresaDiv_Economico = $('#botonBuscar24EmpresaDiv_Economico');

        const chkAcumulado = $("#chkAcumulado");
        //#endregion
        _rowData = null;
        _conceptoDetalle = 0;

        const tblDetalleA = $('#tblDetalleA');
        let dttblDetalleA;

        let dtControlPresupuestal;
        let dtDetalleAgrupado;
        let dtDetalleMovimientos;

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        (function init() {
            $('.select2').select2();
            initTablaControlPresupuestal();
            initTablaDetalleAgrupado();
            initTablaDetalleMovimientos();
            initCombos();
            inittblControl();
            inputFechaInicial.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", new Date(fechaActual.getFullYear(), fechaActual.getMonth(), 1));
            inputFechaFinal.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            botonBuscar.click(cargarControlPresupuestal);
            botonBuscarConsultadoEmpresaDiv.click(cargarControlPresupuestal_Solo_Grafica);
            botonBuscarActualEmpresaDiv.click(cargarControlPresupuestal_Solo_Grafica);
            botonBuscar12EmpresaDiv.click(cargarControlPresupuestal_Solo_Grafica);
            botonBuscar24EmpresaDiv.click(cargarControlPresupuestal_Solo_Grafica);

            botonBuscarConsultadoEmpresaDiv_Agrupado.click(fnAgrupado);
            botonBuscarActualEmpresaDiv_Agrupado.click(fnAgrupado);
            botonBuscar12EmpresaDiv_Agrupado.click(fnAgrupado);
            botonBuscar24EmpresaDiv_Agrupado.click(fnAgrupado);

            botonBuscarConsultadoEmpresaDiv_Economico.click(fnEconomico);
            botonBuscarActualEmpresaDiv_Economico.click(fnEconomico);
            botonBuscar12EmpresaDiv_Economico.click(fnEconomico);
            botonBuscar24EmpresaDiv_Economico.click(fnEconomico);

            convertToMultiselect('#selectCC');
            convertToMultiselect('#selectGrupo');
            convertToMultiselect('#selectModelo');

            selectTipo.change(function () {
                selectGrupo.fillCombo('/ControlPresupuestal/obtenerGruposMaquinaria', { idTipo: selectTipo.val() }, false, 'Todos', () => {
                    selectGrupo.trigger('change');
                });
                convertToMultiselect('#selectGrupo');
            });

            selectTipo.trigger('change');

            selectGrupo.change(function () {
                selectModelo.fillCombo('/ControlPresupuestal/FillCboModeloEquipoMultiple', { listaGrupos: getValoresMultiples('#selectGrupo') }, false, 'Todos', () => {
                    selectModelo.change();
                });
                convertToMultiselect('#selectModelo');
            });

            selectGrupo.trigger('change');

            selectModelo.change(function () {
                selectCC.fillCombo('/ControlPresupuestal/GetComboEconomicosMultiple', {
                    AreaCuenta: selectAreaCuenta.find('option:selected').attr('data-prefijo'),
                    listaModelos: getValoresMultiples('#selectModelo')
                }, false, 'Todos');
                convertToMultiselect('#selectCC');
            });

            selectAreaCuenta.change(function (e) {
                selectCC.fillCombo('/ControlPresupuestal/GetComboEconomicosMultiple', {
                    AreaCuenta: selectAreaCuenta.find('option:selected').attr('data-prefijo'),
                    listaModelos: getValoresMultiples('#selectModelo')
                }, false, 'Todos');
                convertToMultiselect('#selectCC');
            });

            mdlEconomico.on('shown.bs.modal', function () {
                tblDetalleA.DataTable().columns.adjust().draw();
            });
        })();
        function fnAgrupado() {
            var tipo = $(this).data('tipo');
            cargarDetalleAgrupado(_rowData, _conceptoDetalle, tipo);
        }
        function fnEconomico() {
            var tipo = $(this).data('tipo');
            mdlGraficaEconomico(_rowData, _conceptoDetalle, tipo);
        }
        function initCombos(idTipo) {
            selectAreaCuenta.fillCombo('/Conciliacion/getCboCC', null, false, null);
        }
        function mdlGraficaEconomico(rowData, concepto, tipo) {
            let filtros = getFiltros(tipo);
            $('#title-modal2').text(rowData.economico);
            dt = tblDetalleA.DataTable();
            dt.clear().draw();
            axios.post('CargarDetallePresupuestal', { filtros, economico: rowData.economico, concepto })
                .catch(o_O => AlertaGeneral(o_O.message))
                .then(response => {
                    console.log(response.data.data)
                    AddRows(tblDetalleA, response.data.data);
                    initgraficaEconomico(response.data.graficaTendenciaPresupuestoReal);
                });
        }
        function initTablaControlPresupuestal() {
            dtControlPresupuestal = tablaControlPresupuestal.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 'Bt',
                scrollY: '45vh',
                scrollX: 'auto',
                scrollCollapse: true,
                buttons: [{
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32],
                        orthogonal: "exportxls"
                    },
                    className: 'btn btn-xs btn-default',
                    title: '', //Se quita el título para que en el archivo de Excel no se agregue una fila con ese título.
                    init: function (api, node, config) {
                        $(node).removeClass('dt-button');
                    },
                    customize: function (xlsx) {
                        var sheet = xlsx.xl.worksheets['sheet1.xml'];
                        var colRange = sheet.getElementsByTagName('col').length - 1;
                        var numrows = 1;
                        var clR = $('row', sheet);

                        //update Row
                        clR.each(function () {
                            var attr = $(this).attr('r');
                            var ind = parseInt(attr);
                            ind = ind + numrows;
                            $(this).attr("r", ind);
                        });

                        // Create row before data
                        $('row c ', sheet).each(function () {
                            var attr = $(this).attr('r');
                            var pre = attr.substring(0, 1);
                            var ind = parseInt(attr.substring(1, attr.length));
                            ind = ind + numrows;
                            $(this).attr("r", pre + ind);
                        });

                        function Addrow(index, data, sheet) {
                            msg = '<row r="' + index + '">'
                            for (i = 0; i < data.length; i++) {
                                var key = data[i].key;
                                var value = data[i].value;
                                var style = data[i].style;
                                mergeCells(index, data[i].range, data[i].key, sheet);
                                msg += '<c t="inlineStr" s="' + style + '" r="' + key + index + '">';
                                msg += '<is>';
                                msg += '<t>' + value + '</t>';
                                msg += '</is>';
                                msg += '</c>';
                            }
                            msg += '</row>';
                            return msg;
                        }

                        var mergeCells = function (row, colspan, key, sheet) {
                            var mergeCells = $('mergeCells', sheet);

                            mergeCells[0].appendChild(_createNode(sheet, 'mergeCell', {
                                attr: {
                                    ref: key + row + ':' + createCellPos(key, colspan) + row
                                }
                            }));
                        };
                        let listaColumnasIndex = [];
                        function createCellPos(key, n) {
                            let indexInicio = listaColumnasIndex.find(x => x.columna == key).index;
                            let indexFin = indexInicio + n;
                            let columnaFin = listaColumnasIndex.find(x => x.index == indexFin).columna;

                            return columnaFin;

                            // var ordA = 'A'.charCodeAt(0);
                            // var ordZ = 'Z'.charCodeAt(0);
                            // var len = ordZ - ordA + 1;
                            // var s = "";

                            // while (n >= 0) {
                            //     s = String.fromCharCode(n % len + ordA) + s;
                            //     n = Math.floor(n / len) - 1;
                            // }

                            // return s;
                        }
                        function _createNode(doc, nodeName, opts) {
                            var tempNode = doc.createElement(nodeName);

                            if (opts) {
                                if (opts.attr) {
                                    $(tempNode).attr(opts.attr);
                                }

                                if (opts.children) {
                                    $.each(opts.children, function (key, value) {
                                        tempNode.appendChild(value);
                                    });
                                }

                                if (opts.text !== null && opts.text !== undefined) {
                                    tempNode.appendChild(doc.createTextNode(opts.text));
                                }
                            }
                            return tempNode;
                        }
                        function colName(n) {
                            var ordA = 'a'.charCodeAt(0);
                            var ordZ = 'z'.charCodeAt(0);
                            var len = ordZ - ordA + 1;

                            var s = "";
                            while (n >= 0) {
                                s = String.fromCharCode(n % len + ordA) + s;
                                n = Math.floor(n / len) - 1;
                            }
                            return s;
                        }
                        for (n = 0; n < colRange + 1; n++) {
                            listaColumnasIndex.push({ index: n, columna: colName(n).toUpperCase() })
                        }

                        var r1 = Addrow(1, [
                            { key: 'A', value: '', style: '51', range: 3 },
                            { key: 'E', value: 'Depreciación', style: '51', range: 2 },
                            { key: 'H', value: 'Seguro', style: '51', range: 2 },
                            { key: 'K', value: 'Filtros', style: '51', range: 2 },
                            { key: 'N', value: 'Correctivo', style: '51', range: 2 },
                            { key: 'Q', value: 'Depreciación Overhaul', style: '51', range: 2 },
                            { key: 'T', value: 'Aceite', style: '51', range: 2 },
                            { key: 'W', value: 'Carrilería', style: '51', range: 2 },
                            { key: 'Z', value: 'Ansul', style: '51', range: 2 },
                            { key: 'AC', value: 'Otros', style: '51', range: 0 },
                            { key: 'AD', value: 'Conciliacion Daños', style: '51', range: 0 },
                            { key: 'AE', value: 'Total', style: '51', range: 2 }
                        ], sheet);

                        sheet.childNodes[0].childNodes[1].innerHTML = r1 + sheet.childNodes[0].childNodes[1].innerHTML;
                    }
                }],
                initComplete: function (settings, json) {
                    tablaControlPresupuestal.on('click', '.modalDetalle', function () {
                        let rowData = dtControlPresupuestal.row($(this).closest('tr')).data();
                        let concepto = +$(this).attr('concepto');
                        _rowData = rowData;
                        _conceptoDetalle = concepto;
                        cargarDetalleAgrupado(_rowData, _conceptoDetalle, 'consultado');
                    });
                    tablaControlPresupuestal.on('click', '.mdlEconomi', function () {
                        let rowData = dtControlPresupuestal.row($(this).closest('tr')).data();
                        let concepto = +$(this).attr('concepto');
                        _rowData = rowData;
                        _conceptoDetalle = concepto;
                        mdlGraficaEconomico(_rowData, _conceptoDetalle, 'consultado');
                    });

                },
                drawCallback: function (settings) {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
                footerCallback: function (row, data, start, end, display) {
                    if (data.length > 0) {
                        let totalPresupuesto = 0;
                        let totalReal = 0;
                        let totalDiferencia = 0;

                        data.forEach(function (x) {
                            totalPresupuesto += x.presupuestoTotal;
                            totalReal += x.realTotal;
                            totalDiferencia += x.diferenciaTotal;
                        });

                        $(row).find('th').eq(1).html(`${totalPresupuesto >= 0 ? maskNumero(totalPresupuesto) : '-' + (maskNumero(totalPresupuesto).replace('-', ''))}`);
                        $(row).find('th').eq(2).html(`${totalReal >= 0 ? maskNumero(totalReal) : '-' + (maskNumero(totalReal).replace('-', ''))}`);
                        $(row).find('th').eq(3).html(`${totalDiferencia >= 0 ? maskNumero(totalDiferencia) : '-' + (maskNumero(totalDiferencia).replace('-', ''))}`);
                    }
                },
                columns: [
                    {
                        data: 'economico', title: 'Eco', width: '40px', render: function (data, type, row, meta) {
                            return `<a class='mdlEconomi' data-target="#mdlEconomico" data-toggle="modal" href="#mdlEconomico" concepto="1">${data}</a>`;
                        }
                    },
                    { data: 'modelo', title: 'Modelo' },
                    { data: 'horasTrabajadas', title: 'Hrs Trab' },
                    { data: 'diasTrabajados', title: 'Días Trab' },
                    {
                        data: 'presupuestoDepreciacion', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realDepreciacion', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="1">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaDepreciacion', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'presupuestoSeguro', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realSeguro', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="2">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaSeguro', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'presupuestoFiltros', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realFiltros', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="3">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaFiltros', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'presupuestoCorrectivo', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realCorrectivo', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="4">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaCorrectivo', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'presupuestoDepreciacionOverhaul', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realDepreciacionOverhaul', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="5">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaDepreciacionOverhaul', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'presupuestoAceite', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realAceite', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="6">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaAceite', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'presupuestoCarrileria', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realCarrileria', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="7">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaCarrileria', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'presupuestoAnsul', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realAnsul', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="8">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaAnsul', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realOtros', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="9">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realDanos', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a class='modalDetalle' data-target="#modalDetalleAgrupado" data-toggle="modal" href="#modalDetalleAgrupado" concepto="10">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'presupuestoTotal', title: 'Ppto', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'realTotal', title: 'Real', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'diferenciaTotal', title: 'Diferencia', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else if (type === 'exportxls') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", targets: "_all" },
                    { targets: [3], visible: false, searchable: false }
                ]
            });

            dtControlPresupuestal.buttons().container().appendTo($('#divBotonExcel'));
        }

        function initTablaDetalleAgrupado() {
            dtDetalleAgrupado = tablaDetalleAgrupado.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                // scrollY: '45vh',
                // scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaDetalleAgrupado.on('click', 'a', function () {
                        let rowData = dtDetalleAgrupado.row($(this).closest('tr')).data();

                        cargarDetalleMovimientos(rowData);
                    });
                },
                drawCallback: function (settings) {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
                footerCallback: function (row, data, start, end, display) {
                    if (data.length > 0) {
                        let total = 0;

                        data.forEach(function (x) {
                            total += x.importe;
                        });

                        $(row).find('th').eq(1).html(`${total >= 0 ? maskNumero(total) : '-' + (maskNumero(total).replace('-', ''))}`);
                    }
                },
                columns: [
                    { data: 'cuentaDesc', title: 'Cuenta' },
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return `<a data-target="#modalDetalleMovimientos" data-toggle="modal" href="#modalDetalleMovimientos">${data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''))}</a>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaDetalleMovimientos() {
            dtDetalleMovimientos = tablaDetalleMovimientos.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                ordering: false
                , ordering: false
                , searching: false
                , bFilter: true
                , info: false,
                dom: 't',
                // scrollY: '45vh',
                // scrollCollapse: true,
                order: [[0, "desc"]],
                initComplete: function (settings, json) {

                },
                drawCallback: function (settings) {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                },
                footerCallback: function (row, data, start, end, display) {
                    if (data.length > 0) {
                        let total = 0;

                        data.forEach(function (x) {
                            total += x.importe;
                        });

                        $(row).find('th').eq(1).html(`${total >= 0 ? maskNumero(total) : '-' + (maskNumero(total).replace('-', ''))}`);
                    }
                },
                columns: [
                    { data: 'fechaPolizaString', title: 'Fecha Póliza' },
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : '-' + (maskNumero(data).replace('-', ''));
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }



        function cargarControlPresupuestal() {
            let listaEconomico = [];

            getValoresMultiplesCustom('#selectCC').forEach(x => {
                listaEconomico.push(x.text);
            });

            let filtros = {
                area: +selectAreaCuenta.find('option:selected').text().split('-')[0],
                cuenta: +selectAreaCuenta.find('option:selected').text().split('-')[1],
                areaCuenta: selectAreaCuenta.find('option:selected').attr('data-prefijo'),
                tipo: +selectTipo.val(),
                listaGrupos: getValoresMultiples('#selectGrupo'),
                listaModelos: getValoresMultiples('#selectModelo'),
                listaEconomico: listaEconomico,
                fechaInicial: inputFechaInicial.val(),
                fechaFinal: inputFechaFinal.val(),
                acumulado: chkAcumulado.is(":checked"),
                tipoHoraDia: cboCostoPor.val()
            }

            axios.post('CargarControlPresupuestal', { filtros })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaControlPresupuestal, response.data.data);
                        initGrafica(response.data.graficaTendenciaPresupuestoReal);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function cargarControlPresupuestal_Solo_Grafica() {
            var tipo = $(this).data('tipo');
            let filtros = getFiltros(tipo);
            axios.post('CargarControlPresupuestal_Solo_Grafica', { filtros })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        initGrafica(response.data.graficaTendenciaPresupuestoReal);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
        function getFirstDatebyMonth(meses) {
            var dt = new Date();
            dt.setMonth(dt.getMonth() - meses);
            var firstDay = new Date(dt.getFullYear(), dt.getMonth(), 1);
            return firstDay;
        }

        function getFiltros(tipo) {
            let listaEconomico = [];

            getValoresMultiplesCustom('#selectCC').forEach(x => {
                listaEconomico.push(x.text);
            });

            var fi = '';
            var ff = '';
            const d = new Date();
            const d2 = new Date();
            var fa = moment(d).format('DD/MM/YYYY');

            switch (tipo) {
                case 'consultado':
                    {
                        fi = inputFechaInicial.val();
                        ff = inputFechaFinal.val();
                    }
                    break;
                case 'actual':
                    {
                        fi = '01/01/' + d.getFullYear();
                        ff = fa;
                    }
                    break;
                case 12:
                    {
                        fi = moment(getFirstDatebyMonth(12)).format('DD/MM/YYYY');
                        ff = fa;
                    }
                    break;
                case 24:
                    {
                        fi = moment(getFirstDatebyMonth(24)).format('DD/MM/YYYY');
                        ff = fa;
                    }
                    break;
                default:
            }
            let filtros = {
                area: +selectAreaCuenta.find('option:selected').text().split('-')[0],
                cuenta: +selectAreaCuenta.find('option:selected').text().split('-')[1],
                areaCuenta: selectAreaCuenta.find('option:selected').attr('data-prefijo'),
                tipo: +selectTipo.val(),
                listaGrupos: getValoresMultiples('#selectGrupo'),
                listaModelos: getValoresMultiples('#selectModelo'),
                listaEconomico: listaEconomico,
                fechaInicial: fi,
                fechaFinal: ff,
                acumulado: chkAcumulado.is(":checked")
            }
            return filtros;
        }
        function cargarDetalleAgrupado(rowData, concepto, tipo) {

            let filtros = getFiltros(tipo);

            dt = tablaDetalleAgrupado.DataTable();
            dt.clear().draw();
            axios.post('CargarDetalleAgrupado', { filtros, economico: rowData.economico, concepto })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        $('#title-modal').text(rowData.economico);
                        AddRows(tablaDetalleAgrupado, response.data.data);
                        initgraficaReal(response.data.graficaTendenciaPresupuestoReal);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function cargarDetalleMovimientos(rowData) {
            let filtros = {
                area: +selectAreaCuenta.find('option:selected').text().split('-')[0],
                cuenta: +selectAreaCuenta.find('option:selected').text().split('-')[1],
                fechaInicial: inputFechaInicial.val(),
                fechaFinal: inputFechaFinal.val(),
                acumulado: chkAcumulado.is(":checked")
            }

            axios.post('CargarDetalleMovimientos', { filtros, economico: rowData.economico, concepto: rowData.concepto, cta: rowData.cta, scta: rowData.scta, sscta: rowData.sscta })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        $('#title-modal-movimientos').text(rowData.economico);
                        console.log(response.data.data)
                        AddRows(tablaDetalleMovimientos, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function initGrafica(datos) {
            Highcharts.chart('grafica', {
                chart: { type: 'line'},
                lang: highChartsDicEsp,
                title: { text: 'Tendencia Presupuesto VS Real' },
                xAxis: {
                    categories: datos.meses,
                    crosshair: true
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    min: 0
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
                series: [
                    {
                        name: 'Ppto',
                        data: datos.serie1
                    },
                    {
                        name: 'Real',
                        data: datos.serie2
                    }
                ],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                },
                credits: { enabled: false },
                legend: { enabled: false }
            });
        }


        function initgraficaReal(datos) {
            let series = [];

            if (_conceptoDetalle != 9) {
                series = [
                    {
                        name: 'Ppto',
                        data: datos.serie1
                    },
                    {
                        name: 'Real',
                        data: datos.serie2
                    }
                ];
            } else {
                series = [
                    {
                        name: 'Real',
                        data: datos.serie2
                    }
                ];
            }

            Highcharts.chart('graficaReal', {
                chart: { type: 'line' },
                lang: highChartsDicEsp,
                title: { text: _conceptoDetalle != 9 ? 'Tendencia Presupuesto VS Real' : 'Tendencia Real' },
                xAxis: {
                    categories: datos.meses,
                    crosshair: true
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    min: 0
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
                series: series,
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                },
                credits: { enabled: false },
                legend: { enabled: false }
            });
        }


        function initgraficaEconomico(datos) {
            Highcharts.chart('graficaRealEconomico', {
                chart: { type: 'line' },
                lang: highChartsDicEsp,
                title: { text: 'Tendencia Presupuesto VS Real' },
                xAxis: {
                    categories: datos.meses,
                    crosshair: true
                },
                yAxis: {
                    title: {
                        text: ''
                    },
                    min: 0
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle'
                },
                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        // pointStart: 2010
                    }
                },
                series: [
                    {
                        name: 'Ppto',
                        data: datos.serie1
                    },
                    {
                        name: 'Real',
                        data: datos.serie2
                    }
                ],
                responsive: {
                    rules: [{
                        condition: {
                            maxWidth: 500
                        },
                        chartOptions: {
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom'
                            }
                        }
                    }]
                },
                credits: { enabled: false },
                legend: { enabled: false }
            });
        }




        function inittblControl() {
            tblDetalleA.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'excelHtml5', footer: true,
                        text: 'Descargar en excel',
                        exportOptions: {
                            modifier: {
                                page: '_all'
                            }
                        },
                        className: 'btn btn-xs btn-default',
                        init: function (api, node, config) {
                            $(node).removeClass('dt-button');
                        }
                    }
                ],

                columns: [
                    { data: 'concepto', title: 'CONCEPTO' },
                    { data: 'real', title: 'REAL', className: 'text-right' }
                ],

                columnDefs: [
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                initComplete: function (settings, json) { },
                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) { },
                footerCallback: function (tfoot, data, start, end, display) { }
            });

            dtControlPresupuestal.buttons().container().appendTo($('#divBotonExcel'));
        }

        function getValoresMultiplesCustom(selector) {
            var _tempObj = $(selector + ' option:selected').map(function (a, item) {
                return { value: item.value, text: $(item).text() };
            });
            var _tempArrObj = new Array();
            $.each(_tempObj, function (i, e) {
                _tempArrObj.push(e);
            });
            return _tempArrObj;
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => ControlPresupuestal.ControlPresupuestal = new ControlPresupuestal())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();