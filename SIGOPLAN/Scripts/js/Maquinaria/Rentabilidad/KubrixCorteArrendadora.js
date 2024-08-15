(function () {

    $.namespace('maquinaria.rentabilidad.KubrixCorteArrendadora');

    KubrixCorteArrendadora = function ()
    {
        const comboAC = $('#comboAC');
        const comboTipo = $('#comboTipo');
        const cbTipoCorte = $('#cbTipoCorte');
        const inputCorte = $('#inputCorte');
        const chbTipoReporte = $("#chbTipoReporte");
        const cbConfiguracion = $("#cbConfiguracion");
        const comboGrupoK = $("#comboGrupoK");
        const comboModeloK = $("#comboModeloK");
        const cboMaquina = $("#cboMaquina");
        const cboHoraCorte = $("#cboHoraCorte");
        const inputDiaInicio = $("#inputDiaInicio");
        const inputDiaFinal = $("#inputDiaFinal");
        const botonBuscar = $('#botonBuscar');
        const comboDivision = $("#comboDivision");
        const comboResponsable = $("#comboResponsable");

        function init()
        {
            cargarCombos();
        }

        function cargarCombos()
        {
            //Si es multi, cargar solo los seleccionados al cerrar
            $('.comboMulti').on('select2:close', function () {
                let select = $(this)
                $(this).next('span.select2').find('ul').html(function () {
                    let count = select.select2('data').length
                    if (count == 1) return "<li class='lineaMulti'>" + count + " Seleccionado</li>"
                    return "<li class='lineaMulti'>" + count + " Seleccionados</li>"
                })
            })
            comboGrupoK.select2();
            comboModeloK.select2({ closeOnSelect: false });
            cboMaquina.select2();
            //Cargar Combo Grupo            
            comboGrupoK.fillCombo('/Rentabilidad/fillComboGrupo', {}, false, "TODOS");
            comboGrupoK.change(cargarComboModeloK);
            //Cargar Combo Modelo
            cargarComboModeloK();
            //Cargar Combo Economico
            cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: -1 }, false, "TODOS");
        }
        
        function cargarComboModeloK()
        {
            var grupo = comboGrupoK.val();
            if (grupo == "TODOS") {
                comboModeloK.fillCombo('/Rentabilidad/fillComboModelo', { grupoID: -1 }, true);
                comboModeloK.select2('destroy').find('option').prop('selected', false).end().select2({ closeOnSelect: false });
                cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: -1 }, false, "TODOS");
            }
            else {
                comboModeloK.fillCombo('/Rentabilidad/fillComboModelo', { grupoID: grupo }, true);
                comboModeloK.select2('destroy').find('option').prop('selected', true).end().select2({ closeOnSelect: false });
                cboMaquina.fillCombo('/Rentabilidad/fillComboMaquinaria', { grupoID: grupo }, false, "TODOS");
            }
            comboModeloK.trigger({ type: 'select2:close' });
        }


        init();
    };

    $(document).ready(function () {
        maquinaria.rentabilidad.KubrixCorteArrendadora = new KubrixCorteArrendadora();
    }).ajaxStart(function () { $.blockUI({ baseZ: 2000, message: 'Procesando...' }) }).ajaxStop($.unblockUI);
})();