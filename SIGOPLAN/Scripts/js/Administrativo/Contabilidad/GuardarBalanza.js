(() => {
    $.namespace('Administrativo.Contabilidad.GuardarBalanza');
    GuardarBalanza = function () {
        //#region Selectores
        const tablaBalanza = $('#tablaBalanza');
        const inputMes = $('#inputMes');
        const botonBuscar = $('#botonBuscar');
        const botonEliminar = $('#botonEliminar');
        const botonGuardar = $('#botonGuardar');
        //#endregion

        let dtTablaBalanza;

        //#region Variables Date
        const showAnim = "slide";
        const fechaActual = new Date();
        //#endregion

        (function init() {
            initTablaBalanza();
            initMonthPicker(inputMes);

            botonBuscar.click(cargarBalanza);
            botonEliminar.click(eliminarBalanza);
            botonGuardar.click(guardarBalanza)
        })();

        function initTablaBalanza() {
            dtTablaBalanza = tablaBalanza.DataTable({
                language: dtDicEsp,
                paging: false,
                info: false,
                searching: false,
                scrollY: '45vh',
                scrollCollapse: true,
                columns: [
                    { data: 'cta', title: 'cta' },
                    {
                        data: 'scta', title: 'scta', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return '000' + data;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'sscta', title: 'sscta', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return '000' + data;
                            } else {
                                return data;
                            }
                        }
                    },
                    { data: 'descripcion', title: 'Descripción' },
                    {
                        data: 'saldoInicial', title: 'Saldo Inicial', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red; margin-bottom: 0px;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'cargos', title: 'Cargos', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red; margin-bottom: 0px;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'abonos', title: 'Abonos', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red; margin-bottom: 0px;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    },
                    {
                        data: 'saldoActual', title: 'Saldo Actual', render: function (data, type, row, meta) {
                            if (type === 'display') {
                                return data >= 0 ? maskNumero(data) : `<p style="color: red; margin-bottom: 0px;">${('-' + (maskNumero(data).replace('-', '')))}</p>`;
                            } else {
                                return data;
                            }
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                drawCallback: function(settings) {
                    let total = 0;
                    tablaBalanza.DataTable().columns().every(function(colIdx, tableLoop, colLoop) {
                        if (colIdx > 3) {
                            for (let x = 0; x < this.data().length; x++) {
                                total += this.data()[x];
                            }

                            $(this.footer()).html(maskNumero(total));
                        }

                        total = 0;
                    });
                }
            });
        }

        function initMonthPicker(input) {
            $(input).datepicker({
                dateFormat: "mm/yy",
                changeMonth: true,
                changeYear: true,
                showButtonPanel: true,
                maxDate: fechaActual,
                showAnim: showAnim,
                closeText: "Aceptar",
                onClose: function (dateText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                        $(this).datepicker('setDate', new Date(year, month, 1)).trigger('change');

                        $('.date-picker').focusout()//Added to remove focus from datepicker input box on selecting date
                    }
                },
                beforeShow: function (input, inst) {
                    inst.dpDiv.addClass('month_year_datepicker')

                    if ((datestr = $(this).val()).length > 0) {
                        year = datestr.substring(datestr.length - 4, datestr.length);
                        month = datestr.substring(0, 2);
                        $(this).datepicker('option', 'defaultDate', new Date(year, month - 1, 1));
                        $(this).datepicker('setDate', new Date(year, month - 1, 1));
                        $(".ui-datepicker-calendar").hide();
                    }
                }
            }).datepicker("setDate", fechaActual);
        }

        function cargarBalanza() {
            botonEliminar.hide();
            let mes = inputMes.val();
            let listaStringMes = mes.split('/');
            let fecha = '01' + '/' + listaStringMes[0] + '/' + listaStringMes[1];

            axios.post('CalcularBalanza', { fechaAnioMes: fecha })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        AddRows(tablaBalanza, response.data.data);
                        botonEliminar.show();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function eliminarBalanza() {
            let mes = inputMes.val();
            let listaStringMes = mes.split('/');
            let fecha = '01' + '/' + listaStringMes[0] + '/' + listaStringMes[1];
            axios.post('EliminarBalanza', { fechaCorte:  fecha })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha eliminado el corte.');
                    } else {
                        AlertaGeneral('Alerta', message);
                    }
                }).catch(error => AlertaGeneral('Alerta', error.message));
        }

        function guardarBalanza() {
            if (dtTablaBalanza.data().toArray().length > 0) {
                Alert2AccionConfirmar('Alerta', '¿Desea guardar la balanza del mes "' + inputMes.val() + '"?', 'Confirmar', 'Cancelar', () => confirmarGuardadoBalanza());
            } else {
                Alert2Info('La balanza no tiene datos.');
            }
        }

        function confirmarGuardadoBalanza() {
            let mes = inputMes.val();
            let listaStringMes = mes.split('/');
            let fecha = '01' + '/' + listaStringMes[0] + '/' + listaStringMes[1];

            axios.post('GuardarBalanzaCorte', { fechaAnioMes: fecha })
                .then(response => {
                    let { success, datos, message } = response.data;

                    if (success) {
                        Alert2Exito('Se ha guardado la información.');
                        inputMes.val('');
                        dtTablaBalanza.clear().draw();
                    } else {
                        AlertaGeneral(`Alerta`, message);
                    }
                }).catch(error => AlertaGeneral(`Alerta`, error.message));
        }

        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
    }
    $(document).ready(() => Administrativo.Contabilidad.GuardarBalanza = new GuardarBalanza())
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop($.unblockUI);
})();