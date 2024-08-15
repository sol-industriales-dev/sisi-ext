(() => {
    $.namespace("Maquinaria.Caratula.CaratulaIndicadores");

    CaratulaIndicadores = function () {  
//#region VARIABLES
        const tblCaratulaIndicadores = $('#tblCaratulaIndicadores');
        const btnGuardar = $('#btnGuardar');
        const btnNuevo = $('#btnNuevo');
        const mdlNuevo = $('#mdlNuevo');
        const cboObra = $('#cboObra');
        const cboMoneda = $('#cboMoneda');
        const cboManoObra = $('#cboManoObra');
        const txtAxuliar = $('#txtAxuliar');
        const txtIndirectos = $('#txtIndirectos');
        const btnGuardarTabla = $('#btnGuardarTabla');
        const cboManoObraNuevo = $('#cboManoObraNuevo');
//#endregion
        (function init() { 
            initCaratulaIndicadores();
            MostrarIndicadores();
            fncFillCombos();     
                          

        })();

        function fncFillCombos() {
            cboObra.fillCombo("FillAreasCuentas", {}, false);
            cboObra.select2({
                width: "resolve"
            });
         }

         btnNuevo.click(function(){
            mdlNuevo.modal("show");
         });        
         

         btnGuardar.click(function(){
            fncGuadarIndicador();
            limbiarInputs();
         });

         function TipoMoneda(data){
             console.log(data);
             var tipo;
                if (data == true) 
                {
                    tipo = "DLLS";

                }
                else
                {
                    tipo = "MXN";
                }
                console.log(tipo);
                return tipo;

            }

            function TipoManoObra(data){
                var tipo;
                if (data == true) 
                {
                    tipo = "Con mano de obra";

                }
                else
                {
                    tipo = "Sin mano de obra";
                }
                console.log(tipo);
                return tipo;
            }
        
//#region TABLA CARATULA INDICADORES
function initCaratulaIndicadores() {
    dtCaratulaIndicadores = tblCaratulaIndicadores.DataTable({
        language: dtDicEsp,
        destroy: false,
        ordering: false,
        paging: false,
        ordering: false,
        searching: false,
        bFilter: false,
        info: false,
        columns: [
           {data:"id",title:"id",visible:false},
           {data:"moneda",title:"moneda",visible:false},
           {data:"manoObra",title:"Mano obra",visible:false},
           {data:"areaCuenta",title:"C.C."},
           {data:"descripcion",title:"Obra"},
           {data:"moneda",title:"Tipo de moneda",
                render:function(data,row)
                { 
                    let input =``;
                    input+=`<input  type="text" value="${TipoMoneda(data)}" style="width: 40%;"disabled/>&nbsp;`;          
                    return input;
                }
           },
           {title:"Nuevo tipo de moneda",
                render:function(data,row)
                { 
                    let input =``;
                    input+=`<select id="cboMonedaNuevo" class="form-control cboMonedaNuevo" style="width: 100%;">
                                <option value="">--Seleccione--</option>
                                <option value="true">DLLS</option>
                                <option value="false">MXN</option>
                        </select>`;          
                    return input;
                }
           },
           {data:"manoObra",title:"Tipo de mano de obra",
           
                    render:function(data,row)
                    { 
                        let input =``;
                        input+=`<input  type="text" value="${TipoManoObra(data)}" disabled/>&nbsp;`;          
                        return input;
                    }

           },
           {title:"Nuevo tipo de mano de obra",           
                    render:function(data,row)
                    { 
                        let input =``;
                        input+=`<select id="cboManoObraNuevo" class="form-control cboManoObraNuevo" style="width: 100%;">
                                    <option value="">--Seleccione--</option>
                                    <option value="true">Con mano de obra</option>
                                    <option value="false">Sin mano de obra</option>
                            </select>`;          
                        return input;
                    }        
           },           

           {data:"auxiliar",title:"% Eq. Axuliar y otros",
                render:function(data,row)
                { 
                    let input =``;
                    input+=`<input  type="text" value="${data}" style="width: 30%;" disabled/>&nbsp;`;          
                    return input;
                }

           },
           {title:"Nuevo % Eq. Axuliar y otros",
                    render:function(data,row)
                    { 
                        let input =``;
                        input+=`<input id="axuliarNuevo" class="axuliarNuevo" type="text" style="width: 30%;"/>&nbsp;`;          
                        return input;
                    }
           },
           {data:"indirectos",title:"% Indirectos Matriz",
               render:function(data,row)
               { 
                   let input =``;
                   input+=`<input  type="text" value="${data}" style="width: 30%;" disabled/>&nbsp;`;          
                   return input;
               }

           },
           {title:"% Indirectos Matriz",
               render:function(data,row)
               { 
                   let input =``;
                   input+=`<input id="indirectosNuevo" class="indirectosNuevo" type="text" style="width: 30%;"/>&nbsp;`;          
                   return input;
               }
           },
           {data:"indirectos",title:"% Indicador matriz",visible:false},
        ],
        initComplete: function (settings, json) {
            tblCaratulaIndicadores.on('click','.editar', function () {
            });
            tblCaratulaIndicadores.on('click','.classBtn', function () {
                let rowData = dtCaratulaIndicadores.row($(this).closest('tr')).data();
                //Alert2AccionConfirmar('¡Cuidado!','¿Desea eliminar el registro seleccionado?','Confirmar','Cancelar', () => fncAccionConfirmar(param));
            });
        },
        columnDefs: [
            { className: 'dt-center','targets': '_all'}
        ],
    });
}
//#endregion

//#region MOSTRAR DATOS
function MostrarIndicadores() {          
    axios.post("GetIndicadores").then(response => {
        let { success, items, message } = response.data;
        console.log(response);
        if (success) {                 
            dtCaratulaIndicadores.clear().draw();
            dtCaratulaIndicadores.rows.add(response.data.lstIndicadores).draw();                    
        }
        else {
            Alert2Error(message)
        }

    }).catch(error => Alert2Error(error.message));
}
//#endregion

//#region CREAR INDICADORES
function limbiarInputs() {
    cboObra.val("");
    cboObra.trigger("change");
    cboManoObra.val("");
    cboManoObra.trigger("change");
    cboMoneda.val("");
    cboMoneda.trigger("change");    
    txtAxuliar.val("");    
    txtIndirectos.val("");    
}

function fncCamposVacios() {
    let vacio = false;
    cboObra.val() == "--Seleccione--" ? vacio = true : vacio = false;
    cboMoneda.val() == "--Seleccione--" ? vacio = true : vacio = false;
    cboManoObra.val() == "--Seleccione--" ? vacio = true : vacio = false;   
    txtAxuliar.val() == "" ? vacio = true : vacio = false;        
    txtIndirectos.val() == "" ? vacio = true : vacio = false; 
    return vacio;
}  


function objFiltros(){

        let obj = new Object();
        let idCC = cboObra.val();
        let manoObra;
        if (cboManoObra.val() == 'true') {
            manoObra = true;
        }
        else{
            manoObra = false;
        }
        let moneda;
        if (cboMoneda.val() == 'true') {
            moneda = true;
        }
        else{
            moneda = false;
        }        
        let auxiliar = txtAxuliar.val();
        let indirectos = txtIndirectos.val();    


        obj = {
            idCC: idCC,
            manoObra: manoObra,
            moneda: moneda,           
            auxiliar: auxiliar,
            indirectos: indirectos
        };
        return obj;    
}

function fncGuadarIndicador() {
    let parametros = objFiltros();
    if (fncCamposVacios()) {
        Alert2Warning("Favor de llenar todos los controles");
    } else {
        axios.post('/Caratulas/GuardarIndicadores', parametros).then(response => {
            let { success, items, message } = response.data;                    
            if (success) {                                   
                MostrarIndicadores();
                mdlNuevo.modal("hide");
                Alert2Exito("se registro con exito");
                limbiarInputs();                                                
            }
        }).catch(error => Alert2Error(error.message));
    }
}

btnGuardarTabla.click(function(){
    fncGuadarIndicadoresNuevos();
});

function crear(){
    let lstNuevoIndicadores =[];
    $("#tblCaratulaIndicadores").find('tbody').find('tr').each(function (index, value) {
        let rowData = dtCaratulaIndicadores.row($(this).closest('tr')).data();
        console.log(rowData);
        let item ={
            id : rowData.id,
            moneda : $(value).find('.cboMonedaNuevo').val() == "" ? rowData.moneda : $(value).find('.cboMonedaNuevo').val() ,
            manoObra : $(value).find('.cboManoObraNuevo').val() == "" ? rowData.manoObra : $(value).find('.cboManoObraNuevo').val(),
            auxiliar : $(value).find('.axuliarNuevo').val() == "" ? rowData.auxiliar : $(value).find('.axuliarNuevo').val(),
            indirectos : $(value).find('.indirectosNuevo').val() == "" ? rowData.indirectos : $(value).find('.indirectosNuevo').val(),
        }   
        lstNuevoIndicadores.push(item);         
    });
    console.log(lstNuevoIndicadores)
    return lstNuevoIndicadores;
}

function fncGuadarIndicadoresNuevos() {    
    let lstNuevoIndicadores = crear();
        axios.post('/Caratulas/ActualizarIndicadoresNuevos', {lstNuevoIndicadores:lstNuevoIndicadores}).then(response => {
            let { success, items, message } = response.data;                    
            if (success) {                                   
                MostrarIndicadores();                                                             
            }
        }).catch(error => Alert2Error(error.message));
    
}
//#endregion
      
}
    $(document).ready(() => CaratulaIndicadores = new CaratulaIndicadores())
        .ajaxStart(() => { $.blockUI({ message: "Procesando..." }); })
        .ajaxStop(() => { $.unblockUI(); });
})();