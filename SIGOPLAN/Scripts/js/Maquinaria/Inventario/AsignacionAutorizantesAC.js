(() => {
    $.namespace('CH.Plataformas');

//#region CONST PLATAFORMAS

const tblData = $('#tblData');
let dtData;

//#endregion

Plataformas = function () {
    (function init() {
        fncListeners();
    })();

    function fncListeners() {
        //#region INIT DATATABLE
        initTblData();
        fncGetAutorizantes();
        //#endregion
    }
        
    //#region CRUD PLATAFORMAS
    function initTblData() {
        dtData = tblData.DataTable({
            paging: false,
            destroy: true,
            ordering: false,
            language: dtDicEsp,
            "sScrollX": "100%",
            "sScrollXInner": "100%",
            "bScrollCollapse": true,
            scrollY: '65vh',
            scrollCollapse: true,
            "bLengthChange": false,
            "searching": true,
            "bFilter": true,
            "bInfo": true,
            "bAutoWidth": false,
            columns: [
                { data: 'cc', title: 'AC' },
                { data: 'ccDesc', title: 'Descripcion' },
                {
                    data: 'adminMaq',title:'Admin Maq' ,createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`<input class="form-control ui-autocomplete-input" placeholder="" autocomplete="off" data-perfilid='5' data-cc='${rowData.cc}'/>`);
                        $(td).find(`input`).getAutocomplete(eventAutoAuth, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        $(td).find(`input`).val(rowData.adminMaqNom);     
                        $(td).find(`input`).data().idUsuario = rowData.adminMaq;  
                    }
                },
                {
                    data: 'gerenteObra',title:'Gerente de Obra' ,createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`<input class="form-control ui-autocomplete-input" placeholder="" autocomplete="off" data-perfilid='1' data-cc='${rowData.cc}'/>`);
                        $(td).find(`input`).getAutocomplete(eventAutoAuth, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        $(td).find(`input`).val(rowData.gerenteObraNom);     
                        $(td).find(`input`).data().idUsuario = rowData.gerenteObra;  
                    }
                },
                {
                    data: 'directorArea',title:'Gerente/Director Division' ,createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`<input class="form-control ui-autocomplete-input" placeholder="" autocomplete="off" data-perfilid='4' data-cc='${rowData.cc}'/>`);
                        $(td).find(`input`).getAutocomplete(eventAutoAuth, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        $(td).find(`input`).val(rowData.directorAreaNom);     
                        $(td).find(`input`).data().idUsuario = rowData.directorArea;  
                    }
                },
                {
                    data: 'directoDivision',title:'Director División' ,createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`<input class="form-control ui-autocomplete-input" placeholder="" autocomplete="off" data-perfilid='2' data-cc='${rowData.cc}'/>`);
                        $(td).find(`input`).getAutocomplete(eventAutoAuth, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        $(td).find(`input`).val(rowData.directoDivisionNom);     
                        $(td).find(`input`).data().idUsuario = rowData.directoDivision;  
                    }
                },
                {
                    data: 'directorServicios',title:'Director de Servicios' ,createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`<input class="form-control ui-autocomplete-input" placeholder="" autocomplete="off" data-perfilid='11' data-cc='${rowData.cc}'/>`);
                        $(td).find(`input`).getAutocomplete(eventAutoAuth, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        $(td).find(`input`).val(rowData.directorServiciosNom);     
                        $(td).find(`input`).data().idUsuario = rowData.directorServicios;  
                    }
                },
                {
                    data: 'altaDireccion',title:'Alta Dirección' ,createdCell: function (td, data, rowData, row, col) {
                        $(td).html(`<input class="form-control ui-autocomplete-input" placeholder="" autocomplete="off" data-perfilid='3' data-cc='${rowData.cc}'/>`);
                        $(td).find(`input`).getAutocomplete(eventAutoAuth, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        $(td).find(`input`).val(rowData.altaDireccionNom);     
                        $(td).find(`input`).data().idUsuario = rowData.altaDireccion;  
                    }
                },
            ],
            initComplete: function (settings, json) {
                //tblData.on('click','.actualizarPlataforma', function () {
                //    let rowData = dtData.row($(this).closest('tr')).data();
                //    fncLimpiarMdlCEPlataformas();
                //    txtCEPlataforma.val(rowData.plataforma);
                //    btnCEPlataforma.attr("data-id", rowData.id);
                //    lblTitleCEPlataforma.html("Actualizar plataforma");
                //    lblTitleBtnCEPlataforma.html("Actualizar");
                //    mdlCEPlataformas.modal("show");
                //});

            },
            columnDefs: [
                { className: 'dt-center','targets': '_all'}
            ],
        });
    }
    function eventAutoAuth(event, ui) {
        let inp = $(this);
        inp.val(ui.item.label);
        //inp.data().idUsuario = ui.item.id;
        var usuarioID = ui.item.id;
        var perfilID = inp.data().perfilid;
        var cc = inp.data().cc;
        fncCEPlataforma(usuarioID,perfilID,cc)
    }
    function fncGetAutorizantes() {
        axios.post("/SolicitudEquipo/GetAutorizadoresAC").then(response => {
            let { success, items, message } = response.data;
        if (success) {
            //#region FILL DATATABLE
            dtData.clear();
            dtData.rows.add(response.data.items);
            dtData.draw();
            //#endregion
        } else {
            Alert2Error(message);
        }
    }).catch(error => Alert2Error(error.message));
    }

    async function fncCEPlataforma(usuarioID,perfilID,cc) {
        response = await ejectFetchJson("/SolicitudEquipo/SetAutorizadorAC", {usarioID : usuarioID, ac : cc, perfil : perfilID});
        let { success, items, message } = response.data;
        if (success) {

        }
    }



//#endregion
}

$(document).ready(() => {
    CH.Plataformas = new Plataformas();
})
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();