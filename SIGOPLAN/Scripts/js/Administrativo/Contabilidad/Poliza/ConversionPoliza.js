(() => {
    $.namespace('Administrativo.Poliza.ConversionPoliza');
    ConversionPoliza = function () {
        let busqCapturaSession = {};
        const report = $('#report');
        const idEmpresa = $('#idEmpresa');
        const inputTpFiltro = $('#inputTpFiltro');
        const inputMontoFiltro = $('#inputMontoFiltro');
        const inputPolizaFiltro = $('#inputPolizaFiltro');
        const selectFiltroAnio = $('#selectFiltroAnio');
        const selectFiltroMes = $('#selectFiltroMes');
        const btnFiltroBuscar = $('#btnFiltroBuscar');
        const btnCapturarPoliza = $('#btnCapturarPoliza');
        const tblConversionPolizas = $('#tblConversionPolizas');
        const modalPolizaDetalle = $('#modalPolizaDetalle');
        const tblPolizaDetalle = $('#tblPolizaDetalle');
        const modalCapturaPoliza = $('#modalCapturaPoliza');
        const inputFechaConversion = $('#inputFechaConversion');
        const btnGenerarPoliza = $('#btnGenerarPoliza');
        const tblNuevaPoliza = $('#tblNuevaPoliza');
        const btnConvertirPolizas = $('#btnConvertirPolizas');
        const btnCapturaBuscar = $('#btnCapturaBuscar');
        const BuscarCaptura = originURL('/Administrativo/ConversionPoliza/BuscarCaptura');
        const saveOrUpdatePoliza = originURL('/Administrativo/ConversionPoliza/saveOrUpdatePoliza');
        const ObtenerPolizaEnkontrol = originURL('/Administrativo/ConversionPoliza/ObtenerPolizaEnkontrol');
        const saveOrUpdatePolizaSession = originURL('/Administrativo/ConversionPoliza/saveOrUpdatePolizaSession');
        const BuscarTipoCambio = originURL('/Administrativo/TipoCambio/ExisteTipoCambio');
        const fechaActual = new Date();
        const añoInicial = 2018;
        const añoActual = fechaActual.getFullYear();
        const mesActual = fechaActual.getMonth() + 1;
        const meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];
        const inputaplicaFecha = $("#inputaplicaFecha");
        const inputSelectALL = $("#inputSelectALL");
        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar póliza`, action: "descargarPoliza", fn: parametros => { reportePoliza(true, false, false, parametros, true) } },
                { text: `<i class="fa fa-file"></i> Ver póliza`, action: "visorPoliza", fn: parametros => { reportePoliza(true, false, false, parametros, false); } },
                { text: `<i class="fa fa-download"></i> Descargar póliza detalle`, action: "descargarPolizaDetalle", fn: parametros => { reportePoliza(false, true, false, parametros, true) } },
                { text: `<i class="fa fa-file"></i> Ver póliza detalle`, action: "visorPolizaDetalle", fn: parametros => { reportePoliza(false, true, false, parametros, false) } },
            ]
        }
        const TiposMovimiento = {
            Cargo: 1,
            Abono: 2
        }

        let dtPolizaDetalle, dtConversionPolizas, dtNuevapoliza, dtNuevapolizaPeru;

        (() => initForm())();
        function initForm() {

            initComboBox();
            initTablaPolizas();
            initTblPolizaDetalle();
            initDataTblNuevapoliza();
            AddListerners();
            fnValidateTipoCambio();
        }

        function fnValidateTipoCambio() {
            let busq = getFormCaptura();
            axios.post(BuscarTipoCambio, { fechaActual: busq.fechaConversion }).then(response => {
                let { success } = response.data;
                if (!success) {
                    AlertaGeneral('Aviso', 'No hay tipo de cambio capturado el día de hoy')
                }
            }).catch(o_O => AlertaGeneral('Alerta', o_O.message));

        }

        function setBuscarCaptura() {
            dtNuevapoliza.rows().clear().draw();
            let busq = getFormCaptura();
            axios.post(BuscarCaptura, { fechaConversion: busq.fechaConversion, year: busq.year, mes: busq.mes })
                .then(response => {
                    let { success, lst } = response.data;
                    if (success) {
                        busqCapturaSession = busq;
                        getFormCaptura();
                        dtNuevapoliza.rows.add(lst).draw();
                    }
                    else {
                        AlertaGeneral("Aviso", "No se encontro tipo de cambio");
                    }
                }).catch(o_O => AlertaGeneral("Aviso", o_O.message));
        }

        function getFormCaptura() {
            return {
                fechaConversion: inputFechaConversion.val().toDate(),
            };
        }

        function AddListerners() {
            btnFiltroBuscar.on('click', function () {
                let year = +selectFiltroAnio.val();
                let mes = +selectFiltroMes.val();
                let tp = inputTpFiltro.val();
                let poliza = +inputPolizaFiltro.val();
                let monto = inputMontoFiltro.val();
                CargarTblConversionPolizas(year, mes, tp, poliza, monto);
            });
            btnGenerarPoliza.click(setBuscarCaptura);
            btnCapturarPoliza.click(mostrarMdlCapturaPoliza);
            btnConvertirPolizas.click(setGuardarPolizas);
            btnCapturaBuscar.click(setBuscarCaptura);
            $('.modal').on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });

            inputSelectALL.change(fnSetChecked);

        }
        function fnSetChecked() {
            let isChecked = $(this).prop('checked');

            if (isChecked)
                $('.inputCheckedEstatus').prop('checked', true);
            else
                $('.inputCheckedEstatus').prop('checked', false);
        }

        function setGuardarPolizas() {
            let listaPolizas = getLstConversion();
            $.LoadInMemoryThenSave(saveOrUpdatePolizaSession.href, saveOrUpdatePoliza, listaPolizas, { fechaConversion: busqCapturaSession.fechaConversion, aplicaFechaPoliza: inputaplicaFecha.prop('checked') }, 50, thenGuardarPolizas);
        }
        function thenGuardarPolizas() {
            modalCapturaPoliza.modal('hide');
            dtNuevapoliza.rows().clear().draw()
            AlertaGeneral("Aviso", "Conversiones de polizas realizadas con exito.")
        }
        function getLstConversion() {
            let lst = [];
            dtNuevapoliza.rows().every(function (rowIdx, tableLoop, rowLoop) {
                let row = this.node()
                    , data = this.data();
                if ($(row).find("input:checkbox").is(':checked')) {
                    lst.push(data);
                }
            });
            return lst;
        }
        function mostrarMdlCapturaPoliza() {
            modalCapturaPoliza.modal('show');
        }
        function initComboBox() {
            for (let años = añoInicial; años <= añoActual; años++) {
                selectFiltroAnio.append('<option value="' + años + '">' + años + '</option>');
            }
            selectFiltroAnio.val(añoActual);

            meses.forEach(function (value, index, array) {
                selectFiltroMes.append('<option value="' + (index + 1) + '">' + value + '</option>');
            });
            selectFiltroMes.val(mesActual);
            inputFechaConversion.datepicker().datepicker('setDate', new Date());
        }
        function CargarTblConversionPolizas(year, mes, tp, poliza, monto) {
            dtConversionPolizas.clear().draw();
            axios.post('/Administrativo/ConversionPoliza/CargarConversionPolizas', {
                year: year,
                mes: mes,
                tp: tp,
                poliza: poliza,
                monto: monto
            }).then(response => {
                if (response.data.success) {
                    dtConversionPolizas.rows.add(response.data.resultado).draw();
                }
                else {
                    AlertaGeneral('Alerta', response.data.Message);
                }
            }).catch(error => AlertaGeneral('Alerta', error.statusText));
        }

        function obtenerPolizaDetalle(year, mes, tp, poliza, empresa) {
            dtPolizaDetalle.clear().draw();
            axios.post('/Administrativo/ConversionPoliza/ObtenerPolizaDetalle', {
                year: year,
                mes: mes,
                tp: tp,
                poliza: poliza,
                empresa: empresa
            }).then(response => {
                if (response.data.success) {
                    dtPolizaDetalle.rows.add(response.data.Value).draw();
                    modalPolizaDetalle.modal('show');
                }
                else {
                    AlertaGeneral('Alerta', response.data.Message);
                }
            }).catch(error => AlertaGeneral('Alerta', error.statusText));
        }

        function obtenerPolizaDetallePeru(year, mes, tp, poliza, empresa) {
            dtPolizaDetalle.clear().draw();
            axios.post('/Administrativo/ConversionPoliza/obtenerPolizaDetallePeru', {
                year: year,
                mes: mes,
                tp: tp,
                poliza: poliza,
                empresa: empresa
            }).then(response => {
                if (response.data.success) {
                    dtPolizaDetalle.rows.add(response.data.Value).draw();
                    modalPolizaDetalle.modal('show');
                }
                else {
                    AlertaGeneral('Alerta', response.data.Message);
                }
            }).catch(error => AlertaGeneral('Alerta', error.statusText));
        }


        function reportePoliza(esResumen, esPorHoja, esFirma, poliza, esVisor) {
            if (!esVisor) { $.blockUI({ message: 'Procesando...' }); }
            var path = '/Reportes/Vista.aspx?' +
                'esDescargaVisor=' + esVisor +
                '&esVisor=' + true +
                '&idReporte=' + 202 +
                '&isResumen=' + esResumen +
                '&isCC=' + true +
                '&isPorHoja=' + esPorHoja +
                '&isFirma=' + esFirma +
                '&empresa=' + poliza.empresa +
                '&icc=' + '001' +
                '&fcc=' + 'S01' +
                '&iPol=' + poliza.poliza +
                '&fPol=' + poliza.poliza +
                '&iPer=' + moment(poliza.fecha).format('MM/YYYY') +
                '&fPer=' + moment(poliza.fecha).format('MM/YYYY') +
                '&iTp=' + poliza.tp +
                '&fTp=' + poliza.tp +
                '&firma1=' + ' ' +
                '&firma2=' + ' ';
            report.attr('src', path);
            document.getElementById('report').onload = function () {
                $.unblockUI();
                openCRModal();
            };
        }
        function initTblPolizaDetalle() {
            dtPolizaDetalle = tblPolizaDetalle.DataTable({
                ordering: false,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,
                scrollX: false,
                columns: [
                    { data: 'Linea', title: 'Línea' },
                    { data: 'Cuenta', title: 'Cuenta' },
                    { data: 'Subcuenta', title: 'Subcuenta' },
                    { data: 'SubSubcuenta', title: 'SubSubcuenta' },
                    { data: 'DescripcionCuenta', title: 'Cuenta' },
                    { data: 'CC', title: 'CC' },
                    { data: 'Concepto', title: 'Concepto' },
                    { data: 'iTipoMovimiento', title: 'Tipo Movimiento' },
                    { data: 'Monto', title: 'Cargo' },
                    { data: 'Monto', title: 'Abono' },
                ],
                columnDefs: [
                    {
                        targets: [0, 1, 2, 3, 5, 6, 7],
                        className: 'dt-body-center'
                    },
                    {
                        targets: [8],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            if (row.TipoMovimiento == TiposMovimiento.Cargo) {
                                return maskNumero(row.Monto);
                            }
                            else {
                                return maskNumero(0.0);
                            }
                        }
                    },
                    {
                        targets: [9],
                        className: 'dt-body-right',
                        render: function (data, type, row) {
                            if (row.TipoMovimiento == TiposMovimiento.Abono) {
                                return maskNumero(row.Monto);
                            }
                            else {
                                return maskNumero(0.0);
                            }
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) { },
                drawCallback: function (settings) { },
                headerCallback: function (thead, data, start, end, display) { },

                footerCallback: function (tfoot, data, start, end, display) {
                    let totalCargos = 0.0;
                    let totalAbonos = 0.0;

                    data.forEach(function (value, index, array) {
                        totalCargos += value.TipoMovimiento == TiposMovimiento.Cargo ? value.Monto : 0.0;
                        totalAbonos += value.TipoMovimiento == TiposMovimiento.Abono ? value.Monto : 0.0;
                    });

                    $(tfoot).find('th').eq(1).text(maskNumero(totalCargos));
                    $(tfoot).find('th').eq(2).text(maskNumero(totalAbonos));
                },

                initComplete: function (settings, json) { }
            });
        }
        function initDataTblNuevapoliza() {
            dtNuevapoliza = tblNuevaPoliza.DataTable({
                destroy: true,
                language: dtDicEsp,
                ordering: false,
                searching: true,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,
                scrollX: false,
                columns: [
                    { data: 'PalYear' },
                    { data: 'PalMes' },
                    { data: 'PalTP' },
                    { data: 'PalPoliza' },
                    { data: 'fechaPoliza' },
                    { data: 'PalCargo', class: 'text-right', render: data => maskNumero(data) },
                    { data: 'PalCargoUs', class: 'text-right', render: data => maskNumero(data) },
                    { data: 'SecCargo', class: 'text-right', render: data => maskNumero(data) },
                    { data: 'Estatus' },
                    {
                        data: 'Estatus', createdCell: (td, data) => {
                            if (data == "OK") {
                                let label = $(`<label>`),
                                    checkbox = $(`<input>`, {
                                        type: 'checkbox',
                                        class: 'inputCheckedEstatus'
                                        // checked: 'checked'
                                    });
                                label.append(checkbox);
                                $(td).html(label);
                            } else {
                                $(td).html("");
                            }
                        }
                    }
                ]
                , initComplete: function (settings, json) { }
            })
        }

        // function initDataTblNuevapolizaUs() {
        //     dtNuevapoliza = tblNuevaPoliza.DataTable({
        //         destroy: true,
        //         language: dtDicEsp,
        //         ordering: false,
        //         searching: true,
        //         info: false,
        //         language: dtDicEsp,
        //         paging: false,
        //         scrollY: '400px',
        //         scrollCollapse: true,
        //         scrollX: false,
        //         columns: [
        //             { data: 'PalYear' },
        //             { data: 'PalMes' },
        //             { data: 'PalTP' },
        //             { data: 'PalPoliza' },
        //             { data: 'fechaPoliza' },
        //             { data: 'PalCargo', class: 'text-right', render: data => maskNumero(data) },
        //             { data: 'PalCargoUs', class: 'text-right', render: data => maskNumero(data) },
        //             { data: 'SecCargo', class: 'text-right', render: data => maskNumero(data) },
        //             { data: 'Estatus' },
        //             {
        //                 data: 'Estatus', createdCell: (td, data) => {
        //                     if (data == "OK") {
        //                         let label = $(`<label>`),
        //                             checkbox = $(`<input>`, {
        //                                 type: 'checkbox',
        //                                 class: 'inputCheckedEstatus'
        //                                 // checked: 'checked'
        //                             });
        //                         label.append(checkbox);
        //                         $(td).html(label);
        //                     } else {
        //                         $(td).html("");
        //                     }
        //                 }
        //             }
        //         ]
        //         , initComplete: function (settings, json) { }
        //     })
        // }


        function initTablaPolizas() {
            dtConversionPolizas = tblConversionPolizas.DataTable({
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollY: '400px',
                scrollCollapse: true,
                scrollX: false,
                //dom: 'Bfrtip',
                columns: [
                    { data: 'PalYear' },
                    { data: 'PalMes' },
                    { data: 'PalTP' },
                    { data: 'PalPoliza' },
                    { data: 'PalCargo', class: 'text-right', render: data => maskNumero(data) },
                    {
                        render: function (data, type, row) {
                            let botonera = '';
                            if (row.PalEmpresa == 6) {
                                botonera += `<button class="btn btn-sm btn-light btn-success palVisorPeru" data-empresa="${row.PalEmpresa}" data-year="${row.PalYear}" data-mes="${row.PalMes}" data-tp="${row.PalTP}" data-poliza="${row.PalPoliza}">Poliza</button>`;
                                botonera += `<button class="btn btn-sm btn-light detallePeru" data-empresa="${row.PalEmpresa}" data-year="${row.PalYear}" data-mes="${row.PalMes}" data-tp="${row.PalTP}" data-poliza="${row.PalPoliza}">Movimientos</button>`;
                            } else {
                                botonera += `<button class="btn btn-sm btn-light btn-success palVisor" data-empresa="${row.PalEmpresa}" data-year="${row.PalYear}" data-mes="${row.PalMes}" data-tp="${row.PalTP}" data-poliza="${row.PalPoliza}">Poliza</button>`;
                                botonera += `<button class="btn btn-sm btn-light detalle" data-empresa="${row.PalEmpresa}" data-year="${row.PalYear}" data-mes="${row.PalMes}" data-tp="${row.PalTP}" data-poliza="${row.PalPoliza}">Movimientos</button>`;

                            }
                            return botonera;
                        }
                    },
                    { data: 'SecYear' },
                    { data: 'SecMes' },
                    { data: 'SecTP' },
                    { data: 'SecPoliza' },
                    { data: 'SecCargo', class: 'text-right', render: data => maskNumero(data) },
                    {
                        render: function (data, type, row) {
                            let botonera = ``;
                            botonera += `<button class="btn btn-sm btn-light btn-success secVisor" data-empresa="${row.SecEmpresa}" data-year="${row.SecYear}" data-mes="${row.SecMes}" data-tp="${row.SecTP}" data-poliza="${row.SecPoliza}">Poliza</button>`;
                            botonera += `<button class="btn btn-sm btn-light detalle" data-empresa="${row.SecEmpresa}" data-year="${row.SecYear}" data-mes="${row.SecMes}" data-tp="${row.SecTP}" data-poliza="${row.SecPoliza}">Movimientos</button>`;
                            return botonera;
                        }
                    },
                ],
                createdRow: function( row, data, dataIndex ) {
                    switch(data.estatusRegistro)
                    {
                        case 2:
                            $(row).addClass('modificada');
                            break;
                        case 3:
                            $(row).addClass('eliminada');
                            break;
                    }
                        
                },
                buttons: 
                [
                    {
                        extend: 'excel',
                        footer: true,
                        text: 'Descargar en excel',
                    },
                ],

                drawCallback: function () {
                    tblConversionPolizas.find('button.detalle').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();

                        let empresa = $(this).attr("data-empresa");
                        let year = $(this).attr("data-year");
                        let mes = $(this).attr("data-mes");
                        let tp = $(this).attr("data-tp");
                        let poliza = $(this).attr("data-poliza");
                        modalPolizaDetalle.find('.modal-title').text('Detalle póliza: ' + year + '-' + mes + '-' + tp + '-' + poliza);

                        obtenerPolizaDetalle(year, mes, tp, poliza, empresa);
                    });
                    tblConversionPolizas.find('button.detallePeru').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();

                        let empresa = $(this).attr("data-empresa");
                        let year = $(this).attr("data-year");
                        let mes = $(this).attr("data-mes");
                        let tp = $(this).attr("data-tp");
                        let poliza = $(this).attr("data-poliza");
                        modalPolizaDetalle.find('.modal-title').text('Detalle póliza: ' + year + '-' + mes + '-' + tp + '-' + poliza);

                        obtenerPolizaDetallePeru(year, mes, tp, poliza, empresa);
                    });
                    tblConversionPolizas.find('button.palVisor').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let row = $(this).closest('tr'),
                            data = dtConversionPolizas.row(row).data();
                        menuConfig.parametros = {
                            year: data.PalYear,
                            mes: data.PalMes,
                            tp: data.PalTP,
                            poliza: data.PalPoliza,
                            fecha: new Date(data.PalYear, data.PalMes - 1, 1),
                            empresa: data.PalEmpresa
                        };
                        mostrarMenu();
                    });
                    tblConversionPolizas.find('button.palVisorPeru').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let row = $(this).closest('tr'),
                            data = dtConversionPolizas.row(row).data();
                        menuConfig.parametros = {
                            year: data.PalYear,
                            mes: data.PalMes,
                            tp: data.PalTP,
                            poliza: data.PalPoliza,
                            fecha: new Date(data.PalYear, data.PalMes - 1, 1),
                            empresa: data.PalEmpresa
                        };
                        mostrarMenu();
                    });
                    tblConversionPolizas.find('button.secVisor').click(function (e) {
                        e.preventDefault();
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                        let row = $(this).closest('tr'),
                            data = dtConversionPolizas.row(row).data();
                        menuConfig.parametros = {
                            year: data.SecYear,
                            mes: data.SecMes,
                            tp: data.SecTP,
                            poliza: data.SecPoliza,
                            fecha: new Date(data.SecYear, data.SecMes - 1, 1),
                            empresa: data.SecEmpresa
                        };
                        mostrarMenu();
                    });
                },
            });
        }
    }
    $(document).ready(() => {
        Administrativo.Poliza.ConversionPoliza = new ConversionPoliza();
    });
})();