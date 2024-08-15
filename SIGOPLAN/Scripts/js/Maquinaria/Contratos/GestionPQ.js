(() => {
    $.namespace('Maquinaria.DocumentosPorPagar.GestionPQ');

    GestionPQ = function () {

        //#region variables (you know what I mean xD)
        const _version = 1;

        menuConfig = {
            lstOptions: [
                { text: `<i class="fa fa-download"></i> Descargar`, action: "descargar", fn: parametros => { downloadURI(parametros.idPq) } }
                , { text: `<i class="fa fa-file"></i> Visor`, action: "visor", fn: parametros => { setVisor(parametros.idPq) } }
            ]
        }

        const _tipoLinea = {
            ProvisionInteres: 1,
            CambioCC_MontoCCAnterior: 2,
            CambioCC_MontoCCNuevo: 3,
            Renovar_MontoNuevo: 4,
            PoderQuitarLinea: 5
        };

        let _registrosAcumulados = false;
        //#endregion

        //#region selectores
        //#region pantalla principal
        const cboxEstatus = $('#cboxEstatus');
        const divFechaCorte = $('#divFechaCorte');
        const inputFechaCorte = $('#inputFechaCorte');
        const btnRegistrarPQ = $('#btnRegistrarPQ');
        const tblPQs = $('#tblPQs');
        //#endregion

        //#region modalPQ
        const modalPQ = $('#modalPQ');
        const modalPQTitulo = $('#modalPQTitulo');

        const cboxBancoRegPQ = $('#cboxBancoRegPQ');
        const inputCtaCargoRegPQ = $('#inputCtaCargoRegPQ');
        const inputCtaAbonoRegPQ = $('#inputCtaAbonoRegPQ');
        const inputFechaFirmaRegPQ = $('#inputFechaFirmaRegPQ');
        const inputFechaVencimientoRegPQ = $('#inputFechaVencimientoRegPQ');
        const cboxCCRegPQ = $('#cboxCCRegPQ');
        const cboxMonedaRegPQ = $('#cboxMonedaRegPQ');
        const inputImporteRegPQ = $('#inputImporteRegPQ');
        const inputInteresRegPQ = $('#inputInteresRegPQ');
        const divTipoCambio = $('#divTipoCambio');
        const inputTipoCambioRegPQ = $('#inputTipoCambioRegPQ');
        const inputImporteMNRegPQ = $('#inputImporteMNRegPQ');
        const inputArchivoRegPQ = $('#inputArchivoRegPQ');

        const btnGuardarPQ = $('#btnGuardarPQ');
        //#endregion

        //#region modalPoliza
        const modalPoliza = $('#modalPoliza');
        const modalPolizaTitulo = $('#modalPolizaTitulo');

        const inputFechaPolizaPQ = $('#inputFechaPolizaPQ');
        const inputTipoCambioPolizaPQ = $('#inputTipoCambioPolizaPQ');
        const inputInteresPolizaPQ = $('#inputInteresPolizaPQ');

        const divRenovacion = $('#divRenovacion');
        const inputFechaFirmaPolizaPQ = $('#inputFechaFirmaPolizaPQ');
        const inputFechaVencimientoPolizaPQ = $('#inputFechaVencimientoPolizaPQ');
        const inputArchivoRenovacionPQ = $('#inputArchivoRenovacionPQ');

        const tblPoliza = $('#tblPoliza');

        const btnGuardarPoliza = $('#btnGuardarPoliza');
        //#endregion
        //#endregion

        //#region eventos
        //#region pantalla principal
        cboxEstatus.on('change', function () {
            if ($(this).val() == "true") {
                divFechaCorte.show();
            }
            else {
                divFechaCorte.hide();
            }
            getPQs();
        });

        inputFechaCorte.on('change', function () {
            getPQs();
        });

        btnRegistrarPQ.on('click', function () {
            modalPQAcciones('registrar', null, 'Registrar PQ');
        });
        //#endregion

        //#region modalPQ
        inputImporteRegPQ.on('change', function () {
            const importe = unmaskNumero($(this).val());
            const tipoCambio = unmaskNumero(inputTipoCambioRegPQ.val());

            $(this).val(maskNumero(unmaskNumero($(this).val())));

            if (cboxMonedaRegPQ.val() != '' && !cboxMonedaRegPQ.find('option:selected').data('prefijo')) {
                if (!isNaN(importe) && !isNaN(tipoCambio)) {
                    const valorImporteMN = importe * tipoCambio;
                    inputImporteMNRegPQ.val(maskNumero(valorImporteMN));
                }
            }
        });

        inputInteresRegPQ.on('change', function () {
            $(this).val(maskNumeroPorcentaje(unmaskNumero($(this).val())));
        });

        inputTipoCambioRegPQ.on('change', function () {
            const interesTipoCambio = unmaskNumero($(this).val());
            const importe = unmaskNumero(inputImporteRegPQ.val());

            $(this).val(maskNumeroXD(interesTipoCambio, 4));

            inputImporteMNRegPQ.val(maskNumero(interesTipoCambio * importe));
        });

        cboxMonedaRegPQ.on('change', function () {
            if ($(this).val() != '' && !$(this).find('option:selected').data('prefijo')) {
                divTipoCambio.show();
            }
            else {
                divTipoCambio.hide();
                inputTipoCambioRegPQ.val('');
                inputImporteMNRegPQ.val('');
            }
        });

        modalPQ.on('hidden.bs.modal', function () {
            limpiarRegistroPQ();

            if (_registrosAcumulados) {
                _registrosAcumulados = false;
                getPQs();
            }
        });

        btnGuardarPQ.on('click', function () {
            if (validarCamposRegistroPQ()) {
                const idPQ = $(this).data('id');

                if (idPQ) {
                    let textoMensaje = '';
                    const objPQ = crearObjetoPQ(idPQ);
                    const pq = JSON.parse(objPQ.get('pq'));

                    switch ($(this).data('accion')) {
                        case 'modificar':
                            textoMensaje = 'Se actualizara el PQ, ¿Desea continuar?';
                            pq.estatus = true;
                            break;
                        case 'eliminar':
                            pq.estatus = false;
                            textoMensaje = 'Se eliminara el PQ, ¿Desea continuar?';
                            break;
                    }
                    swal({
                        title: 'Alerta!',
                        text: textoMensaje,
                        icon: "warning",
                        buttons: true,
                        dangerMode: true,
                        buttons: ["Cerrar", "Aceptar"]
                    })
                        .then((aceptar) => {
                            if (aceptar) {
                                objPQ.set('pq', pq);
                                guardarPQ(objPQ);
                            }
                        });
                }
                else {
                    guardarPQ(crearObjetoPQ());
                }
            }
        });
        //#endregion
        //#region modalPoliza
        modalPoliza.on('shown.bs.modal', function () {
            tblPoliza.DataTable().columns.adjust().draw();
        });

        inputFechaPolizaPQ.on('change', function () {
            const nuevaFechaCorte = moment($(this).val(), 'DD/MM/YYYY');

            if (btnGuardarPoliza.data('accion') == 'renovar') {

                tblPoliza.DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {

                    if (this.data().tipoLinea == _tipoLinea.ProvisionInteres) {

                        const fechaFirma = moment(this.data().fechaFirma).format('DD/MM/YYYY');

                        let diasDiferencia = moment(nuevaFechaCorte).diff(moment(fechaFirma, 'DD/MM/YYYY'), 'days') + 1;

                        this.data().monto = this.data().interesDiario * diasDiferencia;
                        $(this.node()).find('.montoPoliza').val(maskNumero(this.data().monto));
                        $(this.node()).find('.montoPoliza').trigger('change');
                    }
                });
            }

            getTipoCambioFecha();
        });

        inputInteresPolizaPQ.on('change', function () {
            $(this).val(maskNumeroPorcentaje(unmaskNumero($(this).val())));

            var rowData = tblPoliza.DataTable().rows().data();

            for (let index = 2; index < 4; index++) {
                let conceptoRenovacion = rowData[index].concepto;

                if (conceptoRenovacion.includes('%')) {
                    let lastIndexSpace = conceptoRenovacion.lastIndexOf(' ');

                    conceptoRenovacion = conceptoRenovacion.substring(0, lastIndexSpace);
                }
                conceptoRenovacion += ' ' + $(this).val();
                rowData[index].concepto = conceptoRenovacion;

                tblPoliza.find('tbody').find(`tr:eq(${index})`).find('.conceptoPoliza').val(conceptoRenovacion);
            }
        });

        btnGuardarPoliza.on('click', function () {
            switch ($(this).data('accion')) {
                case 'liquidar':
                    liquidar($(this).data('id'), inputFechaPolizaPQ.val(), crearObjetoPoliza($(this)));
                    break;
                case 'cambiarCC':
                    cambiarCC($(this).data('id'), inputFechaPolizaPQ.val(), crearObjetoPoliza($(this)));
                    break;
                case 'renovar':
                    renovarPQ($(this).data('id'), inputFechaPolizaPQ.val(), crearObjetoPoliza($(this)));
                    break;
                case 'abonar':
                    abonarPQ($(this).data('id'), inputFechaPolizaPQ.val(), crearObjetoPoliza($(this)));
                    break;
            }
        });
        //#endregion
        //#endregion

        //#region fechas
        function initInputsFechas() {
            inputFechaCorte.datepicker().datepicker('setDate', new Date());
            inputFechaFirmaRegPQ.datepicker().datepicker('setDate', new Date());
            inputFechaVencimientoRegPQ.datepicker().datepicker('setDate', new Date());
            inputFechaPolizaPQ.datepicker().datepicker();
            inputFechaFirmaPolizaPQ.datepicker().datepicker('setDate', new Date());
            inputFechaVencimientoPolizaPQ.datepicker().datepicker('setDate', new Date());
        }
        //#endregion

        //#region datatables
        function initTblPQ() {
            tblPQs.DataTable({
                order: [[0, 'asc']],
                ordering: true,
                searching: true,
                info: true,
                language: dtDicEsp,
                paging: true,
                scrollX: false,
                scrollY: '45vh',
                scrollCollapse: true,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],
                buttons: [{
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 16]
                    }
                }],
                dom: 'Bfrtip',
                columns: [
                    { data: 'folioInterno', title: 'FOLIO' },
                    { data: 'banco', title: 'BANCO' },
                    { data: 'fechaFirma', title: 'FECHA FIRMA', className: 'text-center' },
                    { data: 'fechaVencimiento', title: 'FECHA VENCI<br>MIENTO', className: 'text-center' },
                    { data: 'cc', title: 'CC', className: 'text-center' },
                    { data: 'moneda', title: 'MO<br>NEDA', className: 'text-center' },
                    { data: 'importe', title: 'IMPORTE', className: 'text-right' },
                    { data: 'importeMN', title: 'IMPORTE EN MN', className: 'text-right' },
                    { data: 'interes', title: '% INT', className: 'text-right' },
                    { data: 'interesDiario', title: 'INT DIARIO', className: 'text-right' },
                    { data: 'interesSemanal', title: 'INT SEMANAL', className: 'text-right' },
                    { data: 'fechaCorte', title: 'FECHA DE CORTE P/INT', className: 'text-center' },
                    { data: 'interesAcumulado', title: 'INT ACUM', className: 'text-right' },
                    { data: 'tieneAbono', title: 'ABO<br>NO', className: 'text-center' },
                    { data: null, title: 'OPCIONES', className: 'text-center' },
                    { data: 'fechaLiquidacion', title: 'FECHA LIQUIDACIÓN', className: 'text-center' },
                    { data: 'poliza', title: 'PÓLIZA', className: 'text-center', visible: false }
                ],

                columnDefs: [
                    {
                        targets: [0, 1, 14],
                        className: 'text-nowrap'
                    },
                    {
                        targets: [2, 3],
                        render: function (data, type, row) {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    },
                    {
                        targets: [11],
                        render: function (data, type, row) {
                            return `<input type="text" class="form-control inputFechaCortePQ" value="${moment(data).format('DD/MM/YYYY')}" autocomplete="off" /> `;
                        },
                        width: '90px'
                    },
                    {
                        targets: [6, 7, 9, 10, 12],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    },
                    {
                        targets: [8],
                        render: function (data, type, row) {
                            return maskNumeroPorcentaje(data);
                        }
                    },
                    {
                        targets: [13],
                        render: function (data, type, row) {
                            if (data) {
                                return '<i class="far fa-check-circle"></i>';
                            } else {
                                return '';
                            }
                        }
                    },
                    {
                        targets: [14],
                        render: function (data, type, row) {
                            const btnOpcionRenovar = '<button class="btn btn-sm btn-success btnOpcionRenovarPQ" title="Renovar PQ"><i class="fas fa-sync-alt"></i></button>';
                            const btnOpcionEliminar = '<button class="btn btn-sm btn-danger btnOpcionEliminarPQ" title="Eliminar PQ"><i class="fas fa-trash"></i></button>';
                            const btnOpcionCambiarCC = '<button class="btn btn-sm btn-warning btnOpcionCambiarCCPQ" title="Cambiar CC"><i class="fas fa-exchange-alt"></i></button>';
                            const btnOpcionAbono = '<button class="btn btn-sm btn-default btnOpcionAbonarPQ" title="Abonar"><i class="fas fa-money-check-alt"></i></button>';
                            const btnOpcionModificar = '<button class="btn btn-sm btn-primary btnOpcionModificarPQ" title="Modificar"><i class="far fa-edit"></i></button>';
                            const btnOpcionLiquidar = '<button class="btn btn-sm btn-primary btnOpcionLiquidarPQ" title="Liquidar"><i class="fas fa-file-invoice-dollar"></i></button>';
                            const btnVisor = '<button class="btn btn-sm btn-info btnVisor" title="Archivo"><i class="far fa-file-pdf"></i></button>';

                            return btnVisor + ' ' + btnOpcionRenovar + ' ' + btnOpcionCambiarCC + ' ' + btnOpcionAbono + ' ' + btnOpcionLiquidar;
                        },
                        width: '300px'
                    },
                    {
                        targets: [15],
                        render: function (data, type, row) {
                            if (data == null) {
                                return '';
                            } else {
                                return moment(data).format('DD/MM/YYYY');
                            }
                        }
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                    $(row).find('.inputFechaCortePQ').datepicker();
                },
                drawCallback: function (settings) {
                    tblPQs.DataTable().column(9).visible(cboxEstatus.val() == "true");
                    tblPQs.DataTable().column(11).visible(cboxEstatus.val() == "true");
                    tblPQs.DataTable().column(12).visible(cboxEstatus.val() == "true");
                    tblPQs.DataTable().column(14).visible(cboxEstatus.val() == "true");
                    tblPQs.DataTable().column(15).visible(cboxEstatus.val() == "false");

                    let folio = 0;
                    tblPQs.DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
                        if (folio == 0) {
                            folio = this.data().folio;
                        }
                        else {
                            if (folio != this.data().folio) {
                                folio = this.data().folio;
                                const tr = $(this.node()).prev('tr');
                                let trTbl = tblPQs.DataTable().row(tr);

                                trTbl.child('').show();
                                trTbl.child().addClass('rowSeparador');
                            }
                        }
                    });

                    tblPQs.DataTable().columns().every(function (colIdx, tableLpp, rowLoop) {
                        $(this.footer()).html('');

                        let totalImporteMN = 0;
                        let totalInteresDiario = 0;
                        let totalInteresAcumulado = 0;

                        switch (colIdx) {
                            case 7:
                                for (let x = 0; x < this.data().length; x++) {
                                    totalImporteMN += this.data()[x];
                                }
                                $(this.footer()).html(maskNumero(totalImporteMN));
                                break;
                            case 9:
                                for (let x = 0; x < this.data().length; x++) {
                                    totalImporteMN += this.data()[x];
                                }
                                $(this.footer()).html(maskNumero(totalImporteMN));
                                break;
                            case 12:
                                for (let x = 0; x < this.data().length; x++) {
                                    totalImporteMN += this.data()[x];
                                }
                                $(this.footer()).html(maskNumero(totalImporteMN));
                                break;
                        }
                    });
                },
                initComplete: function (settings, json) {
                    tblPQs.on('change', '.inputFechaCortePQ', function () {
                        const rowData = tblPQs.DataTable().row($(this).closest('tr')).data();

                        const nuevaFechaCorteContrato = moment($(this).val(), 'DD/MM/YYYY');
                        const fechaInicioContrato = moment(rowData.fechaFirma).format('DD/MM/YYYY');

                        let diasDiferencia = moment(nuevaFechaCorteContrato).diff(moment(fechaInicioContrato, 'DD/MM/YYYY'), 'days') + 1;

                        diasDiferencia = diasDiferencia < 0 ? 0 : diasDiferencia;

                        tblPQs.DataTable().row($(this).closest('tr')).data().fechaCorte = $(this).val();
                        tblPQs.DataTable().row($(this).closest('tr')).data().interesAcumulado = diasDiferencia * rowData.interesDiario;

                        $(this).closest('tr').find('td').eq(10).text(maskNumero(diasDiferencia * rowData.interesDiario));
                    });

                    tblPQs.on('click', '.btnOpcionEliminarPQ', function () {
                        const rowData = tblPQs.DataTable().row($(this).closest('tr')).data();

                        const tituloModal = 'Eliminar PQ - ' + rowData.banco + ' / ' + rowData.cc;

                        getPQ(rowData.id).done(function (response) {
                            if (response && response.success) {
                                modalPQAcciones('eliminar', response.items, tituloModal);
                            }
                        });
                    });

                    tblPQs.on('click', '.btnOpcionModificarPQ', function () {
                        const rowData = tblPQs.DataTable().row($(this).closest('tr')).data();

                        const tituloModal = 'Modificar PQ - ' + rowData.banco + ' / ' + rowData.cc;

                        getPQ(rowData.id).done(function (response) {
                            if (response && response.success) {
                                modalPQAcciones('modificar', response.items, tituloModal);
                            }
                        });
                    });

                    tblPQs.on('click', '.btnOpcionRenovarPQ', function () {
                        const rowData = tblPQs.DataTable().row($(this).closest('tr')).data();

                        btnGuardarPoliza.data('id', 0);
                        btnGuardarPoliza.data('accion', 'renovar');

                        getPQRenovar(rowData.id).done(function (response) {
                            if (response && response.success) {
                                divRenovacion.show();

                                inputArchivoRenovacionPQ.val('');

                                AddRows(tblPoliza, response.items);

                                btnGuardarPoliza.data('id', rowData.id);

                                const tituloModal = 'Renovar PQ - ' + rowData.banco + ' / ' + rowData.cc;
                                modalPolizaTitulo.text(tituloModal);
                                modalPoliza.modal('show');
                            }
                        });
                    });

                    tblPQs.on('click', '.btnOpcionCambiarCCPQ', function () {
                        const rowData = tblPQs.DataTable().row($(this).closest('tr')).data();

                        btnGuardarPoliza.data('id', 0);
                        btnGuardarPoliza.data('accion', 'cambiarCC');

                        getPQCambiarCC(rowData.id).done(function (response) {
                            if (response && response.success) {
                                divRenovacion.hide();

                                AddRows(tblPoliza, response.items);

                                btnGuardarPoliza.data('id', rowData.id);

                                const tituloModal = 'Cambiar CC PQ - ' + rowData.banco + ' / ' + rowData.cc;
                                modalPolizaTitulo.text(tituloModal);
                                modalPoliza.modal('show');
                            }
                        });
                    });

                    tblPQs.on('click', '.btnOpcionAbonarPQ', function () {
                        const rowData = tblPQs.DataTable().row($(this).closest('tr')).data();

                        btnGuardarPoliza.data('id', 0);
                        btnGuardarPoliza.data('accion', 'abonar');

                        getPQAbono(rowData.id).done(function (response) {
                            if (response && response.success) {
                                divRenovacion.hide();

                                AddRows(tblPoliza, response.items);

                                btnGuardarPoliza.data('id', rowData.id);

                                const tituloModal = 'Abonar - ' + rowData.banco + ' / ' + rowData.cc;
                                modalPolizaTitulo.text(tituloModal);
                                modalPoliza.modal('show');
                            }
                        });
                    });

                    tblPQs.on('click', '.btnOpcionLiquidarPQ', function () {
                        const rowData = tblPQs.DataTable().row($(this).closest('tr')).data();

                        btnGuardarPoliza.data('id', 0);
                        btnGuardarPoliza.data('accion', 'liquidar');

                        getPQLiquidar(rowData.id).done(function (response) {
                            if (response && response.success) {
                                divRenovacion.hide();

                                AddRows(tblPoliza, response.items);

                                btnGuardarPoliza.data('id', rowData.id);

                                const tituloModal = 'Liquidar PQ - ' + rowData.banco + ' / ' + rowData.cc;
                                modalPolizaTitulo.text(tituloModal);
                                modalPoliza.modal('show');
                            }
                        });
                    });

                    tblPQs.on('click', '.btnVisor', function () {
                        const rowData = tblPQs.DataTable().row($(this).closest('tr')).data();

                        menuConfig.parametros = {
                            idPq: rowData.id
                        }

                        mostrarMenu();
                    });
                },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },
                footerCallback: function (tfoot, data, start, end, display) { }
            });
        }

        function initTblPoliza() {
            tblPoliza.DataTable({
                order: [[0, 'asc']],
                ordering: false,
                searching: false,
                info: false,
                language: dtDicEsp,
                paging: false,
                scrollX: false,
                scrollY: false,
                scrollCollapse: false,
                lengthMenu: [[-1, 10, 25, 50], ['Todos', 10, 25, 50]],

                columns: [
                    { data: 'cuenta', title: 'CTA', className: 'text-center' },
                    { data: 'tm', title: 'TM', className: 'text-center' },
                    { data: 'referencia', title: 'REFERENCIA', className: 'text-center' },
                    { data: 'cc', title: 'CC', className: 'text-right' },
                    { data: 'concepto', title: 'CONCEPTO', className: 'text-left' },
                    { data: 'monto', title: 'MONTO', className: 'text-right' },
                    { data: 'itm', title: 'ITM', className: 'text-center' },
                    { data: null, title: 'OPCIONES', className: 'text-center' }
                ],

                columnDefs: [
                    {
                        targets: '_all',
                        className: 'text-nowrap'
                    },
                    {
                        targets: [0],
                        render: function (data, type, row) {
                            const inputCta = '<input type="text" class="form-control ctaPoliza" />';

                            return inputCta;
                        },
                        width: '250px'
                    },
                    {
                        targets: [1],
                        render: function (data, type, row) {
                            const inputTM = '<input type="text" class="form-control tmPoliza" value="' + data + '" />';

                            return inputTM;
                        },
                        width: '40px'
                    },
                    {
                        targets: [2],
                        render: function (data, type, row) {
                            const inputReferencia = '<input type="text" class="form-control referenciaPoliza" value="' + data + '" />';

                            return inputReferencia;
                        },
                        width: '75px'
                    },
                    {
                        targets: [3],
                        render: function (data, type, row) {
                            const inputCC = '<select class="form-control ccPoliza" value=""></select>';

                            return inputCC;
                        },
                        width: '250px'
                    },
                    {
                        targets: [4],
                        render: function (data, type, row) {
                            const inputConcepto = '<input type="text" class="form-control conceptoPoliza" value ="' + data + '" />';

                            return inputConcepto;
                        }
                    },
                    {
                        targets: [5],
                        render: function (data, type, row) {
                            const inputMonto = '<input type="text" class="form-control montoPoliza" value ="' + maskNumero(data) + '" />';

                            return inputMonto;
                        }
                    },
                    {
                        targets: [6],
                        render: function (data, type, row) {
                            const inputITM = '<input type="text" class="form-control itmPoliza" value ="' + data + '" />';

                            return inputITM;
                        },
                        width: '30px'
                    },
                    {
                        targets: [7],
                        render: function (data, type, row) {
                            const btnEliminarLinea = '<button class="btn btn-danger btnOpcionEliminarLineaPoliza" title="Eliminar línea"><i class="fas fa-times-circle"></i></button>';

                            if (row.tipoLinea != null && row.tipoLinea == _tipoLinea.PoderQuitarLinea) {
                                return btnEliminarLinea;
                            }
                            else {
                                return '';
                            }
                        },
                        width: '30px'
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                    $(row).find('.ctaPoliza').getAutocomplete(selectAutocompletePoliza, null, '/Contratos/GetCuenta');
                    $(row).find('.ctaPoliza').text(data.cuentaDescripcion);
                    $(row).find('.ctaPoliza').attr('data-index', data.cuenta);
                    $(row).find('.ctaPoliza').val(data.cuentaDescripcion);

                    let ccPoliza = $(row).find('.ccPoliza');
                    ccPoliza.select2({ dropdownParent: $(modalPoliza) });
                    cboxCCRegPQ.find('option').each(function (value, index, array) {
                        $(this).clone().appendTo(ccPoliza);
                    });
                    ccPoliza.val(data.cc);
                    ccPoliza.trigger('change');
                },
                drawCallback: function (settings) { },
                initComplete: function (settings, json) {
                    tblPoliza.on('change', '.ctaPoliza', function () {
                        const rowData = tblPoliza.DataTable().row($(this).closest('tr')).data();

                        let ctaNueva = $(this).data('index');

                        tblPoliza.DataTable().row($(this).closest('tr')).data().cuenta = ctaNueva;
                        tblPoliza.DataTable().row($(this).closest('tr')).data().cuentaDescripcion = $(this).val();
                    });

                    tblPoliza.on('change', '.tmPoliza', function () {
                        const rowData = tblPoliza.DataTable().row($(this).closest('tr')).data();

                        let tmNuevo = $(this).val();

                        tblPoliza.DataTable().row($(this).closest('tr')).data().tm = tmNuevo;
                    });

                    tblPoliza.on('change', '.referenciaPoliza', function () {
                        const rowData = tblPoliza.DataTable().row($(this).closest('tr')).data();

                        let referenciaNueva = $(this).val();

                        tblPoliza.DataTable().row($(this).closest('tr')).data().referencia = referenciaNueva;
                    });

                    tblPoliza.on('change', '.ccPoliza', function () {
                        const rowData = tblPoliza.DataTable().row($(this).closest('tr')).data();

                        let ccNuevo = $(this).val();

                        tblPoliza.DataTable().row($(this).closest('tr')).data().cc = ccNuevo;
                    });

                    tblPoliza.on('change', '.conceptoPoliza', function () {
                        const rowData = tblPoliza.DataTable().row($(this).closest('tr')).data();

                        let conceptoNuevo = $(this).val();

                        tblPoliza.DataTable().row($(this).closest('tr')).data().concepto = conceptoNuevo;
                    });

                    tblPoliza.on('change', '.montoPoliza', function () {
                        const rowData = tblPoliza.DataTable().row($(this).closest('tr')).data();

                        let montoNuevo = unmaskNumero($(this).val());

                        tblPoliza.DataTable().row($(this).closest('tr')).data().monto = montoNuevo;

                        $(this).val(maskNumero(montoNuevo));
                    });

                    tblPoliza.on('change', '.itmPoliza', function () {
                        const rowData = tblPoliza.DataTable().row($(this).closest('tr')).data();

                        let itmNuevo = $(this).val();

                        tblPoliza.DataTable().row($(this).closest('tr')).data().itm = itmNuevo;
                    });

                    tblPoliza.on('click', '.btnOpcionEliminarLineaPoliza', function () {
                        tblPoliza.DataTable().row($(this).closest('tr')).remove().draw(true);
                    });
                },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).parent().children().addClass('dtHeader');
                    $(thead).children().addClass('text-center');
                },
                footerCallback: function (tfoot, data, start, end, display) { }
            });
        }

        function AddRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }
        //#endregion

        //#region generales
        function validarCamposRegistroPQ() {
            let validacion = true;

            if (!validarCampo(cboxBancoRegPQ)) { validacion = false; }
            if (!validarCampo(inputCtaCargoRegPQ)) { validacion = false; }
            if (!validarCampo(inputCtaAbonoRegPQ)) { validacion = false; }
            if (!validarCampo(inputFechaFirmaRegPQ)) { validacion = false; }
            if (!validarCampo(inputFechaVencimientoRegPQ)) { validacion = false; }
            if (!validarCampo(cboxCCRegPQ)) { validacion = false; }
            if (!validarCampo(inputImporteRegPQ)) { validacion = false; }
            if (!validarCampo(inputInteresRegPQ)) { validacion = false; }
            if (!validarCampo(inputArchivoRegPQ)) { validacion = false; }

            if (!validarCampo(cboxMonedaRegPQ)) {
                validacion = false;
            }
            else {
                if (cboxMonedaRegPQ.val() != '' && !cboxMonedaRegPQ.find('option:selected').data('prefijo')) {
                    if (!validarCampo(inputTipoCambioRegPQ)) { validacion = false; }
                }
                else {
                    inputTipoCambioRegPQ.removeClass('errorClass');
                }
            }

            return validacion;
        }

        function crearObjetoPQ(id) {
            const relCuenta = inputCtaAbonoRegPQ.val().split('-');
            const relCuentaCargo = inputCtaCargoRegPQ.val().split('-');

            const archivoContrato = inputArchivoRegPQ.get(0).files[0];

            const objPQ = {
                id: id ?? 0,
                bancoId: cboxBancoRegPQ.val(),
                ctaAbonoBanco: relCuenta[0],
                sctaAbonoBanco: relCuenta[1],
                ssctaAbonoBanco: relCuenta[2],
                digitoAbonoBanco: relCuenta[3],
                ctaCargoBanco: relCuentaCargo[0],
                sctaCargoBanco: relCuentaCargo[1],
                ssctaCargoBanco: relCuentaCargo[2],
                digitoCargoBanco: relCuentaCargo[3],
                fechaFirma: inputFechaFirmaRegPQ.val(),
                fechaVencimiento: inputFechaVencimientoRegPQ.val(),
                cc: cboxCCRegPQ.val(),
                monedaId: cboxMonedaRegPQ.val(),
                importe: unmaskNumero(inputImporteRegPQ.val()),
                interes: unmaskNumero(inputInteresRegPQ.val()),
                tipoCambio: !cboxMonedaRegPQ.find('option:selected').data('prefijo') ? unmaskNumero(inputTipoCambioRegPQ.val()) : 1
            }

            let formData = new FormData();
            formData.set('archivo', archivoContrato);
            formData.set('pq', JSON.stringify(objPQ));

            return formData;
        }

        function limpiarRegistroPQ() {
            cboxBancoRegPQ.val('');
            inputCtaAbonoRegPQ.val('');
            inputCtaAbonoRegPQ.trigger('change');
            inputCtaCargoRegPQ.val('');
            inputCtaCargoRegPQ.trigger('change');
            inputFechaFirmaRegPQ.val('');
            inputFechaVencimientoRegPQ.val('');
            cboxCCRegPQ.val('');
            cboxCCRegPQ.trigger('change');
            inputImporteRegPQ.val('');
            inputInteresRegPQ.val('');
            cboxMonedaRegPQ.val('');
            cboxMonedaRegPQ.trigger('change');
            inputTipoCambioRegPQ.val('');
            inputImporteMNRegPQ.val('');
            inputArchivoRegPQ.val('');
        }

        function llenarFormPQ(pq) {
            cboxBancoRegPQ.val(pq.bancoId);
            inputCtaAbonoRegPQ.val(pq.ctaRelacion);
            inputCtaCargoRegPQ.val(pq.ctaCargoRelacion);
            inputFechaFirmaRegPQ.val(moment(pq.fechaFirma).format('DD/MM/YYYY'));
            inputFechaVencimientoRegPQ.val(moment(pq.fechaVencimiento).format('DD/MM/YYYY'));
            cboxCCRegPQ.val(pq.cc);
            cboxCCRegPQ.trigger('change');
            inputImporteRegPQ.val(maskNumero(pq.importe));
            inputInteresRegPQ.val(maskNumeroPorcentaje(pq.interes));
            cboxMonedaRegPQ.val(pq.monedaId);
            cboxMonedaRegPQ.trigger('change');
            inputTipoCambioRegPQ.val(pq.tipoCambio != null ? maskNumero(pq.tipoCambio) : '');
            inputImporteMNRegPQ.val(maskNumero(pq.importeMN));
        }

        function desHabilitarFormPQ(estatus) {
            modalPQ.find('form').eq(0).find('input, select').each(function (index, element) {
                $(this).prop('disabled', estatus);
            });

            inputImporteMNRegPQ.prop('disabled', true);
        }

        function modalPQAcciones(accion, pq, modalTitulo) {
            btnGuardarPQ.removeClass('btn-primary');
            btnGuardarPQ.removeClass('btn-danger');
            btnGuardarPQ.removeClass('btn-success');
            btnGuardarPQ.removeClass('btn-warning');

            switch (accion) {
                case 'registrar':
                    desHabilitarFormPQ(false);
                    btnGuardarPQ.data('id', null);
                    btnGuardarPQ.data('accion', null);
                    btnGuardarPQ.addClass('btn-success');
                    btnGuardarPQ.html('<i class="fas fa-plus"></i> Guardar');
                    break;
                case 'modificar':
                    llenarFormPQ(pq);
                    desHabilitarFormPQ(false);
                    btnGuardarPQ.data('id', pq.id);
                    btnGuardarPQ.data('accion', 'modificar');
                    btnGuardarPQ.addClass('btn-warning');
                    btnGuardarPQ.html('<i class="far fa-edit"></i> Actualizar');
                    break;
                case 'eliminar':
                    llenarFormPQ(pq);
                    desHabilitarFormPQ(true);
                    btnGuardarPQ.data('id', pq.id);
                    btnGuardarPQ.data('accion', 'eliminar');
                    btnGuardarPQ.addClass('btn-danger');
                    btnGuardarPQ.html('<i class="fas fa-trash"></i> Eliminar');
                    break;
            }

            modalPQTitulo.text(modalTitulo);
            modalPQ.modal('show');
        }

        function selectAutocompletePoliza(event, ui) {
            $(this).text(ui.item.label);
            $(this).attr('data-index', ui.item.id);
        }

        function crearObjetoPoliza(btnAccion) {
            const data = tblPoliza.DataTable().rows().data();

            let objetoPoliza = new Array();

            $(data).each(function (index, element, array) {
                objetoPoliza.push(element);
            });

            return objetoPoliza;
        }
        //#endregion

        //#region servidor
        function getVersion() {
            return $.get('/Contratos/GetVersionPQ');
        }

        function getBancos() {
            cboxBancoRegPQ.fillCombo('/Contratos/ObtenerInstitucionesPQ', null, false, null);
        }

        function getCCs() {
            cboxCCRegPQ.fillCombo('/Contratos/GetCCs', null, false, null);
            cboxCCRegPQ.select2({ dropdownParent: $(modalPQ) });
        }

        function getMonedas() {
            cboxMonedaRegPQ.fillCombo('/Contratos/GetMonedas', null, false, null);
        }

        function initCatCuentas() {
            inputCtaAbonoRegPQ.fillCombo('/Contratos/GetCuentaAbono', null, false, null);
            inputCtaAbonoRegPQ.select2({ dropdownParent: $(modalPQ) });

            inputCtaCargoRegPQ.fillCombo('/Contratos/GetCuentaCargo', null, false, null);
            inputCtaCargoRegPQ.select2({ dropdownParent: $(modalPQ) });
        }

        function guardarPQ(objetoPQ) {
            let pq = JSON.parse(objetoPQ.get('pq'));
            $.ajax({
                type: 'POST',
                url: '/Contratos/GuardarPQ',
                data: objetoPQ,
                contentType: false,
                processData: false,
                cache: false,
                success: function (response) {
                    if (response.success) {
                        _registrosAcumulados = true;

                        swal({
                            title: 'Confirmación!',
                            text: response.message + (pq.id == 0 ? ' ¿Desea agregar un nuevo PQ?' : ''),
                            icon: 'success',
                            buttons: true,
                            dangerMode: false,
                            buttons: ['Cerrar', 'Aceptar']
                        })
                            .then((aceptar) => {
                                limpiarRegistroPQ();
                                if (aceptar) {
                                    if (pq.id != 0) {
                                        modalPQ.modal('hide');
                                        getPQs();
                                    }
                                }
                                else {
                                    modalPQ.modal('hide');
                                    getPQs();
                                }
                            }
                            );
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                },
                error: function (xhr, status, error) {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                }
            });
        }

        function getPQs() {
            $.get('/Contratos/GetPQs',
                {
                    estatus: cboxEstatus.val(),
                    fechaCorte: inputFechaCorte.val()
                }).then(response => {
                    if (response.success) {
                        AddRows(tblPQs, response.items);

                        const fechaCorteActual = inputFechaCorte.val();

                        tblPQs.DataTable().rows().every(function (rowIdx, tableLoop, rowLoop) {
                            this.data().fechaCorte = fechaCorteActual;
                            $(this.node()).find('.inputFechaCortePQ').val(fechaCorteActual);
                            $(this.node()).find('.inputFechaCortePQ').trigger('change');
                        });
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getPQ(id) {
            return $.get('/Contratos/GetPQ', { id }).then(response => {
                if (response.success) {
                    return response;
                }
                else {
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
            });
        }

        function getPQLiquidar(id) {
            return $.get('/Contratos/GetPQLiquidar',
                {
                    id
                }).then(response => {
                    if (response.success) {
                        return response;
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getPQCambiarCC(id) {
            return $.get('/Contratos/GetPQCambiarCC',
                {
                    id
                }).then(response => {
                    if (response.success) {
                        return response;
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getPQAbono(id) {
            return $.get('/Contratos/GetPQAbono',
                {
                    id
                }).then(response => {
                    if (response.success) {
                        return response;
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getPQRenovar(id) {
            return $.get('/Contratos/GetPQRenovar',
                {
                    id
                }).then(response => {
                    if (response.success) {
                        return response;
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function getFechaServidor() {
            $.get('/Contratos/GetFechaServidor').then(response => {
                inputFechaPolizaPQ.val(response);
            }, error => {
                swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
            });
        }

        function getTipoCambioFecha() {
            $.get('/Contratos/GetTipoCambioFecha', {
                fecha: inputFechaPolizaPQ.val()
            }).then(response => {
                if (response.success) {
                    if (response.tipoCambio != null && response.tipoCambio.moneda == 2 && response.tipoCambio.tipo_cambio > 0) {
                        inputTipoCambioPolizaPQ.val(maskNumero(response.tipoCambio.tipo_cambio));
                    } else {
                        inputTipoCambioPolizaPQ.val('$0.00');
                        swal('Alerta!', 'No se encontró un tipo de cambio', 'warning');
                    }
                } else {
                    inputTipoCambioPolizaPQ.val('$0.00');
                    swal('Alerta!', response.message, 'warning');
                }
            }, error => {
                inputTipoCambioPolizaPQ.val('$0.00');
                swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
            });
        }

        function liquidar(pqId, fechaMovimiento, objetoPoliza, tipoCambio) {
            $.post('/Contratos/Liquidar',
                {
                    idPq: pqId,
                    fechaMovimiento: moment(fechaMovimiento, 'DD/MM/YYYY').toISOString(true),
                    infoPol: objetoPoliza
                }).then(response => {
                    if (response.success) {
                        inputTipoCambioPolizaPQ.val('');

                        tblPoliza.DataTable().clear().draw();
                        getPQs();
                        modalPoliza.modal('hide');

                        swal('Confirmación', response.message, 'success');
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function cambiarCC(pqId, fechaMovimiento, objetoPoliza, tipoCambio) {
            $.post('/Contratos/CambiarCC',
                {
                    idPq: pqId,
                    fechaMovimiento: moment(fechaMovimiento, 'DD/MM/YYYY').toISOString(true),
                    infoPol: objetoPoliza
                }).then(response => {
                    if (response.success) {
                        inputTipoCambioPolizaPQ.val('');

                        tblPoliza.DataTable().clear().draw();
                        getPQs();
                        modalPoliza.modal('hide');

                        swal('Confirmación', response.message, 'success');
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function renovarPQ(pqId, fechaMovimiento, objetoPoliza) {

            const archivo = inputArchivoRenovacionPQ.get(0).files[0];

            let formData = new FormData();

            formData.set('idPq', pqId);
            formData.set('fechaMovimiento', moment(fechaMovimiento, 'DD/MM/YYYY').toISOString(true));
            formData.set('infoPol', JSON.stringify(objetoPoliza));
            formData.set('fechaFirma', moment(inputFechaFirmaPolizaPQ.val(), 'DD/MM/YYYY').toISOString(true));
            formData.set('fechaVencimiento', moment(inputFechaVencimientoPolizaPQ.val(), 'DD/MM/YYYY').toISOString(true));
            formData.set('interes', unmaskNumero(inputInteresPolizaPQ.val()));
            formData.set('archivo', archivo);

            $.ajax({
                type: 'POST',
                url: '/Contratos/RenovarPQ',
                data: formData,
                contentType: false,
                processData: false,
                cache: false,
                success: function (response) {
                    if (response.success) {
                        inputTipoCambioPolizaPQ.val('');

                        tblPoliza.DataTable().clear().draw();
                        getPQs();
                        modalPoliza.modal('hide');

                        swal('Confirmación', response.message, 'success');
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                },
                error: function (xhr, status, error) {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                }
            });
        }

        function abonarPQ(pqId, fechaMovimiento, objetoPoliza) {
            $.post('/Contratos/AbonarPQ',
                {
                    idPq: pqId,
                    fechaMovimiento: moment(fechaMovimiento, 'DD/MM/YYYY').toISOString(true),
                    infoPol: objetoPoliza
                }).then(response => {
                    if (response.success) {
                        tblPoliza.DataTable().clear().draw();
                        getPQs();
                        modalPoliza.modal('hide');

                        swal('Confirmación', response.message, 'success');
                    } else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }

        function downloadURI(elemento) {
            location.href = '/Contratos/DescargarArchivo?idPq=' + elemento;
        }

        function setVisor(elemento) {
            $.get('/Contratos/UrlArchivoPQ',
                {
                    idPq: elemento
                }).then(response => {
                    if (response.success) {
                        $('#myModal').data().ruta = response.items.ubicacionArchivo;
                        $('#myModal').modal('show');
                    }
                    else {
                        swal('Alerta!', response.message, 'warning');
                    }
                }, error => {
                    swal('Error!', `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, 'error');
                });
        }
        //#endregion

        (function init() {
            moment.locale('es');
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };

            getVersion()
                .done(function (response) {
                    if (_version != response) {
                        swal({
                            title: 'Alerta!',
                            text: 'Es necesario limpiar la caché del navegador,\nPresione <<Aceptar>> para limpiar la caché',
                            icon: "warning",
                            buttons: true,
                            dangerMode: true,
                            buttons: ["Cerrar", "Aceptar"]
                        })
                            .then((aceptar) => {
                                if (aceptar) {
                                    window.history.forward(1);
                                    location.reload();
                                }
                            });
                    }
                    else {
                        getBancos();
                        getCCs();
                        initInputsFechas();
                        getMonedas();
                        initTblPQ();
                        initTblPoliza();
                        getPQs();
                        initCatCuentas();
                        getFechaServidor();
                    }
                })
                .fail(function (error) {
                    swal("Error!", `Ocurrió un error al lanzar la petición al servidor:\nError ${error.status} - ${error.statusText}.`, "error");
                });
        })();
    }

    $(document).ready(() => Maquinaria.DocumentosPorPagar.GestionPQ = new GestionPQ())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();