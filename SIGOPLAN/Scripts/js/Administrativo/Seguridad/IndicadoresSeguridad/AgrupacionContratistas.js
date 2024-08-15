(() => {
$.namespace('AgrupacionContratistas.Seguridad');

    //#region SELECTORES
    const cboAgrupaciones = $('#cboAgrupaciones');
    const btnLimpiar = $('#btnLimpiar');
    const btnBuscar = $('#btnBuscar');
    const btnCrearNombreAgrupacion = $('#btnCrearNombreAgrupacion');

    const tblAgrupacionContratistas = $('#tblAgrupacionContratistas');
    const mdlCrearEditarNombreAgrupacion = $('#mdlCrearEditarNombreAgrupacion');
    const lblCrearEditarNombreAgrupacion = $('#lblCrearEditarNombreAgrupacion');
    const txtCrearEditarNombreAgrupacion = $('#txtCrearEditarNombreAgrupacion');
    const btnCrearEditarNombreAgrupacion = $('#btnCrearEditarNombreAgrupacion');

    const tblContratistas = $('#tblContratistas');
    const mdlCrearContratistas = $('#mdlCrearContratistas');
    const cboCrearContratistaEnAgrupacion = $('#cboCrearContratistaEnAgrupacion');
    const btnCrearContratistaEnAgrupacion = $('#btnCrearContratistaEnAgrupacion');
    const btnCancelarCrearContratistaEnAgrupacion = $('#btnCancelarCrearContratistaEnAgrupacion');

    let dtAgrupaciones;
    let dtContratistas;
    //#endregion

    Seguridad = function (){
        (function init() {
            Listeners();
            fncFillCboAgrupaciones();
            fncFillCboContratistas();
            initAgrupacionContratistas();
            initGetContratistas();
        })();
    }

    function Listeners(){
        btnCrearNombreAgrupacion.click(function (e){
            lblCrearEditarNombreAgrupacion.text("Nueva agrupación");
            btnCrearEditarNombreAgrupacion.attr("data-id", 0);
            btnCrearEditarNombreAgrupacion.attr("text", "Guardar");
            txtCrearEditarNombreAgrupacion.val("");
            mdlCrearEditarNombreAgrupacion.modal("show");
        });

        btnLimpiar.click(function (e){
            cboAgrupaciones[0].selectedIndex = 0;
            cboAgrupaciones.trigger("change");
        });

        btnCrearEditarNombreAgrupacion.click(function (e){
            fncCrearEditarAgrupacionContratistas();
        });

        btnBuscar.click(function (e){
            fncGetAgrupacionesContratistas();
        });

        btnCrearContratistaEnAgrupacion.click(function (e){
            fncCrearContratistas();
        });

        btnCancelarCrearContratistaEnAgrupacion.click(function (e){
            cboCrearContratistaEnAgrupacion[0].selectedIndex = 0;
            cboCrearContratistaEnAgrupacion.trigger("change");
        });
    }

    function fncFillCboAgrupaciones(){
        cboAgrupaciones.select2();
        cboAgrupaciones.fillCombo("FillCboAgrupaciones", {}, false);
    }

    function fncFillCboContratistas(){
        cboCrearContratistaEnAgrupacion.select2();
        cboCrearContratistaEnAgrupacion.fillCombo("FillCboContratistas", {}, false);
    }

    function initAgrupacionContratistas() {
        dtAgrupaciones = tblAgrupacionContratistas.DataTable({
            language: dtDicEsp,
            destroy: false,
            ordering: true,
            paging: true,
            searching: false,
            bFilter: false,
            info: false,
            columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'nomAgrupacion', title: 'Agrupación' },
                { 
                    title: 'Contratistas',
                    render: function (data, type, row) {
                        return `<button class="btn-success btn verContratistas" data-id="${row.id}"><i class="fas fa-list-ul"></i></button>`;
                    }
                },
                {
                    render: function (data, type, row) {
                        let btnAcciones = `<button class="btn-warning btn editarNombreAgrupacion" data-id="${row.id}">
                                                <i class='fas fa-pencil-alt'></i></button>&nbsp
                                           <button class="btn-danger btn eliminarAgrupacion" data-id="${row.id}"><i class="far fa-trash-alt"></i></button>`;
                        return btnAcciones;
                    }
                },
            ],
            initComplete: function (settings, json) {
                tblAgrupacionContratistas.on('click','.editarNombreAgrupacion', function () {
                    let rowData = dtAgrupaciones.row($(this).closest('tr')).data();
                    txtCrearEditarNombreAgrupacion.val(rowData.nomAgrupacion);
                    btnCrearEditarNombreAgrupacion.attr("data-id", rowData.id);
                    btnCrearEditarNombreAgrupacion.attr("text", "Actualizar");
                    lblCrearEditarNombreAgrupacion.text("Actualizar agrupación");
                    mdlCrearEditarNombreAgrupacion.modal("show");
                });

                tblAgrupacionContratistas.on('click','.eliminarAgrupacion', function () {
                    let rowData = dtAgrupaciones.row($(this).closest('tr')).data();
                    Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarAgrupacion(rowData.id));
                });

                tblAgrupacionContratistas.on("click", ".verContratistas", function () {
                    let rowData = dtAgrupaciones.row($(this).closest("tr")).data();
                    fncGetContratistas(rowData.id);
                    btnCrearContratistaEnAgrupacion.attr("data-id", rowData.id);
                    mdlCrearContratistas.modal("show");
                })
            },
            columnDefs: [
                { className: 'dt-center','targets': '_all'}
            ],
        });
    }

    function fncEliminarAgrupacion(idAgrupacion){
        // axios.delete("EliminarAgrupacion", { params: { idAgrupacion: idAgrupacion } }).then(response => {
        let obj = new Object();
        obj = {
            idAgrupacion: idAgrupacion
        };
        axios.post("EliminarAgrupacion", obj).then(response => {
            let { success, items, message } = response.data;
            if (success) { //TODO
                Alert2Exito("Éxito al eliminar la agrupación");
                fncGetAgrupacionesContratistas();
                fncFillCboAgrupaciones();
            }
        }).catch(error => Alert2Error(error.message));
    }

    function fncGetAgrupacionesContratistas(){
        let objData = new Object();
        let idAgrupacion = cboAgrupaciones.val() > 0 ? cboAgrupaciones.val() : 0;
        objData = {
            id: idAgrupacion
        };
        axios.post("GetAgrupacionesContratistas", objData).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                dtAgrupaciones.clear();
                dtAgrupaciones.rows.add(response.data.lstAgrupaciones).draw();
            }
        }).catch(error => Alert2Error(error.message));
    }

    function fncCrearEditarAgrupacionContratistas(){
        let objData = fncGetDatosCrearEditarNombreAgrupacion();
        if (objData != null){
            axios.post("CrearEditarAgrupacion", objData).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al registrar el nombre de la agrupación.");
                    fncGetAgrupacionesContratistas();
                    mdlCrearEditarNombreAgrupacion.modal("hide");
                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    function fncGetDatosCrearEditarNombreAgrupacion(){
        let objData = new Object();
        let strMensajeError = "";
        let idAgrupacion = btnCrearEditarNombreAgrupacion.attr("data-id") > 0 ? btnCrearEditarNombreAgrupacion.attr("data-id") : 0;
        let nomAgrupacion = txtCrearEditarNombreAgrupacion.val() != "" ? txtCrearEditarNombreAgrupacion.val() : strMensajeError = "Es necesario ingresar el nombre de la agrupación.";

        if (strMensajeError != ""){
            Alert2Warning(strMensajeError);
        }else{
            objData = {
                id: idAgrupacion,
                nomAgrupacion: nomAgrupacion
            };
            return objData;
        }
    }

    function initGetContratistas() {
        dtContratistas = tblContratistas.DataTable({
            language: dtDicEsp,
            destroy: false,
            ordering: false,
            paging: true,
            searching: false,
            bFilter: false,
            info: false,
            columns: [
                { data: 'id', title: 'id', visible: false },
                { data: 'nomAgrupacion', title: 'Agrupación' },
                { data: 'nomContratista', title: 'Contratista' },
                {
                    render: function (data, type, row) {
                        return `<button class="btn-danger btn eliminarContratista" data-id="${row.id}"><i class="far fa-trash-alt"></i></button>`
                    }
                }
            ],
            initComplete: function (settings, json) {
                tblContratistas.on('click','.eliminarContratista', function () {
                    let rowData = dtContratistas.row($(this).closest('tr')).data();
                    Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncEliminarContratista(rowData.id));
                });
            },
            columnDefs: [
                { className: 'dt-center','targets': '_all'}
            ],
        });
    }

    function fncGetContratistas(idAgrupacion) {
        let objData = new Object();
        objData = {
            idAgrupacion: idAgrupacion
        };
        axios.post("GetContratistas", objData).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                dtContratistas.clear();
                dtContratistas.rows.add(response.data.lstContratistas).draw();
            }
        }).catch(error => Alert2Error(error.message));
    }

    function fncCrearContratistas() {
        let objData = fncGetDatosCrearContratista();
        if (objData != null) {
            axios.post("CrearContratistaEnAgrupacion", objData).then(response => {
                let { success, items, message } = response.data;
                if (success) {
                    Alert2Exito("Éxito al registrar el contratista en la agrupación.");
                    cboCrearContratistaEnAgrupacion[0].selectedIndex = 0;
                    cboCrearContratistaEnAgrupacion.trigger("change");
                    fncGetContratistas(btnCrearContratistaEnAgrupacion.attr("data-id"));
                }else{
                    Alert2Warning(response.data.message);
                }
            }).catch(error => Alert2Error(error.message));
        }
    }

    function fncGetDatosCrearContratista(){
        let objData = new Object();
        let strMensajeError = "";
        let idAgrupacion = btnCrearContratistaEnAgrupacion.attr("data-id") > 0 ? btnCrearContratistaEnAgrupacion.attr("data-id") : 0;
        let idContratista = cboCrearContratistaEnAgrupacion.val() > 0 ? cboCrearContratistaEnAgrupacion.val() : strMensajeError = "Es necesario seleccionar un contratista.";

        if (strMensajeError != "") {
            Alert2Warning(strMensajeError);
        } else {
            objData = {
                idAgruContratista: idAgrupacion,
                idContratista: idContratista
            };
            return objData;
        }
    }

    function fncEliminarContratista(idAgrupacionDet) {
        // axios.delete("EliminarContratistaEnAgrupacion", { params: { idAgrupacionDet: idAgrupacionDet } }).then(response => {
        let obj = new Object();
        obj = {
            idAgrupacionDet: idAgrupacionDet
        };
        axios.post("EliminarContratistaEnAgrupacion", obj).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                Alert2Exito("Éxito al eliminar el contratista de la agrupación.");
                fncGetContratistas(idAgrupacionDet);
            }
        }).catch(error => Alert2Error(error.message));
    }

    $(document).ready(() => AgrupacionContratistas.Seguridad = new Seguridad())
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop($.unblockUI);
})();