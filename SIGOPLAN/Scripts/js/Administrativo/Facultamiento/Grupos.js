(() => {
    $.namespace('Administrativo.RecursosHumanos.BonoAdministrativo');
BonoAdministrativo = function () {
    let itemPeriodo;
    let isGestion = false;
    let itemGrupo = [];
    const cboGrupos = $("#cboGrupos");
    const btnNuevoGrupo = $('#btnNuevoGrupo');
    const tblCC = $('#tblCC');
    const modalGrupo = $("#modalGrupo");
    const txtGrupo = $('#txtGrupo');
    const btnAgregarGrupo = $('#btnAgregarGrupo');
    const tblGrupos = $("#tblGrupos");

    const getTblCC = new URL(window.location.origin + '/Administrativo/Facultamientos/getTblCC');
    const GuardarCCGrupo = new URL(window.location.origin + '/Administrativo/Facultamientos/GuardarCCGrupo');
    const getTblGrupo = new URL(window.location.origin + '/Administrativo/Facultamientos/getTblGrupo');
    const delGrupo = new URL(window.location.origin + '/Administrativo/Facultamientos/delGrupo');
    const GuardarGrupo = new URL(window.location.origin + '/Administrativo/Facultamientos/GuardarGrupo');
    let init = () => {
        InitForm();

}
async function setGuardarCCGrupo() {
    try {
        var _this = $(this);
        var ccID= _this.data("id");
        var grupoID= _this.val();
        response = await ejectFetchJson(GuardarCCGrupo, { ccID,grupoID});
    } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
}
async function CargartblCC() {
    try {
        let grupoID = cboGrupos.val()=="Todos"?0:cboGrupos.val();
        dtCCs.clear().draw();
        response = await ejectFetchJson(getTblCC, { grupoID});
        dtCCs.rows.add(response.data).draw();
    } catch (o_O) {if(dtCCs!=undefined) {AlertaGeneral("Aviso", o_O.message)} }
}
async function CargartblGrupos() {
    try {
        dtGrupos.clear().draw();
        response = await ejectFetchJson(getTblGrupo, { });
        dtGrupos.rows.add(response.data).draw();
    } catch (o_O) {if(dtGrupos!=undefined) {AlertaGeneral("Aviso", o_O.message)} }
}
async function fnEliminarGrupo() {
    try {
        let id = $(this).data("id");
        dtCCs.clear().draw();
        response = await ejectFetchJson(delGrupo, { id});
        CargartblGrupos();
    } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
}
async function setGuardarGrupo() {
    try {
        var grupo= txtGrupo.val();
        if(grupo!=""){
            response = await ejectFetchJson(GuardarGrupo, { grupo});
            CargartblGrupos();
        }
        else{
            AlertaGeneral("Aviso","Debe indicar una descripcion para el nuevo grupo");
        }
    } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
}
function InittblCC() {
    setGrupos();
    dtCCs = tblCC.DataTable({
        destroy: true,
        ordering: false,
        language: dtDicEsp,
        columns: 
            [
                { title:'CC' , data: 'cc'},
                { title:'Descripción' , data: 'descripcion'},
                {
                    title:'Grupo' , data: 'grupoID', createdCell: (td, data, rowData, row, col) => 
                    {
                        $(td).html(`<select>`);
                        $(td).find(`select`).fillComboItems(itemGrupo, "Sin asignar", data === 0 ? null : data);
                        $(td).find(`select`).change(setGuardarCCGrupo);
                        $(td).find(`select`).addClass(`form-control grupo`);
                        $(td).find(`select`).data('id',rowData.id);
                    }
                }
            ],
            initComplete: function (settings, json) {
                
            }
    });
dtGrupos = tblGrupos.DataTable({
    destroy: true,
    ordering: false,
    language: dtDicEsp,
    columns: 
        [
            { title:'Grupo' , data: 'grupo'},
            { title:'Cantidad CCs Vinculados' , data: 'cantidad'},
            {
                title:'Grupo' , data: 'id', createdCell: (td, data, rowData, row, col) => 
                {
                    if(rowData.cantidad==0){
                        $(td).html(`<button>`);
                        $(td).find(`button`).addClass(`btn btn-primary`);
                        $(td).find(`button`).data('id',data);
                        $(td).find(`button`).click(fnEliminarGrupo);
                        $(td).find(`button`).text('Eliminar');
}
                        else{
                            $(td).html('---');
                        }
                }
            }
        ],
        initComplete: function (settings, json) {
                
        }
        });
}

async function setGrupos() {
    try {
        response = await ejectFetchJson('/Administrativo/Facultamientos/LlenarComboDepartamentos');
        if (response.success) {
            itemGrupo = response.items;
        }
    } catch (o_O) { }
}
function fnModalGrupo(){
    CargartblGrupos();
    modalGrupo.modal("show");
}
function InitForm() {

    cboGrupos.fillCombo('/Administrativo/Facultamientos/LlenarComboDepartamentos', null, false, "Todos");
    cboGrupos.change(CargartblCC);
    InittblCC();
    CargartblCC();
    btnNuevoGrupo.click(fnModalGrupo);
    btnAgregarGrupo.click(setGuardarGrupo);
}
init();
}
$(document).ready(() => {
    Administrativo.RecursosHumanos.BonoAdministrativo = new BonoAdministrativo();
})
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();