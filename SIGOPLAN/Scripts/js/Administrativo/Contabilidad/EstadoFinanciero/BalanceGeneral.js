(() => {
    $.namespace('Administrativo.Contabilidad.BalanceGeneral');
    BalanceGeneral = function () {

        //#region Selectores

        //#region Titulo
        const hdrTitulo = $('#hdrTitulo');
        const hdrSubtitulo = $('#hdrSubtitulo');
        //#endregion

        //#region Filtro
        const ftrSelectEmpresa = $('#ftrSelectEmpresa');
        const ftrInputFechaCorte = $('#ftrInputFechaCorte');
        const ftrSelectCC = $('#ftrSelectCC');
        const ftrSelectTipo = $('#ftrSelectTipo');
        const fltrBtnBuscar = $('#fltrBtnBuscar');
        const fltrBtnReporte = $('#fltrBtnReporte');
        const fltrBtnIndicadores = $('#fltrBtnIndicadores');
        //#endregion

        //#region Balance
        const tblBalance = $('#tblBalance');
        //#endregion

        //#region Modal
        const modalBanco = $('#modalBanco');
        const modalCuenta = $('#modalCuenta');
        const modalObra = $('#modalObra');
        const modalParteRelacionada = $('#modalParteRelacionada');
        const modalAlmacen = $('#modalAlmacen');
        const modalInversion = $('#modalInversion');
        const modalDocumento = $('#modalDocumento');
        const modalIndicadores = $('#modalIndicadores');

        const tablaBanco = $('#tablaBanco');
        const tablaCuenta = $('#tablaCuenta');
        const tablaObra = $('#tablaObra');
        const tablaParteRelacionada = $('#tablaParteRelacionada');
        const tablaAlmacen = $('#tablaAlmacen');
        const tablaInversion = $('#tablaInversion');
        const tablaDocumento = $('#tablaDocumento');
        const tablaDocumentoPuras = $('#tablaDocumentoPuras');
        //#endregion

        //#endregion

        //#region Init inputs
        function initInputs() {
            ftrSelectEmpresa.attr('multiple', true);
            convertToMultiselect('#ftrSelectEmpresa');

            ftrSelectCC.attr('multiple', true);
            ftrSelectCC.fillCombo('FillComboCC', null, false, 'Todos');
            convertToMultiselect('#ftrSelectCC');

            initMonthPicker(ftrInputFechaCorte);

            fltrBtnBuscar.click(cargarBalance);

            fltrBtnReporte.on("click", function () {
                fncGetReporte();
            });

            fltrBtnIndicadores.click(cargarIndicadores);
        }

        function initMonthPicker(input) {
            $(input).datepicker({
                dateFormat: "mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                maxDate: new Date(),
                showAnim: 'slide',
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
            }).datepicker("setDate", new Date());
        }
        //#endregion

        //#region Tablas
        function initTableBalance() {
            tblBalance.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    {
                        data: 'concepto', title: '', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                if (row.renglonEnlace) {
                                    let tiposModal = [
                                        { tipoDetalle: 0, nombre: '' },
                                        { tipoDetalle: 1, nombre: 'Banco' },
                                        { tipoDetalle: 2, nombre: 'Cuenta' },
                                        { tipoDetalle: 3, nombre: 'Obra' },
                                        { tipoDetalle: 4, nombre: 'ParteRelacionada' },
                                        { tipoDetalle: 5, nombre: 'Almacen' },
                                        { tipoDetalle: 6, nombre: 'Inversion' },
                                        { tipoDetalle: 7, nombre: 'Documento' }
                                    ];
                                    let modal = tiposModal.filter((x) => x.tipoDetalle == row.tipoDetalle);

                                    return `<a class='modal${modal[0].nombre}' data-target="#modal${modal[0].nombre}" data-toggle="modal${modal[0].nombre}" href="#modal${modal[0].nombre}" nombre-detalle="${modal[0].nombre}">${data}</a>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'corte', title: '', render: function (data, type, row, meta) {
                            if (!row.renglonSubTitulo) {
                                if (type === 'display') {
                                    if (data != 0) {
                                        return data >= 0 ?
                                        '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                        `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                    } else {
                                        return '';
                                    }
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'corteAnterior', title: '', render: function (data, type, row, meta) {
                            if (!row.renglonSubTitulo) {
                                if (type === 'display') {
                                    if (data != 0) {
                                        return data >= 0 ?
                                        '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                        `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                    } else {
                                        return '';
                                    }
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'variacion', title: 'VARIACIÓN', render: function (data, type, row, meta) {
                            if (!row.renglonSubTitulo) {
                                if (type === 'display') {
                                    if (data != 0) {
                                        return data >= 0 ?
                                        '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                        `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                    } else {
                                        return '';
                                    }
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'dolares', title: 'DLLS', render: function (data, type, row, meta) {
                            if (!row.renglonSubTitulo) {
                                if (type === 'display') {
                                    if (data != 0) {
                                        return data >= 0 ?
                                        '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                        `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                    } else {
                                        return '';
                                    }
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    }
                ],

                columnDefs: [
                    { className: 'dt-center', targets: '_all' },
                    { width: '40%', targets: [0] },
                    { width: '15%', targets: [1,2,3,4] }
                ],

                initComplete: function (settings, json) {
                    tblBalance.on('click', 'a', function () {
                        let rowData = tblBalance.DataTable().row($(this).closest('tr')).data();
                        let modal = $(this).attr('data-target');
                        let nombreDetalle = $(this).attr('nombre-detalle');

                        cargarInformacionModal(modal, nombreDetalle, rowData);
                    });
                },
                createdRow: function (row, data, dataIndex) {
                    let celdas = $(row).find('td');

                    if (data.renglonSubTitulo) {
                        celdas.css('font-weight', '700');
                        celdas.css('font-size', '15px');
                        celdas.css('font-style', 'italic');
                        celdas.css('text-align', 'left');
                    } else if (data.renglonGrupo) {
                        celdas.css('font-weight', '700');
                        celdas.css('border-top', '2px solid black');
                        celdas.css('text-align', 'right');
                        celdas.css('font-style', 'italic');
                    } else if (data.renglonEnlace) {
                        $(celdas[0]).css('font-weight', '700');
                        $(celdas[0]).css('text-decoration', 'underline');
                        $(celdas[0]).css('text-align', 'left');
                    } else {
                        celdas.css('text-align', 'middle');
                        $(celdas[0]).css('text-align', 'left');
                    }
                },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) { },
                footerCallback: function (tfoot, data, start, end, displa) { }
            });
        }

        function initTablaBanco() {
            tablaBanco.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                ordering: false,
                scrollX: true,
                scrollY: '45vh',
                scrollCollapse: true,
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
                        $(celdas[0]).css('text-align', 'left');
                    }
                },
                columns: [
                    { data: 'concepto' },
                    {
                        data: 'mesMonto', render: function (data, type, row, meta) {
                            if (!row.renglonGrupo) {
                                if (type === 'display') {
                                    return data > 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'mesMontoResultado', render: function (data, type, row, meta) {
                            if (row.renglonGrupo) {
                                if (type === 'display') {
                                    return data > 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'mesPorcentaje', render: function (data, type, row, meta) {
                            if (!row.renglonGrupo) {
                                if (type === 'display') {
                                    return data > 0 ? (data + '%') : `<p style="color: red;">${data}%</p>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'mesPorcentajeResultado', render: function (data, type, row, meta) {
                            if (row.renglonGrupo) {
                                if (type === 'display') {
                                    return data > 0 ? (data + '%') : `<p style="color: red;">${data}%</p>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'acumuladoMonto', render: function (data, type, row, meta) {
                            if (!row.renglonGrupo) {
                                if (type === 'display') {
                                    return data > 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'acumuladoMontoResultado', render: function (data, type, row, meta) {
                            if (row.renglonGrupo) {
                                if (type === 'display') {
                                    return data > 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'acumuladoPorcentaje', render: function (data, type, row, meta) {
                            if (!row.renglonGrupo) {
                                if (type === 'display') {
                                    return data > 0 ? (data + '%') : `<p style="color: red;">${data}%</p>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        data: 'acumuladoPorcentajeResultado', render: function (data, type, row, meta) {
                            if (row.renglonGrupo) {
                                if (type === 'display') {
                                    return data > 0 ? (data + '%') : `<p style="color: red;">${data}%</p>`;
                                } else {
                                    return data;
                                }
                            } else {
                                return '';
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' },
                    { width: '30%', targets: [0] }
                ]
            });
        }

        function initTablaCuenta() {
            tablaCuenta.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                ordering: false,
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'clienteDesc', title: '' },
                    {
                        data: 'montoPesos', title: 'M.N.', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'montoDolares', title: 'DLLS', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function initTablaObra() {
            tablaObra.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                ordering: false,
                scrollY: '45vh',
                scrollCollapse: true,
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
                    { data: 'obraDesc', title: '' },
                    {
                        data: 'estimacion', title: 'C/ESTIMACIÓN', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'revision70', title: 'EN REVISIÓN AL 70%', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'revision30', title: 'EN REVISIÓN AL 30%', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function initTablaParteRelacionada() {
            tablaParteRelacionada.DataTable({
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
                    { data: 'parteRelacionada', title: '' },
                    {
                        data: 'saldo', title: 'SALDO', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function initTablaAlmacen() {
            tablaAlmacen.DataTable({
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
                    { data: 'concepto', title: '' },
                    {
                        data: 'saldo', title: 'SALDO', render: function (data, type, row, meta) {
                            if (data == "MENOS") {
                                return "";
                            } else {
                                if (!row.renglonSubTitulo) {
                                    if (type === 'display') {
                                        return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                                    } else {
                                        if (data == 0) {
                                            return 0;
                                        } else {
                                            return data;
                                        }
                                    }
                                } else {
                                    return '';
                                }
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function initTablaInversion() {
            tablaInversion.DataTable({
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
                        $(celdas[0]).css('text-align', 'left');
                    }
                },
                columns: [
                    { data: 'inversion', title: '' },
                    {
                        data: 'saldo', title: 'SALDO', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }

        function initTablaDocumento() {
            tablaDocumento.DataTable({
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
                        $(celdas[0]).css('text-align', 'left');
                    }
                },
                columns: [
                    { data: 'documento', title: '' },
                    {
                        data: 'tasa', title: 'TASA', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data > 0 ? (data + '%') : ``;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'totales', title: 'TOTALES', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                if (data != 0) {
                                    return data >= 0 ?
                                    '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                    `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                } else {
                                    return '';
                                }
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'anio', title: '', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                if (data != 0) {
                                    return data >= 0 ?
                                    '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                    `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                } else {
                                    return '';
                                }
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'anioMas1', title: '', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                if (data != 0) {
                                    return data >= 0 ?
                                    '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                    `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                } else {
                                    return '';
                                }
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'anioMas2', title: '', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                if (data != 0) {
                                    return data >= 0 ?
                                    '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                    `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                } else {
                                    return '';
                                }
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'anioMas3', title: '', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                if (data != 0) {
                                    return data >= 0 ?
                                    '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                    `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                } else {
                                    return '';
                                }
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'anioMas4', title: '', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                if (data != 0) {
                                    return data >= 0 ?
                                    '$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") :
                                    `<p style="color: red;">${('$' + (Math.trunc(parseFloat(data))).toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"))}</p>`;
                                } else {
                                    return '';
                                }
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });

            tablaDocumentoPuras.DataTable({
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
                        $(celdas[0]).css('text-align', 'left');
                    }
                },
                columns: [
                    { data: 'documento', title: 'ENTIDAD' },
                    { data: 'equipo', title: 'EQUIPO' },
                    { data: 'inicio', title: 'INICIO' },
                    { data: 'fin', title: 'FIN' },
                    {
                        data: 'renta', title: 'RENTA', render: function (data, type, row, meta) {
                            if (data == 0) {
                                return ''
                            } else {
                                return data > 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            }
                        }
                    },
                    { data: 'pendientes', title: 'PAGOS PENDIENTES' },
                    {
                        data: 'anio', title: '', render: function (data, type, row, meta) {
                            if (data == 0) {
                                return ''
                            } else {
                                return data > 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            }
                        }
                    },
                    {
                        data: 'anio2', title: '', render: function (data, type, row, meta) {
                            if (data == 0) {
                                return ''
                            } else {
                                return data > 0 ? maskNumero(data) : `<p style="color: red;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: 'dt-center', targets: '_all' }
                ]
            });
        }
        //#endregion

        (function init() {
            initInputs();
            initTableBalance();
            initTablaBanco();
            initTablaCuenta();
            initTablaObra();
            initTablaParteRelacionada();
            initTablaAlmacen();
            initTablaInversion();
            initTablaDocumento();

            cargarEncabezados();
            //cargarDatosPruebaGeneral();
            tblBalance.DataTable().columns.adjust().draw();
        })();

        $('.modal').on('shown.bs.modal', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        function cargarBalance() {
            let listaEmpresas = getValoresMultiples('#ftrSelectEmpresa');
            let fechaAnioMes = '01/' + ftrInputFechaCorte.val();
            let listaCC = null;
            if (ftrSelectCC.val()) {
                listaCC = ftrSelectCC.val();
            }
            let tipoBalance = ftrSelectTipo.val();

            axios.post('CalcularBalanceGeneral', { listaEmpresas, fechaAnioMes, listaCC, tipoBalance })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        cargarEncabezados();
                        AddRows(tblBalance, response.data.data);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function fncGetReporte() {
            var path = `/Reportes/Vista.aspx?idReporte=243`;
            $("#report").attr("src", path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function cargarDatosPruebaGeneral() {
            let datosPrueba = [
                { id: 0, concepto: 'BANCOS', corte: '0', corteAnterior: '0', variacion: '0', dolares: '0', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: true, tipoDetalle: 1 },
                { id: 0, concepto: 'CUENTAS POR COBRAR', corte: '0', corteAnterior: '0', variacion: '0', dolares: '0', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: true, tipoDetalle: 2 },
                { id: 0, concepto: 'OBRA POR APROBAR', corte: '0', corteAnterior: '0', variacion: '0', dolares: '0', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: true, tipoDetalle: 3 },
                { id: 0, concepto: 'PARTES RELACIONADAS', corte: '0', corteAnterior: '0', variacion: '0', dolares: '0', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: true, tipoDetalle: 4 },
                { id: 0, concepto: 'ALMACÉN DE MATERIALES', corte: '0', corteAnterior: '0', variacion: '0', dolares: '0', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: true, tipoDetalle: 5 },
                { id: 0, concepto: 'INVERSIÓN EN SUBSIDIARIAS', corte: '0', corteAnterior: '0', variacion: '0', dolares: '0', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: true, tipoDetalle: 6 },
                { id: 0, concepto: 'DOCUMENTOS POR PAGAR CP', corte: '0', corteAnterior: '0', variacion: '0', dolares: '0', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: true, tipoDetalle: 7 },
                { id: 0, concepto: 'A CORTO PLAZO', corte: '1', corteAnterior: '1', variacion: '1', dolares: '1', renglonSubTitulo: true, renglonGrupo: false, renglonEnlace: false, tipoDetalle: 0 },
                { id: 0, concepto: 'PROVEEDORES', corte: '-2', corteAnterior: '2', variacion: '2', dolares: '2', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: false, tipoDetalle: 0 },
                { id: 0, concepto: 'IMPUESTOS POR PAGAR', corte: '4', corteAnterior: '4', variacion: '-4', dolares: '4', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: false, tipoDetalle: 0 },
                { id: 0, concepto: 'COSTOS ESTIMADOS DE OBRAS', corte: '5', corteAnterior: '5', variacion: '5', dolares: '-5', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: false, tipoDetalle: 0 },
                { id: 0, concepto: 'TOTAL A CORTO PLAZO', corte: '7', corteAnterior: '7', variacion: '7', dolares: '7', renglonSubTitulo: false, renglonGrupo: true, renglonEnlace: false, tipoDetalle: 0 },
                { id: 0, concepto: 'A LARGO PLAZO', corte: '8', corteAnterior: '8', variacion: '8', dolares: '8', renglonSubTitulo: true, renglonGrupo: false, renglonEnlace: false, tipoDetalle: 0 },
                { id: 0, concepto: 'DOCUMENTOS POR PAGAR A LP', corte: '9', corteAnterior: '9', variacion: '9', dolares: '9', renglonSubTitulo: false, renglonGrupo: false, renglonEnlace: false, tipoDetalle: 0 },
                { id: 0, concepto: 'TOTAL A LARGO PLAZO', corte: '10', corteAnterior: '10', variacion: '10', dolares: '10', renglonSubTitulo: false, renglonGrupo: true, renglonEnlace: false, tipoDetalle: 0 },
                { id: 0, concepto: 'TOTAL ACTIVOS', corte: '11', corteAnterior: '11', variacion: '11', dolares: '11', renglonSubTitulo: false, renglonGrupo: true, renglonEnlace: false, tipoDetalle: 0 }
            ]

            AddRows(tblBalance, datosPrueba);
        }

        function cargarDatosPruebaDetalle(tipoDetalle) {
            let datosPrueba = [];

            switch (tipoDetalle) {
                case 1:
                    datosPrueba.push(
                        { concepto: 'COBROS A CLIENTES', mesMonto: '1', mesMontoResultado: '0', mesPorcentaje: '1', mesPorcentajeResultado: '0', acumuladoMonto: '1', acumuladoMontoResultado: '0', acumuladoPorcentaje: '1', acumuladoPorcentajeResultado: '0' },
                        { concepto: 'PAGOS A PROVEEDORES', mesMonto: '-1', mesMontoResultado: '0', mesPorcentaje: '-1', mesPorcentajeResultado: '0', acumuladoMonto: '-1', acumuladoMontoResultado: '0', acumuladoPorcentaje: '-1', acumuladoPorcentajeResultado: '0' },
                        { concepto: 'PAGOS A ARRENDADORA CONSTRUPLAN', mesMonto: '0', mesMontoResultado: '0', mesPorcentaje: '-2', mesPorcentajeResultado: '0', acumuladoMonto: '-2', acumuladoMontoResultado: '0', acumuladoPorcentaje: '-2', acumuladoPorcentajeResultado: '0' },
                        { concepto: 'PAGOS DE NOMINA', mesMonto: '-2', mesMontoResultado: '0', mesPorcentaje: '-3', mesPorcentajeResultado: '0', acumuladoMonto: '-3', acumuladoMontoResultado: '0', acumuladoPorcentaje: '-3', acumuladoPorcentajeResultado: '0' },
                        { concepto: 'PROVISIONAL ISR', mesMonto: '-3', mesMontoResultado: '0', mesPorcentaje: '-4', mesPorcentajeResultado: '0', acumuladoMonto: '-4', acumuladoMontoResultado: '0', acumuladoPorcentaje: '-4', acumuladoPorcentajeResultado: '0' },
                        { concepto: 'PAGO DE OTROS IMPUESTOS', mesMonto: '-4', mesMontoResultado: '0', mesPorcentaje: '-5', mesPorcentajeResultado: '0', acumuladoMonto: '-5', acumuladoMontoResultado: '0', acumuladoPorcentaje: '-5', acumuladoPorcentajeResultado: '0' },
                        { concepto: 'FLUJO NETO PROCEDENTE DE ACTIVIDADES DE OPERACIÓN', mesMonto: '0', mesMontoResultado: '1', mesPorcentaje: '0', mesPorcentajeResultado: '1', acumuladoMonto: '0', acumuladoMontoResultado: '1', acumuladoPorcentaje: '0', acumuladoPorcentajeResultado: '1', renglonGrupo: true }
                    )
                    AddRows(tablaBanco, datosPrueba);
                    break;
                case 2:
                    datosPrueba.push(
                        { cuenta: 'FYPASA', montoPesos: '1', montoDolares: '1' },
                        { cuenta: 'GRUPO MODELO', montoPesos: '2', montoDolares: '2' },
                        { cuenta: 'CEBADAS Y MALTAS S DE RL DE CV', montoPesos: '3', montoDolares: '3' },
                        { cuenta: 'PEPSICO', montoPesos: '4', montoDolares: '4' },
                        { cuenta: 'DIFA ARRENDADORA', montoPesos: '5', montoDolares: '5' }
                    )
                    AddRows(tablaCuenta, datosPrueba);
                    break;
                case 3:
                    datosPrueba.push(
                        { obra: 'MINADO LA HERRADURA', estimacion: '0', revision70: '1', revision30: '1' },
                        { obra: 'PATIOS HERRADURA XIV', estimacion: '0', revision70: '2', revision30: '2' },
                        { obra: 'PERSONAL PARA OPERACIÓN DE EQUIPOS', estimacion: '0', revision70: '3', revision30: '3' },
                        { obra: 'DEPÓSITO DE JALES SECOS 3° EXT', estimacion: '0', revision70: '4', revision30: '4' },
                        { obra: 'PATIOS LA YAQUI GRANDE', estimacion: '0', revision70: '5', revision30: '5' }
                    );
                    AddRows(tablaObra, datosPrueba);
                    break;
                case 4:
                    datosPrueba.push(
                        { parteRelacionada: 'CONSTRUPLAN E ICI ENERGÍA/PRÉSTAMO', saldo: '1' },
                        { parteRelacionada: 'CONSTRUPLAN E ICI ENERGÍA/CXC, RETENCIONES', saldo: '2' },
                        { parteRelacionada: 'CONSTRUPLAN ELÉCTRICA MB/PRÉSTAMO', saldo: '3' },
                        { parteRelacionada: 'CONSTRUPLAN ELÉCTRICA MB/CXC', saldo: '4' },
                        { parteRelacionada: 'I S M E', saldo: '5' },
                        { parteRelacionada: 'CONSOLIDADO', saldo: '6', renglonGrupo: true }
                    );
                    AddRows(tablaParteRelacionada, datosPrueba);
                    break;
                case 5:
                    datosPrueba.push(
                        { concepto: 'CONSTRUPLAN', saldo: '', renglonSubTitulo: true },
                        { concepto: 'ALMACÉN DE MATERIALES', saldo: '1' },
                        { concepto: 'MENOS:', saldo: '2' },
                        { concepto: 'RESERVA INSUMOS OBSOLETOS', saldo: '3' },
                        { concepto: 'SALDO NETO INVENTARIOS', saldo: '4', renglonGrupo: true },
                        { concepto: 'ARRENDADORA', saldo: '', renglonSubTitulo: true },
                        { concepto: 'ALMACÉN DE MATERIALES', saldo: '5' },
                        { concepto: 'MENOS:', saldo: '6' },
                        { concepto: 'RESERVA INSUMOS OBSOLETOS', saldo: '7' },
                        { concepto: 'SALDO NETO INVENTARIOS', saldo: '8', renglonGrupo: true }
                    );
                    AddRows(tablaAlmacen, datosPrueba);
                    break;
                case 6:
                    datosPrueba.push(
                        { concepto: 'I S M E', saldo: '1' },
                        { concepto: 'CONSTRUPLAN E ICI ENERGÍA', saldo: '2' },
                        { concepto: 'CONSTRUPLAN ELÉCTRICA MB', saldo: '3' },
                        { concepto: 'CONSTRUPLAN CORP', saldo: '4' },
                        { concepto: 'CONSOLIDADO', saldo: '5', renglonGrupo: true },
                        { concepto: 'ARRENDADORA CONSTRUPLAN', saldo: '6' },
                        { concepto: '', saldo: '7', renglonGrupo: true }
                    );
                    AddRows(tablaInversion, datosPrueba);
                    break;
                case 7:
                    datosPrueba.push(
                        { documento: 'BANAMEX SA (REVOLVENTES)', tasa: '1', totales: '1', anio: '1', anioMas1: '1', anioMas2: '1', anioMas3: '1', anioMas4: '1' },
                        { documento: 'SANTANDER (REVOLVENTES)', tasa: '2', totales: '2', anio: '2', anioMas1: '2', anioMas2: '2', anioMas3: '2', anioMas4: '2' },
                        { documento: 'BANCO BAJIO (REVOLVENTES)', tasa: '3', totales: '3', anio: '3', anioMas1: '3', anioMas2: '3', anioMas3: '3', anioMas4: '3' },
                        { documento: 'SCOTIABANK (REVOLVENTES)', tasa: '4', totales: '4', anio: '4', anioMas1: '4', anioMas2: '4', anioMas3: '4', anioMas4: '4' },
                        { documento: 'BANORTE (REVOLVENTES)', tasa: '5', totales: '5', anio: '5', anioMas1: '5', anioMas2: '5', anioMas3: '5', anioMas4: '5' },
                        { documento: 'TOTAL MONEDA NACIONAL', tasa: '6', totales: '6', anio: '6', anioMas1: '6', anioMas2: '6', anioMas3: '6', anioMas4: '6', renglonGrupo: true }
                    );
                    AddRows(tablaDocumento, datosPrueba);
                    break;
            }
        }

        function cargarEncabezados() {
            let tipo = ftrSelectTipo.val();
            let tipoNombre = '';

            switch (tipo) {
                case '1':
                    tipoNombre = 'ACTIVOS';
                    break;
                case '2':
                    tipoNombre = 'PASIVOS';
                    break;
                default:
                    tipoNombre = '';
                    break;
            }

            let mesAnio = getMesAnio();

            $(tblBalance.DataTable().column(0).header()).text(tipoNombre);
            $(tblBalance.DataTable().column(1).header()).text(mesAnio.mesNombre + ' ' + mesAnio.anio);
            $(tblBalance.DataTable().column(2).header()).text(mesAnio.mesNombreAnterior + ' ' + (mesAnio.anioAnterior_));
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
            let mes = ftrInputFechaCorte.val();
            let listaStringMes = mes.split('/');
            let mesNombre = listaMeses.filter((x) => x.mes == parseInt(listaStringMes[0]))[0].nombre;
            let anio = parseInt(listaStringMes[1]);
            let mesNombreAnterior = '';
            if (parseInt(listaStringMes[0]) - 1 <= 0) {
                mesNombreAnterior = 'DICIEMBRE';
            } else {
                mesNombreAnterior = listaMeses.filter((x) => x.mes == parseInt(listaStringMes[0] - 1))[0].nombre;
            }

            let anioAnterior_ = 0;
            if (mesNombre == "ENERO") {
                anioAnterior_ = anio - 1;
            } else {
                anioAnterior_ = anio;
            }

            return { mesNombre, anio, mesNombreAnterior, anioAnterior_ };
        }

        function cargarIndicadores() {
            let listaEmpresas = getValoresMultiples('#ftrSelectEmpresa');
            let fechaMesCorte = '01/' + ftrInputFechaCorte.val();
            let listaCC = null;
            if (ftrSelectCC.val()) {
                listaCC = ftrSelectCC.val();
            }
            let tipoBalance = ftrSelectTipo.val();

            axios.post('GetBalanceDetalle', { listaEmpresas, fechaMesCorte, listaCC, tipoDetalle: 8, tipoTablaGeneral: 0 })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        initGfxIndicadores(response.data);
                        modalIndicadores.modal('show');
                    } else {
                        AlertaGeneral('Alerta', message);
                    }
                }).catch(error => AlertaGeneral('Alerta', error.message));
        }

        function initGfxIndicadores(data) {
            // Highcharts.chart('gfxIndicadoresRazonCirculante', {
            //     title: {
            //         text: 'Razón del Circulante'
            //     },

            //     credits: {
            //         enabled: false
            //     },
            
            //     subtitle: {
            //         text: 'AC/PC'
            //     },
            
            //     yAxis: {
            //         title: {
            //             text: ''
            //         }
            //     },
            
            //     xAxis: data.gfxRazonCirculante.xAxis,
            
            //     // legend: {
            //     //     layout: 'vertical',
            //     //     align: 'right',
            //     //     verticalAlign: 'middle'
            //     // },
            
            //     // plotOptions: {
            //     //     series: {
            //     //         label: {
            //     //             connectorAllowed: false
            //     //         },
            //     //         pointStart: 2010
            //     //     }
            //     // },
            
            //     series: data.gfxRazonCirculante.series,
            
            //     responsive: {
            //         rules: [{
            //             condition: {
            //                 maxWidth: 500
            //             },
            //             chartOptions: {
            //                 legend: {
            //                     layout: 'horizontal',
            //                     align: 'center',
            //                     verticalAlign: 'bottom'
            //                 }
            //             }
            //         }]
            //     }
            // });

            // Highcharts.chart('gfxIndicadoresDeudaCapitalNeto', {
            //     title: {
            //         text: 'De Deuda a Capital Neto'
            //     },

            //     credits: {
            //         enabled: false
            //     },
            
            //     subtitle: {
            //         text: 'PT/CNC'
            //     },
            
            //     yAxis: {
            //         title: {
            //             text: ''
            //         }
            //     },
            
            //     xAxis: data.gfxDeudaCapitalNeto.xAxis,
            
            //     // legend: {
            //     //     layout: 'vertical',
            //     //     align: 'right',
            //     //     verticalAlign: 'middle'
            //     // },
            
            //     // plotOptions: {
            //     //     series: {
            //     //         label: {
            //     //             connectorAllowed: false
            //     //         },
            //     //         pointStart: 2010
            //     //     }
            //     // },
            
            //     series: data.gfxDeudaCapitalNeto.series,
            
            //     responsive: {
            //         rules: [{
            //             condition: {
            //                 maxWidth: 500
            //             },
            //             chartOptions: {
            //                 legend: {
            //                     layout: 'horizontal',
            //                     align: 'center',
            //                     verticalAlign: 'bottom'
            //                 }
            //             }
            //         }]
            //     }
            // });

            // Highcharts.chart('gfxIndicadoresPruebaAcido', {
            //     title: {
            //         text: 'Prueba del Acido'
            //     },

            //     credits: {
            //         enabled: false
            //     },
            
            //     subtitle: {
            //         text: '(AC-INV)/PC'
            //     },
            
            //     yAxis: {
            //         title: {
            //             text: ''
            //         }
            //     },
            
            //     xAxis: data.gfxPruebaAcido.xAxis,
            
            //     // legend: {
            //     //     layout: 'vertical',
            //     //     align: 'right',
            //     //     verticalAlign: 'middle'
            //     // },
            
            //     // plotOptions: {
            //     //     series: {
            //     //         label: {
            //     //             connectorAllowed: false
            //     //         },
            //     //         pointStart: 2010
            //     //     }
            //     // },
            
            //     series: data.gfxPruebaAcido.series,
            
            //     responsive: {
            //         rules: [{
            //             condition: {
            //                 maxWidth: 500
            //             },
            //             chartOptions: {
            //                 legend: {
            //                     layout: 'horizontal',
            //                     align: 'center',
            //                     verticalAlign: 'bottom'
            //                 }
            //             }
            //         }]
            //     }
            // });

            // Highcharts.chart('gfxIndicadoresRazonEndeudamiento', {
            //     title: {
            //         text: 'Razón de Endeudamiento'
            //     },

            //     credits: {
            //         enabled: false
            //     },
            
            //     subtitle: {
            //         text: 'PT/AT'
            //     },
            
            //     yAxis: {
            //         title: {
            //             text: ''
            //         },
            //         labels: {
            //             //format: '{text}%'
            //         }
            //     },
            
            //     xAxis: data.gfxRazonEndeudamiento.xAxis,
            
            //     // legend: {
            //     //     layout: 'vertical',
            //     //     align: 'right',
            //     //     verticalAlign: 'middle'
            //     // },
            
            //     // plotOptions: {
            //     //     series: {
            //     //         label: {
            //     //             connectorAllowed: false
            //     //         },
            //     //         pointStart: 2010
            //     //     }
            //     // },
            
            //     series: data.gfxRazonEndeudamiento.series,
            
            //     responsive: {
            //         rules: [{
            //             condition: {
            //                 maxWidth: 500
            //             },
            //             chartOptions: {
            //                 legend: {
            //                     layout: 'horizontal',
            //                     align: 'center',
            //                     verticalAlign: 'bottom'
            //                 }
            //             }
            //         }]
            //     }
            // });

            Highcharts.chart('gfxEbitda', {
                title: {
                    text: 'EBITDA'
                },

                credits: {
                    enabled: false
                },
            
                subtitle: {
                    text: ''
                },
            
                yAxis: {
                    title: {
                        text: ''
                    },
                    labels: {
                        //format: '{text}%'
                    }
                },
            
                xAxis: data.gfxEbitda.xAxis,
            
                // legend: {
                //     layout: 'vertical',
                //     align: 'right',
                //     verticalAlign: 'middle'
                // },
            
                // plotOptions: {
                //     series: {
                //         label: {
                //             connectorAllowed: false
                //         },
                //         pointStart: 2010
                //     }
                // },
            
                series: data.gfxEbitda.series,
            
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
                }
            });

            Highcharts.chart('gfxDeudaEbitda', {
                title: {
                    text: 'Relación Deuda/Ebitda'
                },

                credits: {
                    enabled: false
                },
            
                subtitle: {
                    text: ''
                },
            
                yAxis: {
                    title: {
                        text: ''
                    },
                    labels: {
                        //format: '{text}%'
                    }
                },
            
                xAxis: data.gfxDeudaEbitda.xAxis,
            
                // legend: {
                //     layout: 'vertical',
                //     align: 'right',
                //     verticalAlign: 'middle'
                // },
            
                // plotOptions: {
                //     series: {
                //         label: {
                //             connectorAllowed: false
                //         },
                //         pointStart: 2010
                //     }
                // },
            
                series: data.gfxDeudaEbitda.series,
            
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
                }
            });
        }

        function cargarInformacionModal(modal, nombreDetalle, rowData) {
            let listaEmpresas = getValoresMultiples('#ftrSelectEmpresa');
            let fechaMesCorte = '01/' + ftrInputFechaCorte.val();
            let listaCC = getValoresMultiples('#ftrSelectCC');
            let tipoTablaGeneral = +ftrSelectTipo.val();

            axios.post('GetBalanceDetalle', { listaEmpresas, fechaMesCorte, listaCC, tipoDetalle: rowData.tipoDetalle, tipoTablaGeneral })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows($('#tabla' + nombreDetalle), response.data.data);

                        if (nombreDetalle == 'Documento') {
                            AddRows($('#tablaDocumentoPuras'), response.data.dxpPuro);
                        }

                        if (rowData.tipoDetalle == 1) {
                            let mesAnio = getMesAnio();

                            $('#tablaBanco_wrapper').find('thead tr:eq(0) th:eq(1)').addClass('text-center').text(`${mesAnio.mesNombre} ${mesAnio.anio}`);
                            $('#tablaBanco_wrapper').find('thead tr:eq(0) th:eq(2)').addClass('text-center').text(`ACU. ${mesAnio.mesNombre} ${mesAnio.anio}`);
                        }

                        if (rowData.tipoDetalle == 7) {
                            let mesAnio = getMesAnio();

                            tablaBanco.find('thead tr:eq(0) th:eq(3)').addClass('text-center').text(`${mesAnio.anio}`);
                            tablaBanco.find('thead tr:eq(0) th:eq(4)').addClass('text-center').text(`${mesAnio.anio + 1}`);
                            tablaBanco.find('thead tr:eq(0) th:eq(5)').addClass('text-center').text(`${mesAnio.anio + 2}`);
                            tablaBanco.find('thead tr:eq(0) th:eq(6)').addClass('text-center').text(`${mesAnio.anio + 3}`);
                            tablaBanco.find('thead tr:eq(0) th:eq(7)').addClass('text-center').text(`${mesAnio.anio + 4}`);
                        }

                        // cargarDatosPruebaDetalle(rowData.tipoDetalle);

                        $(modal).modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }
    }
    $(document).ready(() => Administrativo.Contabilidad.EstadoResultado = new BalanceGeneral())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();