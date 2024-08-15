(() => {
    $.namespace('Maquinaria.Captura.StandBy');
    StandBy = function () {
        const cboAC = $("#cboAC");
        const cboCC = $("#cboCC");
        const btnCargar = $("#btnCargar");
        const tblData = $("#tblData");

        const cboTipo = $('#cboTipo');
        const fechaIni = $('#fechaIni');
        const fechaFin = $('#fechaFin');

        const columnasHistorico = [
            { title: 'Equipo', data: 'Economico', width: '70px' },
            { title: 'Modelo', data: 'modelo' },
            { title: 'Obra', data: 'ccActual' },
            { title: 'Estatus', data: 'estatus' },
            { title: 'Capturo', data: 'usuarioCapturaNombre' },
            { title: 'Fecha Capturo', data: 'fechaCaptura' },
            { title: 'Valido', data: 'usuarioAutorizaNombre' },
            { title: 'Fecha Valido', data: 'fechaAutoriza' },
            { title: 'Libero', data: 'usuarioLiberaNombre' },
            { title: 'Fecha Libero', data: 'fechaLibera' },
            { title: 'Motivo StandBy', data: 'comentarioJustificacion' },
            { title: 'Comentario de Validación', data: 'comentarioValidacion' }
        ];
        const columnasNoHistorico = [
            { title: 'Número Económico', data: 'Economico' },
            { title: 'Modelo', data: 'modelo' },
            { title: 'CC Actual', data: 'ccActual' },
            { title: 'Fecha depreciación', data: 'fechaDepreciacion' },
            { title: 'Depreciación del equipo', data: 'DepreciacionEquipo' },
            { title: 'Depreciación del equipo semanal', data: 'DepreciacionEquipoSemanal' },
            { title: 'Depreciación del equipo del periodo', data: 'DepreciacionEquipoPeriodo' },
            { title: 'Depreciación de overhaul', data: 'DepreciacionOverhaul' },
            { title: 'Depreciación de overhaul semanal', data: 'DepreciacionOverhaulSemanal' },
            { title: 'Depreciación de overhaul del periodo', data: 'DepreciacionOverhaulPeriodo' },
        ];
        const columnasPorUbicacion = [
            { title: 'Número Económico', data: 'Economico' },
            { title: 'Modelo', data: 'modelo' },
            { title: 'Fecha depreciación', data: 'fechaDepreciacion' },
            { title: 'Depreciación del equipo', data: 'DepreciacionEquipo' },
            { title: 'Depreciación del equipo semanal', data: 'DepreciacionEquipoSemanal' },
            { title: 'Depreciación del equipo del periodo', data: 'DepreciacionEquipoPeriodo' },
            { title: 'Depreciación de overhaul', data: 'DepreciacionOverhaul' },
            { title: 'Depreciación de overhaul semanal', data: 'DepreciacionOverhaulSemanal' },
            { title: 'Depreciación de overhaul del periodo', data: 'DepreciacionOverhaulPeriodo' },
        ];
        const columnasNoHistoricoDefs = [
            {
                targets: [4, 5, 6, 7, 8, 9],
                render: function(data, type, row) {
                    return maskNumero(data);
                }
            }
        ];
        const columnasPorUbicacionDefs = [
            {
                targets: [3, 4, 5, 6, 7, 8],
                render: function(data, type, row) {
                    return maskNumero(data);
                }
            }
        ];

        const strGetData = new URL(window.location.origin + '/StandByNuevo/getListaByEstatus');
        const strGetDataConDep = new URL(window.location.origin + '/StandByNuevo/getListaByEstatusConDepreciacion');

        let dtData;

        let init = () => {
            InitForm();
            btnCargar.click(CargarTblData);

        }

        async function CargarTblData() {
            try {
                dtData.clear().draw();
                var estatus = 0;
                var noAC = cboAC.val();
                var noEconomico = cboCC.val() == undefined || '' ? null : cboCC.val();
                var tipo = cboTipo.val();
                if (true) {
                    if (tipo == 1) {
                        response = await ejectFetchJson(strGetData, { estatus, noAC, noEconomico });

                        dtData.destroy();
                        tblData.empty();
                        InitTblData(columnasHistorico, null);
                        dtData.rows.add(response.data).draw();
                    }
                    else {
                        if (moment(fechaIni.val(), 'DD/MM/YYYY').isValid() && moment(fechaFin.val(), 'DD/MM/YYYY').isValid()) {

                            let fechaInicio = moment(fechaIni.val(), 'DD/MM/YYYY').toISOString(true);
                            let fechaFinal = moment(fechaFin.val(), 'DD/MM/YYYY').toISOString(true);

                            response = await ejectFetchJson(strGetDataConDep, { estatus, noAC, noEconomico, fechaInicio, fechaFinal, tipo});

                            dtData.destroy();
                            tblData.empty();
                            if (tipo == 3) {
                                InitTblData(columnasPorUbicacion, columnasPorUbicacionDefs);
                            }else{
                                InitTblData(columnasNoHistorico, columnasNoHistoricoDefs);
                            }
                            
                            dtData.rows.add(response.data).draw();
                        } else {
                            AlertaGeneral('Alerta', 'Debe ingresar una fecha de inicio y fecha de finalización valida');
                        }
                    }

                }
                else {
                    AlertaGeneral(`Alerta`, `Debe seleccionar un equipo para consultar`);
                }

            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        function InitTblData(columnas, columnasDefs) {
            dtData = tblData.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                columns: columnas,
                columnDefs: columnasDefs,
                initComplete: function (settings, json) {
                },
                headerCallback: function (thead, data, start, end, display) {
                    $(thead).addClass('bg-table-header text-center');
                }
            });
        }

        function toDateFromJson(src) {
            let strfecha = $.toDate(src).split("/")
                , fecha = new Date(+strfecha[2], +strfecha[1], +strfecha[0]);
            return fecha;
        }
        function triggerCombo() {
            cboCC.clearCombo();
            cboCC.fillCombo('/Horometros/cboModalEconomico', { obj: cboAC.val() == "" ? 0 : cboAC.val() });
        }
        function InitForm() {
            fechaIni.datepicker().datepicker();
            fechaFin.datepicker().datepicker();

            cboAC.fillCombo('/CatInventario/FillComboCC', { est: true }, false);
            cboAC.change(triggerCombo);
            cboAC.select2();
            cboCC.fillCombo('/Horometros/cboModalEconomico', { obj: 0 });
            cboCC.select2();
            InitTblData(columnasHistorico);
        }
        init();
    }
    $(document).ready(() => {
        Maquinaria.Captura.StandBy = new StandBy();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();