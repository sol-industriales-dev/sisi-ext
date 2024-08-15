(() => {
    $.namespace('Maquinaria.ActivoFijo.Detalles');
    Detalles = function () {
        const txtFiltroFecha = $('#txtFiltroFecha');
        const btnFiltro = $('#btnFiltro');
        const tblAFDetalles = $('#tblAFDetalles');

        btnFiltro.on('click', function () {
            getDetalles();
        })

        (function init() {
            initFecha();
            initTableDetalles();
        })();

        function initFecha() {
            const añoActual = new Date().getFullYear();
            const mesActual = new Date().getMonth();
            const ultimoDiaDelMes = moment(new Date(añoActual, mesActual)).endOf('month').format('DD');

            txtFiltroFecha.datepicker( {
                dateFormat: 'dd/mm/yy',
                changeDate: false,
                changeYear: true,
                changeMonth: true,
                showButtonPanel: true,
                onClose: function (dataText, inst) {
                    function isDonePressed() {
                        return ($('#ui-datepicker-div').html().indexOf('ui-datepicker-close ui-state-default ui-priority-primary ui-corner-all ui-state-hover') > -1);
                    }

                    if (isDonePressed()) {
                        var mesSeleccionado = $('#ui-datepicker-div .ui-datepicker-month :selected').val();
                        var añoSeleccionado = $('#ui-datepicker-div .ui-datepicker-year :selected').val();
                        var ultimoDiaDelMesSeleccionado = moment(new Date(añoSeleccionado, mesSeleccionado)).endOf('month').format('DD');

                        $(this).datepicker('setDate', new Date(añoSeleccionado, mesSeleccionado, ultimoDiaDelMesSeleccionado)).trigger('change');

                        $(this).focusout()
                    }
                }
            }).datepicker('setDate', new Date(añoActual, mesActual, ultimoDiaDelMes));
        }

        function initTableDetalles() {
            tblAFDetalles.DataTable({
                order: [[0, 'asc']],
                searching: true,
                sorting: true,
                paging: true,
                ordering: true,
                info: true,
                language: dtDicEsp,

                columns: [
                    { data: 'Fecha', title: 'Fecha' },
                    { data: 'Cc', title: 'Cc' },
                    { data: 'Clave', title: 'Clave' },
                    { data: 'Descripcion', title: 'Descripción' },
                    { data: 'MOI', title: 'MOI' },
                    { data: 'Altas', title: 'Altas' },
                    { data: 'Overhaul', title: 'Overhaul' },
                    { data: 'FechaCancelacion', title: 'Fecha de cancelación' },
                    { data: 'FechaBaja', title: 'Fecha de baja' },
                    { data: 'PorcentajeDepreciacion', title: '% de depreciación' }
                ],

                columnDefs: [
                    {
                        targets: [4, 5, 6],
                        render: function (data, type, row) {
                            return maskNumero(data);
                        }
                    }
                ],

                createdRow: function (row, data, dataIndex) {
                },

                drawCallback: function (settings) {
                },

                headerCallback: function (thead, data, start, end, display) {
                    $(thead).addClass('bg-table-header');
                    $(thead).find('th').eq('4').html('MOI al 31/12/' + (moment(txtFiltroFecha.val(), 'DD/MM/YYYY').year() - 1));
                },

                footerCallback: function(tfoot, data, start, end, display) {
                }
            });
        }

        function addRows(tabla, datos) {
            tabla = tabla.DataTable();
            tabla.clear().draw();
            tabla.rows.add(datos).draw();
        }

        function getDetalles() {
            $.get('/ActivoFijo/getDetalles', {
                fechaHasta: moment(txtFiltroFecha.val(), 'DD/MM/YYYY').toISOString(true)
            }).always().then(response => {
                if (response.success) {
                    addRows(tblAFDetalles, response.items);
                }
                else {
                    alert('Error: ' + response.message);
                }
            }, error => {
                alert('Error del servidor: ' + error.statusText);
            });
        }
    }

    $(document).ready(() => {
        Maquinaria.ActivoFijo = new Detalles();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();