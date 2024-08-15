(() => {
$.namespace('Admistrativo.Contabilidad.Propuesta.ConsultaConcentrado');
    ConsultaConcentrado = function (){
        const dpMin = $('#dpMin');
        const dpMax = $('#dpMax');
        const tblConcentrado = $('#tblConcentrado');
        let init = () => {
            initForm();
        }
        const getConcentrado = new URL(window.location.origin + '/Administrativo/Propuesta/getConcentrado');
        async function setTblConcentrado () {
            dtConcentrado.clear();
            response = await ejectFetchJson(getConcentrado, { busq: BusqConcentradoDTO() });
            if (response.success) {
                let lst = response.lstConcentrado.filter(data => data.tipo > 0);
                dtConcentrado.rows.add(lst).draw();
            }
        }
        function BusqConcentradoDTO (){
            return {
                min : dpMin.datepicker("getDate"),
                max: dpMax.datepicker("getDate")
            }
        }
        function initDataTblConcentrado() {
            dtConcentrado = tblConcentrado.DataTable({
                destroy: true,
                language: dtDicEsp,
                createdRow: function (tr, data) { setTipoConcentradoColor(tr, data); },
                columns: [
                    {
                        data: 'fecha', createdCell: function (td, data, rowData, row, col) {
                            $(td).html($.toDate(data));
                        }
                    },
                    { data: 'beneficiario' },
                    { data: 'concepto' },
                    {
                        data: 'obra', createdCell: function (td, data, rowData, row, col) {
                            $(td).text(setObraTipoReservaAutomatico(rowData));
                        }
                    },
                    {
                        data: 'noCheque', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data).addClass('text-right')
                            if (rowData.sonDolares)
                                $(td).html(data).addClass('sonDolares');
                        }
                    },
                    {
                        data: 'cargo', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data)).addClass('text-right');
                        }
                    },
                    {
                        data: 'abono', createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data)).addClass('text-right');
                        }
                    },
                ],
                initComplete: function (settings, json) {
                }
            });
        }
        setTipoConcentradoColor = (tr, { tipo, tipoReserva, tipoReservaAutomatica }) => {
            switch (tipo) {
                case 0: $(tr).addClass("esSaldoConciliado"); break;
                case 1: $(tr).addClass("esCadenaProductiva"); break;
                case 2: setTipoReservaColor(tr, { tipo: tipoReserva }); break;
                case 3: $(tr).addClass("esCheque"); break;
                case 5: $(tr).addClass("esInteresesFactoraje"); break;
                case 6: $(tr).addClass("esPolizasDiario"); break;
                case 7: $(tr).addClass("esEstadoCuenta"); break;
                case 8: $(tr).addClass("esMovimientoCliente"); break;
                default:
                    break;
            }
            switch (tipoReservaAutomatica) {
                case 8: $(tr).addClass("esEstrategica"); break;
                case 10: $(tr).addClass("esStandby"); break;
                case 11: $(tr).addClass("esAnticipo"); break;
                case 12: $(tr).addClass("esEstimacion"); break;
                case 14: $(tr).addClass("esRitchieBros"); break;
                case 100: $(tr).addClass("esAportacion"); break;
                default:
                    break;
            }
        }
        function setObraTipoReservaAutomatico({ obra, tipoReservaAutomatica }) {
            let text = obra;
            switch (tipoReservaAutomatica) {
                case 8: text = "ESTRATÉGICA"; break;
                case 10: text = "STAND BY"; break;
                case 11: text = "ANTICIPO"; break;
                case 12: text = "ESTIMACIÓN"; break;
                case 14: text = "RITCHIE BROS"; break;
                case 100: text = "APORTACIÓN"; break;
                default: text = obra; break;
            }
            return text;
        }
        function initForm() {
            dpMin.datepicker({
                onSelect: () => { setTblConcentrado(); }
            }).datepicker('setDate', new Date());
            dpMax.datepicker({
                onSelect: () => { setTblConcentrado(); }
            }).datepicker('setDate', new Date());
            initDataTblConcentrado();
        }
        init();
    }
    $(document).ready(() => {
        Admistrativo.Contabilidad.Propuesta.ConsultaConcentrado = new ConsultaConcentrado();
    })
})();