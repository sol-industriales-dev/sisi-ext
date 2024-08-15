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
        const getTblPuesto = new URL(window.location.origin + '/Administrativo/Bono/getTblPuesto_Cuadrado');
        const GuardarPlantilla = new URL(window.location.origin + '/Administrativo/Bono/GuardarPlantilla_Cuadrado');
        const getcboTipoNomina = new URL(window.location.origin + '/Administrativo/Bono/getcboTipoNomina');
        let init = () => {
            InitForm();
            btnGuardar.click(setGuardarBono);
            selBuscar.change(CargarTblPuestos);
        }
        async function setGuardarBono() {
            try {
                let plan = getPLantilla()
                    , lst = getLstBonos();
                if (plan.cc.length === 3) {
                    if (lst.length > 0) {
                        response = await ejectFetchJson(GuardarPlantilla, { plan, lst,isGestion });
                        if (response.success) {
                            AlertaGeneral("Aviso", `Bonos guardados con éxito.`);
                            CargarTblPuestos();
                        } else {
                            AlertaGeneral(`Error`, `Ocurrió un error al intentar guardar la plantilla.`);
                        }
                    } else {
                        AlertaGeneral("Aviso", "Escriba todos los datos del bono.");
                    }
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
                    { title:'#' , data: 'empleadoID'},
                    { title:'Empleado' , data: 'empleado'},
                    { title:'PuestoID' , data: 'puestoID', visible:false},
                    { title:'Puesto' , data: 'puesto'},
                    { title:'Tipo Nomina ID' , data: 'tipoNominaID', visible:false},
                    { title:'Tipo Nomina' , data: 'tipoNomina'},
                    {
                        title:'Monto' , data: 'monto', createdCell: (td, data, rowData, row, col) => {
                            $(td).html(`<input>`);
                            $(td).find(`input`).addClass(`form-control monto text-right`);
                            $(td).find(`input`).val(maskNumero(data));
                        }
                    },
                    {
                        title:'Salario' , data: 'salario', createdCell: (td, data, rowData, row, col) => {
                            $(td).html(`<span>`);
                            $(td).find(`span`).addClass(`form-control salario text-right`);
                            $(td).find(`span`).html(maskNumero(data));
                        }
                    },
                    { title:'DeptoID' , data: 'deptoID', visible:false},
                    { title:'Depto' , data: 'depto'},
                ]
                , initComplete: function (settings, json) {
                    tblPuestos.on(`change`, `.monto`, function () {
                        let valor = unmaskNumero(this.value);
                        this.value = maskNumero(valor);
                    });
                }
            });
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
                    tipoNominaCve: data.tipoNominaID,
                    depto: data.deptoID,
                    deptoNombre: data.depto,
                    empleado: data.empleadoID,
                    empleadoNombre: data.empleado
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