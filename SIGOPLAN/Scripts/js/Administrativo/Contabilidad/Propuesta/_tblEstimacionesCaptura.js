(() => {
    $.namespace('Administrativo.Propuesta._tblEstimacionesCaptura');
    _tblEstimacionesCaptura = function () {
        let startDate, endDate, valoresCC, valoresCliente;
        const dpEstFecha = $('#dpEstFecha');
        const btnEstElimina = $('#btnEstElimina');
        const tblEstimacion = $('#tblEstimacion');        
        const btnAddRowEstimacion = $('#btnAddRowEstimacion');
        const btnGuardarEstimacion = $('#btnGuardarEstimacion');
        const btnBuscarEstimaciones = $('#btnBuscarEstimaciones');
        let guardarEstimacion = () => $.post('/Administrativo/Propuesta/guardarLstEstimacionResumen', { lst: getFormEstimacion() });
        let ElimnarEstimacion = () => $.post('/Administrativo/Propuesta/eliminarEstimacion', { lstId: getEstimacionId() });
        let getLstEstimaciones = () => $.post('/Administrativo/Propuesta/getLstFacturasEstimadas', getEstimacionBusq());

        $('table').on('change', '.cliente', function() {
            let idCliente = $(this).val();
            let tipoMoneda = idCliente < 9000 ? 'MXN' : 'DLL';
            $(this).closest('tr').find('td').eq('12').text(tipoMoneda);
        });

        $('table').on('change', '.seleccionItem', function() {
            $(this).data('seleccionado', $(this).data('seleccionado') == 0 ? 1 : 0);
        });

        let init = () => {
            initForm();
            btnAddRowEstimacion.click(addEstimacion);
            btnGuardarEstimacion.click(eventGuardarEstimacion);
            btnEstElimina.click(eventElimnarEstimacion);
            btnBuscarEstimaciones.click(buscarEstimaciones);
        }
        function buscarEstimaciones() {
            eventBusqEstimacion();
        }
        function eventBusqEstimacion() {
            dtEstimacion.clear().draw();
            getLstEstimaciones()
                .then(response => {
                    dtEstimacion.clear().rows.add(response.lst).draw();
                    btnGuardarEstimacion.prop("disabled", response.stAuth === 1);
                });
        }
        function addEstimacion() {
            AddRow(dtEstimacion, {
                cc: "",
                numcte: "",
                factura: "",
                fecha: "",
                fechavenc: "",
                linea: "",
                estimacion: 0,
                anticipo: 0,
                vencido: 0,
                pronostico: 0,
                cobrado: 0,
                fechaResumen: "",
                esActivo: true,
                Enkontrol: false
            });
        }
        function getEstimacionBusq() {
            return {
                fechaInicial: dpEstFecha.val().split(' - ')[0],
                fechaFinal: dpEstFecha.val().split(' - ')[1]
            };
        }
        function eventGuardarEstimacion() {
            let obj = getEstimacionBusq(),
                res = AlertaAceptarRechazar("Aviso", `¿Desea guardar las estimaciones a la fecha ${obj.fechaFinal}?`, guardarEstimacion, null);
            res.then(boton => {
                guardarEstimacion().then(response => {
                    if (response.success) {
                        AlertaGeneral("Aviso", `Estimaciones guardadas con éxito.`);
                    }
                });
            }).catch(err => { });
        }

        function eventElimnarEstimacion() {
            let res = AlertaAceptarRechazar("Aviso", `La estimacion seleccionada será eliminada. ¿Desea continuar?`, ElimnarEstimacion, null);
            res.then(boton => {
                ElimnarEstimacion().then(response => {
                    if (response.success) {
                        
                    }
                });
            }).catch(err => { });
        }
        function getFormEstimacion() {
            ElimnarEstimacion();
            
            let lst = [],
                obj = getEstimacionBusq();
            dtEstimacion.rows().iterator('row', function (context, index) {
                let node = $(this.row(index).node())
                   ,numcte = node.find(".cliente").val()
                    data = {
                        moneda: numcte > 9000 ? 2 : 1,
                        cc: node.find(".cc").val(),
                        numcte: numcte,
                        factura: node.find(".factura").val(),
                        fecha: node.find(".fecha").val(),
                        fechavenc: node.find(".fechavenc").val(),
                        linea: node.find(".linea").val(),
                        estimacion: unmaskNumero(node.find(".est").val()),
                        anticipo: unmaskNumero(node.find(".anticipo").val()),
                        vencido: unmaskNumero(node.find(".vencido").val()),
                        pronostico: unmaskNumero(node.find(".pronostico").val()),
                        cobrado: unmaskNumero(node.find(".cobrado").val()),
                        fechaResumen: obj.fechaFinal,
                        esActivo: node.find("input[type=checkbox]").prop("checked"),
                        Enkontrol: this.row(index).data().Enkontrol
                    },
                    esValida = validEstimacion(data);
                if (esValida) {
                    lst.push(data);
                } else {
                    return;
                }
            });
            return lst;
        }
        function getEstimacionId() {
            let lst = [];
            let lstEliminar = [];
            // try {
            //     dtEstimacion.rows().iterator('row', function (context, index) {
            //         let node = $(this.row(index).node())
            //            ,esActivo = node.find("input[type=checkbox]").prop("checked");
            //         if(!esActivo) {
            //             let id = this.row(index).data().id
            //             this.row(index).remove().draw();
            //             if (id > 0) {
            //                 lst.push(id);   
            //             }
            //         }
            //     });
            //     dtEstimacion.draw();   
            // } catch (o_O) { }
            try {
                dtEstimacion.rows().iterator('row', function (context, index) {
                    let node = $(this.row(index).node());
                    let esActivo = node.find("input[type=checkbox]").data("seleccionado") == 0;
                    if(!esActivo) {
                        let id = this.row(index).data().id
                        lstEliminar.push(index);
                        lst.push(id); 
                        // if (id > 0) {
                        //     lst.push(id); 
                        // }
                    }
                });
                for (let index = lstEliminar.length - 1; index >= 0; index--) {
                    dtEstimacion.row(lstEliminar[index]).remove();
                }
                // lstEliminar.forEach(function(element, index){
                //     dtEstimacion.row(index).remove().draw(true); 
                // });
                dtEstimacion.draw();
            } catch (o_O) { console.log('Horror: ' + o_O); }
            return lst;
        }
        function validEstimacion(data) {
            let valido = false,
               sumaValda = data.estimacion + data.anticipo + data.vencido + data.pronostico + data.cobrado;
            switch (true) {
                case data.factura.length === 0:
                case data.fecha.length === 0:
                case data.fechavenc.length === 0:
                case data.cc.length != 3:
                case sumaValda === 0:
                    valido = false; break;
                default: valido = true; break;
            }
            return valido;
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        setSemanaReservaSelecionada = () => {
            let date = dpEstFecha.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
            dpEstFecha.val(`${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`)
            selectCurrentWeek();
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                dpEstFecha.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        function initDataTblEstimacion() {
            dtEstimacion = tblEstimacion.DataTable({
                destroy: true,
                ordering: false,
                scrollY: "auto",
                scrollCollapse: true,
                paging: true,
                language: dtDicEsp,
                drawCallback: settings => tblEstimacion.find("select").select2(),
                columns: [
                    {
                        data: 'esActivo', width: '1%' ,createdCell: function (td, data, rowData, row, col) {
                            let chbox = $(`<input>`, { type: 'checkbox', checked: rowData.id > 0 ? data : true });
                            $(chbox).addClass('seleccionItem');
                            $(chbox).attr('data-seleccionado', 0);
                            $(chbox).attr('data-id', rowData.id);
                            $(td).html(chbox);
                        }
                    }
                    ,{
                        data: 'cc', width: '15.5%', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(getValoresComboCC(data).prop('disabled', rowData.Enkontrol));
                        }
                    },
                    {
                        data: 'numcte', width: '15.5%', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(getValoresComboClientes(data).prop('disabled', rowData.Enkontrol)).css('min-width', '100px');
                        }
                    },
                    {
                        data: 'factura', width: '6%', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`, {prop: 'disabled', disabled: rowData.Enkontrol });
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control text-right factura`);
                            $(td).find(`input`).val(data);
                        }
                    },
                    {
                        data: 'fecha', width: '6%', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`, {prop: 'disabled', disabled: rowData.Enkontrol });
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control fecha`);
                            $(td).find(`input`).datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", new Date());
                        }
                    },
                    {
                        data: 'fechavenc', width: '6%', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`, {prop: 'disabled', disabled: rowData.Enkontrol });
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control fechavenc`);
                            $(td).find(`input`).datepicker({ dateFormat: 'dd/mm/yy' }).datepicker("setDate", new Date());
                        }
                    },
                    {
                        data: 'linea', width: '12%', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`);
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control linea`);
                            $(td).find(`input`).val(data);
                        }
                    },
                    {
                        data: 'estimacion', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`);
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control text-right estimacion est`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'anticipo', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`);
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control text-right estimacion anticipo`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'vencido', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`);
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control text-right estimacion  vencido`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'pronostico', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`);
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control text-right estimacion pronostico`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'cobrado', createdCell: function (td, data, rowData, row, col) {
                            let input = $(`<input>`);
                            $(td).html(input);
                            $(td).find(`input`).addClass(`form-control text-right estimacion cobrado`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        data: 'numcte', width: '4%', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data < 9000 ? "MXN" : "DLL");
                        }
                    },
                    {
                        data: 'Enkontrol', title: 'Enkontrol'
                    }
                ],
                columnDefs: [
                    {
                        targets: [13],
                        visible: false
                    }
                ]
                ,initComplete: function (settings, json) {
                    tblEstimacion.find('tbody').on('change', '.estimacion', function () {
                        let monto = unmaskNumero(this.value);
                        this.value = maskNumero(monto);
                    });
                }
            });
            dtEstimacion.columns.adjust();
        }

        function cargarCliente() {
            $.post('/Facturacion/Facturacion/FillCboCliente').then(response => valoresCliente = response.items, () => valoresCliente = "Error");
        }
        function cargarCC() {
            $.post('/Administrativo/Propuesta/LlenarComboCC').then(response => valoresCC = response.items, () => valoresCC = "Error");
        }

        function getValoresComboCC(cc) {
            const select = $(`<select class="form-control cc"></select>`);
            select.fillComboItems(valoresCC, null, cc);
            return select;
        }

        function getValoresComboClientes(numcte) {
            const select = $(`<select class="form-control cliente"></select>`)
            select.fillComboItems(valoresCliente, null, numcte);
            return select;
        }

        function initForm() {
            cargarCliente();
            cargarCC();
            dpEstFecha.datepicker({
                firstDay: 0,
                showOtherMonths: true,
                selectOtherMonths: true,
                onSelect: function (dateText, inst) {
                    setSemanaReservaSelecionada();
                },
                beforeShowDay: function (date) {
                    var cssClass = '';
                    if (date >= startDate && date <= endDate)
                        cssClass = 'ui-datepicker-current-day';
                    return [true, cssClass];
                },
                onChangeMonthYear: function (year, month, inst) {
                    selectCurrentWeek();
                },
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 9999);
                    }, 0);
                }
            }).datepicker("setDate", new Date());
            setSemanaReservaSelecionada();
            initDataTblEstimacion();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Propuesta._tblEstimacionesCaptura = new _tblEstimacionesCaptura();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();