(function () {
    $.namespace('Maquinaria.Reporte.RepAnalisisUtilizacion');
    RepAnalisisUtilizacion = function () {
        selAC = $('#selAC');
        selCC = $('#selCC');
        dtIni = $('#dtIni');
        dtFin = $('#dtFin');
        fecha = $('.fecha');
        selTipo = $('#selTipo');
        totalMX = $('#totalMX');
        totalUSD = $('#totalUSD');
        selGrupo = $('#selGrupo');
        radioBtn = $('.radioBtn a');
        btnBuscar = $('#btnBuscar');
        selModelo = $('#selModelo');
        btnExport = $('#btnExport');
        btnReporte = $('#btnReporte');
        inpMinRitmo = $('#inpMinRitmo');
        inpMaxRitmo = $('#inpMaxRitmo');
        tblAnalisis = $('#tblAnalisis');
        radMoneda = $('.radioBtn a[data-toggle="radMoneda"]');
        function init() {
            initForm();
            radioBtn.click(aClick);
            btnExport.click(getExcel);
            btnBuscar.click(setAnalisis);
            btnReporte.click(setReporte);
            selAC.change(setBtn);
            selTipo.change(setSelGrupo);
            selGrupo.change(setSelModelo);
            inpMinRitmo.change(setMinRitmo);
            inpMaxRitmo.change(setMaxRitmo);
            selModelo.change(setSelEconomico);
        }
        //#region Formulario Busqueda
        function initForm() {
            initTable();
            fecha.datepicker().datepicker("setDate", new Date());
            selAC.fillCombo('/RepAnalisisUtilizacion/cboAC', null, false, null);
            selTipo.fillCombo('/CatGrupos/FillCboTipoMaquinaria', { estatus: true }, false, "Todos");
            selGrupo.fillCombo('/CatGrupos/FillCboGrupoMaquina', { obj: 0 }, false, "Todos");
            selModelo.fillCombo('/CatModeloEquipo/FillMultipleModelo', { lstGrupo: [0] }, false, "Todos");
            selCC.fillCombo('/CatMaquina/FillMultipleEconomicos', { lstGrupo: [0], lstModelo: [0] }, false, "Todos");
            convertToMultiselect("#selGrupo");
            convertToMultiselect("#selModelo");
            convertToMultiselect("#selCC");
            btnBuscar.prop("disabled", true);
            btnExport.prop("disabled", true);
            btnReporte.prop("disabled", true);
        }
        function setSelEconomico() {
            if (this.value.length > 0) {
                selCC.fillCombo('/CatMaquina/FillMultipleEconomicos', {
                    lstGrupo: getValoresMultiples("#selGrupo"),
                    lstModelo: getValoresMultiples("#selModelo")
                }
                    , false
                    , "Todos");
                convertToMultiselect("#selCC");
            }
        }
        function setSelModelo() {
            selModelo.fillCombo('/CatModeloEquipo/FillMultipleModelo', { lstGrupo: getValoresMultiples("#selGrupo") }, false, "Todos");
            convertToMultiselect("#selModelo");
            selCC.fillCombo('/CatMaquina/FillMultipleEconomicos', {
                lstGrupo: getValoresMultiples("#selGrupo"),
                lstModelo: getValoresMultiples("#selModelo")
            }
                , false
                , "Todos");
            convertToMultiselect("#selCC");
        }
        function setSelGrupo() {
            selGrupo.fillCombo('/CatGrupos/FillCboGrupoMaquina', { obj: +(this.value) }, false, "Todos");
            convertToMultiselect("#selGrupo");
            selModelo.fillCombo('/CatModeloEquipo/FillMultipleModelo', { lstGrupo: getValoresMultiples("#selGrupo") }, false, "Todos");
            convertToMultiselect("#selModelo");
            selCC.fillCombo('/CatMaquina/FillMultipleEconomicos', {
                lstGrupo: getValoresMultiples("#selGrupo"),
                lstModelo: getValoresMultiples("#selModelo")
            }
                , false
                , "Todos");
            convertToMultiselect("#selCC");
        }
        function setBtn() {
            if (+(this.value.length) > 0) {
                btnBuscar.prop("disabled", false);
                btnReporte.prop("disabled", false);
                btnExport.prop("disabled", false);
            }
            else {
                btnBuscar.prop("disabled", true);
                btnReporte.prop("disabled", true);
                btnExport.prop("disabled", true);
            }
        }
        function setMinRitmo() {
            let min = +(this.value),
                max = +(inpMaxRitmo.val());
            if (min > max)
                inpMaxRitmo.val(min + 1);
        }
        function setMaxRitmo() {
            let max = +(this.value),
                min = +(inpMinRitmo.val());
            if (max < min)
                inpMinRitmo.val(max - 1)
        }
        function getBusqForm() {
            return {
                cc: selAC.val(),
                tipo: selTipo.val(),
                grupo: getValoresMultiples("#selGrupo"),
                modelo: getValoresMultiples("#selModelo"),
                noEco: getValoresMultiples("#selCC"),
                ini: dtIni.val(),
                fin: dtFin.val(),
                ritmoMin: inpMinRitmo.val(),
                ritmoMax: inpMaxRitmo.val(),
                moneda: getRadioValue("radMoneda")
            };
        }
        //#endregion        
        function setReporte() {
            $.blockUI({ message: "Procesando información" });
            $("#report").attr("src", `/Reportes/Vista.aspx?idReporte=92&cc=${selAC.val()}&fin=${dtFin.val()}&grupo=${getValoresMultiples("#selGrupo").toString()}&modelo=${getValoresMultiples("#selModelo").toString()}&noEco=${getValoresMultiples("#selCC").toString()}`);
            document.getElementById('report').onload = function () {
                openCRModal();
                $.unblockUI();
            };
        }
        function getExcel() {
            $.post(`/RepAnalisisUtilizacion/getBusqExpors`, { busq:getBusqForm() }).done(function(response){
                if (response.success)
                    download(`/RepAnalisisUtilizacion/setExport`);
            });
        }

        function download(url) {
            $.blockUI({ message: "Preparando archivo a descargar" });
            var link = document.createElement("button");
            link.download = url;
            link.href = url;
            $(link).unbind("click");
            location.href = url;
            $.unblockUI();
        }
        function getAnalisis() {
            return $.post('/RepAnalisisUtilizacion/getAnalisis', { busq: getBusqForm() });
        }
        //#region Tabla
        function setAnalisis() {
            getAnalisis().done(function (response) {
                if (response.success) {
                    AddRows(tblAnalisis, response.data);
                    totalMX.text(maskNumero(response.totalMX));
                    totalUSD.text(maskNumero(response.totalUSD));
                }
                else {
                    totalMX.text(maskNumero(0));
                    totalUSD.text(maskNumero(0));
                }
            });
        }
        function initTable() {
            dtAnalisis = tblAnalisis.DataTable({
                language: dtDicEsp,
                columns: [
                    { data: 'noEco', width: '8%' },
                    { data: 'grupo', width: '17%' },
                    { data: 'modelo', width: '17%' },
                    { data: 'hi', className: 'text-right' },
                    { data: 'hf', className: 'text-right' },
                    { data: 'ht', className: 'text-right' },
                    {
                        data: 'promSem', className: 'text-right',
                        createdCell: function (td, data, rowData, row, col) {
                            $(td).html(`${data.toFixed(2)}`);
                        }
                    },
                    {
                        data: 'totalUSD', className: 'text-right',
                        createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data));
                        }
                    },
                    {
                        data: 'totalMX', className: 'text-right',
                        createdCell: function (td, data, rowData, row, col) {
                            $(td).html(maskNumero(data));
                        }
                    }
                ]
            });
            totalMX.text(maskNumero(0));
            totalUSD.text(maskNumero(0));
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            for (let i in lst)
                if (lst.hasOwnProperty(i))
                    AddRow(dt, lst[i]);
        }
        function AddRow(dt, obj) {
            dt.row.add(obj).draw(false);
        }
        //#endregion
        //#region aRadio
        function setRadioValue(tog, sel) {
            $(`#${tog}`).prop('value', sel);
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }
        function aClick() {
            let sel = $(this).data('title');
            let tog = $(this).data('toggle');
            $(`a[data-toggle="${tog}"]`).not(`[data-title="${sel}"]`).removeClass('active').addClass('notActive');
            $(`a[data-toggle="${tog}"][data-title="${sel}"]`).removeClass('notActive').addClass('active');
        }
        function getRadioValue(tog) {
            return $(`a.active[data-toggle="${tog}"]`).data('title');
        }
        //#endregion
        init();
    }
    $(document).ready(function () {
        Maquinaria.Reporte.RepAnalisisUtilizacion = new RepAnalisisUtilizacion();
    })
        .ajaxStart(function () { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(function () { $.unblockUI(); });
})();