(() => {
    $.namespace('Enkontrol.Almacen.ValuacionInventarioFisico');
    ValuacionInventarioFisico = function () {
        //#region Selectores
        const inputCC = $('#inputCC');
        const inputCCDesc = $('#inputCCDesc');
        const inputAlmacenNum = $('#inputAlmacenNum');
        const inputAlmacenDesc = $('#inputAlmacenDesc');
        const inputFecha = $('#inputFecha');
        const inputInsumoInicio = $("#inputInsumoInicio");
        const inputInsumoFin = $("#inputInsumoFin");
        const checkSoloConDiferencia = $('#checkSoloConDiferencia');
        const botonBuscar = $('#botonBuscar');
        const botonImprimir = $('#botonImprimir');

        const tablaInsumos = $('#tablaInsumos');
        const report = $('#report');
        //#endregion

        let dtExistencias;
        let dtInsumos;

        //#region Variables Date
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        (function init() {
            initTablaInsumos();
            cargarIntervaloInsumos();
            inputFecha.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            botonBuscar.click(cargarInventarioFisico);
            botonImprimir.click(imprimirInventarioFisico);

            colocarCentroCostoDefault();
        })();

        //#region Eventos
        $('#inputCC, #inputAlmacenNum, #inputFecha, #inputInsumoInicio, #inputInsumoFin').on('change', function () {
            cargarInventarioFisico();
        });

        $('#inputAlmacenNum').on('change', function () {
            cargarDescripcionAlmacen();
        });

        $('#inputCC').on('change', function () {
            cargarDescripcionCC();
        });
        //#endregion


        function initTablaInsumos() {
            dtInsumos = tablaInsumos.DataTable({
                retrieve: true,
                paging: false,
                language: dtDicEsp,
                dom: 't',
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
                },
                columns: [
                    //{ data: 'partida', title: '' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Nombre del Insumo' },
                    { data: 'cantidad', title: 'Inventario Físico', render: (data, type, row) => { return '<p class="cantidad">' + parseFloat(data, 16).toFixed(6).replace(/(\d)(?=(\d{3})+\.)/g, "1,").toString() + '</p><p class="unidad">&nbsp;' + row.unidad + '</p>'; } },
                    { data: 'costoPromedio', title: 'Costo Promedio', render: (data, type, row) => { return '$' + parseFloat(data, 16).toFixed(6).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString(); } },
                    { data: 'total', title: 'Importe', render: (data, type, row) => { return '$' + parseFloat(data, 16).toFixed(6).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString(); } }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function colocarCentroCostoDefault() {
            axios.post('getEmpresa')
                .then(response => {
                    if (response.data == 1) {
                        inputCC.val('998');
                        inputCCDesc.val('OBRAS CERRADAS');
                    } else if (response.data == 2) {
                        inputCC.val('001');
                        inputCCDesc.val('ADMINISTRACIÓN CENTRAL ARRENDADORA');
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function imprimirInventarioFisico() {
            report.attr("src", '/Reportes/Vista.aspx?idReporte=220');

            $.blockUI({ message: 'Generando Imprimible...' });
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }

        function cargarInventarioFisico() {
            let cc = inputCC.val();
            let almacen = +inputAlmacenNum.val();
            let fecha = inputFecha.val();
            let insumoInicio = +inputInsumoInicio.val();
            let insumoFin = +inputInsumoFin.val();
            let soloConDiferencia = checkSoloConDiferencia.prop('checked');

            tablaInsumos.css('display', 'none');
            botonImprimir.attr('disabled', true);

            if (inputCC.length > 0 && almacen > 0 && fecha.length > 0 && insumoInicio > 0 && insumoFin > 0) {
                axios.post('CargarInventarioFisicoInsumo', { cc, almacen, fecha, insumoInicio, insumoFin, soloConDiferencia })
                    .then(response => {
                        let { success, datos, message } = response.data;

                        if (success) {
                            AddRows(tablaInsumos, datos);
                            tablaInsumos.css('display', 'table');
                            botonImprimir.attr('disabled', datos.length < 1);
                        } else {
                            AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }
        function cargarDescripcionAlmacen() {
            let almacen = +inputAlmacenNum.val();
            if (almacen > 0) {
                axios.post('CargarDescripcionAlmacen', { almacen })
                    .then(response => {
                        let { success, descripcion, message } = response.data;
                        if (success) {
                            inputAlmacenDesc.val(descripcion);
                        } else {
                            //AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }
        function cargarDescripcionCC() {
            let cc = inputCC.val();
            if (cc.length > 0) {
                axios.post('CargarDescripcionCC', { cc })
                    .then(response => {
                        let { success, descripcion, message } = response.data;
                        if (success) {
                            inputCCDesc.val(descripcion);
                        } else {
                            //AlertaGeneral(`Alerta`, message);
                        }
                    }).catch(error => AlertaGeneral(`Alerta`, error.message));
            }
        }

        function cargarIntervaloInsumos() {
            axios.post('cargarIntervaloInsumos', {})
                .then(response => {
                    let { success, primerInsumo, ultimoInsumo, message } = response.data;
                    if (success) {
                        inputInsumoInicio.val(primerInsumo.insumo);
                        inputInsumoFin.val(ultimoInsumo.insumo);
                    } else {
                        //AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));

        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
        }
    }
    $(document).ready(() => Enkontrol.Almacen.ValuacionInventarioFisico = new ValuacionInventarioFisico())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();