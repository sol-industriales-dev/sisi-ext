(() => {
    $.namespace('Maquinaria.Captura.StandBy');
    StandBy = function () {
        const cboCC = $("#cboCC");
        const btnCargar = $("#btnCargar");
        const txtFecha = $("#txtFecha");
        const btnGuardar = $('#btnGuardar');
        const tblData = $("#tblData");

        const strGetData = new URL(window.location.origin + '/StandByNuevo/getListaDisponible');
        const strGuardar = new URL(window.location.origin + '/StandByNuevo/GuardarCaptura');

        let init = () => {
            InitForm();
            btnGuardar.click(setGuardar);
            btnCargar.click(CargarTblData);
            $('#tblData').on({
                change: function() {
                    var _this = $(this).is(":checked");
                    var justificacion = $(this).parent().parent().find(".justificacion");
                    if(_this){
                        justificacion.prop("disabled",false);
                        justificacion.val("");
                    }
                    else{
                        justificacion.prop("disabled",true);
                        justificacion.val("");
                    }
                }
            },'input[type="checkbox"]');
        }
        async function setGuardar() {
            try 
            {
                var lst = [];
                var aplica = $(".aplica");
                var completo = true;
                $.each(aplica,function(i,e){
                    if($(e).is(":checked")){
                        var o = {};
                        o.Economico = $(e).data("noEconomico");
                        o.noEconomicoID = $(e).data("noEconomicoID");
                        o.ccActual = $(e).data("cc");
                        o.comentarioJustificacion = $(e).parent().parent().find(".justificacion").val();
                        o.estatus=1;
                        lst.push(o);
                        if(o.comentarioJustificacion=='' || o.comentarioJustificacion == undefined)
                        {
                            completo=false;
                        }
                    }
                });
                if(completo){
                    if(lst.length>0){
                        response = await ejectFetchJson(strGuardar, { lst });
                        if (response.success) {
                            AlertaGeneral("Aviso", `Registros guardados con éxito, pendientes de Autorizacion.`);
                            CargarTblData();
                        } else {
                            AlertaGeneral(`Error`, `Ocurrió un error al intentar guardar la información.`);
                        }
                    }
                    else{
                        AlertaGeneral(`Alerta`, `Debe seleccionar almenos un equipo para solicitar StandBy`);
                    }
                }
                else{
                    AlertaGeneral(`Alerta`, `Es necesario que se escriba la justificación de cada equipo que se solicita para poner en StandBy`);
                }
            } 
            catch (o_O) 
            { 
                AlertaGeneral("Aviso", o_O.message) 
            }
        }
        async function CargarTblData() {
            try {
                let cc = cboCC.val();
                dtData.clear().draw();
                if (cc.length > 0) {
                    response = await ejectFetchJson(strGetData, { cc });
                        dtData.rows.add(response.data).draw();
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        function InitTblData() {
            dtData = tblData.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                columns: [
                    { title:'Equipo' , data: 'Economico',width:'70px'},
                    { title:'Modelo' , data: 'modelo'},
                    {
                        title:'Justificación' , data: 'cc', createdCell: (td, data, rowData, row, col) => {
                            $(td).html("<input type='text'/>");
                            $(td).find(`input[type="text"]`).addClass(`form-control justificacion`);
                            $(td).find(`input[type="text"]`).prop("disabled",true);
                        }
                    },
                    {
                        title:'Solicitar' , data: 'noEconomicoID',width:'70px', createdCell: (td, data, rowData, row, col) => {
                            $(td).html("<input type='checkbox'/>");
                            $(td).find(`input[type="checkbox"]`).addClass(`form-control aplica`);
                            $(td).find(`input[type="checkbox"]`).data("noEconomico",rowData.Economico);
                            $(td).find(`input[type="checkbox"]`).data("noEconomicoID",rowData.noEconomicoID);
                            $(td).find(`input[type="checkbox"]`).data("cc",rowData.cc);
                        }
                    }
                ]
                , initComplete: function (settings, json) {

                }
            });
}

        function toDateFromJson(src) {
            let strfecha = $.toDate(src).split("/")
                , fecha = new Date(+strfecha[2], +strfecha[1], +strfecha[0]);
            return fecha;
        }

        function InitForm() {
            let ahora = new Date();
            $(".fechaMesAnio").MonthPicker({
                Button: false,
                MonthFormat: 'MM, yy',
                i18n: mpDicEsp
            });

            txtFecha.datepicker().datepicker("setDate", ahora);
            cboCC.fillCombo('/CatObra/cboCentroCostosUsuarios', null, false);
            InitTblData();
        }
        init();
    }
    $(document).ready(() => {
        Maquinaria.Captura.StandBy = new StandBy();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();