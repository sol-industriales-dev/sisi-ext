(() => {
$.namespace('DatosDiaros.Maquinaria');

    const tblmDatosDiarios = $('#tblmDatosDiarios');
    let dtDatosDiarios
    const cboProyecto = $('#cboProyecto');
    const cboEstado = $('#cboEstado');
    const btnBuscar = $('#btnBuscar');
    const dtFecha = $('#dtFecha');
    const fechaActual = new Date();
    // const btnCaptura = $('#btnCaptura');
    const btnCrearExcel = $('#btnCrearExcel');
    const btnCapturaDatosDiarios = $('#btnCapturaDatosDiarios');
    const btnEnviarCorreo = $('#btnEnviarCorreo');
    const cboGrupo = $('#cboGrupo');
    const cboModelo = $('#cboModelo');
    const cboEconomico = $('#cboEconomico');

    Maquinaria = function (){
        let init = () => {
            initDataDatosDiarios();
            FillCombos();
            // obtenerCatMaquinas();
            // iniciarModal();
            console.log(fechaActual);
            btnEnviarCorreo.attr('disabled',true)
            PermisoBoton();
        }
        init();
    }
    function FillCombos() {
        $('#cboGrupo').select2();
        $('#cboModelo').select2();
        cboProyecto.fillCombo('/CapturaDatos/ObtenerAreaCuenta', null,false);
        btnBuscar.click(function(){
            obtenerCatMaquinas();
            if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                $('#btnCapturaDatosDiarios').attr('disabled',false)
                let tr = $('#tblmDatosDiarios').find('tr');
                for (let i = 1; i < tr.length; i++) {
                    $(tr[i]).find('td').find('select').attr('disabled',false);
                }
            }else{
                $('#btnCapturaDatosDiarios').attr('disabled',true)
                let tr = $('#tblmDatosDiarios').find('tr');
                for (let i = 1; i < tr.length; i++) {
                    $(tr[i]).find('td').find('select').attr('disabled',true);
                }
            }
        })
        dtFecha.datepicker({
            "dateFormat": "dd/mm/yy",
        }).datepicker("option", "showAnim", "slide")
            .datepicker("setDate", fechaActual);
            // btnCaptura.click(function () {
            //     $('#modalCapturaDatos').modal('show');
            // })
        cboGrupo.fillCombo('/CapturaDatos/ObtenerGrupo', null,false);
        cboModelo.fillCombo('/CapturaDatos/ObtenerModelo?idGrupo='+1, null,false);
      
        cboModelo.attr('disabled',true);
        cboGrupo.change(function () {
            console.log('entro')
        cboModelo.attr('disabled',false);
        cboModelo.fillCombo('/CapturaDatos/ObtenerModelo?idGrupo='+cboGrupo.val(), null,false);
        
        });
        btnCapturaDatosDiarios.val('');
        dtFecha.change(function () {
            if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                let tr = $('#tblmDatosDiarios').find('tr');
                for (let i = 1; i < tr.length; i++) {
                    $(tr[i]).find('td').find('select').attr('disabled',false);
                }
            }else{
                let tr = $('#tblmDatosDiarios').find('tr');
                for (let i = 1; i < tr.length; i++) {
                    $(tr[i]).find('td').find('select').attr('disabled',true);
                }
            }
        })
        btnCapturaDatosDiarios.click(function () {
            CapturaDeDatosDiarios();
        })
        btnCrearExcel.click(function(){
            GenerarExcel();
        })
        btnEnviarCorreo.click(function () {
            EnviandoElCorreo();
        })
        // btnCaptura.click(function () {
        //     dlgModal.dialog("open");
        //     $('#dlgModal').css('min-height', '700px');
        //     obtenerCatMaquinas();
        // })
        cboModelo.change(function () {
            validarTodosVacios();
        })
        cboGrupo.change(function () {
            validarTodosVacios();
        })
        cboEconomico.change(function () {
            validarTodosVacios();
        })
        $('#cboEstatus').change(function () {
            validarTodosVacios();
        })
    }
    // function iniciarModal() {
    //     $("#dialog-form1").removeClass('hide');
    //     dialog1 = $("#dialog-form1").dialog({
    //         draggable: false,
    //         modal: true,
    //         resizable: true,
    //         height: "auto",
    //         width: "auto",
    //         autoOpen: false
    //     });
    //     $("#dlgModal").removeClass('hide');
    //     dlgModal = $("#dlgModal").dialog({
    //         draggable: false,
    //         modal: true,
    //         resizable: true,
    //         width: "100%",
    //         height: "100%",
    //         autoOpen: false,
    //         position: 'absolute'
    //     });
    // }

    function validarTodosVacios() {
        if (cboGrupo.val()=="" && cboModelo.val()=="" &&  $('#cboEconomico').val()=="" && $('#cboEstatus').val()=="0") {
            btnCapturaDatosDiarios.attr('disabled',false);
        }else{
            btnCapturaDatosDiarios.attr('disabled',true);
        }
    }


    function getCapturaDatosDiarios() {
        let parametros = []
        let tr = $('#tblmDatosDiarios').find('tr');
        for (let i = 1; i < tr.length; i++) {
             let td = $(tr[i]).find('td');
             let item = {
                    id:'0',
                    fechaCapturaMaquinaria:fechaActual,
                    idCatMaquina:$(td[0]).text(),
                    FechaPatioMaquinaria: $(td[7]).attr('data-fecha') == "Invalid date"?"" : $(td[7]).attr('data-fecha')=="0000-12-31 11:36:08"?"":$(td[7]).attr('data-fecha'),
                    FechaTMC: $(td[8]).attr('data-fecha') == "Invalid date"?"" : $(td[8]).attr('data-fecha')=="0000-12-31 11:36:08"?"":$(td[8]).attr('data-fecha'),
                    FechaMaquinaria: $(td[9]).attr('data-fecha') == "Invalid date"?"" : $(td[9]).attr('data-fecha')=="0000-12-31 11:36:08"?"":$(td[9]).attr('data-fecha'),
                    idEstatus:$(td[10]).find("option:selected" ).val(),
                };
            parametros.push(item);
        }
        // let parametros = {
        //     Horómetro:'',
        //     FechaPatioMaquinaria:'',
        //     FechaTMC:'',
        //     FechaMaquinaria:'',
        // }
        console.log(parametros)
        return parametros;
    }
    function initDataDatosDiarios() {
   
        dtDatosDiarios = tblmDatosDiarios.DataTable({
            destroy: true
            ,language: dtDicEsp
            ,paging: false
            ,ordering:false
            ,searching: true
            ,bFilter: true
            ,info: false
            ,scrollCollapse: true
            ,scrollY: "300px"
            ,columns: [
                { data: 'idCatMaquina', title: 'No°' , width:'5%' },
                { data: 'economicoDescripcion', title: 'No° Economicos', width:'5%'  },
                { data: 'descripcion', title: 'descripcion', width:'5%'  },
                { data: 'Marca', title: 'Marca', width:'5%'  },
                { data: 'Modelo', title: 'Modelo', width:'5%'  },
                { data: 'Serie', title: 'No° Serie', width:'5%'  },
                { data: 'Horometro', title: 'Horometro', width:'5%'  },
                // { data: 'proveedor', title: 'proveedor' },
                { title: 'Ingreso a PDM' ,data:'FechaPatioMaquinaria', createdCell: (td, data, rowData, row, col) =>  
                {
                    let html = ``;
                    if (data==null) {
                        html=``;
                    }else if ( moment(data).format('YYYY-MM-DD hh:mm')=="0000-12-31 11:36") {
                        html = ``;
                    }else{
                        html = moment(data).format('DD-MM-YYYY');
                    }
                    $(td).attr('data-fecha',moment(data).format('YYYY-MM-DD hh:mm:ss'))
                    return  $(td).html(html);
                }
                },
                { title: 'Ingreso a TMC' ,data:'FechaTMC', createdCell: (td, data, rowData, row, col) =>  
                {
                    let html = ``;
                    if (data==null) {
                        html=``;
                    }else if ( moment(data).format('YYYY-MM-DD hh:mm')=="0000-12-31 11:36") {
                        html = ``;
                    }else{
                        html = moment(data).format('DD-MM-YYYY');
                    }
                    $(td).attr('data-fecha',moment(data).format('YYYY-MM-DD hh:mm:ss'))
                    return  $(td).html(html);
                }
                },
                { title: 'Reingreso a PDM' ,data:'FechaMaquinaria', createdCell: (td, data, rowData, row, col) =>  
                {
                    let html = ``;
                    if (data==null) {
                        html=``;
                    }else if ( moment(data).format('YYYY-MM-DD hh:mm')=="0000-12-31 11:36") {
                            html = ``;
                        }else{
                            html = moment(data).format('DD-MM-YYYY');
                        }
                        $(td).attr('data-fecha',moment(data).format('YYYY-MM-DD hh:mm:ss'))
                    return  $(td).html(html);
                }
                },
                
                { data: 'Status' ,render: (data, type, row, meta) => 
                    {
                        let html = ``;
                            if (data==0) {
                                html = `<select>
                                            <option selected value="0">--SELECCIONE--</option>
                                            <option value="1">Overhaul</option>
                                            <option value="2">Equipo en espera de rehabilitación</option>
                                            <option value="3">Equipo en rehabilitación en TMC</option>
                                            <option value="4">Equipo disponible para obra</option>
                                            <option value="5">Equipo disponible para venta</option>
                                        </select>`;
                            }
                            if (data==1) {
                                html = `<select>
                                            <option selected value="1">Overhaul</option>
                                            <option value="2">Equipo en espera de rehabilitación</option>
                                            <option value="3">Equipo en rehabilitación en TMC</option>
                                            <option value="4">Equipo disponible para obra</option>
                                            <option value="5">Equipo disponible para venta</option>
                                        </select>`;
                            }
                            if (data==2) {
                                html = `<select>
                                            <option  value="1">Overhaul</option>
                                            <option selected value="2">Equipo en espera de rehabilitación</option>
                                            <option value="3">Equipo en rehabilitación en TMC</option>
                                            <option value="4">Equipo disponible para obra</option>
                                            <option value="5">Equipo disponible para venta</option>
                                        </select>`;
                            }
                            if (data==3) {
                                html = `<select>
                                            <option  value="1">Overhaul</option>
                                            <option value="2">Equipo en espera de rehabilitación</option>
                                            <option selected value="3">Equipo en rehabilitación en TMC</option>
                                            <option value="4">Equipo disponible para obra</option>
                                            <option value="5">Equipo disponible para venta</option>
                                        </select>`;
                            }
                            if (data==4) {
                                html = `<select>
                                            <option  value="1">Overhaul</option>
                                            <option value="2">Equipo en espera de rehabilitación</option>
                                            <option value="3">Equipo en rehabilitación en TMC</option>
                                            <option selected value="4">Equipo disponible para obra</option>
                                            <option value="5">Equipo disponible para venta</option>
                                        </select>`;
                            }
                            if (data==5) {
                                html = `<select>
                                            <option  value="1">Overhaul</option>
                                            <option value="2">Equipo en espera de rehabilitación</option>
                                            <option value="3">Equipo en rehabilitación en TMC</option>
                                            <option value="4">Equipo disponible para obra</option>
                                            <option selected value="5">Equipo disponible para venta</option>
                                        </select>`;
                            }
                        return html;
                    }
                },
            ]
            ,initComplete: function (settings, json) {
               
            }
           
        });
        $('.sorting_disabled').css('font-size','10px');
       
    }


    function AddRows(tbl, lst) {
        dt = tbl.DataTable();
        dt.clear().draw();
        dt.draw();
        dt.rows.add(lst).draw(false);
    }
    function getParametros() {
        let parametros = {
            fecha:dtFecha==""?fechaActual:dtFecha.val(),
            areaCuenta : cboProyecto.val()==null?"":cboProyecto.val(),
            status : cboEstado.val()==null?"":cboEstado.val(),
            Estado : $('#cboEstatus').val(),
            ModeloEquipo : $('#cboModelo').val() ,
            Economico : $('#cboEconomico').val() ,
        }
        return parametros;
    }
    function obtenerCatMaquinas() {
        let parametros = getParametros();
        axios.post('/CapturaDatos/ObtenerCatMaquinas', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                    if (success) {
                        
                    AddRows(tblmDatosDiarios,items)
                    if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                    obtenerBoton();
                    $('#btnCapturaDatosDiarios').attr('disabled',false)
                        let tr = $('#tblmDatosDiarios').find('tr');
                        for (let i = 1; i < tr.length; i++) {
                            $(tr[i]).find('td').find('select').attr('disabled',false);
                        }
                }else{
                        $('#btnEnviarCorreo').attr('disabled',true)
                        $('#btnCapturaDatosDiarios').attr('disabled',true)
                        let tr = $('#tblmDatosDiarios').find('tr');
                        for (let i = 1; i < tr.length; i++) {
                            $(tr[i]).find('td').find('select').attr('disabled',true);
                        }
                    }
                }
            });
    }

    function CapturaDeDatosDiarios() {
        let parametros = getCapturaDatosDiarios();
        axios.post('CapturarDatosDiaros', {parametros:JSON.stringify(parametros)} )
        .catch(o_O => AlertaGeneral(o_O.message))
        .then(response => {
            let success = response.data;
            if (success) {
                obtenerCatMaquinas();
                obtenerBoton();
            }
        });
    }

    function GenerarExcel() {
        let parametros = getParametros();
        axios.post('GenerarExcel', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    location.href = `GenerarExcelDatosDiarios`;
                }
            });
    }
    function EnviandoElCorreo() {
        let parametros = getParametros();
        axios.post('EnviarCorreos', {parametros:parametros})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    obtenerBoton();
                }
            });
    }

    function obtenerBoton() {
        axios.post('ObtenerBotonEnviarExcel', {Fecha:fechaActual})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                console.log(success)
                console.log(items)
                if (items == 1) {
                    btnCapturaDatosDiarios.attr('disabled',false);
                    btnEnviarCorreo.attr('disabled',false)
                    let tr = $('#tblmDatosDiarios').find('tr');
                    for (let i = 1; i < tr.length; i++) {
                        $(tr[i]).find('td').find('select').attr('disabled',false);
                    }
                }else if (items == 2) {
                    btnCapturaDatosDiarios.attr('disabled',false);
                    btnEnviarCorreo.attr('disabled',false)
                    let tr = $('#tblmDatosDiarios').find('tr');
                    for (let i = 1; i < tr.length; i++) {
                        $(tr[i]).find('td').find('select').attr('disabled',false);
                    }
                    }else if(items==3){
                    
                    btnCapturaDatosDiarios.attr('disabled',true);
                    btnEnviarCorreo.attr('disabled',true);
                    let tr = $('#tblmDatosDiarios').find('tr');
                    for (let i = 1; i < tr.length; i++) {
                        $(tr[i]).find('td').find('select').attr('disabled',true);
                    }
                }else if(items==3){
                    btnCapturaDatosDiarios.attr('disabled',false);
                    btnEnviarCorreo.attr('disabled',false)
                    let tr = $('#tblmDatosDiarios').find('tr');
                    for (let i = 1; i < tr.length; i++) {
                        $(tr[i]).find('td').find('select').attr('disabled',false);
                    }
                }
            });
    }

    function PermisoBoton() {
        axios.post('PermisoBoton')
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, items} = response.data;
                if (success) {
                    if (items==true) {
                        btnEnviarCorreo.css('display','block')
                    }else{
                        btnEnviarCorreo.css('display','none')
                    }
                }
            });
    }


    $(document).ready(() => {
        DatosDiaros.Maquinaria = new Maquinaria();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();