(() => {
    $.namespace('Enkontrol.Compras.Requisicion.Surtido');
    Surtido = function () {
        //#region Selectores
        const tblInsumos = $('#tblInsumos');
        const tblSurtido = $('#tblSurtido');
        const mdlSurtirAlmacen = $('#mdlSurtirAlmacen');
        const headerModalSurtirAlmacen = $('#headerModalSurtirAlmacen');
        const btnGuardarSurtir = $('#btnGuardarSurtir');
        const selectCCSurtido = $('#selectCCSurtido');
        const inputNumeroSurtido = $('#inputNumeroSurtido');
        const btnCapturarSurtirAlmacen = $('#btnCapturarSurtirAlmacen');
        const btnValidadoAlmacen = $('#btnValidadoAlmacen');
        const selTipoReq = $("#selTipoReq");
        const selAutorizo = $("#selAutorizo");
        const selLab = $("#selLab");
        const dtFecha = $("#dtFecha");
        const txtFolio = $('#txtFolio');
        const txtSolicito = $("#txtSolicito");
        const txtEmpNum = $("#txtEmpNum");
        const txtEmpNom = $("#txtEmpNom");
        const txtUsuNom = $("#txtUsuNom");
        const txtUsuNum = $("#txtUsuNum");
        const txtEstatus = $("#txtEstatus");
        const txtComentarios = $("#txtComentarios");
        const txtDescPartida = $("#txtDescPartida");
        const txtModificacion = $("#txtModificacion");
        const spanEstatus = $("#spanEstatus");
        const spanActivos = $("#spanActivos");
        const txtFolioOrigen = $('#txtFolioOrigen');
        const selectTipoFolio = $('#selectTipoFolio');
        const inputCantidadTotal = $('#inputCantidadTotal');
        const mdlExistenciaDetalle = $('#mdlExistenciaDetalle');
        const btnCancelarValidado = $('#btnCancelarValidado');
        const btnReporte = $('#btnReporte');
        const report = $("#report");
        const btnValidadoCompras = $('#btnValidadoCompras');
        const inputFechaSurtido = $('#inputFechaSurtido');
        const btnAddRenglon = $('#btnAddRenglon');
        const btnRemRenglon = $('#btnRemRenglon');
        //#endregion

        _filaInsumo = null;
        _cantidadInsumo = 0;
        _cantidadCapturadaInsumo = 0;
        _lstSurtidoPorAlmacen = null;

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();

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

        (function init() {
            initTablaInsumos();
            initTablaSurtido();

            selectCCSurtido.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null);
            selLab.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
            selTipoReq.fillCombo('/Enkontrol/Requisicion/FillComboTipoReq', null, false, null);
            selectTipoFolio.fillCombo('/Enkontrol/Requisicion/FillComboTipoFolio', null, false, null);

            selectCCSurtido.change(setUltimoNumeroRequisicion);
            inputNumeroSurtido.change(llenarRequisicion);
            btnValidadoAlmacen.click(validarSurtido);
            btnValidadoCompras.click(validarSurtidoCompras);
            btnCancelarValidado.click(cancelarValidado);
            btnReporte.click(getReporte);
            btnAddRenglon.click(AddNewRenglon);
            btnRemRenglon.click(RemSelRenglon);

            inputFechaSurtido.datepicker({ dateFormat, showAnim }).datepicker("setDate", fechaActual);

            // Se comprueba si hay variables en url.
            const variables = getUrlParams(window.location.href);
            if (variables && variables.cc && variables.req) {
                selectCCSurtido.val(variables.cc);
                inputNumeroSurtido.val(variables.req);
                inputNumeroSurtido.change();
            }
        })();

        tblInsumos.on('click', '.btn-quitar-insumo', function () {
            let td = $(this).closest('td');
            let campoCantidad = $(this).closest('tr').find('.cantidad');
            let campoNuevaCaptura = $(this).closest('tr').find('.capturar');
            let inputComentarioQuitar = $(this).closest('tr').find('.inputComentarioQuitar');

            if ($(this).attr('data-quitar') == 'true') {
                inputComentarioQuitar.remove();
                $(this).removeClass('btn-danger').addClass('btn-default');
                $(this).attr('data-quitar', 'false');

                campoCantidad.removeClass('campoInvalido');
                campoCantidad.val(campoCantidad.attr('data-valor-anterior')).change();

                $(this).closest('tr').removeClass('renglonQuitar');
            } else {
                $(td).append(`<input class="form-control inputComentarioQuitar" style="width: 75%; margin-left: 5px; display: inline-block;">`);
                $(this).removeClass('btn-default').addClass('btn-danger');
                $(this).attr('data-quitar', 'true');

                campoCantidad.attr('data-valor-anterior', campoCantidad.val());
                campoCantidad.addClass('campoInvalido');
                campoCantidad.val(0).change();
                campoNuevaCaptura.val() != '' ? campoNuevaCaptura.val(0) : campoNuevaCaptura.val('');

                $(this).closest('tr').addClass('renglonQuitar');

                tblInsumos.DataTable().columns.adjust().draw();
            }
        });

        tblInsumos.on("click", ".surtidoBoton", function (e) {
            let row = $(this).closest('tr');
            let insumo = row.find('td .insumo').val();
            let cantidadSolicitada = row.find('td .cantidad').val();
            let unidad = row.find('td .unidad').val();

            let insumoDesc = row.find('td .insumoDesc').val();
            headerModalSurtirAlmacen.find('label').text('Insumo: ' + insumo + ' - ' + insumoDesc);

            let lstSurtidoPorAlmacen = $(this).data('lstSurtidoPorAlmacen');

            _filaInsumo = row;
            _cantidadInsumo = parseFloat(row.find('td .cantidad').val());
            _cantidadCapturadaInsumo = parseFloat(row.find('td .cantidadCapturada').val());
            _lstSurtidoPorAlmacen = $(this).data('lstSurtidoPorAlmacen');

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetExistenciaInsumoDetalleAlmacenFisico', { insumo })
                .always($.unblockUI)
                .then(response => {
                    let detalle = [];

                    if (response.success) {
                        detalle = response.existencia;
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }

                    tablaSurtirPorAlmacen(detalle, lstSurtidoPorAlmacen, insumo, cantidadSolicitada, unidad);
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        tblInsumos.on("click", ".existenciaBoton", function (e) {
            let row = $(this).closest('tr');
            let rowData = tblInsumos.DataTable().row(row).data();

            tablaExistenciaTotalDetalle(rowData.insumo);
        });

        // tblInsumos.on("change", ".cantidad", function (e) {
        //     let row = $(this).closest('tr'),
        //         data = row.data(),
        //         valor = +(this.value);
        //     row.find(".cantidad");
        //     //row.find(".porComprar").val(valor).change();
        //     row.find(".exceso").val(valor + data.exceso).change();

        //     getCantidadTotal();
        // });

        btnGuardarSurtir.on('click', function () {
            let lstSurtido = [];
            let lstParaOrdenCompra = [];
            let flagCapturaMayor = false;
            let flagAlmacenVacio = false;
            let flagQuitarYaSurtido = false;
            let flagComentarioQuitar = false;
            let nuevaCapturaTotal = 0;

            tblInsumos.find("tbody tr").each(function (idx, row) {
                // let rowData = tblInsumos.DataTable().row(row).data();
                let lstSurtidoPorAlmacen = $(this).find('.surtidoBoton').data().lstSurtidoPorAlmacen;
                let aSurtirTotal = 0;

                if (lstSurtidoPorAlmacen != undefined) {
                    $(lstSurtidoPorAlmacen).each(function (id, item) {
                        nuevaCapturaTotal += item.aSurtir;
                        aSurtirTotal += item.aSurtir;
                    });
                }

                let rowData = tblInsumos.DataTable().row(row).data();
                let quitar = $(this).find('.btn-quitar-insumo').attr('data-quitar') == 'true' ? true : false;
                let comentarioSurtidoQuitar = quitar ? $(row).find('.inputComentarioQuitar').val() : '';

                if (quitar && comentarioSurtidoQuitar.length == 0) {
                    flagComentarioQuitar = true;
                }

                lstSurtido.push({
                    partida: +($(row).find('td:eq(0)').text()),
                    insumo: rowData.insumo,
                    cantidad: $(this).find('.cantidad').val(),
                    cantidadCapturada: $(this).find('.cantidadCapturada').val(),
                    nuevaCaptura: ((lstSurtidoPorAlmacen != undefined) ? (lstSurtidoPorAlmacen) : (null)),
                    quitar: quitar,
                    comentarioSurtidoQuitar: comentarioSurtidoQuitar,
                    mayorSurtir: aSurtirTotal > (parseFloat($(this).find('.cantidad').val())
                        //  - parseFloat($(this).find('.cantidadCapturada').val()) //Se quitó la resta de lo ya capturado para que no marque error cuando editan una requisición que ya tenía valores capturados.
                    )
                });

                lstParaOrdenCompra.push({
                    insumo: rowData.insumo,
                    cantidadSolicitada: parseFloat($(this).find('.cantidad').val()),
                    cantidadSurtir: parseFloat($(this).find('.cantidadCapturada').val()) + aSurtirTotal,
                    cantidadParaCompra: ($(this).find('.cantidad').val()) - ($(this).find('.cantidadCapturada').val() + aSurtirTotal),
                    quitar: $(this).find('.btn-quitar-insumo').attr('data-quitar') == 'true' ? true : false
                });
            });

            // if (lstSurtido.find(function (sur) { return nuevaCapturaTotal > (sur.cantidad + sur.cantidadCapturada); }) != undefined) {
            if (lstSurtido.find(function (sur) { return sur.mayorSurtir; }) != undefined) {
                flagCapturaMayor = true;
            }

            if (lstSurtido.find(function (sur) { return sur.almacenOrigenID == '' && nuevaCapturaTotal > 0; }) != undefined) {
                flagAlmacenVacio = true;
            }

            if (lstSurtido.find(function (sur) { return sur.quitar && sur.cantidadCapturada > 0; }) != undefined) {
                flagQuitarYaSurtido = true;
            }

            if (flagComentarioQuitar) {
                AlertaGeneral(`Alerta`, `Debe capturar un comentario para cada partida que va quitar.`);
                return;
            }

            if (!flagCapturaMayor && !flagAlmacenVacio && !flagQuitarYaSurtido) {
                let fechaSurtidoCompromiso = inputFechaSurtido.val() != '' ? inputFechaSurtido.val() : null;
                let info = { cc: selectCCSurtido.val(), numero: inputNumeroSurtido.val(), fechaSurtidoCompromiso };

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Enkontrol/Requisicion/GuardarSurtido', { info, lstSurtido })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AlertaGeneral('Alerta', 'Se guardó la información del surtido.');

                            selectCCSurtido.change();
                        } else {
                            AlertaGeneral(`Alerta`, response.message);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                if (flagCapturaMayor) { AlertaGeneral('Alerta', 'No puede capturar una cantidad mayor a la solicitada.'); }
                if (flagAlmacenVacio) { AlertaGeneral('Alerta', 'Seleccione el almacén para la captura de surtido.'); }
                if (flagQuitarYaSurtido) { AlertaGeneral('Alerta', 'Ya tiene surtido ese insumo.'); }
            }
        });

        btnCapturarSurtirAlmacen.on('click', function () {
            let flagSobrepasoDisponible = false;
            let lstSurtidoPorAlmacen = [];
            let totalASurtir = 0;

            tblSurtido.find("tbody tr").each(function (idx, row) {
                let rowData = tblSurtido.DataTable().row(row).data();

                if (typeof rowData !== 'undefined') {
                    let aSurtir = $(row).find('.inputSurtirAlmacen').val() != '' ? parseFloat($(row).find('.inputSurtirAlmacen').val()) : 0;
                    let almacenID = rowData.almacenID;
                    let almacenDesc = rowData.almacen;
                    let disponiblePorAlmacen = +($(row).find('td:eq(8)').text());
                    let area_alm = rowData.area_alm;
                    let lado_alm = rowData.lado_alm;
                    let estante_alm = rowData.estante_alm;
                    let nivel_alm = rowData.nivel_alm;

                    let capturaPreviaSurtido = _lstSurtidoPorAlmacen.filter(function (x) {
                        return x.almacenID == almacenID && x.area_alm == area_alm && x.lado_alm == lado_alm && x.estante_alm == estante_alm && x.nivel_alm == nivel_alm
                    });

                    if (aSurtir > disponiblePorAlmacen) {
                        flagSobrepasoDisponible = true;
                        $(row).find('.inputSurtirAlmacen').addClass('campoInvalido');
                    } else {
                        $(row).find('.inputSurtirAlmacen').removeClass('campoInvalido');
                    }

                    if (aSurtir < 0) {
                        AlertaGeneral('Alerta', 'No puede introducir números negativos.');
                    }

                    totalASurtir += aSurtir;
                    lstSurtidoPorAlmacen.push({
                        almacenID,
                        almacenDesc,
                        area_alm,
                        lado_alm,
                        estante_alm,
                        nivel_alm,
                        aSurtir
                    });
                }
            });

            if (flagSobrepasoDisponible) {
                AlertaGeneral('Alerta', 'No puede surtir más de lo disponible por almacén.');
            } else {
                if (!(totalASurtir > (_cantidadInsumo - _cantidadCapturadaInsumo))) {
                    _filaInsumo.find('td .surtidoBoton').data('lstSurtidoPorAlmacen', lstSurtidoPorAlmacen);
                    _filaInsumo.find('td .surtidoBoton').data('totalASurtir', totalASurtir);
                    _filaInsumo.find('td .capturarSurtido').val(totalASurtir);
                    _filaInsumo.find('td .capturarSurtido').text(totalASurtir);
                    mdlSurtirAlmacen.modal('hide');
                } else {
                    AlertaGeneral('Alerta', 'La cantidad a surtir supera la cantidad restante solicitada por la requisición.');
                }
            }
        });

        function tablaSurtirPorAlmacen(detalle, lstSurtidoPorAlmacen, insumo, cantidadSolicitada, unidad) {
            let detalleFiltrado = detalle.filter(almacen => almacen.existencia > 0);

            agregarRowsCallback(tblSurtido, detalleFiltrado, function () {
                tblSurtido.find('tbody tr').each((index, row) => {
                    let rowData = tblSurtido.DataTable().row(row).data();

                    if (lstSurtidoPorAlmacen != undefined) {
                        let renglonSurtido = lstSurtidoPorAlmacen.find(function (x) {
                            return x.almacenID == rowData.almacenID &&
                                x.area_alm == rowData.area_alm &&
                                x.lado_alm == rowData.lado_alm &&
                                x.estante_alm == rowData.estante_alm &&
                                x.nivel_alm == rowData.nivel_alm;
                        });

                        if (renglonSurtido != undefined && renglonSurtido != null) {
                            if (renglonSurtido.aSurtir != 0) {
                                $(row).find('.inputSurtirAlmacen').val(renglonSurtido.aSurtir);
                            } else {
                                $(row).find('.inputSurtirAlmacen').val('');
                            }
                        }
                    }
                });
            });

            $('#labelCantidadSolicitada').text('Cantidad Solicitada: ' + cantidadSolicitada + ' ' + unidad);

            mdlSurtirAlmacen.modal('show');
        }

        // function crearTabla() {
        //     let table = document.createElement('table');
        //     table.setAttribute('id', 'tblExistenciaDetalle');
        //     table.setAttribute('style', 'width: 100%; margin-bottom: 0px;');
        //     table.classList.add('table');
        //     table.classList.add('tblExistenciaDetalle');

        //     let head = document.createElement('thead');

        //     let tr = document.createElement('tr');

        //     let thAlmacen = document.createElement('th');
        //     thAlmacen.textContent = 'Almacen';
        //     thAlmacen.classList.add('text-center');

        //     let thEntradas = document.createElement('th');
        //     thEntradas.textContent = 'Entradas';
        //     thEntradas.classList.add('text-center');

        //     let thSalidas = document.createElement('th');
        //     thSalidas.textContent = 'Salidas';
        //     thSalidas.classList.add('text-center');

        //     let thUltConsumo = document.createElement('th');
        //     thUltConsumo.textContent = 'Últ. Consumo';
        //     thUltConsumo.classList.add('text-center');

        //     let thUltCompra = document.createElement('th');
        //     thUltCompra.textContent = 'Últ. Compra';
        //     thUltCompra.classList.add('text-center');

        //     let thExistencias = document.createElement('th');
        //     thExistencias.textContent = 'Existencias';
        //     thExistencias.classList.add('text-center');

        //     let thMinimo = document.createElement('th');
        //     thMinimo.textContent = 'Mínimo';
        //     thMinimo.classList.add('text-center');

        //     let thReservados = document.createElement('th');
        //     thReservados.textContent = 'Reservado/Transito';
        //     thReservados.classList.add('text-center');

        //     let thExistenciaReal = document.createElement('th');
        //     thExistenciaReal.textContent = 'Disponible';
        //     thExistenciaReal.classList.add('text-center');

        //     let thSurtir = document.createElement('th');
        //     thSurtir.textContent = 'A Surtir';
        //     thSurtir.classList.add('text-center');

        //     $(tr).append(thAlmacen);
        //     $(tr).append(thEntradas);
        //     $(tr).append(thSalidas);
        //     $(tr).append(thUltConsumo);
        //     $(tr).append(thUltCompra);
        //     $(tr).append(thExistencias);
        //     $(tr).append(thMinimo);
        //     $(tr).append(thReservados);
        //     $(tr).append(thExistenciaReal);
        //     $(tr).append(thSurtir);

        //     $(head).append(tr);
        //     $(table).append(head);

        //     return table;
        // }

        // function crearRowsDetalle(detalle, renglonSurtido, insumo) {

        //     if (detalle.almacenID >= 600 && detalle.almacenID <= 620) {
        //         let familia = insumo.substring(0, 3);

        //         if (familia == '101' || familia == '102') {
        //             AlertaGeneral(`Alerta`, `No se puede dar salida a las familias de insumos "101" y "102" en los almacenes del 600 al 620.`);
        //         }
        //     }
        //     let tr = document.createElement('tr');

        //     let tdAlmacen = document.createElement('td');
        //     tdAlmacen.textContent = detalle.almacen;
        //     tdAlmacen.classList.add('text-center');
        //     tdAlmacen.classList.add('tdAlmacen');
        //     tdAlmacen.style.verticalAlign = 'middle';

        //     $(tdAlmacen).attr('data-almacenid', detalle.almacenID);
        //     $(tdAlmacen).attr('data-almacendesc', detalle.almacen);

        //     let tdEntradas = document.createElement('td');
        //     tdEntradas.setAttribute('align', 'right');
        //     tdEntradas.textContent = formatValue(detalle.entradas);
        //     tdEntradas.classList.add('text-center');
        //     tdEntradas.style.verticalAlign = 'middle';

        //     let tdSalidas = document.createElement('td');
        //     tdSalidas.setAttribute('align', 'right');
        //     tdSalidas.textContent = formatValue(detalle.salidas);
        //     tdSalidas.classList.add('text-center');
        //     tdSalidas.style.verticalAlign = 'middle';

        //     let tdUltConsumo = document.createElement('td');
        //     tdUltConsumo.textContent = detalle.ultimoConsumoString;
        //     tdUltConsumo.classList.add('text-center');
        //     tdUltConsumo.style.verticalAlign = 'middle';

        //     let tdUltCompra = document.createElement('td');
        //     tdUltCompra.textContent = detalle.ultimaCompraString;
        //     tdUltCompra.classList.add('text-center');
        //     tdUltCompra.style.verticalAlign = 'middle';

        //     let tdExistencia = document.createElement('td');
        //     tdExistencia.setAttribute('align', 'right');
        //     tdExistencia.textContent = formatValue(detalle.existencia);
        //     tdExistencia.classList.add('text-center');
        //     tdExistencia.style.verticalAlign = 'middle';

        //     let tdMinimo = document.createElement('td');
        //     tdMinimo.textContent = detalle.minimo == 'SOBRE PEDIDO' ? 'SP' : detalle.minimo != 0 ? formatValue(detalle.minimo) : '';
        //     tdMinimo.classList.add('text-center');
        //     tdMinimo.style.verticalAlign = 'middle';

        //     let tdReservados = document.createElement('td');
        //     tdReservados.setAttribute('align', 'right');
        //     tdReservados.textContent = formatValue(detalle.reservados);
        //     tdReservados.classList.add('text-center');
        //     tdReservados.style.verticalAlign = 'middle';

        //     let tdExistenciaReal = document.createElement('td');
        //     tdExistenciaReal.setAttribute('align', 'right');
        //     tdExistenciaReal.textContent = formatValue(detalle.existencia - detalle.reservados);
        //     tdExistenciaReal.classList.add('text-center');
        //     tdExistenciaReal.style.verticalAlign = 'middle';

        //     let tdSurtir = document.createElement('td');
        //     tdSurtir.classList.add('text-center');
        //     tdSurtir.style.verticalAlign = 'middle';
        //     let inputSurtir = document.createElement('input');
        //     inputSurtir.classList.add('form-control');
        //     inputSurtir.classList.add('text-center');
        //     inputSurtir.classList.add('inputSurtirAlmacen');

        //     $(inputSurtir).val(renglonSurtido != undefined && renglonSurtido != null ? renglonSurtido.aSurtir != 0 ? renglonSurtido.aSurtir : '' : '');

        //     $(inputSurtir).attr('data-existencia', formatValue(detalle.existencia));
        //     $(tdSurtir).append(inputSurtir);

        //     $(tr).append(tdAlmacen);
        //     $(tr).append(tdEntradas);
        //     $(tr).append(tdSalidas);
        //     $(tr).append(tdUltConsumo);
        //     $(tr).append(tdUltCompra);
        //     $(tr).append(tdExistencia);
        //     $(tr).append(tdMinimo);
        //     $(tr).append(tdReservados);
        //     $(tr).append(tdExistenciaReal);
        //     $(tr).append(tdSurtir);

        //     return tr;
        // }

        // function setPartidas(partidas) {
        //     $.ajaxSetup({ async: false });

        //     for (let i = 0; i < partidas.length; i++) {
        //         let listaSurtido = [];

        //         $(partidas[i].listaSurtido).each(function (idx, element) {
        //             listaSurtido.push({ almacenID: element.almacenID, aSurtir: element.aSurtir });
        //         });

        //         _renglonNuevo(partidas[i].partida, false, partidas[i].cancelado == 'A' ? false : true).done(function (_renglon) {
        //             let $renglon = setInsumo(initRenglonInsumo($(_renglon)), partidas[i]);

        //             agregarTooltip($renglon.find('.btn-estatus-activo'), 'ACTIVO');
        //             agregarTooltip($renglon.find('.btn-estatus-inactivo'), 'INACTIVO');

        //             tblInsumos.find(`tbody`).append($renglon);

        //             $renglon.find('.existencia').text(getExistencia(partidas[i].insumo, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
        //             $renglon.find('.existenciaBoton').removeClass('hidden');

        //             agregarTooltip($renglon.find('.existenciaBoton'), 'Desglose por Almacén');

        //             $renglon.data({
        //                 insumo: partidas[i].insumo
        //             });

        //             $renglon.find('.selectAlmacen').fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);

        //             $renglon.find('td .surtidoBoton').data('lstSurtidoPorAlmacen', listaSurtido);
        //             $renglon.find('td .surtidoBoton').data('totalASurtir', partidas[i].totalASurtir);
        //             $renglon.find('td .capturarSurtido').val(partidas[i].totalASurtir);
        //             $renglon.find('td .capturarSurtido').text(partidas[i].totalASurtir);
        //         });
        //     }

        //     $.ajaxSetup({ async: true });

        //     getCantidadTotal();
        // }

        // function _renglonNuevo(partida, nuevo, cancelado) {
        //     return $.post('/Enkontrol/Requisicion/_renglonNuevo', { partida, nuevo, cancelado });
        // }

        // function setInsumo(row, p) {
        //     row.find('.insumo').val(p.insumo).change();
        //     row.find('.insumoDesc').val(p.insumoDesc);
        //     if (p.area == 0)
        //         row.find('.areaCuenta').val("000-000");
        //     else
        //         row.find(`.areaCuenta option[value="${p.area}"][data-prefijo="${p.cuenta}"]`).prop("selected", true).val(p.area);
        //     row.find('.referencia').val(p.referencia_1);
        //     row.find('.cantidad').val(p.cantidad).change();
        //     row.find('.unidad').val(p.unidad);
        //     row.find('.porComprar').val(p.cant_ordenada).change();
        //     row.find('.exceso').val(p.cantidad_excedida_ppto).change();
        //     row.find('.btn-estatus').attr('data-observaciones', p.observaciones != null ? p.observaciones : '');
        //     row.find('.observaciones').val(p.observaciones);
        //     row.find('.cantidadCapturada').val(p.cantidadCapturada)
        //     row.data({
        //         id: p.id,
        //         idReq: p.idReq,
        //         partida: p.partida,
        //         DescPartida: p.partidaDesc,
        //         exceso: 0,
        //         isAreaCueta: p.isAreaCueta
        //     });
        //     row = setRowRadioValue(row, `radCancel${p.partida}`, p.cant_cancelada != 0);
        //     return row;
        // }

        // function setRowRadioValue(row, tog, sel) {
        //     row.find(`#${tog}`).prop('value', sel);
        //     row.find(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
        //     row.find(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');

        //     return row;
        // }

        // function initRenglonInsumo(row, partida) {
        //     row.find('.insumo').getAutocomplete(setInsumoDesc, { cc: selectCCSurtido.val() }, '/Enkontrol/Requisicion/getInsumos');
        //     row.find('.insumoDesc').getAutocomplete(setInsumoBusqPorDesc, { cc: selectCCSurtido.val() }, '/Enkontrol/Requisicion/getInsumosDesc');

        //     row.find('.areaCuenta').fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: selectCCSurtido.val() }, false, "000-000");
        //     //row.find('.fechaReq').datepicker().datepicker("setDate", new Date().addDays(selTipoReq.find(":selected").data().prefijo).toLocaleDateString());
        //     row.find(".cantidad").val(0).commasFormat().change();
        //     row.find(".porComprar").val(0).commasFormat().change();
        //     row.find(".exceso").val(0).commasFormat().change();
        //     //row.find(".existencia").val(0).commasFormat().change();

        //     agregarTooltip(row.find('.btn-estatus-activo'), 'ACTIVO');
        //     agregarTooltip(row.find('.btn-estatus-inactivo'), 'INACTIVO');

        //     row.data({
        //         id: 0,
        //         idReq: 0,
        //         partida: partida,
        //         DescPartida: "",
        //         exceso: 0,
        //         isAreaCueta: false
        //     });

        //     return row;
        // }

        // function setInsumoDesc(e, ui) {
        //     let exceso = ui.item.exceso,
        //         isAreaCueta = ui.item.isAreaCueta,
        //         row = $(this).closest('tr'),
        //         valor = row.find(".cantidad").val();
        //     row.find('.insumoDesc').val(ui.item.id);
        //     row.find('.unidad').text(ui.item.unidad);
        //     row.find(".porComprar").val(valor).change();
        //     row.find('.exceso').val(valor + exceso).change();
        //     row.find('.existencia').text(getExistencia(ui.item.value, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
        //     row.find('.existenciaBoton').removeClass('hidden');
        //     agregarTooltip(row.find('.existenciaBoton'), 'Desglose por Almacén');
        //     row.data({
        //         exceso: exceso,
        //         isAreaCueta: isAreaCueta,
        //         insumo: ui.item.value
        //     });

        //     if (isAreaCueta)
        //         if (row.find(".areaCuenta").val() == "000-000")
        //             row.find(".areaCuenta").val(row.find(".areaCuenta option:eq(1)").val());

        //     if (ui.item.cancelado == 'A') {
        //         row.find('.btn-estatus-activo').css('display', 'inline-block');
        //         row.find('.btn-estatus-inactivo').css('display', 'none');
        //         row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
        //     } else {
        //         row.find('.btn-estatus-activo').css('display', 'none');
        //         row.find('.btn-estatus-inactivo').css('display', 'inline-block');
        //         row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
        //     }
        // }

        // function setInsumoBusqPorDesc(e, ui) {
        //     let exceso = ui.item.exceso,
        //         isAreaCueta = ui.item.isAreaCueta,
        //         row = $(this).closest('tr'),
        //         valor = row.find(".cantidad").val();
        //     row.find('.insumo').val(ui.item.id);
        //     row.find('.insumoDesc').val(ui.item.value);
        //     row.find('.unidad').text(ui.item.unidad);
        //     row.find('.existencia').text(getExistencia(ui.item.id, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
        //     row.find('.existenciaBoton').removeClass('hidden');
        //     agregarTooltip(row.find('.existenciaBoton'), 'Desglose por Almacén');
        //     row.data({
        //         exceso: exceso,
        //         isAreaCueta: isAreaCueta,
        //         insumo: ui.item.id
        //     });

        //     row.find(".porComprar").val(valor).change();
        //     row.find('.exceso').val(valor + exceso).change();

        //     if (isAreaCueta)
        //         if (row.find(".areaCuenta").val() == "000-000")
        //             row.find(".areaCuenta").val(row.find(".areaCuenta option:eq(1)").val());

        //     if (ui.item.cancelado == 'A') {
        //         row.find('.btn-estatus-activo').css('display', 'inline-block');
        //         row.find('.btn-estatus-inactivo').css('display', 'none');
        //         row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
        //     } else {
        //         row.find('.btn-estatus-activo').css('display', 'none');
        //         row.find('.btn-estatus-inactivo').css('display', 'inline-block');
        //         row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
        //     }
        // }

        // function getCantidadTotal() {
        //     let cantidadTotal = 0;
        //     let inputs = tblInsumos.find('tbody tr .cantidad');

        //     inputs.each(function (index, elemento) {
        //         cantidadTotal += parseFloat(elemento.value)
        //     });

        //     inputCantidadTotal.val(cantidadTotal).commasFormat();
        // }

        function setRequisicion(req) {
            selectCCSurtido.val(req.cc);
            inputNumeroSurtido.val(req.numero != '-1' ? req.numero : '');
            dtFecha.val(req.fecha.parseDate().toLocaleDateString());
            selLab.val(req.libre_abordo);

            if (selTipoReq.find('option[value="' + req.tipo_req_oc + '"]').length) {
                selTipoReq.val(req.tipo_req_oc)
            } else {
                AlertaGeneral('Alerta', 'No se encontró el tipo de requisición.');
                selTipoReq.val('1');
            }

            txtFolioOrigen.val(req.folioOrigen);

            setRadioValue("radTmc", req.tmc == 1);
            txtEstatus.val(req.st_autoriza == "N" ? "No autorizada" : "Autorizada");

            let fechaModificacion =
                (req.fecha_modificaString != null ? req.fecha_modificaString : '') + ' ' + (req.hora_modificaString != null ? req.hora_modificaString : '')
            txtModificacion.val(req.numero == 0 ? "" : fechaModificacion);
            txtComentarios.val(req.comentarios);
            txtSolicito.val(req.solicitoNom);
            selAutorizo.val(req.autorizo);
            txtEmpNum.val(req.vobo);
            txtEmpNom.text(req.voboNom);
            txtUsuNum.val(req.empleado_modifica);
            txtUsuNom.text(req.empModificaNom);
            inputNumeroSurtido.data({
                id: req.id,
                solicito: req.solicito,
                st_estatus: req.st_estatus,
                st_impresa: req.st_impresa,
                st_autoriza: req.st_autoriza,
                autoriza_activos: req.autoriza_activos,
                num_vobo: req.num_vobo,
                autoriza: req.fecha_autoriza == null ? "" : req.fecha_autoriza.parseDate()
            });
            setInputEstatus(req.st_estatus);
            req.autoriza_activos == 1 ? spanActivos.removeClass("hidden") : spanActivos.addClass("hidden");
            $('#checkboxConsigna').attr('checked', req.consigna == true ? true : false);
        }

        function setRadioValue(tog, sel) {
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }

        function setInputEstatus(estatus) {
            let isEstatus = estatus != "";
            switch (estatus) {
                case "T":
                    spanEstatus.removeClass("hidden").text("Total Comprado");
                    break;
                case "P":
                    spanEstatus.removeClass("hidden").text("Parcial Comprado");
                    break;
                case "C":
                    spanEstatus.removeClass("hidden").text("Total Cancelado");
                    break;
                default:
                    spanEstatus.addClass("hidden").text("");
                    break;
            }
        }

        function limpiarVista() {
            selectTipoFolio.val('');
            txtFolio.val('');
            $('#fieldsetCaptura').find('input').val('');
            $('#fieldsetCaptura').find('select').val('');
            txtEmpNom.text('');
            txtUsuNom.text('');

            tblInsumos.find('tbody tr').remove();
            inputCantidadTotal.val('');
            txtDescPartida.val('');
        }

        function setUltimoNumeroRequisicion() {
            $.blockUI({ message: 'Cargando...' });

            $.post("/Enkontrol/Requisicion/GetUltimaRequisicionSIGOPLAN", { cc: selectCCSurtido.val() }).done(function (response) {
                inputNumeroSurtido.val(response.numero);
                inputNumeroSurtido.change();
            }).always($.unblockUI);
        }

        function llenarRequisicion() {
            btnGuardarSurtir.attr('disabled', false);
            btnValidadoCompras.attr('disabled', false);
            btnValidadoAlmacen.attr('disabled', false);
            btnCancelarValidado.attr('disabled', true);

            if (+(inputNumeroSurtido.val()) > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post("/Enkontrol/Requisicion/getReq", { cc: selectCCSurtido.val(), num: inputNumeroSurtido.val() }).done(function (response) {
                    if (response.success) {
                        if (!response.requisicionNueva) {
                            tblInsumos.DataTable().clear().draw(); // tblInsumos.find('tbody tr').remove();
                            AddRows(tblInsumos, response.partidas); // setPartidas(response.partidas);
                            setRequisicion(response.req);
                            inputFechaSurtido.val(response.req.fechaSurtidoCompromisoString);
                            tblInsumos.find('tbody tr:eq(0)').addClass('selected');
                            txtDescPartida.val(response.partidas[0].partidaDesc);

                            if (response.req.validadoAlmacen) {
                                AlertaGeneral(`Alerta`, `El surtido de esta Requisición ya está terminado`);
                                btnGuardarSurtir.attr('disabled', true);
                                btnValidadoCompras.attr('disabled', true);
                                btnValidadoAlmacen.attr('disabled', true);
                                btnCancelarValidado.attr('disabled', false);
                            }
                        } else {
                            setDefault(true);
                            inputNumeroSurtido.val(response.ultimaRequisicionNumero);
                            tblInsumos.DataTable().clear().draw(); // tblInsumos.find('tbody tr').remove();
                        }
                    } else {
                        setDefault(true);
                    }
                }).always($.unblockUI);
            } else {
                btnGuardarSurtir.attr('disabled', true);
                btnValidadoCompras.attr('disabled', true);
                btnValidadoAlmacen.attr('disabled', true);
                btnCancelarValidado.attr('disabled', false);
                tblInsumos.DataTable().clear().draw();
            }
        }

        function setDefault(nuevaRequisicion) {
            selAutorizo.prop("disabled", true).html("");
            txtDescPartida.prop("disabled", true).val("");

            txtDescPartida.data({
                partida: 1
            });

            inputCantidadTotal.val('')
            txtFolio.val('');

            txtModificacion.val('');

            setRequisicion({
                id: 0,
                cc: nuevaRequisicion ? selectCCSurtido.val() : '',
                numero: nuevaRequisicion ? inputNumeroSurtido.val() : '',
                fecha: new Date(),
                libre_abordo: "",
                tipo_req_oc: "",
                tmc: 0,
                fecha_autoriza: new Date(),
                fecha_modifica: new Date(),
                hora_modifica: new Date(),
                comentarios: "",
                solicitoNom: "",
                solicito: "",
                autorizo: "",
                vobo: "",
                voboNom: "",
                empleado_modifica: "",
                empModificaNom: "",
                st_estatus: "",
                st_impresa: "",
                st_autoriza: "N",
                autoriza_activos: 0,
                num_vobo: 0
            });

            if (selectCCSurtido.val() == '') {
                inputNumeroSurtido.val('');
                txtEstatus.val('');
            }

            if (nuevaRequisicion) {
                txtEstatus.val('');
                txtModificacion.val('');
            }

            _renglonVacio();
        }

        function _renglonVacio() {
            tblInsumos.find('tbody').load('/Enkontrol/Requisicion/_renglonVacio');
        }

        // function getExistencia(insumo, cc, almacen) {
        //     let existencia = 0;
        //     $.ajax({
        //         url: '/Enkontrol/Requisicion/GetExistenciaInsumo',
        //         datatype: "json",
        //         type: "GET",
        //         async: false,
        //         data: {
        //             insumo: insumo,
        //             cc: cc,
        //             almacen: almacen
        //         },
        //         success: function (response) {
        //             if (response.success) {
        //                 existencia = response.existencia;
        //             }
        //         }
        //     });

        //     return existencia;
        // }

        function tablaExistenciaTotalDetalle(insumo) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetExistenciaInsumoDetalleTotalAlmacenFisico', { insumo })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        mdlExistenciaDetalle.find('.modal-body').html('');

                        let row = '';
                        row = document.createElement('div');
                        row.setAttribute('style', 'max-height: 70vh; overflow-y: auto;');

                        let div = '';
                        div = document.createElement('div');
                        div.setAttribute('id', 'divTable');
                        div.classList.add('col-xs-12');
                        div.classList.add('col-sm-12');
                        div.classList.add('col-md-12');
                        div.classList.add('col-lg-12');

                        let table = crearTablaExistenciaTotal();
                        let body = document.createElement('tbody');

                        if (response.existencia != undefined) {
                            response.existencia.forEach(function (detalle) {
                                body.append(crearRowsTotalDetalle(detalle));
                            });
                        }

                        $(table).append(body);
                        $(div).append(table);
                        $(row).append(div);

                        mdlExistenciaDetalle.find('.modal-body').append(row);
                        mdlExistenciaDetalle.modal('show');
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function crearTablaExistenciaTotal() {
            let table = document.createElement('table');
            table.setAttribute('id', 'tblExistenciaDetalle');
            table.setAttribute('style', 'width: 100%; margin-bottom: 0px;');
            table.classList.add('table');
            table.classList.add('tblExistenciaDetalle');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thAlmacen = document.createElement('th');
            thAlmacen.textContent = 'Almacen';

            let thEntradas = document.createElement('th');
            thEntradas.textContent = 'Entradas';

            let thSalidas = document.createElement('th');
            thSalidas.textContent = 'Salidas';

            let thExistencias = document.createElement('th');
            thExistencias.textContent = 'Existencias';

            let thReservados = document.createElement('th');
            thReservados.textContent = 'Reservados';

            $(tr).append(thAlmacen);
            $(tr).append(thEntradas);
            $(tr).append(thSalidas);
            $(tr).append(thExistencias);
            $(tr).append(thReservados);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function crearRowsTotalDetalle(detalle) {
            let tr = document.createElement('tr');

            let tdAlmacen = document.createElement('td');
            tdAlmacen.textContent = detalle.almacen;

            let tdEntradas = document.createElement('td');
            tdEntradas.setAttribute('align', 'right');
            tdEntradas.textContent = formatValue(detalle.entradas);

            let tdSalidas = document.createElement('td');
            tdSalidas.setAttribute('align', 'right');
            tdSalidas.textContent = formatValue(detalle.salidas);

            let tdExistencia = document.createElement('td');
            tdExistencia.setAttribute('align', 'right');
            tdExistencia.textContent = formatValue(detalle.existencia);

            let tdReservados = document.createElement('td');
            tdReservados.setAttribute('align', 'right');
            tdReservados.textContent = formatValue(detalle.reservados);

            $(tr).append(tdAlmacen);
            $(tr).append(tdEntradas);
            $(tr).append(tdSalidas);
            $(tr).append(tdExistencia);
            $(tr).append(tdReservados);

            return tr;
        }

        function validarSurtido() {
            let cc = selectCCSurtido.val();
            let ccDesc = selectCCSurtido.find('option:selected').text();
            let numero = inputNumeroSurtido.val();

            AlertaConfirmarValidacion(`Alerta`, `¿Desea terminar el surtido de la requisición #${numero} del Centro de Costos "${ccDesc}"?`, cc, numero);
        }

        function AlertaConfirmarValidacion(titulo, mensaje, cc, numero) {
            if (mensaje == null) {
                mensaje = "Error en el resultado de la petición, favor de intentar de nuevo.";
            }

            $("#dialogalertaGeneral").removeClass('hide');
            $("#txtComentarioAlerta").html(mensaje);

            var opt = {
                title: titulo,
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                maxWidth: 600,
                minWidth: 400,
                position: {
                    my: "center",
                    at: "center",
                    within: $(".RenderBody")
                },
                buttons: [
                    {
                        text: "Sí",
                        click: function () {
                            confirmarSurtido(cc, numero);

                            $("#dialogalertaGeneral").addClass('hide');
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "No",
                        click: function () {
                            $("#dialogalertaGeneral").addClass('hide');
                            $(this).dialog("close");
                        }
                    }
                ]
            };

            var theDialog = $("#dialogalertaGeneral").dialog(opt);

            theDialog.dialog("open");
        }

        function confirmarSurtido(cc, numero) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Requisicion/ValidarSurtido', { cc: cc, numero: numero })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información del surtido.`);
                        limpiarVista();
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function validarSurtidoCompras() {
            let cc = selectCCSurtido.val();
            let ccDesc = selectCCSurtido.find('option:selected').text();
            let numero = inputNumeroSurtido.val();

            AlertaAceptarRechazarNormal('Alerta', `¿Desea validar el surtido para compras de la requisición #${numero} del Centro de Costos "${ccDesc}"?`,
                () => confirmarSurtidoCompras(cc, numero));
        }

        function confirmarSurtidoCompras(cc, numero) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Requisicion/ValidarSurtidoCompras', { cc, numero })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información del surtido.`);
                        limpiarVista();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function cancelarValidado() {
            let cc = selectCCSurtido.val();
            let ccDesc = selectCCSurtido.find('option:selected').text();
            let numero = inputNumeroSurtido.val();

            $("#dialogalertaGeneral").removeClass('hide');
            $("#txtComentarioAlerta").html(`¿Desea cancelar por completo el surtido establecido de la requisición #${numero} del Centro de Costos "${ccDesc}"?`);

            var opt = {
                title: 'Alerta',
                autoOpen: false,
                draggable: false,
                resizable: false,
                modal: true,
                maxWidth: 600,
                minWidth: 400,
                position: {
                    my: "center",
                    at: "center",
                    within: $(".RenderBody")
                },
                buttons: [
                    {
                        text: "Sí",
                        click: function () {
                            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                            $.post('/Enkontrol/Requisicion/CancelarValidado', { cc, numero })
                                .always($.unblockUI)
                                .then(response => {
                                    if (response.success) {
                                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                                        inputNumeroSurtido.change();
                                    } else {
                                        AlertaGeneral(`Alerta`, response.message);
                                    }
                                }, error => {
                                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                                }
                                );

                            $("#dialogalertaGeneral").addClass('hide');
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "No",
                        click: function () {
                            $("#dialogalertaGeneral").addClass('hide');
                            $(this).dialog("close");
                        }
                    }
                ]
            };

            var theDialog = $("#dialogalertaGeneral").dialog(opt);

            theDialog.dialog("open");
        }

        function getReporte() {
            let cc = selectCCSurtido.val();
            let numero = +(inputNumeroSurtido.val());

            if (cc == '' || isNaN(numero)) {
                AlertaGeneral(`Alerta`, `Seleccione el centro de costo y el número de la requisición.`);
                return;
            }

            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetReporteSurtidoRequisicion', { cc, numero })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        if (response.data.length > 0) {
                            verReporte();
                        } else {
                            AlertaGeneral(`Alerta`, `La requisición no tiene información de surtido capturada.`);
                        }
                    } else {
                        AlertaGeneral(`Alerta`, response.message);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function verReporte() {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=130');
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        // function agregarTooltip(elemento, mensaje) {
        //     $(elemento).attr('data-toggle', 'tooltip');
        //     $(elemento).attr('data-placement', 'top');

        //     if (mensaje != "") {
        //         $(elemento).attr('title', mensaje);
        //     }

        //     $('[data-toggle="tooltip"]').tooltip({
        //         position: {
        //             my: "center bottom-20",
        //             at: "center top+8",
        //             using: function (position, feedback) {
        //                 $(this).css(position);
        //                 $("<div>")
        //                     .addClass("arrow")
        //                     .addClass(feedback.vertical)
        //                     .addClass(feedback.horizontal)
        //                     .appendTo(this);
        //             }
        //         }
        //     });
        // }

        function initTablaInsumos() {
            tblInsumos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblInsumos.on('focus', 'input', function () {
                        $(this).select();
                    });

                    tblInsumos.on('click', 'td', function () {
                        let row = $(this).closest('tr');

                        if (row.hasClass('selected')) {
                            row.removeClass('selected');
                        } else {
                            tblInsumos.DataTable().$('tr.selected').removeClass('selected');
                            row.addClass('selected');
                        }
                    });

                    tblInsumos.on("click", "tbody tr", function (e) {
                        let rowData = tblInsumos.DataTable().row($(this)).data();

                        txtDescPartida.val(rowData.partidaDesc);
                    });
                },
                createdRow: function (row, rowData) {
                    $(row).find('.insumo').getAutocomplete(setInsumoDesc, { cc: selectCCSurtido.val() }, '/Enkontrol/Requisicion/getInsumos');
                    $(row).find('.insumoDesc').getAutocomplete(setInsumoBusqPorDesc, { cc: selectCCSurtido.val() }, '/Enkontrol/Requisicion/getInsumosDesc');

                    let listaSurtido = [];

                    $(rowData.listaSurtido).each(function (idx, element) {
                        listaSurtido.push({
                            almacenID: element.almacenID,
                            area_alm: element.area_alm,
                            lado_alm: element.lado_alm,
                            estante_alm: element.estante_alm,
                            nivel_alm: element.nivel_alm,
                            aSurtir: element.aSurtir
                        });
                    });

                    $(row).find('td .surtidoBoton').data('lstSurtidoPorAlmacen', listaSurtido);
                    $(row).find('td .surtidoBoton').data('totalASurtir', rowData.totalASurtir);
                    $(row).find('td .capturarSurtido').val(rowData.totalASurtir);
                    $(row).find('td .capturarSurtido').text(rowData.totalASurtir);
                },
                columns: [
                    { data: 'partida', title: 'Partida' },
                    {
                        data: 'insumo', render: function (data, type, row, meta) {
                            return `<input class="form-control insumo" ${row.validadoAlmacen ? 'readonly' : ''} value="${data}">`;
                        }, title: 'Insumo'
                    },
                    {
                        data: 'insumoDesc', render: function (data, type, row, meta) {
                            return `<input class="form-control insumoDesc" ${row.validadoAlmacen ? 'readonly' : ''} value="${data}">`;
                        }, title: 'Descripción'
                    },
                    {
                        data: 'existenciaTotal', render: function (data, type, row, meta) {
                            return `
                                <label class="existencia">${data}</label>
                                <button class="btn btn-xs btn-default existenciaBoton" tabIndex="-1"><i class="fa fa-caret-down"></i></button>`;
                        }, title: 'Existencia Total'
                    },
                    { data: 'existenciaLAB', title: 'Existencia LAB' },
                    {
                        data: 'unidad', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center unidad" readonly tabIndex="-1" value="${data}">`
                        }, title: 'Unidad'
                    },
                    {
                        data: 'cantidad', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center cantidad" readonly value="${data}">`;
                        }, title: 'Cant. Solicitada'
                    },
                    {
                        data: 'cantidadCapturada', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center cantidadCapturada" readonly value="${data}">`;
                        }, title: 'Cant. Guardada'
                    },
                    {
                        data: 'totalASurtir', render: function (data, type, row, meta) {
                            return `
                                <label class="capturarSurtido">${data}</label>
                                <button class="btn btn-xs btn-default surtidoBoton"><i class="fa fa-plus"></i></button>`;
                        }, title: 'A Surtir'
                    },
                    {
                        render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-default btn-quitar-insumo" data-quitar="false"><i class="fa fa-times"></i></button>`;
                        }, title: 'Quitar'
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '35%', targets: [2] },
                    { width: '7%', targets: [1] },
                    { width: '10%', targets: [9] }
                ]
            });
        }

        function initTablaSurtido() {
            tblSurtido.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                initComplete: function (settings, json) {
                    tblSurtido.on('focus', 'input', function () {
                        $(this).select();
                    });
                },
                createdRow: function (row, rowData) {
                    let tdAlmacen = $(row).find('td:eq(0)');

                    $(tdAlmacen).attr('data-almacenid', rowData.almacenID);
                    $(tdAlmacen).attr('data-almacendesc', rowData.almacen);
                },
                columns: [
                    { data: 'almacen', title: 'Almacén' },
                    { data: 'entradas', title: 'Entradas' },
                    { data: 'salidas', title: 'Salidas' },
                    { data: 'ultimoConsumoString', title: 'Últ. Consumo' },
                    { data: 'ultimaCompraString', title: 'Últ. Compra' },
                    { data: 'existencia', title: 'Existencias' },
                    {
                        data: 'minimo', render: function (data, type, row, meta) {
                            if (row.minimo == 'SOBRE PEDIDO') {
                                return 'SP';
                            } else if (row.minimo != 0) {
                                return formatValue(row.minimo);
                            } else {
                                return '';
                            }
                        }, title: 'Mínimo'
                    },
                    { data: 'reservados', title: 'Reservado/Tránsito' },
                    {
                        render: function (data, type, row, meta) {
                            let valor = row.existencia - row.reservados;
                            return valor >= 0 ? valor : 0;
                        }, title: 'Disponible'
                    },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    {
                        render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputSurtirAlmacen" data-existencia="${row.existencia}">`;
                        }, title: 'A Surtir'
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function setInsumoDesc(e, ui) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetInsumoInformacionSurtido', { insumo: +(ui.item.value), cc: selectCCSurtido.val(), numero_requisicion: +(inputNumeroSurtido.val()) }).always($.unblockUI).then(response => {
                if (response.success) {
                    let row = $(this).closest('tr');
                    let rowData = tblInsumos.DataTable().row(row).data();

                    rowData.insumo = response.insumoInformacion.insumo;
                    rowData.insumoDesc = response.insumoInformacion.insumoDesc;
                    rowData.unidad = response.insumoInformacion.unidad;
                    rowData.cantidadCapturada = 0;
                    rowData.totalASurtir = 0;
                    rowData.existenciaTotal = response.insumoInformacion.existenciaTotal;
                    rowData.existenciaLAB = response.insumoInformacion.existenciaLAB;

                    tblInsumos.DataTable().row(row).data(rowData).draw();
                    recargarAutoComplete(row);

                    if (ui.item.compras_req == 0) {
                        // row.find('input, select, button').attr('disabled', true);
                        row.addClass('renglonInsumoBloqueado');

                        AlertaGeneral(`Alerta`, `El insumo "${ui.item.value} - ${ui.item.id}" está bloqueado.`);
                    } else {
                        // row.find('input, select, button').attr('disabled', false);
                        row.removeClass('renglonInsumoBloqueado');
                    }

                    checkRenglonInsumoBloqueado();
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );

            // axios.get(
            //     '/Enkontrol/Requisicion/GetInsumoInformacionSurtido',
            //     { insumo: +(ui.item.value), cc: +(selectCCSurtido.val()), numero_requisicion: +(inputNumeroSurtido.val()) }
            // ).catch(error => AlertaGeneral(error.message)).then(response => {
            //     let { success, data } = response.data;

            //     if (success) {

            //     }

            //     // if (response.success) {

            //     // } else {
            //     //     AlertaGeneral(`Alerta`, response.message);
            //     // }
            // });

            return false;
        }

        function setInsumoBusqPorDesc(e, ui) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Requisicion/GetInsumoInformacionSurtido', { insumo: +(ui.item.id), cc: selectCCSurtido.val(), numero_requisicion: +(inputNumeroSurtido.val()) }).always($.unblockUI).then(response => {
                if (response.success) {
                    let row = $(this).closest('tr');
                    let rowData = tblInsumos.DataTable().row(row).data();

                    rowData.insumo = response.insumoInformacion.insumo;
                    rowData.insumoDesc = response.insumoInformacion.insumoDesc;
                    rowData.unidad = response.insumoInformacion.unidad;
                    rowData.cantidadCapturada = 0;
                    rowData.totalASurtir = 0;
                    rowData.existenciaTotal = response.insumoInformacion.existenciaTotal;
                    rowData.existenciaLAB = response.insumoInformacion.existenciaLAB;

                    tblInsumos.DataTable().row(row).data(rowData).draw();
                    recargarAutoComplete(row);

                    if (ui.item.compras_req == 0) {
                        // row.find('input, select, button').attr('disabled', true);
                        row.addClass('renglonInsumoBloqueado');

                        AlertaGeneral(`Alerta`, `El insumo "${ui.item.value} - ${ui.item.id}" está bloqueado.`);
                    } else {
                        // row.find('input, select, button').attr('disabled', false);
                        row.removeClass('renglonInsumoBloqueado');
                    }

                    checkRenglonInsumoBloqueado();
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );

            return false;
        }

        function getExistencia(insumo, cc, almacen) {
            let existencia = 0;
            $.ajax({
                url: '/Enkontrol/Requisicion/GetExistenciaInsumo',
                datatype: "json",
                type: "GET",
                async: false,
                data: {
                    insumo: insumo,
                    cc: cc,
                    almacen: almacen
                },
                success: function (response) {
                    if (response.success) {
                        existencia = response.existencia;
                    }
                }
            });

            return existencia;
        }

        function checkRenglonInsumoBloqueado() {
            let existeRenglonBloqueado = tblInsumos.find('tbody .renglonInsumoBloqueado').length > 0;

            btnGuardarSurtir.attr('disabled', existeRenglonBloqueado);
            btnValidadoCompras.attr('disabled', existeRenglonBloqueado);
            btnValidadoAlmacen.attr('disabled', existeRenglonBloqueado);
        }

        function AddNewRenglon() {
            let datos = tblInsumos.DataTable().rows().data();

            datos.push({
                'partida': datos.length + 1,
                'insumo': '',
                'insumoDesc': '',
                'existenciaTotal': 0,
                'existenciaLAB': 0,
                'unidad': '',
                'cantidad': 0,
                'cantidadCapturada': 0,
                'totalASurtir': 0,
                'validadoAlmacen': false
            });

            tblInsumos.DataTable().clear();
            tblInsumos.DataTable().rows.add(datos).draw();
        }

        function RemSelRenglon() {
            tblInsumos.DataTable().row(tblInsumos.find("tr.selected")).remove().draw();

            let cuerpo = tblInsumos.find('tbody');

            if (cuerpo.find("tr").length == 0) {
                tblInsumos.DataTable().draw();
            } else {
                tblInsumos.find('tbody tr').each(function (idx, row) {
                    let rowData = tblInsumos.DataTable().row(row).data();

                    if (rowData != undefined) {
                        rowData.partida = ++idx;

                        tblInsumos.DataTable().row(row).data(rowData).draw();

                        recargarAutoComplete(row);
                    }
                });
            }
        }

        function recargarAutoComplete(row) {
            let inputInsumo = $(row).find('.insumo');
            let inputInsumoDesc = $(row).find('.insumoDesc');

            inputInsumo.getAutocomplete(setInsumoDesc, { cc: selectCCSurtido.val() }, '/Enkontrol/Requisicion/getInsumos');
            inputInsumoDesc.getAutocomplete(setInsumoBusqPorDesc, { cc: selectCCSurtido.val() }, '/Enkontrol/Requisicion/getInsumosDesc');
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function agregarRowsCallback(tbl, lst, callback) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();

            callback();
        }

        String.prototype.parseDate = function () {
            return new Date(parseInt(this.replace('/Date(', '')));
        }

        Date.prototype.parseDate = function () {
            return this;
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
    }
    $(document).ready(() => Enkontrol.Compras.Requisicion.Surtido = new Surtido())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();