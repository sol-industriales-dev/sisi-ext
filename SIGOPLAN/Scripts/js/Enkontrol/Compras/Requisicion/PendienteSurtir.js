(() => {
    $.namespace('Enkontrol.Compras.Requisicion.PendienteSurtir');
    PendienteSurtir = function () {
        //#region Selectores
        const selCC = $("#selCC");
        const tblSalidas = $('#tblSalidas');
        const mdlExistenciaDetalle = $('#mdlExistenciaDetalle');
        const labelModal = $('#labelModal');
        const report = $("#report");
        const comboEstatus = $('#comboEstatus');
        const botonBuscar = $('#botonBuscar');
        const selectAlmacen = $('#selectAlmacen');
        const selectValidadoAlmacen = $('#selectValidadoAlmacen');
        const inputFechaInicio = $('#inputFechaInicio');
        const inputFechaFin = $('#inputFechaFin');
        //#endregion

        const getReq = (listaCC, listaAlmacenes, estatus, validadoAlmacen, fechaInicio, fechaFin) => {
            return $.post('/Enkontrol/Requisicion/ObtenerRequisicionesPendientes', { listaCC, listaAlmacenes, estatus, validadoAlmacen, fechaInicio, fechaFin })
        };
        const getReqDet = (cc, req, almacen) => { return $.post('/Enkontrol/Requisicion/GetReqDetSalidasConsumo', { cc, req, almacen }) };
        const guardarSalidas = (salidas) => $.post('/Enkontrol/Requisicion/GuardarSalidasConsumo', { salidas });

        // Datepicker variables.
        const dateFormat = "dd/mm/yy";
        const showAnim = "slide";
        const fechaActual = new Date();
        const fechaInicioAnio = new Date(new Date().getFullYear(), 0, 1);

        (function init() {
            $('.select2').select2();
            initForm();
            initTableSalidas();

            inputFechaInicio.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaInicioAnio);
            inputFechaFin.datepicker({ dateFormat, maxDate: fechaActual, showAnim }).datepicker("setDate", fechaActual);

            $.blockUI({ message: 'Cargando...' });
            comboEstatus.fillComboAsync('/Enkontrol/Requisicion/FillComboEstatusSurtidoRequisicion', null, false, "Ambas", $.unblockUI);
            botonBuscar.click(() => {
                let listaCC = getValoresMultiples('#selCC');
                let listaAlmacenes = getValoresMultiples('#selectAlmacen');
                let estatus = comboEstatus.val() === 'Ambas' ? 0 : comboEstatus.val();
                let validadoAlmacen = +(selectValidadoAlmacen.val());
                let fechaInicio = inputFechaInicio.val();
                let fechaFin = inputFechaFin.val();

                $.blockUI({ message: 'Cargando requisiciones...' });
                getReq(listaCC, listaAlmacenes, estatus, validadoAlmacen, fechaInicio, fechaFin).done(function (response) {
                    if (response.success) {
                        AddRows(tblSalidas, response.data);
                    } else {
                        AlertaGeneral('Aviso', 'Ocurrió un error al buscar las requisiciones pendientes por surtir.');
                    }
                }).always($.unblockUI);
            });
        })();

        function initForm() {
            selCC.fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, 'Todos');
            convertToMultiselect('#selCC');
            selectAlmacen.fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtirTodos', null, false, 'Todos');
            convertToMultiselect('#selectAlmacen');
        }

        function initTableSalidas() {
            tblSalidas.DataTable({
                retrieve: true,
                paging: false,
                deferRender: true,
                searching: false,
                language: dtDicEsp,
                aaSorting: [2, 'desc'],
                rowId: 'id',
                scrollY: "250px",
                scrollCollapse: true,
                bLengthChange: false,
                initComplete: function (settings, json) {
                    tblSalidas.on('click', '.btn-req-detalle', function () {

                        const cc = $(this).attr('cc');
                        const req = $(this).attr('req');
                        const almacen = $(this).attr('almacen');

                        getReqDet(cc, req, almacen).done(function (response) {
                            if (response.success) {
                                tablaExistenciaDetalle(response.data);
                            }
                        });


                    });

                    tblSalidas.on('click', '.btn-imprimir', function () {
                        let rowData = tblSalidas.DataTable().row($(this).closest('tr')).data();

                        verReporteRequisicion(rowData.cc, rowData.numero);
                    });
                },
                columns: [
                    { data: 'ccDescripcion', title: 'Centro de Costo' },
                    { data: 'numero', title: 'Número Requisición' },
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'almacenLAB', title: 'LAB' },
                    {
                        data: 'validadoAlmacen', title: 'Validado Almacén', render: function (data, type, row, meta) {
                            if (data) {
                                // return `<button class="btn btn-sm btn-success">
                                //             <i class="fas fa-check"></i>
                                //         </button>`;

                                return `<h4 style="margin-top: 0px; margin-bottom: 0px; color: green;">&#10004;</h4>`;
                            } else {
                                // return `<button class="btn btn-sm btn-danger">
                                //             <i class="fas fa-times"></i>
                                //         </button>`;

                                return `<h4 style="margin-top: 0px; margin-bottom: 0px; color: red;">&#10008;</h4>`;
                            }
                        }
                    },
                    { data: 'estatusSurtido', title: 'Estatus' },
                    {
                        title: 'Consigna', render: function (data, type, row, meta) {
                            return row.consigna ? '<h4 style="margin-top: 0px; margin-bottom: 0px;">&#10004;</h4>' : ''; // return row.consigna ? '<button class="btn btn-sm btn-success"><i class="fas fa-check"></i></button>' : '';
                        }
                    },
                    { data: 'numeroOCString', title: 'Número OCs' },
                    {
                        sortable: false, data: 'numero',
                        render: function (data, type, row, meta) {
                            return `<button id=${meta.row} class="btn btn-sm btn-primary btn-req-detalle" cc=${row.cc} req=${row.numero} almacen="${row.libre_abordo}">
                                        <i class="fas fa-eye"></i> Detalle
                                    </button>
                                    <button class="btn btn-sm btn-warning surtir" cc=${row.cc} req=${row.numero}>
                                        <i class="fas fa-arrow-right"></i> Surtir
                                    </button>
                                    <button class="btn btn-sm btn-default btn-imprimir">
                                        <i class="fas fa-file"></i> Imprimir
                                    </button>`;
                        },
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
                        targets: [2]
                    },
                    { className: "dt-center", "targets": "_all" },
                    { width: '20%', targets: [0, 3] },
                    { width: '30%', targets: [8] }
                ],
                drawCallback: function (settings) {
                    $('button.surtir').unbind().click(e => {
                        const boton = $(e.currentTarget);
                        const cc = boton.attr('cc');
                        const req = boton.attr('req');
                        if (cc && req) {
                            const getUrl = window.location;
                            const baseUrl = getUrl.protocol + "//" + getUrl.host;
                            const urlSurtido = baseUrl + `/Enkontrol/Requisicion/Surtido?cc=${cc}&req=${req}`;
                            window.location.href = urlSurtido;
                        }
                    })
                }
            });
        }

        function tablaExistenciaDetalle(detalle) {
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

            let table = crearTabla();
            let body = document.createElement('tbody');

            if (detalle != undefined) {
                detalle.forEach(function (detalle) {
                    body.append(crearRowsDetalle(detalle));
                });
            }

            $(table).append(body);
            $(div).append(table);
            $(row).append(div);

            labelModal.text('Requisicion: ' + detalle[0].numeroReq)
            mdlExistenciaDetalle.find('.modal-body').append(row);
            mdlExistenciaDetalle.modal('show');
        }

        function crearTabla() {
            let table = document.createElement('table');
            table.setAttribute('id', 'tblExistenciaDetalle');
            table.setAttribute('style', 'width: 100%; margin-bottom: 0px;');
            table.classList.add('table');
            table.classList.add('tblExistenciaDetalle');

            let head = document.createElement('thead');

            let tr = document.createElement('tr');

            let thInsumo = document.createElement('th');
            thInsumo.textContent = 'Insumo';

            let thSolicitado = document.createElement('th');
            thSolicitado.textContent = 'Solicitado';

            let thAP = document.createElement('th');
            thAP.textContent = 'Almacen Propio';

            let thAE = document.createElement('th');
            thAE.textContent = 'Almacen Externo';

            let thOC = document.createElement('th');
            thOC.textContent = 'Orden Compra';

            let thExistencia = document.createElement('th');
            thExistencia.textContent = 'Existencia LAB';

            let thCCdestino = document.createElement('th');
            thCCdestino.textContent = 'CC Destino';

            let thAlmacenDestino = document.createElement('th');
            thAlmacenDestino.textContent = 'Almacen Destino';

            let thSalida = document.createElement('th');
            thSalida.textContent = 'Salida por Consumo';

            $(tr).append(thInsumo);
            $(tr).append(thSolicitado);
            $(tr).append(thAP);
            $(tr).append(thAE);
            $(tr).append(thOC);
            $(tr).append(thExistencia);
            $(tr).append(thCCdestino);
            $(tr).append(thAlmacenDestino);
            $(tr).append(thSalida);

            $(head).append(tr);
            $(table).append(head);

            return table;
        }

        function crearRowsDetalle(detalle) {
            let tr = document.createElement('tr');

            $(tr).attr('almacenOrigenID', detalle.almacen);
            $(tr).attr('cc', detalle.cc);
            $(tr).attr('importe', 0);
            $(tr).attr('numeroReq', detalle.numeroReq);
            $(tr).attr('insumo', detalle.insumo);
            $(tr).attr('solicitado', detalle.solicitado);

            let tdInsumo = document.createElement('td');
            tdInsumo.textContent = detalle.insumoDescripcion;
            tdInsumo.setAttribute('width', '7%');

            let tdSolicitado = document.createElement('td');
            tdSolicitado.setAttribute('align', 'right');
            tdSolicitado.textContent = formatValue(detalle.solicitado);
            tdSolicitado.setAttribute('width', '1.5%');

            let tdAP = document.createElement('td');
            tdAP.setAttribute('align', 'right');
            tdAP.textContent = formatValue(detalle.cantidadAP);
            tdAP.setAttribute('width', '1.5%');

            let tdAE = document.createElement('td');
            tdAE.setAttribute('align', 'right');
            tdAE.textContent = formatValue(detalle.cantidadAE);
            tdAE.setAttribute('width', '1.5%');

            let tdOC = document.createElement('td');
            tdOC.setAttribute('align', 'right');
            tdOC.textContent = formatValue(detalle.cantidadOC);
            tdOC.setAttribute('width', '1.5%');

            let tdExistencia = document.createElement('td');
            tdExistencia.setAttribute('align', 'right');
            tdExistencia.textContent = formatValue(detalle.existencia);
            tdExistencia.setAttribute('width', '1.5%');

            let tdCCdestino = document.createElement('td');
            let selectCCdestino = document.createElement('select');
            selectCCdestino.setAttribute('id', 'selectCCdestino')
            selectCCdestino.classList.add('form-control');
            $(selectCCdestino).fillCombo('/Enkontrol/Requisicion/FillComboCcAsigReq', null, false, null);
            $(selectCCdestino).val(detalle.cc)
            tdCCdestino.setAttribute('width', '7%');

            let tdAlmacenDestino = document.createElement('td');
            let selectAlmacenDestino = document.createElement('select');
            selectAlmacenDestino.setAttribute('id', 'selectAlmacenDestino')
            selectAlmacenDestino.classList.add('form-control');
            $(selectAlmacenDestino).fillCombo('/Enkontrol/Requisicion/FillComboAlmacenSurtir', null, false, null);
            $(selectAlmacenDestino).val(detalle.almacen)
            tdAlmacenDestino.setAttribute('width', '7%');

            let tdSalida = document.createElement('td');
            let inputSalida = document.createElement('input');
            inputSalida.classList.add('SalidaConsumo');
            inputSalida.setAttribute('value', formatValue(detalle.existencia));

            if (detalle.existencia == 0)
                inputSalida.setAttribute('disabled', true);
            else
                inputSalida.setAttribute('enabled', true);
            tdSalida.setAttribute('width', '1.5%');

            $(tdCCdestino).append(selectCCdestino);
            $(tdAlmacenDestino).append(selectAlmacenDestino);
            $(tdSalida).append(inputSalida);

            $(tr).append(tdInsumo);
            $(tr).append(tdSolicitado);
            $(tr).append(tdAP);
            $(tr).append(tdAE);
            $(tr).append(tdOC);
            $(tr).append(tdExistencia);
            $(tr).append(tdCCdestino);
            $(tr).append(tdAlmacenDestino);
            $(tr).append(tdSalida);

            return tr;
        }

        function crearSalidaConsumo() {
            let lstSalidas = [];

            $('#tblExistenciaDetalle').find('tbody tr td input.SalidaConsumo[enabled]').each(function (index) {
                const row = $(this).closest('tr');
                const cc = $(row).attr('cc');
                const cc_destino = row.find('#selectCCdestino').val()
                const almacenOrigenID = $(row).attr('almacenOrigenID');
                const importe = $(row).attr('importe');
                const almacenDestinoID = row.find('#selectAlmacenDestino').val()
                const numeroReq = $(row).attr('numeroReq');
                const insumo = $(row).attr('insumo');
                const solicitado = $(row).attr('solicitado');
                const cantidad = unmaskNumero(row.find('.SalidaConsumo').val());// cantidad sacarla del input 

                detalle = new Object();
                detalle.cc = cc;
                detalle.cc_destino = cc_destino;
                detalle.almacenOrigenID = almacenOrigenID;
                detalle.importe = importe;
                detalle.almacenDestinoID = almacenDestinoID;
                detalle.numeroReq = numeroReq;
                detalle.insumo = insumo;
                detalle.solicitado = solicitado;
                detalle.cantidad = cantidad;
                lstSalidas.push(detalle);
            });

            if (lstSalidas.length > 0) {
                guardarSalidas(lstSalidas).done(function (response) {
                    if (response.success) {
                        mdlExistenciaDetalle.modal('hide');
                        initForm();
                        verReporte();
                    } else {
                        AlertaGeneral('Aviso', 'Ocurrió un error al intentar guardar las salidas por consumo.');
                    }
                });
            }

        }

        function verReporte() {
            report.attr("src", `/Reportes/Vista.aspx?idReporte=110`);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI();
            };
        }

        function verReporteRequisicion(cc, numero) {
            $.blockUI({ message: 'Procesando...', baseZ: 2000 });

            report.attr("src", '/Reportes/Vista.aspx?idReporte=112' + '&cc=' + cc + '&numero=' + numero);
            report.on('load', function () {
                $.unblockUI();
                openCRModal();
            });
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw();
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
    }
    $(document).ready(() => Enkontrol.Compras.Requisicion.PendienteSurtir = new PendienteSurtir())
    // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
    // .ajaxStop($.unblockUI);
})();