(() => {
    $.namespace('Enkontrol.Almacen.Almacen.ConsultaTraspaso');
    ConsultaTraspaso = function () {
        //#region Selectores
        //#region Salida
        const almOrigenNumSal = $('#almOrigenNumSal');
        const almOrigenDescSal = $('#almOrigenDescSal');
        const numMovSal = $('#numMovSal');
        const ccOrigenNumSal = $('#ccOrigenNumSal');
        const ccOrigenDescSal = $('#ccOrigenDescSal');
        const fechaMovSal = $('#fechaMovSal');
        const entregoNumSal = $('#entregoNumSal');
        const entregoDescSal = $('#entregoDescSal');
        const tipoMovSal = $('#tipoMovSal');
        const comentariosSal = $('#comentariosSal');
        const almDestinoNumSal = $('#almDestinoNumSal');
        const almDestinoDescSal = $('#almDestinoDescSal');
        const ccDestinoNumSal = $('#ccDestinoNumSal');
        const ccDestinoDescSal = $('#ccDestinoDescSal');
        const ordenTraspasoSal = $('#ordenTraspasoSal');
        const folioTraspasoSal = $('#folioTraspasoSal');
        const totalSal = $('#totalSal');
        const tablaPartidasSal = $('#tablaPartidasSal');
        const botonReporteSalida = $('#botonReporteSalida');
        //#endregion

        //#region Entrada
        const almDestinoNumEnt = $('#almDestinoNumEnt');
        const almDestinoDescEnt = $('#almDestinoDescEnt');
        const numMovEnt = $('#numMovEnt');
        const ccDestinoNumEnt = $('#ccDestinoNumEnt');
        const ccDestinoDescEnt = $('#ccDestinoDescEnt');
        const fechaMovEnt = $('#fechaMovEnt');
        const recibioNumEnt = $('#recibioNumEnt');
        const recibioDescEnt = $('#recibioDescEnt');
        const tipoMovEnt = $('#tipoMovEnt');
        const comentariosEnt = $('#comentariosEnt');
        const almOrigenNumEnt = $('#almOrigenNumEnt');
        const almOrigenDescEnt = $('#almOrigenDescEnt');
        const ccOrigenNumEnt = $('#ccOrigenNumEnt');
        const ccOrigenDescEnt = $('#ccOrigenDescEnt');
        const ordenTraspasoEnt = $('#ordenTraspasoEnt');
        const folioTraspasoEnt = $('#folioTraspasoEnt');
        const totalEnt = $('#totalEnt');
        const tablaPartidasEnt = $('#tablaPartidasEnt');
        const botonReporteEntrada = $('#botonReporteEntrada');
        //#endregion

        const report = $("#report");
        //#endregion

        let dtPartidasSal;
        let dtPartidasEnt;

        (function init() {
            agregarListeners();
            initTablaPartidasSal();
            initTablaPartidasEnt();

            numMovSal.change(cargarSalidaTraspaso);
            numMovEnt.change(cargarEntradaTraspaso);
        })();

        almOrigenNumSal.on('change', function () {
            let almacenID = almOrigenNumSal.val();

            botonReporteSalida.attr('disabled', true);

            $.post('/Enkontrol/Almacen/GetNuevaSalidaConsultaTraspaso', { almacenID })
                .then(response => {
                    if (response.success) {
                        almOrigenDescSal.val(response.almacenDesc);
                        numMovSal.val(response.numeroDisponible);
                        entregoNumSal.val(response.entregoNum);
                        entregoDescSal.val(response.entregoDesc);
                        fechaMovSal.val(response.fecha);
                        totalSal.val('$' + formatMoney(0));
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        almDestinoNumEnt.on('change', function () {
            let almacenID = almDestinoNumEnt.val();

            botonReporteEntrada.attr('disabled', true);

            $.post('/Enkontrol/Almacen/GetNuevaEntradaConsultaTraspaso', { almacenID })
                .then(response => {
                    if (response.success) {
                        almDestinoDescEnt.val(response.almacenDesc);
                        numMovEnt.val(response.numeroDisponible);
                        recibioNumEnt.val(response.recibioNum);
                        recibioDescEnt.val(response.recibioDesc);
                        fechaMovEnt.val(response.fecha);
                        totalEnt.val('$' + formatMoney(0));
                    } else {
                        AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                    }
                }, error => {
                    AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                }
                );
        });

        function agregarListeners() {
            botonReporteSalida.click(verReporteSalida);
            botonReporteEntrada.click(verReporteEntrada);
        }

        function initTablaPartidasSal() {
            dtPartidasSal = tablaPartidasSal.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                // scrollY: '45vh',
                // scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'partida', title: 'Pda.' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    {
                        title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            return `${row.area}-${row.cuenta}`;
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Costo Promedio', render: function (data, type, row, meta) {
                            return row.precio != null ? '$' + formatMoney(row.precio) : '$' + formatMoney(0); //Se pone el precio porque en Enkontrol muestra el precio pero con nombre de "Costo Promedio".
                        }
                    },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            return row.importe != null ? '$' + formatMoney(row.importe) : '$' + formatMoney(0);
                        }
                    },
                    { data: 'area_alm', title: 'Área Alm.' },
                    { data: 'lado_alm', title: 'Lado Área' },
                    { data: 'estante_alm', title: 'Estante Lado' },
                    { data: 'nivel_alm', title: 'Nivel Estante' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function initTablaPartidasEnt() {
            dtPartidasEnt = tablaPartidasEnt.DataTable({
                retrieve: true,
                paging: false,
                searching: false,
                language: dtDicEsp,
                bInfo: false,
                // scrollY: '45vh',
                // scrollCollapse: true,
                initComplete: function (settings, json) {

                },
                createdRow: function (row, rowData) {

                },
                columns: [
                    { data: 'partida', title: 'Pda.' },
                    { data: 'insumo', title: 'Insumo' },
                    { data: 'insumoDesc', title: 'Descripción' },
                    {
                        title: 'Área-Cuenta', render: function (data, type, row, meta) {
                            return `${row.area}-${row.cuenta}`;
                        }
                    },
                    { data: 'cantidad', title: 'Cantidad' },
                    { data: 'unidad', title: 'Unidad' },
                    {
                        data: 'precio', title: 'Precio Unitario', render: function (data, type, row, meta) {
                            return row.precio != null ? '$' + formatMoney(row.precio) : '$' + formatMoney(0);
                        }
                    },
                    {
                        data: 'importe', title: 'Importe', render: function (data, type, row, meta) {
                            return row.importe != null ? '$' + formatMoney(row.importe) : '$' + formatMoney(0);
                        }
                    },
                    { data: 'area_alm', title: 'Área Alm.' },
                    { data: 'lado_alm', title: 'Lado Área' },
                    { data: 'estante_alm', title: 'Estante Lado' },
                    { data: 'nivel_alm', title: 'Nivel Estante' }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });
        }

        function cargarSalidaTraspaso() {
            let almacenID = almOrigenNumSal.val();
            let numero = numMovSal.val();

            limpiarSalida();
            botonReporteSalida.attr('disabled', true);

            if ((almacenID != '' && !isNaN(almacenID)) && (numero != '' && !isNaN(numero))) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetSalidaConsultaTraspaso', { almacenID, numero })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            llenarSalida(response.data);
                            botonReporteSalida.attr('disabled', false);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                limpiarSalida();
            }
        }

        function cargarEntradaTraspaso() {
            let almacenID = almDestinoNumEnt.val();
            let numero = numMovEnt.val();

            limpiarEntrada();
            botonReporteEntrada.attr('disabled', true);

            if ((almacenID != '' && !isNaN(almacenID)) && (numero != '' && !isNaN(numero))) {
                $.blockUI({ message: 'Procesando...' });
                $.post('/Enkontrol/Almacen/GetEntradaConsultaTraspaso', { almacenID, numero })
                    .always($.unblockUI)
                    .then(response => {
                        if (response.success) {
                            llenarEntrada(response.data);
                            botonReporteEntrada.attr('disabled', false);
                        } else {
                            AlertaGeneral(`Alerta`, `Error al consultar la información. ${response.message}`);
                        }
                    }, error => {
                        AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
                    }
                    );
            } else {
                limpiarEntrada();
            }
        }

        function llenarSalida(data) {
            almOrigenNumSal.val(data.almacen);
            almOrigenDescSal.val(data.almacenDesc);
            numMovSal.val(data.numero);
            ccOrigenNumSal.val(data.cc);
            ccOrigenDescSal.val(data.ccDesc);
            fechaMovSal.val(data.fechaString);
            entregoNumSal.val(data.empleado);
            entregoDescSal.val(data.empleadoDesc);
            tipoMovSal.val(52);
            comentariosSal.val(data.comentarios);
            almDestinoNumSal.val(data.alm_destino);
            almDestinoDescSal.val(data.alm_destinoDesc);
            ccDestinoNumSal.val(data.cc_destino);
            ccDestinoDescSal.val(data.cc_destinoDesc);
            ordenTraspasoSal.val(data.orden_ct);
            folioTraspasoSal.val(data.folio_traspaso);
            totalSal.val('$' + formatMoney(data.total));

            AddRows(tablaPartidasSal, data.detalle);
        }

        function llenarEntrada(data) {
            almDestinoNumEnt.val(data.almacen); //Campos volteados en la tabla de Enkontrol.
            almDestinoDescEnt.val(data.almacenDesc); //Campos volteados en la tabla de Enkontrol.
            numMovEnt.val(data.numero);
            ccDestinoNumEnt.val(data.cc); //Campos volteados en la tabla de Enkontrol.
            ccDestinoDescEnt.val(data.ccDesc); //Campos volteados en la tabla de Enkontrol.
            fechaMovEnt.val(data.fechaString);
            recibioNumEnt.val(data.empleado);
            recibioDescEnt.val(data.empleadoDesc);
            tipoMovEnt.val(2);
            comentariosEnt.val(data.comentarios);
            almOrigenNumEnt.val(data.alm_destino); //Campos volteados en la tabla de Enkontrol.
            almOrigenDescEnt.val(data.alm_destinoDesc); //Campos volteados en la tabla de Enkontrol.
            ccOrigenNumEnt.val(data.cc_destino); //Campos volteados en la tabla de Enkontrol.
            ccOrigenDescEnt.val(data.cc_destinoDesc); //Campos volteados en la tabla de Enkontrol.
            ordenTraspasoEnt.val(data.orden_ct);
            folioTraspasoEnt.val(data.folio_traspaso);
            totalEnt.val('$' + formatMoney(data.total));

            AddRows(tablaPartidasEnt, data.detalle);
        }

        function limpiarSalida() {
            almOrigenNumSal.val('');
            almOrigenDescSal.val('');
            numMovSal.val('');
            ccOrigenNumSal.val('');
            ccOrigenDescSal.val('');
            fechaMovSal.val('');
            entregoNumSal.val('');
            entregoDescSal.val('');
            tipoMovSal.val('');
            comentariosSal.val('');
            almDestinoNumSal.val('');
            almDestinoDescSal.val('');
            ccDestinoNumSal.val('');
            ccDestinoDescSal.val('');
            ordenTraspasoSal.val('');
            folioTraspasoSal.val('');
            totalSal.val('');

            dtPartidasSal.clear().draw();
        }

        function limpiarEntrada() {
            almDestinoNumEnt.val('');
            almDestinoDescEnt.val('');
            numMovEnt.val('');
            ccDestinoNumEnt.val('');
            ccDestinoDescEnt.val('');
            fechaMovEnt.val('');
            recibioNumEnt.val('');
            recibioDescEnt.val('');
            tipoMovEnt.val('');
            comentariosEnt.val('');
            almOrigenNumEnt.val('');
            almOrigenDescEnt.val('');
            ccOrigenNumEnt.val('');
            ccOrigenDescEnt.val('');
            ordenTraspasoEnt.val('');
            folioTraspasoEnt.val('');
            totalEnt.val('');

            dtPartidasEnt.clear().draw();
        }

        function verReporteSalida() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=128`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function verReporteEntrada() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=129`);
            document.getElementById('report').onload = function () {
                openCRModal();
            };
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }

        function formatMoney(amount, decimalCount = 2, decimal = ".", thousands = ",") {
            try {
                decimalCount = Math.abs(decimalCount);
                decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

                const negativeSign = amount < 0 ? "-" : "";

                let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
                let j = (i.length > 3) ? i.length % 3 : 0;

                return negativeSign + (j ? i.substr(0, j) + thousands : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) + (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
            } catch (e) {
                console.log(e)
            }
        }
    }
    $(document).ready(() => Enkontrol.Almacen.Almacen.ConsultaTraspaso = new ConsultaTraspaso())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();