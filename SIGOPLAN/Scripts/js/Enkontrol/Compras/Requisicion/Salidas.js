(function () {
    $.namespace('Enkontrol.Compras.Requisicion.Salidas');
    Salidas = function () {
        //#region Selectores
        const tblTraspasosPendientes = $('#tblTraspasosPendientes');
        const selectCCOrigen = $('#selectCCOrigen');
        const selectAlmacenOrigen = $('#selectAlmacenOrigen');
        const selectCCDestino = $('#selectCCDestino');
        const selectAlmacenDestino = $('#selectAlmacenDestino');
        const btnGuardarSalidas = $('#btnGuardarSalidas');
        const report = $("#report");
        const mdlUbicacionDetalle = $('#mdlUbicacionDetalle');
        const tblUbicacion = $('#tblUbicacion');
        const btnGuardarUbicacion = $('#btnGuardarUbicacion');
        const inputNumeroRequisicion = $('#inputNumeroRequisicion');
        const btnBuscar = $('#btnBuscar');
        const btnGuardar = $('#btnGuardar');
        const btnModalReporte = $('#btnModalReporte');
        const mdlVerReporte = $('#mdlVerReporte');
        const selectAlmacenReporte = $('#selectAlmacenReporte');
        const inputNumeroReporte = $('#inputNumeroReporte');
        const btnImprimible = $('#btnImprimible');
        //#endregion

        _countRenglonesSalidas = 0;

        function init() {
            initForm();
            initTblTraspasosPendientes();
            initTableUbicacion();

            btnBuscar.click(cargarTraspasosOrigen);
            btnGuardar.click(guardarTraspasosOrigen);
            btnModalReporte.click(function () { mdlVerReporte.modal('show'); });
            btnImprimible.click(verImprimible);
        }

        _filaInsumo = null;

        const getSalidas = (almacenOrigenID, almacenDestinoID) => { return $.post('/Enkontrol/Requisicion/GetSalidas', { almacenOrigenID, almacenDestinoID }) };
        const guardarSalidas = (salidas) => $.post('/Enkontrol/Requisicion/GuardarSalidas', { salidas });

        // $('#fieldsetOrigen').on('change', '#selectAlmacenOrigen, #selectAlmacenDestino', function () {
        //     let almacenOrigenID = selectAlmacenOrigen.val() != '' ? selectAlmacenOrigen.val() : 0;
        //     let almacenDestinoID = selectAlmacenDestino.val() != '' ? selectAlmacenDestino.val() : 0

        //     getSalidas(almacenOrigenID, almacenDestinoID).done(function (response) {
        //         if (response.success) {
        //             AddRows(tblSalidas, response.data);
        //         }
        //     });
        // });

        btnGuardarSalidas.on('click', function () {
            let lstSalidas = [];
            let totalCantidad = 0;

            tblSalidas.find("tbody tr").each(function (idx, row) {
                let rowData = tblSalidas.DataTable().row(row).data();

                let cantidadAutorizar = $(row).find('.inputCantidadAutorizar').val();
                let checkBoxRech = $(row).find('.checkBoxRech').prop('checked');
                let almacenDestinoID = $(row).find('.selectAlmacenDestinoNuevo').val();

                if ((!isNaN(cantidadAutorizar) && cantidadAutorizar != '' && cantidadAutorizar > 0) || checkBoxRech) {
                    rowData.ordenTraspaso = 0;
                    rowData.comentarios = $(row).find('td .inputComentarios').val();
                    rowData.transporte = $(row).find('td inputTransporte').val();
                    rowData.numeroDestino = 0;
                    rowData.cantidadAutorizar = cantidadAutorizar;
                    rowData.checkBoxRechazado = checkBoxRech;
                    rowData.listUbicacionMovimiento = $(row).find('.btnUbicacionDetalle').data('listUbicacionMovimiento');
                    rowData.almacenDestinoID = almacenDestinoID;

                    lstSalidas.push(rowData);
                }

                if (cantidadAutorizar > 0) {
                    totalCantidad += cantidadAutorizar;
                }
            });

            if (lstSalidas.length > 0) {
                btnGuardarSalidas.attr('disabled', true);

                guardarSalidas(lstSalidas).done(function (response) {
                    if (response.success) {
                        btnGuardarSalidas.attr('disabled', false);
                        selectAlmacenOrigen.change();

                        if (response.flagMaquinaStandBy) {
                            AlertaGeneral(`Alerta`, `Información guardada. Se quitó el estado "Stand-By" de la máquina.`);
                        }

                        if (totalCantidad > 0) {
                            // verReporte();
                        }
                    } else {
                        AlertaGeneral('Aviso', `Ocurrió un error al intentar guardar las salidas. ${response.message.length > 0 ? response.message : ``}`);
                    }
                });
            }
        });

        btnGuardarUbicacion.on('click', function () {
            let flagSobrepasoExistencia = false;
            let listUbicacionMovimiento = [];
            let totalUbicacion = 0;

            tblUbicacion.find("tbody tr").each(function (idx, row) {
                let rowData = tblUbicacion.DataTable().row(row).data();
                let inputCantidadSalida = $(row).find('.inputCantidadSalida');
                let cantidadSalida = inputCantidadSalida.val() != '' ? parseFloat(inputCantidadSalida.val()) : 0;

                if (cantidadSalida > rowData.cantidad && rowData.cantidad >= 0) {
                    flagSobrepasoExistencia = true;
                    $(row).find('.inputCantidadSalida').addClass('campoInvalido');
                } else {
                    $(row).find('.inputCantidadSalida').removeClass('campoInvalido');
                }

                if (cantidadSalida < 0) {
                    AlertaGeneral('Alerta', 'No puede introducir números negativos.');
                }

                if (cantidadSalida > 0) {
                    totalUbicacion += cantidadSalida;

                    listUbicacionMovimiento.push({
                        insumo: rowData.insumo,
                        cantidad: rowData.cantidad,
                        area_alm: rowData.area_alm,
                        lado_alm: rowData.lado_alm,
                        estante_alm: rowData.estante_alm,
                        nivel_alm: rowData.nivel_alm,
                        cantidadMovimiento: cantidadSalida
                    });
                }
            });

            if (flagSobrepasoExistencia) {
                AlertaGeneral('Alerta', 'No puede surtir más de las existencias por almacén.');
            } else {
                _filaInsumo.find('td .btnUbicacionDetalle').data('listUbicacionMovimiento', listUbicacionMovimiento);
                _filaInsumo.find('td .btnUbicacionDetalle').data('totalUbicacion', totalUbicacion);
                _filaInsumo.find('td .inputCantidadAutorizar').val(totalUbicacion);

                _filaInsumo.find('td input').change();

                mdlUbicacionDetalle.modal('hide');
            }
        });

        function verReporte() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=128`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
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
        function initForm() {
            selectCCOrigen.fillCombo('/Enkontrol/Almacen/FillComboCC', null, false, 'Todos');
            selectAlmacenOrigen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, 'Todos');
            selectCCDestino.fillCombo('/Enkontrol/Almacen/FillComboCC', null, false, 'Todos');
            selectAlmacenDestino.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, 'Todos');
            // selectAlmacenOrigen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirAcceso', null, false, null);
            // selectAlmacenDestino.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
            selectAlmacenReporte.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, null);
        }

        // function initTableSalidas() {
        //     tblSalidas.DataTable({
        //         retrieve: true,
        //         paging: false,
        //         deferRender: true,
        //         searching: false,
        //         language: dtDicEsp,
        //         rowId: 'id',
        //         bInfo: false,
        //         initComplete: function (settings, json) {
        //             tblSalidas.on('click', 'input[type="checkbox"]', function () {
        //                 let row = $(this).closest('tr');
        //                 let rowData = tblSalidas.DataTable().row(row).data();

        //                 if ($(this).prop('checked')) {
        //                     tblSalidas.DataTable().row($(this).closest('tr')).data().checkbox = true;

        //                     _countRenglonesSalidas++;
        //                 } else {
        //                     tblSalidas.DataTable().row($(this).closest('tr')).data().checkbox = false;

        //                     _countRenglonesSalidas--;
        //                 }

        //                 if ($(this).hasClass('checkBoxRech')) {
        //                     if ($(this).prop('checked')) {
        //                         rowData.checkboxRechazado = true;

        //                         row.find('.btnCantidadTotalAutorizar').attr('disabled', true);
        //                         row.find('.btnUbicacionDetalle').attr('disabled', true);
        //                         row.find('.inputCantidadAutorizar').val('');
        //                     } else {
        //                         rowData.checkboxRechazado = false;

        //                         row.find('.btnCantidadTotalAutorizar').attr('disabled', false);
        //                         row.find('.btnUbicacionDetalle').attr('disabled', false);
        //                         row.find('.inputCantidadAutorizar').val('');
        //                     }
        //                 }
        //             });

        //             tblSalidas.on('click', '.btnCantidadTotalAutorizar', function () {
        //                 let rowData = tblSalidas.DataTable().row($(this).closest('tr')).data();
        //                 let $row = $(this).closest('tr');

        //                 $row.find('.inputCantidadAutorizar').val(rowData.cantidad);
        //                 $row.find('.inputCantidadAutorizar').change();
        //             });

        //             tblSalidas.on('click', '.btnUbicacionDetalle', function () {
        //                 let row = $(this).closest('tr');
        //                 let rowData = tblSalidas.DataTable().row(row).data();

        //                 _filaInsumo = row;

        //                 let cc = rowData.cc;
        //                 let almacenID = rowData.almacenOrigenID;
        //                 let insumo = rowData.insumo;

        //                 $.blockUI({ message: 'Procesando...', baseZ: 2000 });
        //                 $.post('/Enkontrol/Requisicion/GetUbicacionDetalle', { cc, almacenID, insumo })
        //                     .always($.unblockUI)
        //                     .then(response => {
        //                         if (response.success) {
        //                             if (response.data != null) {
        //                                 AddRows(tblUbicacion, response.data);

        //                                 if ($(this).data('listUbicacionMovimiento') != undefined && $(this).data('listUbicacionMovimiento') != null) {
        //                                     let listUbicacionMovimiento = $(this).data('listUbicacionMovimiento');
        //                                     let tablaData = tblUbicacion.DataTable().rows().data();

        //                                     listUbicacionMovimiento.forEach(item => {
        //                                         let renglonData = tablaData.toArray().find(x => {
        //                                             return x.area_alm == item.area_alm &&
        //                                                 x.lado_alm == item.lado_alm &&
        //                                                 x.estante_alm == item.estante_alm &&
        //                                                 x.nivel_alm == item.nivel_alm
        //                                         });

        //                                         if (renglonData != undefined) {
        //                                             renglonData.cantidadMovimiento = item.cantidadMovimiento;
        //                                         }
        //                                     });

        //                                     tblUbicacion.DataTable().clear();
        //                                     tblUbicacion.DataTable().rows.add(tablaData).draw();
        //                                 }

        //                                 mdlUbicacionDetalle.modal('show');
        //                             }
        //                         } else {
        //                             AlertaGeneral(`Alerta`, `Error al recuperar la información.`);
        //                         }
        //                     }, error => {
        //                         AlertaGeneral(
        //                             `Operación fallida`,
        //                             `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`
        //                         );
        //                     }
        //                     );
        //             });
        //         },
        //         createdRow: function (row, rowData) {
        //             $(row).find('.selectAlmacenDestinoNuevo').fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
        //             $(row).find('.selectAlmacenDestinoNuevo').val(rowData.almacenDestinoID);
        //         },
        //         columns: [
        //             { data: 'ccDesc', title: 'Centro de Costo' },
        //             {
        //                 data: 'numero', title: 'Núm. Req.', render: function (data, type, row, meta) {
        //                     if (data != 0) {
        //                         return data;
        //                     } else {
        //                         return '';
        //                     }
        //                 }
        //             },
        //             { data: 'fecha', title: 'Fecha' },
        //             {
        //                 render: function (data, type, row, meta) {
        //                     return row.insumo + ' - ' + row.insumoDesc;
        //                 }, title: 'Insumo'
        //             },
        //             { data: 'cantidad', title: 'Cantidad' },
        //             { data: 'almacenOrigenDesc', title: 'Alm. Origen' },
        //             {
        //                 data: 'almacenDestinoDesc', title: 'Alm. Destino', render: function (data, type, row, meta) {
        //                     let select = document.createElement('select');

        //                     select.classList.add('form-control');
        //                     select.classList.add('selectAlmacenDestinoNuevo');
        //                     select.style.height = '22px';

        //                     return select.outerHTML;
        //                 }
        //             },
        //             // {
        //             //     sortable: false,
        //             //     render: (data, type, row, meta) => {
        //             //         let input = document.createElement('input');

        //             //         input.classList.add('form-control');
        //             //         input.classList.add('inputOrdenTraspaso');
        //             //         input.style.height = '22px';

        //             //         return input.outerHTML;
        //             //     },
        //             //     title: 'Orden Traspaso'
        //             // },
        //             {
        //                 sortable: false,
        //                 render: (data, type, row, meta) => {
        //                     let input = document.createElement('input');

        //                     input.classList.add('form-control');
        //                     input.classList.add('inputTransporte');
        //                     input.style.height = '22px';

        //                     return input.outerHTML;
        //                 },
        //                 title: 'Transporte'
        //             },
        //             {
        //                 sortable: false,
        //                 render: (data, type, row, meta) => {
        //                     let valor = data != '' && data != undefined ? data : 0;

        //                     return `<div class="input-group">
        //                                 <span class="input-group-btn">
        //                                     <button class="btn btn-xs btn-default btnUbicacionDetalle" ${row.checkboxRechazado ? 'disabled' : ''}>
        //                                         <i class="fa fa-arrow-right"></i>
        //                                     </button>
        //                                 </span>
        //                                 <input type="text" class="form-control text-center inputCantidadAutorizar" disabled value="${valor}" style="height: 22px;">
        //                             </div>`;
        //                 },
        //                 title: 'Autorizar'
        //             },
        //             {
        //                 sortable: false,
        //                 render: (data, type, row, meta) => {
        //                     let input = document.createElement('input');

        //                     input.classList.add('form-control');
        //                     input.classList.add('inputComentarios');
        //                     input.style.height = '22px';

        //                     return input.outerHTML;
        //                 },
        //                 title: 'Comentarios'
        //             },
        //             {
        //                 sortable: false,
        //                 render: (data, type, row, meta) => {
        //                     let div = document.createElement('div');
        //                     let checkbox = document.createElement('input');
        //                     let label = document.createElement('label');

        //                     checkbox.id = 'checkboxRech_' + meta.row;
        //                     checkbox.setAttribute('type', 'checkbox');
        //                     checkbox.classList.add('form-control');
        //                     checkbox.classList.add('regular-checkboxdanger');
        //                     checkbox.classList.add('checkBoxRech');
        //                     checkbox.style.height = '25px';

        //                     label.setAttribute('for', checkbox.id);

        //                     $(div).append(checkbox);
        //                     $(div).append(label);

        //                     return div.outerHTML;
        //                 },
        //                 title: 'Rechazar'
        //             }
        //         ],
        //         columnDefs: [
        //             {
        //                 render: function (data, type, row) {
        //                     if (data == null) {
        //                         return '';
        //                     } else {
        //                         return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
        //                     }
        //                 },
        //                 targets: [2]
        //             },
        //             { className: "dt-center", "targets": "_all" },
        //             { width: '20%', targets: [0, 3, 8] }
        //         ]
        //     });
        // }

        // function initTableUbicacion() {
        //     tblUbicacion.DataTable({
        //         retrieve: true,
        //         paging: false,
        //         deferRender: true,
        //         searching: false,
        //         language: dtDicEsp,
        //         bInfo: false,
        //         initComplete: function (settings, json) {
        //             tblUbicacion.on('click', '.btnCantidadTotalSalida', function () {
        //                 let rowData = tblUbicacion.DataTable().row($(this).closest('tr')).data();
        //                 let $row = $(this).closest('tr');

        //                 if (_filaInsumo != null) {
        //                     let cantidadSalida = tblSalidas.DataTable().row(_filaInsumo).data().cantidad;
        //                     let cantidadAnteriorUbicacion = $row.find('.inputCantidadSalida').val();

        //                     let sumatoriaUbicacion = 0;

        //                     tblUbicacion.find('tbody tr').each(function (idx, row) {
        //                         let cantidadUbicacion = $(row).find('.inputCantidadSalida').val();

        //                         sumatoriaUbicacion += cantidadUbicacion;
        //                     });

        //                     sumatoriaUbicacion -= cantidadAnteriorUbicacion;

        //                     let cantidadPendiente = cantidadSalida - sumatoriaUbicacion;

        //                     if (cantidadPendiente <= rowData.cantidad) {
        //                         $row.find('.inputCantidadSalida').val(cantidadPendiente);
        //                         $row.find('.inputCantidadSalida').change();
        //                     } else {
        //                         $row.find('.inputCantidadSalida').val(rowData.cantidad);
        //                         $row.find('.inputCantidadSalida').change();
        //                     }
        //                 } else {
        //                     $row.find('.inputCantidadSalida').val(rowData.cantidad);
        //                     $row.find('.inputCantidadSalida').change();
        //                 }
        //             });
        //         },
        //         columns: [
        //             { data: 'insumoDesc', title: 'Insumo' },
        //             { data: 'cantidad', title: 'Cantidad' },
        //             { data: 'area_alm', title: 'Área' },
        //             { data: 'lado_alm', title: 'Lado' },
        //             { data: 'estante_alm', title: 'Estante' },
        //             { data: 'nivel_alm', title: 'Nivel' },
        //             {
        //                 sortable: false,
        //                 data: 'cantidadMovimiento',
        //                 render: (data, type, row, meta) => {
        //                     let valor = data != undefined ? data : '';

        //                     return `<div class="input-group">
        //                                 <span class="input-group-btn">
        //                                     <button class="btn btn-xs btn-default btnCantidadTotalSalida" type="button">
        //                                         <i class="fa fa-arrow-right"></i>
        //                                     </button>
        //                                 </span>
        //                                 <input type="text" class="form-control text-center inputCantidadSalida" value="${valor}" style="height: 22px;">
        //                             </div>`;
        //                 },
        //                 title: 'Salida'
        //             }
        //         ],
        //         columnDefs: [
        //             { className: "dt-center", "targets": "_all" },
        //             { width: '50%', targets: [0] }
        //         ]
        //     });
        // }

        function initTblTraspasosPendientes() {
            tblTraspasosPendientes.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                scrollCollapse: true,
                bInfo: false,
                searching: false,
                initComplete: function (settings, json) {
                    tblTraspasosPendientes.on('click', 'td', function () {
                        let rowData = tblTraspasosPendientes.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');
                        let selected = $row.hasClass("active");

                        tblTraspasosPendientes.find("tr").removeClass("active");

                        if (!selected) {
                            $row.not("th").addClass("active");

                            // textAreaComentarios.val(rowData.comentarios);
                        } else {
                            // textAreaComentarios.val('');
                        }
                    });

                    tblTraspasosPendientes.on('click', '.btnCantidadTotalAutorizar', function () {
                        let rowData = tblTraspasosPendientes.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');

                        $row.find('.inputCantidadAutorizar').val(rowData.cantidad);
                        $row.find('.inputCantidadAutorizar').change();
                    });

                    tblTraspasosPendientes.on('click', '.btnUbicacionDetalle', function () {
                        let row = $(this).closest('tr');
                        let rowData = tblTraspasosPendientes.DataTable().row(row).data();

                        _filaInsumo = row;

                        let cc = rowData.ccOrigen;
                        let almacenID = rowData.almacenOrigen;
                        let insumo = rowData.insumo;

                        $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                        $.post('/Enkontrol/Requisicion/GetUbicacionDetalle', { cc, almacenID, insumo })
                            .always($.unblockUI)
                            .then(response => {
                                if (response.success) {
                                    if (response.data != null) {
                                        let ubicacionesFiltradas = response.data.filter(x => x.cantidad > 0);

                                        AddRows(tblUbicacion, ubicacionesFiltradas);

                                        if ($(this).data('listUbicacionMovimiento') != undefined && $(this).data('listUbicacionMovimiento') != null) {
                                            let listUbicacionMovimiento = $(this).data('listUbicacionMovimiento');
                                            let tablaData = tblUbicacion.DataTable().rows().data();

                                            listUbicacionMovimiento.forEach(item => {
                                                let renglonData = tablaData.toArray().find(x => {
                                                    return x.area_alm == item.area_alm &&
                                                        x.lado_alm == item.lado_alm &&
                                                        x.estante_alm == item.estante_alm &&
                                                        x.nivel_alm == item.nivel_alm
                                                });

                                                if (renglonData != undefined) {
                                                    renglonData.cantidadMovimiento = item.cantidadMovimiento;
                                                }
                                            });

                                            tblUbicacion.DataTable().clear();
                                            tblUbicacion.DataTable().rows.add(tablaData).draw();
                                        }

                                        mdlUbicacionDetalle.modal('show');
                                    }
                                } else {
                                    AlertaGeneral(`Alerta`, `Error al recuperar la información.`);
                                }
                            }, error => {
                                AlertaGeneral(
                                    `Operación fallida`,
                                    `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`
                                );
                            }
                            );
                    });
                },
                columns: [
                    { data: 'numeroRequisicion', title: 'Número Requisición' },
                    { data: 'insumoDesc', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'ccDesc', title: 'Centro Costo' },
                    { data: 'almacenOrigenDesc', title: 'Almacén Origen' },
                    { data: 'almacenDestinoDesc', title: 'Almacén Destino' },
                    { data: 'fecha', title: 'Fecha' },
                    {
                        sortable: false, data: 'comentariosGestion',
                        render: (data, type, row, meta) => {
                            let valor = data != undefined ? data : '';

                            return `<input type="text" class="form-control text-left inputComentariosGestion" value="${valor}" style="height: 22px;">`;
                        },
                        title: 'Comentarios'
                    },
                    {
                        data: 'cantidadTraspasar', title: 'Autorizar', render: function (data, type, row, meta) {
                            let valor = data != '' && data != undefined ? data : 0;

                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnUbicacionDetalle" ${row.checkboxRechazado ? 'disabled' : ''}>
                                                <i class="fa fa-eye"></i>
                                            </button>
                                        </span>
                                        <input type="text" class="form-control text-center inputCantidadAutorizar" disabled value="${valor}" style="height: 22px;">
                                    </div>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    {
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return $.datepicker.formatDate('dd/mm/yy', new Date(parseInt(data.substr(6))));
                            }
                        },
                        targets: [6]
                    },
                    { width: '15%', targets: [1, 3, 4, 5, 6, 8] }
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
                        let rowData = tblTraspasosPendientes.DataTable().row($(this).closest('tr')).data();
                        let $row = $(this).closest('tr');

                        if (_filaInsumo != null) {
                            let cantidadTraspaso = tblTraspasosPendientes.DataTable().row(_filaInsumo).data().cantidad;
                            let cantidadAnteriorUbicacion = $row.find('.inputCantidadSalida').val();

                            let sumatoriaUbicacion = 0;

                            tblUbicacion.find('tbody tr').each(function (idx, row) {
                                let cantidadUbicacion = $(row).find('.inputCantidadSalida').val();

                                sumatoriaUbicacion += cantidadUbicacion;
                            });

                            sumatoriaUbicacion -= cantidadAnteriorUbicacion;

                            let cantidadPendiente = cantidadTraspaso - sumatoriaUbicacion;

                            if (cantidadPendiente <= rowData.cantidad) {
                                $row.find('.inputCantidadSalida').val(cantidadPendiente);
                                $row.find('.inputCantidadSalida').change();
                            } else {
                                $row.find('.inputCantidadSalida').val(rowData.cantidad);
                                $row.find('.inputCantidadSalida').change();
                            }
                        } else {
                            $row.find('.inputCantidadSalida').val(rowData.cantidad);
                            $row.find('.inputCantidadSalida').change();
                        }
                    });
                },
                columns: [
                    { data: 'insumoDesc', title: 'Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'area_alm', title: 'Área' },
                    { data: 'lado_alm', title: 'Lado' },
                    { data: 'estante_alm', title: 'Estante' },
                    { data: 'nivel_alm', title: 'Nivel' },
                    {
                        sortable: false,
                        data: 'cantidadMovimiento',
                        render: (data, type, row, meta) => {
                            let valor = data != undefined ? data : '';

                            return `<div class="input-group">
                                        <span class="input-group-btn">
                                            <button class="btn btn-xs btn-default btnCantidadTotalSalida" type="button">
                                                <i class="fa fa-arrow-right"></i>
                                            </button>
                                        </span>
                                        <input type="text" class="form-control text-center inputCantidadSalida" value="${valor}" style="height: 22px;">
                                    </div>`;
                        },
                        title: 'Salida'
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '50%', targets: [0] }
                ]
            });
        }

        function cargarTraspasosOrigen() {
            let ccOrigen = selectCCOrigen.val() != 'Todos' ? selectCCOrigen.val() : 0;
            let almacenOrigen = selectAlmacenOrigen.val() != 'Todos' ? selectAlmacenOrigen.val() : 0;
            let ccDestino = selectCCDestino.val() != 'Todos' ? selectCCDestino.val() : 0;
            let almacenDestino = selectAlmacenDestino.val() != 'Todos' ? selectAlmacenDestino.val() : 0;
            let numeroRequisicion = inputNumeroRequisicion.val() != '' && !isNaN(inputNumeroRequisicion.val()) ? inputNumeroRequisicion.val() : 0;

            $.post('/Enkontrol/Almacen/GetTraspasosPendientesOrigen', { ccOrigen, almacenOrigen, ccDestino, almacenDestino, numeroRequisicion }).then(response => {
                if (response.success) {
                    if (response.data != null) {
                        AddRows(tblTraspasosPendientes, response.data);
                    } else {
                        limpiarTabla(tblTraspasosPendientes);
                    }
                } else {
                    AlertaGeneral(`Alerta`, `No se encontró información.`);
                    limpiarTabla(tblTraspasosPendientes);
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                limpiarTabla(tblTraspasosPendientes);
            }
            );
        }

        function guardarTraspasosOrigen() {
            let listaAutorizados = [];

            tblTraspasosPendientes.find('tbody tr').each(function (idx, row) {
                let rowData = tblTraspasosPendientes.DataTable().row($(row)).data();

                let comentariosGestion = $(row).find('.inputComentariosGestion').val();
                let cantidadAutorizar = $(row).find('.inputCantidadAutorizar').val();

                if ((!isNaN(cantidadAutorizar) && cantidadAutorizar != '' && cantidadAutorizar > 0)) {
                    listaAutorizados.push({
                        numeroRequisicion: rowData.numeroRequisicion,
                        ccOrigen: rowData.cc,
                        almacenOrigen: rowData.almacenOrigen,
                        ccDestino: rowData.cc,
                        almacenDestino: rowData.almacenDestino,
                        insumo: rowData.insumo,
                        comentariosGestion: comentariosGestion,
                        cantidad: rowData.cantidad,
                        cantidadTraspasar: cantidadAutorizar,
                        checkBoxAutorizado: true,
                        listUbicacionMovimiento: $(row).find('.btnUbicacionDetalle').data('listUbicacionMovimiento')
                    });
                }
            });

            if (listaAutorizados.length > 0) {
                btnGuardar.attr('disabled', true);

                $.post('/Enkontrol/Almacen/GuardarAutorizacionesTraspasosOrigen', { listaAutorizados }).then(response => {
                    if (response.success) {
                        btnGuardar.attr('disabled', false);

                        // if (response.listaAlmacenesOrigen.length > 1) {
                        //     if (response.flagMaquinaStandBy) {
                        //         AlertaGeneral(`Alerta`, `Se ha guardado la información. Se quitó el estado "Stand-By" de la máquina. Movimientos guardados: ` + response.listaMovimientosString);
                        //     } else {
                        //         AlertaGeneral(`Alerta`, `Se ha guardado la información. Movimientos guardados: ` + response.listaMovimientosString);
                        //     }
                        // } else {
                        if (response.flagMaquinaStandBy) {
                            AlertaGeneral(`Alerta`, `Se ha guardado la información. Se quitó el estado "Stand-By" de la máquina.`);
                        } else {
                            AlertaGeneral(`Alerta`, `Se ha guardado la información.`);
                        }

                        verReporteOrigen(response.listaAlmacenesOrigen[0], response.listaNumeros[0]);
                        // }

                        btnBuscar.click();
                    } else {
                        AlertaGeneral(`Alerta`, `${response.message}`);
                        btnBuscar.click();
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
            } else {
                AlertaGeneral(`Alerta`, `No se ha capturado información.`);
            }
        }

        function verReporteOrigen(almacenID, numero) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });
            $.post('/Enkontrol/Almacen/GetSalidaConsultaTraspaso', { almacenID, numero })
                .always($.unblockUI)
                .then(response => {
                    if (response.success) {
                        verReporte();
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }

        function verImprimible() {
            let almacenID = selectAlmacenReporte.val();
            let numero = inputNumeroReporte.val();

            if (almacenID > 0 && numero > 0) {
                mdlVerReporte.modal('hide');
                selectAlmacenReporte.val('');
                inputNumeroReporte.val('');

                $.blockUI({ message: 'Procesando...', baseZ: 2000 });
                $.post('/Enkontrol/Almacen/GetSalidaConsultaTraspaso', { almacenID, numero })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            verReporte();
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                AlertaGeneral(`Alerta`, `Seleccione un almacén y un número de movimiento válido.`);
            }
        }

        function limpiarTabla(tbl) {
            dt = tbl.DataTable();
            dt.clear().draw();
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

        init();
    }
    $(document).ready(function () {
        Enkontrol.Compras.Requisicion.Salidas = new Salidas();
    })
        .ajaxStart(function () {
            $.blockUI({ message: 'Procesando...' });
        })
        .ajaxStop(function () {
            $.unblockUI();
        });
})();