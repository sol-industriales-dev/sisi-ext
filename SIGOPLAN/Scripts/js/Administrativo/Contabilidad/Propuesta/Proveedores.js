(() => {
    $.namespace('Administrativo.Contabilidad.Proveedores');
    Proveedores = function () {
        const selProvTm = $('#selProvTm');
        const selProvCC = $('#selProvCC');
        const txtProvMax = $('#txtProvMax');
        const txtProvMin = $('#txtProvMin');
        const botonBuscar = $('#botonBuscar');
        const botonGuardar = $('#botonGuardar');
        const btnProvLimit = $('#btnProvLimit');
        const inputGroupBtn = $('.input-group-btn');
        const modalProveedores = $('#modalProveedores');
        const tablaProveedores = $('#tablaProveedores');
        const botonProveedores = $('#botonProveedores');
        const dpFechaPropuesta = $('#dpFechaPropuesta');
        const inputFechaBusqueda = $('#inputFechaBusqueda');
        let dtProveedores ,itemsGiro ,startDate ,endDate ,defColor = "#ddd122" 
        ,inputIds = $('input').map(function() {
            return this.id;
        }).get().join(' ');
        
        function init() {
            initForm();
            btnProvLimit.click(setDataLimit);
            botonBuscar.click(cargarResumenProveedores);
            botonProveedores.click(() => modalProveedores.modal());
            botonGuardar.click(guardarProveedores);
            modalProveedores.on("hide.bs.modal", cargarResumenProveedores);
            inputGroupBtn.click(chngSetAllSelOpt);
            inputGroupBtn.each((i ,btn) => $(btn).click() );
        }
        const FillComboGiro = new URL(window.location.origin + '/Administrativo/Reportes/FillComboGiro');
        const getLimitNoProveedores = new URL(window.location.origin + '/Administrativo/Propuesta/getLimitNoProveedores');
        const getLstAnaliticoVencimiento6Col = new URL(window.location.origin + '/Administrativo/Propuesta/getLstAnaliticoVencimiento6Col');
        let guardarCondensadoSaldos = listaProveedores => $.post('/Administrativo/Propuesta/guardarCondensadoSaldos', { listaProveedores });
        async function setItemsGiro (){
            response = await ejectFetchJson(FillComboGiro);
            if (response.success)
                itemsGiro = response.items;
        }
        async function getLimitProv() {
            try {
                response = await ejectFetchJson(getLimitNoProveedores);
                if (response.success) {
                    txtProvMin.val(response.limit[0]);
                    txtProvMin.data().prov = response.limit[0];
                    txtProvMax.val(response.limit[1]);
                    txtProvMax.data().prov = response.limit[1];
                }   
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message);}
        }
        function chngSetAllSelOpt() {
            let estodo = !this.value,
                select = $(this).next().find("select[multiple]");
            this.value = estodo;
            limpiarMultiselect(select);
            if (estodo) {
                let lstValor = $(`#${select.attr("id")}`).find(`option`).toArray().map(option => option.value);
                select.val(lstValor);
                convertToMultiselect(select);
            }
        }
        function setDataLimit() {
            txtProvMin.val(txtProvMin.data().prov);
            txtProvMax.val(txtProvMax.data().prov);
        }
        setSemanaReservaSelecionada = () => {
            let date = dpFechaPropuesta.datepicker('getDate'),
                prevDom = date.getDate() - (date.getDay() + 7) % 7,
                startDate = new Date(date.getFullYear(), date.getMonth(), prevDom),
                endDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate() - startDate.getDay() + 6);
                dpFechaPropuesta.val(`${startDate.toLocaleDateString()} - ${endDate.toLocaleDateString()}`)
            selectCurrentWeek();
        }
        var selectCurrentWeek = function () {
            window.setTimeout(function () {
                dpFechaPropuesta.find('.ui-datepicker-current-day a').addClass('ui-state-active');
            }, 1);
        }
        function initDatepicker() {
            inputFechaBusqueda.datepicker({
                beforeShow: function(input, inst) {
                    $('#ui-datepicker-div').removeClass(inputIds);
                    $('#ui-datepicker-div').addClass(this.id);
                }
            }).datepicker("setDate", new Date());
            dpFechaPropuesta.datepicker({
                firstDay: 0,
                showOtherMonths: true,
                selectOtherMonths: true,
                onSelect: function (dateText, inst) {
                    setSemanaReservaSelecionada();
                },
                beforeShowDay: function (date) {
                    var cssClass = '';
                    if (date >= startDate && date <= endDate)
                        cssClass = 'ui-datepicker-current-day';
                    return [true, cssClass];
                },
                onChangeMonthYear: function (year, month, inst) {
                    selectCurrentWeek();
                },
                beforeShow: function(input, inst) {
                    $('#ui-datepicker-div').removeClass(inputIds);
                    $('#ui-datepicker-div').addClass(this.id);
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 9999);
                    }, 0);                    
                }
            }).datepicker('setDate', new Date());
            setSemanaReservaSelecionada();
        }
        async function cargarResumenProveedores() {
            try {
                dtProveedores.clear();
                response = await ejectFetchJson(getLstAnaliticoVencimiento6Col, {
                    fecha: inputFechaBusqueda.datepicker("getDate")
                   ,lstCC: selProvCC.val()
                   ,lstTm: selProvTm.val()
                   ,provMin: txtProvMin.val()
                   ,provMax: txtProvMax.val()
                });
                if (response.success) {
                    dtProveedores.rows.add(response.lst).draw();
                } else {
                    AlertaGeneral('Aviso', 'Ocurrió un error al intentar obtener los proveedores.');
                }
            } catch (o_O) {
                AlertaGeneral('Aviso', o_O.message);
            }
        }
        function initDtProveedores() {
            dtProveedores = tablaProveedores.DataTable({
                order: []
                ,destroy: true
                ,ordering:false
                ,language: dtDicEsp
                ,columns: [
                    { title: 'Giro' ,data: 'idGiro' ,width: "12%" ,createdCell: (td, data) => {
                         $(td).html(crearSelGiro(data))
                         if (data > 0) {
                            $(td).closest('tr').css("background-color" ,defColor);
                         }
                        } },
                    { title: 'Proveedor' ,data: 'numpro' ,width: "15%" ,render: (data, type, row) => `${data} - ${row.proveedor}` },
                    { title: 'Centro Costos' ,data: 'cc' ,width: "15%", render: (data, type, row) => `${data} - ${row.descCC}` },
                    { title: 'Factura' ,data: 'factura', width: "5%", sClass: "text-right"},
                    { title: 'Tipo Movimiento' ,data: 'tipoMov'},
                    { title: 'Fecha' ,data: 'fechaFactura', width: "5%"},
                    { title: 'Vence' ,data: 'fechaVence', width: "5%" },
                    { title: 'Por Vencer' ,data: 'porVencer', width: "5%", sClass: "text-right", render: data => maskNumero(data) },
                    { title: '1-7 Días' ,data: 'dias7', width: "5%", sClass: "text-right", render: data => maskNumero(data) },
                    { title: '8-14 Días' ,data: 'dias14',  width: "5%", sClass: "text-right", render: data => maskNumero(data) },
                    { title: '15-30 Días' ,data: 'dias30', width: "5%", sClass: "text-right", render: data => maskNumero(data) },
                    { title: '31-45 Días' ,data: 'dias45', width: "5%", sClass: "text-right", render: data => maskNumero(data) },
                    { title: '46-60 Días' ,data: 'dias60', width: "5%", sClass: "text-right", render: data => maskNumero(data) },
                    { title: 'Más de 60 Días' ,data: 'dias61', width: "5%", sClass: "text-right", render: data => maskNumero(data) },
                    { title: 'Total' ,data: 'total', width: "5%", sClass: "text-right", render: data => maskNumero(data) }
                ]
                ,initComplete: function (settings, json) {
                    tablaProveedores.on('change', 'select', function () {
                        let row = $(this).parent().parent()
                            data = dtProveedores.row(row).data()
                           ,value = this.value
                           ,color = this.value > 0 ? defColor : "";
                        row.css("background-color" ,color);
                    });
                }
            });
        }
        function crearSelGiro(value){
            let select = $(`<select>`)
            select.addClass(`form-control idGiro`);
            select.fillComboItems(itemsGiro, "", value === 0 ? "" : value);
            return select;
        }
        function getLstProv() {
            let lst = [];
            dtProveedores.rows().iterator('row', function (context, index) {
                let data = this.row(index).data(),
                    node = $(this.row(index).node());
                    data.idGiro = +(node.find(`select`).val());
                data.esPropuesta = data.idGiro > 0;
                data.fechaFactura = $.toDate(data.fechaFactura);
                data.fechaVence = $.toDate(data.fechaVence);
                data.fechaPropuesta = dpFechaPropuesta.val();
                if (data.esPropuesta) {
                    lst.push(data);   
                }
            });
            return lst;
        }
        function guardarProveedores() {
            let lstProv = getLstProv();
            if (lstProv.length === 0) {
                return;
            }
            guardarCondensadoSaldos(lstProv)
                .then(response => {
                    if (response.success) {
                        AlertaGeneral('Éxito', 'Cambios guardados correctamente.');
                    } else {
                        AlertaGeneral('Aviso', 'Ocurrió un error al intentar guardar los cambios.')
                    }
                }, () => AlertaGeneral('Aviso', 'Ocurrió un error al intentar guardar los cambios.'));
        }
        async function initForm() {
            getLimitProv();
            selProvCC.fillCombo('/Administrativo/Poliza/getCC', null, true, null);
            selProvTm.fillCombo('/Administrativo/Poliza/getComboTipoMovimiento', {iSistema: "P"}, true, null);
            convertToMultiselect(selProvCC);
            convertToMultiselect(selProvTm);
            await setItemsGiro();
            initDatepicker();
            initDtProveedores();
        }
        init();
    }
    $(document).ready(() => {
        Administrativo.Contabilidad.Proveedores = new Proveedores();
    })
        .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        .ajaxStop(() => $.unblockUI());
})();