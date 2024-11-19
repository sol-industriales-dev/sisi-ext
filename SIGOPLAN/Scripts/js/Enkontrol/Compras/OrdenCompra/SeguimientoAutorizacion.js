(() => {
    $.namespace('Enkontrol.OrdenCompra.SeguimientoAutorizacion');
    SeguimientoAutorizacion = function () {
        //#region Selectores
        const selectCCAut = $('#selectCCAut');
        const tblComprasAut = $('#tblComprasAut');
        const mdlDetalleOC = $('#mdlDetalleOC');
        const mdlDetalleVobo = $('#mdlDetalleVobo');
        const mdlDetalleAutoriza = $('#mdlDetalleAutoriza');
        const tblVobo = $('#tblVobo');
        const tblAutoriza = $('#tblAutoriza');
        const lstVoboPendientes = $('#lstVoboPendientes');
        const fieldsetVobo = $('#fieldsetVobo');
        const labelRequisicionesPendientes = $('#labelRequisicionesPendientes');
        const labelVobosTotal = $('#labelVobosTotal');
        const labelVobosValidados = $('#labelVobosValidados');
        const selectFolioCuadro = $('#selectFolioCuadro');
        const mdlCuadroComparativo = $('#mdlCuadroComparativo');
        const tblPartidasCuadro = $('#tblPartidasCuadro');
        const tblUltimaCompra = $('#tblUltimaCompra');
        const botonBuscar = $('#botonBuscar');
        const selectAreaCuenta = $('#selectAreaCuenta');
        const idEmpresa = $('#idEmpresa');


        //#region Panel Derecho Cuadro
        const divPanelDerecho = $('#divPanelDerecho');
        const inputProv1Num = $('#inputProv1Num');
        const inputProv1Desc = $('#inputProv1Desc');
        const inputProv2Num = $('#inputProv2Num');
        const inputProv2Desc = $('#inputProv2Desc');
        const inputProv3Num = $('#inputProv3Num');
        const inputProv3Desc = $('#inputProv3Desc');

        const inputPrimerSubtotalProv1Num = $('#inputPrimerSubtotalProv1Num');
        const inputPrimerSubtotalProv1Moneda = $('#inputPrimerSubtotalProv1Moneda');
        const inputPrimerSubtotalProv2Num = $('#inputPrimerSubtotalProv2Num');
        const inputPrimerSubtotalProv2Moneda = $('#inputPrimerSubtotalProv2Moneda');
        const inputPrimerSubtotalProv3Num = $('#inputPrimerSubtotalProv3Num');
        const inputPrimerSubtotalProv3Moneda = $('#inputPrimerSubtotalProv3Moneda');

        const inputDescuentoProv1 = $('#inputDescuentoProv1');
        const inputDescuentoProv2 = $('#inputDescuentoProv2');
        const inputDescuentoProv3 = $('#inputDescuentoProv3');

        const inputSegundoSubtotalProv1Num = $('#inputSegundoSubtotalProv1Num');
        const inputSegundoSubtotalProv1Moneda = $('#inputSegundoSubtotalProv1Moneda');
        const inputSegundoSubtotalProv2Num = $('#inputSegundoSubtotalProv2Num');
        const inputSegundoSubtotalProv2Moneda = $('#inputSegundoSubtotalProv2Moneda');
        const inputSegundoSubtotalProv3Num = $('#inputSegundoSubtotalProv3Num');
        const inputSegundoSubtotalProv3Moneda = $('#inputSegundoSubtotalProv3Moneda');

        const inputIVAProv1 = $('#inputIVAProv1');
        const inputIVAProv2 = $('#inputIVAProv2');
        const inputIVAProv3 = $('#inputIVAProv3');

        const inputTotalProv1Num = $('#inputTotalProv1Num');
        const inputTotalProv1Moneda = $('#inputTotalProv1Moneda');
        const inputTotalProv2Num = $('#inputTotalProv2Num');
        const inputTotalProv2Moneda = $('#inputTotalProv2Moneda');
        const inputTotalProv3Num = $('#inputTotalProv3Num');
        const inputTotalProv3Moneda = $('#inputTotalProv3Moneda');

        const inputFletesProv1Num = $('#inputFletesProv1Num');
        const inputFletesProv1Moneda = $('#inputFletesProv1Moneda');
        const inputFletesProv2Num = $('#inputFletesProv2Num');
        const inputFletesProv2Moneda = $('#inputFletesProv2Moneda');
        const inputFletesProv3Num = $('#inputFletesProv3Num');
        const inputFletesProv3Moneda = $('#inputFletesProv3Moneda');

        const inputImportacionProv1Num = $('#inputImportacionProv1Num');
        const inputImportacionProv1Moneda = $('#inputImportacionProv1Moneda');
        const inputImportacionProv2Num = $('#inputImportacionProv2Num');
        const inputImportacionProv2Moneda = $('#inputImportacionProv2Moneda');
        const inputImportacionProv3Num = $('#inputImportacionProv3Num');
        const inputImportacionProv3Moneda = $('#inputImportacionProv3Moneda');

        const inputGranTotalProv1Num = $('#inputGranTotalProv1Num');
        const inputGranTotalProv1Moneda = $('#inputGranTotalProv1Moneda');
        const inputGranTotalProv2Num = $('#inputGranTotalProv2Num');
        const inputGranTotalProv2Moneda = $('#inputGranTotalProv2Moneda');
        const inputGranTotalProv3Num = $('#inputGranTotalProv3Num');
        const inputGranTotalProv3Moneda = $('#inputGranTotalProv3Moneda');

        const inputTipoCambioProv1 = $('#inputTipoCambioProv1');
        const inputTipoCambioProv2 = $('#inputTipoCambioProv2');
        const inputTipoCambioProv3 = $('#inputTipoCambioProv3');

        const inputFechaEntregaProv1 = $('#inputFechaEntregaProv1');
        const inputFechaEntregaProv2 = $('#inputFechaEntregaProv2');
        const inputFechaEntregaProv3 = $('#inputFechaEntregaProv3');

        const inputLABProv1Num = $('#inputLABProv1Num');
        const inputLABProv1Desc = $('#inputLABProv1Desc');
        const inputLABProv2Num = $('#inputLABProv2Num');
        const inputLABProv2Desc = $('#inputLABProv2Desc');
        const inputLABProv3Num = $('#inputLABProv3Num');
        const inputLABProv3Desc = $('#inputLABProv3Desc');

        const inputCondPagoProv1 = $('#inputCondPagoProv1');
        const inputCondPagoProv2 = $('#inputCondPagoProv2');
        const inputCondPagoProv3 = $('#inputCondPagoProv3');

        const textAreaComentarioProv1 = $('#textAreaComentarioProv1');
        const textAreaComentarioProv2 = $('#textAreaComentarioProv2');
        const textAreaComentarioProv3 = $('#textAreaComentarioProv3');
        const inputEmpresaActual = $('#inputEmpresaActual');
        //#endregion
        //#endregion

        _compra = {};
        counReq = 0;
        counReqDes = 0;
        _cc = '';
        _numeroRequisicion = 0;
        _empresaActual = +inputEmpresaActual.val();

        const getVobos = (compra) => $.post('/Enkontrol/OrdenCompra/GetVobos', { compra });
        const getAutorizaciones = (compra) => $.post('/Enkontrol/OrdenCompra/GetAutorizaciones', { compra });
        const getCuadroDet = (cuadro) => $.post('/Enkontrol/OrdenCompra/GetCuadroDet', cuadro);
        const getUltimaCompra = (partidaCuadro) => $.post('/Enkontrol/OrdenCompra/GetUltimaCompra', { partidaCuadro: partidaCuadro });

        (function init() {
            $.fn.dataTable.moment('DD/MM/YYYY');

            verificarComprasPeru();
            initCbo();
            initTableCompras();
            initTableVobo();
            initTableAutoriza();

            initTablePartidasCuadro();
            initTableUltimaCompra();
            OcultarColumnaAreaCuenta();
            // cargarCompras();
            // getContadorRequisicionesPendientes();

            botonBuscar.click(cargarCompras);
            // selectCCAut.change(cargarCompras);
        })();

        $('.nav-tabs a').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });

        selectFolioCuadro.on('change', function () {
            if (selectFolioCuadro.val() != '') {
                getCuadroDetalle({ cc: _cc, numero: _numeroRequisicion, folio: selectFolioCuadro.val() });
            } else {
                limpiarCuadro();
            }
        });

        selectCCAut.click(function () {
            $("#selectCCAut > option").each(function () {
                let val = $(this).val();
                let option = $("#selectCCAut").find(`option[value="${val}"]`);
                let prefijo = option.attr("data-prefijo");
                if (prefijo == "OC_INTERNA") {
                    option.attr("class", "OC_INTERNA");
                    option.attr("title", "OC INTERNA.");
                }
            });
            $(".OC_INTERNA").css("background", "#bfbfbf")
        })

        function initCbo() {
            selectCCAut.fillCombo('/Enkontrol/OrdenCompra/FillComboCcAutTodas', null, false, '--Todos--');
            selectAreaCuenta.fillCombo('/Enkontrol/OrdenCompra/FillComboAreaCuentaTodas', null, false, '--Todos--');


        }

        function OcultarColumnaAreaCuenta() {
            if (idEmpresa.val() == 6) {
                tblComprasAut.DataTable().column(4).visible(false);
            } else {
                tblComprasAut.DataTable().column(4).visible(true);
            }
        }
        function initTableCompras() {
            tblComprasAut.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: true,
                language: dtDicEsp,
                aaSorting: [5, 'desc'],
                rowId: 'id',
                scrollY: "45vh",
                scrollCollapse: true,
                bLengthChange: false,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblComprasAut.on('click', '.btn-detalle-oc', function () {
                        let rowData = tblComprasAut.DataTable().row($(this).closest('tr')).data();

                        let esOC_INTERNA = false;
                        let option = selectCCAut.find(`option[value="${selectCCAut.val()}"]`);
                        let prefijo = option.attr("data-prefijo");
                        if (prefijo == "OC_INTERNA") {
                            esOC_INTERNA = true;
                        }

                        inputNumero.data().PERU_tipoCompra = rowData.PERU_tipoCompra;
                        // selectCC.val(rowData.cc);
                        selectCC.attr('cc', rowData.cc);
                        selectCC.append($("<option>").val(rowData.cc).text(rowData.cc));

                        inputNumero.val(rowData.numero);
                        inputNumero.data().esOC_Interna = esOC_INTERNA;
                        inputNumero.change();

                        mdlDetalleOC.modal('show');
                        recargarHeaders();

                        $('#btnGuardarCambios').css('display', 'none');
                    });

                    tblComprasAut.on('click', '.btn-detalle-vobo', function () {
                        let rowData = tblComprasAut.DataTable().row($(this).closest('tr')).data();

                        _compra = rowData;

                        fieldsetVobo.css('display', 'none');

                        getVobos(rowData).done(response => {
                            if (response.success) {
                                if (response.data.length > 0) {
                                    let vobosTotales = response.data[0].numVobos;
                                    let vobosValidados = 0;

                                    if (response.vobosCapturados[0].usu_numero > 0) {
                                        vobosValidados++;
                                    }

                                    if (response.vobosCapturados[1].usu_numero > 0) {
                                        vobosValidados++;
                                    }

                                    if (response.vobosCapturados[2].usu_numero > 0) {
                                        vobosValidados++;
                                    }

                                    labelVobosTotal.text('VoBos Requeridos: ' + vobosTotales);
                                    labelVobosValidados.text('VoBos Validados: ' + vobosValidados);

                                    let flagVoboPendiente = false;

                                    if (vobosTotales > vobosValidados) {
                                        flagVoboPendiente = true;
                                    }

                                    response.data.forEach(function (element) {
                                        let check = response.vobosCapturados.some(function (x) {
                                            return x.usu_numero == element.usu_numero;
                                        });

                                        if (check) {
                                            element.estatusVobo = 'VALIDADO';
                                        } else {
                                            if (flagVoboPendiente) {
                                                element.estatusVobo = 'PENDIENTE';
                                            } else {
                                                element.estatusVobo = '';
                                            }
                                        }
                                    });

                                    AddRows(tblVobo, response.data);

                                    limpiarLstVoboPendientes();

                                    let countVobo = 0;
                                    for (let i = 0; i < response.data[0].numVobos; i++) {
                                        if (response.vobosCapturados[i].usu_numero == 0 && countVobo == 0) {
                                            AddFilaVobo(i + 1, response.vobosCapturados[i], response.flagPuedeDarVobo);
                                            countVobo++;
                                        } else {
                                            AddFilaVobo(i + 1, response.vobosCapturados[i], false);
                                        }
                                    }

                                    mdlDetalleVobo.modal('show');
                                } else {
                                    AlertaGeneral('Alerta', 'No se encontró información.');
                                }
                            } else {
                                AlertaGeneral('Alerta', 'Error al consultar la información.');
                            }
                        });
                    });

                    tblComprasAut.on('click', '.btn-detalle-autoriza', function () {
                        let rowData = tblComprasAut.DataTable().row($(this).closest('tr')).data();

                        getAutorizaciones(rowData).done(response => {
                            if (response.success) {
                                if (response.data.length > 0) {
                                    AddRows(tblAutoriza, response.data);

                                    mdlDetalleAutoriza.modal('show');
                                } else {
                                    AlertaGeneral('Alerta', 'No se encontró información.');
                                }
                            } else {
                                AlertaGeneral('Alerta', 'Error al consultar la información.');
                            }
                        });
                    });

                    tblComprasAut.on('click', '.btn-cuadro-comparativo', function () {
                        let rowData = tblComprasAut.DataTable().row($(this).closest('tr')).data();

                        selectFolioCuadro.fillCombo('/Enkontrol/OrdenCompra/FillComboFolioCuadro', { cuadrosExistentes: rowData.countCuadroComparativo }, false);
                        selectFolioCuadro.val(1);

                        _cc = rowData.cc;
                        _numeroRequisicion = rowData.numeroRequisicion;

                        mdlCuadroComparativo.modal('show');

                        getCuadroDetalle({ cc: rowData.cc, numero: rowData.numeroRequisicion, folio: 1 });
                    });
                },
                rowCallback: function (row, data) {
                    if (data.voboPendiente) {
                        $(row).addClass('voboPendiente');
                    }
                },
                columns: [
                    {
                        title: 'Centro de Costo', render: function (data, type, row, meta) {
                            return row.cc + ' - ' + row.ccDesc;
                        }
                    },
                    { data: 'numero', title: 'Número' },
                    { data: 'compradorNom', title: 'Comprador' },
                    { data: 'numeroRequisicion', title: 'Número de Requisición' },
                    { data: 'areaCuentaDesc', title: 'Área-Cuenta' },
                    { data: 'proveedorNom', title: 'Proveedor' },
                    { data: 'total', title: 'Total' },
                    {
                        data: 'moneda', render: (data, type, row, meta) => {
                            switch (data) {
                                case 1:
                                    return 'Pesos';
                                case 2:
                                    return 'Dólares';
                                case 4:
                                    return "Soles";
                                default:
                                    return data;
                            }
                        }, title: 'Moneda'
                    },
                    { data: 'fecha', title: 'Fecha' },
                    {
                        data: 'vobosString', title: 'VoBo',
                        render: (data, type, row) => {
                            if (row.vobosString) {
                                let stringVobosColores = '';
                                let listaVobosSeparados = data.split(', ');

                                for (let i = 0; i < listaVobosSeparados.length; i++) {
                                    const element = listaVobosSeparados[i];

                                    let listaEmpleadoColor = element.split('|');
                                    let empleado = listaEmpleadoColor[0];
                                    let color = listaEmpleadoColor[1];

                                    stringVobosColores += `<span style="color: ${color}">${empleado}</span>`;

                                    if (i != listaVobosSeparados.length - 1) {
                                        stringVobosColores += `<span>, </span>`;
                                    }
                                }

                                return stringVobosColores;

                                // let arrVobos = data.split("|");
                                // let color = "";
                                // arrVobos.forEach(element => {
                                //     color = element;
                                // });

                                // let voboStr = data;
                                // voboStr = voboStr.replace("|", "");
                                // voboStr = voboStr.replace("red", "");
                                // voboStr = voboStr.replace("green", "");
                                // voboStr = voboStr.replace("|red", "");
                                // voboStr = voboStr.replace("|green", "");

                                // return `<span style="color: ${color}">${voboStr}</span>`;
                            } else {
                                return "";
                            }
                        }
                    },
                    { data: 'autorizacionesString', title: 'Autorización' },
                    {
                        sortable: false,
                        render: (data, type, row, meta) => {
                            let div = document.createElement('div');

                            div.appendChild(crearBoton('btn-default', 'btn-detalle-oc', 'glyphicon-list', 'Detalle O.C.'));
                            div.appendChild(crearBoton('btn-default', 'btn-detalle-vobo', 'glyphicon-eye-open', 'Detalle VoBo'));
                            div.appendChild(crearBoton('btn-default', 'btn-detalle-autoriza', 'glyphicon-option-horizontal', 'Detalle Autoriza'));

                            let botonCuadroComparativo = crearBoton('btn-default', 'btn-cuadro-comparativo', 'glyphicon-th', 'Cuadro Comparativo');

                            if (!row.tieneCuadro) {
                                $(botonCuadroComparativo).attr('disabled', true);
                            }

                            div.appendChild(botonCuadroComparativo);

                            return div.outerHTML;
                        },
                        title: ''
                    }
                ],
                columnDefs: [
                    { "width": "10%", "targets": 11 },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [8]
                    },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                if (_empresaActual == 6) {
                                    if (row.moneda == 2) {
                                        return maskNumero(data);

                                    } else {
                                        return maskNumero2DCompras_PERU(data);

                                    }

                                } else {
                                    return maskNumero(data);

                                }
                            }
                        },
                        targets: [6]
                    },
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableVobo() {
            tblVobo.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                rowId: 'id',
                scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {
                    if (rowData.estatusVobo == 'VALIDADO') {
                        $(row).addClass('voboValidado');
                    } else if (rowData.estatusVobo == 'PENDIENTE') {
                        $(row).addClass('voboPendiente');
                    }
                },
                columns: [
                    { data: 'usu_numero', title: 'Empleado' },
                    { data: 'usu_nombre', title: 'Descripción' },
                    { data: 'estatusVobo', title: 'VoBo' }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableAutoriza() {
            tblAutoriza.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                "language": dtDicEsp,
                "aaSorting": [0, 'asc'],
                rowId: 'id',
                "scrollCollapse": true,
                bInfo: false,
                'initComplete': function (settings, json) {

                },
                columns: [
                    { data: 'usu_numero', title: 'Empleado' },
                    { data: 'usu_nombre', title: 'Descripción' }
                ],
                columnDefs: [
                    { "className": "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarCompras() {
            let area = 0;
            let cuenta = 0;

            if (selectAreaCuenta.val() != '--Todos--') {
                area = +selectAreaCuenta.find('option:selected').val();
                cuenta = +selectAreaCuenta.find('option:selected').attr('data-prefijo');
            }

            // SE VERIFICA SI EL CC ES DE UNA OC_INTERNA
            let esOC_INTERNA = false;
            let option = selectCCAut.find(`option[value="${selectCCAut.val()}"]`);
            let prefijo = option.attr("data-prefijo");
            if (prefijo == "OC_INTERNA") {
                esOC_INTERNA = true;
            }

            axios.post('/Enkontrol/OrdenCompra/GetListaComprasTodas', { cc: selectCCAut.val(), pendientes: false, propias: true, area, cuenta })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        if (response.data.data.length > 0) {
                            AddRows(tblComprasAut, response.data.data);
                            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                            counReq = 0;
                        } else {
                            limpiarTabla(tblComprasAut);
                            AlertaGeneral('Alerta', 'No se encontró información.');
                        }
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function limpiarTabla(tbl) {
            dt = tbl.DataTable();
            dt.clear().draw();
        }

        function AddFilaVobo(index, vobo, flagBtnVobo) {
            let div = document.createElement('div');
            let label = document.createElement('label');
            let inputNumero = document.createElement('input');
            let inputNombre = document.createElement('input');

            $(inputNumero).addClass('form-control filaVoboNumero');
            $(inputNombre).addClass('form-control filaVoboNombre');

            $(inputNumero).attr('disabled', true);
            $(inputNombre).attr('disabled', true);

            $(label).text('Vobo ' + (index) + ': ');
            $(inputNumero).val(vobo.usu_numero != 0 ? vobo.usu_numero : '');
            $(inputNombre).val(vobo.usu_nombre);

            $(div).append(label);
            $(div).append(inputNumero);
            $(div).append(inputNombre);

            lstVoboPendientes.append(div);
        }

        function limpiarLstVoboPendientes() {
            lstVoboPendientes.empty();
        }

        function fnAgregarTooltip(elemento, mensaje) {
            $(elemento).attr('data-toggle', 'tooltip');
            $(elemento).attr('data-placement', 'top');

            if (mensaje != "") {
                $(elemento).attr('title', mensaje);
            }

            $('[data-toggle="tooltip"]').tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top+8",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .appendTo(this);
                    }
                }
            });
        }

        function crearBoton(claseColor, claseCustom, iconoCustom, tooltipMensaje) {
            let btn = document.createElement('button');
            btn.classList.add('btn', 'btn-xs', claseColor, claseCustom);

            let span = document.createElement('span');
            span.classList.add('glyphicon', iconoCustom);

            btn.appendChild(span);

            fnAgregarTooltip(btn, tooltipMensaje);

            return btn;
        }

        function recargarHeaders() {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        }

        function getContadorRequisicionesPendientes() {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/GetContadorRequisicionesPendientes')
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        labelRequisicionesPendientes.text('Requisiciones sin O.C.: ' + response.data);
                    } else {
                        labelRequisicionesPendientes.text('');
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function getCuadroDetalle(cuadro) {
            getCuadroDet(cuadro).done(function (response) {
                if (response.success) {
                    limpiarCuadro();

                    let cuadro = response.data;
                    let cuadroDetalle = response.data.detalleCuadro;

                    if (cuadro.tieneCuadro) {
                        llenarPanelDerechoProv(cuadro);
                        llenarTablaPartidas(cuadro, cuadroDetalle);
                        tblPartidasCuadro.find('tbody tr:eq(0)').click();
                    } else {
                        llenarTablaPartidas(cuadro, cuadroDetalle);
                        tblPartidasCuadro.find('tbody tr:eq(0)').click();
                    }
                } else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                    limpiarCuadro();
                }
            }).always(function () {
                tblPartidasCuadro.DataTable().columns.adjust();
                tblUltimaCompra.DataTable().columns.adjust();
            });
        }

        function limpiarCuadro() {
            inputProv1Num.val('');
            inputProv1Desc.val('');
            quitarTooltip(inputProv1Desc);
            inputProv2Num.val('');
            inputProv2Desc.val('');
            quitarTooltip(inputProv2Desc);
            inputProv3Num.val('');
            inputProv3Desc.val('');
            quitarTooltip(inputProv3Desc);

            inputPrimerSubtotalProv1Num.val('$0.00');
            inputPrimerSubtotalProv1Moneda.val('');
            inputPrimerSubtotalProv2Num.val('$0.00');
            inputPrimerSubtotalProv2Moneda.val('');
            inputPrimerSubtotalProv3Num.val('$0.00');
            inputPrimerSubtotalProv3Moneda.val('');

            inputDescuentoProv1.val('0.00');
            inputDescuentoProv2.val('0.00');
            inputDescuentoProv3.val('0.00');

            inputSegundoSubtotalProv1Num.val('$0.00');
            inputSegundoSubtotalProv1Moneda.val('');
            inputSegundoSubtotalProv2Num.val('$0.00');
            inputSegundoSubtotalProv2Moneda.val('');
            inputSegundoSubtotalProv3Num.val('$0.00');
            inputSegundoSubtotalProv3Moneda.val('');

            inputIVAProv1.val('');
            inputIVAProv2.val('');
            inputIVAProv3.val('');

            inputTotalProv1Num.val('$0.00');
            inputTotalProv1Moneda.val('');
            inputTotalProv2Num.val('$0.00');
            inputTotalProv2Moneda.val('');
            inputTotalProv3Num.val('$0.00');
            inputTotalProv3Moneda.val('');

            inputFletesProv1Num.val('$0.00');
            inputFletesProv1Moneda.val('');
            inputFletesProv2Num.val('$0.00');
            inputFletesProv2Moneda.val('');
            inputFletesProv3Num.val('$0.00');
            inputFletesProv3Moneda.val('');

            inputImportacionProv1Num.val('$0.00');
            inputImportacionProv1Moneda.val('');
            inputImportacionProv2Num.val('$0.00');
            inputImportacionProv2Moneda.val('');
            inputImportacionProv3Num.val('$0.00');
            inputImportacionProv3Moneda.val('');

            inputGranTotalProv1Num.val('$0.00');
            inputGranTotalProv1Moneda.val('');
            inputGranTotalProv2Num.val('$0.00');
            inputGranTotalProv2Moneda.val('');
            inputGranTotalProv3Num.val('$0.00');
            inputGranTotalProv3Moneda.val('');

            inputTipoCambioProv1.val('$1.00');
            inputTipoCambioProv2.val('$1.00');
            inputTipoCambioProv3.val('$1.00');

            let hoy = new Date().toLocaleDateString();

            inputFechaEntregaProv1.datepicker().datepicker("setDate", hoy);
            inputFechaEntregaProv2.datepicker().datepicker("setDate", hoy);
            inputFechaEntregaProv3.datepicker().datepicker("setDate", hoy);

            inputLABProv1Num.val('');
            inputLABProv1Desc.val('');
            inputLABProv2Num.val('');
            inputLABProv2Desc.val('');
            inputLABProv3Num.val('');
            inputLABProv3Desc.val('');

            inputCondPagoProv1.val('');
            inputCondPagoProv2.val('');
            inputCondPagoProv3.val('');

            textAreaComentarioProv1.val('');
            textAreaComentarioProv2.val('');
            textAreaComentarioProv3.val('');

            let tablaPartidas = tblPartidasCuadro.DataTable();

            tablaPartidas.columns(3).header().to$().text('');
            tablaPartidas.columns(4).header().to$().text('');
            tablaPartidas.columns(5).header().to$().text('');

            tablaPartidas.clear().draw();

            limpiarUltimaCompra();
        }

        function limpiarUltimaCompra() {
            tblUltimaCompra.DataTable().clear().draw();
        }

        function initTablePartidasCuadro() {
            tblPartidasCuadro.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                rowId: 'id',
                scrollY: "250px",
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblPartidasCuadro.on('click', 'tr', function (e) {
                        let rowData = tblPartidasCuadro.DataTable().row($(this).closest('tr')).data();

                        tblPartidasCuadro.find('tr').removeClass('partidaSeleccionada');
                        $(this).closest('tr').addClass('partidaSeleccionada');

                        let partidaCuadro = {
                            cc: rowData.cc,
                            numero: rowData.numero,
                            folio: rowData.folio,
                            partida: rowData.partida,
                            insumo: rowData.insumo
                        }

                        getUltimaCompra(partidaCuadro).done(function (response) {
                            if (response.success) {
                                limpiarUltimaCompra();

                                if (response.data.folioOC != null) {
                                    let listaUltimaCompra = [];
                                    listaUltimaCompra.push(response.data);

                                    AddRows(tblUltimaCompra, listaUltimaCompra);
                                } else {
                                    limpiarUltimaCompra();
                                }
                            } else {
                                AlertaGeneral('Alerta', 'Error al consultar la información.');
                                limpiarUltimaCompra();
                            }
                        });
                    });
                },
                columns: [
                    { data: 'partida', title: 'Pda' },
                    {
                        data: 'insumo',
                        render: function (data, type, row) {
                            return row.insumo + '-' + row.descripcion;
                        },
                        title: 'Insumo'
                    },
                    {
                        data: 'cantidad',
                        render: function (data, type, row) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let span = document.createElement('label');

                            div.classList.add('input-group');
                            input.classList.add('form-control', 'text-right', 'inputCantidad');
                            span.classList.add('input-group-addon');

                            $(input).attr('value', unmaskNumero6D(row.cantidad.toString()));
                            $(span).text(row.unidad);

                            $(div).append(input);
                            $(div).append(span);

                            return div.outerHTML;
                        },
                        title: 'Cantidad'
                    },
                    {
                        defaultContent: '',
                        render: function (data, type, row) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let span = document.createElement('label');

                            div.classList.add('input-group');
                            input.classList.add('form-control', 'text-right', 'inputProv1');
                            span.classList.add('input-group-addon', 'spanProv1');

                            $(input).attr('value', maskNumero6DCompras(row.precio1));
                            $(span).text(row.moneda1);

                            $(div).append(input);
                            $(div).append(span);

                            return div.outerHTML;
                        }
                    },
                    {
                        defaultContent: '',
                        render: function (data, type, row) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let span = document.createElement('label');

                            div.classList.add('input-group');
                            input.classList.add('form-control', 'text-right', 'inputProv2');
                            span.classList.add('input-group-addon', 'spanProv2');

                            $(input).attr('value', maskNumero6DCompras(row.precio2));
                            $(span).text(row.moneda2);

                            $(div).append(input);
                            $(div).append(span);

                            return div.outerHTML;
                        }
                    },
                    {
                        defaultContent: '',
                        render: function (data, type, row) {
                            let div = document.createElement('div');
                            let input = document.createElement('input');
                            let span = document.createElement('label');

                            div.classList.add('input-group');
                            input.classList.add('form-control', 'text-right', 'inputProv3');
                            span.classList.add('input-group-addon', 'spanProv3');

                            $(input).attr('value', maskNumero6DCompras(row.precio3));
                            $(span).text(row.moneda3);

                            $(div).append(input);
                            $(div).append(span);

                            return div.outerHTML;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableUltimaCompra() {
            tblUltimaCompra.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                aaSorting: [0, 'asc'],
                rowId: 'id',
                scrollY: "250px",
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {

                },
                columns: [
                    {
                        data: 'proveedor', title: 'Proveedor', render: function (data, type, row, meta) {
                            return row.proveedorNum + '-' + row.proveedorNom;
                        }
                    },
                    { data: 'folioOC', title: 'Orden Compra' },
                    { data: 'fechaString', title: 'Fecha' },
                    {
                        data: 'precio', title: 'Precio', render: function (data, type, row, meta) {
                            return maskNumero6DCompras(row.precio) + ' ' + row.monedaDesc;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function llenarPanelDerechoProv(cuadro) {
            inputProv1Num.val(cuadro.prov1);
            inputProv1Desc.val(cuadro.nombre_prov1);
            agregarTooltip(inputProv1Desc, cuadro.nombre_prov1);
            inputProv2Num.val(cuadro.prov2);
            inputProv2Desc.val(cuadro.nombre_prov2);
            agregarTooltip(inputProv2Desc, cuadro.nombre_prov2);
            inputProv3Num.val(cuadro.prov3);
            inputProv3Desc.val(cuadro.nombre_prov3);
            agregarTooltip(inputProv3Desc, cuadro.nombre_prov3);

            inputPrimerSubtotalProv1Num.val(maskNumero6DCompras(cuadro.sub_total1));
            inputPrimerSubtotalProv1Moneda.val(cuadro.moneda1Desc);
            inputPrimerSubtotalProv2Num.val(maskNumero6DCompras(cuadro.sub_total2));
            inputPrimerSubtotalProv2Moneda.val(cuadro.moneda2Desc);
            inputPrimerSubtotalProv3Num.val(maskNumero6DCompras(cuadro.sub_total3));
            inputPrimerSubtotalProv3Moneda.val(cuadro.moneda3Desc);

            inputDescuentoProv1.val(maskNumero6DCompras(cuadro.porcent_dcto1));
            inputDescuentoProv2.val(maskNumero6DCompras(cuadro.porcent_dcto2));
            inputDescuentoProv3.val(maskNumero6DCompras(cuadro.porcent_dcto3));

            let segundoSubTotal1 = cuadro.sub_total1 - cuadro.dcto1;
            let segundoSubTotal2 = cuadro.sub_total2 - cuadro.dcto2;
            let segundoSubTotal3 = cuadro.sub_total3 - cuadro.dcto3;

            inputSegundoSubtotalProv1Num.val(maskNumero6DCompras(segundoSubTotal1));
            inputSegundoSubtotalProv1Moneda.val(cuadro.moneda1Desc);
            inputSegundoSubtotalProv2Num.val(maskNumero6DCompras(segundoSubTotal2));
            inputSegundoSubtotalProv2Moneda.val(cuadro.moneda2Desc);
            inputSegundoSubtotalProv3Num.val(maskNumero6DCompras(segundoSubTotal3));
            inputSegundoSubtotalProv3Moneda.val(cuadro.moneda3Desc);

            inputIVAProv1.val(cuadro.porcent_iva1);
            inputIVAProv2.val(cuadro.porcent_iva2);
            inputIVAProv3.val(cuadro.porcent_iva3);

            inputTotalProv1Num.val(maskNumero6DCompras(cuadro.total1));
            inputTotalProv1Moneda.val(cuadro.moneda1Desc);
            inputTotalProv2Num.val(maskNumero6DCompras(cuadro.total2));
            inputTotalProv2Moneda.val(cuadro.moneda2Desc);
            inputTotalProv3Num.val(maskNumero6DCompras(cuadro.total3));
            inputTotalProv3Moneda.val(cuadro.moneda3Desc);

            inputFletesProv1Num.val(maskNumero6DCompras(cuadro.fletes1));
            inputFletesProv1Moneda.val(cuadro.moneda1Desc);
            inputFletesProv2Num.val(maskNumero6DCompras(cuadro.fletes2));
            inputFletesProv2Moneda.val(cuadro.moneda2Desc);
            inputFletesProv3Num.val(maskNumero6DCompras(cuadro.fletes3));
            inputFletesProv3Moneda.val(cuadro.moneda3Desc);

            inputImportacionProv1Num.val(maskNumero6DCompras(cuadro.gastos_imp1));
            inputImportacionProv1Moneda.val(cuadro.moneda1Desc);
            inputImportacionProv2Num.val(maskNumero6DCompras(cuadro.gastos_imp2));
            inputImportacionProv2Moneda.val(cuadro.moneda2Desc);
            inputImportacionProv3Num.val(maskNumero6DCompras(cuadro.gastos_imp3));
            inputImportacionProv3Moneda.val(cuadro.moneda3Desc);

            let granTotal1 = cuadro.total1 + cuadro.fletes1 + cuadro.gastos_imp1;
            let granTotal2 = cuadro.total2 + cuadro.fletes2 + cuadro.gastos_imp2;
            let granTotal3 = cuadro.total3 + cuadro.fletes3 + cuadro.gastos_imp3;

            inputGranTotalProv1Num.val(maskNumero6DCompras(granTotal1));
            inputGranTotalProv1Moneda.val(cuadro.moneda1Desc);
            inputGranTotalProv2Num.val(maskNumero6DCompras(granTotal2));
            inputGranTotalProv2Moneda.val(cuadro.moneda2Desc);
            inputGranTotalProv3Num.val(maskNumero6DCompras(granTotal3));
            inputGranTotalProv3Moneda.val(cuadro.moneda3Desc);

            inputTipoCambioProv1.val(maskNumero6DCompras(cuadro.tipo_cambio1));
            inputTipoCambioProv2.val(maskNumero6DCompras(cuadro.tipo_cambio2));
            inputTipoCambioProv3.val(maskNumero6DCompras(cuadro.tipo_cambio3));

            inputFechaEntregaProv1.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(cuadro.fecha_entrega1.substr(6)))));
            inputFechaEntregaProv2.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(cuadro.fecha_entrega2.substr(6)))));
            inputFechaEntregaProv3.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(cuadro.fecha_entrega3.substr(6)))));

            inputLABProv1Num.val(cuadro.lab1);
            inputLABProv1Desc.val(cuadro.lab1Desc);
            inputLABProv2Num.val(cuadro.lab2);
            inputLABProv2Desc.val(cuadro.lab2Desc);
            inputLABProv3Num.val(cuadro.lab3);
            inputLABProv3Desc.val(cuadro.lab3Desc);

            inputCondPagoProv1.val(cuadro.dias_pago1);
            inputCondPagoProv2.val(cuadro.dias_pago2);
            inputCondPagoProv3.val(cuadro.dias_pago3);

            textAreaComentarioProv1.val(cuadro.comentarios1);
            textAreaComentarioProv2.val(cuadro.comentarios2);
            textAreaComentarioProv3.val(cuadro.comentarios3);
        }

        function llenarTablaPartidas(cuadro, cuadroDetalle) {
            let tablaPartidas = tblPartidasCuadro.DataTable();

            tablaPartidas.columns(3).header().to$().text(cuadro.nombre_prov1);
            tablaPartidas.columns(4).header().to$().text(cuadro.nombre_prov2);
            tablaPartidas.columns(5).header().to$().text(cuadro.nombre_prov3);

            AddRows(tblPartidasCuadro, cuadroDetalle);
        }

        function agregarTooltip(elemento, mensaje) {
            $(elemento).attr('data-toggle', 'tooltip');
            $(elemento).attr('data-placement', 'top');

            if (mensaje != "") {
                $(elemento).attr('title', mensaje);
            }

            $('[data-toggle="tooltip"]').tooltip({
                position: {
                    my: "center bottom-20",
                    at: "center top+8",
                    using: function (position, feedback) {
                        $(this).css(position);
                        $("<div>")
                            .addClass("arrow")
                            .addClass(feedback.vertical)
                            .addClass(feedback.horizontal)
                            .appendTo(this);
                    }
                }
            });
        }
        function quitarTooltip(elemento) {
            $(elemento).removeAttr('data-toggle');
            $(elemento).removeAttr('data-placement');
            $(elemento).removeAttr('title');
        }

        function verificarComprasPeru() {
            if (_empresaActual == 6) {
                selectMoneda.attr('disabled', false);
                // $('.elementoPeru').show();
                $('.elementoPeru').hide();
                $(".elementoNoPeru").hide();
            } else {
                // $('.elementoPeru').hide();
                $('.elementoPeru').show();
            }
        }
    }
    $(document).ready(() => Enkontrol.OrdenCompra.SeguimientoAutorizacion = new SeguimientoAutorizacion())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...', baseZ: 2000 }))
        .ajaxStop($.unblockUI);
})();