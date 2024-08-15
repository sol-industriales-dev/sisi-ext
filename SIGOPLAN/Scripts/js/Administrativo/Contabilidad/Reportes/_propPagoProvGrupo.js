(() => {
    $.namespace('Administrativo.Contabilidad.Reportes._propPagoProvGrupo');
    _propPagoProvGrupo = function () {
        const txtGiroDesc = $('#txtGiroDesc');
        const tblGrupoGiro = $('#tblGrupoGiro');
        const fldGiroGrupo = $('#fldGiroGrupo');
        const chbGiroGrupo = $('#chbGiroGrupo');
        const selGiroPadre = $('#selGiroPadre');
        const chbGiroEstatus = $('#chbGiroEstatus');
        const btnGiroGrupoGuardar = $('#btnGiroGrupoGuardar');
        const btnGiroGrupoLimpiar = $('#btnGiroGrupoLimpiar');
        let init = () => {
            initForm();
            txtGiroDesc.change(esGrupoGiroValido);
            chbGiroGrupo.change(showFieldGrupoGiro);
            btnGiroGrupoGuardar.click(guardarGiroGrupo);
            btnGiroGrupoLimpiar.click(setFormGiroDefault);
        }
        const txtGiroDefault = '-- Nuevo Grupo --';
        const saveGiro = new URL(window.location.origin + '/Administrativo/Reportes/saveGiro');
        const getLstGiro = new URL(window.location.origin + '/Administrativo/Reportes/getLstGiro');
        const FillComboGiro = new URL(window.location.origin + '/Administrativo/Reportes/FillComboGiro');
        async function setItemsGiro (){
            response = await ejectFetchJson(FillComboGiro);
            if (response.success) {
                itemsGiro = response.items;
            }
        }
        async function setLstGiro() {
            try {
                dtGrupoGiro.rows().clear().draw();
                let response = await ejectFetchJson(getLstGiro);
                if (response.success) {
                    dtGrupoGiro.rows.add(response.lst).draw();
                }
            } catch (o_O) { console.log(o_O.message); }
        }        
        async function guardarGiroGrupo() {
            try {
                if (esGrupoGiroValido()) {
                    let obj = getFormGiro(),
                        response = await ejectFetchJson(saveGiro, obj);
                    if (response.success) {
                        refreshSelGiro();
                        setFormGiroDefault();
                        setLstGiro();
                        setLstGiroProv();
                        AlertaGeneral("Aviso", "Giro guardado con éxito");
                    }
                }
            } catch(o_O){ console.log(o_O.message); }
        }
        function getFormGiro() {
            return {
                id: txtGiroDesc.data().id,
                descripcion: txtGiroDesc.val(),
                esActivo: chbGiroEstatus.checked
            };
        }
        function setFormGiroDefault() {
            txtGiroDesc.val("");
            txtGiroDesc.data().id = 0;
            chbGiroEstatus.checked = true;
            selGiroPadre.val(txtGiroDefault);
            txtGiroDesc.removeClass('errorClass');
            tblGrupoGiro.find('tbody tr.selected').removeClass('selected');
        }
        function setFormGiroData({ id, descripcion, esActivo }) {
            txtGiroDesc.val(descripcion);
            selGiroPadre.val(id);
            txtGiroDesc.data().id = id;
            chbGiroEstatus.checked = esActivo;
        }
        function showFieldGrupoGiro() {
            fldGiroGrupo.toggle("slow");
        }
        function esGrupoGiroValido() {
            txtGiroDesc.removeClass('errorClass');
            let esValido = false;
            esValido = !$.esStringVacio(txtGiroDesc.val());
            if (!esValido) {
                txtGiroDesc.addClass('errorClass');
            }
            return esValido;
        }
        function refreshSelGiro() {
            selGiroPadre.fillCombo(FillComboGiro, null, false, txtGiroDefault);
        }
        function initDataTblGrupoGiro() {
            dtGrupoGiro = tblGrupoGiro.DataTable({
                info: false,
                paging: false,
                sortable: false,
                searching: false,
                language: dtDicEsp,
                iDisplayLength: -1,
                columns: [
                    {
                        data: 'esActivo', width: '5%', sortable: false, createdCell: function (td, data, rowData, row, col) {
                            $(td).html(data ? "Activo" : "No activo").addClass(data ? "success" : "danger");
                        }
                    },
                    { data: 'descripcion', sortable: false, }
                ],
                initComplete: function (settings, json) {
                    tblGrupoGiro.find('tbody').on('click', 'tr', function () {
                        if ($(this).hasClass('selected')) {
                            $(this).removeClass('selected');
                            setFormGiroDefault();
                        }
                        else {
                            tblGrupoGiro.find('tr.selected').removeClass('selected');
                            $(this).addClass('selected');
                            let data = dtGrupoGiro.row(this).data();
                            if (data !== undefined) {
                                setFormGiroData(data);
                            }
                        }
                    });
                }
            });
        }
        async function initForm() {
            await setItemsGiro();
            refreshSelGiro();
            setFormGiroDefault();
            initDataTblGrupoGiro();
            setLstGiro();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Reportes._propPagoProvGrupo = new _propPagoProvGrupo();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();