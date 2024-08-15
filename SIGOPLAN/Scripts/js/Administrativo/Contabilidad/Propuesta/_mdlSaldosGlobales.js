(() => {
    $.namespace('Administrativo.Contabilidad.Propuesta._mdlSaldosGlobales');
    _mdlSaldosGlobales = function () {
        let startDate, endDate;
        const tblSaldosCondensados = $('#tblSaldosCondensados');
        const txtFechaSaldo = $('#txtFechaSaldo');
        let init = () => {
            initForm();
        }
        const exportSaldosglobales = '/Administrativo/Propuesta/exportSaldosglobales';
        const getSaldosActuales = () => $.post('/Administrativo/Propuesta/getSaldosActuales', {busq: getBusq()});
        setTblSaldosActuales = () => {
            dtSaldosCondensados.clear().draw();
            getSaldosActuales().then(response => {
                if (response.success) { 
                    dtSaldosCondensados.rows.add(response.lstSaldos).draw();
                }
            });
        }
        exportExcelGlobal = () => exportUrlToFile(exportSaldosglobales)
        function initDataTblSaldosCondensados() {
            dtSaldosCondensados = tblSaldosCondensados.DataTable({
                destroy: true,
                paging: false,
                searching: false,
                info: false,
                ordering: false,
                language: dtDicEsp,
                createdRow: function (tr, data) {
                    $(tr).addClass(data.clase);
                },
                columns: [
                    { data: 'orden', createdCell: function (td, data, rowData, row, col) { $(td).html(data === 0 ? `` : data) } },
                    { data: 'descripcion' },
                    { data: 'saldo', createdCell: function (td, data, rowData, row, col) { setMoneda(td, data); } },
                    { data: 'total', createdCell: function (td, data, rowData, row, col) { setMoneda(td, data); } },
                    { data: 'global', createdCell: function (td, data, rowData, row, col) { setMoneda(td, data); } },
                ],
                initComplete: function (settings, json) { }
            });
        }
        function setMoneda(td, data) {
            let saldo = data === 0 ? `` : maskNumero(data);
            $(td).html(saldo);
        }
        setSemanaGlobalSelecionada = () => {
            let date = txtFechaSaldo.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6),
                diaNombre = endDate.toLocaleDateString("es-MX", { weekday: 'long' }).toUpperCase(),
                diaNumero = endDate.getDate(),
                mesNombre = endDate.toLocaleDateString("es-MX", { month: 'long' }).toUpperCase(),
                anio = endDate.getFullYear();
            txtFechaSaldo.val(`${diaNombre}, ${diaNumero} DE ${mesNombre} DE ${anio}`);
            selectCurrentWeek();
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                txtFechaSaldo.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        function getBusq(){
            let date = txtFechaSaldo.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 7);
            return {
                min: startDate.toLocaleDateString(),
                max: endDate.toLocaleDateString()
            };
        }
        function initForm() {
            txtFechaSaldo.datepicker({
                firstDay: 0,
                showOtherMonths: true,
                selectOtherMonths: true,
                onSelect: function (dateText, inst) {
                    setTblSaldosActuales();
                    setSemanaGlobalSelecionada();
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
            setSemanaGlobalSelecionada();
            initDataTblSaldosCondensados();
            setTblSaldosActuales();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Propuesta._mdlSaldosGlobales = new _mdlSaldosGlobales();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();