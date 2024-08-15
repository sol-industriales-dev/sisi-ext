(() => {
    $.namespace('Enkontrol.Almacen.RegistroInventarioFisico');
    RegistroInventarioFisico = function () {
        //#region Selectores
        const inputAlmacenNum = $('#inputAlmacenNum');
        const inputAlmacenDesc = $('#inputAlmacenDesc');
        const inputFecha = $('#inputFecha');
        const selectExistentes = $('#selectExistentes');
        const botonProcesar = $('#botonProcesar');
        const botonImprimir = $('#botonImprimir');
        const botonCongelar = $('#botonCongelar');
        const botonGuardar = $('#botonGuardar');
        const tablaExistencias = $('#tablaExistencias');
        const divTablaInsumosBotones = $('#divTablaInsumosBotones');
        const divTablaInsumosTabla = $('#divTablaInsumosTabla');
        const botonAgregarRenglon = $('#botonAgregarRenglon');
        const tablaInsumos = $('#tablaInsumos');
        const inputFiltroInsumo = $('#inputFiltroInsumo');
        const inputFiltroInsumoDesc = $('#inputFiltroInsumoDesc');
        const report = $('#report');
        const botonQuitarFiltros = $('#botonQuitarFiltros');
        //#endregion

        let dtExistencias;
        let dtInsumos;

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        _partida = null;
        _colIndex = 0;

        (function init() {
            $(document).keydown(function (e) {
                if (e.which == 40) {
                    return false;
                }
                if (e.which == 38) {
                    return false;
                }
            });
            initTablaExistencias();
            initTablaInsumos();

            inputFecha.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            botonProcesar.click(cargarExistenciasAlmacen);
            botonImprimir.click(imprimirInventarioFisico);
            botonCongelar.click(() => {
                Alert2AccionConfirmar('', `¿Desea congelar el almacén "${inputAlmacenDesc.val()}" al día "${inputFecha.val()}"?`, 'Aceptar', 'Cancelar', congelarAlmacenFecha, 'warning');
            });
            botonGuardar.click(guardarInventarioFisico);
            botonAgregarRenglon.click(agregarRenglon);
            botonQuitarFiltros.click(limpiarFiltros);
        })();

        //#region Eventos
        $('#inputAlmacenNum, #inputFecha').on('change', function () {
            cargarInventarioFisico();
        });

        inputAlmacenNum.on('change', function () {
            inputAlmacenDesc.val('');

            axios.post('ObtenerAlmacenID', { almacen: inputAlmacenNum.val() })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        if (response.data.items != null) {
                            inputAlmacenDesc.val(response.data.items.descripcion);
                        } else {
                            inputAlmacenDesc.val('');
                        }
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        });

        $('#inputFiltroInsumo, #inputFiltroInsumoDesc').on('keyup change', function () {
            dtInsumos.search('');
            dtInsumos.column($(this).data('columnIndex')).search(this.value).draw();

            dtExistencias.search('');
            dtExistencias.column($(this).data('columnIndex') - 1).search(this.value).draw();
        });
        //#endregion

        function initTablaExistencias() {
            dtExistencias = tablaExistencias.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 'Btp',
                buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 3, 4] }, className: 'btn btn-xs btn-default botonExcelExistencias', title: null }],
                order: [[4, 'asc']],
                initComplete: function (settings, json) {

                },
                columns: [
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Nombe del Insumo' },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'unidad', title: 'Unidad' },
                    { data: 'ubicacion', title: 'Ubicación' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    { width: '50%', targets: [0] }
                ]
            });

            $('.botonExcelExistencias').css('display', 'none');
        }

        function initTablaInsumos() {
            dtInsumos = tablaInsumos.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
                // buttons: [{ extend: 'excelHtml5', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8] }, className: 'btn btn-xs btn-default', title: null }],
                scrollY: '50vh',
                scrollCollapse: true,
                initComplete: function (settings, json) {
                    tablaInsumos.on('keydown', 'input', function (e) {
                        let inputFocus = $(':focus');

                        if (e.keyCode == 38) {
                            if (inputFocus.hasClass('inputCantidad')) {
                                let input = inputFocus.closest('tr').prev().find('.inputCantidad');

                                input.focus();
                                input.select();
                            } else if (inputFocus.hasClass('inputPrecio')) {
                                let input = inputFocus.closest('tr').prev().find('.inputPrecio');

                                input.focus();
                                input.select();
                            }
                        }

                        if (e.keyCode == 40) {
                            if (inputFocus.hasClass('inputCantidad')) {
                                let input = inputFocus.closest('tr').next().find('.inputCantidad');

                                input.focus();
                                input.select();
                            } else if (inputFocus.hasClass('inputPrecio')) {
                                let input = inputFocus.closest('tr').next().find('.inputPrecio');

                                input.focus();
                                input.select();
                            }
                        }
                    });

                    tablaInsumos.on('focus', 'input', function () {
                        $(this).select();
                    });

                    tablaInsumos.on('click', '.botonEliminarPartida', function () {
                        let row = $(this).closest('tr');
                        let rowData = dtInsumos.row(row).data();

                        if (rowData.nuevo) {
                            dtInsumos.row(row).remove().draw();

                            let cuerpo = tablaInsumos.find('tbody');

                            if (cuerpo.find("tr").length == 0) {
                                dtInsumos.draw();
                            }
                        } else {
                            _partida = rowData;

                            Alert2AccionConfirmar('', `¿Desea eliminar el insumo "${rowData.insumoDesc}"?`, 'Aceptar', 'Cancelar', eliminarRenglon, 'warning');
                        }
                    });

                    tablaInsumos.on('change', '.inputInsumo', function () {
                        let row = $(this).closest('tr');
                        let insumo = $(this).val();

                        if (insumo.length == 7) {
                            axios.post('/Enkontrol/Requisicion/GetInsumoInformacion', { insumo })
                                .then(response => {
                                    let { success, datos, message } = response.data;

                                    if (success) {
                                        colIndex = $(this).closest("td").parent().children().index($(this).closest("td"));

                                        let ins = response.data;

                                        row.find('.inputInsumo').val(ins.data.value);
                                        row.find('.inputInsumoDesc').val(ins.data.id);

                                        let rowData = dtInsumos.row(row).data();
                                        let insumoNumero = $(row).find('.inputInsumo').val();
                                        let insumoDesc = $(row).find('.inputInsumoDesc').val();
                                        let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;

                                        rowData.insumo = insumoNumero;
                                        rowData.insumoDesc = insumoDesc;
                                        rowData.cantidad = nuevaCantidad;

                                        dtInsumos.row(row).data(rowData).draw();

                                        recargarAutoCompleteInput(row);
                                        scrollAbajoTablaInsumos();

                                        row.find(`td:eq(${colIndex + 1})`).find("input").trigger("focus");
                                    } else {
                                        AlertaGeneral(`Alerta`, message);
                                    }
                                }).catch(error => AlertaGeneral(`Alerta`, error.message));
                        }
                    });

                    tablaInsumos.on('change', '.inputCantidad, .inputArea, .inputLado, .inputEstante, .inputNivel', function () {
                        let row = $(this).closest('tr');

                        colIndex = $(this).closest("td").parent().children().index($(this).closest("td"));

                        // tablaInsumos.find('tbody tr').each(function (index, row) {
                        let rowData = tablaInsumos.DataTable().row(row).data();

                        if (rowData.nuevo) {
                            let insumo = $(row).find('.inputInsumo').val();
                            let insumoDesc = $(row).find('.inputInsumoDesc').val();
                            let nuevaCantidad = !isNaN(unmaskNumero($(row).find('.inputCantidad').val())) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;
                            let area_alm = $(row).find('.inputArea').val();
                            let lado_alm = $(row).find('.inputLado').val();
                            let estante_alm = $(row).find('.inputEstante').val();
                            let nivel_alm = $(row).find('.inputNivel').val();

                            rowData.insumo = insumo;
                            rowData.insumoDesc = insumoDesc;
                            rowData.cantidad = nuevaCantidad;
                            rowData.area_alm = area_alm;
                            rowData.lado_alm = lado_alm;
                            rowData.estante_alm = estante_alm;
                            rowData.nivel_alm = nivel_alm;

                            dtInsumos.row(row).data(rowData).draw();

                            recargarAutoCompleteInput(row);
                            scrollAbajoTablaInsumos();
                        }
                        // });

                        row.find(`td:eq(${colIndex + 1})`).find("input").trigger("focus");
                    });

                    tablaInsumos.on('keyup', 'input', function () {
                        $(this).val($(this).val().toUpperCase());
                    });
                },
                createdRow: function (row, rowData) {
                    recargarAutoCompleteInput(row);
                },
                columns: [
                    {
                        title: '', render: function (data, type, row, meta) {
                            return ``;
                        }
                    },
                    {
                        data: 'insumo', title: 'Insumo', render: function (data, type, row, meta) {
                            return row.nuevo ? `<input class="form-control text-center inputInsumo" value="${data}">` : data;
                        }
                    },
                    {
                        data: 'insumoDesc', title: 'Nombre del Insumo', render: function (data, type, row, meta) {
                            return row.nuevo ? `<input class="form-control inputInsumoDesc" value="${data}">` : data;
                        }
                    },
                    {
                        data: 'cantidad', title: 'Cantidad', render: function (data, type, row, meta) {
                            return `<input class="form-control text-center inputCantidad" value="${data}" ${row.flagInventarioCerrado ? 'disabled' : ''}>`;
                        }
                    },
                    {
                        data: 'area_alm', title: 'Área Almacén', render: function (data, type, row, meta) {
                            return row.nuevo ? `<input class="form-control text-center inputArea" value="${data}">` : data;
                        }
                    },
                    {
                        data: 'lado_alm', title: 'Lado Área', render: function (data, type, row, meta) {
                            return row.nuevo ? `<input class="form-control text-center inputLado" value="${data}">` : data;
                        }
                    },
                    {
                        data: 'estante_alm', title: 'Estante Lado', render: function (data, type, row, meta) {
                            return row.nuevo ? `<input class="form-control text-center inputEstante" value="${data}">` : data;
                        }
                    },
                    {
                        data: 'nivel_alm', title: 'Nivel Estante', render: function (data, type, row, meta) {
                            return row.nuevo ? `<input class="form-control text-center inputNivel" value="${data}">` : data;
                        }
                    },
                    {
                        title: '', render: function (data, type, row, meta) {
                            return `<button class="btn btn-xs btn-danger botonEliminarPartida" ${row.flagInventarioCerrado ? 'disabled' : ''}><i class="fa fa-times"></i></button>`;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", targets: [0, 1, 3, 4, 5, 6, 7, 8] },
                    { searchable: false, orderable: false, targets: 0 },
                    { orderable: false, targets: [1, 2, 3, 4, 5, 6, 7, 8] },
                    { width: '8%', targets: [1] }
                ]
            });

            dtInsumos.on('order.dt search.dt', function () {
                dtInsumos.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                    cell.innerHTML = i + 1;
                });
            }).draw();
        }

        function cargarExistenciasAlmacen() {
            let almacen = +inputAlmacenNum.val();
            let fecha = inputFecha.val();
            let existentes = +selectExistentes.val() == 1;

            if (almacen > 0 && fecha.length > 0) {
                axios.post('CargarExistenciasAlmacen', { almacen, fecha, existentes })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            AddRows(tablaExistencias, datos);

                            tablaExistencias.css('display', 'table');
                            $('.botonExcelExistencias').css('display', 'inline-block');
                            $('.botonExcelExistencias').css('margin-bottom', '5px');
                            divTablaInsumosBotones.css('display', 'none');
                            divTablaInsumosTabla.css('display', 'none');
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function imprimirInventarioFisico() {
            report.attr("src", '/Reportes/Vista.aspx?idReporte=218');

            $.blockUI({ message: 'Generando Imprimible...' });
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function cargarInventarioFisico() {
            let almacen = +inputAlmacenNum.val();
            let fecha = inputFecha.val();

            tablaExistencias.css('display', 'none');
            $('.botonExcelExistencias').css('display', 'none');
            divTablaInsumosBotones.css('display', 'none');
            divTablaInsumosTabla.css('display', 'none');
            botonProcesar.attr('disabled', true);
            botonImprimir.attr('disabled', true);
            botonCongelar.attr('disabled', true);
            botonGuardar.attr('disabled', true);

            if (almacen > 0 && fecha.length > 0) {
                axios.post('CargarInventarioFisico', { almacen, fecha })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            AddRows(tablaInsumos, datos);

                            tablaExistencias.css('display', 'none');
                            $('.botonExcelExistencias').css('display', 'none');

                            if (datos.length > 0) {
                                divTablaInsumosBotones.css('display', 'block');
                            }

                            divTablaInsumosTabla.css('display', 'block');
                            dtInsumos.columns.adjust().draw();

                            botonProcesar.attr('disabled', datos.length > 0);
                            botonImprimir.attr('disabled', datos.length > 0);
                            botonCongelar.attr('disabled', datos.length > 0);
                            botonGuardar.attr('disabled', !(datos.length > 0));

                            //Inventario Cerrado
                            botonGuardar.attr('disabled', response.data.flagInventarioCerrado);
                            botonAgregarRenglon.attr('disabled', response.data.flagInventarioCerrado);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function guardarInventarioFisico() {
            let partidas = [];

            tablaInsumos.find('tbody tr').each((index, row) => {
                let rowData = dtInsumos.row(row).data();

                if (rowData.nuevo) {
                    if (comprobarInformacionRenglonNuevo(row)) {
                        partidas.push({
                            insumo: +$(row).find('.inputInsumo').val(),
                            cantidad: +$(row).find('.inputCantidad').val(),
                            area_alm: $(row).find('.inputArea').val(),
                            lado_alm: $(row).find('.inputLado').val(),
                            estante_alm: $(row).find('.inputEstante').val(),
                            nivel_alm: $(row).find('.inputNivel').val(),
                            nuevo: rowData.nuevo
                        });
                    }
                } else {
                    partidas.push({
                        almacen: rowData.almacen,
                        fecha: rowData.fechaString,
                        insumo: rowData.insumo,
                        cantidad: +$(row).find('.inputCantidad').val(),
                        partida: rowData.partida,
                        nuevo: rowData.nuevo
                    });
                }
            });

            if (partidas.length > 0) {
                let partidasString = JSON.stringify(partidas);

                axios.post('GuardarInventarioFisico', { partidasString })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            Alert2Exito('Se ha guardado la información.');
                            inputAlmacenNum.change();
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function comprobarInformacionRenglonNuevo(row) {
            let insumo = +$(row).find('.inputInsumo').val();
            let cantidad = +$(row).find('.inputCantidad').val();
            let area_alm = $(row).find('.area_alm').val();
            let lado_alm = $(row).find('.lado_alm');
            let estante_alm = $(row).find('.estante_alm');
            let nivel_alm = $(row).find('.nivel_alm');

            if (isNaN(insumo) || insumo < 1000000 || isNaN(cantidad) || area_alm == '' || lado_alm == '' || estante_alm == '' || nivel_alm == '') {
                return false;
            } else {
                return true;
            }
        }

        function congelarAlmacenFecha() {
            let almacen = +inputAlmacenNum.val();
            let fecha = inputFecha.val();

            if (almacen > 0 && fecha.length > 0) {
                axios.post('CongelarAlmacenInventarioFisico', { almacen, fecha })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            Alert2Exito('Se ha guardado la información.');

                            //#region Simular el click al botón de procesar y al botón de imprimir
                            axios.post('CargarExistenciasAlmacen', { almacen, fecha, existentes: true })
                                .then(response => {
                                    let { success, datos, message } = response.data;

                                    if (success) {
                                        AddRows(tablaExistencias, datos);

                                        tablaExistencias.css('display', 'table');
                                        $('.botonExcelExistencias').css('display', 'inline-block');
                                        $('.botonExcelExistencias').css('margin-bottom', '5px');
                                        divTablaInsumosBotones.css('display', 'none');
                                        divTablaInsumosTabla.css('display', 'none');

                                        imprimirInventarioFisico();

                                        //Cargar el inventario físico.
                                        inputAlmacenNum.change();
                                    } else {
                                        AlertaGeneral(`Alerta`, message);
                                    }
                                }).catch(error => AlertaGeneral(`Alerta`, error.message));
                            //#endregion
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function agregarRenglon() {
            let datos = dtInsumos.rows().data();

            datos.push({
                insumo: '',
                insumoDesc: '',
                cantidad: 0,
                area_alm: '',
                lado_alm: '',
                estante_alm: '',
                nivel_alm: '',
                nuevo: true
            });

            dtInsumos.clear();
            dtInsumos.rows.add(datos).draw();

            scrollAbajoTablaInsumos();
        }

        function eliminarRenglon() {
            axios.post('EliminarPartidaInventarioFisico', { partida: _partida })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado la partida.');
                        inputAlmacenNum.change();
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

        function scrollAbajoTablaInsumos() {
            var scrollTabla = document.getElementsByClassName("dataTables_scrollBody")[0];
            scrollTabla.scrollTop = scrollTabla.scrollHeight;
        }

        function recargarAutoCompleteInput(row) {
            // let inputInsumo = $(row).find('.inputInsumo');
            let inputInsumoDesc = $(row).find('.inputInsumoDesc');

            if (inputInsumoDesc.length > 0) {
                // inputInsumo.getAutocomplete(setInsumoDesc, null, '/Enkontrol/Requisicion/GetInsumoInformacion');
                inputInsumoDesc.getAutocomplete(setInsumoBusqPorDesc, null, '/Enkontrol/Requisicion/getInsumosDescAutoComplete');
            }
        }

        function limpiarFiltros() {
            inputFiltroInsumo.val('');
            inputFiltroInsumoDesc.val('');

            dtInsumos.columns().search('').draw();
            dtExistencias.columns().search('').draw();
        }

        // function setInsumoDesc(e, ui) {
        //     let row = $(this).closest('tr');
        //     let rowData = dtInsumos.row(row).data();

        //     row.find('.inputInsumo').val(ui.item.value);
        //     row.find('.inputInsumoDesc').val(ui.item.id);

        //     let insumo = $(row).find('.inputInsumo').val();
        //     let insumoDesc = $(row).find('.inputInsumoDesc').val();
        //     let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;

        //     rowData.insumo = insumo;
        //     rowData.insumoDesc = insumoDesc;
        //     rowData.cantidad = nuevaCantidad;

        //     dtInsumos.row(row).data(rowData).draw();

        //     recargarAutoCompleteInput(row);
        //     scrollAbajoTablaInsumos();
        // }

        function setInsumoBusqPorDesc(e, ui) {
            let row = $(this).closest('tr');
            let rowData = dtInsumos.row(row).data();

            row.find('.inputInsumo').val(ui.item.id);
            row.find('.inputInsumoDesc').val(ui.item.value);

            let insumo = ui.item.id;
            let insumoDesc = $(row).find('.inputInsumoDesc').val();
            let nuevaCantidad = !isNaN($(row).find('.inputCantidad').val()) ? unmaskNumero($(row).find('.inputCantidad').val()) : 0;

            rowData.insumo = insumo;
            rowData.insumoDesc = insumoDesc;
            rowData.cantidad = nuevaCantidad;

            dtInsumos.row(row).data(rowData).draw();

            recargarAutoCompleteInput(row);
            scrollAbajoTablaInsumos();

            row.find('.inputInsumo').val(ui.item.id);

            //Retornar falso para que no se ejecute la función de select por default de JQuery UI.
            return false;
        }
    }
    $(document).ready(() => Enkontrol.Almacen.RegistroInventarioFisico = new RegistroInventarioFisico())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();