(function () {
    $.namespace('Enkontrol.Compras.OrdenCompra.Surtido');

    Surtido = function () {
        //#region Selectores
        const tblPartidas = $('#tblPartidas');
        const textAreaDescPartida = $('#textAreaDescPartida');
        const btnGuardarSurtido = $('#btnGuardarSurtido');
        const inputNumMovimiento = $('#inputNumMovimiento');
        const divNumMovimiento = $('#divNumMovimiento');
        const botonReporte = $('#botonReporte');
        const mdlUbicacionDetalle = $('#mdlUbicacionDetalle');
        const tblUbicacion = $('#tblUbicacion');
        const btnGuardarUbicacion = $('#btnGuardarUbicacion');
        const btnAgregarUbicacion = $('#btnAgregarUbicacion');
        const btnQuitarUbicacion = $('#btnQuitarUbicacion');
        const mdlHistorialInsumo = $('#mdlHistorialInsumo');
        const tblHistorialInsumo = $('#tblHistorialInsumo');
        const mdlCatalogoUbicaciones = $('#mdlCatalogoUbicaciones');
        const tblCatalogoUbicaciones = $('#tblCatalogoUbicaciones');
        const report = $("#report");
        //#region Panel Izquierdo
        selectCC = $('#selectCC');
        inputNumero = $('#inputNumero');
        inputNumeroReq = $('#inputNumeroReq');
        selectBoS = $('#selectBoS');
        const dtpFecha = $('#dtpFecha');
        const inputProvNum = $('#inputProvNum');
        const inputProvNom = $('#inputProvNom');
        const inputCompNum = $('#inputCompNum');
        const inputCompNom = $('#inputCompNom');
        const inputSolNum = $('#inputSolNum');
        const inputSolNom = $('#inputSolNom');
        const inputAutNum = $('#inputAutNum');
        const inputAutNom = $('#inputAutNom');
        inputEmb = $('#inputEmb');
        selectLab = $('#selectLab');
        const inputConFact = $('#inputConFact');
        checkAutoRecep = $('#checkAutoRecep');
        inputAlmNum = $('#inputAlmNum');
        const inputAlmNom = $('#inputAlmNom');
        inputEmpNum = $('#inputEmpNum');
        const inputEmpNom = $('#inputEmpNom');
        const inputOrdenTraspaso = $('#inputOrdenTraspaso');
        const inputComentarios = $('#inputComentarios');
        //#endregion

        //#region Panel Derecho
        selectTipoOC = $('#selectTipoOC');
        selectMoneda = $('#selectMoneda');
        inputTipoCambio = $('#inputTipoCambio');
        const inputSubTotal = $('#inputSubTotal');
        inputIVAPorcentaje = $('#inputIVAPorcentaje');
        const inputIVANumero = $('#inputIVANumero');
        const inputRetencion = $('#inputRetencion');
        const inputTotal = $('#inputTotal');
        const inputTotalFinal = $('#inputTotalFinal');
        const btnGlobal = $('#btnGlobal');
        const btnImprimirCompra = $('#btnImprimirCompra');
        const inputGuiaPrefijo = $('#inputGuiaPrefijo');
        const inputGuiaFolio = $('#inputGuiaFolio');
        const selectTipoDocumento = $('#selectTipoDocumento');
        const inputFolioDocumento = $('#inputFolioDocumento');
        //#endregion

        //#region Elementos Ocultos
        const inputCompradorSesionNum = $('#inputCompradorSesionNum');
        const inputCompradorSesionNom = $('#inputCompradorSesionNom');
        const dtpInicio = $('#dtpInicio');
        const dtpFin = $('#dtpFin');
        //#endregion

        //#endregion

        //#region CONST ELEMENTOS OCULTOS
        const inputEmpresaActual = $('#inputEmpresaActual');
        _empresaActual = +inputEmpresaActual.val();
        //#endregion

        function init() {
            verificarComprasPeru();
            let hoy = new Date();

            dtpInicio.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 0, 1));
            dtpFin.datepicker().datepicker("setDate", new Date(hoy.getFullYear(), 11, 31));
            dtpFecha.datepicker().datepicker();

            isComprador();
            initCbo();

            initTablePartidas();
            initTableUbicacion();
            initTableHistorialInsumo();
            initTableCatalogoUbicaciones();

            btnGlobal.click(aplicarGlobal);

            // Se comprueba si hay variables en url.
            const variables = getUrlParams(window.location.href);

            if (variables && variables.cc && variables.oc) {
                selectCC.val(variables.cc);
                inputNumero.val(variables.oc);
                inputNumero.change();
            }
            divNumMovimiento.hide();

            if (_empresaActual == 6) {
                $("#selectFormaPago").fillCombo('/Enkontrol/OrdenCompra/FillComboFormaPagoPeru', null, false, null);
                // tblPartidas.DataTable().column(2).visible(false);
                selectTipoDocumento.fillCombo('/Enkontrol/OrdenCompra/FillComboTipoDocumentoPeru', null, false, null);
            } else {
                $('.elementoPeru').hide();
            }
        }

        _filaInsumo = null;
        _filaUbicacion = null;

        const getCompra = () => $.post('/Enkontrol/OrdenCompra/GetCompra', {
            cc: selectCC.val(),
            num: inputNumero.val() != '' ? inputNumero.val() : 0,
            PERU_tipoCompra: 'RQ'
        });
        const getComprador = () => $.post('/Enkontrol/OrdenCompra/getComprador');
        const guardarSurtido = (compra, surtido) => $.post('/Enkontrol/OrdenCompra/GuardarSurtido', { compra, surtido });
        const getProveedorInfo = (num) => $.post('/Enkontrol/OrdenCompra/GetProveedorInfo', { num: num });

        selectCC.on('change', e => {
            if ($(e.currentTarget).val() != '' && inputNumero.val() != '') {
                cargarCompra();
            }
            if ($(e.currentTarget).val() != '') {
                divNumMovimiento.show();
            }
        });

        inputNumero.on('change', e => {
            if ($(e.currentTarget).val() != '' && selectCC.val() != '') {
                cargarCompra();
            }
        });

        inputIVAPorcentaje.on('change', e => {
            let valor = $(e.currentTarget).val();

            if (!isNaN(valor)) {
                let iva = parseFloat(valor);
                let subTotal = unmaskNumero(inputSubTotal.val());

                inputIVANumero.val(maskNumero((subTotal * iva) / 100));
                inputTotal.val(maskNumero(subTotal + (unmaskNumero(inputIVANumero.val()))));
            } else {
                $(e.currentTarget).val(16);
                inputIVAPorcentaje.change();
            }
        });

        tblPartidas.on('change', '.inputSurtir', function () {
            let flagSurtir = false;

            tblPartidas.find("tbody tr").each(function (idx, row) {
                let valor = $(row).find(".inputSurtir").val();

                if (valor > 0) {
                    flagSurtir = true;
                }
            });
        });

        btnGuardarSurtido.on('click', function () {
            if (selectLab.val() > 0) {
                let compra = {
                    cc: selectCC.val(),
                    numero: inputNumero.val(),
                    ordenTraspaso: inputOrdenTraspaso.val(),
                    proveedor: inputProvNum.val(),
                    comentarios: inputComentarios.val(),
                    almacen: selectLab.val(),
                    numeroRequisicion: inputNumeroReq.val(),
                    PERU_guiaCompraPrefijo: inputGuiaPrefijo.val(),
                    PERU_guiaCompraFolio: inputGuiaFolio.val(),
                    PERU_tipoDocumento: selectTipoDocumento.val(),
                    PERU_folioDocumento: inputFolioDocumento.val(),
                    PERU_tipoCompra: 'RQ'
                }

                let lstSurtido = [];

                tblPartidas.find("tbody tr").each(function (idx, row) {
                    let rowData = tblPartidas.DataTable().row(row).data();
                    let valorSurtir = $(row).find('.inputCantidadAutorizar').val();
                    let listUbicacionMovimiento = $(row).find('td .btnUbicacionDetalle').data('listUbicacionMovimiento');

                    if ((!isNaN(valorSurtir) && valorSurtir != '' && valorSurtir > 0)) {
                        lstSurtido.push({
                            partida: rowData.partida,
                            insumo: rowData.insumo,
                            cantidad: rowData.cantidad,
                            cantidadMovimiento: rowData.cantidad,
                            surtido: rowData.surtido,
                            aSurtir: valorSurtir,
                            listUbicacionMovimiento: listUbicacionMovimiento
                        });
                    }
                });

                if (lstSurtido.length > 0) {
                    if (_empresaActual != 6) {
                        btnGuardarSurtido.attr('disabled', true);

                        guardarSurtido(compra, lstSurtido).done(function (response) {
                            if (response.success) {
                                AlertaGeneral('Alerta', 'Se ha guardado la información.');
                                verReporte(true);
                                limpiarInformacion();
                                limpiarTabla(tblPartidas);
                                // btnGuardarSurtido.attr('disabled', false);
                            } else {
                                if (response.message.length > 0) {
                                    AlertaGeneral(`Alerta`, response.message);
                                } else {
                                    AlertaGeneral('Alerta', 'Error al guardar la información.');
                                    inputNumero.change();
                                }
                            }
                        });
                    } else {
                        if (inputGuiaPrefijo.val() != '' && inputGuiaFolio.val() != '') {
                            btnGuardarSurtido.attr('disabled', true);

                            guardarSurtido(compra, lstSurtido).done(function (response) {
                                if (response.success) {
                                    AlertaGeneral('Alerta', 'Se ha guardado la información.');
                                    verReporte(true);
                                    limpiarInformacion();
                                    limpiarTabla(tblPartidas);
                                    // btnGuardarSurtido.attr('disabled', false);
                                } else {
                                    if (response.message.length > 0) {
                                        AlertaGeneral(`Alerta`, response.message);
                                    } else {
                                        AlertaGeneral('Alerta', 'Error al guardar la información.');
                                        inputNumero.change();
                                    }
                                }
                            });
                        } else {
                            Alert2AccionConfirmar('Atención', 'No ha capturado la guía de compra. ¿Desea guardar el surtido de la compra?', 'Confirmar', 'Cancelar', () => {
                                btnGuardarSurtido.attr('disabled', true);

                                guardarSurtido(compra, lstSurtido).done(function (response) {
                                    if (response.success) {
                                        AlertaGeneral('Alerta', 'Se ha guardado la información.');
                                        verReporte(true);
                                        limpiarInformacion();
                                        limpiarTabla(tblPartidas);
                                        // btnGuardarSurtido.attr('disabled', false);
                                    } else {
                                        if (response.message.length > 0) {
                                            AlertaGeneral(`Alerta`, response.message);
                                        } else {
                                            AlertaGeneral('Alerta', 'Error al guardar la información.');
                                            inputNumero.change();
                                        }
                                    }
                                });
                            });
                        }
                    }
                } else {
                    AlertaGeneral(`Alerta`, `No ha seleccionado cantidad a surtir.`);
                }
            } else {
                AlertaGeneral(`Alerta`, `Seleccione un almacén destino (L.A.B.).`);
            }
        });

        botonReporte.click(() => {
            const cc = selectCC.val();
            const num = inputNumero.val();
            const numMovimiento = inputNumMovimiento.val();

            if (cc == "" || numMovimiento == "") {
                AlertaGeneral(`Aviso`, `Debe seleccionar un cc e ingresar un número de movimiento con información de compra.`);
                return;
            }

            $.get('/Enkontrol/OrdenCompra/GetDatosReporteEntradaOC', { cc, num, numMovimiento })
                .then(response => {
                    if (response.success) {
                        // Operación exitosa.
                        verReporte(false);
                    } else {
                        // Operación no completada.
                        AlertaGeneral(`Operación fallida`, `No se pudo completar la operación: ${response.message}`);
                    }
                }, error => {
                    // Error al lanzar la petición.
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        btnGuardarUbicacion.on('click', function () {
            let listUbicacionMovimiento = [];
            let totalUbicacion = 0;

            tblUbicacion.find("tbody tr").each(function (idx, row) {
                let rowData = tblUbicacion.DataTable().row(row).data();

                if (rowData != undefined) {
                    totalUbicacion += rowData.cantidad;

                    if (rowData.cantidad > 0) {
                        listUbicacionMovimiento.push({
                            insumo: rowData.insumo,
                            insumoDesc: rowData.insumoDesc,
                            cantidad: rowData.cantidad,
                            cantidadMovimiento: rowData.cantidad,
                            area_alm: rowData.area_alm,
                            lado_alm: rowData.lado_alm,
                            estante_alm: rowData.estante_alm,
                            nivel_alm: rowData.nivel_alm
                        });
                    }
                }
            });

            let cantidadEntrada = tblPartidas.DataTable().row(_filaInsumo).data().cantidad;

            if (totalUbicacion <= cantidadEntrada) {
                if (listUbicacionMovimiento.length > 0) {
                    _filaInsumo.find('td .btnUbicacionDetalle').data('listUbicacionMovimiento', listUbicacionMovimiento);
                    _filaInsumo.find('td .btnUbicacionDetalle').data('totalUbicacion', totalUbicacion);
                    _filaInsumo.find('td .inputCantidadAutorizar').val(totalUbicacion);

                    _filaInsumo.find('td input').change();

                    mdlUbicacionDetalle.modal('hide');
                } else {
                    _filaInsumo.find('td .btnUbicacionDetalle').data('listUbicacionMovimiento', null);
                    _filaInsumo.find('td .btnUbicacionDetalle').data('totalUbicacion', null);
                    _filaInsumo.find('td .inputCantidadAutorizar').val(0);

                    _filaInsumo.find('td input').change();

                    mdlUbicacionDetalle.modal('hide');
                }
            } else {
                AlertaGeneral(`Alerta`, `No se puede capturar una cantidad mayor a la entrada.`);
            }
        });

        btnAgregarUbicacion.on('click', function () {
            let datos = tblUbicacion.DataTable().rows().data();
            let datosPartida = tblPartidas.DataTable().row(_filaInsumo).data();

            datos.push({
                'insumo': datosPartida.insumo,
                'insumoDesc': datosPartida.insumoDesc,
                'cantidad': 0,
                'area_alm': '',
                'lado_alm': '',
                'estante_alm': '',
                'nivel_alm': ''
            });

            tblUbicacion.DataTable().clear();
            tblUbicacion.DataTable().rows.add(datos).draw();
        });

        btnQuitarUbicacion.on('click', function () {
            tblUbicacion.DataTable().row(tblUbicacion.find("tr.active")).remove().draw();

            let cuerpo = tblUbicacion.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                tblUbicacion.DataTable().draw();
            }
        });

        selectLab.on('change', function () {
            let almacen = +($(this).val());

            if (almacen >= 600 && almacen <= 699) {
                AlertaGeneral(`Alerta`, `No se pueden hacer entradas por compra a los almacenes 600.`);
            }
        });

        btnImprimirCompra.on('click', function () {
            let cc = selectCC.val();
            let numero = +inputNumero.val();

            if (cc == '' || numero == 0) {
                AlertaGeneral(`Alerta`, `Debe seleccionar una compra válida.`);
                return;
            }

            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/OrdenCompra/CheckEstatusOrdenCompraImpresa', { cc, numero, PERU_tipoCompra: "RQ" })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        if (cc != '' && !isNaN(numero)) {
                            verReporteCompra(cc, numero);
                        }
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        function verReporteCompra(cc, numero) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", `/Reportes/Vista.aspx?idReporte=113&cc=${cc}&numero=${numero}&PERU_tipoCompra=RQ`);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
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

        function initCbo() {
            // selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCcComCompradorModalEditar', null, false);
            selectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false);

            axios.post('/Enkontrol/Requisicion/FillComboAlmacenSurtir')
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        let listaFiltrada = response.data.items.filter((x) => +x.Value < 900 || +x.Value == 996);
                        selectLab.fillCombo({ items: listaFiltrada }, null, false);
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function isComprador() {
            getComprador().done(response => {
                if (response.success) {
                    inputCompradorSesionNum.val(response.comprador.comprador);
                    inputCompradorSesionNom.val(response.comprador.emplNom);
                } else {
                    AlertaGeneral("Aviso", "No eres comprador.");
                }
            });
        }

        function initTablePartidas() {
            tblPartidas.DataTable({
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
                    tblPartidas.on('click', 'td', function () {
                        let rowData = tblPartidas.DataTable().row($(this).closest('tr')).data();

                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblPartidas.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                            textAreaDescPartida.val(rowData.partidaDescripcion);
                        } else {
                            textAreaDescPartida.val('');
                        }
                    });

                    tblPartidas.on('click', '.btnUbicacionDetalle', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblPartidas.DataTable().row(row).data();

                        _filaInsumo = row;

                        if ($(this).data('listUbicacionMovimiento') == undefined && $(this).data('listUbicacionMovimiento') == null) {
                            tblUbicacion.DataTable().clear();

                            let datos = tblUbicacion.DataTable().rows().data();

                            datos.push({
                                'insumo': rowData.insumo,
                                'insumoDesc': rowData.insumoDesc,
                                'cantidad': 0,
                                'area_alm': '',
                                'lado_alm': '',
                                'estante_alm': '',
                                'nivel_alm': ''
                            });

                            tblUbicacion.DataTable().clear();
                            tblUbicacion.DataTable().rows.add(datos).draw();
                        } else {
                            let listUbicacionMovimiento = $(this).data('listUbicacionMovimiento').filter(function (element) {
                                return element.cantidad > 0;
                            });

                            listUbicacionMovimiento.push({
                                'insumo': rowData.insumo,
                                'insumoDesc': rowData.insumoDesc,
                                'cantidad': 0,
                                'area_alm': '',
                                'lado_alm': '',
                                'estante_alm': '',
                                'nivel_alm': ''
                            });

                            AddRows(tblUbicacion, listUbicacionMovimiento);
                        }

                        mdlUbicacionDetalle.modal('show');
                    });
                },
                columns: [
                    { data: 'partida', title: 'Partida' },
                    {
                        title: 'Insumo', render: function (data, type, row, meta) {
                            return row.insumo + '-' + row.insumoDesc;
                        }
                    },
                    { data: 'areaCuenta', title: 'Área Cuenta' },
                    { data: 'fecha_entrega', title: 'Fecha Entregar' },
                    {
                        data: 'precio', title: 'Precio', render: (data, type, row, meta) => {
                            var mon = selectMoneda.find('option[value="' + row.moneda + '"]').text().trim();
                            return maskNumero(data) + " " + mon;
                        }
                    },
                    { data: 'importe', title: 'Importe' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'cant_recibida', title: 'Cant. Recibida' },
                    {
                        title: 'Cant. Pendiente', render: function (data, type, row, meta) {
                            return row.cantidad - row.cant_recibida;
                        }
                    },
                    {
                        sortable: false,
                        render: (data, type, row, meta) => {
                            let valor = data != '' && data != undefined ? data : 0;

                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnUbicacionDetalle">
                                                <i class="fa fa-arrow-right"></i>
                                            </button>
                                        </span>
                                        <input type="text" class="form-control text-center inputCantidadAutorizar" disabled value="${valor}" style="height: 22px;">
                                    </div>`;
                        },
                        title: 'Recibido'
                    }
                ],
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [3]
                    },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return maskNumero(data);
                            }
                        },
                        targets: [5]
                    },
                    { className: "dt-center", "targets": "_all" },
                    { width: '10%', targets: [1, 9] }
                ]
            });
        }

        function initTableUbicacion() {
            tblUbicacion.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblUbicacion.on('click', '.btnCantidadTotalSalida', function () {
                        let rowData = tblUbicacion.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');

                        $row.find('.inputCantidadSalida').val(rowData.cantidad);
                        $row.find('.inputCantidadSalida').change();
                    });

                    tblUbicacion.on('change', 'input', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblUbicacion.DataTable().row(row).data();

                        let inputCantidadEntradaUbicacion = row.find('.inputCantidadEntradaUbicacion');
                        let inputArea = row.find('.inputArea');
                        let inputLado = row.find('.inputLado');
                        let inputEstante = row.find('.inputEstante');
                        let inputNivel = row.find('.inputNivel');

                        if (!isNaN(inputCantidadEntradaUbicacion.val())) {
                            let area_alm = inputArea.val();
                            let lado_alm = inputLado.val();
                            let estante_alm = inputEstante.val();
                            let nivel_alm = inputNivel.val();
                            let cantidadEntradaUbicacion = unmaskNumero(inputCantidadEntradaUbicacion.val());

                            rowData.area_alm = area_alm;
                            rowData.lado_alm = lado_alm;
                            rowData.estante_alm = estante_alm;
                            rowData.nivel_alm = nivel_alm;
                            rowData.cantidad = cantidadEntradaUbicacion;

                            tblUbicacion.DataTable().row(row).data(rowData).draw();

                            if ($(this).hasClass('inputCantidadEntradaUbicacion')) {
                                row.find('.inputArea').focus();
                            } else if ($(this).hasClass('inputArea')) {
                                row.find('.inputLado').focus();
                            } else if ($(this).hasClass('inputLado')) {
                                row.find('.inputEstante').focus();
                            } else if ($(this).hasClass('inputEstante')) {
                                row.find('.inputNivel').focus();
                            } else if ($(this).hasClass('inputNivel')) {
                                if (cantidadEntradaUbicacion > 0 && $(row).is(":last-child")) {
                                    let datos = tblUbicacion.DataTable().rows().data();

                                    datos.push({
                                        'insumo': rowData.insumo,
                                        'insumoDesc': rowData.insumoDesc,
                                        'cantidad': 0,
                                        'area_alm': '',
                                        'lado_alm': '',
                                        'estante_alm': '',
                                        'nivel_alm': ''
                                    });

                                    tblUbicacion.DataTable().clear();
                                    tblUbicacion.DataTable().rows.add(datos).draw();
                                    $('.inputCantidadEntradaUbicacion:last').focus();
                                }
                            }
                        } else {
                            AlertaGeneral(`Alerta`, `Ingrese una cantidad válida.`);
                            inputCantidadEntradaUbicacion.val('');
                        }
                    });

                    tblUbicacion.on('click', 'td', function () {
                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblUbicacion.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");
                        }
                    });

                    tblUbicacion.on('keyup', '.inputArea, .inputLado, .inputEstante, .inputNivel', function () {
                        $(this).val($(this).val().toUpperCase());
                    });

                    tblUbicacion.on('click', '.btnHistorialInsumo', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblUbicacion.DataTable().row(row).data();

                        _filaUbicacion = row;

                        cargarHistorialInsumo();
                    });

                    tblUbicacion.on('click', '.btnCatalogoUbicaciones', function () {
                        let row = $(this).closest('tr');

                        _filaUbicacion = row;

                        cargarCatalogoUbicaciones();
                    });
                },
                columns: [
                    {
                        data: 'insumoDesc', title: 'Insumo', render: function (data, type, row, meta) {
                            return row.insumo + ' - ' + row.insumoDesc;
                        }
                    },
                    {
                        data: 'cantidad', title: 'Cantidad', render: function (data, type, row, meta) {
                            let valor = data != undefined && data != '' ? data : '';

                            return `<input type="text" class="form-control text-center inputCantidadEntradaUbicacion" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        data: 'area_alm', title: 'Área', render: function (data, type, row, meta) {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-center inputArea" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        data: 'lado_alm', title: 'Lado', render: function (data, type, row, meta) {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-center inputLado" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        data: 'estante_alm', title: 'Estante', render: function (data, type, row, meta) {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-center inputEstante" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        data: 'nivel_alm', title: 'Nivel', render: function (data, type, row, meta) {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-center inputNivel" value="${valor}" style="height: 22px;">`;
                        }
                    },
                    {
                        title: 'Historial', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default btnHistorialInsumo"><i class="fa fa-th-list"></i></button>`;
                        }
                    },
                    {
                        title: 'Catálogo', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default btnCatalogoUbicaciones"><i class="fa fa-bars"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableHistorialInsumo() {
            tblHistorialInsumo.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblHistorialInsumo.on('click', '.btnSeleccionarHistorialInsumo', function () {
                        let rowData = tblHistorialInsumo.DataTable().row($(this).closest('tr')).data();

                        if (_filaUbicacion != null) {
                            $(_filaUbicacion).find('.inputArea').val(rowData.area_alm);
                            $(_filaUbicacion).find('.inputLado').val(rowData.lado_alm);
                            $(_filaUbicacion).find('.inputEstante').val(rowData.estante_alm);
                            $(_filaUbicacion).find('.inputNivel').val(rowData.nivel_alm);

                            mdlHistorialInsumo.modal('hide');

                            tblUbicacion.find('.inputArea').change();
                        }
                    });
                },
                columns: [
                    {
                        data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {
                            return row.insumo + ' - ' + row.insumoDesc;
                        }
                    },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-primary btnSeleccionarHistorialInsumo"><i class="fa fa-arrow-right"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTableCatalogoUbicaciones() {
            tblCatalogoUbicaciones.DataTable({
                retrieve: true,
                deferRender: true,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblCatalogoUbicaciones.on('click', '.btnSeleccionarUbicacion', function () {
                        let rowData = tblCatalogoUbicaciones.DataTable().row($(this).closest('tr')).data();

                        if (_filaUbicacion != null) {
                            $(_filaUbicacion).find('.inputArea').val(rowData.area_alm);
                            $(_filaUbicacion).find('.inputLado').val(rowData.lado_alm);
                            $(_filaUbicacion).find('.inputEstante').val(rowData.estante_alm);
                            $(_filaUbicacion).find('.inputNivel').val(rowData.nivel_alm);

                            mdlCatalogoUbicaciones.modal('hide');

                            tblUbicacion.find('.inputArea').change();
                        }
                    });
                },
                columns: [
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    {
                        title: 'Seleccionar', render: function (data, type, row, meta) {
                            return `<button class="btn btn-sm btn-primary btnSeleccionarUbicacion"><i class="fa fa-arrow-right"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function llenarInformacion(data) {
            //#region Panel Izquierdo
            selectBoS.val(data.bienes_servicios);
            dtpFecha.val($.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.fecha.substr(6)))));

            if ($('#inputEmpresaActual').val() == 6) {
                inputProvNum.val(data.PERU_proveedor);
            } else {
                inputProvNum.val(data.proveedor);
            }

            getProveedorInfo(inputProvNum.val()).done(function (response) {
                if (response != null) {
                    if (response.cancelado != 'C') {
                        inputProvNom.val(response.id);
                        $("#selectMoneda").val(response.moneda);
                        if (response.listaCuentasCorrientes != null && response.listaCuentasCorrientes.length > 0) {
                            $("#selectCuentaCorriente").empty();
                            $("#selectCuentaCorriente").append(`<option value="">--Seleccione--</option>`);

                            response.listaCuentasCorrientes.forEach((element) => {
                                $("#selectCuentaCorriente").append(`<option value="${element.Value}">${element.Text}</option>`);
                            });

                            if (data.PERU_cuentaCorriente != null && data.PERU_cuentaCorriente != '') {
                                $("#selectCuentaCorriente").find(`option:contains('${data.PERU_cuentaCorriente}')`).attr('selected', true);
                            }
                        }

                        if (data.PERU_formaPago != null && data.PERU_formaPago != "") {
                            // $("#selectFormaPago").find(`option:contains('${data.PERU_formaPago}')`).attr('selected', true);
                            $("#selectFormaPago").val(data.PERU_formaPago);
                        }

                        llenarInputProveedorDistinto(inputProvNum.val());
                    } else {
                        AlertaGeneral(`Alerta`, `El proveedor "${response.label} - ${response.id}" no está activo.`);
                        inputProvNum.val('');
                        inputProvNom.val('');

                        llenarInputProveedorDistinto('');
                    }
                }
            }).then($.unblockUI);

            inputProvNom.val(data.proveedorNom);
            inputComentarios.val(data.comentarios);
            inputCompNum.val(data.comprador);
            inputCompNom.val(data.compradorNom);
            inputSolNum.val(data.solicito);
            inputSolNom.val(data.solicitoNom);
            inputAutNum.val(data.autorizo);
            inputAutNom.val(data.autorizoNom);
            inputEmb.val(data.embarquese);
            selectLab.val(data.libre_abordo);
            inputConFact.val(data.concepto_factura);

            if (data.bit_autorecepcion == 'S') {
                checkAutoRecep.prop("checked", true);
            } else {
                checkAutoRecep.prop("checked", false);
            }

            inputAlmNum.val(data.almacen_autorecepcion);
            inputAlmNom.val(data.almacenRecepNom);
            inputEmpNum.val(data.empleado_autorecepcion);
            inputEmpNom.val(data.empleadoRecepNom);
            //#endregion

            //#region Panel Derecho
            selectTipoOC.val(data.tipo_oc_req);
            selectMoneda.val(data.moneda);
            inputTipoCambio.val(data.tipo_cambio);

            if (_empresaActual == 6) {
                inputSubTotal.val(maskNumero2DCompras_PERU(data.sub_total));
                inputIVANumero.val(maskNumero2DCompras_PERU(data.iva));
                inputRetencion.val(maskNumero2DCompras_PERU(data.rentencion_despues_iva));
                inputTotal.val(maskNumero2DCompras_PERU(data.total));
                let totalFinal = (unmaskNumero(inputRetencion.val()) - unmaskNumero((inputTotal.val()))) * -1
                inputTotalFinal.val(maskNumero2DCompras_PERU((totalFinal)));
            } else {
                inputSubTotal.val(maskNumero2DCompras(data.sub_total));
                inputIVANumero.val(maskNumero2DCompras(data.iva));
                inputRetencion.val(maskNumero(data.rentencion_despues_iva));
                inputTotal.val(maskNumero2DCompras(data.total));
                let totalFinal = (unmaskNumero(inputRetencion.val()) - unmaskNumero((inputTotal.val()))) * -1
                inputTotalFinal.val(maskNumero2DCompras((totalFinal)));
            }

            inputIVAPorcentaje.val(data.porcent_iva);
            data.ST_OC == 'A' ? $('#btnGuardarRetenciones').hide() : $('#btnGuardarRetenciones').show();
            //#endregion
        }

        function limpiarInformacion() {
            //#region Panel Izquierdo
            selectBoS.val('');
            dtpFecha.val('');
            inputProvNum.val('');
            inputProvNom.val('');
            inputCompNum.val('');
            inputCompNom.val('');
            inputSolNum.val('');
            inputSolNom.val('');
            inputAutNum.val('');
            inputAutNom.val('');
            // selectEmb.val('');
            inputEmb.val('');
            selectLab.val('');
            //inputObra.val('');
            //inputEstimacion.val('');
            inputConFact.val('');

            checkAutoRecep.prop("checked", false);

            inputAlmNum.val('');
            inputAlmNom.val('');
            inputEmpNum.val('');
            inputEmpNom.val('');
            //#endregion

            //#region Panel Derecho
            selectTipoOC.val('');
            selectMoneda.val('');
            inputTipoCambio.val('');
            inputSubTotal.val('');
            inputIVAPorcentaje.val('');
            inputIVANumero.val('');
            inputRetencion.val('');
            inputTotal.val('');
            inputTotalFinal.val('');
            //#endregion
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }

        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }

        function limpiarTabla(tbl) {
            dt = tbl.DataTable();
            dt.clear().draw();
            textAreaDescPartida.val("")
        }

        function cargarCompra() {
            getCompra().done(response => {
                if (response.success) {
                    inputNumMovimiento.val(response.ultimoMovimiento);
                    if (response.info.cc != null) {
                        if (response.info.estatus != 'C') {
                            // if (response.info.inventariado == 'I') {
                            let flagCompraPendienteSurtir = response.partidas.some(function (x) {
                                return x.cantidad > x.cant_recibida;
                            });

                            if (flagCompraPendienteSurtir) {
                                llenarInformacion(response.info);
                                if (response.partidas.length > 0) { inputNumeroReq.val(response.partidas[0].num_requisicion); }

                                let partidasFiltradas = response.partidas.filter(x => x.inventariado == 'I' || x.inventariado == 'S');

                                AddRows(tblPartidas, partidasFiltradas);

                                // tblPartidas.find('tbody tr:eq(0) td:eq(0)').click()
                                btnGuardarSurtido.attr('disabled', !(partidasFiltradas.length > 0));

                                if (partidasFiltradas.length == 0) {
                                    AlertaGeneral(`Alerta`, `La compra no contiene partidas inventariables.`);
                                }
                            } else {
                                AlertaGeneral(`Alerta`, `La compra ya ha sido surtida.`);

                                limpiarInformacion();
                                limpiarTabla(tblPartidas);

                                textAreaDescPartida.val('');
                            }
                            divNumMovimiento.show(1000);
                            // } else {
                            //     AlertaGeneral(`Alerta`, `La compra "${selectCC.val()}-${inputNumero.val()}" no es inventariable.`);

                            //     limpiarInformacion();
                            //     limpiarTabla(tblPartidas);

                            //     selectCC.val('');
                            //     inputNumero.val('');
                            //     textAreaDescPartida.val('');
                            //     divNumMovimiento.hide(1000);
                            // }
                        } else {
                            AlertaGeneral(`Alerta`, `La compra "${selectCC.val()}-${inputNumero.val()}" está cancelada.`);

                            limpiarInformacion();
                            limpiarTabla(tblPartidas);

                            selectCC.val('');
                            inputNumero.val('');
                            textAreaDescPartida.val('');
                            divNumMovimiento.hide(1000);
                        }
                    } else {
                        AlertaGeneral('Alerta', 'No se encontró información.');

                        limpiarInformacion();
                        limpiarTabla(tblPartidas);

                        textAreaDescPartida.val('');
                    }
                } else {
                    AlertaGeneral('Alerta', 'Error al consultar la información.');
                }
            });
        }

        function getInfoCompra() {
            let compra = {
                //#region Panel Izquierdo
                cc: selectCC.val(),
                numero: inputNumero.val(),
                bienes_servicios: selectBoS.val(),
                fecha: dtpFecha.val(),
                proveedor: inputProvNum.val(),
                comprador: inputCompNum.val(),
                solicito: inputSolNum.val(),
                autorizo: inputAutNum.val(),
                //selectEmb.val(),
                embarquese: inputEmb.val(),
                libre_abordo: selectLab.val(),
                //inputObra.val(),
                //inputEstimacion.val(),
                concepto_factura: inputConFact.val(),

                bit_autorecepcion: checkAutoRecep.prop("checked") ? 'S' : 'N',

                almacen_autorecepcion: inputAlmNum.val(),
                empleado_autorecepcion: inputEmpNum.val(),
                //#endregion

                //#region Panel Derecho
                tipo_oc_req: selectTipoOC.val(),
                moneda: selectMoneda.val(),
                tipo_cambio: inputTipoCambio.val(),
                sub_total: unmaskNumero(inputSubTotal.val()),
                porcent_iva: inputIVAPorcentaje.val(),
                iva: unmaskNumero(inputIVANumero.val()),
                rentencion_despues_iva: unmaskNumero(inputRetencion.val()),
                total: unmaskNumero(inputTotal.val()),
                //inputTotalFinal.val()
                //#endregion
            };

            return compra;
        }

        function cargarHistorialInsumo() {
            if (_filaInsumo != null && selectLab.val() > 0) {
                let rowData = tblPartidas.DataTable().row(_filaInsumo).data();
                let almacen = selectLab.val();
                let insumo = rowData.insumo;

                $.post('/Enkontrol/Almacen/GetHistorialInsumo', { almacen, insumo }).then(response => {
                    if (response.success) {
                        AddRows(tblHistorialInsumo, response.data);
                        mdlHistorialInsumo.modal('show');
                    } else {

                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            } else {
                if (!(selectLab.val() > 0)) {
                    AlertaGeneral(`Alerta`, `No hay un almacén (LAB) seleccionado.`);
                }
            }
        }

        const getUrlParams = function (url) {
            let params = {};
            let parser = document.createElement('a');
            parser.href = url;
            let query = parser.search.substring(1);
            let vars = query.split('&');

            for (let i = 0; i < vars.length; i++) {
                let pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }

            return params;
        };

        function verReporte(entradaNueva) {
            $.blockUI({ message: 'Generando reporte...' });

            report.attr("src", `/Reportes/Vista.aspx?idReporte=118`);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI();
            };

            if (entradaNueva) {
                limpiarInformacion();
                limpiarTabla(tblPartidas);
                selectCC.val('');
                inputNumero.val('');
                inputNumeroReq.val('');
            }
        }

        function aplicarGlobal() {
            if (tblPartidas.DataTable().data().any()) {
                tblPartidas.find('tbody tr').each(function (id, row) {
                    let rowData = tblPartidas.DataTable().row(row).data();
                    let inputCantidadRecibida = $(row).find('.inputCantidadRecibida');

                    inputCantidadRecibida.val(rowData.cantidadPendiente);
                });

                AlertaGeneral(`Alerta`, `Entrada Global. Verifique las cantidades capturadas antes de guardar.`);
            }
        }

        function cargarCatalogoUbicaciones() {
            if (_filaInsumo != null) {
                let almacenID = selectLab.val();

                if (almacenID > 0) {
                    $.post('/Enkontrol/Almacen/GetCatalogoUbicaciones', { almacenID }).then(response => {
                        if (response.success) {
                            AddRows(tblCatalogoUbicaciones, response.data);
                            mdlCatalogoUbicaciones.modal('show');
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
                } else {
                    AlertaGeneral(`Alerta`, `Número de almacén inválido.`);
                }
            }
        }

        String.prototype.parseDate = function () {
            return new Date(parseInt(this.replace('/Date(', '')));
        }
        Date.prototype.parseDate = function () {
            return this;
        }
        Date.prototype.addDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
        }
        $.fn.commasFormat = function () {
            this.each(function (i) {
                $(this).change(function (e) {
                    if (isNaN(parseFloat(this.value))) return;
                    this.value = parseFloat(this.value).toFixed(6);
                });
            });
            return this;
        }

        function llenarInputProveedorDistinto(numeroProveedor) {
            let listInputs = tblPartidas.find('tbody tr .inputProveedorDistinto');

            $(listInputs).each(function (id, element) {
                $(element).val(numeroProveedor);
            });
        }

        function verificarComprasPeru() {
            if (_empresaActual == 6) {
                // selectMoneda.attr('disabled', false);
                $('.elementoPeru').show();
                $(".spanIVA").html("I.G.V.");
                $(".elementoNoPeru").hide();
            } else {
                $('.elementoPeru').hide();
                $(".spanIVA").html("I.V.A.");
            }
        }

        init();
    }

    $(document).ready(function () {
        Enkontrol.Compras.OrdenCompra.Surtido = new Surtido();
    })
        .ajaxStart(function () {
            $.blockUI({ message: 'Procesando...' });
        })
        .ajaxStop(function () {
            $.unblockUI();
        });
})();