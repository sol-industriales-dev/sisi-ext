(() => {
    $.namespace('Administrativo.RecursosHumanos.BonoAdministrativo');
    BonoAdministrativo = function () {
        let itemPeriodo;
        let isGestion = false;
        const cboCC = $("#cboCC");
        const mpFin = $('#mpFin');
        const mpInicio = $('#mpInicio');
        const txtFecha = $("#txtFecha");
        const selBuscar = $('.selBuscar');
        const btnGuardar = $('#btnGuardar');
        const tblPuestos = $("#tblPuestos");
        const divAceptacion = $('#divAceptacion');
        const autoInput = $('.ui-autocomplete-input');
        const getTblPuesto = new URL(window.location.origin + '/Administrativo/Bono/getTblPuesto');
        const GuardarPlantilla = new URL(window.location.origin + '/Administrativo/Bono/GuardarPlantilla');
        const getcboTipoNomina = new URL(window.location.origin + '/Administrativo/Bono/getcboTipoNomina');
        let init = () => {
            InitForm();
            btnGuardar.click(setGuardarBono);
            selBuscar.change(CargarTblPuestos);
        }
        async function setGuardarBono() {
            try {
                let plan = getPLantilla()
                    , lst = getLstBonos()
                    , lstAuth = getLstAuh();
                if (plan.cc.length === 3) {
                    if (lstAuth.length > 0) {
                        if (lst.length > 0) {
                            response = await ejectFetchJson(GuardarPlantilla, { plan, lst, lstAuth,isGestion });
                            if (response.success) {
                                AlertaGeneral("Aviso", `Bonos guardados con éxito.`);
                                CargarTblPuestos();
                            } else {
                                AlertaGeneral(`Error`, `Ocurrió un error al intentar guardar la plantilla.`);
                            }
                        } else {
                            AlertaGeneral("Aviso", "Escriba todos los datos del bono.");
                        }
                    } else { AlertaGeneral("Aviso", "El Vobo y el Solicitante es requerido."); }
                }
                else {
                    AlertaGeneral("Aviso", "Selecciones centro de costo válido")
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        async function CargarTblPuestos() {
            try {
                let cc = cboCC.val()
                divAceptacion.addClass("hidden");
                dtPuestos.clear().draw();
                if (cc.length > 0) {
                    response = await ejectFetchJson(getTblPuesto, { cc , isGestion });
                        mpInicio.val(response.fecha.fechaInicio);
                        mpFin.val(response.fecha.fechaFin);
                        dtPuestos.rows.add(response.lst).draw();
                        divAceptacion.removeClass("hidden");
                        setLstAuth(response.lstAuth);
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        async function setItemPeriodo() {
            try {
                response = await ejectFetchJson(getcboTipoNomina);
                if (response.success) {
                    itemPeriodo = response.items;
                }
            } catch (o_O) { }
        }
        function InitTblPuestos() {
            dtPuestos = tblPuestos.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                columns: [
                    { title:'#' , data: 'puestoID'},
                    { title:'Puesto' , data: 'puesto'},
                    { title:'Tipo Nomina' , data: 'tipoNomina'},
                    {
                        title:'Monto' , data: 'monto', createdCell: (td, data, rowData, row, col) => {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control monto text-right`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    }
                    //{
                    //    title:'Periodicidad' , data: 'periodicidad', createdCell: (td, data, rowData, row, col) => {
                    //        $(td).html(`<select>`);
                    //        $(td).find(`select`).fillComboItems(itemPeriodo, null, data === 0 ? null : data);
                    //        $(td).find(`select`).addClass(`form-control periodicidad`);
                    //    }
                    //}
                ]
                , initComplete: function (settings, json) {
                    tblPuestos.on(`change`, `.monto`, function () {
                        let valor = unmaskNumero(this.value);
                        this.value = maskNumero(valor);
                    });
                }
            });
        }
        function setLstAuth(lst) {
            autoInput.each(function (i) {
                let inp = $(this)
                    , auth = lst[i];
                if (auth !== undefined) {
                    inp.val(auth.aprobadorNombre);
                    inp.data().idUsuario = auth.aprobadorClave;
                }
            });
        }
        function getLstAuh() {
            let lst = []
                , esBreak = false;;
            autoInput.each(function (i) {
                let inp = $(this)
                    , esReq = inp.hasClass("requerido")
                    , esNombre = inp.val().length > 0
                    , tipo = esReq ? 0 : 1;
                if (esReq) {
                    if (!esNombre) {
                        esBreak = true;
                        return false;
                    }
                }
                if (esNombre) {
                    let aprobadorClave = inp.data().idUsuario
                        , aprobadorNombre = inp.val()
                        , orden = i;
                    lst.push(auth = { aprobadorClave, aprobadorNombre, orden, tipo });
                }
            });
            return esBreak ? [] : lst;
        }
        function getPLantilla() {
            let cc = cboCC.val();
            return {
                cc: cc
                , ccNombre: cboCC.find(`option[value="${cc}"]`).text()
                , fechaCaptura: txtFecha.datepicker("getDate")
                , fechaInicio: mpInicio.val()
                , fechaFin: mpFin.val()
            };
        }
        function getLstBonos() {
            let lst = [];
            dtPuestos.rows().every(function () {
                let node = $(this.node())
                    , data = getDetalles(node);
                if (ValidarDetalles(data)) {
                    lst.push(data);
                }
            });
            return lst;
        }
        function getDetalles(row) {
            let data = dtPuestos.row(row).data()
                , objBono = {
                    puesto: data.puestoID,
                    puestoNombre: data.puesto,
                    monto: unmaskNumero(row.find('.monto').val()),
                    periodicidad: 5 /*row.find('.periodicidad').val()*/,
                    tipoNominaCve: data.tipoNominaCve 
                };
            return objBono;
        }
        function ValidarDetalles(obj) {
            bandera = true;
            if (obj.monto <= 0) {
                bandera = false;
            }
            if (obj.periodicidad <= 0) {
                bandera = false;
            }
            return bandera;
        }
        function eventAutoAuth(event, ui) {
            let inp = $(this);
            inp.val(ui.item.label);
            inp.data().idUsuario = ui.item.id;
        }
        function desdeGestion() {
            let cc = localStorage.getItem("cc");
            if (cc !== null) {
                cboCC.val(cc);
                isGestion = true;
                CargarTblPuestos();
                localStorage.removeItem("cc");
            }
        }

        function toDateFromJson(src) {
            let strfecha = $.toDate(src).split("/")
                , fecha = new Date(+strfecha[2], +strfecha[1], +strfecha[0]);
            return fecha;
        }
        function fnNoGestion(){
            isGestion = false;
        }
        function InitForm() {
            let ahora = new Date();
            $(".fechaMesAnio").MonthPicker({
                Button: false,
                MonthFormat: 'MM, yy',
                i18n: mpDicEsp
            });
            mpInicio.datepicker().datepicker("setDate", new Date());
            mpFin.datepicker().datepicker("setDate", new Date());
            txtFecha.datepicker().datepicker("setDate", ahora);
            cboCC.fillCombo('/Administrativo/Bono/getTblP_CC', null, false);
            cboCC.change(fnNoGestion);
            autoInput.getAutocomplete(eventAutoAuth, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            setItemPeriodo();
            InitTblPuestos();
            desdeGestion();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.RecursosHumanos.BonoAdministrativo = new BonoAdministrativo();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
        .ajaxStop(() => { $.unblockUI(); });
})();