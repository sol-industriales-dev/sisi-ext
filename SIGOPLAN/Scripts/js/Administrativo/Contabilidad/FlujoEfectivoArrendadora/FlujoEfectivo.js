(() => {
    $.namespace('Administrativo.FlujoEfectivoArrendadora.Reporte');
    Reporte = function () {
        const selCC = $('#selCC');
        const mpFinal = $('#mpFinal');
        const btnExpor = $('#btnExpor');
        const btnExcel = $('.btnExcel');
        const mpInicial = $('#mpInicial');
        const btnBuscar = $('#btnBuscar');
        const tblAuxCont = $('#tblAuxCont');
        const mdlConcepto = $('#mdlConcepto');
        const mdlMovPoliza = $('#mdlMovPoliza');
        const txtMovPolizaTotal = $('#txtMovPolizaTotal');
        const btnMdlGuardarMovPol = $('#btnMdlGuardarMovPol');
        const getCC = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getCboCCActivosSigoplan');
        const getMovPolFlujo = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getMovPolFlujo');
        const getLstAuxAnual = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/getLstAuxAnual');
        const exportFlujoEfectivo = new URL(window.location.origin + '/Administrativo/FlujoEfectivoArrendadora/exportFlujoEfectivo');
        var year = 2000;
        let init = () => {
            initForm();
            btnBuscar.click(setLstAuxAnual);
            btnExpor.click(() => exportUrlToFile(exportFlujoEfectivo));
            btnMdlGuardarMovPol.click(setGuardar);
            mdlConcepto.on('shown.bs.modal', function () {
                dtAuxCont.columns.adjust();
            });
        }
        async function setLstAuxAnual() {
            try {
                let busq = getForm();
                dtAuxCont.clear().draw();
                btnExcel.prop("disabled", true);
                response = await ejectFetchJson(getLstAuxAnual, busq);
                if (response.success) {
                    dtAuxCont.rows.add(response.lst).draw();
                    year = busq.max.getFullYear();
                    tblAuxCont.find("thead th:eq(4)").text(busq.max.NombreMes().toUpperCase());
                }
                btnExcel.prop("disabled", !response.success);
            } catch (o_O) { AlertaGeneral("", o_O.message) }
        }
        function initDataTblAuxCont() {
            dtAuxCont = tblAuxCont.DataTable({
                destroy: true,
                ordering: false,
                paging: false,
                ordering: false,
                searching: false,
                bFilter: true,
                info: false,
                language: dtDicEsp,
                createdRow: (tr, data) => { $(tr).addClass(data.clase); },
                columns: [
                    { data: 'descripcion' }
                    , { data: 'acum', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'acumPorcentaje', createdCell: (td, data, rowData) => { setTdPorcentaje(td, data, rowData.clase); } }
                    , { data: 'idConcepto', createdCell: td => $(td).html("") }
                    , { data: 'mes', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'mesPorcentaje', createdCell: (td, data, rowData) => { setTdPorcentaje(td, data, rowData.clase); } }
                    , { data: 'ene', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'feb', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'mar', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'abr', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'may', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'jun', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'jul', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'ago', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'sep', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'oct', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'nov', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                    , { data: 'dic', createdCell: (td, data, rowData) => { setTdNumero(td, data, rowData.clase); } }
                ]
                , initComplete: function () {
                    tblAuxCont.on("click", "p", function () {
                        let cell = $(this).closest("td")
                            , row = $(this).closest("tr")
                            , data = dtAuxCont.row(row).data()
                            , eq = cell[0].cellIndex
                            , mes = 0
                            , thisYear = year
                            , monto = unmaskNumero($(this).text());
                        if (monto === 0) {
                            return;
                        }
                        switch (true) {
                            case eq >= 1 && eq <= 2:
                                mes = 11;
                                thisYear = 2000;
                                break;
                            case eq >= 4 && eq <= 5: mes = data.mesActual - 1; break;
                            case eq === 6: mes = 0; break;
                            case eq === 7: mes = 1; break;
                            case eq === 8: mes = 2; break;
                            case eq === 9: mes = 3; break;
                            case eq === 10: mes = 4; break;
                            case eq === 11: mes = 5; break;
                            case eq === 12: mes = 6; break;
                            case eq === 13: mes = 7; break;
                            case eq === 14: mes = 8; break;
                            case eq === 15: mes = 9; break;
                            case eq === 16: mes = 10; break;
                            case eq === 17: mes = 11; break;
                            default: break;
                        }
                        let busq = {
                            max: new Date(thisYear, mes, 1)
                            , idConcepto: data.idConcepto
                        };
                        setMovPol(busq);
                    });
                }
            });
            async function setMovPol(busq) {
                try {
                    dtMpMovPol.clear().draw();
                    response = await ejectFetchJson(getMovPolFlujo, busq);
                    if (response.success) {
                        let total = response.lst.reduce((prev, curr) => prev + curr.monto, 0)
                        txtMovPolizaTotal.val(maskNumero(total));
                        dtMpMovPol.rows.add(response.lst).draw();
                        limpiarLstData();
                        dtMpMovPol.rows().every(function (rowIdx, tableLoop, rowLoop) {
                            let row = this.node();
                            $(row).find("select").prop("disabled", true);
                            $(row).find(".input-group-btn").addClass("hidden");
                        });
                        mdlMovPoliza.modal("show");
                    }
                } catch (o_O) { AlertaGeneral('Aviso', o_O.message) }
            }
            function setTdNumero(td, numero, clase) {
                let esNegativo = numero < 0
                    , esNumero = ["Saldo", "Suma", "InputEncabezado", "SaldoEncabezado"].includes(clase)
                    , texto = $(`<p>`).text(esNumero ? maskNumero2D(numero).replace("-", "") : "");
                if (esNegativo) {
                    texto.addClass(`danger`);
                }
                $(td).html(texto);
            }
            function setTdPorcentaje(td, numero, clase) {
                let esNegativo = numero < 0
                    , esNumero = ["Saldo", "Suma", "InputEncabezado", "SaldoEncabezado"].includes(clase)
                    , texto = $(`<p>`).text(esNumero ? `${maskNumero2D(numero).replace("-", "")}%` : "");
                if (esNegativo) {
                    texto.addClass(`danger`);
                }
                $(td).html(texto);
            }
        }
        function getForm() {
            let fin = mpFinal.MonthPicker('GetSelectedDate')
                , anio = fin.getFullYear()
                , inicio = new Date(anio, 0, 1);
            return {
                min: inicio
                , max: fin
                , lstCC: []
                , lstAC: selCC.val()
            };
        }
        function initForm() {
            let ahora = new Date()
                , inicio = new Date(ahora.getFullYear(), 0, 1);
            mpInicial.MonthPicker().MonthPicker('Disable');
            $(`.periodo`).MonthPicker({
                Button: false,
                MonthFormat: 'MM, yy',
                i18n: mpDicEsp,
                OnAfterChooseMonth: function () {
                    let anio = mpFinal.MonthPicker('GetSelectedDate').getFullYear()
                        , fecha = new Date(anio, 0, 1);
                    mpInicial.val(`${fecha.NombreMes()}, ${fecha.getFullYear()}`)
                }
            });
            seMonthPickerValor(mpInicial, inicio);
            seMonthPickerValor(mpFinal, ahora);
            selCC.fillCombo(getCC, null, false, "TODOS");
            selCC.select2();
            initDataTblAuxCont();
            setLstAuxAnual();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.FlujoEfectivoArrendadora.Reporte = new Reporte();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();