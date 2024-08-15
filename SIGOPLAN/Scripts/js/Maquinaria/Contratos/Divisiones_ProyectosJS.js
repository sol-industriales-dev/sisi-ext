(() => {

$.namespace('Contratos.Divisiones_Proyectos');
const tblAF_DxP_Divisiones_Proyecto = $('#tblAF_DxP_Divisiones_Proyecto');
const cmbcc = $('#cmbCC');
const cmbbDivision =$("#cmbDivision");
const cboIsAdmin =$("#cboIsAdmin");
const cboAdministrador = $('#cboAdministrador');
const btnBuscar = $('#btnBuscar');
const btnLimpiar = $('#btnLimpiar');
const btnGuadarEditarDivisiones = $("#btnGuadarEditarDivisiones");
const btnNuevoDivision = $("#btnNuevoDivision");
const cboCC =$("#cboCC");
const cboDivision =$("#cboDivision");
let dtDivisiones_Proyectos;


    Divisiones_Proyectos = function () {
        (function init() {
            initDivisiones_Proyectos();
            fncGetDivisiones_Proyectos();
            fncListeners();  
        })();
    }

    function fncListeners() {
        // axios.get('GetCCmodal').then(response => {
        //     let { success, items, message } = response.data;
        //  console.log(items)
        //     if (success) {
        //         cboCC.append('<option value="--Seleccione--">--Seleccione--</option>');
        //         let groupOption=``;
        //         items.forEach(x => {
        //                 groupOption += `<option value="${x.Value}">${x.Text}</option>`;
        //         });
        //         cboCC.append(groupOption);

        //     } else {
        //         AlertaGeneral(`Alerta`, message);
        //     }
        // }).catch(error => AlertaGeneral(`Alerta`, error.message));

            // cboCC.fillCombo("/BackLogs/FillCboCC", { areaCuenta: areaCuenta }, false);
            cmbcc.fillCombo('GetCC',null,false);
            cmbbDivision.fillCombo("GetCmbDivision",null,false);
            cboCC.fillCombo('GetCC',null,false);
            cboDivision.fillCombo("GetCmbDivision",null,false);
            
            cmbcc.select2();
            cmbbDivision.select2();
            cboIsAdmin.select2();
            cboCC.select2();
            cboDivision.select2();
            cboAdministrador.select2();

            btnBuscar.click(function(){             
                fncGetDivisiones_ProyectosFiltro();
            });

            btnLimpiar.click(function(){
                
                fncLimpiarCombos();               
            });

            btnNuevoDivision.click(function () {
                $(".modal-title").text("Nueva Division");   
                fncLimpiarFormulario();             
                cboCC.attr("data-id", 0);
            });
            btnGuadarEditarDivisiones.click(function () {
                if (cboCC.attr("data-id") == 0) {
                    fncGuadarDivisionesProyectos();
    
                } else {
                    fncEditarDivisionesProyectos();
                    
                }
    
            });
    }


    function initDivisiones_Proyectos() {
        dtDivisiones_Proyectos = tblAF_DxP_Divisiones_Proyecto.DataTable({
            language: dtDicEsp,
            destroy: false,
            ordering: false,
            paging: true,
            searching: false,
            bFilter: true,
            info: false,
            columns: [                
                { data: 'descripcionCC', title: 'C.C.'},
                { data: 'nombreDivision', title: 'Division:' },
                {data: 'abreviacion', title:'Abreviacion:'},               
                {data:'esAdmin', title:'Admin u Obras:'},                                
                {
                    data: " ", render: (data, type, row) => {
                        let botones = ``;
                        botones += `<button type="button" class="btn btn-warning EditarDivisionesProyectos" data-id="${row.id}"><i class="fa fa-pen"></i></button>&nbsp;`;
                        botones += `<button type="button" class="btn btn-danger eliminarDivision"  data-id="${row.id}"><i class="fa fa-trash"></i></button>`;
                        return botones;
                    }
                }

                //render: function (data, type, row) { }

            ],
            initComplete: function (settings, json) {
                tblAF_DxP_Divisiones_Proyecto.on('click', '.EditarDivisionesProyectos', function () {
                    $("#mdlNuevaDivision").modal('show');                    
                    let rowData =  dtDivisiones_Proyectos.row($(this).closest("tr")).data();
                    let id = parseFloat(rowData.id);
                    cboCC.attr("data-id", id);
                    cboCC.val(rowData.cc);
                    cboCC.trigger("change");
                    cboDivision.val(rowData.divisionId);
                    cboDivision.trigger("change");
                    cboAdministrador.val(`${rowData.isadmin}`);
                    cboAdministrador.trigger("change"); 
                    
                 
                    $(".modal-title").text("");
                    $(".modal-title").text("EDITAR DIVISION");
                });
                tblAF_DxP_Divisiones_Proyecto.on('click', '.eliminarDivision', function () {
                    let rowData =  dtDivisiones_Proyectos.row($(this).closest("tr")).data();
                    let id = parseFloat(rowData.id);
                    Alert2AccionConfirmar('¡Cuidado!', '¿Desea eliminar el registro seleccionado?', 'Confirmar', 'Cancelar', () => fncEliminarDivisionesProyectos(id));
                });
            },
            columnDefs: [
                { className: 'dt-center', 'targets': '_all' }
            ],
        });
    }

    function fncFiltros(){
        let objFiltro = new Object();
        objFiltro = {
            cc: cmbcc.val(),
            divisionId: cmbbDivision.val(),
            isadmin: cboIsAdmin.val()=="--Seleccione--"?null:cboIsAdmin.val()
        };
        console.log(objFiltro)
        return objFiltro;
    }
    
    function fncGetDivisiones_Proyectos() {
        
        axios.post("GetDivisiones_Proyectos").then(response => {
            let { success, items, message } = response.data;
            console.log(response);
            if (success) {
                dtDivisiones_Proyectos.clear().draw();
                dtDivisiones_Proyectos.rows.add(response.data.lstDivisiones_Proyectos).draw();
            }
            else {
                Alert2Error(message)
            }

        }).catch(error => Alert2Error(error.message));
    }

    function fncGetDivisiones_ProyectosFiltro() {
        let objFiltro = fncFiltros();
        axios.post("GetDivisiones_ProyectosFitro",objFiltro).then(response => {
            let { success, items, message } = response.data;
            console.log(response);
            if (success) {
                dtDivisiones_Proyectos.clear().draw();
                dtDivisiones_Proyectos.rows.add(response.data.lstDivisiones_Proyectos).draw();
            }
            else {
                Alert2Error(message)
            }

        }).catch(error => Alert2Error(error.message));
    }

    function GetParametros(){
        let returnParametros = {
            id: cboCC.attr("data-id"),
            esActivo: true,
            isAdmin: cboAdministrador.val(),
            cc: cboCC.val(),
            divisionID: cboDivision.val(),            
        };
        console.log(returnParametros);
        return returnParametros;
    }

                      

    function fncLimpiarCombos() {
        cmbcc.val(""),
        cmbbDivision.val(""); 
        cmbcc.trigger("change");
        cmbbDivision.trigger("change");
        cboIsAdmin.val("--Seleccione--");
        cboIsAdmin.trigger("change");
        
    }

    function fncLimpiarFormulario() {
        cboCC.val(""),
        cboDivision.val("");
        cboCC.trigger("change");
        cboAdministrador.val("--Seleccione--");

        cboDivision.trigger("change"); 
        cboAdministrador.trigger("change");
    }
     

    //AGREGAR
    function fncGuadarDivisionesProyectos() {
        let parametros = GetParametros();
        console.log(parametros);
        if (fncCamposVacios()) {
            Alert2Warning("Es necesario ingresar los datos faltantes");     
        } else {
            axios.post('GuardarDivisiones_Proyectos', parametros).then(response => {
                if (response.data.GuardarDivisiones_Proyectos.estatus==1) {

                    $("#mdlNuevaDivision").modal('hide');
                    Alert2Exito(response.data.GuardarDivisiones_Proyectos.mensaje);
                    fncGetDivisiones_Proyectos();   

                }else if(response.data.GuardarDivisiones_Proyectos.estatus==2){

                     Alert2Warning(response.data.GuardarDivisiones_Proyectos.mensaje);

                }else if(response.data.GuardarDivisiones_Proyectos.estatus==3){

                    Alert2Error(response.data.GuardarDivisiones_Proyectos.mensaje);
               }
            }).catch(error => Alert2Error(error.message));
        }
    }

    function fncEliminarDivisionesProyectos(id) {
        console.log(id)
        axios.post('EliminarDivisionesProyectos', { id: id }).then(response => {
            let { success, items, message } = response.data;
            if (success) {
                Alert2Exito("Se ha eliminado con éxito la división.");
                fncGetDivisiones_Proyectos();
            }
        }).catch(error => Alert2Error(error.message));
    }

    function fncEditarDivisionesProyectos() {
        let parametros = GetParametros();
            if (fncCamposVacios()) {
            Alert2Warning("Es necesario ingresar los datos faltantes");     
                
            }
            else{
                axios.post('EditarDivisionesProyectos', { parametros: parametros }).then(response => {
                    let { success, items, message } = response.data;
                    if (success) {
                        $("#mdlNuevaDivision").modal('hide');
                        Alert2Exito("Se ha modificado con éxito la división.");
                        fncGetDivisiones_Proyectos();    
                    }
                }).catch(error => Alert2Error(error.message)); 
            }
              
            
    }

    function fncCamposVacios() {
        let vacio = false;
        cboCC.val() == "--Seleccione--" ? vacio = true : vacio = false;
        cboDivision.val() == "--Seleccione--" ? vacio = true : vacio = false;
        cboAdministrador.val() == "--Seleccione--" ? vacio = true : vacio = false;     
        return vacio;
    }   

    //EDITAR

    $(document).ready(() => {
        Contratos.Divisiones_Proyectos = new Divisiones_Proyectos();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();