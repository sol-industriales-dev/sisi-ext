(() => {
$.namespace('Dashboard.Caratulas');
    
    const tblCaratula = $('#tblCaratula');
  
    const cboCaratulas = $('#cboCaratulas');
    const cboProyecto = $('#cboProyecto');
    const cboCostoPor = $('#cboCostoPor');
    const btnBuscarConceptos = $('#btnBuscarConceptos');
    const btnGenerarReporte = $('#btnGenerarReporte');

    let conceptosMoneda = [];
    let TipoCambio = 0;
    let dtCaratula;
    let manoObra;
    const report = $('#report');


    Caratulas = function (){
        let init = () => {
            fillCombos();
            initCapturaCaratula();
            btnGenerarReporte.prop('disabled',true);
        }
        init();
    }

    function fillCombos() {
        cboCaratulas.fillCombo("obtenerComboCaratulras", {}, false);
        let a = $('#cboCaratulas').find('option')
        $(a[1]).val()
        $('#cboCaratulas').val($(a[1]).val())
        cboCaratulas.trigger('change');

        cboProyecto.fillCombo("obtenerCC", {}, false);
        btnBuscarConceptos.click(function () {
            CargarCaratulaActiva();
            btnGenerarReporte.prop('disabled',false);
        })
        btnGenerarReporte.click(function () {
            GenerarReporte();
        })
        cboCostoPor.select2();
    }
    function getParametros() {
        let lstCaratula=[];
        let tr = tblCaratula.find('tr');
        console.log(manoObra)
        if (manoObra) {
            console.log('soy true')
            for (let i = 2; i < tr.length; i++) {
                let td = $(tr[i]).find('td');
                    
               let item ={
                grupo : $(td[0]).text(),
                modelo : $(td[1]).text(),
                depreciacion : $(td[4]).find('input').val(),
                inversion : $(td[5]).find('input').val(),
                seguro : $(td[6]).find('input').val(),
                filtros : $(td[7]).find('input').val(),
                correctivo : $(td[8]).find('input').val(),
                auxiliar : $(td[9]).find('input').val(),
                manoObra : $(td[10]).find('input').val(),
                indirectos : $(td[11]).find('input').val(),
                depreciacionOverhaul : $(td[12]).find('input').val(),
                aceite : $(td[13]).find('input').val(),
                carilleria : $(td[14]).find('input').val(),
                ansul : $(td[15]).find('input').val(),
                utilidad : $(td[16]).find('input').val(),
                costoArrendadora : $(td[17]).find('input').val(),
                tipoCambio : TipoCambio,
                tipoMoneda:$(td[2]).text(),
                tipoHoraDia : $(td[3]).text()

               }
               lstCaratula.push(item);
            }            
        }else{
            for (let i = 2; i < tr.length; i++) {
                let td = $(tr[i]).find('td');
                    
               let item ={
                grupo : $(td[0]).text(),
                modelo : $(td[1]).text(),
                depreciacion : $(td[4]).find('input').val(),
                inversion : $(td[5]).find('input').val(),
                seguro : $(td[6]).find('input').val(),
                filtros : $(td[7]).find('input').val(),
                correctivo : $(td[8]).find('input').val(),
                auxiliar : $(td[9]).find('input').val(),
                manoObra : "",
                indirectos : $(td[10]).find('input').val(),
                depreciacionOverhaul : $(td[11]).find('input').val(),
                aceite : $(td[12]).find('input').val(),
                carilleria : $(td[13]).find('input').val(),
                ansul : $(td[14]).find('input').val(),
                utilidad : $(td[15]).find('input').val(),
                costoArrendadora : $(td[16]).find('input').val(),
                tipoCambio : TipoCambio,
                tipoMoneda :$(td[2]).text(),
                tipoHoraDia : $(td[3]).text()
               }
               lstCaratula.push(item);
            }   
        }
        return lstCaratula;
    }
    function GenerarReporte() {
        let lstCaratula = getParametros();
        console.log(lstCaratula)
        axios.post('/Caratulas/GetReporteCaratulacc', {lstCaratula: JSON.stringify(lstCaratula)})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    if (manoObra) {
                            report.attr(`src`, `/Reportes/Vista.aspx?idReporte=225&isCRModal=${false}&CentroDeCosto=${$('#cboProyecto').find('option:selected').text()}`); 
                            document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    }else{
                            report.attr(`src`, `/Reportes/Vista.aspx?idReporte=226&isCRModal=${false}&CentroDeCosto=${$('#cboProyecto').find('option:selected').text()}`); 
                        document.getElementById('report').onload = function () {
                            $.unblockUI();
                            openCRModal();
                        };
                    }
            }
        });
    }

    function initCapturaCaratula() {
        dtCaratula = tblCaratula.DataTable({
            language: dtDicEsp,
            destroy: false,
            scrollX: false,                
            ordering: false,
            paging: false,
            searching: false,
            bFilter: false,
            info: false,
            columns: [
                {data:"id",title:"id", visible:false},
                {data:"grupo",title:"Grupo"}, 
                {data:"modelo",title:"Modelo"},                   
                {title:"Moneda", render: function (data, type, row, meta){
                        let moneda = '';
                        if (row.IndicadorTipoMoneda) {
                            
                            moneda='<h6>USD</h6>';
                          }else{  
                              moneda='<h6>MXN</h6>';
                            }
                        return moneda;
                    }
                }, 
                { data:"tipoHoraDia", title:"Costo por H/D", render: function (data, type, full, meta){
                        return data;
                    }
                },                                                            
                {
                    data: "depreciacion", 
                    title: "Depreciación<br>", 
                    render: function (data, type, row, meta)
                    {
                        let valor = FormatoMoneda(data);
                        let input ='';
                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto depreciacionDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        } else {
                            input = '<input class="cargaMonto depreciacionMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }
                },
                {data:"inversion",title:"Inversión<br>", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';
                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto inversionDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto inversionMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }
                 },
                {data:"seguro",title:"Seguro<br>", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';
                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto seguroDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto seguroMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }
                 },
                {data:"filtros",title:"Filtros<br>", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';
                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto filtrosDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto filtrosMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }
                },
                {data:"mantenimientoCo",title:"Mant. Correctivo<br>", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto mantenimientoDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto mantenimientoMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }
                 },
                {data:"manoObra",title:"Mano de<br>obra", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto manoObraDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto manoObraMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },
                {data:"auxiliar",title:"Eq. Auxiliar<br>y otros", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto auxiliarDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto auxiliarMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },
                {data:"indirectosMatriz",title:"Indirectos<br>matriz", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto indirectosDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto indirectosMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },
                {data:"depreciacionOH",title:"Depreciacion<br>OverHaul", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto depreciacionOHDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto depreciacionOHMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },
                {data:"aceite",title:"Aceite", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {

                            input = '<input class="cargaMonto aceiteDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto aceiteMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },
                {data:"carilleria",title:"Carilleria o<br>pieza especial", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto carilleriaDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto carilleriaMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },
                {data:"ansul",title:"ANSUL", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto ansulDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto ansulMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },
                {data:"utilidad",title:"Utilidad", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto utilidadDLLS dolares" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto utilidadMXN pesos" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },
                {data:"costoTotal",title:"Costo Renta<br>Arrendadora", render: function (data, type, row, meta){
                        let valor = FormatoMoneda(data);
                        let input ='';

                        if (row.IndicadorTipoMoneda) {
                            input = '<input class="cargaMonto totalDLLS" value=' + valor + ' style="width: 100px;" disabled /><br>'; 
                        }else{
                            input = '<input class="cargaMonto totalMXN" value=' + valor + ' style="width: 100px;" disabled /><br>';  
                        }
                        return input;
                    }  
                },

            ],
            initComplete: function (settings, json) {
                tblCaratula.on('click','.classBtn', function () {
                    let rowData = dtCaratula.row($(this).closest('tr')).data();                       
                });
                tblCaratula.on('click','.classBtn', function () {
                    let rowData = dtnombre.row($(this).closest('tr')).data();
                    //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
                });
                 
           
            
            
            },
            drawCallback: function( settings ) { 
                tblCaratula.find('input.cargaMonto').focus(function(e) {
                    $(this).val(Number($(this).val().replace(/[^0-9.-]+/g,"")));
                });
                tblCaratula.find('input.cargaMonto').blur(function(e) {
                    if($(this).val() == '') $(this).val("0.00");
                    else {
                        let auxValor = parseFloat($(this).val()) || 0;
                        $(this).val('$' + auxValor.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    }

                    if($(this).val() != "0.00") $(this).css("background-color", "#B4C6E7");
                    else $(this).css("background-color", "-internal-light-dark(rgb(255, 255, 255), rgb(59, 59, 59));");
                });
                tblCaratula.find('input.dolares').blur(function(e) {
                    var dolares = Number($(this).val().replace(/[^0-9.-]+/g,""));
                    var pesos = ConvertirAMXN(dolares);
                    $(this).parent().find("input.pesos").val('$' + parseFloat(pesos).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    var totalDLLS = 0;
                    $(this).parent().parent().children().find('input.dolares').each(function(x, data){ totalDLLS += Number(data.value.replace(/[^0-9.-]+/g,"")); });
                    $(this).parent().parent().children().find("input.totalDLLS").val('$' + parseFloat(totalDLLS).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    var totalMXN = 0;
                    $(this).parent().parent().children().find('input.pesos').each(function(x, data){ totalMXN += Number(data.value.replace(/[^0-9.-]+/g,"")); });
                    $(this).parent().parent().children().find("input.totalMXN").val('$' + parseFloat(totalMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                });
                tblCaratula.find('input.pesos').blur(function(e) {
                    var pesos = Number($(this).val().replace(/[^0-9.-]+/g,""));
                    var dolares = ConvertirADLLS(pesos);
                    $(this).parent().find("input.dolares").val('$' + parseFloat(dolares).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    var totalDLLS = 0;
                    $(this).parent().parent().children().find('input.dolares').each(function(x, data){ totalDLLS += Number(data.value.replace(/[^0-9.-]+/g,"")); });
                    $(this).parent().parent().children().find("input.totalDLLS").val('$' + parseFloat(totalDLLS).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                    var totalMXN = 0;
                    $(this).parent().parent().children().find('input.pesos').each(function(x, data){ totalMXN += Number(data.value.replace(/[^0-9.-]+/g,"")); });
                    $(this).parent().parent().children().find("input.totalMXN").val('$' + parseFloat(totalMXN).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
                });
            },
            columnDefs: [
                { className: 'dt-center','targets': '_all'},
                { width: '105px', targets: [4,5,6,7,8,9,10,11,12,13,14,15,16,17] }, 
            ],
        });
    }

    function AddRows(tbl, lst) {
        dt = tbl.DataTable();
        dt.clear().draw();
        dt.draw();
        dt.rows.add(lst).draw(false);
    }

    function CargarCaratulaActiva() {

        //#region SE OBTIENE LISTADO DE REPORTE DE CARATULA
        if (cboProyecto.val() > 0 && cboCostoPor.val() > 0) {
            axios.get('/Caratulas/obtenerCaratula?idCaratula='+ cboCaratulas.val() +'&idCC='+cboProyecto.val()+'&esHoraDia='+cboCostoPor.val())
            .then(response => {
                let {success,items}= response.data;
                if (success) {
                if (items.success) {
                    console.log(items)
                    conceptosMoneda = items.conceptosMoneda;
                    TipoCambio = items.tipoCambio;
                    manoObra = items.IndicadorManoObra;
                    AddRows(tblCaratula,items.data);
                    EliminarRows(items.IndicadorManoObra,
                        items.IndicadorTipoMoneda,
                        items.IndicadorAxuliar,
                        items.IndicadorIndirectos);
                }else{
                    AlertaGeneral(`Alerta`, items.items);
                }
                } else {
                    AlertaGeneral(`Alerta`, response.message);
                }
            }).catch(error => AlertaGeneral(`Alerta`, error.message));
        } else {
            let strMensajeError = "";
            cboProyecto.val() == 0 ? strMensajeError += "Seleccionar un Centro de Costo." : "";
            cboCostoPor.val() == 0 ? strMensajeError += "<br>Seleccionar si es por Día o por Hora." : "";
            Alert2Warning(strMensajeError);
        }
        //#endregion
    }

    function EliminarRows(manoObra,tipoMoneda,auxilar,indicador) {
        var table = tblCaratula.DataTable();
        table.columns([9]).visible(manoObra);

    }
    function ConvertirAMXN(dlls){    
        var dolar = TipoCambio;
        var total = 0;
        total = parseFloat(dlls) * parseFloat(dolar);
        var con = total.toFixed(2);
        return con;
    }

    function ConvertirADLLS(mxn){           
        var pesos = TipoCambio;
        var total = 0;
        total = parseFloat(mxn) / parseFloat(pesos);
        var con = total.toFixed(2);
        return con;
    }
    
    function FormatoMoneda(dato){

        var con = '$' + parseFloat(dato).toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");
        return con;
    }
    
    $(document).ready(() => {
        Dashboard.Caratulas = new Caratulas();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();