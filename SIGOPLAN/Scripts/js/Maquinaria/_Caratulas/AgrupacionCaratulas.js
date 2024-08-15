(() => {
$.namespace('AgrupacionCaratulas.Caratulas');

    const btnNuevo = $('#btnNuevo');
    const btnBuscar = $('#btnBuscar');
    const tblAgrupacionCaratulas = $('#tblAgrupacionCaratulas');
    let dtAgrupacionCaratulas

    const cboGrupo = $('#cboGrupo');
    const cboModelo = $('#cboModelo');
    const btnCrearEditar = $('#btnCrearEditar');
    const txtNombreAgrupacion = $('#txtNombreAgrupacion');
    const mdlCrearAgrupacion = $('#mdlCrearAgrupacion');
    let Editar = false;
    Caratulas = function (){
        let init = () => {
            
            initblAgrupacionCaratulas();
            event();
        }
        init();
    }

    function event() {
        btnBuscar.click(function () {
            obteneerAgrupacionCaratulas();
        });
        btnNuevo.click(function () {
            btnCrearEditar.attr('data-id',0);
            Editar =false;
            cboGrupo.prop('disabled',false)
            cboGrupo.val('');
            cboGrupo.trigger('change');
        });
        cboGrupo.fillCombo("obtenerGrupos", {}, false);
        cboGrupo.change(function () {
            if (cboGrupo.val() !=0 && Editar == false) {
                cboModelo.fillCombo("obtenerModelos?idGrupo="+cboGrupo.val(), {}, false);
            }
        });
        btnCrearEditar.click(function () {
            GuardarEditar();
        });
        
    }

    function obteneerAgrupacionCaratulas() {
        axios.post('obtenerAgrupacionCaratulas')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    console.log(items)
                   if (items.success) {
                    AddRows(tblAgrupacionCaratulas,items.items);
                   }else{
                       AlertaGeneral('Alerta',items.items);
                       AddRows(tblAgrupacionCaratulas,[]);
                   }
                }
            });
    }
    function AddRows(tbl, lst) {
        dt = tbl.DataTable();
        dt.clear().draw();
        dt.draw();
        dt.rows.add(lst).draw(false);
    }

    function initblAgrupacionCaratulas() {
        dtAgrupacionCaratulas = tblAgrupacionCaratulas.DataTable({
            destroy: true
            ,paging: false
            ,ordering:false
            ,searching: false
            ,bFilter: true
            ,info: false
            ,language: dtDicEsp
            ,columns: [
                { data: 'id', title: 'id',visible:false },
                { data:'AgrupacionCaratula' , title: 'Descripcion'},
                { data:'lstDetalle',title: 'Agrupacion' ,render: (data, type, row, meta) => {
                    let html = '';
                      if (data != undefined) {
                            data.forEach(x => {
                                html += `<span class='btn btn-primary displayCC'><i class='fab fa-creative-commons-nd'></i>${x.ModeloDescripcion}</span>`;
                            });
                      }

                    return html;
                }},
                { title: 'Acciones' ,render: (data, type, row, meta) => {
                    let html='';
                        html +=`<button class='btn btn-success EditarAgrupacion' data-esActivo="1" data-id="${row.id}"><i class='fas fa-pencil-alt'></i></button>`;
                        html +=`<button class='btn btn-danger EliminarAgrupacion' data-esActivo="0" data-id="${row.id}"><i class="fas fa-trash"></i></button>`;
                    return html;
                }},

            ],columnDefs: [
                { className: "dt-center", "targets": "_all" },
                { "width": "33%", "targets": [1 , 2, 3] }
            ]
            ,initComplete: function (settings, json) {
                tblAgrupacionCaratulas.on("click", ".EliminarAgrupacion", function () {
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
                            fncEliminar($(this).attr("data-id"));
                        }
                    });
                });
                tblAgrupacionCaratulas.on("click", ".EditarAgrupacion", function () {
                    const rowData = dtAgrupacionCaratulas.row($(this).closest("tr")).data();
                    Editar = true;
                    mdlCrearAgrupacion.modal('show');
                    $('.modal-title').text('Editar agrupación');
                    cboGrupo.prop('disabled','enabled');
                    btnCrearEditar.attr('data-id',rowData.id);
                    cboModelo.fillCombo("obtenerModelos?idGrupo="+rowData.idGrupo+"&Editar=2&Agrupacion="+btnCrearEditar.attr('data-id'), {}, false);
                    console.log(rowData);
                    cboGrupo.val(rowData.idGrupo);
                    cboGrupo.trigger('change');
                    txtNombreAgrupacion.val(rowData.AgrupacionCaratula);
                    cboModelo.val(rowData.modelosid);
                    cboModelo.trigger('change');
                });
            }
        });
    }
    var fncEliminar = function (id) {
        axios.post('EliminarAgrupacion', {id:id})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    Alert2Exito('Eliminado Con Exito');
                    obteneerAgrupacionCaratulas();
                }   
            });
    }
   
  
    var GuardarEditar = function () {
            var values = getValoresMultiples('#cboModelo');
            console.log(values)
            var valid = ValidacionTextoVacioBuscar();
            if (valid == true) {

                PostGuardarEditar(values);
            }
    }
    function PostGuardarEditar(values) {
        let parametros = getParametros(values);
        axios.post('GuardarEditar', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    if (items.success) {
                        Alert2Exito(items.items);
                        mdlCrearAgrupacion.modal('hide');
                        obteneerAgrupacionCaratulas();
                    }else{
                        AlertaGeneral('Alerta',items.items)
                    }
                }
            });
    }
    function getParametros(values) {
        let item ={
            id : btnCrearEditar.attr('data-id'),
            AgrupacionCaratula : txtNombreAgrupacion.val(),
            esActivo:true,
            lstDetalle: letDetalle(values),
        };

        return item;
    }
    function letDetalle(values) {
        let detalle = [];

        values.forEach(x => {
           let item = {
            id : 0,
            idAgrupacion : btnCrearEditar.attr('data-id'),
            idGrupo : cboGrupo.val(),
            idModelo : x,
            esActivo : true,
           }
           detalle.push(item);
       })

        return detalle;
    }

    var ValidacionTextoVacioBuscar = function () {
        var valid = false;
        let strMensajeError = "";
        cboModelo.val() == "" ? strMensajeError += "Es necesario seleccionar modelo para guardar." : false;
        txtNombreAgrupacion.val() == "" ? strMensajeError += "<br>Es necesario asignar un nombre a la agrupación." : false;
        strMensajeError != "" ? Alert2Warning(strMensajeError) : false;
        if (strMensajeError == "")
            {
               valid = true
            }else{
                valid = false;
            }
        return valid;
    }







    $(document).ready(() => {
        AgrupacionCaratulas.Caratulas = new Caratulas();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();