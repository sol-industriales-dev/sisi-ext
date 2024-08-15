(() => {
    $.namespace('Enkontrol.Compras.Requisicion.ConfirmarRequisiciones');
    ConfirmarRequisiciones = function () {
        //#region Selectores
        const multiSelectCC = $('#multiSelectCC');
        const tblRequisiciones = $('#tblRequisiciones');
        const btnGuardarValidaciones = $('#btnGuardarValidaciones');
        const btnVerSurtido = $('#btnVerSurtido');
        const btnConfirmarRequisicion = $('#btnConfirmarRequisicion');
        const mdlDetalleReq = $('#mdlDetalleReq');
        const selCCDetReq = $('#selCCDetReq');
        const mdlSurtido = $('#mdlSurtido');
        const tblSurtido = $('#tblSurtido');

        //#region Selectores Vista Parcial Modal Detalle
        tblInsumos = $("#tblInsumos");
        selTipoReq = $("#selTipoReq");
        selAutorizo = $("#selAutorizo");
        selLab = $("#selLab");
        selCC = $("#selCC");
        dtFecha = $("#dtFecha");
        txtNum = $("#txtNum");
        txtFolio = $('#txtFolio');
        txtSolicito = $("#txtSolicito");
        txtEmpNum = $("#txtEmpNum");
        txtEmpNom = $("#txtEmpNom");
        txtUsuNom = $("#txtUsuNom");
        txtUsuNum = $("#txtUsuNum");
        txtEstatus = $("#txtEstatus");
        txtComentarios = $("#txtComentarios");
        txtDescPartida = $("#txtDescPartida");
        txtModificacion = $("#txtModificacion");
        btnLimpiar = $("#btnLimpiar");
        btnAddRenlon = $("#btnAddRenlon");
        btnRemRenlon = $("#btnRemRenlon");
        valAddInsumo = $(".valAddInsumo");
        spanEstatus = $("#spanEstatus");
        spanActivos = $("#spanActivos");
        radioBtn = $('.radioBtn a');
        radTmc = $('.radioBtn a[data-toggle="radTmc"]');
        selectTipoFolio = $('#selectTipoFolio');
        fieldsetCaptura = $('#fieldsetCaptura');
        fieldsetInsumos = $('#fieldsetInsumos');
        txtFolioOrigen = $('#txtFolioOrigen');
        btnFoliosDisponibles = $('#btnFoliosDisponibles');
        mdlFoliosDisponibles = $('#mdlFoliosDisponibles');
        tblFoliosDisponibles = $('#tblFoliosDisponibles');
        mdlEstatusObservaciones = $('#mdlEstatusObservaciones');
        txtEstatusObservaciones = $('#txtEstatusObservaciones');
        btnGuardarObservaciones = $('#btnGuardarObservaciones');
        inputCantidadTotal = $('#inputCantidadTotal');
        mdlExistenciaDetalle = $('#mdlExistenciaDetalle');
        ulComentarios = $("#ulComentarios");
        btnAddComentario = $("#btnAddComentario");
        txtObservaciones = $("#txtObservaciones");
        checkboxConsigna = $('#checkboxConsigna');
        //#endregion

        //#endregion

        _countReq = 0;

        (function init() {
            initTableRequisiciones();
            initTableInsumos();
            initTableSurtido();

            multiSelectCC.fillCombo('/Enkontrol/OrdenCompra/FillComboCc', null, false, 'Todos');
            convertToMultiselect('#multiSelectCC');

            selCC.fillCombo('/Enkontrol/Requisicion/FillComboCcTodos', null, false, null);
            selCCDetReq.fillCombo('/Enkontrol/Requisicion/FillComboCcTodos', null, false, null);
            selLab.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
            selTipoReq.fillCombo('/Enkontrol/Requisicion/FillComboTipoReq', null, false, null);
            selectTipoFolio.fillCombo('/Enkontrol/Requisicion/FillComboTipoFolio', null, false, null);

            // btnGuardarValidaciones.click(guardarValidaciones);
            // btnConfirmarRequisicion.click(confirmarRequisicion);

            seleccionarTodosMultiselect('#multiSelectCC');
            cargarRequisiciones();
        })();

        multiSelectCC.on('change', function () {
            cargarRequisiciones();
        });

        btnVerSurtido.on('click', function () {
            let cc = selCCDetReq.val();
            let numero = txtNum.val();

            if (cc != '' && (!isNaN(numero) && numero > 0)) {
                $.post('/Enkontrol/Requisicion/GetSurtidoDetalle', { cc, numero }).then(response => {
                    if (response.success) {
                        AddRows(tblSurtido, response.data);
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );

                mdlSurtido.modal('show');
            }
        });

        function initTableRequisiciones() {
            tblRequisiciones.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tblRequisiciones.on('click', 'input[type="checkbox"]', function () {
                        if ($(this).prop('checked')) {
                            tblRequisiciones.DataTable().row($(this).closest('tr')).data().checkbox = true;

                            _countReq++;
                        } else {
                            tblRequisiciones.DataTable().row($(this).closest('tr')).data().checkbox = false;

                            _countReq--;
                        }

                        if (_countReq > 0) {
                            btnGuardarValidaciones.removeClass("disabled");
                        } else {
                            btnGuardarValidaciones.addClass("disabled");
                        }
                    });

                    tblRequisiciones.on('click', '.btn-detalle-req', function () {
                        let rowData = tblRequisiciones.DataTable().row($(this).closest('tr')).data();
                        let cc = rowData.cc;
                        let numero = rowData.numero;

                        if (cc != null) {
                            selCCDetReq.val(cc);
                            getRequisicionDetalle(cc, numero);
                        }
                    });
                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'ccDesc', title: 'Centro de Costos' },
                    { data: 'numero', title: 'Número' },
                    { data: 'fecha', title: 'Fecha' },
                    {
                        title: 'Detalle Requisición', render: function (data, type, row, meta) {
                            let btnDetalleReq = document.createElement('btn');
                            let spanBtn = document.createElement('span');

                            btnDetalleReq.classList.add('btn');
                            btnDetalleReq.classList.add('btn-sm');
                            btnDetalleReq.classList.add('btn-default');
                            btnDetalleReq.classList.add('btn-detalle-req');

                            spanBtn.classList.add('fas');
                            spanBtn.classList.add('fa-eye');

                            $(btnDetalleReq).append(spanBtn);

                            return btnDetalleReq.outerHTML;
                        }
                    }
                    // {
                    //     title: 'Confirmar', render: function (data, type, row, meta) {
                    //         let div = document.createElement('div');
                    //         let checkbox = document.createElement('input');
                    //         let label = document.createElement('label');

                    //         checkbox.id = 'checkboxConfirmar_' + meta.row;
                    //         checkbox.setAttribute('type', 'checkbox');
                    //         checkbox.classList.add('form-control');
                    //         checkbox.classList.add('regular-checkbox');
                    //         checkbox.classList.add('checkBoxConfirmar');
                    //         checkbox.style.height = '25px';

                    //         label.setAttribute('for', checkbox.id);

                    //         $(div).append(checkbox);
                    //         $(div).append(label);

                    //         return div.outerHTML;
                    //     }
                    // }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '40%', targets: 0 },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [2]
                    }
                ]
            });
        }

        function initTableInsumos() {
            tblInsumos.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'partida', title: 'Part.' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    {
                        title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            return row.area + '-' + row.cuenta;
                        }
                    },
                    { data: 'referencia_1', title: 'Referencia' },
                    { data: 'cantidad', title: 'Cantidad' },
                    // {
                    //     title: 'Cant. Confirmada', render: function (data, type, row, meta) {
                    //         let html = `<input class="form-control text-center inputCantidadConfirmada" value="${row.cantidad}" style="height: 25px;">`;

                    //         return html;
                    //     }
                    // },
                    { data: 'observaciones', title: 'Observaciones' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '30%', targets: [2, 6] }
                ]
            });
        }

        function initTableSurtido() {
            tblSurtido.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'insumoDesc', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'almacenOrigenDesc', title: 'Almacén Origen' },
                    { data: 'tipoSurtido', title: 'Tipo Surtido' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function getRequisicionesPorUsuarioProcesadas() {
            let listCC = getValoresMultiples('#multiSelectCC');

            if (listCC.length > 0) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Requisicion/GetRequisicionesPorUsuarioProcesadas', { listCC })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            AddRows(tblRequisiciones, response.data);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información.`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                AlertaGeneral(`Alerta`, `Seleccione un Centro de Costos.`);
            }
        }

        function guardarValidaciones() {
            let validaciones = tblRequisiciones.DataTable().rows().data().filter(x => x.checkbox);
            let numeros = [];

            validaciones.each(function (item) {
                numeros.push(item.numero);
            });

            if (validaciones.length > 0) {
                $.post('/Enkontrol/Requisicion/ValidacionesRequisitor', { cc: multiSelectCC.val(), numeros: numeros }).then(response => {
                    if (response.success) {
                        AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        multiSelectCC.change();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al guardar la información.`);
                        multiSelectCC.change();
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            }
        }

        function getRequisicionDetalle(cc, num) {
            $.blockUI({ message: 'Procesando...' });
            $.post('/Enkontrol/Requisicion/getReq', { cc, num })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        if (!response.requisicionNueva) {
                            // tblInsumos.find('tbody').empty();
                            // setPartidas(response.partidas);

                            tblInsumos.DataTable().clear().draw();
                            AddRows(tblInsumos, response.partidas);

                            setRequisicion(response.req);
                            txtDescPartida.val(response.partidas[0].partidaDesc);

                            mdlDetalleReq.modal('show');
                        }
                    } else {
                        AlertaGeneral(`Alerta`, `Error al guardar la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function setPartidas(partidas) {
            $.ajaxSetup({ async: false });

            for (let i = 0; i < partidas.length; i++) {
                _renglonNuevo(partidas[i].partida, false, partidas[i].cancelado == 'A' ? false : true).done(function (_renglon) {
                    let $renglon = setInsumo(initRenglonInsumo($(_renglon)), partidas[i]);

                    agregarTooltip($renglon.find('.btn-estatus-activo'), 'ACTIVO');
                    agregarTooltip($renglon.find('.btn-estatus-inactivo'), 'INACTIVO');

                    tblInsumos.find(`tbody`).append($renglon);

                    $renglon.find('.existencia').closest('td').remove();

                    agregarTooltip($renglon.find('.existenciaBoton'), 'Desglose por Almacén');

                    $renglon.data({
                        insumo: partidas[i].insumo
                    });

                    $renglon.find('.selectAlmacen').fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
                });
            }

            $.ajaxSetup({ async: true });

            getCantidadTotal();
        }

        function setRequisicion(req) {
            selCC.val(req.cc);
            txtNum.val(req.numero != '-1' ? req.numero : '');
            dtFecha.val(req.fecha.parseDate().toLocaleDateString());
            selLab.val(req.libre_abordo);

            if (selTipoReq.find('option[value="' + req.tipo_req_oc + '"]').length) {
                selTipoReq.val(req.tipo_req_oc)
            } else {
                AlertaGeneral('Alerta', 'No se encontró el tipo de requisición.');
                selTipoReq.val('1');
            }

            txtFolioOrigen.val(req.folioOrigen);

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
            txtNum.data({
                id: req.id,
                solicito: req.solicito,
                st_estatus: req.st_estatus,
                st_impresa: req.st_impresa,
                st_autoriza: req.st_autoriza,
                autoriza_activos: req.autoriza_activos,
                num_vobo: req.num_vobo,
                autoriza: req.fecha_autoriza == null ? "" : req.fecha_autoriza.parseDate()
            });
            req.autoriza_activos == 1 ? spanActivos.removeClass("hidden") : spanActivos.addClass("hidden");
            $('#checkboxConsigna').attr('checked', req.consigna == true ? true : false);
        }

        function _renglonNuevo(partida, nuevo, cancelado) {
            return $.post('/Enkontrol/Requisicion/_renglonNuevo', { partida, nuevo, cancelado });
        }

        function setInsumo(row, p) {
            row.find('.insumo').val(p.insumo).change();
            row.find('.insumoDesc').val(p.insumoDesc);
            if (p.area == 0)
                row.find('.areaCuenta').val("000-000");
            else
                row.find(`.areaCuenta option[value="${p.area}"][data-prefijo="${p.cuenta}"]`).prop("selected", true).val(p.area);
            row.find('.referencia').val(p.referencia_1);
            row.find('.cantidad').val(p.cantidad).change();
            row.find('.unidad').text(p.unidad);
            row.find('.porComprar').val(p.cant_ordenada).change();
            row.find('.exceso').val(p.cantidad_excedida_ppto).change();
            row.find('.btn-estatus').attr('data-observaciones', p.observaciones != null ? p.observaciones : '');
            row.find('.observaciones').val(p.observaciones);
            row.find('.cantidadCapturada').val(p.cantidadCapturada)
            row.data({
                id: p.id,
                idReq: p.idReq,
                partida: p.partida,
                DescPartida: p.partidaDesc,
                exceso: 0,
                isAreaCueta: p.isAreaCueta
            });
            row = setRowRadioValue(row, `radCancel${p.partida}`, p.cant_cancelada != 0);
            return row;
        }

        function setRowRadioValue(row, tog, sel) {
            row.find(`#${tog}`).prop('value', sel);
            row.find(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            row.find(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
            return row;
        }

        function initRenglonInsumo(row, partida) {
            row.find('.insumo').getAutocomplete(setInsumoDesc, { cc: selCC.val() }, '/Enkontrol/Requisicion/getInsumos');
            row.find('.insumoDesc').getAutocomplete(setInsumoBusqPorDesc, { cc: selCC.val() }, '/Enkontrol/Requisicion/getInsumosDesc');

            row.find('.areaCuenta').fillCombo('/Enkontrol/Requisicion/FillComboAreaCuenta', { cc: selCC.val() }, false, "000-000");
            row.find(".cantidad").val(0).commasFormat().change();
            row.find(".porComprar").val(0).commasFormat().change();
            row.find(".exceso").val(0).commasFormat().change();

            agregarTooltip(row.find('.btn-estatus-activo'), 'ACTIVO');
            agregarTooltip(row.find('.btn-estatus-inactivo'), 'INACTIVO');

            row.data({
                id: 0,
                idReq: 0,
                partida: partida,
                DescPartida: "",
                exceso: 0,
                isAreaCueta: false
            });

            return row;
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

        function setInsumoDesc(e, ui) {
            let exceso = ui.item.exceso,
                isAreaCueta = ui.item.isAreaCueta,
                row = $(this).closest('tr'),
                valor = row.find(".cantidad").val();
            row.find('.insumoDesc').val(ui.item.id);
            row.find('.unidad').text(ui.item.unidad);
            row.find(".porComprar").val(valor).change();
            row.find('.exceso').val(valor + exceso).change();
            row.find('.existencia').text(getExistencia(ui.item.value, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
            row.find('.existenciaBoton').removeClass('hidden');
            agregarTooltip(row.find('.existenciaBoton'), 'Desglose por Almacén');
            row.data({
                exceso: exceso,
                isAreaCueta: isAreaCueta,
                insumo: ui.item.value
            });

            if (isAreaCueta)
                if (row.find(".areaCuenta").val() == "000-000")
                    row.find(".areaCuenta").val(row.find(".areaCuenta option:eq(1)").val());

            if (ui.item.cancelado == 'A') {
                row.find('.btn-estatus-activo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').css('display', 'none');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            } else {
                row.find('.btn-estatus-activo').css('display', 'none');
                row.find('.btn-estatus-inactivo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            }


        }

        function setInsumoBusqPorDesc(e, ui) {
            let exceso = ui.item.exceso,
                isAreaCueta = ui.item.isAreaCueta,
                row = $(this).closest('tr'),
                valor = row.find(".cantidad").val();
            row.find('.insumo').val(ui.item.id);
            row.find('.insumoDesc').val(ui.item.value);
            row.find('.unidad').text(ui.item.unidad);
            row.find('.existencia').text(getExistencia(ui.item.id, 400, $("#selLab").val() != '' ? $("#selLab").val() : 0));
            row.find('.existenciaBoton').removeClass('hidden');
            agregarTooltip(row.find('.existenciaBoton'), 'Desglose por Almacén');
            row.data({
                exceso: exceso,
                isAreaCueta: isAreaCueta,
                insumo: ui.item.id
            });

            row.find(".porComprar").val(valor).change();
            row.find('.exceso').val(valor + exceso).change();

            if (isAreaCueta)
                if (row.find(".areaCuenta").val() == "000-000")
                    row.find(".areaCuenta").val(row.find(".areaCuenta option:eq(1)").val());

            if (ui.item.cancelado == 'A') {
                row.find('.btn-estatus-activo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').css('display', 'none');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            } else {
                row.find('.btn-estatus-activo').css('display', 'none');
                row.find('.btn-estatus-inactivo').css('display', 'inline-block');
                row.find('.btn-estatus-inactivo').attr('data-observaciones', '');
            }
        }

        function getCantidadTotal() {
            let cantidadTotal = 0;
            let inputs = tblInsumos.find('tbody tr .cantidad');

            inputs.each(function (index, elemento) {
                cantidadTotal += parseFloat(elemento.value)
            });

            inputCantidadTotal.val(cantidadTotal).commasFormat();
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function confirmarRequisicion() {
            let cc = selCCDetReq.val();
            let numero = txtNum.val();

            if ((cc != '' && !isNaN(cc)) && (numero != '' && !isNaN(numero))) {
                let partidas = [];
                let flagCantidadMayor = false;

                tblInsumos.find('tbody tr').each(function (index, row) {
                    let rowData = tblInsumos.DataTable().row(row).data();
                    let cantidadConfirmada = $(row).find('.inputCantidadConfirmada').val();

                    if (cantidadConfirmada > rowData.cantidad) {
                        flagCantidadMayor = true;
                    }

                    rowData.cantidadConfirmada = cantidadConfirmada;

                    partidas.push(rowData);
                });

                let requisicion = {
                    cc: cc,
                    numero: numero,
                    partidas: partidas
                }

                if (!flagCantidadMayor) {
                    $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                    $.post('/Enkontrol/Requisicion/ConfirmarRequisicion', { requisicion })
                        .always($.unblockUI)
                        .then(response => {
                            if (response.success) {
                                AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                                mdlDetalleReq.modal('hide');
                                multiSelectCC.change();
                            } else {
                                AlertaGeneral(`Alerta`, `Error al guardar la información`);
                            }
                        }, error => {
                            AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                        }
                        );
                } else {
                    AlertaGeneral(`Alerta`, `La cantidad confirmada es mayor a la cantidad establecida.`);
                }
            } else {
                AlertaGeneral(`Alerta`, `Centro de Costo/Número inválido.`);
            }
        }

        function cargarRequisiciones() {
            let listCC = getValoresMultiples('#multiSelectCC');

            if (listCC.length > 0) {
                tblRequisiciones.DataTable().clear().draw();
                getRequisicionesPorUsuarioProcesadas();
            } else {
                tblRequisiciones.DataTable().clear().draw();
            }
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
        String.prototype.parseDate = function () {
            return new Date(parseInt(this.replace('/Date(', '')));
        }
        Date.prototype.parseDate = function () {
            return this;
        }
    }
    $(document).ready(() => Enkontrol.Compras.Requisicion.ConfirmarRequisiciones = new ConfirmarRequisiciones())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();