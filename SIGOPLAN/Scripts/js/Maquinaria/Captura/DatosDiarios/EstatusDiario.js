(() => {
$.namespace('DatosDiaros.Maquinaria');
let _Estatus = 0;
let _obj = null;
    const tblmDatosDiarios = $('#tblmDatosDiarios');
    let dtDatosDiarios;
    const cboProyecto = $('#cboProyecto');
    const cboEstado = $('#cboEstado');
    const btnBuscar = $('#btnBuscar');
    const dtFecha = $('#dtFecha');
    let fechaActual = new Date();
    // const btnCaptura = $('#btnCaptura');
    const btnCrearExcel = $('#btnCrearExcel');
    const btnCapturaDatosDiarios = $('#btnCapturaDatosDiarios');
    const btnEnviarCorreo = $('#btnEnviarCorreo');
    const cboGrupo = $('#cboGrupo');
    const cboModelo = $('#cboModelo');
    const cboEconomico = $('#cboEconomico');
    const lblEstatus = $("#lblEstatus");
    const cantActivos = $("#cantActivos");
    const porActivos = $("#porActivos");
    const cantInactivos = $("#cantInactivos");
    const porInactivos = $("#porInactivos");
    const modalResumen = $("#modalResumen");
    const tblResumen = $("#tblResumen");
    const btnEnviar = $("#btnEnviar");
    const btnImprimir = $("#btnImprimir");
    const ireport = $("#report");
    Maquinaria = function (){
        let init = () => {
            initDataDatosDiarios();
            FillCombos();
            
            console.log(fechaActual);
            btnEnviarCorreo.attr('disabled',true)

            tblmDatosDiarios.on("change","._activo",function(){
                var _this = $(this);
                var row = _this.parent().parent();
                if(_this.is(":checked"))
                {
                    _this.removeClass('inactivo');
                    _this.addClass('activo');
                    
                    row.find("._editable").val('');
                    row.find("._tiempo_respuesta_str").val(0);
                    
                    row.find("._editable").not('._permiteActivo').prop("disabled",true);
                }
                else{
                    _this.removeClass('activo');
                    _this.addClass('inactivo');
                    row.find("._tiempo_respuesta_str").val(0);
                    row.find("._editable").prop("disabled",false);
                }
                countEquipos();
            });

            tblmDatosDiarios.on("change","._fecha_real",function(){
                var _this = $(this);
                var row = _this.parent().parent();

                if(_this.val()=="")
                {
                    row.find("._activo").prop("checked",false);
                    row.find("._tiempo_respuesta_str").val("---");
                }
            });
            initDataTblResumen();
            $('#modalResumen').on('shown.bs.modal', function (e) {
                $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
            });
            btnImprimir.click(function(){
                $.blockUI({ message: 'Procesando...' });
                var idReporte = "229";

                var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&cc=" + cboProyecto.val() + "&fecha=" + dtFecha.val();

                ireport.attr("src", path);

                document.getElementById('report').onload = function () {

                    $.unblockUI();
                    openCRModal();

                };

            });
            
        }
        init();
} 

        function enviarReporte(){
            $.blockUI({ message: 'Procesando...' });
            var idReporte = "229";

            var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&cc=" + cboProyecto.val() + "&fecha=" + dtFecha.val()+"&inMemory=1";

            ireport.attr("src", path);

            document.getElementById('report').onload = function () {

                $.unblockUI();

            };

        }
        function initDataTblResumen() {
            dtResumen = tblResumen.DataTable({
                paging: false,
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                "sScrollX": "100%",
                "sScrollXInner": "100%",
                "bScrollCollapse": true,
                scrollY: '65vh',
                scrollCollapse: true,
                "bLengthChange": false,
                "searching": true,
                "bFilter": true,
                "bInfo": true,
                "bAutoWidth": false,
                columns: [
                    { title:'Economico' ,data: 'noEconomico'}
                    ,{ title: 'Estatus', data: 'activo' , createdCell: (td, data, rowData, row, col) =>  
                        {
                            $(td).html(data?'Activo':'Inactivo');
                        }
                    }
                   ,{ title:'Causa' ,data: 'causa'}
                   ,{ title:'Fecha Inicial' ,data: 'fecha_inicial'}
                   ,{ title:'Fecha Proyectada' ,data: 'fecha_proyectada'}
                   ,{ title:'Fecha Real' ,data: 'fecha_real'}
                   ,{ title:'Tiempo Reparación' ,data: 'tiempo_respuesta_str'}
                   ,{ title:'Acciones' ,data: 'acciones'}
                ]
            });
        }
    function countEquipos()
    {
        var quiposActivos = $('.activo');
        var quiposInactivos = $('.inactivo');
        var activos = quiposActivos.length;
        var inactivos = quiposInactivos.length;
        var total = activos + inactivos;
        var porActivo = ((activos * 100) / total).toFixed(2);
        var porInactivo = ((inactivos * 100) / total).toFixed(2);
        cantActivos.html(activos);
        cantInactivos.html(inactivos);
        if(total==0)
        {
            porActivos.html('0%');
            porInactivos.html('0%');
        }
        else{
            porActivos.html(porActivo+'%');
            porInactivos.html(porInactivo+'%');
        }
        
    }
    function getCountEquipos()
    {
        var obj = [];
        var quiposActivos = $('.activo');
        var quiposInactivos = $('.inactivo');
        var activos = quiposActivos.length;
        var inactivos = quiposInactivos.length;
        var total = activos + inactivos;
        var porActivo = ((activos * 100) / total).toFixed(2);
        var porInactivo = ((inactivos * 100) / total).toFixed(2);
        cantActivos.html(activos);
        cantInactivos.html(inactivos);
        if(total==0)
        {
            porActivos.html('0%');
            porInactivos.html('0%');
        }
        else{
            porActivos.html(porActivo+'%');
            porInactivos.html(porInactivo+'%');
        }

        obj.cantActivos = activos;
        obj.cantInactivos = inactivos;
        obj.porActivos = parseFloat(porActivo);
        obj.porInactivos = parseFloat(porInactivo);
        return obj;
    }
    function getCapturaDatosDiarios() {
        var totales = getCountEquipos();
        var paquete = {};
        var obj = {};
        var det = [];

        obj.id = _obj.id;
        obj.fecha = dtFecha.val();
        obj.estatus = 1;
        obj.cc = _obj.cc;
        obj.cantActivos = totales.cantActivos;
        obj.cantInactivos = totales.cantInactivos;
        obj.porActivos = totales.porActivos;
        obj.porInactivos = totales.porInactivos;
        obj.usuario = 0;

        dtDatosDiarios.rows().every(function (rowIdx, tableLoop, rowLoop ) {
            let node = $(this.node());
            let data = this.data();

            var o = {};
            o.id = data.id;
            o.estatusDiarioID = obj.id;
            o.noEconomicoID = data.noEconomicoID;
            o.noEconomico = data.noEconomico;
            o.descripcion = data.descripcion;
            o.activo = node.find('._activo').is(':checked');
            o.causa = node.find('._causa').val();
            o.fecha_inicial = node.find('._fecha_inicial').val();
            o.fecha_proyectada = node.find('._fecha_proyectada').val();
            o.fecha_real = node.find('._fecha_real').val();
            o.tiempo_respuesta_str = node.find('._tiempo_respuesta_str').val();
            o.tiempo_respuesta = parseFloat(node.find('._tiempo_respuesta_str').val());
            o.acciones = node.find('._acciones').val();

            det.push(o);
        });
        paquete.obj = obj;
        paquete.det = det;
        return paquete;
    }
    function FillCombos() {
        cboProyecto.fillCombo('/CapturaDatos/ObtenerAreaCuenta', null,false);
        btnBuscar.click(function(){
            obtenerCatMaquinas();
        })
        //dtFecha.datetimepicker({
        //    format:'Y/m/d H:i',
        //    timepicker:true,
        //    datepicker:true,
        //});
        dtFecha.datepicker({
            "dateFormat": "dd/mm/yy",
        }).datepicker("option", "showAnim", "slide")
            .datepicker("setDate", fechaActual);
    
        btnCapturaDatosDiarios.val('');

        btnCapturaDatosDiarios.click(function () {
            var data = getCapturaDatosDiarios();
            dtResumen.clear().draw();
            
            modalResumen.modal("show");
            dtResumen.rows.add(data.det).draw();  
        })
        btnCrearExcel.click(function(){
            GenerarExcel();
        })
        btnEnviar.click(CapturaDeDatosDiarios);
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
            ,"sScrollX": "100%"
            ,"sScrollXInner": "100%"
            ,"bScrollCollapse": true
            ,scrollY: '65vh'
            ,scrollCollapse: true
            ,columns: [
                { data: 'noEconomico', title: 'No° Economicos', width:'5%'  },
                { data: 'descripcion', title: 'Descripcion', width:'5%'  },
                { data: 'activo', title: 'Estatus', width:'5%'  , createdCell: (td, data, rowData, row, col) =>  
                    {
                        let html = `<input type='checkbox' class='_activo form-control'/>`;
                        
                        $(td).html(html);
                        $(td).find("input[type=checkbox]").prop('checked',data);
                        if(data)
                        {
                            $(td).find("input[type=checkbox]").addClass('activo');
                            if (rowData.causa!='' && rowData.causa!=null){
                                $(td).find("input[type=checkbox]").prop('disabled',true);
                            }
                        }
                        else{
                            $(td).find("input[type=checkbox]").addClass('inactivo');
                        }
                        
                        if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                            
                        }
                        else{
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                    }
                },
                { data: 'causa', title: 'Causa', width:'15%'  , createdCell: (td, data, rowData, row, col) =>  
                    {
                        let html = `<input type='text' class='_causa form-control _editable'/>`;
                        
                        $(td).html(html);
                        $(td).find("input[type=text]").val(data);
                        fechaActual = new Date();
                        if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                            
                        }
                        else{
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                        if(rowData.activo)
                        {
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                        if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                            
                        }
                        else{
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                    }
                },
                { data: 'fecha_inicial', title: 'Fecha Inicial', width:'15%'  , createdCell: (td, data, rowData, row, col) =>  
                    {
                        let html = `<input type='text' class='_fecha_inicial form-control _fecha _editable'/>`;
                        $(td).html(html);
                        var input = $(td).find("input[type=text]");
                        if(data != null)
                        {
                            input.val(moment(data).format('DD/MM/YYYY HH:mm'));
                        }
                        
                        if(rowData.activo)
                        {
                            $(td).find("input[type=text]").val("");
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                        if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                            
                        }
                        else{
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                        
                    }
                },
                { data: 'fecha_proyectada', title: 'Fecha Proyectada', width:'15%'  , createdCell: (td, data, rowData, row, col) =>  
                    {
                        let html = `<input type='text' class='_fecha_proyectada form-control _fecha _editable'/>`;
                        $(td).html(html);
                        var input = $(td).find("input[type=text]");
                        if(data != null)
                        {
                            input.val(moment(data).format('DD/MM/YYYY HH:mm'));
                        }
                        if(rowData.activo)
                        {
                            $(td).find("input[type=text]").val("");
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                        if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                            
                        }
                        else{
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                    }
                },
                { data: 'fecha_real', title: 'Fecha Real', width:'15%'  , createdCell: (td, data, rowData, row, col) =>  
                    {
                        let html = `<input type='text' class='_fecha_real form-control _fecha _editable'/>`;
                        $(td).html(html);
                        var input = $(td).find("input[type=text]");
                        
                        if(data != null)
                        {
                            input.val(moment(data).format('DD/MM/YYYY HH:mm'));
                        }
                        if(rowData.activo)
                        {
                            $(td).find("input[type=text]").val("");
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                        if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                            
                        }
                        else{
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                    }
                },
                { data: 'tiempo_respuesta_str', title: 'Tiempo reparación', width:'5%'  , createdCell: (td, data, rowData, row, col) =>  
                    {
                        let html = `<input type='text' class='_tiempo_respuesta_str form-control' readonly/>`;
                        $(td).html(html);
                        var input = $(td).find("input[type=text]");
                        
                        if(data != null)
                        {
                            input.val(data);
                        }
                        else if(rowData.fecha_real==null && rowData.fecha_inicial!=null){
                            input.val(data);
                        }
                        if(rowData.activo)
                        {
                            $(td).find("input[type=text]").val("");
                        }
                        $(td).find("input[type=text]").prop("disabled",true);
                    }
                },
                { data: 'acciones', title: 'Acciones', width:'20%'  , createdCell: (td, data, rowData, row, col) =>  
                    {
                        let html = `<input type='text' class='_acciones form-control _editable _permiteActivo'/>`;
                        $(td).html(html);
                        $(td).find("input[type=text]").val(data);

                        if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                            
                        }
                        else{
                            $(td).find("input[type=text]").prop("disabled",true);
                        }
                    }
                }
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

    function obtenerCatMaquinas() {
        fechaActual = new Date();
        let fecha = dtFecha==""?fechaActual:dtFecha.val();
        let cc = cboProyecto.val()==null?"":cboProyecto.val();
        axios.post('/CapturaDatos/getEstatus_Diario', {fecha:fecha,cc : cc})
            .catch(o_O => AlertaGeneral(o_O.message))
            .then(response => {
                let { success, data} = response.data;
                if (success) 
                {
                    _obj = data.obj;
                    _Estatus = data.obj.estatus;
                    if(_Estatus==0){
                        lblEstatus.html("No guardada");
                        btnImprimir.hide();
                        $('#btnCapturaDatosDiarios').text('Guardar Captura');
                    }
                    else{
                        lblEstatus.html("Guardada");
                        btnImprimir.show();
                        $('#btnCapturaDatosDiarios').text('Actualizar Captura');
                    }
                    AddRows(tblmDatosDiarios,data.det)
                    if (moment(fechaActual).format('DD/MM/YYYY')==dtFecha.val()) {
                        $('#btnCapturaDatosDiarios').attr('disabled',false)
                    }
                    else{
                        $('#btnEnviarCorreo').attr('disabled',true)
                        $('#btnCapturaDatosDiarios').attr('disabled',true)
                    }
                    var camposFecha = $("._fecha");
                    camposFecha.datetimepicker({
                        onChangeDateTime:function(dp,$input){
                            var _this = ($input);
                            var _row = _this.parent().parent();
                            var _dtInicial = $(_row.find("._fecha_inicial")[0]); 
                            var _dtProyectada = $(_row.find("._fecha_proyectada")[0]); 
                            var _dtReal = $(_row.find("._fecha_real")[0]); 
                            var _tiempoReparacion = $(_row.find("._tiempo_respuesta_str")[0]);

                            var inicio = moment(new Date(), 'd/m/Y H:i');
                            var fin = moment(new Date(), 'd/m/Y H:i');
                            if(_dtInicial.val()=='')
                            {
                                _tiempoReparacion.val(0);
                            }
                            else if(_dtInicial.val()!='' && _dtReal.val()!='')
                            {
                                inicio = moment(_dtInicial.val(), 'd/m/Y H:i');
                                fin = moment(_dtReal.val(), 'd/m/Y H:i');
  
                                horas = fin.diff(inicio, 'hours');
                                _tiempoReparacion.val(horas);
                            }
                            else{
                                inicio = moment(_dtInicial.val(), 'd/m/Y H:i');

                                horas = fin.diff(inicio, 'hours');
                                _tiempoReparacion.val(horas);
                            }
                        }
                    });
                    if(_Estatus==0){
                        var camposFecha_Inicial = $("._fecha_inicial");
                        $.each(camposFecha_Inicial,function(i,e){
                            var _this = $(e);
                            var _row = _this.parent().parent();
                            var _dtInicial = $(_row.find("._fecha_inicial")[0]); 
                            var _dtProyectada = $(_row.find("._fecha_proyectada")[0]); 
                            var _dtReal = $(_row.find("._fecha_real")[0]); 
                            var _tiempoReparacion = $(_row.find("._tiempo_respuesta_str")[0]);

                            var inicio = moment(new Date(), 'd/m/Y H:i');
                            var fin = moment(new Date(), 'd/m/Y H:i');
                            if(_dtInicial.val()=='')
                            {
                                _tiempoReparacion.val(0);
                            }
                            else if(_dtInicial.val()!='' && _dtReal.val()!='')
                            {
                                inicio = moment(_dtInicial.val(), 'd/m/Y H:i');
                                fin = moment(_dtReal.val(), 'd/m/Y H:i');
  
                                horas = fin.diff(inicio, 'hours');
                                _tiempoReparacion.val(horas);
                            }
                            else{
                                inicio = moment(_dtInicial.val(), 'd/m/Y H:i');

                                horas = fin.diff(inicio, 'hours');
                                _tiempoReparacion.val(horas);
                            }
                        });
                    }
                    countEquipos();
                }
            });
    }

    function CapturaDeDatosDiarios() {
        let datos = getCapturaDatosDiarios();

        $.ajax({
            type: "POST",
            url: '/CapturaDatos/saveCapturarDatosDiaros',
            data: {obj: datos.obj, det: datos.det},
            success: function(response) { 
                if(response.success)
                {
                    $("#modalResumen .close").click();
                    $.blockUI({ message: 'Procesando...' });
                    var idReporte = "229";

                    var path = "/Reportes/Vista.aspx?idReporte=" + idReporte + "&cc=" + cboProyecto.val() + "&fecha=" + dtFecha.val()+"&inMemory=1";

                    ireport.attr("src", path);

                    document.getElementById('report').onload = function () {

                        $.unblockUI();
                        
                        btnBuscar.click();
                        AlertaGeneral('Confirmacion','Captura guardada correctamente, se envio el reporte a todos los involucrados');
                    };
                   
                }
            },
            failure: function(errMsg) {
                AlertaGeneral("Alerta","Ocurrio un error, favor de contactar al depto de TI!");
            }
        });
    }




    $(document).ready(() => {
        DatosDiaros.Maquinaria = new Maquinaria();
    })
    .ajaxStart(() => { $.blockUI({ message: 'Procesando...' }); })
    .ajaxStop(() => { $.unblockUI(); });
})();