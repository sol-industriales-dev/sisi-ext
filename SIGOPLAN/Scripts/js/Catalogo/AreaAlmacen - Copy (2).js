(() => {
$.namespace('AreaAlmacen.Almacen');
 
    const btnAgregarAlmacen = $('#btnAgregarAlmacen');
    const btnSeleccionar = $('#btnSeleccionar');
    const cboAlmacenMultiple = $('#cboAlmacenMultiple');
    const tblAl_AreaAlmacen = $('#tblAl_AreaAlmacen');
    const cboAreaCuenta = $('#cboAreaCuenta');
    const btnCrearEditar = $('#btnCrearEditar');
    const txtNombre = $('#txtNombre');
    const btnNuevo = $('#btnNuevo');
    const cboAreaCuenta2 = $('#cboAreaCuenta2');
    const btnBuscar = $('#btnBuscar');
    const tbl_Detalle = $('#tbl_Detalle');
    let dtDetalle;
    let dtAreaAlmacen;
    var idRelacion;
    let lstAlmacenesSeleccionados=[];
    const cboTipoAlmacen = $('#cboTipoAlmacen');
    const cboPrioridad = $('#cboPrioridad');

    Almacen = function (){
        let init = () => {
            btnCrearEditar.attr('data-id',0);
            idRelacion = btnCrearEditar.attr('data-id');
            CargarDatos();
            // CargandoDetalle();
            initDataTblAreaAlmacen();
            initDataTblDetalle();
            fillcombos();
            cargarBotones();
        }
        init();
    }
    function cargarBotones() {
        btnAgregarAlmacen.click(function () {
        cboAlmacenMultiple.fillCombo('/Enkontrol/Almacen/getAlmacenesAreaDisponibles?idRelacion='+idRelacion, null, null);
        })
        
        btnBuscar.click(function () {
            CargarDatos();
        })
        btnNuevo.click(function () {
            btnCrearEditar.attr('data-id',0);
            idRelacion = btnCrearEditar.attr('data-id');
            lstAlmacenesSeleccionados=[];
            AddRows(tbl_Detalle,lstAlmacenesSeleccionados);
            fillcombos();
            txtNombre.val('');
            cboAreaCuenta.prop('disabled',false);

        })
        btnCrearEditar.click(function () {
            // let a = getValoresMultiples('#cboAlmacenMultiple');
            // let parametros = getParametros(a);
            let params={
                id : btnCrearEditar.attr('data-id'),
                Asignacion:txtNombre.val(),
                AreaCuenta:cboAreaCuenta.val(),
                lstAlmacen:getParams(lstAlmacenesSeleccionados),
            }
            CrearModificar(params);
        })
        btnSeleccionar.click(function () {
         
                let item={
                    id:cboAlmacenMultiple.val(),
                    almacen:$('#cboAlmacenMultiple option:selected').html(),
                    TipoAlmacen:cboTipoAlmacen.val(),
                    Prioridad:0,
                }
                lstAlmacenesSeleccionados.push(item);
                AddRows(tbl_Detalle,lstAlmacenesSeleccionados);
                $('#modalAlmacenes').modal('hide');
        })
    }
    function getParams(params) {
        let parametros = [];

        for (let i = 0; i < params.length; i++) {
            const item = {
                almacen:params[i].id,
                TipoAlmacen:params[i].TipoAlmacen,
            }
            parametros.push(item);
        }

        return parametros;
    }
 
    function initDataTblDetalle() {
        dtDetalle = tbl_Detalle.DataTable({
            destroy: true
            ,language: dtDicEsp
            ,paging: false
            ,ordering:false
            ,searching: false
            ,bFilter: true
            ,info: false
            ,columns: [
                { data: 'id', title: 'id',visible:false },
                { data: 'almacen', title: 'Almacen' },
                { data: 'TipoAlmacen', title: 'TipoAlmacen' },
                { data: 'Prioridad', title: 'Prioridad' },
                {
                    render: function (data, type, row) {
                        let btnEliminar = "";
                            btnEliminar = `<button class='btn-eliminar btn btn-danger eliminarAreaAlmacen' data-esActivo="0" data-id="${row.id}">` +
                            `<i class="fas fa-trash"></i></button>`;
                        return btnEliminar;
                    }
                } 
            ]
            ,initComplete: function (settings, json) {
                tbl_Detalle.on("click",".eliminarAreaAlmacen",function () {
                    const rowData = dtDetalle.row($(this).closest("tr")).data();
                    var removed =   lstAlmacenesSeleccionados.splice(lstAlmacenesSeleccionados.indexOf(rowData.id), 1);
                    console.log(removed);
                    AddRows(tbl_Detalle,lstAlmacenesSeleccionados);
                })
            }
        });
    }
    function CargarDatos() {
        axios.post('/Enkontrol/Almacen/getAreaAlmacen',{AreaCuenta:cboAreaCuenta2.val() })
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    AddRows(tblAl_AreaAlmacen,items);
                }
            });
    }
    function initDataTblAreaAlmacen() {
        dtAreaAlmacen = tblAl_AreaAlmacen.DataTable({
            destroy: true
            ,language: dtDicEsp
            ,ordering: false
            ,paging: false
            ,searching: false
            ,bFilter: true
            ,info: false
            ,columns: [
                { data: 'id', title: 'id',visible:false },
                { data: 'Asignacion', title: 'Asignacion' ,visible:false},
                { data: 'Descripcion', title: 'AreaCuenta' },
                { data:'lstAlmacen' ,render: (data, type, row, meta) =>{
                    let html = "";
                    for (let i = 0; i < row.lstAlmacen.length; i++) {
                        html += "<span class='btn btn-primary displayCC'><i class='fab fa-creative-commons-nd'>" + row.lstAlmacen[i].descripcion + "</i></span>";
                    }
                    return html;
                } 
            },
            {
                render: function (data, type, row) {
                    let btnEliminar = "";
                        btnEliminar = `<button class='btn-eliminar btn btn-danger eliminarAreaAlmacen' data-esActivo="0" data-id="${row.id}">` +
                        `<i class="fas fa-trash"></i></button>`;
                    return `<button class='btn-editar btn btn-warning editarAreaAlmacen' data-id="${row.id}">` +
                        `<i class='fas fa-pencil-alt'></i>` +
                        `</button>&nbsp;` + btnEliminar;
                }
            } 
            ]
            ,initComplete: function (settings, json) {

                tblAl_AreaAlmacen.on("click", ".eliminarAreaAlmacen", function () {
                    let esActivo = $(this).attr("data-esActivo");
                    let strMensaje = "¿Desea eliminar el registro seleccionado?";

                    Swal.fire({
                        position: "center",
                        icon: "warning",
                        title: "¡Cuidado!",
                        width: '35%',
                        showCancelButton: true,
                        html: `<h3>${strMensaje}</h3>`,
                        confirmButtonText: "Confirmar",
                        confirmButtonColor: "#5cb85c",
                        cancelButtonText: "Cancelar",
                        cancelButtonColor: "#d9534f",
                        showCloseButton: true
                    }).then((result) => {
                        if (result.value) {
                            fncEliminar($(this).attr("data-id"), esActivo);
                        }
                    });
                });

                tblAl_AreaAlmacen.on("click", ".editarAreaAlmacen", function (e) {
                    const rowData = dtAreaAlmacen.row($(this).closest("tr")).data();
                    btnCrearEditar.attr('data-id',$(this).attr("data-id"));
                    idRelacion = btnCrearEditar.attr('data-id');
                    
                    CargandoDetalle($(this).attr("data-id"));
                    lstAlmacenesSeleccionados=[];
                    fillcombos();
                    CargarComboDatos(rowData);
                    cboAreaCuenta.prop('disabled',true);
                    $('#mdlCrearEditarAreaAlmacen').modal("show");

                });

            }
        });
    }

    function CargandoDetalle(idRelacionado) {
        axios.post('/Enkontrol/Almacen/getDetalleAreaAlmacen', {idRelacion:idRelacionado})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    let item = response.data.items.items;
                    console.log(item)
                    for (let index = 0; index < item.length; index++) {
                        let newitem={
                            id: item[index].idAlmacen,
                            almacen: item[index].Descripcion,
                            TipoAlmacen: item[index].TipoAlmacen ,
                            Prioridad: item[index].Prioridad ,
                        }
                        lstAlmacenesSeleccionados.push(newitem);
                    }

                    AddRows(tbl_Detalle,lstAlmacenesSeleccionados);
                }
            });
    }

    var CargarComboDatos = function (rowData) {
        console.log(rowData)
        var texto = rowData.Asignacion;
        txtNombre.val(texto);
        var roots = rowData.lstAlmacen.map(function (num) {
            return num.almacen;
        });
        console.log(roots)
        // cboComboAgrupaciones.val(rowData.id);
        // cboComboAgrupaciones.trigger("change");
      
        cboAlmacenMultiple.fillCombo('/Enkontrol/Almacen/getAlmacenesAreaDisponibles?idRelacion='+rowData.id, null, null);
        cboAlmacenMultiple.val(roots);
        cboAlmacenMultiple.trigger("change");
        cboAreaCuenta.fillCombo('/Enkontrol/Almacen/getAreaCuentas?idRelacion='+rowData.id, null, null);
        cboAreaCuenta.val(rowData.AreaCuenta);
        cboAreaCuenta.trigger("change");
    }
    function fncEliminar(params) {
        console.log(params)
        let id = params;
        axios.post('/Enkontrol/Almacen/EliminarAreaAlmacen', {id:id})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    Alert2Exito(items.mensaje);
                    CargarDatos();
                    idRelacion = btnCrearEditar.attr('data-id');
                    fillcombos();
                }
            });
    }
    function fillcombos() {
        if (idRelacion==undefined) {
            idRelacion=0;
        }
        cboAreaCuenta2.fillCombo('/Enkontrol/Almacen/getTodasAreaCuentas', null, null);
        cboAlmacenMultiple.fillCombo('/Enkontrol/Almacen/getAlmacenesAreaDisponibles?idRelacion='+idRelacion, null, null);
        cboAreaCuenta.fillCombo('/Enkontrol/Almacen/getAreaCuentas?idRelacion='+idRelacion, null, null);
    
    }

    function AddRows(tbl, lst) {
        dt = tbl.DataTable();
        dt.clear().draw();
        dt.rows.add(lst).draw(false);
    }
    function getParametros(params) {
            let selCC;
            let Prefijo;
            let arreglo = []
    
            for (let i = 0; i < params.length; i++) {
                selCC = '';
                Prefijo = '';
                selCC = $('#cboAlmacenMultiple').find("option[value=" + params[i] + "]");
    
    
                const element = {
                    almacen: params[i],
                };
                arreglo.push(element);
            }
        return arreglo;
    }
    function CrearModificar(parametros) {
        console.log(parametros)
        axios.post('/Enkontrol/Almacen/GuardarEditarAreaAlmacen', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    if (items.mensaje == "Este registro ya existe.") {
                        Alert2Exito(items.mensaje);
                    }else{
                        $('#mdlCrearEditarAreaAlmacen').modal("hide");
                        CargarDatos();
                        Alert2Exito(items.mensaje);
                        fillcombos();
                    }
                    
                }
            });
    }
    function guardarDetalle(parametros) {
        axios.post('/Enkontrol/Almacen/GuardarDetalle', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                        CargandoDetalle(items.id);
                }
            });
    }

    $(document).ready(() => {
        AreaAlmacen.Almacen = new Almacen();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();