(() => {
$.namespace('Administrativo.Contabilidad.EstimacionProveedor');
    EstimacionProveedor = function (){
        const selEstTm = $('#selEstTm');
        const selEstCC = $('#selEstCC');
        const dpEstMin = $('#dpEstMin');
        const dpEstMax = $('#dpEstMax');
        const selEstTipo = $('#selEstTipo');
        const selEstGiro = $('#selEstGiro');
        const dpEstFecha = $('#dpEstFecha');
        const selEstProv = $('#selEstProv');
        const tblEstProv = $('#tblEstProv');
        const txtEstTotal = $('#txtEstTotal');
        const btnEstBuscar = $('#btnEstBuscar');
        const btnEstLimpiar = $('#btnEstLimpiar');
        const btnEstGuardar = $('#btnEstGuardar');
        const txtEstComentario = $('#txtEstComentario');
        const modalProveedores = $('#modalProveedores');
        const botonProveedores = $('#botonProveedores');
        let init = () => {
            initForm();
            txtEstTotal.change(setMaskTotal);
            btnEstLimpiar.click(setFormDefault);
            btnEstBuscar.click(setBuscarEstProv);
            btnEstGuardar.click(setGuardarEstProv);
            modalProveedores.on("hide.bs.modal", setBuscarEstProv);
            botonProveedores.click(() => modalProveedores.modal());
        }
        guardarEstProv = estProv => $.post('/Administrativo/Propuesta/guardarEstProv', estProv);
        getLstEstProv = new URL(window.location.origin + '/Administrativo/Propuesta/getLstEstProv');
        async function setBuscarEstProv() {
            try {
                dtEstProv.clear().draw();
                response = await ejectFetchJson(getLstEstProv, { min: dpEstMin.val(), max: dpEstMax.val() });
                if (response.success) {
                    setFormDefault();
                    dtEstProv.rows.add(response.lst).draw();
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message); }
        }
        function setGuardarEstProv() {
            let estProv = getFormEst();
                if (esEstProvValida(estProv)) {
                    AlertaAceptarRechazar("Aviso", "¿Desea guardar la estimación?", guardarEstProv, null).then(btn => {
                        guardarEstProv(estProv).then(response => {
                            if (response.success) {
                                AlertaGeneral("Aviso", "Estimación guardada con éxito.");
                                setBuscarEstProv();
                            }
                        });
                    }).catch(o_O => AlertaGeneral("Aviso", o_O.message));
                }
                else {
                    AlertaGeneral("Aviso", "Llene la estimación correctamente.");
                }
        }
        function setMaskTotal() {
            let val = unmaskNumero(txtEstTotal.val());
            txtEstTotal.val(maskNumero2D(val));
        }
        function esEstProvValida({cc, numpro, idGiro, idEst, total, tm}) {
                let esCC = cc.length === 3,
                    esProv = numpro.length > 0,
                    esTotal = total !== 0,
                    esGiro = idGiro > 0,
                    esEst = idEst > 0
                    esTm = tm > 0;
                return esCC && esProv && esGiro && esEst && esTotal && esTm;
        }
        function getFormEst() {
            return {
                id: btnEstGuardar.data().id,
                idGiro: selEstGiro.val(),
                idEst: selEstTipo.val(),
                cc: selEstCC.val(),
                tm: selEstTm.val(),
                numpro: selEstProv.val(),
                fecha: dpEstFecha.val(),
                total: unmaskNumero(txtEstTotal.val()),
                comentarios: txtEstComentario.val(),
            };
        }
        function setForm({id ,cc ,numpro ,idGiro ,fecha ,total ,comentarios ,idEst, tm}) {
            btnEstGuardar.data().id = id;
            selEstCC.val(cc).change();
            selEstTipo.val(idEst);
            selEstProv.val(numpro).change();
            selEstGiro.val(idGiro);
            selEstTm.val(tm);
            txtEstComentario.val(comentarios);
            txtEstTotal.val(maskNumero2D(total));
            dpEstFecha.datepicker("setDate",$.toDate(fecha));
        }
        function setFormDefault() {
            btnEstGuardar.data().id = 0;
            selEstTm.val("");
            selEstCC.val("").change();
            selEstGiro.val("");
            selEstTipo.val("");
            selEstProv.val("").change();
            dpEstFecha.datepicker("setDate", new Date);
            txtEstTotal.val(maskNumero2D(0));
            txtEstComentario.val("");
        }
        async function initForm() {
            txtEstTotal.val(maskNumero2D(0));
            dpEstMin.datepicker().datepicker("setDate", new Date);
            dpEstMax.datepicker().datepicker("setDate", new Date);
            dpEstFecha.datepicker().datepicker("setDate", new Date);
            selEstCC.fillCombo('/Administrativo/Poliza/getCC', null, false, null);
            selEstProv.fillCombo('/Administrativo/Reportes/FillComboProv', null, false, null);
            selEstGiro.fillCombo('/Administrativo/Reportes/FillComboGiro', null, false, null);
            selEstTipo.fillCombo('/Administrativo/Propuesta/getTipoEstimacionProveedor', null, false, null);
            selEstTm.fillCombo('/Administrativo/Poliza/getComboTipoMovimiento', {iSistema: "P"}, false, null);
            selEstCC.select2();
            selEstProv.select2();
            setFormDefault();
            initDataTblEstProv();
            setBuscarEstProv();
        }
        function initDataTblEstProv() {
            dtEstProv = tblEstProv.DataTable({
                destroy: true,
                language: dtDicEsp,
                columns: [
                    { data: 'fecha' ,width: "6%" ,render: (data) => $.toDate(data)},
                    { data: 'descProv' ,width: "18%" ,createdCell: (td, data, rowData) => $(td).html(`${rowData.numpro} - ${data}`)},
                    { data: 'descCC' ,width: "15%" ,createdCell: (td, data, rowData) => $(td).html(`${rowData.cc} - ${data}`)},
                    { data: 'descEst' ,width: "10%" ,createdCell: (td, data, rowData) => $(td).html(`${rowData.idEst} - ${data}`)},
                    { data: 'descGiro' ,width: "10%",createdCell: (td, data, rowData) => $(td).html(`${rowData.idGiro} - ${data}`)},
                    { data: 'descTm' ,width: "10%"},
                    { data: 'total' ,width: "7%" ,class:"text-right" ,render: (data) => maskNumero(data)},
                    { data: 'comentarios'},
                ],
                initComplete: function (settings, json) {
                    tblEstProv.find('tbody').on('click', 'tr', function () {
                        let data = dtEstProv.row(this).data();  
                        setForm(data);  
                    });
                }
            });
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.EstimacionProveedor = new EstimacionProveedor();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();